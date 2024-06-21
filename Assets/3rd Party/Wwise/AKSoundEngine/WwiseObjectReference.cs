// dnSpy decompiler from Assembly-CSharp.dll class: WwiseObjectReference
using System;
using UnityEngine;

public abstract class WwiseObjectReference : ScriptableObject
{
	public Guid Guid
	{
		get
		{
			return (!string.IsNullOrEmpty(this.guid)) ? new Guid(this.guid) : Guid.Empty;
		}
	}

	public string ObjectName
	{
		get
		{
			return this.objectName;
		}
	}

	public virtual string DisplayName
	{
		get
		{
			return this.ObjectName;
		}
	}

	public uint Id
	{
		get
		{
			return this.id;
		}
	}

	public abstract WwiseObjectType WwiseObjectType { get; }

	[AkShowOnly]
	[SerializeField]
	private string objectName;

	[AkShowOnly]
	[SerializeField]
	private uint id;

	[AkShowOnly]
	[SerializeField]
	private string guid;
}
