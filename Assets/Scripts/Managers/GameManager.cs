using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Dependencies")]
    public UIManager uIManager;
    public GameStateManager gameStateManager;
    public BubbleManager bubbleManager;
    public AudioManager audioManager;

    [Header("Gameplay Settings")]
    public float bubbleStartingSpeed = 2f;    
    [Tooltip("How frequent bubbles spawn, smaller number = more frequent spawns")] public float bubbleStartingSpawnRate = 2f;    
    public float bubbleSpeedChangeAmount = 0.1f;    
    public float bubbleSpawnRateChangeAmount = 0.2f;    
    public int bubbleSpawnsBeforeDifficultyIncrease = 10;

    // Cached values for resetting game
    public float cachedBubbleStartingSpeed;
    public float cachedBubbleStartingSpawnRate;
    public float cachedBubbleSpeedChangeAmount;
    public float cachedBubbleSpawnRateChangeAmount;




    [Header("Happyness Level change based on accuracy")]
    public int maxScore = 1000;             // determines total range of Happyness Bar
    public int PerfectScore = 100;          
    public int GoodScore = 50;
    public int EarlyLateScore = 25;
    public int MissPenalty = -50;

    [Header("Reference to BabyAnimator")]
    public Animator animator; // TODO: move to an AnimationManager script // Assign your Animator in the Inspector
    public GameObject tears; // TODO: move to an AnimationManager script



   


    [Header("VFX Prefabs")]
    public ParticleSystem VFXBubbleBurst;


    [Header("Tolerances for popping accuracy")]
    public float perfectMinPercent = 90f;       // Min percent distance to get a "Perfect" 
    public float goodMinPercent = 40f;          // Min percent distance to get a "Good" 
    public float earlyLateMinPercent = 0f;      // Min percent distance to get an "Early or late" 



    // Private Variables
    private float score;                        // stores the current gameplay score
    private float happynessPercentage;          // stores the current gameplay score as a percentage of the max score
    private Vector3 scoreZoneCenter;            // The very center of the score Zone
    private float maxDistance;                  // The maximum distance of the score Zone (from edge to center)

    //public TextMeshProUGUI resultText;        // Reference to the TextMeshPro component for displaying result
    //public TextMeshProUGUI scoreText;         // Reference to the TextMeshPro component for displaying score

    private Collider scoreZoneCollider;         // The score zone collider

   




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

        // Cache the starting stats
        cachedBubbleStartingSpeed = bubbleStartingSpeed;
        cachedBubbleStartingSpawnRate = bubbleStartingSpawnRate;
        cachedBubbleSpeedChangeAmount = bubbleSpeedChangeAmount;
        cachedBubbleSpawnRateChangeAmount = bubbleSpawnRateChangeAmount;
    }


    void Start()
    {
        // Get the Burst Zone object and calculate the center and maximum distance
        GameObject burstZoneObject = GameObject.FindGameObjectWithTag("BurstZone");
        scoreZoneCollider = burstZoneObject.GetComponent<Collider>();
        scoreZoneCenter = scoreZoneCollider.bounds.center;
        maxDistance = scoreZoneCollider.bounds.extents.x;

        score = maxScore / 2;
        happynessPercentage = score / maxScore;
        UIManager.Instance.UpdateProgressBar(happynessPercentage);  
        

        


    }

    void Update()
    {
        //Debug.Log("Row 0 has " + rows[0].Count + " objects.");
        //Debug.Log("Row 1 has " + rows[1].Count + " objects.");
        //Debug.Log("Row 2 has " + rows[2].Count + " objects.");
        //Debug.Log("Row 3 has " + rows[3].Count + " objects.");

        // Check if the baby should be crying
        if (happynessPercentage <= 0.1f)
        {
            tears.SetActive(true);
        }
        else
        {
            tears.SetActive(false);
        }
    }




    public void CalculateScore(Vector3 bubblePosition, bool isInScoreZone)
    {
        if (!isInScoreZone)
        {
            UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupMissed, bubblePosition);
            UpdateScore(MissPenalty);
        }
        else
        {
            float distanceX = Mathf.Abs(scoreZoneCenter.x - bubblePosition.x);
            float percentage = Mathf.Clamp01(1 - (distanceX / maxDistance)) * 100;

            if (percentage >= perfectMinPercent)
            {
                UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupPerfect, bubblePosition);
                UpdateScore(PerfectScore);
            }
            else if (percentage >= goodMinPercent)
            {
                UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupGood, bubblePosition);
                UpdateScore(GoodScore);
            }
            else
            {
                if (bubblePosition.x < scoreZoneCenter.x)
                    UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupLate, bubblePosition);
                else
                    UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupEarly, bubblePosition);

                UpdateScore(EarlyLateScore);
            }
        }
    }






    public void UpdateScore(int bubbleAccuracyScore)
    {
        score += bubbleAccuracyScore;
        happynessPercentage = score / maxScore;

        if (score <= 0)
        {
            score = 0;
            GameManager.Instance.gameStateManager.GameOver();
        }
        else if (score > maxScore)
        {
            score = maxScore;
        }

        UIManager.Instance.UpdateProgressBar(happynessPercentage);
        UpdateBabyAnimator();
    }


    void UpdateBabyAnimator() // TODO: Consider moving into it's own Manager Script.. although there is not much at the moment.. we may want to add more animations later
    {
        if (animator != null)
        {            
            animator.SetFloat("HappyLevel", happynessPercentage);
        }
    }





    

    // this gets called when ever a bubble collides with the clearing zone
    public void HandleBubbleClear(Bubble bubble)
    {        
        // Apply the "miss" penalty
        UpdateScore(MissPenalty);
    }


    void OnEnable()
    {
        Actions.W_KeyEvent += () => bubbleManager.DeleteLeftmostBubble(0);
        Actions.A_KeyEvent += () => bubbleManager.DeleteLeftmostBubble(1);
        Actions.S_KeyEvent += () => bubbleManager.DeleteLeftmostBubble(2);
        Actions.D_KeyEvent += () => bubbleManager.DeleteLeftmostBubble(3);
    }

    void OnDisable()
    {
        Actions.W_KeyEvent -= () => bubbleManager.DeleteLeftmostBubble(0);
        Actions.A_KeyEvent -= () => bubbleManager.DeleteLeftmostBubble(1);
        Actions.S_KeyEvent -= () => bubbleManager.DeleteLeftmostBubble(2);
        Actions.D_KeyEvent -= () => bubbleManager.DeleteLeftmostBubble(3);
    }

    public void GameReset()
    {
        // Reset core game stats
        score = maxScore / 2;
        happynessPercentage = score / maxScore;

        // Reset bubble stats to cached values
        bubbleStartingSpeed = cachedBubbleStartingSpeed;
        bubbleStartingSpawnRate = cachedBubbleStartingSpawnRate;
        bubbleSpeedChangeAmount = cachedBubbleSpeedChangeAmount;
        bubbleSpawnRateChangeAmount = cachedBubbleSpawnRateChangeAmount;

        // Initialize all stat values in BubbleManager
        BubbleManager.Instance.initializeBubbleStats();

        // Reset UI
        UIManager.Instance.UpdateProgressBar(happynessPercentage);
        tears.SetActive(false);

        // Update baby animator
        UpdateBabyAnimator();

        // Clear all existing bubbles
        for (int i = 0; i < BubbleManager.Instance.spawnLanes.Length; i++)
        {
            foreach (GameObject bubble in BubbleManager.Instance.lanes[i].ToList())
            {
                if (bubble != null)
                {
                    Destroy(bubble);
                }
            }
            BubbleManager.Instance.lanes[i].Clear();
        }

        // Reset bubble spawn count and restart spawning
        BubbleManager.Instance.ResetSpawnCount();  
        
        // Clear all existing popups
        UIManager.Instance.ClearResultsPopups();

        // Clear all existing bubbleVFX's
        UIManager.Instance.ClearBubbleVFX();



    }



}
