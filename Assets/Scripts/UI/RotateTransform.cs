using System;
using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    private void Start()
    {
        this.thisTransform = base.transform;
    }

    private void Update()
    {
        Quaternion rhs = Quaternion.Euler(this.Speed * Time.deltaTime);
        this.thisTransform.localRotation = this.thisTransform.localRotation * rhs;
    }

    private Transform thisTransform;

    public Vector3 Speed = new Vector3(0f, 20f, 0f);
}
