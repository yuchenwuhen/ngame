using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainComponentManager {

    private static MainComponentManager instance;

    private GameObject main;

    public static void CreateInstance()
    {
        if(instance == null)
        {
            instance = new MainComponentManager();
            GameObject go = GameObject.Find("Main");
            if(go == null)
            {
                go = new GameObject("Main");
                instance.main = go;
                Object.DontDestroyOnLoad(go);
            }
        }
    }

    public static MainComponentManager SharedInstance
    {
        get
        {
            if(instance == null)
            {
                CreateInstance();
            }
            return instance;
        }
    }

    public static T AddMainComponent <T> () where T : UnityEngine.Component
    {
        T t = SharedInstance.main.GetComponent<T>();
        if(t!=null)
        {
            return t;
        }
        return SharedInstance.main.AddComponent<T>();
    }
}
