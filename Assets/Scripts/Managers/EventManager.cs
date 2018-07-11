using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{


    //private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();


    //public static void StartListening(string eventName, Action listener)
    //{
    //    Action thisEvent = null;
    //    if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    //    {
    //        thisEvent += listener;
    //    }
    //    else
    //    {
    //        //thisEvent = new Action(null);
    //        thisEvent += listener;
    //        Instance.eventDictionary.Add(eventName, thisEvent);
    //    }
    //}

    //public static void StopListening(string eventName, Action listener)
    //{
    //    if (Instance == null) return;
    //    Action thisEvent = null;
    //    if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    //    {
    //        thisEvent -= listener;
    //    }
    //}

    //public static void TriggerEvent(string eventName)
    //{
    //    Action thisEvent = null;
    //    if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    //    {
    //        thisEvent.Invoke();
    //    }
    //}
    //public static void ClearEvent(string eventName)
    //{
    //    Action thisEvent = null;
    //    if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
    //    {
    //        Instance.eventDictionary.Remove(eventName);
    //    }
    //}

    //public static void ClearAllEvents()
    //{
    //    Instance.eventDictionary.Clear();
    //}

    protected static IDictionary<string, IList<IObserver>> m_observerMap = new Dictionary<string, IList<IObserver>>();


    public static void StartListening(string notificationName, IObserver observer)
    {
        if (!m_observerMap.ContainsKey(notificationName))
        {
            m_observerMap[notificationName] = new List<IObserver>();
        }
        m_observerMap[notificationName].Add(observer);
    }

    public static void StopListening(string notificationName, object notifyContext)
    {
        if (m_observerMap.ContainsKey(notificationName))
        {
            IList<IObserver> list = m_observerMap[notificationName];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CompareNotifyContext(notifyContext))
                {
                    list.RemoveAt(i);
                    break;
                }
            }
            if (list.Count == 0)
            {
                m_observerMap.Remove(notificationName);
            }
        }
    }
    public static void TriggerEvent(INotification notification)
    {
        IList<IObserver> list = null;
       
        if (m_observerMap.ContainsKey(notification.Name))
        {
            IList<IObserver> collection = m_observerMap[notification.Name];
            list = new List<IObserver>(collection);
        }

        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].NotifyObserver(notification);
            }
        }
    }
}
