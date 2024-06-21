using System;
using System.Collections;
using System.Collections.Generic;
using Algorithms;
using Framework.Managers;
using LuaInterface;
using Models;
using UnityEngine;

public class AutoFightController : ControllerBase
{
    private MainPlayerTargetSelectMgr targetSelectMgr
    {
        get
        {
            if (this._targetSelectMgr == null)
            {
                this._targetSelectMgr = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            }
            if (this._targetSelectMgr.autoattackSelect == null)
            {
                this._targetSelectMgr.autoattackSelect = new AutoAttackTargetSelect();
                this._targetSelectMgr.autoattackSelect.Init();
            }
            return this._targetSelectMgr;
        }
    }

    private UI_ShortcutControl shortControl
    {
        get
        {
            if (this.mShortControl == null)
            {
                this.mShortControl = UIManager.GetUIObject<UI_MainView>().Root.GetComponent<UI_ShortcutControl>();
            }
            return this.mShortControl;
        }
    }

    private EntitiesManager entitiesManager
    {
        get
        {
            if (this._entitiesManager == null)
            {
                this._entitiesManager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            }
            return this._entitiesManager;
        }
    }

    public bool isOpenAutoFight
    {
        get
        {
            return this._isOpenAutoFight;
        }
        set
        {
            this._isOpenAutoFight = value;
            this.SetAutoFightTopText(value);
            if (value)
            {
                this.checkTimer = 0f;
                this.cardController.ReqBagFullState();
                this.InitRecoveryLifeCfg();
                this.InitMaxRecoveryPropData();
            }
            else
            {
                this.targetSelectMgr.SetTargetNull();
                this.cardController = null;
            }
        }
    }

    private CardController cardController
    {
        get
        {
            if (this.mcardController == null)
            {
                this.mcardController = ControllerManager.Instance.GetController<CardController>();
            }
            return this.mcardController;
        }
        set
        {
            this.mcardController = value;
        }
    }

    private AutoAttack playerAutoAttack
    {
        get
        {
            if (this._playerAutoAttack == null)
            {
                this._playerAutoAttack = MainPlayer.Self.GetComponent<AutoAttack>();
            }
            return this._playerAutoAttack;
        }
    }

    public override string ControllerName
    {
        get
        {
            return "auto_fight";
        }
    }

    public override void Awake()
    {
        EntitiesManager entitiesManager = this.entitiesManager;
        entitiesManager.onMainPlayer = (Action)Delegate.Combine(entitiesManager.onMainPlayer, new Action(this.ReqServerAutoFightSettingData));
        this.shortCutsController = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        this.pickController = ControllerManager.Instance.GetController<PickDropController>();
        this.skillViewController = ControllerManager.Instance.GetController<SkillViewControll>();
        GameMainManager manager = ManagerCenter.Instance.GetManager<GameMainManager>();
        manager.RegOnBackToHeroListEvent(this.ControllerName, new Action(this.ResetData));
    }

    private void ReqServerAutoFightSettingData()
    {
        Scheduler.Instance.AddTimer(5f, false, delegate
        {
            ServerStorageManager.Instance.GetData(ServerStorageKey.AutoFight, 0U);
        });
    }

