using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGridInfo : GridInfo {

    public List<Vector2Int> m_GridWallList = new List<Vector2Int>();

    public override Vector2Int GetCurReed()
    {
        int x = 0;
        int y = 0;
        foreach(var grid in m_GridWallList)
        {
            if(grid.x>x)
            {
                x = grid.x;
            }
            if(grid.y>y)
            {
                y = grid.y;
            }
        }
        return new Vector2Int(x, y);
    }

    public override int GetXMaxWall(int y)
    {
        int max = 0;
        foreach(var grid in m_GridWallList)
        {
            if(grid.y==y)
            {
                if (grid.x > max)
                    max = grid.x;
            }
        }
        return max;
    }

    public override bool IsLeftExist(int y)
    {
        foreach(var grid in m_GridWallList)
        {
            if (grid.y == y - 1)
                return true;
        }
        return false;
    }

    public override void ResetWall()
    {
        m_wallCount = m_GridWallList.Count;
    }

    private void Start()
    {
        m_wallCount = m_GridWallList.Count;
    }
}
