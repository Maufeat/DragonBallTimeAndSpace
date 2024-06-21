using System;
using System.Collections.Generic;
using UnityEngine;

public class Bundle
{
    public BetterDictionary<string, UnityEngine.Object> _assets = new BetterDictionary<string, UnityEngine.Object>();
    private string _filename = string.Empty;
    private string _message = string.Empty;
    private AssetBundle _assetbundle;
    private Bundle.BundleStatus _status;
    private Bundle.BundleType _bType;

    public Bundle(string filename)
    {
        this._filename = filename;
        this._status = Bundle.BundleStatus.Need_To_Load;
    }

    public AssetBundle Assetbundle
    {
        get
        {
            return this._assetbundle;
        }
        set
        {
            this._assetbundle = value;
        }
    }

    public string FileName
    {
        get
        {
            return this._filename;
        }
    }

    public string Message
    {
        get
        {
            return this._message;
        }
        set
        {
            this._message = value;
        }
    }

    public Bundle.BundleStatus Status
    {
        get
        {
            return this._status;
        }
        set
        {
            this._status = value;
        }
    }

    public Bundle.BundleType BType
    {
        set
        {
            this._bType = value;
        }
        get
        {
            return this._bType;
        }
    }

    public UnityEngine.Object GetAsset(string assetName)
    {
        UnityEngine.Object @object = (UnityEngine.Object)null;
        return this._assets.TryGetValue(assetName, out @object) ? @object : (UnityEngine.Object)null;
    }

    public void UnLoadAllAssets()
    {
        this._assets.BetterForeach((Action<KeyValuePair<string, UnityEngine.Object>>)(item => UnityEngine.Object.DestroyImmediate(item.Value, true)));
        this._assets.Clear();
    }

    public void UnLoadAsset(string assetName)
    {
        UnityEngine.Object assetToUnload = (UnityEngine.Object)null;
        if (!this._assets.TryGetValue(assetName, out assetToUnload))
            throw new Exception("Could not find asset");
        if ((UnityEngine.Object)null != assetToUnload)
            Resources.UnloadAsset(assetToUnload);
        this._assets.Remove(assetName);
    }

    public void UnLoadAssetBundle(bool unLoadAllLoadedAssetObjects)
    {
        if (this._assets != null)
        {
            if (unLoadAllLoadedAssetObjects)
                this.UnLoadAllAssets();
            if ((UnityEngine.Object)null != (UnityEngine.Object)this._assetbundle)
            {
                this._assetbundle.Unload(unLoadAllLoadedAssetObjects);
                this._assetbundle = (AssetBundle)null;
            }
        }
        this._status = Bundle.BundleStatus.Need_To_Load;
    }

    public enum BundleStatus
    {
        Need_To_Load,
        Ready_To_Use,
        Error,
    }

    public enum BundleType
    {
        Default,
        Scene,
        EffectAndcharacter,
        UI,
    }
}
