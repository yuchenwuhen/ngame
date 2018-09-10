using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LineBackground {

    [SerializeField]
    private static int m_xUnit = 2;
    [SerializeField]
    private static int m_yUnit = 2;
    [SerializeField]
    private static float m_Width =0.93f;
    [SerializeField]
    private static float m_Height = 0.465f;

    private static GameObject[,] m_lineArray;         //存储格子数组
    private static Vector3[] m_unitVector;            //单个格子所存坐标
    [SerializeField]
    private static Vector3 m_startPosition = new Vector3(-11,0,0);            //初始位置

	// Use this for initialization
    [MenuItem("Tools/linerender")]
	static void Create()
    { 

        m_lineArray = new GameObject[m_xUnit, m_yUnit];
        m_lineArray.Initialize();
        m_unitVector = new Vector3[5];
        Vector3 initPos = m_startPosition;
        GameObject obj = new GameObject("LinerenderParent");
        obj.transform.position = Vector3.zero;
        for(int i=0;i<m_xUnit;i++)
        {
            initPos = m_startPosition;
            initPos.x += (float)(m_Width*i / 2.0);
            initPos.y -= (float)(m_Height*i / 2.0) ;
            for(int j=0;j<m_yUnit;j++)
            {
                m_unitVector[0] = new Vector3(initPos.x + (float)(m_Width / 2.0) * j, initPos.y + (float)(m_Height / 2.0) * j, 0);
                m_unitVector[1] = new Vector3(initPos.x + (float)(m_Width / 2.0) * (j + 1), initPos.y + (float)(m_Height / 2.0) * (j + 1), 0);
                m_unitVector[2] = new Vector3(initPos.x + (float)(m_Width / 2.0) * (j + 2), initPos.y + (float)(m_Height / 2.0) * j, 0);
                m_unitVector[3] = new Vector3(initPos.x + (float)(m_Width / 2.0) * (j + 1), initPos.y + (float)(m_Height / 2.0) * (j - 1), 0);
                m_unitVector[4] = new Vector3(initPos.x + (float)(m_Width / 2.0) * j, initPos.y + (float)(m_Height / 2.0) * j, 0);

                m_lineArray[i,j] = new GameObject(i.ToString() + j.ToString());
                m_lineArray[i, j].transform.SetParent(obj.transform);
                LineRenderer lineRenderer = m_lineArray[i, j].AddComponent<LineRenderer>();
                lineRenderer.positionCount = 5;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                lineRenderer.SetPosition(0, m_unitVector[0]);
                lineRenderer.SetPosition(1, m_unitVector[1]);
                lineRenderer.SetPosition(2, m_unitVector[2]);
                lineRenderer.SetPosition(3, m_unitVector[3]);
                lineRenderer.SetPosition(4, m_unitVector[4]);
            }
        }
        
    }

    public static void Update(int x,int y,float w,float h)
    {
        m_xUnit = x;
        m_yUnit = y;
        m_Width = w;
        m_Height = h;
        Create();
    }
	
}
