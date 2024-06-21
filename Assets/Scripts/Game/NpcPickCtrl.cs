using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class NpcPickCtrl : IFFComponent
{
    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        this.Init();
    }

    private void Init()
    {
        if (!(this.Owner is Npc))
        {
            return;
        }
        if (this.Owner == null)
        {
            return;
        }
        this.hasinit = true;
        this.npcmy = (this.Owner as Npc);
        if (this.npcmy.NpcData.GetNpcType() == NpcType.NPC_TYPE_PICK_UP)
        {
            this.Owner.CharEvtMgr.AddListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
            MainPlayer.Self.CharEvtMgr.AddListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
            MainPlayer.Self.CharEvtMgr.AddListener("CharEvt_BuffUpdate", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
            MainPlayer.Self.CharEvtMgr.AddListener("CharEvt_BuffRemove", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
            uint baseOrHeroId = this.Owner.BaseData.GetBaseOrHeroId();
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("npc").GetCacheField_Table("pickupnpc");
            LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(baseOrHeroId.ToString());
            if (cacheField_Table2 != null)
            {
                this.checkState = cacheField_Table2.GetCacheField_Int("userstate");
                this.picker = cacheField_Table2.GetCacheField_Int("picker");
            }
            this.CheckPickup(this.Owner, new object[0]);
        }
    }

    public void CompDispose()
    {
        this.Owner.CharEvtMgr.RemoveListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.CharEvtMgr.RemoveListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
            MainPlayer.Self.CharEvtMgr.RemoveListener("CharEvt_BuffUpdate", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
            MainPlayer.Self.CharEvtMgr.RemoveListener("CharEvt_BuffRemove", new CharacterEventMgr.CharacterEventHandler(this.CheckPickup));
        }
    }

    private void CheckPickup(CharactorBase charb, params object[] args)
    {
        if (!this.Owner.IsLive)
        {
            return;
        }
        if (this.CheckState() && this.IsInRange(this.Owner.CurrentPosition2D, MainPlayer.Self.CurrentPosition2D) && this.CheckPicker())
        {
            this.ReqPick();
        }
    }

    private bool CheckState()
    {
        return this.checkState <= 0 || !MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(this.checkState);
    }

    private bool CheckPicker()
    {
        if (this.picker > 0 && this.npcmy.NpcData.MapNpcData != null && this.npcmy.NpcData.MapNpcData.MasterData != null)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager != null)
            {
                RelationType relationType;
                if (this.npcmy.NpcData.MapNpcData.MasterData.Eid == MainPlayer.Self.EID)
                {
                    relationType = RelationType.Self;
                }
                else
                {
                    relationType = manager.GetOtherPlayerRelationType(new cs_CharacterMapData(this.npcmy.NpcData.MapNpcData.MasterData));
                }
                return (this.picker & (int)relationType) != 0;
            }
        }
        return true;
    }

    private void ReqPick()
    {
        if (this.OnPicking)
        {
            return;
        }
        OccupyController controller = ControllerManager.Instance.GetController<OccupyController>();
        if (controller != null)
        {
            controller.ReqPickUp(this.Owner.EID.Id);
            this.OnPicking = true;
        }
    }

    public void FinishPick(bool succeed)
    {
        if (!succeed)
        {
            this.OnPicking = false;
        }
    }

    private bool IsInRange(Vector2 selfServerPos, Vector2 npcServerPos)
    {
        bool result = false;
        if (NpcPickCtrl.PickRange < 0f)
        {
            NpcPickCtrl.PickRange = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("npcVisiteShowDis").GetCacheField_Float("value");
        }
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(selfServerPos);
        Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(npcServerPos);
        float num = Vector3.Distance(worldPosByServerPos, worldPosByServerPos2);
        if (num <= NpcPickCtrl.PickRange)
        {
            result = true;
        }
        return result;
    }

    public void CompUpdate()
    {
        if (!(this.Owner is Npc))
        {
            return;
        }
        if (this.Owner == null)
        {
            return;
        }
        if (!this.hasinit)
        {
            this.Init();
        }
    }

    public void ResetComp()
    {
    }

    public CharactorBase Owner;

    private bool hasinit;

    private Npc npcmy;

    private int checkState = -1;

    private int picker = -1;

    public static float PickRange = 1f;

    private bool OnPicking;
}
