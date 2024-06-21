using System;
using ProtoBuf;

[ProtoContract]
public class SystemSettingData_Sound
{
    [ProtoMember(1)]
    public int BgMusic;

    [ProtoMember(2)]
    public int EffectMusic;

    [ProtoMember(3)]
    public int Voice;

    [ProtoMember(4)]
    public int MainSound;

    [ProtoMember(5)]
    public bool IsMain;

    [ProtoMember(6)]
    public bool IsTeam;

    [ProtoMember(7)]
    public bool IsOther;

    [ProtoMember(8)]
    public bool IsNPC;

    [ProtoMember(9)]
    public int Save = 5;
}
