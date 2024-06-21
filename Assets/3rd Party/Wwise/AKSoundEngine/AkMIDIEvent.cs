// dnSpy decompiler from Assembly-CSharp.dll class: AkMIDIEvent
using System;

public class AkMIDIEvent : IDisposable
{
	internal AkMIDIEvent(IntPtr cPtr, bool cMemoryOwn)
	{
		this.swigCMemOwn = cMemoryOwn;
		this.swigCPtr = cPtr;
	}

	public AkMIDIEvent() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent(), true)
	{
	}

	internal static IntPtr getCPtr(AkMIDIEvent obj)
	{
		return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
	}

	internal virtual void setCPtr(IntPtr cPtr)
	{
		this.Dispose();
		this.swigCPtr = cPtr;
	}

	~AkMIDIEvent()
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
					AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent(this.swigCPtr);
				}
				this.swigCPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}
	}

	public byte byChan
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChan_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChan_set(this.swigCPtr, value);
		}
	}

	public AkMIDIEvent.tGen Gen
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Gen_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tGen(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Gen_set(this.swigCPtr, AkMIDIEvent.tGen.getCPtr(value));
		}
	}

	public AkMIDIEvent.tCc Cc
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Cc_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tCc(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_Cc_set(this.swigCPtr, AkMIDIEvent.tCc.getCPtr(value));
		}
	}

	public AkMIDIEvent.tNoteOnOff NoteOnOff
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteOnOff_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tNoteOnOff(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteOnOff_set(this.swigCPtr, AkMIDIEvent.tNoteOnOff.getCPtr(value));
		}
	}

	public AkMIDIEvent.tPitchBend PitchBend
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_PitchBend_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tPitchBend(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_PitchBend_set(this.swigCPtr, AkMIDIEvent.tPitchBend.getCPtr(value));
		}
	}

	public AkMIDIEvent.tNoteAftertouch NoteAftertouch
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteAftertouch_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tNoteAftertouch(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_NoteAftertouch_set(this.swigCPtr, AkMIDIEvent.tNoteAftertouch.getCPtr(value));
		}
	}

	public AkMIDIEvent.tChanAftertouch ChanAftertouch
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ChanAftertouch_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tChanAftertouch(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ChanAftertouch_set(this.swigCPtr, AkMIDIEvent.tChanAftertouch.getCPtr(value));
		}
	}

	public AkMIDIEvent.tProgramChange ProgramChange
	{
		get
		{
			IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ProgramChange_get(this.swigCPtr);
			return (!(intPtr == IntPtr.Zero)) ? new AkMIDIEvent.tProgramChange(intPtr, false) : null;
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_ProgramChange_set(this.swigCPtr, AkMIDIEvent.tProgramChange.getCPtr(value));
		}
	}

	public AkMIDIEventTypes byType
	{
		get
		{
			return (AkMIDIEventTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byType_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byType_set(this.swigCPtr, (int)value);
		}
	}

	public byte byOnOffNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byOnOffNote_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byOnOffNote_set(this.swigCPtr, value);
		}
	}

	public byte byVelocity
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byVelocity_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byVelocity_set(this.swigCPtr, value);
		}
	}

	public AkMIDICcTypes byCc
	{
		get
		{
			return (AkMIDICcTypes)AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCc_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCc_set(this.swigCPtr, (int)value);
		}
	}

	public byte byCcValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCcValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byCcValue_set(this.swigCPtr, value);
		}
	}

	public byte byValueLsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueLsb_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueLsb_set(this.swigCPtr, value);
		}
	}

	public byte byValueMsb
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueMsb_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byValueMsb_set(this.swigCPtr, value);
		}
	}

	public byte byAftertouchNote
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byAftertouchNote_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byAftertouchNote_set(this.swigCPtr, value);
		}
	}

	public byte byNoteAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byNoteAftertouchValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byNoteAftertouchValue_set(this.swigCPtr, value);
		}
	}

	public byte byChanAftertouchValue
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChanAftertouchValue_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byChanAftertouchValue_set(this.swigCPtr, value);
		}
	}

	public byte byProgramNum
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byProgramNum_get(this.swigCPtr);
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_byProgramNum_set(this.swigCPtr, value);
		}
	}

	private IntPtr swigCPtr;

	protected bool swigCMemOwn;

	public class tGen : IDisposable
	{
		internal tGen(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tGen() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tGen(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tGen obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tGen()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tGen(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byParam1
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam1_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam1_set(this.swigCPtr, value);
			}
		}

		public byte byParam2
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam2_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tGen_byParam2_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}

	public class tNoteOnOff : IDisposable
	{
		internal tNoteOnOff(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tNoteOnOff() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tNoteOnOff(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tNoteOnOff obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tNoteOnOff()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tNoteOnOff(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byNote
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byNote_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byNote_set(this.swigCPtr, value);
			}
		}

		public byte byVelocity
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byVelocity_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteOnOff_byVelocity_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}

	public class tCc : IDisposable
	{
		internal tCc(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tCc() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tCc(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tCc obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tCc()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tCc(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byCc
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byCc_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byCc_set(this.swigCPtr, value);
			}
		}

		public byte byValue
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byValue_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tCc_byValue_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}

	public class tPitchBend : IDisposable
	{
		internal tPitchBend(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tPitchBend() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tPitchBend(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tPitchBend obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tPitchBend()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tPitchBend(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byValueLsb
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueLsb_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueLsb_set(this.swigCPtr, value);
			}
		}

		public byte byValueMsb
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueMsb_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tPitchBend_byValueMsb_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}

	public class tNoteAftertouch : IDisposable
	{
		internal tNoteAftertouch(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tNoteAftertouch() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tNoteAftertouch(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tNoteAftertouch obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tNoteAftertouch()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tNoteAftertouch(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byNote
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byNote_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byNote_set(this.swigCPtr, value);
			}
		}

		public byte byValue
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byValue_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tNoteAftertouch_byValue_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}

	public class tChanAftertouch : IDisposable
	{
		internal tChanAftertouch(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tChanAftertouch() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tChanAftertouch(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tChanAftertouch obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tChanAftertouch()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tChanAftertouch(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byValue
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tChanAftertouch_byValue_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tChanAftertouch_byValue_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}

	public class tProgramChange : IDisposable
	{
		internal tProgramChange(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = cPtr;
		}

		public tProgramChange() : this(AkSoundEnginePINVOKE.CSharp_new_AkMIDIEvent_tProgramChange(), true)
		{
		}

		internal static IntPtr getCPtr(AkMIDIEvent.tProgramChange obj)
		{
			return (obj != null) ? obj.swigCPtr : IntPtr.Zero;
		}

		internal virtual void setCPtr(IntPtr cPtr)
		{
			this.Dispose();
			this.swigCPtr = cPtr;
		}

		~tProgramChange()
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
						AkSoundEnginePINVOKE.CSharp_delete_AkMIDIEvent_tProgramChange(this.swigCPtr);
					}
					this.swigCPtr = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}
		}

		public byte byProgramNum
		{
			get
			{
				return AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tProgramChange_byProgramNum_get(this.swigCPtr);
			}
			set
			{
				AkSoundEnginePINVOKE.CSharp_AkMIDIEvent_tProgramChange_byProgramNum_set(this.swigCPtr, value);
			}
		}

		private IntPtr swigCPtr;

		protected bool swigCMemOwn;
	}
}
