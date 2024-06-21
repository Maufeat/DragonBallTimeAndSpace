using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenUV : UITweener
{
	public Vector2 offset
	{
		get
		{
			if (this.maskLightMat != null)
			{
				return this.maskLightMat.mainTextureOffset;
			}
			return Vector2.zero;
		}
		set
		{
			if (this.maskLightMat != null)
			{
				this.maskLightMat.mainTextureOffset = value;
			}
		}
	}

	private void Awake()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			this.maskLightMat = component.material;
		}
		if (this.maskLightMat == null)
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
		for (int i = 0; i < this.shaderPropertiesUV.Length; i++)
		{
			this.maskLightMat.SetTextureOffset(this.shaderPropertiesUV[i], offset);
		}
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

	public string[] shaderPropertiesUV;

	public Vector2 from;

	public Vector2 to;

	public Material maskLightMat;
}
