using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicGameConfig : ScriptableObject
{
    public enum SectionType
    {
        Common = 0,
        Teach = 1,
        SevenClick = 2,
    }

    [System.Serializable]
    public class OneSectionData
    {
        public int m_iSectionID;                                 // 小节ID
        public SectionType m_iSectionType;                       // 小节样式
        public List<float> m_songTimePoint = new List<float>();  // 节奏点时间
        public List<int> m_songStyle = new List<int>();          // 节奏点风格
        public List<int> m_songNoteType = new List<int>();       // 音符类型 0为正常音符 -1为空拍
        public int m_iClickIndex;                                // 当该小节为SevenClick时，该字段判断需要玩家点击的是哪一拍
    }

    public AudioClip m_audioClipBgm;          //背景音乐
    public float m_fTouchAgainTime;           //玩家再次touch时间
    public float m_fTouchSuccessTime;         //检测玩家点击成功的有效范围
    public float m_fTouchCheckTime;           //玩家的点击影响物体的检测时间范围
    public List<OneSectionData> m_musicGameData = new List<OneSectionData>(); //单小节数据

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

    /// <summary>
    /// 获取touch成功时间范围
    /// </summary>
    /// <returns></returns>
    public float GetTouchSuccessTime()
    {
        return m_fTouchSuccessTime;
    }

    /// <summary>
    /// 获取touch被检测时间范围
    /// </summary>
    /// <returns></returns>
    public float GetTouchCheckTime()
    {
        return m_fTouchCheckTime;
    }
    /// <summary>
    /// 获取某小节的配置数据
    /// </summary>
    /// <returns>The one music data.</returns>
    /// <param name="iSectionID">I index.</param>
    public OneSectionData GetOneSectionData(int iSectionID)
    {
        return iSectionID > m_musicGameData.Count ? null : m_musicGameData[iSectionID];
    }

    /// <summary>
    /// 获取小节总数量
    /// </summary>
    /// <returns>The section count.</returns>
    public int GetSectionCount()
    {
        return m_musicGameData.Count;
    }

    /// <summary>
    /// 获取指定小节的类型
    /// </summary>
    /// <param name="iSectionID"></param>
    /// <returns></returns>
    public int GetSectionType(int iSectionID)
    {
        return iSectionID > m_musicGameData.Count ? -1 : (int)m_musicGameData[iSectionID].m_iSectionType;
    }

    /// <summary>
    /// 获取指定小节的节奏点个数
    /// </summary>
    /// <param name="iSectionID"></param>
    /// <returns></returns>
    public int GetSectionPointCount(int iSectionID)
    {
        return m_musicGameData[iSectionID].m_songTimePoint.Count;
    }

    /// <summary>
    /// 获取某一个小节,某一节奏点时间
    /// </summary>
    /// <param name="iSectionID"></param>
    /// <param name="iPointID"></param>
    /// <returns></returns>
    public float GetSectionOnePointTime(int iSectionID, int iPointID)
    {
        return m_musicGameData[iSectionID].m_songTimePoint[iPointID] + 0.3f;
    }

    /// <summary>
    /// 获取某一小节，某一节奏点样式
    /// </summary>
    /// <param name="iSectionID"></param>
    /// <param name="iPointID"></param>
    /// <returns></returns>
    public int GetSectionOnePointStyle(int iSectionID, int iPointID)
    {
        if (iSectionID >= m_musicGameData.Count || iPointID >= m_musicGameData[iSectionID].m_songStyle.Count)
        {
            return 0;
        }
        return m_musicGameData[iSectionID].m_songStyle[iPointID];
    }

    /// <summary>
    /// 在SevenClick玩法中,玩家需要点击的节点序号ID
    /// </summary>
    /// <param name="iSectionID"></param>
    /// <returns></returns>
    public int GetSevenClickPlayerIndex(int iSectionID)
    {
        return m_musicGameData[iSectionID].m_iClickIndex;
    }

    /// <summary>
    /// 获取节奏点序列
    /// </summary>
    /// <param name="iSectionID"></param>
    /// <returns></returns>
    public List<float> GetPointTimeList(int iSectionID)
    {
        return m_musicGameData[iSectionID].m_songTimePoint;

    }

    public int GetSectionOnePointNoteType(int iSectionID, int iPointID)
    {
        if(iSectionID >= m_musicGameData.Count || iPointID >= m_musicGameData[iSectionID].m_songNoteType.Count)
        {
            return 0;
        }
        return m_musicGameData[iSectionID].m_songNoteType[iPointID];
    }
}
