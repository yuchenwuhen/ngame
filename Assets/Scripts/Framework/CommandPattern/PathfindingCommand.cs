using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingCommand : ICommand {

    private Vector2Int m_MoveInt = Vector2Int.zero;       //移动坐标
    private GameObject m_HitObj = null;        //交互对象

    private RMoveBaseRole m_moveBaseRole;       //移动角色
    private GameObject m_actor; //操作对象

    public PathfindingCommand(GameObject actor, Vector2Int move)
    {
        m_actor = actor;
        m_MoveInt = move;
    }

    public PathfindingCommand(GameObject actor, GameObject hitobj)
    {
        m_actor = actor;
        m_HitObj = hitobj;
    }

    /// <summary>
    /// 执行操作
    /// </summary>
    public void Execute()
    {
        if (m_actor != null)
        {
            m_moveBaseRole = m_actor.GetComponent<RMoveBaseRole>();

            if (m_moveBaseRole != null)
            {
                m_moveBaseRole.Pathfinding(m_MoveInt, m_HitObj);
            }
        }
        else
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
