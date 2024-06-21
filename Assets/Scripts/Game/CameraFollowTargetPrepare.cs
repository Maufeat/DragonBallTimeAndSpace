using System;
using LuaInterface;
using UnityEngine;

public class CameraFollowTargetPrepare : ICameraState
{
    public Vector3 TargetPos()
    {
        return this._target.transform.position + this.CenterOffset;
    }

    public Vector3 TopPos()
    {
        return this._target.transform.position + this.TopOffSet;
    }

    public Vector3 FeetPos()
    {
        return this._target.transform.position + this.FeetOffset;
    }

    private Vector3 TopOffSet
    {
        get
        {
            if (MainPlayer.Self != null && null != MainPlayer.Self.TopPos)
            {
                this.topOffset = MainPlayer.Self.TopPos.localPosition;
            }
            return this.topOffset;
        }
    }

    private Vector3 FeetOffset
    {
        get
        {
            if (MainPlayer.Self != null && null != MainPlayer.Self.FeetPos)
            {
                this.feetOffset = MainPlayer.Self.FeetPos.localPosition;
            }
            return this.feetOffset;
        }
    }

    public void setTarget(Transform tran)
    {
        this._target = tran;
    }

    private Vector3 CenterOffset
    {
        get
        {
            if (MainPlayer.Self != null && null != MainPlayer.Self.CameraPos)
            {
                this.centerOffset = MainPlayer.Self.CameraPos.localPosition;
            }
            return this.centerOffset;
        }
    }

    public void OnEnter(CameraStateMachine CameraCtrl)
    {
        this.mCameraCtrl = (CameraCtrl as CameraController);
        this._target = this.mCameraCtrl.Target;
        this.cameraTransform = this.mCameraCtrl.transform;
        this.LastPos = this.TargetPos();
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        this.Maxdistance = xmlConfigTable.GetCacheField_Table("CameraMaxdistancePVP").GetCacheField_Float("value");
        this.Mindistance = xmlConfigTable.GetCacheField_Table("CameraMindistancePVP").GetCacheField_Float("value");
        this.MaxAngle = xmlConfigTable.GetCacheField_Table("CameraMaxAnglePVP").GetCacheField_Float("value") * 0.0174532924f;
        this.MinAngle = xmlConfigTable.GetCacheField_Table("CameraMinAnglePVP").GetCacheField_Float("value") * 0.0174532924f;
        this.NormAngle = xmlConfigTable.GetCacheField_Table("CameraNormAnglePVP").GetCacheField_Float("value") * 0.0174532924f;
        this.LockHalfAngle = xmlConfigTable.GetCacheField_Table("CameraLockAnglePVP").GetCacheField_Float("value") / 2f;
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        float magnitude = this.CurrDis.magnitude;
        float angle = this._target.transform.rotation.eulerAngles.y - this.cameraTransform.rotation.eulerAngles.y + 180f;
        this.Apply(this.CurrDis.normalized * magnitude, angle, 0f);
    }

    private void Apply(Vector3 CurrDis, float angle, float angleh)
    {
        if (CurrDis.x == 0f && CurrDis.z == 0f)
        {
            CurrDis.x += 0.01f;
        }
        float num = Mathf.Sqrt(CurrDis.x * CurrDis.x + CurrDis.z * CurrDis.z);
        float num2 = CurrDis.y;
        float num3 = Mathf.Atan(num2 / num);
        num3 -= angleh / 180f * 3.14159274f;
        if (num3 < this.NormAngle && Mathf.Abs(angleh) < 0.01f)
        {
            num3 = Mathf.SmoothDampAngle(num3, this.NormAngle, ref this.angleVelocity, 0.1f, this.CameraSpeed * this.RiseRate);
        }
        if (num3 > this.MaxAngle)
        {
            num3 = this.MaxAngle;
        }
        if (num3 < this.MinAngle)
        {
            num3 = this.MinAngle;
        }
        num2 = Mathf.Sin(num3) * CurrDis.magnitude;
        if ((double)num2 < -0.2)
        {
            num2 = -0.2f;
        }
        num = Mathf.Sqrt(CurrDis.sqrMagnitude - num2 * num2);
        Vector3 vector = CurrDis;
        vector.y = 0f;
        vector = vector.normalized;
        vector *= num;
        vector.y = num2;
        vector = Quaternion.Euler(new Vector3(0f, angle, 0f)) * vector;
        this.cameraTransform.position = this.TargetPos() + vector;
    }

    public void OnUpdate(CameraStateMachine CameraCtrl)
    {
        Vector3 zero = Vector3.zero;
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        float num = this.CurrDis.magnitude;
        num = Mathf.SmoothDamp(num, this.TargetDis, ref this.disVelocity, 0.3f, 20f);
        if (num > this.Maxdistance)
        {
            num = this.Maxdistance;
        }
        if (num < this.Mindistance)
        {
            num = this.Mindistance;
        }
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        float angle = this._target.transform.rotation.eulerAngles.y - this.cameraTransform.rotation.eulerAngles.y + 180f;
        this.Apply(this.CurrDis.normalized * num, angle, 0f);
        this.cameraTransform.LookAt(this.TargetPos());
        this.LastPos = this.TargetPos();
    }

    public void OnExit(CameraStateMachine CameraCtrl)
    {
    }

    public void ChangeState()
    {
    }

    public void OnGUI()
    {
    }

    public Transform cameraTransform;

    private Transform _target;

    public float Maxdistance = 9f;

    public float Mindistance = 3f;

    private Vector3 centerOffset = new Vector3(0f, 1f, 0f);

    private Vector3 topOffset = new Vector3(0f, 1f, 0f);

    private Vector3 feetOffset = new Vector3(0f, 1f, 0f);

    private CameraController mCameraCtrl;

    private float MaxAngle = 0.8f;

    private float MinAngle = -0.3f;

    private float NormAngle = 0.6f;

    private float LockHalfAngle = 2f;

    private float FocusSpeed = 270f;

    private float ResetCameraSpeed = 5f;

    private float angleVelocity;

    private float RotationSpeed = 6f;

    private float RiseRate = 0.3f;

    private Vector3 LastPos;

    private float CameraSpeed;

    private Vector3 CurrDis;

    private float TargetDis = 3f;

    private float disVelocity;
}
