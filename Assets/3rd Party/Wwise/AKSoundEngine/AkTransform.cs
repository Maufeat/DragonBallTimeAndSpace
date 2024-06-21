// dnSpy decompiler from Assembly-CSharp.dll class: AkTransform
using System;

public class AkTransform : IDisposable
{
	internal AkTransform(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkTransform() : this(AkSoundEnginePINVOKE.CSharp_new_AkTransform(), true)
	{
	}

	internal static IntPtr getCPtr(AkTransform obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkTransform()
	{
		this.Dispose();
	}

	public virtual void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkTransform(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public AkVector Position()
	{
		return new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_Position(this.swigCPtr), false);
	}

	public AkVector OrientationFront()
	{
		return new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_OrientationFront(this.swigCPtr), false);
	}

	public AkVector OrientationTop()
	{
		return new AkVector(AkSoundEnginePINVOKE.CSharp_AkTransform_OrientationTop(this.swigCPtr), false);
	}

	public void Set(AkVector in_position, AkVector in_orientationFront, AkVector in_orientationTop)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_Set__SWIG_0(this.swigCPtr, AkVector.getCPtr(in_position), AkVector.getCPtr(in_orientationFront), AkVector.getCPtr(in_orientationTop));
	}

	public void Set(float in_positionX, float in_positionY, float in_positionZ, float in_orientFrontX, float in_orientFrontY, float in_orientFrontZ, float in_orientTopX, float in_orientTopY, float in_orientTopZ)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_Set__SWIG_1(this.swigCPtr, in_positionX, in_positionY, in_positionZ, in_orientFrontX, in_orientFrontY, in_orientFrontZ, in_orientTopX, in_orientTopY, in_orientTopZ);
	}

	public void SetPosition(AkVector in_position)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetPosition__SWIG_0(this.swigCPtr, AkVector.getCPtr(in_position));
	}

	public void SetPosition(float in_x, float in_y, float in_z)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetPosition__SWIG_1(this.swigCPtr, in_x, in_y, in_z);
	}

	public void SetOrientation(AkVector in_orientationFront, AkVector in_orientationTop)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetOrientation__SWIG_0(this.swigCPtr, AkVector.getCPtr(in_orientationFront), AkVector.getCPtr(in_orientationTop));
	}

	public void SetOrientation(float in_orientFrontX, float in_orientFrontY, float in_orientFrontZ, float in_orientTopX, float in_orientTopY, float in_orientTopZ)
	{
		AkSoundEnginePINVOKE.CSharp_AkTransform_SetOrientation__SWIG_1(this.swigCPtr, in_orientFrontX, in_orientFrontY, in_orientFrontZ, in_orientTopX, in_orientTopY, in_orientTopZ);
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
