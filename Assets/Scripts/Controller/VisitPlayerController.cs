using System;
using Framework.Managers;
using massive;
using Models;

public class VisitPlayerController : ControllerBase
{
    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public override void Awake()
    {
        this.visitNetWork = new PlayerVisitNetWork();
        this.visitNetWork.Initialize();
    }

    public void ReqVisitPlayer(EntitiesID Eid)
    {
        this.visitNetWork.Req_HoldonUser_CS(Eid.Id, true);
    }

    public void OnVisitBegin(MSG_Ret_HoldonUser_SC data)
    {
        ControllerManager.Instance.GetController<ProgressUIController>().ShowProgressBar(data.needtime, delegate ()
        {
            this.visitNetWork.Req_HoldonUser_CS(data.thisid, false);
        });
    }

    public void BreakVisitn(ulong holdid)
    {
        ControllerManager.Instance.GetController<ProgressUIController>().BreakProgressBar();
    }

    public void OnVisitComplete(uint retCode)
    {
    }

    public PlayerVisitNetWork visitNetWork;
}
