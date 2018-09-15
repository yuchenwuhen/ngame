using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialoguePanel : UIBase {
    public Sprite[] m_sprites;
    private Text m_bubble_txt;
    private Text m_player_txt;
    private RectTransform bubble_img;
    private RectTransform player_img;

    private List<string> m_dialogueTxt = new List<string>();

    private bool m_isStartTalk = false;
    private int m_curIndex = 0;
    private Vector3 m_playerPos;
    private Vector3 m_npcPos;

    public override void OnAwake()
    {
        base.OnAwake();
        m_bubble_txt = GameObject.Find("bubble_txt").GetComponent<Text>();
        m_player_txt = GameObject.Find("player_txt").GetComponent<Text>();
        bubble_img = GameObject.Find("bubble_img").GetComponent<RectTransform>();
        player_img = GameObject.Find("player_img").GetComponent<RectTransform>();
    }


    public override void Init(object[] parameters)
    {
        base.Init(parameters);
        m_dialogueTxt.Clear();
        List<string> dialogue = parameters[0] as List<string>;
        m_playerPos = (Vector3)parameters[1];
        m_npcPos = (Vector3)parameters[2];
        for (int i=0;i< dialogue.Count; i++)
        {
            m_dialogueTxt.Add(dialogue[i].ToString());
        }
        if(m_dialogueTxt.Count>=2)
        {
            m_bubble_txt.text = m_dialogueTxt[0];
            m_player_txt.text = m_dialogueTxt[1];
            m_curIndex = 2;
        }
        SetDialoguePos(m_playerPos,m_npcPos);
        
    }

    private void SetDialoguePos(Vector3 m_playerPos, Vector3 m_npcPos)
    {
        Vector3 offset1 = new Vector3(-80,150,0);
        Vector3 offset2 = new Vector3(80, 40, 0);
        if (m_playerPos.y> m_npcPos.y)
        {
            bubble_img.GetComponent<Image>().sprite = m_sprites[0];
            player_img.GetComponent<Image>().sprite = m_sprites[1];
            bubble_img.position = m_npcPos-offset2;
            player_img.position = m_playerPos+offset1;
        }else
        {
            bubble_img.GetComponent<Image>().sprite = m_sprites[1];
            player_img.GetComponent<Image>().sprite = m_sprites[0];
            bubble_img.position = m_npcPos + offset1;
            player_img.position = m_playerPos - offset2;
        }
        
    }

    public override void OnAppear()
    {
        base.OnAppear();
        m_isStartTalk = true;

    }

    private void Update()
    {
        if(m_isStartTalk)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //改变内容
                ChangeContext();
            }
        }
    }

    private  void ChangeContext()
    {
        if(m_curIndex< m_dialogueTxt.Count)
        {
            switch(m_curIndex%2)
            {
                case 0:
                    m_bubble_txt.text = m_dialogueTxt[m_curIndex];
                    break;
                case 1:
                    m_player_txt.text = m_dialogueTxt[m_curIndex];
                    break;
                default:
                    break;
            }
            m_curIndex++;
        }else
        {
            m_isStartTalk = false;
            DisAppear();
        }
    }
}
