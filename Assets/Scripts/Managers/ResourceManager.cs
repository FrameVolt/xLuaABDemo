using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class ResourceManager : MonoBehaviour {



    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();


    void Start () {
        if (AppConst.IsDebugMode == true)
        {
            return;
        }
        if (PlayerPrefs.GetInt("FirstLoad") == 0)
            StreamingAssetsToDataPath();

        StartCoroutine(UpdateResource(OnUpdateFinish));
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        
        if (GUI.Button(new Rect(20, 20, 150, 30), "MoveToDataPath")) {
            StreamingAssetsToDataPath();
        }

        if (GUI.Button(new Rect(20, 60, 150, 30), "UpdateResource"))
        {
            StartCoroutine(UpdateResource(OnUpdateFinish));
        }
    }
    #endif
    private void StreamingAssetsToDataPath() {
        if (!Directory.Exists(Application.streamingAssetsPath)) {
            Debug.Log("nothing need move to DataPath.");
            return;
        }

        if (!Directory.Exists(AppConst.DataPath))
            Directory.CreateDirectory(AppConst.DataPath);

        Util.Recursive(Application.streamingAssetsPath, files, paths);

        foreach (var item in paths)
        {
            string targetPath = item.Replace(Application.streamingAssetsPath, AppConst.DataPath);
            Directory.CreateDirectory(targetPath);
        }

        foreach (var item in files)
        {
            byte[] bytes = File.ReadAllBytes(item);
            string targetPath = item.Replace(Application.streamingAssetsPath, AppConst.DataPath);
            File.WriteAllBytes(targetPath, bytes);
        }

        PlayerPrefs.SetInt("FirstLoad", 1);
        Debug.Log("Move finish");
    }

    //跟服务器比对files.txt文件内的MD5值，来更新资源包
    private IEnumerator UpdateResource(Action onUpdateFinish) {
        
        string webListFileUrl = AppConst.WebUrl + "files.txt";

        UnityWebRequest request = UnityWebRequest.Get(webListFileUrl);

        request.SendWebRequest();

        while (!request.isDone)
        {
            yield return null;
        }

        if (request.error != null)
        {
            OnUpdateFailed(request.error);
            yield break;
        }

        string webFilesText = request.downloadHandler.text;

        string[] webFilesWithMD5 = webFilesText.Split('\n');
        foreach (var item in webFilesWithMD5)
        {
            if (string.IsNullOrEmpty(item)) continue;

            string[] files = item.Split('|');

            string webPath = AppConst.WebUrl + files[0];
            string localPath = AppConst.DataPath + "/" + files[0];

            bool canUpdate = !File.Exists(localPath);
            
            if (!canUpdate)
            {
                string remoteMD5 = files[1].Trim();
                string localMD5 = Util.MD5File(localPath);
                canUpdate = !remoteMD5.Equals(localMD5);
                if (canUpdate) File.Delete(localPath);
            }

            if (canUpdate)
            {
                yield return StartCoroutine(Load(webPath, localPath));
            }
        }

        onUpdateFinish.Invoke();
    }

    private IEnumerator Load(string webPath, string localPath)
    {
        UnityWebRequest request = UnityWebRequest.Get(webPath);
        request.SendWebRequest();
        while (!request.isDone)
        {
            yield return null;
        }

        if (request.error != null)
        {
            OnUpdateFailed(request.error);
            yield break;
        }
        DownloadHandler handler = request.downloadHandler;
        File.WriteAllBytes(localPath, handler.data);
        Debug.Log("Load file " + localPath + " is done.");
    }

    private void OnUpdateFinish() {
        Debug.Log("Update Finish");
        SceneManager.LoadScene("MainGame");
    }

    private void OnUpdateFailed(string error)
    {
        Debug.Log("!!!!!!!!!!" + error);
        SceneManager.LoadScene("MainGame");
    }
}
