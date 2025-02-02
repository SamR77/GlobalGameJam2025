using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Static instance property to provide global access
    public static AudioManager Instance { get; private set; }

    [Header("Reference to AudioSource")] 
    public AudioSource audioSource;

    [Header("Bubble Pop Audio Clips")] 
    public AudioClip[] bubblePopSounds;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBubblePopAudio() 
    {
        int randomIndex = Random.Range(0, bubblePopSounds.Length);
        audioSource.pitch = Random.Range(0.75f, 1.25f);
        audioSource.clip = bubblePopSounds[randomIndex];
        audioSource.Play();
    }



 
}
