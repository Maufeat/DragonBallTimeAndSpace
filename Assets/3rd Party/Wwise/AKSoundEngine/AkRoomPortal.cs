// dnSpy decompiler from Assembly-CSharp.dll class: AkRoomPortal
using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Wwise/AkRoomPortal")]
public class AkRoomPortal : AkUnityEventHandler
{
	public ulong GetID()
	{
		return (ulong)((long)base.GetInstanceID());
	}

	protected override void Awake()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		component.isTrigger = true;
		this.portalTransform.Set(component.bounds.center.x, component.bounds.center.y, component.bounds.center.z, base.transform.forward.x, base.transform.forward.y, base.transform.forward.z, base.transform.up.x, base.transform.up.y, base.transform.up.z);
		this.extent.X = component.size.x * base.transform.localScale.x / 2f;
		this.extent.Y = component.size.y * base.transform.localScale.y / 2f;
		this.extent.Z = component.size.z * base.transform.localScale.z / 2f;
		this.frontRoomID = ((!(this.rooms[1] == null)) ? this.rooms[1].GetID() : AkRoom.INVALID_ROOM_ID);
		this.backRoomID = ((!(this.rooms[0] == null)) ? this.rooms[0].GetID() : AkRoom.INVALID_ROOM_ID);
		base.RegisterTriggers(this.closePortalTriggerList, new AkTriggerBase.Trigger(this.ClosePortal));
		base.Awake();
		if (this.closePortalTriggerList.Contains(1151176110))
		{
			this.ClosePortal(null);
		}
	}

	protected override void Start()
	{
		base.Start();
		if (this.closePortalTriggerList.Contains(1281810935))
		{
			this.ClosePortal(null);
		}
	}

	public override void HandleEvent(GameObject in_gameObject)
	{
		this.Open();
	}

	public void ClosePortal(GameObject in_gameObject)
	{
		this.Close();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.UnregisterTriggers(this.closePortalTriggerList, new AkTriggerBase.Trigger(this.ClosePortal));
		if (this.closePortalTriggerList.Contains(-358577003))
		{
			this.ClosePortal(null);
		}
	}

	public void Open()
	{
		this.ActivatePortal(true);
	}

	public void Close()
	{
		this.ActivatePortal(false);
	}

	private void ActivatePortal(bool active)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.frontRoomID != this.backRoomID)
		{
			AkSoundEngine.SetRoomPortal(this.GetID(), this.portalTransform, this.extent, active, this.frontRoomID, this.backRoomID);
		}
		else
		{
			UnityEngine.Debug.LogError(base.name + " is not placed/oriented correctly");
		}
	}

	public void FindOverlappingRooms(AkRoom.PriorityList[] roomList)
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component == null)
		{
			return;
		}
		Vector3 halfExtents = new Vector3(component.size.x * base.transform.localScale.x / 2f, component.size.y * base.transform.localScale.y / 2f, component.size.z * base.transform.localScale.z / 4f);
		this.FillRoomList(Vector3.forward * -0.25f, halfExtents, roomList[0]);
		this.FillRoomList(Vector3.forward * 0.25f, halfExtents, roomList[1]);
	}

	private void FillRoomList(Vector3 center, Vector3 halfExtents, AkRoom.PriorityList list)
	{
		list.rooms.Clear();
		center = base.transform.TransformPoint(center);
		Collider[] array = Physics.OverlapBox(center, halfExtents, base.transform.rotation, -1, QueryTriggerInteraction.Collide);
		foreach (Collider collider in array)
		{
			AkRoom component = collider.gameObject.GetComponent<AkRoom>();
			if (component != null && !list.Contains(component))
			{
				list.Add(component);
			}
		}
	}

	public void SetFrontRoom(AkRoom room)
	{
		this.rooms[1] = room;
		this.frontRoomID = ((!(this.rooms[1] == null)) ? this.rooms[1].GetID() : AkRoom.INVALID_ROOM_ID);
	}

	public void SetBackRoom(AkRoom room)
	{
		this.rooms[0] = room;
		this.backRoomID = ((!(this.rooms[0] == null)) ? this.rooms[0].GetID() : AkRoom.INVALID_ROOM_ID);
	}

	public void UpdateOverlappingRooms()
	{
		AkRoom.PriorityList[] array = new AkRoom.PriorityList[]
		{
			new AkRoom.PriorityList(),
			new AkRoom.PriorityList()
		};
		this.FindOverlappingRooms(array);
		for (int i = 0; i < 2; i++)
		{
			if (!array[i].Contains(this.rooms[i]))
			{
				this.rooms[i] = array[i].GetHighestPriorityRoom();
			}
		}
		this.frontRoomID = ((!(this.rooms[1] == null)) ? this.rooms[1].GetID() : AkRoom.INVALID_ROOM_ID);
		this.backRoomID = ((!(this.rooms[0] == null)) ? this.rooms[0].GetID() : AkRoom.INVALID_ROOM_ID);
	}

	public const int MAX_ROOMS_PER_PORTAL = 2;

	private readonly AkVector extent = new AkVector();

	private readonly AkTransform portalTransform = new AkTransform();

	private ulong backRoomID = AkRoom.INVALID_ROOM_ID;

	public List<int> closePortalTriggerList = new List<int>();

	private ulong frontRoomID = AkRoom.INVALID_ROOM_ID;

	public AkRoom[] rooms = new AkRoom[2];
}
