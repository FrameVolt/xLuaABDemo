using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class AssetBundleBuilder : Editor {

    static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    [MenuItem("AssetBundle/Build asset bundles Windows By Path")]
    private static void BuildWindowsAB() {
        BuildAB(BuildTarget.StandaloneWindows);
    }
    [MenuItem("AssetBundle/Build asset bundles Android By Path")]
    private static void BuildAndroidAB()
    {
        BuildAB(BuildTarget.Android);
    }


    private static void BuildAB(BuildTarget buildTarget)
    {
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.Delete(Application.streamingAssetsPath, true);
        }

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        AddBuildMap("PNGBundle", "*.png");
        AddBuildMap("PrefabBundle", "*.prefab");

        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, maps.ToArray(), BuildAssetBundleOptions.None, buildTarget);
        PackageLuaFiles();
        BuildFileIndex();
        AssetDatabase.Refresh();
        Debug.Log("Build success");
    }

    private static void PackageLuaFiles()
    {
        paths.Clear();
        files.Clear();
        Util.Recursive(CustomSetting.luaPath, files, paths);

        if (!Directory.Exists(Application.streamingAssetsPath + "/Lua"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Lua");

        foreach (var item in paths)
        {
            string targetPath = item.Replace(Application.dataPath, Application.streamingAssetsPath);
            Directory.CreateDirectory(targetPath);
        }
        foreach (var item in files)
        {
            byte[] bytes = File.ReadAllBytes(item);
            string targetPath = item.Replace(Application.dataPath, Application.streamingAssetsPath);
            File.WriteAllBytes(targetPath, bytes);
        }
    }


    static void AddBuildMap(string bundleName, string extName) {
        AssetBundleBuild abb = new AssetBundleBuild();
        abb.assetBundleName = bundleName;
        paths.Clear();
        files.Clear();
        Util.Recursive(CustomSetting.resourceRawPath, files, paths, extName);

        for (int i = 0; i < files.Count; i++)
        {
            files[i] = files[i].Replace(Application.dataPath, "Assets");
        }

        abb.assetNames = files.ToArray();
        maps.Add(abb);
    }

    static void BuildFileIndex() {
        string newFilePath = Application.streamingAssetsPath + "/files.txt";

        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        paths.Clear();
        files.Clear();
        Util.Recursive(Application.streamingAssetsPath, files, paths);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);

        for (int i = 0; i < files.Count; i++)
        {
            if (files[i].EndsWith(".meta") || files[i].Contains(".DS_Store")) continue;
            string value = files[i].Replace(Application.streamingAssetsPath + "/",string.Empty);

            string md5 = Util.MD5File(files[i]);
            sw.WriteLine(value + "|" + md5);
        }

        sw.Close();
        fs.Close();
    }
}
