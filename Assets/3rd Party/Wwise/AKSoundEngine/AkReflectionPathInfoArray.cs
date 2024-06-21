// dnSpy decompiler from Assembly-CSharp.dll class: AkReflectionPathInfoArray
using System;

public class AkReflectionPathInfoArray : AkBaseArray<AkReflectionPathInfo>
{
	public AkReflectionPathInfoArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_GetSizeOf();
		}
	}

	protected override AkReflectionPathInfo CreateNewReferenceFromIntPtr(IntPtr address)
	{
		return new AkReflectionPathInfo(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(IntPtr address, AkReflectionPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkReflectionPathInfo_Clone(address, AkReflectionPathInfo.getCPtr(other));
	}
}
