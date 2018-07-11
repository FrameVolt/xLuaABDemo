using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

// hold the luaenv instance, add lua searching path
public class LuaManager : Singleton<LuaManager> {

    private static LuaEnv luaEnv;

    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 

    public LuaEnv LuaEnv {
        get {
            if (luaEnv == null) {
                luaEnv = new LuaEnv();
                AddCustomLuaPath();
            }
            return luaEnv;
        }
    }

    //add lua searching path
    private void AddCustomLuaPath() {
        luaEnv.AddLoader((ref string filepath) => {
            if (AppConst.useLocalLua == true)
            {
                filepath = Application.dataPath + "/lua/" + filepath.Replace('.', '/') + ".lua.txt";
            }
            else {
                filepath = AppConst.DataPath + "/lua/" + filepath.Replace('.', '/') + ".lua.txt";
            }
            if (File.Exists(filepath))
            {
                return File.ReadAllBytes(filepath);
            }
            else
            {
                return null;
            }
        });
    }

    private void Update()
    {
        if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
        {
            LuaEnv.Tick();
            LuaBehaviour.lastGCTime = Time.time;
        }
    }

}
