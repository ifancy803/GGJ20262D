using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChoicePanel : MonoBehaviour
{
    public ColorPile redSprite;
    public ColorPile blueSprite;
    public ColorPile greenSprite;
    
    private ColorPile currentHoveredPile;
    
    // 保持原来的方法名
    public void SelectClosestPileOnRelease()
    {
        Vector2 mousePos = GetMouseWorldPosition();
        if (mousePos == Vector2.negativeInfinity) return;
        
        ColorPile closestPile = null;
        float minDistance = float.MaxValue;
        
        // 检查红色
        float redDist = Vector2.Distance(mousePos, redSprite.transform.position);
        if (redDist < minDistance)
        {
            minDistance = redDist;
            closestPile = redSprite;
        }
        
        // 检查蓝色
        float blueDist = Vector2.Distance(mousePos, blueSprite.transform.position);
        if (blueDist < minDistance)
        {
            minDistance = blueDist;
            closestPile = blueSprite;
        }
        
        // 检查绿色
        float greenDist = Vector2.Distance(mousePos, greenSprite.transform.position);
        if (greenDist < minDistance)
        {
            minDistance = greenDist;
            closestPile = greenSprite;
        }
        
        // 如果找到了最近的pile，并且不是当前hover的，则更新状态
        if (closestPile != null)
        {
            if (currentHoveredPile != null && currentHoveredPile != closestPile)
            {
                currentHoveredPile.Unhover();
            }
            
            if (closestPile != currentHoveredPile)
            {
                closestPile.Hover();
                currentHoveredPile = closestPile;
            }
        }
        else if (currentHoveredPile != null)
        {
            // 如果没有找到任何pile，但当前有hover的，则取消hover
            currentHoveredPile.Unhover();
            currentHoveredPile = null;
        }
    }
    
    // 添加重置方法，用于隐藏面板时重置状态
    public void ResetHoverState()
    {
        if (currentHoveredPile != null)
        {
            currentHoveredPile.Unhover();
            currentHoveredPile = null;
        }
    }
    
    Vector2 GetMouseWorldPosition()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera cam = canvas != null && canvas.worldCamera != null ? 
                    canvas.worldCamera : Camera.main;
        
        if (cam == null) return Vector2.negativeInfinity;
        
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(cam.transform.position.z);
        return cam.ScreenToWorldPoint(screenPos);
    }
}