using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public List<UiLayer> Layers = new List<UiLayer>();
    public MySlider VolumeSlider;
    private float prevSliderVal = -100;
    public static Action OnVolumeSliderUp;
    public float UiSwitchDelay = 0.25f;

    public float UiSwitchDelayGameplay = 0.001f;
    public float UiSwitchDelayGameover = 5f;

    private void OnEnable()
    {
        GameStateManager.OnGameStateChange += ArmUiSwitch;
        VolumeSlider.MySliderUp += SliderUp;

    }
    private void OnDisable()
    {
        GameStateManager.OnGameStateChange -= ArmUiSwitch;
        VolumeSlider.MySliderUp -= SliderUp;
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            prevSliderVal = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            prevSliderVal = 1;
        }
        VolumeSlider.value = prevSliderVal;
        
    }

    void Update()
    {
        ChangeVolume();
    }
    private void SliderUp()
    {
        OnVolumeSliderUp?.Invoke();
    }
    private void ChangeVolume()
    {
        if (prevSliderVal != VolumeSlider.value)
        {
            AudioManager.instance.SetVolume(VolumeSlider.value);
        }
        prevSliderVal = VolumeSlider.value;
    }
    private void ArmUiSwitch(GameStateManager.GameState state)
    {
        StartCoroutine(SwitchUiRoutine(state));
    }

    IEnumerator SwitchUiRoutine(GameStateManager.GameState state)
    {
        float waitTime = UiSwitchDelay;
        if (state == GameStateManager.GameState.Gameplay)
        {
            waitTime = UiSwitchDelayGameplay;
        }
        if (state == GameStateManager.GameState.Gameover) 
        {
            waitTime = UiSwitchDelayGameover;
        }
        yield return new WaitForSeconds(waitTime);
        SwitchUi(state);
    }
    private void SwitchUi(GameStateManager.GameState state)
    {
        foreach (UiLayer l in Layers)
        {
            if (l.Layer == state)
            {
                l.gameObject.SetActive(true);
            }
            else
            {
                l.gameObject.SetActive(false);
            }
        }
    }
}
