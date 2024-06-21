// dnSpy decompiler from Assembly-CSharp.dll class: AkCallbackSerializer
using System;

public class AkCallbackSerializer : IDisposable
{
	internal AkCallbackSerializer(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkCallbackSerializer() : this(AkSoundEnginePINVOKE.CSharp_new_AkCallbackSerializer(), true)
	{
	}

	internal static IntPtr getCPtr(AkCallbackSerializer obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkCallbackSerializer()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkCallbackSerializer(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public static AKRESULT Init(IntPtr in_pMemory, uint in_uSize)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Init(in_pMemory, in_uSize);
	}

	public static void Term()
	{
		AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Term();
	}

	public static IntPtr Lock()
	{
		return AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Lock();
	}

	public static void SetLocalOutput(uint in_uErrorLevel)
	{
		AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_SetLocalOutput(in_uErrorLevel);
	}

	public static void Unlock()
	{
		AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_Unlock();
	}

	public static AKRESULT AudioSourceChangeCallbackFunc(bool in_bOtherAudioPlaying, object in_pCookie)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkCallbackSerializer_AudioSourceChangeCallbackFunc(in_bOtherAudioPlaying, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;
}
