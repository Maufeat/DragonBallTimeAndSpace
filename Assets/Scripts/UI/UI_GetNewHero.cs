using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_GetNewHero : UIPanelBase
{
    public override void OnDispose()
    {
        base.Dispose();
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.m_Root = root.gameObject;
        this.InitGameObject();
        this.m_start = false;
    }

    private void InitGameObject()
    {
        this.m_itemObj = this.m_Root.transform.Find("Offset_ItemUse/Panel_gethero/Item").gameObject;
        this.m_itemObj.SetActive(false);
    }

    public void AddHeroId(ulong heroId)
    {
        this.m_heroIdQuene.Enqueue(heroId);
        if (!this.m_start)
        {
            this.m_start = true;
            this.ShowGetNewHeroTips();
        }
    }

    public void ShowGetNewHeroTips()
    {
        ulong id = this.m_heroIdQuene.Dequeue();
        GameObject itmObj = UnityEngine.Object.Instantiate<GameObject>(this.m_itemObj);
        itmObj.name = id.ToString();
        itmObj.transform.SetParent(this.m_itemObj.transform.parent);
        itmObj.transform.localScale = Vector3.one;
        itmObj.transform.localPosition = Vector3.zero;
        itmObj.SetActive(false);
        Image img = itmObj.FindChild("groove/role").GetComponent<Image>();
        Text text = itmObj.FindChild("txt_name").GetComponent<Text>();
        TweenAlpha[] components = itmObj.GetComponents<TweenAlpha>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].delay > 0f)
            {
                components[i].onFinished = delegate (UITweener tweener)
                {
                    if (this.m_heroIdQuene.Count > 0)
                    {
                        itmObj.SetActive(false);
                        this.ShowGetNewHeroTips();
                    }
                    else
                    {
                        UIManager.Instance.DeleteUI<UI_GetNewHero>();
                    }
                };
                break;
            }
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", id);
        string field_String = configTable.GetField_String("UIicon");
        string heroName = configTable.GetCacheField_String("name");
        base.GetSprite(ImageType.ROLES, field_String, delegate (Sprite item)
        {
            if (item != null)
            {
                text.text = heroName;
                img.overrideSprite = item;
                img.sprite = item;
                img.color = Color.white;
                img.material = null;
            }
            itmObj.SetActive(true);
        });
    }

    public GameObject m_Root;

    private GameObject m_itemObj;

    private Image m_img;

    private Text m_text;

    private TweenAlpha m_twenAlpha;

    private Queue<ulong> m_heroIdQuene = new Queue<ulong>();

    private bool m_start;
}
