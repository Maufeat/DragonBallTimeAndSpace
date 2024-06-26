/* SimpleLOD 1.5     */
/* By Orbcreation BV */
/* Richard Knol      */
/* March 4, 2015      */

using UnityEngine;
using System;
using System.Collections;
using OrbCreationExtensions;

public class LODSwitcher : MonoBehaviour
{

    public Mesh[] lodMeshes;
    public GameObject[] lodGameObjects;
    public float[] lodScreenSizes;
    public float deactivateAtDistance = 0f;
    public Camera customCamera = null;

    private MeshFilter meshFilter;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Vector3 centerOffset;
    private float pixelsPerMeter;
    private float objectSize;
    private int fixedLODLevel = -1;
    private int lodLevel = 0;
    private int frameOffset = 0;
    private int frameInterval = 10;


    void Start()
    {
        frameOffset = Mathf.RoundToInt(UnityEngine.Random.value * 10f);
        if ((lodMeshes == null || lodMeshes.Length <= 0) && (lodGameObjects == null || lodGameObjects.Length <= 0))
        {
            Debug.LogWarning(gameObject.name + ".LODSwitcher: No lodMeshes/lodGameObjects set. LODSwitcher is now disabled.");
            enabled = false;
        }
        int nrOfLevels = 0;
        if (lodMeshes != null) nrOfLevels = lodMeshes.Length - 1;
        if (lodGameObjects != null) nrOfLevels = Mathf.Max(nrOfLevels, lodGameObjects.Length - 1);
        if (enabled && (lodScreenSizes == null || lodScreenSizes.Length != nrOfLevels))
        {
            Debug.LogWarning(gameObject.name + ".LODSwitcher: lodScreenSizes should have a length of " + nrOfLevels + ". LODSwitcher is now disabled.");
            enabled = false;
        }
        SetLODLevel(0);
        ComputeDimensions();
        lodLevel = -1;
        ComputeLODLevel();
    }

    public void ComputeDimensions()
    {
        Bounds bounds = gameObject.GetWorldBounds();
        centerOffset = bounds.center - transform.position;
        objectSize = bounds.size.magnitude;
        if (skinnedMeshRenderer == null && meshFilter == null) GetMeshFilter();
        if (skinnedMeshRenderer != null)
        {
            bounds = skinnedMeshRenderer.localBounds;
            objectSize = bounds.size.magnitude;
            centerOffset = bounds.center;
            frameInterval = 1;
        }
        if (customCamera == null)
        {
            customCamera = Camera.main;
        }
        if (customCamera == null)
        {
            return;
        }
        Vector3 p0 = customCamera.ScreenToWorldPoint(new Vector3((Screen.width - 100f) / 2f, 0, 1f));
        Vector3 p1 = customCamera.ScreenToWorldPoint(new Vector3((Screen.width + 100f) / 2f, 0, 1f));
        pixelsPerMeter = 1f / (Vector3.Distance(p0, p1) / 100f);
    }
    public void SetCustomCamera(Camera aCamera)
    {
        customCamera = aCamera;
        ComputeDimensions();
    }

    public void SetFixedLODLevel(int aLevel)
    {
        fixedLODLevel = Mathf.Max(0, Mathf.Min(MaxLODLevel(), aLevel));
    }
    public void ReleaseFixedLODLevel()
    {
        fixedLODLevel = -1;
    }

    public int GetLODLevel()
    {
        return lodLevel;
    }

    public int MaxLODLevel()
    {
        if (lodScreenSizes == null) return 0;
        return lodScreenSizes.Length;
    }

    public Mesh GetMesh(int aLevel)
    {
        if (lodMeshes != null && lodMeshes.Length >= aLevel) return lodMeshes[aLevel];
        return null;
    }

