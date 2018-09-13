using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.
	
public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public bool m_IsEnterMenu = false;
    public bool m_IsEnterMusic = false;

	//Awake is always called before any Start functions
	void Awake()
	{
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);	
			
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
			
			
	}

    private void Start()
    {
        if (m_IsEnterMenu)
        {
            UIManager.instance.ShowUIWindow<GameMenu>();
           // UIManager.instance
        }
        if (m_IsEnterMusic)
            BackToScene();
    }

    public void BackToScene()
    {
        UIManager.instance.ShowUIFade(UIState.Musicmenu);
    }



}


