using System;
using System.Collections.Generic;
using Models;
using Obj;

public class DepotController : ControllerBase
{
    public UI_Depot UIDepot
    {
        get
        {
            return UIManager.GetUIObject<UI_Depot>();
        }
    }

    public override void Awake()
    {
        this.mNetwork = new DepotNetwork();
        this.mNetwork.Initialize();
        DragDropManager.Instance.RegisterPutInCb(UIRootType.Depot, new TwoBtnCb(this.PutInCb));
        DragDropManager.Instance.RegisterPutOutCb(UIRootType.Depot, new TwoBtnCb(this.PutOutCb));
        DragDropManager.Instance.RegisterUseItemCb(UIRootType.Depot, new OneBtnCb(this.UseItemCb));
        DragDropManager.Instance.RegisterLeftClickCb(UIRootType.Depot, new OneBtnCb(this.LeftClickCb));
        DragDropManager.Instance.RegisterDestoryItemCb(UIRootType.Depot, new OneBtnCb(this.DestoryCb));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenDepotPanel));
        base.Awake();
    }

    public uint GetItemCount(uint baseid)
    {
        uint num = 0U;
        if (this.mPackData != null && this.mPackData.objects != null)
        {
            List<t_Object> objs = this.mPackData.objects.objs;
            if (objs != null)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    t_Object t_Object = objs[i];
                    if (t_Object != null)
                    {
                        if (t_Object.baseid == baseid)
                        {
                            num += objs[i].num;
                        }
                    }
                }
            }
        }
        return num;
    }

    public uint GetItemCount(string thisid)
    {
        uint num = 0U;
        if (this.mPackData != null && this.mPackData.objects != null)
        {
            List<t_Object> objs = this.mPackData.objects.objs;
            if (objs != null)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    t_Object t_Object = objs[i];
                    if (t_Object != null && t_Object.thisid == thisid)
                    {
                        num += t_Object.num;
                    }
                }
            }
        }
        return num;
    }

    public void InitDataCb(MSG_PackData_SC data)
    {
        this.mPackData = data;
        if (this.UIDepot != null)
        {
            this.UIDepot.SetupPanel();
        }
    }

    public void RefreshDataCb(List<t_Object> dataList)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            t_Object t_Object = dataList[i];
            List<t_Object> objs = this.mPackData.objects.objs;
            for (int j = 0; j < objs.Count; j++)
            {
                if (objs[j].thisid == t_Object.thisid && objs[j].grid_y != t_Object.grid_y)
                {
                    objs.RemoveAt(j);
                    break;
                }
            }
            bool flag = false;
            for (int k = 0; k < objs.Count; k++)
            {
                if (objs[k].grid_y == 65535U)
                {
                    if (objs[k].baseid == t_Object.baseid)
                    {
                        objs[k] = t_Object;
                        flag = true;
                    }
                    break;
                }
                if (objs[k].grid_y == t_Object.grid_y)
                {
                    objs[k] = t_Object;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                this.mPackData.objects.objs.Add(t_Object);
            }
        }
        if (this.UIDepot != null)
        {
            this.UIDepot.SetupPanel();
        }
    }

    public void RemoveObjectCb(List<string> ids)
    {
        List<t_Object> objs = this.mPackData.objects.objs;
        List<t_Object> list = new List<t_Object>();
        for (int i = 0; i < objs.Count; i++)
        {
            if (ids.Contains(objs[i].thisid))
            {
                list.Add(objs[i]);
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            objs.Remove(list[j]);
        }
        if (this.UIDepot != null)
        {
            this.UIDepot.SetupPanel();
        }
    }

    public MSG_PackData_SC GetDepotData()
    {
        return this.mPackData;
    }

    public void OpenDepotPanel(List<VarType> paras)
    {
        UIManager.Instance.ShowUI<UI_Depot>("UI_Depot", null, UIManager.ParentType.CommonUI, false);
    }

    private void LeftClickCb(DragDropButton button)
    {
        if (button.IsEmptyBtn())
        {
            return;
        }
        BagDragDropButtonData data = button.mData as BagDragDropButtonData;
        this.mLeftClickObject = data;
        MoseState curMoseState = MouseStateControoler.Instan.GetCurMoseState();
        if (curMoseState == MoseState.m_itemsplit)
        {
            this.TryReqSplitObject(data);
        }
        else if (curMoseState == MoseState.m_safelock)
        {
            this.ReqOpItemLock(data);
        }
    }

    private void TryReqSplitObject(BagDragDropButtonData data)
    {
        this.UIDepot.SetupSplitPanel(data.mId, this.GetItemCount(data.thisid));
    }

    public void ReqSplitObject(uint num)
    {
        this.mNetwork.ReqSplitObject(this.mLeftClickObject.thisid, num, this.mLeftClickObject.packtype);
    }

    private void ReqOpItemLock(BagDragDropButtonData data)
    {
        if (this.mLeftClickObject.lock_end_time == 0U)
        {
            this.mNetwork.ReqOpItemLock(data.thisid, 1U, string.Empty, data.packtype);
        }
        else
        {
            this.mNetwork.ReqOpItemLock(data.thisid, 2U, string.Empty, data.packtype);
        }
    }

    private void PutInCb(DragDropButton btnFrom, DragDropButton btnTo)
    {
        if (btnTo.mData == null)
        {
            this.mNetwork.ReqMoveObject(btnFrom.mData.thisid, (btnFrom.mUIRootType != UIRootType.Bag) ? PackType.PACK_TYPE_WAREHOUSE1 : PackType.PACK_TYPE_MAIN, PackType.PACK_TYPE_WAREHOUSE1, (uint)btnTo.mPos.y, 0U);
        }
        else if (btnTo.mData.mId == btnFrom.mData.mId)
        {
            this.mNetwork.MergeObjs(btnFrom.mData.thisid, btnTo.mData.thisid, (btnFrom.mUIRootType != UIRootType.Bag) ? PackType.PACK_TYPE_WAREHOUSE1 : PackType.PACK_TYPE_MAIN, PackType.PACK_TYPE_WAREHOUSE1);
        }
        else
        {
            this.mNetwork.ReqSwapObject(btnFrom.mData.thisid, btnTo.mData.thisid, (btnFrom.mUIRootType != UIRootType.Bag) ? PackType.PACK_TYPE_WAREHOUSE1 : PackType.PACK_TYPE_MAIN, PackType.PACK_TYPE_WAREHOUSE1);
        }
    }

    private void PutOutCb(DragDropButton btnFrom, DragDropButton btnTo)
    {
    }

    private void UseItemCb(DragDropButton btnUse)
    {
        if (btnUse.mData == null)
        {
            return;
        }
        this.mNetwork.ReqMoveObject(btnUse.mData.thisid, PackType.PACK_TYPE_WAREHOUSE1, PackType.PACK_TYPE_MAIN, uint.MaxValue, uint.MaxValue);
    }

    private void DestoryCb(DragDropButton btnDestory)
    {
        this.mNetwork.ReqDestroyObject(btnDestory.mData.thisid, PackType.PACK_TYPE_WAREHOUSE1);
    }

    public uint GetUnlockCount()
    {
        return this.mPackData.unlockcount;
    }

    public void ExtendPackage()
    {
        this.mNetwork.UnlockPackage(PackType.PACK_TYPE_WAREHOUSE1);
    }

    public void ExtendPackCb(MSG_PackUnlock_SC mdata)
    {
        if (mdata.result)
        {
            this.mPackData.unlockcount = mdata.unlockcount;
            this.UIDepot.ExtendPackCb(mdata);
        }
    }

    public void TidyPack(PackType packType)
    {
        if (this.mPackData == null || this.mPackData.objects == null)
        {
            return;
        }
        List<t_Object> objs = this.mPackData.objects.objs;
        List<PropsBase> list = new List<PropsBase>();
        for (int i = 0; i < objs.Count; i++)
        {
            list.Add(new PropsBase(objs[i]));
        }
        PackPropComparer comparer = new PackPropComparer();
        list.Sort(comparer);
        this.tidyList = new List<t_TidyPackInfo>();
        for (int j = 0; j < list.Count; j++)
        {
            t_TidyPackInfo t_TidyPackInfo = new t_TidyPackInfo();
            t_TidyPackInfo.thisid = list[j].ThisidStr;
            t_TidyPackInfo.grid_y = (uint)j;
            this.tidyList.Add(t_TidyPackInfo);
        }
        this.mNetwork.TidyPack(packType, this.tidyList);
    }

    public void TidyPackCb(PackType packType)
    {
        MSG_PackData_SC msg_PackData_SC = this.mPackData;
        if (this.tidyList != null)
        {
            for (int i = 0; i < this.tidyList.Count; i++)
            {
                for (int j = 0; j < msg_PackData_SC.objects.objs.Count; j++)
                {
                    if (msg_PackData_SC.objects.objs[j].thisid == this.tidyList[i].thisid)
                    {
                        msg_PackData_SC.objects.objs[j].grid_y = this.tidyList[i].grid_y;
                    }
                }
            }
        }
        if (this.UIDepot != null)
        {
            this.UIDepot.SetupPanel();
        }
    }

    public void TransMoney(PackType packtype, PackType dst_packtype, uint resourceID, uint count)
    {
        this.mNetwork.ReqTransMoney(packtype, dst_packtype, resourceID, count);
    }

    public void ClosePanel()
    {
        UIManager.Instance.DeleteUI<UI_Depot>();
    }

    public override string ControllerName
    {
        get
        {
            return "depot_controller";
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private MSG_PackData_SC mPackData;

    private DepotNetwork mNetwork;

    private BagDragDropButtonData mLeftClickObject;

    private List<t_TidyPackInfo> tidyList;

    private enum LockOperate
    {
        Lock = 1,
        UnLock
    }
}
