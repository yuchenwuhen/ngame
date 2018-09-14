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
    public float m_checkRange = 3f;
    private Transform m_player;
    private BoxCollider m_collider;
    private bool m_isEnter = false;

    private void Start()
    {
        m_player = GameObject.FindWithTag("Player").transform;
    }

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
