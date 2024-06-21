using System;
using Framework.Managers;
using LuaInterface;
using market;
using Models;
using Obj;

public class ExchangeController : ControllerBase
{
    private UI_Exchange ui_exchange
    {
        get
        {
            return UIManager.GetUIObject<UI_Exchange>();
        }
    }

    public override void Awake()
    {
        this.mNetWork = new ExchangeNetWork();
        this.mNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "exchange_controller";
        }
    }

    public void Exchange(uint count)
    {
        this.mNetWork.MSG_ReqCurrencyExchange_SC(count);
    }

    internal void MSG_RetCurrencyExchange_SC(MSG_RetCurrencyExchange_SC msg)
    {
        if (msg.result > 0U)
        {
            MsgBoxController controller = ControllerManager.Instance.GetController<MsgBoxController>();
            if (controller != null)
            {
                LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", 3136UL);
                string des = string.Empty;
                if (configTable != null)
                {
                    string field_String = configTable.GetField_String("notice");
                    des = string.Format(field_String, this.ui_exchange.GetExchangeSonyCount());
                }
                UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
                {
                    UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
                    uiobject.BtnSwitch("btn_cancel", false);
                    uiobject.SetOkCb(delegate (string o)
                    {
                    }, null);
                    uiobject.SetContent(des, "提示", true);
                }, UIManager.ParentType.CommonUI, false);
            }
            if (this.ui_exchange != null)
            {
                this.ui_exchange.RefrashStoneHave();
            }
        }
    }

    internal void ReqRatio()
    {
        this.mNetWork.MSG_ExchangeRatio_CSC();
    }

    internal void MSG_ExchangeRatio_CSC(MSG_ExchangeRatio_CSC msg)
    {
        if (this.ui_exchange != null)
        {
            this.ui_exchange.OnGetRatio(msg.tone2nations);
        }
    }

    public ExchangeNetWork mNetWork;
}
