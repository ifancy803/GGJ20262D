using UnityEngine;
using Unity.Cinemachine;

public class SimpleCinemachineShake2023 : MonoBehaviour
{
    [Header("目标相机")]
    public CinemachineCamera targetCamera;
    
    [Header("抖动设置")]
    [Range(0.1f, 5f)]
    public float shakeIntensity = 1f;
    [Range(0.1f, 2f)]
    public float shakeDuration = 0.5f;
    
    private CinemachineBasicMultiChannelPerlin noiseComponent;
    private Coroutine currentShake;
    
    void Start()
    {
        InitializeCamera();
    }
    
    void InitializeCamera()
    {
        // 自动查找相机
        if (targetCamera == null)
        {
            targetCamera = FindObjectOfType<CinemachineCamera>();
        }
        
        if (targetCamera != null)
        {
            // 获取或添加 Noise 组件
            noiseComponent = targetCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            if (noiseComponent == null)
            {
                noiseComponent = targetCamera.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();
            }
            
            // 确保初始状态
            ResetNoise();
        }
        else
        {
            Debug.LogError("请指定一个 CinemachineCamera");
        }
    }
    
    void ResetNoise()
    {
        if (noiseComponent != null)
        {
            noiseComponent.AmplitudeGain = 0f;
            noiseComponent.FrequencyGain = 0f;
        }
    }
    
    public void TriggerShake()
    {
        TriggerShake(shakeIntensity, shakeDuration);
    }
    
    public void TriggerShake(float intensity, float duration)
    {
        if (noiseComponent == null) return;
        
        // 停止现有的抖动
        if (currentShake != null)
        {
            StopCoroutine(currentShake);
        }
        
        currentShake = StartCoroutine(PerformShake(intensity, duration));
    }
    
    System.Collections.IEnumerator PerformShake(float intensity, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // 使用衰减曲线
            float decay = Mathf.Exp(-progress * 4f); // 指数衰减
            float currentIntensity = intensity * decay;
            
            // 应用抖动
            noiseComponent.AmplitudeGain = currentIntensity;
            noiseComponent.FrequencyGain = currentIntensity * 2f; // 频率随强度变化
            
            yield return null;
        }
        
        // 恢复
        ResetNoise();
        currentShake = null;
    }
    
    public void StopShake()
    {
        if (currentShake != null)
        {
            StopCoroutine(currentShake);
            currentShake = null;
        }
        ResetNoise();
    }
    
    void Update()
    {
        // 测试
        if (Input.GetKeyDown(KeyCode.S))
        {
            TriggerShake();
        }
    }
}