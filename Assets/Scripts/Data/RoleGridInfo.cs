using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleGridInfo : GridInfo {
    private Vector2Int m_GridWall;

    private void Start()
    {
        m_wallCount = 1;
    }

    public Vector2Int GetGridWall()
    {
        return TileManager.Instance.WorldPositionToGridPoint(transform.position);
    }

    public override void ResetWall()
    {
        m_wallCount = 1;
    }

    public override int GetXMaxWall(int y)
    {
        return GetGridWall().x;
    }

    public override Vector2Int GetCurReed()
    {
        return GetGridWall();
    }

    public override bool IsLeftExist(int y)
    {
        return false;
    }
}
