// dnSpy decompiler from Assembly-CSharp.dll class: AkVertexArray
using System;

public class AkVertexArray : AkBaseArray<AkVertex>
{
	public AkVertexArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkVertex_GetSizeOf();
		}
	}

	protected override void ClearAtIntPtr(IntPtr address)
	{
		AkSoundEnginePINVOKE.CSharp_AkVertex_Clear(address);
	}

	protected override AkVertex CreateNewReferenceFromIntPtr(IntPtr address)
	{
		return new AkVertex(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(IntPtr address, AkVertex other)
	{
		AkSoundEnginePINVOKE.CSharp_AkVertex_Clone(address, AkVertex.getCPtr(other));
	}
}
