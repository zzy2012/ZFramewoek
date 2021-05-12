/****************************************************
文件：GetResPerfabsPath.cs
作者：EDZ/#Author#
邮箱：https://blog.csdn.net/u014361280 
日期：2021/02/25 18:23:39
功能：Nothing
描述：Nothing
*****************************************************/

//====================================
//* Author :xianren
//* CreateTime :2020-12-07 15:06:32
//* Version :
//* Description :
//==================================== 
/****************************************************
文件：AddPath.cs
作者：jieyo20200420/xianren
邮箱：https://blog.csdn.net/u014361280 
日期：2020/12/07 15:06:32
功能：Nothing
描述：Nothing
*****************************************************/

using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AutoBuildPath
{
    public static string UIClass =
 @"using UnityEngine;
using System;
public class IMTGMPrefabsPath 
{
   #成员#
}
";
}


public class GetResPerfabsPath
{
    static string path = "Assets/Resources/IMTGMPrefabsPath";

    static string pathPrefabsCS = Application.dataPath + "/Scripts/Const/Params/" + "IMTGMPrefabsPath" + ".cs";

    [MenuItem("Tools/UITools/预制体的路径")]
    static void LugsPrefabs()
    {
        GetAllPrefabs(path);
    }

    static void GetAllPrefabs(string directory)
    {
        if (string.IsNullOrEmpty(directory) || !directory.StartsWith("Assets"))
            throw new ArgumentException("folderPath");

        string[] subFolders = Directory.GetDirectories(directory);
        string[] guids = null;
        string[] assetPaths = null;
        int i = 0, iMax = 0;
       
        if (!Directory.Exists(GetResFolderPath.pathParams))
            Directory.CreateDirectory(GetResFolderPath.pathParams);
        if (File.Exists(pathPrefabsCS))
            File.Delete(pathPrefabsCS);
        FileStream classfile = new FileStream(pathPrefabsCS, FileMode.Create);
        classfile.Close();

        foreach (var folder in subFolders)
        {
            Debug.LogError(folder);
            guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folder });
            assetPaths = new string[guids.Length];
            string scriptPath = pathPrefabsCS;
            FileStream file = new FileStream(scriptPath, FileMode.Open);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            StringBuilder build = new StringBuilder();
            for (i = 0, iMax = assetPaths.Length; i < iMax; ++i)
            {
                string fullPath = "";
                string pathName = "";
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                assetPaths[i] = assetPaths[i].Replace(".prefab", "").Replace("Assets/Resources/", "");
                pathName = assetPaths[i].Replace("/", "_");
                fullPath = "public const string " + pathName + " = " + "\"" + assetPaths[i] + "\"" + ";\r\n\t";
                //Debug.Log(assetPaths[i]);
                build.Append(fullPath);
            }
            AutoBuildPath.UIClass = AutoBuildPath.UIClass.Replace("#成员#", build.ToString());
            fileW.Write(AutoBuildPath.UIClass);
            fileW.Flush();
            fileW.Close();
            file.Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogError("生成成功!");
        }
    }
}


