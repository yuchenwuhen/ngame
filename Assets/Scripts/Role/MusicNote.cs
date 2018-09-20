using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MUSICTYPE
{
    Water,
    Wood,
    Chicken,
    Grass,
    Well,
    Backetball,
    Record
}

public class MusicNote : RProps {
    [SerializeField]
    private int m_musicLevel;
    public float m_checkRange = 3f;
    public MUSICTYPE m_musicType;
    [SerializeField]
    private Sprite[] m_collectSprite;
    [SerializeField]
    private string[] m_collectTxt;
    private Transform m_player;
    private SpriteRenderer m_sprite;
    private BoxCollider m_collider;
    [SerializeField]
    private AudioClip m_collectAudio;
    private GameObject m_basketball;
    private bool m_isEnter = false;
    // Use this for initialization
    void Start () {
        m_player = GameObject.FindWithTag("Player").transform;
        m_basketball = GameObject.Find("basketball");
        m_sprite = this.GetComponent<SpriteRenderer>();
        m_collider = this.GetComponent<BoxCollider>();
        m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
    }
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(m_player.position,transform.position)<m_checkRange)
        {
            m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1f);
            m_collider.enabled = true;
            m_isEnter = true;
        }
        else
        {
            if(m_isEnter)
            {
                m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
                m_collider.enabled = false;
                m_isEnter = false;
            }

        }
	}

    public void EnterMusicPlay()
    {
        
        switch (m_musicType)
        {
            case MUSICTYPE.Water:
                Debug.Log("露珠音乐玩法");
                UIManager.instance.ShowUIFade(UIState.Musicmenu2);
                //CollectMusic();
                //GameManager.instance.collectionNumbers.Add(2);
                break;
            case MUSICTYPE.Record:
                Debug.Log("录制音乐玩法");
                UIManager.instance.ShowUIFade(UIState.Musicmenu3);
                break;
            case MUSICTYPE.Wood:
                Debug.Log("砍木头玩法");
                UIManager.instance.ShowUIFade(UIState.Musicmenu1);
                //CollectMusic();
                //GameManager.instance.collectionNumbers.Add(0);
                //GameManager.instance.collectionNumbers.Add(1);
                break;
            case MUSICTYPE.Backetball:
                Debug.Log("篮球音符");
                CollectMusic(3);
                m_basketball.GetComponent<Animator>().SetTrigger("hit");
                GameManager.instance.AddElements(3);
                break;
            case MUSICTYPE.Chicken:
                Debug.Log("鸡叫音符");
                CollectMusic(4);
                GameManager.instance.AddElements(4);
                break;
            case MUSICTYPE.Grass:
                Debug.Log("草音符");
                CollectMusic(5);
                GameManager.instance.AddElements(5);
                break;
            case MUSICTYPE.Well:
                Debug.Log("水井玩法");
                CollectMusic(6);
                GameManager.instance.AddElements(6);
                break;
            default:
                break;
        }
    }

    void CollectMusic(int id)
    {
        if(!GameManager.instance.IsCollected(id))
        {
            CollectionPanel collectionPanel = UIUtility.Instance.GetUI<CollectionPanel>();
            collectionPanel.Appear();
            collectionPanel.ShowCollect(m_collectSprite[0], m_collectTxt[0]);
        }
        AudioManager.Instance.PlayEffectMusic(m_collectAudio);
    }
}
