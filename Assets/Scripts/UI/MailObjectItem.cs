using System;
using Framework.Managers;
using hero;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.UI;

public class MailObjectItem
{
    public MailObjectItem(UI_Mail mailParent, Transform go)
    {
        this.root = go.gameObject;
        this.parent = mailParent;
        this.icon = go.Find("Item/img_icon").GetComponent<RawImage>();
        this.txtCount = go.Find("Item/txt_count").GetComponent<Text>();
        this.txtCount.text = string.Empty;
    }

    public void UpdateMailItemInfo(t_Object itemobject)
    {
        this.txtCount.text = ((itemobject.num <= 1U) ? string.Empty : itemobject.num.ToString());
        CommonItem commonItem = new CommonItem(this.root.FindChild("Item").transform);
        commonItem.SetCommonItem(itemobject.baseid, itemobject.num, null);
        commonItem.SetCommonItemData(itemobject.baseid, "0", "PACK_TYPE_MAIN", itemobject.lock_end_time, 3, itemobject.bind);
    }

    public void UpdateMailItemInfo(Hero heroobject)
    {
        this.txtCount.text = string.Empty;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)heroobject.baseid);
        if (configTable == null)
        {
            return;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas(AtlasName.HeroItems.ToString(), configTable.GetCacheField_String("icon"), delegate (Sprite sprite)
        {
            if (this.icon == null)
            {
                return;
            }
            if (null != sprite)
            {
                this.icon.texture = sprite.texture;
            }
        });
    }

    public GameObject root;

    private UI_Mail parent;

    private RawImage icon;

    private Text txtCount;
}
