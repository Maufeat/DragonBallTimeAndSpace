using ProtoBuf;

[ProtoContract]
public class SlotData
{
    [ProtoMember(1)]
    public uint m_slotIndex;

    [ProtoMember(2)]
    public uint m_skillId;
}
