using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鼠标行为类
//目前只支持鼠标的移动和鼠标的点击事件
//鼠标的位置信息是空间坐标，需要被转换
public class MouseActivity :Activity
{
    private Vector3 m_vec3LastPos = Vector3.zero; // 上一次鼠标点击的位置信息
    private bool m_bIsFingerMoved = false; // 记录鼠标是否移动过

    public MouseActivity(GameObject player, GameObject camera) : base(player, camera)
    {

    }

    // 手指行为函数
    public override ICommand ActivityHandle()
    {
        ICommand command = null;
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse GetMouseButtonDown");
            m_bIsFingerMoved = false;           
        }
        else if (Input.GetMouseButton(0))
        {
            //Debug.Log("Mouse GetMouseButton");
            if (m_vec3LastPos != Input.mousePosition)
            {
                m_bIsFingerMoved = true;
                // 触发移动事件
                //Debug.Log("Mouse Moved");
                command = base.OnMoveEvent(m_vec3LastPos, Input.mousePosition);
                //Debug.Log("After Mouse Moved");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Mouse GetMouseButtonUp");
            if (!m_bIsFingerMoved)
            {
                // 触发点击事件
                //Debug.Log("Mouse Click Only");
                command = base.OnClickEvent(Input.mousePosition);
            }
        }
        m_vec3LastPos = Input.mousePosition;
        return command;
    }
}
