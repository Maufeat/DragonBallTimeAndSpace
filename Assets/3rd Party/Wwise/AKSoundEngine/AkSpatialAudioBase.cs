// dnSpy decompiler from Assembly-CSharp.dll class: AkSpatialAudioBase
using System;
using UnityEngine;

public abstract class AkSpatialAudioBase : MonoBehaviour
{
	protected void SetGameObjectInHighestPriorityRoom()
	{
		ulong highestPriorityRoomID = this.roomPriorityList.GetHighestPriorityRoomID();
		AkSoundEngine.SetGameObjectInRoom(base.gameObject, highestPriorityRoomID);
	}

	public void EnteredRoom(AkRoom room)
	{
		this.roomPriorityList.Add(room);
		this.SetGameObjectInHighestPriorityRoom();
	}

	public void ExitedRoom(AkRoom room)
	{
		this.roomPriorityList.Remove(room);
		this.SetGameObjectInHighestPriorityRoom();
	}

	public void SetGameObjectInRoom()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, 0f);
		foreach (Collider collider in array)
		{
			AkRoom component = collider.gameObject.GetComponent<AkRoom>();
			if (component != null)
			{
				this.roomPriorityList.Add(component);
			}
		}
		this.SetGameObjectInHighestPriorityRoom();
	}

	private readonly AkRoom.PriorityList roomPriorityList = new AkRoom.PriorityList();
}
