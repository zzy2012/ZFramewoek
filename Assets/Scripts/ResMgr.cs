using UnityEngine;
using System.Collections;

/// <summary>
/// 资源加载管理类
/// </summary>
public class ResMgr : SingleTonMono<ResMgr>
{
    private Hashtable ht = null;                        //容器键值对集合
    void Awake()
    {
        ht = new Hashtable();
    }

    /// <summary>
    /// 调用资源（带对象缓冲技术）
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="isCatch">是否缓存</param>
    /// <returns>资源</returns>
    public T LoadResource<T>(string path, bool isCatch = false) where T : UnityEngine.Object
    {
        if (ht.Contains(path))
            return ht[path] as T;
        T TResource = Resources.Load<T>(path);
        if (TResource == null)
            Debug.LogError(GetType() + "/GetInstance()/TResource 提取的资源找不到，请检查。 path=" + path);
        else if (isCatch)
            ht.Add(path, TResource);
        return TResource;
    }

    /// <summary>
    /// 实例化物体（带对象缓冲技术）
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="parent">父节点</param>
    /// <param name="isCatch">是否缓存</param>
    /// <returns>物体</returns>
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
            Debug.LogError(GetType() + "/Instantiate()/实例化资源不成功，请检查。 path=" + path);
        }
        return goObjClone;
    }
}
