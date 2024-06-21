using System;
using Framework.Managers;

public class PlayerAttributeInfo
{
    public static PlayerAttributeInfo Instance
    {
        get
        {
            if (PlayerAttributeInfo.playerInfo == null)
            {
                PlayerAttributeInfo.playerInfo = new PlayerAttributeInfo();
            }
            return PlayerAttributeInfo.playerInfo;
        }
    }

    public void RefreshPlayerInfo()
    {
        this.RefreshRoleInfo();
    }

    public void RefreshRoleInfo()
    {
        ControllerManager.Instance.GetController<MainUIController>().UpdateFightValue();
        ControllerManager.Instance.GetController<CardController>().UpdateFightValue();
    }

    public void OnFightValueChange(uint oldvalue, uint newvalue)
    {
        if (oldvalue != newvalue)
        {
            LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.StartFightValueChage", new object[]
            {
                Util.GetLuaTable("MainUICtrl"),
                oldvalue,
                newvalue
            });
        }
    }

    public void RefreshBloods()
    {
    }

    private static PlayerAttributeInfo playerInfo;
}
