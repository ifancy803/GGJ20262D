using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 oriColor;
    public Vector3 curColor;
    private SpriteRenderer sr;
    public Color R, G, B, RG, RB, GB, RGB, None;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        changeCor();
    }

    void changeCor()
    {
        if (curColor == new Vector3(1, 0, 0))
        {
            sr.color=R;
        }
        else if (curColor == new Vector3(0, 1, 0))
        {
            sr.color=G;
        }
        else if (curColor == new Vector3(0, 0, 1))
        {
            sr.color=B;
        }
        else if (curColor == new Vector3(1,1,0))
        {
            sr.color=RG;
        }
        else if (curColor == new Vector3(1,0,1))
        {
            sr.color=RB;
        }
        else if (curColor == new Vector3(0, 1, 1))
        {
            sr.color=GB;
        }
        else if (curColor == new Vector3(1,1,1))
        {
            sr.color=RGB;
        }
        else if (curColor == new Vector3(0, 0, 0))
        {
            sr.color=None;
        }
    }
}