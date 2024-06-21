using System;
using UnityEngine;

public static class TweenUtil
{
	public static void Reset(GameObject go)
	{
		TweenPosition[] componentsInChildren = go.GetComponentsInChildren<TweenPosition>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].transform.localPosition = Vector3.zero;
		}
		TweenScale[] componentsInChildren2 = go.GetComponentsInChildren<TweenScale>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].transform.localScale = Vector3.one;
		}
	}

	public static void Play(GameObject go)
	{
		UITweener[] componentsInChildren = go.GetComponentsInChildren<UITweener>(false);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Reset();
			componentsInChildren[i].Play(true);
		}
	}

	public static void SetFinishedDisActive(GameObject go, GameObject disObject = null)
	{
		if (disObject == null)
		{
			disObject = go;
		}
		UITweener[] components = go.GetComponents<UITweener>();
		if (components != null)
		{
			float num = 0f;
			UITweener uitweener = null;
			for (int i = 0; i < components.Length; i++)
			{
				float num2 = components[i].delay + components[i].duration;
				if (num <= num2)
				{
					uitweener = components[i];
					num = components[i].delay + components[i].duration;
				}
			}
			if (uitweener != null)
			{
				uitweener.onFinishedObjDisActive = disObject;
			}
		}
	}

	public static void SetEndPosition(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		TweenPosition[] components = go.GetComponents<TweenPosition>();
		if (components != null)
		{
			for (int i = 0; i < components.Length; i++)
			{
				components[i].to = new Vector3((float)UnityEngine.Random.Range(15, 90), (float)UnityEngine.Random.Range(15, 90), 0f);
			}
		}
	}
}
