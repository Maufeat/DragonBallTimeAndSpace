// dnSpy decompiler from Assembly-CSharp.dll class: AkEnvironment
using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

[AddComponentMenu("Wwise/AkEnvironment")]
[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class AkEnvironment : MonoBehaviour, ISerializationCallbackReceiver
{
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
	}

	[Obsolete("This functionality is deprecated as of Wwise v2018.1.2 and will be removed in a future release.")]
	public int m_auxBusID
	{
		get
		{
			return (int)((this.data != null) ? this.data.Id : 0u);
		}
	}

	[Obsolete("This functionality is deprecated as of Wwise v2018.1.2 and will be removed in a future release.")]
	public uint GetAuxBusID()
	{
		return this.data.Id;
	}

	public float GetAuxSendValueForPosition(Vector3 in_position)
	{
		return 1f;
	}

	public Collider GetCollider()
	{
		return this.m_Collider;
	}

	public void Awake()
	{
		this.m_Collider = base.GetComponent<Collider>();
	}

	public const int MAX_NB_ENVIRONMENTS = 4;

	public static AkEnvironment.AkEnvironment_CompareByPriority s_compareByPriority = new AkEnvironment.AkEnvironment_CompareByPriority();

	public static AkEnvironment.AkEnvironment_CompareBySelectionAlgorithm s_compareBySelectionAlgorithm = new AkEnvironment.AkEnvironment_CompareBySelectionAlgorithm();

	public bool excludeOthers;

	public bool isDefault;

	public AuxBus data = new AuxBus();

	private Collider m_Collider;

	public int priority;

	[SerializeField]
	[HideInInspector]
	private byte[] valueGuid;

	public class AkEnvironment_CompareByPriority : IComparer<AkEnvironment>
	{
		public virtual int Compare(AkEnvironment a, AkEnvironment b)
		{
			int num = a.priority.CompareTo(b.priority);
			return (num != 0 || !(a != b)) ? num : 1;
		}
	}

	public class AkEnvironment_CompareBySelectionAlgorithm : AkEnvironment.AkEnvironment_CompareByPriority
	{
		public override int Compare(AkEnvironment a, AkEnvironment b)
		{
			if (a.isDefault)
			{
				return (!b.isDefault) ? 1 : base.Compare(a, b);
			}
			if (b.isDefault)
			{
				return -1;
			}
			if (a.excludeOthers)
			{
				return (!b.excludeOthers) ? -1 : base.Compare(a, b);
			}
			return (!b.excludeOthers) ? base.Compare(a, b) : 1;
		}
	}
}
