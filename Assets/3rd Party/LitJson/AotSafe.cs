using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AotSafe
{
    public static void ForEach<T>(object enumerable, Action<T> action)
    {
        if (enumerable == null)
        {
            return;
        }
        Type typeFromHandle = typeof(IEnumerable);
        if (!enumerable.GetType().GetInterfaces().Contains(typeFromHandle))
        {
            throw new ArgumentException("Object does not implement IEnumerable interface", "enumerable");
        }
        MethodInfo method = typeFromHandle.GetMethod("GetEnumerator");
        if (method == null)
        {
            throw new InvalidOperationException("Failed to get 'GetEnumerator()' method info from IEnumerable type");
        }
        IEnumerator enumerator = null;
        try
        {
            enumerator = (IEnumerator)method.Invoke(enumerable, null);
            if (enumerator is IEnumerator)
            {
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    action((T)((object)obj));
                }
            }
            else
            {
                Debug.Log(string.Format("{0}.GetEnumerator() returned '{1}' instead of IEnumerator.", enumerable.ToString(), enumerator.GetType().Name));
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }

    private delegate IEnumerator GetEnumerator();
}
