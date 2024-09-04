using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    //public static GameStateManager instance;

    public static Action<GameState> OnGameStateChange;
    

    public static Action<GameState> ChangeGameState;

    public static Action GameRestart;
    [Serializable]
    public enum GameState
    {
        TitleScreen = 0,
        Gameplay = 1,
        Menu = 2,
        Gameover = 3,
        Shopping = 4
    }
    [SerializeReference]
    public GameState CurrentGameState;

    private void OnEnable()
    {
        ChangeGameState += GameStateChange;
    }
    private void OnDisable()
    {
        ChangeGameState -= GameStateChange;
    }

    private void Awake()
    {
        
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        GameStateChange(GameState.TitleScreen);
    }
    public void Restart()
    {
        GameRestart?.Invoke();
        GameStateChange(GameState.Gameplay);
        //SceneManager.LoadScene(0);
    }
    public void GameStateChange(int newState)//
    {
        switch (newState) // use upcast, where 0 - first, 1 - second...
        {
            case ((int)GameState.TitleScreen):
                CurrentGameState = GameState.TitleScreen;
                break;
            case ((int)GameState.Gameplay):
                CurrentGameState = GameState.Gameplay;
                break;
            case ((int)GameState.Menu):
                CurrentGameState = GameState.Menu;
                break;
            case ((int)GameState.Gameover):
                CurrentGameState = GameState.Gameover;
                break;
            case ((int)GameState.Shopping):
                CurrentGameState = GameState.Shopping;
                break;
        }
        OnGameStateChange?.Invoke(CurrentGameState);
    }

    public void GameStateChange(GameState newState)//
    {
        switch (newState) // use upcast, where 0 - first, 1 - second...
        {
            case (GameState.TitleScreen):
                CurrentGameState = GameState.TitleScreen;
                break;
            case (GameState.Gameplay):
                CurrentGameState = GameState.Gameplay;
                break;
            case (GameState.Menu):
                CurrentGameState = GameState.Menu;
                break;
            case (GameState.Gameover):
                CurrentGameState = GameState.Gameover;
                break;
            case (GameState.Shopping):
                CurrentGameState = GameState.Shopping;
                break;
        }
        OnGameStateChange?.Invoke(CurrentGameState);
    }

}
