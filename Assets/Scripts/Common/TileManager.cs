using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public static TileManager Instance;
    public float m_RefreshLayerTime;

    public MyPathNode[,] m_gridNode;                //地图信息

    //存储层级信息 
    private Dictionary<Vector2Int, GridInfo> m_gridLayerDict = new Dictionary<Vector2Int, GridInfo>();
    private List<GridInfo> m_finalLayerList = new List<GridInfo>();     //存储最终层级消息
    public List<WallGridInfo> m_wallGridInfo = new List<WallGridInfo>();    //存储Item信息
    public List<RoleGridInfo> m_roleGridInfo = new List<RoleGridInfo>();    //存储Role信息
    private List<Vector2Int> m_roleVector = new List<Vector2Int>();
    private List<Vector2Int> m_rolePrevVector = new List<Vector2Int>();

    public RoleConfig m_roleConfig;                //配置文件

    private int m_xUnit;
    private int m_yUnit;
    private float m_Width;
    private float m_Height;
    private Vector3 m_startPosition = Vector3.zero;
    //temp 变量
    private float time = 0;
    private List<int> reed = new List<int>();
    private int curNum = 0;

    // Use this for initialization
    private void Awake() {
        if (Instance == null)
            Instance = this;

        InitMapData();
    }

    /// <summary>
    /// 初始化地图信息
    /// </summary>
    private void InitMapData()
    {
        m_xUnit = m_roleConfig.m_xUnit;
        m_yUnit = m_roleConfig.m_yUnit;
        m_Width = m_roleConfig.m_Width;
        m_Height = m_roleConfig.m_Height;
        m_startPosition = m_roleConfig.m_startPosition;
        m_wallGridInfo.AddRange(transform.GetComponentsInChildren<WallGridInfo>());
        m_roleGridInfo.AddRange(transform.GetComponentsInChildren<RoleGridInfo>());

        m_gridNode = new MyPathNode[m_roleConfig.m_xUnit, m_roleConfig.m_yUnit];

        for (int x = 0; x < m_roleConfig.m_xUnit; x++)
        {
            for (int y = 0; y < m_roleConfig.m_yUnit; y++)
            {
                bool isWall = false;
                m_gridNode[x, y] = new MyPathNode()
                {
                    IsWall = isWall,
                    X = x,
                    Y = y,
                };
            }
        }
        //添加墙体
        for (int i = 0; i < m_wallGridInfo.Count; i++)
        {
            for (int j = 0; j < m_wallGridInfo[i].m_GridWallList.Count; j++)
            {
                try
                {
                    Vector2Int vector = new Vector2Int(m_wallGridInfo[i].m_GridWallList[j].x, m_wallGridInfo[i].m_GridWallList[j].y);
                    AddWall(vector.x, vector.y);
                    m_gridLayerDict.Add(vector, m_wallGridInfo[i]);
                }
                catch (Exception e)
                {
                    Debug.Log("字典键值重复添加" + e.Message);
                }

            }

        }
    }

    private void Start()
    {
        RefreshLayer1();
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if(time>m_RefreshLayerTime)
        {
            time = 0;
            RefreshLayer1();
        }
    }

    
    /// <summary>
    /// 刷新层级
    /// </summary>
    private void RefreshLayer1()
    {
        RefreshRoleLayerPosition();
        m_finalLayerList.Clear();
        reed.Clear();
        foreach (var grid in m_gridLayerDict.Values)
        {
            grid.ResetWall();
        }
        for (int i = m_yUnit - 1; i >= 0; i--)
        {
            reed.Add(0);
        }
        curNum = m_yUnit - 1;
        int isOver = 0;
        while (isOver==0)
        {
            Vector2Int vector = new Vector2Int(reed[curNum], curNum);
            if (m_gridLayerDict.ContainsKey(vector))
            {
                if (m_gridLayerDict[vector].IsSingle())
                {
                    m_finalLayerList.Add(m_gridLayerDict[vector]);
                    if (reed[curNum] < m_xUnit)
                    {
                        reed[curNum]++;
                    }else
                    {
                        curNum--;
                    }
                }
                else
                {
                    if(m_gridLayerDict[vector].IsLeftExist(curNum))
                    {
                        if (reed[curNum] < m_xUnit)
                        {
                            reed[curNum] = m_gridLayerDict[vector].GetXMaxWall(curNum) + 1;
                            curNum--;
                        }

                    }
                    else
                    {
                        reed[curNum]++;
                        curNum = m_gridLayerDict[vector].GetCurReed().y;
                        reed[curNum] = m_gridLayerDict[vector].GetCurReed().x + 1;
                        if(!m_finalLayerList.Contains(m_gridLayerDict[vector]))
                            m_finalLayerList.Add(m_gridLayerDict[vector]);

                    }
                }
            }
            else
            {
                if (reed[curNum] < m_xUnit)
                {
                    reed[curNum]++;
                }
                else
                {
                    curNum--;
                }
            }
            for (int i = m_yUnit - 1; i >= 0; i--)
            {
                if(reed[i]!=m_xUnit)
                {
                    isOver = -1;
                }
            }
            if(isOver==0 || curNum==-1)
            {
                isOver = -1;
            }else
            {
                isOver = 0;
            }
        }
        int id = 0;
        for (int i = 0; i < m_finalLayerList.Count; i++)
        {
            if (m_finalLayerList[i].GetComponent<SpriteRenderer>())
            {
                m_finalLayerList[i].GetComponent<SpriteRenderer>().sortingLayerName = "Item";
                m_finalLayerList[i].GetComponent<SpriteRenderer>().sortingOrder = id++;
            }
            else
            {
                m_finalLayerList[i].gameObject.AddComponent<SpriteRenderer>().sortingLayerName = "Item";
                m_finalLayerList[i].gameObject.AddComponent<SpriteRenderer>().sortingOrder = id++;
            }
        }
    }

   

    void RefreshRoleLayerPosition()
    {
        for (int i = 0; i < m_rolePrevVector.Count; i++)
        {
            if(m_gridLayerDict.ContainsKey(m_rolePrevVector[i]))
            {
                m_gridLayerDict.Remove(m_rolePrevVector[i]);
            }
        }
        m_roleVector.Clear();
        for (int i=0;i<m_roleGridInfo.Count;i++)
        {
            m_roleVector.Add(m_roleGridInfo[i].GetGridWall());
            if(!m_gridLayerDict.ContainsKey(m_roleGridInfo[i].GetGridWall()))
            {
                m_gridLayerDict.Add(m_roleGridInfo[i].GetGridWall(), m_roleGridInfo[i]);
            }
        }
        m_rolePrevVector.Clear();
        m_rolePrevVector.AddRange(m_roleVector);
    }


    /// <summary>
    /// 返回格子宽度
    /// </summary>
    /// <returns></returns>
    public float GetTileWidth()
    {
        return m_roleConfig.m_Width;
    }

    /// <summary>
    /// 返回格子长度
    /// </summary>
    /// <returns></returns>
    public float GetTileHeight()
    {
        return m_roleConfig.m_Height;
    }

    /// <summary>
    /// 世界坐标转格子坐标(仅针对于Sprite)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2Int WorldPositionToGridPoint(Vector3 pos)
    {
        pos -= new Vector3(m_startPosition.x, m_startPosition.y, 0);
        Vector2Int tileGridPoint =Vector2Int.zero; 
        tileGridPoint.x = (int)(pos.x / m_Width - pos.y / m_Height);
        tileGridPoint.y = (int)(pos.x / m_Width + pos.y / m_Height);
        return tileGridPoint;
    }

    /// <summary>
    /// 屏幕坐标转世界坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 ScreenPointToWorldPosition(Vector3 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos); 
    }

    /// <summary>
    /// 屏幕坐标转格子坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2Int ScreenPointToTilePoint(Vector3 pos)
    {
        Vector3 pos1 = Camera.main.ScreenToWorldPoint(pos);
        return WorldPositionToGridPoint(pos1);
    }

    /// <summary>
    /// 添加墙体物体
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddWall(int x, int y)
    {
        if (x < m_xUnit && x >= 0 && y < m_yUnit && y >= 0)
        {
            m_gridNode[x, y].IsWall = true;
        }
    }

    /// <summary>
    /// 删除墙体物体
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RemoveWall(int x, int y)
    {
        if (x < m_xUnit && x >= 0 && y < m_yUnit && y >= 0)
        {
            m_gridNode[x, y].IsWall = false;
        }
    }

    /// <summary>
    /// 判断是否为墙体
    /// </summary>
    /// <param name="uniPoint"></param>
    /// <returns></returns>
    public bool IsWallByGrid(Vector2Int point)
    {
        //判断超出边界问题
        if (point.x < 0 || point.x >= m_gridNode.GetLength(0)||point.y<0||point.y>=m_gridNode.GetLength(1))
        {
            return true;
        }
        return m_gridNode[point.x, point.y].IsWall;
    }

    /// <summary>
    /// 将格子坐标转换成世界坐标
    /// </summary>
    /// <param name="uniPoint"></param>
    /// <param name="playerLayer"></param>
    /// <returns></returns>
    public Vector3 GridPointToWorldPosition(Vector2Int gridPoint)
    {
        //返回格子中心位置
        Vector3 pos = Vector3.zero;
        pos.x = (float)((m_Width/2.0*(gridPoint.x+gridPoint.y)) + m_Width/2.0);
        pos.y = (float)(m_Height / 2.0 * (-gridPoint.x + gridPoint.y));
        return pos+m_startPosition;
    }

    // 取得周围的节点
    public List<MyPathNode> getNeibourhood(MyPathNode node)
    {
        List<MyPathNode> list = new List<MyPathNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // 如果是自己，则跳过
                if (i == 0 && j == 0)
                    continue;
                int x = node.X + i;
                int y = node.Y + j;
                // 判断是否越界，如果没有，加到列表中
                if (x < m_xUnit && x >= 0 && y < m_yUnit && y >= 0)
                    list.Add(m_gridNode[x, y]);
            }
        }
        return list;
    }

    // A*寻路
    public List<MyPathNode> FindingPath(Vector2Int s, Vector2Int e)
    {
        MyPathNode startNode = m_gridNode[s.x, s.y];
        MyPathNode endNode = m_gridNode[e.x, e.y];

        List<MyPathNode> openSet = new List<MyPathNode>();
        List<MyPathNode> closeSet = new List<MyPathNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            MyPathNode curNode = openSet[0];

            for (int i = 0, max = openSet.Count; i < max; i++)
            {
                if (openSet[i].fCost <= curNode.fCost &&
                    openSet[i].hCost < curNode.hCost)
                {
                    curNode = openSet[i];
                }
            }

            openSet.Remove(curNode);
            closeSet.Add(curNode);

            // 找到的目标节点
            if (curNode == endNode)
            {
                return generatePath(startNode, endNode);
            }

            // 判断周围节点，选择一个最优的节点
            foreach (var item in getNeibourhood(curNode))
            {
                // 如果是墙或者已经在关闭列表中
                if (item.IsWall || closeSet.Contains(item))
                    continue;
                // 计算当前相领节点现开始节点距离
                int newCost = curNode.gCost + getDistanceNodes(curNode, item);
                // 如果距离更小，或者原来不在开始列表中
                if (newCost < item.gCost || !openSet.Contains(item))
                {
                    // 更新与开始节点的距离
                    item.gCost = newCost;
                    // 更新与终点的距离
                    item.hCost = getDistanceNodes(item, endNode);
                    // 更新父节点为当前选定的节点
                    item.parent = curNode;
                    // 如果节点是新加入的，将它加入打开列表中
                    if (!openSet.Contains(item))
                    {
                        openSet.Add(item);
                    }
                }
            }
        }

        return closeSet;
    }

    // 生成路径
    List<MyPathNode> generatePath(MyPathNode startNode, MyPathNode endNode)
    {
        List<MyPathNode> path = new List<MyPathNode>();
        if (endNode != null)
        {
            MyPathNode temp = endNode;
            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.parent;
            }
            // 反转路径
            path.Reverse();
        }
        return path;
    }

    // 获取两个节点之间的距离
    int getDistanceNodes(MyPathNode a, MyPathNode b)
    {
        int cntX = Mathf.Abs(a.X - b.X);
        int cntY = Mathf.Abs(a.Y - b.Y);
        // 判断到底是那个轴相差的距离更远
        if (cntX > cntY)
        {
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else
        {
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }

    /// <summary>
    /// 输入起始点和终点寻路
    /// </summary>
    /// <param name="startGridPosition"></param>
    /// <param name="endGridPosition"></param>
    public IEnumerable<MyPathNode> FindUpdatedPath(Vector2Int startGridPosition, Vector2Int endGridPosition)
    {
        MySolver<MyPathNode, System.Object> aStar = new MySolver<MyPathNode, System.Object>(m_gridNode);
        IEnumerable<MyPathNode> path = aStar.Search(startGridPosition, endGridPosition, null);


        if (path != null)
        {
            return path;
        }
        return null;
    }

    public class MySolver<TPathNode, TUserContext> : SettlersEngine.SpatialAStar<TPathNode,
    TUserContext> where TPathNode : SettlersEngine.IPathNode<TUserContext>
    {
        protected override Double Heuristic(PathNode inStart, PathNode inEnd)
        {


            int formula = 0;
            int dx = Math.Abs(inStart.X - inEnd.X);
            int dy = Math.Abs(inStart.Y - inEnd.Y);

            if (formula == 0)
                return Math.Sqrt(dx * dx + dy * dy); //Euclidean distance

            else if (formula == 1)
                return (dx * dx + dy * dy); //Euclidean distance squared

            else if (formula == 2)
                return Math.Min(dx, dy); //Diagonal distance

            else if (formula == 3)
                return (dx * dy) + (dx + dy); //Manhatten distance



            else
                return Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y);
        }

        protected override Double NeighborDistance(PathNode inStart, PathNode inEnd)
        {
            return Heuristic(inStart, inEnd);
        }

        public MySolver(TPathNode[,] inGrid)
            : base(inGrid)
        {
        }
    }

    /// <summary>
    /// 获取音乐当前关卡
    /// </summary>
    /// <returns></returns>
    public int[] GetMusicLevel()
    {
        List<int> level = new List<int>();
        for (int i = 0; i < m_roleConfig.m_musicLevel.Length; i++)
        {
            if (m_roleConfig.m_musicLevel[i] == 1)
            {
                if(!level.Contains(i))
                    level.Add(i);
            }
        }
        return level.ToArray();
    }

    /// <summary>
    /// 设置音乐关卡
    /// </summary>
    /// <param name="level"></param>
    public void SetMusicLevel(int level)
    {
        m_roleConfig.m_musicLevel[level] = 1;
    }
    /// <summary>
    /// 获取玩家位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerPosition()
    {
        return m_roleConfig.m_playerPos;
    }

    /// <summary>
    /// 设置玩家位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetPlayerPosition(Vector3 pos)
    {
        m_roleConfig.m_playerPos = pos;
    }
}
