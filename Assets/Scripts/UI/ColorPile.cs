using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ColorPile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(3f,0.5f).SetLoops(1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuart);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f,0.5f).SetLoops(1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuart);
    }
}
