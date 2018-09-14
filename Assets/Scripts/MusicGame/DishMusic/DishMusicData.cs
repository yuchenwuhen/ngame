using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DishMusicData : ScriptableObject
{
    [System.Serializable]
    public class OneMusicData
    {
        public AudioClip audioClip;
        public List<float> m_songTimePoint = new List<float>();
        public List<int> m_songStyle = new List<int>();
    }

    public List<OneMusicData> m_musicData = new List<OneMusicData>();

    public OneMusicData GetOneMusicData(int iIndex)
    {
        if (iIndex > m_musicData.Count)
        {
            return null;
        }
        return m_musicData[iIndex];
    }

    public int GetWaterMusicCount()
    {
        return m_musicData.Count;
    }
}
