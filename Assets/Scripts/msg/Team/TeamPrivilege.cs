using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "TeamPrivilege")]
    public enum TeamPrivilege
    {
        [ProtoEnum(Name = "TeamPrivilege_Invite", Value = 1)]
        TeamPrivilege_Invite = 1,
        [ProtoEnum(Name = "TeamPrivilege_RemoveMember", Value = 2)]
        TeamPrivilege_RemoveMember
    }
}
