using UnityEngine;
using System.Collections;

/// <summary>
/// ��Դ���ع�����
/// </summary>
public class ResMgr : SingleTonMono<ResMgr>
{
    private Hashtable ht = null;                        //������ֵ�Լ���
    void Awake()
    {
        ht = new Hashtable();
    }

    /// <summary>
    /// ������Դ�������󻺳弼����
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">·��</param>
    /// <param name="isCatch">�Ƿ񻺴�</param>
    /// <returns>��Դ</returns>
    public T LoadResource<T>(string path, bool isCatch = false) where T : UnityEngine.Object
    {
        if (ht.Contains(path))
            return ht[path] as T;
        T TResource = Resources.Load<T>(path);
        if (TResource == null)
            Debug.LogError(GetType() + "/GetInstance()/TResource ��ȡ����Դ�Ҳ��������顣 path=" + path);
        else if (isCatch)
            ht.Add(path, TResource);
        return TResource;
    }

    /// <summary>
    /// ʵ�������壨�����󻺳弼����
    /// </summary>
    /// <param name="path">��Դ·��</param>
    /// <param name="parent">���ڵ�</param>
    /// <param name="isCatch">�Ƿ񻺴�</param>
    /// <returns>����</returns>
    public GameObject Instantiate(string path,Transform parent = null, bool isCatch = false)
    {
        GameObject goObj = LoadResource<GameObject>(path, isCatch);
        GameObject goObjClone;
        if (parent != null)
            goObjClone = Instantiate(goObj, parent);
        else
            goObjClone = Instantiate(goObj);
        if (goObjClone == null)
        {
            Debug.LogError(GetType() + "/Instantiate()/ʵ������Դ���ɹ������顣 path=" + path);
        }
        return goObjClone;
    }
}
