using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Image colorChoicePanel;
    public ColorChoicePanel choicePanel; // 添加对ColorChoicePanel的引用

    [Header("外部变量")] 
    public Color maskColor;
    
    public MMF_Player feedback;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 显示面板
            colorChoicePanel.gameObject.SetActive(true);
            // feedback.gameObject.SetActive(true);
            // feedback.PlayFeedbacks();
            
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            // 按住Tab时持续检测鼠标位置并更新hover状态
            if (choicePanel != null)
            {
                choicePanel.SelectClosestPileOnRelease();
                Time.timeScale = 0f;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            // 隐藏面板
            colorChoicePanel.gameObject.SetActive(false);
            
            // 重置hover状态
            if (choicePanel != null)
            {
                choicePanel.ResetHoverState();
            }
            Time.timeScale = 1;
            SimpleCinemachineShake2023.Instance.TriggerShake();
        }
    }
}