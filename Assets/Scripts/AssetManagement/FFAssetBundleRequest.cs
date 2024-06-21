using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Managers;
using ProtoBuf;
using ResoureManager;
using UnityEngine;

public class FFAssetBundleRequest
{
    private FFAssetBundle mFFAB;
    private List<Action<FFAssetBundle>> CallbackList = new List<Action<FFAssetBundle>>();
    private bool _finish;

    public static MonoBehaviour MbHold;
    private static readonly BetterDictionary<string, CharactorAndEffectDepData> AllAssetBundleManifest = new BetterDictionary<string, CharactorAndEffectDepData>();
    private static readonly BetterDictionary<string, FFAssetBundleRequest> AllAssetBundleRequest = new BetterDictionary<string, FFAssetBundleRequest>();
    private static readonly BetterDictionary<string, FFAssetBundleRequest> AssetBundleRequestAddCache = new BetterDictionary<string, FFAssetBundleRequest>();

    private FFAssetBundleRequest(FFAssetBundle FFAB)
    {
        mFFAB = FFAB;
        AssetManager.LoadAssetBundle(mFFAB.Path, (assetBundleName, succeed) =>
        {
            if (!mFFAB.NeedLoadAsset)
            {
                mFFAB.allAssets = new UnityEngine.Object[0];
                AssetBundleRequestFinish();
                return;
            }
            if (succeed)
            {
                Bundle bundle = AssetManager.GetBundle(assetBundleName);
                if (bundle != null)
                {
                    mFFAB.mBundle = bundle;
                    mFFAB.assetBundle = mFFAB.mBundle.Assetbundle;
                    if (MbHold != null)
                    {
                        MbHold.StartCoroutine(LoadAllAssetFromAssetbundle());
                    }
                }
                else
                {
                    AssetBundleRequestFinish();
                    FFDebug.LogWarning(this, "WwwReq null: " + mFFAB.Path);
                }
            }
            else
            {
                AssetBundleRequestFinish();
                FFDebug.LogWarning(this, "WwwReq null: " + mFFAB.Path);
            }
        }, Bundle.BundleType.EffectAndcharacter);
    }

    public static void Reset()
    {
        AllAssetBundleManifest.Clear();
        AllAssetBundleRequest.Clear();
        AssetBundleRequestAddCache.Clear();
    }

    public static void CleverRequest(CharactorAndEffectBundleType type, string strAssetBundleName, Action<FFAssetBundle> callback)
    {
        if (string.IsNullOrEmpty(strAssetBundleName))
        {
            callback(null);
            return;
        }
        string strPkgName = GetPkgName(type);
        string strBundleName = strAssetBundleName.ToLower();
        string strPath = strPkgName + "/" + strBundleName + ".u";
        if (type == CharactorAndEffectBundleType.Charactor || type == CharactorAndEffectBundleType.Effect ||
            type == CharactorAndEffectBundleType.UIEffect || type == CharactorAndEffectBundleType.Avatar ||
            type == CharactorAndEffectBundleType.NPCBulilding)
        {
            if (AllAssetBundleManifest.ContainsKey(strPath))
            {
                CleverRequest1(strPkgName, strBundleName, callback, true);
            }
            else
            {
                LoadDependcyData(strPkgName, strBundleName, delegate (CharactorAndEffectDepData data)
                {
                    AllAssetBundleManifest[strPath] = data;
                    CleverRequest1(strPkgName, strBundleName, callback, true);
                });
            }
        }
        else
        {
            Request(strPkgName, strBundleName, callback, true);
        }
    }

