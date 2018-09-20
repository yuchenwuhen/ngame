using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishMusicManager : MusicManager
{
    [SerializeField]
    public DishMusicConfig m_dishMusicConfig; //盘子关卡配置

    // 临界时间模块 Begin////
    public float m_fTouchAgainTime;           //玩家再次touch时间
    // 临界时间模块 End////

    // 当前状态模块 Begin////
    private bool m_bIsTouch = true;           // 本次触摸是否有效
    private float m_fLastTouchTime;           // 上次触摸的时间
    // 当前状态模块 Begin////

    // 点击音效模块 Begin/////
    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public AudioClip[] m_clickAudios;         // 音效列表
    // 点击音效模块 End/////

    // 不通用分类模块 Begin////
    private Text m_textPoint;                   // 展示文本
                                                // 不通用分类模块 End////

    private List<Animator> m_animatorHand;
    private List<Button> m_btnHand;

    private Button m_btnPause;

    private Dictionary<int, int> m_Index2NoteID; //关卡中的按钮序号对应的收集到的音符ID

    private Dictionary<float, int> m_pointTime2NoteID; //时间点 2 音符ID

    private float m_fBeginClickTime; //规定玩家开始点击的时间
    private float m_fEndClickTime; //规定玩家结束点击的时间

    void Awake()
    {
        // 初始化场景
        InitGame();
    }

    // Use this for initialization
    void Start()
    {
        List<int> listCollectdNoteID = new List<int>();
        ReInitSection(listCollectdNoteID);
    }

    // Update is called once per frame
    void Update()
    {
        float fRunTime = AudioManager.Instance.GetMusicSourceTime();//游戏运行的时间

        // 如果当前不能触摸,检查触摸CD是否已过
        if (!m_bIsTouch && (fRunTime - m_fLastTouchTime) > m_fTouchAgainTime)
        {
            m_bIsTouch = true;
        }

        //// 玩家点击,教学阶段不检测玩家点击
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (m_bIsTouch)
        //    {
        //        // 当前点击有效
        //        m_fLastTouchTime = fRunTime;
        //        //CheckPlayerInput(fRunTime); // 检测玩家有效点击情况
        //        m_bIsTouch = false;  // CD时间内，点击无效
        //    }
        //    else
        //    {
        //        // 当前点击无效
        //        Debug.Log("CD时间内");
        //    }
        //}
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    void InitGame()
    {
        // 临界时间模块 Begin////
        m_fTouchAgainTime = m_dishMusicConfig.GetTouchAgainTime();           //玩家再次touch时间

        m_fBeginClickTime = m_dishMusicConfig.GetBeginClickTime();
        m_fEndClickTime = m_dishMusicConfig.GetEndClickTime();
        // 临界时间模块 End////

        m_bIsTouch = true;           // 本次触摸是否有效

        m_clickAudioSource = GetComponent<AudioSource>();

        m_textPoint = GetComponent<Text>();

        m_animatorHand = new List<Animator>();
        m_btnHand = new List<Button>();
        for (int i = 0; i < 7; i++)
        {
            string sHandIndex = "Hand" + (i + 1).ToString();
            m_animatorHand.Add(transform.Find("Hand").Find(sHandIndex).GetComponent<Animator>());

            string sBtnIndex = "Btn" + (i + 1).ToString();
            m_btnHand.Add(transform.Find("Btn").Find(sBtnIndex).GetComponent<Button>());
        }

        m_btnPause = transform.Find("btnPause").GetComponent<Button>();
        m_btnPause.onClick.AddListener(PopPauseWindow);

        m_Index2NoteID = new Dictionary<int, int>();
        m_pointTime2NoteID = new Dictionary<float, int>();
    }

    /// <summary>
    /// 重新初始化场景信息
    /// </summary>
    void ReInitSection(List<int> listCollectdNoteID)
    {
        m_Index2NoteID.Clear();
        m_pointTime2NoteID.Clear();
        if (listCollectdNoteID.Count == 0 || listCollectdNoteID.Count == 7)
        {
            //for (int i = 0; i < 7; i++)
            //{
            //    Debug.Log(i);
            //    m_btnHand[i].onClick.AddListener(delegate () { this.OnPlayerClickBtn(i); });
            //    m_Index2NoteID.Add(i, i);
            //}
            m_btnHand[0].onClick.AddListener(delegate () { this.OnPlayerClickBtn(0); });
            m_Index2NoteID.Add(0,0);
            m_btnHand[1].onClick.AddListener(delegate () { this.OnPlayerClickBtn(1); });
            m_Index2NoteID.Add(1, 1);
            m_btnHand[2].onClick.AddListener(delegate () { this.OnPlayerClickBtn(2); });
            m_Index2NoteID.Add(2, 2);
            m_btnHand[3].onClick.AddListener(delegate () { this.OnPlayerClickBtn(3); });
            m_Index2NoteID.Add(3, 3);
            m_btnHand[4].onClick.AddListener(delegate () { this.OnPlayerClickBtn(4); });
            m_Index2NoteID.Add(4, 4);
            m_btnHand[5].onClick.AddListener(delegate () { this.OnPlayerClickBtn(5); });
            m_Index2NoteID.Add(5, 5);
            m_btnHand[6].onClick.AddListener(delegate () { this.OnPlayerClickBtn(6); });
            m_Index2NoteID.Add(6, 6);
        }
        else
        {
            int[] IndexList = {3,2,4,1,5,0,6};
            for (int i = 0; i < listCollectdNoteID.Count; i++)
            {
                m_btnHand[IndexList[i]].onClick.AddListener(delegate () { this.OnPlayerClickBtn(i); });
                m_Index2NoteID.Add(IndexList[i], i);
            }

        }
        enabled = true;
        m_bIsTouch = true;

        // 播放音乐
        AudioManager.Instance.PlayMusicSingleAgain(m_dishMusicConfig.GetAudioClipBgm());
    }

    /// <summary>
    /// 播放敲击动画
    /// </summary>
    /// <param name="iNpcID"></param>
    void PlayBeatAnimator(int iNpcID)
    {
        if (iNpcID >= 7)
        {
            Debug.LogError("iNpcID >= 7,iNpcID:" + iNpcID);
            return;
        }
        if (iNpcID == 5)
        {
            m_animatorHand[iNpcID].Play("ChickenBeat");
        }
        else
        {
            m_animatorHand[iNpcID].Play("HandBeat");
        }
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    void PlayClickAudio(int iClickIndex)
    {
        if (m_clickAudioSource)
        {
            m_clickAudioSource.Stop();
            m_clickAudioSource.clip = m_clickAudios[iClickIndex];
            m_clickAudioSource.Play();
        }
    }

    private float fLastClickTimeBtn = 0f;
    void OnPlayerClickBtn(int iIndexBtn)
    {
        Debug.Log(AudioManager.Instance.GetMusicSourceTime());
        Debug.Log(m_fBeginClickTime);
        if (AudioManager.Instance.GetMusicSourceTime() < m_fBeginClickTime || AudioManager.Instance.GetMusicSourceTime() > m_fEndClickTime)
        {
            Debug.Log("不在可以点击的时间范围内");
            return;
        }
        if (AudioManager.Instance.GetMusicSourceTime() - fLastClickTimeBtn > m_fTouchAgainTime)
        {
            PlayBeatAnimator(iIndexBtn);
            PlayClickAudio(iIndexBtn);
            fLastClickTimeBtn = AudioManager.Instance.GetMusicSourceTime();
            m_pointTime2NoteID.Add(AudioManager.Instance.GetMusicSourceTime(), m_Index2NoteID[iIndexBtn]);
        }
    }

    private void PopPauseWindow()
    {
        UIManager.instance.PopPauseWindow(this);
        enabled = false;
        AudioManager.Instance.PauseMusicSingle(m_dishMusicConfig.GetAudioClipBgm());
    }

    public override void Continue()
    {
        enabled = true;
        UIManager.instance.DisappearUIWindow<PausePanel>();
        AudioManager.Instance.PlayMusicSingle(m_dishMusicConfig.GetAudioClipBgm());
    }

    public override void Record()
    {
        List<int> listCollectdNoteID = new List<int>();
        ReInitSection(listCollectdNoteID);
        UIManager.instance.DisappearUIWindow<PausePanel>();
    }

    public override void Exit()
    {
        UIManager.instance.ShowUIFade(UIState.Scene);
        UIManager.instance.DisappearUIWindow<PausePanel>();
    }
}
