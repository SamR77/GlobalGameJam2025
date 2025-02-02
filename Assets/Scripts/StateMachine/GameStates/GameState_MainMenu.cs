using UnityEngine;

// Sam Robichaud 
// NSCC Truro 2025
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class GameState_MainMenu : IGameState
{
    

    public void EnterState(GameStateManager gameStateManager)
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        

        GameManager.Instance.uIManager.UIMainMenu();
        //GameManager.Instance.playerManager.player.SetActive(false);        

        GameManager.Instance.GameReset();
    }

    public void FixedUpdateState(GameStateManager gameStateManager) { }
    public void UpdateState(GameStateManager gameStateManager) { }
    public void LateUpdateState(GameStateManager gameStateManager) { }
    public void ExitState(GameStateManager gameStateManager) { }


}
