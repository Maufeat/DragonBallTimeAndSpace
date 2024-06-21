using System;
using System.Collections;
using System.Reflection;
using System.Text;
using Framework.Managers;
using mobapk;
using Net;
using ProtoBuf;

public class AbattoirMatchNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_MatchInfo_SC>(2576, new ProtoMsgCallback<MSG_MatchInfo_SC>(this.OnReceiveTeamMatchInfo));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_DismissGroup_SC>(2577, new ProtoMsgCallback<MSG_DismissGroup_SC>(this.OnReceiveDismissGroup));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_MyTeamInfo_SC>(2578, new ProtoMsgCallback<MSG_MyTeamInfo_SC>(this.OnReceiveMyTeamInfo));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RefreshRadarPos_CSC>(2583, new ProtoMsgCallback<MSG_RefreshRadarPos_CSC>(this.OnRefreshRadarPos));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_BstUserTeamInfo_SC>(2585, new ProtoMsgCallback<MSG_BstUserTeamInfo_SC>(this.OnGetTeamListAndColor));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RefreshPowerRank_SC>(2579, new ProtoMsgCallback<MSG_RefreshPowerRank_SC>(this.OnRefreshPowerRank));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ServerTimer_SC>(2582, new ProtoMsgCallback<MSG_ServerTimer_SC>(this.OnRefreshServerTime));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_GameOver_SC>(2588, new ProtoMsgCallback<MSG_GameOver_SC>(this.OnGameOver));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RewardBagInfo_SC>(2590, new ProtoMsgCallback<MSG_RewardBagInfo_SC>(this.OnRewardUpdate));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_MobaLevelUp_SC>(2591, new ProtoMsgCallback<MSG_MobaLevelUp_SC>(this.OnMobaLevelUp));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_ClientEffect_SC>(2592, new ProtoMsgCallback<MSG_ClientEffect_SC>(this.OnClientEffect));
    }

    private void OnReceiveTeamMatchInfo(MSG_MatchInfo_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().UpdateTeamMatchInfo(MsgData);
    }

    private void OnReceiveDismissGroup(MSG_DismissGroup_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().DismissGroup(MsgData);
    }

    private void OnReceiveMyTeamInfo(MSG_MyTeamInfo_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().MyTeamInfo(MsgData);
    }

    private void OnRefreshRadarPos(MSG_RefreshRadarPos_CSC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().RefreshRadarPos(MsgData);
    }

    private void OnGetTeamListAndColor(MSG_BstUserTeamInfo_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().RefreashTeamListAndColor(MsgData);
    }

    private void OnRefreshPowerRank(MSG_RefreshPowerRank_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().RefreshPowerRank(MsgData);
    }

    private void OnRefreshServerTime(MSG_ServerTimer_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().RefreshServerTime(MsgData);
    }

    private void OnGameOver(MSG_GameOver_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().GameOver(MsgData);
    }

    private void OnRewardUpdate(MSG_RewardBagInfo_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().RefreshReward(MsgData);
    }

    private void OnMobaLevelUp(MSG_MobaLevelUp_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().OnMobaLevelUp(MsgData);
    }

    private void OnClientEffect(MSG_ClientEffect_SC MsgData)
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().OnClientEffect(MsgData);
    }

    public void SendMatchReq(MSG_UserMatchReq_CS msg)
    {
        base.SendMsg<MSG_UserMatchReq_CS>(CommandID.MSG_UserMatchReq_CS, msg, false);
    }

    public void SendReqRadarPos()
    {
        MSG_RefreshRadarPos_CSC t = new MSG_RefreshRadarPos_CSC();
        base.SendMsg<MSG_RefreshRadarPos_CSC>(CommandID.MSG_RefreshRadarPos_CSC, t, false);
    }

    public void SendMatchReady(MSG_MatchReady_CS msg)
    {
        base.SendMsg<MSG_MatchReady_CS>(CommandID.MSG_MatchReady_CS, msg, false);
    }

    public void SendUseCapsuleItemByPos(MSG_UseSpecialCapsule_CS msg)
    {
        base.SendMsg<MSG_UseSpecialCapsule_CS>(CommandID.MSG_UseSpecialCapsule_CS, msg, false);
    }

    public void SendChooseReward(MSG_UserGetAwardReq_CS msg)
    {
        base.SendMsg<MSG_UserGetAwardReq_CS>(CommandID.MSG_UserGetAwardReq_CS, msg, false);
    }

    private object ToEmty(object obj)
    {
        return string.Empty;
    }

    private object ToJson(object obj, string from = "")
    {
        Type type = obj.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        for (int i = 0; i < fields.Length; i++)
        {
            object value = fields[i].GetValue(obj);
            if (value is IList)
            {
                IList list = value as IList;
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] is IExtensible)
                    {
                        this.ToJson(list[j], string.Concat(new object[]
                        {
                            from,
                            "->",
                            fields[i].Name,
                            "[",
                            j,
                            "]"
                        }));
                    }
                    else
                    {
                        FFDebug.LogError(list[j].GetType().Name, string.Concat(new object[]
                        {
                            from,
                            "->",
                            fields[i].Name,
                            "[",
                            j,
                            "]",
                            "->",
                            list[j]
                        }));
                    }
                }
            }
            else if (value is IExtensible)
            {
                this.ToJson(value, from + "->" + fields[i].Name);
            }
            else
            {
                FFDebug.LogError(fields[i].GetType().Name, string.Concat(new object[]
                {
                    from,
                    "->",
                    fields[i].Name,
                    "->",
                    value
                }));
            }
        }
        return obj;
    }

    private StringBuilder builder = new StringBuilder();

    private class MsgFields
    {
        public string name;

        public object value;
    }
}
