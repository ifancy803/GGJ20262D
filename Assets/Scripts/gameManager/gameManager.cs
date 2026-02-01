using System;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public bool isdead;

    private void Awake()
    {
        isdead = false;
    }

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if ((isdead || Input.GetKeyDown(KeyCode.R)))
        {
            timer = 0;
            Debug.Log("Reset");
            Reset();
        }
    }

    private void Reset()
    {
        colorController.Instance.Reset();
        playerController.Instance.Reset();
        isdead = false;
    }
}
