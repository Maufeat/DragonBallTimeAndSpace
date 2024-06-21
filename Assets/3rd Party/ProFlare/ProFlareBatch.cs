using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProFlareBatch : MonoBehaviour
{
    private void Reset()
    {
        if (this.helperTransform == null)
        {
            this.CreateHelperTransform();
        }
        this.mat = new Material(Shader.Find("ProFlares/Textured Flare Shader"));
        if (this.meshFilter == null)
        {
            this.meshFilter = base.GetComponent<MeshFilter>();
        }
        if (this.meshFilter == null)
        {
            this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
        }
        this.meshRender = base.gameObject.GetComponent<MeshRenderer>();
        if (this.meshRender == null)
        {
            this.meshRender = base.gameObject.AddComponent<MeshRenderer>();
        }
        if (this.FlareCamera == null)
        {
            this.FlareCamera = base.transform.root.GetComponentInChildren<Camera>();
        }
        if (this.meshRender != null)
        {
            this.meshRender.material = this.mat;
        }
        this.SetupMeshes();
        this.dirty = true;
    }

    private void Awake()
    {
        this.PI_Div180 = 0.0174532924f;
        this.Div180_PI = 57.2957764f;
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            this.overdrawDebug = false;
            this.dirty = true;
        }
        if (this.GameCamera == null)
        {
            GameObject gameObject = GameObject.FindWithTag("MainCamera");
            if (gameObject && gameObject.GetComponent<Camera>())
            {
                this.GameCamera = gameObject.GetComponent<Camera>();
            }
        }
        if (this.GameCamera != null)
        {
            this.GameCameraTrans = this.GameCamera.transform;
        }
        this.thisTransform = base.transform;
        this.SetupMeshes();
        if (this.Flares != null)
        {
            for (int i = 0; i < this.Flares.Count; i++)
            {
                if (this.Flares[i].neverCull)
                {
                    this.FlareOcclusionData[i]._CullingState = FlareOcclusion.CullingState.NeverCull;
                }
            }
        }
    }

    private void CreateMat()
    {
        this.mat = new Material(Shader.Find("ProFlares/Textured Flare Shader"));
        if (this.meshRender != null)
        {
            this.meshRender.material = this.mat;
            if (this._atlas != null && this._atlas.texture)
            {
                this.mat.mainTexture = this._atlas.texture;
            }
        }
    }

    public void SwitchCamera(Camera newCamera)
    {
        this.GameCamera = newCamera;
        this.GameCameraTrans = newCamera.transform;
        this.FixedUpdate();
        if (this.Flares != null)
        {
            for (int i = 0; i < this.Flares.Count; i++)
            {
                if (this.FlareOcclusionData[i] != null && this.FlareOcclusionData[i].occluded)
                {
                    this.FlareOcclusionData[i].occlusionScale = 0f;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            UnityEngine.Object.Destroy(this.helperTransform.gameObject);
            UnityEngine.Object.Destroy(this.mat);
        }
        else
        {
            UnityEngine.Object.DestroyImmediate(this.helperTransform.gameObject);
            UnityEngine.Object.DestroyImmediate(this.mat);
        }
        if (this.meshA != null)
        {
            UnityEngine.Object.DestroyImmediate(this.meshA);
            this.meshA = null;
        }
        if (this.meshB != null)
        {
            UnityEngine.Object.DestroyImmediate(this.meshB);
            this.meshB = null;
        }
        if (this.bufferMesh != null)
        {
            UnityEngine.Object.DestroyImmediate(this.bufferMesh);
            this.bufferMesh = null;
        }
    }

    public void AddFlare(ProFlare _flare)
    {
        bool flag = false;
        if (this.Flares != null)
        {
            for (int i = 0; i < this.Flares.Count; i++)
            {
                if (_flare == this.Flares[i])
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                this.Flares.Add(_flare);
                this.FlareOcclusionData = new FlareOcclusion[this.Flares.Count];
                for (int j = 0; j < this.FlareOcclusionData.Length; j++)
                {
                    this.FlareOcclusionData[j] = new FlareOcclusion();
                    if (_flare.neverCull)
                    {
                        this.FlareOcclusionData[j]._CullingState = FlareOcclusion.CullingState.NeverCull;
                    }
                }
                this.dirty = true;
            }
        }
    }

    private void CreateHelperTransform()
    {
        GameObject gameObject = new GameObject("_HelperTransform");
        this.helperTransform = gameObject.transform;
        this.helperTransform.parent = base.transform;
        this.helperTransform.localScale = Vector3.one;
        this.helperTransform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (this.thisTransform)
        {
            this.thisTransform.localPosition = Vector3.forward * this.zPos;
        }
        if (this.helperTransform == null)
        {
            this.CreateHelperTransform();
        }
        bool flag = false;
        if (this.meshA == null)
        {
            flag = true;
        }
        if (this.meshB == null)
        {
            flag = true;
        }
        if (flag)
        {
            this.SetupMeshes();
        }
        if (this.dirty)
        {
            this.ReBuildGeometry();
        }
    }

    private void LateUpdate()
    {
        if (this._atlas == null)
        {
            return;
        }
        if (!this.VR_Mode)
        {
            this.UpdateFlares();
        }
    }

    public void UpdateFlares()
    {
        if (this.meshA != null && this.meshB != null)
        {
            this.bufferMesh = ((!this.PingPong) ? this.meshB : this.meshA);
            this.PingPong = !this.PingPong;
            this.UpdateGeometry();
            this.bufferMesh.vertices = this.vertices;
            this.bufferMesh.colors32 = this.colors;
            this.meshFilter.sharedMesh = this.bufferMesh;
        }
    }

    public void ForceRefresh()
    {
        if (this.Flares != null)
        {
            this.Flares.Clear();
            ProFlare[] array = UnityEngine.Object.FindObjectsOfType(typeof(ProFlare)) as ProFlare[];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i]._Atlas == this._atlas)
                {
                    this.Flares.Add(array[i]);
                }
            }
            this.dirty = true;
        }
    }

    private void ReBuildGeometry()
    {
        if (this.FlareElements != null && this.Flares != null)
        {
            this.FlareElements.Clear();
            int num = 0;
            for (int i = 0; i < this.Flares.Count; i++)
            {
                for (int j = 0; j < this.Flares[i].Elements.Count; j++)
                {
                    if (this.FlareOcclusionData[i]._CullingState == FlareOcclusion.CullingState.CanCull)
                    {
                        this.FlareOcclusionData[i]._CullingState = FlareOcclusion.CullingState.Culled;
                        this.FlareOcclusionData[i].cullFader = 0f;
                    }
                    if (this.FlareOcclusionData[i]._CullingState != FlareOcclusion.CullingState.Culled)
                    {
                        num++;
                    }
                }
            }
            this.FlareElementsArray = new ProFlareElement[num];
            num = 0;
            for (int k = 0; k < this.Flares.Count; k++)
            {
                for (int l = 0; l < this.Flares[k].Elements.Count; l++)
                {
                    if (this.FlareOcclusionData[k]._CullingState != FlareOcclusion.CullingState.Culled)
                    {
                        this.FlareElementsArray[num] = this.Flares[k].Elements[l];
                        num++;
                    }
                }
            }
            UnityEngine.Object.DestroyImmediate(this.meshA);
            UnityEngine.Object.DestroyImmediate(this.meshA);
            UnityEngine.Object.DestroyImmediate(this.bufferMesh);
            this.meshA = null;
            this.meshB = null;
            this.bufferMesh = null;
            this.SetupMeshes();
            this.dirty = false;
        }
    }

    private void SetupMeshes()
    {
        if (this._atlas == null)
        {
            return;
        }
        if (this.FlareElementsArray == null)
        {
            return;
        }
        this.meshA = new Mesh();
        this.meshB = new Mesh();
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        for (int i = 0; i < this.FlareElementsArray.Length; i++)
        {
            ProFlareElement.Type type = this.FlareElementsArray[i].type;
            if (type != ProFlareElement.Type.Single)
            {
                if (type == ProFlareElement.Type.Multi)
                {
                    int count = this.FlareElementsArray[i].subElements.Count;
                    num += 4 * count;
                    num2 += 4 * count;
                    num3 += 4 * count;
                    num4 += 6 * count;
                }
            }
            else
            {
                num += 4;
                num2 += 4;
                num3 += 4;
                num4 += 6;
            }
        }
        this.vertices = new Vector3[num];
        this.uv = new Vector2[num2];
        this.colors = new Color32[num3];
        this.triangles = new int[num4];
        for (int j = 0; j < this.vertices.Length / 4; j++)
        {
            int num5 = j * 4;
            this.vertices[0 + num5] = new Vector3(1f, 1f, 0f);
            this.vertices[1 + num5] = new Vector3(1f, -1f, 0f);
            this.vertices[2 + num5] = new Vector3(-1f, 1f, 0f);
            this.vertices[3 + num5] = new Vector3(-1f, -1f, 0f);
        }
        int num6 = 0;
        for (int k = 0; k < this.FlareElementsArray.Length; k++)
        {
            ProFlareElement.Type type = this.FlareElementsArray[k].type;
            if (type != ProFlareElement.Type.Single)
            {
                if (type == ProFlareElement.Type.Multi)
                {
                    for (int l = 0; l < this.FlareElementsArray[k].subElements.Count; l++)
                    {
                        int num7 = (num6 + l) * 4;
                        Rect rect = this._atlas.elementsList[this.FlareElementsArray[k].elementTextureID].UV;
                        this.uv[0 + num7] = new Vector2(rect.xMax, rect.yMax);
                        this.uv[1 + num7] = new Vector2(rect.xMax, rect.yMin);
                        this.uv[2 + num7] = new Vector2(rect.xMin, rect.yMax);
                        this.uv[3 + num7] = new Vector2(rect.xMin, rect.yMin);
                    }
                    num6 += this.FlareElementsArray[k].subElements.Count;
                }
            }
            else
            {
                int num8 = num6 * 4;
                Rect rect2 = this._atlas.elementsList[this.FlareElementsArray[k].elementTextureID].UV;
                this.uv[0 + num8] = new Vector2(rect2.xMax, rect2.yMax);
                this.uv[1 + num8] = new Vector2(rect2.xMax, rect2.yMin);
                this.uv[2 + num8] = new Vector2(rect2.xMin, rect2.yMax);
                this.uv[3 + num8] = new Vector2(rect2.xMin, rect2.yMin);
                num6++;
            }
        }
        Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        for (int m = 0; m < this.colors.Length / 4; m++)
        {
            int num9 = m * 4;
            this.colors[0 + num9] = color;
            this.colors[1 + num9] = color;
            this.colors[2 + num9] = color;
            this.colors[3 + num9] = color;
        }
        for (int n = 0; n < this.triangles.Length / 6; n++)
        {
            int num10 = n * 4;
            int num11 = n * 6;
            this.triangles[0 + num11] = 0 + num10;
            this.triangles[1 + num11] = 1 + num10;
            this.triangles[2 + num11] = 2 + num10;
            this.triangles[3 + num11] = 2 + num10;
            this.triangles[4 + num11] = 1 + num10;
            this.triangles[5 + num11] = 3 + num10;
        }
        this.meshA.vertices = this.vertices;
        this.meshA.uv = this.uv;
        this.meshA.triangles = this.triangles;
        this.meshA.colors32 = this.colors;
        this.meshA.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
        this.meshB.vertices = this.vertices;
        this.meshB.uv = this.uv;
        this.meshB.triangles = this.triangles;
        this.meshB.colors32 = this.colors;
        this.meshB.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
    }

    private void FixedUpdate()
    {
        if (!this.dirty && this.Flares != null && this.FlareOcclusionData != null)
        {
            for (int i = 0; i < this.Flares.Count; i++)
            {
                this.FlareOcclusionData[i].occluded = false;
                if (this.Flares[i].RaycastPhysics)
                {
                    Vector3 direction = this.GameCameraTrans.position - this.Flares[i].thisTransform.position;
                    float maxDistance = Vector3.Distance(this.GameCameraTrans.position, this.Flares[i].thisTransform.position);
                    if (this.Flares[i].isVisible)
                    {
                        Ray ray = new Ray(this.Flares[i].thisTransform.position, direction);
                        RaycastHit raycastHit;
                        if (Physics.Raycast(ray, out raycastHit, maxDistance, this.Flares[i].mask))
                        {
                            this.FlareOcclusionData[i].occluded = true;
                        }
                    }
                }
            }
        }
    }

    private void UpdateGeometry()
    {
        if (this.GameCamera == null)
        {
            this.meshRender.enabled = false;
            return;
        }
        if (this.Flares != null)
        {
            this.meshRender.enabled = true;
            this.visibleFlares = 0;
            int num = 0;
            for (int i = 0; i < this.Flares.Count; i++)
            {
                this.Flares[i].LensPosition = this.GameCamera.WorldToViewportPoint(this.Flares[i].thisTransform.position);
                Vector3 vector = this.Flares[i].LensPosition;
                bool flag = vector.z > 0f && vector.x + this.Flares[i].OffScreenFadeDist > 0f && vector.x - this.Flares[i].OffScreenFadeDist < 1f && vector.y + this.Flares[i].OffScreenFadeDist > 0f && vector.y - this.Flares[i].OffScreenFadeDist < 1f;
                this.Flares[i].isVisible = flag;
                float num2 = 1f;
                if (vector.x <= 0f || vector.x >= 1f || vector.y <= 0f || vector.y >= 1f)
                {
                    float num3 = 1f / this.Flares[i].OffScreenFadeDist;
                    float a = 0f;
                    float b = 0f;
                    if (vector.x <= 0f || vector.x >= 1f)
                    {
                        a = ((vector.x <= 0.5f) ? Mathf.Abs(vector.x) : (vector.x - 1f));
                    }
                    if (vector.y <= 0f || vector.y >= 1f)
                    {
                        b = ((vector.y <= 0.5f) ? Mathf.Abs(vector.y) : (vector.y - 1f));
                    }
                    num2 = Mathf.Clamp01(num2 - Mathf.Max(a, b) * num3);
                }
                float num4 = 0f;
                if (vector.x > 0.5f - this.Flares[i].DynamicCenterRange && vector.x < 0.5f + this.Flares[i].DynamicCenterRange && vector.y > 0.5f - this.Flares[i].DynamicCenterRange && vector.y < 0.5f + this.Flares[i].DynamicCenterRange && this.Flares[i].DynamicCenterRange > 0f)
                {
                    float num5 = 1f / this.Flares[i].DynamicCenterRange;
                    num4 = 1f - Mathf.Max(Mathf.Abs(vector.x - 0.5f), Mathf.Abs(vector.y - 0.5f)) * num5;
                }
                float num6 = 0f;
                bool flag2 = vector.x > 0f + this.Flares[i].DynamicEdgeBias + this.Flares[i].DynamicEdgeRange && vector.x < 1f - this.Flares[i].DynamicEdgeBias - this.Flares[i].DynamicEdgeRange && vector.y > 0f + this.Flares[i].DynamicEdgeBias + this.Flares[i].DynamicEdgeRange && vector.y < 1f - this.Flares[i].DynamicEdgeBias - this.Flares[i].DynamicEdgeRange;
                bool flag3 = vector.x + this.Flares[i].DynamicEdgeRange > 0f + this.Flares[i].DynamicEdgeBias && vector.x - this.Flares[i].DynamicEdgeRange < 1f - this.Flares[i].DynamicEdgeBias && vector.y + this.Flares[i].DynamicEdgeRange > 0f + this.Flares[i].DynamicEdgeBias && vector.y - this.Flares[i].DynamicEdgeRange < 1f - this.Flares[i].DynamicEdgeBias;
                if (!flag2 && flag3)
                {
                    float num7 = 1f / this.Flares[i].DynamicEdgeRange;
                    float num8 = 0f;
                    float num9 = 0f;
                    bool flag4 = vector.x > 0f + this.Flares[i].DynamicEdgeBias + this.Flares[i].DynamicEdgeRange && vector.x < 1f - this.Flares[i].DynamicEdgeBias - this.Flares[i].DynamicEdgeRange;
                    bool flag5 = vector.x + this.Flares[i].DynamicEdgeRange > 0f + this.Flares[i].DynamicEdgeBias && vector.x - this.Flares[i].DynamicEdgeRange < 1f - this.Flares[i].DynamicEdgeBias;
                    bool flag6 = vector.y > 0f + this.Flares[i].DynamicEdgeBias + this.Flares[i].DynamicEdgeRange && vector.y < 1f - this.Flares[i].DynamicEdgeBias - this.Flares[i].DynamicEdgeRange;
                    bool flag7 = vector.y + this.Flares[i].DynamicEdgeRange > 0f + this.Flares[i].DynamicEdgeBias && vector.y - this.Flares[i].DynamicEdgeRange < 1f - this.Flares[i].DynamicEdgeBias;
                    if (!flag4 && flag5)
                    {
                        num8 = ((vector.x <= 0.5f) ? Mathf.Abs(vector.x - this.Flares[i].DynamicEdgeBias - this.Flares[i].DynamicEdgeRange) : (vector.x - 1f + this.Flares[i].DynamicEdgeBias + this.Flares[i].DynamicEdgeRange));
                        num8 = num8 * num7 * 0.5f;
                    }
                    if (!flag6 && flag7)
                    {
                        num9 = ((vector.y <= 0.5f) ? Mathf.Abs(vector.y - this.Flares[i].DynamicEdgeBias - this.Flares[i].DynamicEdgeRange) : (vector.y - 1f + this.Flares[i].DynamicEdgeBias + this.Flares[i].DynamicEdgeRange));
                        num9 = num9 * num7 * 0.5f;
                    }
                    num6 = Mathf.Max(num8, num9);
                }
                num6 = this.Flares[i].DynamicEdgeCurve.Evaluate(num6);
                float num10 = 1f;
                if (this.Flares[i].UseAngleLimit)
                {
                    Vector3 forward = this.Flares[i].thisTransform.forward;
                    Vector3 forward2 = this.GameCameraTrans.forward;
                    float num11 = Vector3.Angle(forward2, forward);
                    num11 = Mathf.Abs(Mathf.Clamp(180f - num11, -this.Flares[i].maxAngle, this.Flares[i].maxAngle));
                    num10 = 1f - num11 * (1f / (this.Flares[i].maxAngle * 0.5f));
                    if (this.Flares[i].UseAngleCurve)
                    {
                        num10 = this.Flares[i].AngleCurve.Evaluate(num10);
                    }
                }
                float num12 = 1f;
                if (this.Flares[i].useMaxDistance)
                {
                    Vector3 lhs = this.Flares[i].thisTransform.position - this.GameCameraTrans.position;
                    float num13 = Vector3.Dot(lhs, this.GameCameraTrans.forward);
                    float num14 = 1f - num13 / this.Flares[i].GlobalMaxDistance;
                    num12 = 1f * num14;
                    if (num12 < 0.001f)
                    {
                        this.Flares[i].isVisible = false;
                    }
                }
                if (!this.dirty && this.FlareOcclusionData[i] != null)
                {
                    if (this.FlareOcclusionData[i].occluded)
                    {
                        this.FlareOcclusionData[i].occlusionScale = Mathf.Lerp(this.FlareOcclusionData[i].occlusionScale, 0f, Time.deltaTime * 16f);
                    }
                    else
                    {
                        this.FlareOcclusionData[i].occlusionScale = Mathf.Lerp(this.FlareOcclusionData[i].occlusionScale, 1f, Time.deltaTime * 16f);
                    }
                }
                if (!this.Flares[i].isVisible)
                {
                    num2 = 0f;
                }
                float num15 = 1f;
                if (this.FlareCamera)
                {
                    this.helperTransform.position = this.FlareCamera.ViewportToWorldPoint(vector);
                }
                vector = this.helperTransform.localPosition;
                if (!this.VR_Mode && !this.SingleCamera_Mode)
                {
                    vector.z = 0f;
                }
                for (int j = 0; j < this.Flares[i].Elements.Count; j++)
                {
                    ProFlareElement proFlareElement = this.Flares[i].Elements[j];
                    Color globalTintColor = this.Flares[i].GlobalTintColor;
                    if (flag)
                    {
                        ProFlareElement.Type type = proFlareElement.type;
                        if (type != ProFlareElement.Type.Single)
                        {
                            if (type == ProFlareElement.Type.Multi)
                            {
                                for (int k = 0; k < proFlareElement.subElements.Count; k++)
                                {
                                    SubElement subElement = proFlareElement.subElements[k];
                                    subElement.colorFinal.r = subElement.color.r * globalTintColor.r;
                                    subElement.colorFinal.g = subElement.color.g * globalTintColor.g;
                                    subElement.colorFinal.b = subElement.color.b * globalTintColor.b;
                                    float num16 = subElement.color.a * globalTintColor.a;
                                    if (this.Flares[i].useDynamicEdgeBoost)
                                    {
                                        if (proFlareElement.OverrideDynamicEdgeBrightness)
                                        {
                                            num16 += proFlareElement.DynamicEdgeBrightnessOverride * num6;
                                        }
                                        else
                                        {
                                            num16 += this.Flares[i].DynamicEdgeBrightness * num6;
                                        }
                                    }
                                    if (this.Flares[i].useDynamicCenterBoost)
                                    {
                                        if (proFlareElement.OverrideDynamicCenterBrightness)
                                        {
                                            num16 += proFlareElement.DynamicCenterBrightnessOverride * num4;
                                        }
                                        else
                                        {
                                            num16 += this.Flares[i].DynamicCenterBrightness * num4;
                                        }
                                    }
                                    if (this.Flares[i].UseAngleBrightness)
                                    {
                                        num16 *= num10;
                                    }
                                    if (this.Flares[i].useDistanceFade)
                                    {
                                        num16 *= num12;
                                    }
                                    num16 *= this.FlareOcclusionData[i].occlusionScale;
                                    num16 *= this.FlareOcclusionData[i].cullFader;
                                    num16 *= num2;
                                    subElement.colorFinal.a = num16;
                                }
                            }
                        }
                        else
                        {
                            proFlareElement.ElementFinalColor.r = proFlareElement.ElementTint.r * globalTintColor.r;
                            proFlareElement.ElementFinalColor.g = proFlareElement.ElementTint.g * globalTintColor.g;
                            proFlareElement.ElementFinalColor.b = proFlareElement.ElementTint.b * globalTintColor.b;
                            float num16 = proFlareElement.ElementTint.a * globalTintColor.a;
                            if (this.Flares[i].useDynamicEdgeBoost)
                            {
                                if (proFlareElement.OverrideDynamicEdgeBrightness)
                                {
                                    num16 += proFlareElement.DynamicEdgeBrightnessOverride * num6;
                                }
                                else
                                {
                                    num16 += this.Flares[i].DynamicEdgeBrightness * num6;
                                }
                            }
                            if (this.Flares[i].useDynamicCenterBoost)
                            {
                                if (proFlareElement.OverrideDynamicCenterBrightness)
                                {
                                    num16 += proFlareElement.DynamicCenterBrightnessOverride * num4;
                                }
                                else
                                {
                                    num16 += this.Flares[i].DynamicCenterBrightness * num4;
                                }
                            }
                            if (this.Flares[i].UseAngleBrightness)
                            {
                                num16 *= num10;
                            }
                            if (this.Flares[i].useDistanceFade)
                            {
                                num16 *= num12;
                            }
                            num16 *= this.FlareOcclusionData[i].occlusionScale;
                            num16 *= this.FlareOcclusionData[i].cullFader;
                            num16 *= num2;
                            proFlareElement.ElementFinalColor.a = num16;
                        }
                    }
                    else
                    {
                        ProFlareElement.Type type = proFlareElement.type;
                        if (type != ProFlareElement.Type.Single)
                        {
                            if (type == ProFlareElement.Type.Multi)
                            {
                                num15 = 0f;
                            }
                        }
                        else
                        {
                            num15 = 0f;
                        }
                    }
                    float num17 = num15;
                    if (this.Flares[i].useDynamicEdgeBoost)
                    {
                        if (proFlareElement.OverrideDynamicEdgeBoost)
                        {
                            num17 += num6 * proFlareElement.DynamicEdgeBoostOverride;
                        }
                        else
                        {
                            num17 += num6 * this.Flares[i].DynamicEdgeBoost;
                        }
                    }
                    if (this.Flares[i].useDynamicCenterBoost)
                    {
                        if (proFlareElement.OverrideDynamicCenterBoost)
                        {
                            num17 += proFlareElement.DynamicCenterBoostOverride * num4;
                        }
                        else
                        {
                            num17 += this.Flares[i].DynamicCenterBoost * num4;
                        }
                    }
                    if (num17 < 0f)
                    {
                        num17 = 0f;
                    }
                    if (this.Flares[i].UseAngleScale)
                    {
                        num17 *= num10;
                    }
                    if (this.Flares[i].useDistanceScale)
                    {
                        num17 *= num12;
                    }
                    num17 *= this.FlareOcclusionData[i].occlusionScale;
                    if (!proFlareElement.Visible)
                    {
                        num17 = 0f;
                    }
                    if (!flag)
                    {
                        num17 = 0f;
                    }
                    proFlareElement.ScaleFinal = num17;
                    if (flag)
                    {
                        ProFlareElement.Type type = proFlareElement.type;
                        if (type != ProFlareElement.Type.Single)
                        {
                            if (type == ProFlareElement.Type.Multi)
                            {
                                for (int l = 0; l < proFlareElement.subElements.Count; l++)
                                {
                                    SubElement subElement2 = proFlareElement.subElements[l];
                                    if (proFlareElement.useRangeOffset)
                                    {
                                        Vector3 vector2 = vector * -subElement2.position;
                                        float z = vector.z;
                                        if (this.VR_Mode)
                                        {
                                            float num18 = subElement2.position * -1f - 1f;
                                            z = vector.z * (num18 * this.VR_Depth + 1f);
                                        }
                                        Vector3 vector3 = new Vector3(Mathf.Lerp(vector2.x, vector.x, proFlareElement.Anamorphic.x), Mathf.Lerp(vector2.y, vector.y, proFlareElement.Anamorphic.y), z);
                                        vector3 += proFlareElement.OffsetPostion;
                                        subElement2.offset = vector3;
                                    }
                                    else
                                    {
                                        subElement2.offset = vector * -proFlareElement.position;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Vector3 vector4 = vector * -proFlareElement.position;
                            float z2 = vector.z;
                            if (this.VR_Mode)
                            {
                                float num19 = proFlareElement.position * -1f - 1f;
                                z2 = vector.z * (num19 * this.VR_Depth + 1f);
                            }
                            Vector3 vector5 = new Vector3(Mathf.Lerp(vector4.x, vector.x, proFlareElement.Anamorphic.x), Mathf.Lerp(vector4.y, vector.y, proFlareElement.Anamorphic.y), z2);
                            vector5 += proFlareElement.OffsetPostion;
                            proFlareElement.OffsetPosition = vector5;
                        }
                    }
                    float num20 = 0f;
                    if (proFlareElement.rotateToFlare)
                    {
                        num20 = this.Div180_PI * Mathf.Atan2(vector.y, vector.x);
                    }
                    num20 += vector.x * proFlareElement.rotationSpeed;
                    num20 += vector.y * proFlareElement.rotationSpeed;
                    num20 += Time.time * proFlareElement.rotationOverTime;
                    proFlareElement.FinalAngle = proFlareElement.angle + num20;
                }
                if (!this.Flares[i].neverCull && this.useCulling)
                {
                    FlareOcclusion.CullingState cullingState = this.FlareOcclusionData[i]._CullingState;
                    if (this.Flares[i].isVisible)
                    {
                        this.visibleFlares++;
                        if (this.FlareOcclusionData[i].occluded)
                        {
                            if (cullingState == FlareOcclusion.CullingState.Visible)
                            {
                                this.FlareOcclusionData[i].CullTimer = (float)this.cullFlaresAfterTime;
                                cullingState = FlareOcclusion.CullingState.CullCountDown;
                            }
                        }
                        else
                        {
                            if (cullingState == FlareOcclusion.CullingState.Culled)
                            {
                                this.culledFlaresNowVisiable = true;
                            }
                            cullingState = FlareOcclusion.CullingState.Visible;
                        }
                    }
                    else if (cullingState == FlareOcclusion.CullingState.Visible)
                    {
                        this.FlareOcclusionData[i].CullTimer = (float)this.cullFlaresAfterTime;
                        cullingState = FlareOcclusion.CullingState.CullCountDown;
                    }
                    FlareOcclusion.CullingState cullingState2 = cullingState;
                    if (cullingState2 != FlareOcclusion.CullingState.Visible)
                    {
                        if (cullingState2 == FlareOcclusion.CullingState.CullCountDown)
                        {
                            this.FlareOcclusionData[i].CullTimer = this.FlareOcclusionData[i].CullTimer - Time.deltaTime;
                            if (this.FlareOcclusionData[i].CullTimer < 0f)
                            {
                                cullingState = FlareOcclusion.CullingState.CanCull;
                            }
                        }
                    }
                    if (cullingState != FlareOcclusion.CullingState.Culled)
                    {
                        this.FlareOcclusionData[i].cullFader = Mathf.Clamp01(this.FlareOcclusionData[i].cullFader + Time.deltaTime);
                    }
                    if (cullingState == FlareOcclusion.CullingState.CanCull)
                    {
                        num++;
                    }
                    this.FlareOcclusionData[i]._CullingState = cullingState;
                }
                this.reshowCulledFlaresTimer += Time.deltaTime;
                if (this.reshowCulledFlaresTimer > this.reshowCulledFlaresAfter)
                {
                    this.reshowCulledFlaresTimer = 0f;
                    if (this.culledFlaresNowVisiable)
                    {
                        this.dirty = true;
                        this.culledFlaresNowVisiable = false;
                    }
                }
                if (this.dirty || num >= this.cullFlaresAfterCount)
                {
                }
            }
            int num21 = 0;
            for (int m = 0; m < this.FlareElementsArray.Length; m++)
            {
                ProFlareElement.Type type = this.FlareElementsArray[m].type;
                if (type != ProFlareElement.Type.Single)
                {
                    if (type == ProFlareElement.Type.Multi)
                    {
                        for (int n = 0; n < this.FlareElementsArray[m].subElements.Count; n++)
                        {
                            int num22 = (num21 + n) * 4;
                            if (this.FlareElementsArray[m].flare.DisabledPlayMode)
                            {
                                this.vertices[0 + num22] = Vector3.zero;
                                this.vertices[1 + num22] = Vector3.zero;
                                this.vertices[2 + num22] = Vector3.zero;
                                this.vertices[3 + num22] = Vector3.zero;
                            }
                            else
                            {
                                this._scale = this.FlareElementsArray[m].size * this.FlareElementsArray[m].Scale * 0.01f * this.FlareElementsArray[m].flare.GlobalScale * this.FlareElementsArray[m].subElements[n].scale * this.FlareElementsArray[m].ScaleFinal;
                                if (this._scale.x < 0f || this._scale.y < 0f)
                                {
                                    this._scale = Vector3.zero;
                                }
                                Vector3 offset = this.FlareElementsArray[m].subElements[n].offset;
                                float num23 = this.FlareElementsArray[m].FinalAngle;
                                num23 += this.FlareElementsArray[m].subElements[n].angle;
                                this._color = this.FlareElementsArray[m].subElements[n].colorFinal;
                                if (this.useBrightnessThreshold)
                                {
                                    if ((int)this._color.a < this.BrightnessThreshold)
                                    {
                                        this._scale = Vector2.zero;
                                    }
                                    else if ((int)(this._color.r + this._color.g + this._color.b) < this.BrightnessThreshold)
                                    {
                                        this._scale = Vector2.zero;
                                    }
                                }
                                if (this.overdrawDebug)
                                {
                                    this._color = new Color32(20, 20, 20, 100);
                                }
                                if (!this.FlareElementsArray[m].flare.DisabledPlayMode)
                                {
                                    float f = num23 * this.PI_Div180;
                                    float num24 = Mathf.Cos(f);
                                    float num25 = Mathf.Sin(f);
                                    this.vertices[0 + num22] = new Vector3(num24 * (1f * this._scale.x) - num25 * (1f * this._scale.y), num25 * (1f * this._scale.x) + num24 * (1f * this._scale.y), 0f) + offset;
                                    this.vertices[1 + num22] = new Vector3(num24 * (1f * this._scale.x) - num25 * (-1f * this._scale.y), num25 * (1f * this._scale.x) + num24 * (-1f * this._scale.y), 0f) + offset;
                                    this.vertices[2 + num22] = new Vector3(num24 * (-1f * this._scale.x) - num25 * (1f * this._scale.y), num25 * (-1f * this._scale.x) + num24 * (1f * this._scale.y), 0f) + offset;
                                    this.vertices[3 + num22] = new Vector3(num24 * (-1f * this._scale.x) - num25 * (-1f * this._scale.y), num25 * (-1f * this._scale.x) + num24 * (-1f * this._scale.y), 0f) + offset;
                                }
                                Color32 color = this._color;
                                this.colors[0 + num22] = color;
                                this.colors[1 + num22] = color;
                                this.colors[2 + num22] = color;
                                this.colors[3 + num22] = color;
                            }
                        }
                        num21 += this.FlareElementsArray[m].subElements.Count;
                    }
                }
                else
                {
                    int num26 = num21 * 4;
                    if (this.FlareElementsArray[m].flare.DisabledPlayMode)
                    {
                        this.vertices[0 + num26] = Vector3.zero;
                        this.vertices[1 + num26] = Vector3.zero;
                        this.vertices[2 + num26] = Vector3.zero;
                        this.vertices[3 + num26] = Vector3.zero;
                    }
                    this._scale = this.FlareElementsArray[m].size * this.FlareElementsArray[m].Scale * 0.01f * this.FlareElementsArray[m].flare.GlobalScale * this.FlareElementsArray[m].ScaleFinal;
                    if (this._scale.x < 0f || this._scale.y < 0f)
                    {
                        this._scale = Vector3.zero;
                    }
                    Vector3 offsetPosition = this.FlareElementsArray[m].OffsetPosition;
                    float finalAngle = this.FlareElementsArray[m].FinalAngle;
                    this._color = this.FlareElementsArray[m].ElementFinalColor;
                    if (this.useBrightnessThreshold)
                    {
                        if ((int)this._color.a < this.BrightnessThreshold)
                        {
                            this._scale = Vector2.zero;
                        }
                        else if ((int)(this._color.r + this._color.g + this._color.b) < this.BrightnessThreshold)
                        {
                            this._scale = Vector2.zero;
                        }
                    }
                    if (this.overdrawDebug)
                    {
                        this._color = new Color32(20, 20, 20, 100);
                    }
                    if (!this.FlareElementsArray[m].flare.DisabledPlayMode)
                    {
                        float f2 = finalAngle * this.PI_Div180;
                        float num27 = Mathf.Cos(f2);
                        float num28 = Mathf.Sin(f2);
                        this.vertices[0 + num26] = new Vector3(num27 * (1f * this._scale.x) - num28 * (1f * this._scale.y), num28 * (1f * this._scale.x) + num27 * (1f * this._scale.y), 0f) + offsetPosition;
                        this.vertices[1 + num26] = new Vector3(num27 * (1f * this._scale.x) - num28 * (-1f * this._scale.y), num28 * (1f * this._scale.x) + num27 * (-1f * this._scale.y), 0f) + offsetPosition;
                        this.vertices[2 + num26] = new Vector3(num27 * (-1f * this._scale.x) - num28 * (1f * this._scale.y), num28 * (-1f * this._scale.x) + num27 * (1f * this._scale.y), 0f) + offsetPosition;
                        this.vertices[3 + num26] = new Vector3(num27 * (-1f * this._scale.x) - num28 * (-1f * this._scale.y), num28 * (-1f * this._scale.x) + num27 * (-1f * this._scale.y), 0f) + offsetPosition;
                    }
                    Color32 color2 = this._color;
                    this.colors[0 + num26] = color2;
                    this.colors[1 + num26] = color2;
                    this.colors[2 + num26] = color2;
                    this.colors[3 + num26] = color2;
                    num21++;
                }
            }
        }
    }

    public ProFlareBatch.Mode mode;

    public ProFlareAtlas _atlas;

    public List<ProFlare> Flares = new List<ProFlare>();

    public List<ProFlareElement> FlareElements = new List<ProFlareElement>();

    public ProFlareElement[] FlareElementsArray;

    public Camera GameCamera;

    public Transform GameCameraTrans;

    public Camera FlareCamera;

    public Transform FlareCameraTrans;

    public MeshFilter meshFilter;

    public Transform thisTransform;

    public MeshRenderer meshRender;

    public float zPos;

    public Mesh bufferMesh;

    public Mesh meshA;

    public Mesh meshB;

    private bool PingPong;

    public Material mat;

    private Vector3[] vertices;

    private Vector2[] uv;

    private Color32[] colors;

    private int[] triangles;

    public FlareOcclusion[] FlareOcclusionData;

    public bool useBrightnessThreshold = true;

    public int BrightnessThreshold = 1;

    public bool overdrawDebug;

    public bool dirty;

    public bool useCulling = true;

    public int cullFlaresAfterTime = 5;

    public int cullFlaresAfterCount = 5;

    public bool culledFlaresNowVisiable;

    private float reshowCulledFlaresTimer;

    public float reshowCulledFlaresAfter = 0.3f;

    public Transform helperTransform;

    public bool showAllConnectedFlares;

    public bool VR_Mode;

    public float VR_Depth = 0.2f;

    public bool SingleCamera_Mode;

    private Vector3[] verts;

    private Vector2 _scale;

    private Color32 _color;

    private float PI_Div180;

    private float Div180_PI;

    private int visibleFlares;

    public enum Mode
    {
        Standard,
        SingleCamera,
        VR
    }
}
