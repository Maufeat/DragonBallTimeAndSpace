using System;
using System.Collections.Generic;
using UnityEngine;

public class FFAssetBundle
{
    private static BetterDictionary<string, FFAssetBundle> AllFFAssetBundle = new BetterDictionary<string, FFAssetBundle>();
    private List<string> RefList = new List<string>();
    public bool NeedLoadAsset = true;
    private int unloadDaley = -1;
    public string PkgName;
    public string AssetBundleName;
    public UnityEngine.Object[] allAssets;
    public Bundle mBundle;
    public AssetBundle assetBundle;
    public AssetBundleRequest AssetBundleReq;
    public string[] Dependencies;

    private FFAssetBundle(string pkgName, string assetBundleName)
    {
        this.PkgName = pkgName;
        this.AssetBundleName = assetBundleName;
    }

    public static FFAssetBundle GetFFAssetBundle(string pkgName, string assetBundleName)
    {
        string key = pkgName + "/" + assetBundleName + ".u";
        if (!FFAssetBundle.AllFFAssetBundle.ContainsKey(key))
            FFAssetBundle.AllFFAssetBundle[key] = new FFAssetBundle(pkgName, assetBundleName);
        return FFAssetBundle.AllFFAssetBundle[key];
    }

    public static FFAssetBundle[] GetFFAssetBundleArray(
      string pkgName,
      string[] assetBundleNames)
    {
        FFAssetBundle[] ffAssetBundleArray = new FFAssetBundle[assetBundleNames.Length];
        for (int index = 0; index < assetBundleNames.Length; ++index)
            ffAssetBundleArray[index] = FFAssetBundle.GetFFAssetBundle(pkgName, assetBundleNames[index]);
        return ffAssetBundleArray;
    }

    public static void UpdateALL()
    {
        FFAssetBundle.AllFFAssetBundle.BetterForeach((Action<KeyValuePair<string, FFAssetBundle>>)(item => item.Value.Update()));
    }

    public static void UnLoadALL()
    {
        using (Dictionary<string, FFAssetBundle>.Enumerator enumerator = FFAssetBundle.AllFFAssetBundle.GetEnumerator())
        {
            while (enumerator.MoveNext())
                enumerator.Current.Value.Unload();
        }
        FFAssetBundle.AllFFAssetBundle.Clear();
    }

    public string Path
    {
        get
        {
            return this.PkgName + "/" + this.AssetBundleName + ".u";
        }
    }

    public T GetMainAsset<T>() where T : UnityEngine.Object
    {
        if (this.allAssets != null)
        {
            if (this.allAssets.Length > 0)
            {
                if (this.allAssets[0] is T)
                    return this.allAssets[0] as T;
                FFDebug.LogWarning((object)this, (object)("Get " + this.Path + " MainAsset error: Try :" + (object)this.allAssets[0].GetType()));
            }
            else
                FFDebug.LogWarning((object)this, (object)("allAssets " + this.Path + " Length 0"));
        }
        else
            FFDebug.LogWarning((object)this, (object)("Get " + this.Path + " MainAsset error:  allAssets null " + this.PkgName + " " + this.AssetBundleName));
        return (T)null;
    }

    public T[] GetAllAsset<T>() where T : UnityEngine.Object
    {
        if (this.allAssets != null)
        {
            List<T> objList = new List<T>();
            for (int index = 0; index < this.allAssets.Length; ++index)
            {
                if (this.allAssets[index] is T)
                    objList.Add(this.allAssets[index] as T);
            }
            return objList.ToArray();
        }
        FFDebug.LogWarning((object)this, (object)("Get " + this.Path + " GetAllAsset error:  allAssets null"));
        return (T[])null;
    }

    public T GetAssetByName<T>(string name) where T : UnityEngine.Object
    {
        if (this.allAssets != null)
        {
            for (int index = 0; index < this.allAssets.Length; ++index)
            {
                if (this.allAssets[index] is T && this.allAssets[index].name == name)
                    return this.allAssets[index] as T;
            }
        }
        else
            FFDebug.LogWarning((object)this, (object)("Get " + this.Path + " GetAssetByName error:  allAssets null"));
        return (T)null;
    }

    public bool IsLive
    {
        get
        {
            return this.allAssets != null;
        }
    }

    public void Unload()
    {
        if (this.AssetBundleReq != null && !this.AssetBundleReq.isDone)
        {
            FFDebug.LogWarning((object)this, (object)("Unload assetBundle " + this.AssetBundleName + " error : Try Unload when is loading"));
        }
        else
        {
            if (!this.IsLive)
                return;
            try
            {
                if (this.allAssets != null)
                {
                    for (int index = 0; index < this.allAssets.Length; ++index)
                    {
                        UnityEngine.Object allAsset = this.allAssets[index];
                        if (!(allAsset == (UnityEngine.Object)null))
                        {
                            if (allAsset is GameObject)
                                UnityEngine.Object.DestroyImmediate(allAsset, true);
                            else
                                Resources.UnloadAsset(allAsset);
                        }
                    }
                }
                this.allAssets = (UnityEngine.Object[])null;
                if (this.Dependencies != null)
                {
                    foreach (FFAssetBundle ffAssetBundle in FFAssetBundle.GetFFAssetBundleArray(this.PkgName, this.Dependencies))
                    {
                        if (ffAssetBundle != null)
                        {
                            ffAssetBundle.DeductUse(this.Path);
                            ffAssetBundle.UnloadIfNoUse();
                        }
                    }
                }
                if (this.mBundle != null)
                    this.mBundle.Status = Bundle.BundleStatus.Need_To_Load;
                if ((UnityEngine.Object)this.assetBundle != (UnityEngine.Object)null)
                    this.assetBundle.Unload(true);
                this.assetBundle = (AssetBundle)null;
            }
            catch (Exception ex)
            {
                FFDebug.LogError((object)this, (object)("Unload assetBundle " + this.AssetBundleName + " error : " + (object)ex));
            }
        }
    }

    private void Update()
    {
        if (this.unloadDaley < 0)
            return;
        --this.unloadDaley;
        if (this.unloadDaley != 0)
            return;
        if (this.UseCount == 0)
            this.Unload();
        this.unloadDaley = -1;
    }

    public int UseCount
    {
        get
        {
            return this.RefList.Count;
        }
    }

    public void AddUse(string user)
    {
        if (this.RefList.Contains(user))
            return;
        this.RefList.Add(user);
    }

    public void DeductUse(string user)
    {
        if (!this.RefList.Contains(user))
            return;
        this.RefList.Remove(user);
    }

    public void UnloadIfNoUse()
    {
        if (this.UseCount != 0)
            return;
        this.unloadDaley = 90;
    }
}
