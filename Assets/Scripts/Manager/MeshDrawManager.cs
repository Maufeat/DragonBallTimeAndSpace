using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class MeshDrawManager
{
    public MeshDrawManager(GameObject warningEffect, float lifeTime)
    {
        this.WarningEffect = warningEffect;
        this.LifeTime = lifeTime;
    }

    public void CreateProjector(Transform parent, string picName, float radiusOrWidth, float angleOrHeight, MeshDrawManager.MeshType type, float nearClipPlane = 0.1f, float farClipPlane = 6f)
    {
        Projector projector = new GameObject
        {
            name = "WarningProjector",
            transform =
            {
                parent = parent
            }
        }.AddComponent<Projector>();
        Material material = new Material(Resources.Load<Shader>(this.ShaderName));
        material.renderQueue = Const.RenderQueue.SkillSelect;
        radiusOrWidth /= MeshDrawManager.Range2Unit;
        if (type == MeshDrawManager.MeshType.Rectangle)
        {
            SkillSelectRangeEffect.SetProjector(projector, material, nearClipPlane, farClipPlane, radiusOrWidth / 2f);
        }
        else
        {
            SkillSelectRangeEffect.SetProjector(projector, material, nearClipPlane, farClipPlane, radiusOrWidth);
        }
        if (type == MeshDrawManager.MeshType.Sector)
        {
            this.SetProjectorTexture(projector, picName, angleOrHeight, type, 0f, null, null);
        }
        if (type == MeshDrawManager.MeshType.Circle || type == MeshDrawManager.MeshType.Circle_Rand)
        {
            this.SetProjectorTexture(projector, picName, 360f, type, 0f, null, null);
        }
        else if (type == MeshDrawManager.MeshType.Rectangle)
        {
            angleOrHeight /= MeshDrawManager.Range2Unit;
            projector.aspectRatio = angleOrHeight / radiusOrWidth;
            projector.transform.localPosition = new Vector3(0f, 0f, radiusOrWidth / 2f);
            this.SetProjectorTexture(projector, picName, 360f, type, 0f, null, null);
        }
        else if (type == MeshDrawManager.MeshType.Ring)
        {
            this.SetProjectorTexture(projector, picName, 360f, type, angleOrHeight, null, null);
        }
    }

    private void SetProjectorTexture(Projector projector, string TexName, float angleOrWidth, MeshDrawManager.MeshType type, float innerRadius = 0f, Projector SelectorBoundLeft = null, Projector SelectorBoundRight = null)
    {
        if (projector == null)
        {
            return;
        }
        if (projector.material != null)
        {
            if (projector.material.mainTexture == null || projector.material.mainTexture.name != TexName.ToLower())
            {
                projector.gameObject.SetActive(false);
            }
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.OTHERS, TexName, delegate (UITextureAsset item)
            {
                if (item == null)
                {
                    FFDebug.LogWarning(this, "    req  texture   is  null ");
                    return;
                }
                if (projector == null || projector.gameObject == null)
                {
                    return;
                }
                this.usedTextureAssets.Add(item);
                if (item.textureObj == null)
                {
                    return;
                }
                if (type == MeshDrawManager.MeshType.Ring)
                {
                    TextureCoroutineLoad textureCoroutineLoad = TextureCoroutineLoad.Current;
                    textureCoroutineLoad.RunAsync(projector, item.textureObj, angleOrWidth, delegate
                    {
                        projector.gameObject.SetActive(true);
                    }, innerRadius);
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
                    return;
                }
                if (type == MeshDrawManager.MeshType.Sector && !CommonTools.CheckFloatEqual(angleOrWidth, 360f))
                {
                    TextureCoroutineLoad textureCoroutineLoad2 = TextureCoroutineLoad.Current;
                    textureCoroutineLoad2.RunAsync(projector, item.textureObj, angleOrWidth, delegate
                    {
                        projector.gameObject.SetActive(true);
                    }, 3f);
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
                }
            });
        }
    }

    public void SetTestAnim()
    {
        if (this.MeshObj == null)
        {
            return;
        }
        Animator animator = this.MeshObj.GetComponent<Animator>();
        if (animator == null)
        {
            animator = this.MeshObj.AddComponent<Animator>();
        }
        RuntimeAnimatorController runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("TestRes/TestMeshAnim");
        animator.runtimeAnimatorController = runtimeAnimatorController;
    }

    public void Updata()
    {
        this.RunningTime += Time.deltaTime;
    }

    public bool IsOver
    {
        get
        {
            return this.RunningTime > this.LifeTime;
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < this.usedTextureAssets.Count; i++)
        {
            this.usedTextureAssets[i].TryUnload();
        }
        this.usedTextureAssets.Clear();
        if (this.WarningEffect != null)
        {
            UnityEngine.Object.Destroy(this.WarningEffect);
        }
    }

    private const float StandardSize = 1f;

    private string ShaderName = "Shader/SkillProjector";

    private GameObject MeshObj;

    private MeshFilter MF;

    private string MeshName = "mf";

    public GameObject WarningEffect;

    public static float Range2Unit = 3f;

    public List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();

    private float RunningTime;

    private float LifeTime;

    public enum MeshType
    {
        None,
        Rectangle,
        Circle,
        Sector,
        Ring,
        Circle_Rand
    }
}
