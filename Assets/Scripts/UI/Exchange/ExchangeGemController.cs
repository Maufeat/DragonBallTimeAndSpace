using System;
using Models;

namespace UI.Exchange
{
    public class ExchangeGemController : ControllerBase
    {
        public UIPanelBase uiLogin
        {
            get
            {
                return UIManager.GetUIObject<UI_ExchangeGem>();
            }
        }

        public void Close()
        {
            UI_ExchangeGem uiobject = UIManager.GetUIObject<UI_ExchangeGem>();
            if (null != uiobject)
            {
                uiobject.close();
            }
        }

        public override void Awake()
        {
            this.exchangeNetWork = new ExchangeGemNetWork();
            this.exchangeNetWork.Initialize();
        }

        public override void OnUpdate()
        {
        }

        public override string ControllerName
        {
            get
            {
                return "exchangegem_controller";
            }
        }

        public void ReqRecharge(uint point)
        {
            this.exchangeNetWork.ReqRecharge(point);
        }

        public void ReqQueryBalance()
        {
            this.exchangeNetWork.ReqQueryBalance();
        }

        public void UpdatePoint(uint point)
        {
            UI_ExchangeGem uiobject = UIManager.GetUIObject<UI_ExchangeGem>();
            if (null != uiobject)
            {
                uiobject.UpdateLeftPoint(point);
            }
        }

        public void UpdateRate(uint rate)
        {
            UI_ExchangeGem uiobject = UIManager.GetUIObject<UI_ExchangeGem>();
            if (null != uiobject)
            {
                uiobject.UpdateRate(rate);
            }
        }

        public void ShowUI()
        {
            UI_ExchangeGem.LoadView(null);
        }

        public ExchangeGemNetWork exchangeNetWork;
    }
}
