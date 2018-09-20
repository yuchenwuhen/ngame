using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishMusicManager : MonoBehaviour
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
    private Queue<AudioSource> m_runAudios = new Queue<AudioSource>();
//    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public List<AudioClip> m_clickAudios = new List<AudioClip>();         // 音效列表
    private AudioSource m_playaudiosource;  //播放音轨
    // 点击音效模块 End/////

    // 不通用分类模块 Begin////
    private Text m_textPoint;                   // 展示文本
                                                // 不通用分类模块 End////
    //按钮模块 Begin//
    private List<Button> m_btnHand;
    private Button m_btnRecord;
    public Sprite[] m_btnRecordSprite;
    private bool m_isRecording = false;
    private Button m_btnPlay;
    public Sprite[] m_btnPlaySprite;
    private bool m_isPlaying = false;
    private Button m_btnAgain;
    public Sprite[] m_btnAgainSprite;
    private Button m_btnBack;
    //按钮模块 End//


    private Dictionary<int, int> m_Index2NoteID; //关卡中的按钮序号对应的收集到的音符

    private Dictionary<float, int> m_pointTime2NoteID; //时间点 2 音符ID
    private List<float> m_clickTimeList;
    private List<int> m_clickStyleList;

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
        listCollectdNoteID.Add(1);
        listCollectdNoteID.Add(3);
        listCollectdNoteID.Add(6);
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

        m_clickAudios = m_dishMusicConfig.GetAudioClip();
        // 临界时间模块 End////

        m_bIsTouch = true;           // 本次触摸是否有效

        for (int i = 1;i<=4;i++)
        {
            AudioSource m_audios =  GameObject.Find("DishAudio" + i).GetComponent<AudioSource>();
            m_runAudios.Enqueue(m_audios);
        }
  //      m_clickAudioSource = GetComponent<AudioSource>();

        m_textPoint = GetComponent<Text>();

        m_btnHand = new List<Button>();
        for (int i = 0; i < 7; i++)
        {
            string sBtnIndex = "Btn" + (i + 1).ToString();
            m_btnHand.Add(transform.Find("Btn").Find(sBtnIndex).GetComponent<Button>());
        }
        m_playaudiosource = this.GetComponent<AudioSource>();
        m_playaudiosource.clip = m_dishMusicConfig.GetAudioClipBgm();
        m_btnRecord = transform.Find("BtnRecord").GetComponent<Button>();
        m_btnRecord.onClick.AddListener(Record);
        m_btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
        m_btnPlay.onClick.AddListener(Play);
        m_btnAgain = transform.Find("BtnAgain").GetComponent<Button>();
        m_btnAgain.onClick.AddListener(RecordAgain);
        m_btnBack = transform.Find("BtnBack").GetComponent<Button>();
        m_btnBack.onClick.AddListener(Back);

        m_Index2NoteID = new Dictionary<int, int>();

        m_pointTime2NoteID = new Dictionary<float, int>();
        m_clickTimeList = new List<float>();
        m_clickStyleList = new List<int>();
    }

    /// <summary>
    /// 重新初始化场景信息
    /// </summary>
    void ReInitSection(List<int> listCollectdNoteID)
    {
        m_clickTimeList.Clear();
        m_clickStyleList.Clear();
        m_Index2NoteID.Clear();
        m_pointTime2NoteID.Clear();
        int[] IndexList = {0,1,2,3,4,5,6};
        m_btnHand[0].onClick.AddListener(delegate () { this.OnPlayerClickBtn(0); });
        m_Index2NoteID.Add(0, 0);
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

        for (int i = 0; i < 7; i++)
        {
            
            if(listCollectdNoteID.Contains(i))
            {
                m_btnHand[i].gameObject.SetActive(true);
            }else
            {
                m_btnHand[i].gameObject.SetActive(false);
            }
        }

        m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[0];
        m_btnPlay.GetComponent<Image>().sprite = m_btnPlaySprite[0];
        StopRecordAudio();
        enabled = true;
        m_bIsTouch = true;
        m_isRecording = false;
        m_isPlaying = false;

        // 播放音乐
        //AudioManager.Instance.PlayMusicSingleAgain(m_dishMusicConfig.GetAudioClipBgm());
    }


    public  void Play()
    {
        m_isPlaying = !m_isPlaying;
        if (m_isPlaying)
        {
            //正在播放
            m_btnPlay.GetComponent<Image>().sprite = m_btnPlaySprite[1];
            PlayRecordAudio();

        }else
        {
            //没有播放
            m_btnPlay.GetComponent<Image>().sprite = m_btnPlaySprite[0];
            PauseRecordAudio();

        }
        enabled = true;

    }

    public  void Record()
    {
        m_isRecording = !m_isRecording;
        if (m_isRecording)
        {
            //正在录制
            m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[1];
            AudioManager.Instance.PlayMusicSingle(m_dishMusicConfig.GetAudioClipBgm());

        }
        else
        {
            //没有录制
            m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[0];
            AudioManager.Instance.PauseMusicSingle(m_dishMusicConfig.GetAudioClipBgm());

        }

    }

    public void RecordAgain()
    {
        List<int> listCollectdNoteID = new List<int>();
        ReInitSection(listCollectdNoteID);
    }

    public  void Back()
    {
        UIManager.instance.ShowUIFade(UIState.Bookmenu);
    }

    /// <summary>
    /// 播放敲击动画
    /// </summary>
    /// <param name="iNpcID"></param>
    void PlayBeatAnimator(int iNpcID)
    {
        //if (iNpcID >= 7)
        //{
        //    Debug.LogError("iNpcID >= 7,iNpcID:" + iNpcID);
        //    return;
        //}
        //if (iNpcID == 5)
        //{
        //    m_animatorHand[iNpcID].Play("ChickenBeat");
        //}
        //else
        //{
        //    m_animatorHand[iNpcID].Play("HandBeat");
        //}
    }

    /// <summary>
    /// 播放点击音效
    /// </summary>
    void PlayClickAudio(int iClickIndex)
    {
        AudioSource m_runaudio = m_runAudios.Dequeue();
        m_runAudios.Enqueue(m_runaudio);
        if (m_runaudio)
        {
            m_runaudio.Stop();
            m_runaudio.clip = m_clickAudios[iClickIndex];
            m_runaudio.Play();

        }
    }
    /// <summary>
    /// 播放播放音效
    /// </summary>
    void PlayRecordAudio()
    {
        if (m_playaudiosource)
        {
            m_playaudiosource.Play();
        }
    }

    void StopRecordAudio()
    {
        if (m_playaudiosource)
        {
            m_playaudiosource.Stop();
        }
    }
    void PauseRecordAudio()
    {
        if (m_playaudiosource)
        {
            m_playaudiosource.Pause();
        }
    }

    private float fLastClickTimeBtn = 0f;
    void OnPlayerClickBtn(int iIndexBtn)
    {
        if (AudioManager.Instance.GetMusicSourceTime() < m_fBeginClickTime || AudioManager.Instance.GetMusicSourceTime() > m_fEndClickTime)
        {
            Debug.Log("不在可以点击的时间范围内");
            return;
        }
        if (AudioManager.Instance.GetMusicSourceTime() - fLastClickTimeBtn > m_fTouchAgainTime)
        {
            PlayClickAudio(iIndexBtn);
            fLastClickTimeBtn = AudioManager.Instance.GetMusicSourceTime();
            //m_pointTime2NoteID.Add(AudioManager.Instance.GetMusicSourceTime(), m_Index2NoteID[iIndexBtn]);
            m_clickTimeList.Add(AudioManager.Instance.GetMusicSourceTime());
            m_clickStyleList.Add(m_Index2NoteID[iIndexBtn]);
        }
    }


}
