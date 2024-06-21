using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Pet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Pet : UIPanelBase
{
    private PetController petController
    {
        get
        {
            return ControllerManager.Instance.GetController<PetController>();
        }
    }

    public void EnterPetList()
    {
        this.initEvent();
        this.setMenu(UI_Pet.MenuState.Attribute);
        this.ViewPetList();
    }

    private void initEvent()
    {
        UIEventListener.Get(this.PanelPet.Find("background/btn_close").gameObject).onClick = delegate (PointerEventData dat)
        {
            this.petController.ClosePetList();
        };
        UIEventListener.Get(this.MenuOff.transform.Find("info").gameObject).onClick = delegate (PointerEventData pointData)
        {
            this.setMenu(UI_Pet.MenuState.Attribute);
            this.RefreshCurPetInfo(this.curPetInfo);
        };
        UIEventListener.Get(this.MenuOff.transform.Find("skill").gameObject).onClick = delegate (PointerEventData pointData)
        {
            this.setMenu(UI_Pet.MenuState.Skill);
            this.RefreshCurPetInfo(this.curPetInfo);
        };
    }

    private void setMenu(UI_Pet.MenuState state)
    {
        this.uiState = state;
        if (this.uiState == UI_Pet.MenuState.Attribute)
        {
            this.MenuOff.Find("info").gameObject.SetActive(false);
            this.MenuOff.Find("skill").gameObject.SetActive(true);
            this.MenuOn.Find("info").gameObject.SetActive(true);
            this.MenuOn.Find("skill").gameObject.SetActive(false);
            this.attributeInfo.gameObject.SetActive(true);
            this.skillInfo.gameObject.SetActive(false);
        }
        else
        {
            this.MenuOff.Find("info").gameObject.SetActive(true);
            this.MenuOff.Find("skill").gameObject.SetActive(false);
            this.MenuOn.Find("info").gameObject.SetActive(false);
            this.MenuOn.Find("skill").gameObject.SetActive(true);
            this.attributeInfo.gameObject.SetActive(false);
            this.skillInfo.gameObject.SetActive(true);
        }
    }

    public void ViewPetList()
    {
        GameObject gameObject = this.petList.Find("Rect/Item").gameObject;
        this.ListPetData.Clear();
        for (int i = 0; i < this.petController.ListPetData.Count; i++)
        {
            this.ListPetData.Add(this.petController.ListPetData[i]);
        }
        PetBaseComparerPetList comparer = new PetBaseComparerPetList();
        this.ListPetData.Sort(comparer);
        gameObject.SetActive(false);
        if (this.ListPetData.Count == 0)
        {
            this.PanelPet.Find("txt_none").gameObject.SetActive(true);
        }
        else
        {
            this.PanelPet.Find("txt_none").gameObject.SetActive(false);
        }
        if (this.ListPetData.Count > 0)
        {
            if (this.curPetInfo == null)
            {
                this.RefreshCurPetInfo(this.ListPetData[0]);
            }
            this.basicInfo.gameObject.SetActive(true);
        }
        else
        {
            this.basicInfo.gameObject.SetActive(false);
            this.attributeInfo.gameObject.SetActive(false);
            this.skillInfo.gameObject.SetActive(false);
        }
        int num = 0;
        while ((long)num < (long)((ulong)(MainPlayer.Self.petBarCount + 1U)))
        {
            int field_Int = LuaConfigManager.GetXmlConfigTable("petConfig").GetField_Table("maxPetCount").GetField_Int("value");
            if (num >= field_Int)
            {
                break;
            }
            GameObject gameObject2;
            if (num < this.listPetObj.Count)
            {
                gameObject2 = this.listPetObj[num];
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.parent = gameObject.transform.parent;
                gameObject2.transform.localScale = gameObject.transform.localScale;
                gameObject2.transform.localPosition = gameObject.transform.localPosition;
                this.listPetObj.Add(gameObject2);
            }
            if (this.curSelect == null && num == 0)
            {
                this.curSelect = gameObject2.transform.Find("sp_selected").gameObject;
                this.curSelect.SetActive(true);
            }
            if (this.ListPetData.Count > num)
            {
                this.setPetListItem(gameObject2, this.ListPetData[num], false);
                if (this.curPetInfo != null && this.curPetInfo.tempid == this.ListPetData[num].tempid)
                {
                    if (this.curSelect == null)
                    {
                        this.curSelect = gameObject2.transform.Find("sp_selected").gameObject;
                        this.curSelect.SetActive(true);
                    }
                    else
                    {
                        this.curSelect.SetActive(false);
                        this.curSelect = gameObject2.transform.Find("sp_selected").gameObject;
                        this.curSelect.SetActive(true);
                    }
                }
            }
            else
            {
                gameObject2.transform.Find("sp_selected").gameObject.SetActive(false);
                if ((long)num != (long)((ulong)MainPlayer.Self.petBarCount))
                {
                    this.setPetListItem(gameObject2, null, false);
                }
                else
                {
                    this.setPetListItem(gameObject2, null, true);
                }
            }
            num++;
        }
    }

    private void setPetListItem(GameObject ga, PetBase data, bool locked)
    {
        ga.SetActive(true);
        if (data != null)
        {
            ga.transform.Find("pet").gameObject.SetActive(true);
            ga.transform.Find("blank").gameObject.SetActive(false);
            ga.transform.Find("lock").gameObject.SetActive(false);
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)data.id);
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("summonpet", (ulong)data.id);
            if (string.IsNullOrEmpty(data.name))
            {
                ga.transform.Find("pet/txt_petname").GetComponent<Text>().text = configTable.GetField_String("name");
            }
            else
            {
                ga.transform.Find("pet/txt_petname").GetComponent<Text>().text = data.name.ToString();
            }
            string modelColor = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item" + data.quality);
            ga.transform.Find("pet/txt_petname").GetComponent<Text>().color = CommonTools.HexToColor(modelColor);
            ga.transform.Find("pet/txt_lv").GetComponent<Text>().text = data.level.ToString();
            ClassSetItemTexture.Instance.setTexture(ga.transform.Find("pet/img_bg/img_pet").GetComponent<RawImage>(), configTable2.GetField_String("icon"), ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
            if (data.state.Contains(PetState.PetState_Fight))
            {
                ga.transform.Find("pet/state/img_fight").gameObject.SetActive(true);
                ga.transform.Find("pet/state/img_prepare").gameObject.SetActive(false);
                ga.transform.Find("pet/state/img_rest").gameObject.SetActive(false);
            }
            else if (data.state.Contains(PetState.PetState_Assist))
            {
                ga.transform.Find("pet/state/img_fight").gameObject.SetActive(false);
                ga.transform.Find("pet/state/img_prepare").gameObject.SetActive(true);
                ga.transform.Find("pet/state/img_rest").gameObject.SetActive(false);
            }
            else if (data.state.Contains(PetState.PetState_Rest))
            {
                ga.transform.Find("pet/state/img_fight").gameObject.SetActive(false);
                ga.transform.Find("pet/state/img_prepare").gameObject.SetActive(false);
                ga.transform.Find("pet/state/img_rest").gameObject.SetActive(true);
            }
            if (data.state.Contains(PetState.PetState_Dying))
            {
                ga.transform.Find("pet/img_bg/img_mask").gameObject.SetActive(true);
            }
            else
            {
                ga.transform.Find("pet/img_bg/img_mask").gameObject.SetActive(false);
            }
            string spritename = string.Empty;
            switch (data.quality)
            {
                case 1U:
                    spritename = "st0099";
                    break;
                case 2U:
                    spritename = "st0100";
                    break;
                case 3U:
                    spritename = "st0101";
                    break;
                case 4U:
                    spritename = "st0102";
                    break;
            }
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename, delegate (Sprite sprite)
            {
                ga.transform.Find("pet/img_bg").GetComponent<Image>().overrideSprite = sprite;
            });
            UIEventListener.Get(ga).onClick = delegate (PointerEventData eventdata)
            {
                this.curSelect.SetActive(false);
                this.curSelect = ga.transform.Find("sp_selected").gameObject;
                this.curSelect.SetActive(true);
                this.RefreshCurPetInfo(data);
            };
        }
        else if (!locked)
        {
            ga.transform.Find("pet").gameObject.SetActive(false);
            ga.transform.Find("blank").gameObject.SetActive(true);
            ga.transform.Find("lock").gameObject.SetActive(false);
        }
        else
        {
            ga.transform.Find("pet").gameObject.SetActive(false);
            ga.transform.Find("blank").gameObject.SetActive(false);
            ga.transform.Find("lock").gameObject.SetActive(true);
            UIEventListener.Get(ga.transform.Find("lock").gameObject).onClick = delegate (PointerEventData evedata)
            {
                this.SetUnlockWindow();
            };
        }
    }

    private void addTextureAsset(UITextureAsset asset)
    {
        this.usedTextureAssets.Add(asset);
    }

    private void btnUnLockPit()
    {
        this.petController.UnLockPetBar();
    }

    private void SetUnlockWindow()
    {
        LuaTable field_Table = LuaConfigManager.GetXmlConfigTable("petConfig").GetField_Table("unlockcost").GetField_Table((MainPlayer.Self.petBarUnlockcount + 1U).ToString());
        if (field_Table == null)
        {
            return;
        }
        string field_String = field_Table.GetField_String("value");
        string[] array = field_String.Split(new char[]
        {
            '-'
        }, StringSplitOptions.RemoveEmptyEntries);
        if (array.Length != 2)
        {
            FFDebug.LogWarning(this, "XmlManager.Instance.XmlData.petConfig    is   wrong ");
            return;
        }
        uint num = uint.Parse(array[0]);
        LuaTable itemConfig = LuaConfigManager.GetConfigTable("objects", (ulong)num);
        if ((uint)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            num
        })[0] >= uint.Parse(array[1]))
        {
            this.Panel_Extend.Find("info/txt_number").GetComponent<Text>().color = Color.white;
            UIEventListener.Get(this.Panel_Extend.Find("btn_ok").gameObject).onClick = delegate (PointerEventData evedata)
            {
                this.btnUnLockPit();
                this.Panel_Extend.gameObject.SetActive(false);
            };
        }
        else
        {
            this.Panel_Extend.Find("info/txt_number").GetComponent<Text>().color = Color.red;
            UIEventListener.Get(this.Panel_Extend.Find("btn_ok").gameObject).onClick = delegate (PointerEventData evedata)
            {
                TipsWindow.ShowWindow(TipsType.ITEM_NOT_ENOUGH, new string[]
                {
                    itemConfig.GetField_String("name")
                });
            };
        }
        this.Panel_Extend.Find("info/txt_number").GetComponent<Text>().text = array[1];
        ClassSetItemTexture.Instance.SetItemTexture(this.Panel_Extend.Find("info/img_icon").GetComponent<Image>(), num, ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
        this.Panel_Extend.gameObject.SetActive(true);
        UIEventListener.Get(this.Panel_Extend.Find("btn_close").gameObject).onClick = delegate (PointerEventData evedata)
        {
            this.Panel_Extend.gameObject.SetActive(false);
        };
        UIEventListener.Get(this.Panel_Extend.Find("btn_cancel").gameObject).onClick = delegate (PointerEventData evedata)
        {
            this.Panel_Extend.gameObject.SetActive(false);
        };
    }

    public void RefreshCurPetInfo(PetBase data)
    {
        this.curPetInfo = data;
        this.setPetBasic(data);
        if (this.uiState == UI_Pet.MenuState.Attribute)
        {
            this.setPetAttributInfo(data);
        }
        else
        {
            this.setPetSkill(data);
        }
    }

    private void setPetBasic(PetBase data)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("summonpet", (ulong)data.id);
        string field_String = LuaConfigManager.GetXmlConfigTable("petConfig").GetField_Table("TypeName").GetField_Table("petType").GetField_Table(configTable.GetField_String("type")).GetField_String("value");
        this.basicInfo.transform.Find("name/type/img_bg/txt_type").GetComponent<Text>().text = field_String;
        if (string.IsNullOrEmpty(data.name))
        {
            LuaTable configTable2 = LuaConfigManager.GetConfigTable("npc_data", (ulong)data.id);
            this.basicInfo.transform.Find("name/txt_petname").GetComponent<Text>().text = configTable2.GetField_String("name");
        }
        else
        {
            this.basicInfo.transform.Find("name/txt_petname").GetComponent<Text>().text = data.name;
        }
        string modelColor = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item" + data.quality);
        this.basicInfo.transform.Find("name/txt_petname").GetComponent<Text>().color = CommonTools.HexToColor(modelColor);
        switch (configTable.GetField_Uint("type"))
        {
            case 1U:
                this.basicInfo.transform.Find("name/type/img_bg/txt_type").GetComponent<Text>().color = Const.GetColorByName("element1");
                this.basicInfo.transform.Find("name/type/img_bg").GetComponent<Image>().color = Const.GetColorByName("element1bg");
                break;
            case 2U:
                this.basicInfo.transform.Find("name/type/img_bg/txt_type").GetComponent<Text>().color = Const.GetColorByName("element2");
                this.basicInfo.transform.Find("name/type/img_bg").GetComponent<Image>().color = Const.GetColorByName("element2bg");
                break;
            case 3U:
                this.basicInfo.transform.Find("name/type/img_bg/txt_type").GetComponent<Text>().color = Const.GetColorByName("element3");
                this.basicInfo.transform.Find("name/type/img_bg").GetComponent<Image>().color = Const.GetColorByName("element3bg");
                break;
            case 4U:
                this.basicInfo.transform.Find("name/type/img_bg/txt_type").GetComponent<Text>().color = Const.GetColorByName("element4");
                this.basicInfo.transform.Find("name/type/img_bg").GetComponent<Image>().color = Const.GetColorByName("element4bg");
                break;
            default:
                this.basicInfo.transform.Find("name/type/img_bg/txt_type").GetComponent<Text>().color = Const.GetColorByName("element5");
                this.basicInfo.transform.Find("name/type/img_bg").GetComponent<Image>().color = Const.GetColorByName("element5bg");
                break;
        }
        ClassSetItemTexture.Instance.setTexture(this.attributeInfo.Find("img_petbg/img_pet").GetComponent<Image>(), configTable.GetField_String("picture"), ImageType.ROLES, new Action<UITextureAsset>(this.addTextureAsset));
    }

    private void setPetAttributInfo(PetBase data)
    {
        Transform transform = this.attributeInfo.Find("level");
        LuaTable configTable = LuaConfigManager.GetConfigTable("summonpetLevelUp", (ulong)data.level);
        transform.Find("txt_level").GetComponent<Text>().text = data.level.ToString();
        transform.Find("txt_exp").GetComponent<Text>().text = data.exp.ToString() + "/" + configTable.GetField_String("levelupexp");
        transform.Find("exp").GetComponent<Slider>().value = data.exp / configTable.GetField_Uint("levelupexp");
        Transform transform2 = this.attributeInfo.Find("status");
        transform2.Find("pdamamge/txt_value").GetComponent<Text>().text = data.prop.pdamage.ToString();
        transform2.Find("pdefence/txt_value").GetComponent<Text>().text = data.prop.pdefence.ToString();
        transform2.Find("maxhp/txt_value").GetComponent<Text>().text = data.prop.maxhp.ToString();
        transform2.Find("tough/txt_value").GetComponent<Text>().text = data.prop.toughness.ToString();
        transform2.Find("bang/txt_value").GetComponent<Text>().text = data.prop.bang.ToString();
        transform2.Find("accurate/txt_value").GetComponent<Text>().text = data.prop.accurate.ToString();
        transform2.Find("hold/txt_value").GetComponent<Text>().text = data.prop.hold.ToString();
        transform2.Find("hit/txt_value").GetComponent<Text>().text = data.prop.hit.ToString();
        transform2.Find("miss/txt_value").GetComponent<Text>().text = data.prop.miss.ToString();
        Transform transform3 = transform2.Find("pdamamge/prolist");
        Transform transform4 = transform2.Find("pdefence/prolist");
        Transform transform5 = transform2.Find("maxhp/prolist");
        Transform transform6 = transform2.Find("tough/prolist");
        Transform transform7 = transform2.Find("bang/prolist");
        Transform transform8 = transform2.Find("accurate/prolist");
        Transform transform9 = transform2.Find("hold/prolist");
        Transform transform10 = transform2.Find("hit/prolist");
        Transform transform11 = transform2.Find("miss/prolist");
        for (int i = 0; i < 5; i++)
        {
            if ((long)i < (long)((ulong)(data.aptitude.pdamage - 10U)))
            {
                transform3.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform3.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.pdefence - 10U)))
            {
                transform4.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform4.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.maxhp - 10U)))
            {
                transform5.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform5.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.toughness - 10U)))
            {
                transform6.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform6.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.bang - 10U)))
            {
                transform7.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform7.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.accurate - 10U)))
            {
                transform8.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform8.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.hold - 10U)))
            {
                transform9.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform9.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.hit - 10U)))
            {
                transform10.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform10.GetChild(i).gameObject.SetActive(false);
            }
            if ((long)i < (long)((ulong)(data.aptitude.miss - 10U)))
            {
                transform11.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform11.GetChild(i).gameObject.SetActive(false);
            }
        }
        this.setBtnView(false);
        this.initAttributeBtnActiov(data);
    }

    private void initAttributeBtnActiov(PetBase data)
    {
        GameObject gameObject = this.attributeInfo.Find("btn/btn_select/Text").gameObject;
        GameObject gameObject2 = this.attributeInfo.Find("btn/btn_other1/Text").gameObject;
        GameObject gameObject3 = this.attributeInfo.Find("btn/btn_other2/Text").gameObject;
        UIEventListener.Get(this.imgLeft).onClick = delegate (PointerEventData pointData)
        {
            this.setBtnView(true);
        };
        UIEventListener.Get(this.imgRight).onClick = delegate (PointerEventData pointData)
        {
            this.setBtnView(false);
        };
        if (data.state.Contains(PetState.PetState_Fight))
        {
            UIInformationList component = gameObject.GetComponent<UIInformationList>();
            gameObject.GetComponent<Text>().text = component.listInformation[0].content;
            UIInformationList component2 = gameObject2.GetComponent<UIInformationList>();
            gameObject2.GetComponent<Text>().text = component2.listInformation[2].content;
            UIInformationList component3 = gameObject3.GetComponent<UIInformationList>();
            gameObject3.GetComponent<Text>().text = component3.listInformation[1].content;
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_select").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.setBtnView(!this.bViewOtherBtn);
            };
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_other1").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.actionRest(data);
            };
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_other2").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.actionPrepare(data);
            };
        }
        else if (data.state.Contains(PetState.PetState_Assist))
        {
            UIInformationList component4 = gameObject.GetComponent<UIInformationList>();
            gameObject.GetComponent<Text>().text = component4.listInformation[1].content;
            UIInformationList component5 = gameObject2.GetComponent<UIInformationList>();
            gameObject2.GetComponent<Text>().text = component5.listInformation[2].content;
            UIInformationList component6 = gameObject3.GetComponent<UIInformationList>();
            gameObject3.GetComponent<Text>().text = component6.listInformation[0].content;
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_select").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.setBtnView(!this.bViewOtherBtn);
            };
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_other1").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.actionRest(data);
            };
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_other2").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.actionFight(data);
            };
        }
        else
        {
            UIInformationList component7 = gameObject.GetComponent<UIInformationList>();
            gameObject.GetComponent<Text>().text = component7.listInformation[2].content;
            UIInformationList component8 = gameObject2.GetComponent<UIInformationList>();
            gameObject2.GetComponent<Text>().text = component8.listInformation[1].content;
            UIInformationList component9 = gameObject3.GetComponent<UIInformationList>();
            gameObject3.GetComponent<Text>().text = component9.listInformation[0].content;
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_select").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.setBtnView(!this.bViewOtherBtn);
            };
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_other1").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.actionPrepare(data);
            };
            UIEventListener.Get(this.attributeInfo.Find("btn/btn_other2").gameObject).onClick = delegate (PointerEventData pointData)
            {
                this.actionFight(data);
            };
        }
    }

    private void setBtnView(bool bview)
    {
        this.bViewOtherBtn = bview;
        if (this.bViewOtherBtn)
        {
            this.btnOther1.SetActive(true);
            this.btnOther2.SetActive(true);
            this.imgLeft.SetActive(false);
            this.imgRight.SetActive(true);
        }
        else
        {
            this.btnOther1.SetActive(false);
            this.btnOther2.SetActive(false);
            this.imgLeft.SetActive(true);
            this.imgRight.SetActive(false);
        }
    }

    private void actionRest(PetBase data)
    {
        PetState from;
        if (data.state.Contains(PetState.PetState_Fight))
        {
            from = PetState.PetState_Fight;
        }
        else if (data.state.Contains(PetState.PetState_Assist))
        {
            from = PetState.PetState_Assist;
        }
        else
        {
            from = PetState.PetState_Rest;
        }
        this.petController.ReqSwitchPetState(data.tempid, from, PetState.PetState_Rest);
    }

    private void actionPrepare(PetBase data)
    {
        PetState from;
        if (data.state.Contains(PetState.PetState_Fight))
        {
            from = PetState.PetState_Fight;
        }
        else if (data.state.Contains(PetState.PetState_Assist))
        {
            from = PetState.PetState_Assist;
        }
        else
        {
            from = PetState.PetState_Rest;
        }
        this.petController.ReqSwitchPetState(data.tempid, from, PetState.PetState_Assist);
    }

    private void actionFight(PetBase data)
    {
        PetState from;
        if (data.state.Contains(PetState.PetState_Fight))
        {
            from = PetState.PetState_Fight;
        }
        else if (data.state.Contains(PetState.PetState_Assist))
        {
            from = PetState.PetState_Assist;
        }
        else
        {
            from = PetState.PetState_Rest;
        }
        this.petController.ReqSwitchPetState(data.tempid, from, PetState.PetState_Fight);
    }

    private void setPetSkill(PetBase data)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)data.activeskillid);
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("summonpet", (ulong)data.id);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "  skillconfig   is  null");
        }
        else
        {
            string imagename = configTable.GetField_String("skillicon").Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries)[0];
            ClassSetItemTexture.Instance.setTexture(this.skillInfo.Find("active/img_skill").gameObject.GetComponent<Image>(), imagename, ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
        }
        GameObject gameObject = this.skillInfo.Find("PassiveSkills/ScrollbarRect/Rect/skill").gameObject;
        gameObject.SetActive(false);
        for (int i = 0; i < this.listSkillObj.Count; i++)
        {
            this.listSkillObj[i].SetActive(false);
        }
        int num = 0;
        while ((long)num < (long)((ulong)data.passivenum))
        {
            GameObject gameObject2;
            if (num < this.listSkillObj.Count)
            {
                gameObject2 = this.listSkillObj[num];
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.parent = gameObject.transform.parent;
                gameObject2.transform.localScale = gameObject.transform.localScale;
                this.listSkillObj.Add(gameObject2);
            }
            gameObject2.SetActive(true);
            num++;
        }
        if (configTable2 == null)
        {
            FFDebug.LogWarning(this, "  petConfig==null  ");
        }
        else
        {
            for (int j = 1; j < 6; j++)
            {
                this.setMixSkills(this.skillInfo.Find("mixskills/active_mix" + j.ToString() + "/img_skill").GetComponent<Image>(), j, configTable2);
                this.skillInfo.Find("mixskills/active_mix" + j.ToString() + "/img_on").gameObject.SetActive(false);
            }
        }
        this.skillInfo.Find("active/img_on").gameObject.SetActive(false);
        this.setSlectSkill(data);
    }

    private void setSlectSkill(PetBase data)
    {
        if (this.checkIsFightPet(data))
        {
            if (MainPlayer.Self.AssistPet == null)
            {
                this.skillInfo.Find("active/img_on").gameObject.SetActive(true);
            }
            else
            {
                this.skillInfo.Find("active/img_on").gameObject.SetActive(false);
                LuaTable configTable = LuaConfigManager.GetConfigTable("summonpet", (ulong)MainPlayer.Self.AssistPet.id);
                this.skillInfo.Find("mixskills/active_mix" + configTable.GetField_String("type") + "/img_on").gameObject.SetActive(true);
            }
        }
    }

    private bool checkIsFightPet(PetBase data)
    {
        return MainPlayer.Self.FightPet != null && data.state.Contains(PetState.PetState_Fight);
    }

    private void setMixSkills(Image image, int index, LuaTable petConfig)
    {
        string imagename;
        switch (index)
        {
            case 1:
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)petConfig.GetField_Uint("elementskill_1"));
                    imagename = configTable.GetField_String("skillicon").Split(new char[]
                    {
                ','
                    }, StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
            case 2:
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)petConfig.GetField_Uint("elementskill_2"));
                    imagename = configTable.GetField_String("skillicon").Split(new char[]
                    {
                ','
                    }, StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
            case 3:
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)petConfig.GetField_Uint("elementskill_3"));
                    imagename = configTable.GetField_String("skillicon").Split(new char[]
                    {
                ','
                    }, StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
            case 4:
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)petConfig.GetField_Uint("elementskill_4"));
                    imagename = configTable.GetField_String("skillicon").Split(new char[]
                    {
                ','
                    }, StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
            default:
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)petConfig.GetField_Uint("elementskill_5"));
                    imagename = configTable.GetField_String("skillicon").Split(new char[]
                    {
                ','
                    }, StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
        }
        ClassSetItemTexture.Instance.setTexture(image, imagename, ImageType.ITEM, new Action<UITextureAsset>(this.addTextureAsset));
    }

    private void initGameObject()
    {
        this.PanelPet = this.mRoot.Find("Offset_Pet/Panel_Pet");
        this.MenuOff = this.PanelPet.Find("MenuOff");
        this.MenuOn = this.PanelPet.Find("MenuOn");
        this.petList = this.PanelPet.Find("petlist/petlist/UIFoldOutList");
        this.basicInfo = this.PanelPet.Find("basic");
        this.attributeInfo = this.PanelPet.Find("info");
        this.skillInfo = this.PanelPet.Find("skill");
        this.btnSelect = this.attributeInfo.Find("btn/btn_select").gameObject;
        this.btnOther1 = this.attributeInfo.Find("btn/btn_other1").gameObject;
        this.btnOther2 = this.attributeInfo.Find("btn/btn_other2").gameObject;
        this.imgLeft = this.attributeInfo.Find("btn/img_left").gameObject;
        this.imgRight = this.attributeInfo.Find("btn/img_right").gameObject;
        this.Panel_Extend = this.mRoot.Find("Offset_Pet/Panel_Extend");
        this.Panel_Extend.gameObject.SetActive(false);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.initGameObject();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    private Transform mRoot;

    private Transform PanelPet;

    private Transform MenuOff;

    private Transform MenuOn;

    private Transform petList;

    private Transform basicInfo;

    private Transform attributeInfo;

    private Transform skillInfo;

    private Transform Panel_Extend;

    private UI_Pet.MenuState uiState;

    private GameObject curSelect;

    public List<PetBase> ListPetData = new List<PetBase>();

    private List<GameObject> listPetObj = new List<GameObject>();

    public PetBase curPetInfo;

    private GameObject btnSelect;

    private GameObject btnOther1;

    private GameObject btnOther2;

    private GameObject imgRight;

    private GameObject imgLeft;

    private bool bViewOtherBtn;

    private List<GameObject> listSkillObj = new List<GameObject>();

    private enum MenuState
    {
        Attribute,
        Skill
    }
}

