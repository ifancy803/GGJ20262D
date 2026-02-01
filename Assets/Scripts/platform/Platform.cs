using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 oriColor;
    public Vector3 curColor;
    private BoxCollider2D coll;
    private SpriteRenderer sr;
    
    [Header("Color Settings")]
    public Color R, G, B, RG, RB, GB, RGB, None;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        
        // 初始化颜色赋值
        InitColors();
        
        curColor = oriColor;
    }

    private void InitColors()
    {
        // 使用 TryParseHtmlString 将十六进制字符串转为 Color
        ColorUtility.TryParseHtmlString("#CF1B1B", out R);
        ColorUtility.TryParseHtmlString("#4DFF8D", out G);
        ColorUtility.TryParseHtmlString("#63C8FF", out B);
        ColorUtility.TryParseHtmlString("#FDFFB8", out RG);
        ColorUtility.TryParseHtmlString("#FF2DD1", out RB);
        ColorUtility.TryParseHtmlString("#8EF6E4", out GB);
        ColorUtility.TryParseHtmlString("#2C202D", out RGB);
        
        // None 通常设为白色或透明，这里设为白色
        None = Color.white; 
    }

    private void Update()
    {
        ChangeCor();
        
        // 逻辑处理：如果是 None，禁用碰撞体并设为透明
        bool isNone = (sr.color == None);
        coll.enabled = !isNone;
        
        // 修正：Unity 的 Color.a 取值范围是 0 到 1
        Color c = sr.color;
        c.a = isNone ? 0f : 1f; 
        sr.color = c;
    }

    void ChangeCor()
    {
        // 使用 Vector3 进行比较时，建议用 == 或更安全的 Vector3.Distance
        if (curColor == new Vector3(1, 0, 0)) sr.color = R;
        else if (curColor == new Vector3(0, 1, 0)) sr.color = G;
        else if (curColor == new Vector3(0, 0, 1)) sr.color = B;
        else if (curColor == new Vector3(1, 1, 0)) sr.color = RG;
        else if (curColor == new Vector3(1, 0, 1)) sr.color = RB;
        else if (curColor == new Vector3(0, 1, 1)) sr.color = GB;
        else if (curColor == new Vector3(1, 1, 1)) sr.color = RGB;
        else if (curColor == new Vector3(0, 0, 0)) sr.color = None;
    }
}