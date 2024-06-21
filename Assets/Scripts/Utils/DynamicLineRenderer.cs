using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLineRenderer : MonoBehaviour
{
    private void Start()
    {
    }

    private SkinnedMeshRenderer Skin
    {
        get
        {
            if (this._Skin == null)
            {
                this._Skin = base.GetComponentsInChildren<SkinnedMeshRenderer>(true)[0];
            }
            return this._Skin;
        }
    }

    private void OnEnable()
    {
        if (this.TestPlay)
        {
            this.SetNewData(Vector3.zero);
            this.RunningTime = 0f;
            this.stop = false;
        }
    }

    public void StartPlay(Vector3 pos)
    {
        if (this.onplay && !this.UpdateOnPlaying)
        {
            return;
        }
        if (!this.onplay)
        {
            this.RunningTime = 0f;
            this.stop = false;
        }
        this.onplay = true;
        this.lastMat = base.transform.worldToLocalMatrix;
        this.SetNewData(pos);
    }

    public void Reset()
    {
        this.TestPlay = false;
        this.onplay = false;
        this.RunningTime = 0f;
        this.SetState(0f);
    }

    private void Update()
    {
        if (this.TestPlay || this.onplay)
        {
            this.RunningTime += Time.deltaTime;
            if (!this.stop && !this.UpdateOnPlaying)
            {
                this.SetState((!this.UpdateOnPlaying) ? (this.RunningTime / this.TotalTime) : 1f);
            }
            if (this.RunningTime > this.TotalTime)
            {
                if (this.Loop)
                {
                    this.RunningTime = 0f;
                    this.stop = false;
                }
                else
                {
                    this.stop = true;
                }
            }
        }
    }

    public void SavePosdata()
    {
        this.Positions.Clear();
        this.quqList.Clear();
        for (int i = 0; i < this.bones.Length; i++)
        {
            if (i == 0)
            {
                this.Positions.Add(Vector3.zero);
            }
            else
            {
                this.Positions.Add(this.bones[i].localPosition);
            }
            this.quqList.Add(this.bones[i].localRotation);
        }
    }

    public void Play(float Ptime)
    {
        this.SetState(Ptime / this.TotalTime);
    }

    private void CalculateMatrix(Vector3 from, Vector3 to)
    {
        Vector3 a = to + this.lastMat.MultiplyVector(this.EndExcursion);
        Vector3 toDirection = a - from;
        Vector3 fromDirection = this.Positions[this.Positions.Count - 1] - this.Positions[0];
        this.qua = Quaternion.FromToRotation(fromDirection, toDirection);
        float num = toDirection.magnitude / fromDirection.magnitude;
        Vector3 s = new Vector3(num, num, num);
        this.Mat = default(Matrix4x4);
        this.Mat.SetTRS(Vector3.zero, this.qua, s);
    }

    public void SetNewData(Vector3 to)
    {
        if (this.bones.Length < 2)
        {
            return;
        }
        if (to != Vector3.zero)
        {
            this.CalculateMatrix(Vector3.zero, this.lastMat.MultiplyPoint(to));
            this.hasCalculateMatrix = true;
        }
        else
        {
            this.hasCalculateMatrix = false;
            this.Mat = default(Matrix4x4);
            this.qua = default(Quaternion);
        }
        this.RendererPositions.Clear();
        this.totalDistance = 0f;
        Vector3 b = this.Positions[0];
        for (int i = 0; i < this.Positions.Count; i++)
        {
            Vector3 vector = this.Positions[i];
            if (this.hasCalculateMatrix)
            {
                vector = this.Mat.MultiplyPoint(vector);
            }
            if (i > 0)
            {
                this.totalDistance += Vector3.Distance(vector, b);
            }
            b = vector;
            this.RendererPositions.Add(vector);
        }
        this.SetState((float)((!this.UpdateOnPlaying) ? 0 : 1));
    }

    public void SetState(float value)
    {
        if (this.bones.Length < 2)
        {
            return;
        }
        float num = Mathf.Lerp(0f, 1f, value);
        this.SetMesh(num);
        this.SetUV(num);
    }

    private void SetMesh(float value)
    {
        Vector3 vector = this.Positions[0];
        float num = 0f;
        for (int i = 0; i < this.RendererPositions.Count; i++)
        {
            Vector3 vector2 = this.RendererPositions[i];
            if (i > 0)
            {
                float num2 = Vector3.Distance(vector2, vector);
                if (this.totalDistance * value < num + num2)
                {
                    float t = (this.totalDistance * value - num) / num2;
                    vector2 = Vector3.Lerp(vector, vector2, t);
                }
                num += num2;
            }
            if (this.hasCalculateMatrix)
            {
                this.bones[i].localRotation = this.qua * this.quqList[i];
            }
            this.bones[i].localPosition = vector2;
            vector = vector2;
        }
    }

    private void SetUV(float value)
    {
        if (string.IsNullOrEmpty(this.PropertyName))
        {
            return;
        }
        for (int i = 0; i < this.Skin.materials.Length; i++)
        {
            Material material = this.Skin.materials[i];
            Vector4 vector = material.GetVector(this.PropertyName);
            if (this.MaxUVOffset.x > 0f)
            {
                vector.x = value * this.MaxUVOffset.x;
            }
            if (this.MaxUVOffset.y > 0f)
            {
                vector.y = value * this.MaxUVOffset.y;
            }
            if (this.MaxUVOffset.z > 0f)
            {
                vector.z = value * this.MaxUVOffset.z;
            }
            if (this.MaxUVOffset.w > 0f)
            {
                vector.w = value * this.MaxUVOffset.w;
            }
            material.SetVector(this.PropertyName, vector);
        }
    }

    public Vector3 EndExcursion = Vector3.zero;

    public Transform[] bones;

    public FlyObjConfig.TargetType targetType;

    public string PropertyName = "_MainTex_ST";

    public Vector4 MaxUVOffset = Vector4.zero;

    public float TotalTime = 3f;

    public bool TestPlay;

    public bool Loop = true;

    public bool UpdateOnPlaying;

    private SkinnedMeshRenderer _Skin;

    private float totalDistance;

    private List<Vector3> RendererPositions = new List<Vector3>();

    private bool stop;

    private float RunningTime;

    private bool onplay;

    public List<Vector3> Positions = new List<Vector3>();

    public List<Quaternion> quqList = new List<Quaternion>();

    private Quaternion qua;

    private Matrix4x4 Mat;

    private Matrix4x4 lastMat;

    private bool hasCalculateMatrix;

    private List<Vector3> tmpList = new List<Vector3>();
}
