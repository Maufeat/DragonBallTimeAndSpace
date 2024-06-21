// dnSpy decompiler from Assembly-CSharp.dll class: AkRoom
using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[AddComponentMenu("Wwise/AkRoom")]
[DisallowMultipleComponent]
public class AkRoom : AkUnityEventHandler
{
	public static bool IsSpatialAudioEnabled
	{
		get
		{
			return AkSpatialAudioListener.TheSpatialAudioListener != null && AkRoom.RoomCount > 0;
		}
	}

	public ulong GetID()
	{
		return AkSoundEngine.GetAkGameObjectID(base.gameObject);
	}

	private void OnEnable()
	{
		AkRoomParams akRoomParams = new AkRoomParams();
		akRoomParams.Up.X = base.transform.up.x;
		akRoomParams.Up.Y = base.transform.up.y;
		akRoomParams.Up.Z = base.transform.up.z;
		akRoomParams.Front.X = base.transform.forward.x;
		akRoomParams.Front.Y = base.transform.forward.y;
		akRoomParams.Front.Z = base.transform.forward.z;
		akRoomParams.ReverbAuxBus = this.reverbAuxBus.Id;
		akRoomParams.ReverbLevel = this.reverbLevel;
		akRoomParams.WallOcclusion = this.wallOcclusion;
		akRoomParams.RoomGameObj_AuxSendLevelToSelf = this.roomToneAuxSend;
		akRoomParams.RoomGameObj_KeepRegistered = this.roomToneEvent.IsValid();
		AkRoom.RoomCount++;
		AkSoundEngine.SetRoom(this.GetID(), akRoomParams, base.name);
	}

	protected override void Start()
	{
		base.Start();
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		this.roomToneEvent.Post(base.gameObject);
	}

	private void OnDisable()
	{
		AkRoom.RoomCount--;
		AkSoundEngine.RemoveRoom(this.GetID());
	}

	private void OnTriggerEnter(Collider in_other)
	{
		AkSpatialAudioBase[] componentsInChildren = in_other.GetComponentsInChildren<AkSpatialAudioBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].enabled)
			{
				componentsInChildren[i].EnteredRoom(this);
			}
		}
	}

	private void OnTriggerExit(Collider in_other)
	{
		AkSpatialAudioBase[] componentsInChildren = in_other.GetComponentsInChildren<AkSpatialAudioBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].enabled)
			{
				componentsInChildren[i].ExitedRoom(this);
			}
		}
	}

	public static ulong INVALID_ROOM_ID = ulong.MaxValue;

	private static int RoomCount;

	[Tooltip("Higher number has a higher priority")]
	public int priority;

	public AuxBus reverbAuxBus;

	[Range(0f, 1f)]
	public float reverbLevel = 1f;

	[Range(0f, 1f)]
	public float wallOcclusion = 1f;

	public AK.Wwise.Event roomToneEvent;

	[Range(0f, 1f)]
	[Tooltip("Send level for sounds that are posted on the room game object; adds reverb to ambience and room tones. Valid range: (0.f-1.f). A value of 0 disables the aux send.")]
	public float roomToneAuxSend;

	public class PriorityList
	{
		public ulong GetHighestPriorityRoomID()
		{
			AkRoom highestPriorityRoom = this.GetHighestPriorityRoom();
			return (!(highestPriorityRoom == null)) ? highestPriorityRoom.GetID() : AkRoom.INVALID_ROOM_ID;
		}

		public AkRoom GetHighestPriorityRoom()
		{
			if (this.rooms.Count == 0)
			{
				return null;
			}
			return this.rooms[0];
		}

		public void Add(AkRoom room)
		{
			int num = this.BinarySearch(room);
			if (num < 0)
			{
				this.rooms.Insert(~num, room);
			}
		}

		public void Remove(AkRoom room)
		{
			this.rooms.Remove(room);
		}

		public bool Contains(AkRoom room)
		{
			return this.BinarySearch(room) >= 0;
		}

		public int BinarySearch(AkRoom room)
		{
			if (room)
			{
				return this.rooms.BinarySearch(room, AkRoom.PriorityList.s_compareByPriority);
			}
			return -1;
		}

		private static readonly AkRoom.PriorityList.CompareByPriority s_compareByPriority = new AkRoom.PriorityList.CompareByPriority();

		public List<AkRoom> rooms = new List<AkRoom>();

		private class CompareByPriority : IComparer<AkRoom>
		{
			public virtual int Compare(AkRoom a, AkRoom b)
			{
				int num = a.priority.CompareTo(b.priority);
				if (num == 0 && a != b)
				{
					return 1;
				}
				return -num;
			}
		}
	}
}
