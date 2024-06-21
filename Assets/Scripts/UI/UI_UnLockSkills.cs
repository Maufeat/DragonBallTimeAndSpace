using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnLockSkills : UIPanelBase
{
    private UnLockSkillsController lsCtl
    {
        get
        {
            if (this._lsctrl == null)
            {
                this._lsctrl = ControllerManager.Instance.GetController<UnLockSkillsController>();
            }
            return this._lsctrl;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitTextModel();
        this.InitPanel(root);
        this.InitEvent();
        this.InitCurSkill();
    }

    private void InitPanel(Transform root)
    {
        this.lsPanel = new UnlockSkillsPanel(root);
        if (null != this.lsPanel.tf_skillItem)
        {
            this.lsPanel.tf_skillItem.gameObject.SetActive(false);
        }
        if (null != this.lsPanel.tf_costItem)
        {
            this.lsPanel.tf_costItem.gameObject.SetActive(false);
        }
    }

    private void InitEvent()
    {
        Button component = this.lsPanel.tf_btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            UIManager.Instance.DeleteUI<UI_UnLockSkills>();
        });
        if (null != this.lsPanel.tf_nextskillLvup)
        {
            Button component2 = this.lsPanel.tf_nextskillLvup.GetComponent<Button>();
            component2.onClick.RemoveAllListeners();
            component2.onClick.AddListener(delegate ()
            {
                this.lsCtl.ReqLevelUpSkill();
            });
        }
        if (this.lsPanel.wayFind)
        {
            Button button = this.lsPanel.wayFind.GetComponent<Button>();
            if (!button)
            {
                button = this.lsPanel.wayFind.gameObject.AddComponent<Button>();
            }
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
                GlobalRegister.PathFindWithPathWayId(900008U);
                UIManager.Instance.DeleteUI<UI_UnLockSkills>();
            });
        }
    }

    private void InitTextModel()
    {
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        this.textModelDic.Add("2200171b", controller.GetContentByID("2200171b").Split(new char[]
        {
            ','
        }));
        this.textModelDic.Add("2200171c", controller.GetContentByID("2200171c").Split(new char[]
        {
            ','
        }));
        this.textModelDic.Add("180010", controller.GetContentByID("180010").Split(new char[]
        {
            ','
        }));
        this.textModelDic.Add("180011", controller.GetContentByID("180011").Split(new char[]
        {
            ','
        }));
        this.textModelDic.Add("180012", controller.GetContentByID("180012").Split(new char[]
        {
            ','
        }));
        this.textModelDic.Add("180013", controller.GetContentByID("180013").Split(new char[]
        {
            ','
        }));
    }

    private void InitCurSkill()
    {
        this.LoadSkillItem();
        this.lsCtl.OnRefreshCurSkill();
    }

    public void RefreshSkillInfo()
    {
        uint num = this.lsCtl.CurrSkillId;
        uint num2 = this.lsCtl.CurrSkillLevel;
        LuaTable config = this.lsCtl.GetConfig();
        if (null != this.lsPanel.txt_curskillLv)
        {
            this.lsPanel.txt_curskillLv.text = num2.ToString();
        }
        if (null != this.lsPanel.txt_curskillDis)
        {
            this.lsPanel.txt_curskillDis.text = this.GetSkillRangeInfo(config);
        }
        if (null != this.lsPanel.txt_curcoolTime)
        {
            this.lsPanel.txt_curcoolTime.text = this.GetSkillDTimeInfo(config);
        }
        if (null != this.lsPanel.txt_curpostTime)
        {
            this.lsPanel.txt_curpostTime.text = this.GetChantTimeInfo(config);
        }
        if (null != this.lsPanel.txt_curskllInfo)
        {
            this.lsPanel.txt_curskllInfo.text = config.GetField_String("desc");
        }
        if (null != this.lsPanel.txt_curskillType)
        {
            this.lsPanel.txt_curskillType.text = this.GetSkillPassive(config);
        }
        bool flag = this.lsCtl.IsMaxLv(num, num2);
        this.SetNextSkillTransformState(flag);
        if (!flag)
        {
            num = this.lsCtl.GetNextSkillId(num, num2);
            num2 += 1U;
            config = this.lsCtl.GetConfig(num, num2);
            if (config == null)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "nextskill config is null...skill id : ",
                    num,
                    ";skill lv: ",
                    num2
                }));
                return;
            }
            if (null != this.lsPanel.txt_nextskillLv)
            {
                this.lsPanel.txt_nextskillLv.text = num2.ToString();
            }
            if (null != this.lsPanel.txt_nextskllType)
            {
                this.lsPanel.txt_nextskllType.text = this.GetSkillPassive(config);
            }
            if (null != this.lsPanel.txt_nextskillDis)
            {
                this.lsPanel.txt_nextskillDis.text = this.GetSkillRangeInfo(config);
            }
            if (null != this.lsPanel.txt_nextcoolTime)
            {
                this.lsPanel.txt_nextcoolTime.text = this.GetSkillDTimeInfo(config);
            }
            if (null != this.lsPanel.txt_nextpostTime)
            {
                this.lsPanel.txt_nextpostTime.text = this.GetChantTimeInfo(config);
            }
            if (null != this.lsPanel.txt_nextskllInfo)
            {
                this.lsPanel.txt_nextskllInfo.text = config.GetField_String("desc");
            }
            if (null != this.lsPanel.txt_costneedLv)
            {
                this.lsPanel.txt_costneedLv.text = this.GetCostNeedPlayerLv(config);
                this.lsPanel.txt_costneedLv.color = this.GetCostResourcesState(this.lsCtl.IsUnLockLevel(num, num2));
            }
            List<LvCostItem> costItemByType = this.lsCtl.GetCostItemByType(UnLockSkillsController.ItemType.Gold);
            if (costItemByType.Count > 0)
            {
                if (null != this.lsPanel.txt_costneedGold)
                {
                    this.lsPanel.txt_costneedGold.text = costItemByType[0].m_num.ToString();
                    this.lsPanel.txt_costneedGold.color = this.GetCostResourcesState(this.lsCtl.IsEnoughGold());
                }
                if (null != this.lsPanel.txt_currentGold)
                {
                    this.lsPanel.txt_currentGold.text = GlobalRegister.GetCurrencyByID(3U).ToString();
                }
                if (null != this.lsPanel.img_costGold)
                {
                    ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, this.lsCtl.GetObjectIconName(costItemByType[0].m_id), delegate (UITextureAsset item)
                    {
                        if (this.lsPanel == null)
                        {
                            return;
                        }
                        if (item != null && item.textureObj != null)
                        {
                            this.lsPanel.img_costGold.texture = item.textureObj;
                            this.lsPanel.img_costGold.color = Color.white;
                        }
                    });
                }
            }
            List<LvCostItem> costItemByType2 = this.lsCtl.GetCostItemByType(UnLockSkillsController.ItemType.Normal);
            if (costItemByType2.Count > 0 && null != this.lsPanel.tf_costItem)
            {
                int num3 = 0;
                for (int i = 0; i < costItemByType2.Count; i++)
                {
                    GameObject gameObject;
                    if (num3 >= this.costItemlist.Count)
                    {
                        gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lsPanel.tf_costItem.gameObject);
                        gameObject.name = costItemByType2[i].m_id.ToString();
                        gameObject.transform.SetParent(this.lsPanel.tf_costGrid);
                        gameObject.transform.localScale = Vector3.one;
                        gameObject.SetActive(true);
                        this.costItemlist.Add(gameObject);
                    }
                    else
                    {
                        gameObject = this.costItemlist[num3];
                    }
                    UnLockSkillsCostItem unLockSkillsCostItem = gameObject.GetComponent<UnLockSkillsCostItem>();
                    if (null == unLockSkillsCostItem)
                    {
                        unLockSkillsCostItem = gameObject.AddComponent<UnLockSkillsCostItem>();
                    }
                    unLockSkillsCostItem.Initlize(costItemByType2[i]);
                    num3++;
                }
            }
            if (null != this.lsPanel.tf_nextskillLvup)
            {
                bool flag2 = this.lsCtl.IsEnoughGold();
                flag2 &= this.lsCtl.IsUnLockLevel(num, num2);
                for (int j = 0; j < costItemByType2.Count; j++)
                {
                    flag2 &= this.lsCtl.IsEnoughItem(costItemByType2[j].m_id);
                }
                Button component = this.lsPanel.tf_nextskillLvup.GetComponent<Button>();
                if (null != component)
                {
                    component.interactable = flag2;
                }
            }
        }
        else if (this.lsCtl.manualType == UnLockSkillsController.ManualType.Npc)
        {
            int nextSkillToShow = this.GetNextSkillToShow();
            if (nextSkillToShow >= 0)
            {
                SkillDataMapInfo skillDataMapInfo = this.lsCtl.FindSameSkillMap((uint)nextSkillToShow);
                if (skillDataMapInfo == null)
                {
                    return;
                }
                this.lsCtl.CurrSkillId = skillDataMapInfo.m_skillId;
                this.lsCtl.CurrSkillLevel = skillDataMapInfo.m_skillLv;
                this.lsCtl.CurSkillMap = skillDataMapInfo;
            }
        }
        this.lsPanel.SetWayFindBtnState();
    }

    private string GetChantTimeInfo(LuaTable config)
    {
        string result = string.Empty;
        float num = config.GetField_Uint("chanttime") / 1000f;
        int field_Uint = (int)config.GetField_Uint("canbe_passive");
        if (field_Uint == 1)
        {
            result = this.textModelDic["180012"][0];
        }
        else if (num == 0f)
        {
            result = this.textModelDic["180011"][0];
        }
        else
        {
            result = num.ToString();
        }
        return result;
    }

    private string GetSkillRangeInfo(LuaTable config)
    {
        string[] array = config.GetField_String("SkillRange").Split(new char[]
        {
            ':'
        });
        float num = 0f;
        if (array.Length == 2)
        {
            num = float.Parse(array[1]) / 3f;
        }
        return num.ToString() + this.textModelDic["180010"][0];
    }

    private string GetSkillDTimeInfo(LuaTable config)
    {
        return (uint.Parse(config.GetField_String("dtime")) / 1000f).ToString() + this.textModelDic["180010"][1];
    }

    private string GetSkillPassive(LuaTable config)
    {
        string result = string.Empty;
        uint field_Uint = config.GetField_Uint("canbe_passive");
        if (field_Uint == 1U)
        {
            result = this.textModelDic["2200171c"][0];
        }
        else
        {
            result = this.textModelDic["2200171b"][0];
        }
        return result;
    }

    private string GetCostNeedPlayerLv(LuaTable config)
    {
        string result = string.Empty;
        uint field_Uint = config.GetField_Uint("unlocklevel");
        if (field_Uint == 0U)
        {
            result = this.textModelDic["180012"][0];
        }
        else
        {
            result = field_Uint.ToString();
        }
        return result;
    }

    private void SetNextSkillTransformState(bool bmax)
    {
        if (null != this.lsPanel.tf_skillMaxLv)
        {
            this.lsPanel.tf_skillMaxLv.gameObject.SetActive(bmax);
        }
        if (null != this.lsPanel.tf_skillNextLv)
        {
            this.lsPanel.tf_skillNextLv.gameObject.SetActive(!bmax);
        }
        if (null != this.lsPanel.tf_skillMaxCost)
        {
            this.lsPanel.tf_skillMaxCost.gameObject.SetActive(bmax);
        }
        if (null != this.lsPanel.tf_skillCost)
        {
            this.lsPanel.tf_skillCost.gameObject.SetActive(!bmax);
        }
        if (null != this.lsPanel.tf_nextskillLvup)
        {
            this.lsPanel.tf_nextskillLvup.gameObject.SetActive(!bmax & this.lsCtl.manualType == UnLockSkillsController.ManualType.Npc);
            UIButtonState component = this.lsPanel.tf_nextskillLvup.GetComponent<UIButtonState>();
            component.OnPointerExit(null);
        }
    }

    public Color GetCostResourcesState(bool istrue)
    {
        return (!istrue) ? Color.red : Color.green;
    }

    public void UpdateSkill()
    {
        if (null != this.lsPanel.txt_characterName)
        {
            this.lsPanel.txt_characterName.text = this.lsCtl.GetCharacterName();
        }
        this.lsPanel.SetViewStateWhenMaxLv(false);
        this.LoadSkillItem();
        this.RefreshSkillInfo();
    }

    public void LoadSkillItem()
    {
        if (null == this.lsPanel.tf_skillItem)
        {
            FFDebug.LogError(this, "skill item null...");
            return;
        }
        ulong thisid = ulong.Parse(MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid);
        int heroIdByThisid = ControllerManager.Instance.GetController<HeroHandbookController>().GetHeroIdByThisid(thisid);
        LuaTable configTable = LuaConfigManager.GetConfigTable("skillshow", (ulong)((long)heroIdByThisid));
        string field_String = configTable.GetField_String("skill");
        string field_String2 = configTable.GetField_String("breakskill");
        List<uint> list = new List<uint>();
        List<uint> list2 = new List<uint>();
        List<uint> list3 = new List<uint>();
        for (int i = 0; i < this.lsCtl.skillMapList.Count; i++)
        {
            list.Add(this.lsCtl.skillMapList[i].m_skillId);
            list3.Add(this.lsCtl.skillMapList[i].m_skillId % 100U);
            list2.Add(this.lsCtl.skillMapList[i].m_skillLv);
        }
        int count = this.lsCtl.skillMapList.Count;
        if (this.lsCtl.manualType != UnLockSkillsController.ManualType.Npc)
        {
            string[] array = field_String.Split(new string[]
            {
                "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < array.Length; j++)
            {
                uint num = uint.Parse(array[j]);
                if (!list3.Contains(num % 100U))
                {
                    list.Add(num);
                    list2.Add(1U);
                }
            }
            string[] array2 = field_String2.Split(new string[]
            {
                "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < array2.Length; k++)
            {
                uint num2 = uint.Parse(array2[k]);
                if (!list3.Contains(num2 % 100U))
                {
                    list.Add(num2);
                    list2.Add(1U);
                }
            }
        }
        this.skillItemList.ForEach(delegate (GameObject item)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        });
        int num3 = 0;
        for (int l = 0; l < list.Count; l++)
        {
            GameObject gameObject;
            if (num3 >= this.skillItemList.Count)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lsPanel.tf_skillItem.gameObject);
                gameObject.transform.SetParent(this.lsPanel.tf_skillgrid);
                gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(true);
                this.skillItemList.Add(gameObject);
            }
            else
            {
                gameObject = this.skillItemList[num3];
                gameObject.SetActive(true);
            }
            num3++;
            gameObject.name = list[l].ToString();
            if (this.lsCtl.manualType == UnLockSkillsController.ManualType.Npc && this.lsCtl.IsMaxLv(list[l], list2[l]))
            {
                gameObject.SetActive(false);
            }
            else
            {
                UnLockSkillsItem unLockSkillsItem = gameObject.GetComponent<UnLockSkillsItem>();
                if (null == unLockSkillsItem)
                {
                    unLockSkillsItem = gameObject.AddComponent<UnLockSkillsItem>();
                }
                unLockSkillsItem.Initilize(list[l], list2[l], l < count);
            }
        }
    }

    public int GetNextSkillToShow()
    {
        int num = 0;
        for (int i = 0; i < this.skillItemList.Count; i++)
        {
            int num2 = int.Parse(this.skillItemList[i].name);
            int num3 = num2 % 100;
            if ((long)num3 == (long)((ulong)(this.lsCtl.CurrSkillId % 100U)))
            {
                num = i;
                break;
            }
        }
        for (int j = num + 1; j < this.skillItemList.Count; j++)
        {
            if (this.skillItemList[j].activeSelf)
            {
                return int.Parse(this.skillItemList[j].name);
            }
        }
        for (int k = 0; k < num; k++)
        {
            if (this.skillItemList[k].activeSelf)
            {
                return int.Parse(this.skillItemList[k].name);
            }
        }
        return -1;
    }

    public override void OnDispose()
    {
        this.skillItemList.Clear();
        this.textModelDic.Clear();
        this.costItemlist.Clear();
    }

    public void OnSkillItemClick(uint _skillId)
    {
        this.lsCtl.OnSetCurrSkillID(_skillId);
    }

    private UnlockSkillsPanel lsPanel;

    private UnLockSkillsController _lsctrl;

    private List<GameObject> skillItemList = new List<GameObject>();

    public BetterDictionary<string, string[]> textModelDic = new BetterDictionary<string, string[]>();

    private List<GameObject> costItemlist = new List<GameObject>();
}
