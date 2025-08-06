using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip playerDetectedClip;
    [SerializeField] AudioClip clickSound;

    [SerializeField] AudioSource audioSource;

    private void OnEnable()
    {
        GameEvents.OnPlayerDetected += HandlePlayerDetected;
        GameEvents.OnGameStart += HandleGameStart;
    }

    private void HandleGameStart()
    {
     
    }

    private void HandlePlayerDetected(bool isDetected)
    {
        if(isDetected)
        {
            audioSource.clip = playerDetectedClip;
            audioSource.loop = true; 
            audioSource.Play();
        } else
        {
             audioSource.Stop();
        }


    }
}
