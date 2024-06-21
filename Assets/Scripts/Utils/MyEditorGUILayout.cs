using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEditorGUILayout
{
    public static void Label(string Title, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Label(Title, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
    }

    public static uint UIntField(string Title, uint num, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Label(Title, new GUILayoutOption[0]);
        string text = GUILayout.TextField(string.Empty + num, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        uint num2 = num;
        if (!uint.TryParse(text, out num2))
        {
            FFDebug.LogWarning("GUITool", string.Concat(new object[]
            {
                "输入格式错误 str :[",
                text,
                "]tmpnum :",
                num2,
                " num :",
                num
            }));
            return num;
        }
        return num2;
    }

    public static int IntField(string Title, int num, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Label(Title, new GUILayoutOption[0]);
        string text = GUILayout.TextField(string.Empty + num, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        int num2 = num;
        if (!int.TryParse(text, out num2))
        {
            FFDebug.LogWarning("GUITool", string.Concat(new object[]
            {
                "输入格式错误 str :[",
                text,
                "]tmpnum :",
                num2,
                " num :",
                num
            }));
            return num;
        }
        return num2;
    }

    public static float FloatField(string Title, float num, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Label(Title, new GUILayoutOption[0]);
        string text = GUILayout.TextField(string.Empty + num, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        float num2 = num;
        if (!float.TryParse(text, out num2))
        {
            FFDebug.LogWarning("GUITool", string.Concat(new object[]
            {
                "输入格式错误 str :[",
                text,
                "]tmpnum :",
                num2,
                " num :",
                num
            }));
            return num;
        }
        return num2;
    }

    public static string StringField(string Title, string Text, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Label(Title, new GUILayoutOption[0]);
        string result = GUILayout.TextField(string.Empty + Text, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        return result;
    }

    public static string StringArea(string Title, string Text, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Label(Title, new GUILayoutOption[0]);
        string result = GUILayout.TextArea(string.Empty + Text, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        return result;
    }

    public static bool Toggle(string Title, bool value, float Width = 260f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        bool result = GUILayout.Toggle(value, Title, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        return result;
    }

    public static T EnumToggleGroup<T>(string MainTitle, string[] Titles, T value, float Width = 260f)
    {
        GUILayout.Label(MainTitle, new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Space(15f);
        GUILayout.BeginVertical(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        int num = 0;
        foreach (object obj in Enum.GetValues(typeof(T)))
        {
            T result = (T)((object)obj);
            bool flag = GUILayout.Toggle(result.ToString() == value.ToString(), Titles[num], new GUILayoutOption[]
            {
                GUILayout.Width(Width)
            });
            if (flag && result.ToString() != value.ToString())
            {
                return result;
            }
            num++;
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        return value;
    }

    public static object EnumToggleGroup(string MainTitle, object value, float Width = 260f)
    {
        GUILayout.Label(MainTitle, new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        GUILayout.Space(15f);
        GUILayout.BeginVertical(new GUILayoutOption[]
        {
            GUILayout.Width(Width)
        });
        int num = 0;
        foreach (object obj in Enum.GetValues(value.GetType()))
        {
            bool flag = GUILayout.Toggle(obj.ToString() == value.ToString(), obj.ToString(), new GUILayoutOption[]
            {
                GUILayout.Width(Width)
            });
            if (flag && obj.ToString() != value.ToString())
            {
                return obj;
            }
            num++;
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        return value;
    }

    public static void Button(string Title, Action Click, float Hight = 0f, float Width = 0f)
    {
        if (Hight == 0f && Width == 0f)
        {
            if (GUILayout.Button(Title, new GUILayoutOption[]
            {
                GUILayout.Height(40f)
            }))
            {
                Click();
            }
        }
        else
        {
            List<GUILayoutOption> list = new List<GUILayoutOption>();
            if (Hight != 0f)
            {
                list.Add(GUILayout.Height(Hight));
            }
            if (Width != 0f)
            {
                list.Add(GUILayout.Width(Width));
            }
            if (GUILayout.Button(Title, list.ToArray()))
            {
                Click();
            }
        }
    }

    public static void TextButton(string text, string name, Action click, float hight = 0f, float width = 0f)
    {
        if (hight == 0f)
        {
            hight = 20f;
        }
        if (width == 0f)
        {
            width = 40f;
        }
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label(text, new GUILayoutOption[0]);
        MyEditorGUILayout.Button(name, click, hight, width);
        GUILayout.EndHorizontal();
    }

    public static void TextDoubleButton(string text, string name1, Action click1, string name2, Action click2, float hight = 0f, float width = 0f)
    {
        if (hight == 0f)
        {
            hight = 20f;
        }
        if (width == 0f)
        {
            width = 40f;
        }
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label(text, new GUILayoutOption[0]);
        MyEditorGUILayout.Button(name1, click1, hight, width);
        MyEditorGUILayout.Button(name2, click2, hight, width);
        GUILayout.EndHorizontal();
    }

    public static void ListPanel(string title, string name, Action click, object objectvalue, MyEditorGUILayout.DrawHandle drawHandler, float hight = 0f, float width = 0f)
    {
        GUILayout.BeginHorizontal(new GUILayoutOption[]
        {
            GUILayout.Width(width)
        });
        GUILayout.Label(title, new GUILayoutOption[0]);
        if (GUILayout.Button(name, new GUILayoutOption[]
        {
            GUILayout.Height(20f),
            GUILayout.Width(80f)
        }))
        {
            click();
        }
        GUILayout.EndHorizontal();
        if (objectvalue != null)
        {
            List<object> list = new List<object>();
            /*IEnumerator enumerator = objectvalue.CallPublicMethod("GetEnumerator", new object[0]);
            int num = 0;
            while (enumerator != null && enumerator.MoveNext())
            {
                if (drawHandler != null && !drawHandler(num++, enumerator.Current))
                {
                    list.Add(enumerator.Current);
                }
            }
            list.ForEach(delegate (object x)
            {
                objectvalue.CallPublicMethod("Remove", new object[]
                {
                    x
                });
            });*/
        }
    }

    public static string Closestr = "ㄨ";

    public delegate bool DrawHandle(int id, object val);
}
