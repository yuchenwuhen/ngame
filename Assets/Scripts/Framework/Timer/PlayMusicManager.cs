using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MusicSong
{
    one = 0,
}

public class PlayMusicManager : MonoBehaviour 
{
    public AudioClip[] m_audioClip;          //背景音乐信息
    public MusicSong m_musicSong;            //选择的音乐序号

    public float m_touchAgainTime;           //玩家再次touch时间
    public float m_touchSuccessTIme;         //检测玩家点击成功的有效范围
    public float m_touchCheckTIme;           //玩家的点击影响物体的检测时间范围

    private SongData m_songData;            //玩家数据

    private float m_totalTime;              //背景音乐长度，整个游戏的长度

    private int m_checkPointID = 0;         //目前所处的节奏点序号
    private int m_songPointCount;           //玩家节奏点总个数

    private bool isTouch = true;            // 本次触摸是否有效

    private Timer m_Timer;                  // 游戏主时间定时器
    private Timer m_touchTimer;             // 每次触摸的定时器

    private List<NPCBehaviour> m_NPCs = new List<NPCBehaviour>(); //NPC列表
    private int m_PointID = 0; //记录过节点ID索引
    private Dictionary<int, int> m_PointID2NPCID = new Dictionary<int, int>(); //节点2NPCID

    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip[] m_clickAudios;         // 音效列表

    private Animator m_animatorCutWood;       // 劈柴动画
    private Button m_btnReset;                // 重置按钮

    private GameObject m_woodsuccess;         // 木块劈开画面
    private float m_woodSuccessTime = 0.2f;   // 木块劈开画面持续时间

