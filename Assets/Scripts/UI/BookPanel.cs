using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanel : UIBase {

    public GameObject m_image;
    private Button btn_play;
    private Button btn_record;
    private Button m_btnBack;
    private bool m_isPlaying = false;

    private RectTransform m_rectTransform;
    private Button btn_book;
    public Image[] Img_musics;
    public Sprite[] btnPlay_sprites;
    public Image[] m_leftImage;

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
        for(int i=0;i< Img_musics.Length;i++)
        {
            Img_musics[i].enabled = false;
        }
        for (int i = 0; i < m_leftImage.Length; i++)
        {
            m_leftImage[i].enabled = false;
        }
    }

    public override void Appear()
    {
        base.Appear();
        transform.SetSiblingIndex(transform.GetComponentsInChildren<UIBase>().Length-1);

        m_isPlaying = false;
        SetBtnplaySprite();
        SetImgmusic();
    }

    private void EnterScene()
    {
        m_isPlaying = !m_isPlaying;
        AudioManager.Instance.StopAudioMusic();
        UIManager.instance.ShowUIFade(UIState.Scene);
    }

    private void BackToLogin()
    {
        UIManager.instance.ShowUIFade(UIState.Mainmenu);
        AudioManager.Instance.StopAudioMusic();
        AudioManager.Instance.StopStartMusic();
    }

    void PlayMulMusic()
    {
        m_isPlaying = !m_isPlaying;
        SetBtnplaySprite();
        if(m_isPlaying)
        {
            AudioManager.Instance.StopStartMusic();
            AudioManager.Instance.PlayMulMusic(TileManager.Instance.GetMusicLevel());
        }
        else
        {
            AudioManager.Instance.StopAudioMusic();
            AudioManager.Instance.PlayMenuMusic(MenuSingleClip.Menu);
        }
    }

    void SetBtnplaySprite()
    {
        if(!m_isPlaying)
        {
            btn_play.GetComponent<Image>().sprite = btnPlay_sprites[0];
        }
        else
        {
            btn_play.GetComponent<Image>().sprite = btnPlay_sprites[1];
        }
    }

    /// <summary>
    /// 设置音谱灰度
    /// </summary>
    public void SetImgmusic()
    {
        int[] level = TileManager.Instance.GetMusicLevel();
        for (int i=0;i< level.Length;i++)
        {
            Img_musics[level[i]].enabled = true;
        }
        int curlevel = TileManager.Instance.GetCurLevel();
        if(curlevel>=0)
        {
            m_leftImage[curlevel].enabled = true;
            m_leftImage[curlevel].transform.SetAsLastSibling();
        }
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
