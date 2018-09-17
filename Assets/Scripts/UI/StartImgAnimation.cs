using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartImgAnimation : MonoBehaviour {

    [SerializeField]
    private float m_speed = 10;
    [SerializeField]
    private Vector3 m_dir;
    private bool isStartMove = true;
    private Vector3 m_startPosition;
    private void Start()
    {
        Invoke("CloseMove", 10f);
        m_startPosition = transform.position;
    }

    private void Update()
    {
        if(isStartMove)
        {
            float step = m_speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, m_startPosition+m_dir,step);
        }

    }

    private void CloseMove()
    {
        isStartMove = false;
        transform.position = m_startPosition;
    }
}
