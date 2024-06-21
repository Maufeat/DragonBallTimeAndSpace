using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadManager : LSingleton<ThreadManager>
{
    public readonly int maxThreads = 8;
    private List<Action> _actions = new List<Action>();
    private List<ThreadManager.DelayedQueueItem> _delayed = new List<ThreadManager.DelayedQueueItem>();
    private List<ThreadManager.DelayedQueueItem> _currentDelayed = new List<ThreadManager.DelayedQueueItem>();
    private List<Action> _currentActions = new List<Action>();
    private int numThreads;

    public void Init()
    {
    }

    public void RunOnMainThread(Action action)
    {
        this.RunOnMainThread(action, 0.0f);
    }

    public void RunOnMainThread(Action action, float time)
    {
        if ((double)time != 0.0)
        {
            lock (this._delayed)
                this._delayed.Add(new ThreadManager.DelayedQueueItem()
                {
                    time = Time.realtimeSinceStartup + time,
                    action = action
                });
        }
        else
        {
            lock (this._actions)
                this._actions.Add(action);
        }
    }

    public Thread RunAsync(Action a)
    {
        while (this.numThreads >= this.maxThreads)
            Thread.Sleep(1);
        Interlocked.Increment(ref this.numThreads);
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.RunAction), (object)a);
        return (Thread)null;
    }

    private void RunAction(object action)
    {
        FFDebug.Log((object)this, (object)FFLogType.Default, (object)("RunAction on thread " + (object)Thread.CurrentThread.GetHashCode()));
        if (action == null)
            return;
        try
        {
            ((Action)action)();
        }
        catch (Exception ex)
        {
            FFDebug.LogError((object)this, (object)("RunAction Exception e : " + ex.ToString()));
        }
        finally
        {
            Interlocked.Decrement(ref this.numThreads);
        }
    }

    public void Update()
    {
        lock (this._actions)
        {
            this._currentActions.Clear();
            this._currentActions.AddRange((IEnumerable<Action>)this._actions);
            this._actions.Clear();
        }
        for (int index = 0; index < this._currentActions.Count; ++index)
            this._currentActions[index]();
        lock (this._delayed)
        {
            this._currentDelayed.Clear();
            for (int index = 0; index < this._delayed.Count; ++index)
            {
                ThreadManager.DelayedQueueItem delayedQueueItem = this._delayed[index];
                if ((double)delayedQueueItem.time <= (double)Time.realtimeSinceStartup)
                    this._currentDelayed.Add(delayedQueueItem);
            }
            for (int index = 0; index < this._currentDelayed.Count; ++index)
                this._delayed.Remove(this._currentDelayed[index]);
        }
        for (int index = 0; index < this._currentDelayed.Count; ++index)
            this._currentDelayed[index].action();
    }

    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
}
