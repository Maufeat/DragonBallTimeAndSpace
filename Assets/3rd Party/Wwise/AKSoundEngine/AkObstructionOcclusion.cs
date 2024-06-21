// dnSpy decompiler from Assembly-CSharp.dll class: AkObstructionOcclusion
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AkObstructionOcclusion : MonoBehaviour
{
	protected void InitIntervalsAndFadeRates()
	{
		this.refreshTime = UnityEngine.Random.Range(0f, this.refreshInterval);
		this.fadeRate = 1f / this.fadeTime;
	}

	protected void UpdateObstructionOcclusionValues(List<AkAudioListener> listenerList)
	{
		for (int i = 0; i < listenerList.Count; i++)
		{
			if (!this.ObstructionOcclusionValues.ContainsKey(listenerList[i]))
			{
				this.ObstructionOcclusionValues.Add(listenerList[i], new AkObstructionOcclusion.ObstructionOcclusionValue());
			}
		}
		foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
		{
			if (!listenerList.Contains(keyValuePair.Key))
			{
				this.listenersToRemove.Add(keyValuePair.Key);
			}
		}
		for (int j = 0; j < this.listenersToRemove.Count; j++)
		{
			this.ObstructionOcclusionValues.Remove(this.listenersToRemove[j]);
		}
	}

	protected void UpdateObstructionOcclusionValues(AkAudioListener listener)
	{
		if (!listener)
		{
			return;
		}
		if (!this.ObstructionOcclusionValues.ContainsKey(listener))
		{
			this.ObstructionOcclusionValues.Add(listener, new AkObstructionOcclusion.ObstructionOcclusionValue());
		}
		foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
		{
			if (listener != keyValuePair.Key)
			{
				this.listenersToRemove.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.listenersToRemove.Count; i++)
		{
			this.ObstructionOcclusionValues.Remove(this.listenersToRemove[i]);
		}
	}

	private void CastRays()
	{
		if (this.refreshTime > this.refreshInterval)
		{
			this.refreshTime -= this.refreshInterval;
			foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> keyValuePair in this.ObstructionOcclusionValues)
			{
				AkAudioListener key = keyValuePair.Key;
				AkObstructionOcclusion.ObstructionOcclusionValue value = keyValuePair.Value;
				Vector3 a = key.transform.position - base.transform.position;
				float magnitude = a.magnitude;
				if (this.maxDistance > 0f && magnitude > this.maxDistance)
				{
					value.targetValue = value.currentValue;
				}
				else
				{
					value.targetValue = ((!Physics.Raycast(base.transform.position, a / magnitude, magnitude, this.LayerMask.value)) ? 0f : 1f);
				}
			}
		}
		this.refreshTime += Time.deltaTime;
	}

	protected abstract void UpdateObstructionOcclusionValuesForListeners();

	protected abstract void SetObstructionOcclusion(KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObsOccPair);

	private void Update()
	{
		this.UpdateObstructionOcclusionValuesForListeners();
		this.CastRays();
		foreach (KeyValuePair<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> obstructionOcclusion in this.ObstructionOcclusionValues)
		{
			if (obstructionOcclusion.Value.Update(this.fadeRate))
			{
				this.SetObstructionOcclusion(obstructionOcclusion);
			}
		}
	}

	private readonly List<AkAudioListener> listenersToRemove = new List<AkAudioListener>();

	private readonly Dictionary<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue> ObstructionOcclusionValues = new Dictionary<AkAudioListener, AkObstructionOcclusion.ObstructionOcclusionValue>();

	protected float fadeRate;

	[Tooltip("Fade time in seconds")]
	public float fadeTime = 0.5f;

	[Tooltip("Layers of obstructers/occluders")]
	public LayerMask LayerMask = -1;

	[Tooltip("Maximum distance to perform the obstruction/occlusion. Negative values mean infinite")]
	public float maxDistance = -1f;

	[Tooltip("The number of seconds between raycasts")]
	public float refreshInterval = 1f;

	private float refreshTime;

	protected class ObstructionOcclusionValue
	{
		public bool Update(float fadeRate)
		{
			if (Mathf.Approximately(this.targetValue, this.currentValue))
			{
				return false;
			}
			this.currentValue += fadeRate * Mathf.Sign(this.targetValue - this.currentValue) * Time.deltaTime;
			this.currentValue = Mathf.Clamp(this.currentValue, 0f, 1f);
			return true;
		}

		public float currentValue;

		public float targetValue;
	}
}
