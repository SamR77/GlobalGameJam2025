using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;



// Sam Robichaud 
// NSCC Truro 2024
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class UIManager : MonoBehaviour
{
    // Static instance property to provide global access
    public static UIManager Instance { get; private set; }

    [Header("Popped Bubbles Total")]
    public TMP_Text textPoppedTotal;      // Primary bar that updates immediately
    public TMP_Text textplayTimer;    // Secondary bar that lerps


    [Header("Progress Bars")]
    public Slider primaryHappinessBar;      // Primary bar that updates immediately
    public Slider secondaryHappinessBar;    // Secondary bar that lerps


    [Header("Result Popup Messages")]
    public Canvas popupMissed;
    public Canvas popupPerfect;
    public Canvas popupGood;
    public Canvas popupEarly;
    public Canvas popupLate;

    // References to UI Panels
    [Header("UI Screens")]
    public GameObject mainMenuUI;
    public GameObject gamePlayUI;
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject creditsMenuUI;

    private GameManager gameManager;

    private void Awake()
    {
        Instance = this;
        gameManager = GameManager.Instance;
    }
    public void InstantiatePopupResults(Canvas resultCanvas, Vector3 burstPosition)
    {
        Instantiate(resultCanvas, burstPosition, Quaternion.identity);
    }

    public void UpdatePoppedBubbleCountUI()
    {
        // only counts bubbles that were not missed
        int totalPopped = (gameManager.countPerfectBubbles + gameManager.countGoodBubbles + gameManager.countEarlyLateBubbles);

        textPoppedTotal.text = totalPopped.ToString();
    }

    public void UpdateProgressBar(float targetHappinessAmount)
    {
        

        if (primaryHappinessBar != null)
        {
            primaryHappinessBar.value = targetHappinessAmount;
        }
        if (secondaryHappinessBar != null)
        {
            // Start lerping the secondary progress bar
            StartCoroutine(LerpSecondaryProgressBar(secondaryHappinessBar.value, targetHappinessAmount));
        }
    }

    private IEnumerator LerpSecondaryProgressBar(float startValue, float targetValue)
    {
        float timeElapsed = 0f; // Time elapsed for lerping
        float lerpDuration = 1f; // Duration of the lerp (in seconds)

        while (timeElapsed < lerpDuration)
        {
            // Lerp between the current and target value for the secondary progress bar
            secondaryHappinessBar.value = Mathf.Lerp(startValue, targetValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the secondary progress bar ends exactly at the target value
        secondaryHappinessBar.value = targetValue;
    }



    public void UIMainMenu()
    {
        DisableAllUIPanels();
        mainMenuUI.SetActive(true);
    }

    public void UIGamePlay()
    {
        DisableAllUIPanels();
        gamePlayUI.SetActive(true);
    }

    public void UIGameOver()
    {
        DisableAllUIPanels();
        gameOverUI.SetActive(true);
    }

    public void UIPaused()
    {
        DisableAllUIPanels();
        pauseMenuUI.SetActive(true);
    }

    public void UIOptions()
    {
        DisableAllUIPanels();
        optionsMenuUI.SetActive(true);
    }

    public void UICredits()
    {
        DisableAllUIPanels();
        creditsMenuUI.SetActive(true);
    }

    public void DisableAllUIPanels()
    {
        mainMenuUI.SetActive(false);
        gamePlayUI.SetActive(false);
        gameOverUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        creditsMenuUI.SetActive(false);
    }

    public void EnableAllUIPanels()
    {
        mainMenuUI.SetActive(true);
        gamePlayUI.SetActive(true);
        gameOverUI.SetActive(true);
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(true);
        creditsMenuUI.SetActive(true);
    }

    internal void ClearResultsPopups()
    {
        // find all objects in scene with tag "ResultsPopup" and destroy them
        GameObject[] popups = GameObject.FindGameObjectsWithTag("ResultsPopup");
        foreach (GameObject popup in popups)
        {
            Destroy(popup);
        }
    }

    internal void ClearBubbleVFX()
    {
        // find all objects in scene with tag "BubblePopVFX" and destroy them
        GameObject[] popups = GameObject.FindGameObjectsWithTag("BubblePopVFX");
        foreach (GameObject popup in popups)
        {
            Destroy(popup);
        }

    }

    internal void UpdatePlayTimeUI(float playTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(playTime);

        int minutes = time.Minutes;
        int seconds = time.Seconds;
        int centiseconds = time.Milliseconds / 10;

        textplayTimer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
    }
}
