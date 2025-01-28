using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Sam Robichaud 
// NSCC Truro 2025
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class GameManager : MonoBehaviour
{
    // Static instance property to provide global access
    public static GameManager Instance { get; private set; }

    // References to scripts
    //public LevelManager levelManager;
    public UIManager uIManager;
    public GameStateManager gameStateManager;

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        #endregion
        ReferenceCheck();
    }

    private void ReferenceCheck()
    {
        if (gameStateManager == null)
        {
            Debug.LogWarning("gameStateManager reference is empty, attempting to find in children!");

            // Attempt to find component in children
            gameStateManager = GetComponentInChildren<GameStateManager>();

            // Check to see if it's still empty
            if (gameStateManager == null)
            {
                Debug.LogError("Player reference is missing in GameManager and its children!");
            }
        }

        if (uIManager == null)
        {
            Debug.LogWarning("uIManager reference is empty, attempting to find in children!");

            // Attempt to find component in children
            uIManager = GetComponentInChildren<UIManager>();

            // Check to see if it's still empty
            if (uIManager == null)
            {
                Debug.LogError("uIManager reference is missing in GameManager and its children!");
            }
        }


    }


    [Header("Points awarded based on accuracy")]
    public int maxScore = 1000;
    public int PerfectScore = 100;
    public int GoodScore = 50;
    public int EarlyLateScore = 25;
    public int MissPenalty = -50;

    [Header("Reference to BabyAnimator")]
    public Animator animator; // Assign your Animator in the Inspector

    public GameObject tears;

    [Header("Reference to AudioSource")]
    public AudioSource audioSource;

    [Header("Bubble Pop Audio Clips")]
    public AudioClip[] bubblePopSounds;



    /*
    [Header("Result Popup Messages")]
    public Canvas resultPopupMissed;
    public Canvas resultPopupPerfect;
    public Canvas resultPopupGood;
    public Canvas resultPopupEarly;
    public Canvas resultPopupLate;
    */

    [Header("VFX Prefabs")]
    public ParticleSystem VFXBubbleBurst;


    [Header("Bubble Prefab")]
    public GameObject bubblePrefab;

    [Header("Gameplay Rows")]
    public Transform SpawnRow_00;
    public Transform SpawnRow_01;
    public Transform SpawnRow_02;
    public Transform SpawnRow_03;

    [Header("Tolerances for popping accuracy")]
    public float perfectMinPercent = 90f;   // Min percent distance to get a "Perfect" 
    public float goodMinPercent = 40f;      // Min percent distance to get a "Good" 
    public float earlyLateMinPercent = 0f;  // Min percent distance to get an "Early or late" 


    // Private Variables

    private float score;                      // stores the current gameplay score
    private float happynessPercentage;            // stores the current gameplay score as a percentage of the max score

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

        score = maxScore / 2; // Start with half of the maximum score
        // Delete scoreText.text = score.ToString();


        happynessPercentage = score / maxScore; // Convert score to percentage (0 to 1)
        UIManager.Instance.UpdateProgressBar(happynessPercentage);

        // Initialize the rows (4 rows in total)
        rows = new List<GameObject>[4];
        for (int i = 0; i < 4; i++)
        {
            rows[i] = new List<GameObject>();
        }

        // Spawn bubbles every 2 seconds
        InvokeRepeating(nameof(SpawnBubble), 1f, 1f);
    }

    void Update()
    {
        //Debug.Log("Row 0 has " + rows[0].Count + " objects.");
        //Debug.Log("Row 1 has " + rows[1].Count + " objects.");
        //Debug.Log("Row 2 has " + rows[2].Count + " objects.");
        //Debug.Log("Row 3 has " + rows[3].Count + " objects.");

        if (happynessPercentage <= 0.1f)
        {
            tears.SetActive(true);
        }
        else
        {
            tears.SetActive(false);
        }
    }








    // Calculate score and display the appropriate result
    public void CalculateScore(Vector3 bubblePosition, bool isInBurstZone)
    {

        // Check if the bubble is NOT within the scoringZone
        if (!isInBurstZone)
        {
            UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupMissed, bubblePosition);
            UpdateScore(MissPenalty);                  
        }

        // if the bubble is burst within the scoringZone
        else if (isInBurstZone)
        {
            // Calculate the horizontal distance from the Burst Zone center
            float distanceX = Mathf.Abs(burstZoneCenter.x - bubblePosition.x);

            // Calculate percentage distance relative to the maximum distance
            float percentage = Mathf.Clamp01(1 - (distanceX / maxDistance)) * 100;

            // Determine the result based on percentage thresholds
            if (percentage >= perfectMinPercent)
            {
                // PERFECT! bubble popped inside the perfect zone
                UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupPerfect, bubblePosition);
                UpdateScore(PerfectScore);
            }
            else if (percentage >= goodMinPercent)
            {
                // GOOD! bubble popped within the good zone
                UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupGood, bubblePosition);
                UpdateScore(GoodScore);                
            }
            else if (percentage >= earlyLateMinPercent)
            {
                // Early or Late: check if the pop was early or late
                if (bubblePosition.x < burstZoneCenter.x)
                {
                    // EARLY! bubble popped too early inside the defined zones
                    UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupEarly, bubblePosition);
                }
                else
                {
                    // LATE! bubble popped too late inside the defined zones
                    UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupLate, bubblePosition);            
                }
                UpdateScore(EarlyLateScore);
            }
            else
            {
                Debug.Log("is this even getting triggered?!");

                // MISS! bubble popped too early or too late outside of the defined zones
                //Missed(bubblePosition);
            }



        }


        
    }






    void UpdateScore(int bubbleAccuracyScore)
    {
        score += bubbleAccuracyScore;

        happynessPercentage = score / maxScore; // Convert score to percentage (0 to 1)

        // cap the top value of the score
        if (score <= 0)
        {
            score = 0;
            //Debug.Log("You Lose!");
            GameManager.Instance.gameStateManager.GameOver();
        }

        else if (score > maxScore)
        {
            score = maxScore;
        }

        //Debug.Log("Score: " + Score);

        // Delete scoreText.text = score.ToString();


        
        UIManager.Instance.UpdateProgressBar(happynessPercentage);
        UpdateBabyAnimator();

    }


    void UpdateBabyAnimator()
    {
        if (animator != null)
        {
            //Debug.Log("ScorePercentage on animator: " + scorePercentage);
            animator.SetFloat("HappyLevel", happynessPercentage);
        }
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

                // instantiate a VFX prefab at the position of the bubble
                Instantiate(VFXBubbleBurst, bubblePosition, Quaternion.identity);

                // play bubble pop Audio
                PlayBubblePopAudio();
            }
        }
    }

    void PlayBubblePopAudio()
    {




        // Select a random bubble pop sound from the array
        int randomIndex = Random.Range(0, bubblePopSounds.Length);

        // Get the name of the selected audio clip
        // Debug.Log("Selected Audio Clip: " + bubblePopSounds[randomIndex].name);

        // Randomize the pitch of the audio clip
        float minPitch = 0.75f;
        float maxPitch = 1.25f;
        audioSource.pitch = Random.Range(minPitch, maxPitch);


        //play select audio clip
        audioSource.clip = bubblePopSounds[randomIndex];
        audioSource.Play();        
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
        score = maxScore / 2; // Start with half of the maximum score
        
        happynessPercentage = score / maxScore; // Convert score to percentage (0 to 1)
        UIManager.Instance.UpdateProgressBar(happynessPercentage);
        UpdateBabyAnimator();
        
    }


}
