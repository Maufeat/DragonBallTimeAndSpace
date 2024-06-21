using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/TextSpacing")]
public class TextSpacing : BaseMeshEffect
{
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!this.IsActive() || vh.currentVertCount == 0)
        {
            return;
        }
        List<UIVertex> list = new List<UIVertex>();
        vh.GetUIVertexStream(list);
        int currentIndexCount = vh.currentIndexCount;
        for (int i = 6; i < currentIndexCount; i++)
        {
            UIVertex uivertex = list[i];
            uivertex.position += new Vector3(this._textSpacing * (float)(i / 6), 0f, 0f);
            list[i] = uivertex;
            if (i % 6 <= 2)
            {
                vh.SetUIVertex(uivertex, i / 6 * 4 + i % 6);
            }
            if (i % 6 == 4)
            {
                vh.SetUIVertex(uivertex, i / 6 * 4 + i % 6 - 1);
            }
        }
    }

    public float _textSpacing = 1f;
}
