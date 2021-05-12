using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EventEnum
{

}
/// <summary>
/// 事件管理
/// </summary>
public class EventMgr
{
    private static Dictionary<EventEnum, Action<object[]>> eventDict = new Dictionary<EventEnum, Action<object[]>>();
    public static void SendEvent(EventEnum eventName,params object[] param)
    {
        if (eventDict.ContainsKey(eventName))
        {
            if (null == eventDict[eventName])
                Debug.LogError(string.Format("提示：SendEvent:{0}有误，事件未AddEvent。",eventName));
            else
                eventDict[eventName](param);
        }
    }

    public static void AddEvent(EventEnum eventName,Action<object[]> cb)
    {
        if (eventDict.ContainsKey(eventName))
            eventDict[eventName] += cb;
        else
            eventDict[eventName] = cb;
    }

    public static void RemoveEvent(EventEnum eventName, Action<object[]> cb)
    {
        if (eventDict.ContainsKey(eventName))
            eventDict[eventName] -= cb;
    }
}
