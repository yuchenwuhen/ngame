using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : UIBase {

    private Button m_startButton;
    private InputField m_userInput;

	// Use this for initialization
	void Start () {
        m_startButton = transform.Find("btnstart").GetComponent<Button>();
        m_startButton.onClick.AddListener(StartGame);
        m_userInput = transform.Find("userInput").GetComponent<InputField>();
        m_userInput.onEndEdit.AddListener(SaveUserData);
    }
	
    /// <summary>
    /// 开始游戏
    /// </summary>
	void StartGame()
    {
        UIManager.instance.ShowFadeTransition();
    }

    void SaveUserData(string name)
    {
        PlayerPrefs.SetString("username",name);
    }
}
