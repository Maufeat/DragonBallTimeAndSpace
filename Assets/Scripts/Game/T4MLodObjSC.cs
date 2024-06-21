using System;
using UnityEngine;

public class T4MLodObjSC : MonoBehaviour
{
    public void Awake()
    {
        this.ActivateLODScrpt();
    }

    public void ActivateLODScrpt()
    {
        this.ObjLodStatus = 1;
        if (this.LOD2Start > this.LOD3Start)
        {
        }
        if (this.PlayerCamera == null)
        {
            this.PlayerCamera = Camera.main.transform;
        }
        base.InvokeRepeating("AFLODScrpt", UnityEngine.Random.Range(0f, this.Interval), this.Interval);
    }

    public void ActivateLODLay()
    {
        if (this.Mode != 2)
        {
            return;
        }
        if (this.PlayerCamera == null)
        {
            this.PlayerCamera = Camera.main.transform;
        }
        base.InvokeRepeating("AFLODLay", UnityEngine.Random.Range(0f, this.Interval), this.Interval);
    }

    public void AFLODLay()
    {
        if (this.OldPlayerPos == this.PlayerCamera.position)
        {
            return;
        }
        this.OldPlayerPos = this.PlayerCamera.position;
        float num = Vector3.Distance(new Vector3(base.transform.position.x, this.PlayerCamera.position.y, base.transform.position.z), this.PlayerCamera.position);
        int layer = base.gameObject.layer;
        if (num <= this.PlayerCamera.GetComponent<Camera>().layerCullDistances[layer] + 5f)
        {
            if (num < this.LOD2Start && this.ObjLodStatus != 1)
            {
                if (this.LOD3 != null)
                {
                    this.LOD3.enabled = false;
                }
                if (this.LOD2 = null)
                {
                    this.LOD2.enabled = false;
                }
                if (this.LOD1 != null)
                {
                    this.LOD1.enabled = true;
                }
                this.ObjLodStatus = 1;
            }
            else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus != 2)
            {
                if (this.LOD1 != null)
                {
                    this.LOD1.enabled = false;
                }
                if (this.LOD3 != null)
                {
                    this.LOD3.enabled = false;
                }
                if (this.LOD2 != null)
                {
                    this.LOD2.enabled = true;
                }
                this.ObjLodStatus = 2;
            }
            else if (num >= this.LOD3Start && this.ObjLodStatus != 3)
            {
                if (this.LOD1 != null)
                {
                    this.LOD1.enabled = false;
                }
                if (this.LOD2 != null)
                {
                    this.LOD2.enabled = false;
                }
                if (this.LOD3 != null)
                {
                    this.LOD3.enabled = true;
                }
                this.ObjLodStatus = 3;
            }
        }
    }

    public void AFLODScrpt()
    {
        if (this.OldPlayerPos == this.PlayerCamera.position)
        {
            return;
        }
        this.OldPlayerPos = this.PlayerCamera.position;
        float num = Vector3.Distance(new Vector3(base.transform.position.x, this.PlayerCamera.position.y, base.transform.position.z), this.PlayerCamera.position);
        if (num <= this.MaxViewDistance)
        {
            if (num < this.LOD2Start && this.ObjLodStatus != 1)
            {
                if (this.LOD3 != null)
                {
                    this.LOD3.enabled = false;
                }
                if (this.LOD2 != null)
                {
                    this.LOD2.enabled = false;
                }
                if (this.LOD1 != null)
                {
                    this.LOD1.enabled = true;
                }
                this.ObjLodStatus = 1;
            }
            else if (num >= this.LOD2Start && num < this.LOD3Start && this.ObjLodStatus != 2)
            {
                if (this.LOD1 != null)
                {
                    this.LOD1.enabled = false;
                }
                if (this.LOD3 != null)
                {
                    this.LOD3.enabled = false;
                }
                if (this.LOD2 != null)
                {
                    this.LOD2.enabled = true;
                }
                this.ObjLodStatus = 2;
            }
            else if (num >= this.LOD3Start && this.ObjLodStatus != 3)
            {
                if (this.LOD1 != null)
                {
                    this.LOD1.enabled = false;
                }
                if (this.LOD2 != null)
                {
                    this.LOD2.enabled = false;
                }
                if (this.LOD3 != null)
                {
                    this.LOD3.enabled = true;
                }
                this.ObjLodStatus = 3;
            }
        }
        else if (this.ObjLodStatus != 0)
        {
            if (this.LOD1 != null)
            {
                this.LOD1.enabled = false;
            }
            if (this.LOD2 != null)
            {
                this.LOD2.enabled = false;
            }
            if (this.LOD3 != null)
            {
                this.LOD3.enabled = false;
            }
            this.ObjLodStatus = 0;
        }
    }

    public Renderer LOD1;

    public Renderer LOD2;

    public Renderer LOD3;

    [HideInInspector]
    public float Interval = 0.5f;

    [HideInInspector]
    public Transform PlayerCamera;

    [HideInInspector]
    public int Mode;

    private Vector3 OldPlayerPos;

    [HideInInspector]
    public int ObjLodStatus;

    public float MaxViewDistance = 60f;

    public float LOD2Start = 20f;

    public float LOD3Start = 40f;
}
