using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RecordConfig : ScriptableObject
{
    [System.Serializable]
    public class OneRecordData
    {
        public List<AudioClip> audioClips;
        public List<float> m_songTimePoint = new List<float>();
        public List<int> m_songStyle = new List<int>();
    }

    public List<OneRecordData> m_recordData = new List<OneRecordData>();

}
