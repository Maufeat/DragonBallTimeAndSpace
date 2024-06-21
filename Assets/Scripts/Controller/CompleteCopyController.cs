using System;
using System.Collections.Generic;
using copymap;
using Framework.Managers;
using LuaInterface;
using massive;
using Models;
using Obj;

public class CompleteCopyController : ControllerBase
{
    public CopyManager MCopyManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CopyManager>();
        }
    }

    public UI_CompleteCopy UiCompleteCopy
    {
        get
        {
            return UIManager.GetUIObject<UI_CompleteCopy>();
        }
    }

    public void SetFakeData()
    {
        this.resDataList.Clear();
        this.expChange = 9999UL;
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        this.openBoxTime = xmlConfigTable.GetCacheField_Table("CopyAwardBoxOpenTime").GetCacheField_Float("value") / 1000f;
        this.leaveCopyTime = xmlConfigTable.GetCacheField_Table("CopyExitTime").GetCacheField_Float("value") / 1000f;
        PropsBase item = new PropsBase(4U, 120U);
        this.resDataList.Add(item);
        PropsBase item2 = new PropsBase(3U, 300U);
        this.resDataList.Add(item2);
        this.personalAwardDataList.Clear();
        PropsBase item3 = new PropsBase(1005U, 1U);
        this.personalAwardDataList.Add(item3);
        PropsBase item4 = new PropsBase(30001U, 1U);
        this.personalAwardDataList.Add(item4);
        this.boxAwardDataList.Clear();
        CompleteCopyController.AwardData awardData = new CompleteCopyController.AwardData(MainPlayer.Self.EID.Id, null);
        awardData.Prop = new PropsBase(30001U, 1U);
        this.boxAwardDataList.Add(awardData);
        CompleteCopyController.AwardData awardData2 = new CompleteCopyController.AwardData(0UL, null);
        awardData2.Prop = new PropsBase(1005U, 1U);
        this.boxAwardDataList.Add(awardData2);
        CompleteCopyController.AwardData awardData3 = new CompleteCopyController.AwardData(0UL, null);
        awardData3.Prop = new PropsBase(1005U, 1U);
        this.boxAwardDataList.Add(awardData3);
        CompleteCopyController.AwardData awardData4 = new CompleteCopyController.AwardData(0UL, null);
        awardData4.Prop = new PropsBase(1005U, 1U);
        this.boxAwardDataList.Add(awardData4);
    }

    public void SetCompleteCopyData(MSG_Ret_CopymapOver_SC data)
    {
        try
        {
            LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
            this.openBoxTime = xmlConfigTable.GetCacheField_Table("CopyAwardBoxOpenTime").GetCacheField_Float("value") / 1000f;
            this.leaveCopyTime = xmlConfigTable.GetCacheField_Table("CopyExitTime").GetCacheField_Float("value") / 1000f;
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, "xml data error:" + arg);
        }
        this.expChange = (ulong)data.exp;
        this.resDataList.Clear();
        for (int i = 0; i < data.money.Count; i++)
        {
            MSG_RetCurrencyChange_SC msg_RetCurrencyChange_SC = data.money[i];
            if (msg_RetCurrencyChange_SC.isadd)
            {
                PropsBase item = new PropsBase(msg_RetCurrencyChange_SC.currencyid, msg_RetCurrencyChange_SC.changenum);
                this.resDataList.Add(item);
            }
        }
        this.personalAwardDataList.Clear();
        if (data.items != null)
        {
            for (int j = 0; j < data.items.objs.Count; j++)
            {
                if (data.items.objs[j] != null)
                {
                    PropsBase item2 = new PropsBase(data.items.objs[j]);
                    this.personalAwardDataList.Add(item2);
                }
            }
        }
    }

    public void ReqOpenBox()
    {
        this.MCopyManager.MCopyNetWork.ReqCopymapLottery();
    }

    public void ShowCompleteCopyView()
    {
        this.StopAutoattackWhenLeftCopy();
        this.TryToShowCompleteCopyView = true;
    }

    private void CheckShowCompleteCopyView()
    {
        if (this.TryToShowCompleteCopyView)
        {
            BossDieCameraMove bossDieCameraMove = CameraController.Self.CurrStatebyType<BossDieCameraMove>();
            bool flag = UIManager.IsLuaPanelExists("UI_TimeRoom");
            if ((bossDieCameraMove == null || !bossDieCameraMove.OnPlay) && !flag)
            {
                this.TryToShowCompleteCopyView = false;
                LuaScriptMgr.Instance.CallLuaFunction("InstanceOverCtrl.OpenView", new object[]
                {
                    Util.GetLuaTable("InstanceOverCtrl")
                });
            }
        }
    }

    public void CloseCompleteCopyView()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI<UI_CompleteCopy>();
    }

    private void StopAutoattackWhenLeftCopy()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<AutoAttack>().AutoAttackOn)
        {
            MainPlayer.Self.GetComponent<AutoAttack>().SwitchModle(false);
        }
        ControllerManager.Instance.GetController<TaskController>().autoTask.Stop();
    }

    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.CheckShowCompleteCopyView();
    }

    public float openBoxTime = 10f;

    public float leaveCopyTime = 10f;

    public ulong expChange;

    public List<PropsBase> resDataList = new List<PropsBase>();

    public List<PropsBase> personalAwardDataList = new List<PropsBase>();

    public List<CompleteCopyController.AwardData> boxAwardDataList = new List<CompleteCopyController.AwardData>();

    private bool TryToShowCompleteCopyView;

    public class AwardData
    {
        public AwardData(ulong id, t_Object obj)
        {
            this.OwnerId = id;
            if (obj != null)
            {
                this.Prop = new PropsBase(obj);
            }
        }

        public ulong OwnerId;

        public PropsBase Prop;
    }
}
