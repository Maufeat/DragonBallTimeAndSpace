using System;
using LuaInterface;

public class EntitesFactory
{
    public static Npc CreatNpc(cs_MapNpcData MapNpcData)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)MapNpcData.baseid);
        Npc npc = null;
        if (configTable != null)
        {
            NpcType cacheField_Uint = (NpcType)configTable.GetCacheField_Uint("kind");
            if (cacheField_Uint == NpcType.NPC_TYPE_TRAP)
            {
                npc = new Npc_Trap();
            }
            else if (cacheField_Uint == NpcType.NPC_TYPE_SUMMONPET)
            {
                npc = new Npc_Pet();
            }
            else if (cacheField_Uint == NpcType.NPC_TYPE_QUESTGATHER)
            {
                npc = new Npc_TaskCollect();
            }
            else if (cacheField_Uint == NpcType.NPC_TYPE_BUILDING || cacheField_Uint == NpcType.NPC_TYPE_NOBLOCK_LOCK)
            {
                npc = new Npc_Building();
            }
            else if (cacheField_Uint == NpcType.NPC_TYPE_BLOCK_LOCK)
            {
                npc = new Npc_BuildingAirWall();
            }
            else if (cacheField_Uint == NpcType.NPC_TYPE_DYN_BUILDING)
            {
                npc = new NPC_DynamicBuilding();
            }
            else if (cacheField_Uint == NpcType.NPC_TYPE_ASSIST)
            {
                npc = new Npc_Assist();
            }
            else
            {
                npc = new Npc();
            }
        }
        else
        {
            FFDebug.LogWarning("EntitesFactory", "npcConfig null :" + MapNpcData.baseid);
        }
        if (npc != null)
        {
            npc.NpcData.RefreshMapNpcData(MapNpcData);
        }
        return npc;
    }
}
