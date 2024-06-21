using System;
using System.Collections.Generic;

namespace Game.Scene
{
    public class SceneDataAssetBundle
    {
        public SceneDataAssetBundle(string strBundleName)
        {
            this.strAssetBundleName = strBundleName;
            this.lstReqCallback.Clear();
            this.lstRefName.Clear();
            this.currentState = AssetState.NonLoad;
        }

        public void AddUser(string strUser)
        {
            if (!this.lstRefName.Contains(strUser))
            {
                this.lstRefName.Add(strUser);
            }
        }

        public void RemoveUser(string strUser)
        {
            if (this.lstRefName.Contains(strUser))
            {
                this.lstRefName.Remove(strUser);
            }
            else
            {
                FFDebug.Log(this, FFLogType.Scene, this.strAssetBundleName + " RemoveUser " + strUser + " Failed!");
            }
        }

        public static SceneDataAssetBundle GetSceneDataAssetBundle(string strBundleName)
        {
            if (!SceneData.bdAssetBundleMap.ContainsKey(strBundleName))
            {
                SceneData.bdAssetBundleMap[strBundleName] = new SceneDataAssetBundle(strBundleName);
            }
            return SceneData.bdAssetBundleMap[strBundleName];
        }

        public void LoadThis(Action<SceneDataAssetBundle> callback)
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
                        AssetManager.LoadAssetBundle(this.strAssetBundleName, delegate (string assetBundleName, bool succeed)
                        {
                            if (succeed)
                            {
                                Bundle bundle = AssetManager.GetBundle(assetBundleName);
                                if (bundle != null)
                                {
                                    this.bundle = bundle;
                                    for (int j = 0; j < this.lstReqCallback.Count; j++)
                                    {
                                        this.lstReqCallback[j](this);
                                    }
                                    this.lstReqCallback.Clear();
                                    this.currentState = AssetState.Loaded;
                                    return;
                                }
                            }
                            else
                            {
                                FFDebug.LogWarning(this, assetBundleName + "Load AssetBundle failed!");
                            }
                            for (int k = 0; k < this.lstReqCallback.Count; k++)
                            {
                                this.lstReqCallback[k](null);
                            }
                            this.lstReqCallback.Clear();
                            this.currentState = AssetState.NonLoad;
                        }, Bundle.BundleType.Scene);
                    }
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "Load AssetBundle ",
                    this.strAssetBundleName,
                    " Error: ",
                    ex
                }));
            }
        }

        public void UnLoadThis()
        {
            try
            {
                this.bdSceneDataAssetMap.BetterForeach(delegate (KeyValuePair<string, SceneDataAssetObject> item)
                {
                    if (item.Value != null)
                    {
                        item.Value.UnLoadThis();
                    }
                });
                this.bdSceneDataAssetMap.Clear();
                this.lstRefName.Clear();
                this.strDependencies = null;
                for (int i = 0; i < this.lstReqCallback.Count; i++)
                {
                    this.lstReqCallback[i](null);
                }
                this.lstReqCallback.Clear();
                this.currentState = AssetState.UnLoaded;
                SceneData.bdAssetBundleMap.Remove(this.strAssetBundleName);
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "Unload AssetBundle ",
                    this.strAssetBundleName,
                    " Error : ",
                    ex
                }));
            }
        }

        public void LoadAssetByName(string strAssetName, Action<SceneDataAssetObject> callback)
        {
            if (this.bdSceneDataAssetMap.ContainsKey(strAssetName))
            {
                this.bdSceneDataAssetMap[strAssetName].LoadThis(callback);
            }
            else
            {
                SceneDataAssetObject sceneDataAssetObject = new SceneDataAssetObject(this, strAssetName);
                this.bdSceneDataAssetMap[strAssetName] = sceneDataAssetObject;
                sceneDataAssetObject.LoadThis(callback);
            }
        }

        public Bundle bundle;

        public List<Action<SceneDataAssetBundle>> lstReqCallback = new List<Action<SceneDataAssetBundle>>();

        public string strAssetBundleName;

        public AssetState currentState;

        public List<string> lstRefName = new List<string>();

        public string[] strDependencies;

        private BetterDictionary<string, SceneDataAssetObject> bdSceneDataAssetMap = new BetterDictionary<string, SceneDataAssetObject>();
    }
}
