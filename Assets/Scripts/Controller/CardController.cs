using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using Obj;
using UnityEngine;

public class CardController : ControllerBase
{
    public UI_Character mUICard
    {
        get
        {
            return UIManager.GetUIObject<UI_Character>();
        }
    }

    public override void Awake()
    {
        this.Init();
    }

    public void Init()
    {
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        this.ITEM_USE_LEVEL_NOT_ENOUGH = controller.GetContentByIDWithoutColorText("70000");
        this.CARD_GROUP_EXITS = controller.GetContentByIDWithoutColorText("70001");
        this.CARD_ITEM_TYPE_FULL = controller.GetContentByIDWithoutColorText("70002");
        this.CAN_NOT_CHANGE_ON_BATTLE_STATE = CommonTools.GetTextById(4006UL);
        this.mCardNetWork = new CardNetWorker();
        this.mCardNetWork.Initialize();
        DragDropManager.Instance.RegisterPutInCb(UIRootType.Card, new TwoBtnCb(this.PutInCard));
        DragDropManager.Instance.RegisterPutOutCb(UIRootType.Card, new TwoBtnCb(this.PutOutCard));
        DragDropManager.Instance.RegisterUseItemCb(UIRootType.Card, new OneBtnCb(this.UseCard));
        DragDropManager.Instance.RegisterDestoryItemCb(UIRootType.Card, new OneBtnCb(this.DestoryCard));
        DragDropManager.Instance.RegisterDropDownCheckCb(UIRootType.Card, new DropDownCheckCb(this.DropDownCheckCard));
        DragDropManager.Instance.RegisterDragUpCheckCb(UIRootType.Card, new DragUpCheckCb(this.DragUpCheckCard));
    }

