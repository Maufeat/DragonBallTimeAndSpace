using ProtoBuf;

namespace avatar
{
    [ProtoContract(Name = "AvatarPart")]
    public enum AvatarPart
    {
        [ProtoEnum(Name = "AVATAR_HEAD", Value = 1)]
        AVATAR_HEAD = 1,
        [ProtoEnum(Name = "AVATAR_HAIR", Value = 2)]
        AVATAR_HAIR,
        [ProtoEnum(Name = "AVATAR_BODY", Value = 3)]
        AVATAR_BODY
    }
}
