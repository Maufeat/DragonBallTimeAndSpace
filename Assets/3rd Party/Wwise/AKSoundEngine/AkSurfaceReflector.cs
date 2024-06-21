// dnSpy decompiler from Assembly-CSharp.dll class: AkSurfaceReflector
using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[DisallowMultipleComponent]
[AddComponentMenu("Wwise/AkSurfaceReflector")]
public class AkSurfaceReflector : MonoBehaviour
{
	public static ulong GetAkGeometrySetID(MeshFilter meshFilter)
	{
		return (ulong)((long)meshFilter.GetInstanceID());
	}

	public static void AddGeometrySet(AcousticTexture acousticTexture, MeshFilter meshFilter, bool enableDiffraction, bool enableDiffractionOnBoundaryEdges)
	{
		if (!AkSoundEngine.IsInitialized())
		{
			return;
		}
		if (meshFilter == null)
		{
			UnityEngine.Debug.Log("AddGeometrySet(): No mesh found!");
		}
		else
		{
			Mesh sharedMesh = meshFilter.sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			int[] triangles = sharedMesh.triangles;
			int[] array = new int[vertices.Length];
			List<Vector3> list = new List<Vector3>();
			Dictionary<Vector3, int> dictionary = new Dictionary<Vector3, int>();
			for (int i = 0; i < vertices.Length; i++)
			{
				int num = 0;
				if (!dictionary.TryGetValue(vertices[i], out num))
				{
					num = list.Count;
					list.Add(vertices[i]);
					dictionary.Add(vertices[i], num);
					array[i] = num;
				}
				else
				{
					array[i] = num;
				}
			}
			int count = list.Count;
			using (AkAcousticSurfaceArray akAcousticSurfaceArray = new AkAcousticSurfaceArray(1))
			{
				AkAcousticSurface akAcousticSurface = akAcousticSurfaceArray[0];
				akAcousticSurface.textureID = acousticTexture.Id;
				akAcousticSurface.reflectorChannelMask = uint.MaxValue;
				akAcousticSurface.strName = meshFilter.gameObject.name;
				using (AkVertexArray akVertexArray = new AkVertexArray(count))
				{
					for (int j = 0; j < count; j++)
					{
						Vector3 vector = meshFilter.transform.TransformPoint(list[j]);
						using (AkVertex akVertex = akVertexArray[j])
						{
							akVertex.X = vector.x;
							akVertex.Y = vector.y;
							akVertex.Z = vector.z;
						}
					}
					int num2 = sharedMesh.triangles.Length / 3;
					using (AkTriangleArray akTriangleArray = new AkTriangleArray(num2))
					{
						for (int k = 0; k < num2; k++)
						{
							using (AkTriangle akTriangle = akTriangleArray[k])
							{
								akTriangle.point0 = (ushort)array[triangles[3 * k]];
								akTriangle.point1 = (ushort)array[triangles[3 * k + 1]];
								akTriangle.point2 = (ushort)array[triangles[3 * k + 2]];
								akTriangle.surface = 0;
							}
						}
						AkSoundEngine.SetGeometry(AkSurfaceReflector.GetAkGeometrySetID(meshFilter), akTriangleArray, (uint)akTriangleArray.Count(), akVertexArray, (uint)akVertexArray.Count(), akAcousticSurfaceArray, (uint)akAcousticSurfaceArray.Count(), enableDiffraction, enableDiffractionOnBoundaryEdges);
					}
				}
			}
		}
	}

	public static void RemoveGeometrySet(MeshFilter meshFilter)
	{
		if (meshFilter != null)
		{
			AkSoundEngine.RemoveGeometry(AkSurfaceReflector.GetAkGeometrySetID(meshFilter));
		}
	}

	private void Awake()
	{
		this.MeshFilter = base.GetComponent<MeshFilter>();
	}

	private void OnEnable()
	{
		AkSurfaceReflector.AddGeometrySet(this.AcousticTexture, this.MeshFilter, this.EnableDiffraction, this.EnableDiffractionOnBoundaryEdges);
	}

	private void OnDisable()
	{
		AkSurfaceReflector.RemoveGeometrySet(this.MeshFilter);
	}

	[Tooltip("All triangles of the component's mesh will be applied with this texture. The texture will change the filter parameters of the sound reflected from this component.")]
	public AcousticTexture AcousticTexture;

	[Tooltip("Enable or disable geometric diffraction for this mesh.")]
	[Header("Geometric Diffraction (Experimental)")]
	public bool EnableDiffraction;

	[Tooltip("Enable or disable geometric diffraction on boundary edges for this mesh.  Boundary edges are edges that are connected to only one triangle.")]
	public bool EnableDiffractionOnBoundaryEdges;

	private MeshFilter MeshFilter;
}
