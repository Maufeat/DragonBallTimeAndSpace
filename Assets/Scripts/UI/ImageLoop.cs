using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoop : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        if (this.mData != null && this.img != null && this.img.enabled)
        {
            this.mData.useTime -= Time.deltaTime;
            if (this.mData.useTime <= 0f)
            {
                int num = int.Parse(this.img.sprite.name);
                num++;
                if (num >= this.mData.imgs.Count)
                {
                    num = 0;
                }
                this.img.sprite = this.mData.imgs[num];
                this.img.overrideSprite = this.mData.imgs[num];
                this.img.sprite.name = this.mData.imgs[num].name;
                this.mData.useTime = (float)this.mData.Time / 1000f;
            }
        }
    }

    public void Initilize(BiaoqingManager.ImageData data, Image _img)
    {
        this.img = _img;
        this.mData = data;
        this.mData.useTime = (float)this.mData.Time / 1000f;
        this.img.sprite = this.mData.imgs[0];
        this.img.overrideSprite = this.mData.imgs[0];
        this.img.sprite.name = this.mData.imgs[0].name;
    }

    private Image img;

    private BiaoqingManager.ImageData mData;
}
