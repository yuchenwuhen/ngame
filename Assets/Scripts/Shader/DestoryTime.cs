using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTime : MonoBehaviour {
    public float destroyTime = 1f;
    // Use this for initialization
    void Start () {
        Destroy(transform.gameObject, destroyTime);
    }
	
}
