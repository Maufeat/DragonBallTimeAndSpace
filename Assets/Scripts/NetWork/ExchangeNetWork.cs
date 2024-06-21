using System;
using Framework.Managers;
using market;
using Net;
using Obj;

public class ExchangeNetWork : NetWorkBase
{
    private ExchangeController ec
    {
        get
        {
            return ControllerManager.Instance.GetController<ExchangeController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetCurrencyExchange_SC>(CommandID.MSG_RetCurrencyExchange_SC, new ProtoMsgCallback<MSG_RetCurrencyExchange_SC>(this.MSG_RetCurrencyExchange_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ExchangeRatio_CSC>(CommandID.MSG_ExchangeRatio_CSC, new ProtoMsgCallback<MSG_ExchangeRatio_CSC>(this.MSG_ExchangeRatio_CSC));
    }

    private void MSG_RetCurrencyExchange_SC(MSG_RetCurrencyExchange_SC msg)
    {
        if (this.ec != null)
        {
            this.ec.MSG_RetCurrencyExchange_SC(msg);
        }
    }

    public void MSG_ReqCurrencyExchange_SC(uint count)
    {
        base.SendMsg<MSG_ReqCurrencyExchange_CS>(CommandID.MSG_ReqCurrencyExchange_CS, new MSG_ReqCurrencyExchange_CS
        {
            usequantity = count
        }, false);
    }

    public void MSG_ExchangeRatio_CSC()
    {
        MSG_ExchangeRatio_CSC t = new MSG_ExchangeRatio_CSC();
        base.SendMsg<MSG_ExchangeRatio_CSC>(CommandID.MSG_ExchangeRatio_CSC, t, false);
    }

    public void MSG_ExchangeRatio_CSC(MSG_ExchangeRatio_CSC msg)
    {
        if (this.ec != null)
        {
            this.ec.MSG_ExchangeRatio_CSC(msg);
        }
    }
}
