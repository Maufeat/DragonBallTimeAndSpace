//Unity Scatter DOF (disk blur): 13ms
//Unity Scatter DOF (DX11): 6ms
//FxPro DOF with bokeh: 5.2ms
//FxPro DOF no bokeh: 3.9ms

#define FXPRO_EFFECT
//#define DOFPRO_EFFECT

#if FXPRO_EFFECT
#define BLOOMPRO_EFFECT
#define DOFPRO_EFFECT
#endif

using System;
using UnityEngine;
using System.Collections.Generic;

using Object = UnityEngine.Object;

#if FXPRO_EFFECT
namespace FxProNS
{
#elif DOFPRO_EFFECT
namespace DOFProNS {
#endif
    [Serializable]
    public class DOFHelperParams
    {
        //Parameters coming from DOFPro
        public bool UseUnityDepthBuffer = true;
        public bool AutoFocus = true;

        public LayerMask AutoFocusLayerMask = -1;

        [Range(2f, 8f)]
        public float AutoFocusSpeed = 5f;

        [Range(.01f, 1f)]
        public float FocalLengthMultiplier = .33f;

        public float FocalDistMultiplier = 1f;

        [Range(.5f, 2f)]
        public float DOFBlurSize = 1f;

        public bool BokehEnabled = false;

        [Range(2f, 8f)]
        public float DepthCompression = 4f;
        public Camera EffectCamera;
        public Transform Target;

        [Range(0f, 1f)]
        public float BokehThreshold = .5f;

        [Range(.5f, 5f)]
        public float BokehGain = 2f;

        [Range(0f, 1f)]
        public float BokehBias = .5f;

        public bool DoubleIntensityBlur = false;
    }

    public class DOFHelper : Singleton<DOFHelper>, IDisposable
    {
        private static Material _mat;

        public static Material Mat
        {
            get
            {
                if (null == _mat)
                    _mat = new Material(Shader.Find("Hidden/DOFPro"))
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };

                return _mat;
            }
        }

        private DOFHelperParams _p;

        public void SetParams(DOFHelperParams p)
        {
            _p = p;
        }

        public void Init(bool searchForNonDepthmapAlphaObjects)
        {
            if (this._p == null)
            {
                Debug.LogError("Call SetParams first");
                return;
            }
            if (null == this._p.EffectCamera)
            {
                Debug.LogError("null == p.camera");
                return;
            }
            if (null == DOFHelper.Mat)
            {
                return;
            }
            if (!this._p.UseUnityDepthBuffer)
            {
                this._p.EffectCamera.depthTextureMode = DepthTextureMode.None;
                DOFHelper.Mat.DisableKeyword("USE_CAMERA_DEPTH_TEXTURE");
                DOFHelper.Mat.EnableKeyword("DONT_USE_CAMERA_DEPTH_TEXTURE");
            }
            else
            {
                if (this._p.EffectCamera.depthTextureMode != DepthTextureMode.DepthNormals)
                {
                    this._p.EffectCamera.depthTextureMode = DepthTextureMode.Depth;
                }
                DOFHelper.Mat.EnableKeyword("USE_CAMERA_DEPTH_TEXTURE");
                DOFHelper.Mat.DisableKeyword("DONT_USE_CAMERA_DEPTH_TEXTURE");
            }
            this._p.FocalLengthMultiplier = Mathf.Clamp(this._p.FocalLengthMultiplier, 0.01f, 0.99f);
            this._p.DepthCompression = Mathf.Clamp(this._p.DepthCompression, 1f, 10f);
            Shader.SetGlobalFloat("_OneOverDepthScale", this._p.DepthCompression);
            Shader.SetGlobalFloat("_OneOverDepthFar", 1f / this._p.EffectCamera.farClipPlane);
            if (this._p.BokehEnabled)
            {
                DOFHelper.Mat.SetFloat("_BokehThreshold", this._p.BokehThreshold);
                DOFHelper.Mat.SetFloat("_BokehGain", this._p.BokehGain);
                DOFHelper.Mat.SetFloat("_BokehBias", this._p.BokehBias);
            }
        }

        public void SetBlurRadius(int radius)
        {
            Shader.DisableKeyword("BLUR_RADIUS_10");
            Shader.DisableKeyword("BLUR_RADIUS_5");
            Shader.DisableKeyword("BLUR_RADIUS_3");
            Shader.DisableKeyword("BLUR_RADIUS_2");
            Shader.DisableKeyword("BLUR_RADIUS_1");

            if (radius != 10 && radius != 5 && radius != 3 && radius != 2 && radius != 1) radius = 5;

            if (radius < 3) radius = 3;

            //Debug.Log( "blur radius: " + radius );

            Shader.EnableKeyword("BLUR_RADIUS_" + radius);
        }

        private float _curAutoFocusDist = 0f;

        private void CalculateAndUpdateFocalDist()
        {
            if (this._p == null || null == this._p.EffectCamera)
            {
                return;
            }
            float focalDist;

            if (!_p.AutoFocus && null != _p.Target)
            {
                Vector3 targetPosInViewportSpace = _p.EffectCamera.WorldToViewportPoint(_p.Target.position);
                focalDist = targetPosInViewportSpace.z;
                //		float focalDist = (target.position - transform.position).magnitude / camera.farClipPlane;
            }
            else
            {
                focalDist = _curAutoFocusDist = Mathf.Lerp(_curAutoFocusDist, CalculateAutoFocusDist(), Time.deltaTime * _p.AutoFocusSpeed);
                //            Debug.Log("focalDist: " + focalDist);
            }

            focalDist /= _p.EffectCamera.farClipPlane;

            focalDist *= _p.FocalDistMultiplier * _p.DepthCompression;

            Mat.SetFloat("_FocalDist", focalDist);

            //Make sure that focalLength < focalDist
            Mat.SetFloat("_FocalLength", focalDist * _p.FocalLengthMultiplier);
        }

