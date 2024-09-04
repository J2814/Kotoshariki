using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCoverKitty : MonoBehaviour, IGameStateResonder
{
    public float OutOfFrameYPos;
    public float InFrameYPos;
    public float OutOfFrameXPos;
    public float InFrameXPos;

    public bool MeowOnVolumeChange = false;

    public GameStateManager.GameState[] InFrameStates;
    public GameStateManager.GameState[] OutOfFrameStates;

    private void OnEnable()
    {
        if (MeowOnVolumeChange)
        {
            UiManager.OnVolumeSliderUp += Meow;
        }
        GameStateManager.OnGameStateChange += RespondToGameState;
        
    }

    private void OnDisable()
    {
        if (MeowOnVolumeChange)
        {
            UiManager.OnVolumeSliderUp -= Meow;
        }
        GameStateManager.OnGameStateChange -= RespondToGameState;
    }
    void Start()
    {
        DOTween.Init();
    }

    public void RespondToGameState(GameStateManager.GameState gameState)
    {

        foreach (GameStateManager.GameState state in InFrameStates)
        {
            if (state == gameState)
            {
                GetInFrame();
                break;
            }
        }

        foreach (GameStateManager.GameState state in OutOfFrameStates)
        {
            if (state == gameState)
            {
                GetOutOfFrame();
                break;
            }
        }
    }

    public void Meow()
    {
        GetComponentInChildren<FaceAnimator>().PlayAngryAnim();
        Sound sound = AudioManager.instance.RandomSoundFromList(AudioManager.instance.SoundBank.Angry);
        AudioManager.instance.PlaySound(sound);
    }

    private void GetInFrame()
    {
        transform.DOLocalMoveY(InFrameYPos, 0.75f);
        transform.DOLocalMoveX(InFrameXPos, 0.75f);
    }

    private void GetOutOfFrame()
    {
        transform.DOLocalMoveY(OutOfFrameYPos, 0.75f);
        transform.DOLocalMoveX(OutOfFrameXPos, 0.75f);
    }
}
