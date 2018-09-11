using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MUSICTYPE
{
    Water,
}

public class MusicNote : RProps {
    [SerializeField]
    private int m_musicLevel;
    public float m_checkRange = 3f;
    public MUSICTYPE m_musicType;
    private Transform m_player;
    private SpriteRenderer m_sprite;
    private BoxCollider m_collider;

    private bool m_isEnter = false;
    // Use this for initialization
    void Start () {
        m_player = GameObject.FindWithTag("Player").transform;
        m_sprite = this.GetComponent<SpriteRenderer>();
        m_collider = this.GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
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
            }

        }
	}

    public void EnterMusicPlay()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
