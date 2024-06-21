using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class NotifyController : ControllerBase
{
    public override void Awake()
    {
        this.network = new NotifyNetwork();
        this.network.Initialize();
        this.RegNotifyAction(0U, new Action<string>(this.OnGetSetCamDirDist));
    }

    public override string ControllerName
    {
        get
        {
            return "notify";
        }
    }

    public void OnNotify(uint type, string content)
    {
        if (this.cbs.ContainsKey(type) && this.cbs[type] != null)
        {
            this.cbs[type](content);
        }
    }

    public void Dispose()
    {
        this.cbs.Clear();
    }

    public void RegNotifyAction(uint type, Action<string> action)
    {
        this.cbs[type] = action;
    }

    public void UnRegNotifyAction(uint type)
    {
        if (this.cbs.ContainsKey(type))
        {
            this.cbs.Remove(type);
        }
    }

    private void OnGetSetCamDirDist(string data)
    {
        Scheduler.Instance.AddTimer(0.1f, false, delegate
        {
            if (this.cc != null)
            {
                CameraFollowTarget4 cameraFollowTarget = this.cc.CurrState as CameraFollowTarget4;
                if (cameraFollowTarget != null)
                {
                    string[] array = data.Split(new char[]
                    {
                        ','
                    });
                    if (array.Length >= 3)
                    {
                        cameraFollowTarget.SetCameraDirDistAngleV(uint.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
                    }
                    else
                    {
                        FFDebug.LogError(this, "set dir data error");
                    }
                }
                else
                {
                    FFDebug.LogError(this, "cmft 4 is null");
                }
            }
            else
            {
                FFDebug.LogError(this, "cc is null");
            }
        });
    }

    private CameraController cc
    {
        get
        {
            if (this.cc_ == null)
            {
                this.cc_ = UnityEngine.Object.FindObjectOfType<CameraController>();
            }
            return this.cc_;
        }
    }

    public NotifyNetwork network;

    private Dictionary<uint, Action<string>> cbs = new Dictionary<uint, Action<string>>();

    private CameraController cc_;
}
