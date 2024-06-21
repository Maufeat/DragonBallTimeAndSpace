using System;
using System.Collections.Generic;
using Framework.Managers;
using Net;
using Obj;

public class BagNetworker : NetWorkBase
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void RegisterMsg()
    {
        base.RegisterMsg();
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }

    public void ReqMoveObject(string thisid, uint grid_y, PackType _packType, PackType dst_packtype, uint grid_x = 0U)
    {
        base.SendMsg<MSG_ReqMoveObject_CS>(CommandID.MSG_ReqMoveObject_CS, new MSG_ReqMoveObject_CS
        {
            thisid = thisid,
            grid_x = grid_x,
            grid_y = grid_y,
            packtype = _packType,
            dst_packtype = dst_packtype
        }, false);
    }

    public void ReqSwapObject(string src_thisid, string dst_thisid, PackType _packType, PackType dst_packtype)
    {
        base.SendMsg<MSG_ReqSwapObject_CS>(CommandID.MSG_ReqSwapObject_CS, new MSG_ReqSwapObject_CS
        {
            src_thisid = src_thisid,
            dst_thisid = dst_thisid,
            packtype = _packType,
            dst_packtype = dst_packtype
        }, false);
    }

    public void ReqMergeItem(string sourcethisid, string targetthisid, PackType _packType, PackType dst_packtype)
    {
        MSG_MergeObjs_CS msg_MergeObjs_CS = new MSG_MergeObjs_CS();
        msg_MergeObjs_CS.srcthisids.AddRange(new List<string>
        {
            sourcethisid
        });
        msg_MergeObjs_CS.dstthisids.AddRange(new List<string>
        {
            targetthisid
        });
        msg_MergeObjs_CS.packtype = _packType;
        msg_MergeObjs_CS.dst_packtype = dst_packtype;
        base.SendMsg<MSG_MergeObjs_CS>(CommandID.MSG_MergeObjs_CS, msg_MergeObjs_CS, false);
    }

    public void ReqDestroyObject(string thisid, string packtype)
    {
        SecondPwdControl controller = ControllerManager.Instance.GetController<SecondPwdControl>();
        if (controller.CheckNeedInputSecondPwd())
        {
            return;
        }
        base.SendMsg<MSG_ReqDestroyObject_CS>(CommandID.MSG_ReqDestroyObject_CS, new MSG_ReqDestroyObject_CS
        {
            thisid = thisid,
            packtype = (PackType)int.Parse(packtype),
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

    public void ReqUseObjectSpecial(string sourceThisid, uint callNpcID, float posX, float posY)
    {
        base.SendMsg<MSG_ReqUseObject_Special_CS>(CommandID.MSG_ReqUseObject_Special_CS, new MSG_ReqUseObject_Special_CS
        {
            thisid = sourceThisid,
            npcid = (ulong)callNpcID,
            posx = posX,
            posy = posY
        }, false);
    }

    public void ReqFixUpDurability(PackType packType, string thisid)
    {
        base.SendMsg<MSG_ReqFixUpDurability_CS>(CommandID.MSG_ReqFixUpDurability_CS, new MSG_ReqFixUpDurability_CS
        {
            packtype = packType,
            thisid = thisid
        }, false);
    }
}
