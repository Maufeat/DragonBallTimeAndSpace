using System;
using magic;
using UnityEngine;

public class SkillServerData
{
    public void SetData(SkillData skill)
    {
        this.Skillid = skill.skillid;
        this.Level = skill.level;
        this.Lastusetime = skill.lastusetime;
        this.StateOn = (skill.onoff == 1U);
        this.IStorageTimes = skill.overlaytimes;
        this.Lastupdatetime = skill.lastupdatetime;
        this.ActiveStage = skill.active_stages;
        this.IMaxStorageTimes = skill.maxmultitimes;
        string format = " SkillServerData SetData: {0} Lastusetime: {1} onoff: {2} storagetimes: {3} lastupdatetime:{4} invertal {5} at {6}";
        ulong num = SingletonForMono<GameTime>.Instance.GetCurrServerTime() - this.Lastupdatetime;
        FFDebug.Log(this, FFLogType.Skill, string.Format(format, new object[]
        {
            this.Skillid,
            this.Lastusetime,
            this.StateOn,
            this.IStorageTimes,
            this.Lastupdatetime,
            num,
            Time.time
        }));
    }

    public uint Career;

    public uint Skillid;

    public uint Level;

    public ulong Lastusetime;

    public bool StateOn;

    public ulong Lastupdatetime;

    public uint IStorageTimes;

    public uint IMaxStorageTimes;

    public uint ActiveStage;
}
