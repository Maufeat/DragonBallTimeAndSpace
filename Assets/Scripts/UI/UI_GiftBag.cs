using System;
using System.Collections.Generic;
using Framework.Managers;
using massive;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_GiftBag : UIPanelBase
{
    private GiftBagController GiftBagController
    {
        get
        {
            return ControllerManager.Instance.GetController<GiftBagController>();
        }
    }

    private void InitGameObject()
    {
        this.Item = this.mRoot.Find("Offset_Award/Panel_Award/list/Item").gameObject;
        this.closeMask = this.mRoot.Find("Offset_Award/img_mask").gameObject;
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.closeMask).onClick = delegate (PointerEventData data)
        {
            this.GiftBagController.CloseGiftBag();
        };
    }

    public void ViewGiftBag(MSG_Ret_SuccessOpenGift_SC giftInfo)
    {
        for (int i = 0; i < this.awardListObj.Count; i++)
        {
            UnityEngine.Object.Destroy(this.awardListObj[i]);
        }
        for (int j = 0; j < giftInfo.objs.Count; j++)
        {
            float delay = (float)j * this.timeInterval;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Item);
            gameObject.transform.SetParent(this.Item.transform.parent);
            gameObject.transform.localPosition = this.Item.transform.localPosition;
            gameObject.transform.localScale = this.Item.transform.localScale;
            TweenScale component = gameObject.GetComponent<TweenScale>();
            if (component != null)
            {
                component.delay = delay;
            }
            this.awardListObj.Add(gameObject);
            this.setGiftItem(giftInfo.objs[j], gameObject);
        }
    }

    private void setGiftItem(GiftItem Item, GameObject ga)
    {
        CommonItem commonItem = new CommonItem(ga.transform);
        commonItem.SetCommonItem(Item.objid, Item.num, null);
        this.commonItems.Add(commonItem);
        ga.SetActive(true);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        float cacheField_Float = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("ItemDelay").GetCacheField_Float("value");
        this.timeInterval = cacheField_Float / 1000f;
        this.InitGameObject();
        this.InitEvent();
    }

    public override void OnDispose()
    {
        base.Dispose();
        for (int i = 0; i < this.commonItems.Count; i++)
        {
            this.commonItems[i].Dispose();
            UnityEngine.Object.Destroy(this.awardListObj[i]);
        }
        this.commonItems.Clear();
        this.awardListObj.Clear();
    }

    private List<CommonItem> commonItems = new List<CommonItem>();

    private List<GameObject> awardListObj = new List<GameObject>();

    private Transform mRoot;

    private GameObject Item;

    private GameObject closeMask;

    private float timeInterval = 0.2f;
}
