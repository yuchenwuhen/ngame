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

    // 点击音效模块 Begin/////
    private Queue<AudioSource> m_runAudios = new Queue<AudioSource>();
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public List<AudioClip> m_clickAudios = new List<AudioClip>();         // 音效列表
    private AudioSource m_playaudiosource;  //播放音轨
    public List<GameObject> m_audioObject = new List<GameObject>();
    private List<GameObject> m_allAudioObj = new List<GameObject>();
    private Transform m_AudioParent;
    // 点击音效模块 End/////

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

    private List<float> m_clickTimeList;
    private List<int> m_clickStyleList;

    private float m_fBeginClickTime; //规定玩家开始点击的时间
    private float m_fEndClickTime; //规定玩家结束点击的时间

    private List<int> m_listCollectdNoteID;

    private float fLastClickTimeBtn;    
    private bool m_bIsBtnClick;

    private bool m_bIsRecordLoaderMove;
    private bool m_isStartRecord = false;

    // 滑块移动的范围
    private int id = 0;
    private float m_moveSpeed = 0;
    private RectTransform m_recordLoader;
    private RectTransform m_playLoader;
    private bool m_isMoveRecord = false;
    private bool m_isMovePlay = false;
    private float m_fLeftPosX;
    private float m_fRightPosX;
    private float m_fTopPoxY1;
    private float m_fTopPoxY2;
    private float m_fTopPoxY3;
    private float fMusicLength = 0;

    //教学界面
    public GameObject m_teachPanel;
    private Button m_closeBtn;

    void Awake()
    {
        // 初始化场景
        InitGame();
    }

    // Use this for initialization
    public void Init()
    {
        ReInitSection(GameManager.instance.collectionNumbers);
    }

    private void Update()
    {      
        if(m_isMovePlay)
        {
            //播放已录制的音轨
            if(id<m_clickTimeList.Count)
            {
                if(Mathf.Abs(m_clickTimeList[id]- GetTime())<0.002f)
                {
                    //播放当前节点声音
                    PlayClickAudio(m_clickStyleList[id]);
                    id++;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_isMoveRecord)
        {
            m_recordLoader.anchoredPosition = new Vector2(m_recordLoader.anchoredPosition.x+ m_moveSpeed * Time.fixedDeltaTime, m_recordLoader.anchoredPosition.y);
            if(Vector2.Distance(m_recordLoader.anchoredPosition,new Vector2(m_fRightPosX,m_fTopPoxY1))<0.3f*2)
            {
                m_recordLoader.anchoredPosition = new Vector2(m_fLeftPosX, m_fTopPoxY2);
            }else if(Vector2.Distance(m_recordLoader.anchoredPosition, new Vector2(m_fRightPosX, m_fTopPoxY2)) < 0.3f*2)
            {
                m_recordLoader.anchoredPosition = new Vector2(m_fLeftPosX, m_fTopPoxY3);
            }else if(Vector2.Distance(m_recordLoader.anchoredPosition, new Vector2(m_fRightPosX, m_fTopPoxY3)) < 0.3f*2)
            {
                //停止移动
                m_isMoveRecord = false;
            }
        }

        if (m_isMovePlay)
        {
            m_playLoader.anchoredPosition = new Vector2(m_playLoader.anchoredPosition.x + m_moveSpeed * Time.fixedDeltaTime, m_playLoader.anchoredPosition.y);
            if (Vector2.Distance(m_playLoader.anchoredPosition, new Vector2(m_fRightPosX, m_fTopPoxY1)) < 0.3f )
            {
                m_playLoader.anchoredPosition = new Vector2(m_fLeftPosX, m_fTopPoxY2);
            }
            else if (Vector2.Distance(m_playLoader.anchoredPosition, new Vector2(m_fRightPosX, m_fTopPoxY2)) < 0.3f )
            {
                m_playLoader.anchoredPosition = new Vector2(m_fLeftPosX, m_fTopPoxY3);
            }
            else if (Vector2.Distance(m_playLoader.anchoredPosition, new Vector2(m_fRightPosX, m_fTopPoxY3)) < 0.3f )
            {
                //停止移动
                m_isMovePlay = false;
            }
        }
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

        fMusicLength = m_dishMusicConfig.GetAudioClipBgm().length;

        m_clickAudios = m_dishMusicConfig.GetAudioClip();
        // 临界时间模块 End////

        // 播放点击音效的集合
        for (int i = 1; i <= 4; i++)
        {
            AudioSource m_audios =  GameObject.Find("DishAudio" + i).GetComponent<AudioSource>();
            m_runAudios.Enqueue(m_audios);
        }

        // 玩家点击按钮集合
        m_btnHand = new List<Button>();
        for (int i = 0; i < 7; i++)
        {
            string sBtnIndex = "Btn" + (i + 1).ToString();
            m_btnHand.Add(transform.Find("Btn").Find(sBtnIndex).GetComponent<Button>());
        }
        m_closeBtn = GameObject.Find("closeBtn").GetComponent<Button>();
        m_closeBtn.onClick.AddListener(CloseTeachPanel);

        // 主音乐音轨
        m_playaudiosource = this.GetComponent<AudioSource>();
        m_playaudiosource.clip = m_dishMusicConfig.GetAudioClipBgm();

        // 界面上功能按钮
        m_btnRecord = transform.Find("BtnRecord").GetComponent<Button>();
        m_btnRecord.onClick.AddListener(Record);
        m_btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
        m_btnPlay.onClick.AddListener(Play);
        m_btnAgain = transform.Find("BtnAgain").GetComponent<Button>();
        m_btnAgain.onClick.AddListener(RecordAgain);
        m_btnBack = transform.Find("BtnBack").GetComponent<Button>();
        m_btnBack.onClick.AddListener(Back);

        // 索引到音符ID
        m_Index2NoteID = new Dictionary<int, int>();
        m_Index2NoteID.Clear();
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

        // 玩家点击的时间列表
        m_clickTimeList = new List<float>();

        // 玩家点击的音符列表
        m_clickStyleList = new List<int>();

        // 玩家收集到的音符列表
        m_listCollectdNoteID = new List<int>();

        fLastClickTimeBtn = 0f;
        m_bIsBtnClick = false;

        m_AudioParent = transform.Find("AudioParent");
        m_recordLoader = GameObject.Find("RecordLoader").GetComponent<RectTransform>();
        m_playLoader = GameObject.Find("PlayLoader").GetComponent<RectTransform>();
        m_fLeftPosX = transform.Find("PosLimit").Find("PosLimitLeft").GetComponent<RectTransform>().anchoredPosition.x;
        m_fRightPosX = transform.Find("PosLimit").Find("PosLimitRight").GetComponent<RectTransform>().anchoredPosition.x;
        m_fTopPoxY1 = transform.Find("PosLimit").Find("PosLimitTop1").GetComponent<RectTransform>().anchoredPosition.y;
        m_fTopPoxY2 = transform.Find("PosLimit").Find("PosLimitTop2").GetComponent<RectTransform>().anchoredPosition.y;
        m_fTopPoxY3 = transform.Find("PosLimit").Find("PosLimitTop3").GetComponent<RectTransform>().anchoredPosition.y;
    }

    void CloseTeachPanel()
    {
        m_teachPanel.SetActive(false);
    }

    /// <summary>
    /// Record位置
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRecoderPos()
    {
        return m_recordLoader.gameObject.GetComponent<Transform>().position;
    }

    /// <summary>
    /// 重新初始化场景信息，主要给外界UI传递玩家收集到的元素
    /// </summary>
    void ReInitSection(List<int> listCollectdNoteID)
    {
        m_listCollectdNoteID.Clear();
        foreach (int a in listCollectdNoteID)
        {
            m_listCollectdNoteID.Add(a);
        }

        // 重新初始化游戏
        ReInitGame();
    }

    // 重新初始化游戏
    void ReInitGame()
    {
        m_clickTimeList.Clear();
        m_clickStyleList.Clear();
        m_bIsRecordLoaderMove = false;
        m_moveSpeed = 2.9f * Mathf.Abs(m_fRightPosX - m_fLeftPosX) / fMusicLength;
        InitRecordLoaderPos();
        m_isStartRecord = false;
        for (int i = 0; i < m_allAudioObj.Count; i++)
        {
            Destroy(m_allAudioObj[i]);
        }
        m_allAudioObj.Clear();
        // 根据传进来的玩家收集到的音符列表，显示出可点击的音符按钮
        for (int i = 0; i < 7; i++)
        {
            if (m_listCollectdNoteID.Contains(i))
            {
                m_btnHand[i].gameObject.SetActive(true);
            }
            else
            {
                m_btnHand[i].gameObject.SetActive(false);
            }
        }

        m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[0];
        m_btnPlay.gameObject.SetActive(true);

        m_btnPlay.GetComponent<Image>().sprite = m_btnPlaySprite[0];
        m_btnPlay.gameObject.SetActive(false);

        m_btnAgain.gameObject.SetActive(false);

        // 停止播放主界面音效
        StopRecordAudio();

        m_isRecording = false;
        m_isPlaying = false;

        m_bIsBtnClick = false;

        fLastClickTimeBtn = 0f;
        id = 0;

        m_bIsRecordLoaderMove = false;
    }

    /// <summary>
    /// 初始化进度条位置
    /// </summary>
    void InitRecordLoaderPos()
    {
        m_playLoader.anchoredPosition =  m_recordLoader.anchoredPosition = new Vector2(m_fLeftPosX, m_fTopPoxY1);
    }

    /// <summary>
    /// 播放玩家录制好的音乐按钮
    /// </summary>
    public void Play()
    {
        m_isPlaying = !m_isPlaying;
        if (m_isPlaying)
        {
            //正在播放
            m_btnPlay.GetComponent<Image>().sprite = m_btnPlaySprite[1];
            PlayRecordAudio();
            m_isMovePlay = true;
            m_btnRecord.gameObject.SetActive(false);
        }
        else
        {
            //没有播放
            m_btnPlay.GetComponent<Image>().sprite = m_btnPlaySprite[0];
            PauseRecordAudio();
            m_isMovePlay = false;
            m_btnRecord.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 录制按钮
    /// </summary>
    public void Record()
    {
        m_isRecording = !m_isRecording;
        if (m_isRecording)
        {
            //正在录制
            m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[1];
            AudioManager.Instance.PlayMusicSingle(m_dishMusicConfig.GetAudioClipBgm());
            m_isMoveRecord = true;
            m_btnPlay.gameObject.SetActive(false);
            m_btnAgain.gameObject.SetActive(false);

            m_bIsBtnClick = true;

            m_bIsRecordLoaderMove = true;
        }
        else
        {
            //没有录制
            m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[0];
            AudioManager.Instance.PauseMusicSingle(m_dishMusicConfig.GetAudioClipBgm());
            m_isMoveRecord = false; 
            m_btnPlay.gameObject.SetActive(true);
            m_btnAgain.gameObject.SetActive(true);

            m_bIsBtnClick = false;

            m_bIsRecordLoaderMove = false;
        }
    }

    /// <summary>
    /// 重新录制按钮
    /// </summary>
    public void RecordAgain()
    {
        ReInitGame();
        m_bIsBtnClick = false;
        m_btnRecord.gameObject.SetActive(true);
        m_isRecording = true;
        //正在录制
        m_btnRecord.GetComponent<Image>().sprite = m_btnRecordSprite[1];
        AudioManager.Instance.PlayMusicSingleAgain(m_dishMusicConfig.GetAudioClipBgm());

        m_isMoveRecord = true;
        m_btnPlay.gameObject.SetActive(false);
        m_btnAgain.gameObject.SetActive(false);

        m_bIsBtnClick = true;

        m_bIsRecordLoaderMove = true;

        m_isMovePlay = false;
    }

    /// <summary>
    /// 返回主界面按钮
    /// </summary>
    public void Back()
    {
        UIManager.instance.ShowUIFade(UIState.Bookmenu);
        StopRecordAudio();
        AudioManager.Instance.StopMusicSingle(m_dishMusicConfig.GetAudioClipBgm());
        if(m_isRecording)
        {
            Record();
        }
        if (m_isStartRecord)
        {
            //存在数据
            GameManager.instance.m_clickTimeList = m_clickTimeList;
            GameManager.instance.m_clickStyleList = m_clickStyleList;
            UIManager.instance.PassLevel3();
        }

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
    /// 播放录制好的音效
    /// </summary>
    void PlayRecordAudio()
    {
        if (m_playaudiosource)
        {
            m_playaudiosource.clip = m_playaudiosource.clip;
            m_playaudiosource.Play();   
        }
    }

    float GetTime()
    {
        return m_playaudiosource.time;
    }

    /// <summary>
    /// 停止播放BGM声音
    /// </summary>
    void StopRecordAudio()
    {
        if (m_playaudiosource)
        {
            m_playaudiosource.Stop();
        }
    }

    /// <summary>
    /// 暂停播放BGM
    /// </summary>
    void PauseRecordAudio()
    {
        if (m_playaudiosource)
        {
            m_playaudiosource.Pause();
        }
    }

    /// <summary>
    /// 玩家点击某个音符按钮
    /// </summary>
    void OnPlayerClickBtn(int iIndexBtn)
    {
        if (!m_bIsBtnClick)
        {
            Debug.Log("该阶段不可以点击");
            return;
        }

        if (AudioManager.Instance.GetMusicSourceTime() < m_fBeginClickTime || AudioManager.Instance.GetMusicSourceTime() > m_fEndClickTime)
        {
            Debug.Log("不在可以点击的时间范围内");
            return;
        }

        if (AudioManager.Instance.GetMusicSourceTime() - fLastClickTimeBtn > m_fTouchAgainTime)
        {
            PlayClickAudio(iIndexBtn);
            fLastClickTimeBtn = AudioManager.Instance.GetMusicSourceTime();
            m_clickTimeList.Add(AudioManager.Instance.GetMusicSourceTime());
            m_clickStyleList.Add(m_Index2NoteID[iIndexBtn]);
            GameObject go = GameObject.Instantiate(m_audioObject[m_Index2NoteID[iIndexBtn]]);
            go.transform.position = GetRecoderPos();
            go.transform.SetParent(m_AudioParent);
            go.SetActive(true);
            m_allAudioObj.Add(go);
            m_isStartRecord = true;
        }
    }
}
