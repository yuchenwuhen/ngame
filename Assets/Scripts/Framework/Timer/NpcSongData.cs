using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class NpcSongData : MonoBehaviour {

    public float m_startTimePoint;
    public float m_endTimePoint;
    public float m_delayTimePoint = 0;

    private Animator m_animator;
    private byte isCheckStart = 0;
    private Coroutine m_scro;

    private void Start()
    {
        m_animator = this.GetComponent<Animator>();
        m_animator.enabled = false;
    }

    /// <summary>
    /// 检测时间
    /// </summary>
    /// <param name="curtime"></param>
    public void CheckStatusTime(float curtime)
    {
        if(isCheckStart==0)
        {
            if (Mathf.Abs(curtime - m_startTimePoint) < 0.02f)
            {
                isCheckStart = 1;
                if(m_delayTimePoint==0)
                {
                    StartTimeReach();
                }
                else
                {
                    m_scro = StartCoroutine(StartTimeReachIEnumetor());
                }
            }
        }else if(isCheckStart == 1)
        {
            if (Mathf.Abs(curtime - m_endTimePoint) < 0.02f)
            {
                isCheckStart = 2;
                StopTimeReach();
            }
        }
    }

    private void StartTimeReach()
    {
        m_animator.enabled = true;
    }

    private IEnumerator StartTimeReachIEnumetor()
    {
        while(isCheckStart!=2)
        {
            m_animator.enabled = true;
            Debug.Log(m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            yield return new WaitForSeconds(0.15f);
            m_animator.enabled = false;
            yield return new WaitForSeconds(m_delayTimePoint-0.15f);
        }
    }

    private void StopTimeReach()
    {
        m_animator.enabled = false;
        if(m_scro!=null)
        {
            StopCoroutine(m_scro);
        }
    }
}
