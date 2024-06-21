using System;
using apprentice;
using ProtoBuf;

[ProtoContract]
public class SystemSettingData_Show
{
    public MassiveClientSetInfo AddKeyValue(ref MassiveClientSetInfo list)
    {
        ClientSetInfo clientSetInfo = new ClientSetInfo();
        clientSetInfo.key = SystemSettingKey.PlayerBarShow.ToString();
        clientSetInfo.value = ((!this.PlayerBarShow) ? 0U : 1U);
        list.data.Add(clientSetInfo);
        ClientSetInfo clientSetInfo2 = new ClientSetInfo();
        clientSetInfo2.key = SystemSettingKey.PlayerNameShow.ToString();
        clientSetInfo2.value = ((!this.PlayerNameShow) ? 0U : 1U);
        list.data.Add(clientSetInfo2);
        ClientSetInfo clientSetInfo3 = new ClientSetInfo();
        clientSetInfo3.key = SystemSettingKey.PlayerGuildShow.ToString();
        clientSetInfo3.value = ((!this.PlayerGuildShow) ? 0U : 1U);
        list.data.Add(clientSetInfo3);
        ClientSetInfo clientSetInfo4 = new ClientSetInfo();
        clientSetInfo4.key = SystemSettingKey.OthersBarShow.ToString();
        clientSetInfo4.value = ((!this.OthersBarShow) ? 0U : 1U);
        list.data.Add(clientSetInfo4);
        ClientSetInfo clientSetInfo5 = new ClientSetInfo();
        clientSetInfo5.key = SystemSettingKey.OthersNameShow.ToString();
        clientSetInfo5.value = ((!this.OthersNameShow) ? 0U : 1U);
        list.data.Add(clientSetInfo5);
        ClientSetInfo clientSetInfo6 = new ClientSetInfo();
        clientSetInfo6.key = SystemSettingKey.OthersGuildShow.ToString();
        clientSetInfo6.value = ((!this.OthersGuildShow) ? 0U : 1U);
        list.data.Add(clientSetInfo6);
        ClientSetInfo clientSetInfo7 = new ClientSetInfo();
        clientSetInfo7.key = SystemSettingKey.EnemyBarShow.ToString();
        clientSetInfo7.value = ((!this.EnemyBarShow) ? 0U : 1U);
        list.data.Add(clientSetInfo7);
        ClientSetInfo clientSetInfo8 = new ClientSetInfo();
        clientSetInfo8.key = SystemSettingKey.EnemyNameShow.ToString();
        clientSetInfo8.value = ((!this.EnemyNameShow) ? 0U : 1U);
        list.data.Add(clientSetInfo8);
        ClientSetInfo clientSetInfo9 = new ClientSetInfo();
        clientSetInfo9.key = SystemSettingKey.EnemyGuildShow.ToString();
        clientSetInfo9.value = ((!this.EnemyGuildShow) ? 0U : 1U);
        list.data.Add(clientSetInfo9);
        ClientSetInfo clientSetInfo10 = new ClientSetInfo();
        clientSetInfo10.key = SystemSettingKey.NpcNameShow.ToString();
        clientSetInfo10.value = ((!this.NpcNameShow) ? 0U : 1U);
        list.data.Add(clientSetInfo10);
        return list;
    }

    public void CheckKeyValue(string key, uint value)
    {
        switch ((int)Enum.Parse(typeof(SystemSettingKey), key))
        {
            case 2:
                this.PlayerBarShow = (value == 1U);
                break;
            case 3:
                this.PlayerNameShow = (value == 1U);
                break;
            case 4:
                this.PlayerGuildShow = (value == 1U);
                break;
            case 5:
                this.OthersBarShow = (value == 1U);
                break;
            case 6:
                this.OthersNameShow = (value == 1U);
                break;
            case 7:
                this.OthersGuildShow = (value == 1U);
                break;
            case 8:
                this.EnemyBarShow = (value == 1U);
                break;
            case 9:
                this.EnemyNameShow = (value == 1U);
                break;
            case 10:
                this.EnemyGuildShow = (value == 1U);
                break;
            case 11:
                this.NpcNameShow = (value == 1U);
                break;
        }
    }

    [ProtoMember(1)]
    public bool PlayerBarShow;

    [ProtoMember(2)]
    public bool PlayerNameShow;

    [ProtoMember(3)]
    public bool PlayerGuildShow;

    [ProtoMember(4)]
    public bool OthersBarShow;

    [ProtoMember(5)]
    public bool OthersNameShow;

    [ProtoMember(6)]
    public bool OthersGuildShow;

    [ProtoMember(7)]
    public bool EnemyBarShow;

    [ProtoMember(8)]
    public bool EnemyNameShow;

    [ProtoMember(9)]
    public bool EnemyGuildShow;

    [ProtoMember(10)]
    public bool NpcNameShow;
}
