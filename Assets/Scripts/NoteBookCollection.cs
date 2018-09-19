using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteBookCollection : MonoBehaviour {
    public Image[] icons;
	// Use this for initialization
	void Start () {
        foreach(var img in icons)
        {
            img.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetIconSprite()
    {
        for (int i = 0; i < GameManager.instance.GetCollectionLength(); i++)
        {
            int num = GameManager.instance.GetElement(i);
            icons[num].enabled = true;
        }
    }
}
