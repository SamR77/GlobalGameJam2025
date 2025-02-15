

using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using UnityEngine;

public static class DeTweenExtensions
{
    public static Tween DOScale(
        this Transform target, Vector3 midValue,
        Vector3 endValue, float duration)
    {
        Sequence s = DOTween.Sequence();
        TweenerCore<Vector3, Vector3, VectorOptions> pre_t = DOTween.To((DOGetter<Vector3>)(() => target.localScale),
            (DOSetter<Vector3>)(x => target.localScale = x), midValue, duration * 0.65f).SetEase(Ease.OutQuad);
        pre_t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object)target);
        TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To((DOGetter<Vector3>)(() => target.localScale),
            (DOSetter<Vector3>)(x => target.localScale = x), endValue, duration * 0.35f).SetEase(Ease.InQuad);
        t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object)target);
        s.Append(pre_t);
        s.Append(t);
        return s;
    }


}
