using System;
using System.Collections.Generic;
using Framework.Managers;
using guildpk_msg;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_GuildWar : UIPanelBase
{
    private GuildControllerNew mController
    {
        get
        {
            return ControllerManager.Instance.GetController<GuildControllerNew>();
        }
    }

    public override void OnInit(Transform root)
    {
        this.ui_root = root;
        this.InitObject();
        this.InitEvent();
    }

    public void SetOpenType(GuildWarUIType type)
    {
        List<GuildWarUIType> list = new List<GuildWarUIType>(this.uiTypeNodDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            this.uiTypeNodDic[list[i]].gameObject.SetActive(list[i] == type);
        }
    }

    private void InitObject()
    {
        this.teamMatchRoot = this.ui_root.Find("Offset/MatchTeam");
        this.realTimeInfoRoot = this.ui_root.Find("Offset/GWLive");
        this.settlementRoot = this.ui_root.Find("Offset/GWscore");
        this.rankLstRoot = this.ui_root.Find("Offset/GWrank");
        this.btnClose = this.ui_root.Find("Offset/MatchTeam/bg/Panel_title/CloseButton");
        this.waitListRectRoot = this.ui_root.Find("Offset/MatchTeam/bg/Scroll View");
        Transform transform = this.ui_root.Find("Offset/MatchTeam/bg/team/Panel");
        uint num = 0U;
        while ((ulong)num < (ulong)((long)transform.childCount))
        {
            this.teamRectRootDic[num + 1U] = transform.GetChild((int)num);
            num += 1U;
        }
        this.btnquitTeam = this.ui_root.Find("Offset/MatchTeam/bg/team/Button");
        this.uiTypeNodDic[GuildWarUIType.TeamMatch] = this.teamMatchRoot;
        this.uiTypeNodDic[GuildWarUIType.FightingInfo] = this.realTimeInfoRoot;
        this.uiTypeNodDic[GuildWarUIType.SettlementInfo] = this.settlementRoot;
        this.uiTypeNodDic[GuildWarUIType.RankingInfo] = this.rankLstRoot;
        this.teamMatchRoot.gameObject.SetActive(false);
        this.realTimeInfoRoot.gameObject.SetActive(false);
        this.settlementRoot.gameObject.SetActive(false);
        this.rankLstRoot.gameObject.SetActive(false);
        this.startFightReadyNode = this.ui_root.Find("Offset/GStartFightTimeCount");
    }

    private void InitEvent()
    {
        Button component = this.btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Close));
        Button component2 = this.btnquitTeam.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(delegate ()
        {
            this.mController.ReqQuitCurFightTeam();
        });
    }

    private void Close()
    {
        UIManager.Instance.DeleteUI<UI_GuildWar>();
    }

    public void OnGuildPkListInfo(MSG_Ret_GuildPkInfo_SC msg)
    {
        if (msg.guildinfo != null && msg.guildinfo.teaminfo.Count > 0)
        {
            for (int i = 0; i < msg.guildinfo.teaminfo.Count; i++)
            {
                if (msg.guildinfo.teaminfo[i].teamid == 0U)
                {
                    this.InitWaitLst(msg.guildinfo.teaminfo[i].members);
                }
                else if (this.teamRectRootDic.ContainsKey(msg.guildinfo.teaminfo[i].teamid))
                {
                    this.InitTeamLst(this.teamRectRootDic[msg.guildinfo.teaminfo[i].teamid], msg.guildinfo.teamlimit, msg.guildinfo.teaminfo[i]);
                }
            }
        }
        else
        {
            this.InitWaitLst(null);
            List<uint> list = new List<uint>(this.teamRectRootDic.Keys);
            for (int j = 0; j < list.Count; j++)
            {
                this.InitTeamLst(this.teamRectRootDic[list[j]], 0U, null);
            }
        }
    }

    private void InitWaitLst(List<GuildPkMemberInfo> members)
    {
        Transform transform = this.waitListRectRoot.Find("Viewport/Content");
        GameObject gameObject = transform.GetChild(0).gameObject;
        bool flag = members == null || members.Count == 0;
        int num = Mathf.Max(transform.childCount, (members != null) ? members.Count : 0);
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject2;
            if (i < transform.childCount)
            {
                gameObject2 = transform.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(transform, false);
                gameObject2.transform.localScale = gameObject.transform.localScale;
                gameObject2.name = gameObject.name;
            }
            if (flag)
            {
                gameObject2.gameObject.SetActive(false);
            }
            else if (i < members.Count)
            {
                gameObject2.SetActive(true);
                Text component = gameObject2.GetComponent<Text>();
                component.text = members[i].name + "  LV." + members[i].level;
            }
            else
            {
                gameObject2.SetActive(false);
            }
        }
    }

    private void InitTeamLst(Transform scrollRectRoot, uint openNum, GuildPkTeamInfo teamInfo = null)
    {
        bool flag = teamInfo.teamid <= openNum;
        Transform transform = scrollRectRoot.Find("member");
        GameObject gameObject = transform.GetChild(0).gameObject;
        uint num = (uint)Mathf.Max(transform.childCount, teamInfo.members.Count);
        num = (uint)Mathf.Max(num, 5f);
        Text component = scrollRectRoot.Find("img_title/Text").GetComponent<Text>();
        component.text = "小队" + teamInfo.teamid;
        Transform transform2 = scrollRectRoot.Find("img_not_open");
        transform2.gameObject.SetActive(!flag);
        if (!flag)
        {
            Text component2 = transform2.Find("Text").GetComponent<Text>();
            component2.text = string.Format("家族等级{0}开启", teamInfo.unlocklv);
        }
        for (uint num2 = 0U; num2 < num; num2 += 1U)
        {
            GameObject gameObject2;
            if ((ulong)num2 < (ulong)((long)transform.childCount))
            {
                gameObject2 = transform.GetChild((int)num2).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(transform, false);
                gameObject2.name = gameObject.name;
            }
            uint num3 = num2 + 1U;
            gameObject2.gameObject.SetActive(flag);
            GuildPkMemberInfo memberByPosID = this.GetMemberByPosID(num3, teamInfo.members);
            this.InitPkMemberItem(gameObject2, memberByPosID, teamInfo.teamid, num3);
        }
    }

    private void InitPkMemberItem(GameObject itemObj, GuildPkMemberInfo pkMember, uint curTeamId, uint curPos)
    {
        bool isHaveMember = pkMember != null;
        Button component = itemObj.GetComponent<Button>();
        Text component2 = itemObj.transform.Find("txt_player").GetComponent<Text>();
        Text component3 = itemObj.transform.Find("txt_lv").GetComponent<Text>();
        Text component4 = itemObj.transform.Find("txt_join").GetComponent<Text>();
        component2.gameObject.SetActive(isHaveMember);
        component3.gameObject.SetActive(isHaveMember);
        component4.gameObject.SetActive(!isHaveMember);
        if (isHaveMember)
        {
            component2.text = pkMember.name;
            component3.text = "   LV." + pkMember.level;
        }
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            if (isHaveMember)
            {
                TipsWindow.ShowNotice("This pos have other player");
            }
            else
            {
                this.mController.ReqJoinFightTeam(curTeamId, curPos);
            }
        });
    }

    private GuildPkMemberInfo GetMemberByPosID(uint posID, List<GuildPkMemberInfo> members)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].posid == posID)
            {
                return members[i];
            }
        }
        return null;
    }

    public void FrashFightingInfo(List<realtime_guildteam_info> teamrank)
    {
        this.SetOpenType(GuildWarUIType.FightingInfo);
        if (this.uiTypeNodDic.ContainsKey(GuildWarUIType.FightingInfo) && teamrank != null && teamrank.Count > 0)
        {
            Transform transform = this.uiTypeNodDic[GuildWarUIType.FightingInfo];
            Transform transform2 = transform.Find("img_bg/Content");
            GameObject gameObject = transform2.GetChild(0).gameObject;
            int num = Mathf.Max(teamrank.Count, transform2.childCount);
            for (int i = 0; i < num; i++)
            {
                GameObject gameObject2;
                if (i < transform2.childCount)
                {
                    gameObject2 = transform2.GetChild(i).gameObject;
                }
                else
                {
                    gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.name = gameObject.name;
                    gameObject2.transform.SetParent(transform2, false);
                }
                if (i < teamrank.Count)
                {
                    gameObject2.SetActive(true);
                    gameObject2.FindChild("txt_rank").GetComponent<Text>().text = i + 1 + string.Empty;
                    gameObject2.FindChild("txt_name").GetComponent<Text>().text = teamrank[i].guildname;
                    gameObject2.FindChild("txt_survivor").GetComponent<Text>().text = teamrank[i].leftnum + string.Empty;
                }
                else
                {
                    gameObject2.SetActive(false);
                }
            }
        }
    }

    public void FrashReadyFightTimeLeft(uint leftSec)
    {
        this.readyLeftFightTime = leftSec;
        this.startFightReadyNode.gameObject.SetActive(true);
        if (this.isAddFrashLeftReadFightAction)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.FrashLeftReadFightTime));
        }
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.FrashLeftReadFightTime));
        this.isAddFrashLeftReadFightAction = true;
    }

    private void FrashLeftReadFightTime()
    {
        if (this.readyLeftFightTime > 0U)
        {
            if (this.startFightReadyNode == null)
            {
                Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.FrashLeftReadFightTime));
                return;
            }
            this.startFightReadyNode.Find("img_bg/txt_time").GetComponent<Text>().text = this.readyLeftFightTime.ToString();
            this.readyLeftFightTime -= 1U;
        }
        else
        {
            this.startFightReadyNode.gameObject.SetActive(false);
            this.isAddFrashLeftReadFightAction = false;
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.FrashLeftReadFightTime));
        }
    }

    public void OnFightOver(bool isWin, List<finalresult_guildteam_info> teamlist)
    {
        this.SetOpenType(GuildWarUIType.SettlementInfo);
        Transform transform = this.settlementRoot.Find("Panel_title/img_win");
        Transform transform2 = this.settlementRoot.Find("Panel_title/img_lose");
        transform.gameObject.SetActive(isWin);
        transform2.gameObject.SetActive(!isWin);
        Transform transform3 = this.settlementRoot.Find("Scroll View/Viewport/Content");
        GameObject gameObject = transform3.GetChild(0).gameObject;
        int num = Mathf.Max(transform3.childCount, teamlist.Count);
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject2;
            if (i < transform3.childCount)
            {
                gameObject2 = transform3.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.name = gameObject.name;
                gameObject2.transform.SetParent(transform3, false);
            }
            if (i < teamlist.Count)
            {
                gameObject2.gameObject.SetActive(true);
                gameObject2.transform.Find("txt_rank").GetComponent<Text>().text = teamlist[i].rank.ToString();
                gameObject2.transform.Find("txt_family").GetComponent<Text>().text = teamlist[i].name.ToString();
                gameObject2.transform.Find("txt_damage").GetComponent<Text>().text = teamlist[i].totaldmg.ToString();
                gameObject2.transform.Find("txt_kill").GetComponent<Text>().text = teamlist[i].killnum.ToString();
            }
            else
            {
                gameObject2.gameObject.SetActive(false);
            }
        }
        Button component = this.settlementRoot.transform.Find("img_bottom/btn_exit").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.mController.LeaveFight();
        });
    }

    public void OnGetRankScore(List<GuildPkGuildScore> scoreList)
    {
        this.SetOpenType(GuildWarUIType.RankingInfo);
        Button component = this.rankLstRoot.Find("Panel_title/btn_close").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Close));
        Toggle component2 = this.rankLstRoot.Find("ToggleGroup/Panel_tab/win").GetComponent<Toggle>();
        component2.onValueChanged.RemoveAllListeners();
        component2.onValueChanged.AddListener(new UnityAction<bool>(this.OnLookWinRankList));
        Transform transform = this.rankLstRoot.Find("rank/Scroll View/Viewport/Content");
        GameObject gameObject = transform.GetChild(0).gameObject;
        int num = Mathf.Max(transform.childCount, scoreList.Count);
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject2;
            if (i < transform.childCount)
            {
                gameObject2 = transform.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.name = gameObject.name;
                gameObject2.transform.SetParent(transform, false);
            }
            if (i < scoreList.Count)
            {
                gameObject2.gameObject.SetActive(true);
                gameObject2.transform.Find("txt_rank").GetComponent<Text>().text = scoreList[i].rank.ToString();
                gameObject2.transform.Find("txt_name").GetComponent<Text>().text = scoreList[i].guildname.ToString();
                gameObject2.transform.Find("txt_score").GetComponent<Text>().text = scoreList[i].score.ToString();
                gameObject2.transform.Find("img_rank").gameObject.SetActive(i <= 2);
            }
            else
            {
                gameObject2.gameObject.SetActive(false);
            }
        }
    }

    private void OnLookWinRankList(bool b)
    {
        if (b)
        {
            this.mController.TrGetWinTeamRankData(new Action<List<GuildPkWinInfo>>(this.OnGetWinTeamRankListData));
        }
    }

    private void OnGetWinTeamRankListData(List<GuildPkWinInfo> winInfo)
    {
        Transform transform = this.rankLstRoot.Find("win/Scroll View/Viewport/Content");
        GameObject gameObject = transform.GetChild(0).gameObject;
        int num = Mathf.Max(transform.childCount, winInfo.Count);
        Color white = Color.white;
        Color clear = Color.clear;
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject2;
            if (i < transform.childCount)
            {
                gameObject2 = transform.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.name = gameObject.name;
                gameObject2.transform.SetParent(transform, false);
            }
            if (i < winInfo.Count)
            {
                gameObject2.gameObject.SetActive(true);
                gameObject2.transform.Find("txt_rank").GetComponent<Text>().text = (i + 1).ToString();
                gameObject2.transform.Find("txt_name").GetComponent<Text>().text = winInfo[i].win_guild_name;
                gameObject2.transform.Find("txt_score").GetComponent<Text>().text = winInfo[i].win_leader_name;
                gameObject2.GetComponent<Image>().color = ((i % 2 != 0) ? clear : white);
            }
            else
            {
                gameObject2.gameObject.SetActive(false);
            }
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private Transform ui_root;

    private Transform btnClose;

    private Dictionary<uint, Transform> teamRectRootDic = new Dictionary<uint, Transform>();

    private Dictionary<GuildWarUIType, Transform> uiTypeNodDic = new Dictionary<GuildWarUIType, Transform>();

    private Transform waitListRectRoot;

    private Transform btnquitTeam;

    private Transform teamMatchRoot;

    private Transform rankLstRoot;

    private Transform realTimeInfoRoot;

    private Transform settlementRoot;

    private Transform startFightReadyNode;

    private uint readyLeftFightTime;

    private bool isAddFrashLeftReadFightAction;
}
