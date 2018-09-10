using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MusicSong
{
    one=0,
    two
}

public class PlayMusicManager : MonoBehaviour {

    public AudioClip[] m_audioClip;
    public MusicSong m_musicSong;            //选择音乐
    public float m_touchAgainTime;           //玩家再次touch时间
    public float m_touchRangeTime;          //检测玩家点击范围时间
    private List<NpcSongData> m_songDatas = new List<NpcSongData>();     //NPC数据
    private SongData m_songData;            //玩家数据
    private float m_totalTime;          //音乐长度
    private int m_checkPoint = 0;

    private bool isTouch = true;
    private Timer m_Timer;
    private Timer m_touchTimer;

    public Text m_noteTxt;
	// Use this for initialization
	void Start () {
        m_songDatas.AddRange(this.GetComponentsInChildren<NpcSongData>());
        m_songData = this.GetComponentInChildren<SongData>();
        m_totalTime = m_audioClip[(int)(m_musicSong)].length;
        m_Timer = new Timer(m_totalTime);
        StartMusicPlay();
        m_touchTimer = new Timer(m_touchAgainTime);
        m_touchTimer.m_tick += TouchEnd;
    }
	
	// Update is called once per frame
	void Update () {
        m_Timer.Update(Time.deltaTime);
        m_touchTimer.Update(Time.deltaTime);
        foreach(var song in m_songDatas)
        {
            song.CheckStatusTime(m_Timer.m_curTime);
        }
        CheckCurTouchTime(m_Timer.m_curTime);
        if(Input.GetMouseButtonDown(0))
        {
            if(isTouch)
            {
                m_touchTimer.Start();
                CheckPlayerInput(m_Timer.m_curTime);
            }
        }
	}

    /// <summary>
    /// 检测玩家输入
    /// </summary>
    /// <param name="curtime"></param>
    private void CheckPlayerInput(float curtime)
    {
        float curTime1 = m_songData.GetPlayerSongList()[m_checkPoint];
        if(Mathf.Abs(curTime1 - curtime)< m_touchRangeTime)
        {
            m_checkPoint++;
            Debug.Log("检测成功");
        }
    }

    private void CheckCurTouchTime(float curtime)
    {
        if(curtime> (m_songData.GetPlayerSongList()[m_checkPoint]+ m_touchRangeTime/2.0f))
        {
            m_checkPoint++;
            Debug.Log("检测失败");
        }
    }

    /// <summary>
    /// 音乐玩法开始
    /// </summary>
    public void StartMusicPlay()
    {
        m_Timer.m_tick += PlayEnd;
        m_Timer.Start();
        AudioManager.Instance.PlayMusicSingle(m_audioClip[(int)MusicSong.one]);
    }

    /// <summary>
    /// 结束音乐玩法
    /// </summary>
    public void StopMusicPlay()
    {
        m_Timer.Stop();
        m_Timer.m_tick -= PlayEnd;
    }

    /// <summary>
    /// 音乐播放完成
    /// </summary>
    void PlayEnd()
    {

    }

    void TouchEnd()
    {
        m_touchTimer.Stop();
        isTouch = true;
    }


}
