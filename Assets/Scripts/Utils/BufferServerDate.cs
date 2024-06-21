using System;
using System.Collections.Generic;

public class BufferServerDate
{
    public BufferServerDate()
    {
    }

    public BufferServerDate(UserState flag, EntitiesID EID)
    {
        this.thisid = CommonTools.GenernateBuffHash(EID, (ulong)((long)flag), 0UL);
    }

    public UserState flag;

    public ulong settime;

    public ulong overtime;

    public ulong duartion;

    public uint addLayer;

    public ulong giver;

    public ulong uniqueid;

    public ulong thisid;

    public ulong curTime;

    public ulong skillid;

    public ulong configTime;

    public List<ulong> effects;
}
