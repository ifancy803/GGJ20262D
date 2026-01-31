using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine.EventSystems;

public class ColorPile : MonoBehaviour
{
    [Header("颜色设置")]
    public Color maskColor;
    
    [Header("动画设置")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float animationDuration = 0.1f;
    [SerializeField] private Ease easeType = Ease.InOutElastic;
    
    private Vector3 originalScale;
    public MMFeedbacks 粒子Feedbacks;
    void Start()
    {
        // 记录原始缩放
        originalScale = transform.localScale;
    }

    public void Hover()
    {
        transform.DOScale(hoverScale,animationDuration).SetUpdate(true).SetEase(easeType);
        UIManager.Instance.maskColor =  maskColor;
        //粒子Feedbacks.PlayFeedbacks();
    }

    public void Unhover()
    {
        transform.DOScale(originalScale, animationDuration).SetUpdate(true).SetEase(easeType);
    }
    
}