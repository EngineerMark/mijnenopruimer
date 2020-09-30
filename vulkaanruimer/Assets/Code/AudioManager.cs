﻿using System.Collections;
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
    public AudioClip musicClip;

    [Header("Audio Sources")]
    public AudioSource audioSourceSFX;
    public AudioSource audioSourceMusic;

    private void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject); 
    }

    private void Start()
    {
        audioSourceMusic.clip = musicClip;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();
    }

    public void Play(AudioClip clip){
        audioSourceSFX.PlayOneShot(clip);
    }
}
