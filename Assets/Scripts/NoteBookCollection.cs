using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteBookCollection : MonoBehaviour {
    public Image[] icons;
    public Sprite[] sprites;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetIconSprite()
    {
        for (int i = 0; i < GameManager.instance.collectionNumbers.Count; i++)
        {
            int num = GameManager.instance.collectionNumbers[i];
            Debug.Log(num);
            switch (num)
            {
                case 0:
                    icons[0].sprite = sprites[0];
                    break;
                case 1:
                    icons[1].sprite = sprites[1];
                    break;
                default:
                    break;
            }
        }
    }
}
