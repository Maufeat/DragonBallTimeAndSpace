using System;
using System.Text;
using Framework.Managers;
using Game.Scene;
using Net;
using quest;

public class TaskNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_VisitNpcTrade_SC>(2393, new ProtoMsgCallback<MSG_Ret_VisitNpcTrade_SC>(this.OnVisitNpcTrade));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetMapQuestInfo_SC>(2403, new ProtoMsgCallback<MSG_RetMapQuestInfo_SC>(this.HandleMapQuestInfo));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetPlotTalkID_SC>(2408, new ProtoMsgCallback<MSG_RetPlotTalkID_SC>(this.OnRetPlotTalkID));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_CartoonCompleteNotify_SC>(2409, new ProtoMsgCallback<MSG_CartoonCompleteNotify_SC>(this.OnRetComic));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_NotifyCountDown_SC>(CommandID.MSG_Ret_NotifyCountDown_SC, new ProtoMsgCallback<MSG_Ret_NotifyCountDown_SC>(this.NotifyCountDownCb));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_OutOfCircle_SC>(2602, new ProtoMsgCallback<MSG_Ret_OutOfCircle_SC>(this.OnOutOfCircle));
    }

    private void OnOutOfCircle(MSG_Ret_OutOfCircle_SC msg_Ret_OutOfCircle_SC)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.second10Show));
        ManagerCenter.Instance.GetManager<GameScene>().UnRegOnSceneLoadCallBack(new Action(this.OnOutScene));
        this.msg_Ret_OutOfCircle_SC = msg_Ret_OutOfCircle_SC;
        uint state = msg_Ret_OutOfCircle_SC.state;
        if (state != 1U)
        {
            UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
            if (mainView != null && mainView.DramaTips != null)
            {
                mainView.DramaTips.Show(msg_Ret_OutOfCircle_SC.state == 1U, msg_Ret_OutOfCircle_SC.npcid, msg_Ret_OutOfCircle_SC.tipid);
            }
        }
        else
        {
            Scheduler.Instance.AddTimer(10f, true, new Scheduler.OnScheduler(this.second10Show));
            ManagerCenter.Instance.GetManager<GameScene>().RegOnSceneLoadCallBack(new Action(this.OnOutScene));
            UI_MainView mainView2 = ControllerManager.Instance.GetController<MainUIController>().mainView;
            if (mainView2 != null && mainView2.DramaTips != null)
            {
                mainView2.DramaTips.Show(msg_Ret_OutOfCircle_SC.state == 1U, msg_Ret_OutOfCircle_SC.npcid, msg_Ret_OutOfCircle_SC.tipid);
            }
        }
    }

    private void OnOutScene()
    {
        this.msg_Ret_OutOfCircle_SC = null;
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null && mainView.DramaTips != null)
        {
            mainView.DramaTips.Show(false, 0U, 0U);
        }
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.second10Show));
        ManagerCenter.Instance.GetManager<GameScene>().UnRegOnSceneLoadCallBack(new Action(this.OnOutScene));
    }

    private void second10Show()
    {
        MSG_Ret_OutOfCircle_SC msg_Ret_OutOfCircle_SC = this.msg_Ret_OutOfCircle_SC;
        if (msg_Ret_OutOfCircle_SC == null)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.second10Show));
            return;
        }
        if (msg_Ret_OutOfCircle_SC.state != 1U)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.second10Show));
        }
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null && mainView.DramaTips != null)
        {
            mainView.DramaTips.Show(msg_Ret_OutOfCircle_SC.state == 1U, msg_Ret_OutOfCircle_SC.npcid, msg_Ret_OutOfCircle_SC.tipid);
        }
    }

    private void NotifyCountDownCb(MSG_Ret_NotifyCountDown_SC data)
    {
        if (data.bset)
        {
            Scheduler.Instance.AddTimer(data.delay, false, delegate
            {
                ControllerManager.Instance.GetController<CoolDownController>().OpenUI((int)data.seconds);
            });
        }
        else
        {
            ControllerManager.Instance.GetController<CoolDownController>().CloseUI();
        }
    }

    public void ReqMSG_ReqCurActiveQuest_SC()
    {
        MSG_ReqCurActiveQuest_CS t = new MSG_ReqCurActiveQuest_CS();
        base.SendMsg<MSG_ReqCurActiveQuest_CS>(CommandID.MSG_ReqCurActiveQuest_CS, t, false);
    }

    public void ReqVisitNpcTrade(ulong npcid)
    {
        FFDebug.Log(this, FFLogType.Task, "ReqVisitNpc:" + npcid);
        base.SendMsg<MSG_Req_VisitNpcTrade_CS>(CommandID.MSG_Req_VisitNpcTrade_CS, new MSG_Req_VisitNpcTrade_CS
        {
            npc_temp_id = npcid
        }, false);
        if (TestTaskBackData.mSwitch)
        {
            TestTaskBackData.testBackData = string.Empty;
            TestTaskBackData.testBackData += "*************************** ReqVisitNpcTrade ***************************\r\n";
            string testBackData = TestTaskBackData.testBackData;
            TestTaskBackData.testBackData = string.Concat(new object[]
            {
                testBackData,
                "npcid:",
                npcid,
                "\r\n"
            });
            TestTaskBackData.testBackData += "***************************  ReqVisitNpcTrade  ***************************\r\n";
        }
    }

    public void OnVisitNpcTrade(MSG_Ret_VisitNpcTrade_SC mdata)
    {
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (mdata.retcode != 1U)
        {
            if (component != null)
            {
                component.CurrentVisteNpcID = 0UL;
            }
            FFDebug.Log(this, FFLogType.Task, "OnVisitNpcTrade Error " + mdata.retcode);
            return;
        }
        if (component != null)
        {
            component.CurrentVisteNpcID = mdata.npc_temp_id;
        }
        StringBuilder stringBuilder = new StringBuilder(mdata.user_menu);
        stringBuilder.Append(mdata.npc_menu);
        LuaProcess.ParserAndCallNpcLua(stringBuilder.ToString(), mdata.npc_temp_id, mdata.source);
    }

    public void ReqExecuteQuest(uint id, string target, uint offset, uint questdesccrc, ulong chartarget = 0UL, bool isUseNpcID = false)
    {
        MSG_ReqExecuteQuest_CS msg_ReqExecuteQuest_CS = new MSG_ReqExecuteQuest_CS();
        if (chartarget == 0UL)
        {
            TaskController controller = ControllerManager.Instance.GetController<TaskController>();
            controller.TryGetQuestNPCID(ref chartarget, isUseNpcID);
        }
        msg_ReqExecuteQuest_CS.id = id;
        msg_ReqExecuteQuest_CS.target = target;
        msg_ReqExecuteQuest_CS.offset = offset;
        msg_ReqExecuteQuest_CS.questdesccrc = questdesccrc;
        msg_ReqExecuteQuest_CS.chartarget = chartarget;
        base.SendMsg<MSG_ReqExecuteQuest_CS>(CommandID.MSG_ReqExecuteQuest_CS, msg_ReqExecuteQuest_CS, false);
    }

    public void ReqMapQuestInfo()
    {
        base.SendMsg<MSG_ReqMapQuestInfo_CS>(CommandID.MSG_ReqMapQuestInfo_CS, new MSG_ReqMapQuestInfo_CS(), false);
    }

    public void HandleMapQuestInfo(MSG_RetMapQuestInfo_SC data)
    {
        ControllerManager.Instance.GetController<TaskController>().InitNpcTaskMap(data.npclists);
    }

    public void OnRetPlotTalkID(MSG_RetPlotTalkID_SC mdata)
    {
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.DramaTips.ShowDramaTips(mdata.groupid);
        }
    }

    public void OnRetComic(MSG_CartoonCompleteNotify_SC mdata)
    {
        ControllerManager.Instance.GetController<ComicController>().ShowComic(mdata.groupid, mdata.delay, mdata.command);
    }

    public void ReqOnStageEnd()
    {
    }

    private MSG_Ret_OutOfCircle_SC msg_Ret_OutOfCircle_SC;
}
