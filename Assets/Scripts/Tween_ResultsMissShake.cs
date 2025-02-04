using UnityEngine;
using DG.Tweening;

public class Tween_ResultsMissShake : MonoBehaviour
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



        var textSequence = DOTween.Sequence()
            .Append(transform.DOScale(0.75f, 0))
            .Append(transform.DOScale(1.0f, 0.005f).SetEase(Ease.InSine))  // Pop-in effect
            .Join(canvasGroup.DOFade(1f, 0.005f))  // Fade in




            .Append(
                DOTween.Sequence()
                .Append(transform.DOLocalRotate(new Vector3(0, 0, 15), 0.05f, RotateMode.LocalAxisAdd))
                .Append(transform.DOLocalRotate(new Vector3(0, 0, -15), 0.05f, RotateMode.LocalAxisAdd))
                .SetLoops(6, LoopType.Yoyo)
                )
            
            

            //.Append(transform.DOLocalRotate(new Vector3(0, 0, 30), 0.1f, RotateMode.LocalAxisAdd))
            //.Append(transform.DOLocalRotate(new Vector3(0, 0, -30), 0.1f, RotateMode.LocalAxisAdd))

            //.Append(transform.DOLocalRotate(new Vector3(0, 0, 30), 0.1f, RotateMode.LocalAxisAdd))
            //.Append(transform.DOLocalRotate(new Vector3(0, 0, -30), 0.1f, RotateMode.LocalAxisAdd))
            //.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f, RotateMode.LocalAxisAdd))


            .Append(canvasGroup.DOFade(0f, 4.0f)).SetEase(Ease.InSine)  // Fade out
            .Join(transform.DOScale(0f, 4.0f).SetEase(Ease.InSine))  // Pop-in effect


            ;
            
            
            
            //.OnComplete(() => Destroy(gameObject)); 




    }


}
