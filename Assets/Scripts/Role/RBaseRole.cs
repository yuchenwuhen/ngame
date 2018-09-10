using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class RBaseRole : MonoBehaviour {

    public int m_id;                        //ID
    public bool m_isDie;                    //是否死亡
    public string m_name;                   //名字
    private RoleMapData m_roleReadData;
    public List<string> m_dialogueTxt;

    private Vector3 startPosition;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        ResgisterHandler();
        Init();
    }

    protected virtual void Init()
    {
        //TODO 从RoleReadData中获取数据
        //使用临时数据
        
    }

    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
#if UNITY_EFITOR
        if(startPosition != transform.localPosition)
            Debug.Log("Test");
#endif
    }

    /// <summary>
    /// 发基本类型消息
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="eventCode"></param>
    /// <param name="data"></param>
    protected virtual void SendModuleMessage(string msgType,string eventCode,object data)
    {
        Message message = new Message();
        message.AreaCode = msgType;
        message.EventCode = eventCode;
        message.Data = data;
        MessageDispatcher.SendMessage(message);
    }

    /// <summary>
    /// 重载发消息
    /// </summary>
    /// <param name="message"></param>
    protected virtual void SendModuleMessage(Message message)
    {
        MessageDispatcher.SendMessage(message);
    }

    /// <summary>
    /// 注册接收消息函数
    /// </summary>
    protected virtual void ResgisterHandler()
    {
        MessageDispatcher.AddListener(MessageDefines.MsgType_Role, ReceiveMessage);
        MessageDispatcher.AddListener(MessageDefines.MsgType_Global, ReceiveGlobalMessage);
    }

    /// <summary>
    /// 接收全局消息
    /// </summary>
    /// <param name="message"></param>
    void ReceiveGlobalMessage(IMessage message)
    {
        Message mes = message as Message;

        switch (mes.EventCode)
        {
            default:
                break;
        }
    }

    /// <summary>
    /// 清晰状态遇到player
    /// </summary>
    protected virtual void DealWithPlayerOnClear()
    {

    }

    /// <summary>
    /// 模糊状态遇到player
    /// </summary>
    protected virtual void DealWithPlayerOnBlurry()
    {

    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="message"></param>
    protected virtual void ReceiveMessage(IMessage message)
    {
 
    }
}
