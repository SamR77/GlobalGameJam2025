using UnityEngine;
using DG.Tweening;

public class ResultsTextPopAndFade : MonoBehaviour
{
    
    
    [Header("Scale Settings")]
    [SerializeField] private Vector3 originalScale;
    [SerializeField] private Vector3 scaleTo;
    [SerializeField] private float scaleSpeed = 0.25f;



    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 2.0f; // Time in seconds for the text to fade
    [SerializeField] private float startDelay = 0.5f; // Optional delay before fading starts

    public CanvasGroup canvasGroup;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        originalScale = transform.localScale;
        scaleTo = originalScale * 2.0f;

        //transform.DOScale(scaleTo, scaleDuration).SetEase(Ease.InOutSine);
        //transform.DOPunchScale(originalScale * 1.0f, scaleDuration, 1, 100f).SetLoops(1);


        var textSequence = DOTween.Sequence()
            .SetLoops(-1)
            .Append(transform.DOScale(originalScale * 1.5f, 0.1f).SetEase(Ease.OutBack))  // Pop-in effect
            .Join(canvasGroup.DOFade(1f, 0.1f))  // Fade in
            .AppendInterval(2f)  // Hold for 2 seconds
            .Append(canvasGroup.DOFade(0f, 2.0f))  // Fade out
            .Join(transform.DOScale(originalScale * 1.0f, 2.0f).SetEase(Ease.OutBack));  // Pop-in effect
            //.OnComplete(() => Destroy(gameObject)); // Optional: Disable the object


        /*
        var sequence = DOTween.Sequence()
            
            .SetLoops(-1)
            .Append(transform.DOScale(originalScale * 1.5f, 0.001f)).SetEase(Ease.InElastic)
            .AppendInterval(0.25f)
            .Append(transform.DOScale(originalScale, 0.25f)).SetEase(Ease.InOutSine)
            .Join(canvasGroup.DOFade(0, fadeDuration));


        */


    }


}
