using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] public bool isInBurstZone = false;

    [SerializeField] private BubbleGameplayManager bubbleGameplayManager; // Reference to the BubbleBurstZone script

    void Start()
    {
        // Find the BubbleBurstZone script in the scene
        bubbleGameplayManager = FindObjectOfType<BubbleGameplayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bubble to the left
        this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BurstZone")) // Check if the bubble enters the BurstZone trigger zone
        {
            isInBurstZone = true;
        }
        else if (other.CompareTag("Clearer")) // When it collides with the Clearer
        {
            // Notify the Gameplay Manager to handle the "miss"
            bubbleGameplayManager.HandleBubbleClear(this);

            // Destroy the bubble object
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BurstZone")) // Check if the bubble enters the BurstZone trigger zone
        {
            isInBurstZone = false;
        }
    }



}


