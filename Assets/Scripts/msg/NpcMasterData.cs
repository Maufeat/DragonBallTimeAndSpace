using System;
using msg;

public class NpcMasterData
{
    public NpcMasterData(MasterData data)
    {
        this.Eid = new EntitiesID(data.idtype);
        this.MasterName = data.name;
        this.guildid = data.guildid;
        this.country = data.country;
        this.teamid = data.teamid;
    }

    public EntitiesID Eid;

    public string MasterName;

    public uint country;

    public ulong guildid;

    public uint teamid;

    public string tittleName;
}
