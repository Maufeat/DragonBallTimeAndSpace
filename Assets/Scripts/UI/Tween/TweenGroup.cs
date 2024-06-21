using System;
using UnityEngine;

public class TweenGroup : MonoBehaviour
{
	private void OnEnable()
	{
		if (!this.isinit)
		{
			this.isinit = true;
			this.Tweeners = base.transform.GetComponentsInChildren<UITweener>(true);
		}
		this.ResetAndPlay();
	}

	[ContextMenu("Reset and play")]
	private void ResetAndPlay()
	{
		if (this.Tweeners != null)
		{
			for (int i = 0; i < this.Tweeners.Length; i++)
			{
				if (this.Tweeners[i] != null)
				{
					this.Tweeners[i].ResetAndPlayForward();
				}
			}
		}
	}

	private UITweener[] Tweeners;

	private bool isinit;
}
