using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MUSICTYPE
{
    Water,
    Wood,
    Basketball
}

public class MusicNote : RProps {
    [SerializeField]
    private int m_musicLevel;
    public float m_checkRange = 3f;
    public MUSICTYPE m_musicType;
    private Transform m_player;
    private SpriteRenderer m_sprite;
    private BoxCollider m_collider;
    [SerializeField]
    private AudioClip m_efxAudio;

    private bool m_isEnter = false;
    // Use this for initialization
    void Start () {
        m_player = GameObject.FindWithTag("Player").transform;
        m_sprite = this.GetComponent<SpriteRenderer>();
        m_collider = this.GetComponent<BoxCollider>();
        m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
    }
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(m_player.position,transform.position)<m_checkRange)
        {
            m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1f);
            if(m_efxAudio && !m_isEnter)
                AudioManager.Instance.PlayEffectMusic(m_efxAudio);
            m_collider.enabled = true;
            m_isEnter = true;
        }
        else
        {
            if(m_isEnter)
            {
                m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
                m_collider.enabled = false;
                if (m_efxAudio&&m_isEnter)
                    AudioManager.Instance.StopEffectMusic();
                m_isEnter = false;
            }

        }
	}

    public void EnterMusicPlay()
    {
        switch(m_musicType)
        {
            case MUSICTYPE.Water:
                Debug.Log("水井音乐玩法");
                break;
            case MUSICTYPE.Basketball:
                Debug.Log("篮球音乐玩法");
                break;
            case MUSICTYPE.Wood:
                UIManager.instance.ShowUIFade(UIState.Musicmenu1);
                break;
            default:
                break;
        }
        if (m_efxAudio)
            AudioManager.Instance.StopEffectMusic();
    }
}
