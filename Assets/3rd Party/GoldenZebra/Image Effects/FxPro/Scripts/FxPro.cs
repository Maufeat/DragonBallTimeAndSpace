using System;
using System.Collections.Generic;
using FxProNS;
using UnityEngine;

// Token: 0x02000048 RID: 72
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/FxPro™")]
[ExecuteInEditMode]
public class FxPro : MonoBehaviour
{
    // Token: 0x1700004C RID: 76
    // (get) Token: 0x060001B6 RID: 438 RVA: 0x00060098 File Offset: 0x0005E298
    public static Material Mat
    {
        get
        {
            if (null == FxPro._mat)
            {
                FxPro._mat = new Material(Shader.Find("Hidden/FxPro"))
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
            return FxPro._mat;
        }
    }

    // Token: 0x1700004D RID: 77
    // (get) Token: 0x060001B7 RID: 439 RVA: 0x000600D8 File Offset: 0x0005E2D8
    private static Material TapMat
    {
        get
        {
            if (null == FxPro._tapMat)
            {
                FxPro._tapMat = new Material(Shader.Find("Hidden/FxProTap"))
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
            return FxPro._tapMat;
        }
    }

    // Token: 0x060001B8 RID: 440 RVA: 0x00060118 File Offset: 0x0005E318
    public void Start()
    {
        if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
        {
            base.enabled = false;
            return;
        }
        this._filmGrainTextures = new List<Texture2D>();
        for (int i = 1; i <= 4; i++)
        {
            string text = "filmgrain_0" + i;
            Texture2D texture2D = Resources.Load(text) as Texture2D;
            if (null == texture2D)
            {
                Debug.LogError("Unable to load grain texture '" + text + "'");
            }
            else
            {
                this._filmGrainTextures.Add(texture2D);
            }
        }
    }

    // Token: 0x060001B9 RID: 441 RVA: 0x000601B0 File Offset: 0x0005E3B0
    public void Init(bool searchForNonDepthmapAlphaObjects)
    {
        this.Quality = EffectsQuality.Fast;
        FxPro.Mat.SetFloat("_DirtIntensity", Mathf.Exp(this.LensDirtIntensity) - 1f);
        if (null == this.LensDirtTexture || this.LensDirtIntensity <= 0f)
        {
            FxPro.Mat.DisableKeyword("LENS_DIRT_ON");
            FxPro.Mat.EnableKeyword("LENS_DIRT_OFF");
        }
        else
        {
            FxPro.Mat.SetTexture("_LensDirtTex", this.LensDirtTexture);
            FxPro.Mat.EnableKeyword("LENS_DIRT_ON");
            FxPro.Mat.DisableKeyword("LENS_DIRT_OFF");
        }
        if (this.ChromaticAberration)
        {
            FxPro.Mat.EnableKeyword("CHROMATIC_ABERRATION_ON");
            FxPro.Mat.DisableKeyword("CHROMATIC_ABERRATION_OFF");
        }
        else
        {
            FxPro.Mat.EnableKeyword("CHROMATIC_ABERRATION_OFF");
            FxPro.Mat.DisableKeyword("CHROMATIC_ABERRATION_ON");
        }
        if (base.GetComponent<Camera>().hdr)
        {
            Shader.EnableKeyword("FXPRO_HDR_ON");
            Shader.DisableKeyword("FXPRO_HDR_OFF");
        }
        else
        {
            Shader.EnableKeyword("FXPRO_HDR_OFF");
            Shader.DisableKeyword("FXPRO_HDR_ON");
        }
        FxPro.Mat.SetFloat("_SCurveIntensity", this.SCurveIntensity);
        if (this.DOFEnabled)
        {
            if (null == this.DOFParams.EffectCamera)
            {
                this.DOFParams.EffectCamera = base.GetComponent<Camera>();
            }
            this.DOFParams.DepthCompression = Mathf.Clamp(this.DOFParams.DepthCompression, 2f, 8f);
            this.DOFParams.UseUnityDepthBuffer = true;
            Singleton<DOFHelper>.Instance.SetParams(this.DOFParams);
            Singleton<DOFHelper>.Instance.Init(searchForNonDepthmapAlphaObjects);
            FxPro.Mat.DisableKeyword("DOF_DISABLED");
            FxPro.Mat.EnableKeyword("DOF_ENABLED");
            if (!this.DOFParams.DoubleIntensityBlur)
            {
                Singleton<DOFHelper>.Instance.SetBlurRadius((this.Quality != EffectsQuality.Fastest && this.Quality != EffectsQuality.Fast) ? 3 : 2);
            }
            else
            {
                Singleton<DOFHelper>.Instance.SetBlurRadius((this.Quality != EffectsQuality.Fastest && this.Quality != EffectsQuality.Fast) ? 10 : 5);
            }
        }
        else
        {
            FxPro.Mat.EnableKeyword("DOF_DISABLED");
            FxPro.Mat.DisableKeyword("DOF_ENABLED");
        }
        if (this.BloomEnabled)
        {
            this.BloomParams.Quality = this.Quality;
            Singleton<BloomHelper>.Instance.SetParams(this.BloomParams);
            Singleton<BloomHelper>.Instance.Init();
            FxPro.Mat.DisableKeyword("BLOOM_DISABLED");
            FxPro.Mat.EnableKeyword("BLOOM_ENABLED");
        }
        else
        {
            FxPro.Mat.EnableKeyword("BLOOM_DISABLED");
            FxPro.Mat.DisableKeyword("BLOOM_ENABLED");
        }
        if (this.LensCurvatureEnabled)
        {
            this.UpdateLensCurvatureZoom();
            FxPro.Mat.SetFloat("_LensCurvatureBarrelPower", this.LensCurvaturePower);
        }
        if (this.FilmGrainIntensity >= 0.001f)
        {
            FxPro.Mat.SetFloat("_FilmGrainIntensity", this.FilmGrainIntensity);
            FxPro.Mat.SetFloat("_FilmGrainTiling", this.FilmGrainTiling);
            FxPro.Mat.EnableKeyword("FILM_GRAIN_ON");
            FxPro.Mat.DisableKeyword("FILM_GRAIN_OFF");
        }
        else
        {
            FxPro.Mat.EnableKeyword("FILM_GRAIN_OFF");
            FxPro.Mat.DisableKeyword("FILM_GRAIN_ON");
        }
        if (this.VignettingIntensity <= 1f)
        {
            FxPro.Mat.SetFloat("_VignettingIntensity", this.VignettingIntensity);
            FxPro.Mat.EnableKeyword("VIGNETTING_ON");
            FxPro.Mat.DisableKeyword("VIGNETTING_OFF");
        }
        else
        {
            FxPro.Mat.EnableKeyword("VIGNETTING_OFF");
            FxPro.Mat.DisableKeyword("VIGNETTING_ON");
        }
        FxPro.Mat.SetFloat("_ChromaticAberrationOffset", this.ChromaticAberrationOffset);
        if (this.ColorEffectsEnabled)
        {
            FxPro.Mat.EnableKeyword("COLOR_FX_ON");
            FxPro.Mat.DisableKeyword("COLOR_FX_OFF");
            FxPro.Mat.SetColor("_CloseTint", this.CloseTint);
            FxPro.Mat.SetColor("_FarTint", this.FarTint);
            FxPro.Mat.SetFloat("_CloseTintStrength", this.CloseTintStrength);
            FxPro.Mat.SetFloat("_FarTintStrength", this.FarTintStrength);
            FxPro.Mat.EnableKeyword("USE_CAMERA_DEPTH_TEXTURE");
            FxPro.Mat.DisableKeyword("DONT_USE_CAMERA_DEPTH_TEXTURE");
            FxPro.Mat.SetFloat("_OneOverDepthScale", this.OneOverDepthScale);
            FxPro.Mat.SetFloat("_DesaturateDarksStrength", this.DesaturateDarksStrength);
            FxPro.Mat.SetFloat("_DesaturateFarObjsStrength", this.DesaturateFarObjsStrength);
            FxPro.Mat.SetColor("_FogTint", this.FogTint);
            FxPro.Mat.SetFloat("_FogStrength", this.FogStrength);
        }
        else
        {
            FxPro.Mat.EnableKeyword("COLOR_FX_OFF");
            FxPro.Mat.DisableKeyword("COLOR_FX_ON");
        }
    }

    // Token: 0x060001BA RID: 442 RVA: 0x000043C6 File Offset: 0x000025C6
    public void OnEnable()
    {
        this.Init(true);
    }

    // Token: 0x060001BB RID: 443 RVA: 0x000043CF File Offset: 0x000025CF
    public void OnDisable()
    {
        if (null != FxPro.Mat)
        {
            global::UnityEngine.Object.DestroyImmediate(FxPro.Mat);
        }
        RenderTextureManager.Instance.Dispose();
        Singleton<DOFHelper>.Instance.Dispose();
        Singleton<BloomHelper>.Instance.Dispose();
    }

    // Token: 0x060001BC RID: 444 RVA: 0x00004409 File Offset: 0x00002609
    public void OnValidate()
    {
        this.Init(false);
    }

    // Token: 0x060001BD RID: 445 RVA: 0x000606DC File Offset: 0x0005E8DC
    public static RenderTexture DownsampleTex(RenderTexture input, float downsampleBy)
    {
        RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(Mathf.RoundToInt((float)input.width / downsampleBy), Mathf.RoundToInt((float)input.height / downsampleBy), input.depth, input.format);
        renderTexture.filterMode = FilterMode.Bilinear;
        Graphics.BlitMultiTap(input, renderTexture, FxPro.TapMat, FxPro.tmp);
        return renderTexture;
    }

    // Token: 0x060001BE RID: 446 RVA: 0x00060738 File Offset: 0x0005E938
    private RenderTexture ApplyColorEffects(RenderTexture input)
    {
        if (!this.ColorEffectsEnabled)
        {
            return input;
        }
        RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(input.width, input.height, input.depth, input.format);
        Graphics.Blit(input, renderTexture, FxPro.Mat, 5);
        return renderTexture;
    }

    // Token: 0x060001BF RID: 447 RVA: 0x00060784 File Offset: 0x0005E984
    private RenderTexture ApplyLensCurvature(RenderTexture input)
    {
        if (!this.LensCurvatureEnabled)
        {
            return input;
        }
        RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(input.width, input.height, input.depth, input.format);
        Graphics.Blit(input, renderTexture, FxPro.Mat, (!this.LensCurvaturePrecise) ? 4 : 3);
        return renderTexture;
    }

    // Token: 0x060001C0 RID: 448 RVA: 0x000607E0 File Offset: 0x0005E9E0
    private RenderTexture ApplyChromaticAberration(RenderTexture input)
    {
        if (!this.ChromaticAberration)
        {
            return null;
        }
        RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(input.width, input.height, input.depth, input.format);
        renderTexture.filterMode = FilterMode.Bilinear;
        Graphics.Blit(input, renderTexture, FxPro.Mat, 2);
        FxPro.Mat.SetTexture("_ChromAberrTex", renderTexture);
        return renderTexture;
    }

    // Token: 0x060001C1 RID: 449 RVA: 0x00060844 File Offset: 0x0005EA44
    private Vector2 ApplyLensCurvature(Vector2 uv, float barrelPower, bool precise)
    {
        uv = uv * 2f - Vector2.one;
        uv.x *= base.GetComponent<Camera>().aspect * 2f;
        float num = Mathf.Atan2(uv.y, uv.x);
        float num2 = uv.magnitude;
        if (precise)
        {
            num2 = Mathf.Pow(num2, barrelPower);
        }
        else
        {
            num2 = Mathf.Lerp(num2, num2 * num2, Mathf.Clamp01(barrelPower - 1f));
        }
        uv.x = num2 * Mathf.Cos(num);
        uv.y = num2 * Mathf.Sin(num);
        uv.x /= base.GetComponent<Camera>().aspect * 2f;
        return 0.5f * (uv + Vector2.one);
    }

    // Token: 0x060001C2 RID: 450 RVA: 0x00060920 File Offset: 0x0005EB20
    private void UpdateLensCurvatureZoom()
    {
        float num = 1f / this.ApplyLensCurvature(new Vector2(1f, 1f), this.LensCurvaturePower, this.LensCurvaturePrecise).x;
        FxPro.Mat.SetFloat("_LensCurvatureZoom", num);
    }

    // Token: 0x060001C3 RID: 451 RVA: 0x00060970 File Offset: 0x0005EB70
    private void UpdateFilmGrain()
    {
        if (this.FilmGrainIntensity >= 0.001f)
        {
            int num = global::UnityEngine.Random.Range(0, 3);
            FxPro.Mat.SetTexture("_FilmGrainTex", this._filmGrainTextures[num]);
            switch (global::UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    FxPro.Mat.SetVector("_FilmGrainChannel", new Vector4(1f, 0f, 0f, 0f));
                    break;
                case 1:
                    FxPro.Mat.SetVector("_FilmGrainChannel", new Vector4(0f, 1f, 0f, 0f));
                    break;
                case 2:
                    FxPro.Mat.SetVector("_FilmGrainChannel", new Vector4(0f, 0f, 1f, 0f));
                    break;
                case 3:
                    FxPro.Mat.SetVector("_FilmGrainChannel", new Vector4(0f, 0f, 0f, 1f));
                    break;
            }
        }
    }

    // Token: 0x060001C4 RID: 452 RVA: 0x00060A8C File Offset: 0x0005EC8C
    private void RenderEffects(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Bilinear;
        this.UpdateFilmGrain();
        RenderTexture renderTexture = source;
        RenderTexture renderTexture2 = this.ApplyColorEffects(source);
        RenderTextureManager.Instance.SafeAssign(ref renderTexture2, this.ApplyLensCurvature(renderTexture2));
        RenderTextureManager.Instance.SafeAssign(ref renderTexture, FxPro.DownsampleTex(renderTexture2, 4f));
        if (this.Quality == EffectsQuality.Fastest)
        {
            RenderTextureManager.Instance.SafeAssign(ref renderTexture, FxPro.DownsampleTex(renderTexture, 4f));
        }
        RenderTexture renderTexture3 = null;
        RenderTexture renderTexture4 = null;
        if (this.DOFEnabled)
        {
            if (null == this.DOFParams.EffectCamera)
            {
                return;
            }
            renderTexture3 = RenderTextureManager.Instance.RequestRenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
            Singleton<DOFHelper>.Instance.RenderCOCTexture(renderTexture, renderTexture3, (!this.BlurCOCTexture) ? 0f : 1.5f);
            if (this.VisualizeCOC)
            {
                Graphics.Blit(renderTexture3, destination, DOFHelper.Mat, 3);
                RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture3);
                RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
                return;
            }
            renderTexture4 = RenderTextureManager.Instance.RequestRenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
            Singleton<DOFHelper>.Instance.RenderDOFBlur(renderTexture, renderTexture4, renderTexture3);
            FxPro.Mat.SetTexture("_DOFTex", renderTexture4);
            FxPro.Mat.SetTexture("_COCTex", renderTexture3);
            Graphics.Blit(renderTexture4, destination);
        }
        if (this.BloomEnabled)
        {
            RenderTexture renderTexture5 = RenderTextureManager.Instance.RequestRenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
            Singleton<BloomHelper>.Instance.RenderBloomTexture(renderTexture, renderTexture5);
            FxPro.Mat.SetTexture("_BloomTex", renderTexture5);
            if (this.VisualizeBloom)
            {
                Graphics.Blit(renderTexture5, destination);
                return;
            }
        }
        Graphics.Blit(renderTexture2, destination, FxPro.Mat, 0);
        RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture3);
        RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture4);
        RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
        RenderTextureManager.Instance.ReleaseRenderTexture(source);
    }

    // Token: 0x060001C5 RID: 453 RVA: 0x00004412 File Offset: 0x00002612
    [ImageEffectTransformsToLDR]
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.RenderEffects(source, destination);
        RenderTextureManager.Instance.ReleaseAllRenderTextures();
    }

    // Token: 0x060001C6 RID: 454 RVA: 0x00060C9C File Offset: 0x0005EE9C
    public void ActiveDepthOfField(bool bActive)
    {
        this.DOFEnabled = bActive;
        if (bActive)
        {
            FxPro.Mat.DisableKeyword("DOF_DISABLED");
            FxPro.Mat.EnableKeyword("DOF_ENABLED");
            if (this.DOFParams != null)
            {
                this.DOFParams.AutoFocus = false;
            }
        }
        else
        {
            FxPro.Mat.EnableKeyword("DOF_DISABLED");
            FxPro.Mat.DisableKeyword("DOF_ENABLED");
        }
    }

    // Token: 0x040001BF RID: 447
    private const bool VisualizeLensCurvature = false;

    // Token: 0x040001C0 RID: 448
    public EffectsQuality Quality = EffectsQuality.Fast;

    // Token: 0x040001C1 RID: 449
    private static Material _mat;

    // Token: 0x040001C2 RID: 450
    private static Material _tapMat;

    // Token: 0x040001C3 RID: 451
    public bool BloomEnabled = true;

    // Token: 0x040001C4 RID: 452
    public BloomHelperParams BloomParams = new BloomHelperParams();

    // Token: 0x040001C5 RID: 453
    public bool VisualizeBloom;

    // Token: 0x040001C6 RID: 454
    public Texture2D LensDirtTexture;

    // Token: 0x040001C7 RID: 455
    [Range(0f, 2f)]
    public float LensDirtIntensity = 1f;

    // Token: 0x040001C8 RID: 456
    public bool ChromaticAberration = true;

    // Token: 0x040001C9 RID: 457
    public bool ChromaticAberrationPrecise;

    // Token: 0x040001CA RID: 458
    [Range(1f, 2.5f)]
    public float ChromaticAberrationOffset = 1f;

    // Token: 0x040001CB RID: 459
    [Range(0f, 1f)]
    public float SCurveIntensity = 0.5f;

    // Token: 0x040001CC RID: 460
    public bool LensCurvatureEnabled = true;

    // Token: 0x040001CD RID: 461
    [Range(1f, 2f)]
    public float LensCurvaturePower = 1.1f;

    // Token: 0x040001CE RID: 462
    public bool LensCurvaturePrecise;

    // Token: 0x040001CF RID: 463
    [Range(0f, 1f)]
    public float FilmGrainIntensity = 0.5f;

    // Token: 0x040001D0 RID: 464
    [Range(1f, 10f)]
    public float FilmGrainTiling = 4f;

    // Token: 0x040001D1 RID: 465
    [Range(0f, 1f)]
    public float VignettingIntensity = 0.5f;

    // Token: 0x040001D2 RID: 466
    public bool DOFEnabled = true;

    // Token: 0x040001D3 RID: 467
    public bool BlurCOCTexture = true;

    // Token: 0x040001D4 RID: 468
    public DOFHelperParams DOFParams = new DOFHelperParams();

    // Token: 0x040001D5 RID: 469
    public bool VisualizeCOC;

    // Token: 0x040001D6 RID: 470
    private Texture2D _gridTexture;

    // Token: 0x040001D7 RID: 471
    private List<Texture2D> _filmGrainTextures;

    // Token: 0x040001D8 RID: 472
    public bool ColorEffectsEnabled = true;

    // Token: 0x040001D9 RID: 473
    public Color CloseTint = new Color(1f, 0.5f, 0f, 1f);

    // Token: 0x040001DA RID: 474
    public Color FarTint = new Color(0f, 0f, 1f, 1f);

    // Token: 0x040001DB RID: 475
    [Range(0f, 1f)]
    public float CloseTintStrength = 0.5f;

    // Token: 0x040001DC RID: 476
    [Range(0f, 1f)]
    public float FarTintStrength = 0.5f;

    // Token: 0x040001DD RID: 477
    [Range(0f, 100f)]
    public float OneOverDepthScale;

    // Token: 0x040001DE RID: 478
    [Range(0f, 2f)]
    public float DesaturateDarksStrength = 0.5f;

    // Token: 0x040001DF RID: 479
    [Range(0f, 1f)]
    public float DesaturateFarObjsStrength = 0.5f;

    // Token: 0x040001E0 RID: 480
    public Color FogTint = Color.white;

    // Token: 0x040001E1 RID: 481
    [Range(0f, 1f)]
    public float FogStrength = 0.5f;

    // Token: 0x040001E2 RID: 482
    private static Vector2[] tmp = new Vector2[]
    {
        new Vector2(-1f, -1f),
        new Vector2(-1f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, -1f)
    };
}
