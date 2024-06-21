using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using career;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using Models;

public class SkillViewControll : ControllerBase
{
    public UI_SkillView uiSkillView
    {
        get
        {
            return UIManager.GetUIObject<UI_SkillView>();
        }
    }

    public override void Awake()
    {
        this.skillViewNetWork = new SkillViewNetWork();
        this.skillViewNetWork.Initialize();
    }

    public void InitSkillSlotMap()
    {
        if (this.dicEquipSkill == null || 0 >= this.dicEquipSkill.Count)
        {
            FFDebug.LogError(this, "Init Skill Slot Map Faild.");
        }
        else
        {
            this.InitSkillSlotMapToCareerSkillData();
        }
    }

    public void InitSkillSlotMapToLuaData()
    {
        this.SkillSlotMap.Clear();
        string herothisid = MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid;
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.GetHeroSkils", new object[]
        {
            herothisid
        });
        if (array.Length > 0)
        {
            LuaTable luaTable = array[0] as LuaTable;
            IEnumerator enumerator = luaTable.Values.GetEnumerator();
            enumerator.Reset();
            uint num = 1U;
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                LuaTable luaTable2 = obj as LuaTable;
                uint field_Uint = luaTable2.GetField_Uint("skillbaseid");
                if (field_Uint > 0U)
                {
                    LuaTable luaTable3 = MainPlayerSkillHolder.Instance.Getskill_lv_config(field_Uint, 1U);
                    if (luaTable3 != null)
                    {
                        if (luaTable3.GetField_Uint("canbe_passive") == 0U)
                        {
                            this.SkillSlotMap[field_Uint] = num++;
                        }
                    }
                }
            }
        }
    }

    public void InitSkillSlotMapToCareerSkillData()
    {
        uint num = 1U;
        for (int i = 0; i < this.dicEquipSkill.Count; i++)
        {
            uint skillid = this.dicEquipSkill[i].skill.skillid;
            uint level = this.dicEquipSkill[i].skill.level;
            LuaTable luaTable = MainPlayerSkillHolder.Instance.Getskill_lv_config(skillid, level);
            if (luaTable != null)
            {
                if (luaTable.GetField_Uint("canbe_passive") == 0U)
                {
                    this.SkillSlotMap[skillid] = num++;
                }
            }
        }
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "SkillView_CONTROLLER";
        }
    }

    public void RetUpGradeSkill_SC()
    {
        if (this.uiSkillView != null)
        {
            this.uiSkillView.OnStudySkillSuccess();
        }
    }

    public void ReqUpGradeSkill(uint id)
    {
        this.skillViewNetWork.ReqUpGradeSkill(id);
    }

    public void EnterSkillView()
    {
        this.curCareer = (Profession)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_SkillView>("UI_Skill", delegate ()
        {
            if (this.uiSkillView != null)
            {
                this.uiSkillView.mRoot.gameObject.SetActive(false);
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void ReqOpenSkillView()
    {
    }

    public void SetSkillData(CareerSkillInfo data)
    {
        this.curCareer = (Profession)data.curcareer;
        this.dicEquipSkill.Clear();
        data.unlockskills.ForEach(delegate (careerunlockItem _skill)
        {
            if (_skill != null && _skill.skill != null)
            {
                this.dicEquipSkill.Add(_skill);
            }
            else
            {
                string str = string.Format("unlockskills[i].skill null ,please check hero, skill config...", new object[0]);
                Debugger.LogError(str, new object[0]);
            }
        });
        ControllerManager.Instance.GetController<MainUIController>().RefreshMainPlayerJob(1U, data.curcareer);
        if (MainPlayer.Self != null)
        {
            MainPlayerSkillHolder component = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
            if (component != null)
            {
                this.curCareer = (Profession)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
                component.LoadMainPlayerSkill(false);
                this.LoadSkillSlot();
            }
        }
        if (this.uiSkillView != null)
        {
            this.uiSkillView.StartSkillView();
        }
        if (this.mLockSkillCtrl != null)
        {
            this.mLockSkillCtrl.RefreshSkillServerDataMap();
        }
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null)
        {
            ControllerManager.Instance.GetController<MainUIController>().mainView.InitSkillTexture(false);
        }
        this.CheckIsUnlockNewSKills(data.curskills);
    }

    private void CheckIsUnlockNewSKills(List<lineSkillItem> currentSkills)
    {
        if (currentSkills == null || currentSkills.Count < 1)
        {
            return;
        }
        if (this.lstLastSkills == null)
        {
            this.lstLastSkills = new List<uint>();
            for (int i = 0; i < currentSkills.Count; i++)
            {
                this.lstLastSkills.Add(currentSkills[i].skill);
            }
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        for (int j = 0; j < currentSkills.Count; j++)
        {
            lineSkillItem lineSkillItem = currentSkills[j];
            if (!this.lstLastSkills.Contains(lineSkillItem.skill))
            {
                this.lstLastSkills.Add(lineSkillItem.skill);
                if (controller != null && controller.mainView != null)
                {
                    controller.mainView.UnlockSkill(lineSkillItem.skill);
                }
            }
        }
    }

    public void CloseSkillView()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Skill");
    }

    public UnLockSkillsController mLockSkillCtrl
    {
        get
        {
            if (this.m_lockSkillCtrl == null)
            {
                this.m_lockSkillCtrl = ControllerManager.Instance.GetController<UnLockSkillsController>();
            }
            return this.m_lockSkillCtrl;
        }
    }

    public void InitSkillSlotData(bool force)
    {
        if (this.m_slotModel == null || force)
        {
            this.InitSlotModel();
        }
    }

    public void ReqSkillUIIndexDataCb(uint retcode, string _value)
    {
        if (retcode == 1U || string.IsNullOrEmpty(_value))
        {
            FFDebug.LogWarning(this, "SetSkillByServerData CodeType:" + (SkillViewControll.CodeType)retcode);
            this.SetSkillByServerData();
        }
        else
        {
            this.InitSkillSlotData(false);
            this.m_slotModel = this.String2SkillSlotModel(_value);
        }
        if (!this.ComparerServerSkill(this.m_slotModel))
        {
            FFDebug.Log(this, FFLogType.Skill, "ComparerServerSkill");
            this.SetSkillByServerData();
        }
        this.RefreshSkillSlotInfo(this.m_slotModel);
        this.RefreshEvolutionSkillSlotInfo(this.m_slotModel);
        if (this.mLockSkillCtrl != null)
        {
            this.mLockSkillCtrl.HandleNewSkill(this.m_slotModel);
        }
    }

    public uint GetSlotSkillID(uint index)
    {
        if (this.m_slotModel != null && this.m_slotModel.m_slots != null && (ulong)index < (ulong)((long)this.m_slotModel.m_slots.Length))
        {
            return this.m_slotModel.m_slots[(int)((UIntPtr)index)].m_skillId;
        }
        return 0U;
    }

    public uint GetSlotIndexBySkillID(uint skillID)
    {
        if (this.m_slotModel != null && this.m_slotModel.m_slots != null)
        {
            for (int i = 0; i < this.m_slotModel.m_slots.Length; i++)
            {
                if (this.m_slotModel.m_slots[i].m_skillId == skillID)
                {
                    return this.m_slotModel.m_slots[i].m_slotIndex;
                }
            }
        }
        return 0U;
    }

    public float GetSlotSkillCdByID(uint skillID)
    {
        MainPlayerSkillBase mainPlayerSkillBase;
        if (MainPlayerSkillHolder.Instance.MainPlayerSkillList.TryGetValue(skillID, out mainPlayerSkillBase))
        {
            return mainPlayerSkillBase.CDLength;
        }
        return -1f;
    }

    public MainPlayerSkillBase GetSkillBase(uint skillID)
    {
        MainPlayerSkillBase result;
        if (MainPlayerSkillHolder.Instance.MainPlayerSkillList.TryGetValue(skillID, out result))
        {
            return result;
        }
        return null;
    }

    public int GetSkillTypeByID(uint skillID)
    {
        return 0;
    }

    private void SetSkillByServerData()
    {
        this.InitSkillSlotData(true);
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (!manager.isAbattoirScene)
        {
            this.m_slotModel.herothisid = MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid;
        }
        else
        {
            this.m_slotModel.herothisid = "0";
        }
        this.SetSkillSlotDataByLocal();
    }

    public void ReBindShortcutEvent()
    {
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null == uiobject)
        {
            return;
        }
        for (int i = 0; i < uiobject.SkillButtonList.Count; i++)
        {
            uiobject.SkillButtonList[i].BindShortcutEvent();
        }
    }

    private void RefreshSkillSlotInfo(SkillSlotModel _model = null)
    {
        if (_model == null)
        {
            return;
        }
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null == uiobject)
        {
            return;
        }
        for (int i = 0; i < _model.m_slots.Length; i++)
        {
            uiobject.SkillButtonList[i].skillId = (int)_model.m_slots[i].m_skillId;
            uiobject.SkillButtonList[i].SetSkillCdUIState();
            uiobject.SkillButtonList[i].LoadTexture();
        }
    }

    public void RefreshEvolutionSkillSlotInfo(SkillSlotModel _model = null)
    {
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null == uiobject)
        {
            return;
        }
        int i = 0;
        MainPlayerSkillHolder instance = MainPlayerSkillHolder.Instance;
        List<int> list = new List<int>();
        foreach (KeyValuePair<uint, uint> keyValuePair in instance.mainPlayerEquipSkill)
        {
            LuaTable luaTable = instance.Getskill_lv_config(keyValuePair.Value, instance.MainPlayerSkillList[keyValuePair.Value].Level);
            if (luaTable != null)
            {
                SkillRelaseType field_Uint = (SkillRelaseType)luaTable.GetField_Uint("releasetype");
                if (field_Uint == SkillRelaseType.SkillEvolution)
                {
                    list.Add((int)keyValuePair.Value);
                    i++;
                }
            }
        }
        list.Sort();
        for (int j = 0; j < list.Count; j++)
        {
            uiobject.EvolutionButtonList[j].gameObject.SetActive(true);
            uiobject.EvolutionBackGo[j].gameObject.SetActive(true);
            uiobject.EvolutionButtonList[j].skillId = list[j];
            uiobject.EvolutionButtonList[j].SetSkillCdUIState();
            uiobject.EvolutionButtonList[j].LoadTexture();
        }
        while (i < uiobject.EvolutionButtonList.Count)
        {
            uiobject.EvolutionButtonList[i].gameObject.SetActive(false);
            uiobject.EvolutionBackGo[i].gameObject.SetActive(false);
            i++;
        }
        this.ShowCurrSelectEvolution();
    }

    public void ShowCurrSelectEvolution()
    {
        FashionController controller = ControllerManager.Instance.GetController<FashionController>();
        if (controller == null)
        {
            return;
        }
        if (controller.AllFasion == null)
        {
            return;
        }
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null == uiobject)
        {
            return;
        }
        ulong tranSkillid = controller.AllFasion.tranSkillid;
        if (tranSkillid == 0UL)
        {
            ulong num = (ulong)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.heroid;
            LuaTable configTable = LuaConfigManager.GetConfigTable("heros", (ulong)((uint)num));
            if (configTable != null)
            {
                string cacheField_String = configTable.GetCacheField_String("exskill");
                if (!string.IsNullOrEmpty(cacheField_String))
                {
                    string[] array = cacheField_String.Split(new char[]
                    {
                        '|'
                    });
                    if (array.Length > 0)
                    {
                        ulong.TryParse(array[0], out tranSkillid);
                    }
                }
            }
        }
        for (int i = 0; i < uiobject.EvolutionButtonList.Count; i++)
        {
            uiobject.EvolutionButtonList[i].ShowCurrEvoution(tranSkillid > 0UL && (long)uiobject.EvolutionButtonList[i].skillId == (long)tranSkillid);
        }
    }

    private bool ComparerServerSkill(SkillSlotModel _model)
    {
        if (_model != null && _model.m_slots != null && MainPlayer.Self != null)
        {
            MainPlayerSkillHolder instance = MainPlayerSkillHolder.Instance;
            List<uint> list = new List<uint>(instance.mainPlayerEquipSkill.Values);
            for (int i = 0; i < list.Count; i++)
            {
                LuaTable luaTable = instance.Getskill_lv_config(list[i], instance.MainPlayerSkillList[list[i]].Level);
                if (luaTable != null)
                {
                    SkillRelaseType field_Uint = (SkillRelaseType)luaTable.GetField_Uint("releasetype");
                    if (field_Uint != SkillRelaseType.SkillEvolution)
                    {
                        bool flag = false;
                        for (int j = 0; j < _model.m_slots.Length; j++)
                        {
                            if (list[i] == _model.m_slots[j].m_skillId)
                            {
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

    private void InitSlotModel()
    {
        this.m_slotModel = new SkillSlotModel();
        this.m_slotModel.herothisid = string.Empty;
        int num = 12;
        this.m_slotModel.m_slots = new SlotData[num];
        for (int i = 1; i <= num; i++)
        {
            SlotData slotData = new SlotData();
            slotData.m_slotIndex = (uint)i;
            slotData.m_skillId = 0U;
            this.m_slotModel.m_slots[i - 1] = slotData;
        }
    }

    private bool SetSkillSlotDataByLocal()
    {
        List<uint> list = new List<uint>(MainPlayerSkillHolder.Instance.mainPlayerEquipSkill.Values);
        list.Sort(delegate (uint a, uint b)
        {
            uint num = a % 100U;
            uint value = b % 100U;
            return num.CompareTo(value);
        });
        bool result = false;
        if (list != null && 0 < list.Count)
        {
            MainPlayerSkillHolder instance = MainPlayerSkillHolder.Instance;
            for (int i = 0; i < list.Count; i++)
            {
                LuaTable luaTable = instance.Getskill_lv_config(list[i], instance.MainPlayerSkillList[list[i]].Level);
                if (luaTable != null)
                {
                    SkillRelaseType field_Uint = (SkillRelaseType)luaTable.GetField_Uint("releasetype");
                    if (field_Uint != SkillRelaseType.SkillEvolution)
                    {
                        if (!this.Contains(list[i]))
                        {
                            int slotArrayIndex = this.GetSlotArrayIndex(this.FindSameSkill(list[i]));
                            if (slotArrayIndex < this.m_slotModel.m_slots.Length)
                            {
                                this.m_slotModel.m_slots[slotArrayIndex].m_skillId = list[i];
                                result = true;
                            }
                            else
                            {
                                FFDebug.LogError(this, "超出数组范围！");
                            }
                        }
                    }
                }
            }
        }
        return result;
    }

    private bool Contains(uint _skillId)
    {
        bool result = false;
        if (this.m_slotModel.m_slots != null)
        {
            for (int i = 0; i < this.m_slotModel.m_slots.Length; i++)
            {
                if (this.m_slotModel.m_slots[i].m_skillId == _skillId)
                {
                    result = true;
                }
            }
        }
        return result;
    }

    private uint FindSameSkill(uint _skillid)
    {
        if (this.m_slotModel.m_slots != null)
        {
            for (int i = 0; i < this.m_slotModel.m_slots.Length; i++)
            {
                uint num = _skillid % 100U;
                uint num2 = this.m_slotModel.m_slots[i].m_skillId % 100U;
                if (num == num2)
                {
                    return this.m_slotModel.m_slots[i].m_skillId;
                }
            }
        }
        return 0U;
    }

    private int GetSlotArrayIndex(uint _id)
    {
        if (this.m_slotModel.m_slots != null)
        {
            for (int i = 0; i < this.m_slotModel.m_slots.Length; i++)
            {
                if (this.m_slotModel.m_slots[i].m_skillId == _id)
                {
                    return i;
                }
            }
        }
        return int.MaxValue;
    }

    public void CombinSkillIndexData()
    {
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null == uiobject)
        {
            return;
        }
        List<SkillButton> skillButtonList = uiobject.SkillButtonList;
        int count = skillButtonList.Count;
        SkillSlotModel skillSlotModel = new SkillSlotModel();
        skillSlotModel.m_slots = new SlotData[count];
        for (int i = 0; i < count; i++)
        {
            skillButtonList[i].skillId = ((skillButtonList[i].SkillData != null) ? skillButtonList[i].skillId : 0);
            SlotData slotData = new SlotData();
            slotData.m_slotIndex = (uint)skillButtonList[i].BtnIndexId;
            slotData.m_skillId = (uint)skillButtonList[i].skillId;
            skillSlotModel.m_slots[i] = slotData;
        }
        string content = this.SkillSlotModel2String(skillSlotModel);
        ServerStorageManager.Instance.AddUpdateData(this.skillslotkey, content, 0U);
    }

    private string SkillSlotModel2String(SkillSlotModel _model)
    {
        StringBuilder stringBuilder = new StringBuilder();
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (!manager.isAbattoirScene)
        {
            stringBuilder.Append(MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid);
        }
        else
        {
            stringBuilder.Append("0");
        }
        stringBuilder.Append("&");
        for (int i = 0; i < _model.m_slots.Length; i++)
        {
            stringBuilder.Append(_model.m_slots[i].m_slotIndex);
            stringBuilder.Append(":");
            stringBuilder.Append(_model.m_slots[i].m_skillId);
            if (i < _model.m_slots.Length - 1)
            {
                stringBuilder.Append("|");
            }
        }
        return stringBuilder.ToString();
    }

    private SkillSlotModel String2SkillSlotModel(string _str)
    {
        SkillSlotModel skillSlotModel = null;
        if (!string.IsNullOrEmpty(_str))
        {
            string[] array = _str.Split(new char[]
            {
                '&'
            });
            if (array.Length != 2)
            {
                FFDebug.LogError(this, "skill slot data error...");
                return skillSlotModel;
            }
            skillSlotModel = new SkillSlotModel();
            skillSlotModel.herothisid = array[0];
            string[] array2 = array[1].Split(new char[]
            {
                '|'
            });
            if (array2.Length >= 12)
            {
                skillSlotModel.m_slots = new SlotData[array2.Length];
                for (int i = 0; i < array2.Length; i++)
                {
                    if (!string.IsNullOrEmpty(array2[i]))
                    {
                        string[] array3 = array2[i].Split(new char[]
                        {
                            ':'
                        });
                        SlotData slotData = new SlotData();
                        slotData.m_slotIndex = uint.Parse(array3[0].ToString());
                        slotData.m_skillId = uint.Parse(array3[1].ToString());
                        skillSlotModel.m_slots[i] = slotData;
                    }
                }
            }
        }
        return skillSlotModel;
    }

    public string skillslotkey
    {
        get
        {
            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
            string str = "0";
            if (!manager.isAbattoirScene && MainPlayer.Self != null)
            {
                str = MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid;
            }
            return ServerStorageKey.SkillSlotSort.ToString() + str;
        }
    }

    public void LoadSkillSlot()
    {
        if (this.m_slotModel == null)
        {
            FFDebug.LogError(this, "slot model null...");
            return;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            if (this.m_slotModel.herothisid != "0")
            {
                ServerStorageManager.Instance.GetData(this.skillslotkey, 0U);
                return;
            }
        }
        else if (this.m_slotModel.herothisid != MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid)
        {
            ServerStorageManager.Instance.GetData(this.skillslotkey, 0U);
            return;
        }
        if (!manager.isAbattoirScene && this.SetSkillSlotDataByLocal())
        {
            string content = this.SkillSlotModel2String(this.m_slotModel);
            ServerStorageManager.Instance.AddUpdateData(this.skillslotkey, content, 0U);
        }
    }

    public Dictionary<uint, uint> SkillSlotMap = new Dictionary<uint, uint>();

    public List<careerunlockItem> dicEquipSkill = new List<careerunlockItem>();

    public List<jobskillLevelAndExp> listjobLevel = new List<jobskillLevelAndExp>();

    public Dictionary<Profession, jobskillLevelAndExp> dicprofessLevel = new Dictionary<Profession, jobskillLevelAndExp>();

    public Dictionary<uint, List<ExtSkillData>> DicExtSkills = new Dictionary<uint, List<ExtSkillData>>();

    private List<uint> lstLastSkills;

    private SkillViewNetWork skillViewNetWork;

    public Profession curCareer;

    private UnLockSkillsController m_lockSkillCtrl;

    private SkillSlotModel m_slotModel;

    public enum CodeType
    {
        SUCCESS,
        ERROR
    }
}
