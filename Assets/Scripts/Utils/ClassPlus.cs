using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class ClassPlus
{
    public static bool FloatEqual(this float value1, float value2)
    {
        return Mathf.Abs(value1 - value2) < 1E-07f;
    }

    public static float AngleWithNormal(this Vector3 value1, Vector3 value2, Vector3 normal)
    {
        float num = Vector3.Angle(value1, value2);
        if (Vector3.Cross(value1, value2).normalized != normal)
        {
            num = -num;
        }
        return num;
    }

    public static int ToInt(this float value)
    {
        return (int)value;
    }

    public static float ToFloat(this int value)
    {
        return (float)value;
    }

    public static int ToInt(this string value)
    {
        int result = 0;
        if (int.TryParse(value, out result))
        {
            return result;
        }
        return 0;
    }

    public static float ToFloat(this string value)
    {
        float result = 0f;
        if (float.TryParse(value, out result))
        {
            return result;
        }
        return 0f;
    }

    public static Color ToColor(this string value)
    {
        if (value.StartsWith("#"))
        {
            value = value.Replace("#", string.Empty);
        }
        int num = int.Parse(value, NumberStyles.AllowHexSpecifier);
        return new Color((float)(num >> 16 & 255) / 255f, (float)(num >> 8 & 255) / 255f, (float)(num & 255) / 255f, 255f);
    }

    public static void ToDistinct<T>(this IEnumerable<T> list)
    {
        list = list.Distinct<T>();
    }

    public static T RemoveAt<T>(this List<T> list, int index)
    {
        T result = list[index];
        list.RemoveAt(index);
        return result;
    }

    public static T1 RemoveAt<T, T1>(this Dictionary<T, T1> dict, T key)
    {
        T1 result = default(T1);
        if (dict.TryGetValue(key, out result))
        {
            dict.Remove(key);
        }
        return result;
    }

    public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dict, Predicate<KeyValuePair<TKey, TValue>> match)
    {
        List<TKey> list = new List<TKey>();
        Dictionary<TKey, TValue>.Enumerator enumerator = dict.GetEnumerator();
        if (enumerator.MoveNext() && match(enumerator.Current))
        {
            List<TKey> list2 = list;
            KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
            list2.Add(keyValuePair.Key);
        }
        list.ForEach(delegate (TKey x)
        {
            dict.Remove(x);
        });
    }

    public static string RandomSubstring(this string value, char split = ';')
    {
        if (value == null)
        {
            return string.Empty;
        }
        string[] array = value.Split(new char[]
        {
            split
        });
        if (array.Length <= 0)
        {
            return value;
        }
        return array[(int)(UnityEngine.Random.value * 100f) % array.Length];
    }

    public static T RandomGetOne<T>(this List<T> list)
    {
        return (list.Count <= 0) ? default(T) : list.ElementAt((int)(UnityEngine.Random.value * 100f) % list.Count);
    }

    public static T RandomGetOne<T>(this T[] arr)
    {
        return (arr.Length <= 0) ? default(T) : arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    public static List<T> ClearNullItems<T>(this List<T> list)
    {
        List<T> objList = new List<T>();
        for (int index = 0; index < list.Count; ++index)
        {
            if ((object)list[index] != null)
                objList.Add(list[index]);
        }
        return objList;
    }

    public static Dictionary<TKey, TValue> ClearNullItems<TKey, TValue>(
      this Dictionary<TKey, TValue> dict)
    {
        Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        Dictionary<TKey, TValue>.Enumerator enumerator = dict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if ((object)enumerator.Current.Value != null)
                dictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
        }
        return dictionary;
    }


    public static void Copy<Key, Value>(this Dictionary<Key, Value> value, Dictionary<Key, Value> dic)
    {
        if (dic.Count == 0)
        {
            return;
        }
        foreach (KeyValuePair<Key, Value> keyValuePair in dic)
        {
            Value value2;
            if (!value.TryGetValue(keyValuePair.Key, out value2))
            {
                Dictionary<Key, Value>.Enumerator enumerator = dic.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if ((object)enumerator.Current.Value != null)
                        value.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
            }
        }
    }


    public static GameObject FindChild(this GameObject value, string path)
    {
        Transform transform = value.transform.Find(path);
        if (transform != null)
        {
            return transform.gameObject;
        }
        return null;
    }

    public static T Find<T>(this GameObject value, string path) where T : Component
    {
        Transform transform = value.transform.Find(path);
        if (transform != null)
        {
            return transform.GetComponent<T>();
        }
        return (T)((object)null);
    }

    public static void SetLayer(this GameObject value, int layer, bool impactChilds)
    {
        if (value == null)
        {
            return;
        }
        value.layer = layer;
        if (impactChilds)
        {
            foreach (object obj in value.transform)
            {
                Transform transform = obj as Transform;
                transform.gameObject.layer = layer;
                if (transform.childCount > 0)
                {
                    transform.gameObject.SetLayer(layer, impactChilds);
                }
            }
        }
    }

    public static void Reset(this Transform value)
    {
        value.localPosition = Vector3.zero;
        value.localRotation = Quaternion.identity;
        value.localScale = Vector3.one;
    }

    public static void SetParent(this RectTransform value, RectTransform parent, Vector2 pos)
    {
        Vector2 v = value.localScale;
        Vector2 v2 = value.localRotation.eulerAngles;
        value.transform.SetParent(parent);
        value.anchoredPosition = pos;
        value.localScale = v;
        value.localRotation = Quaternion.Euler(v2);
    }

    public static bool HasBeControlledBy(this CharactorBase Cb, BufferState.ControlType flag)
    {
        PlayerBufferControl component = Cb.GetComponent<PlayerBufferControl>();
        return component != null && component.HasBeControlled(flag);
    }

    public static Vector2 GetVector2WithString(string coordinates)
    {
        string[] array = coordinates.Split(new char[]
        {
            ','
        });
        if (string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
        {
            FFDebug.LogWarning(null, "û��Ѱ·����!");
            return Vector2.zero;
        }
        uint num = uint.Parse(array[0]);
        uint num2 = uint.Parse(array[1]);
        return new Vector2(num, num2);
    }
}
