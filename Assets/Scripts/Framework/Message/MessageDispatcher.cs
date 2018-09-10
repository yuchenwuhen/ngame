using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用来给监听信息者收到信息后执行的处理。
/// 注册消息:
/// 
/// </summary>
/// <param name="rMessage"></param>
public delegate void MessageHandler(IMessage rMessage);

public class MessageDispatcher 
{
    /// <summary>
    /// 创建MessageDispatcher在Unity的存根，专门处理延迟信息的发送
    /// </summary>
    private static MessageDispatcherStub m_Stub;

    /// <summary>
    /// 当某些信息没有监听的时候，信息发送的处理由该委托处理。
    /// </summary>
    public static MessageHandler MessageNotHandled = null;

    /// <summary>
    /// 存储标识着延迟发送的信息
    /// </summary>
    private static List<IMessage> m_Messages = new List<IMessage>();

    /// <summary>
    /// 主要存储信息对应的处理
    /// 第一个string是信息的标识，第二个string是监听过滤信息的标识，第三个是信息发送成功的处理
    /// </summary>
    private static Dictionary<string, Dictionary<string, MessageHandler>> m_MessageHandlers = new Dictionary<string, Dictionary<string, MessageHandler>>();

    public MessageDispatcher()
    {
        if(m_Stub==null)
            m_Stub = (new GameObject("MessageDispatcherStub")).AddComponent<MessageDispatcherStub>();
    }

    /// <summary>
    /// 清除所有延迟信息的列表
    /// </summary>
    public static void ClearMessages()
    {
        m_Messages.Clear();
    }

    /// <summary>
    /// 添加一个对信息的监听
    /// </summary>
    /// <param name="rMessageType">监听标识</param>
    /// <param name="rHandler">处理</param>
    /// <param name="rFilter">所监听信息过滤标识</param>
    public static void AddListener(string rMessageType,MessageHandler rHandler,string rFilter = "")
    {
        Dictionary<string, MessageHandler> RecipientDictionary = null;

        if(m_MessageHandlers.ContainsKey(rMessageType))
        {
            RecipientDictionary = m_MessageHandlers[rMessageType];
        }
        else//没有则创建这个信息标识
        {
            RecipientDictionary = new Dictionary<string, MessageHandler>();
            m_MessageHandlers.Add(rMessageType, RecipientDictionary);
        }

        //检查信息过滤集合是否包含过滤信息
        if(!RecipientDictionary.ContainsKey(rFilter))
        {
            RecipientDictionary.Add(rFilter, null);
        }
        RecipientDictionary[rFilter] += rHandler;
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="rMessage"></param>
    public static void SendMessage(IMessage rMessage)
    {
        //是否丢失接收信息的监听者，默认true
        bool ReportMissingRecipient = true;

        //如果信息有延迟时间则把该信息加入到延迟信息列表中等待触发
        if (rMessage.Delay > 0)
        {
            if (!m_Messages.Contains(rMessage))
            {
                m_Messages.Add(rMessage);
            }
            //避免出发下面的监听丢失监听者代码块
            ReportMissingRecipient = false;
            }
        //如果延迟直接查看下信息相应处理列表是否包含这条信息
        else if(m_MessageHandlers.ContainsKey(rMessage.AreaCode))
        {
            Dictionary<string, MessageHandler> lHandlers = m_MessageHandlers[rMessage.AreaCode];

            //遍历信息过滤的标志
            foreach(string lFilter in lHandlers.Keys)
            {
                if(lHandlers[lFilter]==null)
                {
                    continue;
                }
                if(lFilter.Equals(rMessage.Filter))
                {
                    lHandlers[lFilter](rMessage);//执行
                    rMessage.IsSent = true;
                    ReportMissingRecipient = false;
                }
            }
        }

        if(ReportMissingRecipient)
        {
            if(MessageNotHandled == null)
            {
                Debug.LogWarning(string.Format("MessageDispatcher can't handle Message.Type:{0} or Message.Fileter:{0}", rMessage.AreaCode, rMessage.Filter));
            }
            else
            {
                MessageNotHandled(rMessage);
            }
        }
    }

    /// <summary>
    /// 默认参数只发送，无参数，适用于弹出UI
    /// </summary>
    /// <param name="messageDefines"></param>
    public static void SendMessage(string messageType)
    {
        Message message = new Message();
        message.AreaCode = messageType;
        MessageDispatcher.SendMessage(message);
    }

    public static void Update()
    {
        //处理延迟信息列表
        for (int i = m_Messages.Count-1; i>=0;--i)
        {
            IMessage lMessage = m_Messages[i];
            lMessage.Delay -= Time.deltaTime;
            if (lMessage.Delay<0)
            {
                lMessage.Delay = 0;
            }
            if(!lMessage.IsSent&&lMessage.Delay == 0)
            {
                SendMessage(lMessage);
                m_Messages.RemoveAt(i);
            }
        }
    }

    public static void OnDisable()
    {
        m_Stub.enabled = false;
    }
}


public sealed class MessageDispatcherStub:MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        MessageDispatcher.Update();
    }
    public void OnDisable()
    {
        MessageDispatcher.ClearMessages();
    }
}

