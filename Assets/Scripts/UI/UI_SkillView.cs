using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using magic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillView : UIPanelBase
{
    private SkillViewControll skillController
    {
        get
        {
            return ControllerManager.Instance.GetController<SkillViewControll>();
        }
    }

    public override void OnInit(Transform tran)
    {
        base.OnInit(tran);
        this.mRoot = tran;
        this.InitGameObject();
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void InitGameObject()
    {
        this.close = this.mRoot.Find("Offset_Skill/Panel_WeaponSkill/title/btn_close").gameObject;
        UIEventListener.Get(this.close).onClick = delegate (PointerEventData eventData)
        {
            this.skillController.CloseSkillView();
        };
        this.PanelWeaponSkill = this.mRoot.Find("Offset_Skill/Panel_WeaponSkill");
        this.skillsRect = this.PanelWeaponSkill.Find("Panel_SKills/Skills/SkillList/UIFoldOutList/Rect");
        this.tranSkillInfo = this.PanelWeaponSkill.Find("Panel_SKills/SkillInfo");
        this.txt_SkillInfoName = this.tranSkillInfo.Find("txt_skillname").GetComponent<Text>();
        this.txt_SkillInfoDesc = this.tranSkillInfo.Find("content/txt_desc").GetComponent<Text>();
        this.txt_SkillInfoExpend = this.tranSkillInfo.Find("content/txt_labelmp/txt_mp").GetComponent<Text>();
        this.tranExpend = this.tranSkillInfo.Find("content/txt_labelmp");
        this.txt_Type = this.tranSkillInfo.Find("content/txt_labeltype/txt_type").GetComponent<Text>();
        this.tranStudy = this.tranSkillInfo.Find("study");
        this.tranLevelUp = this.tranSkillInfo.Find("upgrade");
        this.unLock = this.tranSkillInfo.Find("unlock");
        this.nextLevel = this.tranSkillInfo.Find("content/txt_next");
        this.txt_Next = this.tranSkillInfo.Find("content/txt_next").GetComponent<Text>();
        this.txt_NextDesc = this.tranSkillInfo.Find("content/txt_nextdesc").GetComponent<Text>();
        UIEventListener.Get(this.tranStudy.Find("btn_equip").gameObject).onClick = delegate (PointerEventData data)
        {
            this.UpGradeSkill();
        };
        UIEventListener.Get(this.tranLevelUp.Find("btn_equip").gameObject).onClick = delegate (PointerEventData data)
        {
            this.UpGradeSkill();
        };
        this.jobdisSlect = new GameObject[4];
        this.jobSelect = new GameObject[4];
    }

    public void StartSkillView()
    {
        Profession profession = (Profession)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
        this.mRoot.gameObject.SetActive(true);
        if (profession == Profession.none)
        {
            profession = Profession.saber;
        }
        this.setJob(profession);
    }

    private void setSkillsInfo(GameObject ga, string skillstr, uint linetype, List<GameObject> lineListObj)
    {
        ga.SetActive(true);
        uint num = uint.Parse(skillstr);
        uint num2 = num * 1000U + 11U;
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)num2);
        string[] array = configTable.GetField_String("extskill").Split(new char[]
        {
            '-'
        }, StringSplitOptions.RemoveEmptyEntries);
        GameObject gameObject = ga.transform.Find("skill").gameObject;
        GameObject gameObject2 = ga.transform.Find("extends/extend").gameObject;
        gameObject.SetActive(false);
        gameObject2.SetActive(false);
        SkillItem skillItem = new SkillItem(gameObject);
        skillItem.Config = configTable;
        this.skillMain.Add(skillItem);
        this.setMainSkillInfo(skillItem, configTable);
        for (int i = 0; i < array.Length; i++)
        {
            uint id = uint.Parse(array[i]);
            bool bstudySkill = false;
            ExtSkillData extSkillData = new ExtSkillData();
            extSkillData.id = id;
            extSkillData.level = 1U;
            extSkillData.masterskill = num;
            if (this.skillController.DicExtSkills.ContainsKey(num) && i < this.skillController.DicExtSkills[num].Count)
            {
                extSkillData = this.skillController.DicExtSkills[num][i];
                bstudySkill = true;
            }
            if (i < lineListObj.Count)
            {
                this.setSkillExtItem(lineListObj[i], extSkillData, bstudySkill);
            }
            else
            {
                GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
                gameObject3.transform.SetParent(ga.transform.Find("extends"));
                gameObject3.name = gameObject2.name + i.ToString();
                gameObject3.transform.localScale = gameObject2.transform.localScale;
                gameObject3.transform.localPosition = gameObject2.transform.localPosition + new Vector3((float)(151 * i), 0f, 0f);
                lineListObj.Add(gameObject3);
                this.setSkillExtItem(gameObject3, extSkillData, bstudySkill);
            }
        }
        if (lineListObj.Count > array.Length)
        {
            for (int j = array.Length; j < lineListObj.Count; j++)
            {
                lineListObj[j].SetActive(false);
            }
        }
    }

    private void setMainSkillInfo(SkillItem ga, LuaTable skillConfig)
    {
        ga.itemobj.SetActive(true);
        Image component = ga.itemobj.transform.Find("img_skill").GetComponent<Image>();
        string imagename = skillConfig.GetField_String("skillicon").Split(new char[]
        {
            ','
        }, StringSplitOptions.RemoveEmptyEntries)[0];
        ClassSetItemTexture.Instance.setTexture(component, imagename, ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
        UIEventListener.Get(ga.itemobj).onClick = delegate (PointerEventData data)
        {
            this.clickSkill(ga.itemobj, skillConfig);
        };
        if (this.IsUnlock(skillConfig.GetField_Uint("id") / 1000U, MainPlayer.Self.GetCurLevel()))
        {
            component.color = new Color(1f, 1f, 1f);
            ga.itemobj.transform.Find("img_lock").gameObject.SetActive(false);
        }
        else
        {
            component.color = Const.SkillLocked;
            ga.itemobj.transform.Find("img_lock").gameObject.SetActive(true);
        }
    }

    private void addTextureAsset(UITextureAsset asset)
    {
        this.usedTextureAssets.Add(asset);
    }

    private void setSkillExtItem(GameObject skillitem, ExtSkillData extSkill, bool bstudySkill = false)
    {
        uint id = extSkill.id;
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_ext", (ulong)id);
        if (configTable != null)
        {
            skillitem.SetActive(true);
            Image component = skillitem.transform.Find("img_skill").GetComponent<Image>();
            string imagename = configTable.GetField_String("icon").Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries)[0];
            ClassSetItemTexture.Instance.setTexture(component, imagename, ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
            skillitem.transform.Find("img_studynext").gameObject.SetActive(false);
            if (configTable.GetField_Uint("studylevel") <= MainPlayer.Self.GetCurLevel())
            {
                skillitem.transform.Find("img_unlock").gameObject.SetActive(false);
            }
            else if ((long)configTable.GetField_Int("studylevel") - (long)((ulong)MainPlayer.Self.GetCurLevel()) <= 5L)
            {
                skillitem.transform.Find("img_unlock").gameObject.SetActive(false);
                skillitem.transform.Find("img_studynext").gameObject.SetActive(true);
                skillitem.transform.Find("img_studynext/txt_lv").GetComponent<Text>().text = string.Format(skillitem.transform.Find("img_studynext/txt_lv").GetComponent<Text>().text, configTable.GetField_Uint("studylevel"));
            }
            else
            {
                skillitem.transform.Find("img_studynext").gameObject.SetActive(false);
                skillitem.transform.Find("img_unlock").gameObject.SetActive(true);
            }
            if (bstudySkill)
            {
                skillitem.transform.Find("img_study").gameObject.SetActive(false);
            }
            else
            {
                skillitem.transform.Find("img_study").gameObject.SetActive(true);
                if ((long)configTable.GetField_Int("studylevel") <= (long)((ulong)MainPlayer.Self.GetCurLevel()))
                {
                    skillitem.transform.Find("img_study/txt_study").gameObject.SetActive(true);
                }
                else
                {
                    skillitem.transform.Find("img_study/txt_study").gameObject.SetActive(false);
                }
            }
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("skill_ext", (ulong)(id + 1U));
            if (configTable2 != null)
            {
                if (configTable2.GetField_Uint("studylevel") <= MainPlayer.Self.GetCurLevel())
                {
                    if (bstudySkill)
                    {
                        skillitem.transform.Find("img_up").gameObject.SetActive(true);
                    }
                    else
                    {
                        skillitem.transform.Find("img_up").gameObject.SetActive(false);
                    }
                }
                else
                {
                    skillitem.transform.Find("img_up").gameObject.SetActive(false);
                }
            }
            else
            {
                skillitem.transform.Find("img_up").gameObject.SetActive(false);
            }
            UIEventListener.Get(skillitem).onClick = delegate (PointerEventData data)
            {
                this.clickSkill(skillitem, extSkill, bstudySkill);
            };
        }
        else
        {
            skillitem.SetActive(false);
        }
    }

    private bool IsUnlock(uint skillID, uint curLevel)
    {
        for (int i = 0; i < this.skillController.dicEquipSkill.Count; i++)
        {
            if (this.skillController.dicEquipSkill[i].skill.skillid == skillID)
            {
                return true;
            }
        }
        return false;
    }

    private void clickSkill(GameObject skill, ExtSkillData extskill, bool bstudy)
    {
        this.selectExtSkill.SelectObjParent = skill;
        this.selectExtSkill.bstudy = bstudy;
        this.selectExtSkill.extskill = extskill;
        Toggle component = skill.GetComponent<Toggle>();
        if (component != null)
        {
            component.isOn = true;
        }
        this.setSkillInfo(extskill, bstudy);
    }

    private void UpGradeSkill()
    {
        if (this.selectExtSkill.extskill != null)
        {
            if (this.selectExtSkill.bstudy)
            {
                this.skillController.ReqUpGradeSkill(this.selectExtSkill.extskill.id + 1U);
            }
            else
            {
                this.skillController.ReqUpGradeSkill(this.selectExtSkill.extskill.id);
            }
        }
    }

    public void OnStudySkillSuccess()
    {
        if (this.selectExtSkill.SelectObjParent != null && this.selectExtSkill.extskill != null)
        {
            if (this.selectExtSkill.bstudy)
            {
                this.selectExtSkill.extskill.id = this.selectExtSkill.extskill.id + 1U;
                this.selectExtSkill.extskill.level = this.selectExtSkill.extskill.level + 1U;
                this.setSkillExtItem(this.selectExtSkill.SelectObjParent, this.selectExtSkill.extskill, this.selectExtSkill.bstudy);
                this.clickSkill(this.selectExtSkill.SelectObjParent, this.selectExtSkill.extskill, this.selectExtSkill.bstudy);
            }
            else
            {
                this.selectExtSkill.bstudy = true;
                this.skillController.DicExtSkills[this.selectExtSkill.extskill.masterskill].Add(this.selectExtSkill.extskill);
                this.setSkillExtItem(this.selectExtSkill.SelectObjParent, this.selectExtSkill.extskill, this.selectExtSkill.bstudy);
                this.clickSkill(this.selectExtSkill.SelectObjParent, this.selectExtSkill.extskill, this.selectExtSkill.bstudy);
            }
        }
        else if (this.skillMain.Count > 0)
        {
            this.clickSkill(this.skillMain[0].itemobj, this.skillMain[0].Config);
        }
    }

    private void clickSkill(GameObject skill, LuaTable config)
    {
        Toggle component = skill.GetComponent<Toggle>();
        if (component != null)
        {
            component.isOn = true;
        }
        this.setSkillInfo(config);
    }

    private void setSkillInfo(LuaTable config)
    {
        this.txt_SkillInfoName.text = config.GetField_String("skillname");
        this.txt_SkillInfoDesc.text = config.GetField_String("desc");
        this.UpdateMagicCost(config.GetField_Uint("magiccost"));
        this.tranSkillInfo.Find("txt_labellv").gameObject.SetActive(false);
        this.tranSkillInfo.Find("txt_lv").gameObject.SetActive(false);
        this.tranSkillInfo.Find("txt_nostudy").gameObject.SetActive(false);
        UIInformationList component = this.txt_Type.gameObject.GetComponent<UIInformationList>();
        if (component.listInformation.Count == 2)
        {
            this.txt_Type.text = component.listInformation[0].content;
        }
        this.tranStudy.gameObject.SetActive(false);
        this.tranLevelUp.gameObject.SetActive(false);
        this.nextLevel.gameObject.SetActive(false);
        this.txt_Next.gameObject.SetActive(false);
        this.txt_NextDesc.gameObject.SetActive(false);
        this.unLock.gameObject.SetActive(false);
    }

    private void setSkillInfo(ExtSkillData extskillData, bool bstudy)
    {
        uint num = extskillData.id + 1U;
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_ext", (ulong)num);
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("skill_ext", (ulong)extskillData.id);
        this.selectSkillId = configTable2.GetField_Uint("id");
        this.txt_SkillInfoName.text = configTable2.GetField_String("skillname");
        this.txt_SkillInfoDesc.text = configTable2.GetField_String("desc");
        this.UpdateMagicCost(configTable2.GetField_Uint("magiccost"));
        this.tranSkillInfo.Find("txt_labellv").gameObject.SetActive(true);
        this.tranSkillInfo.Find("txt_lv").gameObject.SetActive(true);
        this.tranSkillInfo.Find("txt_nostudy").gameObject.SetActive(false);
        UIInformationList component = this.txt_Type.gameObject.GetComponent<UIInformationList>();
        if (component.listInformation.Count == 2)
        {
            if (configTable2.GetField_Bool("isactive"))
            {
                this.txt_Type.text = component.listInformation[0].content;
            }
            else
            {
                this.txt_Type.text = component.listInformation[1].content;
            }
        }
        if (configTable2.GetField_Uint("studylevel") <= MainPlayer.Self.GetCurLevel())
        {
            this.nextLevel.gameObject.SetActive(false);
            this.txt_Next.gameObject.SetActive(false);
            this.txt_NextDesc.gameObject.SetActive(false);
            this.unLock.gameObject.SetActive(false);
            if (bstudy)
            {
                this.tranLevelUp.gameObject.SetActive(true);
                this.tranStudy.gameObject.SetActive(false);
                string[] array = configTable2.GetField_String("upgrade_cost").Split(new char[]
                {
                    '-'
                }, StringSplitOptions.RemoveEmptyEntries);
                this.tranLevelUp.Find("cost/txt_cost").GetComponent<Text>().text = array[1];
                this.tranSkillInfo.Find("txt_lv").GetComponent<Text>().text = extskillData.level.ToString();
                uint num2 = uint.Parse(array[0]);
                uint num3 = uint.Parse(array[1]);
                if ((uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    num2
                })[0] >= num3)
                {
                    this.tranLevelUp.Find("cost/txt_cost").GetComponent<Text>().color = Const.TextColorNormalInUI;
                }
                else
                {
                    this.tranLevelUp.Find("cost/txt_cost").GetComponent<Text>().color = Color.red;
                }
                ClassSetItemTexture.Instance.SetItemTexture(this.tranLevelUp.Find("cost/img_icon").GetComponent<Image>(), uint.Parse(array[0]), ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
                if (configTable != null)
                {
                    this.nextLevel.gameObject.SetActive(true);
                    this.txt_Next.gameObject.SetActive(true);
                    this.txt_NextDesc.gameObject.SetActive(true);
                    this.txt_NextDesc.text = configTable.GetField_String("desc");
                }
                else
                {
                    this.nextLevel.gameObject.SetActive(false);
                    this.txt_Next.gameObject.SetActive(false);
                    this.txt_NextDesc.gameObject.SetActive(false);
                    this.tranLevelUp.gameObject.SetActive(false);
                }
                if ((uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    num2
                })[0] >= num3)
                {
                    this.tranLevelUp.Find("btn_CannotUpgrade").gameObject.SetActive(false);
                }
                else
                {
                    this.tranLevelUp.Find("btn_CannotUpgrade").gameObject.SetActive(true);
                }
                if (configTable != null)
                {
                    if (configTable.GetField_Uint("studylevel") <= MainPlayer.Self.GetCurLevel())
                    {
                        this.tranLevelUp.Find("cantupgrade").gameObject.SetActive(false);
                        this.tranLevelUp.Find("cost").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.tranLevelUp.Find("cantupgrade").gameObject.SetActive(true);
                        this.tranLevelUp.Find("cantupgrade/txt_cr").GetComponent<Text>().text = configTable.GetField_String("studylevel");
                        this.tranLevelUp.Find("cost").gameObject.SetActive(false);
                    }
                    if ((uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                    {
                        Util.GetLuaTable("BagCtrl"),
                        num2
                    })[0] >= num3 && configTable.GetField_Uint("studylevel") <= MainPlayer.Self.GetCurLevel())
                    {
                        this.tranLevelUp.Find("btn_equip").gameObject.SetActive(true);
                    }
                    else
                    {
                        this.tranLevelUp.Find("btn_equip").gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                this.nextLevel.gameObject.SetActive(false);
                this.txt_Next.gameObject.SetActive(false);
                this.txt_NextDesc.gameObject.SetActive(false);
                this.tranLevelUp.gameObject.SetActive(false);
                this.tranStudy.gameObject.SetActive(true);
                string[] array2 = configTable2.GetField_String("upgrade_cost").Split(new char[]
                {
                    '-'
                }, StringSplitOptions.RemoveEmptyEntries);
                this.tranStudy.Find("cost/txt_cost").GetComponent<Text>().text = array2[1];
                ClassSetItemTexture.Instance.SetItemTexture(this.tranStudy.Find("cost/img_icon").GetComponent<Image>(), uint.Parse(array2[0]), ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
                this.tranSkillInfo.Find("txt_lv").gameObject.SetActive(false);
                this.tranSkillInfo.Find("txt_labellv").gameObject.SetActive(false);
                this.tranSkillInfo.Find("txt_nostudy").gameObject.SetActive(true);
                uint num4 = uint.Parse(array2[0]);
                uint num5 = uint.Parse(array2[1]);
                if ((uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    num4
                })[0] >= num5)
                {
                    this.tranStudy.Find("cost/txt_cost").GetComponent<Text>().color = Const.TextColorNormalInUI;
                }
                else
                {
                    this.tranStudy.Find("cost/txt_cost").GetComponent<Text>().color = Color.red;
                }
                if (this.CheckForwardSkill(extskillData) && (uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    num4
                })[0] >= num5)
                {
                    this.tranStudy.Find("btn_equip").gameObject.SetActive(true);
                    this.tranStudy.Find("btn_CannotStudy").gameObject.SetActive(false);
                    this.tranStudy.Find("txt_cantstudy").gameObject.SetActive(false);
                    this.tranStudy.Find("cost").gameObject.SetActive(true);
                }
                else if (this.CheckForwardSkill(extskillData) && (uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    num4
                })[0] < num5)
                {
                    this.tranStudy.Find("btn_equip").gameObject.SetActive(false);
                    this.tranStudy.Find("btn_CannotStudy").gameObject.SetActive(true);
                    this.tranStudy.Find("txt_cantstudy").gameObject.SetActive(false);
                    this.tranStudy.Find("cost").gameObject.SetActive(true);
                }
                else
                {
                    this.tranStudy.Find("btn_equip").gameObject.SetActive(false);
                    this.tranStudy.Find("btn_CannotStudy").gameObject.SetActive(false);
                    this.tranStudy.Find("txt_cantstudy").gameObject.SetActive(true);
                    this.tranStudy.Find("cost").gameObject.SetActive(false);
                }
            }
        }
        else
        {
            this.nextLevel.gameObject.SetActive(false);
            this.txt_Next.gameObject.SetActive(false);
            this.txt_NextDesc.gameObject.SetActive(false);
            this.tranLevelUp.gameObject.SetActive(false);
            this.tranStudy.gameObject.SetActive(false);
            this.unLock.gameObject.SetActive(true);
            this.unLock.Find("txt_cr").GetComponent<Text>().text = configTable2.GetField_String("studylevel");
            this.tranSkillInfo.Find("txt_labellv").gameObject.SetActive(false);
            this.tranSkillInfo.Find("txt_lv").gameObject.SetActive(false);
            this.tranSkillInfo.Find("txt_nostudy").gameObject.SetActive(false);
        }
    }

    private bool CheckForwardSkill(ExtSkillData extskillData)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_ext", (ulong)extskillData.id);
        if (configTable.GetField_Uint("pre_extskill") == 0U)
        {
            return true;
        }
        for (int i = 0; i < this.skillController.DicExtSkills[extskillData.masterskill].Count; i++)
        {
            if (this.skillController.DicExtSkills[extskillData.masterskill][i].id / 1000U == configTable.GetField_Uint("id") / 10000U * 10U + configTable.GetField_Uint("pre_extskill"))
            {
                return true;
            }
        }
        return false;
    }

    private void clearObjList()
    {
        for (int i = 0; i < this.listLineGameObject.Count; i++)
        {
            UnityEngine.Object.Destroy(this.listLineGameObject[i]);
        }
        this.listLineGameObject.Clear();
    }

    private void setAllSkill(LuaTable dicSkillLine)
    {
        GameObject gameObject = this.skillsRect.Find("Item").gameObject;
        gameObject.SetActive(false);
        IEnumerator enumerator = dicSkillLine.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            int field_Int = luaTable.GetField_Int("ID");
            string field_String = luaTable.GetField_String("skillset");
            uint field_Uint = luaTable.GetField_Uint("type");
            if (field_Int - 1 < this.listLineSkillGameObject.Count)
            {
                GameObject lineObj = this.listLineSkillGameObject[field_Int - 1].lineObj;
                this.setSkillsInfo(lineObj, field_String, field_Uint, this.listLineSkillGameObject[field_Int - 1].listObjInLine);
            }
            else
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(this.skillsRect);
                gameObject2.name = gameObject.name + (field_Int - 1).ToString();
                gameObject2.transform.localScale = gameObject.transform.localScale;
                gameObject2.transform.localPosition = gameObject.transform.localPosition + new Vector3(0f, (float)(0 - 90 * (field_Int - 1)), 0f);
                SkillLineObject skillLineObject = new SkillLineObject();
                skillLineObject.lineObj = gameObject2;
                skillLineObject.listObjInLine = new List<GameObject>();
                this.listLineSkillGameObject.Add(skillLineObject);
                this.setSkillsInfo(gameObject2, field_String, field_Uint, skillLineObject.listObjInLine);
            }
        }
        if (this.listLineSkillGameObject.Count > dicSkillLine.Keys.Count)
        {
            for (int i = dicSkillLine.Keys.Count; i < this.listLineSkillGameObject.Count; i++)
            {
                this.listLineSkillGameObject[i].lineObj.SetActive(false);
            }
        }
        this.OnStudySkillSuccess();
    }

    private void setJob(Profession job)
    {
        this.clearObjList();
        switch (job)
        {
            case Profession.saber:
                {
                    LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_1");
                    this.setAllSkill(cacheField_Table);
                    break;
                }
            case Profession.dagger:
                {
                    LuaTable cacheField_Table2 = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_2");
                    this.setAllSkill(cacheField_Table2);
                    break;
                }
            case Profession.greatsword:
                {
                    LuaTable cacheField_Table3 = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_3");
                    this.setAllSkill(cacheField_Table3);
                    break;
                }
            case Profession.ball:
                {
                    LuaTable cacheField_Table4 = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_4");
                    this.setAllSkill(cacheField_Table4);
                    break;
                }
        }
    }

    private void UpdateMagicCost(uint magicCost)
    {
        this.txt_SkillInfoExpend.text = magicCost.ToString();
        if (magicCost > 0U)
        {
            this.tranExpend.gameObject.SetActive(true);
        }
        else
        {
            this.tranExpend.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        for (int i = 0; i < this.skillMain.Count; i++)
        {
            this.skillMain[i].UpdataCD(currServerTime);
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        this.selectExtSkill = null;
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public Transform mRoot;

    private GameObject btnClose;

    private Transform skillsRect;

    private Transform PanelWeaponSkill;

    private Transform firstMenu;

    private Transform SecondMenu;

    private Transform PanelWeapon;

    private GameObject[] jobSelect;

    private GameObject[] jobdisSlect;

    private GameObject close;

    private Transform proficency;

    private Transform tranSkillInfo;

    private Text txt_SkillInfoName;

    private Text txt_SkillInfoDesc;

    private Text txt_SkillInfoExpend;

    private Transform tranExpend;

    private Text txt_Type;

    private Text txt_NextDesc;

    private Text txt_Next;

    private Transform tranStudy;

    private Transform tranLevelUp;

    private Transform unLock;

    private Transform nextLevel;

    private List<SkillItem> skillMain = new List<SkillItem>();

    private List<jobskillLevelAndExp> jobLevel = new List<jobskillLevelAndExp>();

    private GameObject curSelectSkill;

    private uint selectSkillId;

    private uint selectLine;

    private uint selectCareer;

    private SelectExtSkill selectExtSkill = new SelectExtSkill();

    private List<GameObject> listLineGameObject = new List<GameObject>();

    private List<SkillLineObject> listLineSkillGameObject = new List<SkillLineObject>();

    private Profession uistate = Profession.saber;
}
