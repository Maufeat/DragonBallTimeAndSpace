/* SimpleLOD 1.5     */
/* By Orbcreation BV */
/* Richard Knol      */
/* March 4, 2015      */

/* Note: if you also use other packages by Orbcreation,  */
/* you may end up with multiple copies of this file.     */
/* In that case, better delete/merge those files into 1. */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_WP8
#elif UNITY_WP_8_1
#elif UNITY_WSA
#elif UNITY_WSA_8_0
#elif UNITY_WSA_8_1
#elif UNITY_WINRT
#elif UNITY_WINRT_8_0
#elif UNITY_WINRT_8_1
#elif NETFX_CORE
#else
using System.Threading;
#endif

using LOD = UnityEngine.LOD;  // This is needed when SimpleLOD is used in combination with SmartLOD

namespace OrbCreationExtensions
{
    public static class GameObjectExtensions
    {

        public static Bounds GetWorldBounds(this GameObject go)
        {
            if (go.transform == null) return new Bounds();
            Bounds goBounds = new Bounds(go.transform.position, Vector3.zero);
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
            if (renderers != null && renderers.Length > 0) goBounds = renderers[0].bounds;
            foreach (Renderer r in renderers)
            {
                Bounds bounds = r.bounds;
                goBounds.Encapsulate(bounds);
            }
            return goBounds;
        }
        // Bounds are always in world coordinates
        // Bounds always gives a rectangle that is straight with global x,y,z axis.
        // So when you rotate a cude with 45 degrees, its bounds will get bigger
        // localScale is incorporated, so when you increase the scale, the bounds will increase too
        // But watch out: 
        // When you move or alter a GameObject that is set to inactive (activeSelf == false), the bounds are not updated!!!


        public static Vector3[] GetBoundsCorners(this Bounds bounds)
        {
            Vector3[] corners = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                corners[i] = bounds.min;
                if ((i & 1) > 0) corners[i].x += bounds.size.x;
                if ((i & 2) > 0) corners[i].y += bounds.size.y;
                if ((i & 4) > 0) corners[i].z += bounds.size.z;
            }
            return corners;
        }
        public static Vector3[] GetBoundsCenterAndCorners(this Bounds bounds)
        {
            Vector3[] corners = new Vector3[9];
            corners[0] = bounds.center;
            for (int i = 1; i < 9; i++)
            {
                corners[i] = bounds.min;
                if ((i & 1) > 0) corners[i].x += bounds.size.x;
                if ((i & 2) > 0) corners[i].y += bounds.size.y;
                if ((i & 4) > 0) corners[i].z += bounds.size.z;
            }
            return corners;
        }

        public static Vector3[] GetWorldBoundsCorners(this GameObject go)
        {
            return GetBoundsCorners(GetWorldBounds(go));
        }
        // These are in world coordinates

        public static Vector3[] GetWorldBoundsCenterAndCorners(this GameObject go)
        {
            return GetBoundsCenterAndCorners(GetWorldBounds(go));
        }

