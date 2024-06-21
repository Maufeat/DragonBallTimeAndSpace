// dnSpy decompiler from Assembly-CSharp.dll class: AkCommonCommSettings
using System;
using UnityEngine;

[Serializable]
public class AkCommonCommSettings : AkSettingsValidationHandler
{
	public virtual void CopyTo(AkCommunicationSettings settings)
	{
		settings.uPoolSize = this.m_PoolSize;
		settings.uDiscoveryBroadcastPort = this.m_DiscoveryBroadcastPort;
		settings.uCommandPort = this.m_CommandPort;
		settings.uNotificationPort = this.m_NotificationPort;
		settings.bInitSystemLib = this.m_InitializeSystemComms;
		string text = this.m_NetworkName;
		if (string.IsNullOrEmpty(text))
		{
			text = Application.productName;
		}
		settings.szAppNetworkName = text;
	}

	[Tooltip("Size of the communication pool.")]
	public uint m_PoolSize = 262144u;

	public static ushort DefaultDiscoveryBroadcastPort = 24024;

	[Tooltip("The port where the authoring application broadcasts \"Game Discovery\" requests to discover games running on the network. Default value: 24024. (Cannot be set to 0)")]
	public ushort m_DiscoveryBroadcastPort = AkCommonCommSettings.DefaultDiscoveryBroadcastPort;

	[Tooltip("The \"command\" channel port. Set to 0 to request a dynamic/ephemeral port.")]
	public ushort m_CommandPort;

	[Tooltip("The \"notification\" channel port. Set to 0 to request a dynamic/ephemeral port.")]
	public ushort m_NotificationPort;

	[Tooltip("Indicates whether the communication system should be initialized. Some consoles have critical requirements for initialization of their communications system. Set to false only if your game already uses sockets before sound engine initialization.")]
	public bool m_InitializeSystemComms = true;

	[Tooltip("The name used to identify this game within the authoring application. Leave empty to use \"UnityEngine.Application.productName\".")]
	public string m_NetworkName;
}
