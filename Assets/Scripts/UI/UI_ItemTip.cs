using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemTip : UIPanelBase
{
    private ItemTipController mItemTipController
    {
        get
        {
            return ControllerManager.Instance.GetController<ItemTipController>();
        }
    }

    public override void OnInit(Transform root)
    {
        this.Root = root;
        this.Root.position = new Vector3(1000f, 1000f);
        this.onInit();
        base.OnInit(root);
    }

    private void onInit()
    {
        this.skinRoot = this.Root.Find("skintip").gameObject;
        this.skinname = this.Root.Find("skintip/Head/txt_name").GetComponent<Text>();
        this.skindesc = this.Root.Find("skintip/Head/txt_desc").GetComponent<Text>();
        this.skinGetted = this.Root.Find("skintip/txt_name").GetComponent<Text>();
        this.heroAwakeRoot = this.Root.Find("break").gameObject;
        this.heroAwakeName = this.Root.Find("break/txt_name").GetComponent<Text>();
        this.heroAwakeInfoSample = this.Root.Find("break/Head/txt_desc").gameObject;
        this.heroAwakeInfoSample.SetActive(false);
        this.heroDesc = this.Root.Find("break/txt_desc").GetComponent<Text>();
        this.transIcon = this.Root.Find("Skill/Skill_name/Icon").GetComponent<Image>();
        this.transName = this.Root.Find("Skill/Skill_name/Info/Name").GetComponent<Text>();
        this.transType = this.Root.Find("Skill/Skill_name/Info/txt_style").GetComponent<Text>();
        this.transLevel = this.Root.Find("Skill/Skill_name/Info/txt_lv").GetComponent<Text>();
        this.transTitlePanel = this.Root.Find("Skill/Panel_title");
        this.transDesc = this.Root.Find("Skill/txt_desc").GetComponent<Text>();
        this.transDistance = this.transTitlePanel.Find("txt_distance/txt_value").GetComponent<Text>();
        this.transTime = this.transTitlePanel.Find("txt_time/txt_value").GetComponent<Text>();
        this.transCd = this.transTitlePanel.Find("txt_cd/txt_value").GetComponent<Text>();
        this.transSkillPanel = this.Root.Find("Skill");
        this.transUseItem = this.Root.Find("UseItem");
        this.useItemPanelSell = this.transUseItem.Find("Bottom/Panel_sell");
        this.useItemHead = this.transUseItem.Find("Head");
        this.useItemlevel = this.transUseItem.Find("Head/Item_name/info/txt_ueslv/Text").GetComponent<Text>();
        this.useItemValue = this.useItemPanelSell.Find("txt_value").GetComponent<Text>();
        this.useItemUseObj = this.transUseItem.Find("Bottom/txt_use");
    }

    public void SetupPanel(t_Object data)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            Transform child = this.Root.GetChild(i);
            child.localPosition = Vector3.zero;
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        this.mData = data;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)data.baseid);
        if (configTable != null)
        {
            this.mConfig = configTable;
            ObjectType field_Uint = (ObjectType)configTable.GetField_Uint("type");
            if (field_Uint != ObjectType.OBJTYPE_DNA)
            {
                if (field_Uint != ObjectType.OBJTYPE_DNA2)
                {
                    if (field_Uint != ObjectType.OBJTYPE_QUEST)
                    {
                        if (field_Uint != ObjectType.OBJTYPE_CARD)
                        {
                            this.SetupUseItem();
                        }
                        else if (this.mData.card_data == null)
                        {
                            this.SetupUseItem();
                        }
                        else
                        {
                            this.SetupCard();
                        }
                    }
                    else
                    {
                        this.SetupQuestItem();
                    }
                }
                else
                {
                    this.SetupGeneBag();
                }
            }
            else
            {
                this.SetupGene();
            }
        }
    }

    private void SetupCard()
    {
        Transform transform = this.Root.Find("Card");
        Image component = transform.GetComponent<Image>();
        Transform transform2 = transform.Find("Head");
        Image imgIcon = transform2.Find("Card_name/Icon").GetComponent<Image>();
        Transform transform3 = transform2.Find("Card_name/Info");
        Text component2 = transform3.Find("Name").GetComponent<Text>();
        Transform transform4 = transform3.Find("Star");
        Transform transform5 = transform3.Find("txt_bind");
        Transform transform6 = transform3.Find("txt_bind2");
        Transform transform7 = transform3.Find("txt_binded");
        RawImage rawimgCardType = transform2.Find("Panel/CardType/RawImage").GetComponent<RawImage>();
        Text component3 = transform2.Find("Panel/NeedLevel/Level").GetComponent<Text>();
        Text component4 = transform2.Find("Panel/Dur/Level").GetComponent<Text>();
        Image component5 = transform2.Find("Panel_power/panel_value/img_num").GetComponent<Image>();
        FightValueNum fightValueNum = component5.transform.parent.GetComponent<FightValueNum>();
        if (fightValueNum == null)
        {
            fightValueNum = component5.transform.parent.gameObject.AddComponent<FightValueNum>();
        }
        Transform transform8 = transform.Find("Bottom/Sell");
        Transform transform9 = transform.Find("Bottom/Repair");
        Image component6 = transform.Find("Center").GetComponent<Image>();
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < transform.Find("Center").childCount; i++)
        {
            list.Add(transform.Find("Center").GetChild(i));
        }
        List<Transform> list2 = new List<Transform>();
        for (int j = 0; j < 5; j++)
        {
            list2.Add(transform.Find("Random").GetChild(j));
        }
        List<Transform> list3 = new List<Transform>();
        for (int k = 0; k < 5; k++)
        {
            list3.Add(transform.Find("Trigger").GetChild(k));
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, this.mConfig.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            if (asset != null && imgIcon != null)
            {
                Texture2D textureObj = asset.textureObj;
                Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                imgIcon.sprite = sprite;
                imgIcon.overrideSprite = sprite;
                imgIcon.color = Color.white;
                imgIcon.gameObject.SetActive(true);
                imgIcon.material = null;
            }
        });
        component2.text = this.mConfig.GetField_String("name");
        component2.color = CommonTools.GetQualityColor(this.mConfig.GetField_Uint("quality"));
        Color qualityColor = CommonTools.GetQualityColor(this.mConfig.GetField_Uint("quality"));
        component6.color = qualityColor;
        for (int l = 0; l < transform4.childCount; l++)
        {
            transform4.GetChild(l).gameObject.SetActive((long)l < (long)((ulong)this.mData.card_data.cardstar));
        }
        string imgname = "card0" + this.mData.card_data.cardtype;
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ICON, imgname, delegate (UITextureAsset asset)
        {
            if (asset != null && rawimgCardType != null)
            {
                Texture2D textureObj = asset.textureObj;
                rawimgCardType.texture = textureObj;
                rawimgCardType.color = Color.white;
                rawimgCardType.gameObject.SetActive(true);
            }
        });
        component3.text = CommonTools.GetLevelFormat(this.mConfig.GetField_String("uselevel"));
        transform5.gameObject.SetActive(false);
        transform6.gameObject.SetActive(this.mData.bind == 0U);
        transform7.gameObject.SetActive(this.mData.bind != 0U);
        float num = 0f;
        for (int m = 0; m < list.Count; m++)
        {
            Transform transform10 = list[m];
            if (m < this.mData.card_data.base_effect.Count)
            {
                CardEffectItem cardEffectItem = this.mData.card_data.base_effect[m];
                Text component7 = transform10.GetComponent<Text>();
                Text component8 = transform10.Find("Text").GetComponent<Text>();
                LuaTable configTable = LuaConfigManager.GetConfigTable("cardeffect_config", (ulong)cardEffectItem.id);
                if (configTable != null)
                {
                    component7.text = configTable.GetField_String("cardeffectdes");
                    component8.text = "+" + cardEffectItem.value;
                    num += this.GetAttributeFightValue(configTable.GetField_String("protype"), cardEffectItem.value);
                }
                transform10.gameObject.SetActive(true);
            }
            else
            {
                transform10.gameObject.SetActive(false);
            }
        }
        for (int n = 0; n < list2.Count; n++)
        {
            Transform transform11 = list2[n];
            if (n < this.mData.card_data.rand_effect.Count)
            {
                CardEffectItem cardEffectItem2 = this.mData.card_data.rand_effect[n];
                Text component9 = transform11.GetComponent<Text>();
                Text component10 = transform11.Find("Text").GetComponent<Text>();
                LuaTable configTable2 = LuaConfigManager.GetConfigTable("cardeffect_config", (ulong)cardEffectItem2.id);
                if (configTable2 != null)
                {
                    component9.text = configTable2.GetField_String("cardeffectdes");
                    component10.text = "+" + cardEffectItem2.value;
                    num += this.GetAttributeFightValue(configTable2.GetField_String("protype"), cardEffectItem2.value);
                }
                else
                {
                    Debug.LogError("card_data.rand_effect.id=0");
                }
                transform11.gameObject.SetActive(true);
            }
            else
            {
                transform11.gameObject.SetActive(false);
            }
        }
        for (int num2 = 0; num2 < list3.Count; num2++)
        {
            list3[num2].gameObject.SetActive(false);
        }
        if (transform8)
        {
            Transform transform12 = transform8.Find("Text");
            transform12.GetComponent<Text>().text = this.mConfig.GetField_Uint("sell_price").ToString();
            transform8.gameObject.SetActive(UIManager.GetUIObject<UI_NPCshop>() != null);
        }
        LuaTable configTable3 = LuaConfigManager.GetConfigTable("carddata_config", (ulong)this.mConfig.GetField_Uint("id"));
        if (configTable3 != null)
        {
            uint field_Uint = configTable3.GetField_Uint("maxdurable");
            if (component4)
            {
                component4.text = this.mData.card_data.durability + "/" + field_Uint;
            }
            if (transform9)
            {
                Transform transform13 = transform9.Find("Text");
                uint cacheField_Uint = LuaConfigManager.GetXmlConfigTable("newUserInit").GetCacheField_Uint("maxdurablegold");
                transform13.GetComponent<Text>().text = ((field_Uint - this.mData.card_data.durability) * cacheField_Uint).ToString();
            }
        }
        fightValueNum.SetNum((uint)num);
        transform.gameObject.SetActive(true);
    }

    internal void SetupGuildSkillPanel(uint skillId)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            Transform child = this.Root.GetChild(i);
            child.localPosition = Vector3.zero;
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        Transform transform = this.Root.Find("familyskill");
        transform.gameObject.SetActive(true);
        GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
        bool flag = controller.CheckGuildSkillIsLearn(skillId);
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_guild");
        bool active = false;
        for (int j = 0; j < configTableList.Count; j++)
        {
            uint cacheField_Uint = configTableList[j].GetCacheField_Uint("skillid");
            if (cacheField_Uint == skillId)
            {
                transform.Find("txt_desc").GetComponent<Text>().text = configTableList[j].GetCacheField_String("desc");
                string cacheField_String = configTableList[j].GetCacheField_String("skillicon");
                Image icon = transform.Find("Skill_name/Icon").GetComponent<Image>();
                UITextureMgr.Instance.GetTexture(ImageType.ITEM, cacheField_String, delegate (UITextureAsset item)
                {
                    if (icon == null)
                    {
                        return;
                    }
                    if (item != null && item.textureObj != null)
                    {
                        Sprite overrideSprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                        icon.overrideSprite = overrideSprite;
                        this.usedTextureAssets.Add(item);
                    }
                });
                transform.Find("Skill_name/Info/Name").GetComponent<Text>().text = configTableList[j].GetCacheField_String("skillname");
                transform.Find("Skill_name/Info/txt_lv").GetComponent<Text>().text = skillId % 100U + "级";
                if (!flag)
                {
                    break;
                }
            }
            if (cacheField_Uint == skillId + 1U && flag)
            {
                active = true;
                transform.Find("nextlv/txt_desc").GetComponent<Text>().text = configTableList[j].GetCacheField_String("desc");
            }
        }
        transform.Find("nextlv").gameObject.SetActive(active);
    }

    private void SetupHead(Transform transHead)
    {
        Transform transImgIcon = transHead.Find("Item_name/img_icon/img_item");
        Transform transform = transHead.Find("Item_name/info/txt_name");
        if (transImgIcon)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, this.mConfig.GetField_String("icon"), delegate (UITextureAsset asset)
            {
                Image component2 = transImgIcon.GetComponent<Image>();
                if (asset != null && component2 != null)
                {
                    Texture2D textureObj = asset.textureObj;
                    Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                    component2.sprite = sprite;
                    component2.overrideSprite = sprite;
                    component2.color = Color.white;
                    component2.gameObject.SetActive(true);
                    component2.material = null;
                }
            });
        }
        if (transform)
        {
            transform.GetComponent<Text>().text = this.mConfig.GetField_String("name");
            transform.GetComponent<Text>().color = CommonTools.GetQualityColor(this.mConfig.GetField_Uint("quality"));
        }
        Transform transform2 = transHead.Find("Item_name/info/txt_bind");
        Transform transform3 = transHead.Find("Item_name/info/txt_bind2");
        Transform transform4 = transHead.Find("Item_name/info/txt_binded");
        transform2.gameObject.SetActive(false);
        transform3.gameObject.SetActive(this.mData.bind == 0U);
        transform4.gameObject.SetActive(this.mData.bind != 0U);
        Transform transform5 = transHead.Find("txt_desc");
        if (transform5)
        {
            Text component = transform5.GetComponent<Text>();
            string field_String = this.mConfig.GetField_String("desc");
            component.text = GlobalRegister.ConfigColorToRichTextFormat(field_String);
            component.text = component.text.Replace("\\n", "\n");
        }
    }

    private void SetupGeneBag()
    {
        Transform transform = this.Root.Find("Gene_bag");
        Transform transform2 = transform.Find("Bottom/Panel_sell");
        this.SetupHead(transform.Find("Head"));
        if (transform2)
        {
            transform2.gameObject.SetActive(false);
            Transform transform3 = transform2.Find("txt_value");
            transform3.GetComponent<Text>().text = this.mConfig.GetField_Uint("sell_price").ToString();
            transform2.gameObject.SetActive(UIManager.GetUIObject<UI_NPCshop>() != null);
        }
        transform.gameObject.SetActive(true);
    }

    private void SetupGene()
    {
        Transform transGene = this.Root.Find("Gene");
        GeneController controller = ControllerManager.Instance.GetController<GeneController>();
        if (transGene)
        {
            uint num = this.mData.level + this.mData.baseid;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("chipicon"), delegate (UITextureAsset asset)
            {
                Image component3 = transGene.Find("Head/Item_name/img_icon/img_item").GetComponent<Image>();
                if (asset != null && component3 != null)
                {
                    Texture2D textureObj = asset.textureObj;
                    Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                    component3.sprite = sprite;
                    component3.overrideSprite = sprite;
                    component3.color = Color.white;
                    component3.gameObject.SetActive(true);
                    component3.material = null;
                }
            });
            transGene.Find("Head/Item_name/info/txt_name").GetComponent<Text>().text = configTable.GetCacheField_String("chipname");
            transGene.Find("Head/Item_name/info/txt_lv").GetComponent<Text>().text = CommonTools.GetLevelFormat(this.mData.level);
            Transform transform = transGene.Find("Att");
            GameObject gameObject = transform.GetChild(0).gameObject;
            string[] array = configTable.GetCacheField_String("property").Split(new char[]
            {
                '|'
            });
            int num2 = Mathf.Max(array.Length, transform.childCount);
            for (int i = 0; i < num2; i++)
            {
                GameObject gameObject2;
                if (i < transform.childCount)
                {
                    gameObject2 = transform.GetChild(i).gameObject;
                }
                else
                {
                    gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.name = gameObject.name;
                    gameObject2.transform.SetParent(transform, false);
                }
                if (i < array.Length)
                {
                    string[] array2 = array[i].Split(new char[]
                    {
                        ','
                    });
                    gameObject2.gameObject.SetActive(true);
                    if (array2.Length >= 2)
                    {
                        Text component = gameObject2.transform.Find("txt_name").GetComponent<Text>();
                        Text component2 = gameObject2.transform.Find("txt_value").GetComponent<Text>();
                        component.text = controller.GetAttShowNameByConfigKey(array2[0]);
                        component2.text = "+" + array2[1];
                    }
                    else
                    {
                        FFDebug.LogError(this, "Dna config data formart error!");
                    }
                }
                else
                {
                    gameObject2.gameObject.SetActive(false);
                }
            }
        }
        transGene.gameObject.SetActive(true);
    }

    private void SetupQuestItem()
    {
        Transform transform = this.Root.Find("QuestItem");
        Transform transform2 = transform.Find("Bottom/Panel_sell");
        this.SetupHead(transform.Find("Head"));
        if (transform2)
        {
            transform2.gameObject.SetActive(false);
        }
        Transform transform3 = transform.Find("Bottom/txt_use");
        transform3.gameObject.SetActive(this.IsItemCanUse());
        transform.gameObject.SetActive(true);
    }

    private void SetupUseItem()
    {
        this.SetupHead(this.useItemHead);
        this.useItemlevel.text = CommonTools.GetLevelFormat(this.mConfig.GetField_Uint("uselevel"));
        if (this.useItemPanelSell)
        {
            this.useItemValue.text = this.mConfig.GetField_Uint("sell_price").ToString();
            this.useItemPanelSell.gameObject.SetActive(UIManager.GetUIObject<UI_NPCshop>() != null);
        }
        this.useItemUseObj.gameObject.SetActive(this.IsItemCanUse());
        if (!this.transUseItem.gameObject.activeSelf)
        {
            this.transUseItem.gameObject.SetActive(true);
        }
    }

    private bool IsItemCanUse()
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("btnaction").GetCacheField_Table("btnactionmapbyid").GetCacheField_Table(this.mData.baseid);
        if (cacheField_Table == null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)this.mData.baseid);
            if (configTable != null)
            {
                uint field_Uint = configTable.GetField_Uint("type");
                cacheField_Table = LuaConfigManager.GetXmlConfigTable("btnaction").GetCacheField_Table("btnactionmap").GetCacheField_Table(field_Uint.ToString());
                if (cacheField_Table != null && cacheField_Table.GetCacheField_String("actions") == "use")
                {
                    return true;
                }
            }
        }
        else if (cacheField_Table.GetCacheField_String("actions") == "use")
        {
            return true;
        }
        return false;
    }

    public void SetupAttributeDetailPanel(Dictionary<string, int> allAttrDic)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            this.Root.GetChild(i).localPosition = Vector3.zero;
            this.Root.GetChild(i).gameObject.SetActive(false);
        }
        Transform transform = this.Root.Find("AttributeDetail");
        Transform transform2 = transform.Find("detail/Panel/Attr");
        UIManager.Instance.ClearListChildrens(transform2.parent, 1);
        foreach (string text in allAttrDic.Keys)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform2.gameObject);
            gameObject.transform.SetParent(transform2.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            gameObject.transform.Find("Name").GetComponent<Text>().text = text;
            gameObject.transform.Find("Value/total").GetComponent<Text>().text = allAttrDic[text].ToString();
            gameObject.transform.Find("Value/extra").GetComponent<Text>().text = string.Empty;
        }
        transform.gameObject.SetActive(true);
    }

    private float GetAttributeFightValue(string attributeName, uint value)
    {
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("FightCoefficient");
        float num = 0f;
        switch (attributeName)
        {
            case "maxhp":
                num = xmlConfigTable.GetCacheField_Float("hpfight");
                break;
            case "pdam":
                num = xmlConfigTable.GetCacheField_Float("pfight");
                break;
            case "pdef":
                num = xmlConfigTable.GetCacheField_Float("pdeffight");
                break;
            case "mdam":
                num = xmlConfigTable.GetCacheField_Float("mfight");
                break;
            case "mdef":
                num = xmlConfigTable.GetCacheField_Float("mdeffight");
                break;
            case "hit":
                num = xmlConfigTable.GetCacheField_Float("hitfight");
                break;
            case "miss":
                num = xmlConfigTable.GetCacheField_Float("missfight");
                break;
            case "bang":
                num = xmlConfigTable.GetCacheField_Float("bangfight");
                break;
            case "toughness":
                num = xmlConfigTable.GetCacheField_Float("toughnessfight");
                break;
            case "penetrate":
                num = xmlConfigTable.GetCacheField_Float("penetratefight");
                break;
            case "block":
                num = xmlConfigTable.GetCacheField_Float("blockfight");
                break;
            case "accurate":
                num = xmlConfigTable.GetCacheField_Float("accuratefight");
                break;
            case "deflect":
                num = xmlConfigTable.GetCacheField_Float("deflectfight");
                break;
            case "bangextradamage":
                num = xmlConfigTable.GetCacheField_Float("bangextradamfight");
                break;
            case "toughnessreducedamage":
                num = xmlConfigTable.GetCacheField_Float("toughness_reducedamfight");
                break;
            case "penetrateextradamage":
                num = xmlConfigTable.GetCacheField_Float("penetrate_extra_damfight");
                break;
            case "blockreducedamage":
                num = xmlConfigTable.GetCacheField_Float("block_reduce_damfight");
                break;
            case "accurateextradamage":
                num = xmlConfigTable.GetCacheField_Float("accurate_extra_damfight");
                break;
            case "deflectreducedamage":
                num = xmlConfigTable.GetCacheField_Float("deflect_reduce_damfight");
                break;
        }
        return value * num;
    }

    public void SetupSkillPanel(MainPlayerSkillBase skillData)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            Transform child = this.Root.GetChild(i);
            child.localPosition = Vector3.zero;
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, skillData.GetCurSkillIconName(), delegate (UITextureAsset asset)
        {
            if (asset != null && this.transIcon != null)
            {
                Texture2D textureObj = asset.textureObj;
                Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                this.transIcon.sprite = sprite;
                this.transIcon.overrideSprite = sprite;
                this.transIcon.color = Color.white;
                this.transIcon.gameObject.SetActive(true);
                this.transIcon.material = null;
            }
        });
        LuaTable luaTable = MainPlayerSkillHolder.Instance.Getskill_lv_config(skillData.Skillid, skillData.Level);
        if (luaTable == null)
        {
            FFDebug.LogError(this, string.Format("InstallPlayerSkill: ID: {0}  skilllevel: {1}  ", skillData.Skillid, skillData.Level));
            return;
        }
        this.transName.text = luaTable.GetField_String("skillname");
        bool flag = luaTable.GetField_Uint("canbe_passive") == 0U;
        this.transType.text = ((!flag) ? "被动技能" : "主动技能");
        this.transLevel.text = CommonTools.GetLevelFormat(skillData.Level);
        this.transTitlePanel.gameObject.SetActive(flag);
        if (this.transTitlePanel && flag)
        {
            this.transDistance.text = skillData.AttackRange / 3f + "米";
            this.transTime.text = luaTable.GetField_Uint("chanttime") / 1000f + "秒";
            uint field_Uint = luaTable.GetField_Uint("dtime");
            this.transCd.text = field_Uint / 1000U + "秒";
        }
        this.transDesc.text = GlobalRegister.ConfigColorToRichTextFormat(luaTable.GetField_String("desc"));
        if (!this.transSkillPanel.gameObject.activeInHierarchy)
        {
            this.transSkillPanel.gameObject.SetActive(true);
        }
    }

    public void SetupSkinPanel(int skinid, bool getted)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            Transform child = this.Root.GetChild(i);
            child.localPosition = Vector3.zero;
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)((long)skinid));
        if (configTable == null)
        {
            FFDebug.LogError(this, string.Format("avatar_config: ID: {0} ", skinid));
            return;
        }
        this.skinname.text = configTable.GetField_String("name");
        this.skindesc.text = configTable.GetField_String("desc");
        if (getted)
        {
            this.skinGetted.text = CommonTools.GetnoticeById(4021UL);
        }
        else
        {
            int field_Int = configTable.GetField_Int("unlockitem");
            int field_Int2 = configTable.GetField_Int("unlockitem");
            string text = string.Empty;
            string text2 = string.Empty;
            if (field_Int > 0)
            {
                text = CommonTools.GetnoticeById(4019UL);
                LuaTable configTable2 = LuaConfigManager.GetConfigTable("objects", (ulong)((long)field_Int));
                if (configTable2 != null)
                {
                    text2 = configTable2.GetField_String("name");
                }
            }
            else if (field_Int2 > 0)
            {
                text = CommonTools.GetTextById(4018UL);
                LuaTable configTable3 = LuaConfigManager.GetConfigTable("evolution_config", (ulong)((long)field_Int2));
                if (configTable3 != null)
                {
                    text2 = configTable3.GetField_String("name");
                }
            }
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
            {
                this.skinGetted.text = string.Format(text, text2);
            }
            else
            {
                this.skinGetted.text = string.Empty;
            }
        }
        this.skinRoot.gameObject.SetActive(true);
    }

    public void SetupHeroAwakePanel(int awakeheroid)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            Transform child = this.Root.GetChild(i);
            child.localPosition = Vector3.zero;
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("evolution_config", (ulong)((long)awakeheroid));
        if (configTable == null)
        {
            FFDebug.LogError(this, string.Format("evolution_config: ID: {0} ", awakeheroid));
            return;
        }
        GeneController controller = ControllerManager.Instance.GetController<GeneController>();
        int num = 0;
        this.heroAwakeName.text = configTable.GetField_String("name");
        this.heroDesc.text = configTable.GetField_String("desc").Replace("\\n", "\n");
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("pdam"), "pdam", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("mdam"), "mdam", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("pdef"), "pdef", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("mdef"), "mdef", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("maxhp"), "maxhp", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("bang"), "bang", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("bangextradamage"), "bangextradamage", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("toughness"), "toughness", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("toughnessreducedamage"), "toughnessreducedamage", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("hit"), "hit", ref num);
        this.doAddHeroAwakeProperty(controller, configTable.GetField_Int("miss"), "miss", ref num);
        int field_Int = configTable.GetField_Int("avatar");
        this.doAddHeroAwakeAvatarProperty(field_Int, ref num);
        int field_Int2 = configTable.GetField_Int("skillid");
        this.doAddHeroAwakeSkillProperty(field_Int2, ref num);
        for (int j = num; j < this.heroAwakeInfoList.Count; j++)
        {
            this.heroAwakeInfoList[j].gameObject.SetActive(false);
        }
        this.heroAwakeRoot.gameObject.SetActive(true);
    }

    private void doAddHeroAwakeAvatarProperty(int fieldvalue, ref int index)
    {
        if (fieldvalue > 0)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)((long)fieldvalue));
            if (configTable == null)
            {
                return;
            }
            Text text;
            if (this.heroAwakeInfoList.Count > index)
            {
                text = this.heroAwakeInfoList[index];
            }
            else
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.heroAwakeInfoSample);
                gameObject.transform.parent = this.heroAwakeInfoSample.transform.parent;
                gameObject.transform.localScale = Vector3.one;
                text = gameObject.GetComponent<Text>();
                this.heroAwakeInfoList.Add(text);
            }
            index++;
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("textconfig", 4024UL);
            string field_String = configTable.GetField_String("name");
            text.text = string.Format(configTable2.GetField_String("notice"), field_String);
            text.gameObject.SetActive(true);
        }
    }

    private void doAddHeroAwakeSkillProperty(int fieldvalue, ref int index)
    {
        if (fieldvalue > 0)
        {
            LuaTable luaTable = MainPlayerSkillHolder.Instance.Getskill_lv_config((uint)fieldvalue, 1U);
            if (luaTable == null)
            {
                return;
            }
            Text text;
            if (this.heroAwakeInfoList.Count > index)
            {
                text = this.heroAwakeInfoList[index];
            }
            else
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.heroAwakeInfoSample);
                gameObject.transform.parent = this.heroAwakeInfoSample.transform.parent;
                gameObject.transform.localScale = Vector3.one;
                text = gameObject.GetComponent<Text>();
                this.heroAwakeInfoList.Add(text);
            }
            index++;
            LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", 4023UL);
            string field_String = luaTable.GetField_String("skillname");
            text.text = string.Format(configTable.GetField_String("notice"), field_String);
            text.gameObject.SetActive(true);
        }
    }

    private void doAddHeroAwakeProperty(GeneController gc, int fieldvalue, string property, ref int index)
    {
        if (fieldvalue > 0)
        {
            Text text;
            if (this.heroAwakeInfoList.Count > index)
            {
                text = this.heroAwakeInfoList[index];
            }
            else
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.heroAwakeInfoSample);
                gameObject.transform.parent = this.heroAwakeInfoSample.transform.parent;
                gameObject.transform.localScale = Vector3.one;
                text = gameObject.GetComponent<Text>();
                this.heroAwakeInfoList.Add(text);
            }
            index++;
            text.text = gc.GetAttShowNameByConfigKey(property) + "+" + fieldvalue.ToString() + "%";
            text.gameObject.SetActive(true);
        }
    }

    public void SetupBuffPanel(UserState Flag, List<ulong> effects)
    {
        for (int i = 0; i < this.Root.childCount; i++)
        {
            Transform child = this.Root.GetChild(i);
            child.localPosition = Vector3.zero;
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
        }
        Transform transform = this.Root.Find("BuffIcon");
        Transform transform2 = transform.Find("Head/txt_name");
        Transform transform3 = transform.Find("Head/txt_desc");
        LuaTable bufferConfig = ManagerCenter.Instance.GetManager<BufferStateManager>().GetBufferConfig(Flag);
        if (bufferConfig == null)
        {
            return;
        }
        transform2.GetComponent<Text>().text = bufferConfig.GetCacheField_String("name");
        string arg = string.Empty;
        string arg2 = string.Empty;
        string arg3 = string.Empty;
        if (effects != null && effects.Count > 0)
        {
            for (int j = 0; j < effects.Count; j++)
            {
                if (j == 0)
                {
                    arg = effects[j].ToString();
                }
                if (j == 1)
                {
                    arg2 = effects[j].ToString();
                }
                if (j == 2)
                {
                    arg3 = effects[j].ToString();
                }
            }
        }
        transform3.GetComponent<Text>().text = string.Format(bufferConfig.GetCacheField_String("introduce"), arg, arg2, arg3);
        transform.gameObject.SetActive(true);
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public Transform Root;

    private GameObject skinRoot;

    private Text skinname;

    private Text skindesc;

    private Text skinGetted;

    private GameObject heroAwakeRoot;

    private Text heroAwakeName;

    private GameObject heroAwakeInfoSample;

    private Text heroDesc;

    private List<Text> heroAwakeInfoList = new List<Text>();

    private Image transIcon;

    private Text transName;

    private Text transType;

    private Text transLevel;

    private Transform transTitlePanel;

    private Text transDesc;

    private Text transDistance;

    private Text transTime;

    private Text transCd;

    private Transform transSkillPanel;

    private Transform transUseItem;

    private Transform useItemPanelSell;

    private Transform useItemHead;

    private Text useItemlevel;

    private Text useItemValue;

    private Transform useItemUseObj;

    private t_Object mData;

    private LuaTable mConfig;
}
