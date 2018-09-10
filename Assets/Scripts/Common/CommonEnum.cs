using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 公共部分

public enum DiscolorationType
{
    CLEAR = 0,
    BLURRY=1
}

#endregion

#region 角色相关结构体
public enum ROLETYPE
{
    ROLE=0,           //可移动对象
    ITEMS,              //道具
    MAX
}

public enum ROLE_FEATURE
{
    STRONG=0,             //强壮
    SLEEP,              //嗜睡
    MAX
}

public enum MONSTER_TYPE
{
    ZOMBIE=0,               //丧尸
    LOIN,                 //美洲狮
    MAX
}

public enum PROPS_TYPE
{
    FOOD,       //食物
    SODA,       //苏打水
    AXES,
    MAX
}

#endregion

#region   技能相关

public enum METHODTYPE
{
    ATTACKONCE=0,         //直接攻击
    ATTACKDELAY,        //延迟攻击
    ATTACKRANGE,        //范围攻击
    MAX
}

#endregion 

#region  数据相关结构体

public class MessageClass
{
    public int eventCode;  //事件码
    public object data;    //数据
}

public enum PARENTLAYER
{
    FLOOR,
    WALL,
    ROLE,
}

public struct ROLEDATA
{
    int ID;
    string Name;
    public ROLETYPE RoleType;     //角色类型
    public bool IsDestroy;            //是否销毁
    public bool IsDie;                //是否死亡



    private float FloatPointX;    //浮点坐标X
    private float FloatPointY;    //浮点坐标Y
    private float FloorPointX;    //地格坐标
    private float FloorPointY;    //地格坐标
    private float BornPointX;     //出生点坐标
    private float BornPointY;     //出生点坐标
    private float DropItemPointX;     //掉落物品点
    private float DropItemPointY;     //掉落物品点
    private float GridRadiusX;        //占地格半径，包括长宽
    private float GridRadiusY;        //占地格半径，包括长宽
    private int RoleDirection;        //角色朝向，所面对的方向
//    private List<RMethodBase> RMethodBaseList;  //角色被动技能
    private LayerMask m_layerMask;              //层级
}

public class RoleMapData
{
    //    private List<ROLEDATA> m_RoleDataList = new List<ROLEDATA>();
    public static Dictionary<int, RBaseRole> m_RoleTileDic = new Dictionary<int, RBaseRole>();

    public static void Init()
    {

    }

    // 根据ID获取
    public static ROLEDATA GetRoleDataById(int id)
    {
        //TODO 根据id获取数据
        ROLEDATA rOLEDATA = new ROLEDATA();
        return rOLEDATA; 
    }
}

#endregion


