using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static bool isDead;

    private void Awake()
    {
        isDead = false;
    }

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if ((isDead || Input.GetKeyDown(KeyCode.R)))
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
        isDead = false;
    }
}
