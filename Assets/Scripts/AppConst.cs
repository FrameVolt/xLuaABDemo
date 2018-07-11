using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class AppConst{

    public const string AppName = "XluaWithAssetBundle";
    public const string WebUrl = @"http://127.0.0.1:8080/";
    public static bool IsDebugMode = true;
    public static bool useLocalLua = true;

    public static string DataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + AppName + "/";

            }
            else
            {
                return "c:/" + AppName + "/";
            }
        }
    }
}
