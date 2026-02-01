using System;
using UnityEngine;

public class BGMPlayerController : MonoBehaviour
{
    void Awake()
    {
        if(FindObjectsOfType<BGMPlayerController>().Length > 1)
            {
            Destroy(gameObject);
            return;
            }
        DontDestroyOnLoad(gameObject);
    }
}
