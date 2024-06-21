/* SimpleLOD 1.5     */
/* By Orbcreation BV */
/* Richard Knol      */
/* March 4, 2015      */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using OrbCreationExtensions;

using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class LODMaker
{

    public static Mesh MakeLODMesh(Mesh orig, float aMaxWeight, bool recalcNormals = true, float removeSmallParts = 1f, bool reuseOldMesh = false)
    {
        return MakeLODMesh(orig, aMaxWeight, removeSmallParts, 1f, 1f, 1f, 1f, recalcNormals, reuseOldMesh);
    }
    public static Mesh MakeLODMesh(Mesh orig, float aMaxWeight, float removeSmallParts, float protectNormals, float protectUvs, float smallTrianglesFirst, float protectSubMeshesAndSharpEdges, bool recalcNormals, bool reuseOldMesh = false)
    {
        if (!orig.isReadable)
        {
            UnityEngine.Debug.LogError("Sorry, mesh was not marked for read/write upon import");
            return orig;
        }
        float sideLengthWeight;
        float oldAngleWeight;
        float newAngleWeight;
        float uvWeight;
        float areaDiffWeight;
        float normalWeight;
        float vertexWeight;
        float centerDistanceWeight;
        GetWeights(aMaxWeight, removeSmallParts, protectNormals, protectUvs, smallTrianglesFirst, protectSubMeshesAndSharpEdges, out sideLengthWeight, out oldAngleWeight, out newAngleWeight, out uvWeight, out areaDiffWeight, out normalWeight, out vertexWeight, out centerDistanceWeight);

        Mesh mesh = MakeLODMesh(orig, aMaxWeight, removeSmallParts, sideLengthWeight, oldAngleWeight, newAngleWeight, uvWeight, areaDiffWeight, normalWeight, vertexWeight, centerDistanceWeight, recalcNormals, reuseOldMesh);
        return mesh;
    }

    private static void GetWeights(float aMaxWeight, float removeSmallParts, float protectNormals, float protectUvs, float smallTrianglesFirst, float protectSubMeshesAndSharpEdges, out float sideLengthWeight, out float oldAngleWeight, out float newAngleWeight, out float uvWeight, out float areaDiffWeight, out float normalWeight, out float vertexWeight, out float centerDistanceWeight)
    {
        // Set configuration weights, dont fiddle with these unless you test lots of different meshes
        float multiplier = (0.12f / (0.5f + aMaxWeight));  //0.09 / 2f + max
        sideLengthWeight = 3f * multiplier * smallTrianglesFirst;  // 6
        oldAngleWeight = 0.1f * multiplier * smallTrianglesFirst;  // 0.1 -  0.3
        newAngleWeight = 0.4f * multiplier;  // 0.4 - 1
        uvWeight = 800f * multiplier * protectUvs;   // 800
        areaDiffWeight = 10f * multiplier;   // 10
        normalWeight = 50f * multiplier * protectNormals;  // 50
        vertexWeight = 200f * multiplier * protectSubMeshesAndSharpEdges;  // 200
        centerDistanceWeight = 5000f * multiplier;  // 1000
    }

    public static void MakeLODMeshInBackground(object data)
    {
        string startTimeString = "start " + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff");
        Hashtable parameters = (Hashtable)data;
        float maxWeight = (float)parameters["maxWeight"];
        Vector3[] vs = (Vector3[])parameters["vertices"];
        int[] ts = (int[])parameters["triangles"]; ;
        int[] subMeshOffsets = (int[])parameters["subMeshOffsets"];
        Bounds meshBounds = (Bounds)parameters["meshBounds"];
        float removeSmallParts = 1f;
        float protectNormals = 1f;
        float protectUvs = 1f;
        float protectSubMeshesAndSharpEdges = 1f;
        float smallTrianglesFirst = 1f;
        Vector3[] ns = null;
        Vector2[] uv1s = null;
        Vector2[] uv2s = null;
        Vector2[] uv3s = null;
        Vector2[] uv4s = null;
        Color32[] colors32 = null;
        Matrix4x4[] bindposes = null;
        BoneWeight[] bws = null;
        if (parameters.ContainsKey("removeSmallParts")) removeSmallParts = (float)parameters["removeSmallParts"];
        if (parameters.ContainsKey("protectNormals")) protectNormals = (float)parameters["protectNormals"];
        if (parameters.ContainsKey("protectUvs")) protectUvs = (float)parameters["protectUvs"];
        if (parameters.ContainsKey("protectSubMeshesAndSharpEdges")) protectSubMeshesAndSharpEdges = (float)parameters["protectSubMeshesAndSharpEdges"];
        if (parameters.ContainsKey("smallTrianglesFirst")) smallTrianglesFirst = (float)parameters["smallTrianglesFirst"];
        if (parameters.ContainsKey("normals")) ns = (Vector3[])parameters["normals"];
        if (parameters.ContainsKey("uv1s")) uv1s = (Vector2[])parameters["uv1s"];
        if (parameters.ContainsKey("uv2s")) uv2s = (Vector2[])parameters["uv2s"];
        if (parameters.ContainsKey("uv3s")) uv3s = (Vector2[])parameters["uv3s"];
        if (parameters.ContainsKey("uv4s")) uv4s = (Vector2[])parameters["uv4s"];
        if (parameters.ContainsKey("colors32")) colors32 = (Color32[])parameters["colors32"];
        if (parameters.ContainsKey("bindposes")) bindposes = (Matrix4x4[])parameters["bindposes"];
        if (parameters.ContainsKey("boneWeights")) bws = (BoneWeight[])parameters["boneWeights"];

        float sideLengthWeight;
        float oldAngleWeight;
        float newAngleWeight;
        float uvWeight;
        float areaDiffWeight;
        float normalWeight;
        float vertexWeight;
        float centerDistanceWeight;
        GetWeights(maxWeight, removeSmallParts, protectNormals, protectUvs, smallTrianglesFirst, protectSubMeshesAndSharpEdges, out sideLengthWeight, out oldAngleWeight, out newAngleWeight, out uvWeight, out areaDiffWeight, out normalWeight, out vertexWeight, out centerDistanceWeight);

        List<Vector3> newVs;
        List<Vector3> newNs;
        List<Vector2> newUv1s;
        List<Vector2> newUv2s;
        List<Vector2> newUv3s;
        List<Vector2> newUv4s;
        List<Color32> newColors32;
        List<int> newTs;
        List<BoneWeight> newBws;

        MakeLODMesh(vs, ns, uv1s, uv2s, uv3s, uv4s, colors32, ts, ref bindposes, bws, ref subMeshOffsets, meshBounds, maxWeight, removeSmallParts, sideLengthWeight, oldAngleWeight, newAngleWeight, uvWeight, areaDiffWeight, normalWeight, vertexWeight, centerDistanceWeight, out newVs, out newNs, out newUv1s, out newUv2s, out newUv3s, out newUv4s, out newColors32, out newTs, out newBws);

        ((Hashtable)data)["vertices"] = newVs.ToArray();
        ((Hashtable)data)["normals"] = newNs.ToArray();
        ((Hashtable)data)["uv1s"] = newUv1s.ToArray();
        ((Hashtable)data)["uv2s"] = newUv2s.ToArray();
#if UNITY_4_3
			((Hashtable)data)["uv3s"] = new Vector2[0];
			((Hashtable)data)["uv4s"] = new Vector2[0];
#elif UNITY_4_4
			((Hashtable)data)["uv3s"] = new Vector2[0];
			((Hashtable)data)["uv4s"] = new Vector2[0];
#elif UNITY_4_5
			((Hashtable)data)["uv3s"] = new Vector2[0];
			((Hashtable)data)["uv4s"] = new Vector2[0];
#elif UNITY_4_6
			((Hashtable)data)["uv3s"] = new Vector2[0];
			((Hashtable)data)["uv4s"] = new Vector2[0];
#else
        ((Hashtable)data)["uv3s"] = newUv3s.ToArray();
        ((Hashtable)data)["uv4s"] = newUv4s.ToArray();
#endif
        ((Hashtable)data)["colors32"] = newColors32.ToArray();
        ((Hashtable)data)["triangles"] = newTs.ToArray();
        ((Hashtable)data)["bindposes"] = bindposes;
        ((Hashtable)data)["boneWeights"] = newBws.ToArray();
        ((Hashtable)data)["subMeshOffsets"] = subMeshOffsets;
        ((Hashtable)data)["ready"] = true;

        UnityEngine.Debug.Log("compression:" + maxWeight + ", vertices:" + vs.Length + " -> " + newVs.Count + ", triangles:" + (ts.Length / 3) + " -> " + (newTs.Count / 3) + "\n" + startTimeString + "\nended " + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff"));
    }

    private static Mesh MakeLODMesh(Mesh orig, float maxWeight, float removeSmallParts, float sideLengthWeight, float oldAngleWeight, float newAngleWeight, float uvWeight, float areaDiffWeight, float normalWeight, float vertexWeight, float centerDistanceWeight, bool recalcNormals, bool reuseOldMesh)
    {
        string startTimeString = "started " + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff");
        Vector3[] vs = orig.vertices;
        if (vs.Length <= 0)
        {
            Log("Mesh was empty");
            return orig;
        }
        Vector3[] ns = orig.normals;
        if (ns.Length == 0)
        {  // mesh has no normals
            orig.RecalculateNormals();
            ns = orig.normals;
        }
        Vector2[] uv1s = orig.uv;
        Vector2[] uv2s = orig.uv2;
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
        Vector2[] uv3s = orig.uv3;
        Vector2[] uv4s = orig.uv4;
#endif
        Color32[] colors32 = orig.colors32;
        int[] ts = orig.triangles;
        Matrix4x4[] bindposes = orig.bindposes;
        BoneWeight[] bws = orig.boneWeights;
        int[] subMeshOffsets = new int[orig.subMeshCount];
        if (orig.subMeshCount > 1)
        {   // read triangles of submeshes 1 by 1 because I dont know the internal order of the mesh
            for (int s = 0; s < orig.subMeshCount; s++)
            {
                int[] subTs = orig.GetTriangles(s);
                int t = 0;
                for (; t < subTs.Length; t++) ts[subMeshOffsets[s] + t] = subTs[t];
                if (s + 1 < orig.subMeshCount) subMeshOffsets[s + 1] = subMeshOffsets[s] + t;
            }
        }
        Bounds meshBounds = orig.bounds;

        List<Vector3> newVs;
        List<Vector3> newNs;
        List<Vector2> newUv1s;
        List<Vector2> newUv2s;
        List<Vector2> newUv3s;
        List<Vector2> newUv4s;
        List<Color32> newColors32;
        List<int> newTs;
        List<BoneWeight> newBws;

        MakeLODMesh(vs, ns, uv1s, uv2s, uv3s, uv4s, colors32, ts, ref bindposes, bws, ref subMeshOffsets, meshBounds, maxWeight, removeSmallParts, sideLengthWeight, oldAngleWeight, newAngleWeight, uvWeight, areaDiffWeight, normalWeight, vertexWeight, centerDistanceWeight, out newVs, out newNs, out newUv1s, out newUv2s, out newUv3s, out newUv4s, out newColors32, out newTs, out newBws);

        UnityEngine.Debug.Log("compression:" + maxWeight + ", vertices:" + vs.Length + " -> " + newVs.Count + ", triangles:" + (ts.Length / 3) + " -> " + (newTs.Count / 3) + "\n" + startTimeString + "\nended " + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff"));

        if (reuseOldMesh)
        {
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
            orig.uv4 = null;
            orig.uv3 = null;
#endif
            orig.uv2 = null;
            orig.uv = null;
            orig.colors = null;
            orig.tangents = null;
            orig.boneWeights = null;
            orig.bindposes = null;
            orig.triangles = null;
            orig.subMeshCount = 0;
            orig.normals = null;
            FillMesh(orig, newVs.ToArray(), newNs.ToArray(), newUv1s.ToArray(), newUv2s.ToArray(), newUv3s.ToArray(), newUv4s.ToArray(), newColors32.ToArray(), newTs.ToArray(), newBws.ToArray(), bindposes, subMeshOffsets, recalcNormals);
            return orig;
        }
        return CreateNewMesh(newVs.ToArray(), newNs.ToArray(), newUv1s.ToArray(), newUv2s.ToArray(), newUv3s.ToArray(), newUv4s.ToArray(), newColors32.ToArray(), newTs.ToArray(), newBws.ToArray(), bindposes, subMeshOffsets, recalcNormals);

    }

    private static void MakeLODMesh(Vector3[] vs, Vector3[] ns, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, int[] ts, ref Matrix4x4[] bindposes, BoneWeight[] bws, ref int[] subMeshOffsets, Bounds meshBounds, float maxWeight, float removeSmallParts, float sideLengthWeight, float oldAngleWeight, float newAngleWeight, float uvWeight, float areaDiffWeight, float normalWeight, float vertexWeight, float centerDistanceWeight, out List<Vector3> newVs, out List<Vector3> newNs, out List<Vector2> newUv1s, out List<Vector2> newUv2s, out List<Vector2> newUv3s, out List<Vector2> newUv4s, out List<Color32> newColors32, out List<int> newTs, out List<BoneWeight> newBws)
    {
        int logLevel = 1;
        //		Stopwatch stopWatch = new Stopwatch();
        //		stopWatch.Start();
        // Get mesh native size
        Vector3 meshSize = meshBounds.size;
        Vector3 meshCenter = meshBounds.center;
        Vector3 sizeMultiplier = Vector3.zero;  // this will be used to normalize the mesh size to 1 x 1 x 1 meter
        if (meshSize.x > 0f) sizeMultiplier.x = 1f / meshSize.x;
        if (meshSize.y > 0f) sizeMultiplier.y = 1f / meshSize.y;
        if (meshSize.z > 0f) sizeMultiplier.z = 1f / meshSize.z;
        // Additional values per vertex or per triangle that will be needed along the way 
        // to speed things up or store intermediate results
        List<List<int>> trianglesPerGroup = new List<List<int>>();
        int[] triangleGroups = new int[ts.Length / 3];
        List<List<int>> trianglesPerVertex = new List<List<int>>();
        int[] subMeshIdxPerVertex = new int[vs.Length];
        List<int> orderedVertices = new List<int>();
        Vector3[] centerDistances = new Vector3[vs.Length];   // distance from vertex to center of normalized mesh
        bool[] vdel = new bool[vs.Length];  // array that marks which vertices are deleted
        bool[] hasTwinVs = new bool[vs.Length];  // which vertices have a twin vertex in the exact same location
        int[] movedVs = new int[vs.Length]; // pointer to itself or another vertex who's position should be used
        int[] uniqueVs = new int[vs.Length];
        int[] movedUv1s = new int[uv1s.Length]; // pointer to itself or another uv who's position should be used
        int[] movedUv2s = new int[uv2s.Length]; // pointer to itself or another uv who's position should be used
        int[] movedUv3s = new int[uv3s.Length]; // pointer to itself or another uv who's position should be used
        int[] movedUv4s = new int[uv4s.Length]; // pointer to itself or another uv who's position should be used
        int[] movedColors = new int[colors32.Length]; // pointer to itself or another uv who's position should be used
        float[] vertexWeights = new float[vs.Length]; // additional weight per vertex to prevent messing up the uv map or normals
                                                      // initialize helper arrays with default empty values

        for (int i = 0; i < vs.Length; i++)
        {
            vdel[i] = false;
            movedVs[i] = i;
            uniqueVs[i] = i;
            trianglesPerVertex.Add(new List<int>());
            subMeshIdxPerVertex[i] = -1;
        }
        for (int i = 0; i < uv1s.Length; i++)
        {
            movedUv1s[i] = i;
        }
        for (int i = 0; i < uv2s.Length; i++)
        {
            movedUv2s[i] = i;
        }
        for (int i = 0; i < uv3s.Length; i++)
        {
            movedUv3s[i] = i;
        }
        for (int i = 0; i < uv4s.Length; i++)
        {
            movedUv4s[i] = i;
        }
        for (int i = 0; i < colors32.Length; i++)
        {
            movedColors[i] = i;
        }
        for (int i = 0; i < triangleGroups.Length; i++)
        {
            triangleGroups[i] = -1;
        }

        // round all vertices to 4 digits to eliminate small discrepancies
        float x = Mathf.Round(meshBounds.size.x * 10000f);
        float y = Mathf.Round(meshBounds.size.y * 10000f);
        float z = Mathf.Round(meshBounds.size.z * 10000f);
        if (x <= 0f) x = 1f;
        if (y <= 0f) y = 1f;
        if (z <= 0f) z = 1f;
        for (int i = 0; i < vs.Length; i++)
        {
            vs[i].x = Mathf.Round(vs[i].x * x) / x;
            vs[i].y = Mathf.Round(vs[i].y * y) / y;
            vs[i].z = Mathf.Round(vs[i].z * z) / z;
        }

        // build ordered list of vertices ordered by y, z, x
        for (int i = 0; i < vs.Length; i++)
        {
            int j = GetLastVertexWithYSmaller(vs[i].y, orderedVertices, vs, i);
            for (j++; j < orderedVertices.Count; j++)
            {
                if (orderedVertices[j] < 0) break;
                else if (vs[orderedVertices[j]].y > vs[i].y) break;
                else if (vs[orderedVertices[j]].y == vs[i].y)
                {
                    if (vs[orderedVertices[j]].z > vs[i].z) break;
                    else if (vs[i].z == vs[orderedVertices[j]].z)
                    {
                        if (vs[orderedVertices[j]].x > vs[i].x) break;
                    }
                }
            }
            orderedVertices.Insert(j, i);
        }

        // check if the vertex occurs in multiple submeshes
        int subMeshIdx = -1;
        for (int i = 0; i < ts.Length; i++)
        {
            if (i + 1 < subMeshOffsets.Length && i >= subMeshOffsets[subMeshIdx + 1]) subMeshIdx++;
            int vertexSubMeshIdx = subMeshIdxPerVertex[ts[i]];
            if (vertexSubMeshIdx < 0) subMeshIdxPerVertex[ts[i]] = subMeshIdx;
            else if (vertexSubMeshIdx != subMeshIdx)
            {
                vertexWeights[ts[i]] += vs.Length;  // add a massive additional weight, because vertex appears in multiple submeshes
            }
        }

        // Find double vertices and mark one as the main (uniqueVs)
        for (int i = 0; i < vs.Length; i++)
        {
            centerDistances[i] = (vs[i] - meshCenter).Product(sizeMultiplier);  // normalized distance to center
            List<int> doubles = GetVerticesEqualTo(vs[i], orderedVertices, vs);
            if (doubles.Count > 1)
            {
                hasTwinVs[i] = true;
                for (int j = 0; j < doubles.Count; j++)
                {
                    if (j != i)
                    {
                        if (uniqueVs[doubles[j]] == doubles[j]) uniqueVs[doubles[j]] = uniqueVs[i];
                        // check if normals are different
                        if (ns != null && ns.Length != 0 && (ns[doubles[0]].x != ns[doubles[j]].x || ns[doubles[0]].y != ns[doubles[j]].y || ns[doubles[0]].z != ns[doubles[j]].z))
                        {
                            vertexWeights[i] += normalWeight * 0.05f;  // add an additional weight to both
                            vertexWeights[doubles[j]] += normalWeight * 0.05f;  // was 0.25f absolute
                        }
                        // check if uv1s are different
                        if (uv1s != null && uv1s.Length != 0 && (uv1s[doubles[0]].x != uv1s[doubles[j]].x || uv1s[doubles[0]].y != uv1s[doubles[j]].y))
                        {
                            vertexWeights[i] += uvWeight * 0.02f;  // add an additional weight to both
                            vertexWeights[doubles[j]] += uvWeight * 0.02f;  // was 1.5f
                        }
                        // check if uv2s are different
                        if (uv2s != null && uv2s.Length != 0 && (uv2s[doubles[0]].x != uv2s[doubles[j]].x || uv2s[doubles[0]].y != uv2s[doubles[j]].y))
                        {
                            vertexWeights[i] += uvWeight * 0.013f;  // add an additional weight to both
                            vertexWeights[doubles[j]] += uvWeight * 0.013f;  // was 1f
                        }
                    }
                }
            }
        }

        // Build lookup list trianglesPerVertex 
        for (int i = 0; i < ts.Length; i++)
        {
            trianglesPerVertex[uniqueVs[ts[i]]].Add(i / 3);
        }

        // a simple mesh needs less optimizing. We reduce the maxWeight to prevent unwanted stripping
        float aMaxWeight = maxWeight * (0.8f + ((vs.Length / 65536f) * 0.2f));

        float avgNormArea = 0f;
        int count = 0;
        for (int i = 0; i < ts.Length; i += 3)
        {
            int t0 = ts[i];  // read 3 triangle corners
            int t1 = ts[i + 1];
            int t2 = ts[i + 2];
            if (t0 != t1 && t1 != t2 && t2 != t0)
            {  // triangle is not flattened yet
                avgNormArea += Area(
                    vs[movedVs[t0]].Product(sizeMultiplier),
                    vs[movedVs[t1]].Product(sizeMultiplier),
                    vs[movedVs[t2]].Product(sizeMultiplier));  // normalized area of triangle
                count++;
            }
        }
        if (count > 0) avgNormArea = avgNormArea / count;

        int[] cornerToIdxs = new int[6] { 1, 0, 2, 1, 0, 2 };  // declare simple helper arrays
        int[] cornerFromIdxs = new int[6] { 0, 1, 1, 2, 2, 0 };
        for (int pass = 0; pass < 3; pass++)
        {
            float normAreaFrom = avgNormArea * pass * 0.5f;
            float normAreaTo = avgNormArea * (pass + 1) * 0.5f;
            if (pass >= 2) normAreaTo = Mathf.Infinity;
            for (int i = 0; i < ts.Length; i += 3)
            {
                int t0 = ts[i];  // read 3 triangle corners
                int t1 = ts[i + 1];
                int t2 = ts[i + 2];
                if (t0 != t1 && t1 != t2 && t2 != t0)
                {  // triangle is not flattened yet
                   // 6 options to flatten 0->1, 1->0, 1->2, 2->1, 2->0, 0->2
                    int[] cornerFrom = new int[6] { t0, t1, t1, t2, t2, t0 };
                    int[] cornerTo = new int[6] { t1, t0, t2, t1, t0, t2 };
                    float[] weights = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
                    float[] normSideLengths = new float[3] {  // normalized length of triangle sides
						(vs[movedVs[t0]] - vs[movedVs[t1]]).Product(sizeMultiplier).magnitude,
                        (vs[movedVs[t1]] - vs[movedVs[t2]]).Product(sizeMultiplier).magnitude,
                        (vs[movedVs[t2]] - vs[movedVs[t0]]).Product(sizeMultiplier).magnitude
                    };
                    float normArea = Area(
                        vs[movedVs[t0]].Product(sizeMultiplier),
                        vs[movedVs[t1]].Product(sizeMultiplier),
                        vs[movedVs[t2]].Product(sizeMultiplier));  // normalized area of triangle
                    if (normArea < normAreaFrom || normArea >= normAreaTo) continue;
                    normArea = Mathf.Sqrt(normArea);

                    for (int j = 0; j < 6; j++)
                    {
                        weights[j] += normArea * sideLengthWeight;  // bigger area -> more weight
                    }
                    for (int j = 0; j < 6; j++)
                    {
                        weights[j] += normSideLengths[j / 2] * sideLengthWeight; // bigger side length -> more weight
                    }
                    for (int j = 0; j < 6; j++)
                    {
                        weights[j] += vertexWeights[cornerFrom[j]] * normArea * vertexWeight; // add vertex weights
                    }
                    for (int j = 0; j < 6; j++)
                    {
                        if ((j / 2) * 2 == j)
                        {   // compute difference in normals between 3 corner pairs
                            float weight = GetNormalDiffForCorners(ns, cornerFrom[(j / 2) * 2], cornerTo[(j / 2) * 2])
                                * normSideLengths[j / 2] * normalWeight; // bigger difference in normals -> more weight
                            weights[j] += weight;
                            weights[j + 1] += weight;
                        }
                    }

                    int[] allAdjacentTriangles = GetAdjacentTriangles(ts, i, trianglesPerVertex, uniqueVs, triangleGroups, trianglesPerGroup);

                    if (AnyWeightOK(weights, aMaxWeight))
                    {
                        float[] totalAngles = new float[3] { 0f, 0f, 0f };  // sum of corner angles of all triangles per corner. 0 = spike, 180 = flat side, 360 is somewhere inside the mesh
                        Vector3[] totalCenterDist = new Vector3[3];  // sum of distances to center of all corners of adjacent triangles per corner
                        for (int j = 0; j < 3; j++) GetTotalAngleAndCenterDistanceForCorner(allAdjacentTriangles, vs, movedVs, cornerFrom[j * 2], centerDistances, ref totalAngles[j], ref totalCenterDist[j]);  // we compute them in 1 go to prevent searching the triangles twice

                        // This step prevents making dents in the sides of a mesh, or chopping of corners and shrinking the visible size
                        // Its a first rough exclusion of triangle sides
                        // Weights for angles depend on sidelength and total area
                        for (int j = 0; j < 6; j++)
                        {
                            if (weights[j] < aMaxWeight) weights[j] += AngleDiff(totalAngles[cornerFromIdxs[j]]) * normSideLengths[j / 2] * normArea * oldAngleWeight;
                        }

                        if (AnyWeightOK(weights, aMaxWeight))
                        {  // no heavy operations if the triangle is spared anyway
                            for (int j = 0; j < 6; j++)
                            {
                                if (weights[j] < aMaxWeight)
                                {
                                    float newTotalAngle = 0f;
                                    Vector3 newTotalCenterDist = Vector3.zero;
                                    bool createsFlippedTriangles = false;

                                    GetTotalAngleAndCenterDistanceForNewCorner(allAdjacentTriangles, vs, movedVs, uniqueVs, cornerFrom[j], cornerTo[j], centerDistances, maxWeight, ref newTotalAngle, ref newTotalCenterDist, ref createsFlippedTriangles);

                                    // sometimes flattening triangles will cause other triangles to flip upside down. We dont want that
                                    if (createsFlippedTriangles) weights[j] += 100f * normArea;  // was 50

                                    // This step also prevents making dents in the sides of a mesh, or chopping of corners and shrinking the visible size
                                    // Now we examine the difference in total angle before and after. Weighed by sidelength only
                                    if (Mathf.Abs(newTotalAngle) < 10f) weights[j] += AngleCornerDiff(newTotalAngle - totalAngles[cornerToIdxs[j]]) * Mathf.Sqrt(normSideLengths[j / 2]) * newAngleWeight;
                                    else weights[j] += AngleDiff(newTotalAngle - totalAngles[cornerToIdxs[j]]) * Mathf.Sqrt(normSideLengths[j / 2]) * newAngleWeight;

                                    if (ns != null && ns.Length > 0)
                                    {
                                        // We project the movement of the corner on the normal. 
                                        // If you move a triangle corner in the direction of the normal, the mesh shape will change
                                        weights[j] += Vector3.Project(vs[movedVs[cornerFrom[j]]] - vs[movedVs[cornerTo[j]]], ns[cornerFrom[j]]).magnitude * (newTotalCenterDist - totalCenterDist[cornerToIdxs[j]]).magnitude * centerDistanceWeight;
                                    }
                                }
                            }

                            if (AnyWeightOK(weights, aMaxWeight))
                            {  // no heavy operations if the triangle is spared anyway
                                float area = Area(vs[movedVs[t0]], vs[movedVs[t1]], vs[movedVs[t2]]); // not the normalized area
                                for (int j = 0; j < 6; j++)
                                {
                                    if (weights[j] < aMaxWeight)
                                    {
                                        float affectedAreaDiff = 0f;
                                        float affectedUvAreaDiff = 0f;
                                        float totalAreaDiff = 0f;
                                        float totalUvAreaDiff = 0f;
                                        GetUVStretchAndAreaForCorner(allAdjacentTriangles, vs, movedVs, uniqueVs, uv1s, cornerFrom[j], cornerTo[j], ref affectedUvAreaDiff, ref affectedAreaDiff, ref totalUvAreaDiff, ref totalAreaDiff);
                                        // Comparing uv area of all affected triangles before and after will prevent streching the uv map too much
                                        weights[j] += affectedUvAreaDiff * 10f * uvWeight;
                                        weights[j] += (Mathf.Pow(Mathf.Abs(totalUvAreaDiff) + 1f, 2f) - 1f) * 30f * uvWeight;

                                        // Comparing absolute area of all affected triangles before and after will prevent making gaps in the mesh
                                        if (area <= 0f) area = Mathf.Max(affectedAreaDiff, totalAreaDiff);
                                        if (area > 0f)
                                        {
                                            if ((affectedAreaDiff / area) > 1f) weights[j] += (affectedAreaDiff / area) * 0.5f * areaDiffWeight;
                                            weights[j] += (Mathf.Pow(Mathf.Abs(totalAreaDiff / area) + 1f, 2f) - 1f) * 0.5f * areaDiffWeight;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (AnyWeightOK(weights, aMaxWeight))
                    {
                        // Max sure we prefer to flatten the shortest side
                        for (int j = 0; j < 6; j++) weights[j] *= 0.05f + (0.95f * normSideLengths[j / 2]);

                        int minWeightIdx = -1;  // find the flattening option with the smallest weight
                        float mw = Mathf.Infinity;
                        for (int j = 0; j < 6; j++)
                        {
                            if (weights[j] < mw)
                            {
                                mw = weights[j];
                                minWeightIdx = j;
                            }
                        }

                        switch (minWeightIdx)
                        {  // flatten the triangle
                            case 0:
                                MergeVertices(ref t0, t1, hasTwinVs, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, vdel, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors, trianglesPerVertex, logLevel > 1);
                                break;
                            case 1:
                                MergeVertices(ref t1, t0, hasTwinVs, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, vdel, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors, trianglesPerVertex, logLevel > 1);
                                break;
                            case 2:
                                MergeVertices(ref t1, t2, hasTwinVs, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, vdel, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors, trianglesPerVertex, logLevel > 1);
                                break;
                            case 3:
                                MergeVertices(ref t2, t1, hasTwinVs, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, vdel, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors, trianglesPerVertex, logLevel > 1);
                                break;
                            case 4:
                                MergeVertices(ref t2, t0, hasTwinVs, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, vdel, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors, trianglesPerVertex, logLevel > 1);
                                break;
                            case 5:
                                MergeVertices(ref t0, t2, hasTwinVs, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, vdel, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors, trianglesPerVertex, logLevel > 1);
                                break;
                        }
                    }
                }
            }
        }

        newVs = new List<Vector3>();
        newNs = new List<Vector3>();
        newUv1s = new List<Vector2>();
        newUv2s = new List<Vector2>();
        newUv3s = new List<Vector2>();
        newUv4s = new List<Vector2>();
        newColors32 = new List<Color32>();
        newTs = new List<int>();
        List<int> newTGrps = new List<int>();
        newBws = new List<BoneWeight>();
        int[] o2n = new int[vs.Length];  // old vertex idx points to new vertex idx
        FillNewMeshArray(vs, vdel, movedVs, ns, uv1s, movedUv1s, uv2s, movedUv2s, uv3s, movedUv3s, uv4s, movedUv4s, colors32, movedColors, bws, newVs, newNs, newUv1s, newUv2s, newUv3s, newUv4s, newColors32, newBws, o2n);
        FillNewMeshTriangles(ts, o2n, newTs, subMeshOffsets, triangleGroups, newTGrps);

        RemoveEmptyTriangles(newVs, newNs, newUv1s, newUv2s, newUv3s, newUv4s, newColors32, newTs, newBws, subMeshOffsets, newTGrps);

        if (removeSmallParts > 0f) RemoveMiniTriangleGroups(removeSmallParts, sizeMultiplier, maxWeight, newVs, newTs, subMeshOffsets, newTGrps);  // use the original weight for this

        RemoveUnusedVertices(newVs, newNs, newUv1s, newUv2s, newUv3s, newUv4s, newColors32, newBws, newTs);


        /*        stopWatch.Stop();
                TimeSpan timespan = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    timespan.Hours, timespan.Minutes, timespan.Seconds,
                    timespan.Milliseconds / 10);
                UnityEngine.Debug.Log("RunTime " + elapsedTime);
        */
    }

    private static bool AnyWeightOK(float[] weights, float aMaxWeight)
    {
        for (int j = 0; j < 6; j++)
        {
            if (weights[j] < aMaxWeight) return true;
        }
        return false;
    }

    private static int[] GetAdjacentTriangles(int[] ts, int tIdx, List<List<int>> trianglesPerVertex, int[] uniqueVs, int[] triangleGroups, List<List<int>> trianglesPerGroup)
    {
        int t0 = ts[tIdx];
        int t1 = ts[tIdx + 1];
        int t2 = ts[tIdx + 2];
        List<int> foundTriangles = new List<int>();
        List<int> adjacentTriangles = new List<int>();
        List<int> trianglesForVertex = trianglesPerVertex[uniqueVs[t0]];
        for (int i = 0; i < trianglesForVertex.Count; i++)
        {
            adjacentTriangles.Add(ts[trianglesForVertex[i] * 3]);
            adjacentTriangles.Add(ts[(trianglesForVertex[i] * 3) + 1]);
            adjacentTriangles.Add(ts[(trianglesForVertex[i] * 3) + 2]);
            foundTriangles.Add(trianglesForVertex[i]);
            SetTriangleGroup(tIdx / 3, trianglesForVertex[i], triangleGroups, trianglesPerGroup);
        }

        trianglesForVertex = trianglesPerVertex[uniqueVs[t1]];
        for (int i = 0; i < trianglesForVertex.Count; i++)
        {
            int j = 0;
            for (; j < foundTriangles.Count; j++)
            {
                if (foundTriangles[j] == trianglesForVertex[i])
                {
                    break;
                }
            }
            if (j >= foundTriangles.Count)
            {
                adjacentTriangles.Add(ts[trianglesForVertex[i] * 3]);
                adjacentTriangles.Add(ts[(trianglesForVertex[i] * 3) + 1]);
                adjacentTriangles.Add(ts[(trianglesForVertex[i] * 3) + 2]);
                foundTriangles.Add(trianglesForVertex[i]);
                SetTriangleGroup(tIdx / 3, trianglesForVertex[i], triangleGroups, trianglesPerGroup);
            }
        }

        trianglesForVertex = trianglesPerVertex[uniqueVs[t2]];
        for (int i = 0; i < trianglesForVertex.Count; i++)
        {
            int j = 0;
            for (; j < foundTriangles.Count; j++)
            {
                if (foundTriangles[j] == trianglesForVertex[i])
                {
                    break;
                }
            }
            if (j >= foundTriangles.Count)
            {
                adjacentTriangles.Add(ts[trianglesForVertex[i] * 3]);
                adjacentTriangles.Add(ts[(trianglesForVertex[i] * 3) + 1]);
                adjacentTriangles.Add(ts[(trianglesForVertex[i] * 3) + 2]);
                foundTriangles.Add(trianglesForVertex[i]);
                SetTriangleGroup(tIdx / 3, trianglesForVertex[i], triangleGroups, trianglesPerGroup);
            }
        }
        return adjacentTriangles.ToArray();
    }

    private static void SetTriangleGroup(int tIdx0, int tIdx1, int[] triangleGroups, List<List<int>> trianglesPerGroup)
    {
        if (triangleGroups[tIdx0] < 0 && triangleGroups[tIdx1] < 0)
        {
            triangleGroups[tIdx0] = trianglesPerGroup.Count;
            triangleGroups[tIdx1] = trianglesPerGroup.Count;
            trianglesPerGroup.Add(new List<int>());
            trianglesPerGroup[triangleGroups[tIdx0]].Add(tIdx0);
            trianglesPerGroup[triangleGroups[tIdx0]].Add(tIdx1);
        }
        else if (triangleGroups[tIdx0] < 0 && triangleGroups[tIdx1] >= 0)
        {
            triangleGroups[tIdx0] = triangleGroups[tIdx1];
            trianglesPerGroup[triangleGroups[tIdx1]].Add(tIdx0);
        }
        else if (triangleGroups[tIdx0] >= 0 && triangleGroups[tIdx1] < 0)
        {
            triangleGroups[tIdx1] = triangleGroups[tIdx0];
            trianglesPerGroup[triangleGroups[tIdx0]].Add(tIdx1);
        }
        else if (triangleGroups[tIdx0] != triangleGroups[tIdx1])
        {
            List<int> trianglesToRenumber = trianglesPerGroup[triangleGroups[tIdx1]];
            trianglesPerGroup[triangleGroups[tIdx0]].AddRange(trianglesToRenumber);
            trianglesPerGroup[triangleGroups[tIdx1]] = new List<int>();

            // renumber all triangleGroups[tIdx0] to triangleGroups[tIdx1]
            for (int i = 0; i < trianglesToRenumber.Count; i++)
            {
                triangleGroups[trianglesToRenumber[i]] = triangleGroups[tIdx0];
            }
        }
    }

    private static void GetTotalAngleAndCenterDistanceForCorner(int[] ts, Vector3[] vs, int[] movedVs, int vertexIdx, Vector3[] centerDistances, ref float totalAngle, ref Vector3 totalCenterDist)
    {
        totalAngle = 0f;
        totalCenterDist = Vector3.zero;
        int centerDistCount = 0;
        for (int j = 0; j < ts.Length; j++)
        {
            if (ts[j] == vertexIdx)
            {
                int triangleStart = (j / 3) * 3;
                int idx0 = ts[triangleStart];
                int idx1 = ts[triangleStart + 1];
                int idx2 = ts[triangleStart + 2];
                j = triangleStart + 2;
                if (idx0 != idx1 && idx1 != idx2 && idx2 != idx0)
                {
                    int side1 = vertexIdx;
                    int side2 = vertexIdx;
                    if (idx0 != vertexIdx) side1 = idx0;
                    if (idx1 != vertexIdx)
                    {
                        if (side1 == vertexIdx) side1 = idx1;
                        else side2 = ts[triangleStart + 1];
                    }
                    if (idx2 != vertexIdx)
                    {
                        side2 = idx2;
                    }
                    totalAngle += Vector3.Angle((vs[movedVs[side1]] - vs[movedVs[vertexIdx]]), (vs[movedVs[side2]] - vs[movedVs[vertexIdx]]));
                }
                totalCenterDist += centerDistances[idx0] + centerDistances[idx1] + centerDistances[idx2];
                centerDistCount += 3;
            }
        }
        totalCenterDist = totalCenterDist / (float)centerDistCount;
    }
    private static void GetTotalAngleAndCenterDistanceForNewCorner(int[] ts, Vector3[] vs, int[] movedVs, int[] uniqueVs, int vertexIdx, int newIdx, Vector3[] centerDistances, float maxWeight, ref float totalAngle, ref Vector3 totalCenterDist, ref bool flipsTriangles)
    {
        float flipTriangleLimit = 0.5f + Mathf.Clamp01(maxWeight * 0.75f);
        totalAngle = 0f;
        totalCenterDist = Vector3.zero;
        int centerDistCount = 0;
        flipsTriangles = false;
        for (int j = 0; j < ts.Length; j++)
        {
            if (ts[j] == vertexIdx || ts[j] == newIdx)
            {
                int triangleStart = (j / 3) * 3;
                int idx0 = ts[triangleStart];
                int idx1 = ts[triangleStart + 1];
                int idx2 = ts[triangleStart + 2];
                j = triangleStart + 2;

                if (idx0 != idx1 && idx1 != idx2 && idx2 != idx0)
                {
                    int side1 = vertexIdx;
                    int side2 = vertexIdx;
                    if (idx0 != vertexIdx) side1 = idx0;
                    if (idx1 != vertexIdx)
                    {
                        if (side1 == vertexIdx) side1 = idx1;
                        else side2 = ts[triangleStart + 1];
                    }
                    if (idx2 != vertexIdx)
                    {
                        side2 = idx2;
                    }
                    Vector3 oldCrossProduct = Vector3.Cross((vs[movedVs[uniqueVs[side1]]] - vs[movedVs[uniqueVs[vertexIdx]]]), (vs[movedVs[uniqueVs[side2]]] - vs[movedVs[uniqueVs[vertexIdx]]])).normalized;

                    if (idx0 == vertexIdx) idx0 = newIdx;
                    if (idx1 == vertexIdx) idx1 = newIdx;
                    if (idx2 == vertexIdx) idx2 = newIdx;
                    if (idx0 != idx1 && idx1 != idx2 && idx2 != idx0)
                    {
                        side1 = newIdx;
                        side2 = newIdx;
                        if (idx0 != newIdx) side1 = idx0;
                        if (idx1 != newIdx)
                        {
                            if (side1 == newIdx) side1 = idx1;
                            else side2 = ts[triangleStart + 1];
                        }
                        if (idx2 != newIdx)
                        {
                            side2 = idx2;
                        }
                        Vector3 newCrossProduct = Vector3.Cross((vs[movedVs[uniqueVs[side1]]] - vs[movedVs[uniqueVs[newIdx]]]), (vs[movedVs[uniqueVs[side2]]] - vs[movedVs[uniqueVs[newIdx]]])).normalized;
                        if ((newCrossProduct + oldCrossProduct).magnitude < flipTriangleLimit)
                        {
                            flipsTriangles = true;
                        }
                        totalAngle += Vector3.Angle((vs[movedVs[uniqueVs[side1]]] - vs[movedVs[uniqueVs[newIdx]]]), (vs[movedVs[uniqueVs[side2]]] - vs[movedVs[uniqueVs[newIdx]]]));
                    }
                }
                totalCenterDist = totalCenterDist + centerDistances[idx0] + centerDistances[idx1] + centerDistances[idx2];
                centerDistCount += 3;
            }
        }
        totalCenterDist = totalCenterDist / (float)centerDistCount;
    }


    private static void GetUVStretchAndAreaForCorner(int[] ts, Vector3[] vs, int[] movedVs, int[] uniqueVs, Vector2[] uvs, int cFrom, int cTo, ref float affectedUvAreaDiff, ref float affectedAreaDiff, ref float totalUvAreaDiff, ref float totalAreaDiff)
    {
        float totalUvAreaPerUnit = 0f;
        int totalMeasurements = 0;
        float avgUvPerUnitBefore = 0f;
        float avgUvPerUnitAfter = 0f;
        float totalUvAreaBefore = 0f;
        float totalUvAreaAfter = 0f;
        float totalAreaBefore = 0f;
        float totalAreaAfter = 0f;
        float affectedTotalAreaBefore = 0f;
        float affectedTotalAreaAfter = 0f;
        int uniqueCFrom = uniqueVs[cFrom];
        int uniqueCTo = uniqueVs[cTo];
        affectedUvAreaDiff = 0f;
        affectedAreaDiff = 0f;

        int idx0 = 0;
        int idx1 = 0;
        int idx2 = 0;
        float area = 0f;
        float uvArea = 0f;
        for (int j = 0; j < ts.Length; j++)
        {
            if (j % 3 == 0)
            {
                idx0 = ts[j];
                idx1 = ts[j + 1];
                idx2 = ts[j + 2];
                if (idx0 == idx1 || idx1 == idx2 || idx2 == idx0)
                { // already flattened, dont bother
                    j += 2;
                    continue;
                }
                area = Area(vs[movedVs[idx0]], vs[movedVs[idx1]], vs[movedVs[idx2]]);
                totalAreaBefore += area;
                // moet dit niet de movedVS gebruiken?
                if (uvs != null && uvs.Length > 0) uvArea = Area(uvs[idx0], uvs[idx1], uvs[idx2]);
                totalUvAreaBefore += uvArea;
            }
            if (uniqueVs[ts[j]] == uniqueCFrom)
            {
                int sharedCorners = 1;
                if (uniqueVs[idx0] == uniqueCTo || uniqueVs[idx1] == uniqueCTo || uniqueVs[idx2] == uniqueCTo) sharedCorners++;
                if (uvs != null && uvs.Length > 0 && area > 0f && sharedCorners < 2)
                {  // all areas excluding the ones that will be flattened
                    totalUvAreaPerUnit += uvArea / area;  // divide by area, later on the total will be multiplied again
                    totalMeasurements++;
                }
                affectedTotalAreaBefore += area;
            }
        }
        if (totalMeasurements > 0 && totalUvAreaPerUnit > 0f)
        {
            avgUvPerUnitBefore = totalUvAreaPerUnit / totalMeasurements;
        }

        totalUvAreaPerUnit = 0f;
        totalMeasurements = 0;
        for (int j = 0; j < ts.Length; j++)
        {
            if (j % 3 == 0)
            {
                idx0 = ts[j];
                idx1 = ts[j + 1];
                idx2 = ts[j + 2];
                if (uniqueVs[idx0] == uniqueCFrom) idx0 = cTo;
                if (uniqueVs[idx1] == uniqueCFrom) idx1 = cTo;
                if (uniqueVs[idx2] == uniqueCFrom) idx2 = cTo;
                if (idx0 == idx1 || idx1 == idx2 || idx2 == idx0)
                { // flattened, dont bother
                    j += 2;
                    continue;
                }
                area = Area(vs[movedVs[idx0]], vs[movedVs[idx1]], vs[movedVs[idx2]]);
                totalAreaAfter += area;
                // moet dit niet de movedVS gebruiken?
                if (uvs != null && uvs.Length > 0) uvArea = Area(uvs[idx0], uvs[idx1], uvs[idx2]);
                totalUvAreaAfter += uvArea;
            }
            if (uniqueVs[ts[j]] == uniqueCFrom)
            {
                int sharedCorners = 1;
                if (uniqueVs[idx0] == uniqueCTo || uniqueVs[idx1] == uniqueCTo || uniqueVs[idx2] == uniqueCTo) sharedCorners++;
                if (uvs != null && uvs.Length > 0 && area > 0f)
                {
                    totalUvAreaPerUnit += uvArea / area;  // divide by area, later on the total will be multiplied again
                    totalMeasurements++;
                }
                affectedTotalAreaAfter += area;
            }
        }
        affectedAreaDiff = Mathf.Abs(affectedTotalAreaAfter - affectedTotalAreaBefore);
        totalAreaDiff = totalAreaAfter - totalAreaBefore;
        totalUvAreaDiff = totalUvAreaAfter - totalUvAreaBefore;

        if (totalMeasurements > 0 && totalUvAreaPerUnit > 0f)
        {
            avgUvPerUnitAfter = totalUvAreaPerUnit / totalMeasurements;
        }
        float diff = Mathf.Abs(avgUvPerUnitAfter - avgUvPerUnitBefore);
        if (diff > 0f) affectedUvAreaDiff = Mathf.Sqrt(diff) * affectedTotalAreaBefore;
    }

    private static float GetNormalDiffForCorners(Vector3[] ns, int corner1, int corner2)
    {
        if (ns == null || ns.Length <= 0) return 0f;
        return Vector3.Angle(ns[corner1], ns[corner2]) / 180f;
    }

    private static void MergeVertices(ref int oldV, int newV, bool[] hasTwinVS, Vector3[] vs, int[] triangles, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, bool[] deletedVertices, int[] movedVs, int[] uniqueVs, int[] movedUv1s, int[] movedUv2s, int[] movedUv3s, int[] movedUv4s, int[] movedColors, List<List<int>> trianglesPerVertex, bool logYN)
    {
        if (oldV == newV) return;
        deletedVertices[oldV] = true;
        int uniqueOldV = uniqueVs[oldV];
        int uniqueNewV = uniqueVs[newV];
        List<int> trianglesToChange = trianglesPerVertex[uniqueOldV];

        for (int j = 0; j < trianglesToChange.Count; j++)
        {
            int idx = trianglesToChange[j] * 3;
            for (int i = 0; i < 3; i++)
            {
                if (triangles[idx + i] == oldV)
                {
                    triangles[idx + i] = newV;
                }
            }
        }
        if (uniqueOldV != uniqueNewV)
        {
            trianglesPerVertex[uniqueNewV].AddRange(trianglesPerVertex[uniqueOldV]);
            trianglesPerVertex[uniqueOldV].Clear();
        }

        if (hasTwinVS[oldV] || hasTwinVS[movedVs[oldV]])
        {
            MoveVertex(oldV, newV, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors);
            for (int j = 0; j < vs.Length; j++)
            {
                if (j != oldV && vs[oldV].x == vs[movedVs[j]].x && vs[oldV].y == vs[movedVs[j]].y && vs[oldV].z == vs[movedVs[j]].z)
                {
                    MoveVertex(j, newV, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors);
                }
            }
        }
        oldV = newV;
    }

    private static void MoveVertex(int oldV, int newV, int[] movedVs, int[] uniqueVs, int[] movedUv1s, int[] movedUv2s, int[] movedUv3s, int[] movedUv4s, int[] movedColors)
    {
        movedVs[oldV] = movedVs[newV];
        movedVs[movedVs[oldV]] = movedVs[newV];
        movedVs[uniqueVs[oldV]] = movedVs[newV];
        if (movedUv1s.Length > 0)
        {
            movedUv1s[oldV] = movedUv1s[newV];
            movedUv1s[movedVs[oldV]] = movedUv1s[newV];
            movedUv1s[uniqueVs[oldV]] = movedUv1s[newV];
        }
        if (movedUv2s.Length > 0)
        {
            movedUv2s[oldV] = movedUv2s[newV];
            movedUv2s[movedVs[oldV]] = movedUv2s[newV];
            movedUv2s[uniqueVs[oldV]] = movedUv2s[newV];
        }
        if (movedUv3s.Length > 0)
        {
            movedUv3s[oldV] = movedUv3s[newV];
            movedUv3s[movedVs[oldV]] = movedUv3s[newV];
            movedUv3s[uniqueVs[oldV]] = movedUv3s[newV];
        }
        if (movedUv4s.Length > 0)
        {
            movedUv4s[oldV] = movedUv4s[newV];
            movedUv4s[movedVs[oldV]] = movedUv4s[newV];
            movedUv4s[uniqueVs[oldV]] = movedUv4s[newV];
        }
        if (movedColors.Length > 0)
        {
            movedColors[oldV] = movedColors[newV];
            movedColors[movedVs[oldV]] = movedColors[newV];
            movedColors[uniqueVs[oldV]] = movedColors[newV];
        }
    }


    private static void FillNewMeshArray(Vector3[] vs, bool[] vdel, int[] movedVs, Vector3[] ns, Vector2[] uv1s, int[] movedUv1s, Vector2[] uv2s, int[] movedUv2s, Vector2[] uv3s, int[] movedUv3s, Vector2[] uv4s, int[] movedUv4s, Color32[] colors32, int[] movedColors, BoneWeight[] bws, List<Vector3> newVs, List<Vector3> newNs, List<Vector2> newUv1s, List<Vector2> newUv2s, List<Vector2> newUv3s, List<Vector2> newUv4s, List<Color32> newColors32, List<BoneWeight> newBws, int[] o2n)
    {
        bool hasNs = (ns != null && ns.Length > 0);
        bool hasUv1s = false;
        for (int i = 0; uv1s != null && i < uv1s.Length; i++)
        {
            if (uv1s[i].x != 0f || uv1s[i].y != 0f)
            {
                hasUv1s = true;
                break;
            }
        }
        bool hasUv2s = false;
        for (int i = 0; uv2s != null && i < uv2s.Length; i++)
        {
            if (uv2s[i].x != 0f || uv2s[i].y != 0f)
            {
                hasUv2s = true;
                break;
            }
        }
        bool hasUv3s = false;
        for (int i = 0; uv3s != null && i < uv3s.Length; i++)
        {
            if (uv3s[i].x != 0f || uv3s[i].y != 0f)
            {
                hasUv3s = true;
                break;
            }
        }
        bool hasUv4s = false;
        for (int i = 0; uv4s != null && i < uv4s.Length; i++)
        {
            if (uv4s[i].x != 0f || uv4s[i].y != 0f)
            {
                hasUv4s = true;
                break;
            }
        }
        bool hasColors = false;
        for (int i = 0; colors32 != null && i < colors32.Length; i++)
        {
            if (colors32[i].r > 0 || colors32[i].g > 0 || colors32[i].b > 0)
            {
                hasColors = true;
                break;
            }
        }
        bool hasBws = (bws != null && bws.Length > 0);
        int vIdx = 0;
        for (int i = 0; i < vs.Length; i++)
        {
            if (!vdel[i])
            {
                newVs.Add(vs[movedVs[i]]);
                if (hasNs) newNs.Add(ns[i]);
                if (hasUv1s) newUv1s.Add(uv1s[movedUv1s[i]]);
                if (hasUv2s) newUv2s.Add(uv2s[movedUv2s[i]]);
                if (hasUv3s) newUv3s.Add(uv3s[movedUv3s[i]]);
                if (hasUv4s) newUv4s.Add(uv4s[movedUv4s[i]]);
                if (hasColors) newColors32.Add(colors32[movedColors[i]]);
                if (hasBws) newBws.Add(bws[i]);
                o2n[i] = vIdx;
                vIdx++;
            }
            else
            {
                o2n[i] = -1;
            }
        }
    }

    private static void FillNewMeshTriangles(int[] oldTriangles, int[] o2n, List<int> newTriangles, int[] subMeshOffsets, int[] triangleGroups, List<int> newTGrps)
    {
        int subMeshIdx = -1;
        for (int i = 0; i < oldTriangles.Length; i += 3)
        {
            int v0 = oldTriangles[i];
            int v1 = oldTriangles[i + 1];
            int v2 = oldTriangles[i + 2];
            while (subMeshIdx + 1 < subMeshOffsets.Length && i == subMeshOffsets[subMeshIdx + 1])
            {
                subMeshIdx++;
                subMeshOffsets[subMeshIdx] = newTriangles.Count;
            }
            if (o2n[v0] >= 0 && o2n[v1] >= 0 && o2n[v2] >= 0)
            {
                if (o2n[v0] != o2n[v1] && o2n[v1] != o2n[v2] && o2n[v2] != o2n[v0])
                {
                    newTriangles.Add(o2n[v0]);
                    newTriangles.Add(o2n[v1]);
                    newTriangles.Add(o2n[v2]);
                    newTGrps.Add(triangleGroups[i / 3]);
                }
            }
        }
        while (subMeshIdx + 1 < subMeshOffsets.Length)
        {
            subMeshIdx++;
            subMeshOffsets[subMeshIdx] = newTriangles.Count;
        }
    }

    public static void RemoveUnusedVertices(List<Vector3> vs, List<Vector3> ns, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> bws, List<int> ts)
    {
        List<List<int>> trianglesPerVertex = new List<List<int>>();
        for (int i = 0; i < vs.Count; i++)
        {
            trianglesPerVertex.Add(new List<int>());
        }
        for (int i = 0; i < ts.Count; i++)
        {
            trianglesPerVertex[ts[i]].Add(i);
        }

        bool doNs = (ns != null && ns.Count > 0);
        bool doUv1s = (uv1s != null && uv1s.Count > 0);
        bool doUv2s = (uv2s != null && uv2s.Count > 0);
        bool doUv3s = (uv3s != null && uv3s.Count > 0);
        bool doUv4s = (uv4s != null && uv4s.Count > 0);
        bool doColors = (colors32 != null && colors32.Count > 0);
        bool doBws = (bws != null && bws.Count > 0);
        int shiftIdx = 0;
        for (int i = 0; i < vs.Count; i++)
        {
            List<int> triangles = trianglesPerVertex[i];
            if (triangles.Count > 0)
            {
                if (shiftIdx > 0)
                {
                    for (int j = 0; j < triangles.Count; j++)
                    {
                        ts[triangles[j]] -= shiftIdx;
                    }
                }
            }
            else
            {
                vs.RemoveAt(i);
                trianglesPerVertex.RemoveAt(i);
                if (doNs) ns.RemoveAt(i);
                if (doUv1s) uv1s.RemoveAt(i);
                if (doUv2s) uv2s.RemoveAt(i);
                if (doUv3s) uv3s.RemoveAt(i);
                if (doUv4s) uv4s.RemoveAt(i);
                if (doColors) colors32.RemoveAt(i);
                if (doBws) bws.RemoveAt(i);
                shiftIdx++;
                i--;
            }
        }
    }

    public static void RemoveUnusedVertices(List<Vector3> vs, List<Vector3> ns, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> bws, List<List<int>> subMeshes)
    {
        List<List<int>> submeshesPerVertex = new List<List<int>>();
        List<List<int>> trianglesPerVertex = new List<List<int>>();
        for (int i = 0; i < vs.Count; i++)
        {
            submeshesPerVertex.Add(new List<int>());
            trianglesPerVertex.Add(new List<int>());
        }
        for (int s = 0; s < subMeshes.Count; s++)
        {
            List<int> ts = subMeshes[s];
            for (int i = 0; i < ts.Count; i++)
            {
                submeshesPerVertex[ts[i]].Add(s);
                trianglesPerVertex[ts[i]].Add(i);
            }
        }


        bool doNs = (ns != null && ns.Count > 0);
        bool doUv1s = (uv1s != null && uv1s.Count > 0);
        bool doUv2s = (uv2s != null && uv2s.Count > 0);
        bool doUv3s = (uv3s != null && uv3s.Count > 0);
        bool doUv4s = (uv4s != null && uv4s.Count > 0);
        bool doColors = (colors32 != null && colors32.Count > 0);
        bool doBws = (bws != null && bws.Count > 0);
        int shiftIdx = 0;
        for (int i = 0; i < vs.Count; i++)
        {
            List<int> submeshIdxs = submeshesPerVertex[i];
            List<int> triangles = trianglesPerVertex[i];
            if (triangles.Count > 0)
            {
                if (shiftIdx > 0)
                {
                    for (int j = 0; j < submeshIdxs.Count; j++)
                    {
                        subMeshes[submeshIdxs[j]][triangles[j]] -= shiftIdx;
                    }
                }
            }
            else
            {
                vs.RemoveAt(i);
                submeshesPerVertex.RemoveAt(i);
                trianglesPerVertex.RemoveAt(i);
                if (doNs) ns.RemoveAt(i);
                if (doUv1s) uv1s.RemoveAt(i);
                if (doUv2s) uv2s.RemoveAt(i);
                if (doUv3s) uv3s.RemoveAt(i);
                if (doUv4s) uv4s.RemoveAt(i);
                if (doColors) colors32.RemoveAt(i);
                if (doBws) bws.RemoveAt(i);
                shiftIdx++;
                i--;
            }
        }
    }

    public static void RemoveUnusedVertices(List<Vector3> vs, List<Vector3> ns, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> bws, Dictionary<Material, List<int>> subMeshes)
    {
        List<List<Material>> materialsPerVertex = new List<List<Material>>();
        List<List<int>> trianglesPerVertex = new List<List<int>>();
        for (int i = 0; i < vs.Count; i++)
        {
            materialsPerVertex.Add(new List<Material>());
            trianglesPerVertex.Add(new List<int>());
        }
        foreach (Material m in subMeshes.Keys)
        {
            List<int> ts = subMeshes[m];
            for (int i = 0; i < ts.Count; i++)
            {
                materialsPerVertex[ts[i]].Add(m);
                trianglesPerVertex[ts[i]].Add(i);
            }
        }

        bool doNs = (ns != null && ns.Count > 0);
        bool doUv1s = (uv1s != null && uv1s.Count > 0);
        bool doUv2s = (uv2s != null && uv2s.Count > 0);
        bool doUv3s = (uv3s != null && uv3s.Count > 0);
        bool doUv4s = (uv4s != null && uv4s.Count > 0);
        bool doColors = (colors32 != null && colors32.Count > 0);
        bool doBws = (bws != null && bws.Count > 0);
        int shiftIdx = 0;
        for (int i = 0; i < vs.Count; i++)
        {
            List<Material> materials = materialsPerVertex[i];
            List<int> triangles = trianglesPerVertex[i];
            if (triangles.Count > 0)
            {
                if (shiftIdx > 0)
                {
                    for (int j = 0; j < materials.Count; j++)
                    {
                        subMeshes[materials[j]][triangles[j]] -= shiftIdx;
                    }
                }
            }
            else
            {
                vs.RemoveAt(i);
                materialsPerVertex.RemoveAt(i);
                trianglesPerVertex.RemoveAt(i);
                if (doNs) ns.RemoveAt(i);
                if (doUv1s) uv1s.RemoveAt(i);
                if (doUv2s) uv2s.RemoveAt(i);
                if (doUv3s) uv3s.RemoveAt(i);
                if (doUv4s) uv4s.RemoveAt(i);
                if (doColors) colors32.RemoveAt(i);
                if (doBws) bws.RemoveAt(i);
                shiftIdx++;
                i--;
            }
        }
    }

    private static void RemoveEmptyTriangles(List<Vector3> newVs, List<Vector3> newNs, List<Vector2> newUv1s, List<Vector2> newUv2s, List<Vector2> newUv3s, List<Vector2> newUv4s, List<Color32> newColors32, List<int> newTs, List<BoneWeight> newBws, int[] subMeshOffsets, List<int> newTGrps)
    {
        int subMeshIdx = subMeshOffsets.Length - 1;
        bool[] usedVs = new bool[newVs.Count];
        for (int i = newTs.Count - 3; i >= 0; i -= 3)
        {
            while (subMeshIdx > 0 && i + 3 == subMeshOffsets[subMeshIdx])
            {
                subMeshIdx--;
            }
            if (Area(newVs[newTs[i]], newVs[newTs[i + 1]], newVs[newTs[i + 2]]) <= 0f)
            {
                newTs.RemoveAt(i + 2);
                newTs.RemoveAt(i + 1);
                newTs.RemoveAt(i);
                newTGrps.RemoveAt(i / 3);
                for (int s = subMeshIdx + 1; s < subMeshOffsets.Length; s++) subMeshOffsets[s] -= 3;
            }
            else
            {
                usedVs[newTs[i]] = true;
                usedVs[newTs[i + 1]] = true;
                usedVs[newTs[i + 2]] = true;
            }
        }
        bool doNs = (newNs != null && newNs.Count > 0);
        bool doUv1s = (newUv1s != null && newUv1s.Count > 0);
        bool doUv2s = (newUv2s != null && newUv2s.Count > 0);
        bool doUv3s = (newUv3s != null && newUv3s.Count > 0);
        bool doUv4s = (newUv4s != null && newUv4s.Count > 0);
        bool doColors = (newColors32 != null && newColors32.Count > 0);
        bool doBws = (newBws != null && newBws.Count > 0);
        List<int> vertexIdxShift = new List<int>(); // length - index = nr of places to shift, value = old vertexIndex;
        for (int i = usedVs.Length - 1; i >= 0; i--)
        {
            if (!usedVs[i])
            {
                newVs.RemoveAt(i);
                if (doNs) newNs.RemoveAt(i);
                if (doUv1s) newUv1s.RemoveAt(i);
                if (doUv2s) newUv2s.RemoveAt(i);
                if (doUv3s) newUv3s.RemoveAt(i);
                if (doUv4s) newUv4s.RemoveAt(i);
                if (doColors) newColors32.RemoveAt(i);
                if (doBws) newBws.RemoveAt(i);
                vertexIdxShift.Add(i);
            }
        }

        for (int i = 0; i < newTs.Count; i++)
        {
            int vIdx = newTs[i];
            int shift = 0;
            for (int j = vertexIdxShift.Count - 1; j >= 0; j--)
            {
                if (vIdx < vertexIdxShift[j]) break;
                else shift++;
            }
            if (shift > 0) newTs[i] = vIdx - shift;
        }
    }


    private static void RemoveMiniTriangleGroups(float removeSmallParts, Vector3 sizeMultiplier, float aMaxWeight, List<Vector3> newVs, List<int> newTs, int[] subMeshOffsets, List<int> newTGrps)
    {
        // newTGrps: index = index of triangle, value = id of group
        float useMaxWeight = aMaxWeight * 0.5f >= 1f ? aMaxWeight * 0.5f : Mathf.Pow(aMaxWeight * 0.5f, 3f);
        float totalArea = 0f;

        // Build easy accessible list of group id's
        // Find area and triangleCount per group
        List<int> grpNrs = new List<int>();
        List<int> triCountPerGrp = new List<int>();
        List<float> triAreaPerGrp = new List<float>();
        for (int i = 0; i < newTGrps.Count; i++)
        {
            int grpNr = newTGrps[i];
            float area = Area(newVs[newTs[i * 3]].Product(sizeMultiplier), newVs[newTs[(i * 3) + 1]].Product(sizeMultiplier), newVs[newTs[(i * 3) + 2]].Product(sizeMultiplier));
            int j = 0;
            for (; j < grpNrs.Count; j++)
            {
                if (grpNrs[j] == grpNr && grpNr >= 0) break;
            }
            if (j >= grpNrs.Count)
            {
                grpNrs.Add(grpNr);
                triAreaPerGrp.Add(0f);
                triCountPerGrp.Add(0);
            }
            triAreaPerGrp[j] = triAreaPerGrp[j] + area;
            triCountPerGrp[j] = triCountPerGrp[j] + 1;
            totalArea += area;
        }

        removeSmallParts = Mathf.Clamp(removeSmallParts, 0f, 5f) * 0.0028f * useMaxWeight;
        for (int i = 0; i < grpNrs.Count; i++)
        {
            // test to see if group is small enough to delete
            if (((triAreaPerGrp[i] / Mathf.Pow(triCountPerGrp[i], 0.33f) / totalArea)) < removeSmallParts)
            {
                int grpNr = grpNrs[i];
                for (int j = newTGrps.Count - 1; j >= 0; j--)
                {
                    if (newTGrps[j] == grpNr)
                    {
                        newTs.RemoveAt(j * 3);
                        newTs.RemoveAt(j * 3);
                        newTs.RemoveAt(j * 3);
                        newTGrps.RemoveAt(j);
                        for (int s = 0; s < subMeshOffsets.Length; s++)
                        {
                            if (subMeshOffsets[s] > j * 3) subMeshOffsets[s] -= 3;
                        }
                    }
                }
            }
        }
    }

    public static Mesh CreateNewMesh(Vector3[] vs, Vector3[] ns, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, int[] ts, BoneWeight[] bws, Matrix4x4[] bindposes, int[] subMeshOffsets, bool recalcNormals)
    {
        Mesh newMesh = new Mesh();
        FillMesh(newMesh, vs, ns, uv1s, uv2s, uv3s, uv4s, colors32, ts, bws, bindposes, subMeshOffsets, recalcNormals);
        return newMesh;
    }

    public static void FillMesh(Mesh mesh, Vector3[] vs, Vector3[] ns, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, int[] ts, BoneWeight[] bws, Matrix4x4[] bindposes, int[] subMeshOffsets, bool recalcNormals)
    {
        mesh.vertices = vs;
        if (ns != null && ns.Length > 0) mesh.normals = ns;
        if (uv1s != null && uv1s.Length > 0) mesh.uv = uv1s;
        if (uv2s != null && uv2s.Length > 0) mesh.uv2 = uv2s;
#if UNITY_4_3
#elif UNITY_4_4
#elif UNITY_4_5
#elif UNITY_4_6
#else
        if (uv3s != null && uv2s.Length > 0) mesh.uv3 = uv3s;
        if (uv4s != null && uv2s.Length > 0) mesh.uv4 = uv4s;
#endif
        if (colors32 != null && colors32.Length > 0) mesh.colors32 = colors32;
        if (bws != null && bws.Length > 0) mesh.boneWeights = bws;
        if (bindposes != null && bindposes.Length > 0) mesh.bindposes = bindposes;
        if (subMeshOffsets.Length == 1)
        {
            mesh.triangles = ts;
        }
        else
        {
            mesh.subMeshCount = subMeshOffsets.Length;
            for (int s = 0; s < subMeshOffsets.Length; s++)
            {
                subMeshOffsets[s] = Mathf.Max(0, subMeshOffsets[s]);
                int end = s + 1 < subMeshOffsets.Length ? subMeshOffsets[s + 1] : ts.Length;
                if (end - subMeshOffsets[s] > 0)
                {
                    int[] subTs = new int[end - subMeshOffsets[s]];
                    Array.Copy(ts, subMeshOffsets[s], subTs, 0, end - subMeshOffsets[s]);
                    mesh.SetTriangles(subTs, s);
                }
                else
                {
                    List<int> temp = null;
                    mesh.SetTriangles(temp, s);
                }
            }
        }
        if (recalcNormals || mesh.normals == null || mesh.normals.Length <= 0) mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }


    private static float AngleCornerDiff(float angle)
    {
        angle = Mathf.Abs(angle.To180Angle());
        float diff = Mathf.Min(angle, Mathf.Min(Mathf.Abs((180f - angle).To180Angle()), Mathf.Abs((90f - angle).To180Angle())));
        return diff * diff * 10f;
    }
    private static float AngleDiff(float angle)
    {
        angle = Mathf.Abs(angle.To180Angle());
        float diff = Mathf.Min(angle, Mathf.Abs((180f - angle).To180Angle()));
        return diff * diff;
    }
    private static float Area(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (p1 - p0).magnitude * (Mathf.Sin(Vector3.Angle((p1 - p0), (p2 - p0)) * Mathf.Deg2Rad) * (p2 - p0).magnitude) * 0.5f;
    }

    private static int GetVertexEqualTo(Vector3 v, List<int> orderedVertices, Vector3[] vs)
    {
        int i = orderedVertices.Count / 2;
        int increment = i;
        int direction = 1;
        int preventLoops = 0;
        while (i >= 0 && i < orderedVertices.Count)
        {
            Vector3 ov = vs[orderedVertices[i]];
            int prevIncrement = increment;
            int prevDirection = direction;
            increment = Mathf.Max(prevIncrement / 2, 1);
            if (ov.y < v.y) direction = 1;
            else if (ov.y > v.y) direction = -1;
            else
            {  // found y
                if (ov.z < v.z) direction = 1;
                else if (ov.z > v.z) direction = -1;
                else
                {  // found z
                    if (ov.x < v.x) direction = 1;
                    else if (ov.x > v.x) direction = -1;
                    else
                    {  // found x
                       // go to first
                        while (i >= 0 && vs[orderedVertices[i]].IsEqual(v)) i--;
                        i++;
                        return i;
                    }
                }
            }
            i += direction * increment;
            if (direction != prevDirection && increment == 1 && prevIncrement == 1 && !vs[orderedVertices[i]].IsEqual(v)) break;
            if (++preventLoops > orderedVertices.Count) break;  // just in case
        }
        return -1;
    }

    private static List<int> GetVerticesEqualTo(Vector3 v, List<int> orderedVertices, Vector3[] vs)
    {
        List<int> foundVertices = new List<int>();
        int i = GetVertexEqualTo(v, orderedVertices, vs);
        // go to last
        while (i >= 0 && i < orderedVertices.Count && vs[orderedVertices[i]].IsEqual(v))
        {
            foundVertices.Add(orderedVertices[i]);
            i++;
        }
        return foundVertices;
    }
    private static List<int> GetVerticesWithinBox(Vector3 from, Vector3 to, List<int> orderedVertices, Vector3[] vs)
    {
        List<int> foundVertices = new List<int>();
        int i = GetLastVertexWithYSmaller(from.y, orderedVertices, vs, orderedVertices.Count);
        if (i < 0) i = 0;
        for (; i < orderedVertices.Count && vs[orderedVertices[i]].y <= to.y; i++)
        {
            if (vs[orderedVertices[i]].y >= from.y && vs[orderedVertices[i]].x >= from.x && vs[orderedVertices[i]].x <= to.x && vs[orderedVertices[i]].z >= from.z && vs[orderedVertices[i]].z <= to.z)
            {
                foundVertices.Add(orderedVertices[i]);
            }
        }
        return foundVertices;
    }
    private static int GetLastVertexWithYSmaller(float y, List<int> orderedVertices, Vector3[] vs, int limitSearchRange)
    {
        int i = Mathf.Min(orderedVertices.Count, limitSearchRange) / 2;
        int increment = i;
        int direction = 1;
        while (i >= 0 && i < limitSearchRange && i < orderedVertices.Count)
        {
            int idx = orderedVertices[i];
            Vector3 ov = vs[idx];
            int prevIncrement = increment;
            int prevDirection = direction;
            increment = Mathf.Max(prevIncrement / 2, 1);
            if (ov.y < y)
            {
                if ((prevDirection == -1 && prevIncrement == 1) || prevIncrement == 0)
                {
                    for (i++; i < limitSearchRange && i < orderedVertices.Count; i++)
                    {
                        idx = orderedVertices[i];
                        if (vs[idx].y >= y) break;
                    }
                    return --i;
                }
                direction = 1;
            }
            else if (ov.y > y)
            {
                direction = -1;
            }
            else
            {  // found y
                for (i--; i >= 0; i--)
                {
                    idx = orderedVertices[i];
                    if (vs[idx].y < y) break;
                }
                return i;
            }
            i += direction * increment;
        }
        return -1;
    }

    private static bool IsVertexObscured(Vector3[] vs, Vector3[] ns, int[] ts, bool[] vObscured, int[] uniqueVs, Vector3 vertexBoxSize, List<int> orderedVertices, List<List<int>> trianglesPerVertex, int[] subMeshIdxPerVertex, float maxObscureDist, bool hiddenByOtherSubmesh, Vector3 vertex, Vector3 normal, int i)
    {
        List<int> checkVertices = GetVerticesWithinBox(vertex - vertexBoxSize, vertex + vertexBoxSize, orderedVertices, vs);
        for (int j = 0; j < checkVertices.Count; j++)
        {
            if (checkVertices[j] != i)
            {
                List<int> triangles = trianglesPerVertex[checkVertices[j]];
                for (int t = 0; t < triangles.Count; t++)
                {
                    int tIdx = triangles[t] * 3;
                    if (vObscured != null && vObscured[uniqueVs[ts[tIdx]]] && vObscured[uniqueVs[ts[tIdx + 1]]] && vObscured[uniqueVs[ts[tIdx + 2]]]) continue;
                    if (ts[tIdx] == i || ts[tIdx + 1] == i || ts[tIdx + 2] == i) continue;
                    if (hiddenByOtherSubmesh && subMeshIdxPerVertex[ts[tIdx]] == subMeshIdxPerVertex[i]) continue;

                    Vector3 planeNormal = (ns[ts[tIdx]] + ns[ts[tIdx + 1]] + ns[ts[tIdx + 2]]).normalized;
                    float angle = Vector3.Angle(normal, planeNormal);
                    if (angle < 60f)
                    {
                        float dist = FindCollision(vertex, normal, vs[ts[tIdx]], planeNormal);
                        if (dist > 0f && dist < maxObscureDist)
                        {
                            Vector3 point = vertex + (normal * dist);
                            Vector2 bar = point.Barycentric(vs[ts[tIdx]], vs[ts[tIdx + 1]], vs[ts[tIdx + 2]]);
                            if (bar.IsBarycentricInTriangle())
                            {
                                if (vObscured != null) vObscured[uniqueVs[i]] = true;
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    public static float FindCollision(Vector3 fromPos, Vector3 direction, Vector3 pointOnPlane, Vector3 normalPlane)
    {
        float dist = Mathf.Infinity;
        float divideBy = direction.InProduct(normalPlane);
        if (divideBy != 0f) dist = (pointOnPlane - fromPos).InProduct(normalPlane) / direction.InProduct(normalPlane);
        return dist;
    }



    private static void LogVectors(string msg, int[] idxs, Vector3[] vs, int decimals)
    {
        string s = msg + " (" + idxs.Length + "):\n";
        for (int i = 0; i < idxs.Length; i++)
        {
            s = s + "" + i + ": " + vs[idxs[i]].MakeString(decimals) + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, List<Vector3> vs, int decimals)
    {
        string s = msg + " (" + vs.Count + "):\n";
        for (int i = 0; i < vs.Count; i++)
        {
            s = s + "" + i + ": " + vs[i].MakeString(decimals) + "\n";
        }
        Log(s);
    }

    private static void LogArray(string msg, Vector3[] vs, int decimals)
    {
        string s = msg + " (" + vs.Length + "):\n";
        for (int i = 0; i < vs.Length; i++)
        {
            s = s + "" + i + ": " + vs[i].MakeString(decimals) + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, Vector2[] vs, int decimals)
    {
        string s = msg + " (" + vs.Length + "):\n";
        for (int i = 0; i < vs.Length; i++)
        {
            s = s + "" + i + ": " + vs[i].MakeString(decimals) + "\n";
        }
        Log(s);
    }
    private static void LogTriArray(string msg, List<int> ts)
    {
        string s = msg + " (" + ts.Count + "):\n";
        for (int i = 0; i < ts.Count; i += 3)
        {
            s = s + ts[i] + ", " + ts[i + 1] + ", " + ts[i + 2] + "\n";
        }
        Log(s);
    }
    private static void LogTriArray(string msg, int[] ts)
    {
        string s = msg + " (" + ts.Length + "):\n";
        for (int i = 0; i < ts.Length; i += 3)
        {
            s = s + ts[i] + ", " + ts[i + 1] + ", " + ts[i + 2] + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, List<Color32> ts)
    {
        string s = msg + " (" + ts.Count + "):\n";
        for (int i = 0; i < ts.Count; i++)
        {
            s = s + ts[i] + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, Color32[] ts)
    {
        string s = msg + " (" + ts.Length + "):\n";
        for (int i = 0; i < ts.Length; i++)
        {
            s = s + ts[i] + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, List<int> ts)
    {
        string s = msg + " (" + ts.Count + "):\n";
        for (int i = 0; i < ts.Count; i++)
        {
            s = s + ts[i] + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, List<List<int>> ts)
    {
        string s = msg + " (" + ts.Count + "):\n";
        for (int i = 0; i < ts.Count; i++)
        {
            List<int> sub = ts[i];
            for (int j = 0; j < sub.Count; j++) s = s + sub[j] + ",";
            s = s + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, int[] ts)
    {
        string s = msg + " (" + ts.Length + "):\n";
        for (int i = 0; i < ts.Length; i++)
        {
            s = s + ts[i] + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, float[] fs)
    {
        string s = msg + " (" + fs.Length + "):\n";
        for (int i = 0; i < fs.Length; i++)
        {
            s = s + fs[i] + "\n";
        }
        Log(s);
    }
    private static void LogArray(string msg, bool[] ts)
    {
        string s = msg + " (" + ts.Length + "):\n";
        for (int i = 0; i < ts.Length; i += 3)
        {
            s = s + ts[i] + "\n";
        }
        Log(s);
    }
    private static string LogWeights(string msg, float[] w)
    {
        string s = msg + ": ";
        s = s + " weight 0-1: " + w[0].MakeString(1) + " / " + w[1].MakeString(1);
        s = s + " | weight 1>2: " + w[2].MakeString(1) + " / " + w[3].MakeString(1);
        s = s + " | weight 2>0: " + w[4].MakeString(1) + " / " + w[5].MakeString(1);
        return s;
    }
    private static void Log(string msg)
    {
        UnityEngine.Debug.Log(msg + "\n" + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff"));
    }
}
