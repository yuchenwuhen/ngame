using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBtn : UIBase
{

    private Button m_btn;

    public override void OnAwake()
    {
        base.OnAwake();
        m_btn = this.GetComponent<Button>();
        m_btn.onClick.AddListener(BackToMenu);
    }

    public override void Appear()
    {
        base.Appear();
        UIManager.instance.m_UICotroller = false;
        this.GetComponent<Image>().enabled = false;
        Invoke("ShowImg", 0.2f);
    }

    void ShowImg()
    {
        this.GetComponent<Image>().enabled = true;
    }

    void BackToMenu()
    {
        UIManager.instance.ShowUIFade(UIState.Bookmenu);
    }
}