public class PetBaseComparerPetList : IComparer<PetBase>
{
    public int Compare(PetBase a, PetBase b)
    {
        if (a.state.Contains(PetState.PetState_Fight) && !b.state.Contains(PetState.PetState_Fight))
        {
            return -1;
        }
        if (!a.state.Contains(PetState.PetState_Fight) && b.state.Contains(PetState.PetState_Fight))
        {
            return 1;
        }
        if (a.state.Contains(PetState.PetState_Fight) || b.state.Contains(PetState.PetState_Fight))
        {
            return 0;
        }
        if (a.state.Contains(PetState.PetState_Assist) && !b.state.Contains(PetState.PetState_Assist))
        {
            return -1;
        }
        if (!a.state.Contains(PetState.PetState_Assist) && b.state.Contains(PetState.PetState_Assist))
        {
            return 1;
        }
        if (a.state.Contains(PetState.PetState_Assist) && b.state.Contains(PetState.PetState_Assist))
        {
            if (a.state.Contains(PetState.PetState_Dying) && !b.state.Contains(PetState.PetState_Dying))
            {
                return -1;
            }
            if (!a.state.Contains(PetState.PetState_Dying) && b.state.Contains(PetState.PetState_Dying))
            {
                return 1;
            }
            if (a.level > b.level)
            {
                return -1;
            }
            if (a.level < b.level)
            {
                return 1;
            }
            if (a.quality > b.quality)
            {
                return -1;
            }
            if (a.quality < b.quality)
            {
                return 1;
            }
            if (a.id < b.id)
            {
                return -1;
            }
            if (a.id > b.id)
            {
                return 1;
            }
            return 0;
        }
        else
        {
            if (a.state.Contains(PetState.PetState_Dying) && !b.state.Contains(PetState.PetState_Dying))
            {
                return -1;
            }
            if (!a.state.Contains(PetState.PetState_Dying) && b.state.Contains(PetState.PetState_Dying))
            {
                return 1;
            }
            if (a.level > b.level)
            {
                return -1;
            }
            if (a.level < b.level)
            {
                return 1;
            }
            if (a.quality > b.quality)
            {
                return -1;
            }
            if (a.quality < b.quality)
            {
                return 1;
            }
            if (a.id < b.id)
            {
                return -1;
            }
            if (a.id > b.id)
            {
                return 1;
            }
            return 0;
        }
    }
}
