using System;
using System.Collections.Generic;
using Engine;
using Framework.Managers;
using Game.Scene;
using Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : SingletonForMono<InputController>
{
    public int ReSetInputDir
    {
        set
        {
            if (this.CheckCanInput())
            {
                if (value != -1)
                {
                    MainPlayer.Self.MoveToByDir(value, false);
                }
                else
                {
                    MainPlayer.Self.SetInputDir(value);
                }
            }
            if (MainPlayer.Self != null)
            {
                MainPlayer.Self.OnMoveStateChange(this.CurrentInputType == InputType.InputNone);
                MainPlayer.Self.BreakAutoAttack();
            }
        }
    }

    public int InputDir
    {
        get
        {
            return this.inputDir;
        }
        set
        {
            if (value > 360)
            {
                value -= 360;
            }
            if (value != -1)
            {
                this.inputDir = value / 2;
            }
            else
            {
                this.inputDir = -1;
            }
            if (this.CheckCanInput())
            {
                if (this.inputDir != -1)
                {
                    MainPlayer.Self.MoveToByDir(this.inputDir, false);
                }
                else
                {
                    MainPlayer.Self.SetInputDir(this.inputDir);
                }
            }
            if (MainPlayer.Self != null)
            {
                MainPlayer.Self.OnMoveStateChange(this.CurrentInputType == InputType.InputNone);
                MainPlayer.Self.BreakAutoAttack();
            }
        }
    }

    public int FaceDir
    {
        get
        {
            return this.faceDir;
        }
        set
        {
            if (value > 360)
            {
                value -= 360;
            }
            if (value != -1)
            {
                this.faceDir = value / 2;
            }
            else
            {
                this.faceDir = -1;
            }
            if (this.CheckCanInput() && this.faceDir > -1)
            {
                MainPlayer.Self.FaceDir = (uint)this.faceDir;
            }
        }
    }

    public void Reset()
    {
        SingletonForMono<InputController>.Instance.mScreenEventController = new ScreenEventController();
        Cursor.visible = true;
    }

    public void Init()
    {
        this.mLineRenderer = base.gameObject.GetComponent<LineRenderer>();
        if (this.mLineRenderer == null)
        {
            this.mLineRenderer = base.gameObject.AddComponent<LineRenderer>();
            this.mLineRenderer.SetVertexCount(0);
        }
        this.mLineRenderer.SetWidth(0.05f, 0.05f);
        this.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.SetZoomCamera));
        this.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.SetCursorState));
        this.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.SetMouseQuickClick));
        this.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.SetRotateCamera));
    }

    private void SetRotateCamera(ScreenEvent SE)
    {
        if (CameraController.Self != null)
        {
            if (SE.mTpye == ScreenEvent.EventType.Slip)
            {
                CameraController.Self.RotateCamera(SE.SlipDis);
                if (SE.mFingerId == 1)
                {
                    CameraController.Self.RotateMainPlayer(SE.SlipDis, false);
                    this.ReSet();
                }
            }
            else if (SE.mTpye == ScreenEvent.EventType.Click && SE.mFingerId == 1)
            {
                CameraController.Self.RotateMainPlayer(SE.SlipDis, true);
            }
        }
    }

    private void SetCursorState(ScreenEvent SE)
    {
        if (SE.IsDragGraphic || !SE.IsMouseSlip)
        {
            return;
        }
        if (MouseStateControoler.Instan != null)
        {
            MouseStateControoler.Instan.SetCursorState(this.turnedToMouse, SE.CursorPos);
        }
    }

    private void SetMouseQuickClick(ScreenEvent SE)
    {
        if (SE.mTpye == ScreenEvent.EventType.QuickClick)
        {
            if (MainPlayerSkillHolder.Instance.IsSightingState())
            {
                return;
            }
            if (Camera.main == null)
            {
                return;
            }
            Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 300f, Const.LayerForMask.Terrian))
            {
                Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(raycastHit.point, true);
                if (!GraphUtils.PosIsInMap(serverPosByWorldPos, true))
                {
                    return;
                }
                if (MainPlayer.Self != null)
                {
                    MainPlayer.Self.BreakAutoAttack();
                }
                if (MainPlayer.Self != null && MainPlayer.Self.Pfc != null)
                {
                    MainPlayer.Self.Pfc.BeginFindPath(serverPosByWorldPos, PathFindComponent.AutoMoveState.MoveToPoint, null, null);
                }
            }
        }
    }

    private void SetZoomCamera(ScreenEvent SE)
    {
        if (SE.mTpye == ScreenEvent.EventType.Zoom && null != CameraController.Self)
        {
            CameraController.Self.ZoomCamera(SE.mfMouseScrollWheel);
        }
    }

    private bool CheckCanInput()
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        return manager != null && manager.MainPlayer != null && !manager.MainPlayer.HasBeControlledBy(BufferState.ControlType.ForceMove) && !manager.MainPlayer.HasBeControlledBy(BufferState.ControlType.ChaosMove) && manager.PlayerStateInCompetition == PlayerShowState.show;
    }

    private void Update()
    {
        this.ProcessKeyBoard();
        this.mScreenEventController.UpdateScreenEvent();
    }

    public void SetInputDir(int input)
    {
        if (UIManager.GetUIObject<UI_CompleteCopy>())
        {
            this.InputDir = -1;
        }
        else
        {
            this.InputDir = input;
        }
    }

    private void ProcessKeyBoard()
    {
        if (null != EventSystem.current.currentSelectedGameObject)
        {
            if (null == this.inputField)
            {
                this.inputField = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
            }
            if (null != this.inputField)
            {
                if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
                {
                    if (UIManager.GetUIObject<UI_GM>() != null)
                    {
                        UIManager.GetUIObject<UI_GM>().InputFieldActive(true);
                    }
                    else if (UIManager.GetUIObject<UI_FriendPrivateChat>() != null)
                    {
                        UIManager.GetUIObject<UI_FriendPrivateChat>().InputFieldActive(true);
                    }
                    else
                    {
                        ControllerManager.Instance.GetController<ChatControl>().InputFieldActive(true);
                    }
                }
                return;
            }
        }
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            if (UIBagManager.Instance.IsSplitPanelOpen())
            {
                UIBagManager.Instance.EnterSendSplitOk();
            }
            /*else if (UIManager.GetUIObject<UI_GM>() != null)
            {
                UIManager.GetUIObject<UI_GM>().InputFieldActive(false);
            }
            else */if (UIManager.GetUIObject<UI_FriendPrivateChat>() != null)
            {
                UIManager.GetUIObject<UI_FriendPrivateChat>().InputFieldActive(false);
            }
            else
            {
                ControllerManager.Instance.GetController<ChatControl>().InputFieldActive(false);
            }
        }
    }

    public void SetTurnedTo(bool isTurneTo, bool isTurnToMouse)
    {
        if (this.turnedTo && !isTurneTo && CameraController.Self != null)
        {
            this.angle = CameraController.Self.Angle;
        }
        this.turnedTo = isTurneTo;
        this.turnedToMouse = isTurnToMouse;
    }

    public void ReSet()
    {
        if (CameraController.Self != null)
        {
            this.angle = CameraController.Self.Angle;
        }
    }

    private void TestBtn()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        AutoAttack component = MainPlayer.Self.GetComponent<AutoAttack>();
        component.AutoAttackOn = !component.AutoAttackOn;
    }

    private void TestGui()
    {
        int num = 100;
        int num2 = 50;
        if (GUI.Button(new Rect((float)(Screen.width / 2), 5f, (float)num, (float)num2), "msg记录"))
        {
            FFDebug.LogWarning(this, LSingleton<NetWorkModule>.Instance.MainSocket.ClearAndGetMsgLog());
        }
        if (GUI.Button(new Rect((float)(Screen.width / 2 - num), 5f, (float)num, (float)num2), "TEST"))
        {
            this.TestBtn();
        }
        GUI.color = Color.green;
        float num3 = 0f;
        if (SingletonForMono<GameTime>.Instance.CheckPing)
        {
            GUI.Box(new Rect((float)(Screen.width - 120), num3 += 20f, 120f, 20f), "Ping : " + SingletonForMono<GameTime>.Instance.Ping);
        }
        Vector2 currentPosition2D = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.CurrentPosition2D;
        GUI.Box(new Rect((float)(Screen.width - 120), num3 += 20f, 120f, 20f), "Pos : " + GraphUtils.Keep2DecimalPlacesVector2(currentPosition2D).ToString());
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.OtherPlayerData.MapUserData != null)
        {
            GUI.Box(new Rect((float)(Screen.width - 120), num3 += 20f, 120f, 20f), "CharId： " + ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.OtherPlayerData.MapUserData.charid);
            GUI.Box(new Rect((float)(Screen.width - 120), num3 += 20f, 120f, 20f), "Speed ： " + ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.OtherPlayerData.MapUserData.mapdata.movespeed);
            GUI.Box(new Rect((float)(Screen.width - 120), num3 += 20f, 120f, 20f), "Dir   ： " + ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.ServerDir);
        }
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData != null)
        {
            GUI.Box(new Rect((float)(Screen.width - 120), num3 += 20f, 120f, 20f), "LineID： " + ManagerCenter.Instance.GetManager<GameScene>().CurrentLineID);
        }
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData != null)
        {
            GUI.Box(new Rect((float)(Screen.width - 220), num3 + 20f, 220f, 20f), "SceneID： " + ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.sceneID());
        }
        if (InputController.IsShowCharactorInfo)
        {
            this.CharactorInfo.Gui();
        }
        if (this.Autoskill)
        {
            MainPlayer.Self.TryReleaseSkill(1001U);
        }
        this.DrawLineBtn();
        this.DrawDebugLine();
    }

    private void DrawLineBtn()
    {
        int num = 0;
        float num2 = 150f;
        if (GUI.Button(new Rect(30f, num2 + (float)(num++ * 40), 100f, 30f), "攻击区域参数:" + this.showDrawLineBtn))
        {
            this.showDrawLineBtn = !this.showDrawLineBtn;
        }
        if (!this.showDrawLineBtn)
        {
            return;
        }
        this.AddfloatFild(30f, num2 + (float)(num++ * 40), "半径", ref this.radius);
        this.AddfloatFild(30f, num2 + (float)(num++ * 40), "扇形角度", ref this.AngleSize);
        this.AddfloatFild(30f, num2 + (float)(num++ * 40), "高", ref this.mHight);
        this.AddfloatFild(30f, num2 + (float)(num++ * 40), "宽", ref this.mWide);
        this.AddfloatFild(30f, num2 + (float)(num++ * 40), "朝向", ref this.AngleRot);
        if (GUI.Button(new Rect(30f, num2 + (float)(num++ * 40), 100f, 30f), "设置矩形"))
        {
            this.SetRectangle(this.mHight, this.mWide, this.AngleRot);
        }
        if (GUI.Button(new Rect(30f, num2 + (float)(num++ * 40), 100f, 30f), "设置扇形"))
        {
            this.SetSector(this.radius, this.AngleSize, this.AngleRot);
        }
        if (GUI.Button(new Rect(30f, num2 + (float)(num++ * 40), 100f, 30f), "设置圆"))
        {
            this.SetCircle(this.radius);
        }
    }

    private void AddfloatFild(float X, float Y, string Title, ref float num)
    {
        GUI.Label(new Rect(X, Y, 80f, 25f), Title + "(float)");
        string s = GUI.TextField(new Rect(80f + X, Y, 140f, 25f), num.ToString());
        float num2;
        if (float.TryParse(s, out num2))
        {
            num = num2;
        }
    }

    private void SetCircle(float radius)
    {
        this.v3list.Clear();
        for (float num = 0f; num < 6.28318548f; num += 0.1f)
        {
            float x = radius * (float)Math.Cos((double)num);
            float z = radius * (float)Math.Sin((double)num);
            this.v3list.Add(new Vector3(x, this.LineHight, z));
        }
    }

    private void SetSector(float radius, float AngleSize, float AngleRot)
    {
        this.v3list.Clear();
        float num = 3.14159274f * AngleSize / 180f;
        this.v3list.Add(new Vector3(0f, this.LineHight, 0f));
        Quaternion rotation = Quaternion.AngleAxis(AngleRot - (90f - AngleSize / 2f), Vector3.up);
        for (float num2 = 0f; num2 < num; num2 += 0.1f)
        {
            float x = radius * (float)Math.Cos((double)num2);
            float z = radius * (float)Math.Sin((double)num2);
            this.v3list.Add(rotation * new Vector3(x, this.LineHight, z));
        }
    }

    private void SetRectangle(float H, float W, float A)
    {
        Quaternion rotation = Quaternion.AngleAxis(A, Vector3.up);
        this.v3list.Clear();
        this.v3list.Add(rotation * new Vector3(0f, this.LineHight, 0f));
        this.v3list.Add(rotation * new Vector3(W / 2f, this.LineHight, 0f));
        this.v3list.Add(rotation * new Vector3(W / 2f, this.LineHight, H));
        this.v3list.Add(rotation * new Vector3(-W / 2f, this.LineHight, H));
        this.v3list.Add(rotation * new Vector3(-W / 2f, this.LineHight, 0f));
    }

    private void DrawDebugLine()
    {
        if (this.v3list.Count == 0)
        {
            this.mLineRenderer.SetVertexCount(0);
            return;
        }
        this.mLineRenderer.SetVertexCount(this.v3list.Count + 1);
        for (int i = 0; i < this.v3list.Count; i++)
        {
            this.mLineRenderer.SetPosition(i, MainPlayer.Self.ModelObj.transform.localToWorldMatrix.MultiplyPoint(this.v3list[i]));
        }
        this.mLineRenderer.SetPosition(this.v3list.Count, MainPlayer.Self.ModelObj.transform.localToWorldMatrix.MultiplyPoint(this.v3list[0]));
    }

    public InputType CurrentInputType = InputType.InputNone;

    public bool turnedTo = true;

    public bool turnedToMouse = true;

    public float angle;

    private int inputDir = -1;

    private bool onMoveing;

    private int faceDir = -1;

    private LineRenderer mLineRenderer;

    public ScreenEventController mScreenEventController = new ScreenEventController();

    public Action<Vector2> OnVirtualDrag;

    private InputField inputField;

    private bool Autoskill;

    private CharactorInfoShow CharactorInfo = new CharactorInfoShow();

    public static bool IsShowCharactorInfo;

    private UITextureAsset asset;

    public static bool ShowTestBtn;

    private float radius = 5f;

    private float AngleSize = 90f;

    private float mHight = 5f;

    private float mWide = 5f;

    private float AngleRot;

    private bool showDrawLineBtn;

    private float LineHight = 0.1f;

    private List<Vector3> v3list = new List<Vector3>();
}
