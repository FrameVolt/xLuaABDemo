using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public interface IObserver
{
    //对比NotifyContext
    bool CompareNotifyContext(object obj);
    //通知观察者
    void NotifyObserver(INotification notification);
    //记录是Mediator或Command
    object NotifyContext { set; }
    //通知方法
    string NotifyMethod { set; }
}

public class Observer : IObserver
{
    protected object m_notifyContext;
    protected string m_notifyMethod;
    protected readonly object m_syncRoot = new object();

    public Observer(string notifyMethod, object notifyContext)
    {
        this.m_notifyMethod = notifyMethod;
        this.m_notifyContext = notifyContext;
    }

    public bool CompareNotifyContext(object obj)
    {
        lock (this.m_syncRoot)
        {
            return this.NotifyContext.Equals(obj);
        }
    }

    public object NotifyContext
    {
        protected get
        {
            return this.m_notifyContext;
        }
        set
        {
            this.m_notifyContext = value;
        }
    }

    public string NotifyMethod
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

    public virtual void NotifyObserver(INotification notification)
    {
        object notifyContext;

        notifyContext = this.NotifyContext;
        //string notifyMethod = this.NotifyMethod;

        //利用反射获取方法然后执行
        Type type = notifyContext.GetType();
        //这里设置忽略字母的大小写
        //BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
        //根据设置的中介者的名字或者是命令的名字执行对应的方法，具体方法的执行在中介者和命令中已经重写对应的方法实现

        //NotifyMethod : Move方法
        MethodInfo method = type.GetMethod(this.NotifyMethod);


        //notifyContext:是InputManager 那个对象，notification.Body这是传递的数据
        method.Invoke(notifyContext, new object[] { notification.Body });



    }
}