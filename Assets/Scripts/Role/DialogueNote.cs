using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNote : MonoBehaviour {

    public float m_checkRange = 3f;
    private Transform m_player;
    private SpriteRenderer m_sprite;
    private BoxCollider m_collider;
    public GameObject m_parent;

    private bool m_isEnter = false;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindWithTag("Player").transform;
        m_sprite = this.GetComponent<SpriteRenderer>();
        m_collider = this.GetComponent<BoxCollider>();
        m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
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

        }
    }

    public void EnterDialoguePlay()
    {

        if (m_parent != null)
        {
            UIManager.instance.ShowDialogueWindow(m_player.gameObject, m_parent);
        }
    }
}
