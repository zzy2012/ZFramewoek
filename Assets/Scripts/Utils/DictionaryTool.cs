using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 字典工具类
/// </summary>
public static class DictionaryTool
{
    /// <summary>
    /// 尝试根据key得到value，得到了的话直接返回value，没有得到直接返回null
    /// this Dictionary<Tkey,Tvalue> dict 这个字典表示我们要获取值的字典
    /// </summary>
    public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        //if (value == null)
        //    Debug.LogError(string.Format("Key[{0}]不存在", key));
        return value;
    }
}
