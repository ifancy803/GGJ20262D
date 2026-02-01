using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent;
    public List<string> messages = new List<string>();
    
    [Header("打字机设置")]
    public float charsPerSecond = 20f;  // 每秒字符数
    public float messageDelay = 3f;     // 消息间延迟
    public float fadeInTime = 0.5f;     // 淡入时间
    public float fadeOutTime = 0.5f;    // 淡出时间
    
    private Coroutine typewriterCoroutine;
    
    void Start()
    {
        if (textComponent != null)
        {
            textComponent.text = "";
            textComponent.alpha = 0f;
        }
        
        StartCoroutine(ShowMessages());
    }
    
    IEnumerator ShowMessages()
    {
        foreach (string message in messages)
        {
            // 淡入
            yield return StartCoroutine(FadeText(0f, 1f, fadeInTime));
            
            // 打字效果
            yield return StartCoroutine(TypeText(message));
            
            // 等待
            yield return new WaitForSeconds(messageDelay);
            
            // 淡出
            yield return StartCoroutine(FadeText(1f, 0f, fadeOutTime));
            
            // 清空文本
            textComponent.text = "";
        }
    }
    
    IEnumerator TypeText(string text)
    {
        textComponent.text = "";
        
        float delay = 1f / charsPerSecond;
        
        for (int i = 0; i <= text.Length; i++)
        {
            textComponent.text = text.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
    
    IEnumerator FadeText(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            textComponent.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            yield return null;
        }
        
        textComponent.alpha = toAlpha;
    }
    
    // 外部调用显示单个消息
    public void ShowMessage(string message, float displayTime = 3f)
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        
        typewriterCoroutine = StartCoroutine(ShowSingleMessage(message, displayTime));
    }
    
    IEnumerator ShowSingleMessage(string message, float displayTime)
    {
        // 淡入
        yield return StartCoroutine(FadeText(0f, 1f, fadeInTime));
        
        // 打字
        yield return StartCoroutine(TypeText(message));
        
        // 等待
        yield return new WaitForSeconds(displayTime);
        
        // 淡出
        yield return StartCoroutine(FadeText(1f, 0f, fadeOutTime));
        
        textComponent.text = "";
    }
}