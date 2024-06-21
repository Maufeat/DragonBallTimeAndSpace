using System;
using System.Collections.Generic;
using Algorithms;
using Engine;
using Framework.Base;
using Framework.Managers;
using HighlightingSystem;
using LuaInterface;
using map;
using ResoureManager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CinematicEffects;
using UnityStandardAssets.ImageEffects;

namespace Game.Scene
{
    public class GameScene : IManager
    {
        public string ManagerName
        {
            get
            {
                return "scene_manager";
            }
        }

        public int CurrentLineID
        {
            get
            {
                return _currentLineID;
            }
            set
            {
                if (value != _currentLineID)
                {
                    _currentLineID = value;
                    CallLuaListener.SendLuaEvent("OnLineIDChangeLuaListener", false, _currentLineID);
                }
            }
        }

        public bool isAbattoirScene
        {
            get
            {
                AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
                return controller.getState != AbattoirMatchState.None && (this.sceneInfo.copymapid.ToString() == controller.copyid || controller.getState == AbattoirMatchState.Entering);
            }
        }

        public void Awake()
        {
            this.m_resourceManager = MonobehaviourManager.Instance.MgrCenter.GetSpecialTypeManagerByName<ResourceManager>("resource_manager");
        }

        public void initT4M()
        {
            if (this.m_SceneRoot == null)
            {
                if (this.sceneData.useCustomScene)
                {
                    GameObject gameObject = GameObject.Find("Scene");
                    if (null != gameObject)
                    {
                        this.m_SceneRoot = gameObject.transform;
                    }
                    else
                    {
                        FFDebug.LogWarning(this, "Can not find scene node!");
                    }
                }
                else
                {
                    this.m_SceneRoot = new GameObject("Scene").transform;
                }
            }
            if (null == this.t4m)
            {
                GameObject gameObject2 = new GameObject("t4m");
                this.t4m = gameObject2.AddComponent<T4MObjSC>();
                this.t4m.ObjPosition = new Vector3[0];
                this.t4m.BillboardPosition = new Vector3[0];
                this.t4m.Master = 1;
            }
            this.t4m.EnabledLODSystem = true;
        }

        private void UpdateTerrain()
        {
            if (null != this.t4m)
            {
                this.t4m.Rest();
                this.t4m.Awake();
            }
        }

        public void GetTerrainIndex(Vector3 rolePosition, Action callBack = null)
        {
            if (LSingleton<CurrentMapAccesser>.Instance.BlockCountHeight == 1 || LSingleton<CurrentMapAccesser>.Instance.BlockCountWidth == 1)
            {
                return;
            }
            float num = Mathf.Abs(((float)CellInfos.TerrainWdith / 2f + rolePosition.x) / (float)(CellInfos.TerrainWdith / LSingleton<CurrentMapAccesser>.Instance.BlockCountWidth));
            float num2 = Mathf.Abs(((float)(-(float)CellInfos.TerrainHeight) / 2f + rolePosition.z) / (float)(CellInfos.TerrainHeight / LSingleton<CurrentMapAccesser>.Instance.BlockCountHeight));
            this.m_index_x = (int)num;
            this.m_index_z = (int)num2;
            if (this.m_index_x != this.m_pre_index_x || this.m_index_z != this.m_pre_index_z)
            {
                this.m_pre_index_x = this.m_index_x;
                this.m_pre_index_z = this.m_index_z;
                this.LoadTerrainBlock(callBack);
            }
        }

        private void LoadTerrainBlock(Action callBack)
        {
            int num = CellInfos.TerrainWdith / LSingleton<CurrentMapAccesser>.Instance.BlockCountWidth;
            int num2 = CellInfos.TerrainHeight / LSingleton<CurrentMapAccesser>.Instance.BlockCountHeight;
            int num3 = (this.m_index_x - this.m_block_horizontal <= 0) ? 0 : (this.m_index_x - this.m_block_horizontal);
            int num4 = (this.m_index_x + this.m_block_horizontal <= CellInfos.TerrainWdith / num) ? (this.m_index_x + this.m_block_horizontal) : (CellInfos.TerrainWdith / num - 1);
            int num5 = (this.m_index_z - this.m_block_vertical <= 0) ? 0 : (this.m_index_z - this.m_block_vertical);
            int num6 = (this.m_index_z + this.m_block_vertical <= CellInfos.TerrainHeight / num2) ? (this.m_index_z + this.m_block_vertical) : (CellInfos.TerrainHeight / num2 - 1);
            this.m_BlockRequestQueue.Clear();
            List<string> nineBlock = new List<string>();
            string item;
            for (int i = num3; i < num4 + 1; i++)
            {
                for (int j = num5; j < num6 + 1; j++)
                {
                    item = this.GetNumString(i) + this.GetNumString(j);
                    nineBlock.Add(item);
                }
            }
            for (int k = 0; k < nineBlock.Count; k++)
            {
                GameObject gameObject = null;
                if (!this.m_curTerrainBlockMap.TryGetValue(nineBlock[k], out gameObject))
                {
                    this.m_BlockRequestQueue.Enqueue(nineBlock[k]);
                }
            }
            this.LoadBlocksQueue(delegate
            {
                List<string> unloadList = new List<string>();
                this.m_curTerrainBlockMap.BetterForeach(delegate (KeyValuePair<string, GameObject> itemM)
                {
                    if (!nineBlock.Contains(itemM.Key))
                    {
                        unloadList.Add(itemM.Key);
                    }
                });
                for (int l = 0; l < unloadList.Count; l++)
                {
                    GameObject obj = this.m_curTerrainBlockMap[unloadList[l]];
                    this.m_curTerrainBlockMap.Remove(unloadList[l]);
                    UnityEngine.Object.Destroy(obj);
                }
                Resources.UnloadUnusedAssets();
                if (callBack != null)
                {
                    callBack();
                }
            });
        }

        public void LoadBlocksQueue(Action callBack)
        {
            int count = this.m_BlockRequestQueue.Count;
            string text = string.Empty;
            int num = 0;
            if (count == 0)
            {
                this.UpdateTerrain();
                if (callBack != null)
                {
                    callBack();
                }
            }
            for (int i = 0; i < count; i++)
            {
                text = this.m_BlockRequestQueue.Dequeue();
                GameObject gameObject = new GameObject(text);
                gameObject.transform.SetParent(this.m_SceneRoot);
                this.m_curTerrainBlockMap.Add(text, gameObject);
                num++;
                TerrainRegionInfo terrainRegionInfo = null;
                if (this.sceneData.nineBlockInfo.TryGetValue(text, out terrainRegionInfo))
                {
                    for (int j = 0; j < terrainRegionInfo._nObjCount; j++)
                    {
                        this.triNineBlock._lst.Add(this.CloneObjInfo(terrainRegionInfo._lst[j]));
                    }
                }
                if (num == count)
                {
                    this.triNineBlock._nObjCount = this.triNineBlock._lst.Count;
                    if (this.sls == SceneLoadState.Complete)
                    {
                        this.InstantiateSceneGameObject(this.triNineBlock, null);
                    }
                    this.UpdateTerrain();
                    if (callBack != null)
                    {
                        callBack();
                    }
                }
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

        public void OnUpdate()
        {
            switch (this.sls)
            {
                case SceneLoadState.WaitLoadLowPrefabComplete:
                    if (this.bLoadLowCompleted)
                    {
                        if (!this.bLoadZone)
                        {
                            this.sls = SceneLoadState.StartInstantiateLowPrefab;
                        }
                        else
                        {
                            if (this.lstSTQueueLow.Count != 0)
                            {
                                this.lstSTQueueLow[0].Start();
                            }
                            this.nLowWorkCompletedCount = 0;
                            this.sls = SceneLoadState.WaitInstantiateLowPrefabComplete;
                        }
                    }
                    break;
                case SceneLoadState.StartInstantiateLowPrefab:
                    if (0.999f <= this.STQueueScene.Progress)
                    {
                        if (0 < this.lstSTQueueLow.Count)
                        {
                            this.lstSTQueueLow[0].Start();
                            this.nLowWorkCompletedCount = 0;
                            this.sls = SceneLoadState.WaitInstantiateLowPrefabComplete;
                        }
                        else if (0.999f <= this.STQueueHigh.Progress && this.bLoadHighPrefabAllCompleted)
                        {
                            this.STQueueHigh.Clear();
                            this.STQueueHigh = new SimpleTaskQueue();
                            this.SetSceneStaticBatching();
                            this.ClearLoadTempData();
                            ShadowManager.ResetRenderQueue();
                            this.sls = SceneLoadState.Complete;
                        }
                    }
                    break;
                case SceneLoadState.WaitInstantiateLowPrefabComplete:
                    if (0 < this.lstSTQueueLow.Count)
                    {
                        if (this.nLowWorkCompletedCount < this.lstSTQueueLow.Count - 1 && 0.999f <= this.lstSTQueueLow[this.nLowWorkCompletedCount].Progress)
                        {
                            this.lstSTQueueLow[++this.nLowWorkCompletedCount].Start();
                        }
                        if (0.999f <= this.lstSTQueueLow[this.lstSTQueueLow.Count - 1].Progress && 0.999f <= this.STQueueHigh.Progress)
                        {
                            this.lstSTQueueLow.Clear();
                            this.STQueueHigh.Clear();
                            this.STQueueHigh = new SimpleTaskQueue();
                            this.SetSceneStaticBatching();
                            this.ClearLoadTempData();
                            ShadowManager.ResetRenderQueue();
                            this.sls = SceneLoadState.Complete;
                        }
                    }
                    else if (0.999f <= this.STQueueHigh.Progress && this.bLoadHighPrefabAllCompleted)
                    {
                        this.lstSTQueueLow.Clear();
                        this.STQueueHigh.Clear();
                        this.STQueueHigh = new SimpleTaskQueue();
                        this.SetSceneStaticBatching();
                        this.ClearLoadTempData();
                        ShadowManager.ResetRenderQueue();
                        this.sls = SceneLoadState.Complete;
                    }
                    break;
                case SceneLoadState.Complete:
                    F3DSun.SceneLoadFinished = true;
                    break;
            }
            this.CheckDisAppearObj();
        }

        private void LoadSceneByName(string name, Action callBack)
        {
            MapLoader.LoadScene(name, callBack);
        }

        public void SetActiveFloor(bool bOpenFloor)
        {
            if (null == Camera.main)
            {
                return;
            }
            GameObject gameObject = Camera.main.gameObject;
            if (null != gameObject)
            {
                FxPro component = gameObject.GetComponent<FxPro>();
                if (null != component)
                {
                    component.enabled = bOpenFloor;
                }
            }
        }

        public void SetActiveShadow(bool bOpenShadow)
        {
            if (bOpenShadow)
            {
                ShadowManager.OpenShadow();
            }
            else
            {
                ShadowManager.CloseShadow();
            }
        }

        public void SetActiveDepthOfField(bool bActive)
        {
            if (null == Camera.main)
            {
                return;
            }
            GameObject gameObject = Camera.main.gameObject;
            if (null != gameObject)
            {
                UnityStandardAssets.CinematicEffects.DepthOfField component = gameObject.GetComponent<UnityStandardAssets.CinematicEffects.DepthOfField>();
                if (null != component)
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
                FxPro component2 = gameObject.GetComponent<FxPro>();
                if (null != component2 && this.bFxProDoF)
                {
                    component2.ActiveDepthOfField(bActive);
                }
            }
        }

        public void SetActiveAntialiasing(bool bActive)
        {
            if (null == Camera.main)
            {
                return;
            }
            GameObject gameObject = Camera.main.gameObject;
            if (null != gameObject)
            {
                Antialiasing component = gameObject.GetComponent<Antialiasing>();
                if (null != component)
                {
                    component.enabled = bActive;
                }
            }
        }

        public void SetActiveBloomOptimized(bool bActive)
        {
            if (this.haloGo == null)
            {
                return;
            }
            this.haloGo.SetActive(bActive);
        }

        private void UnLoadBeforeChangeScene()
        {
            if (MainPlayer.Self != null)
            {
                MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
                if (component != null)
                {
                    component.manualSelect.SetTarget(null, false, true);
                }
            }
            if (!this.bSameScene)
            {
                this.UnLoadPreScene();
            }
            else
            {
                this.STQueueScene.Clear();
                this.STQueueScene = new SimpleTaskQueue();
            }
            IconRenderCtrl.Reset();
            ManagerCenter.Instance.GetManager<ObjectPoolManager>().OnReSet();
            ManagerCenter.Instance.GetManager<FFEffectManager>().AddUnloadDirectionEffectByAction();
            ManagerCenter.Instance.GetManager<FFEffectManager>().AddUnloadBesideEffectByBuffEffect();
            ManagerCenter.Instance.GetManager<FFEffectManager>().UnloadAllEffectWithBeside();
            ManagerCenter.Instance.GetManager<CharacterModelMgr>().AddUnloadbesideByMainplayer();
            ManagerCenter.Instance.GetManager<CharacterModelMgr>().Unloadbeside();
            ManagerCenter.Instance.GetManager<WeaponResourcesMgr>().AddUnloadbesideByMainplayer();
            ManagerCenter.Instance.GetManager<WeaponResourcesMgr>().UnloadBeside();
            ManagerCenter.Instance.GetManager<LODManager>().OnReSet();
        }

        private void RestCameraState()
        {
            if (CameraController.Self != null)
            {
                if (MainPlayer.Self != null)
                {
                    MainPlayer.Self.SetPlayerLastDirection();
                }
                if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowTarget2D)
                {
                    CameraController.Self.ChangeState(new CameraFollowTarget2D());
                }
                else if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowTarget4)
                {
                    CameraController.Self.ChangeState(new CameraFollowTarget4());
                }
                else if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowPrepare)
                {
                    CameraController.Self.ChangeState(new CameraFollowTargetPrepare());
                }
            }
        }

