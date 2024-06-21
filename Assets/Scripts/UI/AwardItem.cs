using System;
using System.Collections.Generic;
using UnityEngine;

public class AwardItem
{
    public void SetGameOjcet(GameObject root)
    {
        this.Root = root;
    }

    public void SetData(PropsBase prop)
    {
        this.Prop = prop;
        if (this.Prop == null)
        {
            return;
        }
        CommonItem commonItem = new CommonItem(this.Root.transform);
        commonItem.SetCommonItem(this.Prop.config.GetField_Uint("id"), this.Prop.Count, new Action<uint>(this.ClickItem));
        this.citems.Add(commonItem);
    }

    private void ClickItem(uint id)
    {
        if (this.Prop == null)
        {
            return;
        }
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenInfo", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.Prop,
            false
        });
    }

    public void Hide()
    {
        if (this.Root != null)
        {
            this.Root.SetActive(false);
        }
    }

    public void Show()
    {
        if (this.Root != null && this.Prop != null)
        {
            this.Root.SetActive(true);
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < this.citems.Count; i++)
        {
            this.citems[i].Dispose();
        }
        this.citems.Clear();
    }

    public GameObject Root;

    private PropsBase Prop;

    private List<CommonItem> citems = new List<CommonItem>();
}
