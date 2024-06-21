using System;
using LuaInterface;

public class SkillClipConfig : IstorebAble
{
    public void Init(LuaTable config)
    {
        this.CanCancelCloseFist = config.GetField_Bool("CanCancelCloseFist");
        this.CanCancelPreFist = config.GetField_Bool("CanCancelPreFist");
        this.MoveCancelCloseFist = config.GetField_Bool("MoveCancelCloseFist");
        this.PoseTime = config.GetField_Uint("PoseTime") / 1000f;
        this.SpeedCut = config.GetField_Uint("SkillSpeed") / 100f;
        this.SkillStateId = config.GetField_Uint("id");
        this.EffectId = config.GetCacheField_Uint("EffectId");
    }

    public bool IsDirty { get; set; }

    public void RestThisObject()
    {
        this.PoseTime = 0f;
        this.CanCancelPreFist = false;
        this.CanCancelCloseFist = false;
        this.MoveCancelCloseFist = false;
        this.SpeedCut = 0f;
        this.SkillStateId = 0U;
    }

    public void StoreToPool()
    {
        ClassPool.Store<SkillClipConfig>(this, 40);
    }

    public float PoseTime;

    public bool CanCancelPreFist;

    public bool CanCancelCloseFist;

    public bool MoveCancelCloseFist;

    public float SpeedCut;

    public uint SkillStateId;

    public uint EffectId;
}
