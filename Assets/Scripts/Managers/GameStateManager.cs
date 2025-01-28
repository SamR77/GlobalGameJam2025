using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sam Robichaud 
// NSCC Truro 2025
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)


public class GameStateManager : MonoBehaviour
{
    [Header("Debug (read only)")]
    [SerializeField] private string lastActiveState;
    [SerializeField] private string currentActiveState;

    // Private variables to store state information
    private IGameState currentGameState;  // Current active state
    private IGameState lastGameState;     // Last active state (kept private for encapsulation)

    // Public getter for accessing the lastGameState externally (read-only access)
    public IGameState LastGameState
    {
        get { return lastGameState; }
    }

    // Instantiate game state objects
    public GameState_GameInit gameState_GameInit = new GameState_GameInit();
    public GameState_ResetGameStats gameState_ResetGameStats = new GameState_ResetGameStats();
    public GameState_MainMenu gameState_MainMenu = new GameState_MainMenu();
    public GameState_GamePlay gameState_GamePlay = new GameState_GamePlay();
    public GameState_GameOver gameState_GameOver = new GameState_GameOver();
    public GameState_Options gameState_Options = new GameState_Options();
    public GameState_Credits gameState_Credits = new GameState_Credits();
    public GameState_Paused gameState_Paused = new GameState_Paused();



    // Start is called before the first frame update
    void Start()
    {
        


        // Sets currentGameState to GameInitState when GameStateManager is initialized / first loaded
        // GameInitState is responsible for initializing/resetting the game
        currentGameState = gameState_GameInit;

        // Enter the initial game state
        currentGameState.EnterState(this);
    }

    #region State Machine Update Calls


    // Fixed update is called before update, and is used for physics calculations
    private void FixedUpdate()
    {
        // Handle physics updates in the current active state (if applicable)
        currentGameState.FixedUpdateState(this);
    }

    private void Update()
    {
        // Handle regular frame updates in the current active state
        currentGameState.UpdateState(this);

        // Keeping track of active and last states for debugging purposes
        // TODO: I can probably move these out of Update and just set them when switching states ... look into moving down into SwitchToState method
        currentActiveState = currentGameState.ToString();   // Show current state in Inspector
        lastActiveState = lastGameState?.ToString();        // Show last state in Inspector
    }

    // LateUpdate for any updates that need to happen after regular Update
    private void LateUpdate()
    {
        currentGameState.LateUpdateState(this);
    }

    #endregion

    // Method to switch between states
    public void SwitchToState(IGameState newState)
    {
        // Exit the current state (handling cleanup and transitions)
        currentGameState.ExitState(this);

        // Store the current state as the last state before switching
        lastGameState = currentGameState;

        // Switch to the new state
        currentGameState = newState;

        // Enter the new state (initialize any necessary logic)
        currentGameState.EnterState(this);
    }



    public void Pause()
    {
        if (currentGameState != gameState_Paused)
        {
            
            SwitchToState(gameState_Paused);
        }
    }

    // UI Button calls this to resume the game when paused()
    public void Resume()
    {
        if (currentGameState == gameState_Paused)
        {
            SwitchToState(gameState_GamePlay);
        }
    }

    public void GameOver()      { SwitchToState(gameState_GameOver); }
    public void OpenCredits()   { SwitchToState(gameState_Credits); }
    public void OpenOptions()   { SwitchToState(gameState_Options); }

    public void OpenMainMenu()  { SwitchToState(gameState_MainMenu); }
    public void GoBack()        { SwitchToState(lastGameState);    }

    public void Gameplay()      { SwitchToState(gameState_GamePlay); }

    public void ResetGameStats() { SwitchToState(gameState_ResetGameStats); }



}
