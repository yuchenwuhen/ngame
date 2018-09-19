using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMapTeachPanel : UIBase {

    private Button m_closeBtn;

    public override void OnAwake()
    {
        base.OnAwake();
        m_closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        m_closeBtn.onClick.AddListener(SetUIController);
    }

    private void SetUIController()
    {
        UIManager.instance.m_UICotroller = false;
        DisAppear();
       
    }
}
