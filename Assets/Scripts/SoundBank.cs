using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundBank : ScriptableObject
{
    public List<Sound> Angry;

    public List<Sound> Throw;

    public List<Sound> IdleExtra;

    public List<Sound> Bump;

    public Sound Start;

    public Sound Gameover;

    public Sound Pause;

    public Sound Resume;


}
