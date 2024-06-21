using System;
using System.Collections.Generic;
using LuaInterface;
using magic;

public class OneSkillHitResult
{
    public float Delay;

    public EntitiesID Att;

    public uint Hp;

    public int HpChange;

    public List<ATTACKRESULT> AttcodeList;

    public FFActionClip Skillclip;

    public LuaTable Config;
}
