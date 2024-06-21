using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;

public class TransporterController : ControllerBase
{
    private TransporterWork network
    {
        get
        {
            if (this.network_ == null)
            {
                this.network_ = new TransporterWork();
            }
            return this.network_;
        }
    }

    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.AddTeleportItem));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.StartAddTeleportItem));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.EndAddTeleportItem));
    }

    public override void OnUpdate()
    {
    }

    private void StartAddTeleportItem(List<VarType> paramsData)
    {
        this.showItemIds.Clear();
    }

    private void EndAddTeleportItem(List<VarType> paramsData)
    {
        UI_Transporter uiobject = UIManager.GetUIObject<UI_Transporter>();
        if (uiobject != null)
        {
            uiobject.EndAddItem(this.showItemIds);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_Transporter>("UI_Transporter", delegate ()
            {
                UIManager.GetUIObject<UI_Transporter>().EndAddItem(this.showItemIds);
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void ReqTeleport(uint id)
    {
        if (ControllerManager.Instance.GetController<AbattoirMatchController>().getState == AbattoirMatchState.WaitingStart)
        {
            TipsWindow.ShowWindow(5004U);
            return;
        }
        this.network.ReqMSG_Req_TELE_PORT_CS((ulong)id);
    }

    private void AddTeleportItem(List<VarType> paramsData)
    {
        UI_Transporter uiobject = UIManager.GetUIObject<UI_Transporter>();
        if (paramsData.Count > 0)
        {
            uint item = 0U;
            if (!string.IsNullOrEmpty(paramsData[0]) && uint.TryParse(paramsData[0], out item))
            {
                this.showItemIds.Add(item);
            }
        }
    }

    public override string ControllerName
    {
        get
        {
            return "Transporter_Controller";
        }
    }

    private List<uint> showItemIds = new List<uint>();

    private TransporterWork network_;
}
