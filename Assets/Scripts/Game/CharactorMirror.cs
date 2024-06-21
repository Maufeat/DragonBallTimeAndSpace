using System;
using System.Collections.Generic;
using Engine;
using UnityEngine;

public class CharactorMirror : MonoBehaviour
{
    public void Begin()
    {
        if (this.render == null)
        {
            this.render = base.GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        this.list.Clear();
        this.listAlpha.Clear();
        this.isStart = false;
        this.time = 0f;
        this.UseSkill();
    }

    private void UseSkill()
    {
        this.time = 0f;
        this.isStart = true;
    }

    private void FixedUpdate()
    {
        if (this.isStart)
        {
            if (this.time < this.TotalTime)
            {
                this.time += Time.deltaTime;
            }
            else
            {
                this.isStart = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (this.isStart)
        {
            for (int i = 0; i < this.render.Length; i++)
            {
                Mesh mesh = new Mesh();
                this.render[i].BakeMesh(mesh);
                DrawMesh drawMesh = new DrawMesh();
                drawMesh.mesh = mesh;
                drawMesh.matrix = this.render[i].transform.localToWorldMatrix;
                GameMain gameMain = MonobehaviourManager.Instance as GameMain;
                Material material = new Material(gameMain.GetShader("Dragon/Charactor/Fresnel"));
                material.SetFloat("_fresnel_1", 0.149f);
                material.SetFloat("_fresnel", 5.56f);
                material.SetColor("_node_11", this.outlineColor);
                material.SetFloat("_alpha", 1f);
                material.SetFloat("_subtact", 0.35f);
                material.SetFloat("_RenderQueue", 4000f);
                material.SetColor("_node_1578", Color.white);
                material.SetFloat("_light", 5f);
                material.SetFloat("_alpha2", this._alpha);
                drawMesh.material = material;
                this.list.Add(drawMesh);
                this.listAlpha.Add(this._alpha);
            }
        }
        if (this.list.Count > 0)
        {
            for (int j = this.list.Count - 1; j >= 0; j--)
            {
                List<float> list2;
                List<float> list = list2 = this.listAlpha;
                int index2;
                int index = index2 = j;
                float num = list2[index2];
                list[index] = num - Time.deltaTime * this.FadeSpeed;
                this.list[j].material.SetFloat("_alpha2", this.listAlpha[j]);
                if (this.listAlpha[j] <= 0.001f)
                {
                    UnityEngine.Object.Destroy(this.list[j].material);
                    UnityEngine.Object.Destroy(this.list[j].mesh);
                    this.list.RemoveAt(j);
                }
            }
            for (int k = this.list.Count - 1; k >= 0; k--)
            {
                Graphics.DrawMesh(this.list[k].mesh, this.list[k].matrix, this.list[k].material, base.gameObject.layer);
            }
        }
    }

    private SkinnedMeshRenderer[] render;

    private bool isStart;

    public float TotalTime;

    public float FadeSpeed;

    private float time;

    public float rate = 0.01f;

    public Color color = new Color(1f, 1f, 1f, 0f);

    public Color outlineColor = new Color(0.270588249f, 0.670588255f, 1f, 1f);

    private List<DrawMesh> list = new List<DrawMesh>();

    private List<float> listAlpha = new List<float>();

    private float _alpha = 0.6f;
}
