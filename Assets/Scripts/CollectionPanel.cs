using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPanel : UIBase {
    private Image iconImage;
    private Text testPanel;

    public override void OnAwake()
    {
        base.OnAwake();
        iconImage = transform.Find("coIcon").GetComponent<Image>();
        testPanel = transform.Find("coName").GetComponent<Text>();
    }

    public override void Appear()
    {
        base.Appear();
        UIManager.instance.m_UICotroller = false;
    }

    public void ShowCollect(Sprite icon,string txt)
    {
        iconImage.sprite = icon;
        testPanel.text = txt;
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        this.GetComponent<Animator>().SetTrigger("open");
    }
    
}
