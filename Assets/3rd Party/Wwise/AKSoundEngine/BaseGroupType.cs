// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.BaseGroupType
using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	public abstract class BaseGroupType : BaseType
	{
		public abstract WwiseObjectType WwiseObjectGroupType { get; }

		[Obsolete("This functionality is deprecated as of Wwise v2018.1.2 and will be removed in a future release.")]
		public int groupID
		{
			get
			{
				return (int)this.GroupId;
			}
		}

		public override bool IsValid()
		{
			return base.IsValid() && this.GroupId != 0u;
		}

		public uint GroupId;

		public string GroupName;

		[HideInInspector]
		public byte[] groupGuid;
	}
}
