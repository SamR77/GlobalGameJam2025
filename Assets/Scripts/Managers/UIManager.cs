using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System;

// Sam Robichaud 
// NSCC Truro 2024
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class UIManager : MonoBehaviour
{
    // Static instance property to provide global access
    public static UIManager Instance { get; private set; }


    [Header("Progress Bar")]
    public Slider progressBar;

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

    public void InstantiatePopupResults(Canvas resultCanvas, Vector3 burstPosition)
    {
        Instantiate(resultCanvas, burstPosition, Quaternion.identity);
    }

    private void Awake()
    {
        Instance = this;        
    }

    public void UpdateProgressBar(float HappynessAmount)
    {

        if (progressBar != null)
        {
                      
            progressBar.value = HappynessAmount;
        }
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
}