    public void SetMesh(Mesh aMesh, int aLevel)
    {
        if (lodMeshes == null) lodMeshes = new Mesh[aLevel + 1];
        if (lodMeshes.Length <= aLevel) Array.Resize(ref lodMeshes, aLevel + 1);
        if (aLevel > 0)
        {
            if (lodScreenSizes == null) lodScreenSizes = new float[aLevel];
            if (lodScreenSizes.Length < aLevel) Array.Resize(ref lodScreenSizes, aLevel);
        }
        lodMeshes[aLevel] = aMesh;
        if (aLevel == lodLevel)
        {
            lodLevel = -1;
            SetLODLevel(aLevel);  // ensure we use the new model
        }
        ComputeDimensions();
    }

    public void SetRelativeScreenSize(float aValue, int aLevel)
    {
        if (lodScreenSizes == null) lodScreenSizes = new float[aLevel];
        if (lodScreenSizes.Length < aLevel) Array.Resize(ref lodScreenSizes, aLevel);
        for (int i = 0; i < lodScreenSizes.Length; i++)
        {
            if (i + 1 == aLevel) lodScreenSizes[i] = aValue;
            else if (lodScreenSizes[i] == 0f)
            {
                if (i == 0) lodScreenSizes[i] = 0.6f;
                else lodScreenSizes[i] = lodScreenSizes[i - 1] * 0.5f;
            }
        }
    }

    void Update()
    {
        if ((Time.frameCount + frameOffset) % frameInterval != 0) return;  // no need to do this every frame for every object in the scene
        ComputeLODLevel();
    }

    public Vector3 NearestCameraPositionForLOD(int aLevel)
    {
        ComputeDimensions();
        Camera cam = customCamera;
        if (cam == null) cam = Camera.main;
        if (aLevel > 0 && aLevel <= lodScreenSizes.Length)
        {
            float pixelSize = objectSize * pixelsPerMeter;
            float distance = pixelSize / Screen.width / lodScreenSizes[aLevel - 1];
            return transform.position + centerOffset + (cam.transform.rotation * (Vector3.back * distance));
        }
        return cam.transform.position;
    }


    public float ScreenPortion()
    {
        if (customCamera == null)
        {
            customCamera = Camera.main;
        }
        if (customCamera == null)
        {
            return 0f;
        }
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position + centerOffset);
        if (deactivateAtDistance > 0f && distance > deactivateAtDistance) return -1f;
        float pixelSize = objectSize * pixelsPerMeter;
        float screenPortion = pixelSize / distance / Screen.width;
        return Mathf.RoundToInt(screenPortion * 40f) * 0.025f;  // round to steps of 0.025 to prevent frequent switching back and forth
    }

    private void ComputeLODLevel()
    {
        int newLodLevel = 0;
        if (fixedLODLevel >= 0)
        {
            newLodLevel = fixedLODLevel;
        }
        else
        {
            float screenPortion = ScreenPortion();
            if (screenPortion >= 0f)
            {
                for (int i = 0; i < lodScreenSizes.Length; i++)
                {
                    if (screenPortion < lodScreenSizes[i]) newLodLevel++;
                }
            }
            else newLodLevel = -1;
        }
        if (newLodLevel != lodLevel) SetLODLevel(newLodLevel);
    }

    private void GetMeshFilter()
    {
        skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null) meshFilter = gameObject.GetComponent<MeshFilter>();
    }

    public void SetLODLevel(int newLodLevel)
    {
        if (newLodLevel != lodLevel)
        {
            newLodLevel = Mathf.Min(MaxLODLevel(), newLodLevel);
            if (newLodLevel < 0)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                if (lodMeshes != null && lodMeshes.Length > 0) gameObject.GetComponent<Renderer>().enabled = true;
                if (lodMeshes != null && lodMeshes.Length > newLodLevel)
                {
                    if (skinnedMeshRenderer == null && meshFilter == null) GetMeshFilter();
                    if (skinnedMeshRenderer != null) skinnedMeshRenderer.sharedMesh = lodMeshes[newLodLevel];
                    else if (meshFilter != null) meshFilter.sharedMesh = lodMeshes[newLodLevel];
                }
                for (int i = 0; lodGameObjects != null && i < lodGameObjects.Length; i++) lodGameObjects[i].SetActive(i == newLodLevel);
            }
            lodLevel = newLodLevel;
        }
    }
}
