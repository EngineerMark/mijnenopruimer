using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Misc")]
    public AudioMixerGroup masterAudioGroup;
    public float masterVolume;

    [Header("Sound Effects")]
    public AudioClip flagPlacementSFX;
    [Header("Music")]

    [Header("Audio Sources")]
    public AudioSource audioSourceSFX;
    public AudioSource audioSourceMusic;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public void Play(AudioClip clip){
        audioSourceSFX.PlayOneShot(clip);
    }
}
