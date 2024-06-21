using System;
using apprentice;
using ProtoBuf;

[ProtoContract]
public class SystemSettingData_Funtion
{
    public MassiveClientSetInfo AddKeyValue(ref MassiveClientSetInfo list)
    {
        ClientSetInfo clientSetInfo = new ClientSetInfo();
        clientSetInfo.key = SystemSettingKey.AllowPartyInvite.ToString();
        clientSetInfo.value = ((!this.AllowPartyInvite) ? 0U : 1U);
        list.data.Add(clientSetInfo);
        ClientSetInfo clientSetInfo2 = new ClientSetInfo();
        clientSetInfo2.key = SystemSettingKey.AllowGuildInvite.ToString();
        clientSetInfo2.value = ((!this.AllowGuildInvite) ? 0U : 1U);
        list.data.Add(clientSetInfo2);
        ClientSetInfo clientSetInfo3 = new ClientSetInfo();
        clientSetInfo3.key = SystemSettingKey.AllowFriendInvite.ToString();
        clientSetInfo3.value = ((!this.AllowFriendInvite) ? 0U : 1U);
        list.data.Add(clientSetInfo3);
        ClientSetInfo clientSetInfo4 = new ClientSetInfo();
        clientSetInfo4.key = SystemSettingKey.LowHealthWarning.ToString();
        clientSetInfo4.value = ((!this.LowHealthWarning) ? 0U : 1U);
        list.data.Add(clientSetInfo4);
        ClientSetInfo clientSetInfo5 = new ClientSetInfo();
        clientSetInfo5.key = SystemSettingKey.IfMouse.ToString();
        clientSetInfo5.value = ((!this.IfMouse) ? 0U : 1U);
        list.data.Add(clientSetInfo5);
        ClientSetInfo clientSetInfo6 = new ClientSetInfo();
        clientSetInfo6.key = SystemSettingKey.Close2ndPW.ToString();
        clientSetInfo6.value = ((!this.Close2ndPW) ? 0U : 1U);
        list.data.Add(clientSetInfo6);
        return list;
    }

    public void CheckKeyValue(string key, uint value)
    {
        switch ((int)Enum.Parse(typeof(SystemSettingKey), key))
        {
            case 101:
                this.AllowPartyInvite = (value == 1U);
                break;
            case 102:
                this.AllowGuildInvite = (value == 1U);
                break;
            case 103:
                this.AllowFriendInvite = (value == 1U);
                break;
            case 104:
                this.LowHealthWarning = (value == 1U);
                break;
            case 105:
                this.IfMouse = (value == 1U);
                break;
            case 106:
                this.Close2ndPW = (value == 1U);
                break;
        }
    }

    [ProtoMember(1)]
    public bool AllowPartyInvite;

    [ProtoMember(2)]
    public bool AllowGuildInvite;

    [ProtoMember(3)]
    public bool AllowFriendInvite;

    [ProtoMember(4)]
    public bool LowHealthWarning;

    [ProtoMember(5)]
    public bool IfMouse;

    [ProtoMember(6)]
    public bool Close2ndPW;
}
