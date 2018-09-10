using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoleInfoData: IComparer<RoleInfoData> {
    public int m_id;
    public string m_name;
    public Vector3 m_pos;

    public RoleInfoData()
    {

    }

    public RoleInfoData(int id,string name,Vector3 vector3)
    {
        this.m_id = id;
        this.m_pos = vector3;
        this.m_name = name;
    }

    public int Compare(RoleInfoData x,RoleInfoData y)
    {
        return x.m_id.CompareTo(y.m_id);
    }
}
