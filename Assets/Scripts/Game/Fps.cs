using System;
using Framework.Managers;
using UnityEngine;

public class Fps : MonoBehaviour
{
    public bool StartNow
    {
        set
        {
            this.startnow = value;
            this.lowfps = false;
        }
    }

    private void Start()
    {
        Fps.Instance = this;
    }

    public void HidePlayerByPlayer(bool hide)
    {
        this.hidebyplayer = hide;
        if (this.hidebyplayer)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager != null)
            {
                manager.SetVisiblePlayerLimit(0);
            }
        }
        else if (!this.lowfps)
        {
            EntitiesManager manager2 = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager2 != null)
            {
                manager2.SetVisiblePlayerLimit(manager2.nMaxPlayerLimitCount);
            }
        }
    }

    private void Update()
    {
        Fps.fpsDuration += Time.deltaTime;
        Fps.fps++;
        if (Fps.fpsDuration >= 1f)
        {
            Fps.FPS = Fps.fps;
            Fps.fps = 0;
            Fps.fpsDuration = 0f;
            if (this.fpstate == Fps_State.Fps_State_CalFps && Fps.FPS < 30)
            {
                this.fpscounter++;
            }
        }
        if (this.fpstate == Fps_State.Fps_State_CalFps)
        {
            Fps.caltime += Time.deltaTime;
            if (Fps.caltime >= (float)Fps.FPS_CAL_TIME)
            {
                this.fpstate = Fps_State.Fps_State_WaitNext;
                Fps.caltime = 0f;
                if (this.fpscounter >= 3)
                {
                    this.onFpsLow();
                }
                else
                {
                    this.onFpsHigh();
                }
                this.fpscounter = 0;
            }
        }
        else if (this.fpstate == Fps_State.Fps_State_WaitNext)
        {
            Fps.waitime += Time.deltaTime;
            if (Fps.waitime >= (float)Fps.FPS_WAIT_TIME)
            {
                this.fpstate = Fps_State.Fps_State_CalFps;
                Fps.waitime = 0f;
            }
        }
    }

    private void onFpsHigh()
    {
        if (!this.startnow)
        {
            return;
        }
        if (!this.lowfps)
        {
            return;
        }
        this.lowfps = false;
        if (!this.hidebyplayer)
        {
            ChatControl controller = ControllerManager.Instance.GetController<ChatControl>();
            if (controller != null)
            {
                controller.AddChatItemBySystem(CommonTools.GetTextById(4066UL));
            }
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager != null)
            {
                manager.SetVisiblePlayerLimit(manager.nMaxPlayerLimitCount);
            }
        }
    }

    private void onFpsLow()
    {
        if (!this.startnow)
        {
            return;
        }
        if (this.lowfps)
        {
            return;
        }
        this.lowfps = true;
        ChatControl controller = ControllerManager.Instance.GetController<ChatControl>();
        if (controller != null)
        {
            controller.AddChatItemBySystem(CommonTools.GetTextById(4065UL));
        }
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            manager.SetVisiblePlayerLimit(0);
        }
    }

    public static int FPS;

    public static float fpsDuration;

    public static int fps;

    public static float caltime;

    public static int FPS_CAL_TIME = 5;

    public int fpscounter;

    public static float waitime;

    public static int FPS_WAIT_TIME = 10;

    private Fps_State fpstate;

    public bool lowfps;

    public bool startnow;

    public static Fps Instance;

    private bool hidebyplayer;
}
