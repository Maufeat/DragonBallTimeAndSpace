// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.BaseType
using System;
using UnityEngine;

namespace AK.Wwise
{
	[Serializable]
	public abstract class BaseType : ISerializationCallbackReceiver
	{
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}

		public abstract WwiseObjectType WwiseObjectType { get; }

		[Obsolete("This functionality is deprecated as of Wwise v2018.1.2 and will be removed in a future release.")]
		public int ID
		{
			get
			{
				return (int)this.Id;
			}
		}

		public virtual bool IsValid()
		{
			return this.Id != 0u;
		}

		public bool Validate()
		{
			if (this.IsValid())
			{
				return true;
			}
			UnityEngine.Debug.LogWarning("Wwise ID has not been resolved. Consider picking a new " + base.GetType().Name + ".");
			return false;
		}

		protected void Verify(AKRESULT result)
		{
		}

		public override string ToString()
		{
			return (!this.IsValid()) ? ("Empty " + base.GetType().Name) : this.Name;
		}

		public static bool IsByteArrayValidGuid(byte[] byteArray)
		{
			if (byteArray == null)
			{
				return false;
			}
			bool result;
			try
			{
				Guid guid = new Guid(byteArray);
				result = !guid.Equals(Guid.Empty);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public string Name;

		public uint Id;

		[HideInInspector]
		public byte[] valueGuid = new byte[16];
	}
}
