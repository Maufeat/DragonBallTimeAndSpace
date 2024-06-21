using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MapDataType")]
    public enum MapDataType
    {
        [ProtoEnum(Name = "MAP_DATATYPE_USER", Value = 0)]
        MAP_DATATYPE_USER,
        [ProtoEnum(Name = "MAP_DATATYPE_NPC", Value = 1)]
        MAP_DATATYPE_NPC
    }
}
