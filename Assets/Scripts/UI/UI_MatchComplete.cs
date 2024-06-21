using System;
using System.Collections.Generic;
using Framework.Managers;
using rankpk_msg;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_MatchComplete : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<PVPMatchController>();
        this.InitObj(root);
        this.InitEvent();
        this._totalNum = this.controller.totalNum;
        this._enemyTotalNum = this.controller.totalNum;
        this.isInit = true;
        this.txtSide1.GetComponent<Text>().text = "0/" + this._totalNum;
        this.txtSide2.GetComponent<Text>().text = "0/" + this._enemyTotalNum;
        if (this.controller.prepareInfo != null)
        {
            this.UpdateMatchInfo(this.controller.prepareInfo);
        }
        else
        {
            MSG_RankPkReqPrepare_SC msg_RankPkReqPrepare_SC = new MSG_RankPkReqPrepare_SC();
            MSG_RankPkReqPrepare_SC msg_RankPkReqPrepare_SC2 = msg_RankPkReqPrepare_SC;
            uint num = 0U;
            msg_RankPkReqPrepare_SC.readynum = num;
            msg_RankPkReqPrepare_SC2.enemyreadynum = num;
            MSG_RankPkReqPrepare_SC msg_RankPkReqPrepare_SC3 = msg_RankPkReqPrepare_SC;
            num = (uint)this._totalNum;
            msg_RankPkReqPrepare_SC.totalnum = num;
            msg_RankPkReqPrepare_SC3.enemytotalnum = num;
            msg_RankPkReqPrepare_SC.readystate = 0U;
            this.UpdateMatchInfo(msg_RankPkReqPrepare_SC);
        }
    }

    private void InitObj(Transform root)
    {
        this.ui_root = root;
        this.panelRoot = root.Find("Offset_MatchComplete/Panel_MatchComplete");
        this.btnReady = this.panelRoot.Find("btn_ok");
        this.btnClose = this.panelRoot.Find("Panel_title/btn_hide");
        this.txtTime = this.panelRoot.Find("img_time/txt_time");
        this.txtReady = this.panelRoot.Find("btn_ok/Text");
        this.txtSide1 = this.panelRoot.Find("img_side1/text_ready");
        this.txtSide2 = this.panelRoot.Find("img_side2/text_ready");
        this.objReadyPeople = this.panelRoot.Find("img_side1/Panel_ready/Image");
        this.objEnemyReadyPeople = this.panelRoot.Find("img_side2/Panel_ready/Image");
        this.panelRoot.Find("img_mask").gameObject.SetActive(false);
        this.objReadyPeople.gameObject.SetActive(false);
        this.objEnemyReadyPeople.gameObject.SetActive(false);
    }

    private void InitEvent()
    {
        Button component = this.btnReady.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.OnReadyButtonClick));
        Button component2 = this.btnClose.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.OnCloseButtonClick));
    }

    private void OnReadyButtonClick()
    {
        this.controller.mNetWork.RankPkReqPrepare_CS();
    }

    private void OnCloseButtonClick()
    {
        this.CloseUI();
    }

    private void CloseUI()
    {
        this.controller.CloseMatchCompleteUI();
    }

    public void MatchCompleteUpdate()
    {
        if (!this.isInit)
        {
            return;
        }
        this.txtTime.GetComponent<Text>().text = (int)this.controller.leftTime + string.Empty;
        if (this.controller.leftTime < 0f)
        {
            this.CloseUI();
        }
    }

    public void UpdateMatchInfo(MSG_RankPkReqPrepare_SC info)
    {
        this._readyNum = info.readynum;
        this._totalNum = info.totalnum;
        this._enemyReadyNum = info.enemyreadynum;
        this._enemyTotalNum = info.enemytotalnum;
        int num = 0;
        while ((float)num < this._totalNum)
        {
            if ((float)this._readyObjList.Count < this._totalNum)
            {
                GameObject gameObject = this.InstantiateObj(this.objReadyPeople.gameObject);
                if (gameObject != null)
                {
                    this._readyObjList.Add(gameObject);
                }
            }
            num++;
        }
        int num2 = 0;
        while ((float)num2 < this._enemyTotalNum)
        {
            if ((float)this._enemyReadyObjList.Count < this._enemyTotalNum)
            {
                GameObject gameObject2 = this.InstantiateObj(this.objEnemyReadyPeople.gameObject);
                if (gameObject2 != null)
                {
                    this._enemyReadyObjList.Add(gameObject2);
                }
            }
            num2++;
        }
        for (int i = 0; i < this._readyObjList.Count; i++)
        {
            this._readyObjList[i].SetActive((float)i < this._totalNum);
            this._readyObjList[i].transform.Find("img_wait").gameObject.SetActive((float)i >= this._readyNum);
            this._readyObjList[i].transform.Find("img_ready").gameObject.SetActive((float)i < this._readyNum);
        }
        for (int j = 0; j < this._enemyReadyObjList.Count; j++)
        {
            this._enemyReadyObjList[j].SetActive((float)j < this._enemyTotalNum);
            this._enemyReadyObjList[j].transform.Find("img_wait").gameObject.SetActive((float)j >= this._enemyReadyNum);
            this._enemyReadyObjList[j].transform.Find("img_ready").gameObject.SetActive((float)j < this._enemyReadyNum);
        }
        this.txtSide1.GetComponent<Text>().text = this._readyNum + "/" + this._totalNum;
        this.txtSide2.GetComponent<Text>().text = this._enemyReadyNum + "/" + this._enemyTotalNum;
        this.btnReady.gameObject.SetActive(info.readystate == 0U);
    }

    private GameObject InstantiateObj(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
        gameObject.transform.SetParent(go.transform.parent);
        gameObject.transform.localScale = go.transform.localScale;
        gameObject.transform.localRotation = go.transform.localRotation;
        gameObject.SetActive(true);
        return gameObject;
    }

    private Transform ui_root;

    private PVPMatchController controller;

    public Transform panelRoot;

    public Transform btnReady;

    public Transform btnClose;

    public Transform txtTime;

    public Transform txtReady;

    public Transform txtSide1;

    public Transform txtSide2;

    public Transform objReadyPeople;

    public Transform objEnemyReadyPeople;

    private float _readyNum;

    private float _totalNum;

    private float _enemyReadyNum;

    private float _enemyTotalNum;

    private List<GameObject> _readyObjList = new List<GameObject>();

    private List<GameObject> _enemyReadyObjList = new List<GameObject>();

    private bool isInit;
}
