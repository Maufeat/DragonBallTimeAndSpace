using ProtoBuf;

[ProtoContract]
public class SkillSlotModel
{
    [ProtoMember(1)]
    public string herothisid;

    [ProtoMember(2)]
    public SlotData[] m_slots;
}
