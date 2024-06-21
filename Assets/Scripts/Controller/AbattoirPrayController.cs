using System;
using Chat;
using Framework.Managers;
using LuaInterface;
using mobapk;
using Models;

public class AbattoirPrayController : ControllerBase
{
    private UI_AbattoirPray uiAbattoirPray
    {
        get
        {
            return UIManager.GetUIObject<UI_AbattoirPray>();
        }
    }

    private UI_AbattoirTips uiAbattoirTips
    {
        get
        {
            return UIManager.GetUIObject<UI_AbattoirTips>();
        }
    }

    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
        set
        {
        }
    }

    public override void Awake()
    {
        this.abattoirPrayNetWork = new AbattoirPrayNetWork();
        this.abattoirPrayNetWork.Initialize();
    }

    public void OnStartPray(MSG_StartPray_SC MsgData)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_AbattoirPray>("UI_AbattoirPray", delegate ()
        {
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            int getPrayRestTime = controller.getPrayRestTime;
            if (getPrayRestTime > 0)
            {
                this.uiAbattoirPray.OpenShow(MsgData.hopes, getPrayRestTime);
            }
            else
            {
                this.uiAbattoirPray.OpenShow(MsgData.hopes, 15);
            }
        }, UIManager.ParentType.CommonUI, true);
    }

    public void UpdatePrayTime(int time)
    {
        if (this.uiAbattoirPray != null)
        {
            this.uiAbattoirPray.UpdateShow(time);
        }
    }

    public void CloseAbattoirPrayUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this.uiAbattoirPray);
    }

    public void SendPray(uint lv1Index, uint lv2Index, uint lv3Index)
    {
        if (lv1Index < 0U || lv2Index < 0U || lv3Index < 0U)
        {
            return;
        }
        this.abattoirPrayNetWork.SendPray(lv1Index, lv2Index, lv3Index);
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this.uiAbattoirPray);
    }

    public void OnReceiveTipsMsg(MSG_Ret_ChannelChat_SC msg)
    {
        if (msg.channel_type != ChannelType.ChannelType_Moba)
        {
            return;
        }
        if (msg.textid != 0U)
        {
            LuaTable config = LuaConfigManager.GetConfigTable("textconfig", (ulong)msg.textid);
            if (config == null)
            {
                FFDebug.LogWarning(this, "t_text_config not have id " + msg.textid);
                return;
            }
            uint cacheField_Uint = config.GetCacheField_Uint("showtype");
            if (cacheField_Uint != 7U)
            {
                return;
            }
            string content = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(msg.str_chat);
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_AbattoirTips>("UI_AbattoirTips", delegate ()
            {
                if (this.uiAbattoirTips != null)
                {
                    uint cacheField_Uint2 = config.GetCacheField_Uint("showtime");
                    if (cacheField_Uint2 == 0U)
                    {
                        return;
                    }
                    this.uiAbattoirTips.ShowStay(content);
                    Scheduler.Instance.AddTimer(cacheField_Uint2, false, delegate
                    {
                        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this.uiAbattoirTips);
                    });
                }
            }, UIManager.ParentType.Tips, true);
        }
    }

    private AbattoirPrayNetWork abattoirPrayNetWork;
}
