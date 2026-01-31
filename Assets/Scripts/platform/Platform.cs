using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 oriColor;
    public Vector3 curColor;
    private BoxCollider2D coll;
    private SpriteRenderer sr;
    public Color R, G, B, RG, RB, GB, RGB, None;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        curColor = oriColor;
    }

    private void Update()
    {
        changeCor();
        coll.enabled = sr.color != None;
        
        var c = sr.color;
        c.a = c==None? 0:255;
        sr.color = c;
        
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