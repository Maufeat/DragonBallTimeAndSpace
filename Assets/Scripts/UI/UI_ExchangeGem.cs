using System;
using Framework.Managers;
using UI.Exchange;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExchangeGem : UIPanelBase
{
    public static void LoadView(Action Loadover)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ExchangeGem>("UI_Exchange", Loadover, UIManager.ParentType.CommonUI, false);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        GameObject gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/btn_change").gameObject;
        Button component = gameObject.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.doExchange();
        });
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/btn_close").gameObject;
        Button component2 = gameObject.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(delegate ()
        {
            this.close();
        });
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/texchange_data/txt_value").gameObject;
        this.TextRate = gameObject.GetComponent<Text>();
        this.TextRate.text = "0:0";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/texchange_data/txt_value_gold").gameObject;
        this.TextGem = gameObject.GetComponent<Text>();
        this.TextGem.text = "0";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/exchange_input/Panel/txt_value_sony").gameObject;
        this.TextTarget = gameObject.GetComponent<Text>();
        this.TextTarget.text = "0";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/exchange_input/InputField").gameObject;
        this.GemInput = gameObject.GetComponent<InputField>();
        this.GemInput.text = "0";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/txt_title").gameObject;
        Text component3 = gameObject.GetComponent<Text>();
        component3.text = "钻石兑换";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/texchange_data/txt_title_gold").gameObject;
        Text component4 = gameObject.GetComponent<Text>();
        component4.text = "我的点数";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/texchange_data/Image").gameObject;
        Image component5 = gameObject.GetComponent<Image>();
        if (null != component5)
        {
            UnityEngine.Object.DestroyImmediate(component5);
        }
        RawImage rimage = gameObject.AddComponent<RawImage>();
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, "ic0033", delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (rimage == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            rimage.texture = sprite.texture;
            rimage.color = Color.white;
        });
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/exchange_input/txt_title").gameObject;
        Text component6 = gameObject.GetComponent<Text>();
        component6.text = "输入点数";
        gameObject = this.Root.Find("Offset_Exchange/Panel_Exchange/exchange_input/Panel/txt_sony").gameObject;
        Text component7 = gameObject.GetComponent<Text>();
        component7.text = "钻石";
        this.GemInput.onValueChanged.AddListener(delegate (string v)
        {
            this.onInputChanged(v);
        });
        ControllerManager.Instance.GetController<ExchangeGemController>().ReqQueryBalance();
    }

    private void doExchange()
    {
        string text = this.GemInput.text;
        uint num = 0U;
        uint.TryParse(text, out num);
        if (num <= 0U)
        {
            return;
        }
        ControllerManager.Instance.GetController<ExchangeGemController>().ReqRecharge(num);
    }

    public void close()
    {
        if (this.Root != null)
        {
            UnityEngine.Object.Destroy(this.Root.gameObject);
        }
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Exchange");
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        });
    }

    private void onInputChange(string txt)
    {
        if (string.IsNullOrEmpty(txt))
        {
            return;
        }
        int num = 0;
        if (!int.TryParse(txt, out num))
        {
            this.GemInput.text = string.Empty;
            return;
        }
        if ((long)num > (long)((ulong)this._curPoint))
        {
            this.GemInput.text = this._curPoint.ToString();
            return;
        }
    }

    private void onInputChanged(string txt)
    {
        if (string.IsNullOrEmpty(txt))
        {
            return;
        }
        int num = 0;
        int.TryParse(txt, out num);
        this.TextTarget.text = ((long)num * (long)((ulong)this._rate)).ToString();
    }

    public void UpdateLeftPoint(uint point)
    {
        this.TextGem.text = point.ToString();
        this._curPoint = point;
    }

    public void UpdateRate(uint rate)
    {
        this.TextRate.text = "1:" + rate.ToString();
        this._rate = rate;
        this.onInputChanged(this.GemInput.text);
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public Transform Root;

    public Text TextGem;

    public Text TextRate;

    public Text TextTarget;

    public InputField GemInput;

    private uint _rate;

    private uint _curPoint;
}
