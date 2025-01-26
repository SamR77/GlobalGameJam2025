using UnityEngine;

// Sam Robichaud 
// NSCC Truro 2025
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class GameState_ResetGameStats : IGameState
{

    // It will be used to set up all default settings
    // I realize this could be done in the MainMenuState as returning to it could count as a game reset of sorts... but it seems cleaner to have it's own state for this


   



    public void EnterState(GameStateManager gameStateManager)
    {
        Cursor.visible = false;

        GameManager.Instance.bubbleGameplayManager.ResetGameStats();
        GameManager.Instance.bubbleGameplayManager.tears.SetActive(false);

        // Enable all UI Panels, activates them so any scripts can get references to them if needed, each other state will disable them as needed
        GameManager.Instance.uIManager.EnableAllUIPanels();

        // TODO: add Logic to Reset Game Settings like score to default

        // Switch to MainMenu state
        GameManager.Instance.gameStateManager.SwitchToState(new GameState_GamePlay());


    }

    public void FixedUpdateState(GameStateManager gameStateManager) { }
    public void UpdateState(GameStateManager gameStateManager) { }
    public void LateUpdateState(GameStateManager gameStateManager) { }


    public void ExitState(GameStateManager gameStateManager)
    {
        // Disable all UI Panels
        GameManager.Instance.uIManager.DisableAllUIPanels();
    }

}
