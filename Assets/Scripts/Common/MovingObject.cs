using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
	public LayerMask blockingLayer;			//Layer on which collision will be checked.
		
	private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
	private float inverseMoveTime;			//Used to make movement more efficient.
		
		
	//Protected, virtual functions can be overridden by inheriting classes.
	protected virtual void Start ()
	{
			
		//Get a component reference to this object's Rigidbody2D
		rb2D = GetComponent <Rigidbody2D> ();
			
		//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
		inverseMoveTime = 1f / moveTime;
	}

    /// <summary>
    /// move by grid iswall mark
    /// </summary>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    /// <param name="hit"></param>
    /// <returns></returns>
    protected bool Move(Vector2Int dir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2Int start = TileManager.Instance.WorldPositionToGridPoint(transform.position);

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2Int end = start + dir;

        //Check if anything was hit
        if (!TileManager.Instance.IsWallByGrid(end))
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
               StartCoroutine(SmoothMovement(end));
               
            //Return true to say that Move was successful
            return true;
        }
        else
        {
            return false;
        }

    }


    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement (Vector2Int end)
	{
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        Vector3 endPos = TileManager.Instance.GridPointToWorldPosition(end);
		float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
			
		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while(sqrRemainingDistance > float.Epsilon)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, endPos, inverseMoveTime * Time.deltaTime);
				
			//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			rb2D.MovePosition (newPostion);
				
			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
				
			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			yield return null;
		}
	}
		
		
	//The virtual keyword means AttemptMove can be overridden by inheriting classes using the override keyword.
	//AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
	protected virtual void AttemptMove <T> (Vector2Int dir)
		where T : Component
	{
			
		//Set canMove to true if Move was successful, false if failed.
		Move (dir);
	}
		
		
	//The abstract modifier indicates that the thing being modified has a missing or incomplete implementation.
	//OnCantMove will be overriden by functions in the inheriting classes.
	protected abstract void OnCantMove <T> (T component)
		where T : Component;
}

