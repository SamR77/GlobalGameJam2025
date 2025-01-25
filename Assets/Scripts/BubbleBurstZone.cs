using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BubbleBurstZone : MonoBehaviour
{
    public float perfectMinPercent = 90f; // "Perfect" (100%)
    public float goodMinPercent = 40f; // Radius for "Good" (50%)
    public float earlyLateMinPercent = 0f; // Radius for "Early/Late" (0%)

    private Vector3 burstZoneCenter; // The perfect burst point
    private float maxDistance; // The maximum distance for scoring

    public TextMeshProUGUI resultText; // Reference to the TextMeshPro component for displaying result

    private Collider triggerCollider; // The trigger zone collider

    int points = 0;

    private void Start()
    {
        // Get the collider that defines the trigger zone
        triggerCollider = GetComponent<Collider>();

        // Set the burstZoneCenter to the collider's center
        burstZoneCenter = triggerCollider.bounds.center;

        // Convert percentage ranges to distances based on perfectMinPercent
        maxDistance = perfectMinPercent / 100f; // Perfect range in world units
    }

    public void CalculateScore(Vector3 bubblePosition, bool isInBurstZone)
    {
        // Check if the bubble is inside the trigger zone
        if (!triggerCollider.bounds.Contains(bubblePosition))
        {
            //Debug.Log("Bubble missed (outside trigger zone)!");            
            UpdateResultText("Miss!");
            points = 0; // Miss
            return;
        }

        // Calculate only the X distance between the bubble and the center
        float distanceX = Mathf.Abs(burstZoneCenter.x - bubblePosition.x);

        // Calculate the percentage of distance relative to perfect range
        float percentage = Mathf.Clamp01(1 - (distanceX / maxDistance)) * 100;

        Debug.Log($"Distance: {distanceX}, Percentage: {percentage}%");

        // Determine the scoring based on the percentage thresholds
        if (percentage >= perfectMinPercent)
        {
            UpdateResultText("Perfect");
        }
        else if (percentage >= goodMinPercent)
        {
            UpdateResultText("Good!");
        }
        else if (percentage >= earlyLateMinPercent)
        {
            UpdateResultText("Good!");
        }
        else
        {
            UpdateResultText("Miss!");
        }

        // Add points to score here (e.g., GameManager.Instance.AddScore(points))
    }


    // Update the result text with the appropriate message
    private void UpdateResultText(string result)
    {
        Debug.Log("Updating result text1");
        if (resultText != null)
        {
            Debug.Log("Updating result text");
            resultText.text = result; // Update the TextMeshPro component with the result
            resultText.color = new Color(resultText.color.r, resultText.color.g, resultText.color.b, 1f); // Make sure it's fully opaque                                 
            StartCoroutine(FadeOutText());// Start fading out the result text
        }

    }



    // Coroutine to fade out the text gradually
    private IEnumerator FadeOutText()
    {        
        yield return new WaitForSeconds(0.5f); // Wait for 1 second before starting the fade-out

        float fadeDuration = 1f; // Duration to fade the text
        float startAlpha = resultText.alpha; // Store the current alpha
        float endAlpha = 0f; // The final alpha (fully transparent)

        float timeElapsed = 0f;

        // Fade the text over the specified duration
        while (timeElapsed < fadeDuration)
        {
            resultText.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the alpha is set to the final value after the loop
        resultText.alpha = endAlpha;
    }



}
