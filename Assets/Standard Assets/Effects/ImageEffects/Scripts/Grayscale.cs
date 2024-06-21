using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
    [ExecuteInEditMode]
    public class Grayscale : ImageEffectBase
    {
        public void ResetShader()
        {
            if (null == this.shader)
            {
                this.shader = Shader.Find("Hidden/Grayscale Effect");
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            base.material.SetTexture("_RampTex", this.textureRamp);
            base.material.SetFloat("_RampOffset", this.rampOffset);
            Graphics.Blit(source, destination, base.material);
        }

        public Texture textureRamp;

        [Range(-1f, 1f)]
        public float rampOffset;
    }
}
