using System;
using UnityEngine;

public class PJBillBoard : MonoBehaviour
{
    private void Start()
    {
        if (null != Camera.main)
        {
            this.m_cam = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (this.m_cam == null)
        {
            if (null != Camera.main)
            {
                this.m_cam = Camera.main.transform;
            }
            return;
        }
        Vector3 position = base.transform.position;
        if (this.UseX)
        {
            position.x = this.m_cam.position.x;
        }
        if (this.UseY)
        {
            position.y = this.m_cam.position.y;
        }
        if (this.UseZ)
        {
            position.z = this.m_cam.position.z;
        }
        base.transform.LookAt(position);
    }

    public bool UseX = true;

    public bool UseY;

    public bool UseZ = true;

    public Transform m_cam;
}
