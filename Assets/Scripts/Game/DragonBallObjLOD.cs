using System;
using UnityEngine;

public class DragonBallObjLOD : MonoBehaviour
{
    private MeshRenderer[] mrSelf
    {
        get
        {
            if (this._mrSelf == null)
            {
                this._mrSelf = base.GetComponentsInChildren<MeshRenderer>();
            }
            return this._mrSelf;
        }
    }

    private MeshRenderer[] _mrSelf { get; set; }

    private void Start()
    {
        this.fTickTime = 2f;
    }

    private void Update()
    {
        if (DragonBallObjLOD._bInit)
        {
            if (this.fTickTime >= 2f)
            {
                this.ApplyLod();
                this.fTickTime -= 2f;
            }
            this.fTickTime += Time.deltaTime;
        }
    }

    public static void Init(Transform tran)
    {
        DragonBallObjLOD._tranTarget = tran;
        DragonBallObjLOD._bInit = true;
    }

    public static void ResetTarget(Transform tran)
    {
        if (DragonBallObjLOD._bInit)
        {
            DragonBallObjLOD._tranTarget = tran;
        }
    }

    private bool GetLodActive()
    {
        return DragonBallObjLOD.distanceGreaterThen && this.fMaxViewDistance > 149f;
    }

    private void ApplyLod()
    {
        if (null != DragonBallObjLOD._tranTarget)
        {
            Vector3 position = DragonBallObjLOD._tranTarget.position;
            if (this.v3TargetOldPos == position)
            {
                return;
            }
            if (this.GetLodActive())
            {
                if (!this._bShow)
                {
                    for (int i = 0; i < this.mrSelf.Length; i++)
                    {
                        this.mrSelf[i].enabled = true;
                        this._bShow = true;
                    }
                }
                return;
            }
            this.v3TargetOldPos = position;
            Vector3 position2 = base.transform.position;
            float num = Vector3.Distance(new Vector3(position2.x, position.y, position2.z), position);
            if (num < this.fMaxViewDistance)
            {
                if (!this._bShow)
                {
                    for (int j = 0; j < this.mrSelf.Length; j++)
                    {
                        this.mrSelf[j].enabled = true;
                        this._bShow = true;
                    }
                }
            }
            else if (this._bShow)
            {
                for (int k = 0; k < this.mrSelf.Length; k++)
                {
                    this.mrSelf[k].enabled = false;
                    this._bShow = false;
                }
            }
        }
    }

    private const float _fIntervalTime = 2f;

    public float fMaxViewDistance = 100f;

    private static Transform _tranTarget;

    private static bool _bInit;

    public static bool distanceGreaterThen;

    private bool _bShow = true;

    private float fTickTime;

    private Vector3 v3TargetOldPos;
}
