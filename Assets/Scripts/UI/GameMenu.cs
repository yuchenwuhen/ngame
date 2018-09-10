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
    }
	
    /// <summary>
    /// 开始游戏
    /// </summary>
	void StartGame()
    {
        UIManager.instance.ShowFadeTransition();
    }
}
