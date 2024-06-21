using System;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventHelper
{
    public static void AddListener(UnityEvent pEvent, LuaFunction func)
    {
        pEvent.AddListener(delegate ()
        {
            func.Call();
        });
    }

    public static void AddListener(UnityEvent<float> pEvent, LuaFunction func)
    {
        pEvent.AddListener(delegate (float p)
        {
            func.Call((double)p);
        });
    }

    public static void AddListener(UnityEvent<Vector2> pEvent, LuaFunction func)
    {
        pEvent.AddListener(delegate (Vector2 p)
        {
            func.Call(new object[]
            {
                p
            });
        });
    }

    public static void AddListener(UnityEvent<GameObject> pEvent, LuaFunction func)
    {
        pEvent.AddListener(delegate (GameObject p)
        {
            func.Call(new object[]
            {
                p
            });
        });
    }

    public static void AddListener(UnityEvent<bool> pEvent, LuaFunction func)
    {
        pEvent.AddListener(delegate (bool p)
        {
            func.Call(new object[]
            {
                p
            });
        });
    }

    public static void AddListener(UnityEvent<string> pEvent, LuaFunction func)
    {
        pEvent.AddListener(delegate (string p)
        {
            func.Call(new object[]
            {
                p
            });
        });
    }

    public static void RemoveAllListeners(UnityEventBase pEvent)
    {
        pEvent.RemoveAllListeners();
    }
}
