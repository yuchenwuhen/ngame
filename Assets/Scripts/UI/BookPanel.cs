using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanel : UIBase {

    public GameObject m_image;
    private Button btn_play;
    private Button btn_record;
    private Button m_btnBack;

    private RectTransform m_rectTransform;
    private Button btn_book;
    public Image[] Img_musics;

    //private BookPro m_bookPro;
    //private AutoFlip m_autoFlip;

    public override void OnAwake()
    {
        base.OnAwake();
        m_rectTransform = this.GetComponent<RectTransform>();
        btn_play = transform.Find("btn_play").GetComponent<Button>();
        btn_record = transform.Find("btn_record").GetComponent<Button>();
        btn_record.onClick.AddListener(EnterScene);
        //m_bookPro = transform.Find("BookPro").GetComponent<BookPro>();
        //m_autoFlip = transform.Find("BookPro").GetComponent<AutoFlip>();
        //btn_book = transform.Find("btn_book").GetComponent<Button>();
        //btn_book.onClick.AddListener(() => StartCoroutine(DynamicTansform(1f, new Vector3(100, -200, 0), 1f)));
        btn_play.onClick.AddListener(PlayMulMusic);
        m_btnBack = transform.Find("btn_back").GetComponent<Button>();
        m_btnBack.onClick.AddListener(BackToLogin);
    }

    public override void Appear()
    {
        base.Appear();
        transform.SetSiblingIndex(transform.GetComponentsInChildren<UIBase>().Length-1);
    }

    private void EnterScene()
    {
        AudioManager.Instance.StopAudioMusic();
        UIManager.instance.ShowUIFade(UIState.Scene);
        
    }

    private void BackToLogin()
    {
        UIManager.instance.ShowUIFade(UIState.Mainmenu);
        AudioManager.Instance.StopAudioMusic();
    }

    void PlayMulMusic()
    {
        AudioManager.Instance.PlayMulMusic(TileManager.Instance.GetMusicLevel());
    }

    //void SetEventPage()
    //{
    //    int dif =  m_bookPro.currentPaper - 1;
    //    m_autoFlip.PageFlipTime = m_autoFlip.PageFlipTime / 3;
    //    if (dif>0)
    //    {
    //        m_autoFlip.FlipLeftPage();
    //        m_bookPro.currentPaper =2;
    //    }
    //    else if(dif<0)
    //    {
    //        m_autoFlip.FlipRightPage();
    //    }
    //    m_autoFlip.PageFlipTime = 1f;
    //}

    private IEnumerator DynamicTansform(float scale,Vector3 pos,float time)
    {
        float dur = 0f;
        Vector3 startPos = m_rectTransform.localPosition;
        Vector2 startScale = m_rectTransform.localScale;
        Vector3 endPos = m_rectTransform.localPosition + pos;
        Vector2 endScale = new Vector2(m_rectTransform.localScale.x + scale, m_rectTransform.localScale.y + scale);
        while (dur<=time)
        {
            dur += Time.deltaTime;
            m_rectTransform.localPosition = Vector3.Lerp(startPos, endPos, dur / time);
            m_rectTransform.localScale = Vector2.Lerp(startScale, endScale, dur / time);
            yield return null;
        }
        btn_play.gameObject.SetActive(true);
        btn_record.gameObject.SetActive(true);
    }

    void Fade()
    {
        UIManager.instance.ShowFadeTransition();
    }
}
