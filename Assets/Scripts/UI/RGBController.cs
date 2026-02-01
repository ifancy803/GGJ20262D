using Febucci.UI;
using UnityEngine;
using TMPro;

public class RGBController : MonoBehaviour
{
    public TMP_Text r;
    public TMP_Text g;
    public TMP_Text b;

    private Color r_originalColor = Color.red;
    private Color g_originalColor = Color.green;
    private Color b_originalColor = Color.blue;
    
    private Color lastMaskColor = Color.clear;
    
    private void Start()
    {
        if (r != null)
        {
            r.color = r_originalColor;
        }
        if (g != null)
        {
            g.color = g_originalColor;
        }
        if (b != null)
        {
            b.color = b_originalColor;
        }
    }

    private void Update()
    {
        if (UIManager.Instance == null)
            return;
        
        Color currentMaskColor = UIManager.Instance.maskColor;
        
        // 输出当前颜色值用于调试
        if (currentMaskColor != lastMaskColor)
        {
            // 检查是否匹配
            bool matchRed = ColorsAreEqual(currentMaskColor, r_originalColor);
            bool matchGreen = ColorsAreEqual(currentMaskColor, g_originalColor);
            bool matchBlue = ColorsAreEqual(currentMaskColor, b_originalColor);
        }
        
        // 如果颜色没有变化，跳过
        if (currentMaskColor == lastMaskColor) return;
        
        lastMaskColor = currentMaskColor;
        
        // 重置所有颜色
        ResetAllColors();
        
        // 设置选中的颜色为黑色
        if (ColorsAreEqual(currentMaskColor, r_originalColor))
        {
            if (r != null)
            {
                r.color = Color.black;
            }
        }
        else if (ColorsAreEqual(currentMaskColor, g_originalColor))
        {
            if (g != null)
            {
                g.color = Color.black;
            }
        }
        else if (ColorsAreEqual(currentMaskColor, b_originalColor))
        {
            if (b != null)
            {
                b.color = Color.black;
            }
        }
    }
    
    private void ResetAllColors()
    {
        if (r != null) r.color = r_originalColor;
        if (g != null) g.color = g_originalColor;
        if (b != null) b.color = b_originalColor;
    }
    
    // 更可靠的颜色比较方法
    private bool ColorsAreEqual(Color a, Color b)
    {
        // 转换为字符串比较，避免浮点精度问题
        string colorA = ColorUtility.ToHtmlStringRGB(a);
        string colorB = ColorUtility.ToHtmlStringRGB(b);
        
        return colorA == colorB;
    }
}