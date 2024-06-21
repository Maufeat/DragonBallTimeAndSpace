using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;
using quest;
using UnityEngine;

public class TaskController : ControllerBase
{
    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ShowCutScene));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.ExecuteQuest));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.ExecuteQuestUseLastNpcId));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.PathFindToNpcAndVisit));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.CoolDownStart));
        this.taskNetWork = new TaskNetWork();
        this.taskNetWork.Initialize();
        this.autoTask = new TaskAuto();
        this.autoTask.Initialize();
    }

    public override void OnUpdate()
    {
        this.autoTask.Update();
    }

    public override string ControllerName
    {
        get
        {
            return "task_controller";
        }
    }

    public void ExecuteQuest(List<VarType> paras)
    {
        if (paras.Count == 3)
        {
            this.taskNetWork.ReqExecuteQuest(paras[1], paras[0], paras[2], 0U, 0UL, false);
            return;
        }
        if (paras.Count == 4)
        {
            this.taskNetWork.ReqExecuteQuest(paras[1], paras[0], paras[2], 0U, paras[3], false);
            return;
        }
        FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
    }

    public void TryGetQuestNPCID(ref ulong chartarget, bool isUseNpcID)
    {
        if (isUseNpcID)
        {
            chartarget = this.lastVisitNpcId;
        }
        else if (MainPlayer.Self != null)
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component != null && component.TargetCharactor != null)
            {
                Npc npc = component.TargetCharactor as Npc;
                if (npc != null)
                {
                    chartarget = npc.EID.Id;
                }
                OtherPlayer otherPlayer = component.TargetCharactor as OtherPlayer;
                if (otherPlayer != null)
                {
                    chartarget = otherPlayer.EID.Id;
                }
            }
        }
    }

    public void OnCutSceneFinish(string param)
    {
        if (!string.IsNullOrEmpty(param))
        {
            string[] array = param.Split(new char[]
            {
                ','
            });
            if (array != null && array.Length >= 3)
            {
                this.taskNetWork.ReqExecuteQuest(uint.Parse(array[1]), array[0], uint.Parse(array[2]), 0U, 0UL, false);
            }
        }
    }

    public void ExecuteQuestUseLastNpcId(List<VarType> paras)
    {
        if (paras.Count != 3)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.taskNetWork.ReqExecuteQuest(paras[1], paras[0], paras[2], 0U, 0UL, true);
    }

    public void Luafun_ShowCutScene(List<VarType> paras)
    {
        if (paras.Count <= 1)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.CancelFollowLeader();
        uint num = 0U;
        uint.TryParse(paras[0].ToString(), out num);
        if (num > 0U && this.CheckCanPlayCutScene(num))
        {
            this.ShowCutSceneByID(num, delegate
            {
                FFDebug.Log(this, FFLogType.CutScene, string.Concat(new string[]
                {
                    "Luafun_ShowCutScene callback ---------- params: ",
                    paras[2],
                    ",",
                    paras[1],
                    ",",
                    paras[3]
                }));
                this.taskNetWork.ReqExecuteQuest(paras[2], paras[1], paras[3], 0U, 0UL, false);
            });
        }
    }

    public void CancelFollowLeader()
    {
        if (MainPlayer.Self != null)
        {
            AutoAttack component = MainPlayer.Self.GetComponent<AutoAttack>();
            if (component != null && component.AutoAttackOn)
            {
                component.SwitchModle(false);
            }
            AttactFollowTeamLeader component2 = MainPlayer.Self.GetComponent<AttactFollowTeamLeader>();
            if (component2 != null && component2.AutoAttackOn)
            {
                component2.SwitchModle(false);
            }
        }
    }

    public bool CheckCanPlayCutScene(uint id)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("cutscene", (ulong)id);
        if (configTable == null)
        {
            return false;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager == null)
        {
            return false;
        }
        int cacheField_Int = configTable.GetCacheField_Int("mapip");
        return cacheField_Int == 0 || (long)cacheField_Int == (long)((ulong)manager.sceneInfo.mapid);
    }

    public void ShowCutSceneByID(uint id, Action callback)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("cutscene", (ulong)id);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "ShowCutSceneByID cant find cutscene config !!!, id = " + id);
            if (callback != null)
            {
                callback();
            }
        }
        TaskController.CutSceneShowType cacheField_Int = (TaskController.CutSceneShowType)configTable.GetCacheField_Int("show_type");
        TaskController.CutSceneShowType cutSceneShowType = cacheField_Int;
        if (cutSceneShowType != TaskController.CutSceneShowType.CG)
        {
            if (cutSceneShowType == TaskController.CutSceneShowType.Assets)
            {
                this.ShowCutSceneByAssets(id, configTable, callback);
            }
        }
        else
        {
            this.ShowCutSceneByCG(id, configTable, callback);
        }
    }

    private void ShowCutSceneByCG(uint id, LuaTable config, Action callback)
    {
        if (this._cutSceneMgr == null)
        {
            this._cutSceneMgr = ManagerCenter.Instance.GetManager<CutSceneManager>();
        }
        string cacheField_String = config.GetCacheField_String("cutscene_key");
        if (!string.IsNullOrEmpty(cacheField_String))
        {
            this._cutSceneMgr.CurCutscene = cacheField_String;
            SingletonForMono<PlayMovieManager>.Instance.OnPlayMovieComplete = null;
            SingletonForMono<PlayMovieManager>.Instance.OnPlayMovieComplete = delegate ()
            {
                this._cutSceneMgr.CurCutscene = string.Empty;
                if (callback != null)
                {
                    callback();
                }
            };
            SingletonForMono<PlayMovieManager>.Instance.PlayVideo(cacheField_String);
        }
        else if (callback != null)
        {
            callback();
        }
    }

    private void ShowCutSceneByAssets(uint id, LuaTable config, Action callback)
    {
        if (this._cutSceneMgr == null)
        {
            this._cutSceneMgr = ManagerCenter.Instance.GetManager<CutSceneManager>();
        }
        string cacheField_String = config.GetCacheField_String("cutscene_key");
        try
        {
            this._cutSceneMgr.GetCutSceneContent(cacheField_String, delegate (CutSceneContent content)
            {
                if (content != null)
                {
                    content.PlaybackFinished = delegate ()
                    {
                        FFDebug.Log(this, FFLogType.CutScene, "ShowCutSceneByAssets complete ---------- key = : " + content.Key);
                        if (callback != null)
                        {
                            callback();
                        }
                    };
                    if (content.LoadAssets != null)
                    {
                        content.LoadAssets();
                    }
                }
                else
                {
                    callback();
                }
            });
        }
        catch (Exception ex)
        {
            FFDebug.LogWarning(this, "ShowCutSceneByAssets error !!!" + ex.Message);
            if (callback != null)
            {
                callback();
            }
        }
    }

    public void CoolDownStart(List<VarType> paras)
    {
        int second = 3;
        if (paras.Count > 0)
        {
            second = paras[0];
        }
        ControllerManager.Instance.GetController<CoolDownController>().OpenUI(second);
    }

    public void PathFindToNpcAndVisit(List<VarType> paras)
    {
        if (paras.Count != 4)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        uint sceneid = paras[0];
        uint npcid = paras[1];
        float posx = paras[2];
        float posy = paras[3];
        this.PathFindToNpcAndVisit(sceneid, npcid, posx, posy);
    }

    public void PathFindToNpcAndVisitByBag(LuaTable pathwayCfg)
    {
        uint field_Uint = pathwayCfg.GetField_Uint("mapid");
        uint field_Uint2 = pathwayCfg.GetField_Uint("npcid");
        uint field_Uint3 = pathwayCfg.GetField_Uint("pathwayid");
        this.wantobject = pathwayCfg.GetField_String("wantobject");
        Vector2 vector2WithString = ClassPlus.GetVector2WithString(pathwayCfg.GetField_String("coordinates"));
        this.PathFindToNpcAndVisit(field_Uint, field_Uint2, vector2WithString.x, vector2WithString.y);
    }

    public void PathFindToNpcAndVisit(uint sceneid, uint npcid, float posx, float posy)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID() != sceneid)
        {
            FFDebug.Log(this, FFLogType.Task, string.Concat(new object[]
            {
                "Map id not match  ",
                ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID(),
                "   ",
                sceneid
            }));
            return;
        }
        MainPlayer.Self.Pfc.PathFindToNpc(npcid, posx, posy, delegate
        {
            Npc nearestVisitNpc = MainPlayer.Self.GetComponent<FFDetectionNpcControl>().GetNearestVisitNpc();
            MainPlayer.Self.GetComponent<FFDetectionNpcControl>().PriorityVisiteNPCID = 0UL;
            if (nearestVisitNpc == null)
            {
                FFDebug.Log(this, FFLogType.Task, "PathFindToNpcAndVisit npc == null ");
                return;
            }
            if (nearestVisitNpc.NpcData.MapNpcData.baseid != npcid)
            {
                FFDebug.Log(this, FFLogType.Task, "PathFindToNpcAndVisit npc == null " + npcid);
                return;
            }
            this.ReqVisteNpc(nearestVisitNpc.NpcData.MapNpcData.tempid);
            nearestVisitNpc.CloseTopBtn(true);
        }, delegate
        {
            MainPlayer.Self.GetComponent<FFDetectionNpcControl>().PriorityVisiteNPCID = 0UL;
        });
    }

    public void ReqVisteNpc(ulong npcid)
    {
        this.lastVisitNpcId = npcid;
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component.PriorityVisiteNPCID == npcid)
        {
            component.PriorityVisiteNPCID = 0UL;
        }
        CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(npcid, CharactorType.NPC);
        if (charactorByID != null)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().manualSelect.SetTarget(charactorByID, false, true);
        }
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel("UI_NPCtalkAndTaskDlg");
        if (luaUIPanel != null)
        {
            LuaScriptMgr.Instance.CallLuaFunction("UI_NPCtalkAndTaskDlg.ClearOldTalkContent", new object[]
            {
                Util.GetLuaTable("UI_NPCtalkAndTaskDlg")
            });
        }
        this.taskNetWork.ReqVisitNpcTrade(npcid);
    }

    public void ReqMapQuestInfo(Action callback)
    {
        this.taskNetWork.ReqMapQuestInfo();
        this._ReqMapQuestInfoCallback = callback;
    }

    public void InitNpcTaskMap(List<npcQuestList> QuestInfoList)
    {
        this.NpcTaskMap.Clear();
        FFDebug.Log(this, FFLogType.Task, "InitNpcTaskMap----------->" + QuestInfoList.Count);
        for (int i = 0; i < QuestInfoList.Count; i++)
        {
            npcQuestList npcQuestList = QuestInfoList[i];
            if (!this.NpcTaskMap.ContainsKey(npcQuestList.npcid))
            {
                this.NpcTaskMap[npcQuestList.npcid] = new NpcTask();
                this.NpcTaskMap[npcQuestList.npcid].Eid = npcQuestList.npcid;
            }
            NpcTask npcTask = this.NpcTaskMap[npcQuestList.npcid];
            npcTask.RefreshData(npcQuestList);
        }
        this.ShowAllNpcTaskInMap();
        if (this._ReqMapQuestInfoCallback != null)
        {
            this._ReqMapQuestInfoCallback();
            this._ReqMapQuestInfoCallback = null;
        }
    }

    public void RefreshTaskState(TaskInfoRefreshData ServerData)
    {
        this.NpcTaskMap.BetterForeach(delegate (KeyValuePair<uint, NpcTask> item)
        {
            item.Value.RefreshData(ServerData, false);
        });
    }

    public void acceptTaskInfoToNpcTask(List<npcValidQuest> newaccept)
    {
    }

    public void ShowAllNpcTaskInMap()
    {
        UIMapController MapController = ControllerManager.Instance.GetController<UIMapController>();
        if (MapController == null)
        {
            return;
        }
        EntitiesManager EntitiesMgr = ManagerCenter.Instance.GetManager<EntitiesManager>();
        this.NpcTaskMap.BetterForeach(delegate (KeyValuePair<uint, NpcTask> item)
        {
            NpcTask value = item.Value;
            Npc[] npcsByBaseidInFun = EntitiesMgr.GetNpcsByBaseidInFun(value.Eid);
            TaskInfo firstShowTask = value.GetFirstShowTask();
            foreach (Npc npc in npcsByBaseidInFun)
            {
                MapController.SetNpcIconInfoByTask(npc, firstShowTask);
            }
        });
        if (MapController.MapUI != null)
        {
            MapController.MapUI.InitNpcBtnList();
        }
    }

    private void LogOutALLNpcTask()
    {
        string log = string.Empty;
        this.NpcTaskMap.BetterForeach(delegate (KeyValuePair<uint, NpcTask> item)
        {
            log = string.Concat(new object[]
            {
                log,
                "\n--",
                item.Value.Eid,
                "--->"
            });
            item.Value.TaskInfoMap.BetterForeach(delegate (KeyValuePair<uint, TaskInfo> TaskInfo)
            {
                string log2 = log;
                log = string.Concat(new object[]
                {
                    log2,
                    "\n",
                    TaskInfo.Value.questid,
                    "---",
                    TaskInfo.Value.stateNum
                });
            });
        });
        FFDebug.LogWarning(this, log);
    }

    public int GetShowPriorityByBaseID(uint baseID)
    {
        if (this.NpcTaskMap == null)
        {
            return -1;
        }
        if (this.NpcTaskMap.ContainsKey(baseID))
        {
            NpcTask npcTask = this.NpcTaskMap[baseID];
            TaskInfo firstShowTask = npcTask.GetFirstShowTask();
            if (firstShowTask != null)
            {
                return firstShowTask.ShowPriority;
            }
        }
        return 0;
    }

    public void OnRetOnStageStart(string npcidStr, bool isShowName, bool isShowOnStage)
    {
        if (!isShowOnStage)
        {
            return;
        }
        ulong num = 0UL;
        ulong.TryParse(npcidStr, out num);
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().GetNpc(num) == null)
        {
            FFDebug.LogWarning(this, "OnRetOnStageStart can't find this npc uid = " + num);
            return;
        }
        if (CameraController.Self != null)
        {
        }
    }

    private void OnStageEnd()
    {
        this.taskNetWork.ReqOnStageEnd();
    }

    public void OnRetDirChange()
    {
        if (CameraController.Self != null)
        {
            Scheduler.Instance.AddFrame(3U, false, delegate
            {
                CameraController.Self.Reste3DCamera();
            });
        }
    }

    public TaskNetWork taskNetWork;

    public BetterDictionary<uint, NpcTask> NpcTaskMap = new BetterDictionary<uint, NpcTask>();

    public TaskAuto autoTask;

    public ulong lastVisitNpcId;

    public static UI_NpcDlg CurrnetNpcDlg;

    public static UI_NpcDlg CurrnetDramaTips;

    public static uint CurNpcDlgSource;

    private CutSceneManager _cutSceneMgr;

    public string wantobject;

    private Action _ReqMapQuestInfoCallback;

    private enum CutSceneShowType
    {
        CG,
        Assets
    }
}
