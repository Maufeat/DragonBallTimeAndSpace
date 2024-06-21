using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;

public class BufferState
{
    public BufferState(UserState Flag)
    {
        this.mFlag = Flag;
        this.lstBuffData = new List<BufferServerDate>();
        this.CurrBuffConfig = this.BufferstateMgr.GetBufferConfig(Flag);
        if (this.BufferConfig == null)
        {
            this.CurrBuffConfig = (LuaScriptMgr.Instance.CallLuaFunction("GetEmptyTable", new object[0])[0] as LuaTable);
            this.BufferConfig["id"] = (uint)Flag;
            FFDebug.LogWarning(this, "No Buff Config: " + Flag);
        }
        else
        {
            this._animType = (BufferAnimtype)this.BufferConfig.GetCacheField_Uint("Animtype");
            this._animPriority = this.BufferConfig.GetCacheField_Uint("AnimPriority");
            this._animActionid = this.BufferConfig.GetField_Uint("BuffAnim");
        }
    }

    public BufferState()
    {
    }

    public LuaTable CurrBuffConfig
    {
        get
        {
            return this.BufferConfig;
        }
        set
        {
            this.BufferConfig = value;
            if (this.BufferConfig != null)
            {
                this.buffAnim = (this.BufferConfig.GetCacheField_Uint("BuffAnim") > 0U);
                this.RevertAnim = (this.BufferConfig.GetCacheField_Uint("RevertAnim") > 0U);
            }
        }
    }

    public BufferAnimtype AnimType
    {
        get
        {
            return this._animType;
        }
    }

    public uint AnimPriority
    {
        get
        {
            return this._animPriority;
        }
    }

    public uint AnimActionid
    {
        get
        {
            return this._animActionid;
        }
    }

    public static void SetHandle(IBufferStateHandle Handle)
    {
        BufferState.IBufferStateHandleList.Add(Handle);
    }

    public uint CurLayer
    {
        get
        {
            if (this.lstBuffData == null || this.lstBuffData.Count < 1)
            {
                return 0U;
            }
            return (uint)this.lstBuffData.Count;
        }
    }

    public bool OnForceMove { get; protected set; }

    public bool OnChaosMove { get; protected set; }

