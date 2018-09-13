using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialoguePanel : UIBase {

    private Text m_txt;
    private Button btn_next;

    private List<string> m_dialogueTxt = new List<string>();

    public override void OnAwake()
    {
        base.OnAwake();
        m_txt = GameObject.Find("DiaText").GetComponent<Text>();
        btn_next = transform.Find("btn_next").GetComponent<Button>();
        btn_next.onClick.AddListener(()=>StartDialogue());
    }

    public override void Init(object[] parameters)
    {
        base.Init(parameters);
        for(int i=0;i<parameters.Length;i++)
        {
            m_dialogueTxt.Add(parameters[i].ToString());
        }
    }

    IEnumerator StartDialogue()
    {
        //int i = 0;
        yield return null;
    }
}
