using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//手指行为类
//目前只支持手指的移动，手指的点击事件，以及两只手指的缩放
//手指的位置信息是空间坐标，需要被转换
public class FingerActivity : Activity
{
    private Vector2 m_vec2LastPos = Vector2.zero; // 上一次手指点击的位置信息
    private bool m_bIsFingerMoved = false; // 记录手指是否移动过

    private Vector2 m_vec2OldPosition1 = Vector2.zero; // 双指操作中，第一只手指上次的位置信息
    private Vector2 m_vec2OldPosition2 = Vector2.zero; // 双指操作中，第一只手指上次的位置信息

    public FingerActivity(GameObject player, GameObject camera): base(player, camera)
    {
        
    }

    // 手指行为函数
    public override ICommand ActivityHandle()
    {
        ICommand command = null;
        //单指操作
        if (Input.touchCount == 1)
        {
            // 手指刚按下
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                m_bIsFingerMoved = false;
            }
            // 手指移动
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).phase != TouchPhase.Stationary)
            {
                if (m_vec2LastPos != Input.GetTouch(0).position)//解决手指用力向下按也会触发TouchPhase.Moved
                {
                    //手指触摸到场景区域，触发【移动】事件，超出范围就不移动
                    m_bIsFingerMoved = true;
                    //OnFingerMoveEvent(m_vec2LastPos, Input.GetTouch(0).position);
                    command = base.OnMoveEvent(new Vector3(m_vec2LastPos.x, m_vec2LastPos.y, 0), new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));
                }
            }
            // 手指抬起
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // 手指抬起时，手指未移动过，触发【点击】事件
                if (!m_bIsFingerMoved)
                {
                    //OnFingerClickEvent(Input.GetTouch(0).position);
                    command = base.OnClickEvent(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));
                }
            }
            m_vec2LastPos = Input.GetTouch(0).position;
        }
        //多指操作
        else if (Input.touchCount > 1 && Input.touches[0].phase != TouchPhase.Stationary && Input.touches[1].phase != TouchPhase.Stationary)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                m_vec2OldPosition1 = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                m_vec2OldPosition2 = Input.GetTouch(1).position;
            }

            //前两只手指触摸类型都为移动触摸
            if (m_vec2OldPosition1 != Vector2.zero && m_vec2OldPosition2 != Vector2.zero)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    //计算出当前两点触摸点的位置
                    Vector2 vec2NowPosition1 = Input.GetTouch(0).position;
                    Vector2 vec2NowPosition2 = Input.GetTouch(1).position;

                    //函数返回真为放大，返回假为缩小
                    if (IsEnlarge(m_vec2OldPosition1, m_vec2OldPosition2, vec2NowPosition1, vec2NowPosition2))
                    {
                        command = base.OnFingerZoomOutEvent();
                    }
                    else
                    {
                        command = base.OnFingerZoomInEvent();
                    }

                    //备份上一次触摸点的位置，用于对比
                    m_vec2OldPosition1 = vec2NowPosition1;
                    m_vec2OldPosition2 = vec2NowPosition2;
                }
            }
        }
        return command;
    }

    //函数返回真为放大，返回假为缩小
    private bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        var leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        var leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            //放大手势
            return true;
        }
        else
        {
            //缩小手势
            return false;
        }
    }
}