        public static float GetModelComplexity(this GameObject go)
        {
            float complexity = 0f;
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter mf in meshFilters)
            {
                Mesh mesh = mf.sharedMesh;
                float multiplier = 1f;
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    complexity += multiplier * mesh.GetTriangles(i).Length / 3f / 65536f;
                    // nog iets doen met de shader
                    multiplier *= 1.1f; // more materials, higher weight
                }
            }
            return complexity;       // 1 model with 1 submesh of 64K vertices = 1.0f
        }

        public static string GetModelInfoString(this GameObject go)
        {
            string infoString = "";
            int meshCount = 0;
            int subMeshCount = 0;
            int vertexCount = 0;
            int triangleCount = 0;
            Bounds bounds = GetWorldBounds(go);
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter mf in meshFilters)
            {
                Mesh mesh = mf.sharedMesh;
                meshCount++;
                subMeshCount += mesh.subMeshCount;
                vertexCount += mesh.vertices.Length;
                triangleCount += mesh.triangles.Length / 3;
            }
            infoString = infoString + meshCount + " meshes\n";
            infoString = infoString + subMeshCount + " submeshes\n";
            infoString = infoString + vertexCount + " vertices\n";
            infoString = infoString + triangleCount + " triangles\n";
            infoString = infoString + bounds.size + " meters";
            return infoString;
        }

        public static GameObject TopParent(this GameObject go)
        {
            Transform parentTransform = go.transform.parent;
            if (parentTransform == null) return go;
            return TopParent(parentTransform.gameObject);
        }
        public static GameObject FindParentWithName(this GameObject go, string parentName)
        {
            if (go.name == parentName) return go;
            Transform parentTransform = go.transform.parent;
            if (parentTransform == null) return null;
            return FindParentWithName(parentTransform.gameObject, parentName);
        }
        public static GameObject FindMutualParent(this GameObject go1, GameObject go2)
        {
            if (go2 == null || go1 == go2) return null;
            Transform checkTrans = go2.transform;
            while (checkTrans != null)
            {
                if (go1 == checkTrans.gameObject) return go1;
                checkTrans = checkTrans.parent;
            }
            Transform parent1 = go1.transform.parent;
            if (parent1 == null) return null;
            return FindMutualParent(parent1.gameObject, go2);
        }

        public static GameObject FindFirstChildWithName(this GameObject go, string childName)
        {
            if (go.name == childName) return go;
            foreach (Transform child in go.transform)
            {
                GameObject childWithName = FindFirstChildWithName(child.gameObject, childName);
                if (childWithName != null) return childWithName;
            }
            return null;
        }
        public static bool IsChildWithNameUnique(this GameObject go, string childName)
        {
            int total = 0;
            CountChildrenWithName(go, childName, ref total);
            return (total <= 1);
        }
        public static void CountChildrenWithName(this GameObject go, string childName, ref int total)
        {
            if (go.name == childName) total++;
            foreach (Transform child in go.transform)
            {
                CountChildrenWithName(child.gameObject, childName, ref total);
            }
        }

        public static void DestroyChildren(this GameObject go, bool disabledOnly)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in go.transform)
            {
                if ((!child.gameObject.activeSelf) || (!disabledOnly)) children.Add(child);
            }
            // we do it this way because you cant alter the array in a foreach loop
            for (int i = children.Count - 1; i >= 0; i--)
            {
#if UNITY_4_3
					children[i].parent = null;
#elif UNITY_4_4
					children[i].parent = null;
#elif UNITY_4_5
					children[i].parent = null;
#else
                children[i].SetParent(null);
#endif
                GameObject.Destroy(children[i].gameObject);
            }
        }

        public static T GetFirstComponentInParents<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component != null) return component;
            if (go.transform.parent != null && go.transform.parent.gameObject != go) return GetFirstComponentInParents<T>(go.transform.parent.gameObject);
            return null;
        }

        public static T GetFirstComponentInChildren<T>(this GameObject go) where T : Component
        {
            T[] components = go.GetComponentsInChildren<T>();
            if (components != null && components.Length > 0) return components[0];
            return null;
        }

        public static Mesh[] GetMeshes(this GameObject aGo)
        {
            return GetMeshes(aGo, true);
        }
        public static Mesh[] GetMeshes(this GameObject aGo, bool includeDisabled)
        {
            MeshFilter[] mfs = aGo.GetComponentsInChildren<MeshFilter>(includeDisabled);
            SkinnedMeshRenderer[] smr = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
            int newLen = 0;
            if (mfs != null) newLen += mfs.Length;
            if (smr != null) newLen += smr.Length;
            if (newLen == 0) return null;
            Mesh[] meshes = new Mesh[newLen];
            int i = 0;
            for (; mfs != null && i < mfs.Length; i++)
            {
                meshes[i] = mfs[i].sharedMesh;
            }
            int offset = i;
            for (i = 0; smr != null && i < smr.Length; i++)
            {
                meshes[i + offset] = smr[i].sharedMesh;
            }
            return meshes;
        }

        public static int GetTotalVertexCount(this GameObject aGo)
        {
            MeshFilter[] mfs = aGo.GetComponentsInChildren<MeshFilter>(false);
            SkinnedMeshRenderer[] smr = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(false);
            int totalVertexCount = 0;
            for (int i = 0; mfs != null && i < mfs.Length; i++)
            {
                Mesh mesh = mfs[i].sharedMesh;
                if (mesh != null) totalVertexCount += mesh.vertexCount;
            }
            for (int i = 0; smr != null && i < smr.Length; i++)
            {
                Mesh mesh = smr[i].sharedMesh;
                if (mesh != null) totalVertexCount += mesh.vertexCount;
            }
            return totalVertexCount;
        }

        public static Mesh Get1stSharedMesh(this GameObject aGo)
        {
            MeshFilter[] mfs = aGo.GetComponentsInChildren<MeshFilter>(false);
            for (int i = 0; mfs != null && i < mfs.Length; i++)
            {
                if (mfs[i].sharedMesh != null) return mfs[i].sharedMesh;
            }
            SkinnedMeshRenderer[] smrs = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(false);
            for (int i = 0; smrs != null && i < smrs.Length; i++)
            {
                if (smrs[i].sharedMesh != null) return smrs[i].sharedMesh;
            }
            return null;
        }

        public static void SetMeshes(this GameObject aGo, Mesh[] meshes)
        {
            SetMeshes(aGo, meshes, true, 0);
        }
        public static void SetMeshes(this GameObject aGo, Mesh[] meshes, int lodLevel)
        {
            SetMeshes(aGo, meshes, true, lodLevel);
        }
        public static void SetMeshes(this GameObject aGo, Mesh[] meshes, bool includeDisabled, int lodLevel)
        {
            MeshFilter[] mfs = aGo.GetComponentsInChildren<MeshFilter>(includeDisabled);
            SkinnedMeshRenderer[] smr = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
            int meshLen = 0;
            if (mfs != null) meshLen += mfs.Length;
            if (smr != null) meshLen += smr.Length;
            if (meshLen == 0) return;
            int i = 0;
            for (; mfs != null && i < mfs.Length; i++)
            {
                LODSwitcher lodSwitcher = mfs[i].gameObject.GetComponent<LODSwitcher>();
                if (meshes != null && meshes.Length > i)
                {
                    if (lodLevel == 0) mfs[i].sharedMesh = meshes[i];
                    if (lodSwitcher == null && lodLevel > 0)
                    {
                        lodSwitcher = mfs[i].gameObject.AddComponent<LODSwitcher>();
                        lodSwitcher.SetMesh(mfs[i].sharedMesh, 0);
                    }
                    if (lodSwitcher != null) lodSwitcher.SetMesh(meshes[i], lodLevel);
                }
                else
                {
                    if (lodSwitcher != null) lodSwitcher.SetMesh(null, lodLevel);
                    if (lodLevel == 0) mfs[i].sharedMesh = null;
                }
            }
            int offset = i;
            for (i = 0; smr != null && i < smr.Length; i++)
            {
                LODSwitcher lodSwitcher = smr[i].gameObject.GetComponent<LODSwitcher>();
                if (meshes != null && meshes.Length > i + offset)
                {
                    if (lodLevel == 0) smr[i].sharedMesh = meshes[i + offset];
                    if (lodSwitcher == null && lodLevel > 0)
                    {
                        lodSwitcher = smr[i].gameObject.AddComponent<LODSwitcher>();
                        lodSwitcher.SetMesh(smr[i].sharedMesh, 0);
                    }
                    if (lodSwitcher != null) lodSwitcher.SetMesh(meshes[i + offset], lodLevel);
                }
                else
                {
                    if (lodSwitcher != null) lodSwitcher.SetMesh(null, lodLevel);
                    if (lodLevel == 0) smr[i].sharedMesh = null;
                }
            }
        }

        public static Material[] GetMaterials(this GameObject aGo, bool includeDisabled)
        {
            List<Material> materials = new List<Material>();
            MeshRenderer[] mrs = aGo.GetComponentsInChildren<MeshRenderer>(includeDisabled);
            for (int i = 0; i < mrs.Length; i++)
            {
                materials.AddRange(mrs[i].sharedMaterials);
            }
            SkinnedMeshRenderer[] smr = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
            for (int i = 0; i < smr.Length; i++)
            {
                materials.AddRange(smr[i].sharedMaterials);
            }
            return materials.ToArray();
        }

        // Unity Mesh.CombineMeshes function sucks. It makes 1 submesh for each mesh
        // but if the meshes already have submeshes, this info gets lost
        // My function combines all submeshes of all child gameObjects and merges them per unique material
        public static Mesh[] CombineMeshes(this GameObject aGO)
        {
            return CombineMeshes(aGO, new string[0] { });
        }
        public static Mesh[] CombineMeshes(this GameObject aGO, string[] skipSubmeshNames)
        {
            List<Mesh> meshes = new List<Mesh>();
            MeshRenderer[] meshRenderers = aGO.GetComponentsInChildren<MeshRenderer>(false);
            SkinnedMeshRenderer[] skinnedMeshRenderers = aGO.GetComponentsInChildren<SkinnedMeshRenderer>(false);
            int totalVertexCount = 0;
            int totalMeshCount = 0;
            int totalMeshCountAnyLightmapIdx = 0;
            int lightmapIndex = -999;
            bool makeNewGameObject = false;
            if (aGO.GetComponent<SkinnedMeshRenderer>() != null || aGO.GetComponent<MeshRenderer>() != null) makeNewGameObject = true;

            if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0)
            {
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    if (skinnedMeshRenderer.sharedMesh != null)
                    {
                        totalVertexCount += skinnedMeshRenderer.sharedMesh.vertexCount;
                        totalMeshCount++;
                        totalMeshCountAnyLightmapIdx++;
                    }
                }
            }
            if (meshRenderers != null && meshRenderers.Length > 0)
            {
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    MeshFilter filter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                    if (filter != null && filter.sharedMesh != null)
                    {
                        if (lightmapIndex == -999 && meshRenderer.lightmapIndex >= 0 && meshRenderer.lightmapIndex <= 253)
                        {
                            lightmapIndex = meshRenderer.lightmapIndex;
                        }
                        if (lightmapIndex < 0 || meshRenderer.lightmapIndex < 0 || meshRenderer.lightmapIndex > 253 || lightmapIndex == meshRenderer.lightmapIndex)
                        {
                            totalVertexCount += filter.sharedMesh.vertexCount;
                            totalMeshCount++;
                        }
                    }
                    totalMeshCountAnyLightmapIdx++;
                }
            }

            if (totalMeshCount == 0)
            {
                throw new ApplicationException("No meshes found in children. There's nothing to combine.");
                //				Debug.LogError("No meshes found in children. There's nothing to combine.");
                //				return null;
            }

            if (makeNewGameObject)
            {
                GameObject newGO = new GameObject();
                string newName = aGO.name + "_Merged";
                string newUniqueName = newName;
                int uniqueCounter = 0;
                while (GameObject.Find(newUniqueName) != null)
                {
                    newUniqueName = newName + "_" + uniqueCounter;
                    uniqueCounter++;
                }
                newGO.name = newUniqueName;
#if UNITY_4_3
					newGO.transform.parent = aGO.transform.parent;
#elif UNITY_4_4
					newGO.transform.parent = aGO.transform.parent;
#elif UNITY_4_5
					newGO.transform.parent = aGO.transform.parent;
#else
                newGO.transform.SetParent(aGO.transform.parent);
#endif
                newGO.transform.localPosition = aGO.transform.localPosition;
                newGO.transform.localRotation = aGO.transform.localRotation;
                newGO.transform.localScale = aGO.transform.localScale;
                aGO = newGO;
            }

            int partNr = 1;
            int prevMeshOffset = -1;
            int meshOffset = 0;
            while (meshOffset < totalMeshCount)
            {
                if (prevMeshOffset == meshOffset) break;
                prevMeshOffset = meshOffset;
                GameObject topGO = aGO;
                if (totalVertexCount > 65534)
                {
                    topGO = new GameObject();
                    topGO.name = "Merged part " + (partNr++);
#if UNITY_4_3
						topGO.transform.parent = aGO.transform;
#elif UNITY_4_4
						topGO.transform.parent = aGO.transform;
#elif UNITY_4_5
						topGO.transform.parent = aGO.transform;
#else
                    topGO.transform.SetParent(aGO.transform);
#endif
                    topGO.transform.localPosition = Vector3.zero;
                    topGO.transform.localRotation = Quaternion.identity;
                    topGO.transform.localScale = Vector3.one;
                }

                Mesh mesh = null;
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> uv1s = new List<Vector2>();
                List<Vector2> uv2s = new List<Vector2>();
                List<Vector2> uv3s = new List<Vector2>();
                List<Vector2> uv4s = new List<Vector2>();
                List<Color32> colors32 = new List<Color32>();
                List<Transform> bones = new List<Transform>();
                List<Matrix4x4> bindPoses = new List<Matrix4x4>();
                List<BoneWeight> boneWeights = new List<BoneWeight>();
                Dictionary<Material, List<int>> subMeshes = new Dictionary<Material, List<int>>();

                if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0)
                {
                    bool meshLimitReached = false;
                    bool hasSkinnedMesh = false;
                    int counter = 0;
                    int nrOfMeshWithBlendShapes = -1;
                    // First we process the first mesh that has blendshapes
                    int meshNr = 0;
                    foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                    {
                        if (skinnedMeshRenderer.sharedMesh != null)
                        {
                            if (vertices.Count + skinnedMeshRenderer.sharedMesh.vertexCount > 65534) meshLimitReached = true;
                            if (mesh == null && skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
                            {
                                if (meshOffset <= counter && (!meshLimitReached))
                                {
                                    // make copy to preserve blendshapes
                                    mesh = (Mesh)Mesh.Instantiate(skinnedMeshRenderer.sharedMesh);
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
                                    mesh.uv4 = null;
                                    mesh.uv3 = null;
#endif
                                    mesh.uv2 = null;
                                    mesh.uv2 = null;
                                    mesh.boneWeights = null;
                                    mesh.colors32 = null;
                                    mesh.triangles = null;
                                    mesh.tangents = null;
                                    mesh.normals = null;
                                    mesh.vertices = null;

                                    bool allSubmeshesMerged = MergeMeshInto(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.bones, skinnedMeshRenderer.sharedMaterials, vertices, normals, uv1s, uv2s, uv3s, uv4s, colors32, boneWeights, bones, bindPoses, subMeshes, ((skinnedMeshRenderer.transform.localScale.x * skinnedMeshRenderer.transform.localScale.y * skinnedMeshRenderer.transform.localScale.z) < 0f), new Vector4(1, 1, 0, 0), skinnedMeshRenderer.transform, topGO.transform, skinnedMeshRenderer.gameObject.name + "_" + skinnedMeshRenderer.sharedMesh.name, skipSubmeshNames);
                                    if (allSubmeshesMerged && skinnedMeshRenderer.gameObject != topGO)
                                    {
                                        skinnedMeshRenderer.gameObject.SetActive(false);
                                    }
                                    nrOfMeshWithBlendShapes = meshNr;
                                }
                            }
                            meshNr++;
                        }
                    }

                    // then we process all the other skinned meshes except the blendshaped one
                    foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
                    {
                        if (skinnedMeshRenderer.sharedMesh != null)
                        {
                            if (vertices.Count + skinnedMeshRenderer.sharedMesh.vertexCount > 65534) meshLimitReached = true;
                            if (meshOffset <= counter && (!meshLimitReached))
                            {
                                if (counter != nrOfMeshWithBlendShapes)
                                {
                                    bool allSubmeshesMerged = MergeMeshInto(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.bones, skinnedMeshRenderer.sharedMaterials, vertices, normals, uv1s, uv2s, uv3s, uv4s, colors32, boneWeights, bones, bindPoses, subMeshes, ((skinnedMeshRenderer.transform.localScale.x * skinnedMeshRenderer.transform.localScale.y * skinnedMeshRenderer.transform.localScale.z) < 0f), new Vector4(1, 1, 0, 0), skinnedMeshRenderer.transform, topGO.transform, skinnedMeshRenderer.gameObject.name + "_" + skinnedMeshRenderer.sharedMesh.name, skipSubmeshNames);
                                    if (allSubmeshesMerged && skinnedMeshRenderer.gameObject != topGO)
                                    {
                                        skinnedMeshRenderer.gameObject.SetActive(false);
                                    }
                                }
                                hasSkinnedMesh = true;
                                meshOffset++;
                            }
                            counter++;
                        }
                    }

                    // and then the non-skinned meshes that were attached to some bone
                    if (meshRenderers != null && meshRenderers.Length > 0 && hasSkinnedMesh)
                    {
                        foreach (MeshRenderer meshRenderer in meshRenderers)
                        {
                            MeshFilter filter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                            if (filter != null && filter.sharedMesh != null && filter.gameObject != topGO)
                            {
                                if (vertices.Count + filter.sharedMesh.vertexCount > 65534) meshLimitReached = true;
                                if (meshOffset <= counter && (!meshLimitReached))
                                {
                                    bool allSubmeshesMerged = MergeMeshInto(filter.sharedMesh, null, meshRenderer.sharedMaterials, vertices, normals, uv1s, uv2s, uv3s, uv4s, colors32, boneWeights, bones, bindPoses, subMeshes, ((filter.transform.localScale.x * filter.transform.localScale.y * filter.transform.localScale.z) < 0f), meshRenderer.lightmapScaleOffset, filter.transform, topGO.transform, filter.gameObject.name + "_" + filter.sharedMesh.name, skipSubmeshNames);
                                    if (allSubmeshesMerged)
                                    {
                                        // the gameObject has now become a bone, so I wll not disable it, but disable the renderer instead;
                                        meshRenderer.enabled = false;
                                    }
                                    meshOffset++;
                                }
                                counter++;
                            }
                        }
                    }
                }
                else if (meshRenderers != null && meshRenderers.Length > 0)
                {
                    int counter = 0;
                    foreach (MeshRenderer meshRenderer in meshRenderers)
                    {
                        if (lightmapIndex < 0 || meshRenderer.lightmapIndex < 0 || meshRenderer.lightmapIndex > 253 || lightmapIndex == meshRenderer.lightmapIndex)
                        {  // only merge objects with the same lightmapIndex
                            MeshFilter filter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                            if (filter != null && filter.sharedMesh != null)
                            {
                                if (meshOffset <= counter && vertices.Count + filter.sharedMesh.vertexCount <= 65534)
                                {
                                    bool allSubmeshesMerged = MergeMeshInto(filter.sharedMesh, null, meshRenderer.sharedMaterials, vertices, normals, uv1s, uv2s, uv3s, uv4s, colors32, boneWeights, bones, bindPoses, subMeshes, ((filter.transform.localScale.x * filter.transform.localScale.y * filter.transform.localScale.z) < 0f), meshRenderer.lightmapScaleOffset, filter.transform, topGO.transform, filter.gameObject.name + "_" + filter.sharedMesh.name, skipSubmeshNames);
                                    if (allSubmeshesMerged && filter.gameObject != topGO)
                                    {
                                        filter.gameObject.SetActive(false);

                                        // Also disable parent (go's with meshes often have empty parent transforms that we dont need anymore)
                                        Transform parentTransform = filter.gameObject.transform.parent;
                                        if (parentTransform != null && parentTransform.gameObject != topGO)
                                        {
                                            parentTransform.gameObject.SetActive(false);
                                        }
                                    }
                                    meshOffset++;
                                }
                                counter++;
                            }
                        }
                    }
                }

                // remove unused vertices
                LODMaker.RemoveUnusedVertices(vertices, normals, uv1s, uv2s, uv3s, uv4s, colors32, boneWeights, subMeshes);

                if (mesh == null) mesh = new Mesh();
                mesh.vertices = vertices.ToArray();
                bool setValues = false;
                if (normals.Count > 0) mesh.normals = normals.ToArray();

                setValues = false;
                for (int i = 0; i < uv1s.Count; i++)
                {
                    if (uv1s[i].x != 0f || uv1s[i].y != 0f)
                    {
                        setValues = true;
                        break;
                    }
                }
                if (setValues) mesh.uv = uv1s.ToArray();

                setValues = false;
                for (int i = 0; i < uv2s.Count; i++)
                {
                    if (uv2s[i].x != 0f || uv2s[i].y != 0f)
                    {
                        setValues = true;
                        break;
                    }
                }
                if (setValues) mesh.uv2 = uv2s.ToArray();

#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
                setValues = false;
                for (int i = 0; i < uv3s.Count; i++)
                {
                    if (uv3s[i].x != 0f || uv3s[i].y != 0f)
                    {
                        setValues = true;
                        break;
                    }
                }
                if (setValues) mesh.uv3 = uv3s.ToArray();

                setValues = false;
                for (int i = 0; i < uv4s.Count; i++)
                {
                    if (uv4s[i].x != 0f || uv4s[i].y != 0f)
                    {
                        setValues = true;
                        break;
                    }
                }
                if (setValues) mesh.uv4 = uv4s.ToArray();
#endif

                setValues = false;
                for (int i = 0; i < colors32.Count; i++)
                {
                    if (colors32[i].r > 0 || colors32[i].g > 0 || colors32[i].b > 0)
                    {
                        setValues = true;
                        break;
                    }
                }
                if (setValues) mesh.colors32 = colors32.ToArray();

                if (bindPoses.Count > 0) mesh.bindposes = bindPoses.ToArray();
                if (boneWeights.Count > 0)
                {
                    if (boneWeights.Count == vertices.Count) mesh.boneWeights = boneWeights.ToArray();
                    else Debug.LogWarning("Nr of bone weights not equal to nr of vertices.");
                }
                mesh.subMeshCount = subMeshes.Keys.Count;
                Material[] materials = new Material[subMeshes.Keys.Count];
                int mIdx = 0;
                foreach (Material m in subMeshes.Keys)
                {
                    materials[mIdx] = m;
                    mesh.SetTriangles(subMeshes[m].ToArray(), mIdx++);
                }

                if (normals == null || normals.Count <= 0) mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
                if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0)
                {
                    SkinnedMeshRenderer meshRend = topGO.GetComponent<SkinnedMeshRenderer>();

                    if (meshRend == null)
                    {
                        meshRend = topGO.AddComponent<SkinnedMeshRenderer>();
                    }
                    meshRend.quality = skinnedMeshRenderers[0].quality;
                    meshRend.sharedMesh = mesh;
                    meshRend.sharedMaterials = materials;
                    meshRend.bones = bones.ToArray();
                }
                else if (meshRenderers != null && meshRenderers.Length > 0)
                {
                    MeshRenderer meshRend = topGO.GetComponent<MeshRenderer>();
                    if (meshRend == null)
                    {
                        meshRend = topGO.AddComponent<MeshRenderer>();
                    }
                    if (lightmapIndex >= 0 && lightmapIndex <= 253) meshRend.lightmapIndex = lightmapIndex;
                    meshRend.sharedMaterials = materials;
                    MeshFilter meshFilter = topGO.GetComponent<MeshFilter>();
                    if (meshFilter == null)
                    {
                        meshFilter = topGO.AddComponent<MeshFilter>();
                    }
                    meshFilter.sharedMesh = mesh;
                }
                meshes.Add(mesh);
            }

            return meshes.ToArray();
        }

        private static int GiveUniqueNameIfNeeded(GameObject aGo, GameObject topGO, int uniqueId)
        {
            if (IsChildWithNameUnique(topGO, aGo.name)) return uniqueId;
            aGo.name = aGo.name + "_simpleLod" + (++uniqueId);
            return uniqueId;
        }

        public static void SetUpLODLevels(this GameObject go)
        {
            SetUpLODLevels(go, 1f);
        }

        public static void SetUpLODLevels(this GameObject go, float maxWeight)
        {
            SetUpLODLevels(go, new float[3] { 0.6f, 0.3f, 0.15f }, new float[3] { maxWeight * 0.65f, maxWeight, maxWeight * 1.5f });
        }

        public static void SetUpLODLevels(this GameObject go, float[] lodScreenSizes, float[] maxWeights)
        {
            MeshFilter[] mfs = go.GetComponentsInChildren<MeshFilter>(false);
            if (mfs == null || mfs.Length == 0) return;
            for (int i = 0; i < mfs.Length; i++)
            {
                SetUpLODLevelsWithLODSwitcher(mfs[i].gameObject, lodScreenSizes, maxWeights, true);
            }
        }

        public static Mesh[] SetUpLODLevelsWithLODSwitcher(this GameObject go, float[] lodScreenSizes, float[] maxWeights, bool recalcNormals, float removeSmallParts = 1f, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f, int nrOfSteps = 1)
        {
            Mesh mesh = null;
            LODSwitcher lodSwitcher = go.GetComponent<LODSwitcher>();
            if (lodSwitcher != null)
            {
                lodSwitcher.ReleaseFixedLODLevel();
                lodSwitcher.SetLODLevel(0);
            }

            SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
            if (smr != null) mesh = smr.sharedMesh;
            else
            {
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter != null) mesh = meshFilter.sharedMesh;
            }
            if (mesh == null)
            {
                throw new ApplicationException("No mesh found in " + go.name + ". Maybe you need to select a child object?");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f) throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            }

            Mesh[] lodMeshes = mesh.MakeLODMeshes(maxWeights, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst, nrOfSteps);
            if (lodMeshes == null) return null;

            if (lodSwitcher == null)
            {
                // add LODSwitcher component if needed
                lodSwitcher = go.AddComponent<LODSwitcher>();
            }
            Array.Resize(ref lodMeshes, maxWeights.Length + 1);
            for (int i = maxWeights.Length; i > 0; i--) lodMeshes[i] = lodMeshes[i - 1];
            lodMeshes[0] = mesh;

            lodSwitcher.lodMeshes = lodMeshes;
            lodSwitcher.lodScreenSizes = lodScreenSizes;
            lodSwitcher.ComputeDimensions();
            lodSwitcher.enabled = true;
            return lodMeshes;
        }

