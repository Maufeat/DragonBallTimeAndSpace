using System;
using UnityEngine;

internal class DrawMesh
{
    public DrawMesh()
    {
    }

    public DrawMesh(Mesh me, Matrix4x4 ma, Material mat, Color color)
    {
        this.mesh = me;
        this.matrix = ma;
        this.material = new Material(mat);
        if (this.material.HasProperty("_Color"))
        {
            this.material.SetColor("_Color", color);
        }
    }

    public Mesh mesh;

    public Matrix4x4 matrix;

    public Material material;
}
