using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenuAttribute]
public class Sound : ScriptableObject
{
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
}
