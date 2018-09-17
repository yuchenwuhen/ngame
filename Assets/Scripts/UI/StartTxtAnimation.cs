using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTxtAnimation : MonoBehaviour {
    [SerializeField]
    private List<string> m_txtList;
    [SerializeField]
    private float m_showTime = 1f;
    private Text m_curTxt;
    private int m_curIndex = 0;
	// Use this for initialization
	void Start () {
        m_curIndex = 0;
        m_curTxt = this.GetComponent<Text>();
        InvokeRepeating("ChangeTxt", 0, m_showTime);
	}
	
	void ChangeTxt()
    {
        if(m_curIndex<m_txtList.Count)
        {
            m_curTxt.text = m_txtList[m_curIndex];
            m_curIndex++;
        }
    }
}
