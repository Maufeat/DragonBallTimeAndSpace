// dnSpy decompiler from Assembly-CSharp.dll class: AkSpatialAudioEmitter
using System;
using AK.Wwise;
using UnityEngine;

[RequireComponent(typeof(AkGameObj))]
[AddComponentMenu("Wwise/AkSpatialAudioEmitter")]
public class AkSpatialAudioEmitter : AkSpatialAudioBase
{
	private void OnEnable()
	{
		AkEmitterSettings akEmitterSettings = new AkEmitterSettings();
		akEmitterSettings.reflectAuxBusID = this.reflectAuxBus.Id;
		akEmitterSettings.reflectionMaxPathLength = this.reflectionMaxPathLength;
		akEmitterSettings.reflectionsAuxBusGain = this.reflectionsAuxBusGain;
		akEmitterSettings.reflectionsOrder = this.reflectionsOrder;
		akEmitterSettings.reflectorFilterMask = uint.MaxValue;
		akEmitterSettings.roomReverbAuxBusGain = this.roomReverbAuxBusGain;
		akEmitterSettings.useImageSources = 0;
		akEmitterSettings.diffractionMaxEdges = this.diffractionMaxEdges;
		akEmitterSettings.diffractionMaxPaths = this.diffractionMaxPaths;
		akEmitterSettings.diffractionMaxPathLength = this.diffractionMaxPathLength;
		if (AkSoundEngine.RegisterEmitter(base.gameObject, akEmitterSettings) == AKRESULT.AK_Success)
		{
			base.SetGameObjectInRoom();
		}
	}

	private void OnDisable()
	{
		AkSoundEngine.UnregisterEmitter(base.gameObject);
	}

	[Header("Early Reflections")]
	[Tooltip("The Auxiliary Bus with a Reflect plug-in Effect applied.")]
	public AuxBus reflectAuxBus;

	[Tooltip("A heuristic to stop the computation of reflections. Should be no longer (and possibly shorter for less CPU usage) than the maximum attenuation of the sound emitter.")]
	public float reflectionMaxPathLength = 1000f;

	[Tooltip("The gain [0, 1] applied to the reflect auxiliary bus.")]
	[Range(0f, 1f)]
	public float reflectionsAuxBusGain = 1f;

	[Range(1f, 4f)]
	[Tooltip("The maximum number of reflections that will be processed for a sound path before it reaches the listener.")]
	public uint reflectionsOrder = 1u;

	[Tooltip("Send gain (0.f-1.f) that is applied when sending to the aux bus associated with the room that the emitter is in.")]
	[Header("Rooms")]
	[Range(0f, 1f)]
	public float roomReverbAuxBusGain = 1f;

	[Tooltip("The maximum number of edges that the sound can diffract around between the emitter and the listener.")]
	[Header("Geometric Diffraction (Experimental)")]
	public uint diffractionMaxEdges;

	[Tooltip("The maximum number of paths to the listener that the sound can take around obstacles.")]
	public uint diffractionMaxPaths;

	[Tooltip("The maximum length that a diffracted sound can travel.")]
	public uint diffractionMaxPathLength;
}
