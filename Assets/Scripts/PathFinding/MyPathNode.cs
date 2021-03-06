﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MyPathNode : SettlersEngine.IPathNode<System.Object>
{
	public Int32 X { get; set; }
	public Int32 Y { get; set; }
	public Boolean IsWall {get; set;}
	
	public bool IsWalkable(System.Object unused)
	{
		return !IsWall;
	}

    // 与起点的长度
    public int gCost { get; set; }
    // 与目标点的长度
    public int hCost { get; set; }

    // 父节点
    public MyPathNode parent;

    // 总的路径长度
    public int fCost
    {
        get { return gCost + hCost; }
    }

}
