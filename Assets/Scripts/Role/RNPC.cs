using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType
{
    Ming,
}

public class RNPC : RMoveBaseRole {

    [SerializeField]
    private NPCType m_npcType;

    protected override void Init()
    {
        base.Init();
        switch(m_npcType)
        {
            case NPCType.Ming:
                
                break;
            default:
                break;
        }
    }
}
