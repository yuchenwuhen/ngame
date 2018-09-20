using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public List<int> collectionList;
    public GameObject iconAnimPrefab;
    public Sprite[] sprites;
    float timer = 0;
    // Use this for initialization
    void Start () {
        collectionList.Add(0);
        collectionList.Add(3);
        collectionList.Add(5);
        //InitAnimation(collectionList);
    }
	
	// Update is called once per frame
	void Update () {
		if(collectionList.Count > 0)
        {
            timer += Time.deltaTime;
            Debug.Log(collectionList.Count);
            if (timer >= 2)
            {
                GameObject iconAnim = Instantiate(iconAnimPrefab, this.transform.parent);
                iconAnim.transform.Find("jumpAnim").GetComponent<Image>().sprite = sprites[collectionList[0]];
                if(collectionList.Count > 0)
                {
                    collectionList.RemoveAt(0);
                }
                
                timer = 0;
            }
        }
	}

}
