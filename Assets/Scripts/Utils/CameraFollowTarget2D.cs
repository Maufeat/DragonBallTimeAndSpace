﻿using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;

public class CameraFollowTarget2D : ICameraState
{
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

    public void OnEnter(CameraStateMachine CameraCtrl)
    {
        this.mCameraCtrl = (CameraCtrl as CameraController);
        this.mCameraCtrl._bcamera3D = false;
        this._target = this.mCameraCtrl.Target;
        this.cameraTransform = this.mCameraCtrl.transform;
        this.LastPos = this.TargetPos();
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        this.Maxdistancefar = xmlConfigTable.GetCacheField_Table("CameraMaxdistancefar2d").GetCacheField_Float("value");
        this.Mindistancefar = xmlConfigTable.GetCacheField_Table("CameraMindistancefar2d").GetCacheField_Float("value");
        this.Maxdistancemedium = xmlConfigTable.GetCacheField_Table("CameraMaxdistanceMedium2d").GetCacheField_Float("value");
        this.Mindistancemedium = xmlConfigTable.GetCacheField_Table("CameraMindistanceMedium2d").GetCacheField_Float("value");
        this.MaxAngle = xmlConfigTable.GetCacheField_Table("CameraMaxAngle2d").GetCacheField_Float("value") * 0.0174532924f;
        this.MinAngle = xmlConfigTable.GetCacheField_Table("CameraMinAngle2d").GetCacheField_Float("value") * 0.0174532924f;
        this.NormAngle = xmlConfigTable.GetCacheField_Table("CameraNormAngle2d").GetCacheField_Float("value") * 0.0174532924f;
        this.SetDistence();
        this.ResetCameraSpeed = xmlConfigTable.GetCacheField_Table("CameraResetSpeed2d").GetCacheField_Float("value");
        this.AglinDelay();
        this.initGuiset();
    }

    public void SetDistence()
    {
        this.Mindistance = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("CameraMindistance").GetCacheField_Float("value");
        this.Maxdistance = ControllerManager.Instance.GetController<SystemSettingController>().GetMaxCameraDistance();
    }

    public void SetDistenceFar(bool bfar)
    {
        if (bfar)
        {
            this.Maxdistance = this.Maxdistancefar;
            this.Mindistance = this.Mindistancefar;
        }
        else
        {
            this.Maxdistance = this.Maxdistancemedium;
            this.Mindistance = this.Mindistancemedium;
        }
    }

