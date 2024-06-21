using UnityEngine;

public class UI3DTarget : MonoBehaviour
{
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (this.target != null)
        {
            Graphics.Blit(src, this.target);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    public RenderTexture target;
}