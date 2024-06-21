using System;
using Models;
using UnityEngine;

public class QueueController : ControllerBase
{
    private UI_Queue mQueue
    {
        get
        {
            return UIManager.GetUIObject<UI_Queue>();
        }
    }

    public override void Awake()
    {
        this.Init();
    }

    public void CloseQueue()
    {
        this.isLoop = false;
    }

    public override void OnUpdate()
    {
        if (this.isLoop)
        {
            if (this.tmpSecond < 30f)
            {
                this.tmpSecond += Time.deltaTime;
            }
            else
            {
                this.ReqQueueData();
                this.isLoop = false;
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Init()
    {
        this.queueNetWorkder = new QueueNetWorker();
        this.queueNetWorkder.Initialize();
    }

    public void ReqQueueData()
    {
        this.queueNetWorkder.ReqQueue();
    }

    public void RetQueueData(int number, int second)
    {
        if (number > 0)
        {
            UIManager.Instance.ShowUI<UI_Queue>("UI_Queue", delegate ()
            {
                this.mQueue.SetupUI(number, second);
            }, UIManager.ParentType.Tips, false);
            this.tmpSecond = 0f;
            this.isLoop = true;
        }
    }

    public override string ControllerName
    {
        get
        {
            return "queue_controller";
        }
    }

    private const int REFRESH_SECOND = 30;

    private QueueNetWorker queueNetWorkder;

    private float tmpSecond;

    private bool isLoop;
}
