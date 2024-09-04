using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEnablerManager : MonoBehaviour, IGameStateResonder
{
    public List<GameObject> ObjectsToDisableOnPause;
    public List<GameObject> ObjectsToDisableOnGameover;
    public List<GameObject> ObjectsToEnableOnGameover;
    private void OnEnable()
    {
        GameStateManager.OnGameStateChange += RespondToGameState;
    }
    private void OnDisable()
    {
        GameStateManager.OnGameStateChange -= RespondToGameState;
    }
    public void RespondToGameState(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Menu)
        {
            StartCoroutine(DelayBeforePauseRoutine());
        }
        if (gameState == GameStateManager.GameState.TitleScreen)
        {
            Pause(true);
        }
        if (gameState == GameStateManager.GameState.Gameplay) 
        {
            Pause(false);
            Gameover(false);
        }
        if (gameState == GameStateManager.GameState.Gameover)
        {
            Gameover(true);
        }
    }
    private void Pause(bool pause)
    {
        foreach (GameObject obj in ObjectsToDisableOnPause)
        {
            obj.SetActive(!pause);
        }
    }

    private void Gameover(bool gameover)
    {
        foreach (GameObject obj in ObjectsToDisableOnGameover)
        {
            obj.SetActive(!gameover);
        }
        foreach (GameObject obj in ObjectsToEnableOnGameover)
        {
            obj.SetActive(gameover);
        }
    }

    IEnumerator DelayBeforePauseRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        Pause(true);
    }
}
