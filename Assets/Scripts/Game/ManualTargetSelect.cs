using System;
using Engine;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class ManualTargetSelect : ISelectTarget
{
    private MainPlayerTargetSelectMgr selectMgr
    {
        get
        {
            if (this.selectMgr_ == null && MainPlayer.Self != null)
            {
                this.selectMgr_ = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            }
            return this.selectMgr_;
        }
        set
        {
            this.selectMgr_ = value;
        }
    }

    public void Init()
    {
        this.entitiesManager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        SingletonForMono<InputController>.Instance.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.OnClickEvent));
        Scheduler.Instance.AddFrame(5U, true, new Scheduler.OnScheduler(this.CheckDistToTarget4CtrlTaskUI));
    }

    public void Dispose()
    {
        this.selectMgr = null;
        this.entitiesManager = null;
        SingletonForMono<InputController>.Instance.mScreenEventController.RemoveListener(new ScreenEventController.OnScreenEvent(this.OnClickEvent));
    }

    public bool CheckLegal(CharactorBase charactor, bool ignoredeath = false)
    {
        bool flag = false;
        if (charactor == null)
        {
            return flag;
        }
        if (charactor is MainPlayer)
        {
            return flag;
        }
        return ((ignoredeath || charactor.IsLive) && charactor.CharState == CharactorState.CreatComplete) || flag;
    }

    public void SetTarget(CharactorBase charactor, bool ignoredeath = false, bool switchAutoAttack = true)
    {
        this.SetSelectTarget(charactor, false);
        if (!this.CheckLegal(charactor, ignoredeath))
        {
            return;
        }
        this.selectMgr.SetTarget(charactor, MainPlayerTargetSelectMgr.SelectType.ManualSelect, 1U, true, switchAutoAttack);
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.OnTimerReducePriority));
        Scheduler.Instance.AddTimer(2f, false, new Scheduler.OnScheduler(this.OnTimerReducePriority));
    }

    private void OnTimerReducePriority()
    {
        if (this.selectMgr == null)
        {
            return;
        }
        this.selectMgr.CurrentSelectPriority = 2U;
    }

    public void ReqTarget()
    {
    }

    public void OnClickEvent(ScreenEvent SE)
    {
        if (SE.mTpye != ScreenEvent.EventType.Click && SE.mTpye != ScreenEvent.EventType.Select)
        {
            return;
        }
        this.CheckManualSelectCharactor(SE);
    }

    private void CheckManualSelectCharactor(ScreenEvent SE)
    {
        if (null == Camera.main)
        {
            return;
        }
        Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
        RaycastHit[] array = Physics.RaycastAll(ray, (float)(Const.LayerForMask.MainPlayer | Const.LayerForMask.OtherPlayer | Const.LayerForMask.NpcShadow | Const.LayerForMask.NpcNoShadow));
        if (array != null)
        {
            foreach (RaycastHit raycastHit in array)
            {
                CharactorBase charactorByGameObject = this.GetCharactorByGameObject(raycastHit.collider.gameObject);
                if (!(charactorByGameObject is MainPlayer))
                {
                    if (charactorByGameObject is Npc)
                    {
                        Npc npc = charactorByGameObject as Npc;
                        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                        if (configTable != null && configTable.GetField_Bool("not_beselect"))
                        {
                            return;
                        }
                    }
                    if (charactorByGameObject is MainPlayer || charactorByGameObject is Npc_Pet)
                    {
                        return;
                    }
                    if (charactorByGameObject is Npc_TaskCollect && !(charactorByGameObject as Npc_TaskCollect).CheckStateContainDoing())
                    {
                        return;
                    }
                    this.SetTarget(charactorByGameObject, charactorByGameObject is OtherPlayer, SE.mTpye == ScreenEvent.EventType.Click);
                    if (SE.mTpye == ScreenEvent.EventType.Click)
                    {
                        this.SetTargetResult(charactorByGameObject);
                    }
                }
            }
        }
    }

    public CharactorBase GetCharactorByGameObject(GameObject obj)
    {
        return ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorFromGameObject(obj);
    }

    public void SetTargetResult(CharactorBase Newtarget)
    {
        if (Newtarget == null)
        {
            return;
        }
        RelationType relationType = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(Newtarget);
        if (Newtarget is Npc)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)(Newtarget as Npc).NpcData.MapNpcData.baseid);
            if ((MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_GATHER) || MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_HOLD_NPC)) && (configTable.GetField_Uint("kind") == 6U || configTable.GetField_Uint("kind") == 8U))
            {
                return;
            }
            if (Newtarget is Npc_TaskCollect && (Newtarget as Npc_TaskCollect).CheckStateContainDoing())
            {
                MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
            }
            if (configTable.GetField_Uint("kind") == 1U || configTable.GetField_Uint("kind") == 6U || configTable.GetField_Uint("kind") == 8U || configTable.GetField_Uint("kind") == 11U)
            {
                float num = Vector2.Distance(MainPlayer.Self.NextPosition2D, Newtarget.NextPosition2D);
                float distNpcVisitResponse = Const.DistNpcVisitResponse;
                if (num < distNpcVisitResponse)
                {
                    this.OnReachTarget(Newtarget, true);
                }
                else if (configTable.GetField_Uint("visitfollow") == 0U)
                {
                    MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
                }
                else
                {
                    MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFollowMoveingTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
                }
            }
            else if (configTable.GetField_Uint("kind") == 2U || configTable.GetField_Uint("kind") == 3U || configTable.GetField_Uint("kind") == 4U || configTable.GetField_Uint("kind") == 5U)
            {
                MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToNormalAttack, new Action<CharactorBase, bool>(this.OnReachTarget));
            }
            else if (configTable.GetField_Uint("kind") == 20U || configTable.GetField_Uint("kind") == 24U)
            {
                MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
            }
            else if (configTable.GetField_Uint("kind") == 26U || configTable.GetField_Uint("kind") == 27U)
            {
                MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToNormalAttack, new Action<CharactorBase, bool>(this.OnReachTarget));
            }
        }
        else if (Newtarget is OtherPlayer)
        {
            if (relationType == RelationType.Enemy)
            {
                MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToNormalAttack, new Action<CharactorBase, bool>(this.OnReachTarget));
            }
            else
            {
                MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(Newtarget, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
            }
        }
    }

    private void OnReachTarget(CharactorBase cbase, bool isreach)
    {
        if (!isreach)
        {
            return;
        }
        if (cbase == null)
        {
            return;
        }
        RelationType relationType = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(cbase);
        if (MainPlayer.Self.GetComponent<AutoAttack>().AutoAttackOn)
        {
            if (cbase is Npc)
            {
                Npc npc = cbase as Npc;
                LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                if (configTable.GetField_Uint("kind") == 1U)
                {
                    ControllerManager.Instance.GetController<TaskController>().ReqVisteNpc((cbase as Npc).NpcData.MapNpcData.tempid);
                    MainPlayer.Self.GetComponent<FFDetectionNpcControl>().CurrentVisteNpcID = 0UL;
                    if (configTable.GetCacheField_Uint("lookat") == 0U)
                    {
                        MainPlayer.Self.SetPlayerLookAt(npc.ModelObj.transform.position, true);
                        npc.SetPlayerLookAt(MainPlayer.Self.ModelObj.transform.position, true);
                    }
                }
                else if (configTable.GetField_Uint("kind") == 8U)
                {
                    ControllerManager.Instance.GetController<TaskController>().ReqVisteNpc((cbase as Npc).NpcData.MapNpcData.tempid);
                }
                else if (configTable.GetField_Uint("kind") == 6U)
                {
                    ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc.NpcData.MapNpcData.tempid, configTable.GetField_Uint("kind"), null);
                }
                else if (configTable.GetField_Uint("kind") == 11U)
                {
                    ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc.NpcData.MapNpcData.tempid, configTable.GetField_Uint("kind"), null);
                }
                else if (configTable.GetField_Uint("kind") == 11U)
                {
                    if ((cbase as Npc_TaskCollect).CheckStateContainDoing())
                    {
                        ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc.NpcData.MapNpcData.tempid, configTable.GetField_Uint("kind"), null);
                    }
                }
                else if (configTable.GetField_Uint("kind") == 2U || configTable.GetField_Uint("kind") == 3U || configTable.GetField_Uint("kind") == 4U || configTable.GetField_Uint("kind") == 5U)
                {
                    if (relationType != RelationType.Friend)
                    {
                        uint attackSkillIDbystor = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
                        if (attackSkillIDbystor != 0U)
                        {
                            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickNormalAttack(attackSkillIDbystor, false);
                        }
                    }
                }
                else if (configTable.GetField_Uint("kind") == 26U || configTable.GetField_Uint("kind") == 27U)
                {
                    uint attackSkillIDbystor2 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
                    if (attackSkillIDbystor2 != 0U)
                    {
                        MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickNormalAttack(attackSkillIDbystor2, false);
                    }
                }
                else if (configTable.GetField_Uint("kind") == 20U)
                {
                    ControllerManager.Instance.GetController<PickDropController>().MSG_ReqWatchSceneBag_CS(npc, null);
                }
                else if (configTable.GetField_Uint("kind") == 24U)
                {
                    ControllerManager.Instance.GetController<DuoQiController>().ReqHoldFlagCaptureDB(npc.NpcData.BaseData.id);
                }
            }
            else if (cbase is OtherPlayer)
            {
                if (relationType != RelationType.Friend && relationType != RelationType.Neutral)
                {
                    uint attackSkillIDbystor3 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
                    if (attackSkillIDbystor3 != 0U)
                    {
                        MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickNormalAttack(attackSkillIDbystor3, false);
                    }
                }
            }
        }
        else if (cbase is Npc)
        {
            Npc npc2 = cbase as Npc;
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc2.NpcData.MapNpcData.baseid);
            if (configTable2.GetField_Uint("kind") == 1U)
            {
                float num = configTable2.GetCacheField_Float("volume") + Const.DistNpcVisitResponse;
                float num2 = Vector2.Distance(MainPlayer.Self.NextPosition2D, (cbase as Npc).NextPosition2D);
                if (num2 < num + MainPlayer.Self.Pfc.visitNpcRadius)
                {
                    ControllerManager.Instance.GetController<TaskController>().ReqVisteNpc((cbase as Npc).NpcData.MapNpcData.tempid);
                    if (configTable2.GetCacheField_Uint("lookat") == 0U)
                    {
                        MainPlayer.Self.SetPlayerLookAt(npc2.ModelObj.transform.position, true);
                        npc2.SetPlayerLookAt(MainPlayer.Self.ModelObj.transform.position, true);
                    }
                }
                else if (configTable2.GetField_Uint("visitfollow") == 0U)
                {
                    MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(cbase, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
                }
                else
                {
                    MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFollowMoveingTarget(cbase, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
                }
            }
            else if (configTable2.GetField_Uint("kind") == 8U)
            {
                ControllerManager.Instance.GetController<TaskController>().ReqVisteNpc((cbase as Npc).NpcData.MapNpcData.tempid);
            }
            else if (configTable2.GetField_Uint("kind") == 11U)
            {
                if ((cbase as Npc_TaskCollect).CheckStateContainDoing())
                {
                    ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc2.NpcData.MapNpcData.tempid, configTable2.GetField_Uint("kind"), null);
                }
            }
            else if (configTable2.GetField_Uint("kind") == 6U)
            {
                ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc2.NpcData.MapNpcData.tempid, configTable2.GetField_Uint("kind"), null);
            }
            else if (configTable2.GetField_Uint("kind") == 2U || configTable2.GetField_Uint("kind") == 3U || configTable2.GetField_Uint("kind") == 4U || configTable2.GetField_Uint("kind") == 5U)
            {
                if (relationType != RelationType.Friend)
                {
                    uint attackSkillIDbystor4 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
                    if (attackSkillIDbystor4 != 0U)
                    {
                        MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickNormalAttack(attackSkillIDbystor4, true);
                    }
                }
            }
            else if (configTable2.GetField_Uint("kind") == 26U || configTable2.GetField_Uint("kind") == 27U)
            {
                uint attackSkillIDbystor5 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
                if (attackSkillIDbystor5 != 0U)
                {
                    MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickNormalAttack(attackSkillIDbystor5, true);
                }
            }
            else if (configTable2.GetField_Uint("kind") == 20U)
            {
                ControllerManager.Instance.GetController<PickDropController>().MSG_ReqWatchSceneBag_CS(npc2, null);
            }
            else if (configTable2.GetField_Uint("kind") == 24U)
            {
                ControllerManager.Instance.GetController<DuoQiController>().ReqHoldFlagCaptureDB(npc2.NpcData.BaseData.id);
            }
        }
        else if (cbase is OtherPlayer)
        {
            if (relationType != RelationType.Friend && relationType != RelationType.Neutral)
            {
                uint attackSkillIDbystor6 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
                if (attackSkillIDbystor6 != 0U)
                {
                    MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickNormalAttack(attackSkillIDbystor6, false);
                }
            }
        }
        MainPlayer.Self.CharEvtMgr.SendEvent("CharEvt_MainPlayerReachTarget", new object[]
        {
            cbase
        });
    }

    private void CheckDistToTarget4CtrlTaskUI()
    {
        if (MainPlayer.Self != null)
        {
            if (this.Target != null)
            {
                string npcTalkName = GlobalRegister.GetNpcTalkName();
                ulong id = this.Target.EID.Id;
                FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
                if (component != null)
                {
                    ulong currentVisteNpcID = component.CurrentVisteNpcID;
                }
                if (Vector2.Distance(this.Target.NextPosition2D, MainPlayer.Self.NextPosition2D) > 18f)
                {
                    if (UIManager.GetLuaUIPanel("UI_NPCtalkAndTaskDlg") != null)
                    {
                        object[] array = LuaScriptMgr.Instance.CallLuaFunction("UI_NPCtalkAndTaskDlg.IsShowInUI", new object[]
                        {
                            Util.GetLuaTable("UI_NPCtalkAndTaskDlg")
                        });
                        if (array != null && array.Length > 0 && bool.Parse(array[0].ToString()))
                        {
                            LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.CloseTaskDialog", new object[0]);
                        }
                    }
                    if (this.selectMgr != null)
                    {
                        this.selectMgr.manualSelect.SetSelectTarget(null, false);
                    }
                    this.lastSelectNpcPos = Vector2.zero;
                    UIManager.Instance.CloseOpenByNpcUI();
                }
            }
            else if (Vector2.Distance(this.lastSelectNpcPos, Vector2.zero) > 0.1f && Vector2.Distance(this.lastSelectNpcPos, MainPlayer.Self.NextPosition2D) > 18f)
            {
                if (UIManager.GetLuaUIPanel("UI_NPCtalkAndTaskDlg") != null)
                {
                    object[] array2 = LuaScriptMgr.Instance.CallLuaFunction("UI_NPCtalkAndTaskDlg.IsShowInUI", new object[]
                    {
                        Util.GetLuaTable("UI_NPCtalkAndTaskDlg")
                    });
                    if (array2 != null && array2.Length > 0 && bool.Parse(array2[0].ToString()))
                    {
                        LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.CloseTaskDialog", new object[0]);
                    }
                }
                UIManager.Instance.CloseOpenByNpcUI();
                this.lastSelectNpcPos = Vector2.zero;
            }
        }
    }

    public void SetSelectTarget(CharactorBase t, bool ignoredeath = false)
    {
        this.Target = t;
        if (t != null)
        {
            this.lastSelectNpcPos = t.NextPosition2D;
        }
    }

    public const float ManualSelectPriorityDelay = 2f;

    private MainPlayerTargetSelectMgr selectMgr_;

    private EntitiesManager entitiesManager;

    public CharactorBase Target;

    public Vector2 lastSelectNpcPos = Vector2.zero;
}
