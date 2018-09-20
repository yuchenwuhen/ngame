using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public List<int> collectionList;         //玩家已收集音效数组
    public GameObject iconAnimPrefab;        //收集的音效图标动画预制体，用于动态实例化
    public Sprite[] sprites;
    private float timer = 0;
    // Use this for initialization
    void Start () {
        //删除以下三行，并给collectionList赋值
        collectionList.Add(0);
        collectionList.Add(3);
        collectionList.Add(5);
        iconAnimPrefab = Resources.Load("Prefab/moveAnim") as GameObject;
        //播放结局音效

    }
	
	// Update is called once per frame
	void Update () {
		if(collectionList.Count > 0)
        {
            //计时
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                //生成
                GameObject iconAnim = Instantiate(iconAnimPrefab, this.transform.parent);
                iconAnim.transform.Find("jumpAnim").GetComponent<Image>().sprite = sprites[collectionList[0]];
                //清理
                if (collectionList.Count > 0)
                {
                    collectionList.RemoveAt(0);
                }
                Destroy(iconAnim, 10f);
                timer = 0;
            }
        }
	}

}