        private void InitChangeScene()
        {
            this.RestCameraState();
            UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
            if (manager != null)
            {
                manager.ShowMainUI();
            }
            if (this.bSameScene)
            {
                return;
            }
            if (this.triLoad._lst == null)
            {
                this.triLoad._lst = new List<ObjInfo>();
            }
            if (this.triNineBlock._lst == null)
            {
                this.triNineBlock._lst = new List<ObjInfo>();
            }
            if (this.triAllScene._lst == null)
            {
                this.triAllScene._lst = new List<ObjInfo>();
            }
            this.nCurrentZoneID = 0;
            this.nPreZoneID = 0;
            this.sls = SceneLoadState.None;
            this.sceneData.SetSceneDefaultData();
            this.lstSTQueueLow.Clear();
            this.lstTranZone.Clear();
            this.listBatchZone.Clear();
            this.lstLoadZoneID.Clear();
            this.lstLoadedZoneID.Clear();
            this.bLoadZone = false;
            this.bLoadLowCompleted = false;
            this.bLoadHighPrefabAllCompleted = false;
            this.Light = null;
            this.bFxProDoF = false;
        }

        public void CheckSameScene(SceneInfo sceneInfo)
        {
            if (sceneInfo == null)
            {
                FFDebug.LogError(this, "sceneInfo == null");
                return;
            }
            this.strCopyMeshFindName = string.Empty;
            if (this.CurrentSceneData != null)
            {
                this.bSameMapID = (this.CurrentSceneData.mapID() == sceneInfo.mapid);
            }
            else
            {
                this.bSameMapID = false;
            }
            LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("scenesinfo");
            if (xmlConfigTable == null)
            {
                FFDebug.LogError(this, "LuaConfigManager.GetXmlConfigTable(scenesinfo) == null");
                return;
            }
            LuaTable cacheField_Table = xmlConfigTable.GetCacheField_Table("mapinfo");
            if (cacheField_Table == null)
            {
                FFDebug.LogError(this, "LuaConfigManager.GetXmlConfigTable(scenesinfo)GetCacheField_Table(mapinfo) == null");
                return;
            }
            LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(sceneInfo.mapid.ToString());
            if (cacheField_Table2 == null)
            {
                FFDebug.LogError(this, "LuaConfigManager.GetXmlConfigTable(scenesinfo)GetCacheField_Table(mapinfo)GetCacheField_Table(sceneInfo.mapid.ToString()) == null");
                return;
            }
            this.CurrentSceneData = new mapinfo(cacheField_Table2);
            this.CurrentSceneData.SetSceneID(sceneInfo.sceneid);
            this.CurrentLineID = (int)sceneInfo.lineid;
            string text = this.CurrentSceneData.fileName();
            string text2 = this.CurrentSceneData.blockName();
            string text3 = this.CurrentSceneData.hightName();
            LuaTable configTable = LuaConfigManager.GetConfigTable("copymapinfo", (ulong)sceneInfo.copymapid);
            string text4 = string.Empty;
            string text5 = string.Empty;
            if (configTable != null)
            {
                text4 = configTable.GetCacheField_String("copyblock");
                text5 = configTable.GetCacheField_String("mappath");
            }
            if (string.IsNullOrEmpty(text2))
            {
                text2 = text;
                FFDebug.LogWarning(this, "Can not find scene [" + text + "] block file name, and use default block file!");
            }
            if (string.IsNullOrEmpty(text3))
            {
                text3 = text;
                FFDebug.LogWarning(this, "Can not find scene [" + text + "] hight file name, and use default block file!");
            }
            if (text.CompareTo(this.strSceneName) == 0)
            {
                this.bSameScene = true;
            }
            else
            {
                this.bSameScene = false;
            }
            if (text2.CompareTo(this.strBlockName) == 0)
            {
                this.bSameBlock = true;
            }
            else
            {
                this.bSameBlock = false;
            }
            if (text4.CompareTo(this.strCopyBlockBlockName) == 0)
            {
                this.bSameCopyBlock = true;
            }
            else
            {
                this.bSameCopyBlock = false;
            }
            if (text3.CompareTo(this.strHightName) == 0)
            {
                this.bSameHight = true;
            }
            else
            {
                this.bSameHight = false;
            }
            this.strSceneName = text;
            this.strBlockName = text2;
            this.strHightName = text3;
            this.strCopyBlockBlockName = text4;
            this.strCopyMeshFindName = text5;
        }

