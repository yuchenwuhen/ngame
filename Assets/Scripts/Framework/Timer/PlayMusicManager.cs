using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MusicSong
{
    one=0,
}

public class PlayMusicManager : MonoBehaviour 
{

    public AudioClip[] m_audioClip;          //背景音乐信息
    public MusicSong m_musicSong;            //选择的音乐序号

    public float m_touchAgainTime;           //玩家再次touch时间
    public float m_touchSuccessTIme;         //检测玩家点击成功的有效范围
    public float m_touchCheckTIme;           //玩家的点击影响物体的检测时间范围

    private List<NpcSongData> m_NPCSongData = new List<NpcSongData>();     //NPC数据
    private SongData m_songData;            //玩家数据

    private float m_totalTime;              //背景音乐长度，整个游戏的长度

    private int m_checkPointID = 0;         //目前所处的节奏点序号
    private int m_songPointCount;           //玩家节奏点总个数

    private bool isTouch = true;            // 本次触摸是否有效

    private Timer m_Timer;                  // 游戏主时间定时器
    private Timer m_touchTimer;             // 每次触摸的定时器

    private NPCBehaviour m_NPCBehaviour;    //NPC行为
    private List<NPCBehaviour> m_NPCs = new List<NPCBehaviour>();
    //private Transform m_NPC;

    // Use this for initialization
	void Start () 
    {
        //m_NPCSongData.AddRange(this.GetComponentsInChildren<NpcSongData>());
        m_songData = this.GetComponent<SongData>();

        // 当前节奏点ID，整段音乐的节奏点个数
        m_checkPointID = 0;
        m_songPointCount = m_songData.GetPlayerSongList().Count;

        // 主时间定时器
        m_totalTime = m_audioClip[(int)(m_musicSong)].length;
        m_Timer = new Timer(m_totalTime);
        m_Timer.m_tick += PlayEnd;
        m_Timer.Start();

        // 播放音乐
        AudioManager.Instance.PlayMusicSingle(m_audioClip[(int)m_musicSong]);

        // 触摸定时器
        m_touchTimer = new Timer(m_touchAgainTime);
        m_touchTimer.m_tick += TouchEnd;

        m_NPCBehaviour = this.GetComponentInChildren<NPCBehaviour>();
        m_NPCs.AddRange(this.GetComponentsInChildren<NPCBehaviour>());
        //m_NPC = transform.Find("NPC");
        //GameObject game = Instantiate(m_NPC.gameObject) as GameObject;
        //game.transform.SetParent(transform);

        //gameObject.SetActive(false);

        BeginNPCMove();
    }
	
	// Update is called once per frame
	void Update () 
    {
        // 更新主时间定时器
        m_Timer.Update(Time.deltaTime);

        // 检查节点是否全部结束
        if (m_checkPointID >= m_songPointCount)
        {
            return;
        }

        // 触摸CD时间内，更新触摸定时器
        if (!isTouch)
        {
            m_touchTimer.Update(Time.deltaTime);
        }

        // 检测每个NPC的动画
        //foreach (var song in m_NPCSongData)
        //{
        //    song.CheckStatusTime(m_Timer.m_curTime);
        //}

        //检查当前节点是否超时未被点击。如果当前节点超时未点击，则当前节点失败，移到下一个节点
        CheckCurTouchTime(m_Timer.m_curTime);

        // 玩家点击
        if(Input.GetMouseButtonDown(0))
        {
            if(isTouch)
            {
                // 当前点击有效
                m_touchTimer.Restart(); // 点击定时器重0开始计时
                CheckPlayerInput(m_Timer.m_curTime); // 检测玩家有效点击情况
                isTouch = false; // 点击定时器结束之前，点击无效
            }
            else
            {
                // 当前点击无效
                Debug.Log("CD时间内");
            }
        }
	}

    /// <summary>
    /// 检测玩家的CD内点击情况
    /// </summary>
    /// <param name="curtime"></param>
    private void CheckPlayerInput(float curtime)
    {
        float curTime1 = m_songData.GetPlayerSongList()[m_checkPointID];
        if(Mathf.Abs(curTime1 - curtime) < m_touchSuccessTIme)
        {
            Debug.Log("检测成功");
            m_checkPointID++;
            BeginNPCMove();
        }
        else if ((curTime1 - curtime) > m_touchSuccessTIme && (curTime1 - curtime) < m_touchCheckTIme)
        {
            Debug.Log("超前点击");
            m_checkPointID++;
            BeginNPCMove();
        }
        else if ((curtime - curTime1) > m_touchSuccessTIme && (curtime - curTime1) < m_touchCheckTIme)
        {
            Debug.Log("延迟点击");
            m_checkPointID++;
            BeginNPCMove();
        }
        else
        {
            Debug.Log("无效点击");
        }
    }

    /// <summary>
    ///  检测节点是否超时未被点。如果超时为被点击，则当前节点失败，跳到下一个节点
    /// </summary>
    /// <param name="curtime">Curtime.</param>
    private void CheckCurTouchTime(float curtime)
    {
        //Debug.Log("curtime:" + curtime);
        if(curtime > (m_songData.GetPlayerSongList()[m_checkPointID]+ m_touchCheckTIme))
        {
            //Debug.Log("curtime:" + curtime + ",m_songData.GetPlayerSongList()[m_checkPointID]:"
            //          + m_songData.GetPlayerSongList()[m_checkPointID]);
            Debug.Log("节点超时,CheckPoint:" + m_checkPointID);
            m_checkPointID++;
            BeginNPCMove();
        }
    }

    /// <summary>
    /// 音乐玩法开始
    /// </summary>
    public void StartMusicPlay()
    {

    }

    /// <summary>
    /// 结束音乐玩法
    /// </summary>
    public void StopMusicPlay()
    {
        //m_Timer.Stop();
        //m_Timer.m_tick -= PlayEnd;
    }

    /// <summary>
    /// 音乐播放完成
    /// </summary>
    void PlayEnd()
    {

    }

    /// <summary>
    /// 触摸限制时间结束
    /// </summary>
    void TouchEnd()
    {
        m_touchTimer.Stop();
        isTouch = true;
    }

    void BeginNPCMove()
    {
        float checkPonitTime = m_songData.GetPlayerSongList()[m_checkPointID];
        m_NPCBehaviour.BeginMove(checkPonitTime - m_Timer.m_curTime);
    }
}
