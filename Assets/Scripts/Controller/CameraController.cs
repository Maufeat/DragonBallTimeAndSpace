using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using UnityEngine;
using UnityStandardAssets.CinematicEffects;
using UnityStandardAssets.ImageEffects;

public class CameraController : CameraStateMachine
{
    public Vector2 CameraForward
    {
        get
        {
            return this._cameraForward;
        }
    }

    public static Shader EdgeShader
    {
        get
        {
            if (CameraController._edgeShader == null)
            {
                CameraController._edgeShader = Shader.Find("Hidden/EdgeDetect");
            }
            return CameraController._edgeShader;
        }
    }

    private void Start()
    {
        CameraController.Self = this;
        base.transform.GetComponent<Rigidbody>().isKinematic = true;
        if (!this.CheckNeedInMonitorState())
        {
            if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowTarget2D)
            {
                this.EnterState(new CameraFollowTarget2D());
            }
            else if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowTarget4)
            {
                this.EnterState(new CameraFollowTarget4());
            }
            else if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowPrepare)
            {
                this.EnterState(new CameraFollowTargetPrepare());
            }
            else if (GameSystemSettings.GetCurrentCameraState() == CameraState.CameraFollowTargetMonitor)
            {
                this.EnterState(new CameraFollowTargetMonitor());
            }
        }
        else
        {
            this.EnterState(new CameraFollowTargetMonitor());
        }
        this.nOriScreenWidth = Screen.width;
        this.nOriScreenHeight = Screen.height;
        if (null != Camera.main)
        {
            this.camMain = Camera.main;
            UnityStandardAssets.CinematicEffects.DepthOfField component = this.camMain.GetComponent<UnityStandardAssets.CinematicEffects.DepthOfField>();
            if (null != component)
            {
                component.focusTransform = this.Target;
            }
            this.eOriClearFlags = this.camMain.clearFlags;
            this.nOriCullingMask = (~(1 << Const.Layer.UI | 1 << Const.Layer.RT) & this.camMain.cullingMask);
            this.colorOri = this.camMain.backgroundColor;
            this.camMain.depthTextureMode |= DepthTextureMode.Depth;
            LimitPointLightCount component2 = this.camMain.gameObject.GetComponent<LimitPointLightCount>();
            if (component2 == null)
            {
                this.camMain.gameObject.AddComponent<LimitPointLightCount>();
            }
            LimitEffectDistance component3 = this.camMain.gameObject.GetComponent<LimitEffectDistance>();
            if (component3 == null)
            {
                this.camMain.gameObject.AddComponent<LimitEffectDistance>();
            }
        }
        this.SetDOFTarget();
        if (MainPlayer.Self != null && MainPlayer.Self.IsSoul)
        {
            this.ShowGrayscale(true);
        }
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.CameraUpdate));
    }

    public bool CheckNeedInMonitorState()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        return manager != null && null != manager.sceneData.cameraZoneInfo;
    }

    private void SetDOFTarget()
    {
        if (null == this.camMain)
        {
            return;
        }
        FxPro component = this.camMain.GetComponent<FxPro>();
        if (null != component)
        {
            component.DOFEnabled = true;
            if (component.DOFEnabled && component.DOFParams != null)
            {
                component.DOFParams.AutoFocus = false;
                component.DOFParams.Target = this.Target;
                component.DOFParams.EffectCamera = base.GetComponent<Camera>();
            }
        }
        UnityStandardAssets.CinematicEffects.DepthOfField component2 = this.camMain.GetComponent<UnityStandardAssets.CinematicEffects.DepthOfField>();
        if (null != component2)
        {
            UnityEngine.Object.DestroyImmediate(component2);
        }
    }

    public void EnterPrepareState()
    {
        this.EnterState(new CameraFollowTargetPrepare());
    }

    public void RestoreCamera()
    {
        if (this._bcamera3D)
        {
            this.EnterState(new CameraFollowTarget4());
            CameraFollowTarget4 cameraFollowTarget = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget.ResetCamera();
        }
        else
        {
            this.EnterState(new CameraFollowTarget2D());
            CameraFollowTarget2D cameraFollowTarget2D = base.CurrState as CameraFollowTarget2D;
            cameraFollowTarget2D.ResetCamera();
        }
    }

    public void SetTarget(Transform tran)
    {
        this.Target = tran;
        this.SetDOFTarget();
        if (base.CurrState is CameraFollowTarget2D)
        {
            CameraFollowTarget2D cameraFollowTarget2D = base.CurrState as CameraFollowTarget2D;
            cameraFollowTarget2D.setTarget(this.Target);
        }
        else if (base.CurrState is CameraFollowTarget4)
        {
            CameraFollowTarget4 cameraFollowTarget = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget.setTarget(this.Target);
        }
        else if (base.CurrState is CameraFollowTargetPrepare)
        {
            CameraFollowTargetPrepare cameraFollowTargetPrepare = base.CurrState as CameraFollowTargetPrepare;
            cameraFollowTargetPrepare.setTarget(this.Target);
        }
        else if (base.CurrState is CameraFollowTargetMonitor)
        {
            CameraFollowTargetMonitor cameraFollowTargetMonitor = base.CurrState as CameraFollowTargetMonitor;
            cameraFollowTargetMonitor.SetTarget(this.Target);
        }
    }

    public override void CameraUpdate()
    {
        if (this.Target == null)
        {
            return;
        }
        if (MainPlayer.Self != null)
        {
            PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
            if (component != null)
            {
                this.InBattle = component.ContainsState(UserState.USTATE_BATTLE);
            }
        }
        Vector2 vector = new Vector2(base.transform.forward.x, base.transform.forward.z);
        this._cameraForward = vector.normalized;
        this.UpdateAngle();
        base.CameraUpdate();
        this.Rotatediff = Vector3.zero;
        this._bLastHasInput = this._hasInput;
        this._hasInput = false;
        this.mfMouseScrollWheel = 0f;
        if (null != this.camHelp && null != this.camMain)
        {
            this.camHelp.transform.position = this.camMain.transform.position;
            this.camHelp.transform.rotation = this.camMain.transform.rotation;
        }
        if (this.OpenWallCollider && base.CurrState != null)
        {
            bool flag = true;
            float maxDistance = Vector3.Distance(base.transform.position, this.Target.position);
            int num = Physics.RaycastNonAlloc(base.CurrState.TargetPos(), base.transform.position - base.CurrState.TargetPos(), this.hits, maxDistance, Const.LayerForMask.Terrian | Const.LayerForMask.Wall | Const.LayerForMask.Fence);
            this.allhits.Clear();
            for (int i = 0; i < num; i++)
            {
                this.allhits.Add(this.hits[i]);
            }
            this.allhits.Sort(delegate (RaycastHit a, RaycastHit b)
            {
                if (a.distance < b.distance)
                {
                    return -1;
                }
                if (a.distance > b.distance)
                {
                    return 1;
                }
                return 0;
            });
            RaycastHit raycastHit;
            for (int j = num - 1; j >= 0; j--)
            {
                raycastHit = this.allhits[j];
                if (raycastHit.collider.gameObject.layer == Const.LayerForMask.FenceValue || raycastHit.collider.bounds.Contains(base.transform.position))
                {
                    if (j > 0)
                    {
                        Vector3 point = raycastHit.point;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (!this.allhits[k].collider.bounds.Contains(point))
                            {
                                point = this.allhits[k].point;
                                break;
                            }
                            point.x = this.allhits[k].point.x;
                            point.y = this.allhits[k].point.y;
                            point.z = this.allhits[k].point.z;
                        }
                        Vector3 normalized = (-point + base.CurrState.TargetPos()).normalized;
                        base.transform.position = point + normalized;
                        flag = false;
                    }
                    else
                    {
                        Vector3 normalized2 = (-raycastHit.point + base.CurrState.TargetPos()).normalized;
                        base.transform.position = raycastHit.point + normalized2;
                        flag = false;
                    }
                    break;
                }
            }
            if (flag && this.bOnCameraCollider)
            {
                this.AddCameraDistance(-0.1f);
            }
            bool flag2 = Physics.Raycast(new Ray(base.transform.position, (base.CurrState.TopPos() - base.transform.position).normalized), out raycastHit, maxDistance, Const.LayerForMask.Wall);
            if (flag2)
            {
                flag2 = Physics.Raycast(new Ray(base.transform.position, (base.CurrState.FeetPos() - base.transform.position).normalized), out raycastHit, maxDistance, Const.LayerForMask.Wall);
            }
            if (MainPlayer.Self != null && MainPlayer.Self.HightLightControl != null)
            {
                MainPlayer.Self.HightLightControl.OnCull(flag2);
            }
        }
    }

    private UIFllowTarget GetPlayerHpUIFllowTarget()
    {
        if (this.uiFllowTarget == null)
        {
            MainPlayer mainPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer;
            if (mainPlayer == null || mainPlayer.hpdata == null)
            {
                return null;
            }
            GameObject objParent = mainPlayer.hpdata.objParent;
            this.uiFllowTarget = objParent.GetComponent<UIFllowTarget>();
        }
        return this.uiFllowTarget;
    }

    private void OnCollisionEnter(Collision in_other)
    {
        this.bOnCameraCollider = (in_other.collider.gameObject.layer == Const.Layer.Wall || in_other.collider.gameObject.layer == Const.Layer.Terrian);
    }

    private void OnCollisionExit(Collision in_other)
    {
        this.bOnCameraCollider = false;
    }

    public bool HasInput
    {
        get
        {
            return this._hasInput;
        }
    }

    public void RotateCamera(Vector3 input)
    {
        if (base.CurrState is CameraFollowTargetMonitor && !this._bLastHasInput)
        {
            TipsWindow.ShowWindow(TipsType.CANT_ADJUST_CAMERA, null);
        }
        this.Rotatediff = input;
        this._hasInput = true;
        this.Rotatediff.x = this.Rotatediff.x * (1334f / (float)Screen.width);
        this.Rotatediff.y = this.Rotatediff.y * (750f / (float)Screen.height);
    }

    public void RotateMainPlayer(Vector3 input, bool now)
    {
        if (MainPlayer.Self.IsCurFrameMoved || MainPlayer.Self.inJumpState())
        {
            return;
        }
        PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
        if (component != null && !MainPlayer.Self.IsCanMove())
        {
            return;
        }
        if (MainPlayer.Self.InCastSkillState())
        {
            return;
        }
        uint num = (uint)(base.transform.rotation.eulerAngles.y / 2f);
        num %= 180U;
        if (Mathf.Abs(MainPlayer.Self.ServerDir - num) < 1f)
        {
            return;
        }
        MainPlayer.Self.ServerDir = num;
        Vector2 currentPosition2D = MainPlayer.Self.CurrentPosition2D;
        currentPosition2D.x = GraphUtils.Keep2DecimalPlaces(currentPosition2D.x);
        currentPosition2D.y = GraphUtils.Keep2DecimalPlaces(currentPosition2D.y);
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.dir = num;
        cs_MoveData.pos = default(cs_FloatMovePos);
        cs_MoveData.pos.fx = currentPosition2D.x;
        cs_MoveData.pos.fy = currentPosition2D.y;
        MainPlayer.Self.MoveDir(cs_MoveData);
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqMove(cs_MoveData, false, now);
    }

    public void ZoomCamera(float fMouseScrollWheel)
    {
        if (base.CurrState is CameraFollowTargetMonitor)
        {
            if (!this._bLastHasInput)
            {
                TipsWindow.ShowWindow(TipsType.CANT_ADJUST_CAMERA, null);
            }
            this._hasInput = true;
            return;
        }
        this.mfMouseScrollWheel = fMouseScrollWheel;
    }

    public void AglinWithTarget()
    {
        if (base.CurrState is CameraFollowTarget2)
        {
            CameraFollowTarget2 cameraFollowTarget = base.CurrState as CameraFollowTarget2;
            cameraFollowTarget.AglinDelay();
        }
    }

    public void SetCameraTo(Vector3 pos)
    {
        if (base.CurrState is CameraFollowTarget2)
        {
            base.transform.position = pos;
            CameraFollowTarget2 cameraFollowTarget = base.CurrState as CameraFollowTarget2;
            cameraFollowTarget.ResetCameraPos();
        }
        else if (base.CurrState is CameraFollowTarget4)
        {
            base.transform.position = pos;
            CameraFollowTarget4 cameraFollowTarget2 = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget2.ResetCameraPos();
        }
    }

    public void AddCameraDistance(float distance)
    {
        if (base.CurrState is CameraFollowTarget2)
        {
            CameraFollowTarget2 cameraFollowTarget = base.CurrState as CameraFollowTarget2;
            cameraFollowTarget.AddTargetDistance(distance);
        }
        else if (base.CurrState is CameraFollowTarget4)
        {
            CameraFollowTarget4 cameraFollowTarget2 = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget2.AddTargetDistance(distance);
        }
    }

    public void ChangeCameraState(bool bcamera3D)
    {
        if (!this.CheckNeedInMonitorState())
        {
            if (bcamera3D)
            {
                this.ChangeState(new CameraFollowTarget4());
            }
            else
            {
                this.ChangeState(new CameraFollowTarget2D());
            }
        }
    }

    public void Reste3DCamera()
    {
        if (MainPlayer.Self != null)
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            component.SetTargetNull();
        }
        if (base.CurrState is CameraFollowTarget4)
        {
            CameraFollowTarget4 cameraFollowTarget = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget.ResetCamera();
        }
        else if (base.CurrState is CameraFollowTarget2D)
        {
            CameraFollowTarget2D cameraFollowTarget2D = base.CurrState as CameraFollowTarget2D;
            cameraFollowTarget2D.ResetCamera();
        }
    }

    public void setCameraDistance()
    {
        if (base.CurrState is CameraFollowTarget2D)
        {
            CameraFollowTarget2D cameraFollowTarget2D = base.CurrState as CameraFollowTarget2D;
            cameraFollowTarget2D.SetDistence();
        }
    }

    public void setCameraSpeed()
    {
        if (base.CurrState is CameraFollowTarget4)
        {
            CameraFollowTarget4 cameraFollowTarget = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget.setFocuseSpeed();
        }
    }

    private void UpdateAngle()
    {
        if (base.CurrState is CameraFollowTargetMonitor)
        {
            CameraFollowTargetMonitor cameraFollowTargetMonitor = base.CurrState as CameraFollowTargetMonitor;
            if (!cameraFollowTargetMonitor.bUpdateCameraAngle)
            {
                return;
            }
        }
        this.Angle = Vector3.Angle(Vector2.up, this.CameraForward);
        if (this.CameraForward.x < 0f)
        {
            this.Angle = 360f - this.Angle;
        }
    }

    private void DebugDrawStuff()
    {
        Debug.DrawLine(base.transform.position, base.transform.position + new Vector3(this._cameraForward.x, 0f, this._cameraForward.y) * 10f);
    }

    private void DrawEditorView()
    {
        Rect rect = this.ConsoleRect;
        if (!this.IsShow)
        {
            rect.height = 20f;
        }
        rect = GUI.Window(99, rect, delegate (int id)
        {
            GUI.DragWindow(new Rect(0f, 0f, this.ConsoleRect.width - 30f, 20f));
            string text = (!this.IsShow) ? "╉" : "▂";
            if (GUI.Button(new Rect(this.ConsoleRect.width - 30f, 0f, 30f, 20f), text))
            {
                this.IsShow = !this.IsShow;
            }
        }, "相机测试");
        if (this.IsShow)
        {
            this.ConsoleRect = rect;
        }
        else
        {
            this.ConsoleRect.x = rect.x;
            this.ConsoleRect.y = rect.y;
        }
    }

    public void RemoveSelf()
    {
        if (CameraController.Self != null)
        {
            UnityEngine.Object.Destroy(CameraController.Self);
            CameraController.Self = null;
        }
    }

    private void OnPreRender()
    {
        if (this.bUseHelpCamera && null != this.renTex)
        {
            Graphics.Blit(this.renTex, (RenderTexture)null);
        }
    }

    public void SetResolution(int nLevel)
    {
        int num;
        if (0 >= nLevel)
        {
            if (1080 <= Screen.height)
            {
                num = 540;
            }
            else
            {
                num = 360;
            }
        }
        else if (nLevel == 1)
        {
            if (1080 <= Screen.height)
            {
                num = 720;
            }
            else
            {
                num = 480;
            }
        }
        else
        {
            num = Screen.height;
        }
        if (num < 0)
        {
            num = this.nOriScreenHeight;
        }
        else if (num > this.nOriScreenHeight)
        {
            num = this.nOriScreenHeight;
        }
        if (num == this.nCurrentScreenHeight)
        {
            return;
        }
        this.ClearRenderTarget();
        this.nCurrentScreenWidth = num * this.nOriScreenWidth / this.nOriScreenHeight;
        this.nCurrentScreenHeight = num;
        if (num == this.nOriScreenHeight)
        {
            this.bUseHelpCamera = false;
            this.EnableMainCamera();
        }
        else
        {
            this.bUseHelpCamera = true;
            this.CreateHelpCamera();
            this.CreateRenderTarget();
            this.DisableMainCamera();
        }
    }

    private void CreateRenderTarget()
    {
        if (null != this.camHelp)
        {
            RenderTexture targetTexture = new RenderTexture(this.nCurrentScreenWidth, this.nCurrentScreenHeight, 16, RenderTextureFormat.Default);
            this.camHelp.targetTexture = targetTexture;
            this.renTex = targetTexture;
        }
    }

    private void ClearRenderTarget()
    {
        if (null != this.renTex)
        {
            UnityEngine.Object.Destroy(this.renTex);
            this.renTex = null;
            this.camHelp.targetTexture = null;
        }
    }

    private void CreateHelpCamera()
    {
        if (null != this.camHelp || null == this.camMain)
        {
            return;
        }
        GameObject gameObject = new GameObject("HelpCamera");
        gameObject.transform.SetParent(this.camMain.transform.parent);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        Camera camera = gameObject.AddComponent<Camera>();
        camera.CopyFrom(this.camMain);
        this.camHelp = camera;
    }

    public void ShowGrayscale(bool show)
    {
        if (null == this.camMain)
        {
            return;
        }
        Grayscale grayscale = this.camMain.gameObject.GetComponent<Grayscale>();
        if (null == grayscale)
        {
            grayscale = this.camMain.gameObject.AddComponent<Grayscale>();
        }
        grayscale.ResetShader();
        grayscale.enabled = show;
        if (MainPlayer.Self == null)
        {
            return;
        }
        Renderer componentInChildren = MainPlayer.Self.ModelObj.GetComponentInChildren<SkinnedMeshRenderer>();
        if (componentInChildren == null)
        {
            componentInChildren = MainPlayer.Self.ModelObj.GetComponentInChildren<MeshRenderer>();
        }
        componentInChildren.material.SetFloat("_Alpha", (!show) ? 1f : 0.3f);
    }

    private void EnableMainCamera()
    {
        if (null != this.camMain)
        {
            if (this.bActiveSceneCamera)
            {
                this.camMain.clearFlags = this.eOriClearFlags;
                this.camMain.cullingMask = this.nOriCullingMask;
                this.camMain.cullingMask = ~(1 << Const.Layer.Cutscene);
            }
            else
            {
                this.camMain.clearFlags = CameraClearFlags.Color;
                this.camMain.backgroundColor = new Color(0f, 0f, 0f);
                this.camMain.cullingMask = 0;
            }
        }
        if (null != this.camHelp)
        {
            UnityEngine.Object.Destroy(this.camHelp.gameObject);
            this.camHelp = null;
        }
    }

    private void DisableMainCamera()
    {
        if (null != this.camMain)
        {
            this.camMain.clearFlags = CameraClearFlags.Nothing;
            this.camMain.cullingMask = 0;
        }
    }

    public void OpenUIActiveSceneCamera(bool bActive)
    {
        this.bActiveSceneCamera = bActive;
        if (!this.bUseHelpCamera)
        {
            if (this.camMain != null)
            {
                if (bActive)
                {
                    this.camMain.clearFlags = this.eOriClearFlags;
                    this.camMain.cullingMask = this.nOriCullingMask;
                    this.camMain.backgroundColor = this.colorOri;
                }
                else
                {
                    this.camMain.clearFlags = CameraClearFlags.Color;
                    this.camMain.backgroundColor = new Color(0f, 0f, 0f);
                    this.camMain.cullingMask = 0;
                }
            }
        }
        else if (this.camHelp != null)
        {
            if (bActive)
            {
                this.camHelp.clearFlags = this.eOriClearFlags;
                this.camHelp.cullingMask = this.nOriCullingMask;
                this.camHelp.backgroundColor = this.colorOri;
            }
            else
            {
                this.camHelp.clearFlags = CameraClearFlags.Color;
                this.camHelp.backgroundColor = new Color(0f, 0f, 0f);
                this.camHelp.cullingMask = 0;
            }
        }
    }

    public void SetMaxDistance(float distance)
    {
        if (base.CurrState is CameraFollowTarget2)
        {
            CameraFollowTarget2 cameraFollowTarget = base.CurrState as CameraFollowTarget2;
            cameraFollowTarget.SetMaxDistance(distance);
        }
        if (base.CurrState is CameraFollowTarget4)
        {
            CameraFollowTarget4 cameraFollowTarget2 = base.CurrState as CameraFollowTarget4;
            cameraFollowTarget2.SetMaxDistance(distance);
        }
        if (base.CurrState is CameraFollowTarget2D)
        {
            CameraFollowTarget2D cameraFollowTarget2D = base.CurrState as CameraFollowTarget2D;
            cameraFollowTarget2D.SetMaxDistance(distance);
        }
    }

    public Transform Target;

    public static CameraController Self;

    public static bool IsRelive;

    public static bool IsReliveOrg;

    public int LastSceneID = -1;

    private Vector2 _cameraForward;

    public float LookAngle = float.NaN;

    public Vector3 Rotatediff = Vector3.zero;

    public bool _bcamera3D = true;

    public float mfMouseScrollWheel;

    private static Shader _edgeShader;

    private RaycastHit[] hits = new RaycastHit[20];

    private List<RaycastHit> allhits = new List<RaycastHit>();

    public bool InBattle;

    public bool OpenWallCollider = true;

    public float OffSetDistance;

    private UIFllowTarget uiFllowTarget;

    private bool bOnCameraCollider;

    private bool _hasInput;

    private bool _bLastHasInput;

    public float Angle;

    public Rect recScreen;

    public bool EditorWinOpen;

    private Rect ConsoleRect = new Rect(500f, 40f, 400f, 550f);

    private bool IsShow = true;

    private Camera camHelp;

    private Camera camMain;

    private bool bUseHelpCamera;

    private CameraClearFlags eOriClearFlags;

    private int nOriCullingMask;

    private Color colorOri;

    private int nOriScreenWidth;

    private int nOriScreenHeight;

    private int nCurrentScreenWidth;

    private int nCurrentScreenHeight;

    private RenderTexture renTex;

    private bool bActiveSceneCamera = true;
}
