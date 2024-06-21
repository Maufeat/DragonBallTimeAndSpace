using System;
using System.Collections.Generic;
using battle;
using Models;

public class DuoQiController : ControllerBase
{
    private UI_DuoQi mUIDuoQi
    {
        get
        {
            return UIManager.GetUIObject<UI_DuoQi>();
        }
    }

    public override void Awake()
    {
        this.mNetworker = new DuoQiNetworker();
        this.mNetworker.Initialize();
    }

    public void RefreshMyCampId(uint campid)
    {
        this.mMyCampId = campid;
    }

    public void OpenPanel()
    {
        if (UIManager.GetUIObject<UI_DuoQi>() != null)
        {
            UIManager.Instance.DeleteUI<UI_DuoQi>();
        }
        else
        {
            UIManager.Instance.ShowUI<UI_DuoQi>("UI_Battlefield", delegate ()
            {
                this.ReqHoldFlagReport();
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void OpenAccountPanel(uint campid, List<HoldFlagReport> reports)
    {
        this.mItemList = reports;
        bool mIsWin = campid == this.mMyCampId;
        if (this.mUIDuoQi == null)
        {
            UIManager.Instance.ShowUI<UI_DuoQi>("UI_Battlefield", delegate ()
            {
                this.mUIDuoQi.Initilize(true, mIsWin);
                this.mUIDuoQi.SetupPanel(reports);
            }, UIManager.ParentType.CommonUI, false);
        }
        else
        {
            this.mUIDuoQi.Initilize(true, mIsWin);
            this.mUIDuoQi.SetupPanel(reports);
        }
    }

    public void RefreshBallState(List<HoldFlagDBState> states)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (this.IsSameCamp(states[i].campId))
            {
                this.mMyCampBallState = states[i];
                return;
            }
        }
    }

    public bool CheckIsMyBallAndInBase(ulong tempid)
    {
        return this.CheckIsMyBall(tempid) && this.CheckIsMyBallInBase();
    }

    private bool CheckIsMyBall(ulong tempid)
    {
        return this.mMyCampBallState != null && this.mMyCampBallState.tempId == tempid;
    }

    private bool CheckIsMyBallInBase()
    {
        return this.mMyCampBallState != null && this.mMyCampBallState.isInBase;
    }

    public ulong GetingBallEmemyId()
    {
        return this.mGetingBallEmemyId;
    }

    public void RefreshEmemyIdWhoGetingBall(List<HoldFlagDBState> states)
    {
        this.mGetingBallEmemyId = 0UL;
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].DBState && this.IsSameCamp(states[i].campId))
            {
                this.mGetingBallEmemyId = states[i].capUserId;
            }
        }
    }

    public void ReqHoldFlagReport()
    {
        this.mNetworker.ReqHoldFlagReport();
    }

    public void RetHoldFlagReport(List<HoldFlagReport> data)
    {
        this.mItemList = data;
        this.mUIDuoQi.Initilize(false, false);
        this.mUIDuoQi.SetupPanel(data);
    }

    public bool IsSameCamp(uint campid)
    {
        return this.mMyCampId == campid;
    }

    public void ReqHoldFlagCaptureDB(ulong npcid)
    {
        this.mNetworker.ReqHoldFlagCaptureDB(npcid);
    }

    public void ReqHoldFlagPutDownDB()
    {
        this.mNetworker.ReqHoldFlagPutDownDB();
    }

    public void ReqExitCopy()
    {
        this.mNetworker.ReqExitCopy();
    }

    public void ClosePanel()
    {
    }

    public bool InBattleState()
    {
        return !(UIManager.GetUIObject<UI_MainView>() == null) && UIManager.GetUIObject<UI_MainView>().IsInBattleScene();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string ControllerName
    {
        get
        {
            return "duoqi";
        }
    }

    private DuoQiNetworker mNetworker;

    private uint mMyCampId;

    public List<HoldFlagReport> mItemList;

    private HoldFlagDBState mMyCampBallState;

    private ulong mGetingBallEmemyId;
}