#if UNITY_WP8
#elif UNITY_WP_8_1
#elif UNITY_WSA
#elif UNITY_WSA_8_0
#elif UNITY_WSA_8_1
#elif UNITY_WINRT
#elif UNITY_WINRT_8_0
#elif UNITY_WINRT_8_1
#elif NETFX_CORE
#else
        public static IEnumerator SetUpLODLevelsWithLODSwitcherInBackground(this GameObject go, float[] lodScreenSizes, float[] maxWeights, bool recalcNormals, float removeSmallParts = 1f, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f)
        {
            yield return null;
            Mesh mesh = null;
            LODSwitcher lodSwitcher = go.GetComponent<LODSwitcher>();
            if (lodSwitcher != null)
            {
                lodSwitcher.ReleaseFixedLODLevel();
                lodSwitcher.SetLODLevel(0);
            }
            SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
            if (smr != null) mesh = smr.sharedMesh;
            else
            {
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter != null) mesh = meshFilter.sharedMesh;
            }

            if (mesh == null)
            {
                throw new ApplicationException("No mesh found in " + go.name + ". Maybe you need to select a child object?");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f) throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            }

            Mesh mesh0 = mesh;
            Mesh[] lodMeshes = new Mesh[maxWeights.Length];
            for (int i = 0; i < maxWeights.Length; i++)
            {
                yield return null;
                Hashtable lodInfo = new Hashtable();
                lodInfo["maxWeight"] = maxWeights[i];
                lodInfo["removeSmallParts"] = removeSmallParts;
                Vector3[] vs = mesh.vertices;
                if (vs.Length <= 0) throw new ApplicationException("Mesh was empty");
                Vector3[] ns = mesh.normals;
                if (ns.Length == 0)
                {  // mesh has no normals
                    mesh.RecalculateNormals();
                    ns = mesh.normals;
                }
                Vector2[] uv1s = mesh.uv;
                Vector2[] uv2s = mesh.uv2;
#if UNITY_4_3
					Vector2[] uv3s = new Vector2[0];
					Vector2[] uv4s = new Vector2[0];
#elif UNITY_4_4
					Vector2[] uv3s = new Vector2[0];
					Vector2[] uv4s = new Vector2[0];
#elif UNITY_4_5
					Vector2[] uv3s = new Vector2[0];
					Vector2[] uv4s = new Vector2[0];
#elif UNITY_4_6
					Vector2[] uv3s = new Vector2[0];
					Vector2[] uv4s = new Vector2[0];
#else
                Vector2[] uv3s = mesh.uv3;
                Vector2[] uv4s = mesh.uv4;
#endif
                Color32[] colors32 = mesh.colors32;
                int[] ts = mesh.triangles;
                Matrix4x4[] bindposes = mesh.bindposes;
                BoneWeight[] bws = mesh.boneWeights;
                int[] subMeshOffsets = new int[mesh.subMeshCount];
                if (mesh.subMeshCount > 1)
                {   // read triangles of submeshes 1 by 1 because I dont know the internal order of the mesh
                    for (int s = 0; s < mesh.subMeshCount; s++)
                    {
                        int[] subTs = mesh.GetTriangles(s);
                        int t = 0;
                        for (; t < subTs.Length; t++) ts[subMeshOffsets[s] + t] = subTs[t];
                        if (s + 1 < mesh.subMeshCount) subMeshOffsets[s + 1] = subMeshOffsets[s] + t;
                    }
                }
                Bounds meshBounds = mesh.bounds;
                lodInfo["vertices"] = vs;
                lodInfo["normals"] = ns;
                lodInfo["uv1s"] = uv1s;
                lodInfo["uv2s"] = uv2s;
                lodInfo["uv3s"] = uv3s;
                lodInfo["uv4s"] = uv4s;
                lodInfo["colors32"] = colors32;
                lodInfo["triangles"] = ts;
                lodInfo["bindposes"] = bindposes;
                lodInfo["boneWeights"] = bws;
                lodInfo["subMeshOffsets"] = subMeshOffsets;
                lodInfo["meshBounds"] = meshBounds;

                Thread thread = new Thread(LODMaker.MakeLODMeshInBackground);
                thread.Start(lodInfo);
                while (!lodInfo.ContainsKey("ready"))
                {
                    yield return new WaitForSeconds(0.2f);
                }

                lodMeshes[i] = LODMaker.CreateNewMesh((Vector3[])lodInfo["vertices"], (Vector3[])lodInfo["normals"], (Vector2[])lodInfo["uv1s"], (Vector2[])lodInfo["uv2s"], (Vector2[])lodInfo["uv3s"], (Vector2[])lodInfo["uv4s"], (Color32[])lodInfo["colors32"], (int[])lodInfo["triangles"], (BoneWeight[])lodInfo["boneWeights"], (Matrix4x4[])lodInfo["bindposes"], (int[])lodInfo["subMeshOffsets"], recalcNormals);
                mesh = lodMeshes[i];
                go.transform.parent.gameObject.BroadcastMessage("LOD" + (i + 1) + "IsReady", go, SendMessageOptions.DontRequireReceiver);
            }

            yield return null;
            if (lodMeshes != null)
            {
                if (lodSwitcher == null)
                {
                    // add LODSwitcher component if needed
                    lodSwitcher = go.AddComponent<LODSwitcher>();
                }
                Array.Resize(ref lodMeshes, maxWeights.Length + 1);
                for (int i = maxWeights.Length; i > 0; i--) lodMeshes[i] = lodMeshes[i - 1];
                lodMeshes[0] = mesh0;

                lodSwitcher.lodMeshes = lodMeshes;
                lodSwitcher.lodScreenSizes = lodScreenSizes;
                lodSwitcher.ComputeDimensions();
                lodSwitcher.enabled = true;
            }
            go.transform.parent.gameObject.BroadcastMessage("LODsAreReady", go, SendMessageOptions.DontRequireReceiver);
        }
