using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : UIBase {

    private Button m_startButton;
	// Use this for initialization
	void Start () {
        m_startButton = transform.Find("btnstart").GetComponent<Button>();
        m_startButton.onClick.AddListener(StartGame);
        AudioManager.Instance.StopStartMusic();
        AudioManager.Instance.PlayMenuMusic(MenuSingleClip.Start);
       
    }
	
    /// <summary>
    /// 开始游戏
    /// </summary>
	void StartGame()
    {
        UIManager.instance.ShowUIFade(UIState.Animation);

        Invoke("UnShow",5f);
    }
    public void UnShow()
    {
        this.gameObject.SetActive(false);
    }
}
