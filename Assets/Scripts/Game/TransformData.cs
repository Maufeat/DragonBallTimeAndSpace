using System;
using npc;

public class TransformData
{
    public TransformData(HoldNpcData data, Action<TransformData> callback)
    {
        this.Data = data;
        this.onComplete = callback;
        this.CurrentTime = data.resttime;
        this.start = true;
    }

    public void RefreshData(HoldNpcData data)
    {
        this.Data = data;
        this.CurrentTime = data.resttime;
        if (this.CurrentTime > 0f)
        {
            this.start = true;
        }
    }

    public void Update()
    {
        if (this.start)
        {
            this.CurrentTime -= Scheduler.Instance.realDeltaTime;
            if (this.CurrentTime <= 0f)
            {
                this.CurrentTime = 0f;
                this.start = false;
                if (this.onComplete != null)
                {
                    this.onComplete(this);
                }
                this.onComplete = null;
            }
        }
    }

    public void StopCoutDown()
    {
        this.start = false;
        this.CurrentTime = 0f;
    }

    public void Dispose()
    {
        this.start = false;
        this.CurrentTime = 0f;
    }

    private Action<TransformData> onComplete;

    public HoldNpcData Data;

    private bool start;

    public float CurrentTime;
}
