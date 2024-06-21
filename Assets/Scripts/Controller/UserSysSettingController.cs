using System;
using System.Text;
using Models;

public class UserSysSettingController : ControllerBase
{
    public override void Awake()
    {
        this.sysSettingNetWork = new UserSysSettingNetWork();
        this.sysSettingNetWork.Initialize();
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "serversystemsetting_controller";
        }
    }

    public void ReqSysSetting(SYSSETTING type, bool state)
    {
        this.sysSettingNetWork.ReqUserSysSetting(type, state);
    }

    public void WriteSysSettingData(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        Array.Copy(bytes, this.allSetting, this.allSetting.Length);
        if (UserSysSettingController.onSysSettingChanged != null)
        {
            UserSysSettingController.onSysSettingChanged();
        }
    }

    public bool GetSyssettingState(SYSSETTING type)
    {
        return type < SYSSETTING.SETTING_MAX && ((int)this.allSetting[((int)type / (int)SYSSETTING.SETTING_ROLL_WHITE)] & (255 & 1 << ((int)type % (int)SYSSETTING.SETTING_ROLL_WHITE))) != 0;
    }

    private UserSysSettingNetWork sysSettingNetWork;

    private byte[] allSetting = new byte[2];

    public static UserSysSettingController.OnSysSettingChangedHandle onSysSettingChanged;

    public delegate void OnSysSettingChangedHandle();
}
