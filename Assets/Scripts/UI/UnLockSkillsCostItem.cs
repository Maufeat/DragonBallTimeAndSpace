using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UnLockSkillsCostItem : MonoBehaviour
{
    private void OnInit()
    {
        Transform transform = base.transform.Find("img_icon");
        Transform transform2 = base.transform.Find("txt_num");
        if (null != transform)
        {
            this.mIcon = transform.GetComponent<RawImage>();
        }
        if (null != transform2)
        {
            this.mNumber = transform2.GetComponent<Text>();
        }
    }

    public void Initlize(string _icon, uint _num)
    {
        this.OnInit();
        if (null != this.mIcon)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, _icon, delegate (UITextureAsset item)
            {
                if (item != null && item.textureObj != null && null != this.mIcon)
                {
                    this.mIcon.texture = item.textureObj;
                    this.mIcon.color = Color.white;
                }
            });
        }
        if (null != this.mNumber)
        {
            this.mNumber.text = _num.ToString();
            this.mNumber.color = this.m_itemState;
        }
    }

    public void Initlize(LvCostItem _item)
    {
        this.m_costItem = _item;
        UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
        if (controller != null)
        {
            UI_UnLockSkills uiobject = UIManager.GetUIObject<UI_UnLockSkills>();
            if (null != uiobject)
            {
                this.m_itemState = uiobject.GetCostResourcesState(controller.IsEnoughItem(_item.m_id));
            }
            this.Initlize(controller.GetObjectIconName(_item.m_id), _item.m_num);
        }
    }

    public RawImage mIcon;

    public Text mNumber;

    public LvCostItem m_costItem;

    private Color m_itemState = Color.white;
}
