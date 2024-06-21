using System;
using System.Collections.Generic;
using Framework.Managers;
using Net;
using Obj;

public class DepotNetwork : NetWorkBase
{
    public override void Initialize()
    {
        this.mController = ControllerManager.Instance.GetController<DepotController>();
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_PackData_SC>(CommandID.MSG_PackData_SC, new ProtoMsgCallback<MSG_PackData_SC>(this.PackDataCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RefreshObjs_SC>(CommandID.MSG_RefreshObjs_SC, new ProtoMsgCallback<MSG_RefreshObjs_SC>(this.RefreshObjsCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RemoveObject_SC>(CommandID.MSG_RemoveObject_SC, new ProtoMsgCallback<MSG_RemoveObject_SC>(this.RemoveObjectCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_PackUnlock_SC>(CommandID.MSG_PackUnlock_SC, new ProtoMsgCallback<MSG_PackUnlock_SC>(this.UnlockPackageCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_TidyPack_SC>(CommandID.MSG_TidyPack_SC, new ProtoMsgCallback<MSG_TidyPack_SC>(this.TidyPackCb));
    }

    private bool IsDepotItem(PackType packType)
    {
        return packType == PackType.PACK_TYPE_WAREHOUSE1 || packType == PackType.PACK_TYPE_WAREHOUSE2 || packType == PackType.PACK_TYPE_WAREHOUSE3 || packType == PackType.PACK_TYPE_WAREHOUSE4;
    }

    private void PackDataCb(MSG_PackData_SC data)
    {
        if (this.IsDepotItem(data.packtype))
        {
            this.mController.InitDataCb(data);
        }
    }

    private void RefreshObjsCb(MSG_RefreshObjs_SC data)
    {
        List<t_Object> list = new List<t_Object>();
        for (int i = 0; i < data.objs.Count; i++)
        {
            if (this.IsDepotItem(data.objs[i].packtype))
            {
                list.Add(data.objs[i]);
            }
        }
        if (list.Count > 0)
        {
            this.mController.RefreshDataCb(list);
        }
    }

    private void RemoveObjectCb(MSG_RemoveObject_SC data)
    {
        if (data.ids != null && data.ids.Count > 0)
        {
            this.mController.RemoveObjectCb(data.ids);
        }
    }

    public void ReqMoveObject(string thisid, PackType sourcePackType, PackType dstPackType, uint pos, uint grid_x = 0U)
    {
        base.SendMsg<MSG_ReqMoveObject_CS>(CommandID.MSG_ReqMoveObject_CS, new MSG_ReqMoveObject_CS
        {
            packtype = sourcePackType,
            dst_packtype = dstPackType,
            grid_x = grid_x,
            grid_y = pos,
            thisid = thisid
        }, false);
    }

    public void MergeObjs(string sourceThisid, string dstThisid, PackType sourcePackType, PackType dstPackType)
    {
        base.SendMsg<MSG_MergeObjs_CS>(CommandID.MSG_MergeObjs_CS, new MSG_MergeObjs_CS
        {
            packtype = sourcePackType,
            dst_packtype = dstPackType,
            srcthisids =
            {
                sourceThisid
            },
            dstthisids =
            {
                dstThisid
            }
        }, false);
    }

    public void ReqSwapObject(string sourceThisid, string dstThisid, PackType sourcePackType, PackType dstPackType)
    {
        base.SendMsg<MSG_ReqSwapObject_CS>(CommandID.MSG_ReqSwapObject_CS, new MSG_ReqSwapObject_CS
        {
            packtype = sourcePackType,
            dst_packtype = dstPackType,
            src_thisid = sourceThisid,
            dst_thisid = dstThisid
        }, false);
    }

    public void ReqDestroyObject(string thisid, PackType packtype)
    {
        SecondPwdControl controller = ControllerManager.Instance.GetController<SecondPwdControl>();
        if (controller.CheckNeedInputSecondPwd())
        {
            return;
        }
        base.SendMsg<MSG_ReqDestroyObject_CS>(CommandID.MSG_ReqDestroyObject_CS, new MSG_ReqDestroyObject_CS
        {
            thisid = thisid,
            packtype = packtype,
            passwd = controller.PlayerInputSecondPwd
        }, false);
        DragDropButton curDragButton = DragDropManager.Instance.GetCurDragButton();
        BagDragDropButtonData bagDragDropButtonData = curDragButton.mData as BagDragDropButtonData;
        if (bagDragDropButtonData == null || bagDragDropButtonData.lock_end_time == 0U)
        {
            DragDropManager.Instance.GetCurDragButton().mData = null;
        }
    }

    public void ReqSplitObject(string thisid, uint num, PackType packtype)
    {
        base.SendMsg<MSG_ReqSplitObject_CS>(CommandID.MSG_ReqSplitObject_CS, new MSG_ReqSplitObject_CS
        {
            thisid = thisid,
            num = num,
            packtype = packtype
        }, false);
    }

    public void ReqOpItemLock(string thisid, uint opcode, string pwd, PackType packtype)
    {
        base.SendMsg<MSG_ReqOpItemLock_CS>(CommandID.MSG_ReqOpItemLock_CS, new MSG_ReqOpItemLock_CS
        {
            thisid = thisid,
            opcode = opcode,
            passwd = pwd,
            packtype = packtype
        }, false);
    }

    public void UnlockPackage(PackType type)
    {
        base.SendMsg<MSG_PackUnlock_CS>(CommandID.MSG_PackUnlock_CS, new MSG_PackUnlock_CS
        {
            packtype = type
        }, false);
    }

    public void UnlockPackageCb(MSG_PackUnlock_SC mdata)
    {
        if (!mdata.result)
        {
            return;
        }
        if (this.IsDepotItem(mdata.packtype))
        {
            this.mController.ExtendPackCb(mdata);
        }
    }

    public void TidyPack(PackType packType, List<t_TidyPackInfo> infos)
    {
        MSG_TidyPack_CS msg_TidyPack_CS = new MSG_TidyPack_CS();
        msg_TidyPack_CS.packtype = packType;
        msg_TidyPack_CS.infos.AddRange(infos);
        base.SendMsg<MSG_TidyPack_CS>(CommandID.MSG_TidyPack_CS, msg_TidyPack_CS, false);
    }

    private void TidyPackCb(MSG_TidyPack_SC data)
    {
        if (data.result)
        {
            this.mController.TidyPackCb(data.packtype);
        }
        else
        {
            FFDebug.LogError(this, "整理失败：" + data.packtype);
        }
    }

    public void ReqTransMoney(PackType packtype, PackType dst_packtype, uint resourceID, uint count)
    {
        base.SendMsg<MSG_ReqTransMoney_CS>(CommandID.MSG_ReqTransMoney_CS, new MSG_ReqTransMoney_CS
        {
            packtype = packtype,
            dst_packtype = dst_packtype,
            resourceID = resourceID,
            quantity = count
        }, false);
    }

    public override void Uninitialize()
    {
    }

    public override void UnRegisterMsg()
    {
    }

    private DepotController mController;
}