    private static void LoadDependcyData(string strPkgName, string strBundleName, Action<CharactorAndEffectDepData> callback)
    {
        string strPath = strPkgName + "/" + strBundleName + ".bytes";
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(strPath, delegate (byte[] bytes)
        {
            if (bytes != null)
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    CharactorAndEffectDepData obj = Serializer.Deserialize<CharactorAndEffectDepData>(memoryStream);
                    if (callback != null)
                    {
                        callback(obj);
                    }
                    memoryStream.Close();
                }
            }
            else if (callback != null)
            {
                callback(null);
            }
        });
    }

    private static void CleverRequest1(string PkgName, string AssetBundleName, Action<FFAssetBundle> callback, bool LoadAsset = true)
    {
        string path = PkgName + "/" + AssetBundleName + ".u";
        FFAssetBundle FFab = FFAssetBundle.GetFFAssetBundle(PkgName, AssetBundleName);
        FFab.NeedLoadAsset = LoadAsset;
        CharactorAndEffectDepData charactorAndEffectDepData;
        if (AllAssetBundleManifest.TryGetValue(path, out charactorAndEffectDepData))
        {
            if (charactorAndEffectDepData != null)
            {
                FFab.Dependencies = charactorAndEffectDepData.lstDepData.ToArray();
            }
            else
            {
                FFab.Dependencies = null;
            }
        }
        else
        {
            FFab.Dependencies = null;
        }
        if (FFab.Dependencies == null)
        {
            Request(PkgName, AssetBundleName, callback, LoadAsset);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < FFab.Dependencies.Length; i++)
            {
                CleverRequest1(PkgName, FFab.Dependencies[i], delegate (FFAssetBundle res)
                {
                    if (!res.IsLive)
                    {
                        FFDebug.LogWarning("FFAssetBundleRequest", " Load " + AssetBundleName + " Dependencies Error: " + res.Path);
                    }
                    else
                    {
                        res.AddUse(path);
                    }
                    index++;
                    if (index == FFab.Dependencies.Length)
                    {
                        Request(PkgName, AssetBundleName, callback, LoadAsset);
                    }
                }, true);
            }
        }
    }

    public static void Request(string PkgName, string AssetBundleName, Action<FFAssetBundle> callback, bool LoadAsset = true)
    {
        string key = PkgName.ToLower() + "/" + AssetBundleName.ToLower() + ".u";
        FFAssetBundle ffassetBundle = FFAssetBundle.GetFFAssetBundle(PkgName.ToLower(), AssetBundleName.ToLower());
        if (ffassetBundle.IsLive)
        {
            callback(ffassetBundle);
            return;
        }
        FFAssetBundleRequest ffassetBundleRequest;
        if (!AllAssetBundleRequest.TryGetValue(key, out ffassetBundleRequest))
        {
            if (AssetBundleRequestAddCache.ContainsKey(key))
            {
                ffassetBundleRequest = AssetBundleRequestAddCache[key];
            }
            else
            {
                ffassetBundleRequest = new FFAssetBundleRequest(ffassetBundle);
                AssetBundleRequestAddCache[key] = ffassetBundleRequest;
            }
        }
        if (ffassetBundleRequest.Finish)
        {
            callback(ffassetBundleRequest.mFFAB);
        }
        else
        {
            ffassetBundleRequest.CallbackList.Add(callback);
        }
    }

    public static void UpdateALL()
    {
        if (AllAssetBundleRequest.Count == 0 && AssetBundleRequestAddCache.Count == 0) return;

        foreach (var item in AssetBundleRequestAddCache)
        {
            if (!item.Value.Finish)
            {
                AllAssetBundleRequest[item.Key] = item.Value;
            }
        }
        AssetBundleRequestAddCache.Clear();

        var finishedRequests = new List<string>();
        foreach (var item in AllAssetBundleRequest)
        {
            if (item.Value.Finish)
            {
                finishedRequests.Add(item.Key);
            }
            else
            {
                item.Value.Update();
            }
        }

        foreach (var key in finishedRequests)
        {
            AllAssetBundleRequest.Remove(key);
        }
    }

    private static string GetPkgName(CharactorAndEffectBundleType type)
    {
        switch (type)
        {
            case CharactorAndEffectBundleType.Charactor:
                return "characters";
            case CharactorAndEffectBundleType.Effect:
                return "effect";
            case CharactorAndEffectBundleType.AnimatorController:
                return "animatorcontroller";
            case CharactorAndEffectBundleType.Config:
                return "config";
            case CharactorAndEffectBundleType.NPCTalk:
                return "npctalktransformdata";
            case CharactorAndEffectBundleType.Weapon:
                return "weapon";
            case CharactorAndEffectBundleType.Avatar:
                return "characters";
            case CharactorAndEffectBundleType.Other:
                return "objects";
            case CharactorAndEffectBundleType.UIEffect:
                return "uieffect";
            case CharactorAndEffectBundleType.CommonEffect:
                return "commoneffect";
            case CharactorAndEffectBundleType.CharactorTexture:
                return "characters/characterstexture";
            case CharactorAndEffectBundleType.NPCBulilding:
                return "Scenes/NPCBuilding";
            default:
                return null;
        }
    }

    public bool Finish
    {
        get { return _finish; }
    }

    private void Update() { }

    private IEnumerator LoadAllAssetFromAssetbundle()
    {
        mFFAB.AssetBundleReq = mFFAB.assetBundle.LoadAllAssetsAsync();
        yield return mFFAB.AssetBundleReq;
        AssetBundleRequestFinish();
    }

    private void AssetBundleRequestFinish()
    {
        if (mFFAB.AssetBundleReq != null && mFFAB.assetBundle != null)
        {
            mFFAB.allAssets = mFFAB.AssetBundleReq.allAssets;
            if (mFFAB.AssetBundleReq.allAssets == null || mFFAB.AssetBundleReq.allAssets.Length == 0)
            {
                FFDebug.LogWarning(this, "AssetBundleReq.allAssets " + (mFFAB.AssetBundleReq.allAssets == null ? "null" : "Length 0") + ": " + mFFAB.Path);
            }
        }
        foreach (var action in CallbackList)
        {
            try
            {
                if(action != null)
                    action.Invoke(mFFAB);
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, "AssetBundleRequestFinish callback error: " + arg);
            }
        }
        CallbackList.Clear();
        _finish = true;
        mFFAB.AssetBundleReq = null;
    }
}

public enum CharactorAndEffectBundleType
{
    Charactor,
    Effect,
    AnimatorController,
    Config,
    NPCTalk,
    Weapon,
    Avatar,
    Other,
    UIEffect,
    CommonEffect,
    CharactorTexture,
    NPCBulilding,
}