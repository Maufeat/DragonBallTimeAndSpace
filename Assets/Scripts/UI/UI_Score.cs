using System;
using System.Collections.Generic;
using Framework.Managers;
using msg;
using rankpk_msg;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Score : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<PVPCompetitionController>();
        this.InitObj(root);
        this.InitEvent();
        if (this.controller != null && this.controller.teamLeftInfo != null)
        {
            this.SetTeamNumber(this.controller.teamLeftInfo.team1left, this.controller.teamLeftInfo.team2left);
        }
    }

    private void InitObj(Transform root)
    {
        this.ui_root = root;
        this.panelRoot = root.Find("Offset_Score");
        this.panelScore = this.panelRoot.Find("Panel_PVPScore");
        this.txtTime = this.panelRoot.Find("Panel_PVPScore/txt_time");
        this.txtNum1 = this.panelRoot.Find("Panel_PVPScore/img_side1/txt_num1");
        this.txtNum2 = this.panelRoot.Find("Panel_PVPScore/img_side2/txt_num2");
        this.txtInfo = this.panelRoot.Find("Panel_PVPScore/txt_info");
        this.objMask = this.panelRoot.Find("Panel_CompetitonOver/img_mask");
        this.objWin = this.panelRoot.Find("Panel_CompetitonOver/win");
        this.objLose = this.panelRoot.Find("Panel_CompetitonOver/lose");
        this.objBeginTips = this.panelRoot.Find("Panel_PVPTips/begin");
        this.objSpeedupTips = this.panelRoot.Find("Panel_PVPTips/speedup");
        this.transAward = this.panelRoot.Find("Panel_CompetitonOver/awards");
        this.transItem = this.panelRoot.Find("Panel_CompetitonOver/awards/Item1");
        this.transResult = this.panelRoot.Find("PVPResult");
        this.btnExit = this.panelRoot.Find("PVPResult/Panel_title/btn_close");
        this.btnLeave = this.panelRoot.Find("PVPResult/img_bottom/btn_exit");
        this.objTitleWin = this.panelRoot.Find("PVPResult/Panel_title/img_win");
        this.objTitleLose = this.panelRoot.Find("PVPResult/Panel_title/img_lose");
        this.panelRoot.Find("PVPResult/img_bottom/btn_again").gameObject.SetActive(false);
        for (int i = 0; i < this.MAX_TEAM_NUM; i++)
        {
            UI_Score.ResultInfoItem item = this.panelRoot.Find("PVPResult/Image/Panel_side1/img_player" + (i + 1)).gameObject.AddComponent<UI_Score.ResultInfoItem>();
            this._team1ResultList.Add(item);
        }
        for (int j = 0; j < this.MAX_TEAM_NUM; j++)
        {
            UI_Score.ResultInfoItem item2 = this.panelRoot.Find("PVPResult/Image/Panel_side2/img_player" + (j + 1)).gameObject.AddComponent<UI_Score.ResultInfoItem>();
            this._team2ResultList.Add(item2);
        }
        this.transResult.gameObject.SetActive(false);
        this.panelScore.gameObject.SetActive(true);
    }

    private void InitEvent()
    {
        Button component = this.btnExit.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.OnExitButtonClick));
        Button component2 = this.btnLeave.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.OnLeaveButtonClick));
    }

    public void SetScoreTime(float time)
    {
        if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState == StageType.CountDown)
        {
            this.txtInfo.GetComponent<Text>().text = "准备倒计时";
        }
        this.txtTime.GetComponent<Text>().text = GlobalRegister.GetTimeInMinutes((uint)time);
    }

    public void SetCompetition(int arg1)
    {
        if (arg1 == 0)
        {
            this.objBeginTips.gameObject.SetActive(true);
        }
        else if (arg1 == 1)
        {
            this.objSpeedupTips.gameObject.SetActive(true);
        }
        UIInformationList component = this.txtInfo.GetComponent<UIInformationList>();
        TextModelController textModelController = ControllerManager.Instance.GetController<TextModelController>();
        this.txtInfo.GetComponent<Text>().text = textModelController.ChangeTextModel(component.listInformation[2].content);
    }

    public void SetTeamNumber(uint num1, uint num2)
    {
        this.txtNum1.GetComponent<Text>().text = num1.ToString();
        this.txtNum2.GetComponent<Text>().text = num2.ToString();
    }

    public void SetWin(bool isWin, List<RankPKResult> meList, List<RankPKResult> enemyList)
    {
        this.objMask.gameObject.SetActive(true);
        this.objWin.gameObject.SetActive(isWin);
        this.objLose.gameObject.SetActive(!isWin);
        Scheduler.Instance.AddTimer(2f, false, delegate
        {
            this.InitResult(meList, enemyList, isWin);
        });
    }

    private void InitResult(List<RankPKResult> meList, List<RankPKResult> enemyList, bool isWin)
    {
        if (this.objTitleWin == null)
        {
            return;
        }
        this.objTitleWin.gameObject.SetActive(isWin);
        this.objTitleLose.gameObject.SetActive(!isWin);
        this.transResult.gameObject.SetActive(true);
        this.objMask.gameObject.SetActive(false);
        this.objWin.gameObject.SetActive(false);
        this.objLose.gameObject.SetActive(false);
        for (int i = 0; i < this.MAX_TEAM_NUM; i++)
        {
            this._team1ResultList[i].gameObject.SetActive(false);
            this._team2ResultList[i].gameObject.SetActive(false);
            if (i < meList.Count)
            {
                this._team1ResultList[i].Init(meList[i]);
                this._team1ResultList[i].gameObject.SetActive(true);
            }
            if (i < enemyList.Count)
            {
                this._team2ResultList[i].Init(enemyList[i]);
                this._team2ResultList[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetAwards(List<RewardsNumber> rewards)
    {
        this.transAward.gameObject.SetActive(true);
        this.transItem.gameObject.SetActive(false);
        if (this._rewardsObjList != null)
        {
            for (int j = 0; j < this._rewardsObjList.Count; j++)
            {
                UnityEngine.Object.Destroy(this._rewardsObjList[j]);
            }
        }
        this._rewardsObjList.Clear();
        int i;
        for (i = 0; i < rewards.Count; i++)
        {
            GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.transItem.gameObject);
            item.transform.SetParent(this.transAward);
            item.transform.localScale = Vector3.one;
            item.name = i.ToString();
            item.SetActive(true);
            CommonItem commonItem = new CommonItem(item.transform);
            commonItem.SetCommonItem(rewards[i].objectid, rewards[i].number, delegate (uint A_1)
            {
                GlobalRegister.OpenInfoCommon(rewards[i].objectid, item.transform);
            });
            this._rewardsObjList.Add(item);
        }
    }

    private void OnExitButtonClick()
    {
        UIManager.Instance.DeleteUI("UI_Score");
    }

    private void OnLeaveButtonClick()
    {
        ControllerManager.Instance.GetController<PVPCompetitionController>().Req_ExitCopymap_SC();
    }

    private int MAX_TEAM_NUM = 5;

    private Transform ui_root;

    private PVPCompetitionController controller;

    private Transform panelRoot;

    private Transform panelScore;

    private Transform txtTime;

    private Transform txtNum1;

    private Transform txtNum2;

    private Transform txtInfo;

    private Transform objMask;

    private Transform objWin;

    private Transform objLose;

    private Transform objBeginTips;

    private Transform objSpeedupTips;

    private Transform transAward;

    private Transform transItem;

    private Transform transResult;

    private Transform objTitleWin;

    private Transform objTitleLose;

    private Transform btnLeave;

    private Transform btnExit;

    private List<GameObject> _rewardsObjList = new List<GameObject>();

    private List<UI_Score.ResultInfoItem> _team1ResultList = new List<UI_Score.ResultInfoItem>();

    private List<UI_Score.ResultInfoItem> _team2ResultList = new List<UI_Score.ResultInfoItem>();

    private class ResultInfoItem : MonoBehaviour
    {
        private void Awake()
        {
            this.imgRank = base.transform.Find("img_rank").GetComponent<Image>();
            this.txtName = base.transform.Find("txt_name").GetComponent<Text>();
            this.txtDamage = base.transform.Find("txt_damage").GetComponent<Text>();
            this.txtCure = base.transform.Find("txt_cure").GetComponent<Text>();
            this.txtKill = base.transform.Find("txt_kill").GetComponent<Text>();
        }

        public void Init(RankPKResult info)
        {
            this.txtName.text = info.name;
            this.txtDamage.text = info.hurt.ToString();
            this.txtCure.text = info.cure.ToString();
            this.txtKill.text = info.kill + "/" + info.dead;
        }

        private Image imgRank;

        private Text txtName;

        private Text txtDamage;

        private Text txtCure;

        private Text txtKill;
    }
}
