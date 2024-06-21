using System;
using Framework.Managers;
using msg;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_FingtModle : UIPanelBase
{
    public void InitGameObject()
    {
        this.btn_peace = this.mRoot.Find("Offset_FightMode/PanelMode/btn_peace").gameObject;
        this.btn_personal = this.mRoot.Find("Offset_FightMode/PanelMode/btn_personal").gameObject;
        this.btn_guild = this.mRoot.Find("Offset_FightMode/PanelMode/btn_guild").gameObject;
        this.btn_team = this.mRoot.Find("Offset_FightMode/PanelMode/btn_team").gameObject;
    }

    public void InitEvent()
    {
        UIEventListener.Get(this.btn_peace).onClick = delegate (PointerEventData evtData)
        {
            this.switchFightModel(PKMode.PKMode_Peace);
            this.mRoot.gameObject.SetActive(false);
        };
        UIEventListener.Get(this.btn_personal).onClick = delegate (PointerEventData evtData)
        {
            this.switchFightModel(PKMode.PKMode_Personal);
            this.mRoot.gameObject.SetActive(false);
        };
        UIEventListener.Get(this.btn_guild).onClick = delegate (PointerEventData evtData)
        {
            this.switchFightModel(PKMode.PKMode_Guild);
            this.mRoot.gameObject.SetActive(false);
        };
        UIEventListener.Get(this.btn_team).onClick = delegate (PointerEventData evtData)
        {
            this.switchFightModel(PKMode.PKMode_Team);
            this.mRoot.gameObject.SetActive(false);
        };
    }

    private void switchFightModel(PKMode model)
    {
        ControllerManager.Instance.GetController<FightModelController>().SwitchFightModel(model);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.InitGameObject();
        this.InitEvent();
    }

    public Transform mRoot;

    private GameObject btn_peace;

    private GameObject btn_personal;

    private GameObject btn_guild;

    private GameObject btn_team;
}
