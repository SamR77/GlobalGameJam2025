﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public BubbleGameplayManager bubbleGameplayManager;




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
}
