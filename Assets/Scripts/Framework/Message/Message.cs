using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Message : IMessage
{
    protected object m_Data = null;
    public object Data
    {
        get { return m_Data; }
        set { m_Data = value; }
    }

    protected float m_Delay = 0;
    public float Delay
    {
        get { return m_Delay; }
        set { m_Delay = value; }
    }

    protected bool m_IsSent = false;
    public bool IsSent
    {
        get { return m_IsSent; }
        set { m_IsSent = value; }
    }

    protected string m_Type = String.Empty;
    public string AreaCode
    {
        get { return m_Type; }
        set { m_Type = value; }
    }

    protected string m_eventCode = String.Empty;
    public string EventCode
    {
        get { return m_eventCode; }
        set { m_eventCode = value; }
    }

    protected int m_priority = 0;
    public int Priority
    {
        get { return m_priority; }
        set { m_priority = value; }
    }

    protected string m_Filter = String.Empty;
    public string Filter
    {
        get { return m_Filter; }
        set { m_Filter = value; }
    }


    public void Reset()
    {
        m_Type = String.Empty;
        m_Data = null;
        m_IsSent = false;
        m_Filter = string.Empty;
        m_Delay = 0;
    }
}