    public BufferStateManager BufferstateMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<BufferStateManager>();
        }
    }

    public bool HasActionAnim
    {
        get
        {
            return this.buffAnim;
        }
    }

    public bool HasRevertActionAnim
    {
        get
        {
            return this.RevertAnim;
        }
    }

    public bool IsControlBuffer(BufferState.ControlType flag)
    {
        return this.IsContainsFlag((int)this.BufferConfig.GetCacheField_Uint("ControlType"), flag);
    }

    private bool IsContainsFlag(int rflag, BufferState.ControlType flag)
    {
        bool result = false;
        if ((rflag & (int)flag) != 0)
        {
            result = true;
        }
        return result;
    }

    public virtual void Enter(PlayerBufferControl PBControl)
    {
        this.BufferControl = PBControl;
        this.AddEffect(this.BufferConfig.GetField_String("BuffEffect"));
        this.AddActionClipEffect();
        if (this.HasActionAnim)
        {
            this.BufferControl.SetPlayerBuffActionBehaviour(this, false);
        }
        this.OnForceMove = this.IsControlBuffer(BufferState.ControlType.ForceMove);
        this.OnChaosMove = this.IsControlBuffer(BufferState.ControlType.ChaosMove);
        FFDebug.Log(this, FFLogType.Buff, string.Concat(new object[]
        {
            "BufferState Enter",
            this.mFlag,
            ":",
            (int)this.mFlag
        }));
    }

    public virtual void InitBufferServerData()
    {
    }

    public virtual void Update(BufferServerDate newData, bool IsNewAdd)
    {
        this.UpdateBSList(newData, IsNewAdd);
    }

    public virtual void TimeClick(ulong ServerNowTime)
    {
        if (this.lstBuffData == null || this.mFlag == UserState.USTATE_FLOAT_STATE)
        {
        }
        if (this.HasActionAnim && this.HasRevertActionAnim && !this.HasPlayRevertActionAnim)
        {
            if (this.lstBuffData.Count == 1)
            {
                BufferServerDate bufferServerDate = this.lstBuffData[0];
                if (bufferServerDate.overtime > bufferServerDate.settime && ServerNowTime + 1000UL > bufferServerDate.overtime)
                {
                    this.BufferControl.RemovePlayerBuffActionBehaviour(this.mFlag, true);
                    this.HasPlayRevertActionAnim = true;
                    if (this.OnForceMove)
                    {
                        this.OnForceMove = false;
                    }
                    if (this.OnChaosMove)
                    {
                        this.OnChaosMove = false;
                    }
                }
            }
            else
            {
                this.HasPlayRevertActionAnim = false;
            }
        }
    }

    public virtual void UpdateBSList(BufferServerDate newData, bool IsNewAdd)
    {
        if (this.BufferConfig.GetCacheField_Uint("addtype") == 0U)
        {
            if (this.BufferConfig.GetCacheField_Uint("addlayer") <= 1U)
            {
                if (this.BufferConfig.GetCacheField_Uint("replacetype") == 0U)
                {
                    this.RemoveAllBSByGiver(newData.giver);
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                    if (!IsNewAdd)
                    {
                        this.WhenBuffReplace();
                    }
                }
                else if (this.lstBuffData == null || this.lstBuffData.Count < 1)
                {
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                }
            }
            else
            {
                this.RemoveByThisID(newData.thisid);
                uint bsmaxLayerByGiver = this.GetBSMaxLayerByGiver(newData.giver);
                if (bsmaxLayerByGiver < this.BufferConfig.GetCacheField_Uint("addlayer"))
                {
                    this.UpdateBSTime(newData, this.BufferConfig.GetCacheField_Uint("addtype"), this.BufferConfig.GetCacheField_Uint("reset_time"));
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                }
                else if (this.BufferConfig.GetCacheField_Uint("replacetype") == 0U)
                {
                    this.TryRemoveOneBSByGiver(newData.giver);
                    this.UpdateBSTime(newData, this.BufferConfig.GetCacheField_Uint("addtype"), this.BufferConfig.GetCacheField_Uint("reset_time"));
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                    if (!IsNewAdd)
                    {
                        this.WhenBuffReplace();
                    }
                }
            }
        }
        else if (this.BufferConfig.GetCacheField_Uint("addtype") == 1U)
        {
            if (this.BufferConfig.GetCacheField_Uint("addlayer") <= 1U)
            {
                if (this.BufferConfig.GetCacheField_Uint("replacetype") == 0U)
                {
                    this.RemoveAllBS();
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                    if (!IsNewAdd)
                    {
                        this.WhenBuffReplace();
                    }
                }
                else if (this.lstBuffData == null || this.lstBuffData.Count < 1)
                {
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                }
            }
            else
            {
                this.RemoveByThisID(newData.thisid);
                uint bsmaxLayer = this.GetBSMaxLayer();
                if (bsmaxLayer < this.BufferConfig.GetCacheField_Uint("addlayer"))
                {
                    this.UpdateBSTime(newData, this.BufferConfig.GetCacheField_Uint("addtype"), this.BufferConfig.GetCacheField_Uint("reset_time"));
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                }
                else if (this.BufferConfig.GetCacheField_Uint("replacetype") == 0U)
                {
                    this.TryRemoveOneBS();
                    this.UpdateBSTime(newData, this.BufferConfig.GetCacheField_Uint("addtype"), this.BufferConfig.GetCacheField_Uint("reset_time"));
                    this.lstBuffData.Add(newData);
                    this.TryAddBuffIcon(newData);
                    if (!IsNewAdd)
                    {
                        this.WhenBuffReplace();
                    }
                }
            }
        }
    }

    public virtual BufferServerDate GetBSDataByUniqueID(ulong uniqueID)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return null;
        }
        int count = this.lstBuffData.Count;
        for (int i = 0; i < count; i++)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.uniqueid == uniqueID)
            {
                return bufferServerDate;
            }
        }
        return null;
    }

    public virtual BufferServerDate GetBSDataByGiver(ulong giverID)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return null;
        }
        int count = this.lstBuffData.Count;
        for (int i = 0; i < count; i++)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.giver == giverID)
            {
                return bufferServerDate;
            }
        }
        return null;
    }

    public virtual uint GetBSMaxLayerByGiver(ulong giverID)
    {
        uint num = 0U;
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return num;
        }
        int count = this.lstBuffData.Count;
        for (int i = 0; i < count; i++)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.giver == giverID)
            {
                num += 1U;
            }
        }
        return num;
    }

    public virtual uint GetBSMaxLayer()
    {
        uint result = 0U;
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return result;
        }
        return (uint)this.lstBuffData.Count;
    }

    public virtual bool TryRemoveOneBSByGiver(ulong giverID)
    {
        bool result = false;
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return result;
        }
        uint num = 0U;
        int index = 0;
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.giver == giverID && (num == 0U || bufferServerDate.settime < (ulong)num))
            {
                index = i;
                result = true;
            }
        }
        this.lstBuffData.RemoveAt(index);
        return result;
    }

    public virtual bool TryRemoveOneBS()
    {
        bool result = false;
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return result;
        }
        uint num = 0U;
        int index = 0;
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (num == 0U || bufferServerDate.overtime < (ulong)num)
            {
                index = i;
                result = true;
            }
        }
        this.lstBuffData.RemoveAt(index);
        return result;
    }

    public virtual void RemoveAllBS()
    {
        if (this.lstBuffData == null)
        {
            return;
        }
        this.lstBuffData.Clear();
    }

    public virtual void RemoveAllBSByGiver(ulong giverID)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return;
        }
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.giver == giverID)
            {
                this.lstBuffData.RemoveAt(i);
            }
        }
    }

    public virtual void RemoveByThisID(ulong thisid)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return;
        }
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.thisid == thisid)
            {
                this.lstBuffData.RemoveAt(i);
            }
        }
    }

    public virtual void UpdateBSTime(BufferServerDate newData, uint addtype, uint resettype)
    {
        if (resettype == 0U)
        {
            return;
        }
        if (addtype == 0U)
        {
            if (this.lstBuffData == null || this.lstBuffData.Count < 1)
            {
                return;
            }
            for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
            {
                BufferServerDate bufferServerDate = this.lstBuffData[i];
                if (bufferServerDate.flag == newData.flag && bufferServerDate.giver == newData.giver)
                {
                    bufferServerDate.duartion = newData.duartion;
                    bufferServerDate.overtime = newData.overtime;
                }
            }
        }
        else if (addtype == 1U)
        {
            for (int j = this.lstBuffData.Count - 1; j >= 0; j--)
            {
                BufferServerDate bufferServerDate2 = this.lstBuffData[j];
                if (bufferServerDate2.flag == newData.flag)
                {
                    bufferServerDate2.duartion = newData.duartion;
                    bufferServerDate2.overtime = newData.overtime;
                }
            }
        }
    }

    public virtual void TryAddBuffIcon(BufferServerDate newData)
    {
        if (this.BufferConfig.GetCacheField_Uint("IconShowType") != 0U)
        {
            this.BufferControl.TryShowBuffIcon(this.BufferConfig, newData);
        }
    }

    public virtual void RemoveOneBS(BufferServerDate bs)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return;
        }
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.thisid == bs.thisid)
            {
                this.lstBuffData.RemoveAt(i);
                this.TryRemoveBuffIcon(bs);
                break;
            }
        }
    }

    public virtual void TryRemoveBuffIcon(BufferServerDate newData)
    {
        if (this.BufferConfig.GetField_Uint("IconShowType") != 0U)
        {
            this.BufferControl.TryRemoveBuffIcon(this.BufferConfig, newData);
        }
    }

    private void WhenBuffReplace()
    {
        if (this.HasActionAnim)
        {
            this.BufferControl.RemovePlayerBuffActionBehaviour(this.mFlag, false);
            this.BufferControl.SetPlayerBuffActionBehaviour(this, false);
        }
    }

    public virtual void Exit()
    {
        FFDebug.Log(this, FFLogType.Buff, string.Concat(new object[]
        {
            "BufferState Exit :",
            this.mFlag,
            ":",
            (int)this.mFlag
        }));
        this.RemoveAllEffect();
        this.AddEndingEffect();
    }

    public virtual void AddEndingEffect()
    {
        if (string.IsNullOrEmpty(this.BufferConfig.GetField_String("EndingEffect")))
        {
            return;
        }
        FFEffectControl component = this.BufferControl.Owner.GetComponent<FFEffectControl>();
        if (component != null)
        {
            component.AddEffectGroupOnce(this.BufferConfig.GetField_String("EndingEffect"));
        }
    }

    public virtual void AddActionClipEffect()
    {
        if (!this.HasActionAnim)
        {
            return;
        }
        uint field_Uint = this.BufferConfig.GetField_Uint("BuffAnim");
        int selectindex = (!this.BufferControl.Owner.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(this.BufferControl.Owner.animatorControllerName, field_Uint, selectindex);
        if (ffactionClip == null)
        {
            return;
        }
        FFEffectControl component = this.BufferControl.Owner.GetComponent<FFEffectControl>();
        if (component == null)
        {
            return;
        }
        FFeffect[] array = component.AddEffect(ffactionClip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, 1U), null, null, false);
        if (array == null)
        {
            return;
        }
        this.FFeffectList.AddRange(array);
    }

    public virtual void AddEffect(string effgroupname)
    {
        if (string.IsNullOrEmpty(effgroupname))
        {
            return;
        }
        if (this.BufferControl == null)
        {
            return;
        }
        FFEffectControl component = this.BufferControl.Owner.GetComponent<FFEffectControl>();
        if (component == null)
        {
            return;
        }
        FFEffectManager manager = ManagerCenter.Instance.GetManager<FFEffectManager>();
        string[] group = manager.GetGroup(effgroupname);
        for (int i = 0; i < group.Length; i++)
        {
            FFeffect ffeffect = component.AddEffect(group[i], null, null);
            if (ffeffect != null)
            {
                this.FFeffectList.Add(ffeffect);
            }
        }
    }

    public virtual void RemoveAllEffect()
    {
        for (int i = 0; i < this.FFeffectList.Count; i++)
        {
            this.FFeffectList[i].mState = FFeffect.State.Over;
        }
        this.FFeffectList.Clear();
    }

    public virtual string[] GetActiveEffect()
    {
        List<string> list = new List<string>();
        foreach (FFeffect ffeffect in this.FFeffectList)
        {
            if (ffeffect != null && ffeffect.mState != FFeffect.State.none && ffeffect.mState != FFeffect.State.Over && (this.BufferConfig.GetField_Uint("BreakType") & 32U) == 0U)
            {
                list.Add(ffeffect.Clip.EffectName);
            }
        }
        return list.ToArray();
    }

    public const uint RevertActionLength = 1000U;

    public PlayerBufferControl BufferControl;

    private LuaTable BufferConfig;

    public List<BufferServerDate> lstBuffData;

    private bool buffAnim;

    private bool RevertAnim;

    private BufferAnimtype _animType;

    private uint _animPriority;

    private uint _animActionid;

    protected static List<IBufferStateHandle> IBufferStateHandleList;

    public UserState mFlag;

    protected List<FFeffect> FFeffectList = new List<FFeffect>();

    private bool HasPlayRevertActionAnim;

    public enum ControlType
    {
        none,
        NoMove,
        NoSkill,
        ForceMove = 4,
        ChaosMove = 8,
        NoLieve = 16
    }
}
public interface IBufferStateHandle
{
    void OnBufferStateEnter(BufferState buff);

    void OnBufferStateExit(BufferState buff);
}
