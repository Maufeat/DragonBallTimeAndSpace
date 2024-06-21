using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEventController
{
    public void ActiveUpdateByMouse(bool active)
    {
        this.canUpdateByMouse = active;
    }

    private ScreenEventController.TouchInfo GetTouchInfo(int Tid)
    {
        if (!this.TouchInfoMap.ContainsKey(Tid))
        {
            ScreenEventController.TouchInfo touchInfo = new ScreenEventController.TouchInfo();
            touchInfo.TouchId = Tid;
            this.TouchInfoMap[Tid] = touchInfo;
        }
        return this.TouchInfoMap[Tid];
    }

    private ScreenEvent GetScreenEvent(int Tid)
    {
        if (!this.ScreenEventMap.ContainsKey(Tid))
        {
            ScreenEvent value = new ScreenEvent();
            this.ScreenEventMap[Tid] = value;
        }
        return this.ScreenEventMap[Tid];
    }

    public void UpdateScreenEvent()
    {
        if (GUIUtility.hotControl != 0)
        {
            return;
        }
        this.UpdateByMouse();
    }

    private bool CheckUpdateByMouse()
    {
        return this.canUpdateByMouse && !UIManager.GetUIObject<UI_P2PLogin>() && !UIManager.GetUIObject<UI_SelectRole>();
    }

    private void UpdateByMouse()
    {
        if (!this.CheckUpdateByMouse())
        {
            return;
        }
        Vector3 zero = Vector3.zero;
        if (this._currScreenEvent == null)
        {
            this._currScreenEvent = new ScreenEvent[]
            {
                this.GetScreenEvent(0),
                this.GetScreenEvent(1)
            };
        }
        else
        {
            this._currScreenEvent[0] = this.GetScreenEvent(0);
            this._currScreenEvent[1] = this.GetScreenEvent(1);
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            this._currScreenEvent[this.btnid].SlipDis = Vector3.zero;
            if (Input.GetMouseButtonDown(0))
            {
                this.btnid = 0;
                this.leftmousedowntime = Time.realtimeSinceStartup;
            }
            if (Input.GetMouseButtonDown(1))
            {
                this.btnid = 1;
            }
            this._currScreenEvent[this.btnid].ClickDownPos = Input.mousePosition;
            this._currScreenEvent[this.btnid].LastPos = Input.mousePosition;
            this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Began;
            this._currScreenEvent[this.btnid].InputPos = Input.mousePosition;
            this._currScreenEvent[this.btnid].mFingerId = ((!Input.GetMouseButton(0)) ? 1 : 0);
            this._currScreenEvent[this.btnid].IsMouseSlip = false;
            this.SendEvent(this._currScreenEvent[this.btnid]);
            this._currScreenEvent[this.btnid].IsDragGraphic = UITools.IsPointOverGraphic(UITools.PointLayer.Non_Mask);
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (Input.GetMouseButton(0))
            {
                this.btnid = 0;
            }
            if (Input.GetMouseButton(1))
            {
                this.btnid = 1;
            }
            if (Vector3.Distance(Input.mousePosition, this._currScreenEvent[this.btnid].ClickDownPos) >= this.ClickRange)
            {
                if (this._currScreenEvent[this.btnid].IsDragGraphic)
                {
                    this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Slip;
                    this._currScreenEvent[this.btnid].SlipDis = Input.mousePosition - this._currScreenEvent[this.btnid].LastPos;
                    this._currScreenEvent[this.btnid].InputPos = Input.mousePosition;
                    this._currScreenEvent[this.btnid].mFingerId = ((!Input.GetMouseButton(0)) ? 1 : 0);
                    this.SendEvent(this._currScreenEvent[this.btnid]);
                    this._currScreenEvent[this.btnid].LastPos = Input.mousePosition;
                }
                else if (!this._currScreenEvent[this.btnid].IsMouseSlip)
                {
                    this._currScreenEvent[this.btnid].CursorPos = SingletonModel<DllMgr>.Instatnce.GetCursorPos();
                    this._currScreenEvent[this.btnid].IsMouseSlip = true;
                }
                else
                {
                    if (Input.GetMouseButton(1))
                    {
                        SingletonForMono<InputController>.Instance.SetTurnedTo(true, true);
                    }
                    else
                    {
                        SingletonForMono<InputController>.Instance.SetTurnedTo(false, true);
                    }
                    this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Slip;
                    Vector2 vector = SingletonModel<DllMgr>.Instatnce.GetCursorPos() - this._currScreenEvent[this.btnid].CursorPos;
                    Vector3 slipDis = new Vector3(vector.x, -vector.y, 0f);
                    this._currScreenEvent[this.btnid].SlipDis = slipDis;
                    this.SendEvent(this._currScreenEvent[this.btnid]);
                    SingletonModel<DllMgr>.Instatnce.SetCursorPos(this._currScreenEvent[this.btnid].CursorPos);
                }
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            if (Input.GetMouseButtonUp(0))
            {
                this.btnid = 0;
            }
            if (Input.GetMouseButtonUp(1))
            {
                this.btnid = 1;
            }
            if (!this._currScreenEvent[this.btnid].IsMouseSlip)
            {
                if (!this._currScreenEvent[this.btnid].IsDragGraphic)
                {
                    if (Input.GetMouseButtonUp(1))
                    {
                        this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Click;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Select;
                        if (Time.realtimeSinceStartup - this.leftmousedowntime < 0.2f && MouseStateControoler.Instan.GetCurMoseState() == MoseState.m_default && GameSystemSettings.GetMouseClickMove() && MainPlayer.Self != null && MainPlayer.Self.IsLive)
                        {
                            this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.QuickClick;
                        }
                    }
                    this._currScreenEvent[this.btnid].InputPos = Input.mousePosition;
                    this.SendEvent(this._currScreenEvent[this.btnid]);
                }
            }
            else
            {
                this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.SlipEnd;
                SingletonForMono<InputController>.Instance.SetTurnedTo(false, false);
                this._currScreenEvent[this.btnid].InputPos = Input.mousePosition;
                this.SendEvent(this._currScreenEvent[this.btnid]);
                this._currScreenEvent[this.btnid].IsMouseSlip = false;
            }
        }
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0f)
        {
            this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Zoom;
            this._currScreenEvent[this.btnid].mfMouseScrollWheel = axis;
            this.SendEvent(this._currScreenEvent[this.btnid]);
        }
        if (this._currScreenEvent != null && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            this._currScreenEvent[this.btnid].mTpye = ScreenEvent.EventType.Began;
            this._currScreenEvent[this.btnid].InputPos = Input.mousePosition;
            this.SendEvent(this._currScreenEvent[this.btnid]);
            this._currScreenEvent[this.btnid].IsMouseSlip = false;
        }
    }

    private void SendEvent(ScreenEvent SE)
    {
        if (SE.IsDragGraphic)
        {
            return;
        }
        this.OnClickEvent(SE);
    }

    private void OnClickEvent(ScreenEvent SE)
    {
        for (int i = 0; i < this.ScreenEventAddListenerList.Count; i++)
        {
            if (this.ScreenEventAddListenerList[i] != null)
            {
                this.ScreenEventAddListenerList[i](SE);
            }
        }
    }

    public void AddListener(ScreenEventController.OnScreenEvent Evt)
    {
        if (Evt != null && !this.ScreenEventAddListenerList.Contains(Evt))
        {
            this.ScreenEventAddListenerList.Add(Evt);
        }
    }

    public void RemoveListener(ScreenEventController.OnScreenEvent Evt)
    {
        if (Evt != null)
        {
            this.ScreenEventAddListenerList.Remove(Evt);
        }
    }

    private const int ScrollWheeEventId = 11;

    private float ClickRange = 10f;

    private BetterDictionary<int, ScreenEvent> ScreenEventMap = new BetterDictionary<int, ScreenEvent>();

    private List<ScreenEventController.OnScreenEvent> ScreenEventAddListenerList = new List<ScreenEventController.OnScreenEvent>();

    private BetterDictionary<int, ScreenEventController.TouchInfo> TouchInfoMap = new BetterDictionary<int, ScreenEventController.TouchInfo>();

    private bool isoveruiobjectwhenstartinput;

    private bool canUpdateByMouse = true;

    private List<Vector2> Poslist = new List<Vector2>();

    private List<ScreenEvent> EventList = new List<ScreenEvent>();

    protected int btnid;

    private float leftmousedowntime;

    private ScreenEvent[] _currScreenEvent;

    private class TouchInfo
    {
        public int TouchId;

        public Vector3 BeganPos = Vector3.zero;

        public Vector3 LastPos = Vector3.zero;

        public bool HasMove;
    }

    public delegate void OnScreenEvent(ScreenEvent SE);
}
