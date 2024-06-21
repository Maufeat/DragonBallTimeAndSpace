using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenMaterialAlpha : UITweener
{
	public Material Mat
	{
		get
		{
			return this._mat;
		}
		set
		{
			this._mat = value;
		}
	}

	public string Property
	{
		get
		{
			return this._property;
		}
		set
		{
			this._property = value;
		}
	}

	public float alpha
	{
		get
		{
			if (string.IsNullOrEmpty(this.Property))
			{
				return 0f;
			}
			if (this.Mat != null)
			{
				return this.Mat.GetFloat(this.Property);
			}
			return 0f;
		}
		set
		{
			if (!string.IsNullOrEmpty(this.Property) && null != this.Mat)
			{
				this.Mat.SetFloat(this.Property, value);
			}
		}
	}

	private void Awake()
	{
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.alpha = Mathf.Lerp(this.from, this.to, factor);
	}

	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.alpha;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	public float from = 1f;

	public float to = 1f;

	private Transform mTrans;

	private Material _mat;

	private string _property = "_Alpha";
}
