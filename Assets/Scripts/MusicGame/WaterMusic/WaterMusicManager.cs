using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterMusicManager : MonoBehaviour
{
    [SerializeField]
    private WaterMusicData m_waterMusicData;  //水滴玩法配置数据

    public float m_fTouchAgainTime;           //玩家再次touch时间
    public float m_fTouchSuccessTime;         //检测玩家点击成功的有效范围
    public float m_fTouchCheckTime;           //玩家的点击影响物体的检测时间范围

    //private float m_fOneSectitonTime;         //一小段音乐的时间
    private int m_iCheckPointID = 0;          //目前所处的节奏点序号
    private int m_iSongPointCount;            //玩家节奏点总个数

    private bool m_bIsTouch = true;           // 本次触摸是否有效

    private Timer m_oneSectionTimer;          // 一段音乐的定时器
    private Timer m_touchTimer;               // 每次触摸的定时器

    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public AudioClip[] m_clickAudios;         // 音效列表

    public int m_iMaxStar = 3;                // 最多可获得星星数
    private int m_iFailTimes;                 // 失败次数

    //private bool m_bIsAskResult = true;       // 是否调用过结算函数

    private int m_iNowSectionID;              // 玩法中，当前小节ID
    private int m_iSectionCount;              // 玩法中，音乐小节总数
    private bool m_bIsTeachStage;              // 是否在教学阶段

    //private int m_iNpcCount = 5;                  // NPC个数
    private Text textPoint;                   // 展示文本

    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
        // 当前小节ID
        m_iNowSectionID = 0;

        // 该玩法所有的小节总数
        m_iSectionCount = m_waterMusicData.GetWaterMusicCount();

        // 初始化小节主定时器
        m_oneSectionTimer = new Timer(GetNowSectionAudioClip().length);
        m_oneSectionTimer.m_tick += OneSectionEnd;

        // 触摸定时器
        m_touchTimer = new Timer(m_fTouchAgainTime);
        m_touchTimer.m_tick += TouchEnd;

        // 失败次数
        m_iFailTimes = 0;

        // 初始第一次为教学阶段
        m_bIsTeachStage = true;

        m_clickAudioSource = GetComponent<AudioSource>();

        // 初始化小节信息
        ReInitSection();

        textPoint = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_oneSectionTimer.Update(Time.deltaTime);

        // 当前小节节奏点已经全部使用，等待小节定时器结束
        if (m_iCheckPointID >= m_iSongPointCount)
        {
            return;
        }

        // 触摸CD时间内，更新触摸定时器
        if (!m_bIsTouch)
        {
            m_touchTimer.Update(Time.deltaTime);
        }

        // 检查当前节超时；教学阶段，可以检查节点是否需要增加NPC操作
        CheckCurTouchTime(m_oneSectionTimer.m_curTime);

        // 玩家点击,教学阶段不检测玩家点击
        if (!m_bIsTeachStage && Input.GetMouseButtonDown(0))
        {
            if (m_bIsTouch)
            {
                // 当前点击有效
                m_touchTimer.Restart(); // 点击定时器重0开始计时
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
        return m_waterMusicData.GetOneMusicData(m_iNowSectionID).audioClip;
    }

    /// <summary>
    /// 获取当前音乐片段节奏点序列
    /// </summary>
    /// <returns></returns>
    private List<float> GetNowSectionPointTimeList()
    {
        return m_waterMusicData.GetOneMusicData(m_iNowSectionID).m_songTimePoint;
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
        float checkPointTime = GetNowSectionPointTimeList()[m_iCheckPointID];

        if (Mathf.Abs(checkPointTime - curTime) < m_fTouchSuccessTime)
        {
            if (m_bIsTeachStage)
            {
                textPoint.text = "教学阶段";
            }
            else
            {
                textPoint.text = "玩家阶段";
            }
            textPoint.text += m_iCheckPointID.ToString();
        }

        if (curTime > checkPointTime + m_fTouchCheckTime)
        {
            Debug.Log("节点超时");

            CheckPointChange();
            m_iFailTimes++;
        }

        // 教学阶段，检查节奏点是否需要操作
        if (m_bIsTeachStage && Mathf.Abs(checkPointTime - curTime) < m_fTouchSuccessTime)
        {
            Debug.Log("NPC教学");
            m_iCheckPointID++;
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

            // 播放成功音效
            PlayClickAudio(0);
        }
        else if ((checkPointTime - curTime) > m_fTouchSuccessTime && (checkPointTime - curTime) < m_fTouchCheckTime)
        {
            Debug.Log("超前点击");

            CheckPointChange();

            // 播放超前点击音效
            PlayClickAudio(1);

            m_iFailTimes++;
        }
        else if ((curTime - checkPointTime) > m_fTouchSuccessTime && (curTime - checkPointTime) < m_fTouchCheckTime)
        {
            Debug.Log("延迟点击");

            CheckPointChange();

            // 播放超前点击音效
            PlayClickAudio(1);

            m_iFailTimes++;
        }
        else
        {
            Debug.Log("无效点击");

            // 播放无效点击音效
            PlayClickAudio(-1);
        }
    }

    /// <summary>
    /// 当前节点改变，需要做的操作
    /// </summary>
    void CheckPointChange()
    {
        m_iCheckPointID++;
    }

    /// <summary>
    /// 当前小节音乐结束
    /// </summary>
    void OneSectionEnd()
    {
        // 当前是否为教学阶段
        if (m_bIsTeachStage)
        {
            m_bIsTeachStage = false;
        }
        else
        {
            m_bIsTeachStage = true;
            m_iNowSectionID++;
        }
        
        if (m_iNowSectionID >= m_iSectionCount)
        {
            Debug.Log("游戏结束");
            enabled = false;
            return;
        }
        ReInitSection();
    }

    /// <summary>
    /// 重新初始化场景信息
    /// </summary>
    void ReInitSection()
    {
        // 一小节音乐的定时器
        m_oneSectionTimer.ResetTirggerTime(6f);
        m_oneSectionTimer.Restart();

        // 可以触摸
        m_bIsTouch = true;

        // 触摸定时器
        m_touchTimer.Restart();

        // 当前音乐，目前所处的节奏点序号
        m_iCheckPointID = 0;

        // 当前小节音乐，节奏点个数
        m_iSongPointCount = GetNowSectionPointTimeList().Count;

        // 游戏运行
        enabled = true;

        // 播放背景音乐
        AudioManager.Instance.PlayMusicSingle(GetNowSectionAudioClip());
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    /// <param name="iClickState">点击状态 0成功 -1无效 1失败 </param>
    void PlayClickAudio(int iClickState)
    {
        if (m_clickAudioSource)
        {
            m_clickAudioSource.Stop();

            if (iClickState == 0)
            {
                m_clickAudioSource.clip = m_clickAudios[0];
            }
            else if (iClickState == 1)
            {
                m_clickAudioSource.clip = m_clickFailAudio;
            }
            else if(iClickState == -1)
            {
                m_clickAudioSource.clip = m_clickInvalidAudio;
            }
            m_clickAudioSource.Play();

        }
    }
}
