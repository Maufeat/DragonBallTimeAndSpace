using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPositionDirection : UITweener
{
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public Vector3 position
	{
		get
		{
			return this.cachedTransform.localPosition;
		}
		set
		{
			this.cachedTransform.localPosition = value;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		Vector3 localPosition = this.cachedTransform.localPosition;
		switch (this._direction)
		{
		case TweenPositionDirection.Direction.X:
			localPosition.x = this.from * (1f - factor) + this.to * factor;
			break;
		case TweenPositionDirection.Direction.Y:
			localPosition.y = this.from * (1f - factor) + this.to * factor;
			break;
		case TweenPositionDirection.Direction.Z:
			localPosition.z = this.from * (1f - factor) + this.to * factor;
			break;
		}
		this.cachedTransform.localPosition = localPosition;
	}

	public static TweenPositionDirection Begin(GameObject go, float duration, float f)
	{
		TweenPositionDirection tweenPositionDirection = UITweener.Begin<TweenPositionDirection>(go, duration);
		tweenPositionDirection.from = tweenPositionDirection.to;
		tweenPositionDirection.to = f;
		if (duration <= 0f)
		{
			tweenPositionDirection.Sample(1f, true);
			tweenPositionDirection.enabled = false;
		}
		return tweenPositionDirection;
	}

	public float from;

	public float to;

	public TweenPositionDirection.Direction _direction;

	private Transform mTrans;

	public enum Direction
	{
		X,
		Y,
		Z
	}
}
