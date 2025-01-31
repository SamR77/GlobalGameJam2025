using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages bubble spawning, movement, and cleanup
public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance { get; private set; }

    [Header("Bubble Settings")]
    public GameObject bubblePrefab;

   

    [Header("Spawn Rows")]
    public Transform[] spawnRows; // Array of 4 spawn positions

    private float bubbleSpeed;
    private float spawnRate;
    private int spawnsBeforeChange;
    private float bubbleSpeedChangeAmount = 0.2f;
    private float bubbleSpawnRateChangeAmount = 0.2f;

    private List<GameObject>[] rows;
    private int spawnCount = 0;

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
        bubbleSpeed = GameManager.Instance.bubbleSpeed;
        spawnRate = GameManager.Instance.spawnRate;
        spawnsBeforeChange = GameManager.Instance.spawnsBeforeDifficultyIncrease;
        bubbleSpeedChangeAmount = GameManager.Instance.bubbleSpeedChangeAmount;
        bubbleSpawnRateChangeAmount = GameManager.Instance.bubbleSpawnRateChangeAmount;


    rows = new List<GameObject>[spawnRows.Length];
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = new List<GameObject>();
        }

        StartCoroutine(SpawnBubbles());
    }

    IEnumerator SpawnBubbles()
    {
        while (true)
        {
            SpawnBubble();
            spawnCount++;

            // Adjust difficulty every X spawns
            if (spawnCount % spawnsBeforeChange == 0)
            {
                bubbleSpeed += bubbleSpeedChangeAmount;
                spawnRate -= bubbleSpawnRateChangeAmount;
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnBubble()
    {
        int spawnRow = Random.Range(0, spawnRows.Length);
        Transform spawnTransform = spawnRows[spawnRow];

        if (spawnTransform != null)
        {
            GameObject newBubble = Instantiate(bubblePrefab, spawnTransform.position, Quaternion.identity);
            newBubble.GetComponent<Bubble>().SetSpeed(bubbleSpeed);
            rows[spawnRow].Add(newBubble);
        }
    }

    public void DeleteLeftmostBubble(int rowIndex)
    {
        if (rows[rowIndex].Count > 0)
        {
            GameObject leftmostBubble = rows[rowIndex][0];

            if (leftmostBubble != null)
            {
                Vector3 bubblePosition = leftmostBubble.transform.position;
                bool isInBurstZone = leftmostBubble.GetComponent<Bubble>().isInBurstZone;

                // Score calculation handled by GameManager
                GameManager.Instance.CalculateScore(bubblePosition, isInBurstZone);

                rows[rowIndex].RemoveAt(0);
                Destroy(leftmostBubble);

                Instantiate(GameManager.Instance.VFXBubbleBurst, bubblePosition, Quaternion.identity);
                GameManager.Instance.PlayBubblePopAudio();
            }
        }
    }
}