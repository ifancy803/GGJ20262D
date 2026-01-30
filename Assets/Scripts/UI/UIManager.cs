using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image colorChoicePanel;
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
