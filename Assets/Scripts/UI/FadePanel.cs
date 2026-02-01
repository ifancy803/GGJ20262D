using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelController : Singleton<FadePanelController>
{
    public Image fadePanel;
    
    public float fadeDuration;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            FadeIn();
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        // 确保面板可见
        fadePanel.gameObject.SetActive(true);

        // 设置初始透明度（完全不透明）
        Color color = fadePanel.color;
        color.a = 1f;
        fadePanel.color = color;

        float elapsedTime = 0f;

        // 从1到0渐变
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
    }
    
    IEnumerator FadeInCoroutine()
    {
        // 确保面板可见
        fadePanel.gameObject.SetActive(true);
        
        // 设置初始透明度（完全透明）
        Color color = fadePanel.color;
        color.a = 0f;
        fadePanel.color = color;
        
        float elapsedTime = 0f;
        
        // 从0到1渐变
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }
        
        // 确保完全不透明
        color.a = 1f;
        fadePanel.color = color;
        //fadePanel.gameObject.SetActive(false);
    }
}