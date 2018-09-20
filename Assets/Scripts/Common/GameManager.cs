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
    //0,1表示大木材 小木材，2表示露珠，3表示篮球
    //4鸡叫 5草丛 6水井
    private List<int> collectionNumbers = new List<int>();
    public  GameObject m_Tile;

    public Sprite[] m_collectSprite;
    public string[] m_collectTxt;
    public AudioClip[] m_collectAudio;

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
        }
        if (m_IsEnterMusic)
            BackToScene();
    }

    public void BackToScene()
    {
        UIManager.instance.ShowUIFade(UIState.Musicmenu1);
        Debug.Log("jiaoxuetishi ");
    }

    public void Collectlevel2Music()
    {
        if (!IsCollected(2))
        {
            AddElements(2);
            CollectionPanel collectionPanel = UIUtility.Instance.GetUI<CollectionPanel>();
            collectionPanel.Appear();
            collectionPanel.ShowCollect(m_collectSprite[2], m_collectTxt[2]);
            AudioManager.Instance.PlayEffectMusic(m_collectAudio[2]);
        }
    }

    public void CollectLevel1Music()
    {
        if (!(IsCollected(1)||IsCollected(0)))
        {
            AddElements(0);
            AddElements(1);
            CollectionPanel collectionPanel = UIUtility.Instance.GetUI<CollectionPanel>();
            collectionPanel.Appear();
            collectionPanel.ShowCollect(m_collectSprite[0], m_collectTxt[0]);
            AudioManager.Instance.PlayEffectMusic(m_collectAudio[0]);
            Invoke("PlayAgain", 2f);
        }
    }

    void PlayAgain()
    {
        CollectionPanel collectionPanel = UIUtility.Instance.GetUI<CollectionPanel>();
        collectionPanel.Appear();
        collectionPanel.ShowCollect(m_collectSprite[1], m_collectTxt[1]);
        AudioManager.Instance.PlayEffectMusic(m_collectAudio[1]);
    }

    /// <summary>
    /// 添加收集元素
    /// </summary>
    /// <param name="id"></param>
    public void AddElements(int id)
    {
        collectionNumbers.Add(id);
    }

    public int GetElement(int id)
    {
        return collectionNumbers[id];
    }

    public bool IsCollected(int id)
    {
        if(collectionNumbers.Contains(id))
        {
            return true;
        }
        return false;
    }

    public int GetCollectionLength()
    {
        return collectionNumbers.Count;
    }

}


