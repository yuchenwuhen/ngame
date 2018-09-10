using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFsm : MonoBehaviour {

    public enum States
    {
        Hangout,
        Chase,
        Attack,
        BeAttacked,
        Death
    }
    private StateMachine<States> m_fsm;

    private void Awake()
    {
        m_fsm = StateMachine<States>.Initialize(this);
        m_fsm.ChangeState(States.Hangout);
    }

    void Hangout_Enter()
    {
        //四处闲逛
        
    }

    void ReceiveMessage(IMessage message)
    {

    }

    void RegisterHandler()
    {
        MessageDispatcher.AddListener(MessageDefines.MsgType_UI, ReceiveMessage);
    }
    
}
