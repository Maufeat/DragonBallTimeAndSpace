using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownItem : MonoBehaviour
{
    public void InitItem(GameObject rootObj, Text txt, uint duartion)
    {
        this.root = rootObj;
        this.Txt_Time = txt;
        this.fDuartion = duartion;
        if (duartion > 0U)
        {
            this.Txt_Time.text = GlobalRegister.GetTimeInHours(duartion).ToString();
            this._isCounting = true;
        }
        else
        {
            this.Txt_Time.text = string.Empty;
            this._isCounting = false;
        }
    }

    public void StartCountDown(uint duartion)
    {
        if (this.root == null || !this.root.activeSelf)
        {
            return;
        }
        this._isCounting = true;
        this.Txt_Time.text = GlobalRegister.GetTimeInHours(duartion).ToString();
        this.fDuartion = duartion;
    }

    public void StopCountDown()
    {
        if (this.root == null || !this.root.activeSelf)
        {
            return;
        }
        this._isCounting = false;
        this.Txt_Time.text = string.Empty;
        this.fDuartion = 0f;
        this.OnCountDownComplete = null;
    }

    public void Update()
    {
        if (!this._isCounting)
        {
            return;
        }
        if (this.root == null || !this.root.activeSelf)
        {
            this.Txt_Time.text = string.Empty;
            return;
        }
        if (this.fDuartion > 0f)
        {
            this.fDuartion -= Time.deltaTime;
            this.Txt_Time.text = GlobalRegister.GetTimeInHours((uint)this.fDuartion).ToString();
        }
        else
        {
            this._isCounting = false;
            this.fDuartion = 0f;
            this.Txt_Time.text = string.Empty;
            if (this.OnCountDownComplete != null)
            {
                this.OnCountDownComplete(true);
                this.OnCountDownComplete = null;
            }
        }
    }

    public GameObject root;

    public Text Txt_Time;

    public float fDuartion;

    public float fCurTime;

    public Action<bool> OnCountDownComplete;

    private bool _isCounting;
}
