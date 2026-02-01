using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ButtonSoundConfig
{
    public Button targetButton;
    public AudioClip clickSound;
    [Range(0f, 1f)] public float volume = 1f;
    public bool enableHoverSound = false;
    public AudioClip hoverSound;
    [Range(0f, 1f)] public float hoverVolume = 0.5f;
}
public class ButtonSoundManager : MonoBehaviour
{
    [Header("全局设置")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    public AudioSource audioSourcePrefab;
    
    [Header("按钮音效配置")]
    public List<ButtonSoundConfig> buttonSounds = new List<ButtonSoundConfig>();
    
    private Dictionary<Button, AudioSource> buttonAudioSources = new Dictionary<Button, AudioSource>();
    
    void Start()
    {
        InitializeButtonSounds();
        
    }
    
    void InitializeButtonSounds()
    {
        foreach (var config in buttonSounds)
        {
            if (config.targetButton == null) continue;
            
            // 创建或获取AudioSource
            AudioSource audioSource = null;
            
            if (audioSourcePrefab != null)
            {
                audioSource 
= Instantiate(audioSourcePrefab, config.targetButton.transform);
            }
            else
            {
                audioSource 
= config.targetButton.gameObject.AddComponent<AudioSource>();
            }
            
            audioSource
.playOnAwake = false;
            buttonAudioSources
[config.targetButton] = audioSource;
            
            // 添加点击事件
            config
.targetButton.onClick.AddListener(() => OnButtonClick(config));
            
            // 添加悬停事件（如果需要）
            if (config.enableHoverSound)
            {
                AddHoverEvents(config);
            }
        }
    }
    
    void OnButtonClick(ButtonSoundConfig config)
    {
        if (config.clickSound != null && buttonAudioSources.ContainsKey(config.targetButton))
        {
            AudioSource audioSource = buttonAudioSources[config.targetButton];
            audioSource
.volume = config.volume * masterVolume;
            audioSource
.PlayOneShot(config.clickSound);
        }
    }
    
    void AddHoverEvents(ButtonSoundConfig config)
    {
        // 添加鼠标悬停事件
        var hoverHandler = config.targetButton.gameObject.AddComponent<ButtonHoverSound>();
        hoverHandler
.Setup(config.hoverSound, config.hoverVolume * masterVolume);
    }
    
    // 动态添加按钮音效
    public void AddButtonSound(Button button, AudioClip clickSound, float volume = 1f)
    {
        if (button == null || clickSound == null) return;
        
        var config = new ButtonSoundConfig
        {
            targetButton 
= button,
            clickSound 
= clickSound,
            volume 
=
 volume
        };
        
        buttonSounds
.Add(config);
        
        // 如果已经初始化，立即生效
        if (buttonAudioSources.Count > 0)
        {
            AudioSource audioSource = button.gameObject.AddComponent<AudioSource>();
            audioSource
.playOnAwake = false;
            buttonAudioSources
[button] = audioSource;
            
            button
.onClick.AddListener(() => OnButtonClick(config));
        }
    }
    
    void OnDestroy()
    {
        // 清理所有事件监听
        foreach (var config in buttonSounds)
        {
            if (config.targetButton != null)
            {
                config
.targetButton.onClick.RemoveAllListeners();
            }
        }
    }
}

// 悬停音效组件
public class ButtonHoverSound : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip hoverSound;
    private float hoverVolume;
    
    public void Setup(AudioClip sound, float volume)
    {
        hoverSound 
= sound;
        hoverVolume 
= volume;
        
        audioSource 
= gameObject.AddComponent<AudioSource>();
        audioSource
.playOnAwake = false;
    }
    
    void OnMouseEnter()
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource
.volume = hoverVolume;
            audioSource
.PlayOneShot(hoverSound);
        }
    }
}
