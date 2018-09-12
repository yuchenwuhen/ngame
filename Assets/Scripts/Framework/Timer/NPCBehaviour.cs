using UnityEngine;
using System.Collections;

public class NPCBehaviour : MonoBehaviour
{
    public Vector3 m_beginPos;  // 起点位置
    public Vector3 m_endPos;    // 终点位置
    public Vector3 m_LastEndOffset; // 超时的最远移动位置与终点位置的偏移

    private float m_distance;   // 两个位置的距离
    private float m_limitTime;  // 限制的时间

    private bool m_bIsMove = true;

    //private bool m_bIsIvaild = true; // 该NPC是否可用

    public float m_speed;

    private Vector3 m_LastEndPos;
    // Use this for initialization
    void Start()
    {
        m_distance = Vector3.Distance(m_beginPos, m_endPos);

        m_LastEndPos = m_endPos + m_LastEndOffset;
        //m_bIsMove = false;
        //distance = Vector3.Distance(transform.position, target);

        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("init pos:" + transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, target, 
        //                                        (distance / 1f) * Time.deltaTime);

        if (m_bIsMove)
        {
            //transform.position = Vector3.MoveTowards(transform.position, m_endPos,
            //(m_distance / m_limitTime) * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, m_endPos, m_speed * Time.deltaTime);
        }
    }

    public void BeginMove(float pointTime)
    {
        Debug.Log("BeginMove, pointTime:" + pointTime);
        transform.position = new Vector3(m_endPos.x - m_speed * pointTime, m_endPos.y, m_endPos.z);
        Debug.Log("BeginMove, pos:" + transform.position);
        gameObject.SetActive(true);
        //m_limitTime = limitTime;
        m_bIsMove = true;
    }

    public void Move(float time)
    {

        {
            //Debug.Log("time:" + time);
            transform.position = Vector3.MoveTowards(transform.position, m_LastEndPos, m_speed * time);
            //Debug.Log("Move pos:" + transform.position);
        }

    }

    public void EndMove()
    {
        gameObject.SetActive(false);
        m_bIsMove = false;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }
}
