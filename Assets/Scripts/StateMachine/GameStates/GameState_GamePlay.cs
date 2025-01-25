using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Sam Robichaud 
// NSCC Truro 2025
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class GameState_GamePlay : IGameState
{
    public void EnterState(GameStateManager gameStateManager)
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        GameManager.Instance.uIManager.UIGamePlay();
        //GameManager.Instance.playerManager.player.SetActive(true);


    }

    public void FixedUpdateState(GameStateManager gameStateManager) { }

    public void UpdateState(GameStateManager gameStateManager)
    {      

    }

    public void LateUpdateState(GameStateManager gameStateManager) { }

    public void ExitState(GameStateManager gameStateManager) { }
}
