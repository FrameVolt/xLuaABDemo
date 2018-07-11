using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

[LuaCallCSharp]
public static class MyMonoBehaviour {

    private static AssetBundle manifestBundle;
    private static AssetBundleManifest manifest;

    public static void GetAsset<T>(this MonoBehaviour mono, string bundleName, string objName, Action<T> OnFinish) where T : UnityEngine.Object
    {
        mono.StartCoroutine(CreatebyName<T>(mono, bundleName, objName, OnFinish));
    }

    private static IEnumerator CreatebyName<T>(MonoBehaviour mono, string bundleName, string objName, Action<T> OnFinish) where T:UnityEngine.Object
    {
        // 先下载Manifest 文件
        yield return mono.StartCoroutine(AssetBundleManager.DownLoadAssstBundle(Path.Combine(AppConst.DataPath, "StreamingAssets")));
        manifestBundle = AssetBundleManager.GetAssetBundle(Path.Combine(AppConst.DataPath, "StreamingAssets"));
        manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
        string[] depends = manifest.GetAllDependencies(bundleName);

        //下载依赖包
        AssetBundle[] dependsAssetBundle = new AssetBundle[depends.Length];

        for (int i = 0; i < depends.Length; i++)
        {
            yield return mono.StartCoroutine(AssetBundleManager.DownLoadAssstBundle(AppConst.DataPath + depends[i]));
            dependsAssetBundle[i] = AssetBundleManager.GetAssetBundle(AppConst.DataPath + depends[i]);
        }

        //下载正真的Bundle
        yield return mono.StartCoroutine(AssetBundleManager.DownLoadAssstBundle(AppConst.DataPath + bundleName));
        AssetBundle ab = AssetBundleManager.GetAssetBundle(AppConst.DataPath + bundleName);

        T result = null;
        //通过Bundle创建游戏物体
        if (ab)
        {
            result = ab.LoadAsset<T>(objName);
        }

        OnFinish(result);
    }
}
