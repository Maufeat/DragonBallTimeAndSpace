using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using battle;
using career;
using Chat;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using mobapk;
using msg;
using Pet;
using quiz;
using Team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MainView : UIPanelBase
{
    private GameScene gs
    {
        get
        {
            if (this.gs_ == null)
            {
                this.gs_ = ManagerCenter.Instance.GetManager<GameScene>();
            }
            return this.gs_;
        }
    }

    private PetController petController
    {
        get
        {
            return ControllerManager.Instance.GetController<PetController>();
        }
    }

    private TaskUIController taskuiController
    {
        get
        {
            return ControllerManager.Instance.GetController<TaskUIController>();
        }
    }

    private MainUIController uimainController
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    private SevenDaysController sevendaysController
    {
        get
        {
            return ControllerManager.Instance.GetController<SevenDaysController>();
        }
    }

    private AbattoirMatchController abattoirController
    {
        get
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>();
        }
    }

    private float CurrMp
    {
        get
        {
            if (MainPlayer.Self == null)
            {
                return 100f;
            }
            return MainPlayer.Self.MainPlayeData.AttributeData.mp;
        }
    }

    private float MaxMp
    {
        get
        {
            if (MainPlayer.Self == null)
            {
                return 100f;
            }
            return MainPlayer.Self.MainPlayeData.AttributeData.maxmp;
        }
    }

    public void ReadMessage(MessageType type)
    {
        this.DicMessageAndObj = this.DicMessageAndObj.ClearNullItems<MessageType, GameObject>();
        if (this.DicMessageAndObj.ContainsKey(type))
        {
            this.DicMessageAndObj[type].transform.Find("txt_number").GetComponent<Text>().text = string.Empty;
            this.DicMessageAndObj[type].SetActive(false);
            this.DicMessageAndObj.Remove(type);
        }
    }

    public void AddMessageIcon(MessageType msgType, int count, Action callBack)
    {
        this.DicMessageAndObj = this.DicMessageAndObj.ClearNullItems<MessageType, GameObject>();
        if (!this.DicMessageAndObj.ContainsKey(msgType))
        {
            this.DicMessageAndObj[msgType] = this.btnMsgItems[(int)msgType];
        }
        this.setMsgItem(this.DicMessageAndObj[msgType], this.GetMessageCount(msgType) + count, callBack);
    }

    public void ShowFriendTip(bool isShow)
    {
        this.obj_numbertip.SetActive(isShow);
    }

    public int GetMessageCount(MessageType msgType)
    {
        this.DicMessageAndObj = this.DicMessageAndObj.ClearNullItems<MessageType, GameObject>();
        if (!this.DicMessageAndObj.ContainsKey(msgType))
        {
            this.DicMessageAndObj[msgType] = this.btnMsgItems[(int)msgType];
        }
        return this.DicMessageAndObj[msgType].transform.Find("txt_number").GetComponent<Text>().text.ToInt();
    }

    private void setMsgItem(GameObject go, int num, Action callBack)
    {
        if (go == null)
        {
            return;
        }
        go.SetActive(true);
        if (go.transform.Find("txt_number") != null)
        {
            go.transform.Find("txt_number").GetComponent<Text>().text = num.ToString();
        }
        UIEventListener.Get(go).onClick = delegate (PointerEventData eventData)
        {
            callBack();
        };
    }

    public void DeleteMessageIcon()
    {
    }

    private string GetExpPlayerCb()
    {
        return this.expPlayerTip;
    }

    private string GetExpCharacterCb()
    {
        return this.expCharactorTip;
    }

    public void RefreshHeroExpValue(uint level, uint exp)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            this.img_exp_character.fillAmount = 0f;
            this.expCharactorTip = "0/0";
        }
        else
        {
            uint curLevelAllExp = this.uimainController.GetCurLevelAllExp(level);
            float fillAmount = 1f;
            if (curLevelAllExp > 0U)
            {
                fillAmount = exp * 1f / curLevelAllExp;
            }
            this.img_exp_character.fillAmount = fillAmount;
            this.expCharactorTip = exp + "/" + curLevelAllExp;
        }
        this.headLevel2.text = CommonTools.GetLevelFormat(level);
    }

    public void RefreshExpValue(uint level, uint exp)
    {
        try
        {
            this.headLevel.text = CommonTools.GetLevelFormat(level);
            uint num = 0U;
            bool flag = this.uimainController.TryGetLevelAllExp(level, out num);
            float num2;
            if (num > 0U)
            {
                num2 = exp / num;
                this.expSlider.SetIFMaxLevel(false);
            }
            else
            {
                num2 = 1f;
                this.expSlider.SetIFMaxLevel(true);
            }
            this.panelBottom.Find("exp/ExpText").GetComponent<Text>().text = exp.ToString() + "/" + num.ToString();
            this.img_exp_player.fillAmount = num2;
            this.expPlayerTip = exp + "/" + num;
            if (MainPlayer.Self != null)
            {
                if (MainPlayer.Self.OtherPlayerData != null)
                {
                    if (level > MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level)
                    {
                        this.expSlider.LevelUp(level - MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level, num2);
                    }
                    else
                    {
                        this.expSlider.ChangeValue(num2);
                    }
                }
                else
                {
                    FFDebug.LogWarning("RefreshExpValue", "UI_MainView MainPlayer.Self.OtherPlayerData is null");
                }
            }
            else
            {
                FFDebug.LogWarning("RefreshExpValue", "UI_MainView MainPlayer.Self is null");
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogWarning("RefreshExpValue", ex.ToString());
        }
    }

    public void setPersonal()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        if (MainPlayer.Self.OtherPlayerData == null || MainPlayer.Self.MainPlayeData == null)
        {
            return;
        }
        if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata == null || MainPlayer.Self.MainPlayeData.CharacterBaseData == null)
        {
            return;
        }
        uint level = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level;
        ulong exp = MainPlayer.Self.MainPlayeData.CharacterBaseData.exp;
        this.RefreshPersonalLeader();
        this.panelPersonal.Find("txt_playername").GetComponent<Text>().text = MainPlayer.Self.OtherPlayerData.MapUserData.name;
        UI_MainView.hp = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.hp;
        UI_MainView.maxhp = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.maxhp;
        this.personalHpSlider = this.panelPersonal.Find("hp/Slider").GetComponent<Slider>();
        this.personalHpSlider.value = UI_MainView.hp / UI_MainView.maxhp;
        this.panelPersonal.Find("txt_hp/value").GetComponent<Text>().text = UI_MainView.hp + "/" + UI_MainView.maxhp;
        this.panelPersonal.Find("hp/txt_percent").GetComponent<Text>().text = Mathf.Ceil(UI_MainView.hp / UI_MainView.maxhp * 100f) + "%";
        this.watchedhp = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("WatchedHp").GetCacheField_Float("value");
        uint curLevelAllExp = this.uimainController.GetCurLevelAllExp(level);
        this.panelBottom.Find("exp/ExpText").GetComponent<Text>().text = exp.ToString() + "/" + curLevelAllExp.ToString();
        float allValue;
        if (curLevelAllExp > 0U)
        {
            allValue = exp / curLevelAllExp;
            this.expSlider.SetForwardColor(Color.white);
        }
        else
        {
            allValue = 1f;
            this.expSlider.SetForwardColor(ConstClient.MaxLevelColor);
        }
        this.expSlider.SetAllValue(allValue);
        this.m_inBattleEffect.SetActive(false);
        FightValueNum fightValueNum = this.img_fightvalue_item.transform.parent.GetComponent<FightValueNum>();
        if (fightValueNum == null)
        {
            fightValueNum = this.img_fightvalue_item.transform.parent.gameObject.AddComponent<FightValueNum>();
        }
        fightValueNum.SetNum(MainPlayer.Self.GetMainPlayerFightValue());
        MainPlayerTargetSelectMgr mptsm = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        if (mptsm != null && mptsm.TargetCharactor != null)
        {
            OtherPlayer otherPlayer = mptsm.TargetCharactor as OtherPlayer;
            if (otherPlayer != null && otherPlayer.OtherPlayerData.MapUserData.charid == MainPlayer.Self.OtherPlayerData.MapUserData.charid)
            {
                Scheduler.Instance.AddTimer(0.15f, false, delegate
                {
                    mptsm.SetTargetNull();
                });
                Scheduler.Instance.AddTimer(0.2f, false, delegate
                {
                    this.BtnSelectSelfTargetEvent();
                });
            }
        }
    }

    public void RefreshFightValue()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        FightValueNum fightValueNum = this.img_fightvalue_item.transform.parent.GetComponent<FightValueNum>();
        if (fightValueNum == null)
        {
            fightValueNum = this.img_fightvalue_item.transform.parent.gameObject.AddComponent<FightValueNum>();
        }
        fightValueNum.SetNum(MainPlayer.Self.GetMainPlayerFightValue());
    }

    public void RefreshMainPlayerMp()
    {
    }

    public void RefreshPersonalLeader()
    {
        GameObject gameObject = this.panelPersonal.Find("img_leader").gameObject;
        GameObject gameObject2 = this.panelPersonal.Find("img_inv").gameObject;
        gameObject.SetActive(this.teamController.IsMainPlayerLeader());
        gameObject2.SetActive(this.teamController.IsHasInviteAbility());
        if (this.teamController.myTeamInfo == null || MainPlayer.Self == null || MainPlayer.Self.OtherPlayerData == null)
        {
            this.btn_TeamCallBack.SetActive(false);
            return;
        }
        if (this.teamController.myTeamInfo.id != 0U && MainPlayer.Self.OtherPlayerData.MapUserData != null)
        {
            if (this.teamController.myTeamInfo.leaderid == MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString())
            {
                bool active = false;
                for (int i = 0; i < this.teamController.myTeamInfo.mem.Count; i++)
                {
                    if (!this.teamController.myTeamInfo.mem[i].mememberid.Equals(MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString()))
                    {
                        if (this.teamController.myTeamInfo.mem[i].state == MemState.AWAY)
                        {
                            active = true;
                            break;
                        }
                    }
                }
                this.btn_TeamCallBack.SetActive(active);
            }
            else
            {
                this.btn_TeamCallBack.SetActive(false);
            }
        }
        else
        {
            this.btn_TeamCallBack.SetActive(false);
        }
        bool flag = false;
        uint mapid = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        LuaTable copymapInfoByMapid = CommonTools.GetCopymapInfoByMapid(mapid);
        if (copymapInfoByMapid != null)
        {
            flag = (copymapInfoByMapid.GetField_Uint("showinvite") == 1U);
        }
        if (flag)
        {
            this.btn_TeamCallBack.SetActive(false);
        }
    }

    public void ResetMainPlayerHp(float thp, float tmaxhp)
    {
        UI_MainView.hp = thp;
        UI_MainView.maxhp = tmaxhp;
        float num = Mathf.Clamp(UI_MainView.hp / UI_MainView.maxhp, 0f, 1f);
        this.panelPersonal.Find("hp/Slider").GetComponent<Slider>().value = num;
        this.panelPersonal.Find("txt_hp/value").GetComponent<Text>().text = UI_MainView.hp + "/" + UI_MainView.maxhp;
        this.panelPersonal.Find("hp/txt_percent").GetComponent<Text>().text = Mathf.Ceil(num * 100f) + "%";
        float cacheField_Float = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("WatchedHp").GetCacheField_Float("value");
        if (num <= cacheField_Float && num != 0f && GameSystemSettings.IsLowHealthWarning())
        {
            ControllerManager.Instance.GetController<AlertController>().ShowAlert(2);
        }
        else
        {
            ControllerManager.Instance.GetController<AlertController>().CloseAlert(2);
        }
    }

    public bool CheckIsLowHealth()
    {
        return this.personalHpSlider.value <= this.watchedhp && this.personalHpSlider.value != 0f;
    }

    public void ResetMainPlayerCareer(uint level, uint curJob)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("careerLv", (ulong)level);
        base.GetTexture(ImageType.OTHERS, configTable.GetField_String("careerback"), delegate (Texture2D item)
        {
            if (this.imgjobBack == null)
            {
                return;
            }
            if (item != null)
            {
                Image image = this.imgjobBack;
                Sprite overrideSprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                image.overrideSprite = overrideSprite;
            }
            else
            {
                FFDebug.LogWarning(this, "    req  texture   is  null ");
            }
        });
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("careerIcon");
        LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(curJob.ToString());
        if (cacheField_Table2 != null)
        {
            string cacheField_String = cacheField_Table2.GetCacheField_String("iconname");
            base.GetTexture(ImageType.ICON, cacheField_String, delegate (Texture2D item)
            {
                if (item != null)
                {
                    Image image = this.imgjobIcon;
                    Sprite overrideSprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                    image.overrideSprite = overrideSprite;
                }
                else
                {
                    FFDebug.LogWarning(this, "    req  texture   is  null ");
                }
            });
        }
    }

    public void RefreshPetInfo()
    {
        this.petFightData = null;
        this.petAssistData = null;
        MainPlayer.Self.FightPet = null;
        MainPlayer.Self.AssistPet = null;
        if (this.petController.ListPetData.Count == 0)
        {
            this.panelPet.gameObject.SetActive(false);
            return;
        }
        this.panelPet.gameObject.SetActive(true);
        for (int i = 0; i < this.petController.ListPetData.Count; i++)
        {
            if (this.petController.ListPetData[i].state.Contains(PetState.PetState_Fight))
            {
                this.petFightData = this.petController.ListPetData[i];
                MainPlayer.Self.FightPet = this.petFightData;
            }
            if (this.petController.ListPetData[i].state.Contains(PetState.PetState_Assist))
            {
                this.petAssistData = this.petController.ListPetData[i];
                MainPlayer.Self.AssistPet = this.petAssistData;
            }
        }
        this.setFightPet();
    }

    private void setFightPet()
    {
        if (this.petFightData == null)
        {
            this.fightpetObjectbg.SetActive(false);
            this.petBlank.SetActive(true);
        }
        else
        {
            this.fightpetObjectbg.SetActive(true);
            this.petBlank.SetActive(false);
            this.setPetInfo(this.fightpetObjectbg, this.petFightData);
        }
        if (this.petAssistData == null)
        {
            this.assistPetObjectbg.SetActive(false);
            this.assistPetBlank.SetActive(true);
        }
        else
        {
            this.assistPetObjectbg.SetActive(true);
            this.assistPetBlank.SetActive(false);
            this.setPetInfo(this.assistPetObjectbg, this.petAssistData);
        }
    }

    private void setPetInfo(GameObject ga, PetBase data)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("summonpet", (ulong)data.id);
        this.setRawTexture(ga.transform.Find("img_icon").gameObject, configTable.GetField_String("icon"), ImageType.ITEM);
        if (data.state.Contains(PetState.PetState_Dying))
        {
            ga.transform.Find("img_mask").gameObject.SetActive(true);
        }
        else
        {
            ga.transform.Find("img_mask").gameObject.SetActive(false);
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
            RawImage component = ga.GetComponent<RawImage>();
            if (component != null)
            {
                component.texture = sprite.texture;
                component.color = Color.white;
            }
        });
    }

    private void setTexture(GameObject ga, string icon, ImageType type)
    {
        base.GetTexture(type, icon, delegate (Texture2D item)
        {
            Image component = ga.GetComponent<Image>();
            if (component == null)
            {
                FFDebug.LogWarning(this, "   image  is  null");
                return;
            }
            Sprite overrideSprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
            component.overrideSprite = overrideSprite;
        });
    }

    private void setRawTexture(GameObject ga, string icon, ImageType type)
    {
        base.GetTexture(type, icon, delegate (Texture2D item)
        {
            RawImage component = ga.GetComponent<RawImage>();
            if (component == null)
            {
                FFDebug.LogWarning(this, "   image  is  null");
                return;
            }
            Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
            component.texture = sprite.texture;
            component.color = Color.white;
        });
    }

    private uint getScecond(uint time)
    {
        return time / 1000U;
    }

    public void EnableBtnPetQte(uint leftTime, uint timeCount, uint distance, ulong npcID, uint distanceratio)
    {
        PetBase petBase = null;
        for (int i = 0; i < this.petController.ListPetData.Count; i++)
        {
            if (this.petController.ListPetData[i].state.Contains(PetState.PetState_Fight))
            {
                petBase = this.petController.ListPetData[i];
                break;
            }
        }
        if (petBase == null || npcID == 0UL)
        {
            FFDebug.LogWarning(this, "  there  is no fight  Pet!   ||  npcID==0 ");
            return;
        }
        if (timeCount == 0U)
        {
            this._LeftTime = 0f;
            this.imgPetQte.fillAmount = 1f;
        }
        else
        {
            this._LeftTime = this.getScecond(leftTime);
            this._maxTime = this.getScecond(timeCount);
        }
        this._distance = distance;
        this.objBtnPetQte.gameObject.SetActive(true);
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)petBase.talentskillid);
        string icon = configTable.GetField_String("skillicon").Split(new char[]
        {
            ','
        }, StringSplitOptions.RemoveEmptyEntries)[0];
        this.setTexture(this.objBtnPetQte.transform.Find("img_icon").gameObject, icon, ImageType.ITEM);
        UIEventListener.Get(this.objBtnPetQte).onClick = delegate (PointerEventData pointData)
        {
            EntitiesID entryident = default(EntitiesID);
            entryident.Etype = CharactorType.NPC;
            entryident.Id = npcID;
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(entryident) as Npc;
            if (npc != null)
            {
                Vector2 vector = new Vector2(MainPlayer.Self.ModelObj.transform.position.x, MainPlayer.Self.ModelObj.transform.position.z);
                Vector2 vector2 = new Vector2(npc.ModelObj.transform.position.x, npc.ModelObj.transform.position.z);
                Vector2 skillpostion = this.getSkillpostion(this._distance, vector, vector2, distanceratio);
                Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(new Vector3(skillpostion.x, 0f, skillpostion.y), true);
                uint serverDirByClientDir = CommonTools.GetServerDirByClientDir(vector2 - vector);
                this.relaseQTESkill(npcID, serverPosByWorldPos, serverDirByClientDir);
            }
        };
    }

    private Vector2 getSkillpostion(float configDistance, Vector2 v1, Vector2 v2, uint distanceratio)
    {
        float num = distanceratio / 100f;
        float num2 = Vector2.Distance(v1, v2);
        if (num2 * num < configDistance * LSingleton<CurrentMapAccesser>.Instance.CellSizeX)
        {
            float num3 = v2.x - v1.x;
            float num4 = v2.y - v1.y;
            float x = num * num3 + v1.x;
            float y = num * num4 + v1.y;
            return new Vector2(x, y);
        }
        float x2 = configDistance / num2 * (v2.x - v1.x) + v1.x;
        float y2 = configDistance / num2 * (v2.y - v1.y) + v1.y;
        return new Vector2(x2, y2);
    }

    private void relaseQTESkill(ulong bossid, Vector2 position, uint dir)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().ReqRelasePetQTESkill(bossid, dir, position);
    }

    public void DisAbleBtnPetQte()
    {
        this._LeftTime = 0f;
        this._maxTime = 0f;
        this.objBtnPetQte.gameObject.SetActive(false);
    }

    private void QTEUpDate()
    {
        if (this._LeftTime > 0f)
        {
            this._LeftTime -= Scheduler.Instance.realDeltaTime;
            this.imgPetQte.fillAmount = this._LeftTime / this._maxTime;
            if (this._LeftTime <= 0f)
            {
                this.imgPetQte.fillAmount = 0f;
            }
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        if (root == null)
        {
            return;
        }
        //CommonTools.SetScalerMode(CanvasScaler.ScaleMode.ConstantPixelSize);
        this.CombCd = new UI_CombCD();
        this.CombCd.Init(this);
        this.DramaTips = new UI_DramaTips();
        this.DramaTips.Init(root);
        this.VisitNpcView = new UI_VisitNpcInMainView();
        this.VisitNpcView.Init(root);
        this.AchievementView = new UI_Achievement_MainView();
        this.AchievementView.Init(root);
        this.Root = root.gameObject;
        this.transGroupTeamPanel = this.Root.transform.Find("Offset_Main/Panelteam2");
        this.abattoirRankPanel = this.Root.transform.Find("Offset_Main/panel_AbattoirRank");
        Transform transform = this.Root.transform.Find("Offset_Main/Panel_MenuPC");
        this.btn_auto_fight = this.Root.transform.Find("Offset_Main/btn_auto_fight").GetComponent<Button>();
        ShortcutsConfigController scc = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        GameObject gameObject = transform.Find("btn_story").gameObject;
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_story_on_click);
        TextTip textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetText("剧情");
        gameObject = transform.Find("btn_hero").gameObject;
        UIEventListener.Get(transform.Find("btn_hero").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_hero_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "角色(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Role) + ")");
        gameObject = transform.Find("btn_herohandbook").gameObject;
        UIEventListener.Get(transform.Find("btn_herohandbook").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_herohandbook_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "英雄图鉴(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Pokedex) + ")");
        gameObject = transform.Find("btn_bag").gameObject;
        UIEventListener.Get(transform.Find("btn_bag").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_bag_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "背包(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Bag) + ")");
        this.objBagHasNew = gameObject.transform.Find("Image").gameObject;
        this.objBagHasNew.SetActive(false);
        gameObject = transform.Find("btn_dna").gameObject;
        UIEventListener.Get(transform.Find("btn_dna").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_dna_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "强化(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Gene) + ")");
        gameObject = transform.Find("btn_tasklist").gameObject;
        UIEventListener.Get(transform.Find("btn_tasklist").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_tasklist_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "任务(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Task) + ")");
        gameObject = transform.Find("btn_friend").gameObject;
        UIEventListener.Get(transform.Find("btn_friend").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_friend_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "好友(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.FriendMenu) + ")");
        this.obj_numbertip = transform.Find("btn_friend/NumberTip").gameObject;
        gameObject = transform.Find("btn_master").gameObject;
        UIEventListener.Get(transform.Find("btn_master").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_master_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetText("师徒");
        gameObject = transform.Find("btn_family").gameObject;
        UIEventListener.Get(transform.Find("btn_family").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_family_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "家族(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.GuildMenu) + ")");
        gameObject = transform.Find("btn_settings").gameObject;
        UIEventListener.Get(transform.Find("btn_settings").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_settings_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetText("设置");
        gameObject = transform.Find("btn_pvp").gameObject;
        UIEventListener.Get(transform.Find("btn_pvp").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_pvp_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "武道会(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Budokai) + ")");
        gameObject = transform.Find("btn_skill").gameObject;
        Transform transform2 = transform.Find("btn_skill/Image");
        if (transform2 != null)
        {
            this.skillNewIcon = transform2.gameObject;
        }
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_skill_on_click);
        textTip = gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "技能(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.UnLockSkills) + ")");
        textTip = this.btn_auto_fight.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = this.btn_auto_fight.gameObject.AddComponent<TextTip>();
        }
        this.btn_auto_fight.onClick.RemoveAllListeners();
        this.btn_auto_fight.onClick.AddListener(new UnityAction(this.AutoFight));
        textTip.SetTextCb(() => "自动战斗(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.AutoFight) + ")");
        this.SetAutoFightBtnState();
        SkillButton.CanelBtn = this.Root.transform.Find("Offset_Main/PanelSkill/btn_cancel").gameObject;
        SkillButton.CanelBtn.SetActive(false);
        this.objBtnPetQte = this.Root.transform.Find("Offset_Main/PanelSkill/btn_petQTE").gameObject;
        this.objBtnPetQte.SetActive(false);
        this.imgPetQte = this.Root.transform.Find("Offset_Main/PanelSkill/btn_petQTE/img_cd").GetComponent<Image>();
        for (int i = 1; i <= 12; i++)
        {
            GameObject gameObject2 = this.Root.transform.Find("Offset_Main/PanelSkill/btn_skill" + i).gameObject;
            SkillButton skillButton = gameObject2.AddComponent<SkillButton>();
            skillButton.skillId = i;
            skillButton.BtnIndexId = i;
            skillButton.Init();
            skillButton.SetAllSkillButton(ref this.SkillButtonList);
            this.SkillButtonList.Add(skillButton);
            this.txt_skillList.Add(gameObject2.transform);
        }
        for (int j = 1; j <= 12; j++)
        {
            GameObject gameObject3 = this.Root.transform.Find("Offset_Main/PanelShortcut/Panel/Panel_condition/skill" + j).gameObject;
            GameObject gameObject4 = this.Root.transform.Find("Offset_Main/PanelShortcut/Panel/Panel_condition/groove" + j).gameObject;
            SkillButton skillButton2 = gameObject3.AddComponent<SkillButton>();
            skillButton2._skillReleaseType = SkillRelaseType.SkillEvolution;
            skillButton2.skillId = j;
            skillButton2.BtnIndexId = j;
            skillButton2.Init();
            skillButton2.SetAllSkillButton(ref this.EvolutionButtonList);
            this.EvolutionButtonList.Add(skillButton2);
            this.EvolutionBackGo.Add(gameObject4);
        }
        this.InitGameObject();
        this.Root.AddComponent<UI_ShortcutControl>().OnInit(this.Root.transform.Find("Offset_Main/PanelShortcut").gameObject);
        ControllerManager.Instance.GetController<ChatControl>().Initilize(this.Root.transform.Find("ChatPanel"));
        ControllerManager.Instance.GetController<SystemSettingController>().LoadSystemSettingData();
        this.EnableSevenDays(this.sevendaysController.SevenInfo);
        if (this.sevendaysController.SeekInfo != null)
        {
            this.EnableAchievement();
        }
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.InitEvent();
        this.setTaskState();
        this.CheckViewTask();
        this.SetAutoBattleUI(false, true);
        ServerStorageManager.Instance.GetData(ServerStorageKey.FRIEND_IDS, 0U);
        if (MainPlayer.Self == null)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            manager.onMainPlayer = (Action)Delegate.Combine(manager.onMainPlayer, new Action(this.OnSelfOK));
            return;
        }
        this.OnSelfOK();
    }

    private void OnSelfOK()
    {
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        if (controller != null)
        {
            ServerStorageManager.Instance.GetData(controller.skillslotkey, 0U);
        }
        this.RefreshMainPlayerMp();
        this.setPersonal();
        this.RefreshPetInfo();
        Profession occupation = (Profession)MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
        try
        {
            this.InitSkillTexture(true);
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, "InitSkillTexture error:" + arg);
        }
        this.CheckExitCopyBtn();
        uint curLv = this.uimainController.GetCurLv();
        uint exp = (uint)this.uimainController.GetCurExp();
        this.RefreshExpValue(curLv, exp);
        uint secondLv = this.uimainController.GetSecondLv();
        uint exp2 = (uint)this.uimainController.GetSecondExp();
        this.RefreshHeroExpValue(secondLv, exp2);
    }

    public void RefreshSkillButton()
    {
    }

    public void SetSkillBtnShowNew(bool show)
    {
        if (this.skillNewIcon != null && this.skillNewIcon.activeSelf != show)
        {
            this.skillNewIcon.SetActive(show);
        }
    }

    public void FrashShortcutUIShowName()
    {
        for (int i = 0; i < this.SkillButtonList.Count; i++)
        {
            this.SkillButtonList[i].SetShortcutShowName();
        }
    }

    private void InitGameObject()
    {
        this.panelMenu = this.Root.transform.Find("Offset_Main/PanelMenu");
        this.panelTask = this.Root.transform.Find("Offset_Main/PanelTask");
        this.panelTeam = this.Root.transform.Find("Offset_Main/PanelTeam");
        this.TaskList = this.panelTask.Find("TaskList").gameObject;
        this.btn_TaskListHide = this.panelTask.transform.Find("img_taskbg/btn_hide");
        this.TaskListScrollView = this.panelTask.Find("TaskList/TaskScrollView").gameObject;
        this.panelPersonal = this.Root.transform.Find("Offset_Main/PanelPersonal");
        this.panelBottom = this.Root.transform.Find("Offset_Main/PanelBottom");
        this.panelChat = this.Root.transform.Find("ChatPanel");
        this.panelSkill = this.Root.transform.Find("Offset_Main/PanelSkill");
        this.btn_unlockSkill = this.Root.transform.Find("Offset_Main/PanelSkill/btn_unlockskill");
        this.tranTaskSwitch = this.panelTask.Find("switch");
        this.btnTaskOn = this.tranTaskSwitch.Find("btn_task").gameObject;
        this.btnTaskOff = this.tranTaskSwitch.Find("btn_task_off").gameObject;
        this.btnTeamOn = this.tranTaskSwitch.Find("btn_team").gameObject;
        this.btnTeamOff = this.tranTaskSwitch.Find("btn_team_off").gameObject;
        this.goSystemSetting = this.panelMenu.gameObject.FindChild("menu2/left/btn_config");
        this.panel_MapName = this.Root.transform.Find("Offset_Main/Panel_MapName");
        this.btnMsgItems = new GameObject[10];
        this.mapName = this.panel_MapName.Find("txt_map").GetComponent<Text>();
        this.mapNameEn = this.panel_MapName.Find("txt_mapen").GetComponent<Text>();
        this.mapTypIcon = this.panel_MapName.Find("img_icon").GetComponent<Image>();
        GameObject gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_chat").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[1] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = string.Empty;
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_apply").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[3] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = string.Empty;
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_invite").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[2] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = string.Empty;
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_bag").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[4] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = "0";
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_roll").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[6] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = string.Empty;
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_exp").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[7] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = string.Empty;
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_mail").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[5] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = "0";
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_MasterApplly").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[8] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = "0";
        gameObject = this.Root.transform.Find("Offset_Main/PanelBuff/TmpMsg/Item_ApprenticeApply").gameObject;
        gameObject.SetActive(false);
        this.btnMsgItems[9] = gameObject;
        gameObject.transform.Find("txt_number").GetComponent<Text>().text = "0";
        this.panelMenu.gameObject.SetActive(false);
        this.btn_tab = this.panelSkill.Find("btn_tab").GetComponent<Button>();
        this.img_exp_player = this.panelBottom.Find("img_expbg/exp_player").GetComponent<Image>();
        this.img_exp_character = this.panelBottom.Find("img_expbg/exp_character").GetComponent<Image>();
        this.img_exp_player.gameObject.AddComponent<TextTip>().SetTextCb(new TextTipContentCb(this.GetExpPlayerCb));
        this.img_exp_character.gameObject.AddComponent<TextTip>().SetTextCb(new TextTipContentCb(this.GetExpCharacterCb));
        this.expSlider = this.panelBottom.Find("doubleSlider").GetComponent<doubleSlider>();
        this.panelHoldonCountDown = this.Root.transform.Find("Offset_Main/PanelTemp/PanelInfo").gameObject;
        this.lbHoldonCountDownNpcName = this.panelHoldonCountDown.transform.Find("txt_npc").GetComponent<Text>();
        this.lbHoldonCountDownremaintime = this.panelHoldonCountDown.transform.Find("txt_time").GetComponent<Text>();
        this.panelHoldonCountDown.SetActive(false);
        this.imgjobBack = this.panelPersonal.Find("img_job").GetComponent<Image>();
        this.imgjobIcon = this.panelPersonal.Find("img_job/img_icon").GetComponent<Image>();
        this.panelBuffInfo = this.Root.transform.Find("Offset_Main/PanelBuff/PanelBuffInfo").gameObject;
        this.panelBuffInfo.SetActive(false);
        this.spBufferInfoBuffIcon = this.panelBuffInfo.transform.Find("img_buff").GetComponent<Image>();
        this.btnBufferInfoCloseBuffInfo = this.panelBuffInfo.transform.Find("btn_close").GetComponent<Button>();
        this.lbBufferInfoBuffName = this.panelBuffInfo.transform.Find("txt_buffname").GetComponent<Text>();
        this.lbBufferInfoBuffDesc = this.panelBuffInfo.transform.Find("txt_buffdesc").GetComponent<Text>();
        this.panelPet = this.Root.transform.Find("Offset_Main/PanelPet/pet");
        this.fightpetObjectbg = this.panelPet.Find("fightpet/img_bg").gameObject;
        this.petBlank = this.panelPet.transform.Find("fightpet/blank").gameObject;
        this.assistPetObjectbg = this.panelPet.Find("assitpet/img_bg").gameObject;
        this.assistPetBlank = this.panelPet.transform.Find("assitpet/blank").gameObject;
        this.btn_TeamCallBack = this.panelPersonal.Find("btn_team").gameObject;
        int num = 0;
        while ((long)num < (long)((ulong)UI_MainView.maxMemberNum))
        {
            this.teamObj[num] = this.panelTeam.Find("Item" + num.ToString()).gameObject;
            this.headObj[num] = this.teamObj[num].FindChild("img_icon/img_pic");
            this.teamObj[num].SetActive(false);
            UIEventListener.Get(this.teamObj[num]).onClick = new UIEventListener.VoidDelegate(this.TeamMemberClick);
            num++;
        }
        this.mapNameTweenAlpha.AddRange(this.panel_MapName.GetComponentsInChildren<TweenAlpha>());
        this.btn_unlockSkill.gameObject.SetActive(false);
        this.goRegression = this.panelTask.Find("TeamMember/btn_returncopy").gameObject;
        this.targetBossInfo = this.Root.transform.Find("Offset_Main/PanelTarget").gameObject;
        this.Img_BossBG = this.targetBossInfo.GetComponent<Image>();
        this.Img_BossHPF = this.targetBossInfo.FindChild("SliderBoss/FillArea/Fill2").gameObject.GetComponent<Image>();
        this.Img_BossHPB = this.targetBossInfo.FindChild("SliderBoss/FillArea/Fill").gameObject.GetComponent<Image>();
        this.Img_BossHPBG = this.targetBossInfo.FindChild("SliderBoss/Background").gameObject.GetComponent<Image>();
        this.Txt_BossHPLayerNum = this.targetBossInfo.FindChild("name/txt_count").gameObject.GetComponent<Text>();
        this.Txt_BossName = this.targetBossInfo.FindChild("name/txt_name").gameObject.GetComponent<Text>();
        this.Txt_BossLevelNum = this.targetBossInfo.FindChild("name/txt_level").gameObject.GetComponent<Text>();
        this.Slider_BossHP = this.targetBossInfo.FindChild("SliderBoss").gameObject.GetComponent<Slider>();
        this.Slider_BossBack = this.targetBossInfo.FindChild("SliderBoss/FillArea/SliderBack").gameObject.GetComponent<Slider>();
        this.icon3DImageNpc = this.targetBossInfo.FindChild("mask_portrait").gameObject.GetComponent<RawImage>();
        this.Txt_BossHp = this.targetBossInfo.FindChild("txt_hp/value").gameObject.GetComponent<Text>();
        this.Txt_BossHpPercent = this.targetBossInfo.FindChild("txt_hp/percent").gameObject.GetComponent<Text>();
        this.Txt_BossHpPercent.gameObject.SetActive(true);
        this.Txt_BossHpPercent.transform.localPosition = new Vector3(190f, 21f, 0f);
        this.targetOfTargetPanel = this.Root.transform.Find("Offset_Main/2ndTarget");
        this.normalTargetInfo = this.Root.transform.Find("Offset_Main/PanelNormalTarget").gameObject;
        UIEventListener.Get(this.normalTargetInfo.FindChild("ClickArea").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_target_secondmenu_on_click);
        this.Img_NormalBG = this.normalTargetInfo.GetComponent<Image>();
        this.Img_NormalHPF = this.normalTargetInfo.FindChild("SliderBoss/FillArea/Fill").gameObject.GetComponent<Image>();
        this.Img_NormalHPBG = this.normalTargetInfo.FindChild("SliderBoss/Background").gameObject.GetComponent<Image>();
        this.Img_NormalJobBG = this.normalTargetInfo.FindChild("name/name/img_job").gameObject.GetComponent<Image>();
        this.Img_NormalJob = this.normalTargetInfo.FindChild("name/name/img_job/img_icon").gameObject.GetComponent<Image>();
        this.Txt_NormalName = this.normalTargetInfo.FindChild("name/name/txt_name").gameObject.GetComponent<Text>();
        this.Txt_NormalLevelNum = this.normalTargetInfo.FindChild("name/txt_level").gameObject.GetComponent<Text>();
        this.Slider_NormalHP = this.normalTargetInfo.FindChild("SliderBoss").gameObject.GetComponent<Slider>();
        this.Slider_NormalBack = this.normalTargetInfo.FindChild("SliderBoss/FillArea/SliderBack").gameObject.GetComponent<Slider>();
        this.icon3DImage = this.normalTargetInfo.FindChild("mask_portrait").gameObject.GetComponent<RawImage>();
        this.Txt_NormalHp = this.normalTargetInfo.FindChild("txt_hp/value").gameObject.GetComponent<Text>();
        this.Txt_NormalHpPercent = this.normalTargetInfo.FindChild("txt_hp/percent").gameObject.GetComponent<Text>();
        this.Txt_NormalHpPercent.gameObject.SetActive(true);
        this.Txt_NormalHpPercent.transform.localPosition = new Vector3(190f, 21f, 0f);
        this.InitColors();
        this.InitBuffIconController();
        this.CloseTargetInfo();
        if (this.mskillShowImage != null)
        {
            GameObject gameObject2 = this.Root.transform.Find("Offset_Main/Panel_SkillShow").gameObject;
            if (gameObject2 != null)
            {
                this.mskillShowImage.Init(gameObject2);
            }
        }
        this.panelTop = this.Root.transform.Find("Offset_Main/Panel_Top").gameObject;
        this.panelTop.SetActive(true);
        this.btn_activityguide = this.Root.transform.Find("Offset_Main/Panel_Top/btn_activity").gameObject.GetComponent<Button>();
        this.btn_activityguide.gameObject.SetActive(true);
        this.btn_questionnaire = this.Root.transform.Find("Offset_Main/Panel_Top/btn_questionnaire").gameObject.GetComponent<Button>();
        int quesCount = ControllerManager.Instance.GetController<QuestationnaireController>().GetQuesCount();
        this.btn_questionnaire.gameObject.SetActive(quesCount > 0);
        this.btn_sevendays = this.Root.transform.Find("Offset_Main/Panel_Top/btn_7days").gameObject.GetComponent<Button>();
        this.btnCameraReset = this.Root.transform.Find("Offset_Main/PanelSkill/btn_return").gameObject;
        this.btn3dCamera = this.Root.transform.Find("Offset_Main/Panel_Message/3d/btn_2d").gameObject;
        this.btn2dCamera = this.Root.transform.Find("Offset_Main/Panel_Message/3d/btn_3d").gameObject;
        this.headIcon = this.Root.transform.Find("Offset_Main/PanelPersonal/img_portrait1/img_pic").GetComponent<RawImage>();
        this.headIcon2 = this.Root.transform.Find("Offset_Main/PanelPersonal/img_portrait2/img_pic").GetComponent<RawImage>();
        this.headLevel = this.Root.transform.Find("Offset_Main/PanelPersonal/img_portrait1/txt_lv").GetComponent<Text>();
        this.headLevel2 = this.Root.transform.Find("Offset_Main/PanelPersonal/img_portrait2/txt_lv").GetComponent<Text>();
        this.btnVip = this.Root.transform.Find("Offset_Main/btn_vip");
        this.img_fightvalue_item = this.Root.transform.Find("Offset_Main/PanelPersonal/panel_value/img_num").GetComponent<Image>();
        this.btnChangeHero = this.Root.transform.Find("Offset_Main/PanelPersonal/btn_switch").GetComponent<Button>();
        UIEventListener.Get(this.btnChangeHero.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_changehero_on_click);
        this.setCameraIn3DBtn(true);
        this.Offset_Main = this.Root.transform.Find("Offset_Main").gameObject;
        this.com_slidermp = this.Root.transform.Find("Offset_Main/PanelPersonal/mp/Slider ").GetComponent<Slider>();
        this.com_sliderhp = this.Root.transform.Find("Offset_Main/PanelPersonal/hp/Slider").GetComponent<Slider>();
        if (this.com_sliderhp != null)
        {
            this.com_sliderhp.interactable = false;
        }
        if (this.com_slidermp != null)
        {
            this.com_slidermp.interactable = false;
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        controller.mCharacterNetwork.ReqGetMainHero();
        this.setMainHeadIcon();
        this.SetMainHeadIcon2();
        this.m_inBattleEffect = this.panelPersonal.Find("RawImage").gameObject;
        Transform transform = this.Root.transform.Find("Offset_Main/Panel_BattlelLog/Panel_BattlelLog_content/List/Text");
        if (transform != null)
        {
            this.obj_bufflog_item = transform.gameObject;
            this.obj_bufflog_item.SetActive(false);
        }
        Transform transform2 = this.Root.transform.Find("Offset_Main/Panel_BattlelLog/Panel_BattlelSysLog_content/List/Text");
        if (transform2 != null)
        {
            this.obj_bufflog2_item = transform2.gameObject;
            this.obj_bufflog2_item.SetActive(false);
        }
        if (this.gs != null)
        {
            this.gs.RegOnSceneLoadCallBack(new Action(this.OnSceneLoad));
        }
    }

    private void OnSceneLoad()
    {
        this.setMainHeadIcon();
        this.SetMainHeadIcon2();
    }

    private void TeamMemberClick(PointerEventData eventData)
    {
        Memember memember = null;
        if (eventData.pointerPress.name == "PanelPersonal")
        {
            if (eventData.button == PointerEventData.InputButton.Right && this.gs.isAbattoirScene)
            {
                return;
            }
            memember = this.teamController.GetMainPlayerMemeber();
        }
        else
        {
            string name = eventData.pointerPress.name;
            if (!name.StartsWith("Item"))
            {
                return;
            }
            int num;
            if (!int.TryParse(name.Substring(4), out num))
            {
                return;
            }
            if (this.gs.isAbattoirScene)
            {
                List<TeamUser> list = (from item in this.abattoirController.myTeamInfo.users
                                       where item.uid != MainPlayer.Self.OtherPlayerData.MapUserData.charid
                                       select item).ToList<TeamUser>();
                if (num > list.Count)
                {
                    FFDebug.LogError(this, "选中角色超出角色列表Count index:" + num);
                    return;
                }
                TeamUser teamUser = list[num];
                memember = new Memember
                {
                    mememberid = teamUser.uid.ToString(),
                    name = teamUser.name,
                    level = teamUser.level,
                    hp = teamUser.hp,
                    maxhp = teamUser.maxhp,
                    heroid = teamUser.heroid,
                    bodystyle = teamUser.bodystyle,
                    haircolor = teamUser.haircolor,
                    hairstyle = teamUser.hairstyle,
                    headstyle = teamUser.headstyle,
                    antenna = teamUser.antenna,
                    avatarid = teamUser.avatarid
                };
            }
            else
            {
                if (num > this.teamController.myTeamInfo.mem.Count)
                {
                    FFDebug.LogError(this, "选中角色超出角色列表Count index:" + num);
                    return;
                }
                memember = this.teamController.myTeamInfo.mem[num];
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (memember != null)
            {
                CommonTools.SetSecondPanelPos(eventData);
                UIManager.Instance.ShowUI<UI_TeamSecondary>("UI_TeamSecondary", delegate ()
                {
                    UIManager.GetUIObject<UI_TeamSecondary>().Initilize(memember, eventData, false);
                }, UIManager.ParentType.CommonUI, false);
            }
        }
        else if (eventData.pointerPress.name == "PanelPersonal")
        {
            this.BtnSelectSelfTargetEvent();
        }
        else
        {
            this.TeamMemberTargetClick(memember);
        }
    }

    private void TeamMemberTargetClick(Memember memember)
    {
        if (memember != null)
        {
            CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(ulong.Parse(memember.mememberid), CharactorType.Player);
            if (charactorByID != null)
            {
                MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().uiSelect.SetTarget(charactorByID, true, true);
            }
            else
            {
                MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().uiSelect.SetTargetNotInRange(memember);
            }
        }
    }

    private void btn_target_secondmenu_on_click(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && this.mCurSelTargetPlayerID != 0UL)
        {
            OtherPlayer targetPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(this.mCurSelTargetPlayerID, CharactorType.Player) as OtherPlayer;
            Memember memember = this.teamController.GetTeamMememberInfo(this.mCurSelTargetPlayerID);
            if (targetPlayer == null && memember == null)
            {
                FFDebug.LogError(this, string.Format("targetPlayer == null && UI_TeamSecondary cant find mCurSelTargetPlayerID= {0} !!!", this.mCurSelTargetPlayerID));
                return;
            }
            CommonTools.SetSecondPanelPos(eventData);
            UIManager.Instance.ShowUI<UI_TeamSecondary>("UI_TeamSecondary", delegate ()
            {
                if (memember == null)
                {
                    memember = new Memember();
                    memember.mememberid = this.mCurSelTargetPlayerID.ToString();
                    memember.name = targetPlayer.OtherPlayerData.MapUserData.name;
                }
                UIManager.GetUIObject<UI_TeamSecondary>().Initilize(memember, eventData, false);
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void SetChatMaskActive(bool bActive)
    {
        this.Offset_Main.SetActive(bActive);
    }

    public void SetMainHeadIcon2()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        uint heroid = MainPlayer.Self.OtherPlayerData.MapUserData.bakmapshow.heroid;
        if (heroid <= 0U)
        {
            return;
        }
        this.headLevel2.text = CommonTools.GetLevelFormat(this.uimainController.GetCurLv());
        if (this.gs.isAbattoirScene)
        {
            if (this.headIcon2 != null)
            {
                this.headIcon2.gameObject.SetActive(false);
            }
            if (this.btnChangeHero != null)
            {
                this.btnChangeHero.gameObject.SetActive(false);
            }
            return;
        }
        cs_MapUserData mapUserData = MainPlayer.Self.OtherPlayerData.MapUserData;
        if (mapUserData.bakmapshow != null)
        {
            uint[] featureIDs = new uint[]
            {
                mapUserData.bakmapshow.haircolor,
                mapUserData.bakmapshow.hairstyle,
                mapUserData.bakmapshow.facestyle,
                mapUserData.bakmapshow.antenna,
                mapUserData.bakmapshow.avatarid
            };
            GlobalRegister.ShowPlayerRTT(this.headIcon2, heroid, mapUserData.bakmapshow.bodystyle, featureIDs, 0, null);
            if (null == this.headIcon2)
            {
                return;
            }
            this.headIcon2.gameObject.SetActive(true);
            this.btnChangeHero.gameObject.SetActive(true);
        }
        else
        {
            if (null == this.headIcon2)
            {
                return;
            }
            this.headIcon2.gameObject.SetActive(false);
            this.btnChangeHero.gameObject.SetActive(false);
        }
    }

    private void btn_changehero_on_click(PointerEventData eventData)
    {
        ControllerManager.Instance.GetController<HeroHandbookController>().SwitchHero();
    }

    public void SwitchHeroCb()
    {
        this.setPersonal();
    }

    public void setMainHeadIcon()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        if (null == this.headIcon)
        {
            FFDebug.LogWarning(this, "headIcon is null ?");
            return;
        }
        uint heroid = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.heroid;
        this.headIcon.gameObject.SetActive(heroid != 0U);
        if (heroid <= 0U)
        {
            return;
        }
        string levelFormat = CommonTools.GetLevelFormat(this.uimainController.GetCurLv());
        this.headLevel.text = levelFormat;
        cs_MapUserData mapUserData = MainPlayer.Self.OtherPlayerData.MapUserData;
        uint[] featureIDs = new uint[]
        {
            mapUserData.mapshow.haircolor,
            mapUserData.mapshow.hairstyle,
            mapUserData.mapshow.facestyle,
            mapUserData.mapshow.antenna,
            mapUserData.mapshow.avatarid
        };
        GlobalRegister.ShowPlayerRTT(this.headIcon, heroid, mapUserData.mapshow.bodystyle, featureIDs, 0, null);
    }

    public void InitJoyStick()
    {
        JoyStick mJoyStick = GameObject.Find("PanelJoyStick").GetComponent<JoyStick>();
        mJoyStick.gameObject.SetActive(false);
        mJoyStick.ProcessInput = delegate (Vector2 dir)
        {
            if (dir == Vector2.zero)
            {
                SingletonForMono<InputController>.Instance.InputDir = -1;
                return;
            }
            float num = Vector3.Angle(Vector2.up, dir);
            if (dir.x < 0f)
            {
                num = 360f - num;
            }
            int num2 = (int)(num + CameraController.Self.Angle);
            if (num2 > 360)
            {
                num2 -= 360;
            }
            SingletonForMono<InputController>.Instance.InputDir = num2;
        };
        mJoyStick.OnJoyVirtulDragReturn = ((Vector2 dir) => CommonTools.CheckFloatEqual(dir.x, 0f) && CommonTools.CheckFloatEqual(dir.y, 0f));
        mJoyStick.OnJoyStart = delegate ()
        {
            SingletonForMono<InputController>.Instance.OnVirtualDrag = new Action<Vector2>(mJoyStick.OnVirtualDrag);
        };
        mJoyStick.OnJoySetDragState = delegate (bool state)
        {
            if (state)
            {
                SingletonForMono<InputController>.Instance.CurrentInputType = InputType.InPutByVirtualJoystick;
            }
            else
            {
                SingletonForMono<InputController>.Instance.CurrentInputType = InputType.InputNone;
            }
        };
    }

    public void InitSkillTexture(bool isShow = true)
    {
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        FFDebug.Log(this, FFLogType.Skill, "InitSkillTexture at " + Time.time);
        List<uint> list = new List<uint>();
        List<careerunlockItem> dicEquipSkill = ControllerManager.Instance.GetController<SkillViewControll>().dicEquipSkill;
        for (int i = 0; i < dicEquipSkill.Count; i++)
        {
            uint num = dicEquipSkill[i].skill.skillid;
            if (controller.SkillSlotMap == null || controller.SkillSlotMap.ContainsKey(num))
            {
                num = num * 1000U + 11U;
                LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)num);
                if (configTable != null && configTable.GetField_Uint("canbe_passive") == 0U)
                {
                    list.Add(controller.SkillSlotMap[dicEquipSkill[i].skill.skillid]);
                }
            }
        }
        this.VisitNpcView.SetBattleVisitButtonLastSlibling();
    }

    public void UpdateSkillStorage()
    {
        for (int i = 0; i < this.SkillButtonList.Count; i++)
        {
            SkillButton skillButton = this.SkillButtonList[i];
            skillButton.UpdateStorageNum();
        }
    }

    public void ResetLastIconList()
    {
        this.iconList.Clear();
    }

    public void refreshTeamInfo()
    {
        this.RefreshPersonalLeader();
        if (this.teamController.myTeamInfo != null && this.teamController.myTeamInfo.id != 0U)
        {
            this.setTaskState();
            int num = this.teamController.myTeamInfo.mem.Count - 1;
            while ((long)num < (long)((ulong)UI_MainView.maxMemberNum))
            {
                if (num >= 0)
                {
                    this.teamObj[num].SetActive(false);
                }
                num++;
            }
            int num2 = -1;
            this.panelTask.Find("TeamMember/txt_info").gameObject.SetActive(false);
            for (int i = 0; i < this.teamController.myTeamInfo.mem.Count; i++)
            {
                Memember memember = this.teamController.myTeamInfo.mem[i];
                if (memember.mememberid == MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString())
                {
                    num2 = i;
                }
                else
                {
                    int num3 = i;
                    if (num2 >= 0)
                    {
                        num3 = ((i <= num2) ? i : (i - 1));
                    }
                    Transform transform = this.teamObj[num3].transform;
                    transform.gameObject.name = "Item" + i;
                    Transform transform2 = transform.Find("panel_value");
                    if (transform2 != null)
                    {
                        FightValueNum fightValueNum = transform2.GetComponent<FightValueNum>();
                        if (fightValueNum == null)
                        {
                            fightValueNum = transform2.gameObject.AddComponent<FightValueNum>();
                        }
                        fightValueNum.SetNum(memember.fight);
                    }
                    transform.Find("img_leader").gameObject.SetActive(this.teamController.IsLeader(memember.mememberid));
                    transform.Find("img_inv").gameObject.SetActive(this.teamController.IsHasInviteAbility(memember.privilege));
                    string mememberid = memember.mememberid;
                    ControllerManager.Instance.GetController<FriendControllerNew>().AddRecentItem(mememberid);
                    if (!(mememberid == MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString()))
                    {
                        OtherPlayer playerByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetPlayerByID(ulong.Parse(mememberid));
                        if (playerByID != null)
                        {
                            OtherPlayerData otherPlayerData = playerByID.OtherPlayerData;
                            this.UpdateTeamMemeberBuffIcon(otherPlayerData.MapUserData);
                        }
                        else
                        {
                            Transform transform3 = transform.Find("buff");
                            Transform transform4 = transform.Find("debuff");
                            for (int j = 0; j < transform3.childCount; j++)
                            {
                                transform3.GetChild(j).gameObject.SetActive(false);
                            }
                            for (int k = 0; k < transform4.childCount; k++)
                            {
                                transform4.GetChild(k).gameObject.SetActive(false);
                            }
                        }
                        bool needRefreshHeadIcon = false;
                        string b = (num3 < this.iconList.Count) ? this.iconList[num3] : string.Empty;
                        string heroHeadIconString = this.GetHeroHeadIconString(memember);
                        if (heroHeadIconString != b)
                        {
                            needRefreshHeadIcon = true;
                        }
                        this.setTeamObj(this.teamObj[num3], this.headObj[num3], memember, needRefreshHeadIcon);
                        this.UpdateTargetInfo(i);
                    }
                }
            }
            this.iconList.Clear();
            for (int l = 0; l < this.teamController.myTeamInfo.mem.Count; l++)
            {
                Memember memember2 = this.teamController.myTeamInfo.mem[l];
                if (!(memember2.mememberid == MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString()))
                {
                    this.iconList.Add(this.GetHeroHeadIconString(memember2));
                }
            }
        }
        else
        {
            int num4 = 0;
            while ((long)num4 < (long)((ulong)UI_MainView.maxMemberNum))
            {
                this.teamObj[num4].SetActive(false);
                num4++;
            }
            this.panelTask.Find("TeamMember/txt_info").gameObject.SetActive(true);
            this.iconList.Clear();
        }
    }

    private string GetHeroHeadIconString(Memember memember)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(memember.heroid);
        stringBuilder.Append(memember.bodystyle);
        stringBuilder.Append(memember.haircolor);
        stringBuilder.Append(memember.hairstyle);
        stringBuilder.Append(memember.headstyle);
        stringBuilder.Append(memember.antenna);
        return stringBuilder.ToString();
    }

    private void UpdateTargetInfo(int index)
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            Memember memember = this.teamController.myTeamInfo.mem[index];
            controller.UpdateTargetHP(ulong.Parse(memember.mememberid), memember.hp, memember.maxhp);
            controller.UpdateBaseInfo(ulong.Parse(memember.mememberid), memember.name, memember.level);
        }
    }

    private void setTeamObj(GameObject obj, GameObject heroObj, Memember mem, bool needRefreshHeadIcon)
    {
        obj.SetActive(true);
        obj.transform.Find("txt_playername").GetComponent<Text>().text = mem.name;
        Slider component = obj.transform.Find("hp").GetComponent<Slider>();
        if (mem.state == MemState.OFFLINE)
        {
            component.value = 1f;
            obj.transform.Find("hp/txt_percent").GetComponent<Text>().text = "100%";
        }
        else
        {
            component.value = mem.hp / mem.maxhp;
            obj.transform.Find("hp/txt_percent").GetComponent<Text>().text = Mathf.Ceil(mem.hp / mem.maxhp * 100f) + "%";
        }
        obj.transform.Find("txt_hp/value").GetComponent<Text>().text = mem.hp + "/" + mem.maxhp;
        obj.transform.Find("txt_lv").GetComponent<Text>().text = CommonTools.GetLevelFormat(mem.level);
        ulong num = ulong.Parse(mem.mememberid);
        RawImage component2 = heroObj.GetComponent<RawImage>();
        component2.gameObject.SetActive(true);
        uint[] featureIDs = new uint[]
        {
            mem.haircolor,
            mem.hairstyle,
            mem.headstyle,
            mem.antenna,
            mem.avatarid
        };
        if (needRefreshHeadIcon)
        {
            GlobalRegister.ShowPlayerRTT(component2, mem.heroid, mem.bodystyle, featureIDs, 0, null);
        }
        switch (mem.state)
        {
            case MemState.NORMAL:
                ManagerCenter.Instance.GetManager<UITextureMgr>().SetImageGrey4Head(component2, false);
                obj.transform.Find("img_icon/img_mask").gameObject.SetActive(false);
                obj.transform.Find("img_icon/img_auto").gameObject.SetActive(false);
                break;
            case MemState.AWAY:
                ManagerCenter.Instance.GetManager<UITextureMgr>().SetImageGrey4Head(component2, false);
                obj.transform.Find("img_icon/img_mask").gameObject.SetActive(true);
                obj.transform.Find("img_icon/img_auto").gameObject.SetActive(false);
                break;
            case MemState.OFFLINE:
                ManagerCenter.Instance.GetManager<UITextureMgr>().SetImageGrey4Head(component2, true);
                obj.transform.Find("img_icon/img_mask").gameObject.SetActive(false);
                obj.transform.Find("img_icon/img_auto").gameObject.SetActive(false);
                break;
            case MemState.HOSTING:
                ManagerCenter.Instance.GetManager<UITextureMgr>().SetImageGrey4Head(component2, false);
                obj.transform.Find("img_icon/img_mask").gameObject.SetActive(false);
                obj.transform.Find("img_icon/img_auto").gameObject.SetActive(true);
                break;
        }
    }

    private void setTaskState()
    {
        if (this.showTask)
        {
            this.btnTaskOn.SetActive(true);
            this.btnTaskOff.SetActive(false);
            this.btnTeamOff.SetActive(true);
            this.btnTeamOn.SetActive(false);
            this.TaskList.gameObject.SetActive(true);
            this.panelTask.Find("TeamMember").gameObject.SetActive(false);
            this.panelTask.gameObject.SetActive(true);
            return;
        }
        this.panelTask.gameObject.SetActive(false);
    }

    public void ShowRegressionButton(bool bShow)
    {
        this.goRegression.SetActive(bShow);
    }

    private void OnRegression(PointerEventData data)
    {
        LuaScriptMgr.Instance.CallLuaFunction("EnterCopyCtrl.Regression", new object[]
        {
            Util.GetLuaTable("EnterCopyCtrl")
        });
    }

    public void SetChatHidden(bool bHidden)
    {
        if (bHidden)
        {
            this.panelChat.transform.localPosition = this.chatMovePostion;
        }
        else
        {
            this.panelChat.localPosition = this.chatLocalPostion;
        }
    }

    public void CopyGuideSpecialHandling(bool bInGuide)
    {
        this.panelBottom.gameObject.SetActive(!bInGuide);
        if (bInGuide)
        {
            UIEventListener.Get(this.panelPersonal.gameObject).onClick = null;
            UIEventListener.Get(this.btnTaskOn).onClick = null;
            UIEventListener.Get(this.panelTask.Find("TeamMember").gameObject).onClick = null;
            if (this.chatLocalPostion.Equals(Vector3.zero))
            {
                this.chatLocalPostion = this.panelChat.localPosition;
            }
        }
        else
        {
            UIEventListener.Get(this.btnTaskOn).onClick = new UIEventListener.VoidDelegate(this.enterTask);
            UIEventListener.Get(this.panelTask.Find("TeamMember").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterTeam);
            if (this.chatLocalPostion.Equals(Vector3.zero))
            {
                this.chatLocalPostion = this.panelChat.localPosition;
            }
        }
        this.SetChatHidden(bInGuide);
    }

    private bool IsOnPeaceState()
    {
        if (MainPlayer.Self != null)
        {
            PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
            if (component != null)
            {
                return component.ContainsState(UserState.USTATE_PEACE);
            }
        }
        return false;
    }

    public void SwitchBottomUI(bool bottomshow)
    {
        this.bottomShow = bottomshow;
        this.refreshSkillpanelActive();
        if (!bottomshow)
        {
            this.CloseAllSkillEffect();
        }
    }

    public void refreshSkillpanelActive()
    {
        this.panelSkill.gameObject.SetActive(true);
    }

    public void enterBag(PointerEventData data)
    {
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel("UI_Bag");
        if (luaUIPanel != null && luaUIPanel.uiRoot.transform.localPosition != new Vector3(10000f, 10000f, 0f))
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseBagUI", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        }
        else if (!this.gs.isAbattoirScene)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterPackage", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        }
        else
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterCapsulePackage", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        }
    }

    private void enterTeam(PointerEventData data)
    {
        if (ManagerCenter.Instance.GetManager<CopyManager>().MCurrentCopyID != 0U)
        {
            TipsWindow.ShowWindow(TipsType.IN_COPY_PLEASE_WAIT, null);
            return;
        }
        this.teamController.EnterTeam();
    }

    public void enterTask(PointerEventData data)
    {
        ControllerManager.Instance.GetController<TaskUIController>().OpenTaskView();
    }

    private void enterSkillView(PointerEventData data)
    {
        ControllerManager.Instance.GetController<SkillViewControll>().EnterSkillView();
    }

    public void enterFriendView(PointerEventData data)
    {
        ControllerManager.Instance.GetController<FriendControllerNew>().ShowFriendUI(null);
    }

    public void BtnTabEvent()
    {
        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TabSelect.ReqTarget();
    }

    public void BtnSelectSelfTargetEvent()
    {
        CharactorBase self = MainPlayer.Self;
        if (self != null)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().uiSelect.SetTarget(self, false, true);
        }
    }

    private void enterPetView(PointerEventData data)
    {
        this.petController.EnterPetInfo();
    }

    private void changePetState(PointerEventData data)
    {
        if (this.petAssistData != null)
        {
            this.changeState(this.petAssistData);
        }
    }

    private void changeState(PetBase data)
    {
        this.petController.ReqSwitchPetState(data.tempid, PetState.PetState_Assist, PetState.PetState_Fight);
    }

    public void switchToTask(PointerEventData data)
    {
        this.showTask = true;
        this.setTaskState();
        this.showTaskList();
    }

    public void setTaskAndTeamSwtich()
    {
        if (this.showTask)
        {
            this.showTaskList();
        }
    }

    private void showTaskList()
    {
        LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowTaskList", new object[]
        {
            Util.GetLuaTable("MainUICtrl")
        });
    }

    private void switchToTeam(PointerEventData data)
    {
        this.showTask = false;
        this.setTaskState();
        this.refreshTeamInfo();
    }

    private void resetCamera(PointerEventData data)
    {
        CameraController.Self.Reste3DCamera();
    }

    private void resetCamera()
    {
        CameraController.Self.Reste3DCamera();
    }

    private void set2dCamera(PointerEventData data)
    {
        this.setCameraIn3DBtn(false);
        CameraController.Self.ChangeCameraState(false);
    }

    private void set3dCamera(PointerEventData data)
    {
        this.setCameraIn3DBtn(true);
        CameraController.Self.ChangeCameraState(true);
    }

    private void setCameraIn3DBtn(bool CameraIn3d)
    {
        if (CameraIn3d)
        {
            this.btn3dCamera.SetActive(false);
            this.btn2dCamera.SetActive(true);
        }
        else
        {
            this.btn3dCamera.SetActive(true);
            this.btn2dCamera.SetActive(false);
        }
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.panelMenu.Find("menu1/left/btn_bag").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterBag);
        UIEventListener.Get(this.panelMenu.Find("team/btn_team").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterTeam);
        UIEventListener.Get(this.panelMenu.Find("skill/btn_bag").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterSkillView);
        UIEventListener.Get(this.panelMenu.Find("pet/btn_bag").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterPetView);
        UIEventListener.Get(this.panelPet.gameObject).onClick = new UIEventListener.VoidDelegate(this.changePetState);
        UIEventListener.Get(this.btnTaskOff).onClick = new UIEventListener.VoidDelegate(this.switchToTask);
        UIEventListener.Get(this.btnTeamOn).onClick = new UIEventListener.VoidDelegate(this.enterTeam);
        UIEventListener.Get(this.btnTeamOff).onClick = new UIEventListener.VoidDelegate(this.switchToTeam);
        UIEventListener.Get(this.btnCameraReset).onClick = new UIEventListener.VoidDelegate(this.resetCamera);
        LongPressOrClickEventTrigger.Get(this.btn2dCamera).onClick = new LongPressOrClickEventTrigger.VoidDelegate(this.set2dCamera);
        LongPressOrClickEventTrigger.Get(this.btn2dCamera).onLongPress = new LongPressOrClickEventTrigger.VoidDelegateVoid(this.resetCamera);
        LongPressOrClickEventTrigger.Get(this.btn3dCamera).onClick = new LongPressOrClickEventTrigger.VoidDelegate(this.set3dCamera);
        LongPressOrClickEventTrigger.Get(this.btn3dCamera).onLongPress = new LongPressOrClickEventTrigger.VoidDelegateVoid(this.resetCamera);
        UIEventListener.Get(this.panelTask.Find("TeamMember").gameObject).onClick = new UIEventListener.VoidDelegate(this.enterTeam);
        UIEventListener.Get(this.goRegression).onClick = new UIEventListener.VoidDelegate(this.OnRegression);
        UIEventListener.Get(this.panelPersonal.gameObject).onClick = new UIEventListener.VoidDelegate(this.TeamMemberClick);
        UIEventListener.Get(this.panelTask.Find("btn_close").gameObject).onClick = delegate (PointerEventData data)
        {
            this.OpentaskAndTeamView = !this.OpentaskAndTeamView;
            this.viewTask();
        };
        SingletonForMono<InputController>.Instance.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.CheckUnlockFinish));
        this.btn_tab.onClick.RemoveAllListeners();
        this.btn_tab.onClick.AddListener(new UnityAction(this.BtnTabEvent));
        UIEventListener.Get(this.goSystemSetting).onClick = new UIEventListener.VoidDelegate(this.OnSystemSetting);
        UIEventListener.Get(this.btn_activityguide.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnOpenActivityGuide);
        UIEventListener.Get(this.btn_sevendays.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnOpenSevenDays);
        UIEventListener.Get(this.btn_questionnaire.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnOpenQuestionnaire);
        UIEventListener.Get(this.btn_TeamCallBack).onClick = new UIEventListener.VoidDelegate(this.CallTeamMemberBack);
        this.teamController.EnterNoTeam();
        Button component = this.btn_TaskListHide.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.TaskListSwitch));
        Button component2 = this.btnVip.GetComponent<Button>();
        component2.navigation = new Navigation
        {
            mode = Navigation.Mode.None
        };
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.VipRight));
    }

    private void TaskListSwitch()
    {
        TweenPosition component = this.TaskList.GetComponent<TweenPosition>();
        if (component != null)
        {
            component.Reset();
            component.Play(this.isShowTargetInfo);
        }
        TweenRotation component2 = this.btn_TaskListHide.GetComponent<TweenRotation>();
        if (component2 != null)
        {
            component2.Reset();
            component2.Play(this.isShowTargetInfo);
        }
        this.isShowTargetInfo = !this.isShowTargetInfo;
    }

    private void VipRight()
    {
        UI_VipPrivilege uiobject = UIManager.GetUIObject<UI_VipPrivilege>();
        if (uiobject == null)
        {
            UIManager.Instance.ShowUI<UI_VipPrivilege>("UI_VipPrivilege", null, UIManager.ParentType.CommonUI, false);
        }
    }

    private void CallTeamMemberBack(PointerEventData data)
    {
        this.teamController.ReqMemberBackTeam_CS();
    }

    private void OnSystemSetting(PointerEventData data)
    {
        if (UIManager.GetUIObject<UI_GameSetting>() != null)
        {
            ControllerManager.Instance.GetController<SystemSettingController>().CloseUI();
        }
        else
        {
            ControllerManager.Instance.GetController<SystemSettingController>().ShowUI();
        }
    }

    private void OnOpenActivityGuide(PointerEventData data)
    {
        this.uimainController.OpenAcitvityGuide();
    }

    private void OnOpenSevenDays(PointerEventData data)
    {
        ControllerManager.Instance.GetController<SevenDaysController>().ShowSevenDaysUI();
    }

    private void OnOpenQuestionnaire(PointerEventData data)
    {
        ControllerManager.Instance.GetController<QuestationnaireController>().ShowUI();
    }

    public void SetupQuestionnaireBtn(bool show)
    {
        this.btn_questionnaire.gameObject.SetActive(show);
    }

    public void ShowMapName(mapinfo _mapInfo)
    {
        this.mapName.text = _mapInfo.showName();
        this.mapNameEn.text = _mapInfo.name_en();
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", _mapInfo.icon(), delegate (Sprite sprite)
        {
            this.mapTypIcon.overrideSprite = sprite;
        });
        for (int i = 0; i < this.mapNameTweenAlpha.Count; i++)
        {
            this.mapNameTweenAlpha[i].Reset();
            this.mapNameTweenAlpha[i].GetComponent<CanvasRenderer>().SetAlpha(0f);
            this.mapNameTweenAlpha[i].Play(true);
        }
        TweenPosition[] componentsInChildren = this.panel_MapName.GetComponentsInChildren<TweenPosition>();
        for (int j = 0; j < componentsInChildren.Length; j++)
        {
            componentsInChildren[j].Reset();
            componentsInChildren[j].Play(true);
        }
        TweenScale[] componentsInChildren2 = this.panel_MapName.GetComponentsInChildren<TweenScale>();
        for (int k = 0; k < componentsInChildren2.Length; k++)
        {
            componentsInChildren2[k].Reset();
            componentsInChildren2[k].Play(true);
        }
        this.panel_MapName.gameObject.SetActive(true);
    }

    public void ChangeMainPanelMenu(uint menuId, bool menuState)
    {
        TweenScale[] array = null;
        if (menuId == 1U)
        {
            array = this.panelMenu.Find("menu1").GetComponentsInChildren<TweenScale>();
        }
        else if (menuId == 2U)
        {
            array = this.panelMenu.Find("menu2").GetComponentsInChildren<TweenScale>();
        }
        if (array != null)
        {
            if (menuState)
            {
                foreach (TweenScale tweenScale in array)
                {
                    if (tweenScale.strName == "tw_open")
                    {
                        tweenScale.Reset();
                        tweenScale.Play(true);
                    }
                }
            }
            else
            {
                foreach (TweenScale tweenScale2 in array)
                {
                    if (tweenScale2.strName == "tw_close")
                    {
                        tweenScale2.Reset();
                        tweenScale2.Play(true);
                    }
                }
            }
        }
    }

    public void CheckViewTask()
    {
        if (this.OpentaskAndTeamView)
        {
            this.panelTask.Find("btn_close/img_icon").localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else
        {
            this.panelTask.Find("btn_close/img_icon").localRotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        if (this.taskuiController.GetListTaskCount() > 0 || this.teamController.myTeamInfo.id != 0U)
        {
            if (!this.OpentaskAndTeamView)
            {
                this.OpentaskAndTeamView = true;
                this.viewTask();
            }
        }
        else if (this.OpentaskAndTeamView)
        {
            this.OpentaskAndTeamView = false;
            this.viewTask();
        }
    }

    private void viewTask()
    {
        if (this.OpentaskAndTeamView)
        {
            this.panelTask.Find("btn_close/img_icon").localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else
        {
            this.panelTask.Find("btn_close/img_icon").localRotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    public void ShorcutVisitNpc(ulong npcEntityID)
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component == null)
        {
            return;
        }
        component.RecommendVisiteNpcID = npcEntityID;
        this.ShorcutVisitRecommendNpc();
    }

    public void ShorcutVisitRecommendNpc()
    {
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component == null)
        {
            return;
        }
        if (component.RecommendVisiteNpcID == 0UL)
        {
            return;
        }
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(component.RecommendVisiteNpcID, CharactorType.NPC) as Npc;
        if (npc == null)
        {
            FFDebug.LogWarning(this, "ShorcutVisitNpc cant find this npc detectionNpcControl.RecommendVisiteNpcID = " + component.RecommendVisiteNpcID);
            return;
        }
        npc.CloseTopBtn(false);
        if (MainPlayer.Self.Pfc.isPathfinding)
        {
            MainPlayer.Self.GetComponent<PathFindFollowTarget>().CompDispose();
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "ShorcutVisitNpc cant find this npc data id = " + npc.NpcData.MapNpcData.baseid);
            return;
        }
        if (configTable.GetField_Uint("kind") == 6U)
        {
            ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc.NpcData.MapNpcData.tempid, configTable.GetField_Uint("kind"), null);
        }
        else if (configTable.GetField_Uint("kind") == 11U)
        {
            if (npc is Npc_TaskCollect && (npc as Npc_TaskCollect).CheckStateContainDoing())
            {
                ControllerManager.Instance.GetController<CollectController>().ReqHoldOnCollectNPC(npc.NpcData.MapNpcData.tempid, configTable.GetField_Uint("kind"), null);
            }
        }
        else if (configTable.GetField_Uint("kind") == 1U)
        {
            if (configTable.GetCacheField_Uint("lookat") == 0U)
            {
                MainPlayer.Self.SetPlayerLookAt(npc.ModelObj.transform.position, true);
                npc.SetPlayerLookAt(MainPlayer.Self.ModelObj.transform.position, true);
            }
            ControllerManager.Instance.GetController<TaskController>().ReqVisteNpc(npc.NpcData.MapNpcData.tempid);
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().manualSelect.SetSelectTarget(npc, false);
        }
        else
        {
            ControllerManager.Instance.GetController<TaskController>().ReqVisteNpc(npc.NpcData.MapNpcData.tempid);
        }
    }

    public void ShorcutVisitNpcByFindPathNew(uint npcid)
    {
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().SearchNearNPCById(npcid);
        if (npc != null)
        {
            this.ShorcutVisitNpc(npc.EID.Id);
        }
    }

    public void ShowHoldOnCountDownTip(TransformData data)
    {
        this.panelHoldonCountDown.SetActive(true);
        this.lbHoldonCountDownNpcName.text = data.Data.npc_name;
        this.lbHoldonCountDownremaintime.text = CommonTools.GetTimeString(data.CurrentTime);
    }

    public void CloseHoldOnCountDownTip()
    {
        this.panelHoldonCountDown.SetActive(false);
    }

    public void ShowTimeBuffState(MSG_Ret_setTimeState_SC data)
    {
    }

    public void RemoveTimeBuffState(UserState state)
    {
        if (this.TimeBuff.ContainsKey(state))
        {
            this.TimeBuff[state].StopCoutDown();
            this.TimeBuff.Remove(state);
            this.panelBuffInfo.SetActive(false);
        }
    }

    public void CloseAllSkillEffect()
    {
        for (int i = 0; i < this.SkillButtonList.Count; i++)
        {
            SkillButton skillButton = this.SkillButtonList[i];
            skillButton.ShowStandbyEffect(false);
        }
    }

    private void Update()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        if (this.mskillShowImage != null)
        {
            this.mskillShowImage.Updata();
        }
        this.QTEUpDate();
        this.TargetHPUpdator();
        if (this.mainPlayerSkillHolder == null)
        {
            this.mainPlayerSkillHolder = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
            return;
        }
        BetterDictionary<uint, MainPlayerSkillBase> mainPlayerSkillList = this.mainPlayerSkillHolder.MainPlayerSkillList;
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        for (uint num = 1U; num <= 12U; num += 1U)
        {
            int index = (int)(num - 1U);
            SkillButton skillButton = this.SkillButtonList[index];
            if (mainPlayerSkillList.ContainsKey((uint)skillButton.skillId))
            {
                MainPlayerSkillBase mainPlayerSkillBase = mainPlayerSkillList[(uint)skillButton.skillId];
                if (skillButton.SkillData != null)
                {
                    if (!skillButton.IsStorageSkill)
                    {
                        float num2 = mainPlayerSkillBase.GetCDLeft(currServerTime);
                        PublicCDData publicCDDataByGroup = MainPlayer.Self.GetComponent<SkillPublicCDControl>().GetPublicCDDataByGroup(mainPlayerSkillBase.SkillConfig.GetCacheField_Uint("publicCD"));
                        if (num2 > 0f)
                        {
                            skillButton.UpdateButtonCD(num2, mainPlayerSkillBase.CDLength);
                        }
                        else if (mainPlayerSkillBase.SkillConfig.GetCacheField_Uint("publicCD") != 0U && publicCDDataByGroup != null && publicCDDataByGroup.ActivateCDSkill != skillButton.SkillData.SkillConfig.GetCacheField_Uint("skillid"))
                        {
                            skillButton.UpdateButtonCD(publicCDDataByGroup.GetCDLeft(currServerTime), publicCDDataByGroup.CDLength);
                        }
                        else
                        {
                            skillButton.UpdateButtonCD(0f, mainPlayerSkillBase.CDLength);
                        }
                    }
                    else if (skillButton.IsStarageMax())
                    {
                        if (mainPlayerSkillBase.SkillConfig.GetField_Uint("publicCD") != 0U)
                        {
                            PublicCDData publicCDDataByGroup2 = MainPlayer.Self.GetComponent<SkillPublicCDControl>().GetPublicCDDataByGroup(mainPlayerSkillBase.SkillConfig.GetField_Uint("publicCD"));
                            uint cdleft = publicCDDataByGroup2.GetCDLeft(currServerTime);
                            if (publicCDDataByGroup2 != null && publicCDDataByGroup2.ActivateCDSkill != skillButton.SkillData.SkillConfig.GetField_Uint("skillid"))
                            {
                                skillButton.UpdateButtonCD(publicCDDataByGroup2.GetCDLeft(currServerTime), publicCDDataByGroup2.CDLength);
                            }
                        }
                    }
                    else
                    {
                        float cdtime = mainPlayerSkillBase.GetStorageCDLeft(currServerTime);
                        if (skillButton.mSkillStarageType() == SkillSorageType.CDWhenRelase)
                        {
                            skillButton.UpdateStorageCD(cdtime, mainPlayerSkillBase.CDLength);
                        }
                    }
                    skillButton.UpdateButtonState(mainPlayerSkillBase);
                }
            }
        }
        this.TimeBuff.BetterForeach(delegate (KeyValuePair<UserState, TimeBuffStateData> item)
        {
            item.Value.UpdateThis();
        });
    }

    public void UpdateSkillButtonColor()
    {
        for (int i = 1; i < this.SkillButtonList.Count; i++)
        {
            SkillButton skillButton = this.SkillButtonList[i];
            skillButton.TryUpdateButtonColor((uint)this.CurrMp);
        }
    }

    public void UnlockSkill(uint skillID)
    {
        if (this._isUnlocking)
        {
            this.UnlockFinish();
        }
        uint num = skillID * 1000U + 11U;
        LuaTable configTable = LuaConfigManager.GetConfigTable("skill_data", (ulong)num);
        string text = string.Empty;
        if (configTable != null)
        {
            text = configTable.GetField_String("skillicon.").Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
        this.SwitchBottomUI(false);
        this.btn_unlockSkill.transform.localPosition = this._unLockStartPos;
        Image img = this.btn_unlockSkill.GetComponent<Image>();
        if (img.sprite.name != text)
        {
            base.GetTexture(ImageType.ITEM, text, delegate (Texture2D item)
            {
                if (item != null)
                {
                    Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                    img.sprite = sprite;
                    this.btn_unlockSkill.gameObject.SetActive(true);
                }
            });
        }
        SkillButton skillButton = this.FindTargetSkillBtnByID(skillID);
        if (skillButton == null)
        {
            this.PlayTweenPos(null, this.btn_unlockSkill.transform.localPosition + new Vector3(0f, 70f, 0f));
        }
        else
        {
            this.PlayTweenPos(skillButton, skillButton.transform.localPosition);
        }
    }

    private void CheckUnlockFinish(ScreenEvent SE)
    {
        if (this._isUnlocking && SE.mTpye == ScreenEvent.EventType.Click)
        {
            this.UnlockFinish();
        }
    }

    private void UnlockFinish()
    {
        this.btn_unlockSkill.transform.localPosition = this._unLockStartPos;
        this.btn_unlockSkill.gameObject.SetActive(false);
        TweenPosition component = this.btn_unlockSkill.GetComponent<TweenPosition>();
        if (component != null)
        {
            component.Reset();
        }
        if (this._temp != null)
        {
            this._temp.UnlockFinish();
        }
        this._isUnlocking = false;
    }

    private void PlayTweenPos(SkillButton btn, Vector3 targetPos)
    {
        this._isUnlocking = true;
        this._temp = btn;
        this._temp.UnlockStart();
        TweenPosition tweenPosition = this.btn_unlockSkill.GetComponent<TweenPosition>();
        if (tweenPosition == null)
        {
            tweenPosition = this.btn_unlockSkill.gameObject.AddComponent<TweenPosition>();
            tweenPosition.from = this._unLockStartPos;
            tweenPosition.delay = this._fTweenTime;
            tweenPosition.duration = this._fTweenTime;
            tweenPosition.style = UITweener.Style.Once;
            tweenPosition.method = UITweener.Method.Linear;
        }
        tweenPosition.to = targetPos;
        tweenPosition.onFinished = null;
        tweenPosition.onFinished = delegate (UITweener tweener)
        {
            this.btn_unlockSkill.transform.localPosition = this._unLockStartPos;
            this.btn_unlockSkill.gameObject.SetActive(false);
            if (btn != null)
            {
                btn.UnlockFinish();
            }
            this._isUnlocking = false;
        };
        tweenPosition.Reset();
        tweenPosition.Play(true);
    }

    public SkillButton FindTargetSkillBtnByID(uint skillID)
    {
        if (this.SkillButtonList == null || this.SkillButtonList.Count < 1)
        {
            return null;
        }
        for (int i = 0; i < this.SkillButtonList.Count; i++)
        {
            SkillButton skillButton = this.SkillButtonList[i];
            if (skillButton.SkillData != null)
            {
                if (skillButton.SkillData.Skillid == skillID)
                {
                    return skillButton;
                }
            }
        }
        return null;
    }

    public void SetAutoBattleUI(bool IsAutoBattle, bool RootShow = true)
    {
    }

    public CharactorBase CurOwner
    {
        get
        {
            return this._curOwner;
        }
        set
        {
            this._curOwner = value;
        }
    }

    private void InitColors()
    {
        this.colors = new Color[5];
        this.colors[0] = Const.GetColorByName("boss1");
        this.colors[1] = Const.GetColorByName("boss2");
        this.colors[2] = Const.GetColorByName("boss3");
        this.colors[3] = Const.GetColorByName("boss4");
        this.colors[4] = Const.GetColorByName("boss5");
    }

    private void InitBuffIconController()
    {
        this.panelSelfBuffIcon = this.Root.transform.Find("Offset_Main/PanelBuffState/PanelBuff").gameObject;
        int cacheField_Int = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("BufferIconNumTarget").GetCacheField_Int("value");
        this.selfBuffIconCrtl = new BuffIconController(this.panelSelfBuffIcon.transform, cacheField_Int);
        if (MainPlayer.Self != null)
        {
            this.selfBuffIconCrtl.OwenerID = MainPlayer.Self.EID.Id;
        }
        this.bossBuffIcon = this.targetBossInfo.gameObject;
        int cacheField_Int2 = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("BufferIconNumBoss").GetCacheField_Int("value");
        this.bossBuffIconCrtl = new BuffIconController(this.targetBossInfo.transform, cacheField_Int2);
        this.bossBuffIconCrtl.OwenerID = 0UL;
        this.normalBuffIcon = this.normalTargetInfo.FindChild("Panel_buff").gameObject;
        int cacheField_Int3 = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("BufferIconNumTarget").GetCacheField_Int("value");
        this.normalBuffIconCrtl = new BuffIconController(this.normalBuffIcon.transform, cacheField_Int3);
        this.normalBuffIconCrtl.OwenerID = 0UL;
    }

    public void ShowTargetNPCInfo(ulong npcEntryID)
    {
        this.mCurSelTargetPlayerID = 0UL;
        this.isShowTargetInfo = true;
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        Npc npc = manager.GetCharactorByID(npcEntryID, CharactorType.NPC) as Npc;
        if (npc == null)
        {
            FFDebug.LogWarning(this, string.Format("ShowTargetNPCInfo cant find target npc eid = {0} !!!", npcEntryID));
            return;
        }
        this._curOwner = npc;
        cs_MapNpcData mapNpcData = npc.NpcData.MapNpcData;
        if (ControllerManager.Instance.GetController<DuoQiController>().CheckIsMyBallAndInBase(mapNpcData.tempid))
        {
            return;
        }
        uint baseid = npc.NpcData.MapNpcData.baseid;
        uint heroid = npc.NpcData.MapNpcData.showdata.heroid;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, string.Format("ShowTargetNPCInfo cant find npc config baseid = {0} !!!", baseid));
        }
        bool flag = npc.NpcData.MapNpcData.showdata.haircolor > 0U || npc.NpcData.MapNpcData.showdata.hairstyle > 0U || npc.NpcData.MapNpcData.showdata.facestyle > 0U || npc.NpcData.MapNpcData.showdata.antenna > 0U;
        uint[] featureIDs = new uint[]
        {
            npc.NpcData.MapNpcData.showdata.haircolor,
            npc.NpcData.MapNpcData.showdata.hairstyle,
            npc.NpcData.MapNpcData.showdata.facestyle,
            npc.NpcData.MapNpcData.showdata.antenna,
            npc.NpcData.MapNpcData.showdata.avatarid
        };
        if (configTable.GetField_Uint("kind") == 5U)
        {
            this.isBoss = true;
            if (!this.targetBossInfo.gameObject.activeSelf)
            {
                this.targetBossInfo.gameObject.SetActive(true);
            }
            if (this.normalTargetInfo.gameObject.activeSelf)
            {
                this.normalTargetInfo.gameObject.SetActive(false);
            }
            this.bossBuffIconCrtl.OwenerID = this._curOwner.EID.Id;
            if (flag)
            {
                GlobalRegister.ShowPlayerRTT(this.icon3DImageNpc, this.uimainController.GetIDfor3DIconPosData(baseid, heroid), npc.NpcData.MapNpcData.showdata.bodystyle, featureIDs, 0, null);
            }
            else
            {
                GlobalRegister.ShowNpcOrPlayerRTT(this.icon3DImageNpc, this.uimainController.GetIDfor3DIconPosData(baseid, heroid), 0, null);
            }
        }
        else
        {
            if (flag)
            {
                GlobalRegister.ShowPlayerRTT(this.icon3DImageNpc, baseid, npc.NpcData.MapNpcData.showdata.bodystyle, featureIDs, 0, null);
            }
            else
            {
                GlobalRegister.ShowNpcOrPlayerRTT(this.icon3DImage, baseid, 0, null);
            }
            this.isBoss = false;
            this.targetBossInfo.gameObject.SetActive(false);
            this.targetOfTargetPanel.gameObject.SetActive(false);
            this.normalTargetInfo.gameObject.SetActive(true);
            this.normalBuffIconCrtl.OwenerID = this._curOwner.EID.Id;
        }
        RelationType type = manager.CheckRelationBaseMainPlayer(npc);
        this.UpdateBaseInfo(npcEntryID, mapNpcData.name, configTable.GetField_Uint("level"));
        this.UpdateOutlineByRelation(type, false);
        this.InitTargetHPInfo(mapNpcData.maxhp, mapNpcData.hp, configTable.GetField_Uint("hplayer"));
        this.Img_NormalJobBG.gameObject.SetActive(false);
    }

    public void ShowTargetPlayerInfo(ulong otherPlayerID)
    {
        this.mCurSelTargetPlayerID = otherPlayerID;
        this.icon3DImage.gameObject.SetActive(true);
        this.isBoss = false;
        this.isShowTargetInfo = true;
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        OtherPlayer otherPlayer = manager.GetCharactorByID(otherPlayerID, CharactorType.Player) as OtherPlayer;
        if (otherPlayer == null)
        {
            FFDebug.LogWarning(this, string.Format("ShowTargetPlayerInfo cant find target npc eid = {0} !!!", otherPlayerID));
            return;
        }
        this._curOwner = otherPlayer;
        this.normalBuffIconCrtl.OwenerID = otherPlayer.EID.Id;
        cs_MapUserData mapUserData = otherPlayer.OtherPlayerData.MapUserData;
        this.UpdateBaseInfo(otherPlayerID, mapUserData.name, mapUserData.mapdata.level);
        this.UpdateTargetCareer(mapUserData);
        this.UpdateSpriteOrColor(otherPlayer);
        this.InitTargetHPInfo(mapUserData.mapdata.maxhp, mapUserData.mapdata.hp, 1U);
        this.normalTargetInfo.gameObject.SetActive(true);
        this.targetBossInfo.gameObject.SetActive(false);
        this.targetOfTargetPanel.gameObject.SetActive(false);
        uint[] featureIDs = new uint[]
        {
            otherPlayer.OtherPlayerData.MapUserData.mapshow.haircolor,
            otherPlayer.OtherPlayerData.MapUserData.mapshow.hairstyle,
            otherPlayer.OtherPlayerData.MapUserData.mapshow.facestyle,
            otherPlayer.OtherPlayerData.MapUserData.mapshow.antenna,
            otherPlayer.OtherPlayerData.MapUserData.mapshow.avatarid
        };
        GlobalRegister.ShowPlayerRTT(this.icon3DImage, otherPlayer.OtherPlayerData.MapUserData.mapshow.heroid, otherPlayer.OtherPlayerData.MapUserData.mapshow.bodystyle, featureIDs, 0, null);
    }

    public void ShowTargetPlayerInfoNotInRange(ulong otherPlayerID, Memember memember)
    {
        this.mCurSelTargetPlayerID = otherPlayerID;
        this.isBoss = false;
        this.isShowTargetInfo = true;
        OtherPlayer otherPlayer = new OtherPlayer();
        otherPlayer.EID = new EntitiesID(otherPlayerID, CharactorType.Player);
        this._curOwner = otherPlayer;
        this.normalBuffIconCrtl.OwenerID = otherPlayer.EID.Id;
        this.UpdateBaseInfo(otherPlayerID, memember.name, memember.level);
        this.UpdateSpriteOrColorNotInRange(otherPlayerID, CharactorType.Player);
        this.InitTargetHPInfo(memember.maxhp, memember.hp, 1U);
        this.normalTargetInfo.gameObject.SetActive(true);
        this.targetBossInfo.gameObject.SetActive(false);
        this.targetOfTargetPanel.gameObject.SetActive(false);
        uint[] featureIDs = new uint[]
        {
            memember.haircolor,
            memember.hairstyle,
            memember.headstyle,
            memember.antenna,
            memember.avatarid
        };
        GlobalRegister.ShowPlayerRTT(this.icon3DImage, memember.heroid, memember.bodystyle, featureIDs, 0, null);
    }

    public void UpdateBaseInfo(ulong id, string name, uint level)
    {
        if (this._curOwner == null)
        {
            return;
        }
        if (this._curOwner.EID.Id != id)
        {
            return;
        }
        if (this.isBoss)
        {
            this.Txt_BossName.text = name;
            this.Txt_BossLevelNum.text = CommonTools.GetLevelFormat(level);
        }
        else
        {
            this.Txt_NormalName.text = name;
            this.Txt_NormalLevelNum.text = CommonTools.GetLevelFormat(level);
        }
    }

    public void UpdateSpriteOrColor(OtherPlayer targetPlayer)
    {
        if (!this.gs.isAbattoirScene)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            RelationType type = manager.CheckRelationBaseMainPlayer(targetPlayer);
            this.UpdateOutlineByRelation(type, true);
            return;
        }
        this.UpdateSpriteOrColorByPlayerUid(targetPlayer.EID.Id);
    }

    public void UpdateSpriteOrColorNotInRange(ulong uid, CharactorType charatype)
    {
        if (!this.gs.isAbattoirScene || charatype == CharactorType.NPC)
        {
            this.UpdateOutlineByRelation(RelationType.Friend, charatype == CharactorType.Player);
            return;
        }
        this.UpdateSpriteOrColorByPlayerUid(uid);
    }

    public void UpdateSpriteOrColor(RelationType type, bool isPlayer = false)
    {
        if (!this.gs.isAbattoirScene || !isPlayer)
        {
            this.UpdateOutlineByRelation(type, isPlayer);
            return;
        }
        if (isPlayer)
        {
            return;
        }
        this.UpdateOutlineByRelation(type, isPlayer);
    }

    public void UpdateSpriteOrColorByPlayerUid(ulong player_uid)
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        string colorConfigByUid = controller.GetColorConfigByUid(player_uid);
        Color colorByName = Const.GetColorByName(colorConfigByUid);
        string colorStrBGNameByUid = controller.GetColorStrBGNameByUid(player_uid);
        string colorStrBGIconNameByUid = controller.GetColorStrBGIconNameByUid(player_uid);
        string colorStrHPFGIconNameByUid = controller.GetColorStrHPFGIconNameByUid(player_uid);
        this.UpdateLastOutline(this.normalTargetInfo.FindChild("name/img_lv").transform, colorByName);
        this.UpdateLastOutline(this.normalTargetInfo.FindChild("name/txt_level").transform, colorByName);
        this.UpdateLastOutline(this.normalTargetInfo.FindChild("name/name/txt_name").transform, colorByName);
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", colorStrBGIconNameByUid, delegate (Sprite sprite)
        {
            this.Img_NormalHPBG.sprite = sprite;
        });
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", colorStrHPFGIconNameByUid, delegate (Sprite sprite)
        {
            this.Img_NormalHPF.sprite = sprite;
        });
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("main", colorStrBGNameByUid, delegate (Sprite sprite)
        {
            this.Img_NormalBG.sprite = sprite;
        });
    }

    public void UpdateOutlineByRelation(RelationType type, bool isPlayer = false)
    {
        Color c = Const.TextColorNormal;
        string spritename = string.Empty;
        string spritename2 = string.Empty;
        string spritename3 = string.Empty;
        switch (type)
        {
            case RelationType.Self:
                c = Const.GetColorByName("friend");
                spritename2 = "st0141";
                spritename3 = "st0142";
                spritename = "bar_portrait_4";
                goto IL_143;
            case RelationType.Friend:
                if (!isPlayer)
                {
                    spritename2 = "st0145";
                    spritename3 = "st0146";
                    spritename = "bar_portrait_3";
                    c = Const.GetColorByName("friendlynpcname_ol");
                }
                else
                {
                    c = Const.GetColorByName("friend");
                    spritename2 = "st0141";
                    spritename3 = "st0142";
                    spritename = "bar_portrait_4";
                }
                goto IL_143;
            case RelationType.Neutral:
                if (isPlayer)
                {
                    spritename2 = "st0141";
                    spritename3 = "st0142";
                    c = Const.GetColorByName("friendlyplayername_ol");
                    spritename = "bar_portrait_4";
                }
                else
                {
                    c = Const.GetColorByName("neutral");
                    spritename2 = "st0143";
                    spritename3 = "st0144";
                    spritename = "bar_portrait_2";
                }
                goto IL_143;
            case RelationType.Enemy:
                c = Const.GetColorByName("enemy");
                spritename2 = "st0139";
                spritename3 = "st0140";
                spritename = "bar_portrait_1";
                goto IL_143;
        }
        c = Const.GetColorByName("neutral");
        spritename2 = "st0143";
        spritename3 = "st0144";
        spritename = "bar_portrait_2";
    IL_143:
        if (this.isBoss)
        {
            this.UpdateLastOutline(this.targetBossInfo.FindChild("name/img_lv").transform, c);
            this.UpdateLastOutline(this.targetBossInfo.FindChild("name/txt_level").transform, c);
            this.UpdateLastOutline(this.targetBossInfo.FindChild("name/txt_name").transform, c);
            this.UpdateLastOutline(this.targetBossInfo.FindChild("name/txt_x").transform, c);
            this.UpdateLastOutline(this.targetBossInfo.FindChild("name/txt_count").transform, c);
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename2, delegate (Sprite sprite)
            {
                this.Img_BossHPBG.sprite = sprite;
            });
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("main", spritename, delegate (Sprite sprite)
            {
                this.Img_BossBG.sprite = sprite;
            });
        }
        else
        {
            this.UpdateLastOutline(this.normalTargetInfo.FindChild("name/img_lv").transform, c);
            this.UpdateLastOutline(this.normalTargetInfo.FindChild("name/txt_level").transform, c);
            this.UpdateLastOutline(this.normalTargetInfo.FindChild("name/name/txt_name").transform, c);
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename2, delegate (Sprite sprite)
            {
                this.Img_NormalHPBG.sprite = sprite;
            });
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename3, delegate (Sprite sprite)
            {
                this.Img_NormalHPF.sprite = sprite;
            });
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("main", spritename, delegate (Sprite sprite)
            {
                this.Img_NormalBG.sprite = sprite;
            });
        }
    }

    private void InitTargetHPInfo(uint totalhp, uint curHP, uint layer)
    {
        this.totalHP = ((totalhp >= 1U) ? totalhp : 1U);
        this.hpLayer = ((layer >= 1U) ? layer : 1U);
        this.currentHP = curHP;
        this.tempHP = curHP;
        this.segmentHP = this.totalHP / this.hpLayer;
        if (this.segmentHP < 1f)
        {
            this.segmentHP = 1f;
        }
        float value = this.DealHP(this.tempHP, this.totalHP, this.hpLayer);
        if (this.isBoss)
        {
            this.Slider_BossBack.value = value;
            this.Txt_BossHp.text = curHP + "/" + totalhp;
            this.Txt_BossHpPercent.text = Mathf.Ceil(curHP / totalhp * 100f) + "%";
        }
        else
        {
            this.Slider_NormalBack.value = value;
            this.Txt_NormalHp.text = curHP + "/" + totalhp;
            this.Txt_NormalHpPercent.text = Mathf.Ceil(curHP / totalhp * 100f) + "%";
        }
    }

    public void UpdateTargetHP(ulong id, uint curHP, uint totalHP)
    {
        if (this._curOwner == null)
        {
            return;
        }
        if (this._curOwner.EID.Id != id)
        {
            return;
        }
        this.totalHP = ((totalHP >= 1U) ? totalHP : 1U);
        this.currentHP = curHP;
        if (this.isBoss)
        {
            this.Txt_BossHp.text = curHP + "/" + totalHP;
            this.Txt_BossHpPercent.text = Mathf.Ceil(curHP / totalHP * 100f) + "%";
        }
        else
        {
            this.Txt_NormalHp.text = curHP + "/" + totalHP;
            this.Txt_NormalHpPercent.text = Mathf.Ceil(curHP / totalHP * 100f) + "%";
        }
        this.UpdateHPChange(this.currentHP);
    }

    private void UpdateHPChange(uint curHP)
    {
        if (this._curOwner == null)
        {
            return;
        }
        this.currentHP = curHP;
        this.hpChangeTime = 0f;
    }

    private void UpdateHPAmount(float amount)
    {
        amount = Mathf.Clamp01(amount);
        if (this.isBoss)
        {
            this.Slider_BossHP.value = amount;
        }
        else
        {
            this.Slider_NormalHP.value = amount;
        }
    }

    private void UpdateHPImgColor(uint curLayer, uint totalLayer)
    {
        if (totalLayer == 1U)
        {
            this.Img_BossHPF.color = Const.GetColorByName("normalwhite");
            this.Img_BossHPB.gameObject.SetActive(false);
        }
        else if (totalLayer > 1U)
        {
            uint num = (uint)this.colors.Length;
            uint num2 = (totalLayer - curLayer) % num;
            uint num3 = (totalLayer - curLayer + 1U) % num;
            this.Img_BossHPF.color = this.colors[(int)num2];
            this.Img_BossHPB.color = this.colors[(int)((UIntPtr)num3)];
            if (curLayer <= 1U)
            {
                this.Img_BossHPB.gameObject.SetActive(false);
            }
            else
            {
                this.Img_BossHPB.gameObject.SetActive(true);
            }
        }
    }

    private void TargetHPUpdator()
    {
        if (this._curOwner == null)
        {
            return;
        }
        this.tempHP = this.currentHP;
        float num = this.DealHP(this.tempHP, this.totalHP, this.hpLayer);
        Slider slider;
        if (this.isBoss)
        {
            slider = this.Slider_BossBack;
        }
        else
        {
            slider = this.Slider_NormalBack;
        }
        if (slider.value < num)
        {
            slider.value = num;
        }
        else if (slider.value > num && this.hpChangeTime <= 1f)
        {
            this.hpChangeTime += Time.deltaTime;
            slider.value = Mathf.Lerp(slider.value, num, this.hpChangeTime);
        }
    }

    private float DealHP(float tempHP, uint totalHP, uint layer)
    {
        if (layer == 0U)
        {
            layer = 1U;
        }
        float num = totalHP / this.hpLayer;
        uint num2 = (uint)Mathf.CeilToInt(tempHP / num);
        if (num2 >= layer)
        {
            num2 = layer;
        }
        float num3 = (tempHP - (num2 - 1U) * num) / num;
        if (this.isBoss)
        {
            this.UpdateHPImgColor(num2, this.hpLayer);
            this.Txt_BossHPLayerNum.text = num2.ToString();
        }
        this.UpdateHPAmount(num3);
        return num3;
    }

    private void UpdateLastOutline(Transform trans, Color c)
    {
        Outline[] components = trans.GetComponents<Outline>();
        if (components == null || components.Length < 1)
        {
            return;
        }
        Outline outline = components[components.Length - 1];
        outline.effectColor = c;
    }

    private void UpdateTargetCareer(cs_MapUserData playerData)
    {
        this.Img_NormalJobBG.gameObject.SetActive(false);
        uint occupation = playerData.mapshow.occupation;
        LuaTable configTable = LuaConfigManager.GetConfigTable("careerLv", (ulong)playerData.mapdata.level);
        if (configTable != null)
        {
            base.GetTexture(ImageType.OTHERS, configTable.GetField_String("careerback"), delegate (Texture2D item)
            {
                if (item != null)
                {
                    Image img_NormalJobBG = this.Img_NormalJobBG;
                    Sprite overrideSprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                    img_NormalJobBG.overrideSprite = overrideSprite;
                }
                else
                {
                    FFDebug.LogWarning(this, "    req  texture   is  null ");
                }
            });
        }
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("careerIcon").GetCacheField_Table(occupation.ToString());
        if (cacheField_Table != null)
        {
            string cacheField_String = cacheField_Table.GetCacheField_String("iconname");
            base.GetTexture(ImageType.ICON, cacheField_String, delegate (Texture2D item)
            {
                if (item != null)
                {
                    Image img_NormalJob = this.Img_NormalJob;
                    Sprite overrideSprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                    img_NormalJob.overrideSprite = overrideSprite;
                    this.Img_NormalJobBG.gameObject.SetActive(true);
                }
                else
                {
                    FFDebug.LogWarning(this, "    req  texture   is  null ");
                }
            });
        }
    }

    private void ShowPlayerOperate()
    {
        if (this._curOwner == null)
        {
            return;
        }
        if (this._curOwner is OtherPlayer)
        {
            LuaScriptMgr.Instance.CallLuaFunction("PlayerOperateCtrl.ReqPlayerInfo", new object[]
            {
                Util.GetLuaTable("PlayerOperateCtrl"),
                this._curOwner.EID.Id
            });
        }
    }

    public void CloseTargetInfo()
    {
        this.isBoss = false;
        this.isShowTargetInfo = false;
        this.CurOwner = null;
        if (this.bossBuffIconCrtl != null)
        {
            this.bossBuffIconCrtl.Clear();
        }
        if (this.normalBuffIconCrtl != null)
        {
            this.normalBuffIconCrtl.Clear();
        }
        if (this.Img_NormalJobBG != null)
        {
            this.Img_NormalJobBG.gameObject.SetActive(false);
        }
        if (this.targetOfTargetPanel != null)
        {
            this.targetOfTargetPanel.gameObject.SetActive(false);
        }
        if (this.targetBossInfo != null)
        {
            this.targetBossInfo.gameObject.SetActive(false);
        }
        if (this.normalTargetInfo != null)
        {
            this.normalTargetInfo.gameObject.SetActive(false);
        }
    }

    public bool GetNormalTargetActive()
    {
        return this.normalTargetInfo.activeSelf;
    }

    public void UpdateTeamMemeberBuffIcon(cs_MapUserData MapUserData)
    {
        List<StateItem> lstState = MapUserData.mapdata.lstState;
        List<Memember> mem = this.teamController.myTeamInfo.mem;
        List<Memember> list = new List<Memember>();
        for (int i = 0; i < mem.Count; i++)
        {
            if (mem[i].mememberid != MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString())
            {
                list.Add(mem[i]);
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            if (list[j].mememberid == MapUserData.charid.ToString() && list[j].mememberid != MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString())
            {
                Transform transform = this.teamObj[j].transform;
                Transform transform2 = transform.Find("buff");
                Transform transform3 = transform.Find("debuff");
                for (int k = 0; k < transform2.childCount; k++)
                {
                    transform2.GetChild(k).gameObject.SetActive(false);
                }
                for (int l = 0; l < transform3.childCount; l++)
                {
                    transform3.GetChild(l).gameObject.SetActive(false);
                }
                List<StateItem> list2 = new List<StateItem>();
                List<StateItem> list3 = new List<StateItem>();
                for (int m = 0; m < lstState.Count; m++)
                {
                    BufferServerDate buffServerData = CommonTools.GetBuffServerData(lstState[m]);
                    LuaTable configTable = LuaConfigManager.GetConfigTable("charstate", (ulong)buffServerData.flag);
                    uint field_Uint = configTable.GetField_Uint("IconShowType");
                    if (configTable != null && field_Uint != 0U)
                    {
                        if (field_Uint == 1U || field_Uint == 4U)
                        {
                            list2.Add(lstState[m]);
                        }
                        else
                        {
                            list3.Add(lstState[m]);
                        }
                    }
                }
                this.DoProgressBuffIcon(list2, transform2);
                this.DoProgressBuffIcon(list3, transform3);
                break;
            }
        }
    }

    private void DoProgressBuffIcon(List<StateItem> buffDataList, Transform buffParent)
    {
        for (int i = 0; i < buffDataList.Count; i++)
        {
            if (i >= buffParent.childCount)
            {
                break;
            }
            BufferServerDate buffServerData = CommonTools.GetBuffServerData(buffDataList[i]);
            Image image = buffParent.GetChild(i).gameObject.GetComponent<Image>();
            LuaTable configTable = LuaConfigManager.GetConfigTable("charstate", (ulong)buffServerData.flag);
            if (configTable != null)
            {
                string iconName = configTable.GetField_String("BuffIcon");
                if (iconName == string.Empty)
                {
                    iconName = "bf0008";
                    FFDebug.LogWarning("CommonItem", "BuffIconItem.UpdateIcon req  texture   is  null strIcon:" + ("charstate," + buffServerData.flag));
                }
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, iconName, delegate (UITextureAsset asset)
                {
                    if (asset == null)
                    {
                        FFDebug.LogWarning("CommonItem", "BuffIconItem.UpdateIcon req  texture   is  null strIcon:" + iconName);
                        return;
                    }
                    if (image == null)
                    {
                        return;
                    }
                    Texture2D textureObj = asset.textureObj;
                    Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                    image.sprite = sprite;
                    image.overrideSprite = sprite;
                    image.gameObject.SetActive(true);
                });
            }
        }
    }

    public void UpdateSelfBuffIcon(EntitiesID eid, LuaTable bufferConfig, BufferServerDate data)
    {
        if (eid.Etype == CharactorType.Player)
        {
            if (eid.Equals(MainPlayer.Self.EID) && this.selfBuffIconCrtl != null)
            {
                this.selfBuffIconCrtl.AddItem(bufferConfig, data);
            }
        }
        else
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component != null && component.TargetCharactor != null && eid == component.TargetCharactor.EID)
            {
                EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
                Npc npc = manager.GetCharactorByID(eid.Id, CharactorType.NPC) as Npc;
                if (npc == null)
                {
                    return;
                }
                LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                if (configTable == null)
                {
                    return;
                }
                if (configTable.GetField_Uint("kind") == 5U)
                {
                    if (this.bossBuffIconCrtl != null)
                    {
                        this.bossBuffIconCrtl.AddItem(bufferConfig, data);
                    }
                }
                else if (this.normalBuffIconCrtl != null)
                {
                    this.normalBuffIconCrtl.AddItem(bufferConfig, data);
                }
            }
        }
    }

    public void RemoveSelfBuffIcon(EntitiesID eid, LuaTable bufferConfig, BufferServerDate data)
    {
        if (eid.Etype == CharactorType.Player)
        {
            if (eid.Equals(MainPlayer.Self.EID) && this.selfBuffIconCrtl != null)
            {
                this.selfBuffIconCrtl.TryRemoveItem(bufferConfig, data);
            }
            if (this.normalBuffIconCrtl != null)
            {
                this.normalBuffIconCrtl.TryRemoveItem(bufferConfig, data);
            }
        }
        else
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            Npc npc = manager.GetCharactorByID(eid.Id, CharactorType.NPC) as Npc;
            if (npc == null)
            {
                return;
            }
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
            if (configTable == null)
            {
                return;
            }
            if (configTable.GetField_Uint("kind") == 5U)
            {
                if (this.bossBuffIconCrtl != null)
                {
                    this.bossBuffIconCrtl.TryRemoveItem(bufferConfig, data);
                }
            }
            else if (this.normalBuffIconCrtl != null)
            {
                this.normalBuffIconCrtl.TryRemoveItem(bufferConfig, data);
            }
        }
    }

    public void UpdateTargetBuffIcon(ulong entryid, CharactorType type, List<StateItem> targetList)
    {
        if (this._curOwner == null)
        {
            return;
        }
        if (this._curOwner.EID.Id != entryid)
        {
            return;
        }
        if (this.isBoss)
        {
            this.bossBuffIconCrtl.AddTargetItems(targetList);
        }
        else
        {
            this.normalBuffIconCrtl.AddTargetItems(targetList);
        }
    }

    public void CheckExitCopyBtn()
    {
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager != null)
        {
            UI_Map mapUI = ControllerManager.Instance.GetController<UIMapController>().MapUI;
            if (manager.InCompetitionCopy)
            {
                if (mapUI != null)
                {
                    mapUI.SetMiddleExitCopyState(false);
                }
            }
            else
            {
                LuaTable currentCopyConfig = manager.currentCopyConfig;
                if (currentCopyConfig != null)
                {
                    CopyType cacheField_Uint = (CopyType)currentCopyConfig.GetCacheField_Uint("type");
                    if (cacheField_Uint == CopyType.First)
                    {
                        if (mapUI != null)
                        {
                            mapUI.SetMiddleExitCopyState(false);
                        }
                    }
                    else if (mapUI != null)
                    {
                        mapUI.SetMiddleExitCopyState(true);
                    }
                }
                else
                {
                    FFDebug.LogWarning(this, "CheckExitCopyBtn cant find curCopyConfig  id = " + manager.MCurrentCopyID);
                }
            }
        }
        else
        {
            FFDebug.LogWarning(this, "CheckExitCopyBtn copyMgr is null !!!  ");
        }
    }

    public void SetBattleEffectVisibility(bool visibility)
    {
        this.m_inBattleEffect.SetActive(visibility);
    }

    public void OpenStartMatchBox(float startMatchTime, PvpMatchType pvpMatchType)
    {
        UI_Map uiobject = UIManager.GetUIObject<UI_Map>();
        if (uiobject != null)
        {
            GlobalRegister.AddTextTipUpdateComponent(uiobject.btnPvp.gameObject, (uint)pvpMatchType, startMatchTime, 0f, 0f);
            uiobject.ActivePvpBtn(true);
        }
    }

    public void CloseMatchBox()
    {
        UI_Map uiobject = UIManager.GetUIObject<UI_Map>();
        if (uiobject != null)
        {
            uiobject.ActivePvpBtn(false);
        }
    }

    public void ActiveTask(bool active)
    {
        this.panelTask.gameObject.SetActive(active);
    }

    public override void OnDispose()
    {
        this.Root.GetComponent<UI_ShortcutControl>().OnDispose();
        this.CombCd.Dispose();
        this.DramaTips.Dispose();
        this.VisitNpcView.Dispose();
        SingletonForMono<InputController>.Instance.mScreenEventController.RemoveListener(new ScreenEventController.OnScreenEvent(this.CheckUnlockFinish));
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        if (this.Root != null)
        {
            UnityEngine.Object.Destroy(this.Root);
        }
        base.OnDispose();
        if (this.mskillShowImage != null)
        {
            this.mskillShowImage.Dispose();
        }
        ControllerManager.Instance.GetController<ChatControl>().OnMainViewDestroy();
    }

    private void btn_story_on_click(PointerEventData pointerEventData)
    {
        this.CallMethod("MainUICtrl", "OpenAdventure");
    }

    private void SetAutoFightBtnState()
    {
        Image component = this.btn_auto_fight.GetComponent<Image>();
        Image component2 = this.btn_auto_fight.transform.Find("img_auto_fight").GetComponent<Image>();
        Image component3 = this.btn_auto_fight.transform.Find("img_auto_cancel").GetComponent<Image>();
        component.sprite = ((!ControllerManager.Instance.GetController<AutoFightController>().isOpenAutoFight) ? component2.sprite : component3.sprite);
    }

    public void AutoFight()
    {
        AutoFightController controller = ControllerManager.Instance.GetController<AutoFightController>();
        controller.isOpenAutoFight = !controller.isOpenAutoFight;
        this.SetAutoFightBtnState();
    }

    private void btn_hero_on_click(PointerEventData pointerEventData)
    {
        ControllerManager.Instance.GetController<CardController>().OpenCharacterUI();
    }

    private void btn_herohandbook_on_click(PointerEventData pointerEventData)
    {
        ControllerManager.Instance.GetController<HeroHandbookController>().OpenUI();
    }

    private void btn_bag_on_click(PointerEventData pointerEventData)
    {
        this.enterBag(null);
    }

    private void btn_dna_on_click(PointerEventData pointerEventData)
    {
        GlobalRegister.OpenGeneUI(0);
    }

    private void btn_tasklist_on_click(PointerEventData pointerEventData)
    {
        ShortcutsConfigController controller = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        if (controller != null)
        {
            controller.ShortcutsTaskUISwitch();
        }
    }

    private void btn_friend_on_click(PointerEventData pointerEventData)
    {
        ControllerManager.Instance.GetController<FriendControllerNew>().ShowFriendUI(null);
    }

    private void btn_master_on_click(PointerEventData pointerEventData)
    {
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel("UI_Master");
        if (luaUIPanel != null)
        {
            this.CallMethod("MentoringCtrl", "CloseMentoringInfo");
        }
        else
        {
            this.CallMethod("MainUICtrl", "OpenMentoringSystemView");
        }
    }

    private void btn_settings_on_click(PointerEventData pointerEventData)
    {
        this.OnSystemSetting(null);
    }

    private void btn_skill_on_click(PointerEventData pointerEventData)
    {
        if (UIManager.GetUIObject<UI_UnLockSkills>() == null)
        {
            ControllerManager.Instance.GetController<UnLockSkillsController>().OpenFrame(UnLockSkillsController.ManualType.Self);
        }
        else
        {
            ControllerManager.Instance.GetController<UnLockSkillsController>().CloseFrame();
        }
    }

    private void btn_pvp_on_click(PointerEventData pointerEventData)
    {
        if (UIManager.GetUIObject<UI_PVPMatch>() != null)
        {
            ControllerManager.Instance.GetController<PVPMatchController>().CloseUI();
        }
        else
        {
            ControllerManager.Instance.GetController<PVPMatchController>().ShowUI();
        }
    }

    private void btn_family_on_click(PointerEventData pointerEventData)
    {
        GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
        controller.OpenCloseGuildPanel();
    }

    private void CallMethod(string uiName, string funcName)
    {
        if (LuaScriptMgr.Instance == null)
        {
            return;
        }
        string name = uiName + "." + funcName;
        LuaScriptMgr.Instance.CallLuaFunction(name, new object[]
        {
            Util.GetLuaTable(uiName)
        });
    }

    public void ShowBattleLog(string log, Color color)
    {
        this.ShowBattleInner(this.obj_bufflog_item, log, color);
    }

    public void ShowSysLog(string log)
    {
        this.ShowBattleInner(this.obj_bufflog2_item, log, Color.white);
    }

    private void ShowBattleInner(GameObject objItm, string log, Color color)
    {
        if (objItm == null)
        {
            return;
        }
        GameObject gameObject;
        if (objItm.transform.parent.childCount > 50)
        {
            gameObject = objItm.transform.parent.GetChild(1).gameObject;
        }
        else
        {
            gameObject = UnityEngine.Object.Instantiate<GameObject>(objItm);
            gameObject.transform.SetParent(objItm.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
        }
        gameObject.GetComponent<Text>().text = log;
        gameObject.GetComponent<Text>().color = color;
        gameObject.transform.SetAsLastSibling();
        objItm.transform.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 0f;
    }

    public void ShowTargetOfTarget(MSG_AttackTargetChange_SC msg)
    {
        bool flag = msg.choosetype == ChooseTargetType.CHOOSE_TARGE_TTYPE_SET;
        this.targetOfTargetPanel.gameObject.SetActive(flag);
        if (flag)
        {
            if (!this.gs.isAbattoirScene)
            {
                this.Set2EndTargetByReleation(msg);
            }
            else
            {
                this.Set2EndTargetByCharId(msg);
            }
        }
    }

    private void Set2EndTargetByReleation(MSG_AttackTargetChange_SC msg)
    {
        Slider component = this.targetOfTargetPanel.Find("hp/Slider").GetComponent<Slider>();
        component.value = msg.HP / 100f;
        Text component2 = this.targetOfTargetPanel.Find("name/txt_name").GetComponent<Text>();
        Image imgBg = this.targetOfTargetPanel.GetComponent<Image>();
        Image imgSliderBg = component.transform.Find("Background").GetComponent<Image>();
        Image imgSliderFill = component.transform.Find("FillArea/Fill").GetComponent<Image>();
        component2.text = msg.name;
        string spritename = string.Empty;
        string empty = string.Empty;
        string spritename2 = string.Empty;
        switch (msg.relation)
        {
            case 1U:
                spritename2 = "hp_3";
                spritename = "bar_2ndtarget_3";
                break;
            case 2U:
                spritename2 = "hp_1";
                spritename = "bar_2ndtarget_1";
                break;
            case 3U:
                spritename2 = "hp_2";
                spritename = "bar_2ndtarget_2";
                break;
            case 4U:
                spritename2 = "hp_4";
                spritename = "bar_2ndtarget_4";
                break;
        }
        UITextureMgr.Instance.GetSpriteFromAtlas("base", spritename, delegate (Sprite sp)
        {
            imgSliderBg.overrideSprite = sp;
        });
        UITextureMgr.Instance.GetSpriteFromAtlas("base", spritename2, delegate (Sprite sp)
        {
            imgSliderFill.overrideSprite = sp;
        });
        UITextureMgr.Instance.GetSpriteFromAtlas("main", spritename, delegate (Sprite sp)
        {
            imgBg.overrideSprite = sp;
        });
    }

    private void Set2EndTargetByCharId(MSG_AttackTargetChange_SC msg)
    {
        if (!this.abattoirController.IsAbattoirTeamNember(msg.charid))
        {
            this.Set2EndTargetByReleation(msg);
            return;
        }
        string colorConfigByUid = this.abattoirController.GetColorConfigByUid(msg.charid);
        Slider component = this.targetOfTargetPanel.Find("hp/Slider").GetComponent<Slider>();
        component.value = msg.HP / 100f;
        Text component2 = this.targetOfTargetPanel.Find("name/txt_name").GetComponent<Text>();
        Image imgBg = this.targetOfTargetPanel.GetComponent<Image>();
        Image imgSliderBg = component.transform.Find("Background").GetComponent<Image>();
        Image imgSliderFill = component.transform.Find("FillArea/Fill").GetComponent<Image>();
        component2.text = msg.name;
        string color2EndTargetImageBgNameByUid = this.abattoirController.GetColor2EndTargetImageBgNameByUid(msg.charid);
        string color2EndTargetImageFillBgNameByUid = this.abattoirController.GetColor2EndTargetImageFillBgNameByUid(msg.charid);
        string color2EndTargetImageSpFillNameByUid = this.abattoirController.GetColor2EndTargetImageSpFillNameByUid(msg.charid);
        UITextureMgr.Instance.GetSpriteFromAtlas("base", color2EndTargetImageFillBgNameByUid, delegate (Sprite sp)
        {
            imgSliderBg.overrideSprite = sp;
        });
        UITextureMgr.Instance.GetSpriteFromAtlas("base", color2EndTargetImageSpFillNameByUid, delegate (Sprite sp)
        {
            imgSliderFill.overrideSprite = sp;
        });
        UITextureMgr.Instance.GetSpriteFromAtlas("main", color2EndTargetImageBgNameByUid, delegate (Sprite sp)
        {
            imgBg.overrideSprite = sp;
        });
    }

    private void btn_teamitem_onclick(PointerEventData eventData)
    {
        string memberid = eventData.pointerPress.name;
        Memember memember = this.teamController.GetTeamMememberInfo(ulong.Parse(memberid));
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (memberid != MainPlayer.Self.GetCharID().ToString())
            {
                OtherPlayer targetPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(ulong.Parse(memberid), CharactorType.Player) as OtherPlayer;
                if (targetPlayer == null)
                {
                    FFDebug.LogError(this, string.Format("UI_TeamSecondary cant find mCurSelTargetPlayerID= {0} !!!", this.mCurSelTargetPlayerID));
                    return;
                }
                CommonTools.SetSecondPanelPos(eventData);
                UIManager.Instance.ShowUI<UI_TeamSecondary>("UI_TeamSecondary", delegate ()
                {
                    if (memember == null)
                    {
                        memember = new Memember();
                        memember.mememberid = memberid;
                        memember.name = targetPlayer.OtherPlayerData.MapUserData.name;
                    }
                    UIManager.GetUIObject<UI_TeamSecondary>().Initilize(memember, eventData, true);
                }, UIManager.ParentType.CommonUI, false);
            }
        }
        else
        {
            this.TeamMemberTargetClick(memember);
        }
    }

    public void SteupGroupTeam(List<MatchMember> memList)
    {
        this.objGroupItemDic = new Dictionary<ulong, GameObject>();
        this.mMemDic = memList;
        Dictionary<ulong, List<MatchMember>> dictionary = new Dictionary<ulong, List<MatchMember>>();
        for (int i = 0; i < memList.Count; i++)
        {
            MatchMember matchMember = memList[i];
            if (!dictionary.ContainsKey(matchMember.gid))
            {
                dictionary[matchMember.gid] = new List<MatchMember>();
            }
            dictionary[matchMember.gid].Add(matchMember);
        }
        for (int j = 0; j < 4; j++)
        {
            Transform transform = this.transGroupTeamPanel.Find("team" + (j + 1));
            if (j < dictionary.Count)
            {
                List<MatchMember> list = dictionary[(ulong)((long)j + 1L)];
                for (int k = 0; k < 5; k++)
                {
                    Transform child = transform.GetChild(k + 1);
                    if (k < list.Count && child != null)
                    {
                        MatchMember matchMember2 = list[k];
                        Text component = child.Find("txt_lv").GetComponent<Text>();
                        Text component2 = child.Find("txt_playername").GetComponent<Text>();
                        GameObject gameObject = child.Find("img_leader").gameObject;
                        Slider component3 = child.Find("hp").GetComponent<Slider>();
                        component.text = CommonTools.GetLevelFormat(matchMember2.level);
                        component2.text = matchMember2.name;
                        gameObject.SetActive(false);
                        component3.value = 1f;
                        child.gameObject.SetActive(true);
                        child.gameObject.name = matchMember2.userid.ToString();
                        UIEventListener.Get(child.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_teamitem_onclick);
                        this.objGroupItemDic.Add(matchMember2.userid, child.gameObject);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                transform.gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
        this.panelTeam.gameObject.SetActive(false);
        this.SwitchChatModeUI(true);
    }

    public void SwitchChatModeUI(bool isBattle)
    {
        ChatControl controller = ControllerManager.Instance.GetController<ChatControl>();
        if (controller.mUiChat == null)
        {
            return;
        }
        if (isBattle)
        {
            controller.mUiChat.SwitchInputChannel(ChannelType.ChannelType_Team);
        }
        else
        {
            controller.mUiChat.SwitchInputChannel(ChannelType.ChannelType_Scene);
        }
    }

    public void RefreshGroupItemHp(MSG_updateTeamMememberHp_SC data)
    {
        ulong key = ulong.Parse(data.memid);
        if (!this.objGroupItemDic.ContainsKey(key))
        {
            return;
        }
        GameObject gameObject = this.objGroupItemDic[key];
        Slider component = gameObject.transform.Find("hp").GetComponent<Slider>();
        component.value = data.hp * 1f / data.maxhp;
    }

    public bool IsInBattleScene()
    {
        return this.transGroupTeamPanel.gameObject.activeSelf;
    }

    public void ShowGroupTeam()
    {
        this.transGroupTeamPanel.gameObject.SetActive(true);
    }

    public void HideGroupTeam()
    {
        this.transGroupTeamPanel.gameObject.SetActive(false);
        if (this.teamController.CheckIfInTeam(MainPlayer.Self.BaseData.BaseData.id.ToString()))
        {
            this.panelTeam.gameObject.SetActive(true);
        }
    }

    public void EnableSevenDays(MSG_Ret_Day7ActivityInfo_SC SevenInfo)
    {
        bool active = false;
        if (SevenInfo != null && SevenInfo.data != null)
        {
            for (int i = 0; i < SevenInfo.data.Count; i++)
            {
                if (SevenInfo.data[i].state < ActivityState.ACTIVITY_STATE_GOTPRIZE)
                {
                    active = true;
                    break;
                }
            }
        }
        this.btn_sevendays.gameObject.SetActive(active);
    }

    public void EnableAchievement()
    {
        this.AchievementView.EnableAchievement(this.Root.transform);
    }

    public void RefreshBagNewTip(bool hasNew)
    {
        this.objBagHasNew.SetActive(hasNew);
    }

    public void refreshAbattoirTeamInfo()
    {
        GameObject gameObject = this.panelPersonal.Find("img_leader").gameObject;
        GameObject gameObject2 = this.panelPersonal.Find("img_inv").gameObject;
        gameObject.SetActive(false);
        gameObject2.SetActive(false);
        this.btn_TeamCallBack.SetActive(false);
        if (this.abattoirController.myTeamInfo != null && this.abattoirController.myTeamInfo.users.Count > 0)
        {
            List<TeamUser> list = (from item in this.abattoirController.myTeamInfo.users
                                   where item.uid != MainPlayer.Self.OtherPlayerData.MapUserData.charid
                                   select item).ToList<TeamUser>();
            this.showTask = false;
            this.setTaskState();
            for (int i = 0; i < list.Count; i++)
            {
                TeamUser teamUser = list[i];
                this.teamObj[i].name = "Item" + i;
                Transform transform = this.teamObj[i].transform;
                Transform transform2 = transform.Find("panel_value");
                if (transform2 != null)
                {
                    FightValueNum x = transform2.GetComponent<FightValueNum>();
                    if (x == null)
                    {
                        x = transform2.gameObject.AddComponent<FightValueNum>();
                    }
                    transform2.gameObject.SetActive(false);
                }
                transform.Find("img_leader").gameObject.SetActive(false);
                transform.Find("img_inv").gameObject.SetActive(false);
                string text = teamUser.uid.ToString();
                ControllerManager.Instance.GetController<FriendControllerNew>().AddRecentItem(text);
                OtherPlayer playerByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetPlayerByID(ulong.Parse(text));
                if (playerByID != null)
                {
                    OtherPlayerData otherPlayerData = playerByID.OtherPlayerData;
                    this.UpdateTeamMemeberBuffIcon(otherPlayerData.MapUserData);
                }
                else
                {
                    Transform transform3 = transform.Find("buff");
                    Transform transform4 = transform.Find("debuff");
                    for (int j = 0; j < transform3.childCount; j++)
                    {
                        transform3.GetChild(j).gameObject.SetActive(false);
                    }
                    for (int k = 0; k < transform4.childCount; k++)
                    {
                        transform4.GetChild(k).gameObject.SetActive(false);
                    }
                }
                bool needRefreshHeadIcon = false;
                string b = (i < this.iconList.Count) ? this.iconList[i] : string.Empty;
                string abattoirHeroHeadIconString = this.GetAbattoirHeroHeadIconString(teamUser);
                if (abattoirHeroHeadIconString != b)
                {
                    needRefreshHeadIcon = true;
                }
                this.setAbattoirTeamObj(this.teamObj[i], this.headObj[i], teamUser, needRefreshHeadIcon);
                this.UpdateAbattoirTargetInfo(i);
            }
            this.iconList.Clear();
            for (int l = 0; l < list.Count; l++)
            {
                TeamUser memember = list[l];
                this.iconList.Add(this.GetAbattoirHeroHeadIconString(memember));
            }
            int num = 0;
            while ((long)num < (long)((ulong)UI_MainView.maxMemberNum))
            {
                if (num < list.Count)
                {
                    this.teamObj[num].SetActive(true);
                }
                else
                {
                    this.teamObj[num].SetActive(false);
                }
                num++;
            }
        }
        else
        {
            int num2 = 0;
            while ((long)num2 < (long)((ulong)UI_MainView.maxMemberNum))
            {
                this.teamObj[num2].SetActive(false);
                num2++;
            }
            this.panelTask.Find("TeamMember/txt_info").gameObject.SetActive(true);
            this.iconList.Clear();
        }
    }

    private string GetAbattoirHeroHeadIconString(TeamUser memember)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(memember.uid);
        stringBuilder.Append(memember.bodystyle);
        stringBuilder.Append(memember.haircolor);
        stringBuilder.Append(memember.hairstyle);
        stringBuilder.Append(memember.headstyle);
        stringBuilder.Append(memember.antenna);
        return stringBuilder.ToString();
    }

    private void UpdateAbattoirTargetInfo(int index)
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            TeamUser teamUser = this.abattoirController.myTeamInfo.users[index];
            controller.UpdateBaseInfo(teamUser.uid, teamUser.name, teamUser.level);
        }
    }

    private void setAbattoirTeamObj(GameObject obj, GameObject heroObj, TeamUser mem, bool needRefreshHeadIcon)
    {
        obj.SetActive(true);
        obj.transform.Find("txt_playername").GetComponent<Text>().text = mem.name;
        Slider component = obj.transform.Find("hp").GetComponent<Slider>();
        if (!mem.online)
        {
            component.value = 1f;
            obj.transform.Find("hp/txt_percent").GetComponent<Text>().text = "100%";
        }
        else
        {
            component.value = mem.hp / mem.maxhp;
            obj.transform.Find("hp/txt_percent").GetComponent<Text>().text = Mathf.Ceil(mem.hp / mem.maxhp * 100f) + "%";
        }
        obj.transform.Find("txt_hp/value").GetComponent<Text>().text = mem.hp + "/" + mem.maxhp;
        obj.transform.Find("txt_lv").GetComponent<Text>().text = CommonTools.GetLevelFormat(mem.level);
        ulong uid = mem.uid;
        RawImage component2 = heroObj.GetComponent<RawImage>();
        component2.gameObject.SetActive(true);
        if (needRefreshHeadIcon)
        {
            uint[] featureIDs = new uint[]
            {
                mem.haircolor,
                mem.hairstyle,
                mem.headstyle,
                mem.antenna,
                mem.avatarid
            };
            GlobalRegister.ShowPlayerRTT(component2, mem.heroid, mem.bodystyle, featureIDs, 0, null);
        }
        if (!mem.online)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().SetImageGrey4Head(component2, true);
            obj.transform.Find("img_icon/img_mask").gameObject.SetActive(false);
            obj.transform.Find("img_icon/img_auto").gameObject.SetActive(false);
        }
        else
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().SetImageGrey4Head(component2, false);
            obj.transform.Find("img_icon/img_mask").gameObject.SetActive(false);
            obj.transform.Find("img_icon/img_auto").gameObject.SetActive(false);
        }
    }

    public void refreshAbattoirRankList()
    {
        if (this.abattoirRankPanel == null)
        {
            return;
        }
        Transform parent = this.abattoirRankPanel.Find("Rank/list");
        Transform itemSkinTran = this.abattoirRankPanel.Find("Rank/list/item");
        if (itemSkinTran == null)
        {
            return;
        }
        itemSkinTran.gameObject.SetActive(false);
        int i = this.abattoirController.getRankCount;
        while (i < this.rankItemList.Count)
        {
            UnityEngine.Object.Destroy(this.rankItemList[i].gameObject);
            this.rankItemList.RemoveAt(i);
        }
        this.abattoirController.ForRankList(delegate (int index, string color, uint power)
        {
            Transform transform;
            if (index >= this.rankItemList.Count)
            {
                transform = UnityEngine.Object.Instantiate<GameObject>(itemSkinTran.gameObject).transform;
                transform.SetParent(parent);
                transform.localScale = Vector3.one;
                this.rankItemList.Add(transform);
            }
            else
            {
                transform = this.rankItemList[index];
            }
            transform.SetSiblingIndex(index);
            Text component = transform.Find("num").GetComponent<Text>();
            Text component2 = transform.Find("power").GetComponent<Text>();
            Slider component3 = transform.Find("slider").GetComponent<Slider>();
            Image colorImage = transform.Find("slider/Fill Area/Fill").GetComponent<Image>();
            string rankColorImageNameByConfig = this.abattoirController.GetRankColorImageNameByConfig(color);
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", rankColorImageNameByConfig, delegate (Sprite sprite)
            {
                colorImage.sprite = sprite;
            });
            Transform transform2 = transform.Find("icon");
            component.text = string.Format("第{0}名", index + 1);
            component2.text = power.ToString();
            component3.value = power / 1000f;
            transform2.gameObject.SetActive(color == this.abattoirController.getSelfTeamColor);
            transform.gameObject.SetActive(true);
        });
    }

    public void refreshPersonal(bool isAbattoirScene)
    {
        Transform transform = this.panelPersonal.Find("img_portrait2");
        if (!isAbattoirScene)
        {
            this.setPersonal();
            this.btnChangeHero.gameObject.SetActive(true);
            transform.gameObject.SetActive(true);
        }
        else
        {
            GameObject gameObject = this.panelPersonal.Find("img_leader").gameObject;
            GameObject gameObject2 = this.panelPersonal.Find("img_inv").gameObject;
            gameObject.SetActive(false);
            gameObject2.SetActive(false);
            this.btn_TeamCallBack.SetActive(false);
            this.btnChangeHero.gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
        }
    }

    public void refreshSelfTeamColorSpr(bool isAbattoirAndColorReady)
    {
        string spritename = "st0142";
        string spritename2 = "st0141";
        string spritename3 = "bar_portrait_4";
        if (isAbattoirAndColorReady)
        {
            ulong id = MainPlayer.Self.EID.Id;
            spritename = this.abattoirController.GetColorStrHPFGIconNameByUid(id);
            spritename2 = this.abattoirController.GetColorStrBGIconNameByUid(id);
            spritename3 = this.abattoirController.GetColorStrBGNameByUid(id);
        }
        Image HPFGIconImage = this.panelPersonal.Find("hp/Slider/Fill Area/Fill").GetComponent<Image>();
        Image BGIconImage = this.panelPersonal.Find("hp/Slider/Background").GetComponent<Image>();
        Image BGImage = this.panelPersonal.Find("img_portrait1").GetComponent<Image>();
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename, delegate (Sprite sprite)
        {
            HPFGIconImage.sprite = sprite;
        });
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename2, delegate (Sprite sprite)
        {
            BGIconImage.sprite = sprite;
        });
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("main", spritename3, delegate (Sprite sprite)
        {
            BGImage.sprite = sprite;
        });
        int num = 0;
        while ((long)num < (long)((ulong)UI_MainView.maxMemberNum))
        {
            GameObject gameObject = this.teamObj[num];
            HPFGIconImage = gameObject.FindChild("hp/Fill Area/Fill").GetComponent<Image>();
            BGIconImage = gameObject.FindChild("hp/Background").GetComponent<Image>();
            BGImage = gameObject.GetComponent<Image>();
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename, delegate (Sprite sprite)
            {
                HPFGIconImage.sprite = sprite;
            });
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename2, delegate (Sprite sprite)
            {
                BGIconImage.sprite = sprite;
            });
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("main", spritename3, delegate (Sprite sprite)
            {
                BGImage.sprite = sprite;
            });
            num++;
        }
    }

    public void ShowLvFuncShow(bool show)
    {
        if (this.panelTop != null)
        {
            this.panelTop.SetActive(show);
        }
        if (this.AchievementView != null)
        {
            this.AchievementView.ShowAchievement(show);
        }
    }

    private GameScene gs_;

    public UI_CombCD CombCd;

    public UI_DramaTips DramaTips;

    public UI_VisitNpcInMainView VisitNpcView;

    public UI_Achievement_MainView AchievementView;

    private bool bottomShow;

    public bool showTask = true;

    public GameObject Root;

    private GameObject btnExp;

    private Transform panelMenu;

    private Transform panelTask;

    private Transform panelTeam;

    private Transform panelPersonal;

    private Slider personalHpSlider;

    private Transform panelBottom;

    private Transform panelChat;

    private Transform btnVip;

    private Transform tranTaskSwitch;

    public Transform panelSkill;

    private Transform btn_unlockSkill;

    private Transform panel_MapName;

    private Transform btn_TaskListHide;

    private GameObject[] btnMsgItems;

    private doubleSlider expSlider;

    private GameObject btnTaskOn;

    private GameObject btnTaskOff;

    private GameObject btnTeamOn;

    private GameObject btnTeamOff;

    private GameObject btnVisitNpc;

    private GameObject btnVisitNpcSpecial;

    private GameObject TaskList;

    private GameObject TaskListScrollView;

    private GameObject goSystemSetting;

    private GameObject btnCameraReset;

    private GameObject btn3dCamera;

    private GameObject btn2dCamera;

    private Button btn_tab;

    private GameObject objBagHasNew;

    private GameObject panelHoldonCountDown;

    private Text lbHoldonCountDownNpcName;

    private Text lbHoldonCountDownremaintime;

    private Button buffer1;

    private GameObject panelBuffInfo;

    private Image spBufferInfoBuffIcon;

    private Button btnBufferInfoCloseBuffInfo;

    private Text lbBufferInfoBuffName;

    private Text lbBufferInfoBuffDesc;

    private List<TweenAlpha> mapNameTweenAlpha = new List<TweenAlpha>();

    private Text mapName;

    private Text mapNameEn;

    private Image mapTypIcon;

    private RawImage headIcon;

    public RawImage headIcon2;

    private Text headLevel;

    private Text headLevel2;

    private Button btnChangeHero;

    private List<Transform> txt_skillList = new List<Transform>();

    private static uint maxMemberNum = 4U;

    private GameObject[] teamObj = new GameObject[UI_MainView.maxMemberNum];

    private GameObject[] headObj = new GameObject[UI_MainView.maxMemberNum];

    private static float hp;

    private static float maxhp;

    private Transform panelPet;

    private GameObject fightpetObjectbg;

    private GameObject petBlank;

    private GameObject assistPetObjectbg;

    private GameObject assistPetBlank;

    private GameObject btn_TeamCallBack;

    private GameObject m_inBattleEffect;

    public ui_SkillShowImage mskillShowImage = new ui_SkillShowImage();

    private GameObject panelSelfBuffIcon;

    private BuffIconController selfBuffIconCrtl;

    private GameObject targetBossInfo;

    private Image Img_BossBG;

    private Image Img_BossHPF;

    private Image Img_BossHPB;

    private Image Img_BossHPBG;

    private Text Txt_BossHPLayerNum;

    private Text Txt_BossName;

    private Text Txt_BossLevelNum;

    private Slider Slider_BossHP;

    private Slider Slider_BossBack;

    private GameObject bossBuffIcon;

    private BuffIconController bossBuffIconCrtl;

    private Text Txt_BossHp;

    private Text Txt_BossHpPercent;

    private GameObject normalTargetInfo;

    private Image Img_NormalBG;

    private Image Img_NormalHPF;

    private Image Img_NormalHPBG;

    private Image Img_NormalJob;

    private Image Img_NormalJobBG;

    private Text Txt_NormalName;

    private RawImage icon3DImage;

    private RawImage icon3DImageNpc;

    private Text Txt_NormalLevelNum;

    private Slider Slider_NormalHP;

    private Slider Slider_NormalBack;

    private GameObject normalBuffIcon;

    private BuffIconController normalBuffIconCrtl;

    private Text Txt_NormalHp;

    private Text Txt_NormalHpPercent;

    private Transform targetOfTargetPanel;

    private Button btn_auto_fight;

    private Image img_fightvalue_item;

    private Image img_exp_player;

    private Image img_exp_character;

    private GameObject panelTop;

    private Button btn_activityguide;

    private Button btn_sevendays;

    private Button btn_questionnaire;

    private GameObject goRegression;

    public GameObject obj_bufflog_item;

    public GameObject obj_bufflog2_item;

    private GameObject obj_numbertip;

    public Transform transGroupTeamPanel;

    public Transform abattoirRankPanel;

    private List<Transform> rankItemList = new List<Transform>();

    private GameObject skillNewIcon;

    private float watchedhp;

    private Dictionary<MessageType, GameObject> DicMessageAndObj = new Dictionary<MessageType, GameObject>();

    private string expPlayerTip = string.Empty;

    private string expCharactorTip = string.Empty;

    private Image imgjobBack;

    private Image imgjobIcon;

    private PetBase petFightData;

    private PetBase petAssistData;

    public List<SkillButton> SkillButtonList = new List<SkillButton>();

    public List<SkillButton> EvolutionButtonList = new List<SkillButton>();

    public List<GameObject> EvolutionBackGo = new List<GameObject>();

    private GameObject objBtnPetQte;

    private Image imgPetQte;

    private uint _distance;

    private float _LeftTime;

    private float _maxTime;

    private float _curTime;

    private Slider com_sliderhp;

    private Slider com_slidermp;

    private GameObject Offset_Main;

    private List<string> iconList = new List<string>();

    private Vector3 chatMovePostion = new Vector3(1000f, 0f, 0f);

    private Vector3 chatLocalPostion = Vector3.zero;

    private List<GameObject> listTaskItemGameObject = new List<GameObject>();

    private bool isShowTaskList = true;

    private float MemberCallBackTime;

    private bool OpentaskAndTeamView;

    private BetterDictionary<UserState, TimeBuffStateData> TimeBuff = new BetterDictionary<UserState, TimeBuffStateData>();

    private MainPlayerSkillHolder mainPlayerSkillHolder;

    private float _fTweenTime = 0.5f;

    private SkillButton _temp;

    private bool _isUnlocking;

    private Vector3 _unLockStartPos = new Vector3(-385f, -10f, 0f);

    private Color[] colors;

    private CharactorBase _curOwner;

    private bool isShowTargetInfo;

    private bool isBoss;

    private uint totalHP;

    private uint currentHP;

    private float tempHP;

    private float segmentHP;

    private uint hpLayer;

    private float hpChangeTime;

    private ulong mCurSelTargetPlayerID;

    public List<MatchMember> mMemDic;

    private Dictionary<ulong, GameObject> objGroupItemDic = new Dictionary<ulong, GameObject>();
}
