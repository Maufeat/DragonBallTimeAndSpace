// dnSpy decompiler from Assembly-CSharp.dll class: AkGameObjPositionOffsetData
using System;
using UnityEngine;

[Serializable]
public class AkGameObjPositionOffsetData
{
	public AkGameObjPositionOffsetData(bool IReallyWantToBeConstructed = false)
	{
		this.KeepMe = IReallyWantToBeConstructed;
	}

	public bool KeepMe;

	public Vector3 positionOffset;
}