    private void PutInCard(DragDropButton btnFrom, DragDropButton btnTo)
    {
        if (btnFrom.mUIRootType == UIRootType.Bag)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)btnFrom.mData.mId);
            if (configTable != null && configTable.GetField_Uint("type") == 53U)
            {
                this.ReqPutOnCard(PackType.PACK_TYPE_MAIN, btnFrom.mData.thisid, (uint)btnTo.mPos.y);
            }
        }
        else if (btnFrom.mUIRootType == UIRootType.Card)
        {
            if (btnTo.mData == null)
            {
                this.ReqPutOnCard(PackType.PACK_TYPE_HERO_CARD, btnFrom.mData.thisid, (uint)btnTo.mPos.y);
            }
            else if (btnFrom.mData.thisid != btnTo.mData.thisid)
            {
                this.ReqSwapCard(btnFrom.mData.thisid, btnTo.mData.thisid);
            }
        }
    }

    private void PutOutCard(DragDropButton btnFrom, DragDropButton btnTo)
    {
        if (btnTo.mUIRootType == UIRootType.Bag && btnFrom.mData != null)
        {
            this.ReqPutOffCard(btnFrom.mData.thisid, 0, Mathf.CeilToInt(btnTo.mPos.y));
        }
    }

    public void UpdateFightValue()
    {
        if (this.mUICard == null)
        {
            return;
        }
        this.mUICard.RefreshFightValue();
    }

    private void UseCard(DragDropButton btnFrom)
    {
        if (btnFrom.mData != null)
        {
            object obj = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.IsMainPackageFull", new object[0])[0];
            if (!(bool)obj)
            {
                this.ReqPutOffCard(btnFrom.mData.thisid, -1, -1);
            }
            else
            {
                TipsWindow.ShowWindow(4013U);
            }
        }
    }

    public bool CheckBagIsFull()
    {
        return this.bagFullState;
    }

    public void ReqBagFullState()
    {
        object obj = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.IsMainPackageFull", new object[0])[0];
        this.bagFullState = (bool)obj;
    }

    private void DestoryCard(DragDropButton btnFrom)
    {
        UIBagManager.Instance.DoDestoryItem(btnFrom.mData.mId, btnFrom.mData.thisid, PackType.PACK_TYPE_HERO_CARD);
    }

    public void StartDragCard(DragDropButton btnFrom)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)btnFrom.mData.mId);
        if (configTable != null && this.mUICard != null)
        {
            this.mUICard.StartDragCard(configTable.GetField_Uint("cardtype"));
        }
    }

    public void CacelDragCard()
    {
        if (this.mUICard != null)
        {
            this.mUICard.CacelDragCard();
        }
    }

    public bool EqualCurDragButtonType(DragDropButton btnTo)
    {
        DragDropButton curDragButton = DragDropManager.Instance.GetCurDragButton();
        LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)curDragButton.mData.mId);
        if (configTable != null && btnTo != null)
        {
            int field_Uint = (int)configTable.GetField_Uint("cardtype");
            if (btnTo.mPos.y >= (float)((field_Uint - 1) * 10) && btnTo.mPos.y < (float)(field_Uint * 10))
            {
                return true;
            }
        }
        return false;
    }

    public bool EnterCheckCard(DragDropButton btnTo)
    {
        return this.DoDropDownCheck(DragDropManager.Instance.GetCurDragButton(), btnTo, false);
    }

    public bool DragUpCheckCard(DragDropButton btnFrom)
    {
        return true;
    }

    private bool DropDownCheckCard(DragDropButton btnFrom, DragDropButton btnTo)
    {
        return this.DoDropDownCheck(btnFrom, btnTo, true);
    }

    private bool DoDropDownCheck(DragDropButton btnFrom, DragDropButton btnTo, bool isShowTip = false)
    {
        if (GameSystemSettings.IsMainPlayerInBattleState())
        {
            TipsWindow.ShowNotice(this.CAN_NOT_CHANGE_ON_BATTLE_STATE);
            return false;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)btnFrom.mData.mId);
        if (configTable == null || configTable.GetField_Uint("type") != 53U)
        {
            return false;
        }
        uint curLv = ControllerManager.Instance.GetController<MainUIController>().GetCurLv();
        if (configTable.GetField_Uint("uselevel") > curLv)
        {
            if (isShowTip)
            {
                TipsWindow.ShowNotice(this.ITEM_USE_LEVEL_NOT_ENOUGH);
            }
            return false;
        }
        t_Object itemData = ControllerManager.Instance.GetController<ItemTipController>().GetItemData(btnFrom.mData.thisid);
        if (itemData != null && itemData.card_data != null && itemData.card_data.durability == 0U)
        {
            TipsWindow.ShowNotice("耐久度为0");
            return false;
        }
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("carddata_config", (ulong)btnFrom.mData.mId);
        if (configTable2 != null)
        {
            uint field_Uint = configTable2.GetField_Uint("cardtype");
            uint field_Uint2 = configTable2.GetField_Uint("cardgroup");
            if (field_Uint == 0U)
            {
                for (uint num = 1U; num < 6U; num += 1U)
                {
                    if (btnTo.mPos.y >= (num - 1U) * 10U && btnTo.mPos.y < (num - 1U) * 10U + this.GetCardTypeSlotOpenNum(num))
                    {
                        if (!this.alreadExitsCardGroupDic.ContainsKey(field_Uint2) || (this.alreadExitsCardGroupDic.ContainsKey(field_Uint2) && this.alreadExitsCardGroupDic[field_Uint2] == btnTo.mPos.y) || btnFrom.mUIRootType == UIRootType.Card)
                        {
                            return true;
                        }
                        if (isShowTip)
                        {
                            TipsWindow.ShowNotice(this.CARD_GROUP_EXITS);
                        }
                    }
                }
            }
            else if (btnTo.mPos.y >= (field_Uint - 1U) * 10U && btnTo.mPos.y < (field_Uint - 1U) * 10U + this.GetCardTypeSlotOpenNum(field_Uint))
            {
                if (!this.alreadExitsCardGroupDic.ContainsKey(field_Uint2) || (this.alreadExitsCardGroupDic.ContainsKey(field_Uint2) && this.alreadExitsCardGroupDic[field_Uint2] == btnTo.mPos.y) || btnFrom.mUIRootType == UIRootType.Card)
                {
                    return true;
                }
                if (isShowTip)
                {
                    TipsWindow.ShowNotice(this.CARD_GROUP_EXITS);
                }
            }
        }
        return false;
    }

    public void OpenCharacterUI()
    {
        if (UIManager.GetUIObject<UI_Character>() != null)
        {
            UIManager.Instance.DeleteUI<UI_Character>();
        }
        else
        {
            UIManager.Instance.ShowUI<UI_Character>("UI_Character", delegate ()
            {
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void SetupCardPackInfo(CardPackInfo cardPackInfo)
    {
        this.mCardPackInfo = cardPackInfo;
        if (this.mUICard != null)
        {
            this.mUICard.SetupCardPanel(cardPackInfo);
        }
        this.alreadExitsCardGroupDic = new Dictionary<uint, uint>();
        for (uint num = 0U; num < 50U; num += 1U)
        {
            t_Object cardByPos = this.GetCardByPos(num);
            if (cardByPos != null)
            {
                LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)cardByPos.baseid);
                if (configTable != null)
                {
                    uint field_Uint = configTable.GetField_Uint("cardgroup");
                    if (field_Uint != 0U && !this.alreadExitsCardGroupDic.ContainsKey(field_Uint))
                    {
                        this.alreadExitsCardGroupDic.Add(field_Uint, num);
                    }
                }
            }
        }
    }

    public uint GetCardTypeSlotNum(uint cardtype)
    {
        ulong num = (ulong)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.heroid;
        LuaTable configTable = LuaConfigManager.GetConfigTable("heros", (ulong)((uint)num));
        if (configTable != null)
        {
            string field_String = configTable.GetField_String("maxcard");
            string[] array = field_String.Split(new string[]
            {
                "-"
            }, StringSplitOptions.RemoveEmptyEntries);
            return uint.Parse(array[(int)((UIntPtr)(cardtype - 1U))]);
        }
        return 10U;
    }

    public uint GetCardTypeSlotOpenNum(uint cardtype)
    {
        switch (cardtype)
        {
            case 1U:
                return this.mCardPackInfo.gold_opened_num;
            case 2U:
                return this.mCardPackInfo.wood_opened_num;
            case 3U:
                return this.mCardPackInfo.water_opened_num;
            case 4U:
                return this.mCardPackInfo.fire_opened_num;
            case 5U:
                return this.mCardPackInfo.earth_opened_num;
            default:
                return this.mCardPackInfo.gold_opened_num;
        }
    }

    public t_Object GetCardByPos(uint pos)
    {
        for (int i = 0; i < this.mCardPackInfo.objs.Count; i++)
        {
            if (this.mCardPackInfo.objs[i].grid_y == pos)
            {
                return this.mCardPackInfo.objs[i];
            }
        }
        return null;
    }

    public uint GetAllDurabilitys()
    {
        uint num = 0U;
        for (int i = 0; i < this.mCardPackInfo.objs.Count; i++)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)this.mCardPackInfo.objs[i].baseid);
            uint field_Uint = configTable.GetField_Uint("maxdurable");
            num += field_Uint - this.mCardPackInfo.objs[i].card_data.durability;
        }
        return num;
    }

    public void ReqPutOnCard(PackType packtype, DragDropButton dragDropButton)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)dragDropButton.mData.mId);
        if (configTable != null)
        {
            int num = -1;
            uint field_Uint = configTable.GetField_Uint("cardtype");
            int num2 = (int)((field_Uint - 1U) * 10U);
            while ((long)num2 < (long)((ulong)((field_Uint - 1U) * 10U + this.GetCardTypeSlotOpenNum(field_Uint))))
            {
                if (this.GetCardByPos((uint)num2) == null)
                {
                    num = num2;
                    break;
                }
                num2++;
            }
            if (num != -1)
            {
                if (this.DoDropDownCheck(dragDropButton, new DragDropButton
                {
                    mPos = new Vector2(0f, (float)num)
                }, true))
                {
                    this.ReqPutOnCard(packtype, dragDropButton.mData.thisid, (uint)num);
                }
            }
            else
            {
                TipsWindow.ShowNotice(this.CARD_ITEM_TYPE_FULL);
            }
        }
    }

    public void ReqCardPackInfo()
    {
        this.mCardNetWork.ReqCardPackInfo();
    }

    public void ReqPutOnCard(PackType packtype, string thisid, uint grid_y)
    {
        this.mCardNetWork.ReqPutOnCard(packtype, thisid, grid_y);
    }

    public void ReqPutOffCard(string thisid, int grid_x, int grid_y)
    {
        this.mCardNetWork.ReqPutOffCard(thisid, grid_x, grid_y);
    }

    public void ReqSwapCard(string src_thisid, string dst_thisid)
    {
        this.mCardNetWork.ReqSwapCard(src_thisid, dst_thisid);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override string ControllerName
    {
        get
        {
            return "card_controller";
        }
    }

    public const int ROW_MAX_NUM = 10;

    private const int CARD_TYPE_NUM = 5;

    private CardNetWorker mCardNetWork;

    private CardPackInfo mCardPackInfo;

    private string ITEM_USE_LEVEL_NOT_ENOUGH = "道具使用需求玩家等级不足";

    private string CARD_GROUP_EXITS = "已存在相同卡组卡";

    private string CARD_ITEM_TYPE_FULL = "能力卡类型卡已满";

    private string CAN_NOT_CHANGE_ON_BATTLE_STATE = "无法在战斗中更换你的能力卡";

    private Dictionary<uint, uint> alreadExitsCardGroupDic;

    private bool bagFullState;
}
