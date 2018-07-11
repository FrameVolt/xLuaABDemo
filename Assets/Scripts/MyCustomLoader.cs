using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

public class MyCustomLoader : MonoBehaviour {
    LuaEnv luaenv = null;
    // Use this for initialization
    void Start()
    {
        luaenv = new LuaEnv();
        luaenv.AddLoader((ref string filepath) => {
            filepath = Application.dataPath + "/MyLuaPath/" + filepath.Replace('.', '/') + ".lua.txt";
            print(filepath);
            if (File.Exists(filepath))
            {
                return File.ReadAllBytes(filepath);
            }
            else
            {
                return null;
            }
        });

        luaenv.DoString(@"
            require 'MyTestLoaderLua'
        ");
        luaenv.Dispose();
    }

}
