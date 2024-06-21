using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class SkillSelectRangeEffect : IFFComponent
{
    private static Shader SelectShader
    {
        get
        {
            if (SkillSelectRangeEffect._selectShader == null)
            {
                SkillSelectRangeEffect._selectShader = Resources.Load<Shader>("Shader/SkillProjector");
            }
            return SkillSelectRangeEffect._selectShader;
        }
    }

    private static Shader MaskShader
    {
        get
        {
            if (SkillSelectRangeEffect._maskShader == null)
            {
                SkillSelectRangeEffect._maskShader = Resources.Load<Shader>("Shader/MaskCustom");
            }
            return SkillSelectRangeEffect._maskShader;
        }
    }

    public void MoveCircle(Vector3 Pos, float radius, float Range, string SelectTexName, string RangeTexName)
    {
        if (this.Owner.ModelObj == null)
        {
            return;
        }
        this.CreatePrjector(this.RangePJ, "RangeProjector", Range, SkillSelectRangeEffect.MeshType.Circle, 360f, 0.1f, 6f);
        this.SetProjectorTexture(this.RangePJ, RangeTexName, 360f, SkillSelectRangeEffect.MeshType.Circle, Range);
        this.CreatePrjector(this.SelectPJ, "SelectProjector", radius, SkillSelectRangeEffect.MeshType.Circle, 360f, 0.1f, 6f);
        this.SetProjectorTexture(this.SelectPJ, SelectTexName, 360f, SkillSelectRangeEffect.MeshType.Circle, radius);
        this.SelectPJ.aspectRatio = 1f;
        this.TargetPos = Pos;
    }

    public Vector3 GetDireByOwner(Vector2 Inputdire, float size)
    {
        Vector3 vector = new Vector3(Inputdire.x, 0f, Inputdire.y);
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, CameraController.Self.Angle, 0f));
        vector = rotation * vector;
        vector *= size;
        vector.y = 0f;
        return vector;
    }

    public void MoveSector(Vector3 Pos, float radius, float angleDegree, string SelectTexName, string RangeTexName, string FlowerTexName)
    {
        if (this.Owner.ModelObj == null)
        {
            return;
        }
        this.CreatePrjector(this.RangePJ, "RangeProjector", radius, SkillSelectRangeEffect.MeshType.Sector, 360f, 0.1f, 6f);
        this.SetProjectorTexture(this.RangePJ, RangeTexName, 360f, SkillSelectRangeEffect.MeshType.Sector, radius);
        this.CreatePrjector(this.SelectPJ, "SelectProjector", radius, SkillSelectRangeEffect.MeshType.Sector, angleDegree, 0.1f, 6f);
        this.SetProjectorTexture(this.SelectPJ, SelectTexName, angleDegree, SkillSelectRangeEffect.MeshType.Sector, radius);
        this.SelectPJ.aspectRatio = 1f;
        this.TargetPos = Pos;
    }

    public void MoveRectangle(Vector3 Pos, float Length, float Width, string SelectTexName)
    {
        if (this.Owner.ModelObj == null)
        {
            return;
        }
        this.CreatePrjector(this.SelectPJ, "SelectProjector", Length / 2f, SkillSelectRangeEffect.MeshType.Rectangle, 360f, 0.1f, 6f);
        this.SetProjectorTexture(this.SelectPJ, SelectTexName, 360f, SkillSelectRangeEffect.MeshType.Rectangle, Length / 2f);
        this.SelectPJ.aspectRatio = Width / Length;
        this.TargetPos = Pos.normalized * Length;
    }

    public static void SetProjector(Projector projector, Material mat, float nearClipPlane, float farClipPlane, float radiusOrLength)
    {
        projector.transform.position = new Vector3(0f, 0f, 0f);
        projector.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        projector.nearClipPlane = nearClipPlane;
        projector.farClipPlane = farClipPlane;
        projector.ignoreLayers = (1 << Const.Layer.Wall | 1 << Const.Layer.Default | 1 << Const.Layer.Effect | 1 << Const.LayerForMask.FenceValue);
        projector.orthographic = true;
        projector.orthographicSize = radiusOrLength;
        projector.material = mat;
        projector.gameObject.SetActive(false);
    }

    private void SetBounds(Projector projector, float radiusOrLength, float angle, float nearClipPlane, float farClipPlane)
    {
        projector.transform.parent.rotation = Quaternion.Euler(Vector3.zero);
        GameObject gameObject = new GameObject();
        GameObject gameObject2 = new GameObject();
        gameObject.name = "leftbound";
        gameObject2.name = gameObject.name;
        gameObject2.transform.position = Vector3.zero;
        this.SelectorBoundLeft = gameObject.AddComponent<Projector>();
        Material material = new Material(SkillSelectRangeEffect.SelectShader);
        material.renderQueue = Const.RenderQueue.SkillSelect + 2;
        SkillSelectRangeEffect.SetProjector(this.SelectorBoundLeft, material, nearClipPlane, farClipPlane, radiusOrLength * SkillSelectRangeEffect.m_boundLength);
        gameObject.transform.parent = gameObject2.transform;
        gameObject2.transform.parent = projector.transform;
        gameObject2.transform.rotation = Quaternion.Euler(new Vector3(0f, -angle / 2f, 0f));
        this.SelectorBoundLeft.transform.localPosition = new Vector3(0f, 0f, radiusOrLength * SkillSelectRangeEffect.m_boundAddFoward);
        this.SelectorBoundLeft.aspectRatio = 0.05f;
        GameObject gameObject3 = new GameObject();
        GameObject gameObject4 = new GameObject();
        gameObject3.name = "rightbound";
        gameObject4.name = gameObject3.name;
        gameObject4.transform.position = Vector3.zero;
        this.SelectorBoundRight = gameObject3.AddComponent<Projector>();
        SkillSelectRangeEffect.SetProjector(this.SelectorBoundRight, material, nearClipPlane, farClipPlane, radiusOrLength * SkillSelectRangeEffect.m_boundLength);
        gameObject3.transform.parent = gameObject4.transform;
        gameObject4.transform.parent = projector.transform;
        gameObject4.transform.rotation = Quaternion.Euler(new Vector3(0f, angle / 2f, 0f));
        this.SelectorBoundRight.transform.localPosition = new Vector3(0f, 0f, radiusOrLength * SkillSelectRangeEffect.m_boundAddFoward);
        this.SelectorBoundRight.aspectRatio = 0.05f;
        this.SelectorBoundRight.gameObject.SetActive(false);
        this.SelectorBoundLeft.gameObject.SetActive(false);
    }

    private void CreatePrjector(Projector projector, string name, float radiusOrLength, SkillSelectRangeEffect.MeshType type, float angle, float nearClipPlane = 0.1f, float farClipPlane = 6f)
    {
        if (projector == null)
        {
            projector = new GameObject
            {
                name = name
            }.AddComponent<Projector>();
            Material material = new Material(SkillSelectRangeEffect.SelectShader);
            SkillSelectRangeEffect.SetProjector(projector, material, nearClipPlane, farClipPlane, radiusOrLength);
            if (name == "SelectProjector")
            {
                this.SelectPJ = projector;
                material.renderQueue = Const.RenderQueue.SkillSelect;
                GameObject gameObject = new GameObject();
                gameObject.name = name;
                gameObject.transform.position = Vector3.zero;
                gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                projector.transform.parent = gameObject.transform;
                if (type == SkillSelectRangeEffect.MeshType.Rectangle)
                {
                    projector.transform.localPosition = new Vector3(0f, 0f, radiusOrLength);
                }
                else
                {
                    projector.transform.localPosition = Vector3.zero;
                }
                if (type == SkillSelectRangeEffect.MeshType.Sector)
                {
                    this.SetBounds(projector, radiusOrLength, angle, nearClipPlane, farClipPlane);
                }
            }
            else if (name == "RangeProjector")
            {
                this.RangePJ = projector;
                material.renderQueue = Const.RenderQueue.SkillSelect - 1;
            }
            else
            {
                this.FlowerPJ = projector;
                this.FlowerPJ.transform.parent = this.SelectPJ.transform;
                this.FlowerPJ.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                material.renderQueue = Const.RenderQueue.SkillSelect + 1;
            }
            if (this.m_LastTexDataDic.ContainsKey(name))
            {
                this.m_LastTexDataDic[name].lastRadius = radiusOrLength;
                this.m_LastTexDataDic[name].lastType = type;
            }
            else
            {
                SkillSelectRangeEffect.LastTextureData lastTextureData = new SkillSelectRangeEffect.LastTextureData();
                lastTextureData.lastRadius = radiusOrLength;
                lastTextureData.lastType = type;
                this.m_LastTexDataDic.Add(name, lastTextureData);
            }
        }
        else if ((this.m_LastTexDataDic.ContainsKey(name) && this.m_LastTexDataDic[name].lastType != type) || (this.m_LastTexDataDic.ContainsKey(name) && !CommonTools.CheckFloatEqual(this.m_LastTexDataDic[name].lastRadius, radiusOrLength)))
        {
            projector.orthographicSize = radiusOrLength;
            if (name == "SelectProjector")
            {
                projector.transform.parent.transform.position = new Vector3(0f, 0f, 0f);
                projector.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                if (type == SkillSelectRangeEffect.MeshType.Rectangle)
                {
                    projector.transform.localPosition = new Vector3(0f, 0f, radiusOrLength);
                }
                else
                {
                    projector.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
                if (type == SkillSelectRangeEffect.MeshType.Sector)
                {
                    if (this.SelectorBoundRight == null)
                    {
                        this.SetBounds(projector, radiusOrLength, angle, nearClipPlane, farClipPlane);
                    }
                    Projector selectorBoundRight = this.SelectorBoundRight;
                    float orthographicSize = SkillSelectRangeEffect.m_boundLength * radiusOrLength;
                    this.SelectorBoundLeft.orthographicSize = orthographicSize;
                    selectorBoundRight.orthographicSize = orthographicSize;
                    Transform transform = this.SelectorBoundRight.transform;
                    Vector3 localPosition = new Vector3(0f, 0f, SkillSelectRangeEffect.m_boundAddFoward * radiusOrLength);
                    this.SelectorBoundLeft.transform.localPosition = localPosition;
                    transform.localPosition = localPosition;
                }
            }
            else
            {
                projector.transform.position = new Vector3(0f, 0f, 0f);
                projector.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            }
        }
        else if (projector.material.mainTexture != null && this.m_LastTexDataDic.ContainsKey(name) && this.m_LastTexDataDic[name].lastType == type && CommonTools.CheckFloatEqual(this.m_LastTexDataDic[name].lastRadius, radiusOrLength) && CommonTools.CheckFloatEqual(this.m_LastTexDataDic[name].lastTangle, angle) && !this.m_LastTexDataDic[name].isLoading)
        {
            projector.gameObject.SetActive(true);
            if (type == SkillSelectRangeEffect.MeshType.Sector && this.m_LastTexDataDic.ContainsKey(this.m_BoundString) && !this.m_LastTexDataDic[this.m_BoundString].isLoading)
            {
                if (this.SelectorBoundLeft != null && !this.SelectorBoundLeft.gameObject.activeSelf)
                {
                    this.SelectorBoundLeft.gameObject.SetActive(true);
                }
                if (this.SelectorBoundRight != null && !this.SelectorBoundRight.gameObject.activeSelf)
                {
                    this.SelectorBoundRight.gameObject.SetActive(true);
                }
            }
        }
    }

    private void SetProjectorTexture(Projector projector, string TexName, float angleOrWidth, SkillSelectRangeEffect.MeshType type, float radius)
    {
        if (projector == null)
        {
            return;
        }
        if (projector.material != null && (!this.m_LastTexDataDic.ContainsKey(projector.gameObject.name) || !CommonTools.CheckFloatEqual(this.m_LastTexDataDic[projector.gameObject.name].lastTangle, angleOrWidth) || this.m_LastTexDataDic[projector.gameObject.name].lastType != type || (projector.material.mainTexture == null && !this.m_LastTexDataDic[projector.gameObject.name].isLoading) || !CommonTools.CheckFloatEqual(this.m_LastTexDataDic[projector.gameObject.name].lastRadius, radius)))
        {
            projector.gameObject.SetActive(false);
            if (this.m_LastTexDataDic.ContainsKey(projector.gameObject.name))
            {
                this.m_LastTexDataDic[projector.gameObject.name].lastTangle = angleOrWidth;
                this.m_LastTexDataDic[projector.gameObject.name].lastRadius = radius;
                this.m_LastTexDataDic[projector.gameObject.name].lastType = type;
                this.m_LastTexDataDic[projector.gameObject.name].lastTexName = TexName;
                this.m_LastTexDataDic[projector.gameObject.name].isLoading = true;
            }
            else
            {
                SkillSelectRangeEffect.LastTextureData lastTextureData = new SkillSelectRangeEffect.LastTextureData();
                lastTextureData.lastTangle = angleOrWidth;
                lastTextureData.lastRadius = radius;
                lastTextureData.lastType = type;
                lastTextureData.lastTexName = TexName;
                lastTextureData.isLoading = true;
                this.m_LastTexDataDic.Add(projector.gameObject.name, lastTextureData);
            }
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.OTHERS, TexName, delegate (UITextureAsset item)
            {
                if (item == null)
                {
                    FFDebug.LogWarning(this, "    req  texture   is  null ");
                    return;
                }
                this.usedTextureAssets.Add(item);
                if (item.textureObj == null)
                {
                    return;
                }
                if (type == SkillSelectRangeEffect.MeshType.Sector && !CommonTools.CheckFloatEqual(angleOrWidth, 360f))
                {
                    if (this.m_LastTexDataDic.ContainsKey(this.m_BoundString))
                    {
                        this.m_LastTexDataDic[projector.gameObject.name].isLoading = true;
                    }
                    else
                    {
                        SkillSelectRangeEffect.LastTextureData lastTextureData2 = new SkillSelectRangeEffect.LastTextureData();
                        lastTextureData2.isLoading = true;
                        this.m_LastTexDataDic.Add(this.m_BoundString, lastTextureData2);
                    }
                    ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.OTHERS, "bound1", delegate (UITextureAsset item2)
                    {
                        if (item2 == null)
                        {
                            FFDebug.LogWarning(this, "    req  texture   is  null ");
                            return;
                        }
                        this.usedTextureAssets.Add(item2);
                        this.SelectorBoundLeft.material.mainTexture = item2.textureObj;
                        this.SelectorBoundRight.material.mainTexture = item2.textureObj;
                        this.SelectorBoundLeft.gameObject.SetActive(true);
                        this.SelectorBoundRight.gameObject.SetActive(true);
                        this.m_LastTexDataDic[this.m_BoundString].isLoading = false;
                    });
                    projector.material.mainTexture = TextureCoroutineLoad.GetTexture(item.textureObj, angleOrWidth, 6f);
                    projector.gameObject.SetActive(true);
                    this.m_LastTexDataDic[projector.gameObject.name].isLoading = false;
                }
                else
                {
                    item.textureObj.wrapMode = TextureWrapMode.Clamp;
                    projector.material.mainTexture = item.textureObj;
                    if (projector.material.mainTexture == null)
                    {
                        FFDebug.LogWarning(this, string.Concat(new object[]
                        {
                            "SkillProject null AngleOrwidth:",
                            angleOrWidth,
                            " texture:",
                            item.textureObj.name
                        }));
                    }
                    projector.gameObject.SetActive(true);
                    this.m_LastTexDataDic[projector.gameObject.name].isLoading = false;
                }
            });
        }
        this.SelectMeshType = type;
    }

    public void HidleALL()
    {
        this.SelectMeshType = SkillSelectRangeEffect.MeshType.None;
        if (this.SelectorBoundLeft != null)
        {
            this.SelectorBoundLeft.gameObject.SetActive(false);
        }
        if (this.SelectorBoundRight != null)
        {
            this.SelectorBoundRight.gameObject.SetActive(false);
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
    }

    public void CompUpdate()
    {
        if (this.SelectMeshType == SkillSelectRangeEffect.MeshType.None)
        {
            if (this.SelectPJ != null && this.SelectPJ.gameObject.activeSelf)
            {
                this.SelectPJ.gameObject.SetActive(false);
            }
            if (this.RangePJ != null && this.RangePJ.gameObject.activeSelf)
            {
                this.RangePJ.gameObject.SetActive(false);
            }
            if (this.FlowerPJ != null && this.FlowerPJ.gameObject.activeSelf)
            {
                this.FlowerPJ.gameObject.SetActive(false);
            }
            if (this.SelectorBoundLeft != null && this.SelectorBoundLeft.gameObject.activeSelf)
            {
                this.SelectorBoundLeft.gameObject.SetActive(false);
            }
            if (this.SelectorBoundRight != null && this.SelectorBoundRight.gameObject.activeSelf)
            {
                this.SelectorBoundRight.gameObject.SetActive(false);
            }
        }
        else if (this.SelectMeshType == SkillSelectRangeEffect.MeshType.Circle)
        {
            Vector3 position = this.Owner.ModelObj.transform.position;
            position.y += this.EffectHight;
            this.SelectPos = this.TargetPos;
            Vector3 targetPos = this.TargetPos;
            this.SelectPJ.transform.parent.position = new Vector3(targetPos.x, targetPos.y + this.justifyY, targetPos.z);
            this.RangePJ.transform.position = new Vector3(position.x, position.y + this.justifyY, position.z);
        }
        else if (this.SelectMeshType == SkillSelectRangeEffect.MeshType.Sector)
        {
            Vector3 position2 = this.Owner.ModelObj.transform.position;
            position2.y += this.EffectHight;
            Vector3 worldPosition = position2 + this.TargetPos;
            position2.y = worldPosition.y + this.justifyY;
            this.SelectPJ.transform.parent.position = position2;
            this.SelectPJ.transform.parent.LookAt(worldPosition);
            Vector3 localEulerAngles = this.SelectPJ.transform.parent.localEulerAngles;
            localEulerAngles.x = 0f;
            this.SelectPJ.transform.parent.localEulerAngles = localEulerAngles;
            this.SelectPos = position2 + this.TargetPos;
            this.RangePJ.transform.position = position2;
        }
        else if (this.SelectMeshType == SkillSelectRangeEffect.MeshType.Rectangle)
        {
            Vector3 position3 = this.Owner.ModelObj.transform.position;
            position3.y += this.EffectHight;
            Vector3 worldPosition2 = position3 + this.TargetPos;
            position3.y = worldPosition2.y + this.justifyY;
            this.SelectPJ.transform.parent.position = position3;
            this.SelectPJ.transform.parent.LookAt(worldPosition2);
            Vector3 localEulerAngles2 = this.SelectPJ.transform.parent.localEulerAngles;
            localEulerAngles2.x = 0f;
            this.SelectPJ.transform.parent.localEulerAngles = localEulerAngles2;
            this.SelectPos = position3 + this.TargetPos;
        }
    }

    public void CompDispose()
    {
        for (int i = 0; i < this.usedTextureAssets.Count; i++)
        {
            this.usedTextureAssets[i].TryUnload();
        }
        this.usedTextureAssets.Clear();
        this.m_LastTexDataDic.Clear();
    }

    public void ResetComp()
    {
    }

    private CharactorBase Owner;

    private Projector SelectPJ;

    private Projector RangePJ;

    private Projector FlowerPJ;

    private Projector SelectorBoundLeft;

    private Projector SelectorBoundRight;

    private SkillSelectRangeEffect.MeshType SelectMeshType;

    private float EffectHight = 0.1f;

    public Vector3 SelectPos;

    private static Shader _selectShader;

    private float justifyY = 3f;

    public static float m_boundAddFoward = 0.546f;

    public static float m_boundLength = 0.38f;

    private string m_BoundString = "BoundProjector";

    private static Shader _maskShader;

    private List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();

    private Vector3 TargetPos;

    private float Minangle = 0.1f;

    private Dictionary<string, SkillSelectRangeEffect.LastTextureData> m_LastTexDataDic = new Dictionary<string, SkillSelectRangeEffect.LastTextureData>();

    private enum MeshType
    {
        None,
        Circle,
        Sector,
        Rectangle
    }

    private class LastTextureData
    {
        public float lastTangle = -1f;

        public string lastTexName = string.Empty;

        public float lastRadius = -1f;

        public SkillSelectRangeEffect.MeshType lastType;

        public bool isLoading;
    }
}
