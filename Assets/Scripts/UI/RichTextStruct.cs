using System;
using Chat;
using UnityEngine;
using UnityEngine.UI;

public class RichTextStruct
{
    public RichTextStruct(RichTextType _type, string _content, RichTextLine line, ChatLink link, string color = "", bool withunderline = false)
    {
        this.textType = _type;
        this.content = _content;
        this.Obj = UnityEngine.Object.Instantiate<GameObject>(line.Tran.Find("text").gameObject);
        this.Obj.transform.SetParent(line.Tran);
        Text component = this.Obj.GetComponent<Text>();
        component.text = this.content;
        this.Obj.transform.localScale = Vector3.one;
        Image component2 = this.Obj.transform.Find("underline").GetComponent<Image>();
        if (!string.IsNullOrEmpty(color))
        {
            Color color2 = CommonTools.HexToColor(color);
            component.color = color2;
            component2.color = color2;
        }
        if (withunderline)
        {
            component2.gameObject.SetActive(true);
            component2.rectTransform.sizeDelta = new Vector2(component.preferredWidth, 2f);
        }
        else
        {
            component2.gameObject.SetActive(false);
        }
        if (_type == RichTextType.HyperText)
        {
            Button component3 = this.Obj.GetComponent<Button>();
            component3.enabled = true;
            component3.onClick.RemoveAllListeners();
            component3.onClick.AddListener(delegate ()
            {
                LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.NormalDisplayChatUI", new object[]
                {
                    Util.GetLuaTable("MainUICtrl")
                });
                if (line.richText.chatData.charid == MainPlayer.Self.GetCharID())
                {
                    FFDebug.LogWarning(this, "My Convenient!!!");
                    return;
                }
                ConvenientProcess.ProcessConvenient(link);
            });
        }
        else if (_type == RichTextType.CharName)
        {
            Button btn = this.Obj.GetComponent<Button>();
            btn.enabled = true;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate ()
            {
                LuaScriptMgr.Instance.CallLuaFunction("PlayerOperateCtrl.ReqPlayerInfo", new object[]
                {
                    Util.GetLuaTable("PlayerOperateCtrl"),
                    line.richText.chatData.charid,
                    btn.gameObject
                });
                LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.NormalDisplayChatUI", new object[]
                {
                    Util.GetLuaTable("MainUICtrl")
                });
            });
        }
        this.Obj.SetActive(true);
    }

    public float GetWidth()
    {
        float num = 0f;
        if (this.textType == RichTextType.NormalText || this.textType == RichTextType.HyperText || this.textType == RichTextType.CharName)
        {
            num += UITools.GetTextWidth(this.Obj.GetComponent<Text>());
        }
        return num;
    }

    public void DestroyThis()
    {
        GameObject obj = this.Obj;
        UnityEngine.Object.Destroy(obj);
        this.Obj = null;
    }

    private RichTextType textType;

    private string content;

    private GameObject Obj;
}
