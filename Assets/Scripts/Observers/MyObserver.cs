using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class MyObserver : Observer
{
    public MyObserver(Action<Vector3> notifyMethod, object notifyContext) : base(String.Empty, notifyContext)
    {
        m_notifyMethod = notifyMethod;
    }

    protected new Action<Vector3> m_notifyMethod;

    public new Action<Vector3> NotifyMethod
    {
        private get
        {
            return this.m_notifyMethod;
        }
        set
        {
            this.m_notifyMethod = value;
        }
    }

    public override void NotifyObserver(INotification notification)
    {
        NotifyMethod((Vector3)notification.Body);
    }

}
