using UnityEngine;
using DG.Tweening;

public class Tween_ResultsPopAndFade : MonoBehaviour
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



        var textSequence = DOTween.Sequence()
            .Append(transform.DOScale(0.75f, 0))

            .Append(transform.DOScale(1.0f, 0.005f).SetEase(Ease.InSine))  // Pop-in effect
            .Join(canvasGroup.DOFade(1f, 0.005f))  // Fade in

            .AppendInterval(1.5f)  // Hold for 1.5 seconds
            .Append(canvasGroup.DOFade(0f, 4.0f)).SetEase(Ease.InSine)  // Fade out
            .Join(transform.DOScale(0f, 4.0f).SetEase(Ease.InSine))  // Pop-in effect
            .OnComplete(() => Destroy(gameObject)); // Optional: Disable the object





    }


}
