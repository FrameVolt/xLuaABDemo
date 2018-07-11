using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

[System.Serializable]
public class MyInjection
{
    public string name;
    public GameObject value;
}

[LuaCallCSharp]
public class MyLuaBehaviour : MonoBehaviour
{
    public string luaScript;
    //public TextAsset luaScript;
    public MyInjection[] injections;

    internal static LuaEnv luaEnv; //all lua behaviour shared one luaenv only!

    private Action luaStart;
    private Action luaUpdate;
    private Action luaOnDestroy;
    private LuaTable scriptEnv;

    void Awake()
    {
        if(luaEnv == null)
        luaEnv = LuaManager.Instance.LuaEnv;
        scriptEnv = luaEnv.NewTable();

        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("self", this);
        foreach (var injection in injections)
        {
            scriptEnv.Set(injection.name, injection.value);
        }

        //luaEnv.DoString(luaScript.text, "MyLuaBehaviour", scriptEnv);
        luaEnv.DoString(string.Format("require '{0}'",luaScript), "MyLuaBehaviour", scriptEnv);



        Action<MonoBehaviour> luaAwake = scriptEnv.Get<Action<MonoBehaviour>>("awake");
        scriptEnv.Get("start", out luaStart);
        scriptEnv.Get("update", out luaUpdate);
        scriptEnv.Get("ondestroy", out luaOnDestroy);
        if (luaAwake != null)
        {
            luaAwake(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
    }

    void OnDestroy()
    {
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
        scriptEnv.Dispose();
        injections = null;
    }
}
