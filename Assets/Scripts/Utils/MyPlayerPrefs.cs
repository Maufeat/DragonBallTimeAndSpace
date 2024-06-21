using System;
using System.IO;
using System.Text;
using UnityEngine;

public class MyPlayerPrefs
{
    private static MyJson.JsonNode_Object o
    {
        get
        {
            if (MyPlayerPrefs._o == null)
            {
                if (File.Exists(MyPlayerPrefs.configPath))
                {
                    MyPlayerPrefs._o = (MyJson.Parse(File.ReadAllText(MyPlayerPrefs.configPath)) as MyJson.JsonNode_Object);
                }
                else
                {
                    MyPlayerPrefs._o = new MyJson.JsonNode_Object();
                }
            }
            return MyPlayerPrefs._o;
        }
    }

    private static string configPath
    {
        get
        {
            if (string.IsNullOrEmpty(MyPlayerPrefs._configPath))
            {
                string temporaryCachePath = Application.temporaryCachePath;
                if (!Directory.Exists(temporaryCachePath))
                {
                    Directory.CreateDirectory(temporaryCachePath);
                }
                MyPlayerPrefs._configPath = Path.Combine(temporaryCachePath, "my_player_prefs.config");
            }
            return MyPlayerPrefs._configPath;
        }
    }

    private static void Save()
    {
        using (FileStream fileStream = File.Open(MyPlayerPrefs.configPath, FileMode.OpenOrCreate))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(MyPlayerPrefs.o.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
        }
    }

    public static void SetString(string key, string value)
    {
        MyPlayerPrefs.o[key] = new MyJson.JsonNode_ValueString(value);
        MyPlayerPrefs.Save();
    }

    public static void SetFloat(string key, float value)
    {
        MyPlayerPrefs.o[key] = new MyJson.JsonNode_ValueNumber((double)value);
        MyPlayerPrefs.Save();
    }

    public static void SetInt(string key, int value)
    {
        MyPlayerPrefs.o[key] = new MyJson.JsonNode_ValueNumber((double)value);
        MyPlayerPrefs.Save();
    }

    public static string GetString(string key)
    {
        if (MyPlayerPrefs.o.ContainsKey(key))
        {
            return MyPlayerPrefs.o[key].AsString();
        }
        return string.Empty;
    }

    public static float GetFloat(string key)
    {
        if (MyPlayerPrefs.o.ContainsKey(key))
        {
            return (float)MyPlayerPrefs.o[key].AsDouble();
        }
        return 0f;
    }

    public static int GetInt(string key)
    {
        if (MyPlayerPrefs.o.ContainsKey(key))
        {
            return MyPlayerPrefs.o[key].AsInt();
        }
        return 0;
    }

    private static MyJson.JsonNode_Object _o;

    private static string _configPath;
}
