using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using msg;

public class MainPlayerTargetSelectMgr : IFFComponent
{
    public CompnentState State { get; set; }

    public CharactorBase TargetCharactor
    {
        get
        {
            if (this._target == null)
            {
                return null;
            }
            if (this._target.CharState != CharactorState.CreatComplete)
            {
                return null;
            }
            return this._target;
        }
    }

    public UI_HpSystem msystem
    {
        get
        {
            return UIManager.GetUIObject<UI_HpSystem>();
        }
    }

    private EntitiesManager entitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    private PathFindComponent AstarPathFindComponent
    {
        get
        {
            if (this._pathFindComponent == null)
            {
                this._pathFindComponent = MainPlayer.Self.Pfc;
            }
            return this._pathFindComponent;
        }
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.manualSelect = new ManualTargetSelect();
        this.manualSelect.Init();
        this.autoattackSelect = new AutoAttackTargetSelect();
        this.autoattackSelect.Init();
        this.attackFollowTargetSelect = new AttactFollowTeamLeaderTargetSelect();
        this.attackFollowTargetSelect.Init();
        this.normalattackSelect = new NormalAttackTargetSelect();
        this.normalattackSelect.Init();
        this.beattackSelect = new BeAttackTargetSelect();
        this.beattackSelect.Init();
        this.skillhitSelect = new SkillHitTargetSelect();
        this.skillhitSelect.Init();
        this.TabSelect = new TabTargetSelect();
        this.TabSelect.Init();
        this.uiSelect = new UITargetSelect();
        this.uiSelect.Init();
        this.skillattackSelect = new SkillAttackTargetSelect();
        this.skillattackSelect.Init();
        this.FindEnemyDistence = LuaConfigManager.GetXmlConfigTable("targetSelectConfig").GetCacheField_Table("SeachEnemyDistence").GetCacheField_Float("value");
        Scheduler.Instance.AddTimer(this.CacheAttackTime, true, new Scheduler.OnScheduler(this.CancelCache));
        PathFindComponent.OnAstarChangeMapInfo += this.CancelCache;
    }

