using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class FFCharacterModelHold
{
    private static CharacterModelMgr mCharacterModelMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CharacterModelMgr>();
        }
    }

    public ModleObjInPool BonePObj { get; private set; }

    public GameObject BoneUObj
    {
        get
        {
            if (this.BonePObj != null && this.BonePObj.ItemObj != null)
            {
                return this.BonePObj.ItemObj;
            }
            return null;
        }
    }

    public static bool CreateModel(uint heroid, uint bodyid, uint[] featureIDs, string bonename, Action<PlayerCharactorCreateHelper> callback, Action<PlayerCharactorCreateHelper> beforeCreate = null, ulong keyid = 0UL)
    {
        string text = string.Empty;
        string body = string.Empty;
        string facestyle = string.Empty;
        string hairstyle = string.Empty;
        string haircolor = string.Empty;
        string antenna = string.Empty;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)heroid);
        if (configTable == null)
        {
            FFDebug.LogWarning("CreatePlayer", "npc table not exist heroid = " + heroid);
            return false;
        }
        int cacheField_Int = configTable.GetCacheField_Int("kind");
        string respath = string.Empty;
        CharactorAndEffectBundleType btype = CharactorAndEffectBundleType.Avatar;
        if (cacheField_Int == 16)
        {
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("heros", (ulong)heroid);
            if (configTable2 == null)
            {
                FFDebug.LogWarning("CreatePlayer", "heros table not exist heroid = " + heroid);
                return false;
            }
            uint cacheField_Uint = configTable2.GetCacheField_Uint("newavatar");
            LuaTable configTable3 = LuaConfigManager.GetConfigTable("avatar_config", (ulong)cacheField_Uint);
            if (bodyid <= 0U && configTable3 != null)
            {
                bodyid = configTable3.GetCacheField_Uint("body");
            }
            LuaTable configTable4 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)bodyid);
            if (configTable4 == null)
            {
                FFDebug.LogWarning("CreateCharacterByFeatureData not exist bodyid", bodyid);
                return false;
            }
            text = configTable4.GetCacheField_String("resourcefile");
            body = configTable4.GetCacheField_String("resource");
            if (featureIDs != null && featureIDs.Length >= 4)
            {
                LuaTable configTable5 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)featureIDs[0]);
                uint num = featureIDs[1];
                if (num <= 0U && configTable3 != null)
                {
                    num = configTable3.GetCacheField_Uint("hair");
                }
                LuaTable configTable6 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)num);
                uint num2 = featureIDs[2];
                if (num2 <= 0U && configTable3 != null)
                {
                    num2 = configTable3.GetCacheField_Uint("head");
                }
                LuaTable configTable7 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)num2);
                LuaTable configTable8 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)featureIDs[3]);
                if (configTable5 != null)
                {
                    haircolor = configTable5.GetCacheField_String("resource");
                }
                if (configTable6 != null)
                {
                    hairstyle = configTable6.GetCacheField_String("resource");
                }
                if (configTable7 != null)
                {
                    facestyle = configTable7.GetCacheField_String("resource");
                }
                if (configTable8 != null)
                {
                    antenna = configTable8.GetCacheField_String("resource");
                }
                respath = "avatar/" + text.ToLower();
                if (featureIDs.Length > 4 && featureIDs[4] > 0U)
                {
                    LuaTable configTable9 = LuaConfigManager.GetConfigTable("avatar_config", (ulong)featureIDs[4]);
                    if (configTable9 != null)
                    {
                        bonename = configTable9.GetField_String("animatorcontroller");
                    }
                }
            }
        }
        else
        {
            btype = CharactorAndEffectBundleType.Charactor;
            body = configTable.GetField_String("model");
            bonename = (text = configTable.GetField_String("animatorcontroller"));
        }
        PlayerCharactorCreateHelper.LoadBody(respath, btype, bonename, text, body, haircolor, hairstyle, facestyle, antenna, delegate (PlayerCharactorCreateHelper o)
        {
            if (o.rootObj.GetComponent<HighlighterController>() == null)
            {
                o.rootObj.AddComponent<HighlighterController>();
            }
            if (featureIDs != null)
            {
                o.TrySetFaceColorRamp(heroid, featureIDs[2].ToString(), featureIDs[0].ToString());
            }
            o.rootObj.transform.SetParent(FFCharacterModelHold.PoolManager.ObjectPoolRoot, false);
            callback(o);
        }, beforeCreate, false, keyid);
        return true;
    }

    public static bool CreatePlayer(uint heroid, Action<PlayerCharactorCreateHelper> callback)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetConfig("heros").GetCacheField_Table(heroid);
        if (cacheField_Table == null)
        {
            FFDebug.LogWarning("CreatePlayer By Heroid config not exist heroid : ", heroid.ToString());
            return false;
        }
        string cacheField_String = cacheField_Table.GetCacheField_String("newavatar");
        if (string.IsNullOrEmpty(cacheField_String))
        {
            FFDebug.LogWarning("CreatePlayer By Heroid defaultFashion is empty : ", heroid.ToString());
            return false;
        }
        ulong id = 0UL;
        ulong.TryParse(cacheField_String, out id);
        LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", id);
        if (configTable == null)
        {
            return false;
        }
        uint cacheField_Uint = configTable.GetCacheField_Uint("hair");
        uint cacheField_Uint2 = configTable.GetCacheField_Uint("body");
        uint cacheField_Uint3 = configTable.GetCacheField_Uint("head");
        uint[] array = new uint[4];
        array[1] = cacheField_Uint;
        array[2] = cacheField_Uint3;
        uint[] featureIDs = array;
        FFCharacterModelHold.CreateModel(heroid, cacheField_Uint2, featureIDs, FFCharacterModelHold.GetACname(heroid), delegate (PlayerCharactorCreateHelper modelHoldHelper)
        {
            callback(modelHoldHelper);
        }, null, 0UL);
        return true;
    }

    public static string GetACname(uint npcorheroid)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(npcorheroid);
        if (cacheField_Table != null)
        {
            string field_String = cacheField_Table.GetField_String("animatorcontroller");
            if (!string.IsNullOrEmpty(field_String))
            {
                return field_String;
            }
        }
        return "none";
    }

    public void RefreshCharacterAvatorModel()
    {
    }

    private static ObjectPoolManager PoolManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<ObjectPoolManager>();
        }
    }

    public static void UnloadAllbeside(List<string> Besides)
    {
        FFCharacterModelHold.RemoveList.Clear();
        for (int i = 0; i < FFCharacterModelHold.AllBoneObjPoolKey.Count; i++)
        {
            if (!Besides.Contains(FFCharacterModelHold.AllBoneObjPoolKey[i]))
            {
                FFCharacterModelHold.RemoveList.Add(FFCharacterModelHold.AllBoneObjPoolKey[i]);
            }
        }
        for (int j = 0; j < FFCharacterModelHold.RemoveList.Count; j++)
        {
            FFCharacterModelHold.PoolManager.RemoveObjectPool(FFCharacterModelHold.RemoveList[j], true);
            FFCharacterModelHold.AllBoneObjPoolKey.Remove(FFCharacterModelHold.RemoveList[j]);
        }
    }

    public string BoneObjPoolKey;

    public string SkinName;

    public string BoneName;

    private static List<string> AllBoneObjPoolKey = new List<string>();

    private static List<string> RemoveList = new List<string>();
}
