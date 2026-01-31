using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Image colorChoicePanel;
    public ColorChoicePanel choicePanel; // 添加对ColorChoicePanel的引用
    private bool endScale = false;    
    
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
            }
            if(Time.timeScale>0) Time.timeScale-= Time.deltaTime*5;
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

            endScale = true;
            SimpleCinemachineShake2023.Instance.TriggerShake();
        }

        if (endScale && Time.timeScale < 1)
        {
            Time.timeScale += Time.deltaTime * 5;
        }
        else
        {
            endScale = false;
            Time.timeScale = 1;
        }
    }
    
}