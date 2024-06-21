using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Tween/Color")]
public class TweenColor : UITweener
{
	public Color color
	{
		get
		{
			if (this.graphic != null)
			{
				return this.graphic.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.color;
			}
			return Color.black;
		}
		set
		{
			if (this.graphic != null)
			{
				this.graphic.color = value;
			}
			if (this.mMat != null)
			{
				this.mMat.color = value;
			}
			if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = (value.r + value.g + value.b > 0.01f);
			}
		}
	}

	private void Awake()
	{
		this.graphic = base.GetComponentInChildren<Graphic>();
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			this.mMat = component.material;
		}
		this.mLight = base.GetComponent<Light>();
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.color = Color.Lerp(this.from, this.to, factor);
	}

	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.color;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	public Color from = Color.white;

	public Color to = Color.white;

	private Transform mTrans;

	private Graphic graphic;

	private Material mMat;

	private Light mLight;
}
