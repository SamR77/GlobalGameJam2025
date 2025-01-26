using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Use this for Unity's default Text component
using TMPro; // Uncomment this if you're using TextMeshPro

public class FadeAndDestroy : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 2.0f; // Time in seconds for the text to fade
    public float startDelay = 0.5f; // Optional delay before fading starts

   
    public TextMeshProUGUI TXT_Result; // For TextMeshPro support (optional)
    private bool isFading = false;

    void Awake()
    {
        // Attempt to get a TextMeshProUGUI component
        //TXT_Result = GetComponent<TextMeshProUGUI>();

        if (TXT_Result == null)
        {
            Debug.LogError("No Text or TextMeshProUGUI component found on this GameObject. Destroying script.");
            Destroy(this);
        }
    }

    void OnEnable()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        if (isFading) yield break;

        isFading = true;

        // Optional delay before fading starts
        if (startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }

        float elapsedTime = 0f;

        // Cache initial color
        Color initialColor = TXT_Result != null ? TXT_Result.color : TXT_Result.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            if (TXT_Result != null)
            {
                TXT_Result.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure alpha is fully zero
        if (TXT_Result != null)
        {
            TXT_Result.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        }
        // Destroy the GameObject after fading out
        Destroy(gameObject);
    }
}


