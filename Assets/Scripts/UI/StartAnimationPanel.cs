using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartAnimationPanel : UIBase
{

    public List<Image> m_animationImg = new List<Image>();
    public List<Text> m_animationTxt = new List<Text>();
    private float m_showTimePerImg = 2f;
    private float m_curTime = 0;
    private int m_curIndex = 0;

    public override void OnAwake()
    {
        base.OnAwake();
       
    }

    public override void Appear()
    {
        base.Appear();
        m_curIndex = 0;
        SetImgByIndex(m_curIndex);
    }

    private void Update()
    {
        if(m_curIndex< m_animationImg.Count)
        {
            m_curTime += Time.deltaTime;
            if (m_curTime > m_showTimePerImg)
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
            }else
            {
                m_animationImg[i].gameObject.SetActive(false);
                m_animationTxt[i].gameObject.SetActive(false);
            }
        }
        m_curIndex++;
        if(m_curIndex>= m_animationImg.Count)
        {
            Invoke("ShowMainMenu", m_showTimePerImg);
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
        fadeTransition.m_FadeOutEnd -= ReceiveChildUIMessage;
        fadeTransition.m_FadeOutEnd += ReceiveChildUIMessage;
        fadeTransition.Appear();
    }

    void ReceiveChildUIMessage(GameObject sender, EventArgs e)
    {
        if (m_curIndex < m_animationImg.Count)
        {
            SetImgByIndex(m_curIndex);
        }

    }


}
