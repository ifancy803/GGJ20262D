using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Image colorChoicePanel;
    public Image 时停Fadepanel;
    public ColorChoicePanel choicePanel;
    private bool endScale = false;    
    AudioSource audioSource;
    
    [Header("外部变量")] 
    public Color maskColor;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (时停Fadepanel == null) 时停Fadepanel = GameObject.FindGameObjectWithTag("时停Panel").transform.GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            colorChoicePanel.gameObject.SetActive(true);
            audioSource.Play();
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            if (choicePanel != null)
            {
                choicePanel.SelectClosestPileOnRelease();
            }
            
            // 使用 unscaledDeltaTime，不受时间缩放影响
            if(Time.timeScale > 0) 
                Time.timeScale -= Time.unscaledDeltaTime * 2f;
            
            // 确保不会小于0
            if(Time.timeScale < 0.05f) 
                Time.timeScale = 0.00f; // 留一点最小值，不要完全为0

            时停Fadepanel.DOFade(0.8f, 0.5f).SetUpdate(true).SetEase(Ease.Flash);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            colorChoicePanel.gameObject.SetActive(false);
            
            if (choicePanel != null)
            {
                choicePanel.ResetHoverState();
            }

            endScale = true;
            SimpleCinemachineShake2023.Instance.TriggerShake();
        }

        if (endScale && Time.timeScale < 1)
        {
            // 恢复时也使用 unscaledDeltaTime
            Time.timeScale += Time.unscaledDeltaTime * 2f;
            
            时停Fadepanel.DOFade(0f, 0.5f).SetUpdate(true).SetEase(Ease.Flash);
            
            if(Time.timeScale >= 1f)
            {
                Time.timeScale = 1f;
                endScale = false;
            }
        }
    }
}