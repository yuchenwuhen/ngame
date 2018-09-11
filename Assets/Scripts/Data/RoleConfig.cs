using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoleConfig :ScriptableObject  {

    [Tooltip("x轴单元格")]
    [SerializeField]
    public  int m_xUnit = 10;
    [Tooltip("y轴单元格")]
    [SerializeField]
    public  int m_yUnit = 10;
    [Tooltip("菱形精灵宽度")]
    [SerializeField]
    public  float m_Width = 2.56f;
    [Tooltip("菱形精灵高度")]
    [SerializeField]
    public  float m_Height = 1.49f;
    [SerializeField]
    public int[] m_musicLevel;
    [SerializeField]
    public Vector3 m_playerPos;

    [Tooltip("起始位置")]
    [SerializeField]
    public  Vector3 m_startPosition = new Vector3(-11, 0, 0);            //初始位置
    public Vector2Int[] wallPos;               //墙体位置

    [Tooltip("当前分配到的ID号")]public int id;

    public  List<RoleInfoData> m_RoleInfoList = new List<RoleInfoData>();

    IComparer<RoleInfoData> comparer = new RoleInfoData();

    private void SortData()
    {
        m_RoleInfoList.Sort(comparer);
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData(RoleInfoData roleInfoData)
    {
        m_RoleInfoList.Add(roleInfoData);
    }

    /// <summary>
    /// 判断Id是否存在
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsIdExist(int id)
    {
        RoleInfoData roleInfoData = null;
        roleInfoData = m_RoleInfoList.Find(roleData => roleData.m_id == id);
        if (m_RoleInfoList.Contains(roleInfoData))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 移除数据
    /// </summary>
    /// <param name="id"></param>
    public void RemoveData(int id)
    {
        RoleInfoData roleInfoData=null;
        roleInfoData = m_RoleInfoList.Find(roleData => roleData.m_id == id );
        if(m_RoleInfoList.Contains(roleInfoData))
        {
            m_RoleInfoList.Remove(roleInfoData);
        }
    }

    /// <summary>
    /// 返回所存在的ID
    /// </summary>
    /// <returns></returns>
    public List<int> GetIDList()
    {
        List<int> idList = new List<int>();
        foreach(var roleInfoData in m_RoleInfoList)
        {
            idList.Add(roleInfoData.m_id);
        }
        return idList;
    }

    /// <summary>
    /// 根据ID返回信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RoleInfoData GetRoleInfoData(int id)
    {
        RoleInfoData roleInfoData = null;
        roleInfoData = m_RoleInfoList.Find(roleData => roleData.m_id == id);
        if (m_RoleInfoList.Contains(roleInfoData))
        {
            return roleInfoData;
        }
        return null;
    }

}
