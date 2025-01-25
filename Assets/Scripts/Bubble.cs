using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private bool isInBurstZone = false;

    [SerializeField] private BubbleBurstZone bubbleBurstZone; // Reference to the BubbleBurstZone script

    void Start()
    {
        // Find the BubbleBurstZone script in the scene
        bubbleBurstZone = FindObjectOfType<BubbleBurstZone>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bubble to the left
        this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    void OnMouseDown()
    {
        // Notify the BubbleBurstZone directly about the burst
        if (bubbleBurstZone != null)
        {
            bubbleBurstZone.CalculateScore(transform.position, isInBurstZone);
        }

        // Destroy the bubble immediately after being clicked
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BurstZone")) // Check if the bubble enters the BurstZone trigger zone
        {
            isInBurstZone = true;         
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


