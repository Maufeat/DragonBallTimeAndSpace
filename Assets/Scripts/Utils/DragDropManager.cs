using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropManager
{
    public static DragDropManager Instance
    {
        get
        {
            if (DragDropManager._instance == null)
            {
                DragDropManager._instance = new DragDropManager();
            }
            return DragDropManager._instance;
        }
    }

    public DragDropButton GetCurDragButton()
    {
        return this.mCurDragButton;
    }

    public void Init()
    {
        this.mCurDragImage = GameObject.Find("UIRoot").FindChild("DragingIcon").GetComponent<Image>();
        this.uiMouseFllow = this.mCurDragImage.gameObject.AddComponent<UIMouseFllow>();
    }

    public void obj_mask_on_click(PointerEventData eventData)
    {
        if (this.mIsDraging && this.mDestoryCbDic.ContainsKey(this.mCurDragButton.mUIRootType))
        {
            this.mDestoryCbDic[this.mCurDragButton.mUIRootType](this.mCurDragButton);
        }
        this.SetDragStateUI(false);
        if (this.ItemTypeCheck(this.mCurDragButton.mData.mId, ObjectType.OBJTYPE_CARD))
        {
            ControllerManager.Instance.GetController<CardController>().CacelDragCard();
        }
    }

    public void RegisterPutInCb(UIRootType _type, TwoBtnCb _cb)
    {
        if (!this.mPutInCbDic.ContainsKey(_type))
        {
            this.mPutInCbDic.Add(_type, _cb);
        }
        else
        {
            this.mPutInCbDic[_type] = _cb;
        }
    }

    public void RegisterPutOutCb(UIRootType _type, TwoBtnCb _cb)
    {
        if (!this.mPutOutCbDic.ContainsKey(_type))
        {
            this.mPutOutCbDic.Add(_type, _cb);
        }
        else
        {
            this.mPutOutCbDic[_type] = _cb;
        }
    }

    public void RegisterUseItemCb(UIRootType _type, OneBtnCb _cb)
    {
        if (!this.mUseCbDic.ContainsKey(_type))
        {
            this.mUseCbDic.Add(_type, _cb);
        }
        else
        {
            this.mUseCbDic[_type] = _cb;
        }
    }

    public void RegisterLeftClickCb(UIRootType _type, OneBtnCb _cb)
    {
        if (!this.mLeftClickCbDic.ContainsKey(_type))
        {
            this.mLeftClickCbDic.Add(_type, _cb);
        }
        else
        {
            this.mLeftClickCbDic[_type] = _cb;
        }
    }

    public void RegisterDestoryItemCb(UIRootType _type, OneBtnCb _cb)
    {
        if (!this.mDestoryCbDic.ContainsKey(_type))
        {
            this.mDestoryCbDic.Add(_type, _cb);
        }
        else
        {
            this.mDestoryCbDic[_type] = _cb;
        }
    }

    public void RegisterDragUpCheckCb(UIRootType _type, DragUpCheckCb _cb)
    {
        if (!this.mDragUpCheckCbDic.ContainsKey(_type))
        {
            this.mDragUpCheckCbDic.Add(_type, _cb);
        }
        else
        {
            this.mDragUpCheckCbDic[_type] = _cb;
        }
    }

    public void RegisterDropDownCheckCb(UIRootType _type, DropDownCheckCb _cb)
    {
        if (!this.mDropDownCheckCbDic.ContainsKey(_type))
        {
            this.mDropDownCheckCbDic.Add(_type, _cb);
        }
        else
        {
            this.mDropDownCheckCbDic[_type] = _cb;
        }
    }

    public bool IsDraging()
    {
        return this.mIsDraging;
    }

    private void CopyImage(Image imgSource, Image imgTarget)
    {
        imgSource.sprite = imgTarget.sprite;
        imgSource.overrideSprite = imgTarget.overrideSprite;
        imgSource.color = Color.white;
    }

    private void SetRepairImage()
    {
        Transform transform = this.mCurDragImage.transform.Find("imgRepair");
        transform.gameObject.SetActive(false);
        t_Object itemData = ControllerManager.Instance.GetController<ItemTipController>().GetItemData(this.mCurDragButton.mData.thisid);
        if (itemData != null && itemData.card_data != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)this.mCurDragButton.mData.mId);
            if (configTable != null)
            {
                uint field_Uint = configTable.GetField_Uint("maxdurable");
                if (itemData.card_data.durability < field_Uint)
                {
                    transform.transform.localScale = new Vector3(1f, 1f - itemData.card_data.durability * 1f / field_Uint, 1f);
                    transform.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetDragStateUI(bool _isDraging)
    {
        if (this.mCurDragButton == null)
        {
            return;
        }
        this.mIsDraging = _isDraging;
        this.mCurDragImage.gameObject.SetActive(this.mIsDraging);
        Cursor.visible = !this.mIsDraging;
        this.mCurDragButton.mIcon.color = ((!this.mIsDraging) ? Color.white : Color.gray);
    }

    private bool ItemTypeCheck(uint itemId, ObjectType type)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)itemId);
        return configTable != null && configTable.GetField_Uint("type") == 53U;
    }

    private bool CheckCanDragUp(DragDropButton dragButton)
    {
        bool flag = true;
        if (this.mDragUpCheckCbDic.ContainsKey(dragButton.mUIRootType))
        {
            flag = this.mDragUpCheckCbDic[dragButton.mUIRootType](dragButton);
        }
        if (flag && this.ItemTypeCheck(dragButton.mData.mId, ObjectType.OBJTYPE_CARD))
        {
            ControllerManager.Instance.GetController<CardController>().StartDragCard(dragButton);
        }
        return flag;
    }

    private bool CheckCanDropDown(DragDropButton btnFrom, DragDropButton btnTo)
    {
        bool flag = true;
        if (this.mDropDownCheckCbDic.ContainsKey(btnTo.mUIRootType))
        {
            flag = this.mDropDownCheckCbDic[btnTo.mUIRootType](btnFrom, btnTo);
        }
        if (flag && this.ItemTypeCheck(btnFrom.mData.mId, ObjectType.OBJTYPE_CARD))
        {
            ControllerManager.Instance.GetController<CardController>().CacelDragCard();
        }
        return flag;
    }

    public void DragUp(DragDropButton _dragDropButton)
    {
        this.mCurDragButton = _dragDropButton;
        if (!this.CheckCanDragUp(_dragDropButton))
        {
            return;
        }
        this.CopyImage(this.mCurDragImage, this.mCurDragButton.mIcon);
        this.SetRepairImage();
        this.SetDragStateUI(true);
    }

    public void PutIn(DragDropButton mTargetDragButton)
    {
        if (mTargetDragButton == null || !this.CheckCanDropDown(this.mCurDragButton, mTargetDragButton))
        {
            this.SetDragStateUI(false);
            return;
        }
        if (mTargetDragButton != this.mCurDragButton)
        {
            Image mIcon = mTargetDragButton.mIcon;
            Image mIcon2 = this.mCurDragButton.mIcon;
            if (this.mPutInCbDic.ContainsKey(mTargetDragButton.mUIRootType))
            {
                this.mPutInCbDic[mTargetDragButton.mUIRootType](this.mCurDragButton, mTargetDragButton);
                this.CopyImage(mIcon, mIcon2);
            }
            if (this.mCurDragButton.mUIRootType != mTargetDragButton.mUIRootType && this.mPutOutCbDic.ContainsKey(this.mCurDragButton.mUIRootType))
            {
                this.mPutOutCbDic[this.mCurDragButton.mUIRootType](this.mCurDragButton, mTargetDragButton);
            }
        }
        this.SetDragStateUI(false);
    }

    public void Use(DragDropButton mTargetDragButton)
    {
        if (this.mUseCbDic.ContainsKey(mTargetDragButton.mUIRootType))
        {
            this.mUseCbDic[mTargetDragButton.mUIRootType](mTargetDragButton);
        }
    }

    public void LeftClick(DragDropButton mTargetDragButton)
    {
        if (this.mLeftClickCbDic.ContainsKey(mTargetDragButton.mUIRootType))
        {
            this.mLeftClickCbDic[mTargetDragButton.mUIRootType](mTargetDragButton);
        }
    }

    private static DragDropManager _instance;

    private DragDropButton mCurDragButton;

    private Image mCurDragImage;

    private UIMouseFllow uiMouseFllow;

    private bool mIsDraging;

    private Image imgTargetClone;

    private Dictionary<UIRootType, TwoBtnCb> mPutInCbDic = new Dictionary<UIRootType, TwoBtnCb>();

    private Dictionary<UIRootType, TwoBtnCb> mPutOutCbDic = new Dictionary<UIRootType, TwoBtnCb>();

    private Dictionary<UIRootType, OneBtnCb> mUseCbDic = new Dictionary<UIRootType, OneBtnCb>();

    private Dictionary<UIRootType, OneBtnCb> mLeftClickCbDic = new Dictionary<UIRootType, OneBtnCb>();

    private Dictionary<UIRootType, OneBtnCb> mDestoryCbDic = new Dictionary<UIRootType, OneBtnCb>();

    private Dictionary<UIRootType, DragUpCheckCb> mDragUpCheckCbDic = new Dictionary<UIRootType, DragUpCheckCb>();

    private Dictionary<UIRootType, DropDownCheckCb> mDropDownCheckCbDic = new Dictionary<UIRootType, DropDownCheckCb>();
}
