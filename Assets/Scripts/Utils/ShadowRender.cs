using System;
using Framework.Managers;
using Game.Scene;
using UnityEngine;

public class ShadowRender : MonoBehaviour
{
    public void Start()
    {
        this._shadowRenderShader = Shader.Find("Dragon/Shaodow-Render");
        this._shadowReceiveShader = Shader.Find("Dragon/Shaodow-Receive");
        this.CreateShadowMapTexture();
        this.CreateShadowMapCamera();
        this.CreatShadowProjector();
    }

    private void OnDestroy()
    {
        if (this._rtShowmap != null)
        {
            this._rtShowmap.Release();
            UnityEngine.Object.Destroy(this._rtShowmap);
            this._rtShowmap = null;
        }
        if (this._shadowCamera != null && this._shadowCamera.gameObject != null)
        {
            UnityEngine.Object.Destroy(this._shadowCamera.gameObject);
            this._shadowCamera = null;
        }
    }

    private void OnPreRender()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo == null || this._shadowCamera == null || this._shadowProjector == null || this._rtShowmap == null || !string.IsNullOrEmpty(ManagerCenter.Instance.GetManager<CutSceneManager>().CurCutscene))
        {
            return;
        }
        this._shadowCamera.transform.rotation = Quaternion.Euler(ManagerCenter.Instance.GetManager<GameScene>().LightInfo.LightRot);
        this._shadowCamera.cullingMask = this._shadowCamCullLayers;
        this._shadowCamera.targetTexture = this._rtShowmap;
        this._shadowCamera.SetReplacementShader(this._shadowRenderShader, string.Empty);
        this._shadowCamera.Render();
        this._shadowProjector.GetComponent<Projector>().material.SetTexture("_ShadowTex", this._rtShowmap);
        this._shadowProjector.GetComponent<Projector>().material.SetVector("_cameraPos", -this._shadowCamera.transform.forward);
    }

    private void changeResolution(ShadowRender.ResolutionQuality quality)
    {
        this._showmapSize = (int)quality;
    }

    private void CreatShadowProjector()
    {
        this._shadowProjector = new GameObject("ShadowProjector").AddComponent<Projector>();
        this._shadowProjector.transform.parent = this._shadowCamera.transform;
        this._shadowProjector.transform.localPosition = Vector3.zero;
        this._shadowProjector.transform.localRotation = Quaternion.identity;
        this._shadowProjector.transform.localScale = Vector3.one;
        this._shadowProjector.orthographicSize = this._shadowCamViewSize;
        this._shadowProjector.orthographic = true;
        this._shadowProjector.farClipPlane = this._shadowCamFarPlane;
        this._shadowProjector.nearClipPlane = this._shadowCamNearPlane;
        this._shadowProjector.ignoreLayers = Const.LayerForMask.EveryLayer - Const.LayerForMask.Terrian;
        this._shadowProjector.material = new Material(this._shadowReceiveShader);
        Texture2D texture2D = Resources.Load<Texture2D>("Shader/ShadowMask");
        if (texture2D != null)
        {
            this._shadowProjector.material.SetTexture("_Mask", texture2D);
        }
        this._shadowProjector.gameObject.SetActive(false);
        this._shadowProjector.gameObject.SetActive(true);
    }

    private void CreateShadowMapTexture()
    {
        this.changeResolution(this.Quality);
        this._rtShowmap = new RenderTexture(this._showmapSize, this._showmapSize, 0, RenderTextureFormat.RGB565);
        this._rtShowmap.name = "shadow map " + base.GetInstanceID();
        this._rtShowmap.isPowerOfTwo = true;
        this._rtShowmap.hideFlags = HideFlags.HideAndDontSave;
        this._rtShowmap.Create();
        this._oldShowmapSize = this._showmapSize;
        this._rtShowmap.anisoLevel = 0;
        this._rtShowmap.wrapMode = TextureWrapMode.Clamp;
    }

    private void CreateShadowMapCamera()
    {
        GameObject gameObject = new GameObject("Shadow map Camera");
        this._shadowCamera = gameObject.AddComponent<Camera>();
        this._shadowCamera.transform.parent = this._centerTarget.transform;
        this._shadowCamera.transform.position = this._centerTarget.transform.position;
        this._shadowCamera.transform.localScale = Vector3.one;
        this._shadowCamera.enabled = false;
        this._shadowCamera.gameObject.AddComponent<FlareLayer>();
        this._shadowCamera.clearFlags = CameraClearFlags.Color;
        this._shadowCamera.backgroundColor = Color.white;
        this._shadowCamera.renderingPath = RenderingPath.Forward;
        this._shadowCamera.orthographic = true;
        this._shadowCamera.orthographicSize = this._shadowCamViewSize;
        this._shadowCamera.nearClipPlane = this._shadowCamNearPlane;
        this._shadowCamera.farClipPlane = this._shadowCamFarPlane;
        this._shadowCamera.depth = this._shadowCamDepth;
        this._shadowCamera.targetTexture = this._rtShowmap;
    }

    private float _shadowCamDepth = 10f;

    public float _shadowCamViewSize;

    private float _shadowCamFarPlane = 50f;

    private float _shadowCamNearPlane = -50f;

    public LayerMask _shadowCamCullLayers;

    protected Camera _shadowCamera;

    private float _shadowCamOffset = 10f;

    private RenderTexture _rtShowmap;

    private int _showmapSize;

    private int _oldShowmapSize;

    private Shader _shadowRenderShader;

    private Shader _shadowReceiveShader;

    private Projector _shadowProjector;

    public ShadowRender.ResolutionQuality Quality;

    public Transform _centerTarget;

    public enum ResolutionQuality
    {
        Low = 512,
        Medium = 1024,
        Hight = 2048,
        Ultra = 4096,
        UltraPlas = 8192
    }
}