#endif

        public static Mesh[] SetUpLODLevelsAndChildrenWithLODSwitcher(this GameObject go, float[] lodScreenSizes, float[] maxWeights, bool recalcNormals, float removeSmallParts, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f, int nrOfSteps = 1)
        {
            Mesh mesh = null;
            Material[] mats = null;
            LODSwitcher lodSwitcher = go.GetComponent<LODSwitcher>();
            if (lodSwitcher != null)
            {
                lodSwitcher.ReleaseFixedLODLevel();
                lodSwitcher.SetLODLevel(0);
            }
            SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                mesh = smr.sharedMesh;
                mats = smr.sharedMaterials;
                smr.enabled = false;
            }
            else
            {
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter != null) mesh = meshFilter.sharedMesh;
                MeshRenderer rend = go.GetComponent<MeshRenderer>();
                if (rend == null)
                {
                    throw new ApplicationException("No MeshRenderer found");
                }
                mats = rend.sharedMaterials;
                rend.enabled = false;
            }
            if (mesh == null)
            {
                throw new ApplicationException("No mesh found in " + go.name + ". Maybe you need to select a child object?");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f) throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            }

            Mesh[] lod1Meshes = mesh.MakeLODMeshes(maxWeights, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst, nrOfSteps);
            if (lod1Meshes == null) return null;
            Mesh[] lodMeshes = new Mesh[lod1Meshes.Length + 1];
            lodMeshes[0] = mesh;
            for (int i = 0; i < lod1Meshes.Length; i++) lodMeshes[i + 1] = lod1Meshes[i];

            if (lodSwitcher == null) lodSwitcher = go.AddComponent<LODSwitcher>();
            lodSwitcher.lodScreenSizes = lodScreenSizes;

            GameObject[] children = new GameObject[lodMeshes.Length];
            for (int i = 0; i < lodMeshes.Length; i++)
            {
                Transform subTrans = go.transform.FindFirstChildWithName(go.name + "_LOD" + i);
                if (subTrans != null)
                {
                    children[i] = subTrans.gameObject;
                    children[i].SetActive(true);
                }
                if (children[i] == null)
                {
                    children[i] = new GameObject(go.name + "_LOD" + i);
#if UNITY_4_3
						children[i].transform.parent = go.transform;
#elif UNITY_4_4
						children[i].transform.parent = go.transform;
#elif UNITY_4_5
						children[i].transform.parent = go.transform;
#else
                    children[i].transform.SetParent(go.transform);
#endif
                    children[i].transform.localPosition = Vector3.zero;
                    children[i].transform.localRotation = Quaternion.identity;
                    children[i].transform.localScale = Vector3.one;
                }
                MeshFilter filter = children[i].GetComponent<MeshFilter>();
                if (filter == null) filter = children[i].AddComponent<MeshFilter>();
                filter.sharedMesh = lodMeshes[i];
                MeshRenderer rend = children[i].GetComponent<MeshRenderer>();
                if (rend == null) rend = children[i].AddComponent<MeshRenderer>();
                rend.sharedMaterials = mats;
                children[i].SetActive(i == 0);
            }
            lodSwitcher.lodGameObjects = children;
            lodSwitcher.ComputeDimensions();
            lodSwitcher.enabled = true;
            return lodMeshes;
        }

        public static Mesh[] SetUpLODLevelsAndChildrenWithLODGroup(this GameObject go, float[] relativeTransitionHeights, float[] maxWeights, bool recalcNormals, float removeSmallParts, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f, int nrOfSteps = 1)
        {
            GameObject parentGO = null;
            LODGroup lodGroup = null;
            if (relativeTransitionHeights.Length < 0 || relativeTransitionHeights.Length != maxWeights.Length)
            {
                throw new ApplicationException("relativeTransitionHeights and maxWeights arrays need to have equal length and be longer than 0. Example: SetUpLODLevelsWithLODGroup(go, new float[2] {0.6f, 0.4f}, new float[2] {1f, 1.75f})");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f) throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            }

            parentGO = new GameObject(go.name + "_$LodGrp");
            parentGO.transform.position = go.transform.position;
            parentGO.transform.rotation = go.transform.rotation;
            parentGO.transform.localScale = go.transform.localScale;
