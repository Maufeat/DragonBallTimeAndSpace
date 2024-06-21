using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Pry : UIPanelBase
{
    private PryController pryController
    {
        get
        {
            return ControllerManager.Instance.GetController<PryController>();
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.UnInit();
        GameObject gameObject = this.root.gameObject;
        this.root = null;
        UnityEngine.Object.Destroy(gameObject);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.uistate = 1;
        this.InitObj(root);
        this.InitEvent();
    }

    public void UnInit()
    {
        this.uistate = 1;
        this.currentguild = 0UL;
        this.currentguildname = string.Empty;
    }

    private void InitObj(Transform root)
    {
        this.root = root;
        this.lb_GuildName = root.transform.Find("Offset_Activity_Poke/UIPanel_Large/guildinfo/txt_guildname").GetComponent<Text>();
        this.lb_Intructions = root.transform.Find("Offset_Activity_Poke/UIPanel_Large/guildinfo/txt_info").GetComponent<Text>();
        this.btn_SetTarget = root.transform.Find("Offset_Activity_Poke/UIPanel_Large/guildinfo/btn_set").GetComponent<Button>();
        this.mapPanel = root.transform.Find("Offset_Activity_Poke/UIPanel_Large/map").gameObject;
        this.sp_Map = this.mapPanel.transform.Find("img_map").GetComponent<Image>();
        this.btn_WenttoTargetGuild = this.mapPanel.transform.Find("btn_go").GetComponent<Button>();
        this.selectTargetPanel = root.transform.Find("Offset_Activity_Poke/UIPanel_Large/guildlist").gameObject;
        this.btn_SelectGuildConfirm = this.selectTargetPanel.transform.Find("btn_set").GetComponent<Button>();
        this.guildlistscroll = this.selectTargetPanel.transform.Find("UIFoldOutList").gameObject;
        this.guildListNoneTag = this.selectTargetPanel.transform.Find("txt_nonelist").gameObject;
        this.btn_Close = root.transform.Find("Offset_Activity_Poke/UIPanel_Large/title/btn_close").GetComponent<Button>();
        this.selectTargetPanel.SetActive(false);
        this.mapPanel.SetActive(true);
        this.guildListNoneTag.SetActive(false);
    }

    private void InitEvent()
    {
        this.btn_Close.onClick.RemoveAllListeners();
        this.btn_Close.onClick.AddListener(new UnityAction(this.OnClickClose));
        this.btn_SetTarget.onClick.RemoveAllListeners();
        this.btn_SetTarget.onClick.AddListener(new UnityAction(this.OnClickSetTargetWindow));
        this.btn_SelectGuildConfirm.onClick.RemoveAllListeners();
        this.btn_SelectGuildConfirm.onClick.AddListener(new UnityAction(this.OnClickSelectGuildConfirm));
        this.btn_WenttoTargetGuild.onClick.RemoveAllListeners();
        this.btn_WenttoTargetGuild.onClick.AddListener(new UnityAction(this.OnWenttoEnemyGuild));
    }

    public void RefreshData()
    {
        if (!this.pryController.IfHasTargetGuild())
        {
            this.lb_GuildName.text = "暂无";
        }
        else
        {
            this.lb_GuildName.text = this.pryController.currentPryGuild.guildname;
        }
        this.uistate = 1;
        this.selectTargetPanel.SetActive(false);
        this.mapPanel.SetActive(true);
    }

    private void OnClickClose()
    {
        this.pryController.ClosePryUI();
    }

    private void OnClickSetTargetWindow()
    {
        if (this.uistate == 1)
        {
            if (this.pryController.IfHasTargetGuild())
            {
                TipsWindow.ShowWindow(TipsType.TODAY_SET_TARGET_FAMILY, null);
                return;
            }
            this.pryController.pryNetWork.ReqPryGuildList();
        }
        else
        {
            this.uistate = 1;
            this.selectTargetPanel.SetActive(false);
            this.mapPanel.SetActive(true);
        }
    }

    private void OnClickSelectGuildConfirm()
    {
        if (this.currentguild == 0UL)
        {
            TipsWindow.ShowWindow(TipsType.YOU_HAVE_NOT_TARGET_FAMILY, null);
            return;
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystem, string.Format(CommonUtil.GetText(dynamic_textid.IDs.sociaty_changetransmission), this.currentguildname), MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.CommonUI, delegate ()
        {
            this.pryController.pryNetWork.ReqChoosePryEnemyGuild(this.currentguild);
        }, null, null);
    }

    private void OnWenttoEnemyGuild()
    {
        if (!this.pryController.IfHasTargetGuild())
        {
            TipsWindow.ShowWindow(TipsType.NOT_TARGET_FAMILY_CANNOT_SEND, null);
            return;
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystem, CommonUtil.GetText(dynamic_textid.IDs.sociaty_targettransmission), MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.CommonUI, delegate ()
        {
            this.pryController.pryNetWork.ReqIntoPryEnemyGuild();
            this.pryController.ClosePryUI();
        }, null, null);
    }

    public void ShowPryEnemyGuildsList(MSG_Ret_CiTanEnemyGuildList_SC data)
    {
        this.uistate = 2;
        this.selectTargetPanel.SetActive(true);
        this.mapPanel.SetActive(false);
        this.guildList.Clear();
        this.selectflagmap.Clear();
        for (int i = 0; i < data.guildlist.Count; i++)
        {
            this.guildList.Add(data.guildlist[i]);
        }
        if (this.guildList.Count == 0)
        {
            this.guildlistscroll.SetActive(false);
            this.guildListNoneTag.SetActive(true);
        }
        else
        {
            this.guildListNoneTag.SetActive(false);
            this.guildlistscroll.SetActive(true);
            UIFoldOutList component = this.guildlistscroll.GetComponent<UIFoldOutList>();
            component.InitListAction = new Action<int, GameObject>(this.setGuildItem);
            component.SetPageInfo(1, 1, this.guildList.Count, null, null);
        }
    }

    private void setGuildItem(int i, GameObject ga)
    {
        this.CreatItem(ga, this.guildList[i]);
    }

    private void CreatItem(GameObject item, CiTanEnemyGuildItem guild)
    {
        this.selectflagmap[guild.guildid] = item.transform.Find("sp_back/sp_select").gameObject;
        Text component = item.transform.Find("txt_tdname").GetComponent<Text>();
        component.text = guild.guildname;
        Text component2 = item.transform.Find("txt_tdlv").GetComponent<Text>();
        component2.text = guild.guildlevel.ToString();
        Text component3 = item.transform.Find("txt_tdguilder").GetComponent<Text>();
        component3.text = guild.mastername;
        Color color = new Color(0.807843149f, 0.7411765f, 0.6156863f);
        Color color2 = new Color(0.6862745f, 0.6862745f, 0.6862745f);
        if (guild.isvalid == 1U)
        {
            component.color = color;
            component2.color = color;
            component3.color = color;
        }
        else
        {
            component.color = color2;
            component2.color = color2;
            component3.color = color2;
        }
        item.GetComponent<Button>().onClick.RemoveAllListeners();
        item.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (guild.isvalid != 1U)
            {
                return;
            }
            if (this.currentguild != guild.guildid)
            {
                this.currentguild = guild.guildid;
                this.currentguildname = guild.guildname;
                this.RefreshSelectFlag();
            }
        });
    }

    private void RefreshSelectFlag()
    {
        this.selectflagmap.BetterForeach(delegate (KeyValuePair<ulong, GameObject> item)
        {
            if (item.Key == this.currentguild)
            {
                item.Value.SetActive(true);
            }
            else
            {
                item.Value.SetActive(false);
            }
        });
    }

    private Transform root;

    private Text lb_GuildName;

    private Text lb_Intructions;

    private Button btn_SetTarget;

    private GameObject mapPanel;

    private Image sp_Map;

    private Button btn_WenttoTargetGuild;

    private GameObject selectTargetPanel;

    private Button btn_SelectGuildConfirm;

    private GameObject guildlistscroll;

    private GameObject guildListNoneTag;

    private Button btn_Close;

    private int uistate = 1;

    private List<CiTanEnemyGuildItem> guildList = new List<CiTanEnemyGuildItem>();

    private ulong currentguild;

    private string currentguildname = string.Empty;

    private BetterDictionary<ulong, GameObject> selectflagmap = new BetterDictionary<ulong, GameObject>();
}
