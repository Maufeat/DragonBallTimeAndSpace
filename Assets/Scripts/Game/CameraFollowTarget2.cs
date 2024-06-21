using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;

public class CameraFollowTarget2 : ICameraState
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

    public void setTarget(Transform tran)
    {
        this._target = tran;
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

    public void ChangeState()
    {
    }

    public void OnEnter(CameraStateMachine CameraCtrl)
    {
        this.mCameraCtrl = (CameraCtrl as CameraController);
        this._target = this.mCameraCtrl.Target;
        this.cameraTransform = this.mCameraCtrl.transform;
        this.LastPos = this.TargetPos();
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        this.MaxAngle = xmlConfigTable.GetCacheField_Table("CameraMaxAngle").GetCacheField_Float("value");
        this.MinAngle = xmlConfigTable.GetCacheField_Table("CameraMinAngle").GetCacheField_Float("value");
        this.NormAngle = xmlConfigTable.GetCacheField_Table("CameraNormAngle").GetCacheField_Float("value");
        this.Mindistance = xmlConfigTable.GetCacheField_Table("CameraMindistance").GetCacheField_Float("value");
        this.Maxdistance = xmlConfigTable.GetCacheField_Table("CameraMaxdistance").GetCacheField_Float("value");
        this.Maxdistance = ControllerManager.Instance.GetController<SystemSettingController>().GetMaxCameraDistance();
        this.Apply(new Vector3(4f, 3f, 0f), 0f, 0f);
        this.AglinDelay();
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

    public void AddTargetDistance(float distance)
    {
        this.TargetDis += distance;
    }

    public void OnUpdate(CameraStateMachine CameraCtrl)
    {
        if (this.mCameraCtrl.InBattle)
        {
            this.Lock = true;
            this.TargetDis = this.Maxdistance;
        }
        else
        {
            this.Lock = false;
            this.TargetDis = this.Mindistance;
        }
        Vector3 vector = this.mCameraCtrl.Rotatediff * Scheduler.Instance.realDeltaTime * this.RotationSpeed;
        if (!this.Lock)
        {
            this.CurrDis = this.cameraTransform.position - this.TargetPos();
            Vector3 vector2 = this.TargetPos() - this.LastPos;
            vector2.y = 0f;
            this.CameraSpeed = vector2.magnitude / Scheduler.Instance.realDeltaTime;
        }
        else
        {
            this.CameraSpeed = 0f;
        }
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
        if (this.CurrDis.normalized == Vector3.zero)
        {
            this.CurrDis = this.cameraTransform.position - this.TargetPos();
        }
        this.Apply(this.CurrDis.normalized * num, vector.x, vector.y);
        this.CurrDis = this.cameraTransform.position - this.TargetPos();
        this.cameraTransform.LookAt(this.TargetPos());
        this.LastPos = this.TargetPos();
    }

    public void OnExit(CameraStateMachine CameraCtrl)
    {
    }

    public void OnGUI()
    {
        this.MaxAngle = MyEditorGUILayout.FloatField("最大角度", this.MaxAngle, 260f);
        this.MinAngle = MyEditorGUILayout.FloatField("最小角度", this.MinAngle, 260f);
        this.NormAngle = MyEditorGUILayout.FloatField("默认角度", this.NormAngle, 260f);
        this.Maxdistance = MyEditorGUILayout.FloatField("最大距离", this.Maxdistance, 260f);
        this.Mindistance = MyEditorGUILayout.FloatField("最小距离", this.Mindistance, 260f);
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
            uint num = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
            if ((long)CameraController.Self.LastSceneID == (long)((ulong)num))
            {
                return;
            }
            CameraController.Self.LastSceneID = (int)num;
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

    private Vector3 centerOffset = new Vector3(0f, 1f, 0f);

    private Vector3 topOffset = new Vector3(0f, 1f, 0f);

    private Vector3 feetOffset = new Vector3(0f, 1f, 0f);

    private CameraController mCameraCtrl;

    private float MaxAngle = 0.8f;

    private float MinAngle = -0.3f;

    private float NormAngle = 0.6f;

    private float angleVelocity;

    private float RotationSpeed = 6f;

    private float RiseRate = 0.3f;

    private Vector3 LastPos;

    private float CameraSpeed;

    private Vector3 CurrDis;

    private float TargetDis = 3f;

    private float disVelocity;

    private bool Lock;

    private bool EditorWinShow;
}
