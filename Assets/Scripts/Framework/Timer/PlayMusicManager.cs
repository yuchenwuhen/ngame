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

    private float m_touchTimer = 0;             // 每次触摸的定时器

    private AudioSource m_clickAudioSource;   // 点击音效
    public AudioClip m_clickInvalidAudio;     // 点击无效音效
    public AudioClip m_clickFailAudio;        // 点击失败音效
    public AudioClip[] m_clickAudios;         // 音效列表

    private Animator m_animatorCutWood;       // 劈柴动画
    private Button m_btnReset;                // 重置按钮

    private GameObject m_woodsuccess;         // 木块劈开画面
    private float m_woodSuccessTime = 0.2f;   // 木块劈开画面持续时间

    public int m_iMaxStar = 3;                // 最多可获得星星数
    private int m_iFailTimes;

    private bool m_bIsAskResult = true;                // 是否调用过结算函数
    private bool m_bGameStateRun = false;     // 场景是否正在运行

    public int m_InitNpcCount = 10;
    public int m_MaxMoveNpcCount = 3;
    private Queue<NPCBehaviour> queueCanUseNpc = new Queue<NPCBehaviour>();
    private Queue<NPCBehaviour> queueRunNpc = new Queue<NPCBehaviour>();
    private int m_iHandlePointID;
    private GameObject npc;
    private bool m_isFirstStart = true;
    
    // Use this for initialization
    void Start () 
    {
        // 节奏点数据
        m_songData = this.GetComponent<SongData>();
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
        m_songPointCount = m_songData.GetPlayerSongList().Count;
        npc = GameObject.Find("NPC");
        npc.GetComponent<NPCBehaviour>().Init();
        queueCanUseNpc.Enqueue(npc.GetComponent<NPCBehaviour>());
        for (int i = 0; i < m_InitNpcCount - 1; i++)
        {
            NPCBehaviour tmpBehaviour;
            GameObject tmp = Instantiate(npc) as GameObject;
            tmp.transform.SetParent(transform);
            tmpBehaviour = tmp.GetComponent<NPCBehaviour>();
            tmpBehaviour.Init();
            queueCanUseNpc.Enqueue(tmpBehaviour);
        }

        // 需要移动的NPC
        m_iHandlePointID = 0;
        for (int i = 0; i < m_MaxMoveNpcCount; i++)
        {
            float checkPointTime = m_songData.GetPlayerSongList()[m_iHandlePointID];
            int iPointStyle = m_songData.GetPlayerSongStyleList()[m_iHandlePointID];

            NPCBehaviour tmp = queueCanUseNpc.Dequeue();
            tmp.BeginMove(checkPointTime, iPointStyle);

            queueRunNpc.Enqueue(tmp);
            m_iHandlePointID++;
        }
        ResetClick();
    }

    /// <summary>
    /// 重置音乐场景
    /// </summary>
    public void ResetClick()
    {
        if(m_isFirstStart)
        {
            m_isFirstStart = false;
            m_bGameStateRun = true;    // 运行状态
            AudioManager.Instance.PlayMusicSingle(m_audioClip[(int)m_musicSong]);
        }
        else
        {
            m_bIsAskResult = true;
            m_woodsuccess.SetActive(false);
            isTouch = true;            // 本次触摸是否有效
                                       // 初始化所有NPC
                                       // 当前节奏点ID，整段音乐的节奏点个数
            m_checkPointID = 0;
            m_songPointCount = m_songData.GetPlayerSongList().Count;
            m_touchTimer = 0;
            // 失败次数
            m_iFailTimes = 0;
            while (queueRunNpc.Count > 0)
            {
                NPCBehaviour nPCBehaviour = queueRunNpc.Dequeue();
                queueCanUseNpc.Enqueue(nPCBehaviour);
                nPCBehaviour.Init();
            }
            // 需要移动的NPC
            m_iHandlePointID = 0;
            for (int i = 0; i < m_MaxMoveNpcCount; i++)
            {
                float checkPointTime = m_songData.GetPlayerSongList()[m_iHandlePointID];
                int iPointStyle = m_songData.GetPlayerSongStyleList()[m_iHandlePointID];

                NPCBehaviour tmp = queueCanUseNpc.Dequeue();
                tmp.BeginMove(checkPointTime, iPointStyle);

                queueRunNpc.Enqueue(tmp);
                m_iHandlePointID++;
            }
            m_bGameStateRun = true;    // 运行状态
            AudioManager.Instance.PlayMusicSingle(m_audioClip[(int)m_musicSong]);
        }
       
    }


	// Update is called once per frame
	void Update () 
    {
        if (!m_bGameStateRun)
        {
            Debug.Log("Game pause");
            return;
        }      

        // 检查节点是否全部结束
        if (m_checkPointID >= m_songPointCount)
        {
            if (m_bIsAskResult)
            {
                m_bIsAskResult = false;
                Invoke("GameEnd", 2f);
            }
            return;
        }

        // 触摸CD时间内，更新触摸定时器
        if (!isTouch)
        {
            if(AudioManager.Instance.GetMusicSourceTime()-m_touchTimer>=m_touchAgainTime)
            {
                isTouch = true;
            }
        }

        for (int i = 0; i < queueRunNpc.ToArray().Length; i++)
        {
            queueRunNpc.ToArray()[i].Move(Time.deltaTime);
        }

        //检查当前节点是否超时未被点击。如果当前节点超时未点击，则当前节点失败，移到下一个节点
        CheckCurTouchTime(AudioManager.Instance.GetMusicSourceTime());

        // 玩家点击
        if(Input.GetMouseButtonDown(0))
        {
            if(isTouch)
            {
                // 当前点击有效
                m_animatorCutWood.Play("Guoqi", -1, 0f);
                m_animatorCutWood.Update(0f);
                CheckPlayerInput(AudioManager.Instance.GetMusicSourceTime()); // 检测玩家有效点击情况
                m_touchTimer = AudioManager.Instance.GetMusicSourceTime();
                isTouch = false; // 点击定时器结束之前，点击无效
            }
            else
            {
                // 当前点击无效
                Debug.Log("CD时间内");
            }
        }
	}

    private void FixedUpdate()
    {
        if (!m_bGameStateRun)
            return;

        for (int i = 0; i < queueCanUseNpc.ToArray().Length; i++)
        {
            queueCanUseNpc.ToArray()[i].SuccessMove(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 检测玩家的CD内点击情况
    /// </summary>
    /// <param name="curTime"></param>
    private void CheckPlayerInput(float curTime)
    {
        float checkPointTime = m_songData.GetPlayerSongList()[m_checkPointID];
        int iPointStyle = m_songData.GetPlayerSongStyleList()[m_checkPointID];
        if(Mathf.Abs(checkPointTime - curTime) < m_touchSuccessTIme)
        {
            Debug.Log("检测成功");

            CheckPointChange(0);

            // 播放成功音效
            PlayClickSuccessAudio(iPointStyle);

            m_woodsuccess.SetActive(true);
            Invoke("PlayWoodEffect", m_woodSuccessTime);
        }
        else if ((checkPointTime - curTime) > m_touchSuccessTIme && (checkPointTime - curTime) < m_touchCheckTIme)
        {
            Debug.Log("超前点击");

            CheckPointChange(1);

            PlayClickFailAudio();

            m_iFailTimes++;
        }
        else if ((curTime - checkPointTime) > m_touchSuccessTIme && (curTime - checkPointTime) < m_touchCheckTIme)
        {
            Debug.Log("延迟点击");

            CheckPointChange(2);

            PlayClickFailAudio();
            m_iFailTimes++;
        }
        else
        {
            Debug.Log("无效点击");
            PlayClickInvalidAudio();
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
            //Debug.Log("节点超时,curTime:" + m_Timer.m_curTime);
            CheckPointChange(2);

            PlayClickFailAudio();
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
        isTouch = true;
    }

    /// <summary>
    /// 检查的节点改变 
    /// <param name="iState"> 0成功 1超前失败 2延迟失败</param>
    /// </summary>
    void CheckPointChange(int iState)
    {
        // 处理当前节点
        NPCBehaviour tmp1 = queueRunNpc.Dequeue();
        if (iState == 0)
        {
            tmp1.EndMove();
        }
        else if (iState == 1)
        {
            //Debug.LogWarning("超前失败");
            tmp1.ParabolaMove(new Vector3(-800, 0, 0));
        }
        else if (iState == 2)
        {
            //Debug.LogWarning("延迟失败");
            tmp1.ParabolaMove(new Vector3(800, 0, 0));
        }
        queueCanUseNpc.Enqueue(tmp1);

        if (m_iHandlePointID < m_songData.GetPlayerSongList().Count)
        {
            //Debug.Log("m_iHandlePointID:" + m_iHandlePointID);
            float curTime2 = m_songData.GetPlayerSongList()[m_iHandlePointID]; // 获取需要注册的木块
            int iPointStyle = m_songData.GetPlayerSongStyleList()[m_iHandlePointID];

            NPCBehaviour tmp = queueCanUseNpc.Dequeue();
            if (tmp == null)
            {
                Debug.LogError("queueCanUseNpc empty");
                return;
            }
            tmp.BeginMove(curTime2 - AudioManager.Instance.GetMusicSourceTime(), iPointStyle);
            queueRunNpc.Enqueue(tmp);

            m_iHandlePointID++; // 节点ID递增
        }
        m_checkPointID++;
    }



    /// <summary>
    /// 游戏结束,计算游戏结果
    /// </summary>
    void GameEnd()
    {
        Debug.Log("m_iFailTimes:" + m_iFailTimes);
        Debug.Log("GameEnd,queueRunNpc Count:" + queueRunNpc.Count);
        AudioManager.Instance.StopMusicSingle();
        m_bGameStateRun = false;
        if (m_iMaxStar - m_iFailTimes > 0)
        {
            UIManager.instance.CalculationCurMusicResult(m_iMaxStar - m_iFailTimes);
        }
        else
        {
            UIManager.instance.CalculationCurMusicResult(0);
        }
    }

    /// <summary>
    /// 播放点击成功音效
    /// </summary>
    /// <param name="iPointStyle">I point style.</param>
    void PlayClickSuccessAudio(int iPointStyle)
    {

        if (iPointStyle < 0 || iPointStyle >= m_clickAudios.Length)
        {
            Debug.LogError("iPointStyle is illegal, iPointStyle:" + iPointStyle);
            return;
        }

        if (m_clickAudioSource)
        {
            m_clickAudioSource.Stop();
            m_clickAudioSource.clip = m_clickAudios[iPointStyle];
            m_clickAudioSource.Play();
        }
    }

    /// <summary>
    /// 播放点击失败音效
    /// </summary>
    void PlayClickFailAudio()
    {
        if (m_clickAudioSource)
        {
            m_clickAudioSource.Stop();
            m_clickAudioSource.clip = m_clickFailAudio;
            m_clickAudioSource.Play();
        }
    }

    /// <summary>
    /// 播放点击无效音效
    /// </summary>
    void PlayClickInvalidAudio()
    {
        if (m_clickAudioSource)
        {
            m_clickAudioSource.Stop();
            m_clickAudioSource.clip = m_clickInvalidAudio;
            m_clickAudioSource.Play();
        }
    }


}
