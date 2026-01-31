using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image colorChoicePanel;

    [Header("外部变量")] public Vector3 隐藏颜色;
    
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
