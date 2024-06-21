using System;
using UnityEngine;
using UnityEngine.UI;

public class TweenNumber : MonoBehaviour
{
	private void Start()
	{
		this.text = base.gameObject.GetComponent<Text>();
		if (this.text == null)
		{
			this.text = base.gameObject.AddComponent<Text>();
		}
		this.tweenscale = base.gameObject.GetComponent<TweenScale>();
		if (this.tweenscale == null)
		{
			this.tweenscale = base.gameObject.AddComponent<TweenScale>();
		}
		this.tweenscale.enabled = false;
	}

	public void SetChangeToNumber(int _fromNumber, int _tonumber, float _changetime, string _prefix)
	{
		this.fromNumber = (float)_fromNumber;
		this.toNumber = (float)_tonumber;
		this.changeTime = _changetime;
		this.prefix = _prefix;
		this.curNumber = this.fromNumber;
		this.curTime = 0f;
		this.ChangeNumber = (this.toNumber - this.fromNumber) / this.changeTime * Time.deltaTime;
		this.bstart = true;
		this.text.text = this.prefix + ((int)this.curNumber).ToString();
	}

	private void Update()
	{
		if (this.bstart)
		{
			this.curTime += Time.deltaTime;
			this.curNumber += this.ChangeNumber;
			if (this.curTime >= this.changeTime)
			{
				this.curNumber = this.toNumber;
				this.bstart = false;
				this.tweenscale.Reset();
				this.tweenscale.enabled = true;
			}
			this.text.text = this.prefix + ((int)this.curNumber).ToString();
		}
	}

	private Text text;

	private TweenScale tweenscale;

	private float fromNumber;

	private float toNumber;

	private float curNumber;

	private float changeTime;

	private string prefix = string.Empty;

	private float ChangeNumber;

	private bool bstart;

	private float curTime;
}
