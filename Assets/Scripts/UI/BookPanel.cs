using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanel : UIBase {

    private Button btn_play;
    private Button btn_record;
    private Button m_btnBack;
    private Button m_btnCreate;
    public Button m_send;
    private bool m_isPlaying = false;

    private RectTransform m_rectTransform;
    private Button btn_book;
    
    public GameObject[] Img_musics;
    public Sprite[] btnPlay_sprites;
    public Image[] m_leftImage;
    private int[] m_openAudioList;
    public List<Button> btn_volume = new List<Button>();
    public Sprite[] m_openAudioSprite;

    //private BookPro m_bookPro;
    //private AutoFlip m_autoFlip;
    public GameObject CollectionPanel;
    public override void OnAwake()
    {
        base.OnAwake();
        m_rectTransform = this.GetComponent<RectTransform>();
        btn_play = transform.Find("btn_play").GetComponent<Button>();
        btn_record = transform.Find("btn_record").GetComponent<Button>();
        btn_record.onClick.AddListener(EnterScene);
        m_btnCreate = transform.Find("btn_create").GetComponent<Button>();
        m_btnCreate.onClick.AddListener(EnterLevel3);
        m_send.gameObject.SetActive(false);
        //m_bookPro = transform.Find("BookPro").GetComponent<BookPro>();
        //m_autoFlip = transform.Find("BookPro").GetComponent<AutoFlip>();
        //btn_book = transform.Find("btn_book").GetComponent<Button>();
        //btn_book.onClick.AddListener(() => StartCoroutine(DynamicTansform(1f, new Vector3(100, -200, 0), 1f)));
        btn_play.onClick.AddListener(PlayMulMusic);
        m_btnBack = transform.Find("btn_back").GetComponent<Button>();
        m_btnBack.onClick.AddListener(BackToLogin);
        for(int i=0;i< Img_musics.Length;i++)
        {
            Img_musics[i].SetActive(false);
        }
        for(int i=0;i<btn_volume.Count;i++)
        {
            btn_volume[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < m_leftImage.Length; i++)
        {
            m_leftImage[i].enabled = false;
        }
        m_openAudioList = new int[3] { 0, 0, 0 };
        btn_volume[0].onClick.AddListener(delegate() { SetPlayVolume(0); });
        btn_volume[1].onClick.AddListener(delegate () { SetPlayVolume(1); });
        btn_volume[2].onClick.AddListener(delegate () { SetPlayVolume(2); });
        TileManager.Instance.SetSeen();
    }

    public override void Appear()
    {
        base.Appear();
        //transform.SetSiblingIndex(transform.GetComponentsInChildren<UIBase>().Length-1);

        m_isPlaying = false;
        SetBtnplaySprite();
        SetImgmusic();
        //更新主界面
        CollectionPanel.GetComponent<NoteBookCollection>().SetIconSprite();
    }
    //开始录制进入场景
    private void EnterScene()
    {
        m_isPlaying = !m_isPlaying;
        AudioManager.Instance.StopAudioMusic();
        UIManager.instance.ShowUIFade(UIState.Scene);
        Invoke("ShowTeachPanel",4f);
 
    }

    private void EnterLevel3()
    {
        AudioManager.Instance.StopAudioMusic();
        UIManager.instance.ShowUIFade(UIState.Musicmenu3);
    }

    private void ShowTeachPanel()
    {
        BigMapTeachPanel bigMapTeach =  UIUtility.Instance.GetUI<BigMapTeachPanel>();
        bigMapTeach.Appear();

    }

    private void SetPlayVolume(int i)
    {
        if(m_openAudioList[i]==0)
        {
            m_openAudioList[i] = 1;
        }else
        {
            m_openAudioList[i] = 0;
        }
        btn_volume[i].gameObject.GetComponent<Image>().sprite = m_openAudioSprite[m_openAudioList[i]];
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
            int[] level = TileManager.Instance.GetMusicLevel();
            List<int> levelnew = new List<int>();
            for(int i=0;i<level.Length;i++)
            {
                if(m_openAudioList[level[i]]==0)
                {
                    levelnew.Add(level[i]);
                }
            }
            AudioManager.Instance.PlayMulMusic(levelnew.ToArray());
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
        if(level.Length>2)
        {
            m_send.gameObject.SetActive(true);
        }
        for (int i=0;i< level.Length;i++)
        {
            Img_musics[level[i]].SetActive(true);
            btn_volume[level[i]].gameObject.SetActive(true);
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
