using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Vector2Int m_MoveInt;       //移动方向

    private RMoveBaseRole m_moveBaseRole;       //移动角色

    private GameObject m_actor; //操作对象

    public MoveCommand(GameObject actor, Vector2Int move)
    {
        m_actor = actor;
        m_MoveInt = move;
    }

    /// <summary>
    /// 执行操作
    /// </summary>
    public void Execute()
    {
        if(m_actor != null)
        {
            m_moveBaseRole = m_actor.GetComponent<RMoveBaseRole>();

            if (m_moveBaseRole != null)
            {
                m_moveBaseRole.Move(m_MoveInt);
            }
        }else
        {
            Debug.LogError("没有找到行动对象");
        }
    }

    /// <summary>
    /// 撤销操作
    /// </summary>
    public void Undo()
    {
        
    }
}
