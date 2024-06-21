// dnSpy decompiler from Assembly-CSharp.dll class: AkDiffractionPathInfoArray
using System;

public class AkDiffractionPathInfoArray : AkBaseArray<AkDiffractionPathInfo>
{
	public AkDiffractionPathInfoArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_GetSizeOf();
		}
	}

	protected override AkDiffractionPathInfo CreateNewReferenceFromIntPtr(IntPtr address)
	{
		return new AkDiffractionPathInfo(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(IntPtr address, AkDiffractionPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkDiffractionPathInfo_Clone(address, AkDiffractionPathInfo.getCPtr(other));
	}
}
