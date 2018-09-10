using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RProps : RBaseRole {

    public PROPS_TYPE m_propsType;      //道具类型

    protected override void Init()
    {
        base.Init();
    }



    /// <summary>
    /// 碰撞盒检测
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    /// <summary>
    /// 退出碰撞盒碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    /// <summary>
    /// 触发器检测
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    /// <summary>
    /// 退出触发器碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {

    }

}
