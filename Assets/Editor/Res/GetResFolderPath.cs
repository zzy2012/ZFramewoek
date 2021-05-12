/****************************************************
文件：GetResFolderPath.cs
作者：EDZ/#Author#
邮箱：https://blog.csdn.net/u014361280 
日期：2021/02/25 18:24:55
功能：Nothing
描述：Nothing
*****************************************************/

//====================================
//* Author :xianren
//* CreateTime :2020-12-08 20:19:20
//* Version :
//* Description :
//==================================== 
/****************************************************
文件：GetResPath.cs
作者：jieyo20200420/xianren
邮箱：https://blog.csdn.net/u014361280 
日期：2020/12/08 20:19:20
功能：Nothing
描述：Nothing
*****************************************************/
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class AutoBuildPathFolder
{
    public static string UIClass =
 @"using UnityEngine;
using System;
public class IMTGMFoldPath 
{
   #成员#
}
";
}


public class GetResFolderPath
{
    public static string pathParams = Application.dataPath + "/Scripts/Const/Params";

    static string pathFolderCS = Application.dataPath + "/Scripts/Const/Params/" + "IMTGMFoldPath" + ".cs";

    [MenuItem("Tools/UITools/文件夹的路径")]
    static void LugsFolds()
    {
        Dictionary<string, string> PathDict = new Dictionary<string, string>();
        //string fullPath = "Assets/Models/21/21000001" + "/";  //路径
        string directory = "Assets/Resources"+ " / ";  //路径
        if (string.IsNullOrEmpty(directory) || !directory.StartsWith("Assets"))
            throw new ArgumentException("folderPath");

        if (!Directory.Exists(pathParams))
            Directory.CreateDirectory(pathParams);
        if (File.Exists(pathFolderCS))
            File.Delete(pathFolderCS);
       
        FileStream classfile = new FileStream(pathFolderCS, FileMode.Create);
        classfile.Close();

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(directory))
        {
            DirectoryInfo direction = new DirectoryInfo(directory);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            string scriptPath = pathFolderCS;
            FileStream file = new FileStream(scriptPath, FileMode.Open);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            StringBuilder build = new StringBuilder();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta") || PathDict.ContainsKey(files[i].DirectoryName))
                    continue;
                string pathRes = string.Empty;
                string pathName = string.Empty;
                string direName = files[i].DirectoryName.Replace("\\", "/");
                direName = direName.Replace(Application.dataPath + "/Resources/", "");
                PathDict[files[i].DirectoryName] = direName;
                pathName = direName.Replace("/", "_");
                pathRes = "public const string " + pathName + " = " + "\"" + direName+"/" + "\"" + ";\r\n\t";
                build.Append(pathRes);
                #region ob
                //Debug.Log("Name:" + files[i].Name);  //打印出来这个文件架下的所有文件
                //Debug.Log("FullName:" + files[i].FullName);
                //Debug.Log("DirectoryName:" + direName);
                #endregion
            }
            AutoBuildPathFolder.UIClass = AutoBuildPathFolder.UIClass.Replace("#成员#", build.ToString());
            fileW.Write(AutoBuildPathFolder.UIClass);
            fileW.Flush();
            fileW.Close();
            file.Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.LogError("生成成功 有效文件夹路径数量"+PathDict.Count);
        }
    }
}
