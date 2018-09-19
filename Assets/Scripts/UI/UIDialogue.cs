using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour {

    private Transform m_dialogueUp;
    private Transform m_dialogueDown;
    private Text m_uptxt;
    private Text m_downtxt;

    private List<string> m_dialogueTxt = new List<string>();
    private Vector3 m_playerPos;
    private Vector3 m_npcPos;
    private bool m_isStartTalk = false;
    private int m_curIndex = 0;

    // Use this for initialization
    void Awake () {
        m_dialogueUp = transform.Find("UIDialogueUp");
        m_dialogueDown = transform.Find("UIDialogueDown");
        m_uptxt = GameObject.Find("up_txt").GetComponent<Text>();
        m_downtxt = GameObject.Find("down_txt").GetComponent<Text>();
    }

    public void Init(object[] parameters)
    {
        m_dialogueTxt.Clear();
        List<string> dialogue = parameters[0] as List<string>;
        m_playerPos = (Vector3)parameters[1];
        m_npcPos = (Vector3)parameters[2];
        for (int i = 0; i < dialogue.Count; i++)
        {
            m_dialogueTxt.Add(dialogue[i].ToString());
        }
        if (m_dialogueTxt.Count >= 2)
        {
            if(m_playerPos.y > m_npcPos.y)
            {
                m_downtxt.text = m_dialogueTxt[0];
                m_uptxt.text = m_dialogueTxt[1];
            }else
            {
                m_uptxt.text = m_dialogueTxt[0];
                m_downtxt.text = m_dialogueTxt[1];
            }
            
            m_curIndex = 2;
        }
        SetDialoguePos(m_playerPos, m_npcPos);

    }

    private void OnEnable()
    {
        m_isStartTalk = true;
    }

    private void SetDialoguePos(Vector3 m_playerPos, Vector3 m_npcPos)
    {
        Vector3 offset1 = new Vector3(-1.5f, 1.5f, 0);
        Vector3 offset2 = new Vector3(1.7f, 0.5f, 0);
        if (m_playerPos.y > m_npcPos.y)
        {
            m_dialogueUp.position = m_playerPos + offset1;
            m_dialogueDown.position = m_npcPos - offset2;
        }
        else
        {
            m_dialogueUp.position = m_npcPos + offset1;
            m_dialogueDown.position = m_playerPos - offset2;
        }

    }

    private void Update()
    {
        if (m_isStartTalk)
        {

            if (Input.GetMouseButtonDown(0))
            {

                //改变内容
                ChangeContext();
            }
        }
    }

    private void ChangeContext()
    {
        if (m_curIndex < m_dialogueTxt.Count)
        {
            switch (m_curIndex % 2)
            {
                case 0:
                    if (m_playerPos.y > m_npcPos.y)
                    {
                        m_downtxt.text = m_dialogueTxt[m_curIndex];
                    }
                    else
                    {
                        m_uptxt.text = m_dialogueTxt[m_curIndex];
                    }
                    break;
                case 1:
                    if (m_playerPos.y > m_npcPos.y)
                    {
                        m_uptxt.text = m_dialogueTxt[m_curIndex];
                    }
                    else
                    {
                        m_downtxt.text = m_dialogueTxt[m_curIndex];
                    }
                    break;
                default:
                    break;
            }
            m_curIndex++;
        }
        else
        {
            m_isStartTalk = false;
            Invoke("DisAppear", 0.1f);
        }
    }

    public void DisAppear()
    {
        UIManager.instance.m_UICotroller = false;
        UIManager.instance.m_preState = UIState.Scene;
        UIManager.instance.m_curState = UIState.Scene;
        gameObject.SetActive(false);
    }
}
