using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMove : MonoBehaviour{

    public AudioClip moveSound1;                //1 of 2 Audio clips to play when player moves.
    public AudioClip moveSound2;                //2 of 2 Audio clips to play when player moves.

    public float moveTime = 0.1f;           //Time it will take object to move, in seconds.
    public LayerMask blockingLayer;         //Layer on which collision will be checked.

    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;          //Used to make movement more efficient.

    private Coroutine moveCoro;
    public delegate void CompletePathfindingHandler();
    public event CompletePathfindingHandler m_pathEnd;
    //Start overrides the Start function of MovingObject
    protected void Start()
    {
        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    /// <summary>
    /// pathfinding by input coordinate
    /// </summary>
    /// <param name="end"></param>
    /// <returns></returns>
    private List<Vector2Int> nodeList = new List<Vector2Int>();
    public void Pathfinding(Vector2Int end)
    {
        if(!TileManager.Instance.IsWallByGrid(end))
        {
            Vector2Int start = TileManager.Instance.WorldPositionToGridPoint(transform.position);
            List<MyPathNode> enumerator = TileManager.Instance.FindingPath(start, end);
            //IEnumerable<MyPathNode> enumerator = TileManager.Instance.FindUpdatedPath(start, end);
            if (moveCoro != null)
            {
                StopCoroutine(moveCoro);
            }
            nodeList.Clear();
            foreach (var node in enumerator)
            {
                Vector2Int m_node = new Vector2Int(node.X, node.Y);
                if (m_node != start )
                {
                    nodeList.Add(m_node);
                }
            }
            moveCoro = StartCoroutine(MoveCoro());
        }
    }

    //TODO 改成for循环
    IEnumerator MoveCoro()
    {
        foreach(var node in nodeList)
        {
            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            
            Vector3 endPos = TileManager.Instance.GridPointToWorldPosition(node);
            float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
            //While that distance is greater than a very small amount (Epsilon, almost zero):
            while (sqrRemainingDistance > 0.001f)
            {
                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(transform.position, endPos, inverseMoveTime * Time.deltaTime);

                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                rb2D.MovePosition(newPostion);

                //Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;
            }


        }
        if(nodeList.Count>0)
            m_pathEnd();
        //yield return null;
    }

    /// <summary>
    /// move by grid iswall mark
    /// </summary>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    protected bool Move(Vector2Int start, Vector2Int end)
    {

        //Check if anything was hit
        if (!TileManager.Instance.IsWallByGrid(end))
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            moveCoro = StartCoroutine(SmoothMovement(end));

            //Return true to say that Move was successful
            return true;
        }
        else
        {
            return false;
        }

    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector2Int end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        Vector3 endPos = TileManager.Instance.GridPointToWorldPosition(end);
        float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, endPos, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        if(nodeList.Count>0 )
        {
            //Vector2Int end1 = nodeList.Dequeue();
            //Move(end, end1);

        }
    }

    public void MoveEnd()
    {
        
    }

}
