using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;



public static class Util {

    public static void Recursive(string path, List<string> files, List<string> paths, string extName = "*.*")
    {
        string[] names = Directory.GetFiles(path, extName);
        string[] dirs = Directory.GetDirectories(path);
        foreach (var item in names)
        {
            string ext = Path.GetExtension(item);

            if (ext.Equals(".meta")) continue;

            files.Add(item.Replace('\\', '/'));
        }
        foreach (var item in dirs)
        {
            paths.Add(item.Replace('\\', '/'));
            Recursive(item, files, paths, extName);
        }
    }

    public static string MD5File(string file)
    {

        FileStream fs = new FileStream(file, FileMode.Open);

        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(fs);
        fs.Close();


        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString());
        }
        return sb.ToString();
    }
}
