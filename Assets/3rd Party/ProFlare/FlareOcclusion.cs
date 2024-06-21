using System;

[Serializable]
public class FlareOcclusion
{
    public bool occluded;

    public float occlusionScale = 1f;

    public FlareOcclusion.CullingState _CullingState;

    public float CullTimer;

    public float cullFader = 1f;

    public enum CullingState
    {
        Visible,
        CullCountDown,
        CanCull,
        Culled,
        NeverCull
    }
}
