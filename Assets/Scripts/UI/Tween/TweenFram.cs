using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Tween/TweenFram")]
public class TweenFram : UITweener
{
	private void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.image != null && this.sprites != null && this.sprites.Length > 0)
		{
			int num = (int)Mathf.Lerp(0f, (float)this.sprites.Length, factor);
			if (num < this.sprites.Length)
			{
				this.image.sprite = this.sprites[num];
			}
		}
	}

	public static TweenFram Begin(GameObject go, int fram, float du = 1f)
	{
		return UITweener.Begin<TweenFram>(go, du);
	}

	public Sprite[] sprites;

	private Image image;
}
