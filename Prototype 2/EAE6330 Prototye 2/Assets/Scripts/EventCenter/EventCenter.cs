using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_eventTable = new Dictionary<EventType, Delegate>();

    private static void OnAddingListener(EventType i_eventType, Delegate i_callBack)
    {
        if(!m_eventTable.ContainsKey(i_eventType))
        {
            m_eventTable.Add(i_eventType, null);
        }
        Delegate d = m_eventTable[i_eventType];
        if (d != null && d.GetType() != i_callBack.GetType())
        {
            throw new Exception(string.Format("Added Different Types of Delegate For Event: {0}，The Current Event's Delegate is: {1}，The Delegate Being Added is: {2}", i_eventType, d.GetType(), i_callBack.GetType()));
        }
    }

    private static void OnRemovingListener(EventType i_eventType, Delegate callBack)
    {
        if (m_eventTable.ContainsKey(i_eventType))
        {
            Delegate d = m_eventTable[i_eventType];
            if (d == null)
            {
                throw new Exception(string.Format("Error When Removing Event Listener: Event: {0} Has No Delegate", i_eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("Error When Removing Event Listener: Trying To Remove Different Delegate Type For Event: {0}，Current Delegate Type: {1}，Removing Delegate Type: {2}", i_eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("Error When Removing Event Listener: Event Code Not Found: {0}", i_eventType));
        }
    }

    private static void OnListenerRemoved(EventType i_eventType)
    {
        if (m_eventTable[i_eventType] == null)
        {
            m_eventTable.Remove(i_eventType);
        }
    }

    //no parameters
    public static void AddListener(EventType i_eventType, CallBack i_callBack)
    {
        OnAddingListener(i_eventType, i_callBack);
        m_eventTable[i_eventType] = (CallBack)m_eventTable[i_eventType] + i_callBack;
    }
    //Single parameters
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnAddingListener(eventType, callBack);
        m_eventTable[eventType] = (CallBack<T>)m_eventTable[eventType] + callBack;
    }
    //two parameters
    public static void AddListener<T, X>(EventType eventType, CallBack<T, X> callBack)
    {
        OnAddingListener(eventType, callBack);
        m_eventTable[eventType] = (CallBack<T, X>)m_eventTable[eventType] + callBack;
    }


    //no parameters
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        OnRemovingListener(eventType, callBack);
        m_eventTable[eventType] = (CallBack)m_eventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    //single parameters
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnRemovingListener(eventType, callBack);
        m_eventTable[eventType] = (CallBack<T>)m_eventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    //two parameters
    public static void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack)
    {
        OnRemovingListener(eventType, callBack);
        m_eventTable[eventType] = (CallBack<T, X>)m_eventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }



    //no parameters
    public static void Broadcast(EventType eventType)
    {
        Delegate d;
        if (m_eventTable.TryGetValue(eventType, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
    //single parameters
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if (m_eventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
    //two parameters
    public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
    {
        Delegate d;
        if (m_eventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X> callBack = d as CallBack<T, X>;
            if (callBack != null)
            {
                callBack(arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
}