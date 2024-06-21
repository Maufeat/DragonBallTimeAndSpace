// dnSpy decompiler from Assembly-CSharp.dll class: AkAcousticSurfaceArray
using System;

public class AkAcousticSurfaceArray : AkBaseArray<AkAcousticSurface>
{
	public AkAcousticSurfaceArray(int count) : base(count)
	{
	}

	protected override int StructureSize
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_GetSizeOf();
		}
	}

	protected override void ClearAtIntPtr(IntPtr address)
	{
		AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_Clear(address);
	}

	protected override AkAcousticSurface CreateNewReferenceFromIntPtr(IntPtr address)
	{
		return new AkAcousticSurface(address, false);
	}

	protected override void CloneIntoReferenceFromIntPtr(IntPtr address, AkAcousticSurface other)
	{
		AkSoundEnginePINVOKE.CSharp_AkAcousticSurface_Clone(address, AkAcousticSurface.getCPtr(other));
	}
}
