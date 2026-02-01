using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionSound : MonoBehaviour
{
    [Header("碰撞声效设置")]
    public AudioClip[] collisionSounds;
    public AudioClip[] triggerSounds;
    
    [Header("音量控制")]
    [Range(0f, 1f)] public float minVolume = 0.3f;
    [Range(0f, 1f)] public float maxVolume = 1f;
    [Range(0.9f, 1.1f)] public float minPitch = 0.9f;
    [Range(0.9f, 1.1f)] public float maxPitch = 1.1f;
    
    [Header("碰撞过滤")]
    public float minCollisionForce = 1f;
    public float maxCollisionForce = 10f;
    public bool useTagFilter = false;
    public string[] allowedTags = { "Player", "Enemy", "Object" };
    public LayerMask collisionLayers = -1; // 默认所有层
    
    [Header("触发设置")]
    public bool playOnCollisionEnter = true;
    public bool playOnCollisionExit = false;
    public bool playOnTriggerEnter = true;
    public bool playOnTriggerExit = false;
    
    private AudioSource audioSource;
    private Rigidbody rb;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        rb = GetComponent<Rigidbody>();
        
        // 设置AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D音效
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (!playOnCollisionEnter) return;
        
        // 检查碰撞力
        float collisionForce = collision.relativeVelocity.magnitude;
        if (collisionForce < minCollisionForce) return;
        
        // 检查层
        if (collisionLayers != (collisionLayers | (1 << collision.gameObject.layer)))
            return;
        
        // 检查标签
        if (useTagFilter && !IsTagAllowed(collision.gameObject.tag))
            return;
        
        PlayCollisionSound(collisionForce, collision.contacts[0].point);
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (!playOnCollisionExit) return;
        
        // 碰撞退出时的处理（可选）
        PlaySound(triggerSounds, transform.position);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!playOnTriggerEnter) return;
        
        // 检查层
        if (collisionLayers != (collisionLayers | (1 << other.gameObject.layer)))
            return;
        
        // 检查标签
        if (useTagFilter && !IsTagAllowed(other.gameObject.tag))
            return;
        
        PlaySound(triggerSounds, transform.position);
    }
    
    void OnTriggerExit(Collider other)
    {
        if (!playOnTriggerExit) return;
        
        PlaySound(triggerSounds, transform.position);
    }
    
    void PlayCollisionSound(float force, Vector3 position)
    {
        if (collisionSounds.Length == 0) return;
        
        // 根据碰撞力度计算音量和音调
        float forceNormalized = Mathf.Clamp01((force - minCollisionForce) / (maxCollisionForce - minCollisionForce));
        float volume = Mathf.Lerp(minVolume, maxVolume, forceNormalized);
        float pitch = Mathf.Lerp(minPitch, maxPitch, forceNormalized);
        
        // 随机选择音效
        AudioClip clip = collisionSounds[Random.Range(0, collisionSounds.Length)];
        
        // 播放音效
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.transform.position = position;
        audioSource.PlayOneShot(clip);
    }
    
    void PlaySound(AudioClip[] clips, Vector3 position)
    {
        if (clips.Length == 0) return;
        
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.volume = Random.Range(minVolume, maxVolume);
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.transform.position = position;
        audioSource.PlayOneShot(clip);
    }
    
    bool IsTagAllowed(string tag)
    {
        foreach (string allowedTag in allowedTags)
        {
            if (tag == allowedTag) return true;
        }
        return false;
    }
    
    // 手动播放碰撞声效
    public void PlayCollisionSoundManually(float forceMultiplier = 1f)
    {
        PlayCollisionSound(minCollisionForce * forceMultiplier, transform.position);
    }
    
    [ContextMenu("测试播放碰撞声效")]
    public void TestPlay()
    {
        PlayCollisionSoundManually(2f);
    }
}