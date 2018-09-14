using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMusicTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(AudioSettings.outputSampleRate);

        //AudioSource audio;
        //audio.PlayScheduled();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(AudioSettings.dspTime);
        
    }
}
