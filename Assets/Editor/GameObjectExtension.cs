using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// GameObject右键扩展类
/// </summary>
public class GameObjectExtension
{
    static Component[] copiedComponents;

    /// <summary>
    /// 复制GameObject的Transform属性到剪切板
    /// </summary>
    [MenuItem("GameObject/GameObjectExtension/CopyTransInfo", priority = 1)]
    static void CopyTransInfo()
    {
        GameObject obj = Selection.activeGameObject;
        Transform trans = obj.transform;
        string scale = trans.localScale.x + "," + trans.localScale.y + "," + trans.localScale.z;
        string angles = trans.localEulerAngles.x + "," + trans.localEulerAngles.y + "," + trans.localEulerAngles.z;
        //开头加了一个‘是为了防止复制到Excel时因为首位为-而误识别为公式
        string str = "'" + trans.localPosition.x + "," + trans.localPosition.y + "," + trans.localPosition.z + "," + angles + "," + scale;
        TextEditor te = new TextEditor();
        te.content = new GUIContent(str);
        Debug.Log($"复制成功，路径为{te.content.text}");
        te.SelectAll();
        te.Copy();
    }
    /// <summary>
    /// 复制GameObject的路径到剪切板
    /// </summary>
    [MenuItem("GameObject/GameObjectExtension/CopyPath", priority = 0)]
    private static void CopyGameObjectPath()
    {
        Object obj = Selection.activeObject;
        if (obj == null)
        {
            Debug.LogError("You must select Obj first!");
            return;
        }
        string result = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(result))//如果不是资源则在场景中查找
        {
            Transform selectChild = Selection.activeTransform;
            if (selectChild != null)
            {
                result = selectChild.name;
                while (selectChild.parent != null)
                {
                    selectChild = selectChild.parent;
                    result = string.Format("{0}/{1}", selectChild.name, result);
                }
            }
        }
        ClipBoard.Copy(result);
        Debug.Log($"The gameobject:{obj.name}'s path has been copied to the clipboard!");
    }

    /// <summary>
    /// 复制GameObject的所有组件
    /// </summary>
    [MenuItem("GameObject/GameObjectExtension/CopyAllComponents")]
    static void CopyAllCompontents()
    {
        copiedComponents = Selection.activeGameObject.GetComponents<Component>();
    }
    /// <summary>
    /// 粘贴GameObject的所有组件
    /// </summary>
    [MenuItem("GameObject/GameObjectExtension/PasteAllComponents #&P")]
    static void PasteAllCompontents()
    {
        foreach (var targetGameObject in Selection.gameObjects)
        {
            if (!targetGameObject || copiedComponents == null) continue;
            foreach (var copiedComponent in copiedComponents)
            {
                if (!copiedComponent) continue;
                UnityEditorInternal.ComponentUtility.CopyComponent(copiedComponent);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGameObject);
            }
        }
    }
    /// <summary>
    /// 剪切板
    /// </summary>
    public class ClipBoard
    {
        /// <summary>
        /// 将信息复制到剪切板当中
        /// </summary>
        public static void Copy(string format, params object[] args)
        {
            string result = string.Format(format, args);
            TextEditor editor = new TextEditor();
            editor.content = new GUIContent(result);
            editor.OnFocus();
            editor.Copy();
        }
    }
}
