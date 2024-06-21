using System;
using Framework.Managers;
using massive;
using Models;

public class GiftBagController : ControllerBase
{
    public UI_GiftBag uiGiftBag
    {
        get
        {
            return UIManager.GetUIObject<UI_GiftBag>();
        }
    }

    public void RetGiftBag(MSG_Ret_SuccessOpenGift_SC giftInfo)
    {
    }

    public void CloseGiftBag()
    {
        ControllerManager.Instance.GetController<ShortCutUseEquipController>().ShowAddItemList();
        if (this.uiGiftBag != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Award");
        }
    }

    public override void Awake()
    {
        this.giftbagNetWork = new GiftBagNetWork();
        this.giftbagNetWork.Initialize();
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "giftbag_controller";
        }
    }

    public GiftBagNetWork giftbagNetWork;
}
