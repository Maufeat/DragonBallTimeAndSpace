using System;
using UnityEngine;

namespace FxProNS
{
    // Token: 0x02000044 RID: 68
    [Serializable]
    public class BloomHelperParams
    {
        // Token: 0x040001A7 RID: 423
        public EffectsQuality Quality;

        // Token: 0x040001A8 RID: 424
        [Range(0f, 1f)]
        public float BloomThreshold = 0.25f;

        // Token: 0x040001A9 RID: 425
        [Range(0f, 2.5f)]
        public float BloomIntensity = 0.75f;

        // Token: 0x040001AA RID: 426
        [Range(0.25f, 5.5f)]
        public float BloomBlurSize = 1f;
    }

    public class BloomHelper : Singleton<BloomHelper>, IDisposable
    {
        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060001A0 RID: 416 RVA: 0x0005F340 File Offset: 0x0005D540
        public static Material Mat
        {
            get
            {
                if (null == BloomHelper._mat)
                {
                    BloomHelper._mat = new Material(Shader.Find("Hidden/BloomPro"))
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                }
                return BloomHelper._mat;
            }
        }

        // Token: 0x060001A1 RID: 417 RVA: 0x00004355 File Offset: 0x00002555
        public void SetParams(BloomHelperParams _p)
        {
            this.p = _p;
        }

        // Token: 0x060001A2 RID: 418 RVA: 0x0005F380 File Offset: 0x0005D580
        public void Init()
        {
            if (this.p == null)
            {
                return;
            }
            BloomHelper.Mat.SetFloat("_BloomThreshold", this.p.BloomThreshold);
            BloomHelper.Mat.SetFloat("_BloomIntensity", this.p.BloomIntensity);
            RenderTextureManager.Instance.Dispose();
        }

        // Token: 0x060001A3 RID: 419 RVA: 0x0005F3D8 File Offset: 0x0005D5D8
        public void RenderBloomTexture(RenderTexture source, RenderTexture dest)
        {
            int num = ((this.p.Quality != EffectsQuality.Fast && this.p.Quality != EffectsQuality.Fastest) ? 2 : 4);
            float num2 = ((this.p.Quality != EffectsQuality.Fast && this.p.Quality != EffectsQuality.Fastest) ? 1f : 0.5f);
            BloomHelper.Mat.SetVector("_Parameter", new Vector4(this.p.BloomBlurSize * num2, 0f, this.p.BloomThreshold, this.p.BloomIntensity));
            int num3 = source.width / num;
            int num4 = source.height / num;
            RenderTexture renderTexture = RenderTextureManager.Instance.RequestRenderTexture(num3, num4, 0, source.format);
            RenderTexture renderTexture2 = RenderTextureManager.Instance.RequestRenderTexture(num3, num4, 0, source.format);
            RenderTexture renderTexture3 = RenderTextureManager.Instance.RequestRenderTexture(num3, num4, 0, source.format);
            Graphics.Blit(source, renderTexture, BloomHelper.Mat, 1);
            BloomHelper.Mat.SetVector("_Parameter", new Vector4(this.p.BloomBlurSize * num2, 0f, this.p.BloomThreshold, this.p.BloomIntensity));
            Graphics.Blit(renderTexture, renderTexture2, BloomHelper.Mat, 2);
            Graphics.Blit(renderTexture2, renderTexture3, BloomHelper.Mat, 3);
            BloomHelper.Mat.SetTexture("_Bloom", renderTexture3);
            Graphics.Blit(source, dest, BloomHelper.Mat, 0);
            RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture);
            RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture2);
            RenderTextureManager.Instance.ReleaseRenderTexture(renderTexture3);
        }

        // Token: 0x060001A4 RID: 420 RVA: 0x0000435E File Offset: 0x0000255E
        public void Dispose()
        {
            if (null != BloomHelper.Mat)
            {
                global::UnityEngine.Object.DestroyImmediate(BloomHelper.Mat);
            }
            RenderTextureManager.Instance.Dispose();
        }

        // Token: 0x040001AB RID: 427
        private static Material _mat;

        // Token: 0x040001AC RID: 428
        private BloomHelperParams p;
    }
}
