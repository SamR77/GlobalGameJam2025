using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance { get; private set; }

    [Header("Bubble Settings")]
    public GameObject bubblePrefab;
    public Transform[] spawnLanes;
    public float bubbleSpeed;
    public float spawnRate;
    public int spawnsBeforeChange;
    public float bubbleSpeedChangeAmount;
    public float bubbleSpawnRateChangeAmount;
    public float minimumSpawnRate = 0.5f;

    [Header("Collision Detection")]
    // public BoxCollider bubbleClearingZone;
    // public float laneDetectionThreshold = 0.5f;

    public List<GameObject>[] lanes;
    private int spawnCount = 0;
    private Coroutine activeSpawnCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // find gameobject with tag "BubbleClearingZone" and set it as the clearingZone collider
        //bubbleClearingZone = GameObject.FindGameObjectWithTag("BubbleClearingZone").GetComponent<BoxCollider>();
    }

    private void Start()
    {
        InitializeLanes();
        InitializeBubbleStats();
    }

    private void InitializeLanes()
    {
        lanes = new List<GameObject>[spawnLanes.Length];
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = new List<GameObject>();
        }
    }

    public void InitializeBubbleStats()
    {
        bubbleSpeed = GameManager.Instance.bubbleStartingSpeed;
        spawnRate = GameManager.Instance.bubbleStartingSpawnRate;
        spawnsBeforeChange = GameManager.Instance.bubbleSpawnsBeforeDifficultyIncrease;
        bubbleSpeedChangeAmount = GameManager.Instance.bubbleSpeedChangeAmount;
        bubbleSpawnRateChangeAmount = GameManager.Instance.bubbleSpawnRateChangeAmount;
    }

    public void StartSpawning()
    {
        StopSpawning();
        activeSpawnCoroutine = StartCoroutine(HandleBubbleSpawner());
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
        bubbleSpeed += bubbleSpeedChangeAmount;
        spawnRate = Mathf.Max(spawnRate - bubbleSpawnRateChangeAmount, minimumSpawnRate);
        Debug.Log($"Difficulty increased - Speed: {bubbleSpeed}, Spawn Rate: {spawnRate}");
    }

    private IEnumerator HandleBubbleSpawner()
    {
        while (true)
        {
            // Wait before spawning the next bubble
            yield return new WaitForSeconds(spawnRate);

            // Select a random lane for the new bubble
            int spawnLane = Random.Range(0, spawnLanes.Length);
            Transform spawnTransform = spawnLanes[spawnLane];

            if (spawnTransform != null)
            {
                // Instantiate and initialize the new bubble
                GameObject newBubble = Instantiate(bubblePrefab, spawnTransform.position, Quaternion.identity);
                Bubble bubbleComponent = newBubble.GetComponent<Bubble>();
                bubbleComponent.InitializeBubble(bubbleSpeed, spawnLane); // Store lane index for reference

                // Track the bubble in the corresponding lane list
                lanes[spawnLane].Add(newBubble);
            }

            // Increment spawn count and check if it's time to increase difficulty
            spawnCount++;
            if (spawnCount % spawnsBeforeChange == 0)
            {
                IncreaseDifficulty();
            }
        }
    }


    

    public void HandleBubble(Vector3 position, int laneIndex, bool isPopped)
    {
        if (laneIndex >= 0 && laneIndex < lanes.Length && lanes[laneIndex].Count > 0)
        {
            GameObject bubble = lanes[laneIndex][0];
            lanes[laneIndex].RemoveAt(0);

            Vector3 bubblePosition = bubble.transform.position;
            bool isInBurstZone = bubble.GetComponent<Bubble>().isInBurstZone;

            GameManager.Instance.CalculateScore(bubblePosition, isInBurstZone && isPopped);

            Destroy(bubble);
            Instantiate(GameManager.Instance.VFXBubbleBurst, bubblePosition, Quaternion.identity);
            AudioManager.Instance.PlayBubblePopAudio();
        }
    }

    public void DeleteLeftmostBubble(int laneIndex)
    {
        HandleBubble(transform.position, laneIndex, true);
    }

    public void ResetSpawnCount()
    {
        spawnCount = 0;
    }

    public void ClearAllBubbles()
    {
        StopSpawning();

        for (int i = 0; i < lanes.Length; i++)
        {
            foreach (GameObject bubble in lanes[i])
            {
                if (bubble != null)
                {
                    Destroy(bubble);
                }
            }
            lanes[i].Clear();
        }
        ResetSpawnCount();
    }

    private void OnDestroy()
    {
        StopSpawning();
    }
}