#if UNITY_4_3
				go.transform.parent = parentGO.transform;
#elif UNITY_4_4
				go.transform.parent = parentGO.transform;
#elif UNITY_4_5
				go.transform.parent = parentGO.transform;
#else
            go.transform.SetParent(parentGO.transform);
#endif

            if (lodGroup == null)
            {
                // add LODGroup component if needed
                lodGroup = parentGO.AddComponent<LODGroup>();
            }
            else
            {
                // remove existing LOD gameObjects, because we will make new ones
                // let's hope no one will call their sub gameobjects <parent name>_$Lod:*
                Transform subObject = parentGO.transform.FindFirstChildWhereNameContains(go.name + "_$Lod:");
                int safetyCounter = 0;
                while (subObject != null && safetyCounter++ < 10)
                {
#if UNITY_4_3
						subObject.parent = null;
#elif UNITY_4_4
						subObject.parent = null;
#elif UNITY_4_5
						subObject.parent = null;
#else
                    subObject.SetParent(null);
#endif
                    UnityEngine.Object.Destroy(subObject.gameObject);
                    subObject = parentGO.transform.FindFirstChildWhereNameContains(go.name + "_$Lod:");
                }
            }

            // Create LODs
            LOD[] lods = new LOD[maxWeights.Length + 1];
            lods[0] = new LOD(relativeTransitionHeights[0], go.GetComponentsInChildren<MeshRenderer>(false));
            List<Mesh> lodMeshes = new List<Mesh>();
            Mesh[] meshesOrig = GetMeshes(go, false);
            float prevWeight = 0f;
            for (int l = 1; l < lods.Length; l++)
            {
                Mesh[] meshesLOD = new Mesh[meshesOrig.Length];
                for (int i = 0; i < meshesOrig.Length; i++)
                {
                    Mesh lodMesh = meshesOrig[i];
                    if (nrOfSteps < 1) nrOfSteps = 1;
                    for (int s = 0; s < nrOfSteps; s++)
                    {
                        float weight = (maxWeights[l - 1] - prevWeight);
                        lodMesh = lodMesh.MakeLODMesh(((float)(s + 1) * (weight / nrOfSteps)) + prevWeight, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst);
                    }
                    prevWeight = maxWeights[l - 1];
                    meshesLOD[i] = lodMesh;
                    lodMesh.name = go.name + "_" + i + "_LOD" + l;
                    lodMeshes.Add(lodMesh);
                }
                GameObject goLOD = (GameObject)GameObject.Instantiate(go);
                goLOD.name = go.name + "_$Lod:" + l;
#if UNITY_4_3
					goLOD.transform.parent = parentGO.transform;
#elif UNITY_4_4
					goLOD.transform.parent = parentGO.transform;
#elif UNITY_4_5
					goLOD.transform.parent = parentGO.transform;
#else
                goLOD.transform.SetParent(parentGO.transform);
#endif
                goLOD.transform.localPosition = go.transform.localPosition;
                goLOD.transform.localRotation = go.transform.localRotation;
                goLOD.transform.localScale = go.transform.localScale;
                SetMeshes(goLOD, meshesLOD);
                float transitionHeight = l < relativeTransitionHeights.Length ? relativeTransitionHeights[l] : 0f;
                lods[l] = new LOD(transitionHeight, goLOD.GetComponentsInChildren<MeshRenderer>(false));
                meshesOrig = meshesLOD;
            }

            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
            lodGroup.ForceLOD(-1);
            return lodMeshes.ToArray();
        }

        public static Mesh GetSimplifiedMesh(this GameObject go, float maxWeight, bool recalcNormals, float removeSmallParts, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f, int nrOfSteps = 1)
        {
            Mesh mesh = null;
            LODSwitcher lodSwitcher = go.GetComponent<LODSwitcher>();
            if (lodSwitcher != null)
            {
                lodSwitcher.ReleaseFixedLODLevel();
                lodSwitcher.SetLODLevel(0);
            }
            MeshFilter mf = null;
            SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
            if (maxWeight <= 0f) throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            if (smr != null) mesh = smr.sharedMesh;
            else
            {
                mf = go.GetComponent<MeshFilter>();
                if (mf != null) mesh = mf.sharedMesh;
            }
            if (mesh == null)
            {
                throw new ApplicationException("No mesh found. Maybe you need to select a child object?");
            }

            Mesh decimatedMesh = mesh;
            if (nrOfSteps < 1) nrOfSteps = 1;
            for (int i = 0; i < nrOfSteps; i++)
            {
                decimatedMesh = decimatedMesh.MakeLODMesh((float)(i + 1) * (maxWeight / nrOfSteps), recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst);
            }

            if (smr != null)
            {
                smr.sharedMesh = decimatedMesh;
            }
            else if (mf != null)
            {
                mf.sharedMesh = decimatedMesh;
            }
            return decimatedMesh;
        }

