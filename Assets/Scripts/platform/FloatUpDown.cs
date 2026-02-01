using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    [Header("浮动幅度")]
    public float amplitude = 0.2f;   // 上下浮动高度

    [Header("浮动速度")]
    public float speed = 1.5f;       // 浮动频率

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = startPos + Vector3.up * offsetY;
    }
}