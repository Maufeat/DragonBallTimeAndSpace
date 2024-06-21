using System;
using Framework.Managers;
using Game.Scene;
using UnityEngine;

public class CameraFollowTargetMonitor : ICameraState
{
    private CameraZoneInfo czInfo
    {
        get
        {
            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
            if (manager != null)
            {
                return manager.sceneData.cameraZoneInfo;
            }
            return null;
        }
    }

    public Vector3 TargetPos()
    {
        return Vector3.zero;
    }

    public Vector3 TopPos()
    {
        return Vector3.zero;
    }

    public Vector3 FeetPos()
    {
        return Vector3.zero;
    }

    public void OnEnter(CameraStateMachine cameraCtrl)
    {
        this._cameraCtrl = (cameraCtrl as CameraController);
        this._tranTarget = this._cameraCtrl.Target;
        this._tranCamera = this._cameraCtrl.transform;
        this.bNeedCheck = true;
        if (SingletonForMono<InputController>.Instance.InputDir != -1)
        {
            this.bUpdateCameraAngle = false;
        }
    }

    public void ChangeState()
    {
    }

    public void OnUpdate(CameraStateMachine cameraCtrl)
    {
        if (!this.bUpdateCameraAngle && SingletonForMono<InputController>.Instance.InputDir == -1)
        {
            this.bUpdateCameraAngle = true;
        }
        if (this.bDelayCheck)
        {
            if (0f < this.fDelayTime)
            {
                this.fDelayTime -= Scheduler.Instance.realDeltaTime;
            }
            else
            {
                this.bDelayCheck = false;
            }
        }
        this.Apply();
    }

    public void OnExit(CameraStateMachine cameraCtrl)
    {
    }

    public void OnGUI()
    {
    }

    private void CheckChangeCamera(Vector3 v3MainPlyaerPos)
    {
        int currentCameraIndex = this.GetCurrentCameraIndex(v3MainPlyaerPos);
        if (this._nCurrentCameraID != currentCameraIndex)
        {
            this._nCurrentCameraID = currentCameraIndex;
            this._tranCamera.position = this.czInfo._lstData[this._nCurrentCameraID - 1].v3CameraPos;
            this.bDelayCheck = true;
            this.fDelayTime = 1f;
        }
    }

    private int GetCurrentCameraIndex(Vector3 v3MainPlayerPos)
    {
        float num = float.MaxValue;
        int result = 1;
        for (int i = 0; i < this.czInfo._nCount; i++)
        {
            if (this.IsInZone(v3MainPlayerPos, this.czInfo._lstData[i].v3ZoneAA, this.czInfo._lstData[i].v3ZoneBB))
            {
                return this.czInfo._lstData[i].nZoneID;
            }
            float xzdistance = this.GetXZDistance(v3MainPlayerPos, this.czInfo._lstData[i].v3ZoneAA, this.czInfo._lstData[i].v3ZoneBB);
            if (xzdistance < num)
            {
                num = xzdistance;
                result = this.czInfo._lstData[i].nZoneID;
            }
        }
        return result;
    }

    private bool IsInZone(Vector3 v3Pos, Vector3 v3AA, Vector3 v3BB)
    {
        return v3AA.x <= v3Pos.x && v3Pos.x <= v3BB.x && v3BB.z <= v3Pos.z && v3Pos.z <= v3AA.z;
    }

    private float GetXZDistance(Vector3 v3Pos, Vector3 v3AA, Vector3 v3BB)
    {
        Vector3 b = (v3AA + v3BB) * 0.5f;
        Vector3 vector = v3Pos - b;
        vector.y = 0f;
        return vector.magnitude;
    }

    private void Apply()
    {
        if (null != this._tranCamera && null != this._tranTarget)
        {
            if (this.bNeedCheck && !this.bDelayCheck && null != this.czInfo)
            {
                this.CheckChangeCamera(this._tranTarget.position);
            }
            this._tranCamera.LookAt(this._tranTarget);
            this.bNeedCheck = false;
            if (SingletonForMono<InputController>.Instance.InputDir != -1)
            {
                this.bUpdateCameraAngle = false;
            }
        }
    }

    public void SetTarget(Transform tranTarget)
    {
        this._tranTarget = tranTarget;
    }

    private const float fDelayCheckTime = 1f;

    public bool bNeedCheck;

    public bool bUpdateCameraAngle = true;

    private Transform _tranCamera;

    private Transform _tranTarget;

    private CameraController _cameraCtrl;

    private int _nCurrentCameraID;

    private bool bDelayCheck;

    private float fDelayTime = 1f;
}
