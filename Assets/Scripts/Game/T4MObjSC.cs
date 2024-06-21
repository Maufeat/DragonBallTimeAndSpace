using System;
using UnityEngine;

[ExecuteInEditMode]
public class T4MObjSC : MonoBehaviour
{
    public void Awake()
    {
        if (this.Master == 1)
        {
            if (this.PlayerCamera == null && Camera.main)
            {
                this.PlayerCamera = Camera.main.transform;
            }
            else if (this.PlayerCamera == null && !Camera.main)
            {
                Camera[] array = UnityEngine.Object.FindObjectsOfType(typeof(Camera)) as Camera[];
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].GetComponent<AudioListener>())
                    {
                        this.PlayerCamera = array[i].transform;
                    }
                }
            }
            if (this.enabledBillboard && this.BillboardPosition.Length > 0 && this.BillScript[0] != null)
            {
                if (this.BilBbasedOnScript)
                {
                    base.InvokeRepeating("BillScrpt", UnityEngine.Random.Range(0f, this.BillInterval), this.BillInterval);
                }
                else
                {
                    base.InvokeRepeating("BillLay", UnityEngine.Random.Range(0f, this.BillInterval), this.BillInterval);
                }
            }
        }
    }

    public void ReloadShader()
    {
        Renderer[] components = base.gameObject.GetComponents<Renderer>();
        for (int i = 0; i < components.Length; i++)
        {
            for (int j = 0; j < components[i].sharedMaterials.Length; j++)
            {
                if (components[i].sharedMaterials[j])
                {
                    components[i].sharedMaterials[j].shader = Shader.Find(this.SharedShaderName);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (this.ActiveWind)
        {
            Color color = this.Wind * Mathf.Sin(Time.realtimeSinceStartup * this.WindFrequency);
            color.a = this.Wind.w;
            Color color2 = this.Wind * Mathf.Sin(Time.realtimeSinceStartup * this.GrassWindFrequency);
            color2.a = this.Wind.w;
            Shader.SetGlobalColor("_Wind", color);
            Shader.SetGlobalColor("_GrassWind", color2);
            Shader.SetGlobalColor("_TranslucencyColor", this.TranslucencyColor);
            Shader.SetGlobalFloat("_TranslucencyViewDependency;", 0.65f);
        }
        if (this.PlayerCamera && !Application.isPlaying && this.Master == 1 && this.BillboardPreview && this.enabledBillboard && this.BillboardPosition.Length > 0 && this.BillScript[0] != null)
        {
            if (this.BilBbasedOnScript)
            {
                this.BillScrpt();
            }
            else
            {
                this.BillLay();
            }
        }
    }

    private void BillScrpt()
    {
        for (int i = 0; i < this.BillboardPosition.Length; i++)
        {
            if (Vector3.Distance(this.BillboardPosition[i], this.PlayerCamera.position) <= this.BillMaxViewDistance)
            {
                if (this.BillStatus[i] != 1)
                {
                    this.BillScript[i].Render.enabled = true;
                    this.BillStatus[i] = 1;
                }
                if (this.Axis == 0)
                {
                    this.BillScript[i].Transf.LookAt(new Vector3(this.PlayerCamera.position.x, this.BillScript[i].Transf.position.y, this.PlayerCamera.position.z), Vector3.up);
                }
                else
                {
                    this.BillScript[i].Transf.LookAt(this.PlayerCamera.position, Vector3.up);
                }
            }
            else if (this.BillStatus[i] != 0 && !this.BillScript[i].Render.enabled)
            {
                this.BillScript[i].Render.enabled = false;
                this.BillStatus[i] = 0;
            }
        }
    }

    private void BillLay()
    {
        for (int i = 0; i < this.BillboardPosition.Length; i++)
        {
            int layer = this.BillScript[i].gameObject.layer;
            if (Vector3.Distance(this.BillboardPosition[i], this.PlayerCamera.position) <= this.distances[layer])
            {
                if (this.Axis == 0)
                {
                    this.BillScript[i].Transf.LookAt(new Vector3(this.PlayerCamera.position.x, this.BillScript[i].Transf.position.y, this.PlayerCamera.position.z), Vector3.up);
                }
                else
                {
                    this.BillScript[i].Transf.LookAt(this.PlayerCamera.position, Vector3.up);
                }
            }
        }
    }

    private void LODScript()
    {
        if (this.OldPlayerPos == this.PlayerCamera.position)
        {
            return;
        }
        this.OldPlayerPos = this.PlayerCamera.position;
        for (int i = 0; i < this.ObjPosition.Length; i++)
        {
            float num = Vector3.Distance(new Vector3(this.ObjPosition[i].x, this.PlayerCamera.position.y, this.ObjPosition[i].z), this.PlayerCamera.position);
            if (num <= this.MaxViewDistance)
            {
                if (num < this.LOD2Start && this.ObjLodStatus[i] != 1)
                {
                    if (!(this.ObjLodScript[i] == null))
                    {
                        Renderer lod = this.ObjLodScript[i].LOD2;
                        bool flag = false;
                        this.ObjLodScript[i].LOD3.enabled = flag;
                        lod.enabled = flag;
                        this.ObjLodScript[i].LOD1.enabled = true;
                        this.ObjLodStatus[i] = 1;
                    }
                }
                else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus[i] != 2)
                {
                    if (!(this.ObjLodScript[i] == null))
                    {
                        Renderer lod2 = this.ObjLodScript[i].LOD1;
                        bool flag = false;
                        this.ObjLodScript[i].LOD3.enabled = flag;
                        lod2.enabled = flag;
                        this.ObjLodScript[i].LOD2.enabled = true;
                        this.ObjLodStatus[i] = 2;
                    }
                }
                else if (num >= this.LOD3Start && this.ObjLodStatus[i] != 3)
                {
                    if (!(this.ObjLodScript[i] == null))
                    {
                        Renderer lod3 = this.ObjLodScript[i].LOD2;
                        bool flag = false;
                        this.ObjLodScript[i].LOD1.enabled = flag;
                        lod3.enabled = flag;
                        this.ObjLodScript[i].LOD3.enabled = true;
                        this.ObjLodStatus[i] = 3;
                    }
                }
            }
            else if (this.ObjLodStatus[i] != 0)
            {
                if (!(this.ObjLodScript[i] == null))
                {
                    Renderer lod4 = this.ObjLodScript[i].LOD1;
                    bool flag = false;
                    this.ObjLodScript[i].LOD3.enabled = flag;
                    flag = flag;
                    this.ObjLodScript[i].LOD2.enabled = flag;
                    lod4.enabled = flag;
                    this.ObjLodStatus[i] = 0;
                }
            }
        }
    }

    private void LODLay()
    {
        if (this.OldPlayerPos == this.PlayerCamera.position)
        {
            return;
        }
        this.OldPlayerPos = this.PlayerCamera.position;
        for (int i = 0; i < this.ObjPosition.Length; i++)
        {
            float num = Vector3.Distance(new Vector3(this.ObjPosition[i].x, this.PlayerCamera.position.y, this.ObjPosition[i].z), this.PlayerCamera.position);
            int layer = this.ObjLodScript[i].gameObject.layer;
            if (num <= this.distances[layer] + 5f)
            {
                if (num < this.LOD2Start && this.ObjLodStatus[i] != 1)
                {
                    Renderer lod = this.ObjLodScript[i].LOD2;
                    bool enabled = false;
                    this.ObjLodScript[i].LOD3.enabled = enabled;
                    lod.enabled = enabled;
                    this.ObjLodScript[i].LOD1.enabled = true;
                    this.ObjLodStatus[i] = 1;
                }
                else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus[i] != 2)
                {
                    Renderer lod2 = this.ObjLodScript[i].LOD1;
                    bool enabled = false;
                    this.ObjLodScript[i].LOD3.enabled = enabled;
                    lod2.enabled = enabled;
                    this.ObjLodScript[i].LOD2.enabled = true;
                    this.ObjLodStatus[i] = 2;
                }
                else if (num >= this.LOD3Start && this.ObjLodStatus[i] != 3)
                {
                    Renderer lod3 = this.ObjLodScript[i].LOD2;
                    bool enabled = false;
                    this.ObjLodScript[i].LOD1.enabled = enabled;
                    lod3.enabled = enabled;
                    this.ObjLodScript[i].LOD3.enabled = true;
                    this.ObjLodStatus[i] = 3;
                }
            }
        }
    }

    public void Rest()
    {
        T4MLodObjSC[] array = UnityEngine.Object.FindObjectsOfType<T4MLodObjSC>();
        if (array.Length > 0)
        {
            this.ObjLodScript = new T4MLodObjSC[array.Length];
            this.ObjPosition = new Vector3[array.Length];
            this.ObjLodStatus = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                this.ObjLodScript[i] = array[i];
                this.ObjPosition[i] = array[i].gameObject.transform.position;
                this.ObjLodStatus[i] = array[i].ObjLodStatus;
            }
        }
    }

    [HideInInspector]
    public string ConvertType = string.Empty;

    [HideInInspector]
    public bool EnabledLODSystem = true;

    [HideInInspector]
    public Vector3[] ObjPosition;

    [HideInInspector]
    public T4MLodObjSC[] ObjLodScript;

    [HideInInspector]
    public int[] ObjLodStatus;

    [HideInInspector]
    public float MaxViewDistance = 60f;

    [HideInInspector]
    public float LOD2Start = 20f;

    [HideInInspector]
    public float LOD3Start = 40f;

    [HideInInspector]
    public float Interval = 0.5f;

    [HideInInspector]
    public Transform PlayerCamera;

    private Vector3 OldPlayerPos;

    [HideInInspector]
    public int Mode = 1;

    [HideInInspector]
    public int Master;

    [HideInInspector]
    public bool enabledBillboard = true;

    [HideInInspector]
    public Vector3[] BillboardPosition;

    [HideInInspector]
    public float BillInterval = 0.05f;

    [HideInInspector]
    public int[] BillStatus;

    [HideInInspector]
    public float BillMaxViewDistance = 30f;

    [HideInInspector]
    public T4MBillBObjSC[] BillScript;

    [HideInInspector]
    public bool enabledLayerCul = true;

    [HideInInspector]
    public float BackGroundView = 1000f;

    [HideInInspector]
    public float FarView = 200f;

    [HideInInspector]
    public float NormalView = 60f;

    [HideInInspector]
    public float CloseView = 30f;

    private float[] distances = new float[32];

    [HideInInspector]
    public int Axis;

    [HideInInspector]
    public bool LODbasedOnScript = true;

    [HideInInspector]
    public bool BilBbasedOnScript = true;

    [HideInInspector]
    public string SharedShaderName = string.Empty;

    public Material T4MMaterial;

    public MeshFilter T4MMesh;

    [HideInInspector]
    public Color TranslucencyColor = new Color(0.73f, 0.85f, 0.4f, 1f);

    [HideInInspector]
    public Vector4 Wind = new Vector4(0.85f, 0.075f, 0.4f, 0.5f);

    [HideInInspector]
    public float WindFrequency = 0.75f;

    [HideInInspector]
    public float GrassWindFrequency = 1.5f;

    [HideInInspector]
    public bool ActiveWind;

    public bool LayerCullPreview;

    public bool LODPreview;

    public bool BillboardPreview;

    public Texture2D T4MMaskTex2d;

    public Texture2D T4MMaskTexd;
}
