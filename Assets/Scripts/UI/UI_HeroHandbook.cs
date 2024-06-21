using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HeroHandbook : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.transRoot = root;
        Transform transform = root.Find("Offset_Example/Panel_heropokedex");
        this.lbl_heroname = transform.Find("Panel_heroname/text_name").GetComponent<Text>();
        this.lbl_herodesc = transform.Find("Panel_heroname/text_desc").GetComponent<Text>();
        this.lbl_level = transform.Find("Panel_exp/txt_value").GetComponent<Text>();
        this.slider_exp = transform.Find("Panel_exp/lv_pro").GetComponent<Scrollbar>();
        this.slider_exp.gameObject.AddComponent<TextTip>().SetTextCb(new TextTipContentCb(this.GetExpTipContent));
        try
        {
            Image component = transform.Find("Panel_exp/lv_pro/Sliding Area/Handle").GetComponent<Image>();
            component.raycastTarget = false;
        }
        catch (Exception str)
        {
            FFDebug.LogWarning(this, str);
        }
        this.obj_img_icon = transform.Find("Mask/Mask/img_model").GetComponent<RawImage>();
        Color color = this.obj_img_icon.color;
        color.a = 0f;
        this.obj_img_icon.color = color;
        this.objLock = transform.Find("Mask/Mask/img_lock").gameObject;
        this.txtLock = this.objLock.transform.Find("Text").GetComponent<Text>();
        this.BASE_HEIGHT = Mathf.CeilToInt((this.obj_img_icon.GetComponent<Transform>() as RectTransform).sizeDelta.x);
        this.slider_dutiao = transform.Find("Slider_dutiao").GetComponent<Slider>();
        this.tgl_online = transform.Find("Toggle").GetComponent<Toggle>();
        this.obj_handbook_item = transform.Find("Image/Rect/Panel_herolist/img_hero").gameObject;
        this.obj_tupo_item = null;
        this.btn_break = transform.Find("btn_break").gameObject;
        this.btn_sure = transform.Find("btn_set").gameObject;
        this.btn_close = transform.Find("Panel_title/btn_close").gameObject;
        this.image_skills = new List<Image>();
        for (int i = 1; i < 5; i++)
        {
            Image component2 = transform.Find("Panel_skillshow/img_groove" + i + "/img_skill").GetComponent<Image>();
            this.image_skills.Add(component2);
            UIEventListener.Get(component2.gameObject).onEnter = new UIEventListener.VoidDelegate(this.skill_button_enter);
            UIEventListener.Get(component2.gameObject).onDestroy = new UIEventListener.VoidDelegate(this.skill_button_exit);
            UIEventListener.Get(component2.gameObject).onExit = new UIEventListener.VoidDelegate(this.skill_button_exit);
        }
        this.trans_Mask = (transform.Find("Mask/Mask") as RectTransform);
        this.transHeroNamePanel = this.transRoot.Find("Offset_Example/Panel_heropokedex/Panel_heroname");
        this.transExpPanel = this.transRoot.Find("Offset_Example/Panel_heropokedex/Panel_exp");
        this.transBottomPanel = this.transRoot.Find("Offset_Example/Panel_heropokedex/Panel_skillshow");
        this.transFightValuePanel = this.transRoot.Find("Offset_Example/Panel_heropokedex/Mask/Mask/Panel");
        this.transFightValueItem = this.transRoot.Find("Offset_Example/Panel_heropokedex/Mask/Mask/Panel/panel_ce/panel_value/img_num");
        this.transBtnFetters = this.transRoot.Find("Offset_Example/Panel_heropokedex/btn_fetters");
        this.transPanelTipFetters = this.transBtnFetters.Find("Tip4Fetters");
        this.transPanelTipFetters.gameObject.SetActive(false);
        HoverEventListener hoverEventListener = HoverEventListener.Get(this.transBtnFetters.gameObject);
        hoverEventListener.onEnter = (HoverEventListener.VoidDelegate)Delegate.Combine(hoverEventListener.onEnter, new HoverEventListener.VoidDelegate(delegate (PointerEventData p)
        {
            if (this.mSelHeroId != 0)
            {
                this.transPanelTipFetters.gameObject.SetActive(true);
            }
        }));
        HoverEventListener hoverEventListener2 = HoverEventListener.Get(this.transBtnFetters.gameObject);
        hoverEventListener2.onExit = (HoverEventListener.VoidDelegate)Delegate.Combine(hoverEventListener2.onExit, new HoverEventListener.VoidDelegate(delegate (PointerEventData p)
        {
            this.transPanelTipFetters.gameObject.SetActive(false);
        }));
        this.mController = ControllerManager.Instance.GetController<HeroHandbookController>();
        this.mHeroConfigDic = new Dictionary<int, LuaTable>();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("heros");
        configTableList.Sort((LuaTable x, LuaTable y) => x.GetField_Uint("id").CompareTo(y.GetField_Uint("id")));
        for (int j = 0; j < configTableList.Count; j++)
        {
            LuaTable luaTable = configTableList[j];
            this.mHeroConfigDic.Add(luaTable.GetField_Int("id"), luaTable);
        }
        this.mNpcConfigDic = new Dictionary<int, LuaTable>();
        List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList("npc_data");
        for (int k = 0; k < configTableList2.Count; k++)
        {
            this.mNpcConfigDic.Add(configTableList2[k].GetField_Int("id"), configTableList2[k]);
        }
        this.mSkillShowConfigDic = new Dictionary<int, LuaTable>();
        List<LuaTable> configTableList3 = LuaConfigManager.GetConfigTableList("skillshow");
        for (int l = 0; l < configTableList3.Count; l++)
        {
            this.mSkillShowConfigDic.Add(configTableList3[l].GetField_Int("id"), configTableList3[l]);
        }
        UIEventListener.Get(this.btn_sure).onClick = new UIEventListener.VoidDelegate(this.btn_sure_on_click);
        UIEventListener.Get(this.btn_close).onClick = new UIEventListener.VoidDelegate(this.btn_close_on_click);
        UIEventListener.Get(this.obj_img_icon.gameObject).onDrag = new UIEventListener.VoidDelegate(this.obj_img_icon_on_drag);
        this.SetupHandbook();
        base.OnInit(root);
    }

    private string GetExpTipContent()
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        uint level = 1U;
        int num = 0;
        if (this.ltData != null)
        {
            level = (uint)this.ltData.GetField_Int("level");
            num = this.ltData.GetField_Int("exp");
        }
        uint curLevelAllExp = controller.GetCurLevelAllExp(level);
        return num + "/" + curLevelAllExp;
    }

    private void skill_button_enter(PointerEventData eventData)
    {
        int num = 0;
        for (int i = 0; i < this.image_skills.Count; i++)
        {
            if (this.image_skills[i].gameObject == eventData.pointerEnter.gameObject)
            {
                num = i;
                break;
            }
        }
        if (this.ltData == null)
        {
            MainPlayerSkillBase skillData = new MainPlayerSkillBase(MainPlayerSkillHolder.Instance, uint.Parse(this.coreskillarr[num]), 1U, 0U);
            ControllerManager.Instance.GetController<ItemTipController>().OpenSkillPanel(skillData, eventData.pointerEnter.gameObject);
        }
        else
        {
            LuaTable heroDataByHeroId = this.mController.GetHeroDataByHeroId(this.mSelHeroId);
            LuaTable field_Table = heroDataByHeroId.GetField_Table("Skill");
            foreach (object obj in field_Table.Values)
            {
                LuaTable luaTable = obj as LuaTable;
                uint field_Uint = luaTable.GetField_Uint("skillorgid");
                if (field_Uint > 0U)
                {
                    if (field_Uint == uint.Parse(this.coreskillarr[num]))
                    {
                        Debug.LogWarning("equal=" + field_Uint);
                        uint field_Uint2 = luaTable.GetField_Uint("skilllevel");
                        MainPlayerSkillBase skillData2 = new MainPlayerSkillBase(MainPlayerSkillHolder.Instance, uint.Parse(this.coreskillarr[num]), field_Uint2, 0U);
                        ControllerManager.Instance.GetController<ItemTipController>().OpenSkillPanel(skillData2, eventData.pointerEnter.gameObject);
                        break;
                    }
                    Debug.Log("skillorgid=" + field_Uint);
                }
            }
        }
    }

    private void skill_button_exit(PointerEventData eventData)
    {
    }

    private void SetupHandbook()
    {
        UIManager.Instance.ClearListChildrens(this.obj_handbook_item.transform.parent, 1);
        this.heroHandbookObjDic = new Dictionary<int, GameObject>();
        foreach (LuaTable luaTable in this.mHeroConfigDic.Values)
        {
            if (luaTable.GetField_Int("isshow") == 1)
            {
                int heroId = luaTable.GetField_Int("id");
                GameObject gameObject = this.CreateHandbookItem(heroId);
                gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    this.mSelHeroId = heroId;
                    this.SetupInfo();
                    this.SetUpFetterIncreaseTipsData(heroId);
                });
                UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_handbookitem_on_click);
                this.heroHandbookObjDic.Add(heroId, gameObject);
                if (heroId == this.mController.mMainHeroId)
                {
                    this.SetUpFetterIncreaseTipsData(heroId);
                }
            }
        }
    }

    private GameObject CreateHandbookItem(int heroId)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_handbook_item);
        gameObject.transform.SetParent(this.obj_handbook_item.transform.parent, false);
        gameObject.transform.SetAsLastSibling();
        gameObject.name = heroId.ToString();
        gameObject.SetActive(true);
        this.SetHeroIcon(heroId, gameObject);
        return gameObject;
    }

    private void SetHeroIcon(int heroId, GameObject obj)
    {
        Image image = obj.transform.Find("img_get").GetComponent<Image>();
        if (this.mNpcConfigDic.ContainsKey(heroId))
        {
            string field_String = this.mNpcConfigDic[heroId].GetField_String("UIicon");
            base.GetSprite(ImageType.ROLES, field_String, delegate (Sprite item)
            {
                if (image == null)
                {
                    return;
                }
                if (item != null)
                {
                    image.overrideSprite = item;
                    image.sprite = item;
                    image.color = Color.white;
                    image.material = null;
                }
            });
        }
        else
        {
            image.overrideSprite = null;
            image.sprite = null;
            image.color = new Color(0f, 0f, 0f, 0f);
            image.material = null;
            Debug.LogError("------------ npc does not id=" + heroId);
        }
    }

    private void btn_handbookitem_on_click(PointerEventData eventData)
    {
        this.mSelHeroId = int.Parse(eventData.pointerPress.gameObject.name);
        if (this.heroHandbookObjDic.ContainsKey(this.mSelHeroId))
        {
            if (this.m_LastImageSelected != null)
            {
                this.m_LastImageSelected.SetActive(false);
            }
            this.m_LastImageSelected = this.heroHandbookObjDic[this.mSelHeroId].transform.Find("img_selected").gameObject;
            this.m_LastImageSelected.SetActive(true);
        }
        this.SetupInfo();
    }

    public void SetupInfo()
    {
        this.SetupInfoPanel();
        if (this.mSelHeroId != 0)
        {
            this.SetupSkillPanel();
        }
    }

    private int GetSelfHeroLevel(int heroId)
    {
        LuaTable heroDataByHeroId = this.mController.GetHeroDataByHeroId(heroId);
        if (heroDataByHeroId != null)
        {
            return heroDataByHeroId.GetField_Int("level");
        }
        return 0;
    }

    private void SetUpFetterIncreaseTipsData(int heroID)
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("fetters_config");
        int selfHeroLevel = this.GetSelfHeroLevel(heroID);
        List<int> list = new List<int>();
        List<int> list2 = new List<int>();
        int num = 0;
        int num2 = 0;
        if (configTableList != null && configTableList.Count > 0)
        {
            Transform transform = this.transPanelTipFetters.Find("Panel");
            Transform transform2 = this.transPanelTipFetters.Find("Panel/item");
            int num3 = Mathf.Max(configTableList.Count, transform.childCount);
            for (int i = 0; i < num3; i++)
            {
                Transform transform3;
                if (i < transform.childCount)
                {
                    transform3 = transform.GetChild(i);
                }
                else
                {
                    transform3 = UnityEngine.Object.Instantiate<GameObject>(transform2.gameObject).transform;
                    transform3.SetParent(transform);
                    transform3.name = transform2.name;
                    transform3.localScale = transform2.localScale;
                }
                transform3.gameObject.SetActive(true);
                if (i < configTableList.Count)
                {
                    LuaTable luaTable = configTableList[i];
                    string cacheField_String = luaTable.GetCacheField_String("valuetype");
                    transform3.Find("level").GetComponent<Text>().text = cacheField_String;
                    string[] array = cacheField_String.Split(new char[]
                    {
                        '-'
                    });
                    int num4 = int.Parse(array[0]);
                    int num5 = int.Parse(array[1]);
                    bool flag = selfHeroLevel >= num4 && selfHeroLevel <= num5;
                    if (flag)
                    {
                        num = num4;
                        num2 = num5;
                    }
                    string cacheField_String2 = luaTable.GetCacheField_String("addvalue");
                    if (!string.IsNullOrEmpty(cacheField_String2))
                    {
                        string[] array2 = cacheField_String2.Split(new char[]
                        {
                            '|'
                        });
                        for (int j = 0; j < array2.Length; j++)
                        {
                            int item = this.SetIncreaseNumber(transform3.Find("num" + (j + 1)), array2[j]);
                            if (flag)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    else
                    {
                        transform3.gameObject.SetActive(false);
                    }
                }
                else
                {
                    transform3.gameObject.SetActive(false);
                }
            }
            if (this.mHeroConfigDic.ContainsKey(heroID))
            {
                LuaTable luaTable2 = this.mHeroConfigDic[heroID];
                string cacheField_String3 = luaTable2.GetCacheField_String("fetters");
                Transform transform4 = this.transPanelTipFetters.Find("Panel_hero_icon");
                string[] array3 = cacheField_String3.Split(new char[]
                {
                    '|'
                });
                int num6 = array3.Length;
                for (int k = 0; k < num6; k++)
                {
                    Transform child = transform4.GetChild(k);
                    try
                    {
                        int num7 = int.Parse(array3[k]);
                        this.SetHeroIcon(num7, child.gameObject);
                        Transform transform5 = child.Find("img_grey");
                        list2.Add(num7);
                        int selfHeroLevel2 = this.GetSelfHeroLevel(num7);
                        transform5.gameObject.SetActive(selfHeroLevel2 == 0);
                    } catch (Exception)
                    {

                    }
                }
            }
        }
        Transform transform6 = this.transBtnFetters.Find("txt_increase_value");
        if (transform6)
        {
            Text component = transform6.GetComponent<Text>();
            if (component)
            {
                component.text = 0 + "%";
                if (list.Count > 0 && list2.Count > 0 && num > 0 && num2 > 0)
                {
                    int num8 = 0;
                    int num9 = 0;
                    for (int l = 0; l < list2.Count; l++)
                    {
                        int selfHeroLevel3 = this.GetSelfHeroLevel(list2[l]);
                        if (selfHeroLevel3 >= num && selfHeroLevel3 <= num2)
                        {
                            num8++;
                        }
                    }
                    if (num8 > 0)
                    {
                        num9 = list[num8 - 1];
                    }
                    component.text = num9 + "%";
                }
            }
        }
    }

    private int SetIncreaseNumber(Transform text, string str)
    {
        if (text && !string.IsNullOrEmpty(str))
        {
            string[] array = str.Split(new char[]
            {
                ','
            });
            if (array.Length > 1)
            {
                int num = (int.Parse(array[1]) - 10000) / 100;
                text.GetComponent<Text>().text = num + "%";
                return num;
            }
        }
        return 0;
    }

    private void SetupInfoPanel()
    {
        bool flag = false;
        foreach (GameObject gameObject in this.heroHandbookObjDic.Values)
        {
            int num = int.Parse(gameObject.name);
            if (num == this.mController.mMainHeroId)
            {
                flag = true;
            }
            gameObject.transform.Find("img_set").gameObject.SetActive(num == this.mController.mMainHeroId);
            LuaTable heroDataByHeroId = this.mController.GetHeroDataByHeroId(num);
            gameObject.transform.Find("img_grey").gameObject.SetActive(heroDataByHeroId == null);
            gameObject.transform.Find("txt_lv").gameObject.SetActive(heroDataByHeroId != null);
            if (heroDataByHeroId != null)
            {
                gameObject.transform.Find("txt_lv").GetComponent<Text>().text = heroDataByHeroId.GetField_Int("level").ToString() + "级";
            }
        }
        if (this.mSelHeroId == 0 && flag)
        {
            this.mSelHeroId = this.mController.mMainHeroId;
            this.m_LastImageSelected = this.heroHandbookObjDic[this.mSelHeroId].transform.Find("img_selected").gameObject;
            this.m_LastImageSelected.SetActive(true);
        }
        if (this.mSelHeroId == 0)
        {
            this.transHeroNamePanel.gameObject.SetActive(false);
            this.transExpPanel.gameObject.SetActive(false);
            this.transBottomPanel.gameObject.SetActive(false);
            this.tgl_online.gameObject.SetActive(false);
            this.btn_break.gameObject.SetActive(false);
            this.btn_sure.gameObject.SetActive(false);
            this.obj_img_icon.gameObject.SetActive(false);
            this.transFightValuePanel.gameObject.SetActive(false);
        }
        else
        {
            this.transHeroNamePanel.gameObject.SetActive(true);
            this.transExpPanel.gameObject.SetActive(true);
            this.transBottomPanel.gameObject.SetActive(true);
            this.tgl_online.gameObject.SetActive(true);
            this.btn_sure.gameObject.SetActive(true);
            this.obj_img_icon.gameObject.SetActive(true);
            this.transFightValuePanel.gameObject.SetActive(true);
            LuaTable luaTable = this.mHeroConfigDic[this.mSelHeroId];
            this.lbl_heroname.text = luaTable.GetField_String("heroname");
            this.lbl_herodesc.text = luaTable.GetField_String("desc");
            this.ltData = this.mController.GetHeroDataByHeroId(this.mSelHeroId);
            this.txtLock.text = luaTable.GetField_String("getherodesc");
            this.objLock.SetActive(this.ltData == null);
            if (this.ltData == null)
            {
                this.lbl_level.text = "1";
                this.slider_exp.size = 0f;
                this.tgl_online.gameObject.SetActive(false);
                this.btn_sure.gameObject.SetActive(false);
                this.transFightValuePanel.gameObject.SetActive(false);
            }
            else
            {
                this.lbl_level.text = this.ltData.GetField_Int("level").ToString();
                UIManager.Instance.ClearListChildrens(this.transFightValueItem.parent, 1);
                string text = this.ltData.GetField_Int("score").ToString();
                for (int i = 0; i < text.Length; i++)
                {
                    string str = text[i].ToString();
                    GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.transFightValueItem.gameObject);
                    ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("other", "ce" + str, delegate (Sprite sprite)
                    {
                        obj.GetComponent<Image>().sprite = sprite;
                        obj.GetComponent<Image>().overrideSprite = sprite;
                    });
                    obj.transform.SetParent(this.transFightValueItem.parent);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.SetAsLastSibling();
                    obj.SetActive(true);
                }
                MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
                uint curLevelAllExp = controller.GetCurLevelAllExp((uint)this.ltData.GetField_Int("level"));
                if (curLevelAllExp != 0U)
                {
                    this.slider_exp.size = (float)this.ltData.GetField_Int("exp") * 1f / curLevelAllExp;
                }
                this.tgl_online.isOn = false;
                this.tgl_online.interactable = true;
                if (this.mSelHeroId == this.mController.mMainHeroId)
                {
                    if ((long)this.mController.mMainHeroId != (long)((ulong)MainPlayer.Self.MainPlayeData.Owner.BaseData.GetBaseOrHeroId()))
                    {
                        this.tgl_online.isOn = true;
                        this.tgl_online.interactable = false;
                    }
                    else
                    {
                        this.tgl_online.gameObject.SetActive(false);
                        this.btn_sure.gameObject.SetActive(false);
                    }
                }
                else if ((long)this.mController.mMainHeroId == (long)((ulong)MainPlayer.Self.MainPlayeData.Owner.BaseData.GetBaseOrHeroId()))
                {
                    this.tgl_online.isOn = true;
                    this.tgl_online.interactable = false;
                }
            }
            if (this.mCurModelHeroId != this.mSelHeroId)
            {
                this.mCurModelHeroId = this.mSelHeroId;
                if (this.ltData != null)
                {
                    uint field_Int = (uint)this.ltData.GetField_Int("haircolor");
                    uint field_Int2 = (uint)this.ltData.GetField_Int("hairstyle");
                    uint field_Int3 = (uint)this.ltData.GetField_Int("facestyle");
                    uint field_Int4 = (uint)this.ltData.GetField_Int("bodystyle");
                    uint field_Int5 = (uint)this.ltData.GetField_Int("antenna");
                    uint field_Int6 = (uint)this.ltData.GetField_Int("avatarid");
                    uint[] featureIDs = new uint[]
                    {
                        field_Int,
                        field_Int2,
                        field_Int3,
                        field_Int5,
                        field_Int6
                    };
                    GlobalRegister.ShowPlayerRTT(this.obj_img_icon, (uint)this.mSelHeroId, field_Int4, featureIDs, 3, new Action<GameObject>(this.InitScale));
                }
                else
                {
                    GlobalRegister.ShowNpcOrPlayerRTT(this.obj_img_icon, (uint)this.mSelHeroId, 3, new Action<GameObject>(this.InitScale));
                }
            }
        }
    }

    private void InitScale(GameObject obj)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)((long)this.mCurModelHeroId));
        if (configTable != null)
        {
            string field_String = configTable.GetField_String("uimodelsize");
            float num = 1f;
            float.TryParse(field_String, out num);
            num = ((num <= 0f) ? 1f : num);
            obj.transform.localScale = Vector3.one * num;
        }
    }

    private void obj_img_icon_on_drag(PointerEventData eventData)
    {
        IconRenderCtrl component = this.obj_img_icon.GetComponent<IconRenderCtrl>();
        if (component != null && component.target != null)
        {
            component.target.transform.eulerAngles += new Vector3(0f, -eventData.delta.x, 0f);
        }
    }

    private void SetupSkillPanel()
    {
        if (!this.mSkillShowConfigDic.ContainsKey(this.mSelHeroId))
        {
            for (int i = 0; i < this.image_skills.Count; i++)
            {
                this.image_skills[i].gameObject.SetActive(false);
            }
            return;
        }
        string field_String = this.mSkillShowConfigDic[this.mSelHeroId].GetField_String("coreskill");
        this.coreskillarr = field_String.Split(new string[]
        {
            "|"
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int j = 0; j < this.image_skills.Count; j++)
        {
            if (j < this.coreskillarr.Length)
            {
                Image imgIcon = this.image_skills[j];
                imgIcon.sprite = null;
                imgIcon.overrideSprite = null;
                uint skillid = uint.Parse(this.coreskillarr[j]);
                LuaTable luaTable = MainPlayerSkillHolder.Instance.Getskill_lv_config(skillid, 1U);
                if (luaTable != null)
                {
                    string field_String2 = luaTable.GetField_String("skillicon");
                    ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, field_String2, delegate (UITextureAsset asset)
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
                }
            }
            this.image_skills[j].gameObject.SetActive(j < this.coreskillarr.Length);
        }
    }

    private void SetupTuPoPanel()
    {
        UIManager.Instance.ClearListChildrens(this.obj_tupo_item.transform.parent, 1);
        string[] array = this.mHeroConfigDic[this.mSelHeroId].GetField_String("evolution").Split(new string[]
        {
            "|"
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            int key = int.Parse(array[i]);
            if (this.mEvolutionConfigDic.ContainsKey(key))
            {
                LuaTable lt = this.mEvolutionConfigDic[key];
                this.CreateTupoItem(lt);
            }
        }
    }

    private GameObject CreateTupoItem(LuaTable lt)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_tupo_item);
        gameObject.transform.SetParent(this.obj_tupo_item.transform.parent, false);
        gameObject.transform.SetAsLastSibling();
        gameObject.name = lt.GetField_Int("id").ToString();
        gameObject.SetActive(true);
        Image image = null;
        Text text = null;
        Text text2 = null;
        Transform transform = null;
        bool active = false;
        base.GetSprite(ImageType.ROLES, lt.GetField_String("icon"), delegate (Sprite item)
        {
            if (item != null)
            {
                image.overrideSprite = item;
                image.sprite = item;
                image.material = null;
            }
        });
        text.text = lt.GetField_String("name");
        text2.text = lt.GetField_String("desc");
        transform.gameObject.SetActive(active);
        return gameObject;
    }

    private void btn_sure_on_click(PointerEventData eventData)
    {
        LuaTable heroDataByHeroId = this.mController.GetHeroDataByHeroId(this.mSelHeroId);
        if (heroDataByHeroId != null)
        {
            ulong thisid = ulong.Parse(heroDataByHeroId.GetField_String("thisid"));
            this.mController.SetMainHero(thisid, this.IsOnLine());
        }
    }

    public bool IsOnLine()
    {
        return this.tgl_online.isOn;
    }

    private void btn_close_on_click(PointerEventData eventData)
    {
        UIManager.Instance.DeleteUI<UI_HeroHandbook>();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private Transform transRoot;

    private Text lbl_heroname;

    private Text lbl_herodesc;

    private Text lbl_level;

    private Scrollbar slider_exp;

    private RawImage obj_img_icon;

    private Slider slider_dutiao;

    private GameObject objLock;

    private Text txtLock;

    private Toggle tgl_online;

    private GameObject obj_handbook_item;

    private GameObject obj_tupo_item;

    private GameObject btn_sure;

    private GameObject btn_break;

    private GameObject btn_close;

    private List<Image> image_skills;

    private RectTransform trans_Mask;

    private int BASE_HEIGHT = 516;

    private GameObject m_LastImageSelected;

    private Transform transHeroNamePanel;

    private Transform transExpPanel;

    private Transform transBottomPanel;

    private Transform transFightValuePanel;

    private Transform transFightValueItem;

    private Transform transBtnFetters;

    private Transform transPanelTipFetters;

    private LuaTable ltData;

    private string[] coreskillarr;

    private HeroHandbookController mController;

    public int mSelHeroId;

    private Dictionary<int, GameObject> heroHandbookObjDic;

    private Dictionary<int, LuaTable> mHeroConfigDic;

    private Dictionary<int, LuaTable> mNpcConfigDic;

    private Dictionary<int, LuaTable> mEvolutionConfigDic;

    private Dictionary<int, LuaTable> mSkillShowConfigDic;

    private int mCurModelHeroId;
}