    public override void OnUpdate()
    {
        if (!this.isOpenAutoFight)
        {
            return;
        }
        if (!this.isInitLifeRecoveryCfg)
        {
            return;
        }
        if (this.checkTimer < this.checkInterval)
        {
            this.checkTimer += Time.deltaTime;
            return;
        }
        if (!this.isHaveSettingData)
        {
            return;
        }
        if (this.shortCutsController == null)
        {
            return;
        }
        if (MainPlayer.Self == null || MainPlayer.Self.hpdata == null)
        {
            return;
        }
        if (this.targetSelectMgr == null)
        {
            return;
        }
        if (!MainPlayer.Self.IsLive)
        {
            return;
        }
        if (MainPlayer.Self.Pfc == null)
        {
            return;
        }
        if (MainPlayer.Self.IsMoving)
        {
            return;
        }
        bool flag = false;
        if (this.isAutoPick)
        {
            ulong num = 0UL;
            Npc npc = this.pickController.CheckIsHaveBagNpcInNineScreen(this.blackBagList);
            if (npc != null)
            {
                num = npc.EID.Id;
            }
            if (this.targetSelectMgr.TargetCharactor != null && this.blackBagList.Contains(this.targetSelectMgr.TargetCharactor.EID.Id))
            {
                num = 0UL;
                this.targetSelectMgr.SetTargetNull();
            }
            if (num != 0UL)
            {
                flag = true;
                if (this.cardController != null && this.cardController.CheckBagIsFull() && this.pickController.ui_PickDrop != null && !this.pickController.CheckCanPick())
                {
                    TipsWindow.ShowWindow(4013U);
                    flag = false;
                    this.blackBagList.Enqueue(num);
                    if (this.blackBagList.Count > 10)
                    {
                        this.blackBagList.Dequeue();
                    }
                }
                if (flag)
                {
                    this.pickController.ShortcutQuickPickAll(npc);
                }
            }
        }
        int count = this.autoFightSettings.Count;
        if (count > 0)
        {
            CharactorBase charactorBase = this.targetSelectMgr.autoattackSelect.SearchAutoAttackTarget();
            float num2 = MainPlayer.Self.hpdata.GetLifePercent() * 100f;
            if (this.speIndex4PropCall < this.autoFightSettings.Count)
            {
                AutoFightCfg autoFightCfg = this.autoFightSettings[this.speIndex4PropCall];
                if (autoFightCfg.isEnable && this.propId != 0U)
                {
                    if (num2 < (float)autoFightCfg.percent - 0.001f && autoFightCfg.timer > autoFightCfg.cdDuaration)
                    {
                        autoFightCfg.timer = 0f;
                        if (this.shortControl)
                        {
                            this.shortControl.UseItemById(this.propId);
                        }
                    }
                    autoFightCfg.timer += this.checkTimer;
                }
            }
            for (int i = count - 1; i >= 0; i--)
            {
                if (i != this.speIndex4PropCall)
                {
                    AutoFightCfg autoFightCfg2 = this.autoFightSettings[i];
                    if (autoFightCfg2.isEnable)
                    {
                        if (autoFightCfg2.keyType != ShortcutkeyFunctionType.Role)
                        {
                            autoFightCfg2.timer += this.checkTimer;
                            if (num2 <= (float)autoFightCfg2.percent + 0.001f)
                            {
                                if (autoFightCfg2.cfgType == AutoFightCfgType.Prop)
                                {
                                    if (autoFightCfg2.cdDuaration >= 0f)
                                    {
                                        if (autoFightCfg2.timer >= autoFightCfg2.cdDuaration)
                                        {
                                            this.shortCutsController.ExtendItemCallAction(autoFightCfg2.keyType);
                                            autoFightCfg2.timer = 0f;
                                        }
                                    }
                                }
                                else if (!flag)
                                {
                                    if (autoFightCfg2.skillID != 0U)
                                    {
                                        if (autoFightCfg2.cdDuaration >= 0f)
                                        {
                                            if (this.playerAutoAttack != null)
                                            {
                                                if (autoFightCfg2.skillHolder != null)
                                                {
                                                    ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
                                                    uint cdleft = autoFightCfg2.skillHolder.GetCDLeft(currServerTime);
                                                    if (cdleft <= 0U)
                                                    {
                                                        if (autoFightCfg2.skillHolder.PCdData == null || !autoFightCfg2.skillHolder.PCdData.CheckPublicCD())
                                                        {
                                                            if (this.targetSelectMgr.TargetCharactor == null)
                                                            {
                                                                this.targetSelectMgr.autoattackSelect.ReqTarget();
                                                                if (this.targetSelectMgr.TargetCharactor == null)
                                                                {
                                                                    goto IL_5E1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                RelationType npcRelationType = this.entitiesManager.GetNpcRelationType(this.targetSelectMgr.TargetCharactor as Npc);
                                                                if (npcRelationType == RelationType.Friend)
                                                                {
                                                                    goto IL_5E1;
                                                                }
                                                            }
                                                            if (this.targetSelectMgr.TargetCharactor is Npc)
                                                            {
                                                                float num3 = Vector2.Distance(this.targetSelectMgr.TargetCharactor.CurrServerPos, MainPlayer.Self.CurrServerPos);
                                                                if (num3 > autoFightCfg2.attackDistance)
                                                                {
                                                                    Vector2 vector = this.GetNearPoint(this.targetSelectMgr.TargetCharactor.CurrServerPos, autoFightCfg2.attackDistance - 4f);
                                                                    if (this.CheckBlock(vector))
                                                                    {
                                                                        vector = this.GetRandomPosInPoint(vector);
                                                                    }
                                                                    this.MoveToTarget(vector);
                                                                    if (autoFightCfg2.isNormalAttack)
                                                                    {
                                                                        goto IL_5E1;
                                                                    }
                                                                }
                                                            }
                                                            else if (this.targetSelectMgr.TargetCharactor is OtherPlayer && charactorBase != null)
                                                            {
                                                                this.targetSelectMgr.SetTargetNull();
                                                                goto IL_5E1;
                                                            }
                                                            if (this.CheckBlock(this.targetSelectMgr.TargetCharactor.CurrServerPos))
                                                            {
                                                                Vector2 randomPosInPoint = this.GetRandomPosInPoint(this.targetSelectMgr.TargetCharactor.CurrServerPos);
                                                                MainPlayer.Self.Pfc.BeginFindPath(randomPosInPoint, PathFindComponent.AutoMoveState.MoveToAttackNpc, 0.1f, delegate ()
                                                                {
                                                                    this.targetSelectMgr.autoattackSelect.ReqTarget();
                                                                }, null, 0f);
                                                                this.targetSelectMgr.SetTargetNull();
                                                                break;
                                                            }
                                                            this.playerAutoAttack.AutoClickSkill(autoFightCfg2.skillHolder);
                                                            autoFightCfg2.timer = 0f;
                                                            if (!autoFightCfg2.isNormalAttack)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            IL_5E1:;
            }
        }
        this.checkTimer = 0f;
    }

    private Vector2 GetNearPoint(Vector2 attackTargetPos, float distance)
    {
        if (distance < 3f)
        {
            return attackTargetPos;
        }
        Vector2 normalized = (MainPlayer.Self.CurrServerPos - attackTargetPos).normalized;
        Vector2 b = attackTargetPos + normalized * distance;
        return Vector2.Lerp(attackTargetPos, b, 0.9f);
    }

    private Vector2 GetRandomPosInPoint(Vector2 original)
    {
        Vector2 result = default(Vector2);
        result.x = original.x;
        result.y = original.y;
        result.x += UnityEngine.Random.Range(-30f, 30f);
        result.y += UnityEngine.Random.Range(-30f, 30f);
        return result;
    }

    private bool CheckBlock(Vector2 other)
    {
        if (MainPlayer.Self == null)
        {
            return false;
        }
        if (MainPlayer.Self.Pfc == null)
        {
            return false;
        }
        Point start = new Point((int)MainPlayer.Self.CurrServerPos.x, (int)MainPlayer.Self.CurrServerPos.y);
        Point end = new Point((int)other.x, (int)other.y);
        return MainPlayer.Self.Pfc.CheckBlockBetweenPoint(start, end);
    }

    private void MoveToTarget(Vector2 serverPos)
    {
        if (this.CheckBlock(serverPos))
        {
            serverPos = this.GetRandomPosInPoint(serverPos);
        }
        MainPlayer.Self.Pfc.BeginFindPath(serverPos, PathFindComponent.AutoMoveState.MoveToPointByServerPos, null, null);
    }

    public void OnServerSettingBack(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            this.autoFightSettings.Clear();
            MyJson.JsonNode_Object jsonNode_Object = MyJson.Parse(value) as MyJson.JsonNode_Object;
            this.isAutoPick = (jsonNode_Object[this.autoPickJsonKey] as MyJson.JsonNode_ValueNumber);
            this.autoFightSettings = this.InitCfgTypeListByJsonData(jsonNode_Object);
        }
        else
        {
            this.isAutoPick = true;
            this.autoFightSettings = this.InitDefaultCfgList();
        }
        this.InitPropCdData();
        this.InitSkillCdData();
        this.isHaveSettingData = true;
    }

    public void InitPropCdData()
    {
        if (this.autoFightSettings == null)
        {
            return;
        }
        for (int i = 0; i < this.autoFightSettings.Count; i++)
        {
            if (this.autoFightSettings[i].cfgType == AutoFightCfgType.Prop)
            {
                this.autoFightSettings[i].cdDuaration = (this.autoFightSettings[i].timer = this.shortCutsController.GetBindPropCdDuration(this.autoFightSettings[i].keyType));
            }
        }
    }

    public void InitSkillCdData()
    {
        if (this.autoFightSettings == null)
        {
            return;
        }
        ShortcutkeyFunctionType commonAttack = this.GetCommonAttack();
        for (int i = 0; i < this.autoFightSettings.Count; i++)
        {
            if (this.autoFightSettings[i].isEnable)
            {
                if (this.autoFightSettings[i].cfgType != AutoFightCfgType.Prop)
                {
                    if (this.autoFightSettings[i].keyType >= ShortcutkeyFunctionType.ExtendSkill_1 && this.autoFightSettings[i].keyType <= ShortcutkeyFunctionType.ExtendSkill_12)
                    {
                        uint index = ((uint)autoFightSettings[i].keyType % 100 - (uint)ShortcutkeyFunctionType.UnLockSkills);
                        uint slotSkillID = this.skillViewController.GetSlotSkillID(index);
                        if (slotSkillID > 0U)
                        {
                            this.autoFightSettings[i].skillID = slotSkillID;
                            this.autoFightSettings[i].cdDuaration = (this.autoFightSettings[i].timer = this.skillViewController.GetSlotSkillCdByID(slotSkillID) / 1000f);
                            this.autoFightSettings[i].skillHolder = this.skillViewController.GetSkillBase(slotSkillID);
                            this.autoFightSettings[i].isNormalAttack = (commonAttack == this.autoFightSettings[i].keyType);
                        }
                        else
                        {
                            this.autoFightSettings[i].cdDuaration = -1f;
                            this.autoFightSettings[i].skillID = 0U;
                            this.autoFightSettings[i].skillHolder = null;
                        }
                        if (this.autoFightSettings[i].skillHolder != null)
                        {
                            MainPlayerSkillBase skillHolder = this.autoFightSettings[i].skillHolder;
                            float[] skillRangeParam = skillHolder.GetSkillRangeParam(skillHolder.SkillConfig.GetField_String("SkillRange"));
                            this.autoFightSettings[i].attackDistance = ((skillRangeParam[1] <= 0.1f) ? 1f : skillRangeParam[1]);
                        }
                    }
                }
            }
        }
    }

    public List<AutoFightCfg> InitCfgTypeListByJsonData(MyJson.JsonNode_Object jCfg)
    {
        List<AutoFightCfg> list = new List<AutoFightCfg>();
        MyJson.JsonNode_Array jsonNode_Array = jCfg[this.skillsJsonKey] as MyJson.JsonNode_Array;
        for (int i = 0; i < jsonNode_Array.Count; i++)
        {
            list.Add(this.JObjToCfgObj(jsonNode_Array[i] as MyJson.JsonNode_Object));
        }
        return list;
    }

    public ShortcutkeyFunctionType GetCommonAttack()
    {
        uint normalSkillID = MainPlayerSkillHolder.Instance.GetNormalSkillID();
        uint slotIndexBySkillID = this.skillViewController.GetSlotIndexBySkillID(normalSkillID);
        if (slotIndexBySkillID != 0U)
        {
            return (ShortcutkeyFunctionType)500 + (int)slotIndexBySkillID;
        }
        return ShortcutkeyFunctionType.ExtendSkill_1;
    }

    public AutoFightCfg JObjToCfgObj(MyJson.JsonNode_Object jObj)
    {
        return new AutoFightCfg
        {
            isEnable = (jObj["isEnable"] as MyJson.JsonNode_ValueNumber),
            percent = (jObj["percent"] as MyJson.JsonNode_ValueNumber),
            keyType = (ShortcutkeyFunctionType)((int)(jObj["keyType"] as MyJson.JsonNode_ValueNumber)),
            cfgType = (AutoFightCfgType)((int)(jObj["cfgType"] as MyJson.JsonNode_ValueNumber))
        };
    }

    public List<AutoFightCfg> InitDefaultCfgList()
    {
        List<AutoFightCfg> list = new List<AutoFightCfg>();
        AutoFightCfg autoFightCfg = new AutoFightCfg();
        autoFightCfg.isEnable = true;
        autoFightCfg.keyType = this.GetCommonAttack();
        AutoFightCfg item = new AutoFightCfg();
        AutoFightCfg item2 = new AutoFightCfg();
        AutoFightCfg item3 = new AutoFightCfg();
        AutoFightCfg autoFightCfg2 = new AutoFightCfg();
        autoFightCfg2.cfgType = AutoFightCfgType.Prop;
        autoFightCfg2.percent = 30;
        autoFightCfg2.isEnable = true;
        AutoFightCfg autoFightCfg3 = new AutoFightCfg();
        autoFightCfg3.percent = 50;
        AutoFightCfg autoFightCfg4 = new AutoFightCfg();
        autoFightCfg4.percent = 50;
        AutoFightCfg autoFightCfg5 = new AutoFightCfg();
        autoFightCfg5.percent = 50;
        autoFightCfg5.cfgType = AutoFightCfgType.Prop;
        AutoFightCfg autoFightCfg6 = new AutoFightCfg();
        autoFightCfg6.percent = 50;
        autoFightCfg6.cfgType = AutoFightCfgType.Prop;
        list.Add(autoFightCfg);
        list.Add(item);
        list.Add(item2);
        list.Add(item3);
        list.Add(autoFightCfg2);
        list.Add(autoFightCfg3);
        list.Add(autoFightCfg4);
        list.Add(autoFightCfg5);
        list.Add(autoFightCfg6);
        return list;
    }

    private void SetAutoFightTopText(bool state)
    {
        if (MainPlayer.Self != null)
        {
            GameObject gameObject = MainPlayer.Self.hpdata.InitAutoFightText();
            if (!gameObject)
            {
                return;
            }
            gameObject.gameObject.SetActive(state);
        }
    }

    private void ResetData()
    {
        this._isOpenAutoFight = false;
        this._targetSelectMgr = null;
        this._entitiesManager = null;
        this._playerAutoAttack = null;
    }

    private void InitRecoveryLifeCfg()
    {
        if (this.isInitLifeRecoveryCfg)
        {
            return;
        }
        if (this.lifeRecoveryCfgDic.Count == 0)
        {
            LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("drug");
            if (xmlConfigTable != null)
            {
                LuaTable field_Table = xmlConfigTable.GetField_Table("drugitem");
                IEnumerator enumerator = field_Table.Values.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    LuaTable luaTable = obj as LuaTable;
                    uint field_Uint = luaTable.GetField_Uint("id");
                    uint field_Uint2 = luaTable.GetField_Uint("content");
                    this.lifeRecoveryCfgDic[field_Uint] = field_Uint2;
                }
            }
        }
        this.isInitLifeRecoveryCfg = true;
    }

    public void InitMaxRecoveryPropData()
    {
        if (!this.isOpenAutoFight)
        {
            return;
        }
        this.InitRecoveryLifeCfg();
        this.propId = 0U;
        this.leftCount = 0U;
        uint num = 0U;
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        int selfCreateHeroLevel = controller.GetSelfCreateHeroLevel();
        foreach (KeyValuePair<uint, uint> keyValuePair in this.lifeRecoveryCfgDic)
        {
            object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                keyValuePair.Key
            });
            uint num2 = 0U;
            if (array != null && array.Length > 0)
            {
                uint.TryParse(array[0].ToString(), out num2);
            }
            if (num2 > 0U)
            {
                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)keyValuePair.Key);
                if ((long)selfCreateHeroLevel >= (long)((ulong)configTable.GetCacheField_Uint("uselevel")))
                {
                    if (keyValuePair.Value > num)
                    {
                        num = keyValuePair.Value;
                        this.propId = keyValuePair.Key;
                        this.leftCount = num2;
                    }
                }
            }
        }
        if (this.propId != 0U && this.speIndex4PropCall < this.autoFightSettings.Count)
        {
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("objects", (ulong)this.propId);
            if (configTable2 != null)
            {
                this.autoFightSettings[this.speIndex4PropCall].cdDuaration = configTable2.GetCacheField_Uint("cdtime");
            }
        }
    }

    private bool isHaveSettingData;

    private bool isAutoPick = true;

    private List<AutoFightCfg> autoFightSettings = new List<AutoFightCfg>();

    private MainPlayerTargetSelectMgr _targetSelectMgr;

    private UI_ShortcutControl mShortControl;

    private EntitiesManager _entitiesManager;

    private bool _isOpenAutoFight;

    private CardController mcardController;

    private ShortcutsConfigController shortCutsController;

    private PickDropController pickController;

    private SkillViewControll skillViewController;

    private string autoPickJsonKey = "auto_pick";

    private string skillsJsonKey = "skill_settings";

    private AutoAttack _playerAutoAttack;

    private float checkInterval = 0.5f;

    private float checkTimer;

    private float attackCommonDist = 18f;

    private Queue<ulong> blackBagList = new Queue<ulong>();

    private int speIndex4PropCall = 4;

    private bool isInitLifeRecoveryCfg;

    private Dictionary<uint, uint> lifeRecoveryCfgDic = new Dictionary<uint, uint>();

    private uint leftCount;

    private uint propId;
}
