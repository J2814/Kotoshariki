using UnityEngine.Audio;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer Mixer;

    public SoundBank SoundBank;

    private List<AudioSource> sourcePool;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        sourcePool = GetComponentsInChildren<AudioSource>().ToList<AudioSource>();

        
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            SetVolume(PlayerPrefs.GetFloat("MasterVolume"));
        }
        else
        {
            SetVolume(1);
        }
    }
    private void Update()
    {
        
    }
    int RandElement<T>(List<T> list)
    {
        if (list.Count > 0)
        {
            return Random.Range(0, list.Count - 1);
        }
        else
        {
            return -1;
        }
    }

    public Sound RandomSoundFromList(List<Sound> SoundList)
    {
        int index = RandElement<Sound>(SoundList);
        if (index < 0)
        {
            return null;
        }
        else
        {
            return SoundList[index];
        }
    }

    public void PlaySound(Sound sound)
    {
        if (sound == null)
        {
            return;
        }

        bool clipPlayed = false;
        foreach (AudioSource source in sourcePool)
        {
            if (!source.isPlaying) 
            {
                source.outputAudioMixerGroup = sound.mixerGroup;
                source.clip = sound.clip;
                source.Play();
                break;
            }
        }
        if (!clipPlayed)
        {
            AudioSource source= sourcePool[UnityEngine.Random.Range(0, sourcePool.Count - 1)];
            source.outputAudioMixerGroup = sound.mixerGroup;
            source.clip = sound.clip;
            source.Play();
        }
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        Mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public float GetVolume()
    {
        float volume = 0;
        Mixer.GetFloat("MasterVolume", out volume);
        volume = Mathf.Pow(10, volume);
        
        return volume;
    }
}