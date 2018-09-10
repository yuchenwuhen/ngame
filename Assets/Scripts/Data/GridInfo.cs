using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridInfo : MonoBehaviour {

    protected int m_wallCount;

    public abstract void ResetWall();
    public abstract int GetXMaxWall(int y);
    public abstract Vector2Int GetCurReed();
    public abstract bool IsLeftExist(int y);

    public bool IsWallOver()
    {
        m_wallCount -= 1;
        if (m_wallCount <= 0)
        {
            return true;
        }
        return false;
    }

    public bool IsSingle()
    {
        return m_wallCount == 1;
    }
}
