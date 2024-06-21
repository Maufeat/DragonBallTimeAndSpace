using System;
using System.Text;
using msg;
using Net;
using UI.Login;
using UnityEngine;

public class GameTime : SingletonForMono<GameTime>
{
    public GameTime() : base()
    {
        DateTime dateTime = new DateTime(1970, 1, 1);
        this.startTime = dateTime.ToLocalTime();
        this.heartBeatState = HeartBeatState.HeartBeatException;
        this.HeartBeatInterval = 60f;
        this.pinginterval = 15f;
        this.temppingtime = 999f;

        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.SelfUpdate));
    }

    ~GameTime()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.SelfUpdate));
    }

    public ulong GetTimeInterval()
    {
        ulong nowMsecond = this.GetNowMsecond();
        ulong result = nowMsecond - GameTime.lastTime;
        GameTime.lastTime = nowMsecond;
        return result;
    }

    public ulong GetNowMsecond()
    {
        return (ulong)(DateTime.Now - this.startTime).TotalMilliseconds;
    }

    private void UpdateNowMsecond()
    {
        if (this.m_qwClientMsec == 0UL)
        {
            this.m_qwClientMsec = this.GetNowMsecond();
        }
    }

    public uint GetIntervalMsecond()
    {
        ulong nowMsecond = this.GetNowMsecond();
        if (nowMsecond >= this.m_qwClientMsec)
        {
            ulong num = nowMsecond - this.m_qwClientMsec;
            return (uint)num;
        }
        return 0U;
    }

    private uint GetServerTime()
    {
        uint nowSecond = this.GetNowSecond();
        if (nowSecond >= this.m_dwClientTime)
        {
            return nowSecond - this.m_dwClientTime + this.m_dwServerTime;
        }
        FFDebug.Log(this, FFLogType.Default, this.m_dwServerTime);
        return this.m_dwServerTime;
    }

    private uint ConvertDateTimeInt(DateTime time)
    {
        return (uint)(time - this.startTime).TotalSeconds;
    }

    public uint GetNowSecond()
    {
        return this.ConvertDateTimeInt(DateTime.Now);
    }

    public void Init()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_GameTime_SC>(2267, new ProtoMsgCallback<MSG_Ret_GameTime_SC>(this.OnServerTimeInit));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Req_UserGameTime_SC>(2268, new ProtoMsgCallback<MSG_Req_UserGameTime_SC>(this.OnServerTimeReq));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Req_Ping_CS>(2296, new ProtoMsgCallback<MSG_Req_Ping_CS>(this.OnRetPing));
    }

    public void OnServerTimeInit(MSG_Ret_GameTime_SC data)
    {
        uint dwServerTime = (uint)data.gametime;
        this.m_dwServerTime = dwServerTime;
        this.m_dwClientTime = this.GetNowSecond();
    }

    public void OnServerTimeReq(MSG_Req_UserGameTime_SC data)
    {
        this.UpdateNowMsecond();
        this.SendUserGameTime();
        this.missHeartBeatTimes = 0;
        this.tempTime = 0f;
        this.heartBeatState = HeartBeatState.HeartBeatNormal;
    }

    private void SendUserGameTime()
    {
        uint serverTime = this.GetServerTime();
        MSG_Ret_UserGameTime_CS msg_Ret_UserGameTime_CS = new MSG_Ret_UserGameTime_CS();
        msg_Ret_UserGameTime_CS.gametime = (ulong)serverTime;
        LSingleton<NetWorkModule>.Instance.Send<MSG_Ret_UserGameTime_CS>(2269, msg_Ret_UserGameTime_CS, false);
    }

    public void SetServerTime(ulong servertime)
    {
        this.ServertimeOffset = (long)(servertime - this.GetNowMsecond());
    }

    public ulong GetCurrServerTime()
    {
        long nowMsecond = (long)this.GetNowMsecond();
        return (ulong)(nowMsecond + this.ServertimeOffset);
    }

    public uint GetCurrServerTimeBySecond()
    {
        long nowMsecond = (long)this.GetNowMsecond();
        ulong num = (ulong)(nowMsecond + this.ServertimeOffset);
        return (uint)(num / 1000UL);
    }

    public ulong GetCurrServerUlongTimeBySecond()
    {
        long nowMsecond = (long)this.GetNowMsecond();
        ulong num = (ulong)(nowMsecond + this.ServertimeOffset);
        return num / 1000UL;
    }

    public DateTime GetCurrServerDateTime()
    {
        long nowMsecond = (long)this.GetNowMsecond();
        ulong num = (ulong)(nowMsecond + this.ServertimeOffset);
        return this.startTime.AddMilliseconds(num);
    }

    public string GetTimeBySecond(ulong time, string laststr)
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (time > 86400UL)
        {
            stringBuilder.Append((time / 86400UL).ToString());
            stringBuilder.Append("天");
            time %= 86400UL;
            stringBuilder.Append((time / 3600UL).ToString());
            stringBuilder.Append("小时");
        }
        else if (time < 86400UL && time > 3600UL)
        {
            stringBuilder.Append((time / 3600UL).ToString());
            stringBuilder.Append("小时");
            time %= 3600UL;
            stringBuilder.Append((time / 60UL).ToString());
            stringBuilder.Append("分钟");
        }
        else
        {
            stringBuilder.Append((time / 60UL).ToString());
            stringBuilder.Append("分钟");
        }
        stringBuilder.Append(laststr);
        return stringBuilder.ToString();
    }

    public string GetTimeBySecond(ulong time)
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (time > 86400UL)
        {
            stringBuilder.Append((time / 86400UL).ToString());
            stringBuilder.Append("天");
            time %= 86400UL;
            stringBuilder.Append((time / 3600UL).ToString());
            stringBuilder.Append("小时");
        }
        else if (time < 86400UL && time > 3600UL)
        {
            stringBuilder.Append((time / 3600UL).ToString());
            stringBuilder.Append("小时");
            time %= 3600UL;
            stringBuilder.Append((time / 60UL).ToString());
            stringBuilder.Append("分钟");
        }
        else
        {
            stringBuilder.Append((time / 60UL).ToString());
            stringBuilder.Append("分钟");
        }
        stringBuilder.Append("后过期");
        return stringBuilder.ToString();
    }

    public uint GetDayBySecond(ulong time)
    {
        return (uint)(time / 86400UL);
    }

    public uint GetHorBySecond(ulong time)
    {
        time %= 86400UL;
        return (uint)(time / 3600UL);
    }

    public uint GetMinBySecond(ulong time)
    {
        time %= 3600UL;
        return (uint)Mathf.Clamp(time / 60UL, 1f, 59f);
    }

    public DateTime GetServerDateTimeByTimeStamp(ulong timestamp)
    {
        return this.startTime.AddMilliseconds(timestamp);
    }

    public string GetTimeText(ulong time)
    {
        return SingletonForMono<GameTime>.Instance.GetServerDateTimeByTimeStamp(time * 1000UL).ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string GetTimeText1(int second)
    {
        string text = string.Empty;
        int num = second / 60;
        if (num < 10)
        {
            text += "0";
        }
        text = text + num + ":";
        int num2 = second % 60;
        if (num2 < 10)
        {
            text += "0";
        }
        return text + num2;
    }

    public void ReqPing()
    {
        LSingleton<NetWorkModule>.Instance.Send<MSG_Req_Ping_CS>(CommandID.MSG_Req_Ping_CS, new MSG_Req_Ping_CS(), false);
        this.lastreqpingtime = this.GetCurrServerTime();
    }

    public void OnRetPing(MSG_Req_Ping_CS data)
    {
        ulong currServerTime = this.GetCurrServerTime();
        this.Ping = (uint)Mathf.Clamp(currServerTime - this.lastreqpingtime, 0f, 999f);
    }

    private void SelfUpdate()
    {
        if (LSingleton<NetWorkModule>.Instance.MainSocket == null)
        {
            return;
        }
        if (LSingleton<NetWorkModule>.Instance.MainSocket.type == USocketType.Fir)
        {
            return;
        }
        if (this.heartBeatState == HeartBeatState.HeartBeatException)
        {
            return;
        }
        if (!LSingleton<NetWorkModule>.Instance.MainSocket.GetSocketState())
        {
            return;
        }
        if (LoginControllerBase.Instance == null || LoginControllerBase.Instance.loginNetWork.Kickouted)
        {
            return;
        }
        if (this.CheckPing)
        {
            this.temppingtime += Scheduler.Instance.realDeltaTime;
            if (this.temppingtime > this.pinginterval)
            {
                this.temppingtime = 0f;
                this.ReqPing();
            }
        }
        this.tempTime += Scheduler.Instance.realDeltaTime;
        if (this.tempTime > this.HeartBeatInterval)
        {
            this.tempTime = 0f;
            this.missHeartBeatTimes++;
            if (this.missHeartBeatTimes >= 8)
            {
                FFDebug.LogWarning(this, "HeartBeatException Reconnect!!!");
                this.heartBeatState = HeartBeatState.HeartBeatException;
                LSingleton<NetWorkModule>.Instance.Reconnect();
            }
        }
    }

    public float v0 = 4.8f;

    public float a = 9.8f;

    private ulong m_qwClientMsec;

    private DateTime startTime;

    private static ulong lastTime;

    private uint m_dwServerTime;

    private uint m_dwClientTime;

    private ulong m_dwServerStartTime;

    private HeartBeatState heartBeatState;

    public readonly float HeartBeatInterval;

    private int missHeartBeatTimes;

    private float tempTime;

    private long ServertimeOffset;

    public uint Ping;

    public ulong lastreqpingtime;

    private float pinginterval;

    private float temppingtime;

    public bool CheckPing;
}
