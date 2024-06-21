using System;
using UnityEngine;

public class AssetInAB<T> where T : UnityEngine.Object
{
    public AssetInAB(FFAssetBundle ab)
    {
        this.RefCount = 0;
        this.AB = ab;
        if (this.AB != null)
        {
            this.mAsset = this.AB.GetMainAsset<T>();
        }
        else
        {
            FFDebug.LogWarning(this, "GameObjectInAB Error : FFAssetBundle null");
        }
        if (this.mAsset == null)
        {
            FFDebug.LogWarning(this, "GameObjectInAB Error : Gobj null key: " + this.AB.Path);
        }
    }

    public void Unload()
    {
        this.AB.Unload();
    }

    public int RefCount;

    public T mAsset;

    private FFAssetBundle AB;
}
