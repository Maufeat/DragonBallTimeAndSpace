using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class FashionItem : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Root
    {
        get
        {
            return this.root;
        }
    }

    public void SetFashionInfo(UI_Character par, GameObject r, int fashionid)
    {
        this.currFashionid = fashionid;
        this.parent = par;
        this.root = r;
        this.stayImage = this.root.FindChild("img_stay");
        this.iconImage = this.root.FindChild("break_icon").GetComponent<RawImage>();
        this.maskImage = this.root.FindChild("mask_skin");
        this.equipImage = this.root.FindChild("img_sign");
        this.selectedImage = this.root.FindChild("img_selected");
        this.SetSelect(false);
        UIEventListener.Get(this.root).onClick = delegate (PointerEventData evtData)
        {
            this.onSelectItem();
        };
    }

    public void RefreshItem(LuaTable it, bool equiped, bool actived)
    {
        this.equipImage.SetActive(equiped);
        this.maskImage.SetActive(!actived);
        this.getted = actived;
        this.parent.GetTexture(ImageType.ROLES, it.GetField_String("icon"), delegate (Texture2D item)
        {
            if (this.iconImage != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                this.iconImage.texture = sprite.texture;
                this.iconImage.color = Color.white;
            }
        });
    }

    private void onSelectItem()
    {
        this.parent.onSelectFashion(this.currFashionid);
    }

    public void SetSelect(bool selected)
    {
        this.selectedImage.SetActive(selected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.stayImage.SetActive(true);
        this.TryShowSkinTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.stayImage.SetActive(false);
    }

    private void TryShowSkinTip()
    {
        if (this.currFashionid > 0)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenSkinPanel(this.currFashionid, this.getted, base.gameObject);
        }
    }

    private UI_Character parent;

    private GameObject root;

    private int currFashionid;

    private GameObject stayImage;

    private RawImage iconImage;

    private GameObject maskImage;

    private GameObject equipImage;

    private GameObject selectedImage;

    private bool getted;
}
