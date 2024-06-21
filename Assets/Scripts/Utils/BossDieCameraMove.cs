using System;
using System.Collections.Generic;
using System.Text;
using Framework.Managers;
using UnityEngine;

public class BossDieCameraMove : ICameraState
{
    public float ShowUiDelay
    {
        get
        {
            return this.UIdelay;
        }
    }

    public float ShowUIduration
    {
        get
        {
            return this.UIduration;
        }
    }

    private Transform cameraTransform
    {
        get
        {
            if (this.mCameraCtrl != null)
            {
                return this.mCameraCtrl.transform;
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

    public void ChangeState()
    {
    }

    public void OnEnter(CameraStateMachine CameraCtrl)
    {
        this.mCameraCtrl = (CameraCtrl as CameraController);
    }

    public void SetTarget(CharactorBase Target)
    {
        this.BossTarget = Target;
        if (this.TargetTran != null && this.mCameraCtrl != null)
        {
            Vector3 currDis = this.TargetTran.forward * this.Sdistance;
            this.Apply(currDis, this.SRotAngle, this.SHightAngle);
        }
    }

    private void Apply(Vector3 CurrDis, float angle, float angleV)
    {
        float d = Mathf.Sqrt(CurrDis.x * CurrDis.x + CurrDis.z * CurrDis.z);
        float num = Mathf.Sin(angleV / 180f * 3.14159274f) * CurrDis.magnitude;
        if ((double)num < -0.2)
        {
            num = -0.2f;
        }
        d = Mathf.Sqrt(CurrDis.sqrMagnitude - num * num);
        Vector3 vector = CurrDis;
        vector.y = 0f;
        vector = vector.normalized;
        vector *= d;
        vector.y = num;
        vector = Quaternion.Euler(new Vector3(0f, angle, 0f)) * vector;
        this.cameraTransform.position = this.TargetTran.position + vector + this.centerOffset;
        this.cameraTransform.LookAt(this.TargetTran.position + this.centerOffset);
    }

    public void OnUpdate(CameraStateMachine CameraCtrl)
    {
        this.UpdateCameraMove();
    }

    private void UpdateCameraMove()
    {
        if (!this.OnPlay)
        {
            return;
        }
        if (this.TargetTran == null)
        {
            this.OnPlay = false;
            Time.timeScale = 1f;
            return;
        }
        this.m_PlayTime += Scheduler.Instance.realDeltaTime;
        if (this.m_PlayTime > this.TimeLength)
        {
            this.m_PlayTime = this.TimeLength;
            this.OnPlay = false;
            Time.timeScale = 1f;
        }
        if (this.TimeScale == 0f)
        {
            this.TimeScale = 1f;
        }
        float angleV = Mathf.Lerp(this.SHightAngle, this.EHightAngle, this.m_PlayTime / this.TimeLength);
        float angle = Mathf.Lerp(this.SRotAngle, this.ERotAngle, this.m_PlayTime / this.TimeLength);
        float d = Mathf.Lerp(this.Sdistance, this.Edistance, this.m_PlayTime / this.TimeLength);
        Vector3 currDis = this.TargetTran.forward * d;
        this.Apply(currDis, angle, angleV);
        if (!this.OnPlay && this.OnCameraMoveOver != null)
        {
            this.OnCameraMoveOver();
        }
    }

    public bool PlayCameraMove(string paramString)
    {
        if (this.ParseParamString(paramString))
        {
            this.OnPlay = true;
            this.m_PlayTime = 0f;
            Time.timeScale = this.TimeScale;
            return true;
        }
        return false;
    }

    private Transform TargetTran
    {
        get
        {
            if (this.BossTarget != null && this.BossTarget.ModelObj != null)
            {
                return this.BossTarget.ModelObj.transform;
            }
            return null;
        }
    }

    public bool ParseParamString(string paramString)
    {
        try
        {
            string[] array = paramString.Split(new char[]
            {
                '$'
            });
            this.SHightAngle = float.Parse(array[0]);
            this.SRotAngle = float.Parse(array[1]);
            this.Sdistance = float.Parse(array[2]);
            this.EHightAngle = float.Parse(array[3]);
            this.ERotAngle = float.Parse(array[4]);
            this.Edistance = float.Parse(array[5]);
            this.centerOffset.y = float.Parse(array[6]);
            this.TimeScale = float.Parse(array[7]);
            this.TimeLength = float.Parse(array[8]);
            this.UIdelay = float.Parse(array[9]);
            this.UIduration = float.Parse(array[10]);
            if (this.TimeScale <= 0.01f)
            {
                this.TimeScale = 0.01f;
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogWarning(this, "ParseParamString Error :" + arg);
            return false;
        }
        return true;
    }

    public void OnGUI()
    {
        MyEditorGUILayout.Button("返回主角摄像机", delegate
        {
            this.mCameraCtrl.ChangeState(new CameraFollowTarget4());
        }, 0f, 0f);
        MyEditorGUILayout.Button((!(this.TargetTran == null)) ? this.TargetTran.name : "none", delegate
        {
            this.SelectTarget = true;
        }, 0f, 0f);
        if (this.SelectTarget)
        {
            this.ShowList();
        }
        else
        {
            this.EditorAttribute();
        }
    }

    private void EditorAttribute()
    {
        if (this.TargetTran == null)
        {
            return;
        }
        this.SHightAngle = MyEditorGUILayout.FloatField("初始高度角", this.SHightAngle, 260f);
        this.SRotAngle = MyEditorGUILayout.FloatField("初始旋转角", this.SRotAngle, 260f);
        this.Sdistance = MyEditorGUILayout.FloatField("初始距离", this.Sdistance, 260f);
        this.EHightAngle = MyEditorGUILayout.FloatField("结束高度角", this.EHightAngle, 260f);
        this.ERotAngle = MyEditorGUILayout.FloatField("结束旋转角", this.ERotAngle, 260f);
        this.Edistance = MyEditorGUILayout.FloatField("结束距离", this.Edistance, 260f);
        this.centerOffset.y = MyEditorGUILayout.FloatField("目标中心偏离高度", this.centerOffset.y, 260f);
        this.TimeScale = MyEditorGUILayout.FloatField("时间缩放", this.TimeScale, 260f);
        this.TimeLength = MyEditorGUILayout.FloatField("时间长度", this.TimeLength, 260f);
        this.UIdelay = MyEditorGUILayout.FloatField("UI延迟", this.UIdelay, 260f);
        this.UIduration = MyEditorGUILayout.FloatField("UI持续时间", this.UIduration, 260f);
        this.ParamString = this.GenerateParamString();
        MyEditorGUILayout.StringField("参数", this.ParamString, 260f);
        MyEditorGUILayout.Button("播放", delegate
        {
            this.PlayCameraMove(this.ParamString);
        }, 0f, 0f);
    }

    public string GenerateParamString()
    {
        this.getPathResult.Length = 0;
        this.getPathResult.Append(this.SHightAngle + "$");
        this.getPathResult.Append(this.SRotAngle + "$");
        this.getPathResult.Append(this.Sdistance + "$");
        this.getPathResult.Append(this.EHightAngle + "$");
        this.getPathResult.Append(this.ERotAngle + "$");
        this.getPathResult.Append(this.Edistance + "$");
        this.getPathResult.Append(this.centerOffset.y + "$");
        this.getPathResult.Append(this.TimeScale + "$");
        this.getPathResult.Append(this.TimeLength + "$");
        this.getPathResult.Append(this.UIdelay + "$");
        this.getPathResult.Append(this.UIduration + string.Empty);
        return this.getPathResult.ToString();
    }

    private void ShowList()
    {
        this.Pos = GUILayout.BeginScrollView(this.Pos, new GUILayoutOption[0]);
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            MyEditorGUILayout.Button(pair.Key.ToString(), delegate
            {
                this.SetTarget(pair.Value);
                this.SelectTarget = false;
            }, 0f, 0f);
        });
        manager.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            MyEditorGUILayout.Button(pair.Key.ToString(), delegate
            {
                this.SetTarget(pair.Value);
                this.SelectTarget = false;
            }, 0f, 0f);
        });
    }

    public void OnExit(CameraStateMachine CameraCtrl)
    {
        Time.timeScale = 1f;
    }

    private CharactorBase BossTarget;

    private CameraController mCameraCtrl;

    public Action OnCameraMoveOver;

    private float SHightAngle = 30f;

    private float SRotAngle = 30f;

    private float Sdistance = 5f;

    private float EHightAngle = 45f;

    private float ERotAngle = 270f;

    private float Edistance = 8f;

    private Vector3 centerOffset = new Vector3(0f, 1f, 0f);

    private float TimeScale = 0.3f;

    private float TimeLength = 2.5f;

    private float UIdelay = 1.5f;

    private float UIduration = 1f;

    public bool OnPlay;

    private float m_PlayTime;

    private string ParamString = string.Empty;

    private StringBuilder getPathResult = new StringBuilder();

    private bool SelectTarget;

    private Vector2 Pos = Vector2.zero;
}
