using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSongData : ScriptableObject {

    public List<float> m_songTimePoint = new List<float>();
    public List<int> m_songStyle = new List<int>();
}
