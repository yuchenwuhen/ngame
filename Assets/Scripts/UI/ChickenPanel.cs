using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenPanel : UIBase {

    public AudioClip[] m_audioClip;          //背景音乐信息

    private bool m_bGameStateRun = false;     // 场景是否正在运行

    private void Update()
    {
        if (!m_bGameStateRun)
        {
            Debug.Log("Game pause");
            return;
        }
    }
}
