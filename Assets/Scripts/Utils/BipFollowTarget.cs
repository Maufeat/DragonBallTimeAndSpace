using System;
using UnityEngine;

public class BipFollowTarget : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        if (null == this.target)
        {
            return;
        }
        this.targetPos = this.target.position;
        this.targetPos.x = this.target.position.x + this.FixX;
        this.targetPos.y = this.target.position.y + this.FixY;
        this.targetPos.z = this.target.position.z + this.FixZ;
        base.transform.position = this.targetPos;
    }

    public Transform target;

    public float FixY;

    public float FixX;

    public float FixZ;

    private Vector3 targetPos;
}
