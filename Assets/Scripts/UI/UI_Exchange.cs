using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Exchange : UIPanelBase
{
    public override void OnDispose()
    {
        base.Dispose();
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.rootObject = root.gameObject;
        this.ratio = LuaConfigManager.GetXmlConfigTable("newUserInit").GetCacheField_Uint("gembuygold");
        this.stone = GlobalRegister.GetCurrencyByID(2U);
        this.InitObject();
        this.InitEvent();
        this.InitUIData();
        ExchangeController controller = ControllerManager.Instance.GetController<ExchangeController>();
        controller.ReqRatio();
    }

    private void InitObject()
    {
        this.btnClose = this.rootObject.transform.Find("Offset_Exchange/Panel_Exchange/btn_close");
        this.textExchangeRatio = this.rootObject.transform.Find("Offset_Exchange/Panel_Exchange/texchange_data/txt_value");
        this.textStone = this.rootObject.transform.Find("Offset_Exchange/Panel_Exchange/texchange_data/txt_value_gold");
        this.inputExchangeStone = this.rootObject.transform.Find("Offset_Exchange/Panel_Exchange/exchange_input/InputField");
        this.textCanExchangeCount = this.rootObject.transform.Find("Offset_Exchange/Panel_Exchange/exchange_input/Panel/txt_value_sony");
        this.btnExchange = this.rootObject.transform.Find("Offset_Exchange/Panel_Exchange/btn_change");
    }

    private void InitEvent()
    {
        Button component = this.btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Close));
        InputField component2 = this.inputExchangeStone.GetComponent<InputField>();
        component2.onValueChanged.RemoveAllListeners();
        component2.onValueChanged.AddListener(new UnityAction<string>(this.OnInputChange));
        Button component3 = this.btnExchange.GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        component3.onClick.AddListener(new UnityAction(this.StartExchange));
    }

    private void InitUIData()
    {
        Text component = this.textCanExchangeCount.GetComponent<Text>();
        uint num = 0U;
        string text = this.inputExchangeStone.GetComponent<InputField>().text;
        uint.TryParse(text, out num);
        this.curExchangeCount = num * this.ratio;
        component.text = string.Empty + this.curExchangeCount;
        this.RefrashStoneHave();
    }

    internal void OnGetRatio(uint tone2nations)
    {
        this.ratio = tone2nations;
        Text component = this.textExchangeRatio.GetComponent<Text>();
        component.text = 1 + ":" + this.ratio;
    }

    public void RefrashStoneHave()
    {
        this.stone = GlobalRegister.GetCurrencyByID(2U);
        Text component = this.textStone.GetComponent<Text>();
        component.text = string.Empty + this.stone;
    }

    private void Close()
    {
        UIManager.Instance.DeleteUI<UI_Exchange>();
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        });
    }

    public uint GetExchangeSonyCount()
    {
        return this.curExchangeCount;
    }

    private void OnInputChange(string s)
    {
        Text component = this.textCanExchangeCount.GetComponent<Text>();
        InputField component2 = this.inputExchangeStone.GetComponent<InputField>();
        uint num = 0U;
        if (uint.TryParse(s, out num))
        {
            num = (uint)Mathf.Min(num, this.stone);
            component2.text = string.Empty + num;
            this.curExchangeCount = this.ratio * num;
            component.text = string.Empty + this.curExchangeCount;
        }
        else
        {
            component2.text = "1";
        }
    }

    private void StartExchange()
    {
        InputField component = this.inputExchangeStone.GetComponent<InputField>();
        uint count = 0U;
        if (uint.TryParse(component.text, out count))
        {
            ExchangeController controller = ControllerManager.Instance.GetController<ExchangeController>();
            controller.Exchange(count);
        }
    }

    private GameObject rootObject;

    private Transform btnClose;

    private Transform textExchangeRatio;

    private Transform textStone;

    private Transform inputExchangeStone;

    private Transform textCanExchangeCount;

    private Transform btnExchange;

    private uint ratio;

    private uint stone;

    private uint curExchangeCount;
}
