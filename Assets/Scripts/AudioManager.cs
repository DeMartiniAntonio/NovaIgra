using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Settings")][Space(10)]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fillSound;
    [SerializeField] private AudioClip popSound;


    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    public void PlayFillSound()
    {
        audioSource.PlayOneShot(fillSound);
    }

    public void PlayPopSound()
    {
        audioSource.PlayOneShot(popSound);
    }
}
