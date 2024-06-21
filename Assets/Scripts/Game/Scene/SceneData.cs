using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scene
{
    public class SceneData
    {
        public MapZoneData mapZoneData
        {
            get
            {
                return this._mapZoneData;
            }
        }

        public TerrainRegionInfo triAll
        {
            get
            {
                return this._triAll;
            }
        }

        public LightmapInfo lightmapInfo
        {
            get
            {
                return this._lightmapInfo;
            }
        }

        public CameraZoneInfo cameraZoneInfo
        {
            get
            {
                return this._czInfo;
            }
        }

        public BetterDictionary<string, TerrainRegionInfo> nineBlockInfo
        {
            get
            {
                return this._bdNineBlock;
            }
        }

        public bool useNineBlock
        {
            get
            {
                return this._bUseNineBlock;
            }
        }

        public bool useCustomScene
        {
            get
            {
                return this._bUseCustomScene;
            }
            set
            {
                this._bUseCustomScene = value;
            }
        }

        public void SetSceneLighting(string strSceneName, Action callback)
        {
            this.LoadLightmapInfo(strSceneName, delegate
            {
                this.SetRenderSettings();
                this.SetSceneLightmaps();
                if (callback != null)
                {
                    callback();
                }
            });
        }

        public void LoadMapZoneData(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_zonedata.u"
            });
            AssetManager.LoadAsset(assetBundlePath, "MapZoneData", typeof(MapZoneData), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                if (null != obj)
                {
                    this._mapZoneData = (obj as MapZoneData);
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    FFDebug.Log(this, FFLogType.Scene, "MapZoneData.asset is null");
                    this._mapZoneData = null;
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        public void LoadMapCameraZoneInfo(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_camerazoneinfo.u"
            });
            AssetManager.LoadAsset(assetBundlePath, "CameraZoneInfo", typeof(CameraZoneInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                if (null != obj)
                {
                    this._czInfo = (obj as CameraZoneInfo);
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    FFDebug.Log(this, FFLogType.Scene, "CameraZoneInfo.asset is null");
                    this._mapZoneData = null;
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        public void LoadSceneAllInfo(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_allinfo.u"
            });
            AssetManager.LoadAsset(assetBundlePath, strSceneName + "_AllInfo.asset", typeof(TerrainRegionInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object objInfo)
            {
                if (null == objInfo)
                {
                    this._triAll = null;
                    FFDebug.LogWarning(this, "AllInfo.asset is null");
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    this._triAll = (objInfo as TerrainRegionInfo);
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        public void LoadSceneGameObjectDependency(string strSceneName, Action callback)
        {
            this.LoadSceneTexture(strSceneName, delegate
            {
                this.LoadSceneMaterial(strSceneName, delegate
                {
                    if (this._bUseCustomScene)
                    {
                        if (callback != null)
                        {
                            callback();
                        }
                    }
                    else
                    {
                        this.LoadSceneModel(strSceneName, delegate
                        {
                            if (callback != null)
                            {
                                callback();
                            }
                        });
                    }
                });
            });
        }

        public void LoadManifest(string strSceneName, Action callback)
        {
            this._strScenePath = "Scenes/" + strSceneName + "/";
            string assetBundlePath = this._strScenePath + strSceneName;
            AssetManager.LoadAsset(assetBundlePath, "AssetBundleManifest", typeof(AssetBundleManifest), delegate (string assetBundleName, string assetName, UnityEngine.Object objManifest)
            {
                if (null == objManifest)
                {
                    this._manifest = null;
                    FFDebug.Log(this, FFLogType.Scene, "AssetBundleManifest is null");
                }
                else
                {
                    this._manifest = (objManifest as AssetBundleManifest);
                }
                if (callback != null)
                {
                    callback();
                }
            }, Bundle.BundleType.Default);
        }

        public void LoadSceneNineBlockInfo(string strSceneName, Action callback)
        {
            string strAssetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_nb.u"
            });
            AssetManager.LoadAssetBundle(strAssetBundlePath, delegate (string strAssetBundleName, bool bSuccess)
            {
                if (!bSuccess)
                {
                    this._bdNineBlock = null;
                    this._bUseNineBlock = false;
                    FFDebug.Log(this, FFLogType.Scene, "AssetBundle NineBlock is null!");
                    if (callback != null)
                    {
                        callback();
                    }
                    return;
                }
                if (null != this._triAll)
                {
                    int nLoadCompletedCount = 0;
                    int nTotalCount = this._triAll._nIndexX * this._triAll._nIndexY;
                    if (nTotalCount == 0)
                    {
                        this._bdNineBlock = null;
                        this._bUseNineBlock = false;
                        if (callback != null)
                        {
                            callback();
                        }
                        return;
                    }
                    this._bdNineBlock = new BetterDictionary<string, TerrainRegionInfo>();
                    for (int i = 0; i < this._triAll._nIndexX; i++)
                    {
                        for (int j = 0; j < this._triAll._nIndexY; j++)
                        {
                            string assetName2 = this.GetNumString(i) + this.GetNumString(j) + "_nb.asset";
                            AssetManager.LoadAsset(strAssetBundlePath, assetName2, typeof(TerrainRegionInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object objInfo)
                            {
                                nLoadCompletedCount++;
                                if (null == objInfo)
                                {
                                    FFDebug.Log(this, FFLogType.Scene, assetName + " is null");
                                }
                                TerrainRegionInfo value = objInfo as TerrainRegionInfo;
                                string key = assetName.Substring(0, 4);
                                this._bdNineBlock.Add(key, value);
                                if (nTotalCount == nLoadCompletedCount && callback != null)
                                {
                                    this._bUseNineBlock = true;
                                    callback();
                                }
                            }, Bundle.BundleType.Default);
                        }
                    }
                }
                else
                {
                    this._bdNineBlock = null;
                    this._bUseNineBlock = false;
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        public void LoadNineBlockItemPrefab(string strAssetBundleName, string strPrefabName, Action<UnityEngine.Object> callback)
        {
            this.LoadItemBundle(strAssetBundleName, delegate (SceneDataAssetBundle sdAssetBundle)
            {
                if (sdAssetBundle == null)
                {
                    FFDebug.LogWarning(this, "Load SceneDataAssetBundle " + strAssetBundleName + " Error!");
                    if (callback != null)
                    {
                        callback(null);
                    }
                }
                else
                {
                    sdAssetBundle.LoadAssetByName(strPrefabName, delegate (SceneDataAssetObject sdAssetObj)
                    {
                        if (sdAssetObj == null)
                        {
                            FFDebug.LogWarning(this, "Load SceneDataAssetObject " + strPrefabName + " Error!");
                            if (callback != null)
                            {
                                callback(null);
                            }
                        }
                        else if (callback != null)
                        {
                            callback(sdAssetObj.objAssetItem);
                        }
                    });
                }
            });
        }

        private void LoadItemBundle(string strAssetBundleName, Action<SceneDataAssetBundle> callback)
        {
            if (null != this._manifest)
            {
                string strAssetBundlePath = this._strScenePath + strAssetBundleName;
                SceneDataAssetBundle sceneDataAssetBundle = SceneDataAssetBundle.GetSceneDataAssetBundle(strAssetBundlePath);
                sceneDataAssetBundle.strDependencies = this._manifest.GetAllDependencies(strAssetBundleName);
                int nCount = sceneDataAssetBundle.strDependencies.Length;
                if (nCount == 0)
                {
                    this.LoadAssetBundle(strAssetBundlePath, callback);
                }
                else
                {
                    int nIndex = 0;
                    for (int i = 0; i < nCount; i++)
                    {
                        this.LoadDependency(sceneDataAssetBundle.strDependencies[i], delegate (SceneDataAssetBundle bundle)
                        {
                            if (bundle != null)
                            {
                                bundle.AddUser(strAssetBundlePath);
                            }
                            nIndex++;
                            if (nIndex == nCount)
                            {
                                this.LoadAssetBundle(strAssetBundlePath, callback);
                            }
                        });
                    }
                }
            }
            else if (callback != null)
            {
                callback(null);
            }
        }

        private void LoadDependency(string strAssetBundleName, Action<SceneDataAssetBundle> callback)
        {
            if (null != this._manifest)
            {
                string strAssetBundlePath = string.Empty;
                if (strAssetBundleName.Contains("common"))
                {
                    strAssetBundlePath = "Scenes/" + strAssetBundleName;
                }
                else
                {
                    strAssetBundlePath = this._strScenePath + strAssetBundleName;
                }
                SceneDataAssetBundle sceneDataAssetBundle = SceneDataAssetBundle.GetSceneDataAssetBundle(strAssetBundlePath);
                sceneDataAssetBundle.strDependencies = this._manifest.GetAllDependencies(strAssetBundleName);
                int nCount = sceneDataAssetBundle.strDependencies.Length;
                if (nCount == 0)
                {
                    this.LoadAssetBundle(strAssetBundlePath, callback);
                }
                else
                {
                    int nIndex = 0;
                    for (int i = 0; i < nCount; i++)
                    {
                        this.LoadDependency(sceneDataAssetBundle.strDependencies[i], delegate (SceneDataAssetBundle bundle)
                        {
                            if (bundle != null)
                            {
                                bundle.AddUser(strAssetBundlePath);
                            }
                            nIndex++;
                            if (nIndex == nCount)
                            {
                                this.LoadAssetBundle(strAssetBundlePath, callback);
                            }
                        });
                    }
                }
            }
            else if (callback != null)
            {
                callback(null);
            }
        }

        private void LoadAssetBundle(string strPath, Action<SceneDataAssetBundle> callback)
        {
            try
            {
                if (SceneData.bdAssetBundleMap.ContainsKey(strPath))
                {
                    SceneData.bdAssetBundleMap[strPath].LoadThis(callback);
                }
                else
                {
                    SceneDataAssetBundle sceneDataAssetBundle = new SceneDataAssetBundle(strPath);
                    SceneData.bdAssetBundleMap[strPath] = sceneDataAssetBundle;
                    sceneDataAssetBundle.LoadThis(delegate (SceneDataAssetBundle bundle)
                    {
                        SceneData.bdAssetBundleMap[strPath] = bundle;
                        callback(bundle);
                    });
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "Load AssetBundle ",
                    strPath,
                    " Error : ",
                    ex
                }));
            }
        }

        public void LoadItemPrefab(string strPath, string strPrefabName, Action<UnityEngine.Object> callback)
        {
            AssetManager.LoadAsset(strPath, strPrefabName, typeof(GameObject), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                GameObject gameObject = obj as GameObject;
                if (null != gameObject)
                {
                    if (callback != null)
                    {
                        callback(gameObject);
                    }
                }
                else
                {
                    FFDebug.LogWarning(this, "Prefab [ " + strPrefabName + " ] is null");
                    if (callback != null)
                    {
                        callback(null);
                    }
                }
            }, Bundle.BundleType.Default);
        }

        private void LoadLightmapInfo(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_lightmap.u"
            });
            AssetManager.LoadAsset(assetBundlePath, "LightmapInfo.asset", typeof(LightmapInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                if (null != obj)
                {
                    this._lightmapInfo = (obj as LightmapInfo);
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    this._lightmapInfo = null;
                    FFDebug.LogWarning(this, "Scene's LightmapInfo is null");
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        private void SetRenderSettings()
        {
            if (null != this._lightmapInfo)
            {
                RenderSettings.skybox = this._lightmapInfo._matSkybox;
                RenderSettings.ambientMode = this._lightmapInfo._ambientMode;
                RenderSettings.ambientEquatorColor = this._lightmapInfo._colorAmbientEquator;
                RenderSettings.ambientGroundColor = this._lightmapInfo._colorAmbientGround;
                RenderSettings.ambientSkyColor = this._lightmapInfo._colorAmbientSky;
                RenderSettings.ambientIntensity = this._lightmapInfo._fAmbientIntensity;
                RenderSettings.defaultReflectionMode = this._lightmapInfo._reflectionMode;
                RenderSettings.defaultReflectionResolution = this._lightmapInfo._nReflectionResolution;
                RenderSettings.reflectionBounces = this._lightmapInfo._nReflectionBounces;
                RenderSettings.reflectionIntensity = this._lightmapInfo._fReflectionIntensity;
                RenderSettings.fog = this._lightmapInfo._bFog;
                RenderSettings.fogMode = this._lightmapInfo._nFogMode;
                RenderSettings.fogColor = this._lightmapInfo._colorFlog;
                RenderSettings.fogDensity = this._lightmapInfo._fDensity;
                RenderSettings.fogStartDistance = this._lightmapInfo._fStar;
                RenderSettings.fogEndDistance = this._lightmapInfo._fEnd;
                if (this._lightmapInfo._reflectionMode == DefaultReflectionMode.Custom)
                {
                    RenderSettings.customReflection = this._lightmapInfo._customReflection;
                }
            }
        }

        private void SetSceneLightmaps()
        {
            if (null != this._lightmapInfo && 0 < this._lightmapInfo._nLightmapsCount)
            {
                LightmapData[] array = new LightmapData[this._lightmapInfo._nLightmapsCount];
                if (this._lightmapInfo._lightmapsMode == LightmapsMode.NonDirectional)
                {
                    for (int i = 0; i < this._lightmapInfo._nLightmapsCount; i++)
                    {
                        array[i] = new LightmapData();
                        array[i].lightmapFar = this._lightmapInfo._lstTex[i];
                        array[i].lightmapNear = null;
                    }
                }
                else
                {
                    for (int j = 0; j < this._lightmapInfo._nLightmapsCount; j++)
                    {
                        array[j] = new LightmapData();
                        array[j].lightmapFar = this._lightmapInfo._lstTex[j * 2];
                        array[j].lightmapNear = this._lightmapInfo._lstTex[j * 2 + 1];
                    }
                }
                LightmapSettings.lightmaps = array;
            }
        }

        public void UnLoadScene()
        {
            if (null != this._mapZoneData)
            {
                if (this._mapZoneData._lstData != null)
                {
                    this._mapZoneData._lstData.Clear();
                }
                UnityEngine.Object.DestroyImmediate(this._mapZoneData, true);
            }
            if (null != this._lightmapInfo)
            {
                if (this._lightmapInfo._lstTex != null)
                {
                    for (int i = 0; i < this._lightmapInfo._lstTex.Count; i++)
                    {
                        if (null != this._lightmapInfo._lstTex[i])
                        {
                            UnityEngine.Object.DestroyImmediate(this._lightmapInfo._lstTex[i], true);
                        }
                    }
                    this._lightmapInfo._lstTex.Clear();
                }
                UnityEngine.Object.DestroyImmediate(this._lightmapInfo, true);
            }
            if (null != this._textureInfo)
            {
                this._textureInfo._lstTextureName.Clear();
                UnityEngine.Object.DestroyImmediate(this._textureInfo, true);
            }
            if (null != this._modelInfo)
            {
                this._modelInfo._lstModelName.Clear();
                UnityEngine.Object.DestroyImmediate(this._modelInfo, true);
            }
            if (null != this._triAll)
            {
                if (this._triAll._lst != null)
                {
                    for (int j = 0; j < this._triAll._lst.Count; j++)
                    {
                        if (this._triAll._lst[j]._lstCLInfo != null)
                        {
                            this._triAll._lst[j]._lstCLInfo.Clear();
                        }
                    }
                    this._triAll._lst.Clear();
                }
                UnityEngine.Object.DestroyImmediate(this._triAll, true);
            }
            if (this._bdNineBlock != null)
            {
                this._bdNineBlock.Clear();
            }
            SceneData.bdAssetBundleMap.BetterForeach(delegate (KeyValuePair<string, SceneDataAssetBundle> item)
            {
                item.Value.UnLoadThis();
            });
            SceneData.bdAssetBundleMap.Clear();
            this.bdMaterialMap.Clear();
            this._manifest = null;
            this._bUseNineBlock = false;
            this._bUseCustomScene = false;
            RenderSettings.fog = false;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = Color.clear;
            RenderSettings.fogDensity = 0f;
            RenderSettings.fogStartDistance = 0f;
            RenderSettings.fogEndDistance = 0f;
            RenderSettings.ambientLight = Color.clear;
            RenderSettings.haloStrength = 0f;
            RenderSettings.flareStrength = 0f;
            RenderSettings.skybox = null;
        }

        public void SetSceneDefaultData()
        {
            this._mapZoneData = null;
            this._triAll = null;
            this._lightmapInfo = null;
            this._textureInfo = null;
            this._modelInfo = null;
            this._materialInfo = null;
            this._bdNineBlock = null;
            this._manifest = null;
            this._strScenePath = string.Empty;
            this._bUseNineBlock = false;
            this._bUseCustomScene = false;
            this._czInfo = null;
        }

        private void LoadSceneTexture(string strSceneName, Action callback)
        {
            this.LoadSceneTextureInfo(strSceneName, delegate
            {
                this.LoadSceneCommonTexture(strSceneName, delegate
                {
                    this.LoadSceneUncommonTexture(strSceneName, delegate
                    {
                        if (callback != null)
                        {
                            callback();
                        }
                    });
                });
            });
        }

        private void LoadSceneTextureInfo(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_scenetexinfo.u"
            });
            AssetManager.LoadAsset(assetBundlePath, "SceneTexInfo.asset", typeof(SceneTextureInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                if (null != obj)
                {
                    this._textureInfo = (obj as SceneTextureInfo);
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    this._textureInfo = null;
                    FFDebug.LogWarning(this, "SceneTexInfo.asset is null");
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        private void LoadSceneCommonTexture(string strSceneName, Action callback)
        {
            if (null == this._textureInfo)
            {
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            int nTexTotalCount = this._textureInfo._lstTextureName.Count;
            int nLoadedCount = 0;
            string str = string.Empty;
            string assetbundlePath = string.Empty;
            if (nTexTotalCount == 0)
            {
                FFDebug.Log(this, FFLogType.Scene, "Can not find scene common texture.");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            for (int i = 0; i < nTexTotalCount; i++)
            {
                str = this._textureInfo._lstTextureName[i];
                assetbundlePath = "Scenes/commontexture/" + str + ".u";
                AssetManager.LoadAssetBundle(assetbundlePath, delegate (string assetBundleName, bool succeed)
                {
                    nLoadedCount++;
                    if (!succeed)
                    {
                        FFDebug.LogWarning(this, "Load common texure assetbundle [ " + assetBundleName + " ] error!");
                    }
                    if (nLoadedCount == nTexTotalCount && callback != null)
                    {
                        callback();
                    }
                }, Bundle.BundleType.Default);
            }
        }

        private void LoadSceneUncommonTexture(string strSceneName, Action callback)
        {
            if (null == this._textureInfo)
            {
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            int nGroupCount = this._textureInfo._nGroupCount;
            string assetbundlePath = string.Empty;
            int nLoadedCount = 0;
            if (nGroupCount == 0)
            {
                FFDebug.LogWarning(this, "Can not find scene uncommon texture!");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            for (int i = 0; i < nGroupCount; i++)
            {
                assetbundlePath = string.Concat(new string[]
                {
                    "Scenes/",
                    strSceneName,
                    "/texture/",
                    strSceneName.ToLower(),
                    "_texgroup_",
                    i.ToString(),
                    ".u"
                });
                AssetManager.LoadAssetBundle(assetbundlePath, delegate (string assetBundleName, bool succeed)
                {
                    nLoadedCount++;
                    if (!succeed)
                    {
                        FFDebug.LogWarning(this, "Load uncommon texture assetbundle [ " + assetBundleName + " ] error!");
                    }
                    if (nGroupCount == nLoadedCount && callback != null)
                    {
                        callback();
                    }
                }, Bundle.BundleType.Default);
            }
        }

        private void LoadSceneMaterial(string strSceneName, Action callback)
        {
            this.LoadSceneMaterialBundle(strSceneName, delegate
            {
                if (this._bUseCustomScene)
                {
                    this.LoadSceneMaterialInfo(strSceneName, delegate
                    {
                        this.LoadSceneMaterialAsset(strSceneName, delegate
                        {
                            if (callback != null)
                            {
                                callback();
                            }
                        });
                    });
                }
                else if (callback != null)
                {
                    callback();
                }
            });
        }

        private void LoadSceneMaterialBundle(string strSceneName, Action callback)
        {
            string assetbundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/material/",
                strSceneName.ToLower(),
                "_allmaterial.u"
            });
            AssetManager.LoadAssetBundle(assetbundlePath, delegate (string assetBundleName, bool succeed)
            {
                if (!succeed)
                {
                    FFDebug.LogWarning(this, "AssetBundle allmaterial.u is null");
                }
                if (callback != null)
                {
                    callback();
                }
            }, Bundle.BundleType.Default);
        }

        private void LoadSceneMaterialInfo(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_scenematerialinfo.u"
            });
            AssetManager.LoadAsset(assetBundlePath, "SceneMaterialInfo.asset", typeof(SceneMaterialInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                if (null != obj)
                {
                    this._materialInfo = (obj as SceneMaterialInfo);
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    this._materialInfo = null;
                    FFDebug.LogWarning(this, "SceneMaterialInfo.asset is null");
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        private void LoadSceneMaterialAsset(string strSceneName, Action callback)
        {
            if (null != this._materialInfo)
            {
                string assetBundlePath = string.Concat(new string[]
                {
                    "Scenes/",
                    strSceneName,
                    "/material/",
                    strSceneName.ToLower(),
                    "_allmaterial.u"
                });
                int nLoadCompletedCount = 0;
                int nCount = this._materialInfo._lstMaterialName.Count;
                for (int i = 0; i < nCount; i++)
                {
                    AssetManager.LoadAsset(assetBundlePath, this._materialInfo._lstMaterialName[i], typeof(Material), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
                    {
                        nLoadCompletedCount++;
                        if (null != obj)
                        {
                            Material value = obj as Material;
                            this.bdMaterialMap.Add(assetName, value);
                        }
                        else
                        {
                            FFDebug.LogWarning(this, "Material's asset is null");
                        }
                        if (nLoadCompletedCount == nCount && callback != null)
                        {
                            callback();
                        }
                    }, Bundle.BundleType.Default);
                }
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private void LoadSceneModel(string strSceneName, Action callback)
        {
            this.LoadSceneModelInfo(strSceneName, delegate
            {
                this.LoadSceneCommonModel(strSceneName, delegate
                {
                    this.LoadSceneUncommonModel(strSceneName, delegate
                    {
                        if (callback != null)
                        {
                            callback();
                        }
                    });
                });
            });
        }

        private void LoadSceneModelInfo(string strSceneName, Action callback)
        {
            string assetBundlePath = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/info/",
                strSceneName.ToLower(),
                "_scenemodelinfo.u"
            });
            AssetManager.LoadAsset(assetBundlePath, "SceneModelInfo.asset", typeof(SceneModelInfo), delegate (string assetBundleName, string assetName, UnityEngine.Object obj)
            {
                if (null != obj)
                {
                    this._modelInfo = (obj as SceneModelInfo);
                    if (callback != null)
                    {
                        callback();
                    }
                }
                else
                {
                    this._modelInfo = null;
                    FFDebug.LogWarning(this, "SceneModelInfo.asset is null");
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }, Bundle.BundleType.Default);
        }

        private void LoadSceneCommonModel(string strSceneName, Action callback)
        {
            if (null == this._modelInfo)
            {
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            int nFBXTotalCount = this._modelInfo._lstModelName.Count;
            int nLoadedCount = 0;
            string str = string.Empty;
            string assetbundlePath = string.Empty;
            if (nFBXTotalCount == 0)
            {
                FFDebug.Log(this, FFLogType.Scene, "Can not find scene common model.");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            for (int i = 0; i < nFBXTotalCount; i++)
            {
                str = this._modelInfo._lstModelName[i];
                assetbundlePath = "Scenes/commonmodel/" + str + ".u";
                AssetManager.LoadAssetBundle(assetbundlePath, delegate (string assetBundleName, bool success)
                {
                    nLoadedCount++;
                    if (!success)
                    {
                        FFDebug.LogWarning(this, "Load common model assetbundle [ " + assetBundleName + " ] error!");
                    }
                    if (nLoadedCount == nFBXTotalCount && callback != null)
                    {
                        callback();
                    }
                }, Bundle.BundleType.Default);
            }
        }

        private void LoadSceneUncommonModel(string strSceneName, Action callback)
        {
            if (null == this._modelInfo)
            {
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            int nGroupCount = this._modelInfo._nGroupCount;
            if (nGroupCount == 0)
            {
                FFDebug.LogWarning(this, "Can not find scene uncommon texture!");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            int nLoadedCount = 0;
            string assetbundlePath = string.Empty;
            string arg = string.Concat(new string[]
            {
                "Scenes/",
                strSceneName,
                "/model/",
                strSceneName.ToLower(),
                "_modelgroup_"
            });
            for (int i = 0; i < nGroupCount; i++)
            {
                assetbundlePath = arg + i + ".u";
                AssetManager.LoadAssetBundle(assetbundlePath, delegate (string assetBundleName, bool succeed)
                {
                    nLoadedCount++;
                    if (!succeed)
                    {
                        FFDebug.LogWarning(this, "Load modelgroup assetbundle [ " + assetBundleName + " ] error!");
                    }
                    if (nLoadedCount == nGroupCount && callback != null)
                    {
                        callback();
                    }
                }, Bundle.BundleType.Default);
            }
        }

        private string GetNumString(int n)
        {
            string result = string.Empty;
            if (n < 10)
            {
                result = "0" + n;
            }
            else
            {
                result = n.ToString();
            }
            return result;
        }

        public static BetterDictionary<string, SceneDataAssetBundle> bdAssetBundleMap = new BetterDictionary<string, SceneDataAssetBundle>();

        public BetterDictionary<string, Material> bdMaterialMap = new BetterDictionary<string, Material>();

        private bool _bUseCustomScene;

        private bool _bUseNineBlock;

        private MapZoneData _mapZoneData;

        private TerrainRegionInfo _triAll;

        private LightmapInfo _lightmapInfo;

        private SceneTextureInfo _textureInfo;

        private SceneModelInfo _modelInfo;

        private SceneMaterialInfo _materialInfo;

        private CameraZoneInfo _czInfo;

        private BetterDictionary<string, TerrainRegionInfo> _bdNineBlock;

        private AssetBundleManifest _manifest;

        private string _strScenePath = string.Empty;
    }
}
