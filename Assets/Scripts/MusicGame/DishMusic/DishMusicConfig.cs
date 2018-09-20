using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DishMusicConfig : ScriptableObject
{
    [System.Serializable]
    public class ClickData
    {
        public float m_clickTime;
        public int m_clickstyle;
    }

    [System.Serializable]
    public class AudioData
    {
        public int m_clickstyle;
        public AudioClip audioClip;
    }

    public AudioClip m_audioClipBgm;          //背景音乐
    public float m_fTouchAgainTime;           //玩家再次touch时间

    public float m_fBeginClickTime;           //玩家开始点击的时间
    public float m_fEndClickTime;             //玩家结束点击的时间
    [Tooltip("0表示大木材 1小木材，2表示露珠，3表示篮球,4鸡叫 5草丛 6罐子")]
    public List<AudioData> m_audioDatas = new List<AudioData>();
    public List<ClickData> clickDatas = new List<ClickData>();

    /// <summary>
    /// 获取背景音效
    /// </summary>
    /// <returns></returns>
    public AudioClip GetAudioClipBgm()
    {
        return m_audioClipBgm;
    }

    /// <summary>
    /// 获取再次touch时间范围
    /// </summary>
    /// <returns></returns>
    public float GetTouchAgainTime()
    {
        return m_fTouchAgainTime;
    }

    public float GetBeginClickTime()
    {
        return m_fBeginClickTime;
    }

    public float GetEndClickTime()
    {
        return m_fEndClickTime;
    }
}
