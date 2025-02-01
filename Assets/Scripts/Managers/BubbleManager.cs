using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages bubble spawning, movement, and cleanup
public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance { get; private set; }

    [Header("Bubble Settings")]
    public GameObject bubblePrefab;


    [Header("Spawn Lanes")]
    public Transform[] spawnLanes; // Array of 4 spawn positions

    private float bubbleSpeed;
    private float spawnRate;
    private int spawnsBeforeChange;
    private float bubbleSpeedChangeAmount = 0.2f;
    private float bubbleSpawnRateChangeAmount = 0.2f;
    private float minimumSpawnRate = 0.5f; // set minimum spawn rate, if it goes to zero it cause infinite spawning and break the game.

    public List<GameObject>[] lanes;
    public int spawnCount = 0;
    private Coroutine activeSpawnCoroutine;

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion
    }

    void Start()
    {
        bubbleSpeed = GameManager.Instance.bubbleStartingSpeed;
        spawnRate = GameManager.Instance.bubbleStartingSpawnRate;
        spawnsBeforeChange = GameManager.Instance.bubbleSpawnsBeforeDifficultyIncrease;
        bubbleSpeedChangeAmount = GameManager.Instance.bubbleSpeedChangeAmount;
        bubbleSpawnRateChangeAmount = GameManager.Instance.bubbleSpawnRateChangeAmount;


        lanes = new List<GameObject>[spawnLanes.Length];
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = new List<GameObject>();
        }
    }

    public void StartSpawning()
    {
        StopSpawning();
        activeSpawnCoroutine = StartCoroutine(SpawnBubbles());
    }

    public void StopSpawning()
    {
        if (activeSpawnCoroutine != null)
        {
            StopCoroutine(activeSpawnCoroutine);
            activeSpawnCoroutine = null;
        }
    }

    private void IncreaseDifficulty()
    {
        // Increase bubble speed
        bubbleSpeed += bubbleSpeedChangeAmount;

        // Decrease spawn rate but ensure it doesn't go below minimum
        float newSpawnRate = spawnRate - bubbleSpawnRateChangeAmount;
        spawnRate = Mathf.Max(newSpawnRate, minimumSpawnRate);

        // Log difficulty changes for debugging
        Debug.Log($"Difficulty increased - Speed: {bubbleSpeed}, Spawn Rate: {spawnRate}");
    }

    public IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnBubble();
            spawnCount++;

            // Adjust difficulty every X spawns
            if (spawnCount % spawnsBeforeChange == 0)
            {
                IncreaseDifficulty();
            }
        }
    }



    void SpawnBubble()
    {
        int spawnLane = Random.Range(0, spawnLanes.Length);
        Transform spawnTransform = spawnLanes[spawnLane];

        if (spawnTransform != null)
        {
            GameObject newBubble = Instantiate(bubblePrefab, spawnTransform.position, Quaternion.identity);
            newBubble.GetComponent<Bubble>().SetSpeed(bubbleSpeed);
            lanes[spawnLane].Add(newBubble);
        }
    }

    public void DeleteLeftmostBubble(int rowIndex)
    {
        if (lanes[rowIndex].Count > 0)
        {
            GameObject leftmostBubble = lanes[rowIndex][0];

            if (leftmostBubble != null)
            {
                Vector3 bubblePosition = leftmostBubble.transform.position;
                bool isInBurstZone = leftmostBubble.GetComponent<Bubble>().isInBurstZone;

                // Score calculation handled by GameManager
                GameManager.Instance.CalculateScore(bubblePosition, isInBurstZone);

                lanes[rowIndex].RemoveAt(0);
                Destroy(leftmostBubble);

                Instantiate(GameManager.Instance.VFXBubbleBurst, bubblePosition, Quaternion.identity);
                GameManager.Instance.PlayBubblePopAudio();
            }
        }
    }

    internal void ResetSpawnCount()
    {
        spawnCount = 0;
    }
}