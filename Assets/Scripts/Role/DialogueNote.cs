using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNote : MonoBehaviour {

    public float m_checkRange = 3f;
    private Transform m_player;
    private SpriteRenderer m_sprite;
    private BoxCollider m_collider;
    public GameObject m_parent;
    private Animator m_animator;

    private bool m_isEnter = false;
    public float m_changeTime = 10f;
    private float m_curTime = 0;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindWithTag("Player").transform;
        m_sprite = this.GetComponent<SpriteRenderer>();
        m_collider = this.GetComponent<BoxCollider>();
        m_collider.enabled = false;
        m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
        if(m_parent.GetComponent<Animator>())
        {
            m_animator = m_parent.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance.m_curState == UIState.Dialogue)
            return;
        if (Vector3.Distance(m_player.position, transform.position) < m_checkRange)
        {
            m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1f);
            m_collider.enabled = true;
            m_isEnter = true;
        }
        else
        {
            if (m_isEnter)
            {
                m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
                m_collider.enabled = false;
                m_isEnter = false;
            }
            m_curTime += Time.deltaTime;
            if (m_curTime > m_changeTime)
            {
                m_curTime = 0;
                if (m_animator)
                {
                    m_animator.SetBool("Dothing", true);
                    Invoke("SetIdle", 2f);
                }

            }
        }
    }

    void SetIdle()
    {
        m_animator.SetBool("Dothing", false);
    }

    public void EnterDialoguePlay()
    {

        if (m_parent != null && !UIManager.instance.m_UICotroller)
        {
            UIManager.instance.ShowDialogueWindow(m_player.gameObject, m_parent);
            m_collider.enabled = false;
            m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
        }
    }
}
