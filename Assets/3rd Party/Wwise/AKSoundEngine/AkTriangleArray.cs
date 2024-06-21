// dnSpy decompiler from Assembly-CSharp.dll class: AkTriangleArray
using System;

public class AkTriangleArray : AkBaseArray<AkTriangle>
{
	public AkTriangleArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkTriangle_GetSizeOf();
		}
	}

	protected override void ClearAtIntPtr(IntPtr address)
	{
		AkSoundEnginePINVOKE.CSharp_AkTriangle_Clear(address);
	}

	protected override AkTriangle CreateNewReferenceFromIntPtr(IntPtr address)
	{
		return new AkTriangle(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(IntPtr address, AkTriangle other)
	{
		AkSoundEnginePINVOKE.CSharp_AkTriangle_Clone(address, AkTriangle.getCPtr(other));
	}
}
