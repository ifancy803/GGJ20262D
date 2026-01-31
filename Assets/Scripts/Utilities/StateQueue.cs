using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// 单帧状态
/// </summary>
[Serializable]
public class FrameState
{
    public float timestamp;
    public bool person_detected;
    public bool left_hand_raised;
    public bool right_hand_raised;
    public bool hands_together;
    public bool left_foot_grounded;
    public bool right_foot_grounded;
    public Vector2? left_foot_grid_pos;  // (grid_x, grid_y)
    public Vector2? right_foot_grid_pos;  // (grid_x, grid_y)
    
    public FrameState()
    {
    }
    
    public FrameState(float timestamp, bool person_detected, bool left_hand_raised, 
        bool right_hand_raised, bool hands_together, bool left_foot_grounded, 
        bool right_foot_grounded, Vector2? left_foot_grid_pos, Vector2? right_foot_grid_pos)
    {
        this.timestamp = timestamp;
        this.person_detected = person_detected;
        this.left_hand_raised = left_hand_raised;
        this.right_hand_raised = right_hand_raised;
        this.hands_together = hands_together;
        this.left_foot_grounded = left_foot_grounded;
        this.right_foot_grounded = right_foot_grounded;
        this.left_foot_grid_pos = left_foot_grid_pos;
        this.right_foot_grid_pos = right_foot_grid_pos;
    }
}

/// <summary>
/// 状态队列，线程安全
/// </summary>
public class StateQueue
{
    private readonly ConcurrentQueue<FrameState> queue;
    private readonly int maxSize;
    private readonly object lockObject = new object();
    
    public StateQueue(int maxSize = 100)
    {
        this.maxSize = maxSize;
        this.queue = new ConcurrentQueue<FrameState>();
    }
    
    /// <summary>
    /// 添加状态
    /// </summary>
    public void Push(FrameState state)
    {
        lock (lockObject)
        {
            queue.Enqueue(state);
            
            // 如果超过最大大小，移除最旧的状态
            while (queue.Count > maxSize)
            {
                queue.TryDequeue(out _);
            }
        }
    }
    
    /// <summary>
    /// 获取最近N个状态
    /// </summary>
    public List<FrameState> GetRecent(int count)
    {
        lock (lockObject)
        {
            var states = new List<FrameState>(queue);
            if (states.Count == 0)
                return new List<FrameState>();
            
            int startIndex = Mathf.Max(0, states.Count - count);
            return states.GetRange(startIndex, states.Count - startIndex);
        }
    }
    
    /// <summary>
    /// 清空队列
    /// </summary>
    public void Clear()
    {
        lock (lockObject)
        {
            while (queue.TryDequeue(out _)) { }
        }
    }
    
    /// <summary>
    /// 获取队列大小
    /// </summary>
    public int Count
    {
        get
        {
            lock (lockObject)
            {
                return queue.Count;
            }
        }
    }
}
