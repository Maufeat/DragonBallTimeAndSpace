using System;
using Models;
using Obj;

public class VipPrivilegeController : ControllerBase
{
    private UI_VipPrivilege ui_exchange
    {
        get
        {
            return UIManager.GetUIObject<UI_VipPrivilege>();
        }
    }

    public override void Awake()
    {
        this.mNetWork = new VipPrivilegeNetWork();
        this.mNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "vipprivilege_controller";
        }
    }

    public void GetCurVipCardInfo()
    {
        this.mNetWork.ReqGetVIPCardInfo_CS();
    }

    internal void RetGetVIPCardInfo_SC(MSG_RetGetVIPCardInfo_SC msg)
    {
        if (this.ui_exchange == null)
        {
            return;
        }
        this.ui_exchange.OnGetVIPCardInfo_SC(msg.vipcardinfo);
    }

    internal void RetBuyVIPCard_SC(MSG_RetBuyVIPCard_SC msg)
    {
        if (this.ui_exchange == null)
        {
            return;
        }
        this.ui_exchange.OnGetVIPCardInfo_SC(msg.vipcardinfo);
    }

    public void RegOnPlayerDataChangeAction(Action cb, bool regOrUnreg)
    {
        MainPlayerData component = MainPlayer.Self.GetComponent<MainPlayerData>();
        if (component != null)
        {
            component.ListenCharBaseDataChange(cb, regOrUnreg);
        }
    }

    internal void RetAcepVIPCardPrize_SC(MSG_RetAcepVIPCardPrize_SC msg)
    {
        if (this.ui_exchange == null)
        {
            return;
        }
        this.ui_exchange.OnGetVIPCardInfo_SC(msg.vipcardinfo);
    }

    internal void ReqRaf(RaffUseType rafType)
    {
        if (this.ui_exchange == null)
        {
            return;
        }
        this.mNetWork.ReqRaffVIPCardPrize_CS(rafType);
    }

    internal void RetRaffVIPCardPrize_SC(MSG_RetRaffVIPCardPrize_SC msg)
    {
        if (this.ui_exchange == null)
        {
            return;
        }
        this.ui_exchange.OnGetVIPCardInfo_SC(msg.vipcardinfo);
        this.ui_exchange.OnRaffVIPCardPrize_SC(msg.id, msg.quantity);
    }

    public void ReqBuyVipCard(uint cardid, uint count)
    {
        this.mNetWork.ReqBuyVIPCard_CS(cardid, count);
    }

    public void ReqGetDailyAward()
    {
        this.mNetWork.ReqAcepVIPCardPrize_CS();
    }

    public VipPrivilegeNetWork mNetWork;
}
