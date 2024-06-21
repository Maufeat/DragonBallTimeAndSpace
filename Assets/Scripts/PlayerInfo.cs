using System;
using Net;

public class PlayerInfo : StructData
{
    public override OctetsStream WriteData(OctetsStream os)
    {
        return os;
    }

    public string strName = string.Empty;

    public byte bySex;

    public ushort wLevel;

    public ulong qwExp;

    public uint dwFairyStone;

    public uint dwGold;

    public uint dwCoPatch;

    public uint dwLeaderShip;

    public uint dwPhyPower;

    public uint dwMaxPhyPower;

    public uint dwMaxFriendNum;

    public uint dwFriendPoint;

    public uint dwHeroBagCount;

    public uint airLimit;

    public uint curAir;

    public ushort wdMorale;

    public uint vip;

    public uint vip_gem;

    public uint battle_coin;

    public uint dwBuyHeroBagNum;

    public uint dwEquipBagNum;

    public uint dwBuyEquipBagNum;

    public uint dwWeekCardDay;

    public uint dwMonthCardDay;

    public ulong qwMessage;

    public byte byDisturb;

    public uint dwRoleId;

    public ushort wZoneId;

    public string strUuid;

    public string strPasswd;
}
