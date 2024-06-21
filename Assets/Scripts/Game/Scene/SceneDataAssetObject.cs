using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scene
{
    public class SceneDataAssetObject
    {
        public SceneDataAssetObject(SceneDataAssetBundle bundle, string objName)
        {
            this.sdAssetBundle = bundle;
            this.strObjName = objName;
            this.objAssetItem = null;
            this.lstReqCallback.Clear();
            this.currentState = AssetState.NonLoad;
        }

        public void LoadThis(Action<SceneDataAssetObject> callback)
        {
            try
            {
                this.lstReqCallback.Add(callback);
                if (this.currentState == AssetState.Loaded)
                {
                    for (int i = 0; i < this.lstReqCallback.Count; i++)
                    {
                        this.lstReqCallback[i](this);
                    }
                    this.lstReqCallback.Clear();
                }
                else if (this.currentState != AssetState.Loading)
                {
                    if (this.currentState == AssetState.NonLoad)
                    {
                        this.currentState = AssetState.Loading;
                        AssetManager.LoadAsset(this.sdAssetBundle.strAssetBundleName, this.strObjName, typeof(UnityEngine.Object), delegate (string assetBundleName, string assetName, UnityEngine.Object objItem)
                        {
                            if (null == objItem)
                            {
                                for (int j = 0; j < this.lstReqCallback.Count; j++)
                                {
                                    this.lstReqCallback[j](null);
                                }
                                this.lstReqCallback.Clear();
                                return;
                            }
                            this.objAssetItem = objItem;
                            for (int k = 0; k < this.lstReqCallback.Count; k++)
                            {
                                this.lstReqCallback[k](this);
                            }
                            this.lstReqCallback.Clear();
                            this.currentState = AssetState.Loaded;
                        }, Bundle.BundleType.Default);
                    }
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "Load Asset ",
                    this.strObjName,
                    " Error: ",
                    ex
                }));
            }
        }

        public void UnLoadThis()
        {
            try
            {
                if (this.objAssetItem is GameObject)
                {
                    UnityEngine.Object.DestroyImmediate(this.objAssetItem, true);
                }
                else
                {
                    Resources.UnloadAsset(this.objAssetItem);
                }
                this.objAssetItem = null;
                for (int i = 0; i < this.lstReqCallback.Count; i++)
                {
                    this.lstReqCallback[i](null);
                }
                this.lstReqCallback.Clear();
                this.currentState = AssetState.UnLoaded;
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "Unload Asset ",
                    this.strObjName,
                    " Error: ",
                    ex
                }));
            }
        }

        public SceneDataAssetBundle sdAssetBundle;

        public string strObjName;

        public UnityEngine.Object objAssetItem;

        public List<Action<SceneDataAssetObject>> lstReqCallback = new List<Action<SceneDataAssetObject>>();

        public AssetState currentState;
    }
}
