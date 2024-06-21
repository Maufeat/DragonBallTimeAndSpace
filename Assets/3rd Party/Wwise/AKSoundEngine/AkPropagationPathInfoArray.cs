// dnSpy decompiler from Assembly-CSharp.dll class: AkPropagationPathInfoArray
using System;

public class AkPropagationPathInfoArray : AkBaseArray<AkPropagationPathInfo>
{
	public AkPropagationPathInfoArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_GetSizeOf();
		}
	}

	protected override AkPropagationPathInfo CreateNewReferenceFromIntPtr(IntPtr address)
	{
		return new AkPropagationPathInfo(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(IntPtr address, AkPropagationPathInfo other)
	{
		AkSoundEnginePINVOKE.CSharp_AkPropagationPathInfo_Clone(address, AkPropagationPathInfo.getCPtr(other));
	}
}
