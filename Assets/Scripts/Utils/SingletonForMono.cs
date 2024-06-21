using System;
using UnityEngine;

public class SingletonForMono<T> : MonoBehaviour where T : SingletonForMono<T>
{
    public static T Instance
    {
        get
        {
            if (SingletonForMono<T>.applicationQuit)
            {
                SingletonForMono<T>._instance = (T)((object)null);
                return SingletonForMono<T>._instance;
            }
            object obj = SingletonForMono<T>.lockObj;
            lock (obj)
            {
                if (SingletonForMono<T>._instance != null)
                {
                    return SingletonForMono<T>._instance;
                }
                SingletonForMono<T>._instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
                if (SingletonForMono<T>._instance == null)
                {
                    string name = typeof(T).ToString() + "(Singleton)";
                    SingletonForMono<T>._instance = new GameObject(name).AddComponent<T>();
                    UnityEngine.Object.DontDestroyOnLoad(SingletonForMono<T>._instance);
                    SingletonForMono<T>._instance.CreateNewInstanceSuccessful();
                }
            }
            return SingletonForMono<T>._instance;
        }
    }

    public virtual void CreateNewInstanceSuccessful()
    {
    }

    public bool IsExistInstance
    {
        get
        {
            return SingletonForMono<T>._instance != null;
        }
    }

    private void OnApplicationQuit()
    {
        SingletonForMono<T>.applicationQuit = true;
    }

    public virtual void Destroy()
    {
        if (this.IsExistInstance)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public static object lockObj = new object();

    private static T _instance;

    private static bool applicationQuit = false;
}