        public void ChangeScene(SceneInfo sceneInfo, Action callBack, Action<float> Progress = null)
        {
            if (Progress != null)
            {
                Progress(0f);
            }
            this.sceneInfo = sceneInfo;
            ManagerCenter.Instance.GetManager<EntitiesManager>().UnLoadCharactors();
            this.UnLoadBeforeChangeScene();
            this.InitChangeScene();
            if (this.bSameScene && (!this.bSameBlock || !this.bSameCopyBlock))
            {
                Action<Action> task = delegate (Action callback)
                {
                    MapLoader.LoadMapConfigData(this.strBlockName, this.strCopyBlockBlockName, this.strCopyMeshFindName, callback);
                };
                this.STQueueScene.AddTask(task, "ReadMapData", false);
            }
            if (this.bSameScene && !this.bSameHight)
            {
                Action<Action> task2 = delegate (Action callback)
                {
                    MapLoader.LoadMapHightDataByName(this.strHightName, callback);
                };
                this.STQueueScene.AddTask(task2, "ReadHightData", false);
            }
            if (!this.bSameScene)
            {
                Action<Action> task3 = delegate (Action callback)
                {
                    this.LoadSceneByName(this.strSceneName, callback);
                };
                this.STQueueScene.AddTask(task3, "LoadSceneBundleByName", false);
                Action<Action> task4 = delegate (Action callback)
                {
                    this.initT4M();
                    this.ReadMap(this.strSceneName, this.strBlockName, this.strCopyBlockBlockName, this.strHightName, this.strCopyMeshFindName, delegate
                    {
                        MapHightDataHolder.FlyMapHeight = (float)this.CurrentSceneData.mapflyheight();
                        callback();
                    });
                };
                this.STQueueScene.AddTask(task4, "ReadMapData", false);
                this.STQueueScene.AddTask(new Action<Action>(this.SetSceneLighting), "SetSceneLightingParam", false);
                this.STQueueScene.AddTask(new Action<Action>(this.LoadSceneAllItemInfo), "LoadSceneAllItemInfo", false);
                this.STQueueScene.AddTask(new Action<Action>(this.LoadNineBlockInfo), "LoadNineBlockInfo", false);
                this.STQueueScene.AddTask(new Action<Action>(this.LoadSceneManifest), "LoadSceneManifest", false);
                this.STQueueScene.AddTask(new Action<Action>(this.InitMapZoneData), "LoadMapZoneData", false);
                Action<Action> task5 = delegate (Action callback)
                {
                    Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(new Vector2(sceneInfo.pos.fx, sceneInfo.pos.fy));
                    this.GetInitialZoneID(worldPosByServerPos);
                    callback();
                };
                this.STQueueScene.AddTask(task5, "GetLoadZoneID", false);
                this.STQueueScene.AddTask(new Action<Action>(this.LoadSceneDependency), "LoadSceneDependency", false);
                this.STQueueScene.AddTask(new Action<Action>(this.SetSceneObjectMaterial), "SetSceneObjectMaterial", false);
                Action<Action> task6 = delegate (Action callback)
                {
                    Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(new Vector2(sceneInfo.pos.fx, sceneInfo.pos.fy));
                    if (null != this.sceneData.triAll && 1 < this.sceneData.triAll._nIndexX && 1 < this.sceneData.triAll._nIndexY)
                    {
                        LSingleton<CurrentMapAccesser>.Instance.BlockCountWidth = this.sceneData.triAll._nIndexX;
                        LSingleton<CurrentMapAccesser>.Instance.BlockCountHeight = this.sceneData.triAll._nIndexY;
                        this.GetTerrainIndex(worldPosByServerPos, callback);
                    }
                    else
                    {
                        callback();
                    }
                };
                this.STQueueScene.AddTask(task6, "LoadSceneTerrain", false);
                Action<Action> task7 = delegate (Action callback)
                {
                    if (this.sceneData.useNineBlock && this.triNineBlock._lst != null)
                    {
                        for (int i = 0; i < this.triNineBlock._lst.Count; i++)
                        {
                            this.triAllScene._lst.Add(this.CloneObjInfo(this.triNineBlock._lst[i]));
                        }
                    }
                    if (null != this.sceneData.triAll)
                    {
                        for (int j = 0; j < this.sceneData.triAll._lst.Count; j++)
                        {
                            this.triAllScene._lst.Add(this.CloneObjInfo(this.sceneData.triAll._lst[j]));
                        }
                    }
                    this.triAllScene._nObjCount = this.triAllScene._lst.Count;
                    this.InstantiateSceneGameObject(this.triAllScene, callback);
                };
                this.STQueueScene.AddTask(task7, "InstantiateSceneGameObject", false);
                this.STQueueScene.AddTask(new Action<Action>(this.AdjustCamera), "AdjustCamera", false);
                this.STQueueScene.AddTask(new Action<Action>(this.InitMapCameraZone), "InitMapCameraZone", false);
            }
            Action<Action> task8 = delegate (Action callback)
            {
                string name = "battle";
                ControllerManager.Instance.GetController<UIHpSystem>().LoadAtas(name, callback);
            };
            this.STQueueScene.AddTask(task8, "LoadUIAtlas", false);
            this.STQueueScene.AddTask(new Action<Action>(ControllerManager.Instance.GetController<UIHpSystem>().LoadUIHpSystemAsset), "LoadUIAsset", false);
            Action<Action> task9 = delegate (Action callback)
            {
                new GameObject("AreaMusicTool", new Type[]
                {
                    typeof(AreaMusicTool)
                });
                callback();
            };
            this.STQueueScene.AddTask(task9, "创建AreaMusicTool", false);
            this.STQueueScene.Finish = delegate ()
            {
                ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ResetLastPos();
                this.SetRenderInfoOnLoadSceneOver();
                ManagerCenter.Instance.GetManager<EntitiesManager>().SetCurrentMapEnablePlayerLimit(sceneInfo.mapid);
                callBack();
                GameObject gameObject = GameObject.Find("Scene/ZoneRoot_0/NonStatic_0/Main_Camera");
                if (gameObject == null)
                {
                    gameObject = GameObject.Find("Main Camera");
                }
                if (gameObject && gameObject.GetComponent<HighlightingRenderer>() == null)
                {
                    gameObject.AddComponent<HighlightingRenderer>();
                }
                if (!this.bSameMapID && this.CurrentSceneData.isPlay() == 1)
                {
                    LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.AddMapName", new object[]
                    {
                        Util.GetLuaTable("MainUICtrl"),
                        this.CurrentSceneData.showName(),
                        this.CurrentSceneData.name_en(),
                        this.CurrentSceneData.icon()
                    });
                }
                SceneMusicMgr manager = ManagerCenter.Instance.GetManager<SceneMusicMgr>();
                if (manager != null)
                {
                    manager.OnChangeScene(sceneInfo.mapid);
                }
                this.STQueueScene.OnStep = null;
                this.tempOnLoadActions.Clear();
                this.tempOnLoadActions.AddRange(this.onLoadSceneActions);
                if (this.tempOnLoadActions.Count > 0)
                {
                    for (int i = 0; i < this.tempOnLoadActions.Count; i++)
                    {
                        if (this.tempOnLoadActions[i] != null)
                        {
                            this.tempOnLoadActions[i]();
                        }
                    }
                    this.tempOnLoadActions.Clear();
                }
                if (MainPlayer.Self != null)
                {
                    MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
                    if (component != null)
                    {
                        component.SetTargetNull();
                    }
                }
                Resources.UnloadUnusedAssets();
                GC.Collect();
            };
            if (Progress != null)
            {
                this.STQueueScene.OnStep = delegate (SimpleTaskQueue.Task st)
                {
                    Progress(this.STQueueScene.Progress);
                };
            }
            this.STQueueScene.Start();
        }

        public void RegOnSceneLoadCallBack(Action cb)
        {
            if (!this.onLoadSceneActions.Contains(cb))
            {
                this.onLoadSceneActions.Add(cb);
            }
        }

        public void UnRegOnSceneLoadCallBack(Action cb)
        {
            if (this.onLoadSceneActions.Contains(cb))
            {
                this.onLoadSceneActions.Remove(cb);
            }
        }

        private void InstantiateSceneGameObject(TerrainRegionInfo tri, Action callback)
        {
            if (0 >= tri._nObjCount)
            {
                FFDebug.Log(this, FFLogType.Scene, "TerrainRegionInfo Count <= 0");
                if (this.sceneData.useCustomScene)
                {
                    this.sls = SceneLoadState.Complete;
                }
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            this.bLoadZone = false;
            this.GetTriLoad(tri, this.bLoadZone);
            this.triLoad._nObjCount = this.triLoad._lst.Count;
            if (this.sceneData.useNineBlock)
            {
                InfoCompare comparer = new InfoCompare();
                this.triLoad._lst.Sort(comparer);
                this.nIndex = this.triLoad._lst.FindIndex((ObjInfo info) => info._nPriority < 3);
                this.nSTQCount = 0;
                this.nLoadCompleted = 0;
                string strAssetBundleName = string.Empty;
                if (this.nIndex == 0 && callback != null)
                {
                    callback();
                }
                if (this.nIndex == -1)
                {
                    this.nIndex = this.triLoad._nObjCount;
                    this.sls = SceneLoadState.StartInstantiateLowPrefab;
                }
                this.nLowTotalCount = this.triLoad._nObjCount - this.nIndex;
                this.LoadAllHightPrefabCompelted = callback;
                ObjInfo objInfo = new ObjInfo();
                Queue<ObjInfo> queue = null;
                for (int i = 0; i < this.nIndex; i++)
                {
                    objInfo = this.triLoad._lst[i];
                    strAssetBundleName = objInfo._strFather + "/" + objInfo._strPrefabName + ".u";
                    if (this.dicHighObjInfo.TryGetValue(objInfo._strPrefabName, out queue))
                    {
                        queue.Enqueue(objInfo);
                    }
                    else
                    {
                        queue = new Queue<ObjInfo>();
                        queue.Enqueue(objInfo);
                        this.dicHighObjInfo.Add(objInfo._strPrefabName, queue);
                    }
                    this.sceneData.LoadNineBlockItemPrefab(strAssetBundleName, objInfo._strPrefabName, new Action<UnityEngine.Object>(this.LoadHighItemPrefabCompleted));
                }
                for (int j = this.nIndex; j < this.triLoad._nObjCount; j++)
                {
                    objInfo = this.triLoad._lst[j];
                    strAssetBundleName = objInfo._strFather + "/" + objInfo._strPrefabName + ".u";
                    if (this.dicLowObjInfo.TryGetValue(objInfo._strPrefabName, out queue))
                    {
                        queue.Enqueue(objInfo);
                    }
                    else
                    {
                        queue = new Queue<ObjInfo>();
                        queue.Enqueue(objInfo);
                        this.dicLowObjInfo.Add(objInfo._strPrefabName, queue);
                    }
                    this.sceneData.LoadNineBlockItemPrefab(strAssetBundleName, objInfo._strPrefabName, new Action<UnityEngine.Object>(this.LoadLowItemPrefabCompleted));
                }
            }
            else
            {
                this.nIndex = this.triLoad._lst.FindIndex((ObjInfo info) => info._nPriority < 3);
                this.nSTQCount = 0;
                this.nLoadCompleted = 0;
                string text = "Scenes/" + this.strSceneName + "/";
                string strPath = string.Empty;
                if (this.nIndex == 0 && callback != null)
                {
                    callback();
                }
                if (this.nIndex == -1)
                {
                    this.nIndex = this.triLoad._nObjCount;
                    this.sls = SceneLoadState.StartInstantiateLowPrefab;
                }
                this.nLowTotalCount = this.triLoad._nObjCount - this.nIndex;
                this.LoadAllHightPrefabCompelted = callback;
                ObjInfo objInfo2 = new ObjInfo();
                Queue<ObjInfo> queue2 = null;
                for (int k = 0; k < this.nIndex; k++)
                {
                    objInfo2 = this.triLoad._lst[k];
                    strPath = string.Concat(new string[]
                    {
                        text,
                        objInfo2._strFather,
                        "/",
                        objInfo2._strPrefabName,
                        ".u"
                    });
                    if (this.dicHighObjInfo.TryGetValue(objInfo2._strPrefabName, out queue2))
                    {
                        queue2.Enqueue(objInfo2);
                    }
                    else
                    {
                        queue2 = new Queue<ObjInfo>();
                        queue2.Enqueue(objInfo2);
                        this.dicHighObjInfo.Add(objInfo2._strPrefabName, queue2);
                    }
                    this.sceneData.LoadItemPrefab(strPath, objInfo2._strPrefabName, new Action<UnityEngine.Object>(this.LoadHighItemPrefabCompleted));
                }
                for (int l = this.nIndex; l < this.triLoad._nObjCount; l++)
                {
                    objInfo2 = this.triLoad._lst[l];
                    strPath = string.Concat(new string[]
                    {
                        text,
                        objInfo2._strFather,
                        "/",
                        objInfo2._strPrefabName,
                        ".u"
                    });
                    if (this.dicLowObjInfo.TryGetValue(objInfo2._strPrefabName, out queue2))
                    {
                        queue2.Enqueue(objInfo2);
                    }
                    else
                    {
                        queue2 = new Queue<ObjInfo>();
                        queue2.Enqueue(objInfo2);
                        this.dicLowObjInfo.Add(objInfo2._strPrefabName, queue2);
                    }
                    this.sceneData.LoadItemPrefab(strPath, objInfo2._strPrefabName, new Action<UnityEngine.Object>(this.LoadLowItemPrefabCompleted));
                }
            }
        }

        private void InstantiateZoneGameObject(Action callback)
        {
            if (null == this.sceneData.triAll)
            {
                FFDebug.LogWarning(this, "sceneData.triAll is null");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            if (0 >= this.sceneData.triAll._nObjCount)
            {
                FFDebug.Log(this, FFLogType.Scene, "sceneData.triAll._nObjCount <= 0");
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            this.bLoadZone = true;
            this.GetTriLoad(this.sceneData.triAll, this.bLoadZone);
            this.triLoad._nObjCount = this.triLoad._lst.Count;
            this.nIndex = this.triLoad._lst.FindIndex((ObjInfo info) => info._nPriority < 3);
            this.nSTQCount = 0;
            this.nLoadCompleted = 0;
            string text = "Scenes/" + this.strSceneName + "/";
            if (this.nIndex == 0 && callback != null)
            {
                callback();
            }
            if (this.nIndex == -1)
            {
                this.nIndex = this.triLoad._nObjCount;
                this.sls = SceneLoadState.WaitLoadLowPrefabComplete;
                this.bLoadLowCompleted = true;
            }
            this.nLowTotalCount = this.triLoad._nObjCount - this.nIndex;
            this.LoadAllHightPrefabCompelted = callback;
            ObjInfo objInfo = new ObjInfo();
            Queue<ObjInfo> queue = null;
            for (int i = 0; i < this.nIndex; i++)
            {
                objInfo = this.triLoad._lst[i];
                string strPath = string.Concat(new string[]
                {
                    text,
                    objInfo._strFather,
                    "/",
                    objInfo._strPrefabName,
                    ".u"
                });
                if (this.dicHighObjInfo.TryGetValue(objInfo._strPrefabName, out queue))
                {
                    queue.Enqueue(objInfo);
                }
                else
                {
                    queue = new Queue<ObjInfo>();
                    queue.Enqueue(objInfo);
                    this.dicHighObjInfo.Add(objInfo._strPrefabName, queue);
                }
                this.sceneData.LoadItemPrefab(strPath, objInfo._strPrefabName, new Action<UnityEngine.Object>(this.LoadHighItemPrefabCompleted));
            }
            for (int j = this.nIndex; j < this.triLoad._nObjCount; j++)
            {
                objInfo = this.triLoad._lst[j];
                string strPath = string.Concat(new string[]
                {
                    text,
                    objInfo._strFather,
                    "/",
                    objInfo._strPrefabName,
                    ".u"
                });
                if (this.dicLowObjInfo.TryGetValue(objInfo._strPrefabName, out queue))
                {
                    queue.Enqueue(objInfo);
                }
                else
                {
                    queue = new Queue<ObjInfo>();
                    queue.Enqueue(objInfo);
                    this.dicLowObjInfo.Add(objInfo._strPrefabName, queue);
                }
                this.sceneData.LoadItemPrefab(strPath, objInfo._strPrefabName, new Action<UnityEngine.Object>(this.LoadLowItemPrefabCompleted));
            }
        }

        private void LoadHighItemPrefabCompleted(UnityEngine.Object obj)
        {
            GameObject go = obj as GameObject;
            this.nLoadCompleted++;
            if (null != go)
            {
                ObjInfo oi = new ObjInfo();
                Queue<ObjInfo> queue = null;
                if (this.dicHighObjInfo.TryGetValue(go.name.ToLower(), out queue))
                {
                    oi = queue.Dequeue();
                }
                Action<Action> task = delegate (Action workcallback)
                {
                    this.InstantiateSceneItemObject(go, oi);
                    workcallback();
                };
                this.STQueueHigh.AddTask(task);
            }
            if (this.nLoadCompleted == this.nIndex)
            {
                if (this.LoadAllHightPrefabCompelted != null)
                {
                    this.STQueueHigh.Finish = this.LoadAllHightPrefabCompelted;
                }
                else
                {
                    this.STQueueHigh.Finish = delegate ()
                    {
                    };
                }
                this.bLoadHighPrefabAllCompleted = true;
                this.STQueueHigh.Start();
            }
        }

        private void LoadLowItemPrefabCompleted(UnityEngine.Object obj)
        {
            GameObject go = obj as GameObject;
            if (null != go)
            {
                ObjInfo oi = new ObjInfo();
                Queue<ObjInfo> queue = null;
                if (this.dicLowObjInfo.TryGetValue(go.name.ToLower(), out queue))
                {
                    oi = queue.Dequeue();
                }
                Action<Action> task = delegate (Action workcallback)
                {
                    this.InstantiateSceneItemObject(go, oi);
                    workcallback();
                };
                this.stq.AddTask(task);
            }
            this.nSTQCount++;
            if (this.nSTQCount == 20)
            {
                this.stq.Finish = delegate ()
                {
                };
                this.nSTQCount = 0;
                this.lstSTQueueLow.Add(this.stq);
                this.stq = new SimpleTaskQueue();
                if (this.nSTQCount + this.lstSTQueueLow.Count * 20 == this.nLowTotalCount)
                {
                    this.bLoadLowCompleted = true;
                    this.sls = SceneLoadState.WaitLoadLowPrefabComplete;
                }
            }
            if (this.nSTQCount + this.lstSTQueueLow.Count * 20 == this.nLowTotalCount && this.nSTQCount % 20 != 0)
            {
                this.stq.Finish = delegate ()
                {
                };
                this.lstSTQueueLow.Add(this.stq);
                this.bLoadLowCompleted = true;
                this.sls = SceneLoadState.WaitLoadLowPrefabComplete;
            }
        }

        private void InstantiateSceneItemObject(GameObject go, ObjInfo oi)
        {
            try
            {
                if (null == go || oi == null)
                {
                    FFDebug.LogWarning(this, "Can't Instant Null");
                }
                else
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
                    gameObject.name = oi._strObjName;
                    Transform transform = gameObject.transform;
                    transform.position = oi._v3Position;
                    transform.rotation = Quaternion.Euler(oi._v3Rotation);
                    transform.localScale = oi._v3Scale;
                    if (oi._nLightmapIndex != -1)
                    {
                        MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
                        if (null != component)
                        {
                            component.lightmapIndex = oi._nLightmapIndex;
                            component.lightmapScaleOffset = oi._v4LightmapScaleOffset;
                        }
                    }
                    if (oi._lstCLInfo != null && oi._lstCLInfo.Count != 0)
                    {
                        Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>();
                        if (componentsInChildren != null && 2 <= componentsInChildren.Length)
                        {
                            for (int i = 1; i < componentsInChildren.Length; i++)
                            {
                                MeshRenderer component2 = componentsInChildren[i].GetComponent<MeshRenderer>();
                                if (null != component2)
                                {
                                    component2.lightmapIndex = oi._lstCLInfo[i - 1]._nLightmapIndex;
                                    component2.lightmapScaleOffset = oi._lstCLInfo[i - 1]._v4LightmapScaleOffset;
                                }
                                componentsInChildren[i].gameObject.SetActive(oi._lstCLInfo[i - 1]._bActice);
                            }
                        }
                    }
                    if (oi._nObjType == 1000)
                    {
                        this.haloGo = gameObject;
                    }
                    if (oi._bIsLight)
                    {
                        this.Light = gameObject.GetComponent<Light>();
                        if (LightmapSettings.lightmaps != null && 0 < LightmapSettings.lightmaps.Length)
                        {
                            gameObject.GetComponent<Light>().alreadyLightmapped = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(oi._strIndexXY))
                    {
                        GameObject gameObject2 = null;
                        if (!this.m_curTerrainBlockMap.TryGetValue(oi._strIndexXY, out gameObject2))
                        {
                            FFDebug.Log(this, FFLogType.Scene, string.Concat(new string[]
                            {
                                "Find Node Error: ",
                                oi._strIndexXY,
                                " ",
                                oi._strObjName,
                                " ",
                                oi._strPrefabName
                            }));
                            gameObject2 = new GameObject("error");
                        }
                        transform.SetParent(gameObject2.transform);
                        gameObject.isStatic = true;
                        MeshRenderer[] componentsInChildren2 = transform.GetComponentsInChildren<MeshRenderer>();
                        for (int j = 0; j < componentsInChildren2.Length; j++)
                        {
                            componentsInChildren2[j].gameObject.isStatic = true;
                        }
                    }
                    else
                    {
                        if (oi._bStatic)
                        {
                            Transform transform2 = this.lstTranZone[(1 + this.ZoneIDToIndex(oi._nZoneID)) * 3 + 1].Find("batchZone" + oi._nStaticBatchID);
                            if (transform2 == null)
                            {
                                transform2 = new GameObject("batchZone" + oi._nStaticBatchID).transform;
                                transform2.SetParent(this.lstTranZone[(1 + this.ZoneIDToIndex(oi._nZoneID)) * 3 + 1]);
                                this.listBatchZone.Add(transform2);
                            }
                            transform.SetParent(transform2);
                            gameObject.isStatic = true;
                            MeshRenderer[] componentsInChildren3 = transform.GetComponentsInChildren<MeshRenderer>();
                            for (int k = 0; k < componentsInChildren3.Length; k++)
                            {
                                componentsInChildren3[k].gameObject.isStatic = true;
                            }
                        }
                        else
                        {
                            transform.SetParent(this.lstTranZone[(1 + this.ZoneIDToIndex(oi._nZoneID)) * 3 + 2]);
                        }
                        if (transform.name.Contains("DynamicPlayerLight"))
                        {
                            transform.gameObject.SetActive(false);
                        }
                    }
                    MapDisAreaIdentifier component3 = gameObject.GetComponent<MapDisAreaIdentifier>();
                    if (component3 != null)
                    {
                        component3.ID = oi._InstanceId;
                    }
                }
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, "InstantiateSceneItemObject Error! " + arg);
            }
        }

        private void AdjustCamera(Action callback)
        {
            GameObject gameObject = GameObject.Find("Main Camera");
            if (gameObject != null)
            {
                this.defaultCamera = gameObject.GetComponent<Camera>();
            }
            if (this.defaultCamera == null)
            {
                this.defaultCamera = Camera.main;
            }
            if (this.m_SceneRoot != null)
            {
                Camera componentInChildren = this.m_SceneRoot.GetComponentInChildren<Camera>();
                if (componentInChildren != null && componentInChildren.gameObject.activeSelf)
                {
                    FFDebug.Log(this, FFLogType.Scene, "use scene camera");
                    this.defaultCamera.gameObject.SetActive(false);
                    this.GetCameraDefaultEffectParam(componentInChildren);
                }
                else
                {
                    FFDebug.Log(this, FFLogType.Scene, "use default camera");
                    this.defaultCamera.fieldOfView = 45f;
                    this.defaultCamera.gameObject.SetActive(true);
                }
            }
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            manager.OnMainCameraChange();
            callback();
        }

        private void GetCameraDefaultEffectParam(Camera cam)
        {
            if (null != cam)
            {
                FxPro component = cam.GetComponent<FxPro>();
                if (null != component)
                {
                    this.bFxProDoF = true;
                }
            }
        }

        private void SetSceneLighting(Action callback)
        {
            if (!this.sceneData.useCustomScene)
            {
                this.sceneData.SetSceneLighting(this.strSceneName, delegate
                {
                    this.LightInfo = new LightInfo();
                    if (this.sceneData.lightmapInfo != null)
                    {
                        this.LightInfo.Intensity = this.sceneData.lightmapInfo._intensity;
                        this.LightInfo.LightColor = this.sceneData.lightmapInfo._lightColor;
                        this.LightInfo.LightRot = this.sceneData.lightmapInfo._lightRot;
                        this.LightInfo.IsDynLight = this.sceneData.lightmapInfo._isDynLight;
                        this.LightInfo.LightDir = this.sceneData.lightmapInfo._lightDir;
                        this.LightInfo.RTLightDir = this.sceneData.lightmapInfo._rtLightDir;
                    }
                    if (callback != null)
                    {
                        callback();
                    }
                });
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private void GetLightInfo()
        {
            if (this.bSameScene)
            {
                return;
            }
            GameObject gameObject = GameObject.Find("Scene");
            if (null != gameObject)
            {
                GameObject gameObject2 = gameObject.FindChild("ZoneRoot_0/NonStatic_0/DynamicPlayerLight");
                GameObject gameObject3 = gameObject.FindChild("ZoneRoot_0/NonStatic_0/DynamicPlayerRTLight");
                if (null != gameObject2)
                {
                    Light component = gameObject2.GetComponent<Light>();
                    if (null != component)
                    {
                        this.LightInfo = new LightInfo();
                        this.LightInfo.Intensity = component.intensity;
                        this.LightInfo.LightColor = component.color;
                        this.LightInfo.LightRot = component.transform.rotation.eulerAngles;
                        this.LightInfo.IsDynLight = false;
                        this.LightInfo.LightDir = component.transform.forward;
                    }
                    gameObject2.gameObject.SetActive(false);
                }
                if (null != gameObject3)
                {
                    this.LightInfo.RTLightDir = gameObject3.transform.forward;
                    gameObject3.gameObject.SetActive(false);
                }
            }
        }

        private void TryGetLightByLightObjKeyName(string key)
        {
            GameObject gameObject = GameObject.Find("Scene");
            if (null != gameObject)
            {
                GameObject gameObject2 = gameObject.FindChild("ZoneRoot_0/Static_0/batchZone0");
                if (null != gameObject2)
                {
                    for (int i = 0; i < gameObject2.transform.childCount; i++)
                    {
                        Light component = gameObject2.transform.GetChild(i).GetComponent<Light>();
                        if (null != component && component.gameObject.activeInHierarchy && component.gameObject.name.ToLower().StartsWith(key.ToLower()))
                        {
                            this.LightInfo = new LightInfo();
                            this.LightInfo.Intensity = component.intensity;
                            this.LightInfo.LightColor = component.color;
                            this.LightInfo.LightRot = component.transform.rotation.eulerAngles;
                            this.LightInfo.IsDynLight = false;
                            this.LightInfo.LightDir = component.transform.forward;
                            break;
                        }
                    }
                }
            }
        }

        public void SetMatLightInfoByLightObjKeyName(GameObject obj, string keyName)
        {
            this.TryGetLightByLightObjKeyName(keyName);
            this.SetMatLightInfo(obj, false);
        }

        private void LoadSceneAllItemInfo(Action callback)
        {
            if (!this.sceneData.useCustomScene)
            {
                this.sceneData.LoadSceneAllInfo(this.strSceneName, callback);
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private void LoadNineBlockInfo(Action callback)
        {
            if (!this.sceneData.useCustomScene)
            {
                this.sceneData.LoadSceneNineBlockInfo(this.strSceneName, callback);
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private void LoadSceneManifest(Action callback)
        {
            if (this.sceneData.useNineBlock)
            {
                this.sceneData.LoadManifest(this.strSceneName, callback);
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private void LoadSceneDependency(Action callback)
        {
            if (!this.sceneData.useNineBlock)
            {
                this.sceneData.LoadSceneGameObjectDependency(this.strSceneName, callback);
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private void SetSceneObjectMaterial(Action callback)
        {
            if (this.sceneData.useCustomScene)
            {
                MeshRenderer[] componentsInChildren = this.m_SceneRoot.GetComponentsInChildren<MeshRenderer>();
                if (componentsInChildren != null)
                {
                    for (int i = 0; i < componentsInChildren.Length; i++)
                    {
                        SceneObjectMaterialRef component = componentsInChildren[i].gameObject.GetComponent<SceneObjectMaterialRef>();
                        if (null != component)
                        {
                            Material sharedMaterial = null;
                            if (this.sceneData.bdMaterialMap.TryGetValue(component._strRefMatName, out sharedMaterial))
                            {
                                componentsInChildren[i].sharedMaterial = sharedMaterial;
                            }
                        }
                    }
                }
                ParticleSystemRenderer[] componentsInChildren2 = this.m_SceneRoot.GetComponentsInChildren<ParticleSystemRenderer>(true);
                if (componentsInChildren2 != null)
                {
                    for (int j = 0; j < componentsInChildren2.Length; j++)
                    {
                        SceneObjectMaterialRef component2 = componentsInChildren2[j].gameObject.GetComponent<SceneObjectMaterialRef>();
                        if (null != component2)
                        {
                            Material sharedMaterial2 = null;
                            if (this.sceneData.bdMaterialMap.TryGetValue(component2._strRefMatName, out sharedMaterial2))
                            {
                                componentsInChildren2[j].sharedMaterial = sharedMaterial2;
                            }
                        }
                    }
                }
            }
            if (callback != null)
            {
                callback();
            }
        }

        private void ClearLoadTempData()
        {
            if (null != this.triLoad && this.triLoad._lst != null)
            {
                for (int i = 0; i < this.triLoad._nObjCount; i++)
                {
                    if (this.triLoad._lst[i]._lstCLInfo != null)
                    {
                        this.triLoad._lst[i]._lstCLInfo.Clear();
                    }
                }
                this.triLoad._nObjCount = 0;
                this.triLoad._lst.Clear();
            }
            if (null != this.triNineBlock && this.triNineBlock._lst != null)
            {
                for (int j = 0; j < this.triNineBlock._nObjCount; j++)
                {
                    if (this.triNineBlock._lst[j]._lstCLInfo != null)
                    {
                        this.triNineBlock._lst[j]._lstCLInfo.Clear();
                    }
                }
                this.triNineBlock._nObjCount = 0;
                this.triNineBlock._lst.Clear();
            }
            if (null != this.triAllScene && this.triAllScene._lst != null)
            {
                for (int k = 0; k < this.triAllScene._nObjCount; k++)
                {
                    if (this.triAllScene._lst[k]._lstCLInfo != null)
                    {
                        this.triAllScene._lst[k]._lstCLInfo.Clear();
                    }
                }
                this.triAllScene._nObjCount = 0;
                this.triAllScene._lst.Clear();
            }
            this.LoadAllHightPrefabCompelted = null;
            this.bLoadHighPrefabAllCompleted = false;
            this.bLoadLowCompleted = false;
            this.dicHighObjInfo.BetterForeach(delegate (KeyValuePair<string, Queue<ObjInfo>> item)
            {
                item.Value.Clear();
            });
            this.dicHighObjInfo.Clear();
            this.dicLowObjInfo.BetterForeach(delegate (KeyValuePair<string, Queue<ObjInfo>> item)
            {
                item.Value.Clear();
            });
            this.dicLowObjInfo.Clear();
        }

        private void GetTriLoad(TerrainRegionInfo tri, bool bLoadZone)
        {
            bool flag = false;
            if (GameSystemSettings.GetIsLoadLowPriorityObject())
            {
                flag = true;
            }
            int count = tri._lst.Count;
            if (this.lstLoadZoneID.Count != 0)
            {
                if (bLoadZone)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (this.sls == SceneLoadState.Complete)
                        {
                            ObjInfo objInfo = tri._lst[i];
                            if (objInfo._nZoneID != 0 && this.lstLoadZoneID[this.ZoneIDToIndex(objInfo._nZoneID)])
                            {
                                if (flag)
                                {
                                    this.triLoad._lst.Add(this.CloneObjInfo(objInfo));
                                }
                                else if (3 <= objInfo._nPriority)
                                {
                                    this.triLoad._lst.Add(this.CloneObjInfo(objInfo));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (this.sls == SceneLoadState.None)
                        {
                            ObjInfo objInfo2 = tri._lst[j];
                            if (objInfo2._nZoneID == 0 || this.lstLoadZoneID[this.ZoneIDToIndex(objInfo2._nZoneID)])
                            {
                                if (flag)
                                {
                                    this.triLoad._lst.Add(this.CloneObjInfo(objInfo2));
                                }
                                else if (3 <= objInfo2._nPriority)
                                {
                                    this.triLoad._lst.Add(this.CloneObjInfo(objInfo2));
                                }
                            }
                        }
                    }
                }
                for (int k = 0; k < this.lstLoadZoneID.Count; k++)
                {
                    this.lstLoadZoneID[k] = false;
                }
            }
            else if (flag)
            {
                for (int l = 0; l < count; l++)
                {
                    this.triLoad._lst.Add(this.CloneObjInfo(tri._lst[l]));
                }
            }
            else
            {
                for (int m = 0; m < count; m++)
                {
                    ObjInfo objInfo3 = tri._lst[m];
                    if (3 <= objInfo3._nPriority)
                    {
                        this.triLoad._lst.Add(this.CloneObjInfo(objInfo3));
                    }
                }
            }
        }

        private void InstantiateZoneGameObjectCompleted()
        {
        }

        public void OnSceneLoadNotifyServer()
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager != null && manager.PNetWork != null)
            {
                manager.PNetWork.Notify_SceneLoaded_CS((ulong)this.sceneInfo.mapid);
            }
        }

        public void SetMatLightInfo(GameObject ModelObj, bool isIconRender = false)
        {
            if (this.LightInfo != null && null != ModelObj)
            {
                Renderer[] componentsInChildren = ModelObj.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    if (componentsInChildren[i].sharedMaterial != null)
                    {
                        if (componentsInChildren[i].sharedMaterial.HasProperty("_LightColor"))
                        {
                            componentsInChildren[i].sharedMaterial.SetColor("_LightColor", this.LightInfo.LightColor);
                        }
                        float intensity = this.LightInfo.Intensity;
                        Vector3 v;
                        if (!isIconRender)
                        {
                            v = this.LightInfo.LightDir;
                        }
                        else
                        {
                            Vector3 rtlightDir = new Vector3(-1f, -1f, -1f);
                            if (this.LightInfo.RTLightDir != Vector3.zero)
                            {
                                rtlightDir = this.LightInfo.RTLightDir;
                            }
                            v = Vector3.Normalize(ModelObj.transform.TransformDirection(rtlightDir));
                        }
                        if (componentsInChildren[i].sharedMaterial.HasProperty("_LightDir"))
                        {
                            componentsInChildren[i].sharedMaterial.SetVector("_LightDir", v);
                        }
                        if (componentsInChildren[i].sharedMaterial.HasProperty("_LightIntensity"))
                        {
                            componentsInChildren[i].sharedMaterial.SetFloat("_LightIntensity", intensity);
                        }
                    }
                }
            }
        }

        public void CheckLoadZoneData()
        {
            if (null != this.sceneData.mapZoneData)
            {
                if (this.nCurrentZoneID != 0)
                {
                    int num = this.CheckLoadAreaID();
                    if (num != 0)
                    {
                        this.nPreZoneID = this.nCurrentZoneID;
                        this.nCurrentZoneID = 0;
                        this.lstLoadZoneID[this.ZoneIDToIndex(num)] = true;
                        if (!this.lstLoadedZoneID[this.ZoneIDToIndex(num)])
                        {
                            this.lstLoadedZoneID[this.ZoneIDToIndex(num)] = true;
                            this.InstantiateZoneGameObject(new Action(this.InstantiateZoneGameObjectCompleted));
                        }
                        else
                        {
                            this.lstTranZone[(1 + this.ZoneIDToIndex(num)) * 3].gameObject.SetActive(true);
                            this.lstLoadZoneID[this.ZoneIDToIndex(num)] = false;
                        }
                    }
                    else
                    {
                        num = this.GetPlayerZoneID();
                        if (num != -1 && num != this.nCurrentZoneID)
                        {
                            this.nPreZoneID = this.nCurrentZoneID;
                            this.nCurrentZoneID = num;
                            this.lstLoadZoneID[this.ZoneIDToIndex(num)] = true;
                            if (!this.lstLoadedZoneID[this.ZoneIDToIndex(num)])
                            {
                                this.lstLoadedZoneID[this.ZoneIDToIndex(num)] = true;
                                this.InstantiateZoneGameObject(new Action(this.InstantiateZoneGameObjectCompleted));
                            }
                            else
                            {
                                this.lstTranZone[(1 + this.ZoneIDToIndex(num)) * 3].gameObject.SetActive(true);
                                this.lstTranZone[(1 + this.ZoneIDToIndex(this.nPreZoneID)) * 3].gameObject.SetActive(false);
                                this.lstLoadZoneID[this.ZoneIDToIndex(num)] = false;
                            }
                        }
                    }
                }
                else
                {
                    int playerZoneID = this.GetPlayerZoneID();
                    if (playerZoneID != -1)
                    {
                        this.nCurrentZoneID = playerZoneID;
                        if (playerZoneID == this.nPreZoneID)
                        {
                            for (int i = 3; i < this.lstTranZone.Count; i += 3)
                            {
                                if ((1 + this.ZoneIDToIndex(playerZoneID)) * 3 != i && this.lstLoadedZoneID[i / 3 - 1])
                                {
                                    this.lstTranZone[i].gameObject.SetActive(false);
                                }
                            }
                        }
                        else if (this.nPreZoneID != 0)
                        {
                            this.lstTranZone[(1 + this.ZoneIDToIndex(this.nPreZoneID)) * 3].gameObject.SetActive(false);
                        }
                        else
                        {
                            for (int j = 3; j < this.lstTranZone.Count; j += 3)
                            {
                                if ((1 + this.ZoneIDToIndex(playerZoneID)) * 3 != j && this.lstLoadedZoneID[j / 3 - 1])
                                {
                                    this.lstTranZone[j].gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Vector2 GetPlayerPosition()
        {
            return MainPlayer.Self.CurrentPosition2D;
        }

        private Vector3 GetPlayerWorldPosition()
        {
            Vector2 playerPosition = this.GetPlayerPosition();
            return GraphUtils.GetWorldPosByServerPos(playerPosition);
        }

        private void InitMapCameraZone(Action callback)
        {
            this.sceneData.LoadMapCameraZoneInfo(this.strSceneName, callback);
        }

        private void InitMapZoneData(Action callback)
        {
            if (!this.sceneData.useCustomScene)
            {
                this.sceneData.LoadMapZoneData(this.strSceneName, delegate
                {
                    int num = (!(null == this.sceneData.mapZoneData)) ? this.sceneData.mapZoneData._lstData.Count : 0;
                    for (int i = 0; i < num; i++)
                    {
                        this.lstLoadZoneID.Add(false);
                    }
                    for (int j = 0; j < num; j++)
                    {
                        this.lstLoadedZoneID.Add(false);
                    }
                    for (int k = 0; k <= num; k++)
                    {
                        Transform transform = new GameObject("ZoneRoot_" + k).transform;
                        transform.SetParent(this.m_SceneRoot);
                        this.lstTranZone.Add(transform);
                        Transform transform2 = new GameObject("Static_" + k).transform;
                        transform2.SetParent(transform);
                        this.lstTranZone.Add(transform2);
                        Transform transform3 = new GameObject("NonStatic_" + k).transform;
                        transform3.SetParent(transform);
                        this.lstTranZone.Add(transform3);
                    }
                    if (callback != null)
                    {
                        callback();
                    }
                });
            }
            else if (callback != null)
            {
                callback();
            }
        }

        private int GetPlayerZoneID()
        {
            Vector3 playerWorldPosition = this.GetPlayerWorldPosition();
            if (null != this.sceneData.mapZoneData)
            {
                for (int i = 0; i < this.sceneData.mapZoneData._nCount; i++)
                {
                    if (playerWorldPosition.x >= this.sceneData.mapZoneData._lstData[i].v3ZoneAA.x && playerWorldPosition.z <= this.sceneData.mapZoneData._lstData[i].v3ZoneAA.z && playerWorldPosition.x <= this.sceneData.mapZoneData._lstData[i].v3ZoneBB.x && playerWorldPosition.z >= this.sceneData.mapZoneData._lstData[i].v3ZoneBB.z)
                    {
                        return this.sceneData.mapZoneData._lstData[i].nZoneID;
                    }
                }
            }
            return -1;
        }

        private void GetInitialZoneID(Vector3 v3PlayerPos)
        {
            if (null != this.sceneData.mapZoneData)
            {
                for (int i = 0; i < this.sceneData.mapZoneData._nCount; i++)
                {
                    if (v3PlayerPos.x >= this.sceneData.mapZoneData._lstData[i].v3ZoneAA.x && v3PlayerPos.z <= this.sceneData.mapZoneData._lstData[i].v3ZoneAA.z && v3PlayerPos.x <= this.sceneData.mapZoneData._lstData[i].v3ZoneBB.x && v3PlayerPos.z >= this.sceneData.mapZoneData._lstData[i].v3ZoneBB.z)
                    {
                        this.nCurrentZoneID = this.sceneData.mapZoneData._lstData[i].nZoneID;
                    }
                }
            }
            if (this.nCurrentZoneID != 0)
            {
                this.lstLoadZoneID[this.ZoneIDToIndex(this.nCurrentZoneID)] = true;
                this.lstLoadedZoneID[this.ZoneIDToIndex(this.nCurrentZoneID)] = true;
            }
            else if (null != this.sceneData.mapZoneData)
            {
                float num = float.MaxValue;
                float num2 = float.MaxValue;
                int num3 = 0;
                int nID = 0;
                for (int j = 0; j < this.sceneData.mapZoneData._nCount; j++)
                {
                    float num4 = (this.sceneData.mapZoneData._lstData[j].v3ZoneAA.x + this.sceneData.mapZoneData._lstData[j].v3ZoneBB.x) / 2f;
                    float num5 = (this.sceneData.mapZoneData._lstData[j].v3ZoneAA.z + this.sceneData.mapZoneData._lstData[j].v3ZoneBB.z) / 2f;
                    float num6 = Mathf.Pow(Mathf.Abs(v3PlayerPos.x - num4), 2f) + Mathf.Pow(Mathf.Abs(v3PlayerPos.z - num5), 2f);
                    if (num6 >= num && num6 < num2)
                    {
                        nID = this.sceneData.mapZoneData._lstData[j].nZoneID;
                        num2 = num6;
                    }
                    if (num6 < num && num6 < num2)
                    {
                        nID = num3;
                        num3 = this.sceneData.mapZoneData._lstData[j].nZoneID;
                        num2 = num;
                        num = num6;
                    }
                }
                this.lstLoadZoneID[this.ZoneIDToIndex(num3)] = true;
                this.lstLoadZoneID[this.ZoneIDToIndex(nID)] = true;
                this.lstLoadedZoneID[this.ZoneIDToIndex(num3)] = true;
                this.lstLoadedZoneID[this.ZoneIDToIndex(nID)] = true;
            }
        }

        private int CheckLoadAreaID()
        {
            if (MainPlayer.Self != null && null != this.sceneData.mapZoneData)
            {
                Vector3 playerWorldPosition = this.GetPlayerWorldPosition();
                int count = this.sceneData.mapZoneData._lstAreaData.Count;
                for (int i = 0; i < count; i++)
                {
                    LoadAreaData loadAreaData = this.sceneData.mapZoneData._lstAreaData[i];
                    if (playerWorldPosition.x >= loadAreaData.v3AreaAA.x && playerWorldPosition.x <= loadAreaData.v3AreaBB.x && playerWorldPosition.z <= loadAreaData.v3AreaAA.z && playerWorldPosition.z >= loadAreaData.v3AreaBB.z)
                    {
                        return loadAreaData.nZoneID;
                    }
                }
            }
            return 0;
        }

        private int ZoneIDToIndex(int nID)
        {
            if (nID == 0)
            {
                return -1;
            }
            return (int)Mathf.Log((float)nID, 2f);
        }

        private int IndexToZoneID(int nIndex)
        {
            return (int)Mathf.Pow(2f, (float)nIndex);
        }

        private ObjInfo CloneObjInfo(ObjInfo oi)
        {
            if (oi == null)
            {
                return null;
            }
            ObjInfo objInfo = new ObjInfo();
            objInfo._strObjName = oi._strObjName;
            objInfo._strFather = oi._strFather;
            objInfo._v3Position = oi._v3Position;
            objInfo._v3Rotation = oi._v3Rotation;
            objInfo._v3Scale = oi._v3Scale;
            objInfo._strPrefabName = oi._strPrefabName;
            objInfo._strPrefabPath = oi._strPrefabPath;
            objInfo._nPriority = oi._nPriority;
            objInfo._nZoneID = oi._nZoneID;
            objInfo._nStaticBatchID = oi._nStaticBatchID;
            objInfo._nObjType = oi._nObjType;
            objInfo._bStatic = oi._bStatic;
            objInfo._bCommon = oi._bCommon;
            objInfo._nLightmapIndex = oi._nLightmapIndex;
            objInfo._v4LightmapScaleOffset = oi._v4LightmapScaleOffset;
            objInfo._bIsLight = oi._bIsLight;
            objInfo._strIndexXY = oi._strIndexXY;
            objInfo._InstanceId = oi._InstanceId;
            objInfo._lstCLInfo = new List<ChildLightmapInfo>();
            objInfo._lstCLInfo.AddRange(oi._lstCLInfo.ToArray());
            return objInfo;
        }

        private void SetSceneStaticBatching()
        {
        }

        private void UnLoadPreScene()
        {
            if (this.haloGo != null)
            {
                this.haloGo = null;
            }
            if (this.t4m != null)
            {
                UnityEngine.Object.Destroy(this.t4m.gameObject);
                this.t4m = null;
            }
            this.m_curTerrainBlockMap.BetterForeach(delegate (KeyValuePair<string, GameObject> item)
            {
                GameObject value = item.Value;
                this.m_curTerrainBlockMap.Remove(item.Key);
                UnityEngine.Object.Destroy(value);
            });
            this.m_curTerrainBlockMap.Clear();
            if (null != this.m_SceneRoot)
            {
                UnityEngine.Object.Destroy(this.m_SceneRoot.gameObject);
                this.m_SceneRoot = null;
            }
            this.m_pre_index_x = -1;
            this.m_pre_index_z = -1;
            this.PathFinder = null;
            if (this.sceneData != null)
            {
                this.sceneData.UnLoadScene();
            }
            this.STQueueScene.Clear();
            this.STQueueScene = new SimpleTaskQueue();
            this.STQueueHigh.Clear();
            this.STQueueHigh = new SimpleTaskQueue();
            AssetManager.UnloadAssetBundle(true, Bundle.BundleType.Default);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            UnityEngine.SceneManagement.SceneManager.LoadScene("EmptyScene");
            ControllerManager.Instance.GetController<OccupyController>().RemoveAllHoldTransform();
        }

        private void ReadMap(string scenename, string blockName, string copymapBlockName, string hightmapname, string navmeshfindname, Action callback)
        {
            MapLoader.LoadMapHightDataByName(hightmapname, delegate
            {
                MapLoader.LoadMapConfigData(blockName, copymapBlockName, navmeshfindname, delegate
                {
                    MapLoader.LoadDisAreaDataByName(scenename, delegate (MapDisAreaFile mapdisInfo)
                    {
                        if (mapdisInfo != null && mapdisInfo.areainfos != null)
                        {
                            this.mMapDisDatas = mapdisInfo.areainfos;
                        }
                        if (callback != null)
                        {
                            callback();
                        }
                    });
                });
            });
        }

        public void CheckInDisArea(int x, int y)
        {
            if (this.mDisObjs == null)
            {
                this.mDisObjs = UnityEngine.Object.FindObjectsOfType<MapDisAreaIdentifier>();
            }
            if (this.mMapDisDatas != null)
            {
                for (int i = 0; i < this.mMapDisDatas.Count; i++)
                {
                    bool flag = false;
                    if ((long)x >= (long)((ulong)this.mMapDisDatas[i].posX) && (long)x <= (long)((ulong)(this.mMapDisDatas[i].posX + this.mMapDisDatas[i].width - 1U)) && (long)y >= (long)((ulong)this.mMapDisDatas[i].posY) && (long)y <= (long)((ulong)(this.mMapDisDatas[i].posY + this.mMapDisDatas[i].height - 1U)))
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        for (int j = 0; j < this.mDisObjs.Length; j++)
                        {
                            if (this.mMapDisDatas[i].modelID == this.mDisObjs[j].ID)
                            {
                                this.SetObjDisapper(this.mDisObjs[j].gameObject);
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < this.mDisObjs.Length; k++)
                        {
                            if (this.mMapDisDatas[i].modelID == this.mDisObjs[k].ID)
                            {
                                this.SetObjAppear(this.mDisObjs[k].gameObject);
                            }
                        }
                    }
                }
            }
        }

        private void SetObjDisapper(GameObject obj)
        {
            Renderer component = obj.GetComponent<Renderer>();
            if (component != null)
            {
                Material material = component.material;
                if (material != null)
                {
                    float @float = material.GetFloat("_Alpha");
                    if (@float >= 1f)
                    {
                        if (!this.disAppearObjList.Contains(material))
                        {
                            this.disAppearObjList.Add(material);
                        }
                        if (this.appearObjList.Contains(material))
                        {
                            this.appearObjList.Remove(material);
                        }
                    }
                }
            }
        }

        private void SetObjAppear(GameObject obj)
        {
            Renderer component = obj.GetComponent<Renderer>();
            if (component != null)
            {
                Material material = component.material;
                if (material != null)
                {
                    float @float = material.GetFloat("_Alpha");
                    if (@float <= 1f)
                    {
                        if (!this.appearObjList.Contains(material))
                        {
                            this.appearObjList.Add(material);
                        }
                        if (this.disAppearObjList.Contains(material))
                        {
                            this.disAppearObjList.Remove(material);
                        }
                    }
                }
            }
        }

        private void CheckDisAppearObj()
        {
            for (int i = 0; i < this.disAppearObjList.Count; i++)
            {
                if (this.disAppearObjList[i].HasProperty("_Alpha"))
                {
                    float @float = this.disAppearObjList[i].GetFloat("_Alpha");
                    if (@float >= 0f)
                    {
                        this.disAppearObjList[i].SetFloat("_Alpha", @float - Time.deltaTime * this.disAppearSpeed);
                    }
                }
            }
            for (int j = 0; j < this.appearObjList.Count; j++)
            {
                if (this.appearObjList[j].HasProperty("_Alpha"))
                {
                    float float2 = this.appearObjList[j].GetFloat("_Alpha");
                    if (float2 <= 1f)
                    {
                        this.appearObjList[j].SetFloat("_Alpha", float2 + Time.deltaTime * this.disAppearSpeed);
                    }
                }
            }
        }

        public void ShowBlockPoint()
        {
            if (this.stoppoints == null)
            {
                this.stoppoints = new GameObject("stoppoints").transform;
                this.stoppoints.transform.SetParent(this.m_SceneRoot);
                this.stoppoints.transform.position = new Vector3(LSingleton<CurrentMapAccesser>.Instance.CellSizeX / 2f, 0f, -LSingleton<CurrentMapAccesser>.Instance.CellSizeX / 2f);
            }
            if (1 < this.stoppoints.GetComponentsInChildren<Transform>().Length)
            {
                return;
            }
            int infoUpperBound = CellInfos.GetInfoUpperBound(1);
            int infoUpperBound2 = CellInfos.GetInfoUpperBound(0);
            GameObject gameObject = null;
            int num = 0;
            TileFlag tileFlag = TileFlag.TILE_BLOCK_NORMAL;
            Shader shader = Shader.Find("Unlit/Color");
            for (int i = 0; i <= infoUpperBound; i++)
            {
                bool flag = true;
                for (int j = 0; j <= infoUpperBound2; j++)
                {
                    if (GraphUtils.IsBlockPointForMove((int)CellInfos.MapInfos[j, i]))
                    {
                        if (flag)
                        {
                            flag = false;
                        }
                        TileFlag flagByRowAndColumn = (TileFlag)CellInfos.GetFlagByRowAndColumn((uint)i, (uint)j);
                        if (null != gameObject && j - num == 1 && tileFlag == flagByRowAndColumn)
                        {
                            gameObject.transform.localScale += new Vector3(0f, 0f, LSingleton<CurrentMapAccesser>.Instance.CellSizeX);
                            gameObject.transform.position += new Vector3(0f, 0f, -LSingleton<CurrentMapAccesser>.Instance.CellSizeX / 2f);
                            num = j;
                            tileFlag = flagByRowAndColumn;
                        }
                        else
                        {
                            GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            gameObject2.transform.localScale = new Vector3(LSingleton<CurrentMapAccesser>.Instance.CellSizeX, LSingleton<CurrentMapAccesser>.Instance.CellSizeX, LSingleton<CurrentMapAccesser>.Instance.CellSizeX);
                            gameObject2.name = "Cube" + j.ToString() + i.ToString();
                            gameObject2.transform.SetParent(this.stoppoints);
                            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos((float)i, (float)j);
                            worldPosByServerPos.y = MapHightDataHolder.GetMapHeight(worldPosByServerPos.x, worldPosByServerPos.z);
                            gameObject2.transform.position = worldPosByServerPos;
                            UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
                            gameObject2.GetComponent<MeshRenderer>().useLightProbes = false;
                            gameObject2.GetComponent<MeshRenderer>().receiveShadows = true;
                            gameObject2.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
                            Material material = new Material(shader);
                            material.color = this.GetColorByTileFlag(flagByRowAndColumn);
                            gameObject2.GetComponent<MeshRenderer>().sharedMaterial = material;
                            gameObject = gameObject2;
                            num = j;
                            tileFlag = flagByRowAndColumn;
                        }
                    }
                }
            }
            if (null != this.stoppoints)
            {
            }
        }

        public void ResetBlock()
        {
            if (null != this.stoppoints && MainPlayer.Self != null && null != MainPlayer.Self.ModelObj)
            {
                this.stoppoints.transform.position = new Vector3(0f, MainPlayer.Self.ModelObj.transform.position.y + 0.5f, 0f);
            }
        }

        public void ClearBlock()
        {
            if (null != this.stoppoints)
            {
                UnityEngine.Object.Destroy(this.stoppoints.gameObject);
                Resources.UnloadUnusedAssets();
            }
        }

        private Color GetColorByTileFlag(TileFlag flag)
        {
            if (flag == (TileFlag)0)
            {
                return Color.black;
            }
            if (flag == (TileFlag)(-1))
            {
                return Color.white;
            }
            Color color = new Color(0f, 0f, 0f, 1f);
            string[] names = Enum.GetNames(typeof(TileFlag));
            bool flag2 = true;
            if (names == null)
            {
                return Color.black;
            }
            for (int i = 0; i < names.Length; i++)
            {
                TileFlag tileFlag = (TileFlag)((int)Enum.Parse(typeof(TileFlag), names[i], true));
                uint num = (uint)tileFlag;
                if (((ulong)num & (ulong)((long)flag)) != 0UL)
                {
                    color += this.dicColor[tileFlag];
                    flag2 = false;
                }
            }
            if (flag2)
            {
                return Color.black;
            }
            return color;
        }

        private void DrawNineScreenByLineRender()
        {
            int num = LSingleton<CurrentMapAccesser>.Instance.CellNumX / 13;
            int num2 = LSingleton<CurrentMapAccesser>.Instance.CellNumY / 19;
            Transform transform = new GameObject("line").transform;
            transform.SetParent(this.m_SceneRoot);
            for (int i = 1; i <= num; i++)
            {
                this.DrawLine(transform, GraphUtils.GetWorldPosByServerPos(new Vector2((float)(13 * i), 0f)) + new Vector3(0f, 0.1f, 0f), GraphUtils.GetWorldPosByServerPos(new Vector2((float)(13 * i), (float)(LSingleton<CurrentMapAccesser>.Instance.CellNumY - 1))) + new Vector3(0f, -1f, 0f), 0.1f);
            }
            for (int j = 1; j <= num2; j++)
            {
                this.DrawLine(transform, GraphUtils.GetWorldPosByServerPos(new Vector2(0f, (float)(19 * j))) + new Vector3(0f, 0.1f, 0f), GraphUtils.GetWorldPosByServerPos(new Vector2((float)(LSingleton<CurrentMapAccesser>.Instance.CellNumX - 1), (float)(19 * j))) + new Vector3(0f, -1f, 0f), 0.1f);
            }
        }

        private void DrawLine(Transform root, Vector3 start, Vector3 end, float width = 0.1f)
        {
            GameObject gameObject = new GameObject("line");
            gameObject.transform.SetParent(root);
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.SetWidth(width, width);
        }

        public void OnReSet()
        {
            this.UnLoadPreScene();
            this.STQueueScene.Clear();
            this.STQueueScene = new SimpleTaskQueue();
            this.STQueueHigh.Clear();
            this.STQueueHigh = new SimpleTaskQueue();
            for (int i = 0; i < this.lstSTQueueLow.Count; i++)
            {
                this.lstSTQueueLow[i].Clear();
            }
            this.lstSTQueueLow.Clear();
            this.ClearLoadTempData();
            if (this.m_SceneRoot != null)
            {
                UnityEngine.Object.Destroy(this.m_SceneRoot);
            }
            this.CurrentSceneData = null;
            this.m_SceneRoot = null;
            this.strSceneName = string.Empty;
            this.strBlockName = string.Empty;
        }

        private void SetRenderInfoOnLoadSceneOver()
        {
            if (this.sceneData != null && this.sceneData.useCustomScene)
            {
                this.GetLightInfo();
            }
            ShadowManager.OpenShadow();
            if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer != null)
            {
                this.SetMatLightInfo(ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.ModelObj, false);
            }
            GameObject gameObject = GameObject.Find("Scene");
            if (null != gameObject)
            {
                Light[] componentsInChildren = gameObject.GetComponentsInChildren<Light>();
                int num = ~(1 << Const.Layer.UI | 1 << Const.Layer.CutSceneUI);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].cullingMask = (componentsInChildren[i].cullingMask & num);
                }
            }
            if (RenderSettings.fog)
            {
                Shader.DisableKeyword("FogOn");
            }
            else
            {
                Shader.DisableKeyword("FogOn");
            }
        }

        public void SetFogAndLight()
        {
            FogAndLightCenter fogAndLightCenter = UnityEngine.Object.FindObjectOfType<FogAndLightCenter>();
            if (fogAndLightCenter != null)
            {
                MainPlayer mainPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer;
                GameObject gameObject = GameObject.Find("Scene");
                Light mainLight = null;
                if (null != gameObject && gameObject.FindChild("ZoneRoot_0/NonStatic_0/DynamicLight") != null)
                {
                    mainLight = gameObject.FindChild("ZoneRoot_0/NonStatic_0/DynamicLight").GetComponent<Light>();
                }
                GameObject skybox = GameObject.Find("P_com_fog_02");
                GameObject modelObj = mainPlayer.ModelObj;
                Transform transform = mainPlayer.ModelObj.transform;
                fogAndLightCenter.InitData(mainPlayer.ModelObj.transform, mainLight, skybox);
            }
        }

        public const int nInstantiateMaxCount = 20;

        public const int nHighPriority = 3;

        private const int nZoneNodeCount = 3;

        private ResourceManager m_resourceManager;

        private AssetManager _assetManager;

        public SceneInfo sceneInfo;

        private T4MObjSC t4m;

        private List<Action> tempOnLoadActions = new List<Action>();

        private List<Action> onLoadSceneActions = new List<Action>();

        private readonly int m_block_horizontal = 1;

        private readonly int m_block_vertical = 1;

        private int m_index_x;

        private int m_index_z;

        private int m_pre_index_x = -1;

        private int m_pre_index_z = -1;

        private BetterDictionary<string, GameObject> m_curTerrainBlockMap = new BetterDictionary<string, GameObject>();

        private Queue<string> m_BlockRequestQueue = new Queue<string>();

        private Transform m_SceneRoot;

        public PathFinder PathFinder;

        public mapinfo CurrentSceneData;

        private int _currentLineID = -1;

        public bool bSameMapID;

        public bool bSameBlock;

        public bool bSameHight;

        public bool bSameScene;

        public bool bSameCopyBlock;

        private int nCurrentZoneID;

        private int nPreZoneID;

        private List<bool> lstLoadZoneID = new List<bool>();

        private List<bool> lstLoadedZoneID = new List<bool>();

        public SceneData sceneData = new SceneData();

        private SimpleTaskQueue STQueueScene = new SimpleTaskQueue();

        private SimpleTaskQueue STQueueHigh = new SimpleTaskQueue();

        private List<SimpleTaskQueue> lstSTQueueLow = new List<SimpleTaskQueue>();

        private List<Transform> lstTranZone = new List<Transform>();

        public SceneLoadState sls;

        private Camera defaultCamera;

        private string strSceneName = string.Empty;

        private string strBlockName = string.Empty;

        private string strCopyBlockBlockName = string.Empty;

        private string strCopyMeshFindName = string.Empty;

        private string strHightName = string.Empty;

        private int nLowWorkCompletedCount;

        private bool bLoadHighPrefabAllCompleted;

        private bool bLoadLowCompleted;

        private bool bLoadZone;

        private int nLoadCompleted;

        private int nIndex;

        private int nSTQCount;

        private int nLowTotalCount;

        private BetterDictionary<string, Queue<ObjInfo>> dicHighObjInfo = new BetterDictionary<string, Queue<ObjInfo>>();

        private BetterDictionary<string, Queue<ObjInfo>> dicLowObjInfo = new BetterDictionary<string, Queue<ObjInfo>>();

        private Action LoadAllHightPrefabCompelted;

        private SimpleTaskQueue stq = new SimpleTaskQueue();

        private TerrainRegionInfo triAllScene = ScriptableObject.CreateInstance<TerrainRegionInfo>();

        private TerrainRegionInfo triNineBlock = ScriptableObject.CreateInstance<TerrainRegionInfo>();

        private TerrainRegionInfo triLoad = ScriptableObject.CreateInstance<TerrainRegionInfo>();

        private List<Transform> listBatchZone = new List<Transform>();

        public LightInfo LightInfo;

        public Light Light;

        public List<AreaInfo> mMapDisDatas;

        public MapDisAreaIdentifier[] mDisObjs;

        public List<Material> disAppearObjList = new List<Material>();

        public List<Material> appearObjList = new List<Material>();

        private float disAppearSpeed = 2f;

        private GameObject haloGo;

        private bool bFxProDoF;

        private Transform stoppoints;

        private Dictionary<TileFlag, Color> dicColor = new Dictionary<TileFlag, Color>
        {
            {
                TileFlag.TILE_BLOCK_NORMAL,
                Color.red
            },
            {
                TileFlag.TILE_BLOCK_MAGIC,
                Color.green
            },
            {
                TileFlag.TILE_BLOCK_ENTRY,
                Color.blue
            },
            {
                TileFlag.TILE_BLOCK_BUILDING,
                Color.yellow
            }
        };
    }
}

public class InfoCompare : IComparer<ObjInfo>
{
    public int Compare(ObjInfo info1, ObjInfo info2)
    {
        if (info1 == null)
        {
            if (info2 == null)
            {
                return 0;
            }
            return 1;
        }
        else
        {
            if (info2 == null)
            {
                return -1;
            }
            if (info1._nPriority == info2._nPriority)
            {
                return 0;
            }
            return (info1._nPriority <= info2._nPriority) ? 1 : -1;
        }
    }
}
