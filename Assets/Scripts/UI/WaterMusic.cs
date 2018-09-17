using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMusic : UIBase {

    private WaterMusicManager m_waterMusicManager;

    public override void OnAwake()
    {
        base.OnAwake();
        m_waterMusicManager = this.GetComponent<WaterMusicManager>();
    }

    public override void Appear()
    {
        base.Appear();
        //TODO 重置按钮
        m_waterMusicManager.ReInitSection();
    }

    public void Reset()
    {
        m_waterMusicManager.ReInitSection();
    }
}
