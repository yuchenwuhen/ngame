using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath : MonoBehaviour {
	private Grid grid;
    private MyPathNode[,] m_nodeList;
    // Use this for initialization
    void Start () {
        m_nodeList = TileManager.Instance.m_gridNode;

        List<MyPathNode> list = FindingPath(new Vector2Int(24, 4), new Vector2Int(23, 6));
        Debug.Log(list);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // A*寻路
    List<MyPathNode> FindingPath(Vector2Int s, Vector2Int e)
    {
        MyPathNode startNode = m_nodeList[s.x,s.y];
        MyPathNode endNode = m_nodeList[e.x, e.y];

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
                generatePath(startNode, endNode);
                return closeSet;
            }

            // 判断周围节点，选择一个最优的节点
            foreach (var item in TileManager.Instance.getNeibourhood(curNode))
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
    void generatePath(MyPathNode startNode, MyPathNode endNode) {
		List<MyPathNode> path = new List<MyPathNode>();
		if (endNode != null) {
            MyPathNode temp = endNode;
			while (temp != startNode) {
				path.Add (temp);
				temp = temp.parent;
			}
			// 反转路径
			path.Reverse ();
		}
    }

	// 获取两个节点之间的距离
	int getDistanceNodes(MyPathNode a, MyPathNode b) {
		int cntX = Mathf.Abs (a.X - b.X);
		int cntY = Mathf.Abs (a.Y - b.Y);
		// 判断到底是那个轴相差的距离更远
		if (cntX > cntY) {
			return 14 * cntY + 10 * (cntX - cntY);
		} else {
			return 14 * cntX + 10 * (cntY - cntX);
		}
	}


}
