using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterMusicManager : MusicManager
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
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public AudioClip[] m_clickAudios;         // 音效列表
    private List<AudioSource> m_listClickAudioSources;// 点击音效AudioSource列表
    private int m_iClickAudioSourceIndex = 0;
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

    public Vector3 m_middleRightPos;//中间点的坐标
    public Vector3 m_middleLeftPos; //中间偏左的坐标

    private bool m_isFirstStart = true;

    private Button m_btnReset;

    public float m_NpcSuccessTime;

    public Sprite m_WaterNoteTeachSprite;
    public Sprite m_WaterNoteSuccessSprite;
    private AudioSource m_audio;

    private RectTransform m_leftNode;
    private RectTransform m_rightNode;

    public bool m_bGameStateRun = false;     // 场景是否正在运行
    void Awake()
    {
        // 初始化场景
        InitGame();
    }

    // Use this for initialization
    void Start()
    {
        m_audio = this.GetComponent<AudioSource>();
        //Debug.Log("InitTime:" + m_fInitTime);
        // 播放音乐(放在Start中调用，保证开始游戏时才会放音乐)
        ReInitSection();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bGameStateRun)
            return;
        //Debug.Log(transform.Find("WaterNote1").position);
        //float fNowTime = AudioManager.Instance.GetMusicSourceTime();
        //float fRunTime = fNowTime - m_fInitTime;//游戏运行的时间
        float fRunTime = AudioManager.Instance.GetMusicSourceTime();//修改未不依赖差值
        //Debug.Log("fRunTime:" + fRunTime);

        if (m_bIsPointEnd)
        {
            //Debug.Log("PointEnd");
            return;
        }

        // 检查当前节超时；教学阶段，可以检查节点是否需要增加NPC操作
        CheckCurPoint(fRunTime);

        // 如果当前拍为空拍，不处理后续玩家的任何点击
        if (m_musicGameConfig.GetSectionOnePointNoteType(m_iNowSectionID, m_iNowPointID) < 0)
        {
            return;
        }

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

        //水滴的范围位置begin//
        m_leftNode = GameObject.Find("LeftNote").GetComponent<RectTransform>();
        m_rightNode = GameObject.Find("RightNote").GetComponent<RectTransform>();
        //水滴的范围位置end//

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

        m_listClickAudioSources = new List<AudioSource>();
        GameObject gameObjectClickAudioSource = transform.Find("ClickAudioSources").Find("ClickAudioSource").gameObject;
        AudioSource clickAudioSource = transform.Find("ClickAudioSources").Find("ClickAudioSource").GetComponent<AudioSource>();
        m_listClickAudioSources.Add(clickAudioSource);
        for (int i = 0; i < 5; i++)
        {
            GameObject tmp = Instantiate(gameObjectClickAudioSource) as GameObject;
            tmp.transform.SetParent(transform.Find("ClickAudioSources"));
            AudioSource tmpAudioSource = tmp.GetComponent<AudioSource>();
            m_listClickAudioSources.Add(tmpAudioSource);
        }
        m_iClickAudioSourceIndex = 0;

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

        for (int i = 0; i < 8; i++)
        {
            string sWaterNote = "WaterNote" + (i + 1).ToString();
            transform.Find("WaterNote").Find(sWaterNote).gameObject.SetActive(false);
            transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().sprite = m_WaterNoteTeachSprite;
            transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().SetNativeSize();
        }

        if (m_bIsTeachStage)
        {
            InitWaterNotePos(m_musicGameConfig.GetPointTimeList(m_iNowSectionID));
        }

        m_animatorLeaf = transform.Find("Leaf").GetComponent<Animator>();
        m_animatorHead = transform.Find("Head").GetComponent<Animator>();
        m_bIsHeadPlayAnimator = false;

        m_animatorWaterDrop1 = transform.Find("WaterDrop").Find("WaterDrop1").GetComponent<Animator>();      // 水滴动画1
        m_animatorWaterDrop2 = transform.Find("WaterDrop").Find("WaterDrop2").GetComponent<Animator>();      // 水滴动画2
        m_animatorWaterDrop3 = transform.Find("WaterDrop").Find("WaterDrop3").GetComponent<Animator>();      // 水滴动画3
        m_animatorWaterDrop4 = transform.Find("WaterDrop").Find("WaterDrop4").GetComponent<Animator>();      // 水滴动画4

        m_btnReset = transform.Find("ResetBtn").GetComponent<Button>();
        m_btnReset.onClick.AddListener(PopPauseWindow);
    }

    /// <summary>
    /// 重新初始化场景信息
    /// </summary>
    public void ReInitSection()
    {
        m_bGameStateRun = true;
        if (m_isFirstStart)
        {
            m_isFirstStart = false;
            enabled = true;
            // 播放音乐(放在Start中调用，保证开始游戏时才会放音乐)
            AudioManager.Instance.PlayMusicSingleAgain(m_musicGameConfig.GetAudioClipBgm());
            m_fInitTime = AudioManager.Instance.GetMusicSourceTime();
        }
        else
        {
            //初始化数据
            enabled = true;

            m_iNowSectionID = 0;              // 该玩法中，当前小节ID
            m_iSectionCount = m_musicGameConfig.GetSectionCount();              // 该玩法中，音乐小节总数
            m_bIsPointEnd = false;
            m_bIsTeachStage = m_musicGameConfig.GetSectionType(m_iNowSectionID) == 1 ? true : false;             // 是否在教学阶段
            m_bIsSevenClickStage = m_musicGameConfig.GetSectionType(m_iNowSectionID) == 2 ? true : false;        // 是否在七次点击阶段
            m_bIsNpcHasAction = false;

            m_iNowPointID = 0;                // 当前小节中，目前所处的节奏点序号
            m_iNowSectionPointCount = m_musicGameConfig.GetSectionPointCount(m_iNowSectionID);      // 当前小节中，节奏点总个数
            m_bIsTouch = true;           // 本次触摸是否有效

            m_iClickAudioSourceIndex = 0;

            m_iFailTimes = 0;

            if (m_bIsTeachStage)
            {
                m_textPoint.text = "教学阶段";
            }
            else
            {
                m_textPoint.text = "玩家阶段";
            }

            Debug.Log("reset");
            for (int i = 0; i < 8; i++)
            {
                string sWaterNote = "WaterNote" + (i + 1).ToString();
                transform.Find("WaterNote").Find(sWaterNote).gameObject.SetActive(false);
                transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().sprite = m_WaterNoteTeachSprite;
                transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().SetNativeSize();
            }

            if (m_bIsTeachStage)
            {
                InitWaterNotePos(m_musicGameConfig.GetPointTimeList(m_iNowSectionID));
            }

            m_bIsHeadPlayAnimator = false;

            // 播放音乐
            AudioManager.Instance.PlayMusicSingleAgain(m_musicGameConfig.GetAudioClipBgm());
            m_fInitTime = AudioManager.Instance.GetMusicSourceTime();
        }   
    }

    /// <summary>
    /// 检查当前节点是否超时;检查当前节奏点是否到达，可以增加NPC处理
    /// </summary>
    /// <param name="fRunTime"></param>
    void CheckCurPoint(float fRunTime)
    {
        float checkPointTime = m_musicGameConfig.GetSectionOnePointTime(m_iNowSectionID, m_iNowPointID);
        //int iPointStyle = m_musicGameConfig.GetSectionOnePointStyle(m_iNowSectionID, m_iNowPointID);

        // 教学关中，NPC行动，但是不更新当前节点ID，更新节点一样走原来的超时逻辑
        if (m_bIsTeachStage && !m_bIsNpcHasAction && Mathf.Abs(checkPointTime - fRunTime) < m_NpcSuccessTime)
        {
            Debug.Log("NPC行动");
            Debug.Log("checkPointTime:" + checkPointTime + ",fRunTime:" + fRunTime);
            m_bIsNpcHasAction = true;

            // SevenClick 关卡
            if (m_bIsSevenClickStage && m_iNowPointID != m_musicGameConfig.GetSevenClickPlayerIndex(m_iNowSectionID))
            {
                Debug.Log("NPC打盘子");
            }

            // 教学关
            if (m_bIsTeachStage)
            {
                if (m_musicGameConfig.GetSectionOnePointNoteType(m_iNowSectionID, m_iNowPointID) >= 0)
                {
                    // 如果符号类型大于0，则正常显示音符
                    string sWaterNote = "WaterNote" + (m_iNowPointID + 1).ToString();
                    //Debug.Log(sWaterNote);
                    transform.Find("WaterNote").Find(sWaterNote).gameObject.SetActive(true);
                }
                else
                {
                    // 如果音符类型为-1，则空拍处理

                }
            }
        }

        if (fRunTime > checkPointTime + m_fTouchCheckTime)
        {
            Debug.Log("节奏点超时");

            // 播放失败动画(只有当玩家关卡，且不为空拍是才能调用)
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
        Debug.Log("checkPointTime:" + checkPointTime + ",fRunTime:" + fRunTime);
        if (Mathf.Abs(checkPointTime - fRunTime) < m_fTouchSuccessTime)
        {
            Debug.Log("检测成功");
            string sWaterNote = "WaterNote" + (m_iNowPointID + 1).ToString();
            transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().sprite = m_WaterNoteSuccessSprite;
            transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().SetNativeSize();
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

        Debug.Log(m_iNowSectionID + "|" + m_iNowPointID);
        if (m_iNowPointID >= m_musicGameConfig.GetSectionPointCount(m_iNowSectionID))
        {
            Debug.Log(m_iNowSectionID + "|" + m_iNowPointID);
            Debug.Log("改变小节");
            m_iNowSectionID++;
            if (m_iNowSectionID >= m_musicGameConfig.GetSectionCount())
            {
                Debug.Log("节奏点全部结束");

                UIManager.instance.CalculationCurMusicResult(CaculateStar(m_iFailTimes));
                enabled = false;
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
            if (m_bIsTeachStage)
            {
                for (int i = 0; i < 8; i++)
                {
                    string sWaterNote = "WaterNote" + (i + 1).ToString();                   
                    transform.Find("WaterNote").Find(sWaterNote).gameObject.SetActive(false);
                    transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().sprite = m_WaterNoteTeachSprite;
                    transform.Find("WaterNote").Find(sWaterNote).GetComponent<Image>().SetNativeSize();
                }
                InitWaterNotePos(m_musicGameConfig.GetPointTimeList(m_iNowSectionID));
            }
        }
    }

    private int CaculateStar(int fail)
    {
        if (fail == 0)
            return 3;
        else if (fail > 0 && fail <= 5)
            return 2;
        else if (fail > 5 && fail <= 9)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    /// <param name="iClickState">点击状态 0成功 -1无效 1失败 </param>
    void PlayClickAudio(int iClickState, int iStyle)
    {
        m_listClickAudioSources[m_iClickAudioSourceIndex % 5].Stop();

        if (iClickState == 0)
        {
            m_listClickAudioSources[m_iClickAudioSourceIndex % 5].clip = m_clickAudios[iStyle];
        }
        else if (iClickState == 1)
        {
            m_listClickAudioSources[m_iClickAudioSourceIndex % 5].clip = m_clickFailAudio;
        }
        else if (iClickState == -1)
        {
            m_listClickAudioSources[m_iClickAudioSourceIndex % 5].clip = m_clickInvalidAudio;
        }
        m_listClickAudioSources[m_iClickAudioSourceIndex % 5].Play();

        m_iClickAudioSourceIndex++;
    }

    /// <summary>
    /// 播放成功动画
    /// </summary>
    void PlaySuccessAnimator()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 4);
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

    /// <summary>
    /// 初始化水滴音符的位置
    /// </summary>
    /// <param name="listPointTime"></param>
    void InitWaterNotePos(List<float> listPointTime)
    {
        int iPointCount = listPointTime.Count;
        if (iPointCount == 0)
        {
            return;
        }
        else if (iPointCount == 1)
        {
            string sWaterNote = "WaterNote1";
            transform.Find("WaterNote").Find(sWaterNote).GetComponent<RectTransform>().anchoredPosition = (m_leftNode.anchoredPosition + m_rightNode.anchoredPosition) / 2;
        }
        else
        {
            float fBeginTime = listPointTime[0];
            float fEndTime = listPointTime[iPointCount - 1];

            float fBeginPosX = m_leftNode.anchoredPosition.x;
            float fEndPosX = m_rightNode.anchoredPosition.x;

            float fRatePosX2Time = (fEndPosX - fBeginPosX) / (fEndTime - fBeginTime);
            for (int i = 0; i < iPointCount; i++)
            {
                string sWaterNote = "WaterNote" + (i + 1).ToString();
                transform.Find("WaterNote").Find(sWaterNote).GetComponent<RectTransform>().anchoredPosition = new Vector2(m_leftNode.anchoredPosition.x + (listPointTime[i] - fBeginTime) * fRatePosX2Time, m_leftNode.anchoredPosition.y);
            }
        }      
    }

    private void PopPauseWindow()
    {
        UIManager.instance.PopPauseWindow(this);
        m_bGameStateRun = false;
        AudioManager.Instance.PauseMusicSingle(m_musicGameConfig.GetAudioClipBgm());
    }

    public override void Continue()
    {
        m_bGameStateRun = true;
        UIManager.instance.DisappearUIWindow<PausePanel>();
        AudioManager.Instance.PlayMusicSingle(m_musicGameConfig.GetAudioClipBgm());
    }

    public override void Record()
    {
        ReInitSection();
        UIManager.instance.DisappearUIWindow<PausePanel>();
    }

    public override void Exit()
    {
        UIManager.instance.ShowUIFade(UIState.Scene);
        UIManager.instance.DisappearUIWindow<PausePanel>();
    }
}
