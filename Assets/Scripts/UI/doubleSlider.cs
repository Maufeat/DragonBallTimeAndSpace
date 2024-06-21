using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doubleSlider : MonoBehaviour
{
    private void Start()
    {
    }

    public void LevelUp(uint level, float value)
    {
        for (uint num = 0U; num < level; num += 1U)
        {
            this.callBackQueue.Enqueue(delegate
            {
                this.ChangeValue(1f);
            });
        }
        this.callBackQueue.Enqueue(delegate
        {
            if (this.mlevelMax)
            {
                this.SetForwardColor(ConstClient.MaxLevelColor);
            }
            this.ChangeValue(value);
        });
    }

    public void SetIFMaxLevel(bool levelMax)
    {
        this.mlevelMax = levelMax;
    }

    public void SetForwardColor(Color color)
    {
        this.forward.transform.Find("FillArea/Fill").GetComponent<Image>().color = color;
    }

    public void SetAllValue(float value)
    {
        this.backGround.value = value;
        this.forward.value = value;
    }

    public void ChangeValue(float value)
    {
        this.backGround.value = value;
        this.sliderValue = this.forward.value;
        this.curTime = 0f;
    }

    private void Update()
    {
        if (this.curTime <= 1f)
        {
            this.curTime += Time.deltaTime * this.speed;
            this.forward.value = Mathf.Lerp(this.sliderValue, this.backGround.value, this.curTime);
        }
        else
        {
            if (!this.mlevelMax)
            {
                if (this.forward.value >= 1f)
                {
                    this.SetAllValue(0f);
                }
            }
            else if (this.callBackQueue.Count > 0 && this.forward.value >= 1f)
            {
                this.SetAllValue(0f);
            }
            if (this.callBackQueue.Count > 0)
            {
                this.callBackQueue.Dequeue()();
            }
        }
        if (this.backGround.value < this.forward.value)
        {
            this.forward.value = this.backGround.value;
        }
    }

    public Slider forward;

    public Slider backGround;

    public float speed = 1f;

    private Queue<Action> callBackQueue = new Queue<Action>();

    private bool mlevelMax;

    private float curTime = 1f;

    private float sliderValue;
}
