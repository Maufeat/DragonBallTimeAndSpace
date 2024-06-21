using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class PlayerVisitCtrl : IFFComponent
{
    private VisitPlayerController visitPlayerController
    {
        get
        {
            return ControllerManager.Instance.GetController<VisitPlayerController>();
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        this.Init();
    }

    private void Init()
    {
        if (this.Owner == MainPlayer.Self)
        {
            return;
        }
        if (this.Owner == null)
        {
            return;
        }
        if (this.Owner.hpdata == null)
        {
            return;
        }
        this.hasinit = true;
        this.hpdata = this.Owner.hpdata;
        this.TryPlayerShowTopButton();
        this.Owner.CharEvtMgr.AddListener("CharEvt_BuffUpdate", new CharacterEventMgr.CharacterEventHandler(this.OwnerBuffChange));
        this.Owner.CharEvtMgr.AddListener("CharEvt_BuffRemove", new CharacterEventMgr.CharacterEventHandler(this.OwnerBuffChange));
        this.Owner.CharEvtMgr.AddListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.DistanceWithMainPlayerChange));
        MainPlayer.Self.CharEvtMgr.AddListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.DistanceWithMainPlayerChange));
        MainPlayer.Self.CharEvtMgr.AddListener("CharEvt_MainPlayerReachTarget", new CharacterEventMgr.CharacterEventHandler(this.OnMainPlayerReachMe));
    }

    private void TryPlayerShowTopButton()
    {
        if (this.Owner == null || MainPlayer.Self == null)
        {
            return;
        }
        PlayerBufferControl component = this.Owner.GetComponent<PlayerBufferControl>();
        if (component == null)
        {
            FFDebug.LogWarning(this, "TryShowTopButton bufferControl null Owner: " + this.Owner.EID.ToString());
            return;
        }
        this.hpdata.CloseNPCTopButton();
        FFDetectionNpcControl component2 = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component2 == null)
        {
            return;
        }
        if (!component2.IsNearestNpc(this.Owner.EID.Id))
        {
            return;
        }
        for (int i = 0; i < this.UserStateShowIconList.Count; i++)
        {
            UserState flag = this.UserStateShowIconList[i];
            if (component.ContainsState(flag))
            {
                BufferState userState = component.GetUserState(flag);
                this.hpdata.ShowNPCTopButton(string.Empty);
                break;
            }
        }
    }

    private bool IsInRange(Vector2 selfServerPos, Vector2 npcServerPos)
    {
        bool result = false;
        if (this.TopButtonshowRange < 0f)
        {
            this.TopButtonshowRange = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("npcVisiteShowDis").GetCacheField_Float("value");
        }
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(selfServerPos);
        Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(npcServerPos);
        float num = Vector3.Distance(worldPosByServerPos, worldPosByServerPos2);
        if (num <= this.TopButtonshowRange)
        {
            result = true;
        }
        return result;
    }

    private void OwnerBuffChange(CharactorBase charb, params object[] args)
    {
        if (args.Length > 0)
        {
            UserState item = (UserState)((int)args[0]);
            if (this.UserStateShowIconList.Contains(item))
            {
                this.TryPlayerShowTopButton();
            }
        }
    }

    private void DistanceWithMainPlayerChange(CharactorBase charb, params object[] args)
    {
        this.TryPlayerShowTopButton();
    }

    private void OnMainPlayerReachMe(CharactorBase charb, params object[] args)
    {
        if (args.Length > 0)
        {
            CharactorBase charactorBase = (CharactorBase)args[0];
            if (charactorBase == this.Owner)
            {
                this.TryReqHoldon();
            }
        }
    }

    private void TryReqHoldon()
    {
        PlayerBufferControl component = this.Owner.GetComponent<PlayerBufferControl>();
        if (component == null)
        {
            FFDebug.LogWarning(this, "TryShowTopButton bufferControl null Owner: " + this.Owner.EID.ToString());
            return;
        }
        for (int i = 0; i < this.UserStateShowIconList.Count; i++)
        {
            UserState flag = this.UserStateShowIconList[i];
            if (component.ContainsState(flag))
            {
                BufferState userState = component.GetUserState(flag);
                this.visitPlayerController.ReqVisitPlayer(this.Owner.EID);
                break;
            }
        }
    }

    public void ClickPlayerTopBtn()
    {
        if (this.Owner == MainPlayer.Self)
        {
            return;
        }
        if (this.Owner == null)
        {
            return;
        }
        if (!this.hasinit)
        {
            return;
        }
        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().manualSelect.SetTargetResult(this.Owner);
    }

    public void CompDispose()
    {
        this.Owner.CharEvtMgr.RemoveListener("CharEvt_BuffUpdate", new CharacterEventMgr.CharacterEventHandler(this.OwnerBuffChange));
        this.Owner.CharEvtMgr.RemoveListener("CharEvt_BuffRemove", new CharacterEventMgr.CharacterEventHandler(this.OwnerBuffChange));
        this.Owner.CharEvtMgr.RemoveListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.DistanceWithMainPlayerChange));
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.CharEvtMgr.RemoveListener("CharEvt_MoveOneStep", new CharacterEventMgr.CharacterEventHandler(this.DistanceWithMainPlayerChange));
            MainPlayer.Self.CharEvtMgr.RemoveListener("CharEvt_MainPlayerReachTarget", new CharacterEventMgr.CharacterEventHandler(this.OnMainPlayerReachMe));
        }
    }

    public void CompUpdate()
    {
        if (this.Owner == MainPlayer.Self)
        {
            return;
        }
        if (this.Owner == null)
        {
            return;
        }
        if (!this.hasinit)
        {
            this.Init();
        }
    }

    public void ResetComp()
    {
    }

    public CharactorBase Owner;

    private HpStruct hpdata;

    private List<UserState> UserStateShowIconList = new List<UserState>
    {
        UserState.USTATE_FOOD_STATE
    };

    private bool hasinit;

    private float TopButtonshowRange = -1f;
}
