// dnSpy decompiler from Assembly-CSharp.dll class: WwiseAudioClip
using System;
using ProtoBuf;

[ProtoContract]
[Serializable]
public class WwiseAudioClip
{
	[ProtoMember(1)]
	public string AudioBankName;

	[ProtoMember(2)]
	public string AudioStartEventName;

	[ProtoMember(3)]
	public string AudioEndEventName;

	[ProtoMember(4)]
	public int StartDalay;

	[ProtoMember(5)]
	public uint Duration = 90u;

	[ProtoMember(6)]
	public WwiseAudioClip.TriggerType TrgType;

	[ProtoMember(7)]
	public bool CanMove;

	[ProtoMember(8)]
	public bool CanRot;

	public enum TriggerType
	{
		Once,
		Loop
	}
}
