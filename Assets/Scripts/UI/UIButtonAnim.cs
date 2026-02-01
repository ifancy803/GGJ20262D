using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // 引入DOTween

public class UIButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Transform targetTransform;
    private Vector3 originalScale;

    // 参数配置
    public float hoverScale = 1.1f; // 悬停缩放倍数
    public float animDuration = 0.2f; // 动画时间

    void Awake()
    {
        targetTransform = transform;
        originalScale = targetTransform.localScale;
    }

    // 鼠标进入：变大
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetTransform.DOScale(originalScale * hoverScale, animDuration).SetEase(Ease.OutBack);
    }

    // 鼠标移出：恢复
    public void OnPointerExit(PointerEventData eventData)
    {
        targetTransform.DOScale(originalScale, animDuration).SetEase(Ease.OutQuad);
    }

    // 鼠标按下：微缩（按压感）
    public void OnPointerDown(PointerEventData eventData)
    {
        targetTransform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.OutQuad);
    }

    // 鼠标松开：回弹
    public void OnPointerUp(PointerEventData eventData)
    {
        targetTransform.DOScale(originalScale * hoverScale, 0.1f).SetEase(Ease.OutBack);
    }
}