/****************************************************
文件：GetResTexturesPath.cs
作者：EDZ/#Author#
邮箱：https://blog.csdn.net/u014361280 
日期：2021/02/25 18:25:07
功能：Nothing
描述：Nothing
*****************************************************/

/****************************************************
文件：GetResTexturesPath.cs
作者：EDZ/#Author#
邮箱：https://blog.csdn.net/u014361280 
日期：2021/02/06 11:07:51
功能：Nothing
描述：Nothing
*****************************************************/

using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AutoBuildTexturePath
{
    public static string UIClass =
 @"using UnityEngine;
using System;
public class IMTGMTexturesPath 
{
   #成员#
}
";
}



public class GetResTexturesPath
{
    static string path = "Assets/Resources/Texture";

    static string pathPrefabsCS = Application.dataPath + "/Scripts/Const/Params/" + "IMTGMTexturesPath" + ".cs";

    [MenuItem("Tools/UITools/图片的路径")]
    static void LugsRTextures()
    {
        GetAllTextures(path);
    }

    private static void GetAllTextures(string directory)
    {
        if (string.IsNullOrEmpty(directory) || !directory.StartsWith("Assets"))
            throw new ArgumentException("folderPath");

        string[] subFolders = Directory.GetDirectories(directory,"xrPath");
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
            //Debug.LogError(folder);
            guids = AssetDatabase.FindAssets("t:Sprite", new string[] { folder });
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
                assetPaths[i] = assetPaths[i].Replace(".png", "").Replace(".jpg","").Replace("Assets/Resources/", "");
                pathName = assetPaths[i].Replace("/", "_");
                fullPath = "public const string " + pathName + " = " + "\"" + assetPaths[i] + "\"" + ";\r\n\t";
                //Debug.Log(assetPaths[i]);
                build.Append(fullPath);
            }
            AutoBuildTexturePath.UIClass = AutoBuildTexturePath.UIClass.Replace("#成员#", build.ToString());
            fileW.Write(AutoBuildTexturePath.UIClass);
            fileW.Flush();
            fileW.Close();
            file.Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogError("生成成功!");
        }
    }
}
