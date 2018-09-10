using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Message类相关信息的接口，就好像一条短信一样或者一个通知一样。
/// Message由MessageDispatcher类发送到监听对应信息者的对象中去。
/// 对象可以通过MessageDispatcher类传递一个命令或者一些数据给另一个或多个监听对应信息的类。
/// 可通过继承此接口自定义自己传递的数据信息
/// </summary>
public interface IMessage
{
    /// <summary>
    /// 信息标识，可以是任何值
    /// </summary>
    string AreaCode { get; set; }

    /// <summary>
    /// 事件码
    /// </summary>
    string EventCode { get; set; }


    /// <summary>
    /// 监听者的信息过滤标识
    /// </summary>
    string Filter { get; set; }

    /// <summary>
    /// 信息发送延迟时间,s
    /// </summary>
    float Delay { get; set; }

    /// <summary>
    /// 信息的核心数据
    /// </summary>
    object Data { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    int Priority { get; set; }

    /// <summary>
    /// 信息是否已发送
    /// </summary>
    bool IsSent { get; set; }

    /// <summary>
    /// 重置信息实例
    /// </summary>
    void Reset();
}

