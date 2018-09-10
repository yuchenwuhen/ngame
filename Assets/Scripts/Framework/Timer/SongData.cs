using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongData : MonoBehaviour {

    [SerializeField]
    private PlayerSongData playerSong;

    public List<float> GetPlayerSongList()
    {
        if (playerSong != null)
            return playerSong.m_songTimePoint;
        return null;
    }
}
