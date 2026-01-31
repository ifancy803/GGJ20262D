using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Image colorChoicePanel;

    [Header("外部变量")] public Color maskColor;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            colorChoicePanel.gameObject.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            colorChoicePanel.gameObject.SetActive(false);
        }
    }
}
