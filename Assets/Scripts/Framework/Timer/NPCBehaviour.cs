using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NPCBehaviour : MonoBehaviour
{
    public Vector3 m_beginPos;  // 起点位置
    public Vector3 m_endPos;    // 终点位置
    public Vector3 m_LastEndOffset; // 超时的最远移动位置与终点位置的偏移
    public float m_speed;       //移动速度
    public Sprite[] m_randomSprite;

    private bool m_bIsMove = false; // 是否移动
    private Vector3 m_LastEndPos; // 最远的距离

    private int m_iStyle;

    // Use this for initialization
    void Start()
    {
        m_LastEndPos = m_endPos + m_LastEndOffset;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("init pos:" + transform.position);
    }

    public void BeginMove(float pointTime)
    {
        Debug.Log("BeginMove, pointTime:" + pointTime);
        transform.position = new Vector3(m_endPos.x - m_speed * pointTime, m_endPos.y, m_endPos.z);
        Debug.Log("BeginMove, pos:" + transform.position);
        gameObject.SetActive(true);

        m_bIsMove = true;
        m_iStyle = 0;

        Image img = gameObject.GetComponent<Image>();

        img.sprite = m_randomSprite[Random.Range(0,3)];
        //img.SetNativeSize();
    }

    public void Move(float time)
    {
        if (m_bIsMove)
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
