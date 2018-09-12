using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    butterfly,
}

public class RAnimal : MonoBehaviour {
    public enum State
    {
        Idle,
        PlayerEnter
    }
    public AnimationType m_animationType;
    [SerializeField]
    private float m_speed = 1f;
    [Range(1,5)]
    [SerializeField]
    private float m_randTime;
    [SerializeField]
    private float m_checkPlayerTime = 1f;
    [SerializeField]
    private float m_changePosDistance = 1f;

    private float m_curTime = 0;
    private State m_state = State.Idle;
    public Transform[] m_randTransformList;
    private Transform m_player;
    private bool m_nowChangePos = false;
    private bool isCanMove = true;
    private Coroutine micro;


    protected  void Start()
    {
        m_player = GameObject.FindWithTag("Player").transform;
        transform.position = m_randTransformList[Random.Range(0, m_randTransformList.Length)].position;
        InvokeRepeating("CheckPlayerPos", 1, m_checkPlayerTime);

    }

    void Update()
    {
        m_curTime += Time.deltaTime;
        if(m_curTime>m_randTime || m_nowChangePos)
        {
            if(isCanMove)
            {
                if (micro != null)
                    StopCoroutine(micro);
                micro = StartCoroutine(SmoothMove());
                m_nowChangePos = false;
                m_curTime = 0;
                isCanMove = false;
            }
        }
    }

    IEnumerator SmoothMove()
    {
        Vector3 lastpos = m_randTransformList[Random.Range(0, m_randTransformList.Length)].position;
        float sqrRemainingDistance = (transform.position - lastpos).sqrMagnitude;
        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > 0.001f)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            transform.position = Vector3.MoveTowards(transform.position, lastpos, m_speed * Time.deltaTime);


            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - lastpos).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        isCanMove = true;
    }

    void CheckPlayerPos()
    {
        if(Vector3.Distance(m_player.position,transform.position)<m_changePosDistance)
        {
            m_nowChangePos = true;
            isCanMove = true;
        }else
        {
            m_nowChangePos = false;
        }
    }

}
