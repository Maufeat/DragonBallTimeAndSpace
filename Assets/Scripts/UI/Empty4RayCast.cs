using System;
using UnityEngine.UI;

public class Empty4RayCast : MaskableGraphic
{
    protected Empty4RayCast()
    {
        base.useLegacyMeshGeneration = false;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }
}
