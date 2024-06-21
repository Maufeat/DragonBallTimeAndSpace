using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;

public class CameraFollowTarget4 : ICameraState
{
    public Vector3 TargetPos()
    {
        if (this._target == null)
        {
            return this.CenterOffset;
        }
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

    private bool CanMoveCamera
    {
        get
        {
            if (MainPlayer.Self != null)
            {
                MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
                if (component != null && component.TargetCharactor != null && component.TargetCharactor.ModelObj != null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    private Vector3 LookTarget
    {
        get
        {
            if (MainPlayer.Self != null)
            {
                MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
                if (component != null && component.TargetCharactor != null && component.TargetCharactor.ModelObj != null)
                {
                    return component.TargetCharactor.ModelObj.transform.position + this.CenterOffset;
                }
            }
            return this._target.transform.position + this.CenterOffset;
        }
    }

    public void OnEnter(CameraStateMachine CameraCtrl)
    {
        this.mCameraCtrl = (CameraCtrl as CameraController);
        this.mCameraCtrl._bcamera3D = true;
        this._target = this.mCameraCtrl.Target;
        this.cameraTransform = this.mCameraCtrl.transform;
        this.LastPos = this.TargetPos();
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        this.Maxdistance = xmlConfigTable.GetCacheField_Table("CameraMaxdistance").GetCacheField_Float("value");
        this.Mindistance = xmlConfigTable.GetCacheField_Table("CameraMindistance").GetCacheField_Float("value");
        this.Maxdistance = ControllerManager.Instance.GetController<SystemSettingController>().GetMaxCameraDistance();
        this.MaxAngle = xmlConfigTable.GetCacheField_Table("CameraMaxAngle").GetCacheField_Float("value") * 0.0174532924f;
        this.MinAngle = xmlConfigTable.GetCacheField_Table("CameraMinAngle").GetCacheField_Float("value") * 0.0174532924f;
        this.NormAngle = xmlConfigTable.GetCacheField_Table("CameraNormAngle").GetCacheField_Float("value") * 0.0174532924f;
        this.LockHalfAngle = xmlConfigTable.GetCacheField_Table("CameraLockAngle").GetCacheField_Float("value") / 2f;
        this.NormDistance = xmlConfigTable.GetCacheField_Table("CameraNormDistance").GetCacheField_Float("value");
        this.ZoomSpeed = xmlConfigTable.GetCacheField_Table("CameraZoomSpeed").GetCacheField_Float("value");
        this.BMOVE_CALIBRATION_Y_ANGINE = (xmlConfigTable.GetCacheField_Table("MoveCalibrationYAngle").GetField_Int("value") != 0);
        this.BRESETBUTTON_CALIBRATION_Y_ANGINE = (xmlConfigTable.GetCacheField_Table("ResetButtonCalibrationYAngle").GetField_Int("value") != 0);
        this.BTRACKING_ADJUST_CAMERA = (xmlConfigTable.GetCacheField_Table("TrackingAdjustCamera").GetField_Int("value") != 0);
        CameraFollowTarget4.angleV = this.NormAngle;
        this.setFocuseSpeed();
        this.ResetCameraSpeed = xmlConfigTable.GetCacheField_Table("CameraResetSpeed").GetCacheField_Float("value");
        this.AglinDelay();
        this.initGuiset();
    }

    public void setFocuseSpeed()
    {
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        switch (GameSystemSettings.GetCameraSpeedType())
        {
            case CameraSpeedType.slow:
                this.FocusSpeed = xmlConfigTable.GetCacheField_Table("CameraFocusSpeedslow").GetCacheField_Float("value");
                break;
            case CameraSpeedType.normal:
                this.FocusSpeed = xmlConfigTable.GetCacheField_Table("CameraFocusSpeednormal").GetCacheField_Float("value");
                break;
            case CameraSpeedType.fast:
                this.FocusSpeed = xmlConfigTable.GetCacheField_Table("CameraFocusSpeedfast").GetCacheField_Float("value");
                break;
        }
    }

    public static float angleV
    {
        get
        {
            return CameraFollowTarget4.angleV_;
        }
        set
        {
            CameraFollowTarget4.angleV_ = value;
        }
    }

    public void Apply(Vector3 CurrDis, float angle, float angleh, bool onReset = false)
    {
        if (CurrDis.x == 0f && CurrDis.z == 0f)
        {
            CurrDis.x += 0.01f;
        }
        float num = Mathf.Sqrt(CurrDis.x * CurrDis.x + CurrDis.z * CurrDis.z);
        float num2 = CurrDis.y;
        bool flag = (onReset && this.BRESETBUTTON_CALIBRATION_Y_ANGINE) || Mathf.Abs(angleh) >= 0.001f || this.BMOVE_CALIBRATION_Y_ANGINE;
        if (Mathf.Abs(angleh) > 0.01f && !onReset)
        {
            CameraFollowTarget4.angleV -= angleh / 180f * 3.14159274f;
        }
        else if (flag)
        {
            CameraFollowTarget4.angleV = Mathf.Atan(num2 / num);
            if (onReset && this.CameraSpeed == 0f)
            {
                this.CameraSpeed = 10f;
            }
            if ((onReset || CameraFollowTarget4.angleV < this.NormAngle) && Mathf.Abs(angleh) < 0.01f)
            {
                CameraFollowTarget4.angleV = Mathf.SmoothDampAngle(CameraFollowTarget4.angleV, this.NormAngle, ref this.angleVelocity, 0.1f, this.CameraSpeed * this.RiseRate);
            }
        }
        if (CameraFollowTarget4.angleV > this.MaxAngle)
        {
            CameraFollowTarget4.angleV = this.MaxAngle;
        }
        if (CameraFollowTarget4.angleV < this.MinAngle)
        {
            CameraFollowTarget4.angleV = this.MinAngle;
        }
        num2 = Mathf.Sin(CameraFollowTarget4.angleV) * CurrDis.magnitude;
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
        Vector3 vector2 = this.TargetPos() + vector;
        RaycastHit raycastHit;
        if (Physics.Raycast(new Ray(vector2, Vector3.down), out raycastHit, 0.5f, Const.LayerForMask.Terrian) && raycastHit.distance < 0.3f)
        {
            vector2.y = raycastHit.point.y + 0.3f;
        }
        this.cameraTransform.position = vector2;
    }

    public void ResetCameraPos()
    {
        CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
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
        CameraFollowTarget4.targetdistance = this.NormDistance;
        CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
        this.CurrDisForReset = this.cameraTransform.position - this.TargetPos();
        this.bResetCamera = true;
    }

    private void ResetCameraPostion()
    {
        float num = CameraFollowTarget4.CurrDis.magnitude;
        num = Mathf.SmoothDamp(num, CameraFollowTarget4.targetdistance, ref this.disVelocity, 0.3f, 20f);
        if (num > this.Maxdistance)
        {
            num = this.Maxdistance;
        }
        if (num < this.Mindistance)
        {
            num = this.Mindistance;
        }
        float num2 = 0f;
        if (Mathf.Abs(this.resetCurrentAngles) < Mathf.Abs(this.resetTargetAngles))
        {
            num2 = this.resetTargetAngles * Scheduler.Instance.realDeltaTime * this.ResetCameraSpeed;
            this.resetCurrentAngles += num2;
            if (Mathf.Abs(this.resetCurrentAngles) >= Mathf.Abs(this.resetTargetAngles))
            {
                num2 = 0f;
            }
        }
        if (Mathf.Abs(this.resetCurrentAngles) >= Mathf.Abs(this.resetTargetAngles))
        {
            this.resetCurrentAngles = this.resetTargetAngles;
            this.bResetCamera = false;
            this.Apply(CameraFollowTarget4.CurrDis.normalized * num, num2, 0f, true);
            CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
            this.resetCurrentAngles = 0f;
            this.resetTargetAngles = 0f;
        }
        else
        {
            this.Apply(CameraFollowTarget4.CurrDis.normalized * num, num2, 0f, true);
            CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
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
        CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
        float num = CameraFollowTarget4.CurrDis.magnitude;
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
            this.Apply(CameraFollowTarget4.CurrDis.normalized * num, 0f, 0f, false);
            CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
        }
        else
        {
            this.Apply(CameraFollowTarget4.CurrDis.normalized * num, 0f, 0f, false);
        }
        this.cameraTransform.LookAt(this.TargetPos());
    }

    private void SetZoomCamera()
    {
        CameraFollowTarget4.targetdistance -= this.mCameraCtrl.mfMouseScrollWheel * this.ZoomSpeed * Scheduler.Instance.realDeltaTime;
    }

    public void AddTargetDistance(float distance)
    {
        CameraFollowTarget4.targetdistance += distance;
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
        this.SetZoomCamera();
        if (CameraFollowTarget4.targetdistance > this.Maxdistance)
        {
            CameraFollowTarget4.targetdistance = this.Maxdistance;
        }
        if (CameraFollowTarget4.targetdistance < this.Mindistance)
        {
            CameraFollowTarget4.targetdistance = this.Mindistance;
        }
        Vector3 vector = Vector3.zero;
        if (this.CanMoveCamera || !GameSystemSettings.GetCameraTrack() || !this.mCameraCtrl.InBattle)
        {
            vector = this.mCameraCtrl.Rotatediff * Scheduler.Instance.realDeltaTime * this.RotationSpeed;
        }
        else if (GameSystemSettings.GetCameraTrack())
        {
            if (this.BTRACKING_ADJUST_CAMERA)
            {
                vector = this.mCameraCtrl.Rotatediff * Scheduler.Instance.realDeltaTime * this.RotationSpeed;
            }
            else if (this.mCameraCtrl.HasInput && this.cant_Adjust_Camera_Tranking)
            {
                this.cant_Adjust_Camera_Tranking = false;
                TipsWindow.ShowWindow(TipsType.CANT_ADJUST_CAMERA_TRANKING, null);
            }
        }
        if ((double)vector.magnitude > 0.0001)
        {
            this.Lock = 10;
        }
        if (this.Lock >= 0)
        {
            this.Lock--;
        }
        if (this.Lock < 0 && !this.mCameraCtrl.InBattle)
        {
            Vector3 vector2 = this.TargetPos() - this.LastPos;
            vector2.y = 0f;
            this.CameraSpeed = vector2.magnitude / Scheduler.Instance.realDeltaTime;
        }
        else
        {
            this.CameraSpeed = 0f;
        }
        float num = CameraFollowTarget4.CurrDis.magnitude;
        num = Mathf.SmoothDamp(num, CameraFollowTarget4.targetdistance, ref this.disVelocity, 0.3f, 20f);
        if (num > this.Maxdistance)
        {
            num = this.Maxdistance;
        }
        if (num < this.Mindistance)
        {
            num = this.Mindistance;
        }
        if (CameraFollowTarget4.CurrDis.normalized == Vector3.zero)
        {
            CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
        }
        Vector3 old = this.LookTarget - this.TargetPos();
        float num2 = 0f;
        if (GameSystemSettings.GetCameraTrack() && !this.mCameraCtrl.HasInput)
        {
            this.cant_Adjust_Camera_Tranking = true;
            if (this.mCameraCtrl.InBattle && old.magnitude > 2f)
            {
                float num3 = CommonTools.DismissYSize(-CameraFollowTarget4.CurrDis).AngleWithNormal(CommonTools.DismissYSize(old), Vector3.up);
                if (Mathf.Abs(num3) < this.LockHalfAngle)
                {
                    num2 = 0f;
                }
                else
                {
                    float num4 = this.FocusSpeed * Scheduler.Instance.realDeltaTime;
                    num2 = Mathf.Abs(num3) - this.LockHalfAngle;
                    num2 = ((num2 >= num4) ? num4 : num2);
                    num2 = ((num3 <= 0f) ? -1f : 1f) * num2;
                }
            }
        }
        this.Apply(CameraFollowTarget4.CurrDis.normalized * num, num2 + vector.x, vector.y, false);
        CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
        this.cameraTransform.LookAt(this.TargetPos());
        this.LastPos = this.TargetPos();
    }

    public void OnExit(CameraStateMachine CameraCtrl)
    {
    }

    private void initGuiset()
    {
        this.gMaxdistance = this.Maxdistance;
        this.gMindistance = this.Mindistance;
        this.gMaxAngle = this.MaxAngle * 57.29578f;
        this.gMinAngle = this.MinAngle * 57.29578f;
        this.gNormAngle = this.NormAngle * 57.29578f;
        this.gLockAngle = this.LockHalfAngle * 2f;
        this.gFocusSpeed = this.FocusSpeed;
        this.gResetCameraSpeed = this.ResetCameraSpeed;
        this.gZoomSpeed = this.ZoomSpeed;
        this.gTargetDis = this.NormDistance;
        this.gbMoveCalibrationYAngle = this.BMOVE_CALIBRATION_Y_ANGINE;
        this.gbResetButtonCalibrationYAngle = this.BRESETBUTTON_CALIBRATION_Y_ANGINE;
        this.gbTrackingAdjustCamera = this.BTRACKING_ADJUST_CAMERA;
    }

    public void OnGUI()
    {
        this.gMaxdistance = MyEditorGUILayout.FloatField("相机与目标最大距离（单位：米）", this.gMaxdistance, 260f);
        this.gMindistance = MyEditorGUILayout.FloatField("相机与目标最小距离（单位：米）", this.gMindistance, 260f);
        this.gMaxAngle = MyEditorGUILayout.FloatField("相机与地面最大角度（单位：度）", this.gMaxAngle, 260f);
        this.gMinAngle = MyEditorGUILayout.FloatField("相机与地面最小角度（单位：度）", this.gMinAngle, 260f);
        this.gNormAngle = MyEditorGUILayout.FloatField("相机与地面默认角度角度（单位：度）", this.gNormAngle, 260f);
        this.gLockAngle = MyEditorGUILayout.FloatField("相机目标锁定角度（单位：度）", this.gLockAngle, 260f);
        this.gFocusSpeed = MyEditorGUILayout.FloatField("相机锁定目标速度 （单位：度/秒）", this.gFocusSpeed, 260f);
        this.gResetCameraSpeed = MyEditorGUILayout.FloatField("相机归正速度 （单位：度/秒）", this.gResetCameraSpeed, 260f);
        this.gTargetDis = MyEditorGUILayout.FloatField("相机与目标的默认距离（单位：米）", this.gTargetDis, 260f);
        this.gZoomSpeed = MyEditorGUILayout.FloatField("相机的缩放速度（单位：米/秒）", this.gZoomSpeed, 260f);
        this.gbMoveCalibrationYAngle = MyEditorGUILayout.Toggle("移动归位调整俯仰角（说明：勾选允许）", this.gbMoveCalibrationYAngle, 260f);
        this.gbResetButtonCalibrationYAngle = MyEditorGUILayout.Toggle("归位按钮调整俯仰角（说明：勾选允许）", this.gbResetButtonCalibrationYAngle, 260f);
        this.gbTrackingAdjustCamera = MyEditorGUILayout.Toggle("镜头追踪拉镜头（说明：勾选允许）", this.gbTrackingAdjustCamera, 260f);
        MyEditorGUILayout.Button("确定", delegate
        {
            this.Maxdistance = this.gMaxdistance;
            this.Mindistance = this.gMindistance;
            this.MaxAngle = this.gMaxAngle * 0.0174532924f;
            this.MinAngle = this.gMinAngle * 0.0174532924f;
            this.NormAngle = this.gNormAngle * 0.0174532924f;
            this.LockHalfAngle = this.gLockAngle / 2f;
            this.FocusSpeed = this.gFocusSpeed;
            this.ResetCameraSpeed = this.gResetCameraSpeed;
            this.ZoomSpeed = this.gZoomSpeed;
            this.NormDistance = this.gTargetDis;
            this.BMOVE_CALIBRATION_Y_ANGINE = this.gbMoveCalibrationYAngle;
            this.BRESETBUTTON_CALIBRATION_Y_ANGINE = this.gbResetButtonCalibrationYAngle;
            this.BTRACKING_ADJUST_CAMERA = this.gbTrackingAdjustCamera;
        }, 0f, 0f);
    }

    public void AglinDelay()
    {
        if (CameraFollowTarget4.CurrDis != Vector3.zero)
        {
            this.cameraTransform.position = this.TargetPos() + CameraFollowTarget4.CurrDis;
            this.cameraTransform.LookAt(this.TargetPos());
        }
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.Aglin));
    }

    public void Aglin()
    {
        if (this.cameraTransform == null)
        {
            return;
        }
        CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
        float num = CameraFollowTarget4.targetdistance;
        if (num > this.Maxdistance)
        {
            num = this.Maxdistance;
        }
        if (num < this.Mindistance)
        {
            num = this.Mindistance;
        }
        float angle = this._target.transform.rotation.eulerAngles.y - this.cameraTransform.rotation.eulerAngles.y;
        if (!CameraController.IsRelive)
        {
            int num2 = (int)ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
            if (CameraController.Self.LastSceneID != num2)
            {
                CameraController.Self.LastSceneID = num2;
            }
            else
            {
                angle = 0f;
            }
        }
        if (CameraController.IsReliveOrg)
        {
            CameraController.IsReliveOrg = false;
            angle = 0f;
        }
        this.Apply(CameraFollowTarget4.CurrDis.normalized * num, angle, 0f, false);
        CameraFollowTarget4.CurrDis = this.cameraTransform.position - this.TargetPos();
    }

    public void SetCamAndPlayerDir(float playerDir, float angleBetween, float distToPlayer)
    {
    }

    public void SetCameraDirDistAngleV(uint dir, float dist, float angleVer)
    {
        MainPlayer.Self.ServerDir = dir;
        Vector2 currentPosition2D = MainPlayer.Self.CurrentPosition2D;
        currentPosition2D.x = GraphUtils.Keep2DecimalPlaces(currentPosition2D.x);
        currentPosition2D.y = GraphUtils.Keep2DecimalPlaces(currentPosition2D.y);
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.dir = dir;
        cs_MoveData.pos = default(cs_FloatMovePos);
        cs_MoveData.pos.fx = currentPosition2D.x;
        cs_MoveData.pos.fy = currentPosition2D.y;
        MainPlayer.Self.MoveDir(cs_MoveData);
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqMove(cs_MoveData, false, false);
        Scheduler.Instance.AddTimer(0.2f, false, delegate
        {
            CameraFollowTarget4.targetdistance = dist;
            Vector3 vector = -MainPlayer.Self.ModelObj.transform.forward;
            float num = 0.0174532924f * angleVer;
            vector.y = Mathf.Tan(num);
            CameraFollowTarget4.CurrDis = vector.normalized * CameraFollowTarget4.targetdistance;
            CameraFollowTarget4.angleV = num;
            SingletonForMono<InputController>.Instance.angle = MainPlayer.Self.ModelObj.transform.rotation.eulerAngles.y;
        });
    }

    public float GetVerticleAngleBetweenCamAndPlayer()
    {
        if (this.cameraTransform != null && MainPlayer.Self != null && MainPlayer.Self.ModelObj)
        {
            return 57.29578f * CameraFollowTarget4.angleV;
        }
        return 0f;
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

    private float NormDistance = 12f;

    private float LockHalfAngle = 2f;

    private float FocusSpeed = 270f;

    private float ZoomSpeed = 200f;

    private float ResetCameraSpeed = 5f;

    private bool BMOVE_CALIBRATION_Y_ANGINE = true;

    private bool BRESETBUTTON_CALIBRATION_Y_ANGINE = true;

    private bool BTRACKING_ADJUST_CAMERA = true;

    private bool bTrackTargeting;

    public float Inithight;

    private float angleVelocity;

    public static float angleV_ = -100f;

    private float RotationSpeed = 6f;

    private float RiseRate = 0.3f;

    private Vector3 LastPos;

    private float CameraSpeed;

    private Vector3 CurrDisForReset;

    public static Vector3 CurrDis;

    public static float targetdistance = 12f;

    private float disVelocity;

    private bool bResetCamera;

    private float resetTargetAngles;

    private float resetCurrentAngles;

    private bool bChangeState;

    private float startChageTime;

    private float changeStateduration = 0.8f;

    private bool cant_Adjust_Camera_Tranking = true;

    private int Lock = -1;

    private bool EditorWinShow;

    private float gMaxdistance;

    private float gMindistance;

    private float gMaxAngle;

    private float gMinAngle;

    private float gNormAngle;

    private float gLockAngle;

    private float gFocusSpeed;

    private float gResetCameraSpeed;

    private float gZoomSpeed;

    private float gTargetDis;

    private bool gbMoveCalibrationYAngle;

    private bool gbResetButtonCalibrationYAngle;

    private bool gbTrackingAdjustCamera;
}
