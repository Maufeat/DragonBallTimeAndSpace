using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProFlare : MonoBehaviour
{
    private void Awake()
    {
        this.DisabledPlayMode = false;
        this.Initialised = false;
        this.thisTransform = base.transform;
    }

    private void Start()
    {
        this.thisTransform = base.transform;
        if (this._Atlas != null && this.FlareBatches.Length == 0)
        {
            this.PopulateFlareBatches();
        }
        if (!this.Initialised)
        {
            this.Init();
        }
        if (this.Elements != null)
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                this.Elements[i].flareAtlas = this._Atlas;
                this.Elements[i].flare = this;
            }
        }
    }

    private void PopulateFlareBatches()
    {
        ProFlareBatch[] array = UnityEngine.Object.FindObjectsOfType(typeof(ProFlareBatch)) as ProFlareBatch[];
        int num = 0;
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i]._atlas == this._Atlas)
                {
                    num++;
                }
            }
            this.FlareBatches = new ProFlareBatch[num];
            int num2 = 0;
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j]._atlas == this._Atlas)
                {
                    this.FlareBatches[num2] = array[j];
                    num2++;
                }
            }
        }
    }

    private void Init()
    {
        if (this.thisTransform == null)
        {
            this.thisTransform = base.transform;
        }
        if (this._Atlas == null)
        {
            return;
        }
        this.PopulateFlareBatches();
        if (this.Elements != null)
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                this.Elements[i].flareAtlas = this._Atlas;
            }
        }
        if (this.FlareBatches != null)
        {
            for (int j = 0; j < this.FlareBatches.Length; j++)
            {
                if (this.FlareBatches[j] != null && this.FlareBatches[j]._atlas == this._Atlas)
                {
                    this.FlareBatches[j].AddFlare(this);
                }
            }
        }
        this.Initialised = true;
    }

    public void ReInitialise()
    {
        this.Initialised = false;
        this.Init();
    }

    private void OnEnable()
    {
        if (Application.isPlaying && this.DisabledPlayMode)
        {
            this.DisabledPlayMode = false;
        }
        else
        {
            if (this._Atlas != null && this._Atlas && this.FlareBatches != null)
            {
                for (int i = 0; i < this.FlareBatches.Length; i++)
                {
                    if (this.FlareBatches[i] != null)
                    {
                        this.FlareBatches[i].dirty = true;
                    }
                    else
                    {
                        this.Initialised = false;
                    }
                }
            }
            this.Init();
        }
    }

    private void OnDisable()
    {
        if (Application.isPlaying)
        {
            this.DisabledPlayMode = true;
        }
        else if (this.FlareBatches != null)
        {
            for (int i = 0; i < this.FlareBatches.Length; i++)
            {
                if (this.FlareBatches[i] != null)
                {
                    this.FlareBatches[i].Flares.Remove(this);
                    this.FlareBatches[i].dirty = true;
                }
                else
                {
                    this.Initialised = false;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (this.FlareBatches != null)
        {
            for (int i = 0; i < this.FlareBatches.Length; i++)
            {
                if (this.FlareBatches[i] != null)
                {
                    this.FlareBatches[i].Flares.Remove(this);
                    this.FlareBatches[i].dirty = true;
                }
                else
                {
                    this.Initialised = false;
                }
            }
        }
    }

    private void Update()
    {
        if (!this.Initialised)
        {
            this.Init();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = this.GlobalTintColor;
        Gizmos.DrawSphere(base.transform.position, 0.1f);
    }

    public ProFlareAtlas _Atlas;

    public ProFlareBatch[] FlareBatches = new ProFlareBatch[0];

    public bool EditingAtlas;

    public bool isVisible = true;

    public List<ProFlareElement> Elements = new List<ProFlareElement>();

    public Transform thisTransform;

    public Vector3 LensPosition;

    public bool EditGlobals;

    public float GlobalScale = 100f;

    public float GlobalBrightness = 1f;

    public Color GlobalTintColor = Color.white;

    public bool useMaxDistance = true;

    public bool useDistanceScale = true;

    public bool useDistanceFade = true;

    public float GlobalMaxDistance = 150f;

    public bool UseAngleLimit;

    public float maxAngle = 90f;

    public bool UseAngleScale;

    public bool UseAngleBrightness = true;

    public bool UseAngleCurve;

    public AnimationCurve AngleCurve = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0f, 0f),
        new Keyframe(1f, 1f)
    });

    public LayerMask mask = 1;

    public bool RaycastPhysics;

    public bool Occluded;

    public float OccludeScale = 1f;

    public float OffScreenFadeDist = 0.2f;

    public bool useDynamicEdgeBoost;

    public float DynamicEdgeBoost = 1f;

    public float DynamicEdgeBrightness = 0.1f;

    public float DynamicEdgeRange = 0.3f;

    public float DynamicEdgeBias = -0.1f;

    public AnimationCurve DynamicEdgeCurve = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    });

    public bool useDynamicCenterBoost;

    public float DynamicCenterBoost = 1f;

    public float DynamicCenterBrightness = 0.2f;

    public float DynamicCenterRange = 0.3f;

    public float DynamicCenterBias;

    public bool neverCull;

    private bool Initialised;

    public bool DisabledPlayMode;
}
