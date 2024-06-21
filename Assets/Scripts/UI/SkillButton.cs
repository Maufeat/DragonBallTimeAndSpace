using System;
using System.Collections.Generic;
using Engine;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int skillId
    {
        get
        {
            return this._skillId;
        }
        set
        {
            this._skillId = value;
        }
    }

    private Vector3 WordInitPos
    {
        get
        {
            return base.transform.parent.localToWorldMatrix.MultiplyPoint(this.LocInitPos);
        }
    }

    private UI_ShortcutControl usc
    {
        get
        {
            if (this.usc_ == null)
            {
                this.usc_ = base.gameObject.GetComponentInParent<UI_ShortcutControl>();
            }
            return this.usc_;
        }
    }

    public uint IMaxStorageTimes
    {
        get
        {
            if (this.SkillData == null)
            {
                return 0U;
            }
            return this.SkillData.IMaxStorageTimes;
        }
    }

    public uint ICurStorageTimes
    {
        get
        {
            if (this.SkillData == null)
            {
                return 0U;
            }
            return this.SkillData.IStorageTimes;
        }
    }

    public bool IsStorageSkill
    {
        get
        {
            return this.SkillData != null && this.SkillData.IsStorageType;
        }
    }

    public MainPlayerSkillBase SkillData
    {
        get
        {
            if (this.skillId < 0)
            {
                return null;
            }
            if (MainPlayer.Self == null)
            {
                return null;
            }
            MainPlayerSkillHolder component = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
            if (component != null)
            {
                BetterDictionary<uint, MainPlayerSkillBase> mainPlayerSkillList = component.MainPlayerSkillList;
                BetterDictionary<uint, uint> mainPlayerEquipSkill = component.mainPlayerEquipSkill;
                if (mainPlayerEquipSkill.Count == 0)
                {
                    return null;
                }
                if (mainPlayerEquipSkill.ContainsValue((uint)this.skillId))
                {
                    return (!mainPlayerSkillList.ContainsKey((uint)this.skillId)) ? null : mainPlayerSkillList[(uint)this.skillId];
                }
            }
            return null;
        }
    }

    private void Start()
    {
        this.BindShortcutEvent();
    }

    private void Update()
    {
        if (null != EventSystem.current.currentSelectedGameObject && null != EventSystem.current.currentSelectedGameObject.GetComponent<InputField>())
        {
            return;
        }
        if (this._lastClickEffect != null)
        {
            this.lastClickTime += Time.deltaTime;
            if (this.lastClickTime > 1f)
            {
                ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("ui_chooseskill");
                if (effobj != null)
                {
                    effobj.MakeItemBackToPool(this._lastClickEffect);
                    this._lastClickEffect = null;
                }
            }
        }
        if (this.isDown)
        {
            this.OnDrag_PC();
            if (Input.GetMouseButtonDown(0))
            {
                this.isDown = false;
                this.OnEndDrag_PC(false);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                this.isDown = false;
                this.OnEndDrag_PC(true);
            }
        }
        if (this.isInDraging)
        {
            if (this.dragTem != null)
            {
                Vector2 zero = Vector2.zero;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.uiCam.transform.parent as RectTransform, Input.mousePosition, this.uiCam, out zero))
                {
                    (this.dragTem.transform as RectTransform).anchoredPosition = zero;
                }
            }
            if (this.uiClickDelayFram > 1)
            {
                this.uiClickDelayFram--;
                return;
            }
            if (Input.GetMouseButtonUp(0))
            {
                this.OnEndDragForAdjustIndex(null);
            }
        }
    }

    public void SetAllSkillButton(ref List<SkillButton> sbDataButtons)
    {
        this.allSkillButton = sbDataButtons;
    }

    public void BindShortcutEvent()
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        ShortcutkeyFunctionType functionType = this.BtnIndexId + (ShortcutkeyFunctionType)500;
        ShortcutsConfigController controller = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        if (controller != null)
        {
            controller.BindEventToFunctionType(functionType, new Action(this.OnShortcut));
        }
    }

    private void OnShortcut()
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        if (this.isInDraging)
        {
            return;
        }
        this.TriggerSkillOrEnterSelectState();
    }

    private UI_MainView umv
    {
        get
        {
            if (this.umv_ == null)
            {
                this.umv_ = UIManager.GetUIObject<UI_MainView>();
            }
            return this.umv_;
        }
    }

    public void CancelOtherButtonCheckState(bool isAll = false)
    {
        if (this.umv == null)
        {
            return;
        }
        for (int i = 0; i < this.umv.SkillButtonList.Count; i++)
        {
            if (isAll)
            {
                this.umv.SkillButtonList[i].isDown = false;
                this.umv.SkillButtonList[i].OnEndDrag_PC(true);
            }
            else if (this.umv.SkillButtonList[i] != this)
            {
                this.umv.SkillButtonList[i].isDown = false;
                this.umv.SkillButtonList[i].OnEndDrag_PC(true);
            }
        }
    }

    public void TriggerSkillOrEnterSelectState()
    {
        if (MainPlayer.Self.IsSoul || this.SkillData == null)
        {
            return;
        }
        if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.RotateCamera || this.SkillData.mSightType == MainPlayerSkillBase.SightType.Click)
        {
            this.OnEndDrag_PC(false);
            this.CancelOtherButtonCheckState(false);
            return;
        }
        if (UIBagManager.Instance.IsInSightingStateByObjectSkill())
        {
            UIBagManager.Instance.SetSelectPosType(SummonNpcState.None);
            SkillSelectRangeEffect component = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
            if (component != null)
            {
                component.HidleALL();
            }
        }
        MainPlayerSkillHolder component2 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
        component2.OnChantStage = true;
        this.isDown = true;
        this.CancelOtherButtonCheckState(false);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < this.usedTextureAssets.Count; i++)
        {
            this.usedTextureAssets[i].TryUnload();
        }
        this.usedTextureAssets.Clear();
        ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("ui_fangkuang_big");
        if (effobj != null)
        {
            effobj.MakeItemBackToPool(this._lastShowEffect);
            this._lastShowEffect = null;
        }
        ObjectPool<EffectObjInPool> effobj2 = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("ui_chooseskill");
        if (effobj2 != null)
        {
            effobj2.MakeItemBackToPool(this._lastClickEffect);
            this._lastClickEffect = null;
        }
    }

    public void Init()
    {
        if (this._isInit)
        {
            return;
        }
        this._isInit = true;
        this.fakeRectTransform = base.GetComponent<RectTransform>();
        this.uiCam = base.transform.root.GetComponent<Canvas>().worldCamera;
        this.InitRealSprite();
        this.LocInitPos = base.transform.localPosition;
        this.InitEvent();
        this.BtnImage = base.GetComponent<RawImage>();
        this._button = base.GetComponent<Button>();
        this._TxtCD = base.transform.Find("txt_cd").gameObject.GetComponent<Text>();
        this._ImgCD = base.transform.Find("img_cd").gameObject.GetComponent<Image>();
        if (this._TxtNum == null)
        {
            this._TxtNum = base.transform.Find("txt_num").gameObject.GetComponent<Text>();
            this._TxtNum.gameObject.SetActive(true);
            this._TxtNum.text = string.Empty;
        }
        this._TxtShortcut = base.transform.Find("txt_ktip").gameObject.GetComponent<Text>();
        this.SetShortcutShowName();
        this.InitEffectComps();
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null && MainPlayer.Self != null)
        {
            manager.DoEentityActionOrCache(MainPlayer.Self.EID, delegate (CharactorBase basePlayer)
            {
                this.TryUpdateButtonColor(MainPlayer.Self.CurMP);
            });
        }
    }

    public void SetShortcutShowName()
    {
        ShortcutkeyFunctionType sft = this.BtnIndexId + (ShortcutkeyFunctionType)500;
        this._TxtShortcut.text = ControllerManager.Instance.GetController<ShortcutsConfigController>().GetKeyNameForItemByFunctionType(sft);
    }

    public void SetSkillCdUIState()
    {
        bool flag = this.SkillData != null;
        this._ImgCD.gameObject.SetActive(false);
        this._TxtCD.gameObject.SetActive(false);
        this._TxtNum.text = string.Empty;
        this._isInitEffect = false;
        this.InitEffectComps();
    }

    public void InitEvent()
    {
        this.RealSprite = base.transform.parent.Find("skill_click_area_" + this.BtnIndexId).gameObject;
        UIEventListener.Get(this.RealSprite).onDown = new UIEventListener.VoidDelegate(this.OnSkillClickDown);
        UIEventListener.Get(this.RealSprite).onUp = new UIEventListener.VoidDelegate(this.OnSkillClickUp);
        UIEventListener.Get(this.RealSprite).onBeginDrag = new UIEventListener.VoidDelegate(this.OnBeginDragForAdjustIndex);
        UIEventListener.Get(this.RealSprite).onEnter = new UIEventListener.VoidDelegate(this.skill_button_enter);
        UIEventListener.Get(this.RealSprite).onDestroy = new UIEventListener.VoidDelegate(this.skill_button_exit);
        UIEventListener.Get(this.RealSprite).onExit = new UIEventListener.VoidDelegate(this.skill_button_exit);
    }

    private void skill_button_enter(PointerEventData eventData)
    {
        if (this.SkillData == null)
        {
            return;
        }
        Scheduler.Instance.AddTimer(0.5f, false, new Scheduler.OnScheduler(this.TryShowSkillTip));
    }

    private void TryShowSkillTip()
    {
        if (this.SkillData != null)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenSkillPanel(this.SkillData, this.RealSprite);
        }
    }

    private void skill_button_exit(PointerEventData eventData)
    {
    }

    public int LoadTexture()
    {
        this.BtnImage = base.GetComponent<RawImage>();
        if (this.SkillData == null)
        {
            Color black = Color.black;
            black.a = 0f;
            this.BtnImage.color = black;
            this.BtnImage.texture = null;
            base.transform.Find("img_tx2").gameObject.SetActive(false);
            return 0;
        }
        this._skillReleaseType = (SkillRelaseType)this.SkillData.SkillConfig.GetField_Int("releasetype");
        string Imagename = this.SkillData.GetCurSkillIconName();
        if (this.BtnImage.texture == null || this.BtnImage.texture.name != Imagename)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, Imagename, delegate (UITextureAsset item)
            {
                if (this.BtnImage == null)
                {
                    return;
                }
                if (item != null && item.textureObj != null)
                {
                    if (this.SkillData.SkillConfig.GetField_String("skillicon") != Imagename)
                    {
                        return;
                    }
                    Sprite sprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                    this.BtnImage.texture = sprite.texture;
                    this.BtnImage.color = Color.white;
                    if (this.RealSprite != null)
                    {
                        Texture2D texture2D = this.BtnImage.texture as Texture2D;
                        this.RealSprite.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                        this.usedTextureAssets.Add(item);
                    }
                }
                else
                {
                    this.BtnImage.texture = null;
                    if (this.RealSprite != null)
                    {
                        this.RealSprite.GetComponent<Image>().sprite = null;
                    }
                }
            });
        }
        if (this._TxtNum == null)
        {
            this._TxtNum = base.transform.Find("txt_num").gameObject.GetComponent<Text>();
            this._TxtNum.gameObject.SetActive(true);
            this._TxtNum.text = string.Empty;
        }
        this.UpdateStorageNum();
        return 1;
    }

    private void InitRealSprite()
    {
        if (this.RealSprite != null)
        {
            return;
        }
        if (base.gameObject.transform.parent.Find("skill_click_area_" + this.BtnIndexId))
        {
            return;
        }
        this.RealSprite = new GameObject("skill_click_area_" + this.BtnIndexId);
        this.RealSprite.transform.SetParent(base.transform.parent);
        this.RealSprite.transform.localPosition = base.transform.localPosition;
        this.RealSprite.transform.localEulerAngles = base.transform.localEulerAngles;
        this.RealSprite.transform.localScale = base.transform.localScale;
        Image image = this.RealSprite.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);
        this.realRectTransform = this.RealSprite.GetComponent<RectTransform>();
        RectTransform component = base.GetComponent<RectTransform>();
        this.realRectTransform.pivot = component.pivot;
        this.realRectTransform.localPosition = this.fakeRectTransform.localPosition;
        this.realRectTransform.localScale = this.fakeRectTransform.localScale;
        this.realRectTransform.sizeDelta = this.fakeRectTransform.sizeDelta;
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.RealSprite);
        gameObject.GetComponent<Image>().sprite = null;
        gameObject.GetComponent<Image>().overrideSprite = null;
        gameObject.GetComponent<Image>().raycastTarget = false;
        gameObject.name = "DeadMask";
        gameObject.transform.SetParent(this.RealSprite.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        base.GetComponent<Button>().targetGraphic = image;
    }

    public void ShowCurrEvoution(bool selected)
    {
        if (selected)
        {
            if (this._lastShowEffect != null || this.es == SkillButton.EffectState.Effect_loading)
            {
                return;
            }
            this.es = SkillButton.EffectState.Effect_loading;
            ManagerCenter.Instance.GetManager<FFEffectManager>().LoadUIEffobj("ui_fangkuang_big", delegate
            {
                ObjectPool<EffectObjInPool> effobj2 = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("ui_fangkuang_big");
                if (effobj2 != null)
                {
                    if (this.es == SkillButton.EffectState.Effect_remove)
                    {
                        return;
                    }
                    effobj2.GetItemFromPool(delegate (EffectObjInPool OIP)
                    {
                        this._lastShowEffect = OIP;
                        this.es = SkillButton.EffectState.Effect_finish;
                        this._lastShowEffect.ItemObj.SetActive(true);
                        this._lastShowEffect.ItemObj.name = "ui_fangkuang_big";
                        this._lastShowEffect.ItemObj.SetLayer(Const.Layer.UI, true);
                        this._lastShowEffect.ItemObj.transform.SetParent(base.transform);
                        this._lastShowEffect.ItemObj.transform.localPosition = Vector3.zero;
                        this._lastShowEffect.ItemObj.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        this._lastShowEffect.ItemObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
                        this._lastShowEffect.ItemObj.transform.localScale = new Vector3(40f, 40f);
                        ManagerCenter.Instance.GetManager<FFEffectManager>().OnEffectUserd(this._lastShowEffect.ItemObj, "ui_fangkuang_big");
                    });
                }
            });
        }
        else if (this._lastShowEffect != null)
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("ui_fangkuang_big");
            if (effobj != null)
            {
                effobj.MakeItemBackToPool(this._lastShowEffect);
                this._lastShowEffect = null;
            }
            this.es = SkillButton.EffectState.Effect_none;
        }
        else if (this.es == SkillButton.EffectState.Effect_loading)
        {
            this.es = SkillButton.EffectState.Effect_remove;
        }
    }

    public void ShowClickEffect()
    {
        if (this._lastClickEffect != null)
        {
            return;
        }
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadUIEffobj("ui_chooseskill", delegate
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj("ui_chooseskill");
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    this._lastClickEffect = OIP;
                    this._lastClickEffect.ItemObj.SetActive(true);
                    this._lastClickEffect.ItemObj.name = "ui_chooseskill";
                    this._lastClickEffect.ItemObj.SetLayer(Const.Layer.UI, true);
                    this._lastClickEffect.ItemObj.transform.SetParent(base.transform);
                    this._lastClickEffect.ItemObj.transform.localPosition = Vector3.zero;
                    this._lastClickEffect.ItemObj.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    this._lastClickEffect.ItemObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
                    RectTransform component = base.transform.GetComponent<RectTransform>();
                    this._lastClickEffect.ItemObj.transform.localScale = new Vector3(component.sizeDelta.x + 5f, component.sizeDelta.y + 5f);
                    this.lastClickTime = 0f;
                    ManagerCenter.Instance.GetManager<FFEffectManager>().OnEffectUserd(this._lastClickEffect.ItemObj, "ui_chooseskill");
                });
            }
        });
    }

    private void OnSkillClickUp(PointerEventData ped)
    {
        this.isPressDown = false;
        this.pressTimer = 0f;
        if (this.uiClickDelayFram > 0)
        {
            this.uiClickDelayFram = 0;
            return;
        }
        if (this.dragTem != null)
        {
            UnityEngine.Object.DestroyImmediate(this.dragTem);
        }
        this.TriggerSkillOrEnterSelectState();
    }

    private void OnSkillClickDown(PointerEventData ped)
    {
        this.ShowClickEffect();
        this.isPressDown = true;
        this.pressTimer = 0f;
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        this.BtnImage.color = this._button.colors.normalColor;
        this._button.transform.localScale = Vector3.one;
        if (this.SkillData == null)
        {
            return;
        }
        if (!this.InOnSelfBtn(eventData.position) && this.SkillData.mSightType == MainPlayerSkillBase.SightType.Click)
        {
            return;
        }
        if (!this.IsOnCanelBtn(eventData.position) && (this.SkillData.CurrState == MainPlayerSkillBase.state.Standby || this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") == 0U || this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") == 1U || this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") == 4U || this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") == 5U))
        {
            Vector3 drag = this.Getdir();
            this.ProcessDrag(drag, true, this.MaxMoveDis < 0.1f);
        }
        if (eventData != null)
        {
            eventData.pointerDrag = null;
            eventData.pointerPress = null;
        }
        this.realRectTransform.localPosition = this.LocInitPos;
        this.fakeRectTransform.localPosition = this.LocInitPos;
        this.FirstPress = Vector3.zero;
        SkillSelectRangeEffect component = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
        if (component != null)
        {
            component.HidleALL();
        }
        SkillButton.CanelBtn.SetActive(false);
        this.MaxMoveDis = 0f;
    }

    private void OnBeginDragForAdjustIndex(PointerEventData ped)
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        this.uiClickDelayFram = 2;
        if (this.usc && !this.usc.mIsUnlock)
        {
            return;
        }
        if (this.SkillData == null)
        {
            return;
        }
        this.isInDraging = true;
        this.CancelOtherButtonCheckState(true);
        if (this.dragTem == null)
        {
            Image component = base.transform.parent.Find("img_bg" + this.BtnIndexId).GetComponent<Image>();
            this.dragTem = UnityEngine.Object.Instantiate<GameObject>(component.gameObject);
            this.dragTem.transform.localScale = Vector3.one * 1.2f;
            if (this.SkillData != null)
            {
                Texture2D texture2D = this.BtnImage.mainTexture as Texture2D;
                this.dragTem.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
            }
            this.dragTem.GetComponent<Image>().material = null;
            this.dragTem.transform.SetParent(component.transform.root.Find("DragIcon"), false);
        }
    }

    private void OnEndDragForAdjustIndex(PointerEventData ped)
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        if (this.usc && !this.usc.mIsUnlock)
        {
            return;
        }
        this.isInDraging = false;
        if (this.dragTem != null)
        {
            UnityEngine.Object.DestroyImmediate(this.dragTem.gameObject);
        }
        this.uiClickDelayFram = 0;
        SkillButton skillButton = null;
        if (this.allSkillButton != null && this.allSkillButton.Count > 0)
        {
            for (int i = 0; i < this.allSkillButton.Count; i++)
            {
                RectTransform component = this.allSkillButton[i].RealSprite.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(component, Input.mousePosition, this.uiCam))
                {
                    skillButton = this.allSkillButton[i];
                }
            }
        }
        if (skillButton)
        {
            if (this.skillId == skillButton.skillId)
            {
                return;
            }
            if (this.SkillData == null && skillButton.SkillData == null)
            {
                return;
            }
            int skillId = this.skillId;
            int skillId2 = skillButton.skillId;
            this.skillId = skillId2;
            skillButton.skillId = skillId;
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                controller.CombinSkillIndexData();
            }
        }
    }

    public void OnEndDrag_PC(bool isCanel = false)
    {
        if (this.SkillData == null)
        {
            return;
        }
        this.SkillData.mSightState = MainPlayerSkillBase.SightState.None;
        this.BtnImage.color = this._button.colors.normalColor;
        this._button.transform.localScale = Vector3.one;
        if (this.SkillData.CurrState == MainPlayerSkillBase.state.CD)
        {
            if (this.ICurStorageTimes <= 0U)
            {
                return;
            }
        }
        else if (this.SkillData.CurrState != MainPlayerSkillBase.state.Standby)
        {
            FFBehaviourControl component = MainPlayer.Self.GetComponent<FFBehaviourControl>();
            FFBehaviourState_Skill ffbehaviourState_Skill = component.CurrState as FFBehaviourState_Skill;
            if (ffbehaviourState_Skill == null || ffbehaviourState_Skill.CurrSkillClip == null || ffbehaviourState_Skill.CurrSkillClip.mState != SkillClip.State.AfterPose || ffbehaviourState_Skill.CurrSkillClip.AnimConfig == null || !ffbehaviourState_Skill.CurrSkillClip.AnimConfig.CanCancelCloseFist)
            {
                return;
            }
        }
        if (!isCanel && (this._skillReleaseType == SkillRelaseType.Normal || this._skillReleaseType == SkillRelaseType.NormalAttack || this._skillReleaseType == SkillRelaseType.Comb || this._skillReleaseType == SkillRelaseType.ZELASCombo || this._skillReleaseType == SkillRelaseType.Chant || this._skillReleaseType == SkillRelaseType.SpellChannel || this._skillReleaseType == SkillRelaseType.SkillEvolution))
        {
            Vector3 drag = this.Getdir();
            this.ProcessDrag(drag, true, this.MaxMoveDis < 0.1f);
        }
        if (this._skillReleaseType != SkillRelaseType.SkillEvolution)
        {
            this.realRectTransform.localPosition = this.LocInitPos;
            this.fakeRectTransform.localPosition = this.LocInitPos;
        }
        this.FirstPress = Vector3.zero;
        SkillSelectRangeEffect component2 = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
        if (component2 != null)
        {
            component2.HidleALL();
        }
        SkillButton.CanelBtn.SetActive(false);
        this.MaxMoveDis = 0f;
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        if (this.SkillData == null)
        {
            return;
        }
        if (this.SkillData.CurrState != MainPlayerSkillBase.state.Standby && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 1U && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 4U && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 5U)
        {
            return;
        }
        if (this.SkillData.mSightType != MainPlayerSkillBase.SightType.Click && this.SkillData.mSightType != MainPlayerSkillBase.SightType.RotateCamera)
        {
            SkillButton.CanelBtn.SetActive(true);
        }
    }

    private void OnDrag(PointerEventData eventData)
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        this.BtnImage.color = this._button.colors.pressedColor;
        this._button.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        if (this.SkillData == null)
        {
            return;
        }
        if (this.SkillData.CurrState != MainPlayerSkillBase.state.Standby && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 0U && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 1U && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 4U && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 5U)
        {
            FFDebug.LogError(this, (SkillRelaseType)this.SkillData.SkillConfig.GetCacheField_Uint("releasetype"));
            return;
        }
        if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.Click)
        {
            return;
        }
        Vector3 vector;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.realRectTransform, eventData.position, eventData.pressEventCamera, out vector))
        {
            if (this.FirstPress == Vector3.zero)
            {
                this.FirstPress = vector;
            }
            this.realRectTransform.position = this.WordInitPos + (vector - this.FirstPress);
        }
        this.ProcessDrag(this.Getdir(), false, false);
        if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.RotateCamera)
        {
            CameraController.Self.RotateCamera(eventData.delta);
        }
        this.IsOnCanelBtn(eventData.position);
    }

    public void OnDrag_PC()
    {
        if (this._skillReleaseType == SkillRelaseType.SkillEvolution)
        {
            return;
        }
        this.BtnImage.color = this._button.colors.pressedColor;
        this._button.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        if (this.SkillData == null)
        {
            return;
        }
        if (this.SkillData.CurrState != MainPlayerSkillBase.state.Standby)
        {
            return;
        }
        if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.Click)
        {
            return;
        }
        this.ProcessDrag(this.Getdir(), false, false);
    }

    private Vector3 Getdir()
    {
        Vector3 vector = this.RealSprite.transform.position - this.WordInitPos;
        if (vector.magnitude > this.MaxMoveDis)
        {
            this.MaxMoveDis = vector.magnitude;
        }
        if (vector.magnitude <= this.Range)
        {
            base.transform.position = this.RealSprite.transform.position;
        }
        else
        {
            vector = vector.normalized * this.Range;
            base.transform.position = this.WordInitPos + vector;
        }
        return vector;
    }

    private bool IsOnCanelBtn(Vector2 pos)
    {
        SkillButton.overuiresults.Clear();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = pos;
        EventSystem.current.RaycastAll(pointerEventData, SkillButton.overuiresults);
        for (int i = 0; i < SkillButton.overuiresults.Count; i++)
        {
            if (SkillButton.overuiresults[i].gameObject == SkillButton.CanelBtn)
            {
                return true;
            }
        }
        return false;
    }

    private bool InOnSelfBtn(Vector2 pos)
    {
        bool result = false;
        SkillButton.overuiresults.Clear();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = pos;
        EventSystem.current.RaycastAll(pointerEventData, SkillButton.overuiresults);
        for (int i = 0; i < SkillButton.overuiresults.Count; i++)
        {
            if (SkillButton.overuiresults[i].gameObject == this.BtnImage.gameObject)
            {
                return true;
            }
        }
        return result;
    }

    private void ProcessDrag(Vector3 drag, bool IsEnd = false, bool IsClick = false)
    {
        if (UIManager.GetUIObject<UI_CompleteCopy>())
        {
            return;
        }
        if (MainPlayer.Self == null)
        {
            FFDebug.LogWarning(this, "MainPlayer.Self  NULL");
            return;
        }
        if (this.SkillData == null)
        {
            FFDebug.LogWarning(this, "SkillBTN  SkillConfig  NULL");
            return;
        }
        SkillSelectRangeEffect component = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
        if (component == null)
        {
            FFDebug.LogError(this, "SkillBTN  SkillSelectRangeEffect  NULL");
            return;
        }
        Vector3 pos = new Vector3(0f, -50f, 0f);
        if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.RotateCamera || this.SkillData.mSightType == MainPlayerSkillBase.SightType.Click)
        {
            if (IsClick)
            {
                this.BreakAutoAttck(this.SkillData.Skillid);
                MainPlayerSkillHolder component2 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
                if (component2.mainPlayerEquipSkill.Count > 0 && this.SkillData.Skillid == component2.mainPlayerEquipSkill[1U])
                {
                    component2.ClickNormalAttack(this.SkillData.Skillid, true);
                }
                else
                {
                    component2.ClickSkillAttack(this.SkillData.Skillid, true);
                }
            }
        }
        else if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.Sector)
        {
            pos = component.GetDireByOwner(drag / this.Range, 8f);
            try
            {
                if (pos.magnitude < 0.2f && MainPlayer.Self != null && MainPlayer.Self.ModelObj != null)
                {
                    pos = MainPlayer.Self.ModelObj.transform.forward * 0.19f;
                    this.GetDireByTargeet(this.SkillData.Sightradius, ref pos);
                }
                Vector3 position = MainPlayer.Self.ModelObj.transform.position;
                Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = MyMathf.GetRayPoint(ray, position.y, 1000f);
                rayPoint.y = position.y;
                pos = rayPoint - position;
                component.MoveSector(pos, this.SkillData.Sightradius, this.SkillData.Sightangle, this.SkillData.Sighttex1name, this.SkillData.Sighttex2name, this.SkillData.Sighttex3name);
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, "MoveSector error:" + arg);
            }
            if (IsEnd)
            {
                this.BreakAutoAttck(this.SkillData.Skillid);
                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(this.SkillData.Skillid);
            }
            this.SkillData.mSightState = ((!IsEnd) ? MainPlayerSkillBase.SightState.Begin : MainPlayerSkillBase.SightState.End);
        }
        else if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.Circle)
        {
            pos = component.GetDireByOwner(drag / this.Range, this.SkillData.Sightsize);
            try
            {
                if (pos.magnitude < 0.2f)
                {
                    pos = MainPlayer.Self.ModelObj.transform.forward * this.SkillData.Sightsize;
                    this.GetDireByTargeet(this.SkillData.Sightsize, ref pos);
                }
                Vector3 position2 = MainPlayer.Self.ModelObj.transform.position;
                Ray ray2 = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
                Vector3 vector = MyMathf.GetRayPoint(ray2, position2.y, 1000f);
                vector.y = position2.y;
                Vector3 point = vector - position2;
                if (point.magnitude > this.SkillData.Sightsize)
                {
                    vector = MyMathf.GetCircularPoint(position2, point, this.SkillData.Sightsize);
                }
                component.MoveCircle(vector, this.SkillData.Sightradius, this.SkillData.Sightsize, this.SkillData.Sighttex1name, this.SkillData.Sighttex2name);
            }
            catch (Exception arg2)
            {
                FFDebug.LogError(this, "MoveCircle error:" + arg2);
            }
            if (IsEnd)
            {
                this.BreakAutoAttck(this.SkillData.Skillid);
                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(this.SkillData.Skillid);
            }
            this.SkillData.mSightState = ((!IsEnd) ? MainPlayerSkillBase.SightState.Begin : MainPlayerSkillBase.SightState.End);
        }
        else if (this.SkillData.mSightType == MainPlayerSkillBase.SightType.Rectangle)
        {
            pos = component.GetDireByOwner(drag / this.Range, 8f);
            try
            {
                if (pos.magnitude < 0.2f && MainPlayer.Self != null && MainPlayer.Self.ModelObj != null)
                {
                    pos = MainPlayer.Self.ModelObj.transform.forward * 0.19f;
                    this.GetDireByTargeet(this.SkillData.Sightsize, ref pos);
                }
                Vector3 position3 = MainPlayer.Self.ModelObj.transform.position;
                Ray ray3 = ResolutionManager.Instance.MainCameraScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint2 = MyMathf.GetRayPoint(ray3, position3.y, 1000f);
                rayPoint2.y = position3.y;
                pos = rayPoint2 - position3;
                component.MoveRectangle(pos, this.SkillData.Sightsize, this.SkillData.Sightradius, this.SkillData.Sighttex1name);
            }
            catch (Exception arg3)
            {
                FFDebug.LogError(this, "MoveSector error:" + arg3);
            }
            if (IsEnd)
            {
                this.BreakAutoAttck(this.SkillData.Skillid);
                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(this.SkillData.Skillid);
            }
            this.SkillData.mSightState = ((!IsEnd) ? MainPlayerSkillBase.SightState.Begin : MainPlayerSkillBase.SightState.End);
        }
        this.UpdateStorageNum();
    }

    private void GetDireByTargeet(float range, ref Vector3 Dire)
    {
        if (MainPlayer.Self != null)
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component != null && component.TargetCharactor != null && component.TargetCharactor.ModelObj != null)
            {
                Vector3 vector = CommonTools.DismissYSize(component.TargetCharactor.ModelObj.transform.position);
                Vector3 vector2 = CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position);
                float num = Vector3.Distance(vector2, vector);
                if (num < range)
                {
                    Dire = vector - vector2;
                }
            }
        }
    }

    private void BreakAutoAttck(uint skillid)
    {
    }

    public void UpdateButtonState(MainPlayerSkillBase skill)
    {
        MainPlayerSkillBase.state currState = skill.CurrState;
        if (this._curState != currState)
        {
            MainPlayerSkillBase.state curState = this._curState;
            this._curState = currState;
            this.UpdateButtonColorByState(currState, curState);
        }
        this.CheckSkillIsEnableForTarget(skill);
        this.CheckInNoSkillState();
        if (this.SkillData.IsStorageType && skill.mSorageType == SkillSorageType.CDWhenRelase)
        {
            this.UpdateStorageNum();
        }
    }

    private void CheckSkillIsEnableForTarget(MainPlayerSkillBase skill)
    {
        MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        if (component == null || component.TargetCharactor == null)
        {
            this.SetImageGrey(this.BtnImage, this.bIsGrey);
            return;
        }
        if (skill.CurrState != MainPlayerSkillBase.state.CD && skill.CurrState != MainPlayerSkillBase.state.Release)
        {
            bool cacheField_Bool = this.SkillData.SkillConfig.GetCacheField_Bool("NeedTarget");
            if (cacheField_Bool)
            {
                CharactorBase target = this.SkillData.GetTarget(false, false);
                if (target == null)
                {
                    this.SetImageGrey(this.BtnImage, this.bIsGrey);
                }
                else
                {
                    float num = Vector2.Distance(target.CurrServerPos, MainPlayer.Self.CurrServerPos);
                    if (target is Npc)
                    {
                        uint baseid = (target as Npc).NpcData.MapNpcData.baseid;
                        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
                        if (configTable != null)
                        {
                            num -= configTable.GetField_Uint("volume");
                        }
                    }
                    if (this.SkillData.AttackRange >= num)
                    {
                        this.SetImageGrey(this.BtnImage, false);
                    }
                    else
                    {
                        this.SetImageGrey(this.BtnImage, true);
                    }
                }
            }
            else
            {
                this.SetImageGrey(this.BtnImage, false);
            }
        }
    }

    private void SetImageGrey(RawImage btnImage, bool bIsGrey)
    {
        UITextureMgr manager = ManagerCenter.Instance.GetManager<UITextureMgr>();
        if (ManagerCenter.Instance.GetManager<GameScene>().isAbattoirScene)
        {
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            if (!controller.CheckCanEnter((uint)this.skillId))
            {
                bIsGrey = true;
            }
        }
        manager.SetImageGrey(this.BtnImage, bIsGrey);
    }

    private void UpdateButtonColorByState(MainPlayerSkillBase.state newState, MainPlayerSkillBase.state lastState = MainPlayerSkillBase.state.Standby)
    {
        if (this.skillId == 1)
        {
            if (newState != MainPlayerSkillBase.state.CD)
            {
                this.SetImageGrey(this.BtnImage, false);
            }
            else
            {
                this.SetImageGrey(this.BtnImage, true);
            }
        }
        else if (this.skillId != 1)
        {
            switch (newState)
            {
                case MainPlayerSkillBase.state.CD:
                    this.SetImageGrey(this.BtnImage, true);
                    this.bIsGrey = true;
                    this.ShowSwitchOnEffect(false);
                    break;
                case MainPlayerSkillBase.state.Release:
                    if (this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") == 3U)
                    {
                        this.ShowSwitchOnEffect(true);
                    }
                    else if (this.IsStorageSkill)
                    {
                        if (this.SkillData.mSorageType == SkillSorageType.CDAfterRelase && this.SkillData.IStorageTimes == this.SkillData.IMaxStorageTimes)
                        {
                            this.SetImageGrey(this.BtnImage, false);
                            this.bIsGrey = true;
                        }
                        else if (!this.bInNoSkillState && this.SkillData.IStorageTimes == 0U)
                        {
                            this.SetImageGrey(this.BtnImage, true);
                            this.bIsGrey = false;
                        }
                    }
                    else if (this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 4U && this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 5U)
                    {
                        this.SetImageGrey(this.BtnImage, true);
                        this.bIsGrey = true;
                    }
                    break;
                case MainPlayerSkillBase.state.ReleaseOver:
                    if (this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") == 3U)
                    {
                        this.SetImageGrey(this.BtnImage, true);
                        this.bIsGrey = true;
                        this.ShowSwitchOnEffect(false);
                    }
                    break;
                case MainPlayerSkillBase.state.Standby:
                    if (this.SkillData.SkillConfig.GetCacheField_Uint("releasetype") != 3U)
                    {
                        this.ShowStandbyEffect(true);
                    }
                    else if (lastState == MainPlayerSkillBase.state.CD)
                    {
                        this.ShowStandbyEffect(true);
                    }
                    this.TryUpdateButtonColor(MainPlayer.Self.CurMP);
                    break;
                default:
                    this.TryUpdateButtonColor(MainPlayer.Self.CurMP);
                    break;
            }
        }
    }

    private void CheckInNoSkillState()
    {
        if (MainPlayer.Self.IsSoul)
        {
            this.SetImageGrey(this.BtnImage, true);
            return;
        }
        if (MainPlayer.Self != null)
        {
            PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
            if (component != null)
            {
                if (component.HasBeControlled(BufferState.ControlType.NoSkill))
                {
                    this.bInNoSkillState = true;
                }
                else
                {
                    if (component.HasBeControlled(BufferState.ControlType.NoLieve) && this.SkillData.SkillConfig.GetCacheField_Uint("canrelieve") == 0U)
                    {
                        return;
                    }
                    this.bInNoSkillState = false;
                }
            }
            else
            {
                this.bInNoSkillState = false;
            }
        }
        if (this.bInNoSkillState)
        {
            if (!this.bIsGrey)
            {
                this.SetImageGrey(this.BtnImage, true);
                this.bIsGrey = true;
            }
        }
        else if (this.bIsGrey)
        {
            this.UpdateButtonColorByState(this.SkillData.CurrState, MainPlayerSkillBase.state.Standby);
        }
    }

    public void UpdateButtonCD(float cdtime, float maxCDTime)
    {
        if (this._ImgCD == null)
        {
            this._ImgCD = base.transform.parent.Find("btn_skill" + this.BtnIndexId + "/img_cd").GetComponent<Image>();
        }
        uint num = (uint)(cdtime / 1000f);
        if (cdtime < 10000f && cdtime > 0f && num > 0U)
        {
            this._TxtCD.gameObject.SetActive(true);
            this._TxtCD.text = string.Format("{0}", num);
        }
        else
        {
            this._TxtCD.gameObject.SetActive(false);
        }
        if (cdtime > 0f)
        {
            this._ImgCD.gameObject.SetActive(true);
            this._ImgCD.fillAmount = cdtime / maxCDTime;
        }
        else
        {
            cdtime = 0f;
            this._ImgCD.gameObject.SetActive(false);
        }
    }

    public void TryUpdateButtonColor(uint currentMP)
    {
        if (this.skillId == 1 || this.SkillData == null)
        {
            return;
        }
        if (this.IsStorageSkill && this.ICurStorageTimes < 1U)
        {
            this.SetImageGrey(this.BtnImage, true);
            this.bIsGrey = true;
            return;
        }
        if (this._curState == MainPlayerSkillBase.state.Standby)
        {
            uint cacheField_Uint = this.SkillData.SkillConfig.GetCacheField_Uint("magiccost");
            bool flag = currentMP >= cacheField_Uint;
            if (flag && !this.bInNoSkillState)
            {
                this.SetImageGrey(this.BtnImage, false);
                this.bIsGrey = false;
            }
            else
            {
                this.SetImageGrey(this.BtnImage, true);
                this.bIsGrey = true;
            }
        }
    }

    public void UpdateStorageCD(float cdtime, float maxCDTime)
    {
        if (this.ICurStorageTimes >= this.IMaxStorageTimes)
        {
            this._ImgCD.gameObject.SetActive(false);
            this._TxtCD.text = string.Empty;
            return;
        }
        uint num = (uint)(cdtime / 1000f);
        if (cdtime < 10000f && cdtime > 0f && num > 0U)
        {
            this._TxtCD.gameObject.SetActive(true);
            this._TxtCD.text = string.Format("{0}", num);
        }
        else
        {
            this._TxtCD.gameObject.SetActive(false);
        }
        if (cdtime > 0f)
        {
            this._ImgCD.gameObject.SetActive(true);
            this._ImgCD.fillAmount = cdtime / maxCDTime;
        }
        else
        {
            cdtime = 0f;
            this._ImgCD.gameObject.SetActive(false);
            if (this.ICurStorageTimes < this.IMaxStorageTimes)
            {
                this.SkillData.UpdateStoreTimes(this.ICurStorageTimes + 1U);
                this.UpdateStorageNum();
                this.TryUpdateButtonColor(MainPlayer.Self.CurMP);
            }
        }
    }

    public bool IsStarageMax()
    {
        bool result = false;
        if (this.ICurStorageTimes == this.IMaxStorageTimes)
        {
            result = true;
        }
        return result;
    }

    public SkillSorageType mSkillStarageType()
    {
        return this.SkillData.mSorageType;
    }

    public void UpdateStorageNum()
    {
        if (this.skillId == 1 || !this.IsStorageSkill || this.SkillData == null)
        {
            return;
        }
        if (this.ICurStorageTimes > 1U)
        {
            this._TxtNum.text = string.Format("{0}", this.ICurStorageTimes);
        }
        else
        {
            this._TxtNum.text = string.Empty;
        }
        this._showStorageTimes = this.ICurStorageTimes;
        this.SkillData.CheckStorageSkillIsCD();
    }

    public void ShowStandbyEffect(bool isShow)
    {
        this.InitEffectComps();
        if (this.skillId == 1)
        {
            return;
        }
        if (isShow)
        {
            this._standByController.gameObject.SetActive(true);
            this._standByController.ShowEffect();
        }
        else
        {
            this._standByController.gameObject.SetActive(false);
        }
    }

    public void ShowSwitchOnEffect(bool isShow)
    {
        this.InitEffectComps();
        if (this.skillId == 1)
        {
            return;
        }
        if (isShow)
        {
            this._switchOnController.gameObject.SetActive(true);
            this._switchOnController.ShowEffect();
        }
        else
        {
            this._switchOnController.gameObject.SetActive(false);
        }
    }

    public void InitEffectComps()
    {
        if (this._isInitEffect)
        {
            return;
        }
        this._isInitEffect = true;
        this._objEffectSwitchOn = base.transform.Find("img_tx2").gameObject;
        if (this.skillId != 1)
        {
            this._objEffectStandBy = base.transform.Find("img_tx1").gameObject;
            if (this._standByController == null)
            {
                this._standByController = this._objEffectStandBy.AddComponent<UI_EffectComponent>();
            }
            if (this._switchOnController == null)
            {
                this._switchOnController = this._objEffectSwitchOn.AddComponent<UI_EffectComponent>();
            }
            this._standByController.InitEffect();
            this._switchOnController.InitEffect();
            this.ShowSwitchOnEffect(false);
            this.ShowStandbyEffect(false);
        }
        else
        {
            this._objEffectSwitchOn.gameObject.SetActive(true);
        }
    }

    public void TryActiceSelf(bool isActive)
    {
        if (this._isUnlocking)
        {
            return;
        }
        base.gameObject.SetActive(isActive);
    }

    public void UnlockStart()
    {
        this._isUnlocking = true;
        if (base.gameObject.activeSelf)
        {
            base.gameObject.SetActive(false);
        }
    }

    public void UnlockFinish()
    {
        this._isUnlocking = false;
        if (!base.gameObject.activeSelf)
        {
            base.gameObject.SetActive(true);
        }
    }

    public const float FDefaultCDTime = 10f;

    public const float FThousand = 1000f;

    public const int INormalAttackID = 1;

    public const int ISwithSkillType = 3;

    public GameObject RealSprite;

    private RectTransform realRectTransform;

    private RectTransform fakeRectTransform;

    public float Range = 0.25f;

    private Vector3 LocInitPos;

    private GameObject dragTem;

    private int _skillId = -1;

    public int BtnIndexId = -1;

    public static GameObject CanelBtn;

    public RawImage BtnImage;

    private Button _button;

    private Text _TxtCD;

    private Image _ImgCD;

    private Text _TxtShortcut;

    private Text _TxtNum;

    private Camera uiCam;

    public SkillRelaseType _skillReleaseType;

    private List<SkillButton> allSkillButton;

    private UI_ShortcutControl usc_;

    private GameObject _objEffectStandBy;

    private UI_EffectComponent _standByController;

    private GameObject _objEffectSwitchOn;

    private UI_EffectComponent _switchOnController;

    private bool _isInitEffect;

    private bool _isInit;

    private MainPlayerSkillBase.state _curState = MainPlayerSkillBase.state.Standby;

    private bool _isUnlocking;

    private List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();

    public bool isDown;

    public bool isPressDown;

    private float pressTimer;

    private UI_MainView umv_;

    private int uiClickDelayFram;

    private SkillButton.EffectState es;

    private EffectObjInPool _lastShowEffect;

    private EffectObjInPool _lastClickEffect;

    private float lastClickTime;

    public bool isInDraging;

    private Vector3 FirstPress = Vector3.zero;

    private float MaxMoveDis;

    private static List<RaycastResult> overuiresults = new List<RaycastResult>();

    private float RunningTime;

    private bool bInNoSkillState;

    private bool bIsGrey;

    private uint _showStorageTimes;

    private enum EffectState
    {
        Effect_none,
        Effect_loading,
        Effect_finish,
        Effect_remove
    }
}