        private float CalculateAutoFocusDist()
        {
            if (null == _p.EffectCamera) return 0f;

            RaycastHit hitInfo;

            //Return farClipPlane if nothing was hit

            return Physics.Raycast(_p.EffectCamera.transform.position, _p.EffectCamera.transform.forward, out hitInfo, Mathf.Infinity, _p.AutoFocusLayerMask.value) ? hitInfo.distance : _p.EffectCamera.farClipPlane;
        }

        public void RenderCOCTexture(RenderTexture src, RenderTexture dest, float blurScale)
        {
            this.CalculateAndUpdateFocalDist();
            if (this._p == null || null == this._p.EffectCamera)
            {
                return;
            }
            if (this._p.EffectCamera.depthTextureMode == DepthTextureMode.None)
            {
                this._p.EffectCamera.depthTextureMode = DepthTextureMode.Depth;
            }
            if (this._p.DOFBlurSize > 0.001f)
            {
                RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
                RenderTexture renderTexture2 = RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
                Graphics.Blit(src, renderTexture, DOFHelper.Mat, 0);
                DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new Vector4(blurScale, 0f, 0f, 0f));
                Graphics.Blit(renderTexture, renderTexture2, DOFHelper.Mat, 2);
                DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new Vector4(0f, blurScale, 0f, 0f));
                Graphics.Blit(renderTexture2, dest, DOFHelper.Mat, 2);
                RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
                RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture2);
            }
            else
            {
                Graphics.Blit(src, dest, DOFHelper.Mat, 0);
            }
        }

        public void RenderDOFBlur(RenderTexture src, RenderTexture dest, RenderTexture cocTexture)
        {
            if (this._p == null || null == cocTexture)
            {
                return;
            }
            DOFHelper.Mat.SetTexture("_COCTex", cocTexture);
            if (this._p.BokehEnabled)
            {
                DOFHelper.Mat.SetFloat("_BlurIntensity", this._p.DOFBlurSize);
                Graphics.Blit(src, dest, DOFHelper.Mat, 4);
            }
            else
            {
                RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
                DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new Vector4(this._p.DOFBlurSize, 0f, 0f, 0f));
                Graphics.Blit(src, renderTexture, DOFHelper.Mat, 1);
                DOFHelper.Mat.SetVector("_SeparableBlurOffsets", new Vector4(0f, this._p.DOFBlurSize, 0f, 0f));
                Graphics.Blit(renderTexture, dest, DOFHelper.Mat, 1);
                RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
            }
        }

        public void RenderEffect(RenderTexture src, RenderTexture dest)
        {
            RenderEffect(src, dest, false);
        }

        public void RenderEffect(RenderTexture src, RenderTexture dest, bool visualizeCOC)
        {
            RenderTexture cocRenderTexture = RenderTextureManager.Instance.RequestRenderTexture(src.width, src.height, src.depth, src.format);
            RenderCOCTexture(src, cocRenderTexture, 0f);

            //COC visualization
            if (visualizeCOC)
            {
                Graphics.Blit(cocRenderTexture, dest);
                RenderTextureManager.Instance.ReleaseRenderTexture(cocRenderTexture);
                RenderTextureManager.Instance.ReleaseAllRenderTextures();
                return;
            }

            RenderDOFBlur(src, dest, cocRenderTexture);
            RenderTextureManager.Instance.ReleaseRenderTexture(cocRenderTexture);
            //RenderTextureManager.Instance.ReleaseAllRenderTextures();
        }

        public static GameObject[] GetNonDepthmapAlphaObjects()
        {
            if (!Application.isPlaying)
                return new GameObject[0];

            Renderer[] allRenderers = Object.FindObjectsOfType<Renderer>();

            var selectedGOs = new List<GameObject>();
            var nonDepthMappedMaterial = new List<Material>();

            foreach (Renderer rend in allRenderers)
            {
                if (null == rend.sharedMaterials)
                    continue;

                //Skip particle systems
                if (null != rend.GetComponent<ParticleSystem>())
                    continue;

                foreach (Material mat in rend.sharedMaterials)
                {
                    if (null == mat.shader)
                        continue;

                    bool shouldFlagShader = null == mat.GetTag("RenderType", false);

                    //Skip transparent materials
                    if (!shouldFlagShader && (mat.GetTag("RenderType", false).ToLower() == "transparent" || mat.GetTag("Queue", false).ToLower() == "transparent"))
                        continue;

                    if (null == mat.GetTag("OUTPUT_DEPTH_TO_ALPHA", false) || mat.GetTag("OUTPUT_DEPTH_TO_ALPHA", false).ToLower() != "true")
                    {
                        //					if (shouldLog) Debug.Log("<color=green>OUTPUT_DEPTH_TO_ALPHA:</color>" + mat.GetTag("OUTPUT_DEPTH_TO_ALPHA", false) );
                        shouldFlagShader = true;
                    }

                    //				Debug.Log ("shouldFlagShader: " + shouldFlagShader);

                    if (shouldFlagShader)
                    {
                        //Skip duplicates
                        if (nonDepthMappedMaterial.Contains(mat))
                            continue;

                        nonDepthMappedMaterial.Add(mat);

                        Debug.Log("Non-depthmapped: " + GetFullPath(rend.gameObject));
                        selectedGOs.Add(rend.gameObject);
                    }
                }
            }

            return selectedGOs.ToArray();
        }

        public static string GetFullPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return "'" + path + "'";
        }

        public void Dispose()
        {
            if (null != Mat)
                Object.DestroyImmediate(Mat);

            RenderTextureManager.Instance.Dispose();
        }
    }
}