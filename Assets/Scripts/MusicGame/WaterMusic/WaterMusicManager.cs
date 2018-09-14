using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMusicManager : MonoBehaviour
{
    [SerializeField]
    private WaterMusicData m_waterMusicData; //歌曲数据

    public float m_fTouchAgainTime;           //玩家再次touch时间
    public float m_fTouchSuccessTime;         //检测玩家点击成功的有效范围
    public float m_fTouchCheckTime;           //玩家的点击影响物体的检测时间范围

    private float m_fOneSectitonTime;          //一小段音乐的时间
    private int m_iCheckPointID = 0;           //目前所处的节奏点序号
    private int m_iSongPointCount;             //玩家节奏点总个数

    private bool m_bIsTouch = true;            // 本次触摸是否有效

    private Timer m_oneSectionTimer;        // 一段音乐的定时器
    private Timer m_touchTimer;             // 每次触摸的定时器

    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public AudioClip[] m_clickAudios;         // 音效列表

    public int m_iMaxStar = 3;                // 最多可获得星星数
    private int m_iFailTimes;                 // 失败次数

    private bool m_bIsAskResult = true;       // 是否调用过结算函数
    private bool m_bGameStateRun = false;     // 场景是否正在运行

    private int m_iNowSection;                // 当前进行到第几段音乐
    private int m_iSectionCount;              // 音乐小节数

    private int m_iNpcCount = 5;                  // NPC个数
    void Awake()
    {

    }
    // Use this for initialization
    void Start ()
    {
        m_iNowSection = 0;
        m_iSectionCount = m_waterMusicData.GetWaterMusicCount();

        // 初始化一段时间定时器
        m_oneSectionTimer = new Timer(GetNowSectionAudioClip().length);
        m_oneSectionTimer.m_tick += OneSectionEnd;
        m_oneSectionTimer.Restart();

        m_bIsTouch = true;
        // 触摸定时器
        m_touchTimer = new Timer(m_fTouchAgainTime);
        m_touchTimer.m_tick += TouchEnd;

        // 初始化所有NPC
        GameObject waterNpc = GameObject.Find("WaterNPC");
        for (int i = 0; i < m_iNpcCount; i++)
        {
            GameObject tmp = Instantiate(waterNpc) as GameObject;
            tmp.transform.SetParent(transform);
        }

        //目前所处的节奏点序号
        m_iCheckPointID = 0;

        // 当前音乐片段的节点数
        m_iSongPointCount = GetNowSectionPointTimeList().Count;

        // 失败次数
        m_iFailTimes = 0;

        m_bGameStateRun = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_bGameStateRun)
        {
            return;
        }

        m_oneSectionTimer.Update(Time.deltaTime);

        if (m_iCheckPointID >= m_iSongPointCount)
        {
            return;
        }

        // 触摸CD时间内，更新触摸定时器
        if (!m_bIsTouch)
        {
            m_touchTimer.Update(Time.deltaTime);
        }


        // 检查当前节点是否超时
        CheckCurTouchTime(m_oneSectionTimer.m_curTime);

        // 玩家点击
        if (Input.GetMouseButtonDown(0))
        {
            if (m_bIsTouch)
            {
                // 当前点击有效
                m_touchTimer.Restart(); // 点击定时器重0开始计时
                //m_animatorCutWood.Play("Guoqi", -1, 0f);
                //m_animatorCutWood.Update(0f);
                CheckPlayerInput(m_oneSectionTimer.m_curTime); // 检测玩家有效点击情况
                m_bIsTouch = false; // 点击定时器结束之前，点击无效
            }
            else
            {
                // 当前点击无效
                Debug.Log("CD时间内");
            }
        }
    }

    /// <summary>
    /// 获取当前片段的音乐
    /// </summary>
    /// <returns></returns>
    private AudioClip GetNowSectionAudioClip()
    {
        return m_waterMusicData.GetOneMusicData(m_iNowSection).audioClip;
    }

    /// <summary>
    /// 当前音乐片段关键点
    /// </summary>
    /// <returns></returns>
    private List<float> GetNowSectionPointTimeList()
    {
        return m_waterMusicData.GetOneMusicData(m_iNowSection).m_songTimePoint;
    }

    /// <summary>
    /// 触摸限制时间结束
    /// </summary>
    void TouchEnd()
    {
        m_touchTimer.Stop();
        m_bIsTouch = true;
    }

    /// <summary>
    ///  检测节点是否超时未被点。如果超时为被点击，则当前节点失败，跳到下一个节点
    /// </summary>
    /// <param name="curTime"></param>
    private void CheckCurTouchTime(float curTime)
    {
        if (curTime > GetNowSectionPointTimeList()[m_iCheckPointID] + m_fTouchCheckTime)
        {
            Debug.Log("节点超时");

            CheckPointChange();
            m_iFailTimes++;
        }
    }

    /// <summary>
    /// 检测玩家的CD内点击情况
    /// </summary>
    /// <param name="curTime"></param>
    private void CheckPlayerInput(float curTime)
    {
        float checkPointTime = GetNowSectionPointTimeList()[m_iCheckPointID];
        if (Mathf.Abs(checkPointTime - curTime) < m_fTouchSuccessTime)
        {
            Debug.Log("检测成功");
            CheckPointChange();

            //// 播放成功音效
            //PlayClickSuccessAudio(iPointStyle);

            //m_woodsuccess.SetActive(true);
            //Invoke("PlayWoodEffect", m_woodSuccessTime);
        }
        else if ((checkPointTime - curTime) > m_fTouchSuccessTime && (checkPointTime - curTime) < m_fTouchCheckTime)
        {
            Debug.Log("超前点击");

            CheckPointChange();

            //PlayClickFailAudio();

            m_iFailTimes++;
        }
        else if ((curTime - checkPointTime) > m_fTouchSuccessTime && (curTime - checkPointTime) < m_fTouchCheckTime)
        {
            Debug.Log("延迟点击");

            CheckPointChange();

            //PlayClickFailAudio();
            m_iFailTimes++;
        }
        else
        {
            Debug.Log("无效点击");
            //PlayClickInvalidAudio();
        }
    }

    /// <summary>
    /// 当前节点改变，需要做的操作
    /// </summary>
    void CheckPointChange()
    {
        m_iCheckPointID++;
    }

    void OneSectionEnd()
    {
        m_iNowSection++;
        if (m_iNowSection >= m_iSectionCount)
        {
            Debug.Log("游戏结束");
            m_bGameStateRun = false;
        }

        m_iSectionCount = m_waterMusicData.GetWaterMusicCount();

        m_oneSectionTimer = new Timer(GetNowSectionAudioClip().length);
        m_oneSectionTimer.Restart();

        m_bIsTouch = true;

        // 触摸定时器
        m_touchTimer.Restart();

        //目前所处的节奏点序号
        m_iCheckPointID = 0;

        // 当前音乐片段的节点数
        m_iSongPointCount = GetNowSectionPointTimeList().Count;

        // 失败次数
        //m_iFailTimes = 0;

        m_bGameStateRun = true;
    }
}
