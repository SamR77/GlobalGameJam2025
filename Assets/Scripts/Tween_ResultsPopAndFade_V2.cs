using UnityEngine;
using DG.Tweening;

public class Tween_ResultsPopAndFade_V2 : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private Vector3 originalScale;
    [SerializeField] private Vector3 scaleTo;

    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        originalScale = transform.localScale;
        scaleTo = originalScale * 2.0f;

        Vector3 StartingSize = transform.localScale;

        // Declare the sequence variable first
        Sequence textSequence = null;

        // Assign the sequence
        textSequence = DOTween.Sequence() 
                            
                .Append(transform.DOScale(transform.localScale * 0.1f, 0))

                .Append(transform.DOScale(scaleTo, originalScale, 0.1f).SetEase(Ease.OutSine))
                .Join(canvasGroup.DOFade(1f, 0.005f))  // Fade in

                .AppendInterval(1.5f)  // Hold for 1.5 seconds
                .Append(canvasGroup.DOFade(0f, 4.0f)).SetEase(Ease.InSine)  // Fade out
                .Join(transform.DOScale(0f, 4.0f).SetEase(Ease.InSine))  // Pop-in effect

            // Ensure proper cleanup
            .OnComplete(() =>
            {
                textSequence.Kill();  // Kill the sequence
                Destroy(gameObject);  // Destroy the GameObject
            });


        
    }
}
