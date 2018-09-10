using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

    private bool m_Ticking; // 当前状态

    public float m_curTime { get; private set; }// 当前时间

    private float m_triggerTime;// 触发时间

    public delegate void EventHandler();
    public event EventHandler m_tick;// 触发事件

    public Timer(float second)
    {
        m_curTime = 0f;
        m_triggerTime = second;
    }

    public void Start()
    {
        m_Ticking = true;
    }

    public void Update(float deltaTime)
    {
        if(m_Ticking)
        {
            m_curTime += deltaTime;

            if(m_curTime > m_triggerTime)
            {
                m_Ticking = false;
                m_tick();
            }
        }
    }

    public void Stop()
    {
        m_Ticking = false;
    }

    public void Restart()
    {
        m_Ticking = true;
        m_curTime = 0f;
    }

    public void ResetTirggerTime(float second)
    {
        m_triggerTime = second;
    }
}
