using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAnimator : MonoBehaviour
{
    public void StartButton(GameObject Caller) 
    {
        Sound sound = AudioManager.instance.SoundBank.Start;
        AudioManager.instance.PlaySound(sound);
    }

    public void PauseButton(GameObject Caller)
    {
        Sound sound = AudioManager.instance.SoundBank.Pause;
        AudioManager.instance.PlaySound(sound);
    }

    public void ResumeButton(GameObject Caller)
    {
        Sound sound = AudioManager.instance.SoundBank.Resume;
        AudioManager.instance.PlaySound(sound);
    }


}
