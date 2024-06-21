using System;
using Framework.Managers;
using massive;
using Net;

public class SystemSettingNetWork : NetWorkBase
{
    private SystemSettingController controller
    {
        get
        {
            if (this._controller == null)
            {
                this._controller = ControllerManager.Instance.GetController<SystemSettingController>();
            }
            return this._controller;
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
    }

    public void SendOutStuck()
    {
        MSG_Req_DivorceStuck_CS t = new MSG_Req_DivorceStuck_CS();
        base.SendMsg<MSG_Req_DivorceStuck_CS>(CommandID.MSG_Req_DivorceStuck_CS, t, false);
    }

    private SystemSettingController _controller;
}