    private void Apply(Vector3 CurrDis, float angle, float angleh)
    {
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

    public void ResetCameraPos()
    {
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
    }

    public void ResetCamera()
    {
        this.resetTargetAngles = this._target.transform.rotation.eulerAngles.y - this.cameraTransform.rotation.eulerAngles.y;
        if (Mathf.Abs(this.resetTargetAngles) > 180f)
        {
            if (this.resetTargetAngles > 0f)
            {
                this.resetTargetAngles -= 360f;
            }
            else
            {
                this.resetTargetAngles += 360f;
            }
        }
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        this.bResetCamera = true;
    }

    private void ResetCameraPostion()
    {
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
        this.resetCurrentAngles += this.resetTargetAngles * Scheduler.Instance.realDeltaTime * this.ResetCameraSpeed;
        if (Mathf.Abs(this.resetCurrentAngles) >= Mathf.Abs(this.resetTargetAngles))
        {
            this.resetCurrentAngles = this.resetTargetAngles;
            this.bResetCamera = false;
            this.Apply(this.CurrDis.normalized * num, this.resetCurrentAngles, 0f);
            this.CurrDis = this.cameraTransform.position - this.TargetPos();
            this.resetCurrentAngles = 0f;
            this.resetTargetAngles = 0f;
        }
        else
        {
            this.Apply(this.CurrDis.normalized * num, this.resetCurrentAngles, 0f);
        }
        this.cameraTransform.LookAt(this.TargetPos());
    }

    public void ChangeState()
    {
        this.bChangeState = true;
        this.startChageTime = Time.time;
    }

    private void ChangeCamreaState()
    {
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        float num = this.CurrDis.magnitude;
        float t = (Time.time - this.startChageTime) / this.changeStateduration;
        if (num > this.Maxdistance)
        {
            num = Mathf.SmoothStep(num, this.Maxdistance, t);
        }
        if (num < this.Mindistance)
        {
            num = Mathf.SmoothStep(num, this.Mindistance, t);
        }
        if (num <= this.Maxdistance && num >= this.Mindistance)
        {
            this.bChangeState = false;
            this.startChageTime = 0f;
            this.Apply(this.CurrDis.normalized * num, 0f, 0f);
            this.CurrDis = this.cameraTransform.position - this.TargetPos();
        }
        else
        {
            this.Apply(this.CurrDis.normalized * num, 0f, 0f);
        }
        this.cameraTransform.LookAt(this.TargetPos());
    }

    public void OnUpdate(CameraStateMachine CameraCtrl)
    {
        if (this.bChangeState)
        {
            this.ChangeCamreaState();
            return;
        }
        if (this.bResetCamera)
        {
            this.ResetCameraPostion();
            return;
        }
        Vector3 vector = Vector3.zero;
        vector = this.mCameraCtrl.Rotatediff * Scheduler.Instance.realDeltaTime * this.RotationSpeed;
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
        this.Apply(this.CurrDis.normalized * num, vector.x, vector.y);
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        this.cameraTransform.LookAt(this.TargetPos());
        this.LastPos = this.TargetPos();
    }

    public void OnExit(CameraStateMachine CameraCtrl)
    {
    }

    private void initGuiset()
    {
        this.gMaxdistancefar = this.Maxdistancefar;
        this.gMindistancefar = this.Mindistancefar;
        this.gMaxdistancemedium = this.Maxdistancemedium;
        this.gMindistancemedium = this.Mindistancemedium;
        this.gMaxAngle = this.MaxAngle * 57.29578f;
        this.gMinAngle = this.MinAngle * 57.29578f;
        this.gNormAngle = this.NormAngle * 57.29578f;
        this.gResetCameraSpeed = this.ResetCameraSpeed;
    }

    public void OnGUI()
    {
        this.gMaxdistancefar = MyEditorGUILayout.FloatField("相机与目标最大距离（远距离 单位：米）", this.gMaxdistancefar, 260f);
        this.gMindistancefar = MyEditorGUILayout.FloatField("相机与目标最小距离（远距离 单位：米）", this.gMindistancefar, 260f);
        this.gMaxdistancemedium = MyEditorGUILayout.FloatField("相机与目标最大距离 (近距离（单位：米）", this.gMaxdistancemedium, 260f);
        this.gMindistancemedium = MyEditorGUILayout.FloatField("相机与目标最小距离（近距离 单位：米）", this.gMindistancemedium, 260f);
        this.gMaxAngle = MyEditorGUILayout.FloatField("相机与地面最大角度（单位：度）", this.gMaxAngle, 260f);
        this.gMinAngle = MyEditorGUILayout.FloatField("相机与地面最小角度（单位：度）", this.gMinAngle, 260f);
        this.gNormAngle = MyEditorGUILayout.FloatField("相机与地面默认角度角度（单位：度）", this.gNormAngle, 260f);
        this.gResetCameraSpeed = MyEditorGUILayout.FloatField("相机重置速度 （单位：度/秒）", this.gResetCameraSpeed, 260f);
        MyEditorGUILayout.Button("确定", delegate
        {
            this.Maxdistancefar = this.gMaxdistancefar;
            this.Mindistancefar = this.gMindistancefar;
            this.Maxdistancemedium = this.gMaxdistancemedium;
            this.Mindistancemedium = this.gMindistancemedium;
            this.MaxAngle = this.gMaxAngle * 0.0174532924f;
            this.MinAngle = this.gMinAngle * 0.0174532924f;
            this.NormAngle = this.gNormAngle * 0.0174532924f;
            this.ResetCameraSpeed = this.gResetCameraSpeed;
        }, 0f, 0f);
        MyEditorGUILayout.Button("相机归正", delegate
        {
            this.ResetCamera();
        }, 0f, 0f);
        MyEditorGUILayout.Button((!this.bFarCamera) ? "相机距离为近" : "相机距离为远", delegate
        {
            this.bFarCamera = !this.bFarCamera;
            this.SetDistenceFar(this.bFarCamera);
        }, 0f, 0f);
    }

    public void AglinDelay()
    {
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.Aglin));
    }

    public void Aglin()
    {
        if (this.cameraTransform == null)
        {
            return;
        }
        if (!CameraController.IsRelive)
        {
            int num = (int)ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
            if (CameraController.Self.LastSceneID == num)
            {
                return;
            }
            CameraController.Self.LastSceneID = num;
        }
        if (CameraController.IsReliveOrg)
        {
            CameraController.IsReliveOrg = false;
            return;
        }
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        float num2 = this.CurrDis.magnitude;
        num2 = Mathf.SmoothDamp(num2, this.TargetDis, ref this.disVelocity, 0.3f, 20f);
        if (num2 > this.Maxdistance)
        {
            num2 = this.Maxdistance;
        }
        if (num2 < this.Mindistance)
        {
            num2 = this.Mindistance;
        }
        float angle = this._target.transform.rotation.eulerAngles.y - this.cameraTransform.rotation.eulerAngles.y;
        this.Apply(this.CurrDis.normalized * num2, angle, 0f);
    }

    public void SetMaxDistance(float distance)
    {
        this.Maxdistance = distance;
    }

    public Transform cameraTransform;

    private Transform _target;

    public float Maxdistance = 9f;

    public float Mindistance = 3f;

    public float Maxdistancemedium = 9f;

    public float Mindistancemedium = 3f;

    public float Maxdistancefar = 9f;

    public float Mindistancefar = 3f;

    private Vector3 centerOffset = new Vector3(0f, 1f, 0f);

    private Vector3 topOffset = new Vector3(0f, 1f, 0f);

    private Vector3 feetOffset = new Vector3(0f, 1f, 0f);

    private CameraController mCameraCtrl;

    private float MaxAngle = 0.8f;

    private float MinAngle = -0.3f;

    private float NormAngle = 0.6f;

    private float ResetCameraSpeed = 5f;

    private float angleVelocity;

    private float RotationSpeed = 6f;

    private float RiseRate = 0.3f;

    private Vector3 LastPos;

    private float CameraSpeed;

    private Vector3 CurrDis;

    private float TargetDis = 3f;

    private float disVelocity;

    private bool bResetCamera;

    private float resetTargetAngles;

    private float resetCurrentAngles;

    private bool bChangeState;

    private float startChageTime;

    private float changeStateduration = 0.8f;

    private bool Lock;

    private bool EditorWinShow;

    private float gMaxdistancefar;

    private float gMindistancefar;

    private float gMaxdistancemedium;

    private float gMindistancemedium;

    private float gMaxAngle;

    private float gMinAngle;

    private float gNormAngle;

    private float gResetCameraSpeed;

    private bool bFarCamera = true;
}