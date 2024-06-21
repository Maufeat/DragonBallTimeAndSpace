using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
    public static string SelfName
    {
        get
        {
            return "AssetManager";
        }
    }

    public static void Init(MonoBehaviour mono)
    {
        AssetManager._mono = mono;
        AssetManager._MaxLoadingCount = ResourceHelper.MaxDownLoadAssetBundleCount;
        AssetManager.ReadyToUse = true;
    }

    public static void LoadAsset(string assetBundlePath, string assetName, Type assetType, OnAssetLoadComplete OnAssetLoadCompleteCallback, Bundle.BundleType BType = Bundle.BundleType.Default)
    {
        try
        {
            if (!AssetManager.ReadyToUse)
            {
                throw new Exception("AssetManager is not ready to use");
            }
            if (OnAssetLoadCompleteCallback == null)
            {
                throw new Exception("LoadAsset callback func is null");
            }
            if (!AssetManager.IsFileExist(assetBundlePath))
            {
                OnAssetLoadCompleteCallback(assetBundlePath, assetName, null);
            }
            else
            {
                AssetData assetData = new AssetData(AssetLoadType.LoadAsset, assetBundlePath);
                assetData.AssetName = assetName;
                assetData.AssetType = assetType;
                assetData.OnAssetLoadComplete = OnAssetLoadCompleteCallback;
                AssetManager.ProcessTask(assetData);
                if (assetData.Bundle != null)
                {
                    assetData.Bundle.BType = BType;
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError("AssetManager", string.Format("LoadAsset({0}, {1}, {2}) error : {3}", new object[]
            {
                assetBundlePath,
                assetName,
                assetType.ToString(),
                ex.ToString()
            }));
        }
    }

    public static void LoadAssetBundle(string assetbundlePath, OnAssetBundleLoadComplete assetBundleLoadCompleteCallback, Bundle.BundleType BType = Bundle.BundleType.Default)
    {
        try
        {
            if (!AssetManager.ReadyToUse)
            {
                throw new Exception("AssetManager is not ready to use");
            }
            if (assetBundleLoadCompleteCallback == null)
            {
                throw new Exception("LoadAssetBundle callback func is null");
            }
            if (!AssetManager.IsFileExist(assetbundlePath))
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Assetbundle file not exist:[" + assetbundlePath + "]");
                assetBundleLoadCompleteCallback(assetbundlePath, false);
            }
            else
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Request download asetbundle:[" + assetbundlePath + "]");
                AssetData assetData = new AssetData(AssetLoadType.LoadAssetBundle, assetbundlePath);
                assetData.OnAssetBundleLoadComplete = assetBundleLoadCompleteCallback;
                AssetManager.ProcessTask(assetData);
                if (assetData.Bundle != null)
                {
                    assetData.Bundle.BType = BType;
                }
            }
        }
        catch (Exception ex)
        {
            string str = string.Format("LoadAssetBundle({0}) error : {1}", assetbundlePath, ex.Message);
            FFDebug.LogError(AssetManager.SelfName, str);
        }
    }

    private static void ProcessTask(AssetData data)
    {
        UnityEngine.Object asset = null;
        if (!AssetManager.IsNeedDownloadTask(data, ref asset))
        {
            if (data.AssetLoadType == AssetLoadType.LoadAsset)
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new string[]
                {
                    "LoadAsset Already in mapBundle,OnAssetloadComplete,assetbundlepath:[",
                    data.FileName,
                    "],assetname:][",
                    data.AssetName,
                    "]"
                }));
                if (data.OnAssetLoadComplete != null)
                {
                    data.OnAssetLoadComplete(data.FileName, data.AssetName, asset);
                }
            }
            else if (data.OnAssetBundleLoadComplete != null)
            {
                if (data.DownloadTaskState == DownloadTaskState.Error)
                {
                    data.OnAssetBundleLoadComplete(data.FileName, false);
                }
                else
                {
                    FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "LoadAssetBundle Already in mapBundle,OnAssetBundleLoadComplete,assetbundlepath:[" + data.FileName + "]");
                    data.OnAssetBundleLoadComplete(data.FileName, true);
                }
            }
            return;
        }
        DownLoadTaskCollection downLoadTaskCollection = null;
        if (AssetManager._mapDownLoadTaskCollection.TryGetValue(data.FileName, out downLoadTaskCollection))
        {
            if (downLoadTaskCollection.AssetBundleCreateRequest == null)
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Alrady Exist a Task[" + data.FileName + "] in the queue, but AssetBundleCreateRequest is NULL");
                downLoadTaskCollection.DownLoadTaskList.Add(data);
                return;
            }
            if (!downLoadTaskCollection.AssetBundleCreateRequest.isDone)
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Alrady Exist a AssetBundleCreateRequest[" + data.FileName + "] downloading in process.");
                data.AssetBundleCreateRequest = downLoadTaskCollection.AssetBundleCreateRequest;
                data.DownloadTaskState = DownloadTaskState.WaitCreateAssetBundleFinish;
                downLoadTaskCollection.DownLoadTaskList.Add(data);
                return;
            }
            if (downLoadTaskCollection.AssetBundleCreateRequest.progress == 0f || null == downLoadTaskCollection.AssetBundleCreateRequest.assetBundle)
            {
                FFDebug.LogWarning(AssetManager.SelfName, "Alrady Exist a AssetBundleCreateRequest[" + data.FileName + "] download complate, but AssetBundleCreateRequest is Wrong.");
                AssetManager._mapDownLoadTaskCollection.Remove(data.FileName);
            }
            else
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Alrady Exist a AssetBundleCreateRequest[" + data.FileName + "] download complate.");
                data.AssetBundleCreateRequest = downLoadTaskCollection.AssetBundleCreateRequest;
                if (data.AssetLoadType != AssetLoadType.LoadAsset)
                {
                    data.DownloadTaskState = DownloadTaskState.ReadyToUse;
                    downLoadTaskCollection.DownLoadTaskList.Add(data);
                    return;
                }
                data.DownloadTaskState = DownloadTaskState.LoadAsset;
            }
        }
        FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "NewTask[" + data.FileName + "] ");
        downLoadTaskCollection = new DownLoadTaskCollection();
        downLoadTaskCollection.DownLoadTaskList.Add(data);
        downLoadTaskCollection.AssetBundlePath = data.FileName;
        AssetManager._mapDownLoadTaskCollection[data.FileName] = downLoadTaskCollection;
        AssetManager._AssetTasks.Enqueue(data);
    }

    private static bool CheckDownLoadTaskNeedDelete(DownLoadTaskCollection Task)
    {
        if (Task.NeedDelete)
        {
            Task.Clear();
            AssetManager._mapDownLoadTaskCollection.Remove(Task.AssetBundlePath);
            return true;
        }
        return false;
    }

    public static void Update()
    {
        if (AssetManager.ReadyToUse)
        {
            int num = AssetManager._MaxLoadingCount - AssetManager._lstLoadingTaskCollection.Count;
            DownLoadTaskCollection downLoadTaskCollection = null;
            if (AssetManager._AssetTasks.Count > 0)
            {
                int num2 = (num <= AssetManager._AssetTasks.Count) ? num : AssetManager._AssetTasks.Count;
                for (int i = 0; i < num2; i++)
                {
                    AssetData assetData = AssetManager._AssetTasks.Dequeue();
                    if (assetData != null)
                    {
                        downLoadTaskCollection = null;
                        if (AssetManager._mapDownLoadTaskCollection.TryGetValue(assetData.FileName, out downLoadTaskCollection))
                        {
                            downLoadTaskCollection.DownloadTaskState = assetData.DownloadTaskState;
                            AssetManager._lstLoadingTaskCollection.Add(downLoadTaskCollection);
                        }
                    }
                }
            }
        }
    }

    public static void LateUpdate()
    {
        if (AssetManager.ReadyToUse)
        {
            for (int i = 0; i < AssetManager._lstLoadingTaskCollection.Count; i++)
            {
                if (AssetManager._lstLoadingTaskCollection[i] != null)
                {
                    switch (AssetManager._lstLoadingTaskCollection[i].DownloadTaskState)
                    {
                        case DownloadTaskState.CreateAssetbundle:
                            AssetManager.CreateAssetBundle(AssetManager._lstLoadingTaskCollection[i]);
                            break;
                        case DownloadTaskState.WaitCreateAssetBundleFinish:
                            AssetManager.WaitCreateAssetBundle(AssetManager._lstLoadingTaskCollection[i]);
                            break;
                        case DownloadTaskState.LoadAsset:
                            AssetManager.LoadAsset(AssetManager._lstLoadingTaskCollection[i]);
                            break;
                        case DownloadTaskState.WaitLoadAssetFinish:
                            AssetManager.WaitLoadAssetFinish(AssetManager._lstLoadingTaskCollection[i]);
                            break;
                        case DownloadTaskState.ReadyToUse:
                        case DownloadTaskState.Error:
                            AssetManager.FinishAssetTask(AssetManager._lstLoadingTaskCollection[i]);
                            break;
                    }
                }
            }
            for (int j = 0; j < AssetManager._removeTmpList.Count; j++)
            {
                DownLoadTaskCollection downLoadTaskCollection = AssetManager._removeTmpList[j];
                if (downLoadTaskCollection != null)
                {
                    AssetManager._lstLoadingTaskCollection.Remove(downLoadTaskCollection);
                    AssetManager._mapDownLoadTaskCollection.Remove(downLoadTaskCollection.AssetBundlePath);
                }
            }
            AssetManager._removeTmpList.Clear();
        }
    }

    public static bool IsNeedDownloadTask(AssetData data, ref UnityEngine.Object asset)
    {
        bool result;
        try
        {
            if (data.DownloadTaskState != DownloadTaskState.Wait)
            {
                throw new Exception("Wrong task status");
            }
            if (data.AssetLoadType == AssetLoadType.LoadAsset)
            {
                Bundle bundle = AssetManager.GetBundle(data.FileName);
                if (bundle.Status == Bundle.BundleStatus.Error)
                {
                    throw new Exception(bundle.Message);
                }
                data.Bundle = bundle;
                UnityEngine.Object asset2 = bundle.GetAsset(data.AssetName);
                if (null != asset2)
                {
                    asset = asset2;
                    data.DownloadTaskState = DownloadTaskState.ReadyToUse;
                    FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new string[]
                    {
                        "Asset Already complete,assetbundlepath:[",
                        data.FileName,
                        "],assetname:][",
                        data.AssetName,
                        "]"
                    }));
                    return false;
                }
                if (bundle.Status == Bundle.BundleStatus.Need_To_Load)
                {
                    data.DownloadTaskState = DownloadTaskState.CreateAssetbundle;
                }
                else if (bundle.Status == Bundle.BundleStatus.Ready_To_Use)
                {
                    data.DownloadTaskState = DownloadTaskState.LoadAsset;
                }
            }
            else if (data.AssetLoadType == AssetLoadType.LoadAssetBundle)
            {
                Bundle bundle2 = AssetManager.GetBundle(data.FileName);
                if (bundle2.Status == Bundle.BundleStatus.Error)
                {
                    throw new Exception(bundle2.Message);
                }
                data.Bundle = bundle2;
                if (bundle2.Status == Bundle.BundleStatus.Need_To_Load)
                {
                    data.DownloadTaskState = DownloadTaskState.CreateAssetbundle;
                }
                else if (bundle2.Status == Bundle.BundleStatus.Ready_To_Use)
                {
                    data.DownloadTaskState = DownloadTaskState.ReadyToUse;
                    FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "AssetBundle Already complete,assetbundlepath:[" + data.FileName + "]");
                    return false;
                }
            }
            FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new string[]
            {
                "Object Need Load,assetLoadType:[",
                data.AssetLoadType.ToString(),
                "]assetbundlepath:[",
                data.FileName,
                "]assetname:[",
                data.AssetName,
                "],taskDownloadState:[",
                data.DownloadTaskState.ToString(),
                "]"
            }));
            result = true;
        }
        catch (Exception ex)
        {
            string message = string.Format("InitializeAssetData({0}) error : {1}", data.ToString(), ex.Message);
            data.Message = message;
            data.DownloadTaskState = DownloadTaskState.Error;
            result = true;
        }
        return result;
    }

    private static void CreateAssetBundle(DownLoadTaskCollection data)
    {
        try
        {
            if (data.DownloadTaskState != DownloadTaskState.CreateAssetbundle)
            {
                throw new Exception("Wrong task status");
            }
            data.DownloadTaskState = DownloadTaskState.WaitCreateAssetBundleFinish;
            data.AssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(data.FilePath);
        }
        catch (Exception ex)
        {
            string text = string.Format("CreateAssetBundle({0}) error : {1}", data.ToString(), ex.Message);
            for (int i = 0; i < data.DownLoadTaskList.Count; i++)
            {
                data.DownLoadTaskList[i].Bundle.Message = text;
                data.DownLoadTaskList[i].Bundle.Status = Bundle.BundleStatus.Error;
                data.DownLoadTaskList[i].Message = text;
            }
            data.DownloadTaskState = DownloadTaskState.Error;
            FFDebug.LogError("AssetManager", text);
        }
    }

    private static void WaitCreateAssetBundle(DownLoadTaskCollection data)
    {
        try
        {
            if (data.DownloadTaskState != DownloadTaskState.WaitCreateAssetBundleFinish)
            {
                throw new Exception("Wrong task status");
            }
            if (data.AssetBundleCreateRequest == null)
            {
                throw new Exception("AssetBundleCreateRequest is null");
            }
            if (data.AssetBundleCreateRequest.isDone)
            {
                AssetBundle assetBundle = data.AssetBundleCreateRequest.assetBundle;
                string assetBundlePath = data.AssetBundlePath;
                if (AssetManager.CheckDownLoadTaskNeedDelete(data))
                {
                    assetBundle.Unload(true);
                    FFDebug.LogWarning("AssetManager", " Delete when Load over : " + assetBundlePath);
                }
                else
                {
                    if (null == assetBundle)
                    {
                        throw new Exception("AssetBundle is null");
                    }
                    for (int i = 0; i < data.DownLoadTaskList.Count; i++)
                    {
                        data.DownLoadTaskList[i].Bundle.Assetbundle = assetBundle;
                        data.DownLoadTaskList[i].Bundle.Status = Bundle.BundleStatus.Ready_To_Use;
                        data.DownLoadTaskList[i].AssetBundleCreateRequest = null;
                        if (data.DownLoadTaskList[i].AssetLoadType == AssetLoadType.LoadAsset)
                        {
                            data.DownloadTaskState = DownloadTaskState.LoadAsset;
                            return;
                        }
                    }
                    data.DownloadTaskState = DownloadTaskState.ReadyToUse;
                }
            }
        }
        catch (Exception ex)
        {
            string text = string.Format("WaitCreateAssetBundle({0}) error : {1}", data.ToString(), ex.Message);
            for (int j = 0; j < data.DownLoadTaskList.Count; j++)
            {
                data.DownLoadTaskList[j].Bundle.Message = text;
                data.DownLoadTaskList[j].Bundle.Status = Bundle.BundleStatus.Error;
                data.DownLoadTaskList[j].DownloadTaskState = DownloadTaskState.Error;
            }
            data.DownloadTaskState = DownloadTaskState.Error;
            FFDebug.LogError("AssetManager", text);
        }
    }

    private static void LoadAsset(DownLoadTaskCollection data)
    {
        try
        {
            if (data.DownloadTaskState != DownloadTaskState.LoadAsset)
            {
                throw new Exception("Wrong task status");
            }
            for (int i = 0; i < data.DownLoadTaskList.Count; i++)
            {
                if (data.DownLoadTaskList[i].Bundle == null && null == data.DownLoadTaskList[i].Bundle.Assetbundle)
                {
                    throw new Exception("AssetBundle is null");
                }
                AssetBundleRequest value = null;
                if (!data.MapAssetBundleRequest.TryGetValue(data.DownLoadTaskList[i].AssetName, out value))
                {
                    value = data.DownLoadTaskList[i].Bundle.Assetbundle.LoadAssetAsync(data.DownLoadTaskList[i].AssetName, data.DownLoadTaskList[i].AssetType);
                    FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "ADD Load asset name:[" + data.DownLoadTaskList[i].AssetName + string.Empty);
                    data.MapAssetBundleRequest.Add(data.DownLoadTaskList[i].AssetName, value);
                    data.DownLoadTaskList[i].DownloadTaskState = DownloadTaskState.WaitCreateAssetBundleFinish;
                }
                else
                {
                    FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "DownLoadTaskCollection already containt AssetBundleRequest :[" + data.DownLoadTaskList[i].AssetName + string.Empty);
                }
            }
            data.DownloadTaskState = DownloadTaskState.WaitLoadAssetFinish;
        }
        catch (Exception ex)
        {
            string text = string.Format("LoadAsset({0}) error : {1}", data.ToString(), ex.Message);
            for (int j = 0; j < data.DownLoadTaskList.Count; j++)
            {
                data.DownLoadTaskList[j].Message = text;
                data.DownLoadTaskList[j].DownloadTaskState = DownloadTaskState.Error;
            }
            FFDebug.LogError("AssetManager", text);
        }
    }

    private static void WaitLoadAssetFinish(DownLoadTaskCollection data)
    {
        try
        {
            if (data.DownloadTaskState != DownloadTaskState.WaitLoadAssetFinish)
            {
                throw new Exception("Wrong task status");
            }
            data.MapAssetBundleRequest.BetterForeach(delegate (KeyValuePair<string, AssetBundleRequest> item)
            {
                if (item.Value == null)
                {
                    throw new Exception("assetBundleRequest is null");
                }
                if (item.Value.isDone)
                {
                    UnityEngine.Object asset = item.Value.asset;
                    if (null == asset)
                    {
                        throw new Exception("Asset is null");
                    }
                    for (int k = 0; k < data.DownLoadTaskList.Count; k++)
                    {
                        if (!data.DownLoadTaskList[k].Bundle._assets.ContainsKey(item.Key))
                        {
                            data.DownLoadTaskList[k].Bundle._assets.Add(item.Key, asset);
                        }
                        else
                        {
                            data.DownLoadTaskList[k].Bundle._assets[item.Key] = asset;
                        }
                        data.DownLoadTaskList[k].DownloadTaskState = DownloadTaskState.ReadyToUse;
                    }
                }
            });
            if (!data.MapAssetBundleRequest.CheckBetterForeach(new BetterDictionary<string, AssetBundleRequest>.CheckList(AssetManager.IsAssetLoadComplete)))
            {
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Load Asset not all complet,assetbundle path[" + data.AssetBundlePath + "]");
            }
            else
            {
                for (int i = 0; i < data.DownLoadTaskList.Count; i++)
                {
                    data.DownLoadTaskList[i].AssetBundleRequest = null;
                }
                FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new string[]
                {
                    "Load Asset all complet,assetbundle path[",
                    data.AssetBundlePath,
                    "],DownloadTaskState:[",
                    data.DownloadTaskState.ToString(),
                    "]"
                }));
                data.DownloadTaskState = DownloadTaskState.ReadyToUse;
            }
        }
        catch (Exception ex)
        {
            string text = string.Format("WaitLoadAssetFinish({0}) error : {1}", data.ToString(), ex.Message);
            for (int j = 0; j < data.DownLoadTaskList.Count; j++)
            {
                data.DownLoadTaskList[j].Message = text;
                data.DownLoadTaskList[j].DownloadTaskState = DownloadTaskState.Error;
            }
            data.DownloadTaskState = DownloadTaskState.Error;
            FFDebug.LogError("AssetManager", text);
        }
    }

    private static bool IsAssetLoadComplete(KeyValuePair<string, AssetBundleRequest> map)
    {
        FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new object[]
        {
            "asset state,assetbundle path[",
            map.Key,
            "],isdone:[",
            map.Value.isDone,
            "]"
        }));
        if (map.Value.isDone)
        {
            FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, map.Value.asset.name);
        }
        return map.Value.isDone;
    }

    private static void FinishAssetTask(DownLoadTaskCollection data)
    {
        try
        {
            if (data.DownloadTaskState != DownloadTaskState.Error && data.DownloadTaskState != DownloadTaskState.ReadyToUse)
            {
                throw new Exception("Wrong task status");
            }
            if (data.DownloadTaskState == DownloadTaskState.ReadyToUse)
            {
                for (int i = 0; i < data.DownLoadTaskList.Count; i++)
                {
                    if (data.DownLoadTaskList[i].AssetLoadType == AssetLoadType.LoadAsset)
                    {
                        Bundle bundle = AssetManager.GetBundle(data.AssetBundlePath);
                        if (bundle == null)
                        {
                            throw new Exception("Get Bundle is null");
                        }
                        FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, "Get asset name:[" + data.DownLoadTaskList[i].AssetName + "]");
                        UnityEngine.Object asset = bundle.GetAsset(data.DownLoadTaskList[i].AssetName);
                        if (null == asset)
                        {
                            throw new Exception("Get Asset is null");
                        }
                        if (data.DownLoadTaskList[i].OnAssetLoadComplete == null)
                        {
                            throw new Exception("Load asset complete OnAssetLoadComplete is null");
                        }
                        try
                        {
                            FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new object[]
                            {
                                "DownLoadTaskList:[",
                                data.DownLoadTaskList.Count,
                                "],index:[",
                                i,
                                "]"
                            }));
                            data.DownLoadTaskList[i].OnAssetLoadComplete(data.AssetBundlePath, data.DownLoadTaskList[i].AssetName, asset);
                        }
                        catch (Exception ex)
                        {
                            FFDebug.LogWarning(AssetManager.SelfName, "LoadAssetBundle callback error : " + ex.Message);
                        }
                    }
                    if (data.DownLoadTaskList[i].AssetLoadType == AssetLoadType.LoadAssetBundle)
                    {
                        if (AssetManager.GetBundle(data.AssetBundlePath) == null)
                        {
                            throw new Exception("Get Bundle is null");
                        }
                        if (data.DownLoadTaskList[i].OnAssetBundleLoadComplete == null)
                        {
                            throw new Exception("Load asset complete OnAssetLoadComplete is null");
                        }
                        try
                        {
                            FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new object[]
                            {
                                "DownLoadTaskList:[",
                                data.DownLoadTaskList.Count,
                                "],index:[",
                                i,
                                "]"
                            }));
                            AssetData assetData = data.DownLoadTaskList[i];
                            data.DownLoadTaskList[i].OnAssetBundleLoadComplete(data.AssetBundlePath, true);
                        }
                        catch (Exception ex2)
                        {
                            FFDebug.LogWarning(AssetManager.SelfName, "LoadAssetBundle complete,OnAssetBundleLoadComplete error : " + ex2.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex3)
        {
            string text = string.Format("FinishAssetTask({0}) error : {1}", data.ToString(), ex3.Message);
            for (int j = 0; j < data.DownLoadTaskList.Count; j++)
            {
                data.DownLoadTaskList[j].Message = text;
                data.DownLoadTaskList[j].DownloadTaskState = DownloadTaskState.Error;
            }
            data.DownloadTaskState = DownloadTaskState.Error;
            FFDebug.LogError(AssetManager.SelfName, text);
        }
        finally
        {
            if (data.DownloadTaskState == DownloadTaskState.Error)
            {
                for (int k = 0; k < data.DownLoadTaskList.Count; k++)
                {
                    FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, data.DownLoadTaskList[k].Message);
                    if (data.DownLoadTaskList[k].AssetLoadType == AssetLoadType.LoadAsset)
                    {
                        if (data.DownLoadTaskList[k].OnAssetLoadComplete == null)
                        {
                            throw new Exception("Load asset complete OnAssetLoadComplete is null");
                        }
                        try
                        {
                            data.DownLoadTaskList[k].OnAssetLoadComplete(data.AssetBundlePath, data.DownLoadTaskList[k].AssetName, null);
                        }
                        catch (Exception ex4)
                        {
                            FFDebug.LogWarning(AssetManager.SelfName, "LoadAssetBundle callback error : " + ex4.Message);
                        }
                    }
                    if (data.DownLoadTaskList[k].AssetLoadType == AssetLoadType.LoadAssetBundle)
                    {
                        if (data.DownLoadTaskList[k].OnAssetBundleLoadComplete == null)
                        {
                            throw new Exception("Load asset complete OnAssetLoadComplete is null");
                        }
                        try
                        {
                            data.DownLoadTaskList[k].OnAssetBundleLoadComplete(data.AssetBundlePath, false);
                        }
                        catch (Exception ex5)
                        {
                            FFDebug.LogWarning(AssetManager.SelfName, "LoadAssetBundle complete,OnAssetBundleLoadComplete error : " + ex5.Message);
                        }
                    }
                }
            }
            AssetManager._removeTmpList.Add(data);
            data = null;
        }
    }

    public static Bundle GetBundle(string assetBundleName)
    {
        Bundle bundle = null;
        if (!AssetManager._mapBundles.TryGetValue(assetBundleName, out bundle))
        {
            bundle = new Bundle(assetBundleName);
            AssetManager._mapBundles.Add(assetBundleName, bundle);
        }
        return bundle;
    }

    public static void UnloadAllAssets(string assetBundleName)
    {
        try
        {
            if (!AssetManager.ReadyToUse)
            {
                throw new Exception("AssetManager is not ready to use");
            }
            Bundle bundle = null;
            if (!AssetManager._mapBundles.TryGetValue(assetBundleName, out bundle))
            {
                throw new Exception("could not find assetbundle");
            }
            bundle.UnLoadAllAssets();
        }
        catch (Exception ex)
        {
            FFDebug.LogError("AssetManager", string.Format("UnloadAllAssets({0}) error : {1}", assetBundleName, ex.Message));
        }
    }

    public static void UnloadAsset(string assetBundleName, string assetName)
    {
        try
        {
            if (!AssetManager.ReadyToUse)
            {
                throw new Exception("AssetManager is not ready to use");
            }
            Bundle bundle = null;
            if (!AssetManager._mapBundles.TryGetValue(assetBundleName, out bundle))
            {
                throw new Exception("could not find assetbundle");
            }
            bundle.UnLoadAsset(assetName);
        }
        catch (Exception ex)
        {
            FFDebug.LogError("AssetManager", string.Format("UnloadAsset({0}, {1}) error : {2}", assetBundleName, assetName, ex.Message));
        }
    }

    public static void UnloadAssetBundle(string assetBundleName, bool unloadAllLoadedObjects)
    {
        try
        {
            if (!AssetManager.ReadyToUse)
            {
                throw new Exception("AssetManager is not ready to use");
            }
            Bundle bundle = null;
            if (!AssetManager._mapBundles.TryGetValue(assetBundleName, out bundle))
            {
                throw new Exception("could not find assetbundle");
            }
            bundle.UnLoadAssetBundle(unloadAllLoadedObjects);
        }
        catch (Exception ex)
        {
            FFDebug.LogError("AssetManager", string.Format("UnloadAssetBundle({0}, {1}) error : {2}", assetBundleName, unloadAllLoadedObjects.ToString(), ex.Message));
        }
    }

    public static void UnloadAssetBundle(bool unloadAllLoadedObjects, Bundle.BundleType Btype = Bundle.BundleType.Default)
    {
        try
        {
            if (!AssetManager.ReadyToUse)
            {
                throw new Exception("AssetManager is not ready to use");
            }
            FFDebug.Log(AssetManager.SelfName, FFLogType.ResourceLoad, string.Concat(new object[]
            {
                "Task count:[",
                AssetManager._mapDownLoadTaskCollection.Count,
                "]bundle count:[",
                AssetManager._mapBundles.Count,
                "]"
            }));
            Bundle bundle = null;
            AssetManager._mapDownLoadTaskCollection.BetterForeach(delegate (KeyValuePair<string, DownLoadTaskCollection> kvp)
            {
                bundle = AssetManager.GetBundle(kvp.Value.AssetBundlePath);
                if (bundle.BType == Btype)
                {
                    if (kvp.Value.DownloadTaskState == DownloadTaskState.WaitCreateAssetBundleFinish)
                    {
                        kvp.Value.GoDelete();
                    }
                    else
                    {
                        kvp.Value.Clear();
                        AssetManager._mapDownLoadTaskCollection.Remove(kvp.Key);
                    }
                }
            });
            AssetManager._mapBundles.BetterForeach(delegate (KeyValuePair<string, Bundle> pair)
            {
                bundle = pair.Value;
                if (bundle == null)
                {
                    throw new Exception("could not find assetbundle");
                }
                if (bundle.BType == Btype)
                {
                    bundle.UnLoadAssetBundle(unloadAllLoadedObjects);
                }
            });
        }
        catch (Exception ex)
        {
            FFDebug.LogError(AssetManager.SelfName, string.Format("UnloadAssetBundle({0}) error : {1}", unloadAllLoadedObjects.ToString(), ex.Message));
        }
    }

    public static void AsynUnloadUnusedAssets()
    {
        AssetManager._mono.StartCoroutine(AssetManager.UnloadUnusedAssets());
    }

    private static IEnumerator UnloadUnusedAssets()
    {
        AsyncOperation iteratorVariable0 = Resources.UnloadUnusedAssets();
        yield return iteratorVariable0;
        yield break;
    }

    private static bool IsFileExist(string assetBundlePath)
    {
        string path = ResourceHelper.GetPath(assetBundlePath);
        if (string.IsNullOrEmpty(path))
        {
            FFDebug.LogWarning(AssetManager.SelfName, "File not Exist,path:[" + assetBundlePath + "]");
            return false;
        }
        return true;
    }

    private static MonoBehaviour _mono;

    public static bool ReadyToUse = false;

    public static Queue<AssetData> _AssetTasks = new Queue<AssetData>();

    private static int _MaxLoadingCount = 0;

    public static BetterDictionary<string, Bundle> _mapBundles = new BetterDictionary<string, Bundle>();

    public static BetterDictionary<string, DownLoadTaskCollection> _mapDownLoadTaskCollection = new BetterDictionary<string, DownLoadTaskCollection>();

    private static List<DownLoadTaskCollection> _lstLoadingTaskCollection = new List<DownLoadTaskCollection>();

    private static List<DownLoadTaskCollection> _removeTmpList = new List<DownLoadTaskCollection>();
}
