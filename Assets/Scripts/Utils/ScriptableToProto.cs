using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;
using UnityEngine;

public class ScriptableToProto
{
    public static object CreateInstance(Type t)
    {
        return ScriptableObject.CreateInstance(t);
    }

    private static void RegProtoMetaType<T>() where T : ScriptableObject
    {
        MetaType item = RuntimeTypeModel.Default.Add(typeof(T), true);
        ScriptableToProto.MetaTypelist.Add(item);
    }

    public static void SetInstanceFactory()
    {
        if (ScriptableToProto.HasSetFactory)
        {
            return;
        }
        MethodInfo method = typeof(ScriptableToProto).GetMethod("CreateInstance");
        ScriptableToProto.RegProtoMetaType<AudioConfig>();
        ScriptableToProto.RegProtoMetaType<AnimatorControllerInfo>();
        ScriptableToProto.RegProtoMetaType<BipBindData>();
        ScriptableToProto.RegProtoMetaType<EffectClip>();
        ScriptableToProto.RegProtoMetaType<EffectGroup>();
        ScriptableToProto.RegProtoMetaType<FFActionClip>();
        ScriptableToProto.RegProtoMetaType<FFMaterialAnimClip>();
        ScriptableToProto.RegProtoMetaType<FlyObjConfig>();
        ScriptableToProto.RegProtoMetaType<AvatarDatas>();
        for (int i = 0; i < ScriptableToProto.MetaTypelist.Count; i++)
        {
            if (ScriptableToProto.MetaTypelist[i] != null)
            {
                ScriptableToProto.MetaTypelist[i].SetFactory(method);
            }
        }
        ScriptableToProto.HasSetFactory = true;
    }

    public static void Save<T>(T t, string path, string postfix) where T : ScriptableObject
    {
        if (t == null)
        {
            return;
        }
        try
        {
            string path2 = Path.Combine(path, t.name.ToLower() + postfix);
            if (File.Exists(path2))
            {
                File.Delete(path2);
            }
            using (Stream stream = File.Create(path2))
            {
                Serializer.Serialize<T>(stream, t);
                stream.Close();
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogWarning("ScriptableToProto", "Save error: " + arg);
        }
    }

    private static bool TrySetMember<T>(T t, string membername, object value)
    {
        Type typeFromHandle = typeof(T);
        try
        {
            if (typeFromHandle.GetMember(membername, BindingFlags.Public | BindingFlags.SetField) != null)
            {
                object obj = typeFromHandle.InvokeMember(membername, BindingFlags.Public | BindingFlags.SetField, null, t, new object[]
                {
                    value
                });
                return true;
            }
            FFDebug.LogError("ScriptableToProto", typeFromHandle.Name + " no Public Member " + membername);
        }
        catch (Exception arg)
        {
            FFDebug.LogError("ScriptableToProto", typeFromHandle.Name + " TrySetMember error: \n" + arg);
        }
        return false;
    }

    public static T2 GetMemberValue<T1, T2>(T1 t, string membername)
    {
        Type typeFromHandle = typeof(T1);
        try
        {
            MemberInfo[] member = typeFromHandle.GetMember(membername, BindingFlags.Public | BindingFlags.GetField);
            if (member != null)
            {
                object obj = typeFromHandle.InvokeMember(membername, BindingFlags.Public | BindingFlags.GetField, null, t, new object[0]);
                return (T2)((object)obj);
            }
            FFDebug.LogError("ScriptableToProto", typeFromHandle.Name + " no Public Member " + membername);
        }
        catch (Exception arg)
        {
            FFDebug.LogError("ScriptableToProto", typeFromHandle.Name + " GetMemberValue error: \n" + arg);
        }
        return default(T2);
    }

    public static T Save<T>(T[] Tarray, string path, string name) where T : ScriptableObject
    {
        T t = (T)((object)null);
        if (Tarray == null)
        {
            return (T)((object)null);
        }
        try
        {
            t = ScriptableObject.CreateInstance<T>();
            t.name = Path.GetFileNameWithoutExtension(name);
            if (ScriptableToProto.TrySetMember<T>(t, "ProtoList", Tarray))
            {
                string path2 = Path.Combine(path, name.ToLower());
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
                using (Stream stream = File.Create(path2))
                {
                    Serializer.Serialize<T>(stream, t);
                    stream.Close();
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(string.Empty, ex.Message);
        }
        return t;
    }

    public static void Read<T>(string path, Action<T> callback) where T : ScriptableObject
    {
        try
        {
            ScriptableToProto.LoadByteDataHandle(path, delegate (byte[] bytes)
            {
                if (bytes != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream(bytes))
                    {
                        T obj = Serializer.Deserialize<T>(memoryStream);
                        if (callback != null)
                        {
                            try
                            {
                                callback(obj);
                            }
                            catch (Exception arg2)
                            {
                                FFDebug.LogWarning("ScriptableToProto", "Read error: " + arg2);
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        callback((T)((object)null));
                    }
                    catch (Exception str)
                    {
                        FFDebug.LogError("ScriptableToProto", str);
                    }
                }
            });
        }
        catch (Exception arg)
        {
            FFDebug.LogError("ScriptableToProto", path + "\nerror: " + arg);
        }
    }

    private static List<MetaType> MetaTypelist = new List<MetaType>();

    private static bool HasSetFactory = false;

    public static Action<string, Action<byte[]>> LoadByteDataHandle;
}
