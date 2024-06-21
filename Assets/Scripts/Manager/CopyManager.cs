using System;
using System.Collections.Generic;
using copymap;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class CopyManager : IManager
{
    public UI_CompleteCopy UiCompleteCopy
    {
        get
        {
            return UIManager.GetUIObject<UI_CompleteCopy>();
        }
    }

    public bool InCopy
    {
        get
        {
            return this.currentcopyid != 0U;
        }
    }

    public bool InCompetitionCopy
    {
        get
        {
            return this.InWuDaoHuiCopy || this.InDeathPKCopy || this.InDuoqiCopy;
        }
    }

    public bool InWuDaoHuiCopy
    {
        get
        {
            return this.currentcopyid == this.configPkmap;
        }
    }

    public bool InDeathPKCopy
    {
        get
        {
            return this.currentcopyid == this.configSidoou;
        }
    }

    public bool InDuoqiCopy
    {
        get
        {
            return this.currentcopyid == this.configDuoqi;
        }
    }

    public uint MCurrentCopyID
    {
        get
        {
            return this.currentcopyid;
        }
    }

    public uint MCurrentSubCopyMapId
    {
        get
        {
            return this.subcopymapidx;
        }
    }

    public void Init()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ShowEnterCopy));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ExitCopy));
        this.MCopyNetWork = new CopyNetWork();
        this.MCopyNetWork.Initialize();
        this.defaultCopy = new CopyDefault();
        this.defaultCopy.InitCopy();
        this.AddCopy(new CopyForest());
        this.AddCopy(new CopyHighWay());
        this.AddCopy(new CopyGuide());
        this.configPkmap = LuaConfigManager.GetXmlConfigTable("rank_pk").GetCacheField_Table("pkcopymapindex").GetCacheField_Uint("value");
        this.configSidoou = LuaConfigManager.GetXmlConfigTable("rank_pk").GetCacheField_Table("deathcopymapindex").GetCacheField_Uint("value");
        this.configDuoqi = LuaConfigManager.GetXmlConfigTable("rank_pk").GetCacheField_Table("duoqicopymapindex").GetCacheField_Uint("value");
    }

    public ICopy CurrentCopy
    {
        get
        {
            return this.GetCopy(this.currentcopyid);
        }
    }

    private void AddCopy(ICopy copy)
    {
        if (!this.CopyList.Contains(copy))
        {
            this.CopyList.Add(copy);
            copy.InitCopy();
        }
    }

    public T GetCopy<T>() where T : class, ICopy
    {
        for (int i = 0; i < this.CopyList.Count; i++)
        {
            ICopy copy = this.CopyList[i];
            if (copy is T)
            {
                return (T)((object)copy);
            }
        }
        return this.defaultCopy as T;
    }

    public ICopy GetCopy(uint copyid)
    {
        for (int i = 0; i < this.CopyList.Count; i++)
        {
            ICopy copy = this.CopyList[i];
            if (copy.Try(copyid))
            {
                return copy;
            }
        }
        return this.defaultCopy;
    }

    private void EnterNewCopy(uint id)
    {
        ICopy copy = this.GetCopy(id);
        if (copy != null)
        {
            copy.EnterCopy();
            if (this.OnCopyEnterEvt != null)
            {
                this.OnCopyEnterEvt(copy);
            }
        }
    }

    private void ExitCurrentCopy()
    {
        if (this.CurrentCopy != null)
        {
            this.CurrentCopy.ExitCopy();
            if (this.OnCopyExitEvt != null)
            {
                this.OnCopyExitEvt(this.CurrentCopy);
            }
        }
    }

    public void SetCurrentCopy(uint id, uint subcopy)
    {
        this.subcopymapidx = subcopy;
        if (this.currentcopyid != id)
        {
            this.ExitCurrentCopy();
            this.currentcopyid = id;
            CallLuaListener.SendLuaEvent("OnCopyIDChangeLuaListener", false, new object[]
            {
                this.currentcopyid
            });
            if (this.currentcopyid != 0U)
            {
                this.currentCopyConfig = LuaConfigManager.GetConfigTable("copymapmaster", (ulong)this.currentcopyid);
                this.EnterNewCopy(id);
            }
            else
            {
                this.currentCopyConfig = null;
                this.CloseCompleteCopyView();
            }
            if (this.OnCopyChangeEvt != null)
            {
                this.OnCopyChangeEvt();
            }
        }
    }

    public void Luafun_ShowEnterCopy(List<VarType> paras)
    {
        if (paras.Count != 1)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        uint num = paras[0];
        LuaScriptMgr.Instance.CallLuaFunction("EnterCopyCtrl.ReqEnterCopyInfo", new object[]
        {
            Util.GetLuaTable("EnterCopyCtrl"),
            num
        });
    }

    public void Luafun_ExitCopy(List<VarType> paras)
    {
        if (paras.Count != 0)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.StopAutoattackWhenLeftCopy();
        LuaScriptMgr.Instance.CallLuaFunction("EnterCopyCtrl.ReqExitCopymap", new object[]
        {
            Util.GetLuaTable("EnterCopyCtrl")
        });
        ICopy copy = this.GetCopy(this.currentcopyid);
        if (copy != null)
        {
            copy.ExitCopy();
        }
    }

    public void ShowCompleteCopyViewOver(MSG_Ret_CopymapOver_SC data)
    {
        ControllerManager.Instance.GetController<CompleteCopyController>().SetCompleteCopyData(data);
        ControllerManager.Instance.GetController<CompleteCopyController>().ShowCompleteCopyView();
    }

    private void ShowCompleteCopyView(Action<UI_CompleteCopy> callback)
    {
        ICopy copy = this.GetCopy(this.currentcopyid);
        if (copy != null)
        {
            copy.ExitCopy();
        }
        if (this.UiCompleteCopy == null)
        {
            this.StopAutoattackWhenLeftCopy();
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_CompleteCopy>("UI_InstanceOver", delegate ()
            {
                if (callback != null)
                {
                    callback(this.UiCompleteCopy);
                }
            }, UIManager.ParentType.CommonUI, false);
        }
        else if (callback != null)
        {
            callback(this.UiCompleteCopy);
        }
    }

    public void CloseCompleteCopyView()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_InstanceOver");
        UI_Map mapUI = ControllerManager.Instance.GetController<UIMapController>().MapUI;
        if (mapUI != null)
        {
            mapUI.SetMiddleExitCopyState(false);
        }
    }

    private void StopAutoattackWhenLeftCopy()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<AutoAttack>().AutoAttackOn)
        {
            MainPlayer.Self.GetComponent<AutoAttack>().SwitchModle(false);
        }
        ControllerManager.Instance.GetController<TaskController>().autoTask.Stop();
    }

    public void ReqExitCopy()
    {
        string s_describle = string.Empty;
        bool flag = false;
        bool teamid = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.teamid != 0U;
        uint num = this.currentcopyid / 100U;
        LuaTable configTable = LuaConfigManager.GetConfigTable("copymapmaster", (ulong)num);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, string.Concat(new object[]
            {
                "ReqExitCopy cant find this copy map id = ",
                num,
                " currentcopyid ",
                this.currentcopyid
            }));
        }
        else
        {
            flag = (configTable.GetField_Uint("daytimelimit") != 9999U);
        }
        if (!teamid && !flag)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.copy_leave_single_nolimit);
        }
        else if (!teamid && flag)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.copy_leave_single_limited);
        }
        else if (teamid && !flag)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.copy_leave_team_nolimit);
        }
        else if (teamid && flag)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.copy_leave_team_limited);
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, MsgBoxController.MsgOptionYes, MsgBoxController.MsgOptionNo, UIManager.ParentType.CommonUI, delegate ()
        {
            this.StopAutoattackWhenLeftCopy();
            LuaScriptMgr.Instance.CallLuaFunction("EnterCopyCtrl.ReqExitCopymap", new object[]
            {
                Util.GetLuaTable("EnterCopyCtrl")
            });
            ICopy copy = this.GetCopy(this.currentcopyid);
            if (copy != null)
            {
                copy.ExitCopy();
            }
        }, null, null);
    }

    public void ReqFinalExitCopy()
    {
        LuaScriptMgr.Instance.CallLuaFunction("EnterCopyCtrl.ReqExitCopymap", new object[]
        {
            Util.GetLuaTable("EnterCopyCtrl")
        });
        ICopy copy = this.GetCopy(this.currentcopyid);
        if (copy != null)
        {
            copy.ExitCopy();
        }
    }

    public void SetBossTempid(List<string> idList)
    {
        if (idList == null)
        {
            return;
        }
        this.currentBossTempidList = new List<ulong>();
        for (int i = 0; i < idList.Count; i++)
        {
            this.currentBossTempidList.Add(ulong.Parse(idList[i]));
        }
    }

    public CharactorBase[] GetCurrentCopyBossArr()
    {
        this.BossArr.Clear();
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        for (int i = 0; i < this.currentBossTempidList.Count; i++)
        {
            CharactorBase charactorByID = manager.GetCharactorByID(this.currentBossTempidList[i], CharactorType.NPC);
            if (charactorByID != null)
            {
                this.BossArr.Add(charactorByID);
            }
        }
        return this.BossArr.ToArray();
    }

    public bool IsCopyBoss(EntitiesID bossEID)
    {
        if (bossEID.Etype != CharactorType.NPC)
        {
            return false;
        }
        if (this.currentBossTempidList == null)
        {
            FFDebug.LogWarning(this, "currentBossTempidList null");
            return false;
        }
        return this.currentBossTempidList.Contains(bossEID.Id);
    }

    public void CheckPlayCopyBossDieCameraMove(CharactorBase boss)
    {
        if (boss == null)
        {
            return;
        }
        if (!this.InCopy)
        {
            return;
        }
        if (this.IsCopyBoss(boss.EID))
        {
            if (this.currentCopyConfig == null)
            {
                FFDebug.LogWarning(this, "CheckPlayCopyBossDieCameraMove currentCopyConfig null");
                return;
            }
            string field_String = this.currentCopyConfig.GetField_String("bossdiecameramove");
            if (string.IsNullOrEmpty(field_String))
            {
                FFDebug.LogWarning(this, "ParamString null");
                return;
            }
            BossDieCameraMove bossDieCameraMove = new BossDieCameraMove();
            bossDieCameraMove.OnCameraMoveOver = delegate ()
            {
                Time.timeScale = 1f;
            };
            bossDieCameraMove.SetTarget(boss);
            if (bossDieCameraMove.PlayCameraMove(field_String))
            {
                CameraController.Self.ChangeState(bossDieCameraMove);
            }
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
        for (int i = 0; i < this.CopyList.Count; i++)
        {
            this.CopyList[i].Update();
        }
    }

    public void OnReSet()
    {
        this.CopyList.Clear();
    }

    public CopyNetWork MCopyNetWork;

    private uint currentcopyid;

    private uint subcopymapidx;

    public LuaTable currentCopyConfig;

    private List<ulong> currentBossTempidList;

    public ICopy defaultCopy;

    private uint configPkmap;

    private uint configSidoou;

    private uint configDuoqi;

    public Action<ICopy> OnCopyEnterEvt;

    public Action<ICopy> OnCopyExitEvt;

    public Action OnCopyChangeEvt;

    private List<ICopy> CopyList = new List<ICopy>();

    private List<CharactorBase> BossArr = new List<CharactorBase>();
}

public interface ICopy
{
    uint CopyID { get; }
    bool Try(uint copymapid);
    void InitCopy();
    void EnterCopy();
    void ExitCopy();
    void OnComplete(float time);
    void OnOver(uint countdowm);
    void Update();
}
