using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 目录工具类
/// </summary>
public class DirectoryHelper : MonoBehaviour
{
    //查找子节点对象
    public static Transform FindTheChildNode(Transform goParent, string childName)
    {
        Transform searchTrans = null;
        searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChildNode(trans.transform, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }

    /// <summary>
    /// 给子节点添加脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T AddChildNodeCompnent<T>(Transform goParent, string childName) where T : Component
    {
        Transform searchTranform = null;                //查找特定节点结果

        //查找特定子节点
        searchTranform = FindTheChildNode(goParent, childName);
        //如果查找成功，则考虑如果已经有相同的脚本了，则先删除，否则直接添加。
        if (searchTranform != null)
        {
            //如果已经有相同的脚本了，则先删除
            T[] componentScriptsArray = searchTranform.GetComponents<T>();
            for (int i = 0; i < componentScriptsArray.Length; i++)
            {
                if (componentScriptsArray[i] != null)
                {
                    Destroy(componentScriptsArray[i]);
                }
            }
            return searchTranform.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
        //如果查找不成功，返回Null.
    }

    /// <summary>
    /// 给子节点添加父对象
    /// </summary>
    /// <param name="parents">父对象的方位</param>
    /// <param name="child">子对象的方法</param>
    public static void AddChildNodeToParentNode(Transform parents, Transform child)
    {
        child.SetParent(parents, false);
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
        child.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 删除所有子节点
    /// </summary>
    /// <param name="transform"></param>
    public static void DelAllChildNode(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    ///// <summary>
    ///// 根据名称查找一个子对象
    ///// </summary>
    ///// <param name="name">子对象名称</param>
    ///// <returns></returns>
    //public static GameObject xrFindChildGameObject(Transform transform, string name)
    //{
    //    Transform[] trans = transform.GetComponentsInChildren<Transform>();
    //    foreach (var item in trans)
    //    {
    //        if (item.name == name)
    //            return item.gameObject;
    //    }
    //    Debug.Log($"{transform.name} 里找不到名为{name} 的子对象");
    //    return null;
    //}

    ///// <summary>
    ///// 根据名称获取一个子对象的组件
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="name"></param>
    ///// <returns></returns>
    //public static T xrGetOrAddCompoentInChildren<T>(Transform transform, string name) where T : Component
    //{
    //    GameObject child = xrFindChildGameObject(transform, name);
    //    if (child)
    //    {
    //        if (child.GetComponent<T>() == null)
    //            child.AddComponent<T>();
    //        return child.GetComponent<T>();
    //    }
    //    Debug.Log($"{child.name}没有查找到这个游戏对象");
    //    return null;
    //}
}
