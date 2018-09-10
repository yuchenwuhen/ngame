using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class RMoveBaseRole : RBaseRole {

    public int m_useNum;        //行动消耗点
    public int m_isCanSkill;    //是否能使用技能

    protected Animator m_animator;
    protected delegate Event m_moveEndDelegate();

    protected override void Init()
    {
        base.Init();
        m_animator = this.GetComponent<Animator>();
    }

    protected override void SetSortingLayer()
    {
        this.GetComponent<SpriteRenderer>().sortingLayerID = 4;
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="opsName"></param>
    protected virtual void PlayAnimation(string opsName)
    {
        m_animator.SetTrigger(opsName);
    }

    /// <summary>
    /// 移动的方向和速度
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    public virtual void Move(Vector2Int direction)
    {

    }

    /// <summary>
    /// 寻路
    /// </summary>
    /// <param name="pos"></param>
    public virtual void Pathfinding(Vector2Int pos,GameObject hitObj)
    {

    }

}
