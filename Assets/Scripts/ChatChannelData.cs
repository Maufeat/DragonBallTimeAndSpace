using System;
using Chat;
using LuaInterface;

public class ChatChannelData
{
    public ChatChannelData(ChannelType type)
    {
        this.ChannelType = type;
        this.ChannelConfig = LuaConfigManager.GetConfigTable("chatchannel", (ulong)type);
        this.iscooldown = false;
        if (this.ChannelConfig != null)
        {
            this.leftTime = this.ChannelConfig.GetField_Float("cdrate");
        }
        this.laststring = string.Empty;
    }

    public void Update()
    {
        if (this.iscooldown)
        {
            this.leftTime -= Scheduler.Instance.realDeltaTime;
            if (this.leftTime < 0f)
            {
                this.leftTime = 0f;
                this.iscooldown = false;
            }
        }
    }

    public bool CheckSend()
    {
        if (this.iscooldown)
        {
            TipsWindow.ShowWindow(1302U);
            FFDebug.Log(this, FFLogType.Network, "CDing");
            return false;
        }
        if (!this.CheckProp())
        {
            FFDebug.Log(this, FFLogType.Network, "Lack of Props");
            return false;
        }
        this.leftTime = this.ChannelConfig.GetField_Float("cdrate");
        this.iscooldown = true;
        return true;
    }

    private bool CheckProp()
    {
        string field_String = this.ChannelConfig.GetField_String("cost");
        string[] array = field_String.Split(new char[]
        {
            ';'
        });
        for (int i = 0; i < array.Length; i++)
        {
            if (!string.IsNullOrEmpty(array[i]))
            {
                string[] array2 = array[i].Split(new char[]
                {
                    '-'
                });
                uint num = uint.Parse(array2[0]);
                uint num2 = uint.Parse(array2[1]);
                uint num3 = (uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    num
                })[0];
                if (num3 < num2)
                {
                    string field_String2 = LuaConfigManager.GetConfigTable("objects", (ulong)num).GetField_String("name");
                    TipsWindow.ShowWindow(1305U, new string[]
                    {
                        this.ChannelConfig.name,
                        field_String2,
                        num2.ToString()
                    });
                    return false;
                }
            }
        }
        return true;
    }

    public ChannelType ChannelType;

    public LuaTable ChannelConfig;

    private bool iscooldown;

    public string laststring;

    public uint repeattime;

    private float leftTime;
}
