using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class FFActionClip : ScriptableObject
{
    [ProtoMember(126)]
    public string UObjname
    {
        get
        {
            return base.name;
        }
        set
        {
            base.name = value;
        }
    }

    public string[] GetEffectsByGroupID(FFActionClip.EffectType _type, uint groupId = 1U)
    {
        List<FFActionClipEffects> list = null;
        if (_type == FFActionClip.EffectType.Type_Skill)
        {
            list = this.SkillEffectList;
        }
        if (_type == FFActionClip.EffectType.Type_Hit)
        {
            list = this.HitEffectList;
        }
        if (_type == FFActionClip.EffectType.Type_Fly)
        {
            list = this.FlyEffectList;
        }
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (groupId == list[i].group)
                {
                    return list[i].effects;
                }
            }
        }
        return new string[0];
    }

    [ProtoMember(127)]
    public FFActionClip[] ProtoList;

    [ProtoMember(1)]
    public string ClipName = string.Empty;

    [ProtoMember(2)]
    public string ACName = string.Empty;

    [ProtoMember(3)]
    public FFActionClip.PoseType mPoseType;

    [ProtoMember(4)]
    public uint PoseTimeStartF;

    [ProtoMember(5)]
    public uint PoseTimeEndF;

    [ProtoMember(6)]
    public uint AttackTimeF;

    [ProtoMember(7)]
    public uint CloseFistTimeF;

    [ProtoMember(8)]
    public uint TransitionDurationF;

    [ProtoMember(9)]
    public uint StartMoveF;

    [ProtoMember(10)]
    public uint EndMoveF;

    [ProtoMember(11)]
    public uint CanMoveCancelSkillTime;

    [ProtoMember(12)]
    public uint CanMoveCancelCloseFistTime;

    [ProtoMember(13)]
    public bool ApplyFeetIK;

    [ProtoMember(14)]
    public bool ApplyAniMotion;

    [ProtoMember(15)]
    public string[] SkillEffects = new string[0];

    [ProtoMember(16)]
    public string[] HitEffects = new string[0];

    [ProtoMember(17)]
    public string[] SkillMaterialEffects = new string[0];

    [ProtoMember(18)]
    public string[] HitMaterialEffects = new string[0];

    [ProtoMember(19)]
    public string[] FlyObjEffect = new string[0];

    [ProtoMember(20)]
    public uint[] FakeAttackTimeFs = new uint[0];

    [ProtoMember(21)]
    public string[] CameraShakes = new string[0];

    [ProtoMember(22)]
    public string[] CameraAnims = new string[0];

    [ProtoMember(23)]
    public List<FFActionClipEffects> SkillEffectList = new List<FFActionClipEffects>();

    [ProtoMember(24)]
    public List<FFActionClipEffects> HitEffectList = new List<FFActionClipEffects>();

    [ProtoMember(25)]
    public List<FFActionClipEffects> FlyEffectList = new List<FFActionClipEffects>();

    [ProtoMember(26)]
    public float MirrorTotalTime;

    [ProtoMember(27)]
    public float MirrorFadeSpeed;

    public enum PoseType
    {
        None,
        Pauce,
        Loop
    }

    public enum EffectType
    {
        Type_Skill = 1,
        Type_Hit,
        Type_Fly
    }
}
