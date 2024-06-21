using System;
using System.Collections.Generic;
using career;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;

public class UnLockSkillsController : ControllerBase
{
    public UI_UnLockSkills skillView
    {
        get
        {
            return UIManager.GetUIObject<UI_UnLockSkills>();
        }
    }

    public List<careerunlockItem> dicEquipSkill
    {
        get
        {
            return ControllerManager.Instance.GetController<SkillViewControll>().dicEquipSkill;
        }
    }

    public override string ControllerName
    {
        get
        {
            return "unlockskills_controller";
        }
    }

    public List<SkillDataMapInfo> skillMapList
    {
        get
        {
            return this.skillMap;
        }
    }

    public SkillDataMapInfo CurSkillMap
    {
        get
        {
            return this.m_curSkillMap;
        }
        set
        {
            this.m_curSkillMap = value;
            this.OnRefreshViewInfo();
        }
    }

    public uint CurrSkillId
    {
        get
        {
            return this.currSkillId;
        }
        set
        {
            this.currSkillId = value;
        }
    }

    public uint CurrSkillLevel
    {
        get
        {
            return this.currSkillLevel;
        }
        set
        {
            this.currSkillLevel = value;
        }
    }

    public override void Awake()
    {
        this._network = new UnLockSkillNetWork();
        this._network.Initialize();
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenSkillLevelUp));
    }

    public void OpenSkillLevelUp(List<VarType> varParams)
    {
        UI_UnLockSkills uiobject = UIManager.GetUIObject<UI_UnLockSkills>();
        if (uiobject != null && this.manualType == UnLockSkillsController.ManualType.Self)
        {
            UIManager.Instance.DeleteUI<UI_UnLockSkills>();
            Scheduler.Instance.AddFrame(20U, false, delegate
            {
                this.OpenFrame(UnLockSkillsController.ManualType.Npc);
            });
        }
        else
        {
            this.OpenFrame(UnLockSkillsController.ManualType.Npc);
        }
    }

    public void OpenFrame(UnLockSkillsController.ManualType _type = UnLockSkillsController.ManualType.Self)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            TipsWindow.ShowWindow(5001U);
            return;
        }
        this.m_manualType = _type;
        UIManager.Instance.ShowUI<UI_UnLockSkills>("UI_UnLockSkills", delegate ()
        {
            UI_UnLockSkills uiobject = UIManager.GetUIObject<UI_UnLockSkills>();
            uiobject.uiPanelRoot.SetAsLastSibling();
            if (_type == UnLockSkillsController.ManualType.Npc)
            {
                uiobject.RegOpenUIByNpc(string.Empty);
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseFrame()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_UnLockSkills");
    }

    public void RefreshSkillServerDataMap()
    {
        this.skillMap.Clear();
        for (int i = 0; i < this.dicEquipSkill.Count; i++)
        {
            SkillDataMapInfo skillDataMapInfo = new SkillDataMapInfo();
            skillDataMapInfo.m_skillId = this.dicEquipSkill[i].skill.skillid;
            skillDataMapInfo.m_skillLv = this.dicEquipSkill[i].skill.level;
            LuaTable config = this.GetConfig(skillDataMapInfo.m_skillId, skillDataMapInfo.m_skillLv);
            this.skillMap.Add(skillDataMapInfo);
        }
        this.skillMap.Sort(delegate (SkillDataMapInfo data1, SkillDataMapInfo data2)
        {
            uint num = data1.m_skillId % 100U;
            uint value = data2.m_skillId % 100U;
            return num.CompareTo(value);
        });
        this.OnRefreshCurSkill();
        if (null != this.skillView)
        {
            this.skillView.LoadSkillItem();
        }
    }

    private SkillDataMapInfo FindSkill(uint _skillId)
    {
        for (int i = 0; i < this.skillMap.Count; i++)
        {
            if (_skillId == this.skillMap[i].m_skillId)
            {
                return this.skillMap[i];
            }
        }
        return null;
    }

    public void OnRefreshCurSkill()
    {
        if (this.skillMap == null || this.skillMap.Count < 1)
        {
            return;
        }
        if (this.currSkillId == 0U)
        {
            this.currSkillId = this.skillMap[0].m_skillId;
            this.currSkillLevel = this.skillMap[0].m_skillLv;
        }
        if (this.skillMap != null && this.skillMap.Count >= 1 && !this.OnSameHeroSkills(this.currSkillId, this.skillMap[0].m_skillId))
        {
            this.currSkillId = this.skillMap[0].m_skillId;
            this.currSkillLevel = this.skillMap[0].m_skillLv;
        }
        SkillDataMapInfo skillDataMapInfo = this.FindSameSkillMap(this.currSkillId);
        if (this.manualType == UnLockSkillsController.ManualType.Npc && this.IsMaxLv(this.currSkillId, this.currSkillLevel))
        {
            bool flag = false;
            for (int i = 0; i < this.skillMap.Count; i++)
            {
                if (!this.IsMaxLv(this.skillMap[i].m_skillId, this.skillMap[i].m_skillLv))
                {
                    skillDataMapInfo = this.skillMap[i];
                    flag = true;
                    break;
                }
            }
            skillDataMapInfo = ((!flag) ? null : skillDataMapInfo);
        }
        if (skillDataMapInfo != null)
        {
            this.currSkillId = skillDataMapInfo.m_skillId;
            this.CurrSkillLevel = skillDataMapInfo.m_skillLv;
        }
        this.OnRefreshViewInfo();
    }

    private List<LvCostItem> GetCostItem(uint _skillId, uint _lv)
    {
        LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Getlv_config((ulong)_skillId, _lv);
        string cacheField_String = luaTable.GetCacheField_String("levelupcost");
        string[] array = cacheField_String.Split(new char[]
        {
            ';'
        });
        int num = array.Length;
        List<LvCostItem> list = new List<LvCostItem>();
        if (num > 0)
        {
            for (int i = 0; i < num; i++)
            {
                string[] array2 = array[i].Split(new char[]
                {
                    '-'
                });
                if (array2.Length == 2)
                {
                    list.Add(new LvCostItem
                    {
                        m_id = uint.Parse(array2[0]),
                        m_num = uint.Parse(array2[1])
                    });
                }
            }
        }
        else
        {
            FFDebug.LogError(this, "item list is null...");
        }
        return list;
    }

    public List<LvCostItem> GetCostItemByType(UnLockSkillsController.ItemType _type, uint skillId, uint skillLv)
    {
        List<LvCostItem> costItem = this.GetCostItem(skillId, skillLv);
        List<LvCostItem> list = new List<LvCostItem>();
        if (costItem.Count > 0)
        {
            for (int i = 0; i < costItem.Count; i++)
            {
                if (_type == UnLockSkillsController.ItemType.Normal)
                {
                    if (costItem[i].m_id != 3U)
                    {
                        list.Add(costItem[i]);
                    }
                }
                else if (_type == UnLockSkillsController.ItemType.Gold && costItem[i].m_id == 3U)
                {
                    list.Add(costItem[i]);
                }
            }
        }
        return list;
    }

    public List<LvCostItem> GetCostItemByType(UnLockSkillsController.ItemType _type)
    {
        List<LvCostItem> costItem = this.GetCostItem(this.currSkillId, this.currSkillLevel);
        List<LvCostItem> list = new List<LvCostItem>();
        if (costItem.Count > 0)
        {
            for (int i = 0; i < costItem.Count; i++)
            {
                if (_type == UnLockSkillsController.ItemType.Normal)
                {
                    if (costItem[i].m_id != 3U)
                    {
                        list.Add(costItem[i]);
                    }
                }
                else if (_type == UnLockSkillsController.ItemType.Gold && costItem[i].m_id == 3U)
                {
                    list.Add(costItem[i]);
                }
            }
        }
        return list;
    }

    public string GetObjectIconName(uint _id)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)_id);
        return configTable.GetCacheField_String("icon");
    }

    public string GetCharacterName()
    {
        string result = string.Empty;
        if (LuaConfigManager.GetConfigTable("newUser", (ulong)MainPlayer.Self.OtherPlayerData.GetBaseOrHeroId()) == null)
        {
            result = MainPlayer.Self.OtherPlayerData.GetCharabaseConfig().GetField_String("name");
        }
        else
        {
            result = MainPlayer.Self.OtherPlayerData.MapUserData.name;
        }
        return result;
    }

    public uint GetPlayerLevel()
    {
        return ControllerManager.Instance.GetController<MainUIController>().GetCurLv();
    }

    public bool IsUnLockLevel(uint _skillId, uint _skilllv)
    {
        LuaTable config = this.GetConfig(_skillId, _skilllv);
        if (config == null)
        {
            return false;
        }
        uint field_Uint = config.GetField_Uint("unlocklevel");
        return this.GetPlayerLevel() >= field_Uint;
    }

    public uint GetLvupSkillId(uint skillId, uint skillLv)
    {
        return this.GetConfig(skillId, skillLv).GetField_Uint("nextskillid");
    }

    public bool IsEnoughGold()
    {
        uint num = this.GetCostItemByType(UnLockSkillsController.ItemType.Gold)[0].m_num;
        uint currencyByID = MainPlayer.Self.GetCurrencyByID(3U);
        return currencyByID >= num;
    }

    public bool IsEnoughGold(uint skillId, uint skillLv)
    {
        List<LvCostItem> costItemByType = this.GetCostItemByType(UnLockSkillsController.ItemType.Gold, skillId, skillLv);
        if (costItemByType.Count == 0)
        {
            return true;
        }
        uint num = costItemByType[0].m_num;
        uint currencyByID = MainPlayer.Self.GetCurrencyByID(3U);
        return currencyByID >= num;
    }

    public List<PropsBase> GetBagItemsData()
    {
        return MainPlayer.Self.MainPackageTableList();
    }

    public List<PropsBase> GetItemsByPackage(uint _itemId)
    {
        List<PropsBase> bagItemsData = this.GetBagItemsData();
        List<PropsBase> list = new List<PropsBase>();
        for (int i = 0; i < bagItemsData.Count; i++)
        {
            if (_itemId == bagItemsData[i]._obj.baseid)
            {
                list.Add(bagItemsData[i]);
            }
        }
        return list;
    }

    public bool IsEnoughItem(uint _itemId)
    {
        List<PropsBase> itemsByPackage = this.GetItemsByPackage(_itemId);
        if (itemsByPackage != null)
        {
            List<LvCostItem> costItemByType = this.GetCostItemByType(UnLockSkillsController.ItemType.Normal);
            uint num = 0U;
            for (int i = 0; i < itemsByPackage.Count; i++)
            {
                if (itemsByPackage[i]._obj.baseid == _itemId)
                {
                    num += itemsByPackage[i]._obj.num;
                }
            }
            for (int j = 0; j < costItemByType.Count; j++)
            {
                if (_itemId == costItemByType[j].m_id && num >= costItemByType[j].m_num)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsEnoughItem(uint _itemId, uint _num, uint skillId, uint skillLv)
    {
        List<PropsBase> itemsByPackage = this.GetItemsByPackage(_itemId);
        if (itemsByPackage != null)
        {
            uint num = 0U;
            for (int i = 0; i < itemsByPackage.Count; i++)
            {
                if (itemsByPackage[i]._obj.baseid == _itemId)
                {
                    num += itemsByPackage[i]._obj.num;
                }
            }
            if (num >= _num)
            {
                return true;
            }
        }
        return false;
    }

    public void OnRefreshViewInfo()
    {
        if (null != this.skillView)
        {
            this.skillView.UpdateSkill();
        }
    }

    public void OnSetCurrSkillID(uint _skillid)
    {
        SkillDataMapInfo skillDataMapInfo = this.FindSameSkillMap(_skillid);
        this.currSkillId = _skillid;
        this.currSkillLevel = ((skillDataMapInfo != null) ? skillDataMapInfo.m_skillLv : 1U);
        this.CurSkillMap = skillDataMapInfo;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        this.m_curSkillMap = null;
        this.tipsQueue.Clear();
    }

    public LuaTable GetConfig(uint _skillid, uint _lv)
    {
        return ManagerCenter.Instance.GetManager<SkillManager>().Getlv_config((ulong)_skillid, _lv);
    }

    public LuaTable GetConfig(uint _skillid)
    {
        return this.GetConfig(_skillid, this.currSkillLevel);
    }

    public LuaTable GetConfig()
    {
        return this.GetConfig(this.currSkillId, this.currSkillLevel);
    }

    public bool IsMaxLv(uint _skillid, uint _lv)
    {
        return 0U == this.GetNextSkillId(_skillid, _lv);
    }

    public uint GetNextSkillId(uint _skillid, uint _lv)
    {
        LuaTable config = this.GetConfig(_skillid, _lv);
        return config.GetField_Uint("nextskillid");
    }

    public void HandleNewSkill(SkillSlotModel slotModel)
    {
        List<uint> list = this.CalculateNewSkill(slotModel);
        if (list.Count > 0 && !this.IsExsitNormalSkill(list))
        {
            this.tipsQueue.Enqueue(list);
            this.ShowSkillTipsFrame();
        }
    }

    private List<uint> CalculateNewSkill(SkillSlotModel slotModel)
    {
        List<uint> list = new List<uint>();
        if (this.mpsStorageList != null && 0 < this.mpsStorageList.Count)
        {
            for (int i = 0; i < slotModel.m_slots.Length; i++)
            {
                uint skillId = slotModel.m_slots[i].m_skillId;
                if (skillId != 0U)
                {
                    bool flag = false;
                    for (int j = 0; j < this.mpsStorageList.Count; j++)
                    {
                        if (this.IsSameSkill(this.mpsStorageList[j], skillId))
                        {
                            flag = true;
                        }
                    }
                    if (!flag && !list.Contains(skillId))
                    {
                        list.Add(skillId);
                    }
                }
            }
            for (int k = 0; k < this.dicEquipSkill.Count; k++)
            {
                uint skillid = this.dicEquipSkill[k].skill.skillid;
                LuaTable luaTable = MainPlayerSkillHolder.Instance.Getskill_lv_config(skillid, 1U);
                if (luaTable != null)
                {
                    if (luaTable.GetField_Uint("canbe_passive") != 0U)
                    {
                        bool flag2 = false;
                        for (int l = 0; l < this.mpsStorageList.Count; l++)
                        {
                            if (this.IsSameSkill(this.mpsStorageList[l], skillid))
                            {
                                flag2 = true;
                            }
                        }
                        if (!flag2 && !list.Contains(skillid))
                        {
                            list.Add(skillid);
                        }
                    }
                }
            }
        }
        if (this.IsExsitNormalSkill(list))
        {
            this.mpsStorageList.Clear();
        }
        for (int m = 0; m < slotModel.m_slots.Length; m++)
        {
            this.mpsStorageList.Add(slotModel.m_slots[m].m_skillId);
        }
        for (int n = 0; n < this.dicEquipSkill.Count; n++)
        {
            LuaTable luaTable2 = MainPlayerSkillHolder.Instance.Getskill_lv_config(this.dicEquipSkill[n].skill.skillid, 1U);
            if (luaTable2 != null)
            {
                if (luaTable2.GetField_Uint("canbe_passive") != 0U)
                {
                    this.mpsStorageList.Add(this.dicEquipSkill[n].skill.skillid);
                }
            }
        }
        return list;
    }

    public bool IsSameSkill(uint _skillid1, uint _skillid2)
    {
        return _skillid1 % 100U == _skillid2 % 100U && this.OnSameHeroSkills(_skillid1, _skillid2);
    }

    public bool OnSameHeroSkills(uint _skillid1, uint _skillid2)
    {
        return _skillid1 / 10000U == _skillid2 / 10000U;
    }

    public SkillDataMapInfo FindSameSkillMap(uint _skillid)
    {
        for (int i = 0; i < this.skillMap.Count; i++)
        {
            if (this.IsSameSkill(_skillid, this.skillMap[i].m_skillId))
            {
                return this.skillMap[i];
            }
        }
        return null;
    }

    private bool IsExsitNormalSkill(List<uint> skilllist)
    {
        uint num = skilllist.Find((uint id) => id % 100U == 1U);
        return num != 0U;
    }

    private void ShowSkillTipsFrame()
    {
        UI_UnLockSkillItemTips uiobject = UIManager.GetUIObject<UI_UnLockSkillItemTips>();
        if (null == uiobject)
        {
            this.LoadSkillTipsFrame();
        }
        else
        {
            if (this.tipsQueue.Count <= 0)
            {
                this.tipsQueue.Clear();
                return;
            }
            List<uint> skilllist = this.tipsQueue.Dequeue();
            uiobject.UpdateNewSkillItemTips(skilllist);
            this.ShowSkillTipsFrame();
        }
    }

    private void LoadSkillTipsFrame()
    {
        UIManager.Instance.ShowUI<UI_UnLockSkillItemTips>("UI_UnLockSkillsItemTips", delegate ()
        {
            this.ShowSkillTipsFrame();
        }, UIManager.ParentType.CommonUI, false);
    }

    public SkillButton FindTargetSkillBtnByID(uint _skillId)
    {
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        if (controller.SkillSlotMap.ContainsKey(_skillId))
        {
            UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
            if (null != mainView)
            {
                SkillButton skillButton = mainView.FindTargetSkillBtnByID(_skillId);
                if (null != skillButton)
                {
                    return skillButton;
                }
            }
        }
        return null;
    }

    public void ReqLevelUpSkill()
    {
        if (!this.IsMaxLv(this.currSkillId, this.currSkillLevel))
        {
            string herothisid = MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid;
            this._network.OnReqMSG_LevelupHeroSkill_CS(herothisid, this.currSkillId, this.currSkillLevel);
        }
    }

    public void RetLevelUpSkill(uint skillbaseid, uint skilllevel)
    {
    }

    public UnLockSkillsController.ManualType manualType
    {
        get
        {
            return this.m_manualType;
        }
    }

    public UnLockSkillNetWork _network;

    private List<SkillDataMapInfo> skillMap = new List<SkillDataMapInfo>();

    private SkillDataMapInfo m_curSkillMap;

    private uint currSkillId;

    private uint currSkillLevel;

    private Queue<List<uint>> tipsQueue = new Queue<List<uint>>();

    private List<uint> mpsStorageList = new List<uint>();

    private UnLockSkillsController.ManualType m_manualType;

    public enum ManualType
    {
        Self,
        Npc
    }

    public enum ItemType
    {
        Normal,
        Gold
    }
}
