using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeBuffStateData
{
    public TimeBuffStateData()
    {
    }

    public TimeBuffStateData(UserState _state, uint _maxTime, float _leftTime, GameObject _btn)
    {
        this.state = _state;
        this.maxTime = _maxTime;
        this.leftTime = _leftTime;
        this.start = true;
        this.btnicon = _btn;
        this.spcd = this.btnicon.transform.Find("img_cd").GetComponent<Image>();
    }

    public virtual void RefreshData(uint _maxTime, float _leftTime)
    {
        this.maxTime = _maxTime;
        this.leftTime = _leftTime;
        if (this.leftTime > 0f)
        {
            this.btnicon.SetActive(true);
            this.start = true;
        }
        else
        {
            this.btnicon.SetActive(false);
            this.start = false;
        }
    }

    public virtual void UpdateThis()
    {
        if (this.start)
        {
            this.leftTime -= Scheduler.Instance.realDeltaTime;
            if (this.leftTime <= 0f)
            {
                this.leftTime = 0f;
                this.start = false;
                this.btnicon.SetActive(false);
            }
            this.spcd.fillAmount = this.leftTime / this.maxTime;
        }
    }

    public virtual void StopCoutDown()
    {
        this.start = false;
        this.btnicon.SetActive(false);
    }

    public Action<TimeBuffStateData> onComplete;

    public UserState state;

    protected uint maxTime;

    protected float leftTime;

    protected bool start;

    protected GameObject btnicon;

    protected Image spcd;
}
