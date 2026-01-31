using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 oriColor, curColor;
    private Color cor;
    public Color R, G, B, RG, RB, GB, RGB, None;

    private void Awake()
    {
        cor = GetComponent<Color>();
    }

    private void Update()
    {
        changeCor();
    }

    void changeCor()
    {
        if (curColor == new Vector3(1, 0, 0))
        {
            cor=R;
        }
        else if (curColor == new Vector3(0, 1, 0))
        {
            cor=G;
        }
        else if (curColor == new Vector3(0, 0, 1))
        {
            cor=B;
        }
        else if (curColor == new Vector3(1,1,0))
        {
            cor=RG;
        }
        else if (curColor == new Vector3(1,0,1))
        {
            cor=RB;
        }
        else if (curColor == new Vector3(0, 1, 1))
        {
            cor=GB;
        }
        else if (curColor == new Vector3(1,1,1))
        {
            cor=RGB;
        }
        else if (curColor == new Vector3(0, 0, 0))
        {
            cor=None;
        }
    }
}
