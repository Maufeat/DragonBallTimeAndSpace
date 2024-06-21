using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenMaskOffset : UITweener
{
	public Vector2 offset
	{
		get
		{
			if (this.maskLightMat != null)
			{
				return this.maskLightMat.GetTextureOffset(this.strMaterialOffset);
			}
			return Vector2.zero;
		}
		set
		{
			if (this.maskLightMat != null)
			{
				this.maskLightMat.SetTextureOffset(this.strMaterialOffset, value);
			}
		}
	}

	private void Awake()
	{
		Graphic component = base.GetComponent<Graphic>();
		if (component != null)
		{
			this.maskLightMat = component.material;
		}
		if (this.maskLightMat == null || !this.maskLightMat.HasProperty(this.strMaterialOffset))
		{
			base.enabled = false;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.maskLightMat == null)
		{
			return;
		}
		Vector2 offset = this.from * (1f - factor) + this.to * factor;
		this.maskLightMat.SetTextureOffset(this.strMaterialOffset, offset);
	}

	public static TweenMaskOffset Begin(GameObject go, float duration, Vector2 offset)
	{
		TweenMaskOffset tweenMaskOffset = UITweener.Begin<TweenMaskOffset>(go, duration);
		tweenMaskOffset.from = tweenMaskOffset.offset;
		tweenMaskOffset.to = offset;
		if (duration <= 0f)
		{
			tweenMaskOffset.Sample(1f, true);
			tweenMaskOffset.enabled = false;
		}
		return tweenMaskOffset;
	}

	public string strMaterialOffset = "_MaskOffset";

	public Vector2 from;

	public Vector2 to;

	public Material maskLightMat;
}
