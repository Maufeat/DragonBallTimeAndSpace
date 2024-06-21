using System;
using System.Collections.Generic;
using UnityEngine;

public class UILoader
{
    public static void LoadAssetBundle(string pkgname, string bundlename, Action<UIAssetBundle> callback)
    {
        try
        {
            if (string.IsNullOrEmpty(bundlename))
            {
                FFDebug.LogWarning("UILoader", "LoadAssetBundle Error bundlename is nullorempty");
                callback(null);
            }
            else if (UILoader.UIAssetBundles.ContainsKey(pkgname + "/" + bundlename))
            {
                UILoader.UIAssetBundles[pkgname + "/" + bundlename].LoadThis(callback);
            }
            else
            {
                UIAssetBundle uiassetBundle = new UIAssetBundle(pkgname, bundlename);
                UILoader.UIAssetBundles[pkgname + "/" + bundlename] = uiassetBundle;
                uiassetBundle.LoadThis(delegate (UIAssetBundle bundle)
                {
                    UILoader.UIAssetBundles[pkgname + "/" + bundlename] = bundle;
                    callback(bundle);
                });
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogWarning("UILoader", string.Concat(new string[]
            {
                "LoadAssetBundle Exception ",
                pkgname,
                "  ",
                bundlename,
                "  ",
                ex.ToString()
            }));
            callback(null);
        }
    }

    public static void UnLoadAssetbundle(string pkgname, string bundlename)
    {
        if (UILoader.UIAssetBundles.ContainsKey(pkgname + "/" + bundlename))
        {
            UILoader.UIAssetBundles[pkgname + "/" + bundlename].UnLoadThis();
            UILoader.UIAssetBundles.Remove(pkgname + "/" + bundlename);
        }
    }

    public static void LoadObject(string pkgname, string bundlename, string objectname, Action<UnityEngine.Object> callback)
    {
        if (string.IsNullOrEmpty(bundlename) || string.IsNullOrEmpty(objectname))
        {
            FFDebug.LogWarning("UILoader", "LoadObject Error bundlename or objectname is nullorempty");
            callback(null);
            return;
        }
        UILoader.LoadAssetBundle(pkgname, bundlename, delegate (UIAssetBundle bundle)
        {
            if (bundle == null)
            {
                FFDebug.LogWarning("UILoader", "Error when load " + pkgname + "/" + bundlename);
                callback(null);
                return;
            }
            bundle.LoadAssetByName(objectname, delegate (UIAssetObj obj)
            {
                if (obj == null)
                {
                    callback(null);
                    return;
                }
                callback(obj.Obj);
            });
        });
    }

    public static void LoadObject(string pkgname, string bundlename, string objectname, Action<UIAssetObj> callback)
    {
        if (string.IsNullOrEmpty(bundlename) || string.IsNullOrEmpty(objectname))
        {
            FFDebug.Log("UILoader", FFLogType.UI, "LoadObject Error bundlename or objectname is nullorempty");
            callback(null);
            return;
        }
        UILoader.LoadAssetBundle(pkgname, bundlename, delegate (UIAssetBundle bundle)
        {
            if (bundle == null)
            {
                FFDebug.Log("UILoader", FFLogType.UI, "Error when load " + pkgname + "/" + bundlename);
                callback(null);
                return;
            }
            bundle.LoadAssetByName(objectname, delegate (UIAssetObj obj)
            {
                if (obj == null)
                {
                    callback(null);
                    return;
                }
                callback(obj);
            });
        });
    }

    public static void StartLoadDependcy(string pkgname, string bundlename, Action<UIAssetBundle> callback)
    {
        if (string.IsNullOrEmpty(bundlename))
        {
            FFDebug.LogWarning("UILoader", "StartLoadDependcy Error bundlename is nullorempty");
            callback(null);
            return;
        }
        if (UILoader.AssetBundleManifests.ContainsKey(pkgname))
        {
            UILoader.LoadDependcy(pkgname, bundlename, callback);
        }
        else
        {
            UILoader.LoadObject(pkgname, pkgname, "AssetBundleManifest", delegate (UnityEngine.Object o)
            {
                AssetBundleManifest assetBundleManifest = o as AssetBundleManifest;
                if (assetBundleManifest == null)
                {
                    FFDebug.LogWarning("UILoader", "Load AssetBundleManifest " + pkgname + "Error!");
                    callback(null);
                }
                UILoader.AssetBundleManifests[pkgname] = assetBundleManifest;
                UILoader.LoadDependcy(pkgname, bundlename, callback);
            });
        }
    }

    public static void LoadDependcy(string pkgname, string bundlename, Action<UIAssetBundle> callback)
    {
        if (bundlename == "shader.u")
        {
            callback(null);
            return;
        }
        string pkgName = pkgname;
        string assetBundleName = bundlename;
        string path = pkgName + "/" + assetBundleName;
        UIAssetBundle uiab = UIAssetBundle.GetUIAssetBundle(pkgName, assetBundleName);
        if (!UILoader.AssetBundleManifests.ContainsKey(pkgName))
        {
            FFDebug.LogWarning("UILoader", "AssetBundleManifests has no key :" + pkgName);
            callback(null);
            return;
        }
        uiab.Dependencies = UILoader.AssetBundleManifests[pkgName].GetAllDependencies(assetBundleName);
        if (uiab.Dependencies.Length == 0 || assetBundleName.Contains("msyh"))
        {
            UILoader.LoadAssetBundle(pkgName, assetBundleName, callback);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < uiab.Dependencies.Length; i++)
            {
                UILoader.LoadDependcy(pkgName, uiab.Dependencies[i], delegate (UIAssetBundle bundle)
                {
                    if (bundle != null)
                    {
                        bundle.AddUse(path);
                    }
                    index++;
                    if (index == uiab.Dependencies.Length)
                    {
                        UILoader.LoadAssetBundle(pkgName, assetBundleName, callback);
                    }
                });
            }
        }
    }

    public static BetterDictionary<string, UIAssetBundle> UIAssetBundles = new BetterDictionary<string, UIAssetBundle>();

    public static Dictionary<string, AssetBundleManifest> AssetBundleManifests = new Dictionary<string, AssetBundleManifest>();
}
