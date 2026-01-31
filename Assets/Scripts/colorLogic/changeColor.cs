using UnityEngine;

public class changeColor : MonoBehaviour
{
    public Color receiveCor;
    private Platform[] platforms;

    void Start()
    {
        // 找到当前场景中所有 Platform
        var objects = Object.FindObjectsByType<Platform>(FindObjectsSortMode.None);
    }

    void Update()
    {
        receiveCor = UIManager.Instance.maskColor;
        if (receiveCor==Color.red)
        {
            foreach (var p in platforms)
                changeCol(color.red, p);
        }

        else if (receiveCor==Color.green)
        {
            foreach (var p in platforms)
                changeCol(color.green, p);
        }

        else if (receiveCor==Color.blue)
        {
            foreach (var p in platforms)
                changeCol(color.blue, p);
        }
    }

    enum color
    {
        red,green,blue
    }

    void changeCol(color c, Platform p)
    {
        var oriCor = p.oriColor;
        if (c == color.red)
        {
            p.curColor = new Vector3(0, oriCor.y, oriCor.z);
        }
        else if (c == color.blue)
        {
            p.curColor = new Vector3(oriCor.x, oriCor.y, 0);
        }
        else if (c == color.green)
        {
            p.curColor = new Vector3(oriCor.x, 0, oriCor.z);
        }
    }
}
