using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> where T : new()
{
    private static T _instance;

    public static T _Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }
}

public class SingleTonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return Instance;
        }
    }
}