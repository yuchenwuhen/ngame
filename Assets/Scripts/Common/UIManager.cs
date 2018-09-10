using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;
    public bool ISstart = false;

    public enum UIState
    {
        Mainmenu,
        Bookmenu,
        Scene
    }

    private UIState m_curState = UIState.Mainmenu;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        ResgisterHandler();
        //ShowOptionWindow(null);
        if(ISstart)
        ShowUIWindow<GameMenu>();
    }

    private void Update()
    {

    }

    /// <summary>
    /// 显示无回调功能UI窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ShowUIWindow<T>() where T:UIBase
    {
        T ui = UIUtility.Instance.GetUI<T>();
        ui.Appear();
    }

    /// <summary>
    /// 显示拾取窗口
    /// </summary>
    /// <param name="message"></param>
    public void ShowOptionWindow(IMessage message)
    {

        UIChoicePanel ui = UIUtility.Instance.GetUI<UIChoicePanel>();
        ui.m_uiCompleteHandle -= ReceiveChildUIMessage;
        ui.m_uiCompleteHandle += ReceiveChildUIMessage;
        ui.Appear();
    }

    public void ShowDialogueWindow(GameObject actor)
    {
        UIDialoguePanel dialoguePanel = UIUtility.Instance.GetUI<UIDialoguePanel>();
        List<string> txt = actor.GetComponent<RBaseRole>().m_dialogueTxt;
        dialoguePanel.Appear();
        dialoguePanel.Init(txt.ToArray());
    }

    /// <summary>
    /// 显示过渡动画
    /// </summary>
    public void ShowFadeTransition()
    {
        FadeTransition fadeTransition = UIUtility.Instance.GetUI<FadeTransition>();
        fadeTransition.m_FadeOutEnd -= ReceiveChildUIMessage;
        fadeTransition.m_FadeOutEnd += ReceiveChildUIMessage;
        fadeTransition.Appear();
    }

    /// <summary>
    /// 获取UI操作回调参数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ReceiveChildUIMessage(GameObject sender,EventArgs e)
    {
        UIBase uiBase = sender.GetComponent<UIBase>();
        switch(uiBase.GetUIType())
        {
            case UIType.ChoiceWindow:
                DealChoiceWindowCallback(e);
                break;
            case UIType.FadeWindow:
                DealFadeWindowCallback(e);
                break;
            default:
                break;
        }
        
    }

    private void DealFadeWindowCallback(EventArgs e)
    {
        switch(m_curState)
        {
            case UIState.Mainmenu:
                GameMenu gameMenu = UIUtility.Instance.GetUI<GameMenu>();
                gameMenu.DisAppear();
                ShowUIWindow<BookPanel>();
                m_curState = UIState.Bookmenu;
                break;
            case UIState.Bookmenu:
                BookPanel bookPanel = UIUtility.Instance.GetUI<BookPanel>();
                bookPanel.DisAppear();
                m_curState = UIState.Scene;
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// 处理ChoiceUI回调消息
    /// </summary>
    /// <param name="e"></param>
    private void DealChoiceWindowCallback(EventArgs e)
    {
        if (e.m_data.Equals("OptionA"))
        {
            Debug.Log("UI 点击A");

        }
        else if (e.m_data.Equals("OptionB"))
        {
            Debug.Log("UI 点击B");
        }
    }

    /// <summary>
    /// 接受消息
    /// </summary>
    /// <param name="message"></param>
    void ReceiveMessage(IMessage message)
    {
        Message mes = message as Message;
        switch(mes.EventCode)
        {
            default:
                break;
        }
    }

    /// <summary>
    /// 注册消息对应方法
    /// </summary>
    void ResgisterHandler()
    {
        MessageDispatcher.AddListener(MessageDefines.MsgType_UI, ReceiveMessage);
    }
}
