using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Static instance property to provide global access
    public static AudioManager Instance { get; private set; }

    [Header("Reference to AudioSource")] // TODO: move to an AudioManager script
    public AudioSource audioSource;

    [Header("Bubble Pop Audio Clips")] // TODO: move to an AudioManager script
    public AudioClip[] bubblePopSounds;

    public void PlayBubblePopAudio() // TODO: move this into a an audioManager script
    {
        int randomIndex = Random.Range(0, bubblePopSounds.Length);
        audioSource.pitch = Random.Range(0.75f, 1.25f);
        audioSource.clip = bubblePopSounds[randomIndex];
        audioSource.Play();
    }

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
