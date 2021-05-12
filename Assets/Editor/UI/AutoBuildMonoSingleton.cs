/****************************************************
文件：AutoBuildMonoSingleton.cs
作者：EDZ/#Author#
邮箱：https://blog.csdn.net/u014361280 
日期：2021/02/25 18:21:57
功能：Nothing
描述：Nothing
*****************************************************/

/****************************************************
文件：AutoBuildMonoSingleton.cs
作者：jieyo20200420
邮箱：https://blog.csdn.net/u014361280 
日期：2020/12/02 11:58:00
功能：Nothing
描述：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 生成模板   MonoSingleton<#类名#>
/// </summary>
public class AutoBuildTemplateMonoSingleton
{
    public static string UIClass =
 @"using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using XR;
using SUIFW;
public class #类名# : BaseUIForm
{
  
    protected override void OnInitialization()
    {
        InitComponents();
    }

    //auto
    #region 成员
    #成员#
    public void InitComponents()
	{
		#查找#
	}
   #endregion
}
";
}

public class AutoBuildMonoSingleton
{
    [MenuItem("Tools/UITools/自动生成MonoSingleton脚本/创建或刷新界面UI")]
    public static void BuildUIScript()
    {

        var dicUIType = new Dictionary<string, string>();

        dicUIType.Add("Img", "Image");
        dicUIType.Add("Btn", "Button");
        dicUIType.Add("Txt", "Text");
        dicUIType.Add("Tran", "Transform");
        dicUIType.Add("Input", "InputField");
        dicUIType.Add("Tgl", "Toggle");
        dicUIType.Add("Slider", "Slider");
        dicUIType.Add("CanG", "CanvasGroup");

        GameObject[] selectobjs = Selection.gameObjects;

        foreach (GameObject go in selectobjs)
        {
            //选择的物体
            GameObject selectobj = go.transform.root.gameObject;

            //物体的子物体
            Transform[] _transforms = selectobj.GetComponentsInChildren<Transform>(true);

            List<Transform> childList = new List<Transform>(_transforms);

            //UI需要查询的物体
            var mainNode = from trans in childList where trans.name.Contains('_') && dicUIType.Keys.Contains(trans.name.Split('_')[0]) select trans;

            var nodePathList = new Dictionary<string, string>();

            //循环得到物体路径
            foreach (Transform node in mainNode)
            {
                Transform tempNode = node;
                string nodePath = "/" + tempNode.name;

                while (tempNode != tempNode.root)
                {
                    tempNode = tempNode.parent;

                    int index = nodePath.IndexOf('/');

                    nodePath = nodePath.Insert(index, "/" + tempNode.name);
                }
                nodePath = nodePath.Substring(2 + selectobj.name.Length);
                //Debug.Log("1.**************** node.name:" + node.name + " nodePath:" + nodePath);
                nodePathList.Add(node.name, nodePath);
            }

            //成员变量字符串
            string memberstring = "";
            //查询代码字符串
            string loadedcontant = "";
            foreach (Transform itemtran in mainNode)
            {
                string typeStr = dicUIType[itemtran.name.Split('_')[0]];

                memberstring += "[HideInInspector]\r\n\t";
                memberstring += "public " + typeStr + " " + itemtran.name + " = null;\r\n\t";
                //loadedcontant += itemtran.name + " = " + "xrGetOrAddCompoentInChildren<" + typeStr + ">(\"" + itemtran.name + "\");\r\n\t\t";
                loadedcontant += itemtran.name + " = " + "transform.Find(\"" + nodePathList[itemtran.name] + "\").GetComponent<" + typeStr + ">();\r\n\t\t";
            }
            //Debug.Log("2.**************** loadedcontant:" + loadedcontant);

            //创建文件夹
            string panelFolderPath = Application.dataPath + "/Scripts/UGUI/" + selectobj.name;
            string templatesFolderPath = Application.dataPath + "/Scripts/UGUI/TemplateUIForms/";
            string modelClassName = Path.Combine(panelFolderPath, selectobj.name + "_Model.cs");
            string viewClassName = Path.Combine(panelFolderPath, selectobj.name + "_View.cs");
            string ctrlClassName = Path.Combine(panelFolderPath, selectobj.name + "_Ctrl.cs");
            string[] className = new string[] { modelClassName, viewClassName, ctrlClassName };
            if (!Directory.Exists(panelFolderPath))
            {
                Directory.CreateDirectory(panelFolderPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            //拷贝文件，并重命名
            if (!File.Exists(ctrlClassName))
                File.Copy(Path.Combine(templatesFolderPath, "TemplateUIForms_Ctrl.cs"), ctrlClassName);
            if (!File.Exists(viewClassName))
                File.Copy(Path.Combine(templatesFolderPath, "TemplateUIForms_View.cs"), viewClassName);
            if (!File.Exists(modelClassName))
                File.Copy(Path.Combine(templatesFolderPath, "TemplateUIForms_Model.cs"), modelClassName);
            //三个脚本处理
            foreach (string scriptPath in className)
            {
                FileStream classfile = new FileStream(scriptPath, FileMode.Open);
                StreamReader read = new StreamReader(classfile);
                string classStr = read.ReadToEnd();
                read.Close();
                classfile.Close();
                File.Delete(scriptPath);

                string splitStr = "//auto code";
                string[] contents = Regex.Split(classStr, splitStr, RegexOptions.IgnoreCase);
                string unchangeStr = contents[0];
                StringBuilder build = new StringBuilder();
                build.Append(unchangeStr);
                //view脚本
                if (scriptPath.Contains("_View"))
                {
                    string changeStr = contents[1];
                    build.Append(splitStr);
                    build.Append(changeStr);
                    build.Append(splitStr);
                    build.Append(contents[2]);
                }
                classStr = build.ToString();

                classStr = classStr.Replace("TemplateUIForms", selectobj.name);
                if (scriptPath.Contains("_View"))
                {
                    //替换模板中内容
                    if (classStr.Contains("//#查找#"))
                        classStr = classStr.Replace("//#查找#", loadedcontant);
                    else
                    //替换已经存在脚本中内容
                    {
                        classStr = ReplaceStringBetween(classStr, "#region 成员查找", "#endregion 成员查找", loadedcontant);
                    }

                    if (classStr.Contains("//#成员#"))
                        classStr = classStr.Replace("//#成员#", memberstring);
                    else
                    {
                        //替换已经存在脚本中内容
                        classStr = ReplaceStringBetween(classStr, "#region 成员变量", "#endregion 成员变量", memberstring);
                    }
                    //Debug.Log("3.classStr:" + classStr + " loadedcontant:" + loadedcontant);
                }
                FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
                StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
                fileW.Write(classStr);
                fileW.Flush();
                fileW.Close();
                file.Close();

                Debug.Log("创建脚本 " + scriptPath + ".cs 成功!");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public static Assembly GetAssembly()
    {
        Assembly[] AssbyCustmList = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < AssbyCustmList.Length; i++)
        {
            string assbyName = AssbyCustmList[i].GetName().Name;

            if (assbyName == "Assembly-CSharp")
            {
                return AssbyCustmList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 将原string中，部分content内容替换为目标content
    /// </summary>
    /// <param name="srcStr"></param>
    /// <param name="startName"></param>
    /// <param name="endName"></param>
    /// <param name="needStr"></param>
    /// <returns></returns>
    public static string ReplaceStringBetween(string srcStr, string startName, string endName, string needStr)
    {
        string[] strArr = Regex.Split(srcStr, startName, RegexOptions.IgnoreCase);
        string[] strArr2 = Regex.Split(strArr[1], endName, RegexOptions.IgnoreCase);
        return srcStr.Replace(strArr2[0], needStr);
    }
}
