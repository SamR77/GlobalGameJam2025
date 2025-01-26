using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleGameplayManager : MonoBehaviour
{
    public int maxScore = 1000;
    public int Score;

    public GameObject bubblePrefab;

    public Transform SpawnRow_00;
    public Transform SpawnRow_01;
    public Transform SpawnRow_02;
    public Transform SpawnRow_03;

    public float perfectMinPercent = 90f;   // Min percent distance to get a "Perfect" 
    public float goodMinPercent = 40f;      // Min percent distance to get a "Good" 
    public float earlyLateMinPercent = 0f;  // Min percent distance to get an "Early or late" 

    public int PerfectScore = 100;
    public int GoodScore = 50;
    public int EarlyLateScore = 25;
    public int MissPenalty = -50;

    private Vector3 burstZoneCenter;        // The very center of the Burst Zone
    private float maxDistance;              // The maximum distance of the Burst Zone

    public TextMeshProUGUI resultText;      // Reference to the TextMeshPro component for displaying result
    public TextMeshProUGUI scoreText;       // Reference to the TextMeshPro component for displaying score

    private Collider burstZoneCollider;       // The trigger zone collider

    private List<GameObject>[] rows;        // List of objects for each row



    void Start()
    {
        // Get the Burst Zone object and calculate the center and maximum distance
        GameObject burstZoneObject = GameObject.FindGameObjectWithTag("BurstZone");
        burstZoneCollider = burstZoneObject.GetComponent<Collider>();
        burstZoneCenter = burstZoneCollider.bounds.center;
        maxDistance = burstZoneCollider.bounds.extents.x; // Half of the collider width

        Score = maxScore / 2; // Start with half of the maximum score
        scoreText.text = Score.ToString();


        // Initialize the rows (4 rows in total)
        rows = new List<GameObject>[4];
        for (int i = 0; i < 4; i++)
        {
            rows[i] = new List<GameObject>();
        }

        // Spawn bubbles every 2 seconds
        InvokeRepeating(nameof(SpawnBubble), 2f, 2f);
    }

    void Update()
    {
        //Debug.Log("Row 0 has " + rows[0].Count + " objects.");
        //Debug.Log("Row 1 has " + rows[1].Count + " objects.");
        //Debug.Log("Row 2 has " + rows[2].Count + " objects.");
        //Debug.Log("Row 3 has " + rows[3].Count + " objects.");   
    }
 

    // Calculate score and display the appropriate result
    public void CalculateScore(Vector3 bubblePosition, bool isInBurstZone)
    {
        
        // Check if the bubble is inside the Burst Zone collider
        if (!isInBurstZone || !burstZoneCollider.bounds.Contains(bubblePosition))
        {            
            UpdateResultText("Miss!");
            UpdateScore(MissPenalty);
            return;
        }

        // Calculate the horizontal distance from the Burst Zone center
        float distanceX = Mathf.Abs(burstZoneCenter.x - bubblePosition.x);

        // Calculate percentage distance relative to the maximum distance
        float percentage = Mathf.Clamp01(1 - (distanceX / maxDistance)) * 100;

        // Determine the result based on percentage thresholds
        if (percentage >= perfectMinPercent)
        {
            UpdateResultText("Perfect!");
            UpdateScore(PerfectScore);
        }
        else if (percentage >= goodMinPercent)
        {
            UpdateResultText("Good!");
            UpdateScore(GoodScore);
        }
        else if (percentage >= earlyLateMinPercent)
        {
            UpdateResultText("Early/Late!");
            UpdateScore(EarlyLateScore);
        }
        else
        {
            UpdateResultText("Miss!");
            UpdateScore(MissPenalty);
            
     
        }
    }

    // Update the result text with the appropriate message
    private void UpdateResultText(string result)
    {
        // TODO: convert to spawn a UIText object prefab(at position of Bubble) with the result message that fades over time
        // may have to call in DeleteLeftmostBubble() method
        


        if (resultText != null)
        {
            resultText.text = result;
            resultText.color = new Color(resultText.color.r, resultText.color.g, resultText.color.b, 1f);
        }
    }



    void UpdateScore(int bubbleAccuracyScore)
    {
        Score += bubbleAccuracyScore;

        // cap the top value of the score
        if (Score <= 0)
        {
            Debug.Log("You Lose!");
            GameManager.Instance.gameStateManager.GameOver();
        }

        else if (Score > maxScore)
        {
            Score = maxScore;
        }      

        Debug.Log("Score: " + Score);

        scoreText.text = Score.ToString();
 

    }


    // Spawns a bubble at a random row
    private void SpawnBubble()
    {
        int spawnRow = Random.Range(0, 4); // Randomly select a row (0-3)
        Transform spawnTransform = spawnRow switch
        {
            0 => SpawnRow_00,
            1 => SpawnRow_01,
            2 => SpawnRow_02,
            3 => SpawnRow_03,
            _ => null
        };

        if (spawnTransform != null)
        {
            GameObject newBubble = Instantiate(bubblePrefab, spawnTransform.position, Quaternion.identity);
            rows[spawnRow].Add(newBubble);
        }
    }

    private void DeleteLeftmostBubble(int rowIndex)
    {
        if (rows[rowIndex].Count > 0)
        {
            GameObject leftmostBubble = rows[rowIndex][0];

            if (leftmostBubble != null) // Check if the GameObject is valid
            {
                // Cache necessary data before destroying the object
                Vector3 bubblePosition = leftmostBubble.transform.position;
                bool isInBurstZone = leftmostBubble.GetComponent<Bubble>().isInBurstZone;

                // Calculate the score for the leftmost bubble
                CalculateScore(bubblePosition, isInBurstZone);

                // Remove the bubble from the list and destroy it
                rows[rowIndex].RemoveAt(0);
                Destroy(leftmostBubble);
            }
            
        }
    }

    public void HandleBubbleClear(Bubble bubble)
    {
        // Apply the "miss" penalty
        UpdateScore(MissPenalty);

        // Find and remove the bubble from its row list
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].Contains(bubble.gameObject))
            {
                rows[i].Remove(bubble.gameObject);
                break;
            }
        }
    }


    void OnEnable()
    {
        // Subscribe to the Action events
        Actions.W_KeyEvent += () => DeleteLeftmostBubble(0); // Row 0 (W key)
        Actions.A_KeyEvent += () => DeleteLeftmostBubble(1); // Row 1 (A key)
        Actions.S_KeyEvent += () => DeleteLeftmostBubble(2); // Row 2 (S key)
        Actions.D_KeyEvent += () => DeleteLeftmostBubble(3); // Row 3 (D key)
    }

    void OnDisable()
    {
        // Unsubscribe from the Action events to prevent memory leaks
        Actions.W_KeyEvent -= () => DeleteLeftmostBubble(0);
        Actions.A_KeyEvent -= () => DeleteLeftmostBubble(1);
        Actions.S_KeyEvent -= () => DeleteLeftmostBubble(2);
        Actions.D_KeyEvent -= () => DeleteLeftmostBubble(3);
    }


    //reset the game stats
    public void ResetGameStats()
    {
        Score = maxScore / 2; // Start with half of the maximum score
        scoreText.text = Score.ToString();
    }


}
