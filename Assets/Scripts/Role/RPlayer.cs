using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayer : RMoveBaseRole {

    public enum States
    {
        Idle,
        Move,
        Pathfinding
    }

    private StateMachine<States> m_fsm;
    private PlayerMove m_playerMove;
    private GameObject m_hitObj = null;

    protected override void Init()
    {
        base.Init();
        m_fsm = StateMachine<States>.Initialize(this,States.Idle);
        m_playerMove = this.GetComponent<PlayerMove>();
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="direction"></param>
    public override void Move(Vector2Int direction)
    {
        base.Move(direction);
    }

    public override void Pathfinding(Vector2Int pos,GameObject hitObj)
    {
        base.Pathfinding(pos,hitObj);
        m_hitObj = null;
        //判断hitObj是否为空
        if (hitObj!=null)
        {
            //点击到了交互对象
            Vector2Int hitpos = TileManager.Instance.WorldPositionToGridPoint(hitObj.transform.position);
            Vector2Int playerpos = TileManager.Instance.WorldPositionToGridPoint(transform.position);
            Vector2Int newpos = GetAroundPoint(playerpos, hitpos);
            if (newpos != playerpos)
            {
                m_playerMove.Pathfinding(newpos);
                m_hitObj = hitObj;
                PlayAnimation(1f);
            }     
        }
        else
        {
            //点击了地面
            if (pos != TileManager.Instance.WorldPositionToGridPoint(transform.position))
            {
                m_playerMove.Pathfinding(pos);
                PlayAnimation(1f);
            }

        }
        m_playerMove.m_pathEnd -= PathEndCallback;
        m_playerMove.m_pathEnd += PathEndCallback;

    }

    protected override void PlayAnimation(float value)
    {
        base.PlayAnimation(value);
        m_animator.SetFloat("speed",value);
    }

    void PathEndCallback()
    {

        PlayAnimation(0f);
    }

    private Vector2Int GetAroundPoint(Vector2Int pos,Vector2Int hitpos)
    {
        int xdir = pos.x - hitpos.x;
        int ydir = pos.y - hitpos.y;
        Vector2Int newpos = hitpos;
        if(xdir!=0)
        {
            newpos.x += xdir > 0 ? 1 : -1;
            if (!TileManager.Instance.IsWallByGrid(newpos))
            {
                return newpos;
            }
        }
        newpos = hitpos;
        if (ydir!=0)
        {
            newpos.y += ydir > 0 ? 1 : -1;
            if (!TileManager.Instance.IsWallByGrid(newpos))
            {
                return newpos;
            }
        }
        return pos;
    }

    /// <summary>
    /// 收到消息
    /// </summary>
    /// <param name="message"></param>
    protected override void ReceiveMessage(IMessage message)
    {
        base.ReceiveMessage(message);
        Message msg = message as Message;
        switch(msg.EventCode)
        {
            default:
                break;
        }
    }

}
