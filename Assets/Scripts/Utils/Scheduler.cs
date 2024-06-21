using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Scheduler
{
    private Scheduler()
    {
        this._curFrame = 0U;
        this._curAllotID = 0U;
        this.mTimeStarted = true;
        this.mTimeDelta = 0f;
        this.mTimeStart = 0f;
        this._frameDelegates = new List<Scheduler.FrameScheduler>();
        this._timeSchedulers = new List<Scheduler.TimeScheduler>();
        this._updateScheduler = new List<Scheduler.OnScheduler>();
        this._fixedUpdateScheduler = new List<Scheduler.OnScheduler>();
        this._guiScheduler = new List<Scheduler.OnScheduler>();
        this._queue = new Queue<Action>();
    }

    public float unityDeltaTime
    {
        get
        {
            return Time.deltaTime;
        }
    }

    public float realDeltaTime
    {
        get
        {
            return this.mrealTimeDelta;
        }
    }

    public uint FrameSinceStartup
    {
        get
        {
            return this._curFrame;
        }
    }

    ~Scheduler()
    {
        this._frameDelegates.Clear();
        this._frameDelegates = null;
        this._timeSchedulers.Clear();
        this._timeSchedulers = null;
        this._updateScheduler.Clear();
        this._updateScheduler = null;
        this._fixedUpdateScheduler.Clear();
        this._fixedUpdateScheduler = null;
        this._guiScheduler.Clear();
        this._guiScheduler = null;
        this._curAllotID = 0U;
        this.mTimeStarted = true;
        this.mTimeDelta = 0f;
        this.mTimeStart = 0f;
    }

    public void Update()
    {
        this._curFrame += 1U;
        this.mrealTimeDelta = this.UpdateRealTimeDelta();
        this.UpdateFrameScheduler();
        this.UpdateTimeScheduler();
        this.UpdateUpdator();
        this.UpdateQueue();
    }

    public void GuiUpdate()
    {
        if (this._guiScheduler.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < this._guiScheduler.Count; i++)
        {
            try
            {
                this._guiScheduler[i]();
            }
            catch (Exception arg)
            {
                FFDebug.LogWarning(this, "GuiUpdator error: " + arg);
            }
        }
    }

    public void FixedUpdate()
    {
        this.UpdateFixedUpdator();
    }

    private void UpdateUpdator()
    {
        if (this._updateScheduler.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < this._updateScheduler.Count; i++)
        {
            try
            {
                this._updateScheduler[i]();
            }
            catch (Exception arg)
            {
                FFDebug.LogWarning(this, "UpdateUpdator error: " + arg);
            }
        }
    }

    private void UpdateFixedUpdator()
    {
        if (this._fixedUpdateScheduler.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < this._fixedUpdateScheduler.Count; i++)
        {
            try
            {
                this._fixedUpdateScheduler[i]();
            }
            catch (Exception arg)
            {
                FFDebug.LogWarning(this, "UpdateFixedUpdator error: " + arg);
            }
        }
    }

    private void UpdateFrameScheduler()
    {
        for (int i = 0; i < this._frameDelegates.Count; i++)
        {
            Scheduler.FrameScheduler frameScheduler = this._frameDelegates[i];
            if (frameScheduler.RealFrame <= this._curFrame)
            {
                try
                {
                    frameScheduler.Callback();
                }
                catch (Exception arg)
                {
                    FFDebug.LogWarning(this, "UpdateFrameScheduler error: " + arg);
                }
                if (!frameScheduler.IsLoop)
                {
                    this._frameDelegates.RemoveAt(i);
                    continue;
                }
                frameScheduler.RealFrame += frameScheduler.Frame;
            }
        }
    }

    private void UpdateTimeScheduler()
    {
        for (int i = 0; i < this._timeSchedulers.Count; i++)
        {
            Scheduler.TimeScheduler timeScheduler = this._timeSchedulers[i];
            if (timeScheduler.RealTime <= Time.realtimeSinceStartup)
            {
                try
                {
                    if (timeScheduler != null && timeScheduler.Callback != null)
                    {
                        timeScheduler.Callback();
                    }
                }
                catch (Exception arg)
                {
                    FFDebug.LogWarning(this, "UpdateTimeScheduler error: " + arg);
                }
                if (timeScheduler.IsLoop)
                {
                    timeScheduler.RealTime += timeScheduler.Time;
                }
                else if (i < this._timeSchedulers.Count)
                {
                    this._timeSchedulers.RemoveAt(i);
                    continue;
                }
            }
        }
    }

    private float UpdateRealTimeDelta()
    {
        this.mRt = Time.realtimeSinceStartup;
        if (this.mTimeStarted)
        {
            float b = this.mRt - this.mTimeStart;
            this.mActual += Mathf.Max(0f, b);
            this.mTimeDelta = 0.001f * Mathf.Round(this.mActual * 1000f);
            this.mActual -= this.mTimeDelta;
            if (this.mTimeDelta > 1f)
            {
                this.mTimeDelta = 1f;
            }
            this.mTimeStart = this.mRt;
        }
        else
        {
            this.mTimeStarted = true;
            this.mTimeStart = this.mRt;
            this.mTimeDelta = 0f;
        }
        return this.mTimeDelta;
    }

    private void UpdateQueue()
    {
        while (this._queue.Count > 0)
        {
            this._queue.Dequeue()();
        }
    }

    public void AddFrame(uint frame, bool loop, Scheduler.OnScheduler callback)
    {
        this._curAllotID += 1U;
        Scheduler.FrameScheduler item = new Scheduler.FrameScheduler
        {
            ID = this._curAllotID,
            Frame = frame,
            RealFrame = frame + this._curFrame,
            IsLoop = loop,
            Callback = callback
        };
        this._frameDelegates.Add(item);
    }

    public void RemoveFrame(Scheduler.OnScheduler callback)
    {
        for (int i = 0; i < this._frameDelegates.Count; i++)
        {
            Scheduler.FrameScheduler frameScheduler = this._frameDelegates[i];
            if (frameScheduler.Callback == callback)
            {
                this._frameDelegates.RemoveAt(i);
                break;
            }
        }
    }

    public void AddTimer(float time, bool loop, Scheduler.OnScheduler callback)
    {
        this._curAllotID += 1U;
        Scheduler.TimeScheduler item = new Scheduler.TimeScheduler
        {
            ID = this._curAllotID,
            Time = time,
            RealTime = time + Time.realtimeSinceStartup,
            IsLoop = loop,
            Callback = callback
        };
        this._timeSchedulers.Add(item);
    }

    public void RemoveTimer(Scheduler.OnScheduler callback)
    {
        for (int i = 0; i < this._timeSchedulers.Count; i++)
        {
            Scheduler.TimeScheduler timeScheduler = this._timeSchedulers[i];
            if (timeScheduler.Callback == callback)
            {
                this._timeSchedulers.RemoveAt(i);
                break;
            }
        }
    }

    public void AddUpdator(Scheduler.OnScheduler callback)
    {
        if (!this._updateScheduler.Contains(callback))
        {
            this._updateScheduler.Add(callback);
        }
    }

    public void AddQueue(Action callback)
    {
        if (!this._queue.Contains(callback))
        {
            this._queue.Enqueue(callback);
        }
    }

    public void RemoveUpdator(Scheduler.OnScheduler callback)
    {
        this._updateScheduler.Remove(callback);
    }

    public void AddFixedUpdator(Scheduler.OnScheduler callback)
    {
        this._fixedUpdateScheduler.Add(callback);
    }

    public void RemoveFixedUpdator(Scheduler.OnScheduler callback)
    {
        this._fixedUpdateScheduler.Remove(callback);
    }

    public void AddGuiUpdator(Scheduler.OnScheduler callback)
    {
        this._guiScheduler.Add(callback);
    }

    public void RemoveGuiUpdator(Scheduler.OnScheduler callback)
    {
        this._guiScheduler.Remove(callback);
    }

    public static readonly Scheduler Instance = new Scheduler();

    private List<Scheduler.FrameScheduler> _frameDelegates;

    private List<Scheduler.TimeScheduler> _timeSchedulers;

    private List<Scheduler.OnScheduler> _updateScheduler;

    private List<Scheduler.OnScheduler> _fixedUpdateScheduler;

    private List<Scheduler.OnScheduler> _guiScheduler;

    private Queue<Action> _queue;

    private uint _curFrame;

    private uint _curAllotID;

    private float mRt;

    private float mTimeStart;

    private float mTimeDelta;

    private float mActual;

    private bool mTimeStarted;

    private float mrealTimeDelta;

    private class FrameScheduler
    {
        public uint ID;

        public uint Frame;

        public uint RealFrame;

        public bool IsLoop;

        public Scheduler.OnScheduler Callback;
    }

    private class TimeScheduler
    {
        public uint ID;

        public float RealTime;

        public float Time;

        public bool IsLoop;

        public Scheduler.OnScheduler Callback;
    }

    public delegate void OnScheduler();
}
