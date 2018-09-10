using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChoicePanel : UIBase {

    private Button m_btnOptionA;
    private Button m_btnOptionB;

    public event UICompleteHandle m_uiCompleteHandle = null;
    private int itemFlag = 0;                   //标记当前List显示的个数

    public override void OnAwake()
    {
        base.OnAwake();
        m_btnOptionA = GameObject.Find("OptionA").GetComponent<Button>();
        m_btnOptionB = GameObject.Find("OptionB").GetComponent<Button>();
        m_btnOptionA.onClick.AddListener(ClickOptionA);
        m_btnOptionB.onClick.AddListener(ClickOptionB);
        m_uiType = UIType.ChoiceWindow;
    }

    /// <summary>
    /// 初始化弹框列表
    /// </summary>
    /// <param name="parameters"></param>
    public override void Init(object[] parameters)
    {
        base.Init(parameters);

    }

    private void ClickOptionA()
    {
        EventArgs eventArgs = new EventArgs("OptionA");
        m_uiCompleteHandle(this.gameObject, eventArgs);
        DisAppear();
    }

    private void ClickOptionB()
    {
        EventArgs eventArgs = new EventArgs("OptionB");
        m_uiCompleteHandle(this.gameObject, eventArgs);
        DisAppear();
    }

    public override void DisAppear()
    {
        base.DisAppear();
    }

}