#if UNITY_WP8
#elif UNITY_WP_8_1
#elif UNITY_WSA
#elif UNITY_WSA_8_0
#elif UNITY_WSA_8_1
#elif UNITY_WINRT
#elif UNITY_WINRT_8_0
#elif UNITY_WINRT_8_1
#elif NETFX_CORE
#else
        public static IEnumerator GetSimplifiedMeshInBackground(this GameObject go, float maxWeight, bool recalcNormals, float removeSmallParts, System.Action<Mesh> result)
        {
            Mesh mesh = null;
            LODSwitcher lodSwitcher = go.GetComponent<LODSwitcher>();
            if (lodSwitcher != null)
            {
                lodSwitcher.ReleaseFixedLODLevel();
                lodSwitcher.SetLODLevel(0);
            }
            SkinnedMeshRenderer smr = go.GetComponent<SkinnedMeshRenderer>();
            if (maxWeight <= 0f) throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            if (smr != null) mesh = smr.sharedMesh;
            else
            {
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter != null) mesh = meshFilter.sharedMesh;
            }
            if (mesh == null)
            {
                throw new ApplicationException("No mesh found. Maybe you need to select a child object?");
            }

            Hashtable lodInfo = new Hashtable();
            lodInfo["maxWeight"] = maxWeight;
            lodInfo["removeSmallParts"] = removeSmallParts;
            Vector3[] vs = mesh.vertices;
            if (vs.Length <= 0) throw new ApplicationException("Mesh was empty");
            Vector3[] ns = mesh.normals;
            if (ns.Length == 0)
            {  // mesh has no normals
                mesh.RecalculateNormals();
                ns = mesh.normals;
            }
            Vector2[] uv1s = mesh.uv;
            Vector2[] uv2s = mesh.uv2;
