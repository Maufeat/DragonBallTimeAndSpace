using System;
using System.Collections.Generic;

public class SimpleTaskQueue
{
    public void AddTask(Action<Action> task)
    {
        this.AddTask(new SimpleTaskQueue.Task
        {
            Work = task,
            WorkName = "step_" + this.tasklist.Count
        });
    }

    public void AddTask(Action<Action> task, string workName, bool NeedWait = false)
    {
        this.AddTask(new SimpleTaskQueue.Task
        {
            Work = task,
            WorkName = workName,
            NeedWait = NeedWait
        });
    }

    public void AddTask(SimpleTaskQueue.Task TK)
    {
        this.tasklist.Enqueue(TK);
    }

    private void CheckNext()
    {
        if (this.tasklist.Count > 0)
        {
            SimpleTaskQueue.Task task = this.tasklist.Dequeue();
            if (this.OnStep != null)
            {
                this.OnStep(task);
            }
            task.Work(new Action(this.CheckNext));
        }
        else if (this.Finish != null)
        {
            Action finish = this.Finish;
            this.Finish = null;
            finish();
        }
    }

    public void Start()
    {
        this.MaxCount = this.tasklist.Count;
        this.CheckNext();
    }

    public float Progress
    {
        get
        {
            return 1f - (float)this.tasklist.Count / (float)this.MaxCount;
        }
    }

    public void Clear()
    {
        this.tasklist.Clear();
    }

    private Queue<SimpleTaskQueue.Task> tasklist = new Queue<SimpleTaskQueue.Task>();

    public Action Finish;

    public Action<SimpleTaskQueue.Task> OnStep;

    private int MaxCount;

    public class Task
    {
        public Action<Action> Work;

        public string WorkName;

        public bool NeedWait;
    }
}
