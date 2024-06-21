using System;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraManager : MonoBehaviour
{
    public void Init(Vector3 cameraFocasCenter)
    {
        this.cameraFocasCenter = cameraFocasCenter;
    }

    private void Start()
    {
        this.target = base.GetComponent<Camera>();
        FreeCameraManager.freeCamera = this;
    }

    private void OnDestroy()
    {
        FreeCameraManager.freeCamera = null;
    }

    private void Update()
    {
        if (this.target == null)
        {
            return;
        }
        Vector3 mousePosition = Input.mousePosition;
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0f)
        {
            Vector3 vector = this.target.transform.position;
            float num = Vector3.Distance(vector, this.cameraFocasCenter);
            float num2 = (num - this.minFreeViewDis) / (this.maxFreeViewDis - this.minFreeViewDis);
            float num3 = 0.005f;
            if (num2 * this.scaleMulti > num3)
            {
                num3 = num2 * this.scaleMulti;
            }
            num2 -= axis * num3;
            num = Mathf.Lerp(this.minFreeViewDis, this.maxFreeViewDis, num2);
            vector = this.cameraFocasCenter + num * (vector - this.cameraFocasCenter).normalized;
            this.SetTargetPos(vector);
        }
        if (Input.GetMouseButtonDown(2))
        {
            this.oldPos = mousePosition;
            this.oldPos1 = mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                this.oldPos1 = mousePosition;
                if (this.oldPos != mousePosition)
                {
                    Vector3 position = this.target.transform.position;
                    float dis = Vector3.Distance(position, this.cameraFocasCenter);
                    Vector3 a = this.GetRayPoint(mousePosition, dis) - this.GetRayPoint(this.oldPos, dis);
                    Vector3 b = -a;
                    Vector3 targetPos = position + b;
                    targetPos.y = ((targetPos.y >= this.minFreeViewDis) ? targetPos.y : this.minFreeViewDis);
                    targetPos.y = ((targetPos.y <= this.maxFreeViewDis) ? targetPos.y : this.maxFreeViewDis);
                    this.SetTargetPos(targetPos);
                    this.cameraFocasCenter += b;
                    this.oldPos = mousePosition;
                }
            }
            else
            {
                this.oldPos = mousePosition;
                if (this.oldPos1 != mousePosition)
                {
                    Vector3 v = mousePosition - this.oldPos1;
                    this.RotateTarget(v);
                    this.oldPos1 = mousePosition;
                }
            }
        }
    }

    private void SetTargetPos(Vector3 pos)
    {
        this.target.transform.position = pos;
    }

    private void RotateTarget(Vector2 offset)
    {
        Vector3 position = this.target.transform.position;
        float num = Vector3.Distance(position, this.cameraFocasCenter);
        Vector3 eulerAngles = this.target.transform.eulerAngles;
        eulerAngles.x -= offset.y / 2f * this.rotateMultiY;
        eulerAngles.y += offset.x / 2f * this.rotateMultiX;
        eulerAngles.y = ((eulerAngles.y < 360f) ? eulerAngles.y : (eulerAngles.y - 360f));
        eulerAngles.y = ((eulerAngles.y >= -360f) ? eulerAngles.y : (eulerAngles.y + 360f));
        eulerAngles.x = ((eulerAngles.x <= this.maxFreeViewAngle) ? eulerAngles.x : this.maxFreeViewAngle);
        eulerAngles.x = ((eulerAngles.x >= this.minFreeViewAngle) ? eulerAngles.x : this.minFreeViewAngle);
        this.target.transform.eulerAngles = eulerAngles;
        Vector3 rayPoint = this.GetRayPoint((float)(Screen.width / 2) * Vector2.right + (float)(Screen.height / 2) * Vector2.up, num - this.target.nearClipPlane);
        this.SetTargetPos(position + this.cameraFocasCenter - rayPoint);
    }

    private Vector3 GetRayPoint(Vector3 v3, float dis)
    {
        return this.target.ScreenPointToRay(v3).GetPoint(dis);
    }

    public static FreeCameraManager freeCamera;

    private float rotateMultiX = 0.2f;

    private float rotateMultiY = 0.2f;

    private float scaleMulti = 0.3f;

    private float minFreeViewDis = 0.5f;

    private float maxFreeViewDis = 100f;

    private float minFreeViewAngle;

    private float maxFreeViewAngle = 89f;

    private Camera target;

    private Vector3 cameraFocasCenter = Vector3.zero;

    private Vector3 oldPos;

    private Vector3 oldPos1;

    private List<Vector2> offsetList = new List<Vector2>();

    private int offsetCount = 5;
}
