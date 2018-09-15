using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishPanel : UIBase {

    private DishMusicManager m_dishMusicManager;

    public override void OnAwake()
    {
        base.OnAwake();
        m_dishMusicManager = this.GetComponent<DishMusicManager>();
    }

    public override void Appear()
    {
        base.Appear();
        //TODO 重置按钮
    }
}
