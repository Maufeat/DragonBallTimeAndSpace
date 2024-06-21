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

namespace OrbCreationExtensions
{
    public static class MeshExtensions
    {

        public static void RecalculateTangents(this Mesh mesh)
        {
            int vertexCount = mesh.vertexCount;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] texcoords = mesh.uv;
            int[] triangles = mesh.triangles;
            int triangleCount = triangles.Length / 3;
            Vector4[] tangents = new Vector4[vertexCount];
            Vector3[] tan1 = new Vector3[vertexCount];
            Vector3[] tan2 = new Vector3[vertexCount];
            int tri = 0;
            if (texcoords.Length <= 0) return;
            for (int i = 0; i < (triangleCount); i++)
            {
                int i1 = triangles[tri];
                int i2 = triangles[tri + 1];
                int i3 = triangles[tri + 2];

                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];
                Vector3 v3 = vertices[i3];

                Vector2 w1 = texcoords[i1];
                Vector2 w2 = texcoords[i2];
                Vector2 w3 = texcoords[i3];

                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;

                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;

                float div = s1 * t2 - s2 * t1;
                float r = div == 0.0f ? 0.0f : 1.0f / div;
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;

                tri += 3;
            }

            for (int i = 0; i < (vertexCount); i++)
            {
                Vector3 n = normals[i];
                Vector3 t = tan1[i];

                // Gram-Schmidt orthogonalize
                Vector3.OrthoNormalize(ref n, ref t);

                tangents[i].x = t.x;
                tangents[i].y = t.y;
                tangents[i].z = t.z;

                // Calculate handedness
                tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f) ? -1.0f : 1.0f;
            }
            mesh.tangents = tangents;
        }

        public static Mesh ScaledRotatedTranslatedMesh(this Mesh mesh, Vector3 scale, Quaternion rotate, Vector3 translate)
        {
            Mesh newMesh = (Mesh)Mesh.Instantiate(mesh);
            Vector3[] vertices = newMesh.vertices;
            Vector3[] normals = newMesh.normals;
            bool rotateNormals = true;
            if (normals == null || normals.Length < vertices.Length || rotate == Quaternion.identity) rotateNormals = false;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x *= scale.x;
                vertices[i].y *= scale.y;
                vertices[i].z *= scale.z;
                vertices[i] = rotate * vertices[i];
                if (rotateNormals) normals[i] = rotate * normals[i];
                vertices[i] += translate;
            }
            newMesh.vertices = vertices;
            if (rotateNormals) newMesh.normals = normals;
            newMesh.RecalculateBounds();
            return newMesh;
        }

        public static bool IsSkinnedMesh(this Mesh mesh)
        {
            return mesh.blendShapeCount > 0 || (mesh.bindposes != null && mesh.bindposes.Length > 0);
        }
        public static int GetTriangleCount(this Mesh orig)
        {
            return orig.triangles.Length / 3;
        }
        public static Mesh MakeLODMesh(this Mesh orig, float aMaxWeight, bool recalcNormals, float removeSmallParts = 1f, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f)
        {
            return LODMaker.MakeLODMesh(orig, aMaxWeight, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst, recalcNormals);
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
        public static IEnumerator MakeLODMeshInBackground(this Mesh mesh, float maxWeight, bool recalcNormals, float removeSmallParts, System.Action<Mesh> result)
        {
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

        public static Mesh[] MakeLODMeshes(this Mesh mesh, float[] maxWeights, bool recalcNormals, float removeSmallParts = 1f, float protectNormals = 1f, float protectUvs = 1f, float protectSubMeshesAndSharpEdges = 1f, float smallTrianglesFirst = 1f, int nrOfSteps = 1)
        {
            if (maxWeights.Length < 1)
            {
                Debug.LogError("Mesh.GetLODLevelMeshes: maxWeights arrays is empty");
                return null;
            }

            // Create LODs
            Mesh[] lodMeshes = new Mesh[maxWeights.Length];
            float prevWeight = 0f;
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (nrOfSteps < 1) nrOfSteps = 1;
                for (int s = 0; s < nrOfSteps; s++)
                {
                    float weight = (maxWeights[i] - prevWeight);
                    mesh = mesh.MakeLODMesh(((float)(s + 1) * (weight / nrOfSteps)) + prevWeight, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst);
                }
                prevWeight = maxWeights[i];
                lodMeshes[i] = mesh;
            }
            return lodMeshes;
        }

        public static Vector4 GetUvRange(this Mesh mesh)
        {
            Vector4 range = new Vector4(0, 0, 1, 1);
            Vector2[] uvs = mesh.uv;
            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 uv = uvs[i];
                if (uv.x < range.x) range.x = uv.x;
                if (uv.y < range.y) range.y = uv.y;
                if (uv.x > range.z) range.z = uv.x;
                if (uv.y > range.w) range.w = uv.y;
            }
            return range;
        }

        public static bool CheckUvsWithin01Range(this Mesh mesh)
        {
            Vector2[] uvs = mesh.uv;
            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 uv = uvs[i];
                if (uv.x < 0f || uv.x > 1f || uv.y < 0f || uv.y > 1f) return false;
            }
            return true;
        }

        public static void ClampUvs(this Mesh mesh)
        {
            Vector2[] uvs = mesh.uv;
            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 uv = uvs[i];
                uv.x = Mathf.Clamp01(uv.x);
                uv.y = Mathf.Clamp01(uv.y);
                uvs[i] = uv;
            }
            mesh.uv = uvs;
        }
        public static void WrapUvs(this Mesh mesh)
        {
            Vector2[] uvs = mesh.uv;
            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 uv = uvs[i];
                while (uv.x > 1f) uv.x -= 1f;
                while (uv.x < 0f) uv.x += 1f;
                while (uv.y > 1f) uv.y -= 1f;
                while (uv.y < 0f) uv.y += 1f;
                uvs[i] = uv;
            }
            mesh.uv = uvs;
        }

        public static void SetAtlasRectForSubmesh(this Mesh mesh, Rect atlasRect, int submeshIndex)
        {
            if (submeshIndex >= mesh.subMeshCount) return;
            int[] tris = mesh.GetTriangles(submeshIndex);
            List<int> uvsToMove = new List<int>();
            for (int i = 0; i < tris.Length; i++)
            {
                int j = 0;
                for (; j < uvsToMove.Count; j++)
                {
                    if (uvsToMove[j] == tris[i]) break;
                }
                if (j >= uvsToMove.Count)
                {
                    uvsToMove.Add(tris[i]);
                }
            }

            Vector2[] uvs = mesh.uv;
            for (int i = 0; i < uvsToMove.Count; i++)
            {
                Vector2 uv = uvs[uvsToMove[i]];
                // when uv coordinates are outside of 0-1, the uv moves to a neighbouring texture
                uv.x = (Mathf.Clamp01(uv.x) * atlasRect.width) + atlasRect.x;
                uv.y = (Mathf.Clamp01(uv.y) * atlasRect.height) + atlasRect.y;
                uvs[uvsToMove[i]] = uv;
            }
            mesh.uv = uvs;
        }

        public static void MergeSubmeshInto(this Mesh mesh, int from, int to)
        {
            int[] fromTris = mesh.GetTriangles(from);
            int[] toTris = mesh.GetTriangles(to);
            List<int> newTris = new List<int>();
            for (int i = 0; i < toTris.Length; i++) newTris.Add(toTris[i]);
            for (int i = 0; i < fromTris.Length; i++) newTris.Add(fromTris[i]);
            mesh.SetTriangles(newTris.ToArray(), to);
            // move all submeshes 1 up 
            for (int i = from + 1; i < mesh.subMeshCount; i++)
            {
                mesh.SetTriangles(mesh.GetTriangles(i), i - 1);
            }
            List<int> temp = null;
            mesh.SetTriangles(temp, mesh.subMeshCount - 1);
            mesh.subMeshCount = mesh.subMeshCount - 1;
        }

        public static Mesh CopyAndRemoveSubmeshes(this Mesh orig, int[] submeshesToRemove)
        {
            Mesh mesh = (Mesh)Mesh.Instantiate(orig);
            List<List<int>> submeshes = new List<List<int>>(mesh.subMeshCount);
            List<Vector3> vs = new List<Vector3>(orig.vertices);
            List<Vector3> ns = new List<Vector3>(orig.vertexCount);
            List<Vector2> uv1s = new List<Vector2>(orig.vertexCount);
            List<Vector2> uv2s = new List<Vector2>();
            List<Vector2> uv3s = new List<Vector2>();
            List<Vector2> uv4s = new List<Vector2>();
            List<Color32> colors32 = new List<Color32>();
            List<BoneWeight> bws = new List<BoneWeight>();
            ns.AddRange(orig.normals);
            uv1s.AddRange(orig.uv);
            uv2s.AddRange(orig.uv2);
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
            uv3s.AddRange(orig.uv3);
            uv4s.AddRange(orig.uv4);
#endif
            colors32.AddRange(orig.colors32);
            bws.AddRange(orig.boneWeights);

            for (int s = 0; s < mesh.subMeshCount; s++)
            {
                bool remove = false;
                for (int i = 0; i < submeshesToRemove.Length; i++)
                {
                    if (submeshesToRemove[i] == s) remove = true;
                }
                if (!remove)
                {
                    List<int> submesh = new List<int>();
                    submesh.AddRange(mesh.GetTriangles(s));
                    submeshes.Add(submesh);
                }
            }

            LODMaker.RemoveUnusedVertices(vs, ns, uv1s, uv2s, uv3s, uv4s, colors32, bws, submeshes);

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
            mesh.normals = null;
            mesh.tangents = null;
            mesh.triangles = null;
            mesh.vertices = vs.ToArray();
            if (ns.Count > 0) mesh.normals = ns.ToArray();
            if (uv1s.Count > 0) mesh.uv = uv1s.ToArray();
            if (uv2s.Count > 0) mesh.uv2 = uv2s.ToArray();
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
            if (uv3s.Count > 0) mesh.uv3 = uv3s.ToArray();
            if (uv4s.Count > 0) mesh.uv4 = uv4s.ToArray();
#endif
            if (colors32.Count > 0) mesh.colors32 = colors32.ToArray();
            if (bws.Count > 0) mesh.boneWeights = bws.ToArray();

            mesh.subMeshCount = submeshes.Count;
            for (int s = 0; s < submeshes.Count; s++)
            {
                mesh.SetTriangles(submeshes[s].ToArray(), s);
            }

            if (ns == null || ns.Count <= 0) mesh.RecalculateNormals();
            RecalculateTangents(mesh);
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh CopyAndRemoveHiddenTriangles(this Mesh orig, int subMeshIdx, Matrix4x4 localToWorldMatrix, Mesh[] hidingMeshes, int[] hidingSubMeshes, Matrix4x4[] hidingLocalToWorldMatrices, float maxRemoveDistance = 0.01f)
        {
            if (subMeshIdx >= orig.subMeshCount) return null;
            if (hidingMeshes.Length <= 0) return null;
            if (hidingMeshes.Length != hidingSubMeshes.Length || hidingMeshes.Length != hidingLocalToWorldMatrices.Length) return null;
            // copy orig mesh and get everything in Lists
            Mesh mesh = (Mesh)Mesh.Instantiate(orig);
            List<List<int>> submeshes = new List<List<int>>(mesh.subMeshCount);
            List<Vector3> vs = new List<Vector3>(orig.vertices);
            List<Vector3> ns = new List<Vector3>(orig.vertexCount);
            List<Vector2> uv1s = new List<Vector2>(orig.vertexCount);
            List<Vector2> uv2s = new List<Vector2>();
            List<Vector2> uv3s = new List<Vector2>();
            List<Vector2> uv4s = new List<Vector2>();
            List<Color32> colors32 = new List<Color32>();
            List<BoneWeight> bws = new List<BoneWeight>();
            ns.AddRange(orig.normals);
            if (ns == null || ns.Count <= 0)
            {
                orig.RecalculateNormals();
                ns.AddRange(orig.normals);
            }
            uv1s.AddRange(orig.uv);
            uv2s.AddRange(orig.uv2);
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
            uv3s.AddRange(orig.uv3);
            uv4s.AddRange(orig.uv4);
#endif
            colors32.AddRange(orig.colors32);
            bws.AddRange(orig.boneWeights);
            for (int s = 0; s < orig.subMeshCount; s++)
            {
                List<int> submesh = new List<int>();
                submesh.AddRange(orig.GetTriangles(s));
                submeshes.Add(submesh);
            }
            List<int> ts = submeshes[subMeshIdx];

            // Make combined arrays for triangles/vertices/normals in hiding meshes
            List<Vector3> hidingVs = new List<Vector3>();
            List<int> hidingTs = new List<int>();
            Mesh prevMesh = null;
            int triOffset = 0;
            int prevHvsCount = 0;
            for (int h = 0; h < hidingMeshes.Length; h++)
            {
                Mesh hidingMesh = hidingMeshes[h];

                int[] hts = hidingMesh.GetTriangles(hidingSubMeshes[h]);
                if (prevMesh != hidingMesh) triOffset += prevHvsCount;
                for (int i = 0; i < hts.Length; i++)
                {
                    hidingTs.Add(hts[i] + triOffset);
                }

                if (prevMesh != hidingMesh)
                {
                    Matrix4x4 hidingMatrix = hidingLocalToWorldMatrices[h];
                    Vector3[] hvs = hidingMesh.vertices;
                    for (int i = 0; i < hvs.Length; i++)
                    {
                        hidingVs.Add(hidingMatrix.MultiplyPoint3x4(hvs[i]));  // vertex in world coordinates
                    }
                    prevHvsCount = hvs.Length;
                    prevMesh = hidingMesh;
                }
            }

            // get bounding box per triangle
            List<Vector3> triMinCorners = new List<Vector3>();
            List<Vector3> triMaxCorners = new List<Vector3>();
            for (int t = 0; t < hidingTs.Count; t += 3)
            {
                Vector3 v0 = hidingVs[hidingTs[t]];
                Vector3 v1 = hidingVs[hidingTs[t + 1]];
                Vector3 v2 = hidingVs[hidingTs[t + 2]];
                triMinCorners.Add(new Vector3(Mathf.Min(Mathf.Min(v0.x, v1.x), v2.x), Mathf.Min(Mathf.Min(v0.y, v1.y), v2.y), Mathf.Min(Mathf.Min(v0.z, v1.z), v2.z)));
                triMaxCorners.Add(new Vector3(Mathf.Max(Mathf.Max(v0.x, v1.x), v2.x), Mathf.Max(Mathf.Max(v0.y, v1.y), v2.y), Mathf.Max(Mathf.Max(v0.z, v1.z), v2.z)));
            }

            List<int> newTs = new List<int>();
            for (int t = 0; t < ts.Count; t += 3)
            {
                // we use vertices in world coordinates
                Vector3 v0 = localToWorldMatrix.MultiplyPoint3x4(vs[ts[t]]);
                Vector3 v1 = localToWorldMatrix.MultiplyPoint3x4(vs[ts[t + 1]]);
                Vector3 v2 = localToWorldMatrix.MultiplyPoint3x4(vs[ts[t + 2]]);
                if (!IsTriangleHidden(v0, v1, v2, maxRemoveDistance, triMinCorners, triMaxCorners, hidingVs, hidingTs))
                {
                    newTs.Add(ts[t]);
                    newTs.Add(ts[t + 1]);
                    newTs.Add(ts[t + 2]);
                }
            }

            // remove unused if anything deleted
            submeshes[subMeshIdx] = newTs;
            LODMaker.RemoveUnusedVertices(vs, ns, uv1s, uv2s, uv3s, uv4s, colors32, bws, submeshes);

            // Fill new mesh
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
            mesh.normals = null;
            mesh.tangents = null;
            mesh.triangles = null;
            mesh.vertices = vs.ToArray();
            if (ns.Count > 0) mesh.normals = ns.ToArray();
            if (uv1s.Count > 0) mesh.uv = uv1s.ToArray();
            if (uv2s.Count > 0) mesh.uv2 = uv2s.ToArray();
#if UNITY_4_5
#elif UNITY_4_6
#else
            if (uv3s.Count > 0) mesh.uv3 = uv3s.ToArray();
            if (uv4s.Count > 0) mesh.uv4 = uv4s.ToArray();
#endif
            if (colors32.Count > 0) mesh.colors32 = colors32.ToArray();
            if (bws.Count > 0) mesh.boneWeights = bws.ToArray();

            mesh.subMeshCount = submeshes.Count;
            for (int s = 0; s < submeshes.Count; s++)
            {
                if (s == subMeshIdx) mesh.SetTriangles(newTs.ToArray(), s);
                else mesh.SetTriangles(submeshes[s].ToArray(), s);
            }

            if (ns == null || ns.Count <= 0) mesh.RecalculateNormals();
            RecalculateTangents(mesh);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static bool IsTriangleHidden(Vector3 v0, Vector3 v1, Vector3 v2, float maxDistance, List<Vector3> triMinCorners, List<Vector3> triMaxCorners, List<Vector3> hidingVs, List<int> hidingTs)
        {
            Vector3 n = GetNormal(v0, v1, v2);  // compute the normal rather than using the existing one. Because I want it square to the triangle plane
            List<int> trianglesToCheck = GetTrianglesWithinRange(v0, v1, v2, maxDistance, triMinCorners, triMaxCorners);
            if (!IsHidden((v0 + v1 + v2) / 3f, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            if (!IsHidden(v0, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            if (!IsHidden(v1, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            if (!IsHidden(v2, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            return true;
        }
        private static bool IsHidden(Vector3 v, Vector3 n, float maxDistance, List<Vector3> hidingVs, List<int> hidingTs, List<int> trianglesToCheck)
        {

            for (int i = 0; i < trianglesToCheck.Count; i++)
            {
                int t = trianglesToCheck[i] * 3;
                Vector3 hc0 = hidingVs[hidingTs[t]];
                Vector3 hc1 = hidingVs[hidingTs[t + 1]];
                Vector3 hc2 = hidingVs[hidingTs[t + 2]];
                Vector3 hn = GetNormal(hc0, hc1, hc2);
                float angle = Vector3.Angle(n, hn);
                if (angle < 60f)
                {
                    float dist = DistanceToPlane(v, n, hc0, hn);
                    if (dist > 0f && dist <= maxDistance)
                    {
                        Vector3 point = v + (n * dist);
                        Vector2 bar = point.Barycentric(hc0, hc1, hc2);
                        if (bar.IsBarycentricInTriangle())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static List<int> GetTrianglesWithinRange(Vector3 v0, Vector3 v1, Vector3 v2, float maxDistance, List<Vector3> triMinCorners, List<Vector3> triMaxCorners)
        {
            List<int> idxs = new List<int>();
            Vector3 from = new Vector3(Mathf.Min(Mathf.Min(v0.x, v1.x), v2.x) - maxDistance, Mathf.Min(Mathf.Min(v0.y, v1.y), v2.y) - maxDistance, Mathf.Min(Mathf.Min(v0.z, v1.z), v2.z) - maxDistance);
            Vector3 to = new Vector3(Mathf.Max(Mathf.Max(v0.x, v1.x), v2.x) + maxDistance, Mathf.Max(Mathf.Max(v0.y, v1.y), v2.y) + maxDistance, Mathf.Max(Mathf.Max(v0.z, v1.z), v2.z) + maxDistance);
            for (int i = 0; i < triMaxCorners.Count; i++)
            {
                Vector3 maxCorner = triMaxCorners[i];
                if (maxCorner.x > from.x && maxCorner.y > from.y && maxCorner.z > from.z)
                {
                    Vector3 minCorner = triMinCorners[i];
                    if (minCorner.x < to.x && minCorner.y < to.y && minCorner.z < to.z)
                    {
                        idxs.Add(i);
                    }
                }
            }
            return idxs;
        }

        public static float DistanceToPlane(Vector3 fromPos, Vector3 direction, Vector3 pointOnPlane, Vector3 normalPlane)
        {
            float dist = Mathf.Infinity;
            float divideBy = direction.InProduct(normalPlane);
            if (divideBy != 0f) dist = (pointOnPlane - fromPos).InProduct(normalPlane) / direction.InProduct(normalPlane);
            return dist;
        }

        private static Vector3 GetNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            return Vector3.Cross(v1 - v0, v2 - v0).normalized;
        }

    }
}



