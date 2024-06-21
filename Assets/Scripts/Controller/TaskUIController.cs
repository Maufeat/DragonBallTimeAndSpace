using System;
using System.Collections.Generic;
using System.Xml;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;
using msg;
using quest;
using UnityEngine;

public class TaskUIController : ControllerBase
{
    private MainUIController mainuicontroller
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    public TaskController taskController
    {
        get
        {
            return ControllerManager.Instance.GetController<TaskController>();
        }
    }

    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.openProgressBar));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.openTaskNoteAccept));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.openTaskDoing));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.openTaskFinish));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.showProBarClient));
        this.taskUINetWork = new TaskUINetWork();
        this.taskUINetWork.Initialize();
    }

    public void openTaskNoteAccept(List<VarType> paras)
    {
        this.openTaskByState(paras);
    }

    public void openTaskDoing(List<VarType> paras)
    {
        this.openTaskByState(paras);
    }

    public void openTaskFinish(List<VarType> paras)
    {
        this.openTaskByState(paras);
    }

    private void openTaskByState(List<VarType> paras)
    {
        string text = paras[0].ToString();
        string text2 = paras[1].ToString();
        string text3 = paras[2].ToString();
        string text4 = "0";
        string text5 = string.Concat(new string[]
        {
            text,
            "|",
            text2,
            "|",
            text3,
            "|",
            text4
        });
        LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.OpenTaskDialogByState", new object[]
        {
            text5
        });
    }

    public void openProgressBar(List<VarType> paras)
    {
        if (paras == null || paras.Count < 8)
        {
            FFDebug.LogWarning(this, string.Format("Invalid arguments to method,equire 8 paras,count = {0} ", paras.Count));
            return;
        }
        this._nextTaskInfo = new NextTaskInfo();
        this._nextTaskInfo.npcID = paras[0];
        this._nextTaskInfo.buffAnim = paras[1];
        this._nextTaskInfo.revertAnim = paras[2];
        this._nextTaskInfo.strIcon = paras[3];
        this._nextTaskInfo.strInfo = paras[4];
        this._nextTaskInfo.strNextTarget = paras[5];
        this._nextTaskInfo.nextSwitchID = paras[6];
        this._nextTaskInfo.nextOffet = paras[7];
        FFDebug.Log(this, FFLogType.HoldOn, this._nextTaskInfo.ToString());
        OccupyController controller = ControllerManager.Instance.GetController<OccupyController>();
        if (controller != null)
        {
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().SearchNearNPCById((uint)this._nextTaskInfo.npcID);
            if (npc != null)
            {
                this._isQusetBarActive = true;
                ulong npcid = npc.NpcData.MapNpcData.tempid;
                TaskController controller2 = ControllerManager.Instance.GetController<TaskController>();
                if (controller2 != null && controller2.lastVisitNpcId != 0UL)
                {
                    npcid = controller2.lastVisitNpcId;
                }
                controller.ReqHoldon(npcid, 1U, 1U);
            }
            else
            {
                this._isQusetBarActive = false;
            }
            OccupyController occupyController = controller;
            occupyController.OnHoldOnComplete = (Action<bool>)Delegate.Combine(occupyController.OnHoldOnComplete, new Action<bool>(this.ReqNextTask));
        }
        ProgressUIController controller3 = ControllerManager.Instance.GetController<ProgressUIController>();
        if (controller3 != null && !string.IsNullOrEmpty(this._nextTaskInfo.strIcon))
        {
            controller3.StrIcon = this._nextTaskInfo.strIcon;
        }
        if (controller3 != null && !string.IsNullOrEmpty(this._nextTaskInfo.strInfo))
        {
            controller3.StrInfo = this._nextTaskInfo.strInfo;
        }
        PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            component.CacheBuffAnimInfo(UserState.USTATE_QUESTBAR, this._nextTaskInfo.buffAnim, this._nextTaskInfo.revertAnim);
        }
    }

    public void showProBarClient(List<VarType> paras)
    {
        if (paras == null || paras.Count < 7)
        {
            FFDebug.LogWarning(this, string.Format("Invalid arguments to method,equire 7 paras,count = {0} ", paras.Count));
            return;
        }
        if (this._isQusetBarActive)
        {
            return;
        }
        this._nextTaskInfo = new NextTaskInfo();
        this._nextTaskInfo.buffAnim = paras[0];
        this._nextTaskInfo.revertAnim = paras[1];
        this._nextTaskInfo.strIcon = paras[2];
        this._nextTaskInfo.strInfo = paras[3];
        this._nextTaskInfo.strNextTarget = paras[4];
        this._nextTaskInfo.nextSwitchID = paras[5];
        this._nextTaskInfo.nextOffet = paras[6];
        FFDebug.Log(this, FFLogType.HoldOn, this._nextTaskInfo.ToString());
        OccupyController occupyCtrl = ControllerManager.Instance.GetController<OccupyController>();
        if (occupyCtrl != null)
        {
            ControllerManager.Instance.GetController<ProgressUIController>().ShowProgressBar(3f, delegate ()
            {
                occupyCtrl.HoldOnComplete(0U);
            });
            this.taskController.lastVisitNpcId = 0UL;
            OccupyController occupyCtrl2 = occupyCtrl;
            occupyCtrl2.OnHoldOnComplete = (Action<bool>)Delegate.Combine(occupyCtrl2.OnHoldOnComplete, new Action<bool>(this.ReqNextTask));
        }
        ProgressUIController controller = ControllerManager.Instance.GetController<ProgressUIController>();
        if (controller != null && !string.IsNullOrEmpty(this._nextTaskInfo.strIcon))
        {
            controller.StrIcon = this._nextTaskInfo.strIcon;
        }
        if (controller != null && !string.IsNullOrEmpty(this._nextTaskInfo.strInfo))
        {
            controller.StrInfo = this._nextTaskInfo.strInfo;
        }
        PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            component.CacheBuffAnimInfo(UserState.USTATE_QUESTBAR, this._nextTaskInfo.buffAnim, this._nextTaskInfo.revertAnim);
        }
        PlayerBufferControl component2 = MainPlayer.Self.GetComponent<PlayerBufferControl>();
        if (component2 != null)
        {
            component2.AddStateByLocal(new StateItem
            {
                lasttime = 3000UL,
                overtime = SingletonForMono<GameTime>.Instance.GetCurrServerTime() + 3000UL,
                settime = SingletonForMono<GameTime>.Instance.GetCurrServerTime(),
                uniqid = CommonTools.GenernateBuffHash(MainPlayer.Self.EID, 30UL, 0UL)
            });
        }
    }

    private void ReqNextTask(bool success)
    {
        OccupyController controller = ControllerManager.Instance.GetController<OccupyController>();
        if (controller != null)
        {
            OccupyController occupyController = controller;
            occupyController.OnHoldOnComplete = (Action<bool>)Delegate.Remove(occupyController.OnHoldOnComplete, new Action<bool>(this.ReqNextTask));
        }
        this._isQusetBarActive = false;
        if (!success)
        {
            return;
        }
        if (this._nextTaskInfo == null)
        {
            return;
        }
        if (this._nextTaskInfo.nextSwitchID <= 0 || this._nextTaskInfo.nextOffet <= 0 || string.IsNullOrEmpty(this._nextTaskInfo.strNextTarget))
        {
            return;
        }
        TaskController controller2 = ControllerManager.Instance.GetController<TaskController>();
        if (controller2 == null || controller2.taskNetWork == null)
        {
            return;
        }
        controller2.taskNetWork.ReqExecuteQuest((uint)this._nextTaskInfo.nextSwitchID, this._nextTaskInfo.strNextTarget, (uint)this._nextTaskInfo.nextOffet, 0U, controller2.lastVisitNpcId, false);
        this._nextTaskInfo = null;
    }

    public bool CheckTaskInTaskListByID(uint id)
    {
        bool result = false;
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.CheckTaskInTaskListByID", new object[]
        {
            Util.GetLuaTable("NpcTalkAndTaskDlgCtrl"),
            id
        });
        if (array != null && array.Length > 0)
        {
            result = (bool)array[0];
        }
        return result;
    }

    public void CkeckCopyTaskList()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<FindDirections>() != null)
        {
            MainPlayer.Self.GetComponent<FindDirections>().Reset();
        }
        LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.CheckCopyTaskList", new object[]
        {
            Util.GetLuaTable("NpcTalkAndTaskDlgCtrl")
        });
    }

    public void PathFinding()
    {
    }

    public void PathFinding(MSG_Ret_QuestInfo_SC questInfo, bool byserver)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("questconfig", (ulong)questInfo.id);
        LuaTable luaTable = null;
        this.dicState.Clear();
        uint num = 7U;
        if (byserver)
        {
            num = configTable.GetField_Uint("pathfindstate");
        }
        if (questInfo.state == 99U)
        {
            if ((num & 4U) != 0U)
            {
                luaTable = LuaConfigManager.GetConfigTable("pathway", (ulong)configTable.GetField_Uint("pathwaypre"));
            }
        }
        else if (questInfo.state != 100U)
        {
            if ((num & 1U) != 0U)
            {
                string[] array = configTable.GetField_String("pathwaydoing").Split(new char[]
                {
                    ';'
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    string[] array2 = array[i].Split(new char[]
                    {
                        '-'
                    }, StringSplitOptions.RemoveEmptyEntries);
                    uint key = (uint)int.Parse(array2[0]);
                    uint value = (uint)int.Parse(array2[1]);
                    this.dicState[key] = value;
                }
                if (this.dicState.ContainsKey(questInfo.state))
                {
                    luaTable = LuaConfigManager.GetConfigTable("pathway", (ulong)this.dicState[questInfo.state]);
                }
                else if (this.dicState.ContainsKey(1U))
                {
                    luaTable = LuaConfigManager.GetConfigTable("pathway", (ulong)this.dicState[1U]);
                }
                else
                {
                    luaTable = null;
                }
            }
        }
        else if ((num & 2U) != 0U)
        {
            luaTable = LuaConfigManager.GetConfigTable("pathway", (ulong)configTable.GetField_Uint("pathwaydone"));
        }
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData == null)
        {
            FFDebug.Log(this, FFLogType.Task, "current   scene  data is  null ");
            return;
        }
        if (luaTable == null)
        {
            FFDebug.Log(this, FFLogType.Task, "PathConfig   is  null");
            return;
        }
        this.taskController.autoTask.Start(questInfo.id, luaTable.GetField_Uint("pathwayid"));
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID() == luaTable.GetField_Uint("mapid"))
        {
            this.FindPath(luaTable.GetField_Uint("pathwayid"), delegate
            {
                this.taskController.autoTask.EnterState(questInfo.id, TaskAuto.E_Type.Seek);
            });
        }
        else
        {
            this.taskUINetWork.ReqChangeMapFindPath(luaTable.GetField_Uint("pathwayid"), questInfo.id);
        }
    }

    public void FindPathInterface(uint pathwayid, Action callback)
    {
        LuaTable pathFindCfg = LuaConfigManager.GetConfigTable("pathway", (ulong)pathwayid);
        if (pathFindCfg == null)
        {
            FFDebug.Log(this, FFLogType.Task, "PathConfig   is  null");
            return;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        uint currMapid = manager.CurrentSceneData.mapID();
        uint destMapID = pathFindCfg.GetField_Uint("mapid");
        if (this.IsNeedSpecielMapTip(currMapid, destMapID))
        {
            return;
        }
        if (manager.sceneInfo != null && manager.sceneInfo.copymapid != 0U)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("copymapinfo", (ulong)manager.sceneInfo.copymapid);
            uint cacheField_Uint = configTable.GetCacheField_Uint("mapid");
            if (destMapID != cacheField_Uint)
            {
                TipsWindow.ShowNotice(897U);
                return;
            }
        }
        if (manager.CurrentSceneData.mapID() == destMapID)
        {
            uint field_Uint = pathFindCfg.GetField_Uint("pathwaynull");
            if (field_Uint != 0U)
            {
                TipsWindow.ShowNotice(837U);
                return;
            }
            this.FindPath(pathFindCfg.GetField_Uint("pathwayid"), callback);
        }
        else
        {
            Vector2 neareastTeleportPoint = Vector2.zero;
            uint field_Uint2 = pathFindCfg.GetField_Uint("iscopymap");
            bool flag = field_Uint2 == 1U;
            if (flag)
            {
                TipsWindow.ShowNotice(839U);
                return;
            }
            if (this.TryGetNeareastTeleportPoint(currMapid, out neareastTeleportPoint))
            {
                MainPlayer.Self.Pfc.PathFindOfDeviation(neareastTeleportPoint, delegate
                {
                    Vector2 zero = Vector2.zero;
                    if (!this.TryGetNeareastTeleportPoint(destMapID, out zero))
                    {
                        if (destMapID == 702U && currMapid != 689U && currMapid != 702U)
                        {
                            TipsWindow.ShowNotice(856U);
                        }
                        else
                        {
                            TipsWindow.ShowNotice(22U);
                        }
                    }
                    else
                    {
                        float num = Vector2.Distance(neareastTeleportPoint, MainPlayer.Self.NextPosition2D);
                        if (num < 10f)
                        {
                            this.taskUINetWork.ReqChangeMapFindPath(pathFindCfg.GetField_Uint("pathwayid"), 0U);
                        }
                    }
                });
            }
            else
            {
                TipsWindow.ShowNotice(23U);
            }
        }
    }

    private bool IsNeedSpecielMapTip(uint curMapID, uint desMapID)
    {
        if (curMapID == 689U && desMapID == 702U)
        {
            TipsWindow.ShowNotice(855U);
            return true;
        }
        if (curMapID == 702U && desMapID == 689U)
        {
            TipsWindow.ShowNotice(857U);
            return true;
        }
        return false;
    }

    private bool TryGetNeareastTeleportPoint(uint mapID, out Vector2 serverPoint)
    {
        serverPoint = Vector2.zero;
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("scenesinfo").GetCacheField_Table("mapinfo");
        LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(mapID.ToString());
        string field_String = cacheField_Table2.GetField_String("xmlName");
        string url = Application.streamingAssetsPath + "/Scenes/MapData/" + field_String + ".xml";
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(XmlReader.Create(url, new XmlReaderSettings
        {
            IgnoreComments = true
        }));
        XmlNodeList xmlNodeList = xmlDocument.SelectNodes("map/zonedef");
        bool result = false;
        float num = float.MaxValue;
        if (xmlNodeList != null)
        {
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement xmlElement = xmlNodeList[i] as XmlElement;
                if ("64".Equals(xmlElement.GetAttribute("type").ToString()))
                {
                    Vector2 vector = new Vector2(float.Parse(xmlElement.GetAttribute("x").ToString()), float.Parse(xmlElement.GetAttribute("y").ToString()));
                    float num2 = Vector2.Distance(vector, MainPlayer.Self.NextPosition2D);
                    if (num2 < num)
                    {
                        serverPoint = vector;
                        num = num2;
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    public void FindPath(uint pathwayid, Action callback)
    {
        LuaTable pathFindCfg = LuaConfigManager.GetConfigTable("pathway", (ulong)pathwayid);
        string field_String = pathFindCfg.GetField_String("coordinates");
        string[] array = field_String.Split(new char[]
        {
            ','
        });
        if (array.Length < 2 || string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
        {
            FFDebug.LogWarning(this, "没有寻路坐标!");
            return;
        }
        float x = (float)int.Parse(array[0]);
        float y = (float)int.Parse(array[1]);
        if (pathFindCfg != null)
        {
            if (MainPlayer.Self != null && MainPlayer.Self.Pfc != null)
            {
                uint field_Uint = pathFindCfg.GetField_Uint("propid");
                if (field_Uint == 0U)
                {
                    PathFindComponent.CurrentPathFindNpc = pathFindCfg.GetField_Uint("npcid");
                    Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().SearchNearNPCById(pathFindCfg.GetField_Uint("npcid"), -1f);
                    if (npc != null)
                    {
                        Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(npc.ModelObj.transform.position, true);
                        x = serverPosByWorldPos.x;
                        y = serverPosByWorldPos.y;
                    }
                    MainPlayer.Self.Pfc.PathFindOfDeviation(new Vector2(x, y), delegate
                    {
                        this.mainuicontroller.VisiteNPC(pathFindCfg.GetField_Uint("npcid"));
                        if (callback != null)
                        {
                            callback();
                        }
                    });
                }
                else
                {
                    PropsBase otherActionDat = null;
                    otherActionDat = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
                    {
                        Util.GetLuaTable("BagCtrl"),
                        field_Uint
                    })[0];
                    MainPlayer.Self.Pfc.PathFindOfDeviation(new Vector2(x, y), delegate
                    {
                        if (otherActionDat != null)
                        {
                            ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
                            if (controller != null)
                            {
                                controller.ShowUseProp(otherActionDat);
                            }
                        }
                        if (callback != null)
                        {
                            callback();
                        }
                    });
                }
            }
        }
        else
        {
            FFDebug.LogWarning(this, "    pathcfg    is  null");
        }
    }

    public void UsePropInTask(uint baseid)
    {
        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            baseid
        })[0];
        if (propsBase != null)
        {
            ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
            if (controller != null)
            {
                controller.ShowUseProp(propsBase);
            }
        }
    }

    public void CacheTaskTrackItem(GameObject itemRoot, uint questID, int finishState, string degreeVar, uint curDegree, uint maxDegree)
    {
        TaskUIController.TaskTrackItemCache item = new TaskUIController.TaskTrackItemCache(itemRoot, questID, finishState, degreeVar, curDegree, maxDegree);
        this.taskTrackItem.Enqueue(item);
    }

    public override void OnUpdate()
    {
        if (this.taskTrackItem.Count > 0)
        {
            TaskUIController.TaskTrackItemCache taskTrackItemCache = this.taskTrackItem.Dequeue();
            taskTrackItemCache.InitAll();
            if (this.taskTrackItem.Count == 0)
            {
                Canvas.ForceUpdateCanvases();
            }
        }
    }

    public override string ControllerName
    {
        get
        {
            return "taskui_controller";
        }
    }

    public void OpenTaskView()
    {
        LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.OpenTaskList", new object[]
        {
            Util.GetLuaTable("NpcTalkAndTaskDlgCtrl")
        });
    }

    public void TryEnterAutoTaskState(uint taskID, uint state)
    {
        if (taskID == this.taskController.autoTask.CurTaskId)
        {
            if (state >= 100U)
            {
                this.taskController.autoTask.EnterState(taskID, TaskAuto.E_Type.Complete);
            }
            else
            {
                this.taskController.autoTask.EnterState(taskID, TaskAuto.E_Type.Seek);
            }
        }
    }

    public void RefreshTaskInfoInMainUI()
    {
        ControllerManager.Instance.GetController<MainUIController>().RefreshTaskInfo();
    }

    public void PathFindByIDAndState(uint id, uint state, bool byserver)
    {
        this.PathFinding(new MSG_Ret_QuestInfo_SC
        {
            id = id,
            state = state
        }, byserver);
    }

    public int GetListTaskCount()
    {
        int result = 0;
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.GetListTaskCount", new object[]
        {
            Util.GetLuaTable("NpcTalkAndTaskDlgCtrl")
        });
        if (array != null && array.Length > 0)
        {
            result = int.Parse(array[0].ToString());
        }
        return result;
    }

    public FirstCopyInfo GetFirstCopyTask()
    {
        this._info.Clear();
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.GetFisrtCopyTask", new object[]
        {
            Util.GetLuaTable("NpcTalkAndTaskDlgCtrl")
        });
        LuaTable luaTable = array[0] as LuaTable;
        this._info.id = luaTable.GetField_Uint("id");
        this._info.isEmpty = (luaTable.GetField_Int("isempty") == 0);
        this._info.state = luaTable.GetField_Uint("state");
        return this._info;
    }

    public TaskUINetWork taskUINetWork;

    private bool _isQusetBarActive;

    private NextTaskInfo _nextTaskInfo;

    private Dictionary<uint, uint> dicState = new Dictionary<uint, uint>();

    private Queue<TaskUIController.TaskTrackItemCache> taskTrackItem = new Queue<TaskUIController.TaskTrackItemCache>();

    private FirstCopyInfo _info = default(FirstCopyInfo);

    private class TaskTrackItemCache
    {
        public TaskTrackItemCache(GameObject itemRoot, uint questID, int finishState, string degreeVar, uint curDegree, uint maxDegree)
        {
            this.itemRoot = itemRoot;
            this.questID = questID;
            this.finishState = finishState;
            this.degreeVar = degreeVar;
            this.curDegree = curDegree;
            this.maxDegree = maxDegree;
        }

        private void MatchReg()
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("questconfig", (ulong)this.questID);
            if (configTable == null)
            {
                this.state = TaskUIController.TaskTrackMatchState.MatchFail;
                return;
            }
            WayFindItem wayFindItem;
            if (this.finishState == 0)
            {
                wayFindItem = GlobalRegister.GetWayFindItemByStr(configTable.GetCacheField_String("predesc"));
            }
            else if (this.finishState == 1)
            {
                wayFindItem = GlobalRegister.GetWayFindItemByQuestID(configTable, this.degreeVar);
            }
            else
            {
                wayFindItem = GlobalRegister.GetWayFindItemByStr(configTable.GetCacheField_String("commitdesc"));
            }
            if (wayFindItem != null)
            {
                this.item = wayFindItem;
                this.state = TaskUIController.TaskTrackMatchState.Matched;
            }
            else
            {
                this.state = TaskUIController.TaskTrackMatchState.MatchFail;
            }
        }

        public void Init()
        {
            if (this.itemRoot != null)
            {
                this.item.InitUI(this.itemRoot, this.questID, this.finishState, this.degreeVar, this.curDegree, this.maxDegree);
            }
        }

        public void InitAll()
        {
            this.MatchReg();
            this.Init();
        }

        public GameObject itemRoot;

        public uint questID;

        public int finishState;

        public string degreeVar;

        public uint curDegree;

        public uint maxDegree;

        private WayFindItem item;

        public TaskUIController.TaskTrackMatchState state;
    }

    private enum TaskTrackMatchState
    {
        NotMatch,
        Matched,
        MatchFail
    }
}
