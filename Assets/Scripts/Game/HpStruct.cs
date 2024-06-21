using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HpStruct
{
    private GameObject objHP
    {
        get
        {
            return this.objHP_;
        }
        set
        {
            this.objHP_ = value;
        }
    }

    private GameScene gs
    {
        get
        {
            return ManagerCenter.Instance.GetManager<GameScene>();
        }
    }

    public void SetOwner(CharactorBase Char)
    {
        this.Owner = Char;
    }

    public void SetTarget(GameObject target, Transform top)
    {
        if (top != null)
        {
            this.Target.followTarget = top.gameObject;
        }
        else
        {
            this.Target.followTarget = target;
        }
    }

    private void initObject(NormalObjectInPool tobj, GameObject target, Transform top, Transform hitRoot)
    {
        this.hpHitRoot = hitRoot;
        this.objParent = tobj.ItemObj;
        this.poolItem = tobj;
        this.obj = this.objParent.transform.Find("content").gameObject;
        this.obj.transform.Find("btn_visit").gameObject.SetActive(false);
        this.Target = this.objParent.GetComponent<UIFllowTarget>();
        if (this.Target == null)
        {
            this.Target = this.objParent.AddComponent<UIFllowTarget>();
        }
        this.Target.maincaCamera = null;
        if (top != null)
        {
            this.Target.followTarget = top.gameObject;
        }
        else
        {
            this.Target.followTarget = target;
        }
        this.obj.SetActive(true);
        this.hpParent = this.obj.transform.Find("hp").gameObject;
        CommonUtil.SetActive(this.hpParent, true);
        this.objHP = this.obj.transform.Find("hp/Slider").gameObject;
        this.sliderIn = this.objHP.transform.Find("Slider").GetComponent<Slider>();
        this.killedBtn = this.obj.transform.Find("arrow/killedBtn").gameObject;
        this.selectBtn = this.objParent.transform.parent.parent.Find("choose").gameObject;
        this.img_selectArrowL = this.selectBtn.transform.Find("img_arrow").gameObject.GetComponent<Image>();
        this.img_selectArrowR = this.selectBtn.transform.Find("img_arrow2").gameObject.GetComponent<Image>();
        this.img_selectImgL = this.selectBtn.transform.Find("img_line").gameObject.GetComponent<Image>();
        this.img_selectImgR = this.selectBtn.transform.Find("img_line2").gameObject.GetComponent<Image>();
        this.img_circleSmall = this.selectBtn.transform.Find("circle_small").gameObject.GetComponent<Image>();
        this.img_circleBig = this.selectBtn.transform.Find("circle_big").gameObject.GetComponent<Image>();
        this.m_ProjectorBig = this.selectBtn.transform.Find("Project_big").GetComponent<Projector>();
        this.m_ProjectorSmall = this.selectBtn.transform.Find("Project_small").GetComponent<Projector>();
        if (this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name") != null)
        {
            this.charactorName = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").gameObject;
        }
        if (this.obj.transform.Find("PlayerInfo/txt_name") != null)
        {
            this.charactorName = this.obj.transform.Find("PlayerInfo/txt_name").gameObject;
            this.functionIcon = this.obj.transform.Find("PlayerInfo/txt_name/img_zhu").gameObject;
        }
        this.objEffectSet = this.obj.FindChild("effect");
        if (this.objEffectSet != null)
        {
            for (int i = 0; i < this.objEffectSet.transform.childCount; i++)
            {
                this.objEffectSet.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        this.killedBtn.SetActive(false);
        this.effect1 = this.killedBtn.transform.Find("img_tx1").gameObject.AddComponent<UI_EffectComponent>();
        this.effect1.InitEffect();
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            if (this.obj != null && this.Owner is Npc)
            {
                this.obj.SetActive(true);
            }
            this.SetSelect(false);
            this.SetPlayerNameShowBySetting(null);
        });
        this.AddUIChileScale(this.objParent, "/PlayerInfo", true, false);
        this.AddUIChileScale(this.objParent, "/hp", true, true);
        this.AddUIChileScale(this.objParent, "/btn_visit", true, true);
    }

    private void AddUIChileScale(GameObject target, string path, bool bScale, bool isUseLimit)
    {
        Transform transform = target.transform.Find("content" + path);
        if (transform != null)
        {
            UIChildScaler uichildScaler = transform.GetComponent<UIChildScaler>();
            if (uichildScaler == null)
            {
                uichildScaler = transform.gameObject.AddComponent<UIChildScaler>();
                uichildScaler.m_bScaler = bScale;
                uichildScaler.isUseLimit = isUseLimit;
                LayoutElement component = transform.GetComponent<LayoutElement>();
                if (component != null)
                {
                    uichildScaler.elementOriginalMinHeight = component.minHeight;
                }
                if (uichildScaler.m_bScaler)
                {
                    transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                }
            }
        }
    }

    public void SetActiveKilledBtn(bool bActive)
    {
        if (this.killedBtn == null)
        {
            return;
        }
        if (this.killedBtn.activeSelf == bActive)
        {
            return;
        }
        this.killedBtn.SetActive(bActive);
        if (bActive)
        {
            UIEventListener.Get(this.killedBtn).onClick = new UIEventListener.VoidDelegate(this.OnClickKilledBtn);
            if (this.effect1 != null)
            {
                this.effect1.ShowEffect();
            }
        }
    }

    private void OnClickKilledBtn(PointerEventData data)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().DrinkBloodRelase(this.Owner);
    }

    public void InitHpStruct(NormalObjectInPool tobj, GameObject target, Transform top, cs_MapUserData tdata, Transform hitRoot)
    {
        if (tdata.charid == MainPlayer.Self.OtherPlayerData.MapUserData.charid)
        {
            this.bSelf = true;
        }
        else
        {
            this.bSelf = false;
        }
        this.initObject(tobj, target, top, hitRoot);
        this.hp = tdata.mapdata.hp;
        this.maxhp = tdata.mapdata.maxhp;
        this.data = tdata;
        ControllerManager.Instance.GetController<UIHpSystem>().listHp[this.Owner.EID.Id] = this;
        Text component = this.charactorName.GetComponent<Text>();
        component.text = this.data.name;
        component.gameObject.SetActive(true);
        Text component2 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>();
        component2.text = string.Empty;
        if (!string.IsNullOrEmpty(tdata.mapdata.guildname))
        {
            string format = "[{0}]";
            component2.text = string.Format(format, tdata.mapdata.guildname);
            component2.gameObject.SetActive(true);
        }
        else
        {
            component2.gameObject.SetActive(false);
        }
        Text component3 = this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>();
        component3.text = string.Empty;
        if (!string.IsNullOrEmpty(tdata.mapdata.titlename))
        {
            string format2 = "[{0}]";
            component3.text = string.Format(format2, tdata.mapdata.titlename);
            component3.gameObject.SetActive(true);
        }
        else
        {
            component3.gameObject.SetActive(false);
        }
        if (this.data.mapdata.maxhp == 0U)
        {
            this.objHP.GetComponent<Slider>().value = 0f;
            this.sliderIn.value = 0f;
        }
        else
        {
            this.setHpSliderValue();
        }
        this.RefreshModel();
        this.InitNPCTopButton();
        this.SetPlayerNameShowBySetting(null);
    }

    public GameObject InitAutoFightText()
    {
        if (!this.obj)
        {
            return null;
        }
        Transform transform = this.obj.transform.Find("PlayerInfo/txt_auto_fight");
        if (transform)
        {
            return transform.gameObject;
        }
        Text component = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>();
        Text component2 = this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>();
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(component2.gameObject);
        gameObject.transform.SetParent(component2.transform.parent, false);
        gameObject.name = "txt_auto_fight";
        Text component3 = gameObject.GetComponent<Text>();
        component3.font = component.font;
        component3.color = new Color(0.9529412f, 0.8509804f, 0.8509804f, 0.8117647f);
        component3.transform.SetSiblingIndex(1);
        component3.text = "自动战斗中      ";
        GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(component3.gameObject);
        gameObject2.transform.SetParent(component3.transform, false);
        Text component4 = gameObject2.GetComponent<Text>();
        component4.font = component.font;
        component4.alignment = TextAnchor.MiddleLeft;
        component4.rectTransform.pivot = new Vector2(0f, 0.5f);
        component4.rectTransform.anchorMax = Vector2.up * 0.5f;
        component4.rectTransform.anchorMin = Vector2.up * 0.5f;
        component4.rectTransform.anchoredPosition = Vector3.right * 105f;
        component4.gameObject.SetActive(true);
        TextAnimation textAnimation = component4.gameObject.AddComponent<TextAnimation>();
        textAnimation.SetAnimationText("......");
        return gameObject;
    }

    public void setHpSliderValue()
    {
        this.setHpSlider();
        this.sliderIn.value = this.hp / this.maxhp;
    }

    public void SetHPActive(bool active)
    {
        this.hpParent.SetActive(active);
    }

    public void ShowExpChange(int change)
    {
        this.hpController.SetExp((float)change, this.hpParent.transform);
    }

    public void InitHpStruct(NormalObjectInPool tobj, GameObject target, Transform top, cs_MapNpcData tdata, Transform hitRoot)
    {
        this.initObject(tobj, target, top, hitRoot);
        this.hp = tdata.hp;
        this.maxhp = tdata.maxhp;
        this.npcdata = tdata;
        ControllerManager.Instance.GetController<UIHpSystem>().listHp[this.Owner.EID.Id] = this;
        Text component = this.charactorName.GetComponent<Text>();
        this.charactorName.GetComponent<Text>().text = this.npcdata.name;
        this.charactorName.gameObject.SetActive(true);
        Text component2 = tobj.ItemObj.transform.Find("content/PlayerInfo/txt_owner").GetComponent<Text>();
        if (!string.IsNullOrEmpty(this.npcdata.Owner))
        {
            component2.text = "[" + this.npcdata.Owner + "]";
            component2.gameObject.SetActive(true);
        }
        else
        {
            component2.text = string.Empty;
            component2.gameObject.SetActive(false);
        }
        Text component3 = tobj.ItemObj.transform.Find("content/PlayerInfo/txt_title").GetComponent<Text>();
        component3.text = this.npcdata.titlename;
        component3.gameObject.SetActive(!string.IsNullOrEmpty(this.npcdata.titlename));
        Text component4 = tobj.ItemObj.transform.Find("content/PlayerInfo/txt_npc_introduce").GetComponent<Text>();
        component4.text = this.npcdata.introduce;
        component4.gameObject.SetActive(!string.IsNullOrEmpty(this.npcdata.introduce));
        if (this.npcdata.maxhp == 0U)
        {
            this.objHP.GetComponent<Slider>().value = 0f;
        }
        else
        {
            this.setHpSlider();
        }
        if (this.functionIcon != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.npcdata.baseid);
            if (configTable != null)
            {
                string cacheField_String = configTable.GetCacheField_String("function");
                if (!string.IsNullOrEmpty(cacheField_String))
                {
                    UITextureMgr.Instance.GetTexture(ImageType.ICON, cacheField_String, delegate (UITextureAsset tex)
                    {
                        if (tex != null && tex.textureObj != null)
                        {
                            if (this.functionIcon == null)
                            {
                                return;
                            }
                            this.functionIcon.SetActive(true);
                            Image component5 = this.functionIcon.GetComponent<Image>();
                            component5.sprite = Sprite.Create(tex.textureObj, new Rect(0f, 0f, (float)tex.textureObj.width, (float)tex.textureObj.height), new Vector2(0.5f, 0.5f));
                            component5.material = null;
                        }
                    });
                }
            }
        }
        this.RefreshModel();
        this.InitNPCTopButton();
        this.SetPlayerNameShowBySetting(null);
        this.OnResetHidNpc(GameSystemSettings.bHideNpc);
    }

    public void OnResetHidNpc(bool state)
    {
        if (this.obj != null)
        {
            this.obj.SetActive(!state);
        }
    }

    public void RefreshModel()
    {
        if (this.Owner.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_WEAK))
        {
            this.SetActiveKilledBtn(true);
        }
        else
        {
            this.SetActiveKilledBtn(false);
        }
        if (this.Owner is Npc_TaskCollect)
        {
            if ((this.Owner as Npc_TaskCollect).CheckStateContainDoing())
            {
                this.ActiveName();
                this.ActiveInfo();
            }
            else
            {
                this.DisActiveName();
                this.DisActiveInfo();
            }
        }
        if (this.Owner is OtherPlayer)
        {
            OtherPlayer otherPlayer = this.Owner as OtherPlayer;
            otherPlayer.OnRelationChange();
        }
        bool flag = false;
        PlayerBufferControl component = this.Owner.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            flag = component.ContainsState(UserState.USTATE_YMC_FAKEDEATH_STATE);
        }
        if (this.data != null)
        {
            if (this.Owner.IsLive && !flag)
            {
                this.SetHeadTextColor();
                this.SetHpBarSprite();
            }
            else
            {
                this.SetDeathStateColor();
            }
        }
        if (this.npcdata != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.npcdata.baseid);
            if (configTable != null && configTable.GetCacheField_Uint("kind") == 8U)
            {
                return;
            }
            this.relation = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
            if (this.Owner.IsLive && !flag)
            {
                if (this.obj == null)
                {
                    return;
                }
                this.obj.transform.Find("PlayerInfo/txt_npc_introduce").gameObject.SetActive(false);
                this.obj.transform.Find("PlayerInfo/txt_npc_introduce").GetComponent<Outline>().effectColor = Color.green;
                this.obj.transform.Find("PlayerInfo/txt_npc_introduce").GetComponent<Text>().color = Color.black;
                if (this.relation == RelationType.Neutral)
                {
                    this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("titlenamenormal_ol");
                    this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Outline>().effectColor = Const.GetColorByName("neutralnpcname_ol");
                    this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("titlenamenormal");
                    this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Text>().color = Const.GetColorByName("neutralnpcname");
                    this.SetSelectUIColor(Const.GetColorByName("neutralnpcname"), this.Owner);
                }
                else if (this.relation == RelationType.Enemy)
                {
                    this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("titlenamenormal_ol");
                    this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Outline>().effectColor = Const.GetColorByName("enemynpcname_ol");
                    this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("titlenamenormal");
                    this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Text>().color = Const.GetColorByName("enemynpcname");
                    this.SetSelectUIColor(Const.GetColorByName("enemynpcname"), this.Owner);
                }
                else if (this.relation == RelationType.Friend)
                {
                    this.obj.transform.Find("PlayerInfo/txt_npc_introduce").gameObject.SetActive(true);
                    if (this.Owner is Npc)
                    {
                        Npc npc = this.Owner as Npc;
                        if (configTable != null && configTable.GetCacheField_Uint("kind") == 25U)
                        {
                            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("friendlyplayername_ol");
                            this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Outline>().effectColor = Const.GetColorByName("friendlyplayername_ol");
                            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("friendlyplayername");
                            this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Text>().color = Const.GetColorByName("friendlyplayername");
                        }
                        else
                        {
                            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("titlenamefriendly_ol");
                            this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Outline>().effectColor = Const.GetColorByName("friendlynpcname_ol");
                            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("titlenamefriendly");
                            this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Text>().color = Const.GetColorByName("friendlynpcname");
                        }
                        this.SetSelectUIColor(Const.GetColorByName("friendlynpcname"), this.Owner);
                    }
                }
                this.SetHpBarSpriteByRelation(this.relation);
            }
            else
            {
                this.SetDeathStateColor();
            }
        }
    }

    private void SetHeadTextColor()
    {
        if (this.obj == null)
        {
            return;
        }
        this.relation = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
        if (this.relation == RelationType.Neutral)
        {
            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("titlenamenormal_ol");
            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("titlenamenormal");
            this.SetSelectUIColor(Const.GetColorByName("friendlyplayername"), this.Owner);
        }
        else if (this.relation == RelationType.Enemy)
        {
            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("titlenamenormal_ol");
            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("titlenamenormal");
            this.SetSelectUIColor(Const.GetColorByName("enemynpcname"), this.Owner);
        }
        else if (this.relation == RelationType.Friend || this.relation == RelationType.Self)
        {
            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Outline>().effectColor = Const.GetColorByName("titlenamenormal_ol");
            this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>().color = Const.GetColorByName("titlenamenormal");
            this.SetSelectUIColor(Const.GetColorByName("friendlynpcname"), this.Owner);
        }
        if (this.gs.isAbattoirScene)
        {
            this.SetAbattoirHeadTextColorByUid(this.Owner.EID.Id);
        }
        else
        {
            this.SetHeadTextColorByRelation(this.relation);
        }
    }

    private void SetAbattoirHeadTextColorByUid(ulong playerUid)
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        string colorConfigByUid = controller.GetColorConfigByUid(playerUid);
        if (colorConfigByUid == string.Empty)
        {
            return;
        }
        Shadow component = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Outline>();
        Color colorByName = Const.GetColorByName(colorConfigByUid);
        this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Outline>().effectColor = colorByName;
        component.effectColor = colorByName;
        Graphic component2 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>();
        colorByName = Const.GetColorByName(colorConfigByUid);
        this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>().color = colorByName;
        component2.color = colorByName;
    }

    private void SetHeadTextColorByRelation(RelationType relation)
    {
        if (relation == RelationType.Neutral)
        {
            Shadow component = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Outline>();
            Color colorByName = Const.GetColorByName("friendlyplayername_ol");
            this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Outline>().effectColor = colorByName;
            component.effectColor = colorByName;
            Graphic component2 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>();
            colorByName = Const.GetColorByName("friendlyplayername");
            this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>().color = colorByName;
            component2.color = colorByName;
        }
        else if (relation == RelationType.Enemy)
        {
            Shadow component3 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Outline>();
            Color colorByName = Const.GetColorByName("enemylyplayername_ol");
            this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Outline>().effectColor = colorByName;
            component3.effectColor = colorByName;
            Graphic component4 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>();
            colorByName = Const.GetColorByName("enemynpcname");
            this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>().color = colorByName;
            component4.color = colorByName;
        }
        else if (relation == RelationType.Friend || relation == RelationType.Self)
        {
            Shadow component5 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Outline>();
            Color colorByName = Const.GetColorByName("friendlyplayername_ol");
            this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Outline>().effectColor = colorByName;
            component5.effectColor = colorByName;
            Graphic component6 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>();
            colorByName = Const.GetColorByName("friendlyplayername");
            this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>().color = colorByName;
            component6.color = colorByName;
        }
    }

    public void RefreshRoleUI()
    {
        if (this.Owner != null)
        {
            RelationType type = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
            this.SetSelectUIColor(Const.ColorByRelation(type, this.data != null), this.Owner);
        }
        this.RefreshHPStructInfo();
    }

    public void RefreshHPStructInfo()
    {
        cs_MapUserData cs_MapUserData = this.data;
        if (cs_MapUserData == null)
        {
            return;
        }
        bool flag = this.TeamCtrl.IsMyTeamNember(this.Owner.EID);
        Transform transform = this.obj.transform.Find("PlayerInfo/Panel_name_node/img_fuction");
        if (flag)
        {
            transform.gameObject.SetActive(true);
            Image rImage = transform.Find("img_outline/img_icon").GetComponent<Image>();
            UITextureMgr.Instance.GetSpriteFromAtlas("base", "icon_team", delegate (Sprite sp)
            {
                rImage.sprite = sp;
            });
        }
        else
        {
            transform.gameObject.SetActive(false);
            bool flag2 = false;
            if (flag2)
            {
            }
        }
        Text component = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>();
        component.text = string.Empty;
        if (!string.IsNullOrEmpty(cs_MapUserData.mapdata.guildname))
        {
            string format = "[{0}]";
            component.text = string.Format(format, cs_MapUserData.mapdata.guildname);
            component.gameObject.SetActive(true);
        }
        else
        {
            component.gameObject.SetActive(false);
        }
        this.SetPlayerNameShowBySetting(null);
    }

    public void SetPlayerNameShowBySetting(BetterDictionary<string, bool> nameSwitchData = null)
    {
        if (nameSwitchData == null)
        {
            nameSwitchData = GameSystemSettings.nameSwitchData;
        }
        if (nameSwitchData == null || nameSwitchData.Count == 0)
        {
            FFDebug.LogError(this, "can not get name switch data");
            return;
        }
        if (!this.obj)
        {
            return;
        }
        this.relation = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
        if (this.data == null)
        {
            bool flag = true;
            if (this.Owner is Npc_TaskCollect)
            {
                flag = (this.Owner as Npc_TaskCollect).CheckStateContainDoing();
            }
            GameObject gameObject = this.obj.transform.Find("PlayerInfo/txt_name").gameObject;
            if (this.relation == RelationType.Friend)
            {
                gameObject.SetActive(!nameSwitchData["DB.NPCName"] && flag);
                this.objHP.SetActive(false);
                if (this.Owner is Npc)
                {
                    Npc npc = this.Owner as Npc;
                    LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                    if (configTable != null && configTable.GetCacheField_Uint("kind") == 25U)
                    {
                        this.objHP.SetActive(!nameSwitchData["DB.OthersHpBar"]);
                    }
                }
            }
            else
            {
                gameObject.SetActive(!nameSwitchData["DB.EnemyName"] && flag);
                this.objHP.SetActive(!nameSwitchData["DB.EnemyHpBar"]);
            }
            return;
        }
        bool flag2 = false;
        PlayerBufferControl component = this.Owner.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            flag2 = component.ContainsState(UserState.USTATE_YMC_FAKEDEATH_STATE);
        }
        this.objHP.gameObject.SetActive(false);
        if (this.Owner.IsLive && !flag2)
        {
            GameObject gameObject2 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").gameObject;
            GameObject gameObject3 = this.obj.transform.Find("PlayerInfo/txt_title").gameObject;
            GameObject gameObject4 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").gameObject;
            Text component2 = this.obj.transform.Find("PlayerInfo/txt_title").GetComponent<Text>();
            Text component3 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>();
            bool flag3 = true;
            if (this.Owner is Npc_TaskCollect)
            {
                flag3 = (this.Owner as Npc_TaskCollect).CheckStateContainDoing();
            }
            if (this.relation == RelationType.Friend || this.relation == RelationType.Neutral)
            {
                gameObject2.SetActive(!nameSwitchData["DB.OthersName"] && flag3);
                gameObject4.SetActive(!nameSwitchData["DB.OthersGuild"]);
                gameObject3.SetActive(!nameSwitchData["DB.OthersTitle"]);
                this.objHP.SetActive(!nameSwitchData["DB.OthersHpBar"]);
            }
            else if (this.relation == RelationType.Enemy)
            {
                gameObject2.SetActive(!nameSwitchData["DB.EnemyName"] && flag3);
                this.objHP.SetActive(!nameSwitchData["DB.EnemyHpBar"]);
                gameObject4.SetActive(!nameSwitchData["DB.EnemyGuild"]);
                gameObject3.SetActive(!nameSwitchData["DB.EnemyTitle"]);
            }
            else if (this.relation == RelationType.Self)
            {
                gameObject2.SetActive(!nameSwitchData["DB.SelfName"]);
                gameObject4.SetActive(!nameSwitchData["DB.SelfGuild"]);
                gameObject3.SetActive(!nameSwitchData["DB.SelfTitle"]);
                this.objHP.SetActive(!nameSwitchData["DB.SelfHpBar"]);
            }
            this.SetHpBarSprite();
        }
    }

    public void RevertOrBleedChangeHp(float NewHP, float Change, List<ATTACKRESULT> attCodeList, bool force = false)
    {
        this.hp = NewHP;
        if (Change <= 0f)
        {
            this.setActiveHp();
            this.bleedValue(NewHP, Change, attCodeList);
        }
        else if (this.Owner.EID.Id == MainPlayer.Self.EID.Id || force)
        {
            this.setActiveHp();
            this.hpController.SetRevert(this.Owner.EID, Change, this.hpParent.transform);
        }
    }

    public void RevertOrBleedChangeHp(float NewHP, float Change, bool force = false)
    {
        this.hp = NewHP;
        if (Change <= 0f)
        {
            this.setActiveHp();
            this.bleedValue(NewHP, Change, null);
        }
        else if (this.Owner.EID.Id == MainPlayer.Self.EID.Id || force)
        {
            this.setActiveHp();
            this.hpController.SetRevert(this.Owner.EID, Change, this.hpParent.transform);
        }
    }

    private void bleedValue(float NewHP, float Change, List<ATTACKRESULT> attCode = null)
    {
        this.sliderTargetValue = this.hp / this.maxhp;
        this.curTime = 0f;
        this.sliderValue = this.sliderIn.value;
        OneSkillHitResult oneSkillHitResult = new OneSkillHitResult();
        oneSkillHitResult.Att = MainPlayer.Self.EID;
        oneSkillHitResult.Hp = (uint)this.hp;
        oneSkillHitResult.HpChange = (int)Change;
        if (attCode == null)
        {
            oneSkillHitResult.AttcodeList = new List<ATTACKRESULT>();
            oneSkillHitResult.AttcodeList.Add(ATTACKRESULT.ATTACKRESULT_NORMAL);
        }
        else
        {
            oneSkillHitResult.AttcodeList = attCode;
        }
        this.hpController.SetHit(this.Owner.EID, oneSkillHitResult, true, this.hpParent.transform);
    }

    public void UpData()
    {
        if (this.curTime <= 1f)
        {
            this.curTime += Time.deltaTime;
            this.sliderIn.value = Mathf.Lerp(this.sliderValue, this.sliderTargetValue, this.curTime);
        }
    }

    public void HitChangeHp(EntitiesID _Owner, OneSkillHitResult HitResult)
    {
        if (this.Owner == null)
        {
            return;
        }
        if (this.Owner.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_DEATH))
        {
            this.hp = 0f;
        }
        else
        {
            this.hp = HitResult.Hp;
        }
        this.sliderTargetValue = this.hp / this.maxhp;
        this.curTime = 0f;
        this.sliderValue = this.sliderIn.value;
        this.setHitvalue(_Owner, HitResult);
    }

    public float GetLifePercent()
    {
        float value = this.hp / this.maxhp;
        return Mathf.Clamp(value, 0f, 1f);
    }

    public void setActiveHp()
    {
        this.setHpSlider();
    }

    public void UnActiveHp()
    {
        this.curTime = 0f;
    }

    private ITeamController TeamCtrl
    {
        get
        {
            if (this.gs.isAbattoirScene)
            {
                return ControllerManager.Instance.GetController<AbattoirMatchController>();
            }
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    private UIHpSystem hpController
    {
        get
        {
            return ControllerManager.Instance.GetController<UIHpSystem>();
        }
    }

    private void setHitvalue(EntitiesID Owner, OneSkillHitResult HitResult)
    {
        bool attIsMainPlayer = false;
        if (HitResult.Att.Etype == CharactorType.NPC)
        {
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(HitResult.Att) as Npc;
            if (npc != null && npc.NpcData.MapNpcData != null && npc.NpcData.MapNpcData.MasterData != null)
            {
                if (npc.NpcData.MapNpcData.MasterData.Eid.Equals(MainPlayer.Self.EID))
                {
                    attIsMainPlayer = true;
                }
                if (this.TeamCtrl.IsMyTeamNember(npc.NpcData.MapNpcData.MasterData.Eid))
                {
                }
            }
        }
        else
        {
            if (HitResult.Att.Equals(MainPlayer.Self.EID))
            {
                attIsMainPlayer = true;
            }
            if (this.TeamCtrl.IsMyTeamNember(HitResult.Att))
            {
            }
        }
        if (this.TeamCtrl.IsMyTeamNember(Owner) || Owner.Equals(MainPlayer.Self.EID))
        {
            ulong key = 0UL;
            if (HitResult.Att.Etype == CharactorType.NPC)
            {
                Npc npc2 = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(HitResult.Att) as Npc;
                if (npc2 != null && npc2.NpcData.MapNpcData != null && npc2.NpcData.MapNpcData.MasterData != null)
                {
                    key = npc2.NpcData.MapNpcData.MasterData.Eid.Id;
                }
            }
            else
            {
                key = HitResult.Att.Id;
            }
            if (this.hpController.listHp.ContainsKey(key) && this.hpController.listHp[key] != null)
            {
                this.hpController.listHp[key].setActiveHp();
            }
        }
        this.setActiveHp();
        this.hpController.SetHit(Owner, HitResult, attIsMainPlayer, this.hpParent.transform);
        this.RefreshRoleUI();
    }

    public void PlayUIEffect(string name)
    {
        if (this.objEffectSet == null)
        {
            return;
        }
        GameObject gameObject = this.objEffectSet.FindChild(name);
        if (gameObject)
        {
            TweenUtil.SetFinishedDisActive(gameObject, null);
            TweenUtil.Reset(gameObject);
            TweenUtil.Play(gameObject);
            gameObject.SetActive(true);
        }
    }

    public void ResetData(cs_MapUserData tdata)
    {
        this.data = tdata;
        this.ResetHp(this.data.mapdata.hp, this.data.mapdata.maxhp);
        Text component = this.charactorName.GetComponent<Text>();
        component.text = this.data.name;
        this.RefreshRoleUI();
    }

    public void ResetData(cs_MapNpcData tdata)
    {
        this.npcdata = tdata;
        this.ResetHp(this.npcdata.hp, this.npcdata.maxhp);
        if (this.npcdata != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.npcdata.baseid);
            if (configTable == null)
            {
                return;
            }
            this.relation = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
            if (this.Owner != null && this.Owner is OtherPlayer)
            {
                OtherPlayer otherPlayer = this.Owner as OtherPlayer;
                otherPlayer.OnRelationChange();
            }
            if (this.Owner is OtherPlayer)
            {
                if (this.relation == RelationType.Neutral)
                {
                    this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("friendlyplayername_ol"));
                }
                else if (this.relation == RelationType.Enemy)
                {
                    this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("enemylyplayername_ol"));
                }
                else if (this.relation == RelationType.Friend)
                {
                    this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("friendlyplayername_ol"));
                }
            }
            else if (this.relation == RelationType.Neutral)
            {
                this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("neutralnpcname_ol"));
            }
            else if (this.relation == RelationType.Enemy)
            {
                this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("enemynpcname_ol"));
            }
            else if (this.relation == RelationType.Friend && this.Owner is Npc)
            {
                Npc npc = this.Owner as Npc;
                if (configTable != null && configTable.GetCacheField_Uint("kind") == 25U)
                {
                    this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("friendlyplayername_ol"));
                }
                else
                {
                    this.RefreshNpcTitle(this.npcdata.titlename, Const.GetColorByName("friendlynpcname_ol"));
                }
            }
        }
    }

    public void ResetHp(float thp, float tmaxHp)
    {
        this.hp = thp;
        this.maxhp = tmaxHp;
        if (this.obj != null)
        {
            this.setHpSlider();
        }
    }

    public void ResetHp(float thp)
    {
        this.hp = thp;
        if (this.obj != null)
        {
            this.setHpSlider();
        }
    }

    private void setHpSlider()
    {
        this.objHP.GetComponent<Slider>().value = this.hp / this.maxhp;
    }

    public void RefreshNpcTitle(string content, Color color)
    {
        Transform transform = this.obj.transform.Find("occupyinfo/txt_occupyinfo");
        Transform transform2 = this.obj.transform.Find("PlayerInfo/txt_name");
        if (transform == null || transform2 == null)
        {
            return;
        }
        if (this.Owner != null && this.Owner is Npc)
        {
            Npc npc = this.Owner as Npc;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
            if (configTable != null && configTable.GetField_Uint("kind") != 8U)
            {
                content = string.Empty;
            }
        }
        transform2.GetComponent<Outline>().effectColor = color;
        if (string.IsNullOrEmpty(content))
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }
        transform.parent.gameObject.SetActive(true);
        Text component = transform.GetComponent<Text>();
        component.text = content;
        component.GetComponent<Outline>().effectColor = color;
        this.SetHpBarSprite();
    }

    public void OnMainCameraChange()
    {
        if (null != this.Target)
        {
            this.Target.maincaCamera = null;
        }
    }

    public void Refresh()
    {
        if (this.obj == null)
        {
            return;
        }
        if (this.Owner.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_BATTLE))
        {
            return;
        }
        this.UnActiveHp();
        if (Camera.main == null)
        {
            return;
        }
        if (this.Target == null)
        {
            return;
        }
        if (this.Target.followTarget == null)
        {
            return;
        }
    }

    private void SetHpBarSpriteByRelation(RelationType rt)
    {
        string spritename = string.Empty;
        string spritename2 = string.Empty;
        switch (rt)
        {
            case RelationType.Self:
                spritename = "bar_hp_4";
                spritename2 = "hp_4";
                break;
            case RelationType.Friend:
                spritename = "bar_hp_3";
                spritename2 = "hp_3";
                if (this.Owner != null)
                {
                    if (this.Owner is OtherPlayer)
                    {
                        spritename = "bar_hp_4";
                        spritename2 = "hp_4";
                    }
                    else if (this.Owner is Npc)
                    {
                        Npc npc = this.Owner as Npc;
                        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                        if (configTable != null && configTable.GetCacheField_Uint("kind") == 25U)
                        {
                            spritename = "bar_hp_4";
                            spritename2 = "hp_4";
                        }
                    }
                }
                break;
            case RelationType.Neutral:
                spritename = "bar_hp_2";
                spritename2 = "hp_2";
                if (this.Owner != null && this.Owner is OtherPlayer)
                {
                    spritename = "bar_hp_4";
                    spritename2 = "hp_4";
                }
                break;
            case RelationType.Enemy:
                spritename = "bar_hp_1";
                spritename2 = "hp_1";
                break;
        }
        if (this.objHP != null)
        {
            Transform bgOut = this.objHP.transform.Find("Background");
            Transform bgIn = this.objHP.transform.Find("Slider/Background");
            Transform fillOut = this.objHP.transform.Find("FillArea/Fill");
            Transform fillIn = this.objHP.transform.Find("Slider/FillArea/Fill");
            UITextureMgr.Instance.GetSpriteFromAtlas("base", spritename, delegate (Sprite sp)
            {
                if (bgOut == null)
                {
                    return;
                }
                bgOut.GetComponent<Image>().overrideSprite = sp;
                bgIn.GetComponent<Image>().overrideSprite = sp;
            });
            UITextureMgr.Instance.GetSpriteFromAtlas("base", spritename2, delegate (Sprite sp)
            {
                if (fillOut == null)
                {
                    return;
                }
                fillOut.GetComponent<Image>().overrideSprite = sp;
                fillIn.GetComponent<Image>().overrideSprite = sp;
            });
        }
        if (this.IsSelect)
        {
            this.relation = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
            MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
            if (controller != null && this.Owner != null)
            {
                controller.UpdateTargetRelation(this.relation, this.data != null);
            }
        }
    }

    public void DisableHpBar()
    {
        if (this.obj != null)
        {
            return;
        }
    }

    public void DisActiveInfo()
    {
        this._forceHideTopBtn = true;
        if (this.npcTopBtn != null)
        {
            this.npcTopBtn.SetActive(false);
        }
        if (this.obj != null)
        {
            this.obj.transform.Find("PlayerInfo").gameObject.SetActive(false);
        }
    }

    public void ActiveInfo()
    {
        this._forceHideTopBtn = false;
        if (this.npcTopBtn != null)
        {
            this.TryShowTopButton();
        }
        if (this.obj != null)
        {
            this.obj.transform.Find("PlayerInfo").gameObject.SetActive(true);
        }
    }

    public void TopUISwitch(bool state)
    {
        if (this.obj != null)
        {
            this.obj.transform.gameObject.SetActive(!state);
            this.obj.transform.Find("PlayerInfo").gameObject.SetActive(!state);
            this.obj.transform.Find("hp").gameObject.SetActive(!state);
            this.obj.transform.Find("effect").gameObject.SetActive(!state);
        }
    }

    public void SetAssistTag(bool bassist)
    {
        if (this.obj != null)
        {
            this.obj.transform.Find("PlayerInfo/txt_name/img_zhu").gameObject.SetActive(false);
        }
    }

    public void DisActiveName()
    {
        if (this.obj != null)
        {
            this.obj.transform.Find("PlayerInfo/txt_name").gameObject.SetActive(false);
        }
    }

    public void ActiveName()
    {
        if (this.obj != null)
        {
            this.obj.transform.Find("PlayerInfo/txt_name").gameObject.SetActive(true);
        }
    }

    public void Distory()
    {
        if (this.Target != null && this.Target.followTarget != null)
        {
            this.Target.followTarget = null;
        }
        this._forceHideTopBtn = false;
        this.poolItem.DisableAndBackToPool(false);
        this.obj = null;
        this.objParent = null;
        if (this.data != null)
        {
            ControllerManager.Instance.GetController<UIHpSystem>().listHp.Remove(this.Owner.EID.Id);
        }
        else if (this.npcdata != null)
        {
            ControllerManager.Instance.GetController<UIHpSystem>().listHp.Remove(this.Owner.EID.Id);
        }
        for (int i = 0; i < this.usedTextureAssets.Count; i++)
        {
            this.usedTextureAssets[i].TryUnload();
        }
        this.usedTextureAssets.Clear();
    }

    public void SetDeathStateColor()
    {
        Transform transform = this.obj.transform.Find("PlayerInfo/txt_name");
        Transform transform2 = this.obj.transform.Find("PlayerInfo/txt_owner");
        Transform transform3 = this.objHP.transform.Find("Background");
        if (transform != null)
        {
            transform.GetComponent<Text>().color = Const.GetColorByName("death");
            transform.GetComponent<Outline>().effectColor = Const.GetColorByName("deathoutline");
        }
        if (transform2 != null)
        {
            transform2.GetComponent<Text>().color = Const.GetColorByName("death");
            transform2.GetComponent<Outline>().effectColor = Const.GetColorByName("deathoutline");
        }
        if (transform3 != null)
        {
            transform3.GetComponent<Image>().color = Const.GetColorByName("deathhp");
        }
    }

    public void InitNPCTopButton()
    {
        this.npcTopBtn = this.obj.transform.Find("btn_visit").transform.gameObject;
        RawImage component = this.npcTopBtn.transform.Find("img_visit").GetComponent<RawImage>();
        component.gameObject.SetActive(false);
        this.npcTopBtn.SetActive(false);
        Scheduler.Instance.AddTimer(0.5f, false, new Scheduler.OnScheduler(this.TryShowTopButton));
    }

    public void TryShowTopButton()
    {
        if (this.Owner == null || MainPlayer.Self == null)
        {
            return;
        }
        if (this.Owner is Npc)
        {
            Npc npc = this.Owner as Npc;
            PlayerBufferControl component = npc.GetComponent<PlayerBufferControl>();
            if (component != null && component.ContainsState(UserState.USTATE_TIPS))
            {
                FFDetectionNpcControl component2 = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
                if (component2 != null)
                {
                    bool flag = component2.IsNearestNpc(npc.EID.Id);
                    if (flag)
                    {
                        npc.CloseTopBtn(true);
                        npc.ShowTopBtn();
                        return;
                    }
                }
                float num = 0f;
                float cacheField_Float = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("npcVisiteShowDis").GetCacheField_Float("value");
                bool flag2 = this.IsInRange(this.Owner.CurrentPosition2D, MainPlayer.Self.CurrentPosition2D, cacheField_Float, out num);
                if (flag2)
                {
                    npc.CloseTopBtn(true);
                    npc.ShowTopBtn();
                }
            }
        }
    }

    public void ShowNPCTopButton(string strIcon)
    {
        if (this._forceHideTopBtn)
        {
            return;
        }
        if (this.npcTopBtn == null)
        {
            return;
        }
        this.npcTopBtn.SetActive(true);
        this.UpdateTopIcon(strIcon);
    }

    public void CloseNPCTopButton()
    {
        if (this.npcTopBtn == null)
        {
            return;
        }
        this.npcTopBtn.SetActive(false);
    }

    public void UpdateTopIcon(string strIcon)
    {
        RawImage img = this.npcTopBtn.transform.Find("img_visit").GetComponent<RawImage>();
        if (string.IsNullOrEmpty(strIcon))
        {
            img.gameObject.SetActive(false);
            this.npcTopBtn.SetActive(false);
            return;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ICON, strIcon, delegate (UITextureAsset item)
        {
            if (item != null && item.textureObj != null)
            {
                if (img == null)
                {
                    return;
                }
                this.usedTextureAssets.Add(item);
                Sprite sprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                img.texture = sprite.texture;
                img.color = Color.white;
                img.gameObject.SetActive(true);
            }
        });
    }

    private void OnClickTopButton()
    {
        this.ClickTopButton();
    }

    public void ClickTopButton()
    {
        if (this.Owner == null)
        {
            return;
        }
        if (this.Owner is OtherPlayer)
        {
            PlayerVisitCtrl component = this.Owner.GetComponent<PlayerVisitCtrl>();
            if (component != null)
            {
                component.ClickPlayerTopBtn();
            }
            return;
        }
        if (this.Owner is Npc)
        {
            Npc npc = this.Owner as Npc;
            npc.CloseTopBtn(true);
        }
        float num = 0f;
        float range = Const.DistNpcVisitResponse * 1f - 1f;
        bool flag = this.IsInRange(this.Owner.NextPosition2D, MainPlayer.Self.NextPosition2D, range, out num);
        if (flag)
        {
            MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
            if (controller != null && controller.mainView != null)
            {
                controller.mainView.ShorcutVisitNpc(this.Owner.EID.Id);
            }
        }
        else
        {
            MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(this.Owner, PathFindFollowTarget.FollowType.FollowToVisite, new Action<CharactorBase, bool>(this.OnReachTarget));
        }
    }

    private bool IsInRange(Vector2 selfServerPos, Vector2 npcServerPos, float range, out float distance)
    {
        bool result = false;
        distance = 0f;
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(selfServerPos);
        Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(npcServerPos);
        distance = Vector3.Distance(worldPosByServerPos, worldPosByServerPos2);
        if (distance <= range)
        {
            result = true;
        }
        return result;
    }

    private void OnReachTarget(CharactorBase cbase, bool isreach)
    {
        if (!isreach)
        {
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null && controller.mainView != null)
        {
            controller.mainView.ShorcutVisitNpc(this.Owner.EID.Id);
        }
    }

    public Transform GetEffectBindPoint()
    {
        if (this.obj != null)
        {
            return this.obj.transform.Find("effect");
        }
        return null;
    }

    public void UpdateSelectBtnPos()
    {
        if (this.Owner == null || this.selectBtn == null)
        {
            return;
        }
        Vector3 a = Vector3.zero;
        if (null != this.Owner.GetFeetSelectTrans())
        {
            a = this.Owner.GetFeetSelectTrans().position;
        }
        else if (null != this.Owner.ModelObj)
        {
            a = this.Owner.ModelObj.transform.position;
        }
        this.selectBtn.transform.position = a + new Vector3(0f, 0.1f, 0f);
        this.selectBtn.transform.localEulerAngles = Vector3.zero;
        this.selectBtn.transform.localScale = Vector3.one;
    }

    public void SetSelect(bool BenSelect)
    {
        if (this.obj == null)
        {
            return;
        }
        if (this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name") != null)
        {
            Text component = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>();
            component.fontSize = 18;
        }
        if (this.obj.transform.Find("PlayerInfo/txt_name") != null)
        {
            Text component2 = this.obj.transform.Find("PlayerInfo/txt_name").GetComponent<Text>();
            component2.fontSize = 18;
        }
        if (this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname") != null)
        {
            Text component3 = this.obj.transform.Find("PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Text>();
            component3.fontSize = 18;
        }
        this.IsSelect = BenSelect;
    }

    public void SetSelectUIActive(bool active)
    {
        this.selectBtn.gameObject.SetActive(active);
        this.RefreshRoleUI();
        this.UpdateTweens();
        this.UpdateSelectBtnPos();
        if (active)
        {
            if (!this._isUpdating)
            {
                this._isUpdating = true;
                Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.SelectUpdator));
            }
        }
        else if (this._isUpdating)
        {
            this._isUpdating = false;
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.SelectUpdator));
        }
    }

    public void SetSelectUIColor(Color _color, CharactorBase target)
    {
        if (this.mptsm == null)
        {
            this.mptsm = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        }
        if (this.mptsm == null)
        {
            return;
        }
        if (target != this.mptsm.TargetCharactor)
        {
            return;
        }
        this.img_selectArrowL.color = _color;
        this.img_selectArrowR.color = _color;
        this.img_selectImgL.color = _color;
        this.img_selectImgR.color = _color;
        this.img_circleSmall.color = _color;
        this.img_circleBig.color = _color;
        if (this.m_ProjectorBig)
        {
            this.m_ProjectorBig.material.SetColor("_MainColor", _color);
        }
        if (this.m_ProjectorSmall)
        {
            this.m_ProjectorSmall.material.SetColor("_MainColor", _color);
        }
    }

    public void SetSelectUIColor(bool isRed)
    {
        this._isRed = isRed;
        string name = string.Empty;
        if (isRed)
        {
            this._curTime = 0f;
            name = "itemlimit";
        }
        else
        {
            name = "normalwhite";
        }
        this.img_selectArrowL.color = Const.GetColorByName(name);
        this.img_selectArrowR.color = Const.GetColorByName(name);
        this.img_selectImgL.color = Const.GetColorByName(name);
        this.img_selectImgR.color = Const.GetColorByName(name);
        this.img_circleSmall.color = Const.GetColorByName(name);
        this.img_circleBig.color = Const.GetColorByName(name);
    }

    private void UpdateTweens()
    {
        float num = 45f;
        float num2 = 60f;
        float num3 = 15f;
        if (this.Owner == null)
        {
            return;
        }
        if (this.Owner is Npc)
        {
            Npc npc = this.Owner as Npc;
            int num4 = (int)npc.NpcData.MapNpcData.ApparentBodySize;
            if (num4 == 0)
            {
                num4 = 100;
            }
            num += (float)(num4 - 100);
            num2 = num + num3;
        }
        this.img_selectImgL.transform.localPosition = new Vector3(-num, 0f, 0f);
        this.img_selectImgR.transform.localPosition = new Vector3(num, 0f, 0f);
        TweenPosition component = this.img_selectArrowL.GetComponent<TweenPosition>();
        TweenPosition component2 = this.img_selectArrowR.GetComponent<TweenPosition>();
        component.from = new Vector3(-(num2 + num3), 0f, 0f);
        component.to = new Vector3(-num2, 0f, 0f);
        component2.from = new Vector3(num2 + num3, 0f, 0f);
        component2.to = new Vector3(num2, 0f, 0f);
    }

    private void SelectUpdator()
    {
        this.UpdateSelectBtnPos();
        if (this._isRed)
        {
            if (this._curTime < this._fDuartionTime)
            {
                this._curTime += Time.deltaTime;
            }
            else
            {
                this._isRed = false;
                this._curTime = 0f;
                this.RefreshRoleUI();
            }
        }
    }

    private void SetHpBarSprite()
    {
        if (this.gs.isAbattoirScene)
        {
            this.SetHpBarSpriteByColorConfig();
        }
        else
        {
            this.relation = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
            this.SetHpBarSpriteByRelation(this.relation);
        }
    }

    private void SetHpBarSpriteByColorConfig()
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        string colorImageSpFillNameByUid = controller.GetColorImageSpFillNameByUid(this.Owner.EID.Id);
        string colorImageSpBgNameByUid = controller.GetColorImageSpBgNameByUid(this.Owner.EID.Id);
        if (this.objHP != null)
        {
            Transform bgOut = this.objHP.transform.Find("Background");
            Transform bgIn = this.objHP.transform.Find("Slider/Background");
            Transform fillOut = this.objHP.transform.Find("FillArea/Fill");
            Transform fillIn = this.objHP.transform.Find("Slider/FillArea/Fill");
            UITextureMgr.Instance.GetSpriteFromAtlas("base", colorImageSpBgNameByUid, delegate (Sprite sp)
            {
                if (bgOut != null)
                {
                    bgOut.GetComponent<Image>().overrideSprite = sp;
                }
                if (bgIn != null)
                {
                    bgIn.GetComponent<Image>().overrideSprite = sp;
                }
            });
            UITextureMgr.Instance.GetSpriteFromAtlas("base", colorImageSpFillNameByUid, delegate (Sprite sp)
            {
                if (fillOut != null)
                {
                    fillOut.GetComponent<Image>().overrideSprite = sp;
                }
                if (fillIn != null)
                {
                    fillIn.GetComponent<Image>().overrideSprite = sp;
                }
            });
        }
        if (this.IsSelect)
        {
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (uiobject != null)
            {
                uiobject.UpdateSpriteOrColorByPlayerUid(this.Owner.EID.Id);
            }
        }
    }

    public const float HP_DUARTION = 3f;

    public GameObject objParent;

    private GameObject obj;

    private float hp;

    private float maxhp;

    private Hpchange hitValue;

    private Hpchange expChange;

    private cs_MapUserData data;

    private cs_MapNpcData npcdata;

    private UIFllowTarget Target;

    private GameObject hpParent;

    private bool bSelf;

    private GameObject killedBtn;

    private GameObject npcTopBtn;

    private GameObject selectBtn;

    private Image img_selectArrowL;

    private Image img_selectArrowR;

    private Image img_selectImgL;

    private Image img_selectImgR;

    private Image img_circleSmall;

    private Image img_circleBig;

    private Projector m_ProjectorSmall;

    private Projector m_ProjectorBig;

    private Color m_ProjectorColor;

    private CharactorBase Owner;

    private GameObject charactorName;

    private GameObject functionIcon;

    private Transform hpHitRoot;

    private GameObject objEffectSet;

    private bool _forceHideTopBtn;

    private List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();

    private GameObject objHP_;

    private UI_EffectComponent effect1;

    private NormalObjectInPool poolItem;

    [SerializeField]
    private RelationType relation = RelationType.Neutral;

    private MainPlayerTargetSelectMgr mptsm;

    private float sliderValue;

    private float sliderTargetValue;

    private float bloodSpeed = 1f;

    private Slider sliderIn;

    private float curTime;

    private float forwardDis = 1.5f;

    private bool IsSelect;

    private float _fDuartionTime = 1f;

    private float _curTime;

    private bool _isUpdating;

    private bool _isRed;
}
