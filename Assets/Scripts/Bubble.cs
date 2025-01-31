using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float bubbleSpeed = 1f;
    [SerializeField] public bool isInBurstZone = false;

 

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move the bubble to the left
        this.transform.Translate(Vector3.left * bubbleSpeed * Time.deltaTime);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BurstZone")) // Check if the bubble enters the BurstZone trigger zone
        {
            isInBurstZone = true;
        }
        else if (other.CompareTag("Clearer")) // When it collides with the Clearer
        {
            // Notify the Gameplay Manager to apply the score for a miss
            GameManager.Instance.HandleBubbleClear(this);
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

    internal void SetSpeed(float spawnSpeed)
    {
        bubbleSpeed = spawnSpeed;
    }
}


