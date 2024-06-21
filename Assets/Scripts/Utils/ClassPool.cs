using System;
using System.Collections.Generic;

public class ClassPool
{
    private static Stack<IstorebAble> GetStack<T>()
    {
        Type typeFromHandle = typeof(T);
        if (!ClassPool.StackMap.ContainsKey(typeFromHandle))
        {
            ClassPool.StackMap[typeFromHandle] = new Stack<IstorebAble>();
        }
        return ClassPool.StackMap[typeFromHandle];
    }

    public static T GetObject<T>() where T : class, IstorebAble, new()
    {
        object lockObject = ClassPool._lockObject;
        T result;
        lock (lockObject)
        {
            Stack<IstorebAble> stack = ClassPool.GetStack<T>();
            T t = (T)((object)null);
            if (stack.Count > 0)
            {
                t = (stack.Pop() as T);
                t.RestThisObject();
            }
            else
            {
                t = Activator.CreateInstance<T>();
            }
            t.IsDirty = false;
            result = t;
        }
        return result;
    }

    public static void Store<T>(IstorebAble t, int MaxStackCount) where T : class, IstorebAble, new()
    {
        object lockObject = ClassPool._lockObject;
        lock (lockObject)
        {
            if (t != null)
            {
                Stack<IstorebAble> stack = ClassPool.GetStack<T>();
                if (stack.Count < MaxStackCount)
                {
                    t.IsDirty = true;
                    if (!stack.Contains(t))
                    {
                        stack.Push(t);
                    }
                }
            }
        }
    }

    public static void Clear<T>()
    {
        object lockObject = ClassPool._lockObject;
        lock (lockObject)
        {
            Stack<IstorebAble> stack = ClassPool.GetStack<T>();
            stack.Clear();
        }
    }

    private static BetterDictionary<Type, Stack<IstorebAble>> StackMap = new BetterDictionary<Type, Stack<IstorebAble>>();

    private static object _lockObject = new object();
}
