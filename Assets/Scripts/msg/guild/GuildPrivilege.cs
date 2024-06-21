using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "GuildPrivilege")]
    public enum GuildPrivilege
    {
        [ProtoEnum(Name = "GUILDPRI_MANAGE_POSITION", Value = 1)]
        GUILDPRI_MANAGE_POSITION = 1,
        [ProtoEnum(Name = "GUILDPRI_REMOVE_MEMBER", Value = 2)]
        GUILDPRI_REMOVE_MEMBER,
        [ProtoEnum(Name = "GUILDPRI_MODIFY_POSITIONNAME", Value = 4)]
        GUILDPRI_MODIFY_POSITIONNAME = 4,
        [ProtoEnum(Name = "GUILDPRI_MODIFY_NOTIFY", Value = 8)]
        GUILDPRI_MODIFY_NOTIFY = 8,
        [ProtoEnum(Name = "GUILDPRI_MANAGE_BUILDINGDEV", Value = 16)]
        GUILDPRI_MANAGE_BUILDINGDEV = 16,
        [ProtoEnum(Name = "GUILDPRI_ACCEPT_JOIN", Value = 32)]
        GUILDPRI_ACCEPT_JOIN = 32,
        [ProtoEnum(Name = "GUILDPRI_ASSIGN_POSITION", Value = 64)]
        GUILDPRI_ASSIGN_POSITION = 64
    }
}
