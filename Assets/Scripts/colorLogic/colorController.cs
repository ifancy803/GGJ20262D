using System;
using UnityEngine;

public class colorController : Singleton<colorController>
{
    public Color receiveCor=Color.black;
    public Platform[] platforms;

    void Start()
    {
        receiveCor=Color.black;
        // 找到当前场景中所有 Platform
        platforms = FindObjectsByType<Platform>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if(UIManager.Instance!=null)
            receiveCor = UIManager.Instance.maskColor;
        if (receiveCor==Color.red)
        {
            foreach (var p in platforms)
            {
                var oriCor = p.oriColor;
                p.curColor = new Vector3(0, oriCor.y, oriCor.z);
            }

        }

        else if (receiveCor==Color.green)
        {
            foreach (var p in platforms)
            {
                var oriCor = p.oriColor;
                p.curColor = new Vector3(oriCor.x, oriCor.y, 0);
            }
        }

        else if (receiveCor==Color.blue)
        {
            foreach (var p in platforms)
            {
                var oriCor = p.oriColor;
                p.curColor = new Vector3(oriCor.x, 0, oriCor.z);
            }
        }
    }

    public void Reset()
    {
        if(UIManager.Instance!=null)
            UIManager.Instance.maskColor = Color.black;
        
        foreach (var p in platforms)
        {
            p.curColor = p.oriColor;
        }
    }
}
