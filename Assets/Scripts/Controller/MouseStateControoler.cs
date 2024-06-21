using System;
using System.Collections.Generic;
using Engine;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class MouseStateControoler : MonoBehaviour
{
    private void Start()
    {
        MouseStateControoler.Instan = this;
        this.m_default = Resources.Load<Texture2D>("Mouse/default");
        this.m_atks.Add(Resources.Load<Texture2D>("Mouse/atk"));
        this.m_dialog.Add(Resources.Load<Texture2D>("Mouse/dialog"));
        this.m_collectionEnable.Add(Resources.Load<Texture2D>("Mouse/collection"));
        this.m_collectionDisable.Add(Resources.Load<Texture2D>("Mouse/default"));
        this.m_itemsplit.Add(Resources.Load<Texture2D>("Mouse/itemsplit"));
        this.m_safelock.Add(Resources.Load<Texture2D>("Mouse/safelock"));
        this.m_pick.Add(Resources.Load<Texture2D>("Mouse/pick"));
        this.m_hover.Add(Resources.Load<Texture2D>("Mouse/hover"));
        this.m_scale3.Add(Resources.Load<Texture2D>("Mouse/scale3"));
        this.m_scale_topleft_bottomright.Add(Resources.Load<Texture2D>("Mouse/scale4"));
        this.m_itemsell.Add(Resources.Load<Texture2D>("Mouse/itemsplit"));
        this.m_itemrepair.Add(Resources.Load<Texture2D>("Mouse/itemsplit"));
        this.SetMoseState(MoseState.m_default);
        base.gameObject.AddComponent<ScreenCapture>();
    }

    public MoseState GetCurMoseState()
    {
        return this.m_curState;
    }

    private void Update()
    {
        if (this.m_curState != MoseState.m_default)
        {
            Cursor.SetCursor(this.m_cur[this.m_curIdx], this.GetCursorOffect(), CursorMode.Auto);
            if (this.m_fdelta > this.m_fsep)
            {
                this.m_fdelta = 0f;
                this.m_curIdx++;
                if (this.m_curIdx == this.m_cur.Count)
                {
                    this.m_curIdx = 0;
                }
            }
            this.m_fdelta += Time.deltaTime;
        }
        if (this.CheckMouseState() && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            GameObject gameObject = ManagerCenter.Instance.GetManager<EscManager>().OnePointColliderObject();
            if (gameObject == null || gameObject.GetComponentInParent<DragDropButton>() == null)
            {
                this.ResetHover(true);
            }
        }
        if (this.CheckHoverState())
        {
            return;
        }
        this.CheckManualSelectCharactor();
    }

    public void SetCursorState(bool isTurn, Vector2 point)
    {
        Cursor.visible = !isTurn;
    }

    public void SetMoseState(MoseState _ms)
    {
        this.m_curState = _ms;
        if (_ms == MoseState.m_atk)
        {
            this.m_cur = this.m_atks;
        }
        else if (_ms == MoseState.m_dialog)
        {
            this.m_cur = this.m_dialog;
        }
        else if (_ms == MoseState.m_colletionenable)
        {
            this.m_cur = this.m_collectionEnable;
        }
        else if (_ms == MoseState.m_colletiondisable)
        {
            this.m_cur = this.m_collectionDisable;
        }
        else if (_ms == MoseState.m_itemsplit)
        {
            this.m_cur = this.m_itemsplit;
        }
        else if (_ms == MoseState.m_safelock)
        {
            this.m_cur = this.m_safelock;
        }
        else if (_ms == MoseState.m_pick)
        {
            this.m_cur = this.m_pick;
        }
        else if (_ms == MoseState.m_hover)
        {
            this.m_cur = this.m_hover;
        }
        else if (_ms == MoseState.m_scale3)
        {
            this.m_cur = this.m_scale3;
        }
        else if (_ms == MoseState.m_scale_topleft_bottomright)
        {
            this.m_cur = this.m_scale_topleft_bottomright;
        }
        else if (_ms == MoseState.m_itemsell)
        {
            this.m_cur = this.m_itemsell;
        }
        else if (_ms == MoseState.m_itemrepair)
        {
            this.m_cur = this.m_itemrepair;
        }
        else if (_ms == MoseState.m_default)
        {
            Cursor.SetCursor(this.m_default, this.GetCursorOffect(), CursorMode.Auto);
        }
    }

    public bool IsContinuedMouseState()
    {
        return this.m_curState == MoseState.m_itemsplit || this.m_curState == MoseState.m_safelock || this.m_curState == MoseState.m_scale3 || this.m_curState == MoseState.m_scale_topleft_bottomright || this.m_curState == MoseState.m_hover || this.m_curState == MoseState.m_itemsell || this.m_curState == MoseState.m_itemrepair;
    }

    public Vector2 GetCursorOffect()
    {
        Vector2 zero = Vector2.zero;
        switch (this.m_curState)
        {
            case MoseState.m_default:
                zero = new Vector2(6f, 4f);
                break;
            case MoseState.m_atk:
                zero = new Vector2(6f, 5f);
                break;
            case MoseState.m_dialog:
                zero = new Vector2(15f, 11f);
                break;
            case MoseState.m_colletionenable:
                zero = new Vector2(15f, 16f);
                break;
            case MoseState.m_colletiondisable:
                zero = new Vector2(7f, 5f);
                break;
            case MoseState.m_safelock:
                zero = new Vector2(4f, 4f);
                break;
            case MoseState.m_itemsplit:
                zero = new Vector2(3f, 3f);
                break;
            case MoseState.m_pick:
                zero = new Vector2(15f, 16f);
                break;
            case MoseState.m_hover:
                zero = new Vector2(6f, 4f);
                break;
            case MoseState.m_scale3:
                zero = new Vector2(4f, 21f);
                break;
            case MoseState.m_scale_topleft_bottomright:
                zero = new Vector2(21f, 21f);
                break;
            case MoseState.m_itemsell:
                zero = new Vector2(3f, 3f);
                break;
            case MoseState.m_itemrepair:
                zero = new Vector2(3f, 3f);
                break;
        }
        return zero;
    }

    public bool CheckMouseState()
    {
        return this.m_curState == MoseState.m_itemsplit || this.m_curState == MoseState.m_safelock || this.m_curState == MoseState.m_hover || this.m_curState == MoseState.m_itemsell || this.m_curState == MoseState.m_itemrepair;
    }

    private void CheckManualSelectCharactor()
    {
        if (this.IsContinuedMouseState())
        {
            return;
        }
        if (null == Camera.main)
        {
            return;
        }
        if (!UITools.IsPointOverGraphic(UITools.PointLayer.Non_Mask))
        {
            Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
            int num = Physics.RaycastNonAlloc(ray, this.hits, 300f, Const.LayerForMask.MainPlayer | Const.LayerForMask.OtherPlayer | Const.LayerForMask.NpcShadow | Const.LayerForMask.NpcNoShadow | Const.LayerForMask.Charactor);
            if (num > 0)
            {
                int i = 0;
                while (i < num)
                {
                    RaycastHit raycastHit = this.hits[i];
                    CharactorBase charactorFromGameObject = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorFromGameObject(raycastHit.collider.gameObject);
                    if (charactorFromGameObject is MainPlayer)
                    {
                        i++;
                    }
                    else if (charactorFromGameObject is Npc)
                    {
                        bool flag = true;
                        if (charactorFromGameObject is Npc_TaskCollect)
                        {
                            flag = (charactorFromGameObject as Npc_TaskCollect).CheckStateContainDoing();
                        }
                        if (flag && i == 0)
                        {
                            this.Highlighting(charactorFromGameObject);
                        }
                        Npc npc = charactorFromGameObject as Npc;
                        if (ControllerManager.Instance.GetController<DuoQiController>().CheckIsMyBallAndInBase(npc.NpcData.MapNpcData.tempid))
                        {
                            return;
                        }
                        this.config = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                        if (this.config == null)
                        {
                            return;
                        }
                        if (this.config.GetCacheField_Bool("not_beselect"))
                        {
                            return;
                        }
                        uint field_Uint = this.config.GetField_Uint("kind");
                        if (field_Uint == 1U)
                        {
                            this.SetMoseState(MoseState.m_dialog);
                        }
                        else if (field_Uint == 2U || field_Uint == 3U || field_Uint == 4U || field_Uint == 5U || field_Uint == 22U)
                        {
                            this.SetMoseState(MoseState.m_atk);
                        }
                        else if (field_Uint == 6U || field_Uint == 8U || field_Uint == 11U)
                        {
                            if (flag)
                            {
                                this.SetMoseState(MoseState.m_colletionenable);
                            }
                            else
                            {
                                this.SetMoseState(MoseState.m_colletiondisable);
                            }
                        }
                        else if (field_Uint == 20U || field_Uint == 24U)
                        {
                            this.SetMoseState(MoseState.m_pick);
                        }
                        return;
                    }
                    else
                    {
                        if (charactorFromGameObject is OtherPlayer)
                        {
                            if (i == 0)
                            {
                                this.Highlighting(charactorFromGameObject);
                            }
                            RelationType relationType = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(charactorFromGameObject);
                            if (relationType == RelationType.Enemy)
                            {
                                this.SetMoseState(MoseState.m_atk);
                                return;
                            }
                            if (relationType != RelationType.Self)
                            {
                                this.SetMoseState(MoseState.m_dialog);
                                return;
                            }
                        }
                        if (i == 0)
                        {
                            this.Highlighting(charactorFromGameObject);
                        }
                        if (charactorFromGameObject is MainPlayer || charactorFromGameObject is Npc_Pet)
                        {
                            return;
                        }
                        if (charactorFromGameObject is Npc_TaskCollect && !(charactorFromGameObject as Npc_TaskCollect).CheckStateContainDoing())
                        {
                            return;
                        }
                        break;
                    }
                }
            }
        }
        this.SetMoseState(MoseState.m_default);
    }

    private void Highlighting(CharactorBase _tempselecttarget)
    {
        if (_tempselecttarget != null && _tempselecttarget.ModelObj != null)
        {
            HighlighterController component = _tempselecttarget.ModelObj.GetComponent<HighlighterController>();
            if (component != null)
            {
                component.MouseOver(_tempselecttarget.rlationType, _tempselecttarget is OtherPlayer);
            }
        }
    }

    private bool CheckHoverState()
    {
        if (Input.anyKey)
        {
            this.ResetHover(this.m_curState == MoseState.m_hover);
        }
        if (this.m_curState != MoseState.m_default && (this.m_curState != MoseState.m_hover || !(this.cursorHoverPos == Input.mousePosition) || !this.CheckMouseInScreen()))
        {
            if (this.m_curState == MoseState.m_hover)
            {
                this.ResetHover(true);
            }
            return false;
        }
        if (Time.realtimeSinceStartup - this.cursorHoverTime > this.HOVER_TIME)
        {
            this.SetMoseState(MoseState.m_hover);
            return true;
        }
        return false;
    }

    private void ResetHover(bool resetState = true)
    {
        this.cursorHoverTime = Time.realtimeSinceStartup;
        this.cursorHoverPos = Input.mousePosition;
        if (resetState)
        {
            this.SetMoseState(MoseState.m_default);
        }
    }

    private bool CheckMouseInScreen()
    {
        return Input.mousePosition.x < (float)Screen.width && Input.mousePosition.x > 0f && Input.mousePosition.y < (float)Screen.height && Input.mousePosition.y > 0f;
    }

    public static MouseStateControoler Instan;

    public float m_fsep = 0.1f;

    public float m_fdelta;

    public Texture2D m_default;

    public List<Texture2D> m_atks = new List<Texture2D>();

    public List<Texture2D> m_dialog = new List<Texture2D>();

    public List<Texture2D> m_collectionEnable = new List<Texture2D>();

    public List<Texture2D> m_collectionDisable = new List<Texture2D>();

    public List<Texture2D> m_itemsplit = new List<Texture2D>();

    public List<Texture2D> m_safelock = new List<Texture2D>();

    public List<Texture2D> m_cur = new List<Texture2D>();

    public List<Texture2D> m_pick = new List<Texture2D>();

    public List<Texture2D> m_hover = new List<Texture2D>();

    public List<Texture2D> m_scale3 = new List<Texture2D>();

    public List<Texture2D> m_scale_topleft_bottomright = new List<Texture2D>();

    public List<Texture2D> m_itemsell = new List<Texture2D>();

    public List<Texture2D> m_itemrepair = new List<Texture2D>();

    private MoseState m_curState;

    public int m_curIdx;

    private Texture2D t2d;

    private LuaTable config;

    internal object cur;

    private Vector3 cursorHoverPos;

    private float cursorHoverTime;

    private float HOVER_TIME = 2f;

    private RaycastHit[] hits = new RaycastHit[20];
}
