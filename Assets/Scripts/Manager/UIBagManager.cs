using System;
using System.Collections;
using System.Collections.Generic;
using Engine;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Obj;
using UnityEngine;

public delegate bool DropDownCheckCb(DragDropButton btnFrom, DragDropButton btnTo);

public delegate void EscPanelCb();

public class UIBagManager
{

    public static UIBagManager Instance
    {
        get
        {
            if (UIBagManager._instance == null)
            {
                UIBagManager._instance = new UIBagManager();
            }
            return UIBagManager._instance;
        }
    }

    public void Initilize()
    {
        this.netWorker = new BagNetworker();
        this.netWorker.Initialize();
        DragDropManager.Instance.RegisterUseItemCb(UIRootType.Bag, new OneBtnCb(this.UseItemCb));
        DragDropManager.Instance.RegisterPutInCb(UIRootType.Bag, new TwoBtnCb(this.PutInCb));
        DragDropManager.Instance.RegisterDestoryItemCb(UIRootType.Bag, new OneBtnCb(this.DestoryItemCb));
        DragDropManager.Instance.RegisterDragUpCheckCb(UIRootType.Bag, new DragUpCheckCb(this.DragUpCheck));
        DragDropManager.Instance.RegisterLeftClickCb(UIRootType.Bag, new OneBtnCb(this.LeftClickCb));
        DragDropManager.Instance.RegisterDropDownCheckCb(UIRootType.Bag, new DropDownCheckCb(this.DropDownCheckCb));
        ManagerCenter.Instance.GetManager<EscManager>().RegisterEscPanelCb("UI_Bag", new EscPanelCb(this.EscCloseBagPanel));
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.CheckCallObjectUsetState));
    }

    private void EscCloseBagPanel()
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.TryEsc", new object[0]);
    }

    private void LeftClickCb(DragDropButton button)
    {
        if (button.IsEmptyBtn())
        {
            return;
        }
        BagDragDropButtonData bagDragDropButtonData = button.mData as BagDragDropButtonData;
        this.mLeftClickObject = bagDragDropButtonData;
        MoseState curMoseState = MouseStateControoler.Instan.GetCurMoseState();
        if (curMoseState == MoseState.m_itemsplit)
        {
            this.TryReqSplitObject(bagDragDropButtonData);
        }
        else if (curMoseState == MoseState.m_safelock)
        {
            this.ReqOpItemLock(bagDragDropButtonData);
        }
        else if (curMoseState == MoseState.m_itemsell)
        {
            bool key = Input.GetKey(KeyCode.LeftControl);
            this.TryReqSellItem(bagDragDropButtonData, key);
        }
        else if (curMoseState == MoseState.m_itemrepair)
        {
            this.TryItemRepair(bagDragDropButtonData);
        }
    }

    private void TryItemRepair(BagDragDropButtonData button)
    {
        MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)button.mId);
        if (configTable != null)
        {
            if (configTable.GetField_Uint("type") == 53U)
            {
                this.mRepairThisid = button.thisid;
                LuaTable configTable2 = LuaConfigManager.GetConfigTable("carddata_config", (ulong)button.mId);
                uint field_Uint = configTable2.GetField_Uint("maxdurable");
                t_Object itemData = CommonTools.GetItemData(this.mRepairThisid);
                uint cacheField_Uint = LuaConfigManager.GetXmlConfigTable("newUserInit").GetCacheField_Uint("maxdurablegold");
                uint num = (field_Uint - itemData.card_data.durability) * cacheField_Uint;
                string textById = CommonTools.GetTextById(2805UL);
                string s_describle = string.Format(textById, num);
                ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", s_describle, "确定", "取消", UIManager.ParentType.Tips, new Action(this.ReqItemRepair), null, null);
            }
            else
            {
                Debug.Log("该道具无需修理!");
            }
        }
    }

    public void TryReqCharacterRepair()
    {
        uint allDurabilitys = ControllerManager.Instance.GetController<CardController>().GetAllDurabilitys();
        uint cacheField_Uint = LuaConfigManager.GetXmlConfigTable("newUserInit").GetCacheField_Uint("maxdurablegold");
        string textById = CommonTools.GetTextById(2805UL);
        string s_describle = string.Format(textById, allDurabilitys * cacheField_Uint);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", s_describle, "确定", "取消", UIManager.ParentType.Tips, new Action(this.ReqCharacterRepair), null, null);
    }

    private void ReqCharacterRepair()
    {
        this.netWorker.ReqFixUpDurability(PackType.PACK_TYPE_HERO_CARD, "0");
    }

    private void ReqItemRepair()
    {
        this.netWorker.ReqFixUpDurability(PackType.PACK_TYPE_MAIN, this.mRepairThisid);
    }

    private void TryReqSplitObject(BagDragDropButtonData data)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)data.mId);
        if (configTable == null)
        {
            return;
        }
        if (configTable.GetField_Uint("maxnum") <= 1U)
        {
            TipsWindow.ShowWindow(3001U);
            return;
        }
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenSplitPanel", new object[]
        {
            data.thisid,
            0
        });
    }

    public void TryReqUseObject(PropsBase data)
    {
        this._data = data;
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenSplitPanel", new object[]
        {
            data._obj.thisid,
            1
        });
    }

    public void ReqSplitObject(uint num, int type)
    {
        if (type == 0)
        {
            this.netWorker.ReqSplitObject(this.mLeftClickObject.thisid, num, this.mLeftClickObject.packtype);
        }
        else if (type == 1)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                this._data,
                num
            });
        }
    }

    private void ReqOpItemLock(BagDragDropButtonData data)
    {
        if (this.mLeftClickObject.lock_end_time == 0U)
        {
            this.netWorker.ReqOpItemLock(data.thisid, 1U, string.Empty, data.packtype);
        }
        else
        {
            this.netWorker.ReqOpItemLock(data.thisid, 2U, string.Empty, data.packtype);
        }
    }

    private void TryReqSellItem(BagDragDropButtonData data, bool fastSell)
    {
        ControllerManager.Instance.GetController<ShopController>().MSG_ReqSellItem_CS(data, fastSell);
    }

    private void UseItemCb(DragDropButton button)
    {
        if (button.IsEmptyBtn())
        {
            return;
        }
        this.mUseButton = button;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)button.mData.mId);
        if (configTable != null)
        {
            BagDragDropButtonData bagDragDropButtonData = button.mData as BagDragDropButtonData;
            uint field_Uint = configTable.GetField_Uint("isusebind");
            if (field_Uint == 1U)
            {
                object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.IsItemBind", new object[]
                {
                    bagDragDropButtonData.thisid
                });
                if (array != null && array[0] != null && array[0].ToString() == "0")
                {
                    ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", "使用后绑定", "确定", "取消", UIManager.ParentType.Tips, new Action(this.UseItemDo), null, null);
                    return;
                }
            }
            this.UseItemDo();
        }
    }

    private void UseItemDo()
    {
        if (this.mUseButton == null || this.mUseButton.mData == null)
        {
            FFDebug.LogError(this, "mUseButton==null|| mUseButton.mData == null");
            return;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)this.mUseButton.mData.mId);
        if (configTable == null)
        {
            FFDebug.LogError(this, "LuaConfigManager.GetConfigTable(objects, " + this.mUseButton.mData.mId + ") == null");
            return;
        }
        BagDragDropButtonData bagDragDropButtonData = this.mUseButton.mData as BagDragDropButtonData;
        if (bagDragDropButtonData == null)
        {
            FFDebug.LogError(this, "mUseButton.mData as BagDragDropButtonData == null");
            return;
        }
        uint num = 0U;
        uint effectratio = 0U;
        uint num2 = this.TryGetCallNpcID(bagDragDropButtonData.mId, out num, out effectratio);
        if (num2 != 0U)
        {
            this.SetSkillObjectData(bagDragDropButtonData.thisid, num2, num, effectratio);
            this.SetSelectPosType(SummonNpcState.SelectPos);
            return;
        }
        bool flag = this.TryGetUseByPos(bagDragDropButtonData.mId, out num, out effectratio);
        if (flag)
        {
            this.SetSkillObjectData(bagDragDropButtonData.thisid, flag, num, effectratio);
            this.SetSelectPosType(SummonNpcState.SelectPos);
            return;
        }
        bool flag2 = this.TryGetUseBySelectIndex(bagDragDropButtonData.mId, bagDragDropButtonData.thisid);
        if (flag2)
        {
            return;
        }
        if (UIManager.GetUIObject<UI_Depot>() != null)
        {
            this.netWorker.ReqMoveObject(bagDragDropButtonData.thisid, uint.MaxValue, PackType.PACK_TYPE_MAIN, PackType.PACK_TYPE_WAREHOUSE1, uint.MaxValue);
        }
        else if (UIManager.GetUIObject<UI_NPCshop>() != null)
        {
            ControllerManager.Instance.GetController<ShopController>().MSG_ReqSellItem_CS(bagDragDropButtonData, false);
        }
        else if (ControllerManager.Instance.GetController<ShopController>().CheckInBusinessSellUI())
        {
            ControllerManager.Instance.GetController<ShopController>().DragInSell(bagDragDropButtonData);
        }
        else if (configTable.GetField_Uint("type") == 53U)
        {
            ControllerManager.Instance.GetController<CardController>().ReqPutOnCard(PackType.PACK_TYPE_MAIN, this.mUseButton);
        }
        else if (bagDragDropButtonData.monclickitem == null)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenInfoCommon", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                bagDragDropButtonData.mId,
                bagDragDropButtonData.root
            });
        }
        else
        {
            bagDragDropButtonData.monclickitem(bagDragDropButtonData.mId);
        }
        if (configTable.GetField_Uint("type") == 60U || configTable.GetField_Uint("type") == 67U)
        {
            MainPlayer.Self.StopMoveImmediate(null);
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
        }
    }

    private SkillSelectRangeEffect SelectRE
    {
        get
        {
            if (this.SelectRE_ == null)
            {
                if (MainPlayer.Self == null)
                {
                    return null;
                }
                this.SelectRE_ = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
            }
            return this.SelectRE_;
        }
    }

    public void SetSelectPosType(SummonNpcState snsType)
    {
        if (snsType == SummonNpcState.SelectPos)
        {
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (uiobject != null && uiobject.SkillButtonList != null && uiobject.SkillButtonList.Count > 0 && uiobject.SkillButtonList[0] != null)
            {
                uiobject.SkillButtonList[0].CancelOtherButtonCheckState(true);
            }
        }
        this.sns = snsType;
    }

    public void SetSkillObjectData(string objID, uint npcID, uint range, uint effectratio)
    {
        this.thisID = objID;
        this.callNpcID = npcID;
        this.range = range;
        this.ratio = effectratio;
    }

    public void SetSkillObjectData(string objID, bool callUseItemByPos, uint range, uint effectratio)
    {
        this.thisID = objID;
        this.callUseItemByPos = callUseItemByPos;
        this.range = range;
        this.ratio = effectratio;
    }

    private void CheckCallObjectUsetState()
    {
        if (this.SelectRE == null)
        {
            return;
        }
        int num = 0;
        Vector2 vector = Vector2.zero;
        SummonNpcState summonNpcState = this.sns;
        if (summonNpcState != SummonNpcState.None)
        {
            if (summonNpcState == SummonNpcState.SelectPos)
            {
                if (MainPlayer.Self == null || MainPlayer.Self.ModelObj == null)
                {
                    return;
                }
                Vector3 position = MainPlayer.Self.ModelObj.transform.position;
                Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
                Vector3 vector2 = MyMathf.GetRayPoint(ray, position.y, 1000f);
                vector2.y = position.y;
                Vector3 point = vector2 - position;
                if (point.magnitude > this.range)
                {
                    vector2 = MyMathf.GetCircularPoint(position, point, this.range);
                }
                this.SelectRE.MoveCircle(vector2, this.ratio, this.range, "Rang3", "Rang2");
                if (Input.GetMouseButtonDown(0))
                {
                    num = 1;
                    vector = GraphUtils.GetServerPosByWorldPos(vector2, true);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    num = 2;
                }
            }
        }
        if (this.sns == SummonNpcState.SelectPos && num != 0)
        {
            if (num == 1)
            {
                if (this.callNpcID != 0U)
                {
                    this.netWorker.ReqUseObjectSpecial(this.thisID, this.callNpcID, vector.x, vector.y);
                }
                else if (this.callUseItemByPos)
                {
                    AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
                    controller.UseCapsuleItemByPos(this.thisID, vector.x, vector.y);
                }
            }
            if (num == 2)
            {
            }
            Scheduler.Instance.AddTimer(0.21f, false, delegate
            {
                this.SelectRE.HidleALL();
                this.SetSelectPosType(SummonNpcState.None);
            });
        }
    }

    public bool IsInSightingStateByObjectSkill()
    {
        return this.sns == SummonNpcState.SelectPos;
    }

    public uint TryGetCallNpcID(uint objId, out uint range, out uint ratio)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("other").GetCacheField_Table("callnpcobject");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        range = 0U;
        ratio = 0U;
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            uint cacheField_Uint = luaTable.GetCacheField_Uint("id");
            if (cacheField_Uint == objId)
            {
                range = uint.Parse(luaTable.GetField_String("releaserange"));
                ratio = uint.Parse(luaTable.GetField_String("effectrange"));
                return uint.Parse(luaTable.GetField_String("callnpcid"));
            }
        }
        return 0U;
    }

    public bool TryGetUseByPos(uint objId, out uint range, out uint ratio)
    {
        if (objId == 500004U)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("mobapk").GetCacheField_Table("bomb");
            range = uint.Parse(cacheField_Table.GetField_String("range"));
            ratio = uint.Parse(cacheField_Table.GetField_String("effectrange"));
            return true;
        }
        range = 0U;
        ratio = 0U;
        return false;
    }

    public bool TryGetUseBySelectIndex(uint objId, string sendid)
    {
        if (objId == 500002U)
        {
            string sendid2 = sendid;
            AbattoirMatchController abattoirController = ControllerManager.Instance.GetController<AbattoirMatchController>();
            abattoirController.OpenTransfer(delegate (int selectIndex)
            {
                if (selectIndex == -1)
                {
                    return;
                }
                abattoirController.UseCapsuleItemByPos(sendid, (float)selectIndex, 0f);
            });
            return true;
        }
        return false;
    }

    private bool DropDownCheckCb(DragDropButton mCurDragButton, DragDropButton mTargetDragButton)
    {
        if (GameSystemSettings.IsMainPlayerInBattleState())
        {
            return false;
        }
        if (mTargetDragButton.mData != null && mCurDragButton.mUIRootType != UIRootType.Bag && mCurDragButton.mUIRootType != UIRootType.Depot)
        {
            List<PropsBase> list = MainPlayer.Self.MainPackageTableList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i]._obj.grid_y == mTargetDragButton.mPos.y)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void PutInCb(DragDropButton mCurDragButton, DragDropButton mTargetDragButton)
    {
        if (mCurDragButton.mUIRootType == UIRootType.Bag || mCurDragButton.mUIRootType == UIRootType.Depot)
        {
            BagDragDropButtonData bagDragDropButtonData = mCurDragButton.mData as BagDragDropButtonData;
            BagDragDropButtonData bagDragDropButtonData2 = mTargetDragButton.mData as BagDragDropButtonData;
            if (!mTargetDragButton.IsEmptyBtn())
            {
                bool flag = false;
                if (bagDragDropButtonData.mId == bagDragDropButtonData2.mId)
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)bagDragDropButtonData.mId);
                    if (configTable != null && configTable.GetField_Uint("maxnum") > 1U)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    this.netWorker.MergeObjs(bagDragDropButtonData.thisid, bagDragDropButtonData2.thisid, this.GetDragDropButtonPackType(mCurDragButton), this.GetDragDropButtonPackType(mTargetDragButton));
                }
                else
                {
                    this.netWorker.ReqSwapObject(bagDragDropButtonData.thisid, bagDragDropButtonData2.thisid, bagDragDropButtonData.packtype, bagDragDropButtonData2.packtype);
                }
            }
            else
            {
                GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
                PackType dst_packtype = manager.isAbattoirScene ? bagDragDropButtonData.packtype : PackType.PACK_TYPE_MAIN;
                this.netWorker.ReqMoveObject(bagDragDropButtonData.thisid, (uint)mTargetDragButton.mPos.y, bagDragDropButtonData.packtype, dst_packtype, 0U);
            }
        }
    }

    private PackType GetDragDropButtonPackType(DragDropButton button)
    {
        if (button.mUIRootType == UIRootType.Bag)
        {
            return PackType.PACK_TYPE_MAIN;
        }
        if (button.mUIRootType == UIRootType.Depot)
        {
            return PackType.PACK_TYPE_WAREHOUSE1;
        }
        return PackType.PACK_TYPE_NONE;
    }

    public void DestoryItemCb(DragDropButton mCurDragButton)
    {
        if (mCurDragButton.IsEmptyBtn())
        {
            return;
        }
        BagDragDropButtonData bagDragDropButtonData = mCurDragButton.mData as BagDragDropButtonData;
        this.DoDestoryItem(bagDragDropButtonData.mId, bagDragDropButtonData.thisid, bagDragDropButtonData.packtype);
    }

    public void DoDestoryItem(uint mid, string thisid, PackType packtype)
    {
        if (packtype == PackType.PACK_TYPE_QUEST)
        {
            TipsWindow.ShowNotice(4033U);
            return;
        }
        int num = (int)packtype;
        this.DoDestoryItem(mid, thisid, num.ToString());
    }

    public bool IsSplitPanelOpen()
    {
        return bool.Parse(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.IsSplitPanelOpen", new object[0])[0].ToString());
    }

    public void EnterSendSplitOk()
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterSendSplitOk", new object[0]);
    }

    public void DoDestoryItem(uint mid, string thisid, string packtype)
    {
        UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
        {
            UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
            string field_String = LuaConfigManager.GetConfigTable("objects", (ulong)mid).GetField_String("name");
            uiobject.SetContent("确认删除<color=#FF6600>" + field_String + "</color>?", "提示", true);
            uiobject.SetOkCb(new MessageOkCb(this.ReqDestoryItemSureCb), new SaveDataItem
            {
                thisid = thisid,
                packtype = packtype
            });
        }, UIManager.ParentType.CommonUI, false);
    }

    private void ReqDestoryItemSureCb(SaveDataItem data)
    {
        new BagNetworker().ReqDestroyObject(data.thisid, data.packtype);
    }

    private bool DragUpCheck(DragDropButton button)
    {
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetUIBagState", new object[0]);
        int num = int.Parse(array[0].ToString());
        return num != 1 && num != 4;
    }

    public void ShowDeadMask(bool isShow)
    {
        object obj = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemTransform", new object[0])[0];
        if (obj != null)
        {
            Transform parent = (obj as Transform).parent.parent;
            for (int i = 1; i < parent.childCount; i++)
            {
                DragDropButtonDataBase mData = parent.GetChild(i).GetComponent<DragDropButton>().mData;
                if (mData != null)
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)mData.mId);
                    if (configTable != null)
                    {
                        string field_String = configTable.GetField_String("state_limit");
                        string[] array = field_String.Split(new char[]
                        {
                            '|'
                        });
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (array[j] == 4.ToString())
                            {
                                isShow = true;
                                break;
                            }
                        }
                        parent.GetChild(i).Find("Item/DeadMask").gameObject.SetActive(isShow);
                    }
                }
            }
        }
    }

    private static UIBagManager _instance;

    private BagNetworker netWorker;

    private BagDragDropButtonData mLeftClickObject;

    private string mRepairThisid;

    private PropsBase _data;

    private DragDropButton mUseButton;

    private SummonNpcState sns;

    private SkillSelectRangeEffect SelectRE_;

    private uint callNpcID;

    private bool callUseItemByPos;

    private uint range;

    private uint ratio;

    private string thisID = string.Empty;

    private enum LockOperate
    {
        Lock = 1,
        UnLock
    }

    private enum SateLimitEnum
    {
        None,
        FightState,
        NotTotalLoseControlState,
        TotalLoseControlState,
        DeadState
    }
}
