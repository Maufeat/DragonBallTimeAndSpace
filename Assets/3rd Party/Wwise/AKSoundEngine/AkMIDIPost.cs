// dnSpy decompiler from Assembly-CSharp.dll class: AkMIDIPost
using System;
using UnityEngine;

public class AkMIDIPost : AkMIDIEvent
{
	internal AkMIDIPost(IntPtr cPtr, bool cMemoryOwn) : base(AkSoundEnginePINVOKE.CSharp_AkMIDIPost_SWIGUpcast(cPtr), cMemoryOwn)
	{
		this.swigCPtr = cPtr;
	}

	public AkMIDIPost() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIPost(), true)
	{
	}

	internal static IntPtr getCPtr(AkMIDIPost obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal override void setCPtr(IntPtr cPtr)
	{
		base.setCPtr(AkSoundEnginePINVOKE.CSharp_AkMIDIPost_SWIGUpcast(cPtr));
		this.swigCPtr = cPtr;
	}

	~AkMIDIPost()
	{
		this.Dispose();
	}

	public override void Dispose()
	{
		lock (this)
		{
			if (this.swigCPtr != IntPtr.Zero)
			{
				if (this.swigCMemOwn)
				{
					this.swigCMemOwn = false;
					AkSoundEnginePINVOKE.CSharp_delete_AkMIDIPost(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}

	public uint uOffset
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIPost_uOffset_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIPost_uOffset_set(this.swigCPtr, value);
		}
	}

	public AKRESULT PostOnEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uNumPosts)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AkMIDIPost_PostOnEvent(this.swigCPtr, in_eventID, akGameObjectID, in_uNumPosts);
	}

	public void Clone(AkMIDIPost other)
	{
		AkSoundEnginePINVOKE.CSharp_AkMIDIPost_Clone(this.swigCPtr, AkMIDIPost.getCPtr(other));
	}

	public static int GetSizeOf()
	{
		return AkSoundEnginePINVOKE.CSharp_AkMIDIPost_GetSizeOf();
	}

	private IntPtr swigCPtr;
}
