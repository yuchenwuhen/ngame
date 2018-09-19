using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementsOnTouch : MonoBehaviour {
    public GameObject collectionPanel;
    public GameObject hitFx;
    public AudioClip hitSound;
    public GameObject hitPoint;
    public bool hasCollected;
	// Use this for initialization
	void Start () {
        hasCollected = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDown()
    {
        OnTouch();
    }
    public void OnTouch()
    {
        string myName = this.gameObject.name;
        GameObject fx = Instantiate<GameObject>(hitFx, transform.position, hitPoint.transform.rotation);
        Destroy(fx, 0.668f);
        AudioManager.Instance.PlayEffectMusic(hitSound);
        Debug.Log(myName);
        if (hasCollected == false)
        {
            hasCollected = true;
            switch (myName)
            {
                case "chicken1":
                    collectionPanel.GetComponent<Animator>().SetTrigger("open");
                    collectionPanel.GetComponent<CollectionPanel>().iconImage.sprite = collectionPanel.GetComponent<CollectionPanel>().icons[0];
                    collectionPanel.GetComponent<CollectionPanel>().testPanel.text = collectionPanel.GetComponent<CollectionPanel>().texts[0];
                    GameManager.instance.collectionNumbers.Add(0);
                    break;
                case "chicken2":
                    collectionPanel.GetComponent<Animator>().SetTrigger("open");
                    collectionPanel.GetComponent<CollectionPanel>().iconImage.sprite = collectionPanel.GetComponent<CollectionPanel>().icons[1];
                    collectionPanel.GetComponent<CollectionPanel>().testPanel.text = collectionPanel.GetComponent<CollectionPanel>().texts[1];
                    GameManager.instance.collectionNumbers.Add(1);
                    break;
                default:
                    break;
            }
        }
    

    }
}
