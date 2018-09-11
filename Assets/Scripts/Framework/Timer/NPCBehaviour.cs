using UnityEngine;
using System.Collections;

public class NPCBehaviour : MonoBehaviour
{
    public Vector3 m_beginPos;  // 起点位置
    public Vector3 m_endPos;    // 终点位置

    private float m_distance;   // 两个位置的距离
    private float m_limitTime;  // 限制的时间

    private bool m_bIsMove;

    private bool m_bIsIvaild = true; // 该NPC是否可用
    // Use this for initialization
    void Start()
    {
        m_distance = Vector3.Distance(m_beginPos, m_endPos);
        m_bIsMove = false;
        //distance = Vector3.Distance(transform.position, target);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("init pos:" + transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, target, 
        //                                        (distance / 1f) * Time.deltaTime);

        if (m_bIsMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_endPos,
                                                     (m_distance / m_limitTime) * Time.deltaTime);
        }
    }

    public void BeginMove(float limitTime)
    {
        transform.position = m_beginPos;
        m_limitTime = limitTime;
        m_bIsMove = true;
    }

    public void EndMove()
    {
        m_bIsMove = false;
    }

}
