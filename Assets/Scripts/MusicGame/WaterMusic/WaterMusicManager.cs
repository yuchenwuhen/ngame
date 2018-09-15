using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterMusicManager : MonoBehaviour
{
    [SerializeField]
    public MusicGameConfig m_musicGameConfig; //玩法通用配置数据

    // 临界时间模块 Begin////
    private float m_fTouchAgainTime;           //玩家再次touch时间
    private float m_fTouchSuccessTime;         //检测玩家点击成功的有效范围
    private float m_fTouchCheckTime;           //玩家的点击影响物体的检测时间范围
    // 临界时间模块 End////

    // 当前状态模块 Begin////
    private int m_iNowSectionID;              // 该玩法中，当前小节ID
    private int m_iSectionCount;              // 该玩法中，音乐小节总数
    private bool m_bIsPointEnd;               // 该玩法中,所有的节奏点是否全部走完

    private bool m_bIsTeachStage;             // 是否在教学阶段(雨滴关卡特有字段)
    private bool m_bIsSevenClickStage;        // 是否是七拍点击阶段(盘子管卡特有字段)
    private bool m_bIsNpcHasAction;           // NPC是否行动过(超时检测时，可以检测当前节点时间，让NPC做出一定动作)

    private int m_iNowPointID;                // 当前小节中，目前所处的节奏点序号
    private int m_iNowSectionPointCount;      // 当前小节中，节奏点总个数

    private bool m_bIsTouch = true;           // 本次触摸是否有效
    private float m_fLastTouchTime;           // 上次触摸的时间
    private float m_fInitTime;                // 初始化获取的音乐播放时间(改字段主要是为了兼容，start中获取到的初始播放音乐时间不为0)
    // 当前状态模块 Begin////

    // 点击音效模块 Begin/////
    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public AudioClip[] m_clickAudios;         // 音效列表
    // 点击音效模块 End/////

    // 结算相关模块 Begin////
    private int m_iMaxStar = 3;                // 最多可获得星星数(目前结算面板只支持配置3)
    private int m_iFailTimes;                 // 失败次数
    // 结算相关模块 End////

    // 不通用分类模块 Begin////
    private Text m_textPoint;                   // 展示文本
    // 不通用分类模块 End////

    // 动画模块 Begin////
    private Animator m_animatorLeaf;            // 叶子动画
    private Animator m_animatorHead;            // 头部动画
    private bool m_bIsHeadPlayAnimator;         // 本节奏点是否播放过头部动画,避免重复播放
    private Animator m_animatorWaterDrop1;      // 水滴动画1
    private Animator m_animatorWaterDrop2;      // 水滴动画2
    private Animator m_animatorWaterDrop3;      // 水滴动画3
    private Animator m_animatorWaterDrop4;      // 水滴动画4
    // 动画模块 End////
    void Awake()
    {
        // 初始化场景
        InitGame();
    }

    // Use this for initialization
    void Start()
    {
        // 播放音乐(放在Start中调用，保证开始游戏时才会放音乐)
        AudioManager.Instance.PlayMusicSingle(m_musicGameConfig.GetAudioClipBgm());
        m_fInitTime = AudioManager.Instance.GetMusicSourceTime();
        //Debug.Log("InitTime:" + m_fInitTime);
    }

    // Update is called once per frame
    void Update()
    {
        float fNowTime = AudioManager.Instance.GetMusicSourceTime();
        float fRunTime = fNowTime - m_fInitTime;//游戏运行的时间
        //Debug.Log("fRunTime:" + fRunTime);

        if (m_bIsPointEnd)
        {
            //Debug.Log("PointEnd");
            return;
        }
        // 检查当前节超时；教学阶段，可以检查节点是否需要增加NPC操作
        CheckCurPoint(fRunTime);

        // 如果当前不能触摸,检查触摸CD是否已过
        if (!m_bIsTouch && (fRunTime - m_fLastTouchTime) > m_fTouchAgainTime)
        {
            m_bIsTouch = true;
        }

        // 玩家点击,教学阶段不检测玩家点击
        if (!m_bIsTeachStage && Input.GetMouseButtonDown(0))
        {
            if (m_bIsTouch)
            {
                // 当前点击有效
                m_fLastTouchTime = fRunTime;
                CheckPlayerInput(fRunTime); // 检测玩家有效点击情况
                m_bIsTouch = false;  // CD时间内，点击无效
            }
            else
            {
                // 当前点击无效
                Debug.Log("CD时间内");
            }
        }
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    void InitGame()
    {
        // 临界时间模块 Begin////
        m_fTouchAgainTime = m_musicGameConfig.GetTouchAgainTime();           //玩家再次touch时间
        m_fTouchSuccessTime = m_musicGameConfig.GetTouchSuccessTime();       //检测玩家点击成功的有效范围
        m_fTouchCheckTime = m_musicGameConfig.GetTouchCheckTime();           //玩家的点击影响物体的检测时间范围
        // 临界时间模块 End////

        // 当前状态模块 Begin////
        m_iNowSectionID = 0;              // 该玩法中，当前小节ID
        m_iSectionCount = m_musicGameConfig.GetSectionCount();              // 该玩法中，音乐小节总数
        m_bIsPointEnd = false;
        m_bIsTeachStage = m_musicGameConfig.GetSectionType(m_iNowSectionID) == 1 ? true : false;             // 是否在教学阶段
        m_bIsSevenClickStage = m_musicGameConfig.GetSectionType(m_iNowSectionID) == 2 ? true : false;        // 是否在七次点击阶段
        m_bIsNpcHasAction = false;

        m_iNowPointID = 0;                // 当前小节中，目前所处的节奏点序号
        m_iNowSectionPointCount = m_musicGameConfig.GetSectionPointCount(m_iNowSectionID);      // 当前小节中，节奏点总个数
        m_bIsTouch = true;           // 本次触摸是否有效
        // 当前状态模块 Begin////

        m_clickAudioSource = GetComponent<AudioSource>();

        m_iFailTimes = 0;

        m_textPoint = GetComponentInChildren<Text>();
        if (m_bIsTeachStage)
        {
            m_textPoint.text = "教学阶段";
        }
        else
        {
            m_textPoint.text = "玩家阶段";
        }
        m_animatorLeaf = transform.Find("Leaf").GetComponent<Animator>();
        m_animatorHead = transform.Find("Head").GetComponent<Animator>();
        m_bIsHeadPlayAnimator = false;

        m_animatorWaterDrop1 = transform.Find("WaterDrop1").GetComponent<Animator>();      // 水滴动画1
        m_animatorWaterDrop2 = transform.Find("WaterDrop2").GetComponent<Animator>();      // 水滴动画2
        m_animatorWaterDrop3 = transform.Find("WaterDrop3").GetComponent<Animator>();      // 水滴动画3
        m_animatorWaterDrop4 = transform.Find("WaterDrop4").GetComponent<Animator>();      // 水滴动画4
    }

    /// <summary>
    /// 重新初始化场景信息
    /// </summary>
    void ReInitSection()
    {
        //初始化数据
        InitGame();
        // 播放音乐
        AudioManager.Instance.PlayMusicSingle(m_musicGameConfig.GetAudioClipBgm());
        m_fInitTime = AudioManager.Instance.GetMusicSourceTime();
    }

    /// <summary>
    /// 检查当前节点是否超时;检查当前节奏点是否到达，可以增加NPC处理
    /// </summary>
    /// <param name="fRunTime"></param>
    void CheckCurPoint(float fRunTime)
    {
        float checkPointTime = m_musicGameConfig.GetSectionOnePointTime(m_iNowSectionID, m_iNowPointID);
        //int iPointStyle = m_musicGameConfig.GetSectionOnePointStyle(m_iNowSectionID, m_iNowPointID);

        // Seven中，NPC行动，但是不更新当前节点ID，等到超时的时候再更新
        if (!m_bIsNpcHasAction && Mathf.Abs(checkPointTime - fRunTime) < m_fTouchSuccessTime)
        {
            Debug.Log("NPC行动");
            m_bIsNpcHasAction = true;
            if (m_bIsSevenClickStage && m_iNowPointID != m_musicGameConfig.GetSevenClickPlayerIndex(m_iNowSectionID))
            {
                Debug.Log("NPC打盘子");
            }
        }

        if (fRunTime > checkPointTime + m_fTouchCheckTime)
        {
            Debug.Log("节奏点超时");

            // 播放失败动画
            //PlayFailedAnimator();

            m_iNowPointID++;
            OnNowPointIDChange(); //当前节奏点改变之后一定要调用此函数
        }
    }

    /// <summary>
    /// 检测玩家的CD时间之外的有效点击
    /// </summary>
    /// <param name="fRunTime"></param>
    void CheckPlayerInput(float fRunTime)
    {
        float checkPointTime = m_musicGameConfig.GetSectionOnePointTime(m_iNowSectionID, m_iNowPointID);
        int iPointStyle = m_musicGameConfig.GetSectionOnePointStyle(m_iNowSectionID, m_iNowPointID);
        if (Mathf.Abs(checkPointTime - fRunTime) < m_fTouchSuccessTime)
        {
            Debug.Log("检测成功");
            // 播放成功音效
            PlayClickAudio(0, iPointStyle);

            // 播放成功动画
            PlaySuccessAnimator();

            m_iNowPointID++;
            OnNowPointIDChange(); //当前节奏点改变之后一定要调用此函数
        }
        else if ((checkPointTime - fRunTime) > m_fTouchSuccessTime && (checkPointTime - fRunTime) < m_fTouchCheckTime)
        {
            Debug.Log("超前点击");
            PlayClickAudio(1, iPointStyle);

            // 播放失败动画
            PlayFailedAnimator();

            m_iNowPointID++;
            OnNowPointIDChange(); //当前节奏点改变之后一定要调用此函数
            m_iFailTimes++;
        }
        else if ((fRunTime - checkPointTime) > m_fTouchSuccessTime && (fRunTime - checkPointTime) < m_fTouchCheckTime)
        {
            Debug.Log("延迟点击");
            PlayClickAudio(1, iPointStyle);

            // 播放失败动画
            PlayFailedAnimator();

            m_iNowPointID++;
            OnNowPointIDChange(); //当前节奏点改变之后一定要调用此函数
            m_iFailTimes++;
        }
        else
        {
            Debug.Log("无效点击");
            PlayClickAudio(-1, iPointStyle);

            // 播放失败动画
            PlayFailedAnimator();
        }
    }

    /// <summary>
    /// 当前节奏点改变之后一定要调用此函数【WARNING】
    /// </summary>
    void OnNowPointIDChange()
    {
        // 更新一些对每个节奏点生效的数据
        m_bIsNpcHasAction = false;
        m_bIsHeadPlayAnimator = false;
        if (m_iNowPointID >= m_musicGameConfig.GetSectionPointCount(m_iNowSectionID))
        {
            Debug.Log("改变小节");
            m_iNowSectionID++;
            if (m_iNowSectionID >= m_musicGameConfig.GetSectionCount())
            {
                Debug.Log("节奏点全部结束");
                m_bIsPointEnd = true;
                return;
            }
            m_iNowPointID = 0;
            // 更新一些在小节中生效的数据
            m_bIsTeachStage = m_musicGameConfig.GetSectionType(m_iNowSectionID) == 0 ? false : true;
            m_bIsSevenClickStage = m_musicGameConfig.GetSectionType(m_iNowSectionID) == 2 ? true : false;        // 是否在七次点击阶段
            if (m_bIsTeachStage)
            {
                m_textPoint.text = "教学阶段";
            }
            else
            {
                m_textPoint.text = "玩家阶段";
            }
        }
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    /// <param name="iClickState">点击状态 0成功 -1无效 1失败 </param>
    void PlayClickAudio(int iClickState, int iStyle)
    {
        if (m_clickAudioSource)
        {
            m_clickAudioSource.Stop();

            if (iClickState == 0)
            {
                m_clickAudioSource.clip = m_clickAudios[iStyle];
            }
            else if (iClickState == 1)
            {
                m_clickAudioSource.clip = m_clickFailAudio;
            }
            else if (iClickState == -1)
            {
                m_clickAudioSource.clip = m_clickInvalidAudio;
            }
            m_clickAudioSource.Play();

        }
    }

    /// <summary>
    /// 播放成功动画
    /// </summary>
    void PlaySuccessAnimator()
    {
        int iRandomValue = Random.Range(0, 4);
        Debug.Log(iRandomValue);
        switch(iRandomValue)
        {
            case 0:
                m_animatorLeaf.Play("WaterHighLeft");
                m_animatorWaterDrop1.Play("WaterDrop");
                break;
            case 1:
                m_animatorLeaf.Play("WaterHighRight");
                m_animatorWaterDrop4.Play("WaterDrop");
                break;
            case 2:
                m_animatorLeaf.Play("WaterLowLeft");
                m_animatorWaterDrop2.Play("WaterDrop");
                break;
            case 3:
                m_animatorLeaf.Play("WaterLowRight");
                m_animatorWaterDrop3.Play("WaterDrop");
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// 播放失败动画
    /// </summary>
    void PlayFailedAnimator()
    {
        if (!m_bIsHeadPlayAnimator)
        {
            m_animatorHead.Play("WaterMusicHead");
            m_bIsHeadPlayAnimator = true;
        } 
    }
}