#if UNITY_4_3
				Vector2[] uv3s = new Vector2[0];
				Vector2[] uv4s = new Vector2[0];
#elif UNITY_4_4
				Vector2[] uv3s = new Vector2[0];
				Vector2[] uv4s = new Vector2[0];
#elif UNITY_4_5
				Vector2[] uv3s = new Vector2[0];
				Vector2[] uv4s = new Vector2[0];
#elif UNITY_4_6
				Vector2[] uv3s = new Vector2[0];
				Vector2[] uv4s = new Vector2[0];
#else
            Vector2[] uv3s = mesh.uv3;
            Vector2[] uv4s = mesh.uv4;
#endif
            Color32[] colors32 = mesh.colors32;
            int[] ts = mesh.triangles;
            Matrix4x4[] bindposes = mesh.bindposes;
            BoneWeight[] bws = mesh.boneWeights;
            int[] subMeshOffsets = new int[mesh.subMeshCount];
            if (mesh.subMeshCount > 1)
            {   // read triangles of submeshes 1 by 1 because I dont know the internal order of the mesh
                for (int s = 0; s < mesh.subMeshCount; s++)
                {
                    int[] subTs = mesh.GetTriangles(s);
                    int t = 0;
                    for (; t < subTs.Length; t++) ts[subMeshOffsets[s] + t] = subTs[t];
                    if (s + 1 < mesh.subMeshCount) subMeshOffsets[s + 1] = subMeshOffsets[s] + t;
                }
            }
            Bounds meshBounds = mesh.bounds;
            lodInfo["vertices"] = vs;
            lodInfo["normals"] = ns;
            lodInfo["uv1s"] = uv1s;
            lodInfo["uv2s"] = uv2s;
            lodInfo["uv3s"] = uv3s;
            lodInfo["uv4s"] = uv4s;
            lodInfo["colors32"] = colors32;
            lodInfo["triangles"] = ts;
            lodInfo["bindposes"] = bindposes;
            lodInfo["boneWeights"] = bws;
            lodInfo["subMeshOffsets"] = subMeshOffsets;
            lodInfo["meshBounds"] = meshBounds;

            Thread thread = new Thread(LODMaker.MakeLODMeshInBackground);
            thread.Start(lodInfo);
            while (!lodInfo.ContainsKey("ready"))
            {
                yield return new WaitForSeconds(0.2f);
            }
            result(LODMaker.CreateNewMesh((Vector3[])lodInfo["vertices"], (Vector3[])lodInfo["normals"], (Vector2[])lodInfo["uv1s"], (Vector2[])lodInfo["uv2s"], (Vector2[])lodInfo["uv3s"], (Vector2[])lodInfo["uv4s"], (Color32[])lodInfo["colors32"], (int[])lodInfo["triangles"], (BoneWeight[])lodInfo["boneWeights"], (Matrix4x4[])lodInfo["bindposes"], (int[])lodInfo["subMeshOffsets"], recalcNormals));
        }
