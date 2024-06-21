using Framework.Managers;
using ResoureManager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAssetObj
{
    private List<Action<UIAssetObj>> loadcallbacks = new List<Action<UIAssetObj>>();
    public UIAssetBundle uiBundle;
    public string ObjName;
    public UnityEngine.Object Obj;
    public AssetState CurrentState;

    public UIAssetObj(UIAssetBundle bundle, string objname)
    {
        this.uiBundle = bundle;
        this.ObjName = objname;
        this.Obj = (UnityEngine.Object)null;
        this.loadcallbacks.Clear();
        this.CurrentState = AssetState.NonLoad;
    }

    public void LoadThis(Action<UIAssetObj> callback)
    {
        this.loadcallbacks.Add(callback);
        if (this.CurrentState == AssetState.Loaded)
        {
            for (int index = 0; index < this.loadcallbacks.Count; ++index)
                this.loadcallbacks[index](this);
            this.loadcallbacks.Clear();
        }
        else
        {
            if (this.CurrentState == AssetState.Loading || this.CurrentState != AssetState.NonLoad)
                return;
            this.CurrentState = AssetState.Loading;
            ManagerCenter.Instance.GetManager<ResourceManager>().LoadAssetFromAssetbundle(this.uiBundle.assetBundle, this.ObjName, (Action<UnityEngine.Object>)(obj =>
            {
                if (obj == (UnityEngine.Object)null)
                {
                    for (int index = 0; index < this.loadcallbacks.Count; ++index)
                        this.loadcallbacks[index]((UIAssetObj)null);
                    this.loadcallbacks.Clear();
                }
                else
                {
                    this.Obj = obj;
                    for (int index = 0; index < this.loadcallbacks.Count; ++index)
                        this.loadcallbacks[index](this);
                    this.loadcallbacks.Clear();
                    this.CurrentState = AssetState.Loaded;
                }
            }));
        }
    }

    public void UnLoadThis(bool unloadab = true)
    {
        if (this.Obj is GameObject)
            UnityEngine.Object.DestroyImmediate(this.Obj, true);
        else
            Resources.UnloadAsset(this.Obj);
        this.Obj = (UnityEngine.Object)null;
        if (unloadab)
            this.uiBundle.UnLoadThis();
        this.loadcallbacks.Clear();
        this.CurrentState = AssetState.UnLoaded;
    }
}
