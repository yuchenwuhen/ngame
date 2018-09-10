using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOutCommand : ICommand
{
    private Camera m_camera; //操作对象

    private Vector3 m_offset;

    public CameraZoomOutCommand()
    {
        m_camera = Camera.main;
        m_offset = Vector3.zero;
    }

    public CameraZoomOutCommand(Vector3 offset)
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
            //物体变小
            if (m_camera.orthographicSize <= 1f)
            {
                m_camera.orthographicSize = 0.5f;
            }
            else 
            {
                m_camera.orthographicSize -= 0.5f;
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
