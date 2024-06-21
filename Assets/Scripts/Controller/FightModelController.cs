using System;
using Framework.Managers;
using Models;
using msg;

public class FightModelController : ControllerBase
{
    public UI_FingtModle ui_fightModel
    {
        get
        {
            return UIManager.GetUIObject<UI_FingtModle>();
        }
    }

    public override void Awake()
    {
        this.fightModelNetWork = new FightModelNetWork();
        this.fightModelNetWork.Initialize();
    }

    public void EnterFightModel()
    {
        if (this.ui_fightModel != null && this.ui_fightModel.mRoot != null)
        {
            this.ui_fightModel.mRoot.gameObject.SetActive(!this.ui_fightModel.mRoot.gameObject.activeSelf);
            return;
        }
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_FingtModle>("UI_FightMode", null, UIManager.ParentType.CommonUI, false);
    }

    public void SwitchFightModel(PKMode model)
    {
        this.fightModelNetWork.ReqSwitchPKMode_CS(model);
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "fightmodel_controller";
        }
    }

    private FightModelNetWork fightModelNetWork;
}
