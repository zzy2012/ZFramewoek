using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ŀ¼������
/// </summary>
public class DirectoryHelper : MonoBehaviour
{
    //�����ӽڵ����
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
    /// ���ӽڵ���ӽű�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="goParent">������</param>
    /// <param name="childName">�Ӷ�������</param>
    /// <returns></returns>
    public static T AddChildNodeCompnent<T>(Transform goParent, string childName) where T : Component
    {
        Transform searchTranform = null;                //�����ض��ڵ���

        //�����ض��ӽڵ�
        searchTranform = FindTheChildNode(goParent, childName);
        //������ҳɹ�����������Ѿ�����ͬ�Ľű��ˣ�����ɾ��������ֱ����ӡ�
        if (searchTranform != null)
        {
            //����Ѿ�����ͬ�Ľű��ˣ�����ɾ��
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
        //������Ҳ��ɹ�������Null.
    }

    /// <summary>
    /// ���ӽڵ���Ӹ�����
    /// </summary>
    /// <param name="parents">������ķ�λ</param>
    /// <param name="child">�Ӷ���ķ���</param>
    public static void AddChildNodeToParentNode(Transform parents, Transform child)
    {
        child.SetParent(parents, false);
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
        child.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// ɾ�������ӽڵ�
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
    ///// �������Ʋ���һ���Ӷ���
    ///// </summary>
    ///// <param name="name">�Ӷ�������</param>
    ///// <returns></returns>
    //public static GameObject xrFindChildGameObject(Transform transform, string name)
    //{
    //    Transform[] trans = transform.GetComponentsInChildren<Transform>();
    //    foreach (var item in trans)
    //    {
    //        if (item.name == name)
    //            return item.gameObject;
    //    }
    //    Debug.Log($"{transform.name} ���Ҳ�����Ϊ{name} ���Ӷ���");
    //    return null;
    //}

    ///// <summary>
    ///// �������ƻ�ȡһ���Ӷ�������
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
    //    Debug.Log($"{child.name}û�в��ҵ������Ϸ����");
    //    return null;
    //}
}
