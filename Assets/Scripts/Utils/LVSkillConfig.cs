using System;
using LuaInterface;

public class LVSkillConfig : IstorebAble
{
    public void Init(LuaTable config, float skilltotaltime)
    {
        this.SkillStateId = config.GetField_Uint("id");
        this.UseType = config.GetField_Uint("usetype");
        this.SkillTotalTime = skilltotaltime;
    }

    public void RestThisObject()
    {
        this.SkillStateId = 0U;
        this.UseType = 0U;
    }

    public bool IsDirty { get; set; }

    public void StoreToPool()
    {
        ClassPool.Store<LVSkillConfig>(this, 40);
    }

    public uint SkillStateId;

    public uint UseType;

    public float SkillTotalTime;
}
