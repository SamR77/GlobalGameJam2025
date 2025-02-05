using UnityEngine;
using DG.Tweening;

public class Tween_ResultsMissShake : MonoBehaviour
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
            .Join(canvasGroup.DOFade(0, 0))  // Set to invisible

            .Append(transform.DOScale(1.0f, 0.01f).SetEase(Ease.InSine))  // Pop-in effect
            .Join(canvasGroup.DOFade(1.0f, 0.01f))  // Fade in
            .Join(transform.DOShakePosition(0.5f, 0.075f, 100, 90, false, true).SetEase(Ease.InSine)) // Shake effect



            /*
            .Append(
                DOTween.Sequence()
                .Append(transform.DOLocalRotate(new Vector3(0, 0, 15), 0.05f, RotateMode.LocalAxisAdd))
                .Append(transform.DOLocalRotate(new Vector3(0, 0, -15), 0.05f, RotateMode.LocalAxisAdd))
                .SetLoops(6, LoopType.Yoyo)
                )

            */

            .AppendInterval(0.25f)  // Wait for 1 second


            .Append(canvasGroup.DOFade(0f, 4.0f)).SetEase(Ease.InSine)  // Fade out
            .Join(transform.DOScale(0f, 4.0f).SetEase(Ease.InSine))  // Pop-in effect


            ;
            
            
            
            //.OnComplete(() => Destroy(gameObject)); 




    }


}
