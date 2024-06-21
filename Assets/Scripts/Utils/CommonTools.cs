using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using msg;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonTools
{
    public static Quaternion GetClientDirQuaternionByServerDir(int dir)
    {
        dir *= 2;
        Quaternion quaternion = default(Quaternion);
        Vector3 vector = Vector3.zero;
        dir %= 360;
        if (dir == 0)
        {
            vector = Vector3.forward;
        }
        if (dir == 90)
        {
            vector = Vector3.right;
        }
        if (dir == 180)
        {
            vector = Vector3.back;
        }
        if (dir == 270)
        {
            vector = Vector3.left;
        }
        if (dir > 0 && dir < 90)
        {
            vector = new Vector3(Mathf.Cos((float)(90 - dir) * 0.0174532924f), 0f, Mathf.Sin((float)(90 - dir) * 0.0174532924f));
        }
        if (dir > 90 && dir < 180)
        {
            vector = new Vector3(Mathf.Cos((float)(dir - 90) * 0.0174532924f), 0f, -Mathf.Sin((float)(dir - 90) * 0.0174532924f));
        }
        if (dir > 180 && dir < 270)
        {
            vector = new Vector3(-Mathf.Sin((float)(dir - 180) * 0.0174532924f), 0f, -Mathf.Cos((float)(dir - 180) * 0.0174532924f));
        }
        if (dir > 270 && dir < 360)
        {
            vector = new Vector3(-Mathf.Cos((float)(dir - 270) * 0.0174532924f), 0f, Mathf.Sin((float)(dir - 270) * 0.0174532924f));
        }
        return Quaternion.LookRotation(vector.normalized);
    }

    public static Vector3 DismissYSize(Vector3 old)
    {
        return new Vector3(old.x, 0f, old.z);
    }

    public static Vector3 ToClientPos(Position pos)
    {
        return GraphUtils.GetWorldPosByServerPos(CommonTools.ToServerPos(pos));
    }

    public static Vector2 ToServerPos(Position pos)
    {
        return new Vector2(pos.x, pos.y);
    }

    public static Vector2 GetClientDirVector2ByServerDir(int dir)
    {
        while (dir < 0)
        {
            dir += 180;
        }
        dir *= 2;
        Vector2 vector = Vector2.zero;
        dir %= 360;
        if (dir == 0)
        {
            vector = new Vector2(0f, -1f);
        }
        if (dir == 90)
        {
            vector = new Vector2(1f, 0f);
        }
        if (dir == 180)
        {
            vector = new Vector2(0f, 1f);
        }
        if (dir == 270)
        {
            vector = new Vector2(-1f, 0f);
        }
        if (dir > 0 && dir < 90)
        {
            vector = new Vector3(Mathf.Cos((float)(90 - dir) * 0.0174532924f), -Mathf.Sin((float)(90 - dir) * 0.0174532924f));
        }
        if (dir > 90 && dir < 180)
        {
            vector = new Vector3(Mathf.Cos((float)(dir - 90) * 0.0174532924f), Mathf.Sin((float)(dir - 90) * 0.0174532924f));
        }
        if (dir > 180 && dir < 270)
        {
            vector = new Vector3(-Mathf.Sin((float)(dir - 180) * 0.0174532924f), Mathf.Cos((float)(dir - 180) * 0.0174532924f));
        }
        if (dir > 270 && dir < 360)
        {
            vector = new Vector3(-Mathf.Cos((float)(dir - 270) * 0.0174532924f), -Mathf.Sin((float)(dir - 270) * 0.0174532924f));
        }
        return vector.normalized;
    }

    public static Vector2 ToServerVector(Vector2 v2)
    {
        Vector2 result = v2;
        result.y *= -1f;
        return result;
    }

    public static uint GetServerDirByClientDir(Vector2 vdir)
    {
        vdir.Normalize();
        float num = Vector2.Dot(vdir.normalized, Vector2.up);
        uint num2 = (uint)Mathf.RoundToInt((float)(Math.Acos((double)num) * 57.295780181884766));
        if (vdir.x < 0f)
        {
            num2 = 360U - num2;
        }
        return num2 / 2U;
    }

    public static bool IsInRange(float x1, float y1, float x2, float y2, float range)
    {
        float num = Mathf.Abs(x1 - x2);
        float num2 = Mathf.Abs(y1 - y2);
        return num <= range && num2 <= range;
    }

    public static bool IsInRange(Vector2 p1, Vector2 p2, float range)
    {
        return CommonTools.IsInRange(p1.x, p1.y, p2.x, p2.y, range);
    }

    public static bool IsInDistance(Vector2 p1, Vector2 p2, float distance)
    {
        return Vector2.Distance(p1, p2) < distance;
    }

    public static string GetStringWithColor(string content, string color)
    {
        return string.Format(color, content);
    }

    public static string GetTimeString(float t)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int num = (int)t;
        if (num > 3600)
        {
            return "99:99";
        }
        int num2 = num / 60;
        if (num2 < 10)
        {
            stringBuilder.Append("0");
        }
        stringBuilder.Append(num2.ToString());
        stringBuilder.Append(":");
        int num3 = num % 60;
        if (num3 < 10)
        {
            stringBuilder.Append("0");
        }
        stringBuilder.Append(num3);
        return stringBuilder.ToString();
    }

    public static string GetTimeStringByCeilToInt(float t)
    {
        if (t < 60f)
        {
            return Mathf.CeilToInt(t).ToString();
        }
        if (t < 3600f)
        {
            return Mathf.CeilToInt(t / 60f).ToString() + "m";
        }
        if (t < 86400f)
        {
            return Mathf.CeilToInt(t / 3600f).ToString() + "h";
        }
        return string.Empty;
    }

    public static Color HexToColor(string cs)
    {
        string value = cs.Substring(0, 2);
        string value2 = cs.Substring(2, 2);
        string value3 = cs.Substring(4, 2);
        string value4 = "FF";
        if (cs.Length == 8)
        {
            value4 = cs.Substring(6, 2);
        }
        float num = (float)Convert.ToInt32(value, 16);
        float num2 = (float)Convert.ToInt32(value2, 16);
        float num3 = (float)Convert.ToInt32(value3, 16);
        float num4 = (float)Convert.ToInt32(value4, 16);
        return new Color(num / 255f, num2 / 255f, num3 / 255f, num4 / 255f);
    }

    public static string ColorToHex(Color color)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string value = string.Format("{0:X}", (int)(color.r * 255f));
        string value2 = string.Format("{0:X}", (int)(color.g * 255f));
        string value3 = string.Format("{0:X}", (int)(color.b * 255f));
        string value4 = string.Format("{0:X}", (int)(color.a * 255f));
        stringBuilder.Append(value);
        stringBuilder.Append(value2);
        stringBuilder.Append(value3);
        stringBuilder.Append(value4);
        return stringBuilder.ToString();
    }

    public static bool CheckNameFormat(string content)
    {
        int num = Encoding.UTF8.GetBytes(content).Length;
        if (num < 4)
        {
            FFDebug.LogWarning("CommonTools", "wordlength < 4 " + num);
            return false;
        }
        if (num > 21)
        {
            FFDebug.LogWarning("CommonTools", "wordlength > 21 " + num);
            return false;
        }
        for (int i = 0; i < content.Length; i++)
        {
            if ((content[i] < '0' || content[i] > '9') && (content[i] < 'A' || content[i] > 'Z') && (content[i] < 'a' || content[i] > 'z') && (content[i] < '一' || content[i] > '龻'))
            {
                return false;
            }
        }
        return true;
    }

    public static Mesh CreateSector(float radius, float angleDegree, int segments, bool isTile)
    {
        if (segments == 0)
        {
            segments = 1;
            FFDebug.LogWarning("CommonTools", "segments must be larger than zero.");
        }
        Mesh mesh = new Mesh();
        Vector3[] array = new Vector3[3 + segments - 1];
        array[0] = new Vector3(0f, 0f, 0f);
        float num = 0.0174532924f * angleDegree;
        float num2 = num / 2f;
        float num3 = num / (float)segments;
        for (int i = 1; i < array.Length; i++)
        {
            array[i] = new Vector3(Mathf.Cos(num2) * radius, 0f, Mathf.Sin(num2) * radius);
            num2 -= num3;
        }
        int[] array2 = new int[segments * 3];
        int j = 0;
        int num4 = 1;
        while (j < array2.Length)
        {
            array2[j + 2] = num4 + 1;
            array2[j + 1] = num4;
            array2[j] = 0;
            j += 3;
            num4++;
        }
        mesh.vertices = array;
        mesh.triangles = array2;
        Vector2[] array3 = new Vector2[array.Length];
        num2 = num / 2f;
        if (!isTile)
        {
            for (int k = 0; k < array3.Length; k++)
            {
                if (angleDegree == 360f)
                {
                    if (k == 0)
                    {
                        array3[k] = new Vector2(0.5f, 0.1f);
                    }
                    else
                    {
                        array3[k] = new Vector2(0.5f, 1f);
                    }
                }
                else if (k == 0)
                {
                    array3[k] = new Vector2(0.5f, 0f);
                }
                else if (k == 1)
                {
                    array3[k] = new Vector2(0f, 1f);
                }
                else if (k == array3.Length - 1)
                {
                    array3[k] = new Vector2(1f, 1f);
                }
                else
                {
                    array3[k] = new Vector2(0.5f, 1f);
                }
            }
        }
        else
        {
            for (int l = 0; l < array3.Length; l++)
            {
                if (l == 0)
                {
                    array3[l] = new Vector2(0.5f, 0.5f);
                }
                else
                {
                    array3[l] = new Vector2(Mathf.Cos(num2) * 0.5f + 0.5f, Mathf.Sin(num2) * 0.5f + 0.5f);
                    num2 -= num3;
                }
            }
        }
        mesh.uv = array3;
        return mesh;
    }

    public static Mesh CreateMeshByAray(float[] radius, float angleDegree)
    {
        int num = radius.Length;
        if (num == 0)
        {
            num = 1;
            FFDebug.LogWarning("CommonTools", "segments must be larger than zero.");
        }
        Mesh mesh = new Mesh();
        Vector3[] array = new Vector3[3 + num - 1];
        array[0] = new Vector3(0f, 0f, 0f);
        float num2 = 0.0174532924f * angleDegree;
        float num3 = num2 / 2f;
        float num4 = num2 / (float)num;
        for (int i = 1; i < array.Length; i++)
        {
            float num5;
            if (i == array.Length - 1)
            {
                num5 = radius[0];
            }
            else
            {
                num5 = radius[i - 1];
            }
            array[i] = new Vector3(Mathf.Cos(num3) * num5, 0f, Mathf.Sin(num3) * num5);
            num3 -= num4;
        }
        int[] array2 = new int[num * 3];
        int j = 0;
        int num6 = 1;
        while (j < array2.Length)
        {
            array2[j + 2] = num6 + 1;
            array2[j + 1] = num6;
            array2[j] = 0;
            j += 3;
            num6++;
        }
        mesh.vertices = array;
        mesh.triangles = array2;
        Vector2[] array3 = new Vector2[array.Length];
        array3[0] = new Vector2(0f, 1f);
        for (int k = 1; k < array3.Length; k++)
        {
            float num7;
            if (k == array.Length - 1)
            {
                num7 = radius[0];
            }
            else
            {
                num7 = radius[k - 1];
            }
            array3[k] = new Vector2(0f, 1f - num7);
        }
        mesh.uv = array3;
        return mesh;
    }

    public static void CreateSectorByAray(GameObject FiveanglerObj, float[] radius)
    {
        FiveanglerObj.transform.localEulerAngles = new Vector3(0f, 90f, 270f);
        FiveanglerObj.transform.localScale = new Vector3(97f, 97f, 97f);
        FiveanglerObj.transform.localPosition = new Vector3(0f, -9f, -2f);
        MeshFilter meshFilter = FiveanglerObj.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = FiveanglerObj.AddComponent<MeshFilter>();
        }
        Material material = new Material(Shader.Find("Unlit/Texture"));
        MeshRenderer meshRenderer = FiveanglerObj.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = FiveanglerObj.AddComponent<MeshRenderer>();
        }
        Material sharedMaterial = material;
        meshRenderer.sharedMaterial = sharedMaterial;
        List<float> list = new List<float>();
        for (int i = 0; i < radius.Length; i++)
        {
            list.Add(Mathf.Clamp(radius[i], 0f, 1f));
        }
        meshFilter.mesh = CommonTools.CreateMeshByAray(list.ToArray(), 360f);
    }

    public static Mesh CreateRing(float outsideradius, float insideradius)
    {
        float num = 360f;
        int num2 = 37;
        Mesh mesh = new Mesh();
        Vector3[] array = new Vector3[num2 * 2];
        float num3 = 0.0174532924f * num;
        float num4 = num3 / 2f;
        float num5 = num3 / (float)num2;
        for (int i = 0; i < num2; i++)
        {
            array[i] = new Vector3(Mathf.Cos(num4) * outsideradius, 0f, Mathf.Sin(num4) * outsideradius);
            array[i + num2] = new Vector3(Mathf.Cos(num4) * insideradius, 0f, Mathf.Sin(num4) * insideradius);
            num4 -= num5;
        }
        int[] array2 = new int[num2 * 6];
        for (int j = 0; j < num2; j++)
        {
            int num6 = j * 6;
            array2[num6] = j;
            array2[num6 + 1] = j + num2;
            array2[num6 + 2] = j + num2 - 1;
            array2[num6 + 3] = j;
            array2[num6 + 4] = j + 1;
            array2[num6 + 5] = j + num2;
        }
        mesh.vertices = array;
        mesh.triangles = array2;
        Vector2[] array3 = new Vector2[array.Length];
        for (int k = 0; k < num2; k += 2)
        {
            array3[k] = new Vector2(0f, 0f);
            array3[k + 1] = new Vector2(1f, 0f);
            array3[k + num2 - 1] = new Vector2(0f, 1f);
            array3[k + num2] = new Vector2(1f, 1f);
        }
        mesh.uv = array3;
        return mesh;
    }

    public static Mesh CreateRectangle(float Length, float Width)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-Width / 2f, 0f, 0f),
            new Vector3(Width / 2f, 0f, 0f),
            new Vector3(-Width / 2f, 0f, Length),
            new Vector3(Width / 2f, 0f, Length)
        };
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f)
        };
        int[] triangles = new int[]
        {
            2,
            1,
            0,
            2,
            3,
            1
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        return mesh;
    }

    public static bool CheckFloatEqual(float f1, float f2)
    {
        return (double)Mathf.Abs(f1 - f2) < 1E-08;
    }

    public static string GetCurrenyStr(uint num)
    {
        string result = string.Empty;
        if (num < 100000U)
        {
            result = num.ToString("N0");
        }
        else
        {
            result = (num / 10000U).ToString("N0") + "万";
        }
        return result;
    }

    public static BufferServerDate GetBuffServerData(StateItem serverdata)
    {
        return new BufferServerDate
        {
            giver = CommonTools.GetGiverIDFromHash(serverdata.uniqid),
            flag = CommonTools.GetUserStateFromHash(serverdata.uniqid),
            uniqueid = CommonTools.GetUniqueIDFromHash(serverdata.uniqid),
            addLayer = 1U,
            thisid = serverdata.uniqid,
            settime = serverdata.settime,
            duartion = serverdata.lasttime,
            overtime = serverdata.overtime,
            curTime = 0UL,
            skillid = serverdata.skilluuid,
            configTime = serverdata.configtime,
            effects = serverdata.effects
        };
    }

    public static ulong GenernateBuffHash(EntitiesID Eid, ulong userState, ulong uniqueID)
    {
        return (userState & 65535UL) << 48 | (uniqueID & 255UL) << 40 | (ulong)((ulong)((long)Eid.Etype & 255L) << 32) | (Eid.Id & unchecked((ulong)-1));
    }

    public static ulong GenernateBuffHash(ulong entryID, ulong userState, ulong entrytype, ulong uniqueID)
    {
        return (userState & 65535UL) << 48 | (uniqueID & 255UL) << 40 | (entrytype & 255UL) << 32 | (entryID & unchecked((ulong)-1));
    }

    public static UserState GetUserStateFromHash(ulong uniqueID)
    {
        return (UserState)((short)(uniqueID >> 48 & 65535UL));
    }

    public static ulong GetUniqueIDFromHash(ulong uniqueID)
    {
        return uniqueID >> 40 & 255UL;
    }

    public static ulong GetEntryTypeFromHash(ulong uniqueID)
    {
        return uniqueID >> 32 & 255UL;
    }

    public static ulong GetEntryIDFromHash(ulong uniqueID)
    {
        return uniqueID & unchecked((ulong)-1);
    }

    public static ulong GetGiverIDFromHash(ulong uniqueID)
    {
        return uniqueID & unchecked((ulong)-1);
    }

    public static EntitiesID GetGiverEIDFromHash(ulong uniqueID)
    {
        return new EntitiesID
        {
            Id = CommonTools.GetEntryIDFromHash(uniqueID),
            Etype = (CharactorType)CommonTools.GetEntryTypeFromHash(uniqueID)
        };
    }

    public static string ParseIntToDoubleString(int i)
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (i < 10)
        {
            stringBuilder.Append("0");
            stringBuilder.Append(i.ToString());
        }
        else if (i < 100)
        {
            stringBuilder.Append(i.ToString());
        }
        return stringBuilder.ToString();
    }

    public static string GetOpenChatType()
    {
        return PlayerPrefs.GetString("DB_OPEN_CHAT_TYPE");
    }

    public static void SaveOpenChatType(string str)
    {
        try
        {
            PlayerPrefs.SetString("DB_OPEN_CHAT_TYPE", str);
        }
        catch (Exception ex)
        {
            FFDebug.LogError("GlobalRegister", string.Format("SaveOpenChatType error  :{0}", ex.ToString()));
        }
    }

    public static void SetGameObjectLayer(GameObject go, string layer, bool isContainsChild = true)
    {
        go.layer = LayerMask.NameToLayer(layer);
        if (isContainsChild && go.transform.childCount > 0)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                CommonTools.SetGameObjectLayer(go.transform.GetChild(i).gameObject, layer, true);
            }
        }
    }

    public static void SetGameObjectLayer(GameObject go, int layer, bool isContainsChild = true)
    {
        go.layer = layer;
        if (isContainsChild && go.transform.childCount > 0)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                CommonTools.SetGameObjectLayer(go.transform.GetChild(i).gameObject, layer, true);
            }
        }
    }

    public static void Get3DIconPosAndRot(string data, out Vector3 pos, out Vector3 rot, float[] resolution)
    {
        Vector3 vector = new Vector3(0f, -0.7f, 1.5f);
        Vector3 vector2 = new Vector3(0f, 180f, 0f);
        pos = vector;
        rot = vector2;
        float num = float.MaxValue;
        string[] array = data.Split(new char[]
        {
            ';'
        });
        float num2 = resolution[0] / resolution[1];
        for (int i = 0; i < array.Length; i++)
        {
            if (!string.IsNullOrEmpty(array[i]))
            {
                string[] array2 = array[i].Split(new char[]
                {
                    '|'
                });
                if (array2.Length >= 7)
                {
                    string[] array3 = array2[6].Split(new char[]
                    {
                        '*'
                    });
                    float num3 = 1f;
                    if (array3.Length < 2)
                    {
                        FFDebug.LogError(data, "‘|’分割第六项为分辨率 有错误");
                    }
                    else
                    {
                        num3 = float.Parse(array3[0]) / float.Parse(array3[1]);
                    }
                    float num4 = Mathf.Abs(num2 - num3);
                    if (num4 < num)
                    {
                        num = num4 + 0.001f;
                        pos = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
                        rot = new Vector3(float.Parse(array2[3]), float.Parse(array2[4]), float.Parse(array2[5]));
                        if (array3.Length >= 2 && Mathf.Abs(float.Parse(array3[0]) - resolution[0]) < 1f && Mathf.Abs(float.Parse(array3[1]) - resolution[1]) < 1f)
                        {
                            return;
                        }
                    }
                }
                else if (array2.Length >= 6)
                {
                    pos = new Vector3(float.Parse(array2[0]), float.Parse(array2[1]), float.Parse(array2[2]));
                    rot = new Vector3(float.Parse(array2[3]), float.Parse(array2[4]), float.Parse(array2[5]));
                }
            }
        }
    }

    public static void Get3DIconPosAndRot(uint npcid, out Vector3 pos, out Vector3 rot, float[] resolution, GlobalRegister.ModelIconUIType uiType)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcid);
        if (configTable != null)
        {
            string cacheField_String = configTable.GetCacheField_String("modelposfor3dicon");
            if (uiType == GlobalRegister.ModelIconUIType.Charactor)
            {
                cacheField_String = configTable.GetCacheField_String("modelCon4Cha");
            }
            else if (uiType == GlobalRegister.ModelIconUIType.Guide)
            {
                cacheField_String = configTable.GetCacheField_String("modelCon4Guide");
            }
            else if (uiType == GlobalRegister.ModelIconUIType.Herohandbook)
            {
                cacheField_String = configTable.GetCacheField_String("modelCon4Hero");
            }
            CommonTools.Get3DIconPosAndRot(cacheField_String, out pos, out rot, resolution);
        }
        else
        {
            Vector3 vector = new Vector3(0f, -0.7f, 1.5f);
            Vector3 vector2 = new Vector3(0f, 180f, 0f);
            pos = vector;
            rot = vector2;
        }
    }

    public static void DestroyComponent<T>(Transform t, bool childOrParent = true) where T : UnityEngine.Object
    {
        if (t != null)
        {
            T component = t.gameObject.GetComponent<T>();
            if (component != null)
            {
                UnityEngine.Object.DestroyImmediate(component);
            }
            if (childOrParent)
            {
                if (t.childCount > 0)
                {
                    for (int i = 0; i < t.childCount; i++)
                    {
                        CommonTools.DestroyComponent<T>(t.GetChild(i), childOrParent);
                    }
                }
            }
            else if (t.parent != null)
            {
                CommonTools.DestroyComponent<T>(t.parent, childOrParent);
            }
        }
    }

    public static string GetLuaAttrName(string configName)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("pdamage", "PDamage");
        dictionary.Add("pdefence", "PDefence");
        dictionary.Add("mdamage", "MDamage");
        dictionary.Add("mdefence", "MDefence");
        dictionary.Add("maxhp", "MaxHp");
        if (dictionary.ContainsKey(configName))
        {
            return dictionary[configName];
        }
        return configName[0].ToString().ToUpper() + configName.Substring(1);
    }

    public static string GetFirstSkillIcon(string str)
    {
        return str.Split(new char[]
        {
            ','
        })[0];
    }

    public static string GetTextById(ulong textid)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", textid);
        if (configTable == null)
        {
            return string.Empty;
        }
        string field_String = configTable.GetField_String("tips");
        if (!string.IsNullOrEmpty(field_String))
        {
            return field_String;
        }
        return configTable.GetField_String("notice");
    }

    public static string GetnoticeById(ulong textid)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", textid);
        if (configTable != null)
        {
            return configTable.GetField_String("notice");
        }
        return string.Empty;
    }

    public static uint GetPathWayId(ulong mapId, uint npcId)
    {
        string key = mapId + "_" + npcId.ToString();
        if (CommonTools.mPathWayIdDic.Count == 0)
        {
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("pathway");
            for (int i = 0; i < configTableList.Count; i++)
            {
                string key2 = configTableList[i].GetField_Uint("mapid") + "_" + configTableList[i].GetField_Uint("npcid");
                uint field_Uint = configTableList[i].GetField_Uint("pathwayid");
                if (!CommonTools.mPathWayIdDic.ContainsKey(key2))
                {
                    CommonTools.mPathWayIdDic.Add(key2, field_Uint);
                }
            }
        }
        if (CommonTools.mPathWayIdDic.ContainsKey(key))
        {
            return CommonTools.mPathWayIdDic[key];
        }
        return 0U;
    }

    public static string GetTextWithoutColorModel(string txt)
    {
        string[] array = txt.Split(new char[]
        {
            ':'
        });
        string text = txt.Substring(array[0].Length);
        Regex regex = new Regex("<[^>]*>", RegexOptions.IgnoreCase);
        Regex regex2 = new Regex("\\[.*?\\]", RegexOptions.Multiline);
        text = regex.Replace(text, string.Empty);
        text = regex2.Replace(text, string.Empty);
        return array[0] + text;
    }

    public static string GetOfflineText(uint time)
    {
        string result = string.Empty;
        uint num = SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond() - time;
        if (num < 60U)
        {
            result = "离线";
        }
        else if (num < 3600U)
        {
            result = "离线" + SingletonForMono<GameTime>.Instance.GetMinBySecond((ulong)num) + "分钟";
        }
        else if (num < 86400U)
        {
            result = "离线" + SingletonForMono<GameTime>.Instance.GetHorBySecond((ulong)num) + "小时";
        }
        else if (num < 2592000U)
        {
            result = "离线" + SingletonForMono<GameTime>.Instance.GetDayBySecond((ulong)num) + "天";
        }
        else
        {
            result = "长期离线";
        }
        return result;
    }

    public static string GetLevelFormat(uint level)
    {
        return level + "级";
    }

    public static string GetLevelFormat(string level)
    {
        return level + "级";
    }

    public static Color GetQualityColor(uint quality)
    {
        return Const.GetColorByName("quality" + quality);
    }

    public static void SetFaceIcon(string spriteName, Image img)
    {
        BiaoqingManager.ImageData image = ManagerCenter.Instance.GetManager<BiaoqingManager>().GetImage(spriteName);
        ImageLoop imageLoop = img.GetComponent<ImageLoop>();
        if (imageLoop == null)
        {
            imageLoop = img.gameObject.AddComponent<ImageLoop>();
        }
        imageLoop.Initilize(image, img);
        img.color = Color.white;
        img.gameObject.SetActive(true);
    }

    public static CanvasScaler mCanvasScaler
    {
        get
        {
            if (CommonTools._canvasScaler == null)
            {
                CommonTools._canvasScaler = GameObject.Find("UIRoot").GetComponent<CanvasScaler>();
            }
            return CommonTools._canvasScaler;
        }
    }

    public static void SetUIScale(int num)
    {
        if (num < 20)
        {
            Debug.LogError("ui scale num < 20.it's too small.");
            return;
        }
        float scaleFactor = (float)num / 100f;
        CommonTools.mCanvasScaler.scaleFactor = scaleFactor;
    }

    public static void SetMouseSpeed(uint num)
    {
        if (num > 20U || num < 1U)
        {
            Debug.LogError("ui MouseSpeed num > 20 || num < 1. it's out of range.");
            return;
        }
    }

    public static void SetPixelPercent(uint num)
    {
        if (num < 1U || num > 100U)
        {
            Debug.LogError("ui PixelPercent num < 1 || num > 100 it's out of range.");
            return;
        }
    }

    public static void SetScalerMode(CanvasScaler.ScaleMode mode)
    {
        CommonTools.mCanvasScaler.uiScaleMode = mode;
    }

    public static t_Object GetItemData(string thisid)
    {
        LuaTable luaTable = null;
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemData", new object[]
        {
            thisid
        });
        if (array.Length > 0)
        {
            luaTable = (array[0] as LuaTable);
        }
        if (luaTable != null)
        {
            return ControllerManager.Instance.GetController<ItemTipController>().GetObjectData(array[0] as LuaTable);
        }
        return null;
    }

    public static bool IsCurrentInFubenScence()
    {
        uint mapid = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        return CommonTools.GetCopymapInfoByMapid(mapid) != null;
    }

    public static LuaTable GetCopymapInfoByMapid(uint mapid)
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("copymapinfo");
        for (int i = 0; i < configTableList.Count; i++)
        {
            uint field_Uint = configTableList[i].GetField_Uint("mapid");
            if (field_Uint == mapid)
            {
                return configTableList[i];
            }
        }
        return null;
    }

    public static void SetSecondPanelPos(PointerEventData eventData)
    {
        CommonTools.secondPosEventData = eventData;
    }

    public static Vector2 GetSecondPanelPos()
    {
        Canvas canvas = UIManager.FindInParents<Canvas>(UIManager.Instance.UIRoot.gameObject);
        Vector2 result;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, CommonTools.secondPosEventData.position, CommonTools.secondPosEventData.pressEventCamera, out result))
        {
            return result;
        }
        return Vector2.zero;
    }

    public static Vector2 GetSecondPanelAreaPos(Vector2 sizeData)
    {
        Vector2 sizeDelta = (UIManager.Instance.UIRoot as RectTransform).sizeDelta;
        Vector2 secondPanelPos = CommonTools.GetSecondPanelPos();
        if (secondPanelPos == Vector2.zero)
        {
            return secondPanelPos;
        }
        if (secondPanelPos.x + sizeData.x > sizeDelta.x / 2f)
        {
            secondPanelPos.x -= sizeData.x;
        }
        if (secondPanelPos.y - sizeData.y < -sizeDelta.y / 2f)
        {
            secondPanelPos.y += sizeData.y;
        }
        return secondPanelPos;
    }

    private static Dictionary<string, uint> mPathWayIdDic = new Dictionary<string, uint>();

    private static CanvasScaler _canvasScaler;

    private static PointerEventData secondPosEventData;
}
