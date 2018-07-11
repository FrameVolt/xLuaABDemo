using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class AssetBundleManager
{
    static public Dictionary<string, AssetBundle> dictAssetBundle;

    static AssetBundleManager()
    {
        dictAssetBundle = new Dictionary<string, AssetBundle>();
    }
    public static AssetBundle GetAssetBundle(string url)
    {
        string key = url;

        AssetBundle ab;
        if (dictAssetBundle.TryGetValue(key, out ab))
        {
            return ab;
        }
        else
        {
            Debug.LogWarning("not find assetBUndle in Dictionary");
            return null;
        }
    }
    //硬盘里取AssetBundle
    public static IEnumerator DownLoadAssstBundle(string url)
    {
        string key = url;

        if (!dictAssetBundle.ContainsKey(key))//Dictionary not found, load from dataPath
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(url);

            while (!request.isDone)
            {
                yield return null;
            }
            dictAssetBundle.Add(key, request.assetBundle);
        }
        else
        {
            Debug.Log("already loaded in memory, use Dictionary");
        }

    }

}
