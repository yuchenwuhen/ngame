using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAnimationPanel : UIBase
{

    public List<Image> m_animationImg = new List<Image>();
    public List<Text> m_animationTxt = new List<Text>();
    [SerializeField]
    private float m_showTimePerImg = 4f;
    private float m_curTime = 0;
    private int m_curIndex = 0;
    private float m_showTime1;
    private Button m_skipBtn;

    private bool m_controller = true;

    public override void OnAwake()
    {
        base.OnAwake();
        m_skipBtn = GameObject.Find("SkipBtn").GetComponent<Button>();
        m_skipBtn.onClick.AddListener(SkipAniamtion);
    }
    public override void OnStart()
    {
        base.OnStart();
    }
    public override void Appear()
    {
        base.Appear();
        m_controller = true;
        m_curIndex = 0;
        m_showTime1 = m_showTimePerImg;
        SetImgByIndex(m_curIndex);
        AudioManager.Instance.StopStartMusic();
        AudioManager.Instance.PlayMenuMusic(MenuSingleClip.Start);
    }

    void SkipAniamtion()
    {
        m_controller = false;
        ShowMainMenu();
    }

    private void Update()
    {
        if(m_curIndex< m_animationImg.Count)
        {
            m_curTime += Time.deltaTime;
            if (m_curTime > m_showTime1)
            {
                m_curTime = 0;
                ShowFade();
            }
        }
    }

    void SetImgByIndex(int index)
    {
        for(int i=0;i<m_animationImg.Count; i++)
        {
            if(i==index)
            {
                m_animationImg[i].gameObject.SetActive(true);
                m_animationTxt[i].gameObject.SetActive(true);
                if(i==0||i==m_animationImg.Count-1)
                {
                    m_showTime1 = m_showTimePerImg;
                }else
                {
                    m_showTime1 = m_showTimePerImg + 2f;
                }
            }else
            {
                m_animationImg[i].gameObject.SetActive(false);
                m_animationTxt[i].gameObject.SetActive(false);
            }
        }
        m_curIndex++;
        if(m_curIndex>= m_animationImg.Count && m_controller)
        {
            Invoke("ShowMainMenu", m_showTimePerImg-1f);
        }
    }

    void ShowMainMenu()
    {
        UIManager.instance.ShowUIFade(UIState.Bookmenu);
        AudioManager.Instance.StopStartMusic();
    }

    void ShowFade()
    {
        FadeTransition fadeTransition = UIUtility.Instance.GetUI<FadeTransition>();
        fadeTransition.m_FadeOutEnd -= ReceiveMessage;
        fadeTransition.m_FadeOutEnd += ReceiveMessage;
        fadeTransition.Appear();
    }

    void ReceiveMessage(GameObject sender, EventArgs e)
    {
        if (m_curIndex < m_animationImg.Count)
        {
            SetImgByIndex(m_curIndex);
            Debug.Log(m_curIndex);
        }

    }


}