    public void CompDispose()
    {
        this.SetTargetNull();
        this.manualSelect.Dispose();
        this.manualSelect = null;
        this.autoattackSelect.Dispose();
        this.autoattackSelect = null;
        this.attackFollowTargetSelect.Dispose();
        this.attackFollowTargetSelect = null;
        this.normalattackSelect.Dispose();
        this.normalattackSelect.Init();
        this.beattackSelect.Dispose();
        this.beattackSelect = null;
        this.skillhitSelect.Dispose();
        this.skillhitSelect = null;
        this.TabSelect.Dispose();
        this.TabSelect = null;
        this.uiSelect.Dispose();
        this.uiSelect = null;
        this.skillattackSelect.Dispose();
        this.skillattackSelect = null;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CancelCache));
        PathFindComponent.OnAstarChangeMapInfo -= this.CancelCache;
    }

    public void CompUpdate()
    {
    }

    private void CancelCache()
    {
        this.CacheAttackState.Clear();
    }

    public void OldTargetCharactorCancel(CharactorBase oldone)
    {
        if (oldone == this.TargetCharactor)
        {
            return;
        }
        if (oldone != null)
        {
            oldone.CancelSelect();
            MainPlayerTargetSelectMgr.SetTargetSelectEffect(oldone, false);
        }
        this.CurrentSelectType = MainPlayerTargetSelectMgr.SelectType.None;
    }

    public void NewTargetCharactorSelect()
    {
        if (this.TargetCharactor != null)
        {
            this.TargetCharactor.TargetSelect();
            MainPlayerTargetSelectMgr.SetTargetSelectEffect(this.TargetCharactor, true);
        }
        if (this.TargetCharactor == null)
        {
            this.CurrentSelectType = MainPlayerTargetSelectMgr.SelectType.None;
        }
    }

    public void RefreshTargetCharactor(CharactorBase oldone)
    {
        if (oldone == this.TargetCharactor)
        {
            return;
        }
        if (oldone != null)
        {
            oldone.CancelSelect();
            MainPlayerTargetSelectMgr.SetTargetSelectEffect(oldone, false);
        }
        if (this.TargetCharactor != null)
        {
            this.TargetCharactor.TargetSelect();
            MainPlayerTargetSelectMgr.SetTargetSelectEffect(this.TargetCharactor, true);
        }
        if (this.TargetCharactor == null)
        {
            this.CurrentSelectType = MainPlayerTargetSelectMgr.SelectType.None;
        }
    }

    public static void SetTargetSelectEffect(CharactorBase Char, bool IsSelect)
    {
        if (Char == null)
        {
            return;
        }
        if (Char.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        if (Char is Npc)
        {
            Npc npc = Char as Npc;
            if (npc.hpdata != null)
            {
                npc.hpdata.RefreshModel();
            }
        }
        else if (Char is OtherPlayer)
        {
            OtherPlayer otherPlayer = Char as OtherPlayer;
            if (otherPlayer.hpdata != null)
            {
                otherPlayer.hpdata.RefreshModel();
            }
        }
    }

    public CharactorBase GetTempTarget()
    {
        return this._target;
    }

    public void SetTarget(CharactorBase charactor, MainPlayerTargetSelectMgr.SelectType type, uint priority, bool isSendSelectMessage = true, bool switchautattack = true)
    {
        CharactorBase tempTarget = this.GetTempTarget();
        if (tempTarget != null && tempTarget.hpdata != null)
        {
            tempTarget.hpdata.SetSelectUIActive(false);
        }
        this.OldTargetCharactorCancel(tempTarget);
        this._target = charactor;
        if (this._target != null && this._target.hpdata != null)
        {
            this._target.hpdata.SetSelectUIActive(true);
        }
        this.CurrentSelectType = type;
        this.CurrentSelectPriority = priority;
        this.NewTargetCharactorSelect();
        ulong id = 0UL;
        MapDataType mdt = MapDataType.MAP_DATATYPE_NPC;
        if (charactor is Npc)
        {
            id = (charactor as Npc).NpcData.MapNpcData.tempid;
            mdt = MapDataType.MAP_DATATYPE_NPC;
        }
        else if (charactor is OtherPlayer)
        {
            id = (charactor as OtherPlayer).OtherPlayerData.MapUserData.charid;
            mdt = MapDataType.MAP_DATATYPE_USER;
        }
        if (isSendSelectMessage)
        {
            this.entitiesManager.PNetWork.ReqOnTargetChange_SC(id, ChooseTargetType.CHOOSE_TARGE_TTYPE_SET, mdt);
        }
    }

    public void CancelNpcSelect(CharactorBase oldnpc)
    {
        if (oldnpc != this._target)
        {
            return;
        }
        this.SetTargetNull();
    }

    public void SetTargetNull()
    {
        CharactorBase tempTarget = this.GetTempTarget();
        if (this.msystem != null)
        {
            this.msystem.SetSelectUIActive(false);
        }
        if (this.setNullCallBack != null)
        {
            for (int i = 0; i < this.setNullCallBack.Count; i++)
            {
                if (this.setNullCallBack[i] != null)
                {
                    this.setNullCallBack[i](this._target);
                }
            }
        }
        MapDataType mdt = MapDataType.MAP_DATATYPE_USER;
        if (this._target != null)
        {
            if (this._target is Npc)
            {
                mdt = MapDataType.MAP_DATATYPE_NPC;
            }
            if (this._target is OtherPlayer)
            {
                mdt = MapDataType.MAP_DATATYPE_USER;
            }
        }
        this._target = null;
        this.CurrentSelectType = MainPlayerTargetSelectMgr.SelectType.None;
        this.CurrentSelectPriority = 0U;
        this.OldTargetCharactorCancel(tempTarget);
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.CloseTargetInfo();
        }
        this.manualSelect.SetSelectTarget(null, false);
        this.entitiesManager.PNetWork.ReqOnTargetChange_SC(0UL, ChooseTargetType.CHOOSE_TARGE_TTYPE_CANCEL, mdt);
    }

    public void RegSetSelcetTargetNullCallBack(Action<CharactorBase> cb)
    {
        if (this.setNullCallBack == null)
        {
            this.setNullCallBack = new List<Action<CharactorBase>>();
        }
        if (!this.setNullCallBack.Contains(cb))
        {
            this.setNullCallBack.Add(cb);
        }
    }

    public void UnRegSetSelcetTargetNullCallBack(Action<CharactorBase> cb)
    {
        if (this.setNullCallBack == null)
        {
            return;
        }
        if (this.setNullCallBack.Contains(cb))
        {
            this.setNullCallBack.Remove(cb);
        }
    }

    public bool CanAttack(CharactorBase Char)
    {
        bool flag = false;
        if (Char == null)
        {
            return flag;
        }
        if (Char == MainPlayer.Self)
        {
            return flag;
        }
        if (Char.CharState != CharactorState.CreatComplete)
        {
            return flag;
        }
        if (this.CacheAttackState.TryGetValue(Char.EID, out flag))
        {
            return flag;
        }
        if (Char is OtherPlayer)
        {
            RelationType otherPlayerRelationType = this.entitiesManager.GetOtherPlayerRelationType(Char as OtherPlayer);
            if (otherPlayerRelationType == RelationType.Enemy)
            {
                flag = true;
            }
        }
        else if (Char is Npc)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)(Char as Npc).NpcData.MapNpcData.baseid);
            if (!configTable.GetField_Bool("not_beselect"))
            {
                RelationType npcRelationType = this.entitiesManager.GetNpcRelationType(Char as Npc);
                if (npcRelationType == RelationType.Enemy)
                {
                    flag = true;
                }
                else if (npcRelationType == RelationType.Neutral && (configTable.GetField_Uint("kind") == 2U || configTable.GetField_Uint("kind") == 22U))
                {
                    flag = true;
                }
            }
        }
        if (flag)
        {
            flag = this.CanSelect(Char);
        }
        this.CacheAttackState[Char.EID] = flag;
        return flag;
    }

    public bool CanSelect(CharactorBase Char)
    {
        return Char != null;
    }

    public void ResetComp()
    {
    }

    public ManualTargetSelect manualSelect;

    public AutoAttackTargetSelect autoattackSelect;

    public AttactFollowTeamLeaderTargetSelect attackFollowTargetSelect;

    public NormalAttackTargetSelect normalattackSelect;

    public BeAttackTargetSelect beattackSelect;

    public SkillHitTargetSelect skillhitSelect;

    public SkillAttackTargetSelect skillattackSelect;

    public TabTargetSelect TabSelect;

    public UITargetSelect uiSelect;

    public float FindEnemyDistence = 9999f;

    private CharactorBase _target;

    public MainPlayerTargetSelectMgr.SelectType CurrentSelectType;

    public uint CurrentSelectPriority;

    private Dictionary<EntitiesID, bool> CacheAttackState = new Dictionary<EntitiesID, bool>();

    private float CacheAttackTime = 20f;

    private PathFindComponent _pathFindComponent;

    private List<Action<CharactorBase>> setNullCallBack;

    public enum SelectType
    {
        None,
        BeUnderAttack,
        AutoAttackSelect,
        NormalAttackSelect,
        SkillAttackSelect,
        ManualSelect,
        UISelect
    }
}
