using System;
using LuaInterface;

public class NpcData : CharabaseData
{
    public override uint GetAppearanceid()
    {
        if (this.MapNpcData != null)
        {
            return this.MapNpcData.appearanceid;
        }
        return base.GetAppearanceid();
    }

    public override void SetHp(uint hp)
    {
        base.SetHp(hp);
        this.MapNpcData.hp = hp;
    }

    public override uint GetBaseOrHeroId()
    {
        if (this.MapNpcData != null)
        {
            return this.MapNpcData.baseid;
        }
        return base.GetAppearanceid();
    }

    public NpcType GetNpcType()
    {
        LuaTable charabaseConfig = this.GetCharabaseConfig();
        if (charabaseConfig != null)
        {
            return (NpcType)charabaseConfig.GetCacheField_Uint("kind");
        }
        return NpcType.NPC_TYPE_NONE;
    }

    public override string GetModelName()
    {
        string text = base.GetModelName();
        if (text == "none" && !this.IsSimpleNpc())
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("newUser", (ulong)this.MapNpcData.showdata.heroid);
            if (configTable != null)
            {
                text = configTable.GetCacheField_String("Model");
            }
        }
        return text;
    }

    public bool IsSimpleNpc()
    {
        uint num = this.MapNpcData.showdata.coat;
        if (num == 0U)
        {
            num = this.MapNpcData.showdata.heroid;
        }
        return LuaConfigManager.GetConfigTable("newUser", (ulong)num) == null;
    }

    public void RefreshMapNpcData(cs_MapNpcData npcData)
    {
        this.Owner.EID.Etype = CharactorType.NPC;
        this.Owner.EID.Id = npcData.tempid;
        this.Owner.BaseData.RefreshBaseData(new cs_BaseData(CharactorType.NPC, npcData.tempid, npcData.x, npcData.y, npcData.dir));
        if (this.MapNpcData != null)
        {
            npcData.FHitPercent = this.MapNpcData.FHitPercent;
            npcData.ApparentBodySize = this.MapNpcData.ApparentBodySize;
            npcData.NpcType = this.MapNpcData.NpcType;
        }
        this.MapNpcData = npcData;
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.pos = new cs_FloatMovePos(this.MapNpcData.x, this.MapNpcData.y);
        cs_MoveData.dir = this.MapNpcData.dir;
        this.Owner.RecodeSeverMoveData(cs_MoveData);
        FFDebug.Log(this, FFLogType.Npc, string.Concat(new object[]
        {
            "Eid: ",
            this.Owner.EID,
            "appearanceid--->",
            this.MapNpcData.appearanceid
        }));
        if (this.Owner is Npc_Building)
        {
            return;
        }
        (this.Owner as Npc).RefreshNpcMapUserData();
    }

    public void RefreshHp(uint hp)
    {
        this.MapNpcData.hp = hp;
    }

    public cs_MapNpcData MapNpcData;
}
