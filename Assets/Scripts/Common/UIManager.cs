using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UIState
{
    Mainmenu,
    Animation,
    Bookmenu,
    Musicmenu1,
    Musicmenu2,
    Musicmenu3,
    MusicResultMenu,
    Scene,
    Dialogue
}
public class UIManager : MonoBehaviour {

    public static UIManager instance = null;

    public bool m_UICotroller { get; set; }

    public UIState m_curState = UIState.Mainmenu;
    public UIState m_preState = UIState.Mainmenu;
    private int m_curMusicLevel;

    public GameObject m_MainIn;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        ResgisterHandler();
        m_MainIn = GameObject.Find("GameMenu");
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

    public void DisappearUIWindow<T>() where T:UIBase
    {
        T ui = UIUtility.Instance.GetUI<T>();
        ui.DisAppear();
    }

    public void ShowUIFade(UIState state)
    {
        switch(state)
        {
            case UIState.Musicmenu1:
            case UIState.Musicmenu2:
            case UIState.Musicmenu3:
                if (m_preState == UIState.Dialogue)
                    return;
                break;
        }
        m_curState = state;
        FadeTransition fadeTransition = UIUtility.Instance.GetUI<FadeTransition>();
        fadeTransition.m_FadeOutEnd -= ReceiveChildUIMessage;
        fadeTransition.m_FadeOutEnd += ReceiveChildUIMessage;
        fadeTransition.Appear();
    }


    /// <summary>
    /// 显示选择窗口
    /// </summary>
    /// <param name="message"></param>
    public void ShowOptionWindow(IMessage message)
    {

        UIChoicePanel ui = UIUtility.Instance.GetUI<UIChoicePanel>();
        ui.m_uiCompleteHandle -= ReceiveChildUIMessage;
        ui.m_uiCompleteHandle += ReceiveChildUIMessage;
        ui.Appear();
    }

    /// <summary>
    /// 获取UI操作回调参数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ReceiveChildUIMessage(GameObject sender, EventArgs e)
    {
        if (m_preState != m_curState)
        {
            switch (m_preState)
            {
                case UIState.Mainmenu:
                    if(m_MainIn)
                        m_MainIn.SetActive(false);
                    break;
                case UIState.Bookmenu:
                case UIState.Scene:
                    DisappearUIWindow<BookPanel>();
                    DisappearUIWindow<SettingBtn>();
                    if (m_MainIn)
                        m_MainIn.SetActive(false);
                    break;
                case UIState.Musicmenu1:
                    DisappearUIWindow<MusicPanel>();
                    DisappearUIWindow<MusicResultPanel>();
                    break;
                case UIState.Musicmenu2:
                    DisappearUIWindow<WaterMusic>();
                    DisappearUIWindow<MusicResultPanel>();
                    break;
                case UIState.Musicmenu3:
                    DisappearUIWindow<DishPanel>();
                    DisappearUIWindow<MusicResultPanel>();
                    break;
                case UIState.Animation:
                    DisappearUIWindow<StartAnimationPanel>();
                    break;
                default:
                    break;
            }
        }

        switch (m_curState)
        {
            case UIState.Mainmenu:
                AudioManager.Instance.PlayMenuMusic(MenuSingleClip.Start);
                m_MainIn.SetActive(true);
                break;
            case UIState.Bookmenu:
                AudioManager.Instance.PlayMenuMusic(MenuSingleClip.Menu);
                ShowUIWindow<BookPanel>();
                break;
            case UIState.Scene:
                AudioManager.Instance.StopStartMusic();
                AudioManager.Instance.PlayMenuMusic(MenuSingleClip.Scene);
                ShowUIWindow<SettingBtn>();
                m_UICotroller = false;
                break;
            case UIState.Musicmenu1:
                AudioManager.Instance.StopStartMusic();
                ShowUIWindow<MusicPanel>();
                break;
            case UIState.Musicmenu2:
                AudioManager.Instance.StopStartMusic();
                ShowUIWindow<WaterMusic>();
                break;
            case UIState.Musicmenu3:
                AudioManager.Instance.StopStartMusic();
                ShowUIWindow<DishPanel>();
                break;
            case UIState.Animation:
                ShowUIWindow<StartAnimationPanel>();
                FadeTransition fadeTransition = UIUtility.Instance.GetUI<FadeTransition>();
                fadeTransition.m_FadeOutEnd -= ReceiveChildUIMessage;
                break;
            default:
                break;
        }
        
        m_preState = m_curState;
    }

    /// <summary>
    /// 显示对话窗口
    /// </summary>
    /// <param name="actor"></param>
    public void ShowDialogueWindow(GameObject player, GameObject actor)
    {
        UIDialoguePanel dialoguePanel = UIUtility.Instance.GetUI<UIDialoguePanel>();
        List<string> txt = actor.GetComponent<RBaseRole>().m_dialogueTxt;
        object[] m_obj = new object[3];
        m_obj[0] = txt;
        m_obj[1] = Camera.main.WorldToScreenPoint(player.transform.position);
        m_obj[2] = Camera.main.WorldToScreenPoint(actor.transform.position);
        dialoguePanel.Appear();
        dialoguePanel.Init(m_obj);
        m_preState = m_curState;
        m_curState = UIState.Dialogue;
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
    /// 设置当前音乐等级 0代表fail,1-3代表相应星级
    /// </summary>
    /// <param name="level"></param>
    public void CalculationCurMusicResult(int level)
    {
        m_curMusicLevel = level;
        MusicResultPanel ui = UIUtility.Instance.GetUI<MusicResultPanel>();
        ui.Appear();
        List<object> levellist = new List<object>();
        levellist.Add(m_curMusicLevel);
        ui.Init(levellist.ToArray());
        if(level>0)
        {
            switch(m_curState)
            {
                case UIState.Musicmenu1:
                    TileManager.Instance.SetMusicLevel(0);
                    break;
                case UIState.Musicmenu2:
                    TileManager.Instance.SetMusicLevel(1);
                    break;
                case UIState.Musicmenu3:
                    TileManager.Instance.SetMusicLevel(2);
                    break;
                default:
                    break;
            }
            //等级大于0
            Debug.Log("当前状态" + m_curState);
        }
    }

    public void SetRecordButton()
    {
        switch (m_curState)
        {
            case UIState.Musicmenu1:
                UIUtility.Instance.GetUI<MusicPanel>().Reset();
                break;
            case UIState.Musicmenu2:
                UIUtility.Instance.GetUI<WaterMusic>().Reset();
                break;
            case UIState.Musicmenu3:
                UIUtility.Instance.GetUI<DishPanel>().Reset();
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

    public void PopPauseWindow(MusicManager manager)
    {
        PausePanel ui = UIUtility.Instance.GetUI<PausePanel>();
        ui.Appear();
        object[] obj = new object[1];
        obj[0] = manager;
        ui.Init(obj);
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
