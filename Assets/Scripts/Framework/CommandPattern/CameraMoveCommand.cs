using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveCommand : ICommand
{
    private Vector2Int m_MoveInt;       //移动方向

    private Camera m_camera; //操作对象

    private Vector3 m_offset;

    public CameraMoveCommand(Vector3 offset)
    {
        m_camera = Camera.main;
        m_offset = offset;
    }

    /// <summary>
    /// 执行操作
    /// </summary>
    public void Execute()
    {
        if (m_camera != null)
        {
            // 摄像机移，速度控制，todo
            //m_camera.transform.position += m_offset;
            m_camera.GetComponent<CameraActivity>().OffsetMove(m_offset);
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
