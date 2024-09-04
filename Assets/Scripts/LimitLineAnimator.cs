using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitLineAnimator : MyAnimator, IGameStateResonder
{
    public string CountDownAnim;
    public string IdleAnim;
    public string GameoverAnim;

    private bool gameover;

    private void OnEnable()
    {
        GameStateManager.GameRestart += Init;
        GameStateManager.OnGameStateChange += RespondToGameState;
        Limit.StartCountDown += SwitchToStartCountDownAnim;
        Limit.StopCountDown += SwitchToIdleAnim;
    }

    private void OnDisable()
    {
        GameStateManager.GameRestart -= Init;
        GameStateManager.OnGameStateChange -= RespondToGameState;
        Limit.StartCountDown -= SwitchToStartCountDownAnim;
        Limit.StopCountDown -= SwitchToIdleAnim;
    }
    private void Awake()
    {
        SetUpAnimator();
    }
    private void Start()
    {
        Init();
    }

    public void RespondToGameState(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Gameover)
        {
            Sound sound = AudioManager.instance.SoundBank.Gameover;
            AudioManager.instance.PlaySound(sound);
            gameover = true;
            SwitchToGameoverAnim();
        }
    }

    

    private void SwitchToStartCountDownAnim(float f)
    {
        if (!gameover)
        {
            ChangeAnimationState(CountDownAnim);
        }
    }

    private void SwitchToIdleAnim()
    {
        if (!gameover)
        {
            ChangeAnimationState(IdleAnim);
        }
    }

    private void SwitchToGameoverAnim()
    {
        ChangeAnimationState(GameoverAnim);
    }

    private void Init()
    {
        gameover = false;
        SwitchToIdleAnim();
    }
}
