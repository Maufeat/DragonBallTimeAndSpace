using System;

public class PublicCDData
{
    public PublicCDData(string sid, string svalue)
    {
        this.GroupID = uint.Parse(sid);
        this.CDLength = ulong.Parse(svalue);
    }

    public PublicCDData(uint id, uint value)
    {
        this.GroupID = id;
        this.CDLength = (ulong)value;
    }

    public ulong LastActiveTime
    {
        get
        {
            return this.lastactivetime;
        }
        set
        {
            this.lastactivetime = value;
        }
    }

    public bool CheckPublicCD()
    {
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        return currServerTime - this.lastactivetime <= this.CDLength;
    }

    public void ActivateCD(uint skillid)
    {
        this.ActivateCDSkill = skillid;
        this.lastactivetime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
    }

    public uint GetCDLeft(ulong Now)
    {
        if (this.lastactivetime == 0UL)
        {
            return 0U;
        }
        if (Now - this.lastactivetime > this.CDLength)
        {
            return 0U;
        }
        return (uint)(this.CDLength - (Now - this.lastactivetime));
    }

    public uint GroupID;

    public ulong CDLength;

    public uint ActivateCDSkill;

    private ulong lastactivetime;
}
