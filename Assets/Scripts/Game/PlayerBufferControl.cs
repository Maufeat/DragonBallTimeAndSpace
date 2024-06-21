using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using msg;
using UnityEngine;

public class PlayerBufferControl : IFFComponent
{
    public FFBehaviourControl mFFBC
    {
        get
        {
            return this.Owner.ComponentMgr.GetComponent<FFBehaviourControl>();
        }
    }

    public CompnentState State { get; set; }

    private BufferStateManager BufferstateMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<BufferStateManager>();
        }
    }

    private GameScene gs
    {
        get
        {
            return ManagerCenter.Instance.GetManager<GameScene>();
        }
    }

    private AbattoirMatchController abattoirController
    {
        get
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>();
        }
    }

    public BetterDictionary<int, BufferState> AllBufferMap()
    {
        return this.BufferStateList;
    }

    private void calculatebufferStateControlType()
    {
        this._bufferStateControlType = 0U;
        this.BufferStateList.BetterForeach(delegate (KeyValuePair<int, BufferState> item)
        {
            this._bufferStateControlType |= item.Value.CurrBuffConfig.GetField_Uint("ControlType");
        });
    }

    public bool HasBeControlled(BufferState.ControlType flag)
    {
        bool result = false;
        if (((ulong)this._bufferStateControlType & (ulong)((long)flag)) != 0UL)
        {
            result = true;
        }
        return result;
    }

    public void AddStateByLocal(StateItem stateInfo)
    {
        BufferServerDate buffServerData = CommonTools.GetBuffServerData(stateInfo);
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        if (buffServerData.overtime > currServerTime)
        {
            ulong num = buffServerData.overtime - currServerTime;
            if (buffServerData.duartion > num)
            {
                buffServerData.curTime = buffServerData.duartion - num;
            }
        }
        else if (buffServerData.overtime > buffServerData.settime)
        {
            FFDebug.Log(this, FFLogType.Buff, string.Concat(new object[]
            {
                this.Owner.EID,
                " time out :",
                buffServerData.flag,
                " overtime:",
                buffServerData.overtime
            }));
            return;
        }
        this.UpdateState(buffServerData);
    }

    public void AddStateByArray(List<StateItem> Bufferlist)
    {
        for (int i = 0; i < Bufferlist.Count; i++)
        {
            this.AddStateByLocal(Bufferlist[i]);
        }
    }

    public void UpdateState(BufferServerDate ServerDate)
    {
        if (this.Owner is MainPlayer)
        {
        }
        if (!this.BufferStateList.ContainsKey((int)ServerDate.flag))
        {
            BufferState bufferState = BufferStateFactory.ProduceBufferState(ServerDate.flag);
            this.BufferStateList.AddData((int)ServerDate.flag, bufferState);
            if (this.gs.isAbattoirScene)
            {
                if (ServerDate.flag == UserState.USTATE_MOBA_PRAY_HOPE21)
                {
                    this.abattoirController.ShowReliveInSitu(true);
                }
                else if (ServerDate.flag == UserState.USTATE_MOBA_SUPER_RADAR)
                {
                    this.abattoirController.ShowRadarAreaIcon(false);
                }
            }
            if ((this.BufferStateList[(int)ServerDate.flag].IsControlBuffer(BufferState.ControlType.NoSkill) || this.BufferStateList[(int)ServerDate.flag].IsControlBuffer(BufferState.ControlType.NoLieve)) && this.Owner is MainPlayer)
            {
                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().OnBreak(CSkillBreakType.InBuff);
            }
            this.calculatebufferStateControlType();
            try
            {
                bufferState.Enter(this);
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, "BufferState Enter error :" + arg);
            }
            this.BufferStateList[(int)ServerDate.flag].Update(ServerDate, true);
            if (bufferState.mFlag == UserState.USTATE_DEATH)
            {
                this.Owner.Die();
            }
            else if (bufferState.mFlag == UserState.USTATE_SOUL && this.Owner is MainPlayer)
            {
                if (null != CameraController.Self)
                {
                    CameraController.Self.ShowGrayscale(true);
                }
                MainPlayerSkillHolder.Instance.normalAttackAutoAttack.ActiveSelf = false;
                MainPlayerSkillHolder.Instance.skillAttackAutoAttack.ActiveSelf = false;
                if (this.gs.isAbattoirScene)
                {
                    this.abattoirController.SetFightState(AbattoirFightState.USTATE_SOUL);
                }
            }
        }
        else
        {
            this.BufferStateList[(int)ServerDate.flag].Update(ServerDate, false);
        }
        this.Owner.OnUpdateCharacterBuff(ServerDate.flag);
    }

    public bool ContainsState(UserState Flag)
    {
        return this.ContainsState((int)Flag);
    }

    public bool ContainsState(int Flag)
    {
        return this.BufferStateList.ContainsKey(Flag);
    }

    public uint getBufferAnimActionID(BufferAnimtype animtype)
    {
        BufferState bufferState = null;
        this.BufferStateList.BetterForeach(delegate (KeyValuePair<int, BufferState> item)
        {
            if (item.Value.AnimType == animtype && (bufferState == null || bufferState.AnimPriority < item.Value.AnimPriority))
            {
                bufferState = item.Value;
            }
        });
        return (bufferState != null) ? bufferState.AnimActionid : 0U;
    }

    public BufferState GetUserState(UserState Flag)
    {
        if (this.ContainsState(Flag))
        {
            return this.BufferStateList[(int)Flag];
        }
        return null;
    }

    public T GetUserState<T>(UserState Flag) where T : BufferState
    {
        BufferState userState = this.GetUserState(Flag);
        if (userState is T)
        {
            return userState as T;
        }
        return (T)((object)null);
    }

    public void RemoveState(UserState flag)
    {
        if (this.Owner == null)
        {
            return;
        }
        BufferServerDate bs = new BufferServerDate(flag, this.Owner.EID);
        this.RemoveState(bs);
    }

    public void RemoveState(BufferServerDate bs)
    {
        UserState flag = bs.flag;
        if (this.BufferStateList.ContainsKey((int)flag))
        {
            if (flag == UserState.USTATE_DEATH)
            {
                BufferState bufferState = this.BufferStateList[(int)flag];
                this.BufferStateList.RemoveData((int)flag);
                this.calculatebufferStateControlType();
                this.RemovePlayerBuffActionBehaviour(flag, true);
                bufferState.Exit();
                if (this.Owner == MainPlayer.Self)
                {
                    CallLuaListener.SendLuaEvent("OnUserReliveLuaListener", true, new object[]
                    {
                        this.Owner
                    });
                }
            }
            else if (flag == UserState.USTATE_SOUL)
            {
                CameraController.Self.ShowGrayscale(false);
                BufferState bufferState2 = this.BufferStateList[(int)flag];
                this.BufferStateList.RemoveData((int)flag);
                this.calculatebufferStateControlType();
                this.RemovePlayerBuffActionBehaviour(flag, true);
                bufferState2.Exit();
                if (this.gs.isAbattoirScene)
                {
                    this.abattoirController.SetFightState(AbattoirFightState.USTATE_RELIVE);
                }
            }
            else
            {
                BufferState bufferState3 = this.BufferStateList[(int)flag];
                bufferState3.RemoveOneBS(bs);
                if (bufferState3.CurLayer < 1U)
                {
                    this.BufferStateList.RemoveData((int)flag);
                    this.calculatebufferStateControlType();
                    this.RemovePlayerBuffActionBehaviour(flag, true);
                    bufferState3.Exit();
                }
            }
            if (this.gs.isAbattoirScene)
            {
                if (bs.flag == UserState.USTATE_MOBA_PRAY_HOPE21)
                {
                    this.abattoirController.ShowReliveInSitu(false);
                }
                else if (bs.flag == UserState.USTATE_MOBA_SUPER_RADAR)
                {
                    this.abattoirController.ShowRadarAreaIcon(true);
                }
            }
        }
        else
        {
            FFDebug.Log(this, FFLogType.Buff, string.Format("RemoveState Error: No State {0}", flag));
        }
        this.Owner.OnRemoveCharacterBuff(flag);
    }

    public bool HasNoTask()
    {
        return !this.ContainsState(UserState.USTATE_QUEST_START) && !this.ContainsState(UserState.USTATE_QUEST_DOING) && !this.ContainsState(UserState.USTATE_QUEST_FINISH);
    }

    public void OnSceneChange()
    {
        if (this.BufferStateList.ContainsKey(1179))
        {
            this.BufferStateList[1179].Exit();
            this.BufferStateList[1179].Enter(this);
        }
    }

    public void CacheBuffAnimInfo(UserState state, int buffAnim, int reverAnim)
    {
        this.cacheInfo.Clear();
        this.cacheInfo.flag = state;
        if (buffAnim < 0)
        {
            buffAnim = 0;
        }
        if (reverAnim < 0)
        {
            reverAnim = 0;
        }
        this.cacheInfo.buffAnim = (uint)buffAnim;
        this.cacheInfo.revertAnim = (uint)reverAnim;
    }

    public void SetPlayerBuffActionBehaviour(BufferState Buffer, bool forceImmediately = false)
    {
        if (this.mFFBC == null)
        {
            return;
        }
        FFBehaviourState_InBuffAction ffbehaviourState_InBuffAction = this.mFFBC.CurrStatebyType<FFBehaviourState_InBuffAction>();
        if (ffbehaviourState_InBuffAction == null)
        {
            ffbehaviourState_InBuffAction = ClassPool.GetObject<FFBehaviourState_InBuffAction>();
            ffbehaviourState_InBuffAction.PlayImmediately = (this.mFFBC.CurrState == null || forceImmediately);
            this.mFFBC.ChangeState(ffbehaviourState_InBuffAction);
        }
        ffbehaviourState_InBuffAction.SetBuffer(Buffer);
    }

    public bool ResetSetPlayerBuffActionBehaviour()
    {
        bool HasActionAnim = false;
        this.BufferStateList.BetterForeach(delegate (KeyValuePair<int, BufferState> item)
        {
            BufferState value = item.Value;
            if (value.HasActionAnim)
            {
                this.SetPlayerBuffActionBehaviour(value, true);
                HasActionAnim = true;
            }
        });
        return HasActionAnim;
    }

    public void RemovePlayerBuffActionBehaviour(UserState Flag, bool playRevertAnim = true)
    {
        if (this.mFFBC == null)
        {
            return;
        }
        FFBehaviourState_InBuffAction ffbehaviourState_InBuffAction = this.mFFBC.CurrStatebyType<FFBehaviourState_InBuffAction>();
        if (ffbehaviourState_InBuffAction != null)
        {
            ffbehaviourState_InBuffAction.RemoveBuffer(Flag, playRevertAnim);
        }
    }

    public void TryShowBuffIcon(LuaTable bufferConfig, BufferServerDate data)
    {
        if (this.Owner == null)
        {
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.AddSelfBuffIcon(this.Owner.EID, bufferConfig, data);
        }
    }

    public void TryRemoveBuffIcon(LuaTable bufferConfig, BufferServerDate data)
    {
        if (this.Owner == null)
        {
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.RemoveSelfBuffIcon(this.Owner.EID, bufferConfig, data);
        }
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        if (this.Owner is OtherPlayer)
        {
            OtherPlayer otherPlayer = this.Owner as OtherPlayer;
            if (otherPlayer.OtherPlayerData.MapUserData != null)
            {
                this.AddStateByArray(otherPlayer.OtherPlayerData.MapUserData.mapdata.lstState);
            }
            else
            {
                FFDebug.Log(this, FFLogType.Buff, "op.OtherPlayerData.MapUserData null");
            }
        }
        if (this.Owner is Npc)
        {
            Npc npc = this.Owner as Npc;
            if (npc.NpcData.MapNpcData != null)
            {
                this.AddStateByArray(npc.NpcData.MapNpcData.lstState);
            }
            else
            {
                FFDebug.Log(this, FFLogType.Buff, "op.OtherPlayerData.MapUserData null");
            }
        }
        this.cacheInfo = new CacheBufferInfo();
    }

    public void CompUpdate()
    {
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        List<BufferState> listValues = this.BufferStateList.ListValues;
        for (int i = 0; i < listValues.Count; i++)
        {
            listValues[i].TimeClick(currServerTime);
        }
        this.UpdateBuffCtrlMove();
    }

    private void UpdateBuffCtrlMove()
    {
        if (this.Owner != MainPlayer.Self)
        {
            return;
        }
        bool flag = false;
        List<KeyValuePair<int, BufferState>> keyValuePairs = this.BufferStateList.KeyValuePairs;
        for (int i = 0; i < keyValuePairs.Count; i++)
        {
            BufferState value = keyValuePairs[i].Value;
            if (value.OnForceMove)
            {
            }
            if (value.OnChaosMove)
            {
                flag = true;
            }
        }
        if (flag)
        {
            this.RandomTime += Time.deltaTime;
            if (this.RandomTime > 0.25f)
            {
                this.ChaosMoveDir = UnityEngine.Random.Range(0, 180);
                this.RandomTime = 0f;
            }
            MainPlayer.Self.MoveToByDir(this.ChaosMoveDir, false);
        }
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    public CharactorBase Owner;

    private BetterDictionary<int, BufferState> BufferStateList = new BetterDictionary<int, BufferState>();

    private uint _bufferStateControlType;

    public CacheBufferInfo cacheInfo;

    private int ChaosMoveDir;

    private float RandomTime;

    public delegate void VoidDelegate(UserState state);
}
