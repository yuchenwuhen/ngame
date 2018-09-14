using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicGameConfig : ScriptableObject
{
    [System.Serializable]
    public class OneSectionData
    {
        public AudioClip audioClipBgm;
        public List<float> m_songTimePoint = new List<float>();
        public List<int> m_songStyle = new List<int>();
    }

    public List<OneSectionData> m_musicGameData = new List<OneSectionData>();

    /// <summary>
    /// 获取某小节的配置数据
    /// </summary>
    /// <returns>The one music data.</returns>
    /// <param name="iIndex">I index.</param>
    public OneSectionData GetOneSectionData(int iIndex)
    {
        return iIndex > m_musicGameData.Count ? null : m_musicGameData[iIndex];
    }

    /// <summary>
    /// 获取小节总数量
    /// </summary>
    /// <returns>The section count.</returns>
    public int GetSectionCount()
    {
        return m_musicGameData.Count;
    }
}