#endif

        private static bool MergeMeshInto(
            Mesh fromMesh,
            Transform[] fromBones,
            Material[] fromMaterials,
            List<Vector3> vertices,
            List<Vector3> normals,
            List<Vector2> uv1s,
            List<Vector2> uv2s,
            List<Vector2> uv3s,
            List<Vector2> uv4s,
            List<Color32> colors32,
            List<BoneWeight> boneWeights,
            List<Transform> bones,
            List<Matrix4x4> bindposes,
            Dictionary<Material, List<int>> subMeshes,
            bool usesNegativeScale,
            Vector4 lightmapScaleOffset,
            Transform fromTransform,
            Transform topTransform,
            string submeshName,
            string[] skipSubmeshNames)
        {
            if (fromMesh == null) return false;
            bool allSubmeshesMerged = true;
            int vertexOffset = vertices.Count;
            Vector3[] fromVertices = fromMesh.vertices;
            Vector3[] fromNormals = fromMesh.normals;
            Vector2[] fromUv1s = fromMesh.uv;
            Vector2[] fromUv2s = fromMesh.uv2;
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
            Vector2[] fromUv3s = fromMesh.uv3;
            Vector2[] fromUv4s = fromMesh.uv4;
#endif
            Color32[] fromColors32 = fromMesh.colors32;
            BoneWeight[] fromBoneWeights = fromMesh.boneWeights;
            Matrix4x4[] fromBindposes = fromMesh.bindposes;
            List<int> old2NewBoneIndex = new List<int>();

            Vector3 oldPos = fromTransform.localPosition;
            Quaternion oldRot = fromTransform.localRotation;
            Vector3 oldScale = fromTransform.localScale;
            bool remapVertices = false;

            /* 
            test with vertex colors
            fromColors32 = new Color32[fromVertices.Length];
            for(int i=0;i<fromVertices.Length;i++) {
                fromColors32[i] = new Color((fromVertices[i].x - fromMesh.bounds.min.x) / fromMesh.bounds.max.x,
                    (fromVertices[i].y - fromMesh.bounds.min.y) / fromMesh.bounds.max.y,
                    (fromVertices[i].z - fromMesh.bounds.min.z) / fromMesh.bounds.max.z
                    );
            }
            */
            if (fromBones != null)
            {
                fromTransform.localPosition = Vector3.zero;
                fromTransform.localRotation = Quaternion.identity;
                fromTransform.localScale = Vector3.one;
            }

            if (fromBones == null || fromBones.Length == 0) remapVertices = true;
            if ((fromBones == null || fromBones.Length == 0) && bones != null && bones.Count > 0)
            {  // merging a non-skinned mesh into a skinned mesh
                fromBones = new Transform[] { fromTransform };
                Matrix4x4 newBindPose = fromTransform.worldToLocalMatrix * topTransform.localToWorldMatrix;
                fromBindposes = new Matrix4x4[] { newBindPose };
                fromBoneWeights = new BoneWeight[fromVertices.Length];
                for (int i = 0; i < fromVertices.Length; i++)
                {
                    fromBoneWeights[i].boneIndex0 = 0;
                    fromBoneWeights[i].weight0 = 1;
                }
            }

            if (fromBones != null)
            {
                bool hasDifferentBindposes = false;
                for (int from = 0; from < fromBones.Length; from++)
                {
                    int to = 0;
                    old2NewBoneIndex.Add(from);
                    for (; to < bones.Count; to++)
                    {
                        if (fromBones[from] == bones[to])
                        {
                            old2NewBoneIndex[from] = to;
                            if (fromBindposes[from] != bindposes[to])
                            {
                                hasDifferentBindposes = true;
                                if (fromBones[from] != null)
                                {
                                    Debug.Log(fromTransform.gameObject.name + ": The bindpose of " + fromBones[from].gameObject.name + " is different, vertices will be moved to match the bindpose of the merged mesh");
                                }
                                else
                                {
                                    Debug.LogError(fromTransform.gameObject.name + ": There is an error in the bonestructure. A bone could not be found.");
                                }
                            }
                        }
                    }
                    if (to >= bones.Count)
                    { // bone not found
                        old2NewBoneIndex[from] = bones.Count;
                        bones.Add(fromBones[from]);
                        bindposes.Add(fromBindposes[from]);
                    }
                }

                if (hasDifferentBindposes)
                {
                    for (int i = 0; i < fromVertices.Length; i++)
                    {
                        Vector3 v = fromVertices[i];
                        BoneWeight bw = fromBoneWeights[i];
                        if (fromBones[bw.boneIndex0] != null)
                        {
                            // apply the old bindpose and inverse apply the new bindpose to put the vertex in the right position
                            v = ApplyBindPose(fromVertices[i], fromBones[bw.boneIndex0], fromBindposes[bw.boneIndex0], bw.weight0);
                            if (bw.weight1 > 0f) v = v + ApplyBindPose(fromVertices[i], fromBones[bw.boneIndex1], fromBindposes[bw.boneIndex1], bw.weight1);
                            if (bw.weight2 > 0f) v = v + ApplyBindPose(fromVertices[i], fromBones[bw.boneIndex2], fromBindposes[bw.boneIndex2], bw.weight2);
                            if (bw.weight3 > 0f) v = v + ApplyBindPose(fromVertices[i], fromBones[bw.boneIndex3], fromBindposes[bw.boneIndex3], bw.weight3);

                            Vector3 v0 = v;
                            v = UnApplyBindPose(v0, bones[old2NewBoneIndex[bw.boneIndex0]], bindposes[old2NewBoneIndex[bw.boneIndex0]], bw.weight0);
                            if (bw.weight1 > 0f) v = v + UnApplyBindPose(v0, bones[old2NewBoneIndex[bw.boneIndex1]], bindposes[old2NewBoneIndex[bw.boneIndex1]], bw.weight1);
                            if (bw.weight2 > 0f) v = v + UnApplyBindPose(v0, bones[old2NewBoneIndex[bw.boneIndex2]], bindposes[old2NewBoneIndex[bw.boneIndex2]], bw.weight2);
                            if (bw.weight3 > 0f) v = v + UnApplyBindPose(v0, bones[old2NewBoneIndex[bw.boneIndex3]], bindposes[old2NewBoneIndex[bw.boneIndex3]], bw.weight3);
                            fromVertices[i] = v;
                        }
                    }

                }
            }
            if (boneWeights != null && fromBoneWeights != null && fromBoneWeights.Length > 0)
            {
                for (int i = 0; i < fromBoneWeights.Length; i++)
                {
                    BoneWeight bw = new BoneWeight();
                    bw.boneIndex0 = old2NewBoneIndex[fromBoneWeights[i].boneIndex0];
                    bw.boneIndex1 = old2NewBoneIndex[fromBoneWeights[i].boneIndex1];
                    bw.boneIndex2 = old2NewBoneIndex[fromBoneWeights[i].boneIndex2];
                    bw.boneIndex3 = old2NewBoneIndex[fromBoneWeights[i].boneIndex3];
                    bw.weight0 = fromBoneWeights[i].weight0;
                    bw.weight1 = fromBoneWeights[i].weight1;
                    bw.weight2 = fromBoneWeights[i].weight2;
                    bw.weight3 = fromBoneWeights[i].weight3;
                    boneWeights.Add(bw);
                }
            }

            Matrix4x4 transformMatrix = topTransform.worldToLocalMatrix * fromTransform.localToWorldMatrix;
            if (remapVertices)
            {
                for (int i = 0; i < fromVertices.Length; i++)
                {
                    Vector3 v = fromVertices[i];
                    fromVertices[i] = transformMatrix.MultiplyPoint3x4(v);
                }
            }
            vertices.AddRange(fromVertices);

            Quaternion rotation = Quaternion.LookRotation(transformMatrix.GetColumn(2), transformMatrix.GetColumn(1));
            if (fromNormals != null && fromNormals.Length > 0)
            {
                for (int i = 0; i < fromNormals.Length; i++) fromNormals[i] = rotation * fromNormals[i];
                normals.AddRange(fromNormals);
            }

            if (fromUv1s == null || fromUv1s.Length != fromVertices.Length) fromUv1s = new Vector2[fromVertices.Length];
            if (fromUv1s != null && fromUv1s.Length > 0) uv1s.AddRange(fromUv1s);

            if (fromUv2s == null || fromUv2s.Length != fromVertices.Length) fromUv2s = new Vector2[fromVertices.Length];
            for (int i = 0; fromUv2s != null && i < fromUv2s.Length; i++)
            {
                uv2s.Add(new Vector2(lightmapScaleOffset.z + (fromUv2s[i].x * lightmapScaleOffset.x), lightmapScaleOffset.w + (fromUv2s[i].y * lightmapScaleOffset.y)));
            }
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
            if (fromUv3s == null || fromUv3s.Length != fromVertices.Length) fromUv3s = new Vector2[fromVertices.Length];
            if (fromUv3s != null && fromUv3s.Length > 0) uv3s.AddRange(fromUv3s);
            if (fromUv4s == null || fromUv4s.Length != fromVertices.Length) fromUv4s = new Vector2[fromVertices.Length];
            if (fromUv4s != null && fromUv4s.Length > 0) uv4s.AddRange(fromUv3s);
#endif
            if (fromColors32 == null || fromColors32.Length != fromVertices.Length) fromColors32 = new Color32[fromVertices.Length];
            if (fromColors32 != null && fromColors32.Length > 0) colors32.AddRange(fromColors32);

            int d = 0;
            for (int i = 0; i < fromMaterials.Length; i++)
            {
                if (i < fromMesh.subMeshCount)
                {
                    string thisSubmeshName = submeshName + "_" + i;
                    int j = 0;
                    for (; j < skipSubmeshNames.Length; j++)
                    {
                        if (thisSubmeshName == skipSubmeshNames[j]) break;
                    }
                    if (j >= skipSubmeshNames.Length)
                    {
                        int[] ts = fromMesh.GetTriangles(i);
                        if (ts.Length > 0)
                        {
                            if (fromMaterials[i] != null && !subMeshes.ContainsKey(fromMaterials[i]))
                            {
                                subMeshes.Add(fromMaterials[i], new List<int>());
                            }
                            List<int> subMesh = subMeshes[fromMaterials[i]];
                            for (int t = 0; t < ts.Length; t += 3)
                            {
                                if (usesNegativeScale)
                                {  // flip triangle
                                    int t1 = ts[t + 1];
                                    int t2 = ts[t + 2];
                                    ts[t + 1] = t2;
                                    ts[t + 2] = t1;
                                    d++;
                                }
                                ts[t] += vertexOffset;
                                ts[t + 1] += vertexOffset;
                                ts[t + 2] += vertexOffset;
                            }
                            subMesh.AddRange(ts);
                        }
                    }
                    else
                    {
                        allSubmeshesMerged = false;
                    }
                }
            }
            if (fromBones != null)
            {
                fromTransform.localPosition = oldPos;
                fromTransform.localRotation = oldRot;
                fromTransform.localScale = oldScale;
            }
            return allSubmeshesMerged;
        }

        private static Vector3 ApplyBindPose(Vector3 vertex, Transform bone, Matrix4x4 bindpose, float boneWeight)
        {
            Matrix4x4 newMatrix = (bone.localToWorldMatrix * bindpose);
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    newMatrix[r, c] *= boneWeight;
                }
            }
            return newMatrix.MultiplyPoint3x4(vertex);
        }
        private static Vector3 UnApplyBindPose(Vector3 vertex, Transform bone, Matrix4x4 bindpose, float boneWeight)
        {
            Matrix4x4 newMatrix = (bone.localToWorldMatrix * bindpose).inverse;
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    newMatrix[r, c] *= boneWeight;
                }
            }
            return newMatrix.MultiplyPoint3x4(vertex);
        }
    }
}
