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

    private Image m_imgSource;

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

    public void Init()
    {
        gameObject.SetActive(false);
        m_imgSource = gameObject.GetComponent<Image>();
    }

    private IEnumerator move(Vector3 startPos,  float time)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(startPos, m_endPos, dur / time);
            yield return null;
        }
    }

    public void BeginMove(Vector3 startPos, float time)
    {
        StartCoroutine(move(startPos, time));
    }

    public void BeginMove(float pointTime, int iStyle)
    {
        transform.position = new Vector3(m_endPos.x - m_speed * pointTime, m_endPos.y, m_endPos.z);

        gameObject.SetActive(true);
        m_bIsMove = true;

        m_imgSource.sprite = m_randomSprite[iStyle];
        m_imgSource.SetNativeSize();
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

    public float time = 0.5f;//代表从A点出发到B经过的时长
    public float g = -1000;//重力加速度

    private Vector3 speed;//初速度向量
    private Vector3 Gravity = Vector3.zero;//重力向量
    private bool m_IsParabolaMove = false;
    private Vector3 pointB;//点B
    private float dTime = 0;

    //通过一个式子计算初速度
    public void ParabolaMove(Vector3 dir)
    {
        m_bIsMove = false;
        Vector3 pointA = transform.position;
        pointB = transform.position + dir;
        speed = new Vector3((pointB.x - pointA.x) / time, (pointB.y - pointA.y) / time - 0.5f * g * time, 
                            (pointB.z - pointA.z) / time);
        m_IsParabolaMove = true;
        dTime = 0;
    }

    public void SuccessMove(float time)
    {
        if (m_IsParabolaMove)
        {
            Gravity.y = g * (dTime += time);//v=at
                                                           //模拟位移
            transform.Translate(speed * time);
            transform.Translate(Gravity * time);
            if (Vector3.Distance(transform.position, pointB) < 10f)
            {
                EndMove();
                m_IsParabolaMove = false;
            }
        }
    }
}