    public int m_iMaxStar;
    private int m_iFailTimes;
    // Use this for initialization
    void Start () 
    {
        // 节奏点数据
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

        // NPC列表
        m_NPCs.AddRange(this.GetComponentsInChildren<NPCBehaviour>());

        Debug.Log("NPC Count:" + m_NPCs.Count);
        m_PointID = 0; // 已绑定木桩的节点序列ID
        int i = 0;
        for (; i < m_NPCs.Count; i++)
        {
            float checkPointTime = m_songData.GetPlayerSongList()[m_PointID];
            int iPointStype = m_songData.GetPlayerSongStyleList()[m_PointID];
            m_NPCs[i].BeginMove(checkPointTime - m_Timer.m_curTime);
            m_PointID2NPCID.Add(m_PointID, i);
            m_PointID++;
        }

        // 获取音效播放源
        m_clickAudioSource = GetComponent<AudioSource>();

        // 人物劈柴动画
        m_animatorCutWood = GameObject.Find("CutWood").GetComponent<Animator>();

        // 重置按钮
        m_btnReset = transform.Find("Reset").GetComponent<Button>();
        m_btnReset.onClick.AddListener(ResetClick);

        // 木块被劈开的画面
        m_woodsuccess = GameObject.Find("Woodsuccess");
        m_woodsuccess.SetActive(false);
        Debug.Log("EndBegin");
    }
    private bool isset = true;
	// Update is called once per frame
	void Update () 
    {
        // 更新主时间定时器
        m_Timer.Update(Time.deltaTime);
        //Debug.Log("cur time:" + m_Timer.m_curTime);

        // 更新NPC行为
        int i = 0;
        for (; i < m_NPCs.Count; i++)
        {
            m_NPCs[i].Move(Time.deltaTime);
        }

        // 检查节点是否全部结束
        //m_songPointCount
        if (m_checkPointID >= m_songPointCount && isset)
        {
            isset = false;
            Invoke("GameEnd", 2f);
            return;
        }

        // 触摸CD时间内，更新触摸定时器
        if (!isTouch)
        {
            m_touchTimer.Update(Time.deltaTime);
        }

        //检查当前节点是否超时未被点击。如果当前节点超时未点击，则当前节点失败，移到下一个节点
        CheckCurTouchTime(m_Timer.m_curTime);

        // 玩家点击
        if(Input.GetMouseButtonDown(0))
        {
            if(isTouch)
            {
                // 当前点击有效
                m_touchTimer.Restart(); // 点击定时器重0开始计时
                m_animatorCutWood.Play("Guoqi", -1, 0f);
                m_animatorCutWood.Update(0f);
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
    /// <param name="curTime"></param>
    private void CheckPlayerInput(float curTime)
    {
        float checkPointTime = m_songData.GetPlayerSongList()[m_checkPointID];
        if(Mathf.Abs(checkPointTime - curTime) < m_touchSuccessTIme)
        {
            Debug.Log("检测成功");

            CheckPointChange();

            PlayClickAudio(0);

            m_woodsuccess.SetActive(true);
            Invoke("PlayWoodEffect", m_woodSuccessTime);
        }
        else if ((checkPointTime - curTime) > m_touchSuccessTIme && (checkPointTime - curTime) < m_touchCheckTIme)
        {
            Debug.Log("超前点击");

            CheckPointChange();

            PlayClickAudio(1);

            m_iFailTimes++;
        }
        else if ((curTime - checkPointTime) > m_touchSuccessTIme && (curTime - checkPointTime) < m_touchCheckTIme)
        {
            Debug.Log("延迟点击");

            CheckPointChange();

            PlayClickAudio(1);
            m_iFailTimes++;
        }
        else
        {
            Debug.Log("无效点击");
            PlayClickAudio(2);
        }
    }

    private void PlayWoodEffect()
    {
        m_woodsuccess.SetActive(false);
    }

    /// <summary>
    ///  检测节点是否超时未被点。如果超时为被点击，则当前节点失败，跳到下一个节点
    /// </summary>
    /// <param name="curTime"></param>
    private void CheckCurTouchTime(float curTime)
    {
        //Debug.Log("curtime:" + curTime);
        if(curTime > (m_songData.GetPlayerSongList()[m_checkPointID]+ m_touchCheckTIme))
        {
            Debug.Log("节点超时,CheckPoint:" + m_checkPointID);
            Debug.Log("节点超时,curTime:" + m_Timer.m_curTime);
            CheckPointChange();

            PlayClickAudio(1);
            m_iFailTimes++;
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

    /// <summary>
    /// 检查的节点改变
    /// </summary>
    void CheckPointChange()
    {
        int iNPCID = m_PointID2NPCID[m_checkPointID]; // 当前节点使用的NPCID

        // 如果所有的节点ID都被注册木块,则不处理
        if (m_PointID >= m_songData.GetPlayerSongList().Count)
        {
            m_NPCs[iNPCID].EndMove();
        }
        else
        {
            Debug.Log("m_NPCs[iNPCID].GetPos():" + m_NPCs[iNPCID].GetPos());
            float curTime2 = m_songData.GetPlayerSongList()[m_PointID]; // 获取需要注册的木块
            m_NPCs[iNPCID].BeginMove(curTime2 - m_Timer.m_curTime);
            m_PointID2NPCID.Add(m_PointID, iNPCID); // 绑定节点ID和NPCID
            m_PointID++; // 节点ID递增
        }
        
        m_checkPointID++;
    }

    private void FixedUpdate()
    {
        //Debug.Log(Time.deltaTime);
    }

    /// <summary>
    /// 重置音乐场景
    /// </summary>
    public void ResetClick()
    {
        //SceneManager.LoadScene("Main");
        UIManager.instance.ShowUIFade(UIState.Musicmenu);

        // 节奏点数据
        //m_songData = this.GetComponent<SongData>();

        // 当前节奏点ID，整段音乐的节奏点个数
        m_checkPointID = 0;
        //m_songPointCount = m_songData.GetPlayerSongList().Count;

        // 主时间定时器
        //m_totalTime = m_audioClip[(int)(m_musicSong)].length;
        //m_Timer = new Timer(m_totalTime);
        //m_Timer.m_tick += PlayEnd;
        m_Timer.Restart();

        // 播放音乐
        AudioManager.Instance.PlayMusicSingle(m_audioClip[(int)m_musicSong]);

        // 触摸定时器
        //m_touchTimer = new Timer(m_touchAgainTime);
        //m_touchTimer.m_tick += TouchEnd;
        m_touchTimer.Restart();

        // NPC列表
        //m_NPCs.AddRange(this.GetComponentsInChildren<NPCBehaviour>());

        Debug.Log("NPC Count:" + m_NPCs.Count);
        m_PointID = 0; // 已绑定木桩的节点序列ID
        m_PointID2NPCID.Clear();
        int i = 0;
        for (; i < m_NPCs.Count; i++)
        {
            float checkPointTime = m_songData.GetPlayerSongList()[m_PointID];
            m_NPCs[i].BeginMove(checkPointTime - m_Timer.m_curTime);
            m_PointID2NPCID.Add(m_PointID, i);
            m_PointID++;
        }

        // 获取音效播放源
        //m_clickAudioSource = GetComponent<AudioSource>();

        // 人物劈柴动画
        //m_animatorCutWood = GameObject.Find("CutWood").GetComponent<Animator>();

        // 重置按钮
        //m_btnReset = transform.Find("Reset").GetComponent<Button>();
        //m_btnReset.onClick.AddListener(ResetClick);

        // 木块被劈开的画面
        //m_woodsuccess = GameObject.Find("Woodsuccess");
        m_woodsuccess.SetActive(false);

        isTouch = true;            // 本次触摸是否有效

        m_iFailTimes = 0;
        Debug.Log("EndResetClick");
    }

    void PlayClickAudio(int iClickState)
    {
        if (iClickState < 0 || iClickState >= m_clickAudios.Length)
        {
            Debug.LogError("PlayClickAudio, iClickState is illegal");
            return;
        }
        if (m_clickAudioSource)
        {
            m_clickAudioSource.clip = m_clickAudios[iClickState];
            m_clickAudioSource.Stop();
            m_clickAudioSource.Play();
        }
    }

    /// <summary>
    /// 游戏结束,计算游戏结果
    /// </summary>
    void GameEnd()
    {
        if (m_iMaxStar - m_iFailTimes >= 0)
        {
            UIManager.instance.CalculationCurMusicResult(m_iMaxStar - m_iFailTimes);
        }
        else
        {
            UIManager.instance.CalculationCurMusicResult(0);
        }
    }
}
