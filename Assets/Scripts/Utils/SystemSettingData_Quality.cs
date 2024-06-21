using System;
using ProtoBuf;

[ProtoContract]
public class SystemSettingData_Quality
{
    [ProtoMember(1)]
    public int QualityLevel;

    [ProtoMember(2)]
    public bool IsAutoSetting;

    [ProtoMember(3)]
    public bool Vsync;

    [ProtoMember(4)]
    public int Shadow;

    [ProtoMember(5)]
    public int ShadowDistance;

    [ProtoMember(6)]
    public int Antialiasing;

    [ProtoMember(7)]
    public int Save = 5;
}
