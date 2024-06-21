using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using msg;

public class OtherPlayerData : CharabaseData
{
    public cs_MapUserData MapUserData
    {
        get
        {
            return this._MapUserData;
        }
        set
        {
            this._MapUserData = value;
        }
    }

    public override uint GetAppearanceid()
    {
        if (this.MapUserData != null)
        {
            return this.MapUserData.appearanceid;
        }
        return base.GetAppearanceid();
    }

    public override uint GetBaseOrHeroId()
    {
        if (this.MapUserData != null && this.MapUserData.mapshow != null)
        {
            return this.MapUserData.mapshow.heroid;
        }
        return base.GetBaseOrHeroId();
    }

    public override void SetHp(uint hp)
    {
        base.SetHp(hp);
        this.MapUserData.mapdata.hp = hp;
    }

    public virtual LuaTable GetHeroConfig()
    {
        uint baseOrHeroId = this.GetBaseOrHeroId();
        if (baseOrHeroId != 0U)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetConfig("heros").GetCacheField_Table(baseOrHeroId);
            if (cacheField_Table != null)
            {
                return cacheField_Table;
            }
        }
        return null;
    }

    public void RefreshSpeedAndState(cs_MapUserData userData)
    {
        this.Owner.moveSpeed = 1f / (userData.mapdata.movespeed / 1000f);
        ControllerManager.Instance.GetController<MainUIController>().UpdateTeamMemeberBuffIcon(userData);
        PlayerBufferControl component = this.Owner.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            component.AddStateByArray(userData.mapdata.lstState);
        }
    }

    public void RefreshMapShow(cs_MapUserData mapData, out bool mapshowchanged, out bool bakmapshowchanged)
    {
        mapshowchanged = (this.MapUserData.mapshow.antenna != mapData.mapshow.antenna || this.MapUserData.mapshow.face != mapData.mapshow.face || this.MapUserData.mapshow.facestyle != mapData.mapshow.facestyle || this.MapUserData.mapshow.haircolor != mapData.mapshow.haircolor || this.MapUserData.mapshow.hairstyle != mapData.mapshow.hairstyle || this.MapUserData.mapshow.bodystyle != mapData.mapshow.bodystyle || this.MapUserData.mapshow.coat != mapData.mapshow.coat);
        bakmapshowchanged = (this.MapUserData.bakmapshow.antenna != mapData.bakmapshow.antenna || this.MapUserData.bakmapshow.face != mapData.bakmapshow.face || this.MapUserData.bakmapshow.facestyle != mapData.bakmapshow.facestyle || this.MapUserData.bakmapshow.haircolor != mapData.bakmapshow.haircolor || this.MapUserData.bakmapshow.hairstyle != mapData.bakmapshow.hairstyle || this.MapUserData.bakmapshow.bodystyle != mapData.bakmapshow.bodystyle || this.MapUserData.bakmapshow.coat != mapData.bakmapshow.coat);
        if (mapshowchanged)
        {
            this.RefreshMapShow(mapData.mapshow);
            (this.Owner as OtherPlayer).CreatPlayerModel();
        }
        if (bakmapshowchanged)
        {
            this.RefreshBakMapShow(mapData.bakmapshow);
        }
    }

    public void RefreshMapUserData(cs_MapUserData userData)
    {
        bool flag = false;
        if (this.MapUserData != null && (this.MapUserData.mapshow.haircolor != userData.mapshow.haircolor || this.MapUserData.mapshow.hairstyle != userData.mapshow.hairstyle || this.MapUserData.mapshow.facestyle != userData.mapshow.facestyle || this.MapUserData.mapshow.face != userData.mapshow.face || this.MapUserData.mapshow.bodystyle != userData.mapshow.bodystyle || this.MapUserData.mapshow.coat != userData.mapshow.coat))
        {
            flag = true;
        }
        this.Owner.EID.Etype = CharactorType.Player;
        this.Owner.EID.Id = userData.charid;
        this.Owner.BaseData.RefreshBaseData(new cs_BaseData(CharactorType.Player, userData.charid, userData.mapdata.pos.fx, userData.mapdata.pos.fy, userData.mapdata.dir));
        this.MapUserData = userData;
        this.RefreshMapShow(this.MapUserData.mapshow);
        this.RefreshBakMapShow(this.MapUserData.bakmapshow);
        this.RefreshMapData(this.MapUserData.mapdata);
        ControllerManager.Instance.GetController<MainUIController>().UpdateTeamMemeberBuffIcon(this.MapUserData);
        if (this.Owner == MainPlayer.Self)
        {
            CallLuaListener.SendLuaEvent("OnMainPlayerHeroChangeLuaListener", false, new object[]
            {
                this.GetBaseOrHeroId()
            });
        }
        PlayerBufferControl component = this.Owner.GetComponent<PlayerBufferControl>();
        if (this.Owner != MainPlayer.Self && component != null)
        {
            component.AddStateByArray(this.MapUserData.mapdata.lstState);
        }
        FFWeaponHold component2 = this.Owner.GetComponent<FFWeaponHold>();
        if (component2 != null)
        {
            component2.ChangeWeapon(this.MapUserData.mapshow.weapon);
        }
        OtherPlayer otherPlayer = this.Owner as OtherPlayer;
        otherPlayer.RefreshPlayerMapUserData();
        if (otherPlayer is MainPlayer)
        {
            FFBehaviourControl component3 = otherPlayer.GetComponent<FFBehaviourControl>();
            if (component3 != null && !(component3.CurrState is FFBehaviourState_Walk))
            {
                otherPlayer.SetPlayerPosition(this.MapUserData.mapdata.pos, this.MapUserData.mapdata.dir);
            }
        }
        if (flag)
        {
            otherPlayer.onModelCreate = delegate ()
            {
                MainPlayerTargetSelectMgr component4 = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
                if (component4.TargetCharactor != null)
                {
                    OtherPlayer otherPlayer2 = component4.TargetCharactor as OtherPlayer;
                    Npc npc = component4.TargetCharactor as Npc;
                    ulong num = 0UL;
                    int num2 = 0;
                    if (otherPlayer2 != null)
                    {
                        num = otherPlayer2.OtherPlayerData.MapUserData.charid;
                        num2 = 1;
                    }
                    if (npc != null)
                    {
                        num = npc.NpcData.MapNpcData.tempid;
                        num2 = 2;
                    }
                    if (num != 0UL)
                    {
                        component4.SetTargetNull();
                        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
                        if (controller != null)
                        {
                            component4.manualSelect.SetTarget((num2 != 1) ? (CharactorBase)npc : (CharactorBase)otherPlayer2, false, true);
                            controller.SetSelectToShowTarget(num);
                        }
                    }
                }
            };
            otherPlayer.CreatPlayerModel();
        }
        if (this.Owner is MainPlayer)
        {
            ControllerManager.Instance.GetController<OccupyController>().RefreshOccupyNpcShow();
        }
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.pos = this.MapUserData.mapdata.pos;
        cs_MoveData.dir = this.MapUserData.mapdata.dir;
        this.Owner.RecodeSeverMoveData(cs_MoveData);
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager.MainPlayer != null)
        {
            ControllerManager.Instance.GetController<UIMapController>().SetMyPos(manager.MainPlayer.CurrServerPos, manager.MainPlayer.ServerDir, true, true);
        }
    }

    public override string GetACname()
    {
        uint avatarid = this.MapUserData.mapshow.avatarid;
        if (avatarid > 0U)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)avatarid);
            if (configTable != null)
            {
                string field_String = configTable.GetField_String("animatorcontroller");
                if (!string.IsNullOrEmpty(field_String))
                {
                    return field_String;
                }
            }
        }
        return base.GetACname();
    }

    public void RefreshMapShow(cs_CharacterMapShow mapShow)
    {
        if (this.MapUserData == null)
        {
            return;
        }
        this.MapUserData.mapshow = mapShow;
    }

    public void RefreshBakMapShow(cs_CharacterMapShow mapShow)
    {
        if (this.MapUserData == null)
        {
            return;
        }
        this.MapUserData.bakmapshow = mapShow;
    }

    public void RefreshMapData(cs_CharacterMapData mapData)
    {
        if (this.MapUserData == null)
        {
            return;
        }
        this.MapUserData.mapdata = mapData;
    }

    public void RefreshPlayerLevel(uint level)
    {
        if (this.MapUserData == null)
        {
            return;
        }
        this.MapUserData.mapdata.level = level;
    }

    public void RefreshState(List<StateItem> states)
    {
        if (this.MapUserData == null)
        {
            return;
        }
        this.MapUserData.mapdata.lstState.Clear();
        this.MapUserData.mapdata.lstState.AddRange(states);
    }

    private cs_MapUserData _MapUserData;
}
