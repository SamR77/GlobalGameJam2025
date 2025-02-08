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
    public float bubbleStartingSpeed = 3f;    
    [Tooltip("How frequent bubbles spawn, smaller number = more frequent spawns")] public float bubbleStartingSpawnRate = 2f;    
    public float bubbleSpeedChangeAmount = 0.25f;    
    public float bubbleSpawnRateChangeAmount = 0.2f;    
    public int bubbleSpawnsBeforeDifficultyIncrease = 5;

    // Cached values for resetting game
    public float cachedBubbleStartingSpeed;
    public float cachedBubbleStartingSpawnRate;
    public float cachedBubbleSpeedChangeAmount;
    public float cachedBubbleSpawnRateChangeAmount;

    [Header("Happyness Level change based on accuracy")]
    public int maxHappinessPoints = 1000;             // determines total range of Happyness Bar
    public int perfectTimingAward = 100;          
    public int goodTimingAward = 50;
    public int earlyLateTimingAward = 25;
    public int missPenalty = -50;

    [Header("Reference to BabyAnimator")]
    public Animator babyAnimator; // TODO: move to an AnimationManager script // Assign your Animator in the Inspector
    public GameObject tears; // TODO: move to an AnimationManager script

    [Header("VFX Prefabs")]
    public ParticleSystem VFXBubbleBurst;

    [Header("Tolerances for popping accuracy")]
    public float perfectMinPercent = 95f;       // Min percent distance to get a "Perfect" 
    public float goodMinPercent = 35f;          // Min percent distance to get a "Good" 
    public float earlyLateMinPercent = 0f;      // Min percent distance to get an "Early or late" 

    [Header("Counts of Popped Bubbles")]
    [SerializeField] internal int countPerfectBubbles;
    [SerializeField] internal int countGoodBubbles;
    [SerializeField] internal int countEarlyLateBubbles;
    [SerializeField] internal int countMissedBubbles;
    [SerializeField] internal string timePlayed;


    // Private Variables
    public float happinessPoints;                        // stores the current gameplay score
    public float happinessPercentage;          // stores the current gameplay score as a percentage of the max score
    private Vector3 scoreZoneCenter;            // The very center of the score Zone
    private float maxDistance;                  // The maximum distance of the score Zone (from edge to center)

    private Collider scoreZoneCollider;         // The score zone collider

    public float playTime;
    public bool playTimerOn = false;

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

        happinessPoints = maxHappinessPoints / 2;
        happinessPercentage = happinessPoints / maxHappinessPoints;
        UIManager.Instance.UpdateProgressBar(happinessPercentage); 
    }

    void Update()
    {
        //Debug.Log("Row 0 has " + rows[0].Count + " objects.");
        //Debug.Log("Row 1 has " + rows[1].Count + " objects.");
        //Debug.Log("Row 2 has " + rows[2].Count + " objects.");
        //Debug.Log("Row 3 has " + rows[3].Count + " objects.");

        // Check if the baby should be crying
        if (happinessPercentage <= 0.1f)
        {
            tears.SetActive(true);
        }
        else
        {
            tears.SetActive(false);
        }

        HandlePlayTimer();

    }

    private void HandlePlayTimer()
    {
        if (playTimerOn == true)
        {
            playTime += Time.deltaTime;
            UIManager.Instance.UpdatePlayTimeUI(playTime);
        }
    }   


    public void CalculateScore(Vector3 bubblePosition, bool isInScoreZone)
    {
        if (!isInScoreZone)
        {
            UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupMissed, bubblePosition);
            countMissedBubbles += 1;
            uIManager.UpdatePoppedBubbleCountUI();
            UpdateScore(missPenalty);
        }
        else
        {
            float distanceX = Mathf.Abs(scoreZoneCenter.x - bubblePosition.x);
            float percentage = Mathf.Clamp01(1 - (distanceX / maxDistance)) * 100;

            if (percentage >= perfectMinPercent)
            {
                UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupPerfect, bubblePosition);
                countPerfectBubbles += 1;
                uIManager.UpdatePoppedBubbleCountUI();
                UpdateScore(perfectTimingAward);
            }
            else if (percentage >= goodMinPercent)
            {
                UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupGood, bubblePosition);
                countGoodBubbles += 1;
                uIManager.UpdatePoppedBubbleCountUI();
                UpdateScore(goodTimingAward);
            }
            else
            {
                if (bubblePosition.x < scoreZoneCenter.x)
                    UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupLate, bubblePosition);
                else
                    UIManager.Instance.InstantiatePopupResults(UIManager.Instance.popupEarly, bubblePosition);

                countEarlyLateBubbles += 1;
                uIManager.UpdatePoppedBubbleCountUI();
                UpdateScore(earlyLateTimingAward);
            }
        }
    }


    public void UpdateScore(int bubbleAccuracyScore)
    {
        happinessPoints += bubbleAccuracyScore;
        happinessPercentage = happinessPoints / maxHappinessPoints;

        if (happinessPoints <= 0)
        {
            happinessPoints = 0;
            GameManager.Instance.gameStateManager.GameOver();
        }
        else if (happinessPoints > maxHappinessPoints)
        {
            happinessPoints = maxHappinessPoints;
        }

        UIManager.Instance.UpdateProgressBar(happinessPercentage);
        UpdateBabyAnimator();
    }


    void UpdateBabyAnimator() // TODO: Consider moving into it's own Manager Script.. although there is not much at the moment.. we may want to add more animations later
    {
        if (babyAnimator != null)
        {                 
            // Start lerping the HappyLevel parameter
            StartCoroutine(lerpBabyAnimator(babyAnimator.GetFloat("HappyLevel"), happinessPercentage));
        }
    }



    private IEnumerator lerpBabyAnimator(float startValue, float targetValue)
    {
        float timeElapsed = 0f; // Time elapsed for lerping
        float lerpDuration = 1.0f; // Duration of the lerp (in seconds)

        while (timeElapsed < lerpDuration)
        {
            // Lerp between current and target HappyLevel value
            float newValue = Mathf.Lerp(startValue, targetValue, timeElapsed / lerpDuration);
            babyAnimator.SetFloat("HappyLevel", newValue);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the secondary progress bar ends exactly at the target value
        babyAnimator.SetFloat("HappyLevel", targetValue);
    }


    public void GameReset()
    {
        // Reset core game stats
        happinessPoints = maxHappinessPoints / 2;
        happinessPercentage = happinessPoints / maxHappinessPoints;

        // reset all bubble counts
        countPerfectBubbles = 0;
        countGoodBubbles = 0;
        countEarlyLateBubbles = 0;
        countMissedBubbles = 0;
        timePlayed = "";
        uIManager.UpdatePoppedBubbleCountUI();

        // Reset the PlayTimer
        playTime = 0;

        // Reset bubble stats to cached values
        bubbleStartingSpeed = cachedBubbleStartingSpeed;
        bubbleStartingSpawnRate = cachedBubbleStartingSpawnRate;
        bubbleSpeedChangeAmount = cachedBubbleSpeedChangeAmount;
        bubbleSpawnRateChangeAmount = cachedBubbleSpawnRateChangeAmount;

        if(BubbleManager.Instance == null)
        {
            Debug.Log("BubbleManager is null");
        }
        else if (BubbleManager.Instance != null)
        {
            // Initialize all stat values in BubbleManager
            BubbleManager.Instance.InitializeBubbleStats();

            // Clear all existing bubbles
            if (BubbleManager.Instance.lanes != null)
            {
                for (int i = 0; i < BubbleManager.Instance.spawnLanes.Length; i++)
                {
                    if (BubbleManager.Instance.lanes[i] != null)
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
                }
            }

            // Reset bubble spawn count
            BubbleManager.Instance.ResetSpawnCount();
        }

        
        if (UIManager.Instance == null)
        {
            Debug.Log("UIManager is null");
        }
        else if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateProgressBar(happinessPercentage);
            UIManager.Instance.ClearResultsPopups();
            UIManager.Instance.ClearBubbleVFX();  // this is causing issues on reset
        }

        // Update visual elements if available        
        
        if (tears == null)
        {
            Debug.Log("Tears is null");
        }       
        else if (tears != null)
        {
            tears.SetActive(false);
        }

        if(babyAnimator == null)
        {
            Debug.Log("Animator is null");
        }
        else if (babyAnimator != null)
        {
            //happinessPoints = maxHappinessPoints / 2;
            //happinessPercentage = happinessPoints / maxHappinessPoints;

            Debug.Log(happinessPercentage);
            babyAnimator.SetFloat("HappyLevel", happinessPercentage);
                       
        }
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


}
