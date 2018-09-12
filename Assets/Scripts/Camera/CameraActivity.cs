using UnityEngine;
using System.Collections;

public class CameraActivity : MonoBehaviour
{

    private Coroutine moveCoro;

    private float inverseMoveTime;          //Used to make movement more efficient.
    public float moveTime = 0.1f;           //Time it will take object to move, in seconds.

    public float moveSpeed = 0.2f;          //Camera Move Speed
    // Use this for initialization
    void Start()
    {
        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;

        transform.position = new Vector3(3.7f, 3.7f, -10f);

        Camera.main.orthographicSize = 5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(Vector3 endPos)
    {
        if (moveCoro != null)
        {
            StopCoroutine(moveCoro);
        }
        moveCoro = StartCoroutine(SmoothMovement(endPos));
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 endPos)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        //Vector3 endPos = TileManager.Instance.GridPointToWorldPosition(end);
        float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > 0.001f)
        {
            Debug.Log(sqrRemainingDistance);
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(transform.position, endPos, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            //rb2D.MovePosition(newPostion);
            transform.position = newPostion;
            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
    }

    public void OffsetMove(Vector3 offset)
    {
        // 摄像机的移动与手指移动相反
        //transform.position -= offset * moveSpeed;

        Vector3 lastPos = transform.position - offset * moveSpeed;
        BoundCheck(Camera.main.orthographicSize, ref lastPos);
        transform.position = lastPos;
    }

    public void ZoomIn()
    {
        //摄像机视野变大
        float orthographicSize = Camera.main.orthographicSize;
        if (orthographicSize >= 15f)
        {
            orthographicSize = 15f;
        }
        else
        {
            orthographicSize += 0.5f;
        }

        Vector3 lastPos = transform.position;
        BoundCheck(orthographicSize, ref lastPos);

        transform.position = lastPos;
        Camera.main.orthographicSize = orthographicSize;
    }

    public void ZoomOut()
    {
        //摄像机视野变小
        float orthographicSize = Camera.main.orthographicSize;
        if (orthographicSize <= 5f)
        {
            orthographicSize = 5f;
        }
        else
        {
            orthographicSize -= 0.5f;
        }

        Vector3 lastPos = transform.position;
        BoundCheck(orthographicSize, ref lastPos);

        transform.position = lastPos;
        Camera.main.orthographicSize = orthographicSize;
    }

    /// <summary>
    /// 检查摄像机是否移出边界
    /// </summary>
    /// <param name="orthographicSize">Orthographic size.</param>
    /// <param name="checkPos">Check position.</param>
    private void BoundCheck(float orthographicSize, ref Vector3 checkPos)
    {
        float MaxX = 29f - (9f / 5f) * orthographicSize;
        float MaxY = 27f - orthographicSize;

        if (checkPos.x > MaxX)
        {
            checkPos.x = MaxX;
        }

        if (checkPos.x < -MaxX)
        {
            checkPos.x = -MaxX;
        }

        if (checkPos.y > MaxY)
        {
            checkPos.y = MaxY;
        }

        if (checkPos.y < -MaxY)
        {
            checkPos.y = -MaxY;
        }
    }
}
