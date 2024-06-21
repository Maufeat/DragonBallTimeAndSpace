using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetBundle
{
    public List<Action<UIAssetBundle>> ReqCallbacks = new List<Action<UIAssetBundle>>();
    private List<string> RefList = new List<string>();
    private BetterDictionary<string, UIAssetObj> allAssetMap = new BetterDictionary<string, UIAssetObj>();
    private Bundle bundle;
    public AssetBundle assetBundle;
    public string PkgName;
    public string AssetBundleName;
    public AssetState CurrentState;
    public string[] Dependencies;
    private int normalUseCount;

    public UIAssetBundle(string pkgname, string bundlename)
    {
        this.PkgName = pkgname;
        this.AssetBundleName = bundlename;
        this.assetBundle = (AssetBundle)null;
        this.ReqCallbacks.Clear();
        this.RefList.Clear();
        this.CurrentState = AssetState.NonLoad;
    }

    public string Path
    {
        get
        {
            return this.PkgName + "/" + this.AssetBundleName;
        }
    }

    public int DependencyUseCount
    {
        get
        {
            return this.RefList.Count;
        }
    }

    public static UIAssetBundle GetUIAssetBundle(string pkgname, string bundlename)
    {
        if (!UILoader.UIAssetBundles.ContainsKey(pkgname + "/" + bundlename))
            UILoader.UIAssetBundles[pkgname + "/" + bundlename] = new UIAssetBundle(pkgname, bundlename);
        return UILoader.UIAssetBundles[pkgname + "/" + bundlename];
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

    public void LoadThis(Action<UIAssetBundle> callback)
    {
        this.ReqCallbacks.Add(callback);
        if (this.CurrentState == AssetState.Loaded)
        {
            for (int index = 0; index < this.ReqCallbacks.Count; ++index)
                this.ReqCallbacks[index](this);
            this.ReqCallbacks.Clear();
        }
        else
        {
            if (this.CurrentState == AssetState.Loading || this.CurrentState != AssetState.NonLoad)
                return;
            this.CurrentState = AssetState.Loading;
            AssetManager.LoadAssetBundle(this.Path, (OnAssetBundleLoadComplete)((assetBundleName, succeed) =>
            {
                if (succeed)
                {
                    Bundle bundle = AssetManager.GetBundle(assetBundleName);
                    if (bundle != null)
                    {
                        this.assetBundle = bundle.Assetbundle;
                        this.bundle = bundle;
                        for (int index = 0; index < this.ReqCallbacks.Count; ++index)
                            this.ReqCallbacks[index](this);
                        this.ReqCallbacks.Clear();
                        this.CurrentState = AssetState.Loaded;
                        return;
                    }
                }
                for (int index = 0; index < this.ReqCallbacks.Count; ++index)
                    this.ReqCallbacks[index]((UIAssetBundle)null);
                this.ReqCallbacks.Clear();
                this.CurrentState = AssetState.NonLoad;
            }), Bundle.BundleType.UI);
        }
    }

    public T[] LoadAllAsset<T>() where T : UnityEngine.Object
    {
        return (UnityEngine.Object)this.assetBundle == (UnityEngine.Object)null ? (T[])null : this.assetBundle.LoadAllAssets<T>();
    }

    public void LoadAssetByName(string name, Action<UIAssetObj> callback)
    {
        if (this.allAssetMap.ContainsKey(name))
        {
            this.allAssetMap[name].LoadThis(callback);
        }
        else
        {
            UIAssetObj uiAssetObj = new UIAssetObj(this, name);
            this.allAssetMap[name] = uiAssetObj;
            uiAssetObj.LoadThis(callback);
        }
    }

    public void UnLoadThis()
    {
        try
        {
            this.allAssetMap.BetterForeach((Action<KeyValuePair<string, UIAssetObj>>)(item =>
            {
                if (item.Value == null)
                    return;
                item.Value.UnLoadThis(false);
            }));
            this.allAssetMap.Clear();
            if (this.Dependencies != null)
            {
                for (int index = 0; index < this.Dependencies.Length; ++index)
                {
                    UIAssetBundle uiAssetBundle = UIAssetBundle.GetUIAssetBundle(this.PkgName, this.Dependencies[index]);
                    if (uiAssetBundle != null)
                    {
                        uiAssetBundle.DeductUse(this.Path);
                        if (uiAssetBundle.DependencyUseCount == 0)
                            uiAssetBundle.UnLoadThis();
                    }
                }
            }
            for (int index = 0; index < this.ReqCallbacks.Count; ++index)
                this.ReqCallbacks[index]((UIAssetBundle)null);
            this.ReqCallbacks.Clear();
            this.CurrentState = AssetState.UnLoaded;
            if (this.bundle != null)
                this.bundle.Status = Bundle.BundleStatus.Need_To_Load;
            UILoader.UIAssetBundles.Remove(this.Path);
            if (!((UnityEngine.Object)this.assetBundle != (UnityEngine.Object)null))
                return;
            this.assetBundle.Unload(true);
            this.assetBundle = (AssetBundle)null;
        }
        catch (Exception ex)
        {
            FFDebug.LogError((object)"UILoader", (object)("Unload assetBundle " + this.AssetBundleName + " error : " + (object)ex));
        }
    }
}
