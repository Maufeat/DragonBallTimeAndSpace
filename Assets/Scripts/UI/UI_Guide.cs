using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Guide : UIPanelBase
{
    private GuideController guideControl
    {
        get
        {
            return ControllerManager.Instance.GetController<GuideController>();
        }
    }

    private string curHightLightObjPath
    {
        get
        {
            return this._curHightLightObjPath;
        }
        set
        {
            this._curHightLightObjPath = value;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.InitGameObject();
        this.InitEvent();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.UnInit();
        if (this.mRoot != null)
        {
            this.mRoot = null;
        }
    }

    public void UnInit()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.OnUpdate));
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.UpdateOnlyStep));
    }

    private void InitGameObject()
    {
        this.panelGuide = this.mRoot.Find("Offset_Guide/Panel_Guide").gameObject;
        this.imgArea = this.mRoot.Find("Offset_Guide/Panel_Guide/img_area").gameObject;
        this.imgArea_mask = this.mRoot.Find("Offset_Guide/Panel_Guide/img_areaMask").gameObject;
        this.img_maskevent = this.mRoot.Find("Offset_Guide/Panel_Guide/img_maskevent").gameObject;
        this.img_mask = this.mRoot.Find("Offset_Guide/Panel_Guide/img_mask");
        this.content = this.mRoot.Find("Offset_Guide/Panel_Guide/content");
        this.guideBtnParent = this.mRoot.Find("Offset_Guide/Panel_Guide");
        this.imgArrow = this.mRoot.Find("Offset_Guide/Panel_Guide/img_arrow");
        this.contentBg = this.mRoot.Find("Offset_Guide/Panel_Guide/content/img_bg");
        this.imgIcon = this.mRoot.Find("Offset_Guide/Panel_Guide/img_icon").GetComponent<RawImage>();
        this.txtGuide = this.mRoot.Find("Offset_Guide/Panel_Guide/content/img_bg/Text").GetComponent<Text>();
        this.transExplain = this.mRoot.Find("Offset_Guide/Panel_Guide/tips");
        this.transTips1 = this.mRoot.Find("Offset_Guide/Panel_Guide/tips/img_bg");
        this.transTips2 = this.mRoot.Find("Offset_Guide/Panel_Guide/tips/img_bg (1)");
        this.btnCloseExplain = this.mRoot.Find("Offset_Guide/Panel_Guide/Panel_explain/btn_close").GetComponent<Button>();
        this.btnCloseGuide = this.mRoot.Find("Offset_Guide/Panel_Guide/content/Button").GetComponent<Button>();
        this.centerImage = this.mRoot.Find("Offset_Guide/Panel_Guide/panel_tip_group/mask_center").GetComponent<Image>();
        this.topImage = this.mRoot.Find("Offset_Guide/Panel_Guide/panel_tip_group/mask_top").GetComponent<Image>();
        this.bottomImage = this.mRoot.Find("Offset_Guide/Panel_Guide/panel_tip_group/mask_bottom").GetComponent<Image>();
        this.leftImage = this.mRoot.Find("Offset_Guide/Panel_Guide/panel_tip_group/mask_left").GetComponent<Image>();
        this.rightImage = this.mRoot.Find("Offset_Guide/Panel_Guide/panel_tip_group/mask_right").GetComponent<Image>();
        this.originalMaskAlpha = this.topImage.color.a;
        this.SetMask(0f);
        this.SetGuideModel();
        foreach (object obj in this.img_mask)
        {
            Transform transform = (Transform)obj;
            transform.GetComponent<Image>().material = null;
            if (transform.name == "img_mask")
            {
                transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Shader/GuideMask");
            }
        }
    }

    public void StartGuide(uint guideId)
    {
        this._guideQueue.Clear();
        this._guideId = guideId;
        this.btnCloseGuide.gameObject.SetActive(!this.guideControl.IsCompulsoryGuide(guideId));
        if (!this.guideControl.DicGuide.ContainsKey(this._guideId))
        {
            this.EndGuide();
            return;
        }
        this._guideQueue = this.guideControl.DicGuide[this._guideId];
        this._curGuideStepNum = 0;
        this._guideStepNum = this._guideQueue.Count;
        LuaTable luaTable = this._guideQueue[this._curGuideStepNum];
        this._guideType = luaTable.GetField_Uint("triggertype");
        if (this._guideType == 7U || this._guideType == 9U)
        {
            this.imgArrow.transform.position = new Vector3(10000f, 0f, 0f);
            this.imgArea.transform.position = new Vector3(10000f, 0f, 0f);
            this.img_maskevent.transform.position = new Vector3(10000f, 0f, 0f);
            this.content.transform.position = new Vector3(10000f, 0f, 0f);
            this.imgIcon.transform.position = new Vector3(10000f, 0f, 0f);
            this.transExplain.gameObject.SetActive(true);
            this.transTips1.gameObject.SetActive(this._guideType == 7U);
            this.transTips2.gameObject.SetActive(this._guideType == 9U);
            this.Now = Time.realtimeSinceStartup;
            this._stepLastTimeEndBig = Time.realtimeSinceStartup + luaTable.GetField_Uint("lasttime");
            this._needLastTimeEndBig = (luaTable.GetField_Uint("lasttime") != 0U);
            Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.UpdateOnlyStep));
            return;
        }
        this.finishSept();
        this.timeEnableClick = false;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.OnUpdate));
        this.setRootActive(true);
        this.guideQueue();
    }

    private void guideQueue()
    {
        if (this._guideQueue.Count == 0 || this._curGuideStepNum >= this._guideStepNum)
        {
            if (this.guideState == UI_Guide.GuideState.FindHightLightGroupPos && this.animationState != UI_Guide.AnimationState.None && !this.isNeedCheckCenterImageMouseEvent)
            {
                this.animationState = UI_Guide.AnimationState.End;
            }
            else
            {
                this.EndGuide();
            }
            return;
        }
        LuaTable luaTable = this._guideQueue[this._curGuideStepNum];
        this._curGuideStepNum++;
        this.curConfig = luaTable;
        string[] array = luaTable.GetField_String("zoom").Split(new char[]
        {
            ','
        });
        if (array.Length > 1)
        {
            this._zoomMask = new Vector3(array[0].ToFloat(), array[1].ToFloat());
        }
        this.isNeedCheckCenterImageMouseEvent = false;
        this.img_mask.position = this.imgMaskInitPostion;
        this._iffreeze = (luaTable.GetField_Uint("iffreeze") == 1U);
        this.img_mask.gameObject.SetActive(this._iffreeze);
        this.img_maskevent.SetActive(this._iffreeze);
        this.curHightLightObjPath = luaTable.GetField_String("hightlightcomponent");
        this.showNpcId = luaTable.GetField_Uint("shownpc");
        this._highlighttype = luaTable.GetField_Uint("highlighttype");
        this._childIndex = luaTable.GetField_String("UInumber").Split(new char[]
        {
            ','
        });
        this._bondUIName = luaTable.GetField_String("UIbond");
        this._secondUIName = luaTable.GetField_String("UIbonus");
        this._offsetUI = this.GetTransFromString(this.curConfig.GetField_String("positionorigin"));
        this._offsetSecondUI = this.GetTransFromString(this.curConfig.GetField_String("positionbonus"));
        this.lastTime = Time.realtimeSinceStartup;
        this.Now = Time.realtimeSinceStartup;
        this._stepLastTimeEndBig = Time.realtimeSinceStartup + luaTable.GetField_Uint("lasttime");
        this._stepLastTimeEndSmall = Time.realtimeSinceStartup + luaTable.GetField_Uint("steptime");
        this._needLastTimeEndBig = (luaTable.GetField_Uint("lasttime") != 0U);
        this._needLastTimeEndSmall = (luaTable.GetField_Uint("steptime") != 0U);
        this.guideControl.Initial_UI = luaTable.GetField_String("Uiinitial");
        int field_Int = luaTable.GetField_Int("triggertype");
        int field_Int2 = luaTable.GetField_Int("highlighttype");
        if (field_Int == 5)
        {
            this.guideState = UI_Guide.GuideState.Npc;
            this.npcId = ((this.showNpcId != 0U) ? this.showNpcId : luaTable.GetField_Uint("trignpc"));
        }
        else if (field_Int == 6)
        {
            this.guideState = UI_Guide.GuideState.Quest;
            this.npcId = this.showNpcId;
        }
        else if (field_Int == 8)
        {
            this.guideState = UI_Guide.GuideState.ChangeRole;
            this.npcId = this.showNpcId;
        }
        else if (field_Int == 10)
        {
            this.guideState = UI_Guide.GuideState.BagFull;
            this.npcId = this.showNpcId;
        }
        else if (field_Int == 11)
        {
            this.guideState = UI_Guide.GuideState.GetItem;
            this.npcId = this.showNpcId;
        }
        else if (field_Int2 == 2)
        {
            string[] array2 = this.curHightLightObjPath.Split(new char[]
            {
                ','
            });
            this.curHightLightObjPos = new Vector3((float)array2[0].ToInt(), (float)array2[1].ToInt());
            this.guideState = UI_Guide.GuideState.FindHightLightPos;
            this.npcId = 0U;
        }
        else if (field_Int2 == 3)
        {
            this.guideState = UI_Guide.GuideState.BagItem;
            this.npcId = 0U;
        }
        else if (field_Int2 == 4)
        {
            this.guideState = UI_Guide.GuideState.ChildItem;
            this.npcId = 0U;
        }
        else if (field_Int2 == 5)
        {
            this.curHightLightObjPos = new Vector3(30000f, 0f);
            this.guideState = UI_Guide.GuideState.FindHightLightGroupPos;
            this.npcId = 0U;
            this.isMouseEnter = false;
            this.mouseStayTimer = 0f;
            Scheduler.Instance.AddFrame(15U, false, delegate
            {
                this.stepMouseHoverStayTime = this.curConfig.GetField_Float("steptime");
                this.animationState = UI_Guide.AnimationState.Guiding;
                if (this.stepMouseHoverStayTime > 0.1f)
                {
                    this.isNeedCheckCenterImageMouseEvent = true;
                    this.SetMask(this.originalMaskAlpha);
                }
                else if (this._curGuideStepNum == 1)
                {
                    this.animationState = UI_Guide.AnimationState.Start;
                }
                this.InitGroupMask(this.curHightLightObjPath);
            });
        }
        else
        {
            this.guideState = UI_Guide.GuideState.FindHightLightObj;
            this.npcId = 0U;
        }
        if (this.npcId != 0U)
        {
            Scheduler.Instance.AddTimer(2f, true, new Scheduler.OnScheduler(this.CheckNpcInScene));
        }
        this.stepGuideCallBack = delegate ()
        {
            this.curHightLightObj = null;
            this.bFreezetime(false);
            this.finishSept();
            this.guideQueue();
        };
        if (this.guideState == UI_Guide.GuideState.Quest && this.showNpcId == 0U)
        {
            this.HideAllContent();
            this.NextStep();
        }
        if (this.guideState == UI_Guide.GuideState.Npc && this.showNpcId == 0U)
        {
            this.HideAllContent();
            this.NextStep();
        }
        if (this.guideState == UI_Guide.GuideState.ChangeRole && this.showNpcId == 0U)
        {
            this.HideAllContent();
            this.NextStep();
        }
        if (this.guideState == UI_Guide.GuideState.BagFull && this.showNpcId == 0U)
        {
            this.HideAllContent();
            this.NextStep();
        }
        if (this.guideState == UI_Guide.GuideState.GetItem && this.showNpcId == 0U)
        {
            this.HideAllContent();
            this.NextStep();
        }
        if (this._needSkipStep)
        {
            this._needSkipStep = false;
            this.HideAllContent();
            this.NextStep();
        }
        ShortcutsConfigController controller = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        if (controller != null && this._iffreeze)
        {
            controller.ShortcutSwitch(false);
        }
        this.SetUITransAndSecondUI();
    }

    private void SetMask(float a)
    {
        Color color = this.topImage.color;
        color.a = a;
        this.topImage.color = color;
        this.bottomImage.color = color;
        this.leftImage.color = color;
        this.rightImage.color = color;
    }

    private void InitGroupMask(string pathContent)
    {
        if (this.curConfig == null)
        {
            this.EndGuide();
            return;
        }
        string[] array = pathContent.Split(new char[]
        {
            ':'
        });
        if (array.Length <= 1)
        {
            return;
        }
        Transform uitransform = UIManager.GetUITransform(this._bondUIName);
        if (!uitransform)
        {
            return;
        }
        Transform transform = uitransform.parent.Find(array[0]);
        if (!transform)
        {
            return;
        }
        List<RectTransform> list = new List<RectTransform>();
        List<string> list2 = new List<string>();
        for (int i = 1; i < array.Length; i++)
        {
            list2.Add(array[i]);
        }
        this.FindChildRectTransformsByName(transform, list2, list);
        Camera component = UIManager.Instance.GetUICamera().GetComponent<Camera>();
        RectTransform component2 = UIManager.Instance.UIRoot.GetComponent<RectTransform>();
        Vector3 a = Vector3.zero;
        Bounds bounds = default(Bounds);
        int num = 0;
        for (int j = 0; j < list.Count; j++)
        {
            Vector3[] array2 = new Vector3[4];
            list[j].GetWorldCorners(array2);
            for (int k = 0; k < array2.Length; k++)
            {
                num++;
                a += array2[k];
            }
        }
        bounds.center = a / (float)num;
        for (int l = 0; l < list.Count; l++)
        {
            Vector3[] array3 = new Vector3[4];
            list[l].GetWorldCorners(array3);
            for (int m = 0; m < array3.Length; m++)
            {
                bounds.Encapsulate(array3[m]);
            }
        }
        float x = this.centerImage.transform.localScale.x;
        Vector3 position = this.centerImage.transform.position;
        Vector2 vector = bounds.size / component2.transform.localScale.x;
        this.centerImage.rectTransform.sizeDelta = vector;
        vector *= x;
        CanvasScaler component3 = component2.GetComponent<CanvasScaler>();
        Vector3 v = component.WorldToScreenPoint(position);
        Vector2 vector2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(component2, v, component, out vector2);
        Vector3[] array4 = new Vector3[4];
        component2.GetWorldCorners(array4);
        float num2 = float.MaxValue;
        float num3 = float.MinValue;
        float num4 = float.MaxValue;
        float num5 = float.MinValue;
        for (int n = 0; n < array4.Length; n++)
        {
            num2 = Mathf.Min(num2, array4[n].x);
            num3 = Mathf.Max(num3, array4[n].x);
            num4 = Mathf.Min(num4, array4[n].y);
            num5 = Mathf.Max(num5, array4[n].y);
        }
        float num6 = (this.centerImage.transform.position.x - num2) / (num3 - num2);
        num6 = Mathf.Clamp01(num6);
        float num7 = (this.centerImage.transform.position.y - num4) / (num5 - num4);
        num7 = Mathf.Clamp01(num7);
        float num8 = 4f;
        this.topImage.transform.position = position + bounds.size.y * 0.5f * Vector3.up * x;
        this.topImage.rectTransform.pivot = new Vector2(num6, 0f);
        this.topImage.rectTransform.sizeDelta = new Vector2(component3.referenceResolution.x * num8, (component3.referenceResolution.y * (1f - num7) - vector.y * 0.5f) * num8);
        this.bottomImage.transform.position = position + bounds.size.y * 0.5f * Vector3.down * x;
        this.bottomImage.rectTransform.pivot = new Vector2(num6, 1f);
        this.bottomImage.rectTransform.sizeDelta = new Vector2(component3.referenceResolution.x * num8, (component3.referenceResolution.y * num7 - vector.y * 0.5f) * num8);
        this.leftImage.rectTransform.sizeDelta = new Vector2((component3.referenceResolution.x * num6 - vector.x * 0.5f) * num8, vector.y);
        this.leftImage.transform.position = position + bounds.size.x * 0.5f * Vector3.left * x;
        this.rightImage.rectTransform.sizeDelta = new Vector2((component3.referenceResolution.x * (1f - num6) - vector.x * 0.5f) * num8, vector.y);
        this.rightImage.transform.position = position + bounds.size.x * 0.5f * Vector3.right * x;
        this.img_maskevent.gameObject.SetActive(false);
        this.imgArea.gameObject.SetActive(false);
        this.avatarid = this.curConfig.GetField_Uint("avatarid");
        if (this.isNeedCheckCenterImageMouseEvent && this.isMouseEnter)
        {
            this.mouseStayTimer += Time.deltaTime;
            if (this.mouseStayTimer > this.stepMouseHoverStayTime && this.stepGuideCallBack != null)
            {
                this.stepGuideCallBack();
            }
        }
        this.SetTipContentPosition(bounds.center);
    }

    private void OnCenterImageMouseEnter()
    {
        this.isMouseEnter = true;
    }

    private void OnCenterImageMouseExit()
    {
    }

    private void OnCenterImageMouseStay()
    {
        this.isMouseEnter = true;
    }

    private void OnMouseClick()
    {
        this.guideQueue();
    }

    private void CheckCenterImageMouseHoverEvent()
    {
        if (this.centerImage)
        {
            Camera component = UIManager.Instance.GetUICamera().GetComponent<Camera>();
            bool flag = RectTransformUtility.RectangleContainsScreenPoint(this.centerImage.rectTransform, Input.mousePosition, component);
            if (this.isIn != flag)
            {
                if (flag)
                {
                    this.OnCenterImageMouseEnter();
                }
                else
                {
                    this.OnCenterImageMouseExit();
                }
                this.isIn = flag;
            }
            if (this.isIn)
            {
                this.OnCenterImageMouseStay();
            }
        }
    }

    private void CheckCenterImageMouseClickEvent()
    {
        if (this.centerImage)
        {
            Camera component = UIManager.Instance.GetUICamera().GetComponent<Camera>();
            bool flag = RectTransformUtility.RectangleContainsScreenPoint(this.centerImage.rectTransform, Input.mousePosition, component);
            if (flag && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
            {
                this.OnMouseClick();
            }
        }
    }

    private void FindChildRectTransformsByName(Transform node, List<string> names, List<RectTransform> list)
    {
        for (int i = 0; i < node.childCount; i++)
        {
            if (node.gameObject.activeSelf)
            {
                if (names.Contains(node.name))
                {
                    RectTransform component = node.GetComponent<RectTransform>();
                    if (component != null && !list.Contains(component))
                    {
                        list.Add(component);
                    }
                }
                Transform child = node.GetChild(i);
                if (names.Contains(child.name))
                {
                    RectTransform component2 = child.GetComponent<RectTransform>();
                    if (component2 != null && !list.Contains(component2))
                    {
                        list.Add(component2);
                    }
                }
                if (child.childCount > 0)
                {
                    this.FindChildRectTransformsByName(child, names, list);
                }
            }
        }
    }

    private void setRootActive(bool active)
    {
        this.mRoot.gameObject.SetActive(active);
    }

    private void EndGuide()
    {
        if (this.guideState == UI_Guide.GuideState.FindHightLightGroupPos && !this.isNeedCheckCenterImageMouseEvent && this.animationState == UI_Guide.AnimationState.Guiding)
        {
            this.animationState = UI_Guide.AnimationState.End;
        }
        else
        {
            this.guideControl.IsGuide = false;
            ShortcutsConfigController controller = ControllerManager.Instance.GetController<ShortcutsConfigController>();
            if (controller != null)
            {
                controller.ShortcutSwitch(true);
            }
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.OnUpdate));
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.UpdateOnlyStep));
            this.guideControl.Req_GuideOver_CS(this._guideId);
            this.finishSept();
            this.curConfig = null;
            this.bFreezetime(false);
            this.setRootActive(false);
            this.Close();
            this.guideState = UI_Guide.GuideState.None;
        }
    }

    private void finishSept()
    {
        if (this.npc != null)
        {
            this.npc.EnableGuideEffect(false, this.npcTempId);
        }
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNpcInScene));
        this.img_mask.gameObject.SetActive(false);
        this._highlighttype = 1U;
        this._iffreeze = false;
        this._useButtonEvent = false;
        this.timeEnableClick = false;
        this.stepGuideCallBack = null;
        this.Now = 0f;
        this.lastTime = 0f;
        this.curHightLightObjPath = null;
        this.npcTempId = 0UL;
        this.curHightLightObjPos = Vector3.zero;
        this._transPanelBase = null;
        this._transSecondPanelBase = null;
        this.curHightLightObj = null;
        this.OnClickCallBack = null;
        this.SkillBtnOnDragCallBack = null;
        this._zoomMask = Vector3.one;
        this._itemBaseId = 0U;
        this.clearImgareaEvent();
    }

    private void OnUpdate()
    {
        if (this.timeEnableClick)
        {
            this.Now = Time.realtimeSinceStartup;
            if (this.Now - this.lastTime >= this.viewTime)
            {
                this.doNext();
            }
        }
        switch (this.guideState)
        {
            case UI_Guide.GuideState.ViewContent:
                this.guideState = UI_Guide.GuideState.None;
                break;
            case UI_Guide.GuideState.FindHightLightObj:
            case UI_Guide.GuideState.BagItem:
            case UI_Guide.GuideState.ChildItem:
                if (!string.IsNullOrEmpty(this.curHightLightObjPath))
                {
                    if (this.guideState == UI_Guide.GuideState.FindHightLightObj)
                    {
                        this.curHightLightObj = GameObject.Find(this.curHightLightObjPath);
                    }
                    else if (this.guideState == UI_Guide.GuideState.BagItem)
                    {
                        this._itemBaseId = (uint)this.curHightLightObjPath.ToFloat();
                        Transform bagItemTransform = this.GetBagItemTransform(this._itemBaseId);
                        this.curHightLightObj = ((!(bagItemTransform == null)) ? bagItemTransform.gameObject : null);
                    }
                    else if (this.guideState == UI_Guide.GuideState.ChildItem)
                    {
                        this.curHightLightObj = this.FindHightObjectByIndex();
                    }
                    if (this.curHightLightObj != null)
                    {
                        Scheduler.Instance.AddFrame(25U, false, delegate
                        {
                            this.setSelect();
                            this.viewArrow();
                            this.viewSelect();
                            this.viewContent();
                            this.viewIcon();
                            this.curHightLightObjPath = null;
                        });
                        this.timeEnableClick = true;
                        this.guideState = UI_Guide.GuideState.ViewContent;
                        return;
                    }
                    this.Now = Time.realtimeSinceStartup;
                    if (this.Now - this.lastTime > this.maxFindTime)
                    {
                        this.EndGuide();
                    }
                }
                break;
            case UI_Guide.GuideState.FindHightLightPos:
                this.curHightLightObj = this.imgArea;
                this.setSelect();
                this.viewArrow();
                this.viewSelect();
                this.viewContent();
                this.viewIcon();
                this.guideState = UI_Guide.GuideState.ViewContent;
                this.timeEnableClick = true;
                break;
            case UI_Guide.GuideState.FindHightLightGroupPos:
                this.InitGroupMask(this.curHightLightObjPath);
                if (this.isNeedCheckCenterImageMouseEvent)
                {
                    this.CheckCenterImageMouseHoverEvent();
                }
                else
                {
                    this.CheckCenterImageMouseClickEvent();
                }
                break;
        }
    }

    private void setSelect()
    {
        this.lastTime = Time.realtimeSinceStartup;
        this.Now = Time.realtimeSinceStartup;
        this.UpdatePos();
        this.HideAllContent();
        if (this.curConfig.GetField_Bool("needfreezetime"))
        {
            this.bFreezetime(true);
        }
    }

    private void HideAllContent()
    {
        this.imgArrow.gameObject.SetActive(false);
        this.contentBg.gameObject.SetActive(false);
        this.imgArea.gameObject.SetActive(false);
        this.imgIcon.transform.localPosition = new Vector3(10000f, 10000f, 0f);
    }

    private void UpdatePos()
    {
        if (this.curConfig == null)
        {
            return;
        }
        Vector3 transFromString = this.GetTransFromString(this.curConfig.GetField_String("posdialogue"));
        Vector3 transFromString2 = this.GetTransFromString(this.curConfig.GetField_String("posarrow"));
        Vector3 transFromString3 = this.GetTransFromString(this.curConfig.GetField_String("pospainting"));
        Vector3 transFromString4 = this.GetTransFromString(this.curConfig.GetField_String("positionsquare"));
        this.imgArea_mask.SetActive(true);
        if (this.imgArea == this.curHightLightObj)
        {
            this.imgArea.transform.localPosition = this.curHightLightObjPos;
            this.imgArea_mask.transform.localPosition = this.curHightLightObjPos;
        }
        else
        {
            if (!this.curHightLightObj)
            {
                this.curHightLightObj = this.FindHightObjectByIndex();
            }
            if (this.curHightLightObj != null)
            {
                this.imgArea.transform.position = this.curHightLightObj.transform.position;
                this.imgArea_mask.transform.position = this.curHightLightObj.transform.position;
            }
            this.imgArea.transform.localPosition += transFromString4;
            this.imgArea_mask.transform.localPosition += transFromString4;
        }
        this.imgArea_mask.GetComponent<RectTransform>().sizeDelta = new Vector2(this.curConfig.GetField_Float("width"), this.curConfig.GetField_Float("hight"));
        this.imgArea.GetComponent<RectTransform>().sizeDelta = new Vector2(this.curConfig.GetField_Float("width"), this.curConfig.GetField_Float("hight"));
        this.content.localPosition = this.imgArea.transform.localPosition + transFromString;
        this.txtGuide.text = this.curConfig.GetField_String("guidetext");
        this.avatarid = this.curConfig.GetField_Uint("avatarid");
        this.imgArrow.localPosition = this.imgArea.transform.localPosition + new Vector3((float)transFromString2[0].ToInt(), (float)transFromString2[1].ToInt());
        this.imgArrow.rotation = Quaternion.Euler(new Vector3(0f, 0f, (float)transFromString2[2].ToInt()));
        this.imgIcon.transform.localPosition = this.imgArea.transform.localPosition + transFromString3;
    }

    private GameObject FindHightObjectByIndex()
    {
        GameObject gameObject = GameObject.Find(this.curHightLightObjPath);
        if (gameObject && this._childIndex != null)
        {
            for (int i = 0; i < this._childIndex.Length; i++)
            {
                gameObject = gameObject.transform.GetChild(float.Parse(this._childIndex[i]).ToInt()).gameObject;
            }
            return gameObject;
        }
        return null;
    }

    private void SetTipContentPosition(Vector3 centerImageWorldPosition)
    {
        if (!this.panelGuide)
        {
            return;
        }
        this.SetGuideModel();
        Vector3 vector = this.panelGuide.transform.InverseTransformPoint(centerImageWorldPosition);
        Vector3 transFromString = this.GetTransFromString(this.curConfig.GetField_String("posdialogue"));
        Vector3 transFromString2 = this.GetTransFromString(this.curConfig.GetField_String("posarrow"));
        Vector3 transFromString3 = this.GetTransFromString(this.curConfig.GetField_String("pospainting"));
        this.imgArrow.rotation = Quaternion.Euler(new Vector3(0f, 0f, (float)transFromString2[2].ToInt()));
        this.txtGuide.text = this.curConfig.GetField_String("guidetext");
        float num = 0.5f;
        if (this.animationState == UI_Guide.AnimationState.Guiding)
        {
            this.content.localPosition = vector + transFromString;
            this.imgArrow.localPosition = vector + new Vector3((float)transFromString2[0].ToInt(), (float)transFromString2[1].ToInt());
            this.imgIcon.transform.localPosition = vector + transFromString3;
            this.centerImage.transform.position = centerImageWorldPosition;
        }
        else if (this.animationState == UI_Guide.AnimationState.Start)
        {
            if (this.helpButtonPositon == Vector3.zero)
            {
                Transform uitransform = UIManager.GetUITransform(this._bondUIName);
                List<RectTransform> list = new List<RectTransform>();
                this.FindChildRectTransformsByName(uitransform, new List<string>(new string[]
                {
                    "Image_help"
                }), list);
                if (list.Count == 0)
                {
                    Debug.LogError("Not find help button");
                    return;
                }
                this.helpButtonPositon = this.panelGuide.transform.InverseTransformPoint(list[0].transform.position);
            }
            this.FadeImageMaskAnimation(num, this.originalMaskAlpha);
            this.animationState = UI_Guide.AnimationState.Animating;
            TweenPosition tweenPosition = TweenPosition.Begin(this.content.gameObject, num, vector + transFromString);
            tweenPosition.from = this.helpButtonPositon;
            TweenScale tweenScale = TweenScale.Begin(this.content.gameObject, num, Vector3.one);
            tweenScale.from = Vector3.zero;
            tweenScale.transform.localScale = Vector3.zero;
            TweenPosition tweenPosition2 = TweenPosition.Begin(this.imgArrow.gameObject, num, vector + transFromString2);
            tweenPosition2.from = this.helpButtonPositon;
            TweenScale tweenScale2 = TweenScale.Begin(this.imgArrow.gameObject, num, Vector3.one);
            tweenScale2.from = Vector3.zero;
            tweenScale2.transform.localScale = Vector3.zero;
            TweenPosition tweenPosition3 = TweenPosition.Begin(this.imgIcon.gameObject, num, vector + transFromString3);
            tweenPosition3.from = this.helpButtonPositon;
            TweenScale tweenScale3 = TweenScale.Begin(this.imgIcon.gameObject, num, Vector3.one);
            tweenScale3.from = Vector3.zero;
            tweenScale3.transform.localScale = Vector3.zero;
            TweenPosition tweenPosition4 = TweenPosition.Begin(this.centerImage.gameObject, num, vector);
            tweenPosition4.from = this.helpButtonPositon;
            TweenScale tweenScale4 = TweenScale.Begin(this.centerImage.gameObject, num, Vector3.one);
            tweenScale4.from = Vector3.zero;
            tweenScale4.transform.localScale = Vector3.zero;
            tweenPosition.onFinished = delegate (UITweener tween)
            {
                this.animationState = UI_Guide.AnimationState.Guiding;
            };
        }
        else if (this.animationState == UI_Guide.AnimationState.End)
        {
            this.animationState = UI_Guide.AnimationState.Animating;
            this.FadeImageMaskAnimation(num, 0f);
            TweenPosition tweenPosition5 = TweenPosition.Begin(this.content.gameObject, num, this.helpButtonPositon);
            TweenScale tweenScale5 = TweenScale.Begin(this.content.gameObject, num, Vector3.zero);
            TweenPosition tweenPosition6 = TweenPosition.Begin(this.imgArrow.gameObject, num, this.helpButtonPositon);
            TweenScale tweenScale6 = TweenScale.Begin(this.imgArrow.gameObject, num, Vector3.zero);
            TweenPosition tweenPosition7 = TweenPosition.Begin(this.imgIcon.gameObject, num, this.helpButtonPositon);
            TweenScale tweenScale7 = TweenScale.Begin(this.imgIcon.gameObject, num, Vector3.zero);
            TweenPosition tweenPosition8 = TweenPosition.Begin(this.centerImage.gameObject, num, this.helpButtonPositon);
            TweenScale tweenScale8 = TweenScale.Begin(this.centerImage.gameObject, num, Vector3.zero);
            tweenPosition5.onFinished = delegate (UITweener tween)
            {
                this.EndGuide();
            };
        }
        else if (this.animationState == UI_Guide.AnimationState.Animating)
        {
        }
    }

    private void FadeImageMaskAnimation(float duaration, float destAlpha)
    {
        Color color = this.topImage.color;
        color.a = destAlpha;
        TweenColor.Begin(this.topImage.gameObject, duaration, color);
        TweenColor.Begin(this.bottomImage.gameObject, duaration, color);
        TweenColor.Begin(this.leftImage.gameObject, duaration, color);
        TweenColor.Begin(this.rightImage.gameObject, duaration, color);
    }

    private void UpdateSibling()
    {
        if (this._transPanelBase != null && !this._iffreeze)
        {
            base.uiPanelRoot.SetParent(this._transPanelBase.parent);
            base.uiPanelRoot.SetSiblingIndex(this._transPanelBase.GetSiblingIndex() + 1);
            return;
        }
    }

    private void UpdateStepTime()
    {
        if (this._stepLastTimeEndBig - this.Now < 0f && this._needLastTimeEndBig)
        {
            this.EndGuide();
        }
        if (this._stepLastTimeEndSmall - this.Now < 0f && this._needLastTimeEndSmall)
        {
            this.NextStep();
        }
    }

    private void UpdateOnlyStep()
    {
        this.Now = Time.realtimeSinceStartup;
        if (this._stepLastTimeEndBig - this.Now < 0f && this._needLastTimeEndBig)
        {
            this.EndGuide();
        }
    }

    private void viewSelect()
    {
        this.imgArea.gameObject.SetActive(true);
        if (this.curHightLightObj != null)
        {
            this.UpdatePos();
            this.UpdateSibling();
            this.UpdateStepTime();
        }
        this.SetGuideModel();
    }

    private void viewHightLight()
    {
        if (this.curHightLightObj != null)
        {
            this.img_mask.position = this.imgArea.transform.position;
            this.img_mask.localScale = this._zoomMask;
        }
    }

    private void viewArrow()
    {
        this.imgArrow.gameObject.SetActive(true);
    }

    private void viewContent()
    {
        this.contentBg.gameObject.SetActive(true);
    }

    private void viewIcon()
    {
        this.imgIcon.gameObject.SetActive(true);
    }

    private void bFreezetime(bool freeze)
    {
    }

    private void getListener()
    {
        this.listener = null;
        if (this.curHightLightObj == null)
        {
            return;
        }
        SkillButton component = this.curHightLightObj.GetComponent<SkillButton>();
        if (component != null)
        {
            this.listener = component.RealSprite.GetComponent<UIEventListener>();
            if (this.listener != null)
            {
                this.SkillBtnOnDragCallBack = this.listener.onDown;
                this.SkillBtnOnEndDragCallBack = this.listener.onUp;
                this.SkillBtnBeginDragCallBack = this.listener.onBeginDrag;
            }
        }
        else if (this._highlighttype == 2U)
        {
            this.listener = this.imgArea.GetComponent<UIEventListener>();
            this.OnClickCallBack = new UIEventListener.VoidDelegate(this.OnZoneClick);
        }
        else if (this._highlighttype == 3U)
        {
            this.listener = this.curHightLightObj.GetComponent<UIEventListener>();
            this.OnClickCallBack = new UIEventListener.VoidDelegate(this.OnItemClick);
        }
        else
        {
            this.listener = this.curHightLightObj.GetComponent<UIEventListener>();
            if (this.listener != null && this.listener.onClick != null)
            {
                this.OnClickCallBack = this.listener.onClick;
            }
            else if (this.curHightLightObj.GetComponent<Button>())
            {
                this._useButtonEvent = true;
                this.OnClickCallBack = new UIEventListener.VoidDelegate(this.OnButtonClick);
            }
            else if (this.curHightLightObj.GetComponent<Toggle>())
            {
                this._useButtonEvent = true;
                this.OnClickCallBack = new UIEventListener.VoidDelegate(this.OnToggleClick);
            }
        }
    }

    private void OnZoneClick(PointerEventData eventData)
    {
    }

    private void OnItemClick(PointerEventData eventData)
    {
        eventData.button = PointerEventData.InputButton.Right;
        this.curHightLightObj.GetComponent<DragDropButton>().OnPointerUp(eventData);
    }

    private void OnButtonClick(PointerEventData eventData)
    {
        this.curHightLightObj.GetComponent<Button>().OnPointerClick(eventData);
    }

    private void OnToggleClick(PointerEventData eventData)
    {
        this.curHightLightObj.GetComponent<Toggle>().OnPointerClick(eventData);
    }

    private void clearImgareaEvent()
    {
        UIEventListener.Get(this.imgArea).onEndDrag = null;
        UIEventListener.Get(this.imgArea).onDown = null;
        UIEventListener.Get(this.imgArea).onUp = null;
        UIEventListener.Get(this.imgArea).onDrag = null;
        UIEventListener.Get(this.imgArea).onBeginDrag = null;
        UIEventListener.Get(this.imgArea).onClick = null;
    }

    private void doNext()
    {
        this.viewHightLight();
        this.guideState = UI_Guide.GuideState.None;
        this.viewSelect();
        this.getListener();
        if (this.listener != null || this._useButtonEvent)
        {
            if (this.SkillBtnOnDragCallBack != null)
            {
                UIEventListener.Get(this.imgArea).onUp = delegate (PointerEventData data)
                {
                    this.SkillBtnOnEndDragCallBack(data);
                    this.stepGuideCallBack();
                };
                UIEventListener.Get(this.imgArea).onEndDrag = delegate (PointerEventData data)
                {
                    this.SkillBtnOnEndDragCallBack(data);
                };
                UIEventListener.Get(this.imgArea).onDown = delegate (PointerEventData data)
                {
                    this.SkillBtnOnDragCallBack(data);
                };
                UIEventListener.Get(this.imgArea).onDrag = this.SkillBtnOnDragCallBack;
                UIEventListener.Get(this.imgArea).onBeginDrag = this.SkillBtnBeginDragCallBack;
            }
            if (this.OnClickCallBack != null)
            {
                UIEventListener.Get(this.imgArea).onClick = delegate (PointerEventData data)
                {
                    if (this._itemBaseId != 0U)
                    {
                        this.GetBagItemNum(this._itemBaseId);
                    }
                    if (this._highlighttype == 2U)
                    {
                        this.PassEvent<IPointerClickHandler>(data, ExecuteEvents.pointerClickHandler);
                    }
                    else
                    {
                        if (this.curHightLightObj != null)
                        {
                            data.pointerPress = this.curHightLightObj;
                        }
                        this.OnClickCallBack(data);
                    }
                    this.stepGuideCallBack();
                };
            }
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().setInGuide(false);
            this.imgArea_mask.SetActive(false);
        }
    }

    private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
    {
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, list);
        GameObject gameObject = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < list.Count; i++)
        {
            if (gameObject != list[i].gameObject)
            {
                ExecuteEvents.Execute<T>(list[i].gameObject, data, function);
            }
        }
    }

    private void InitEvent()
    {
        this.btnCloseExplain.onClick.RemoveAllListeners();
        this.btnCloseExplain.onClick.AddListener(new UnityAction(this.CloseExplain));
        this.btnCloseGuide.onClick.RemoveAllListeners();
        this.btnCloseGuide.onClick.AddListener(new UnityAction(this.CloseGuide));
    }

    public void Close()
    {
        this.guideControl.CloseGuideUI();
    }

    public void CloseExplain()
    {
        this.transExplain.gameObject.SetActive(false);
        if (this._guideId == 0U)
        {
            this.Close();
        }
    }

    public void CloseGuide()
    {
        this.EndGuide();
    }

    public void NextStep()
    {
        if (this.stepGuideCallBack != null)
        {
            this.stepGuideCallBack();
        }
    }

    private void CheckNpcInScene()
    {
        if (this.npcId == 0U)
        {
            return;
        }
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
            {
                this.npc = pair.Value;
                if (this.npcId == this.npc.NpcData.MapNpcData.baseid)
                {
                    this.npcTempId = this.npc.EID.Id;
                    Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNpcInScene));
                    if (this.showNpcId == this.npc.NpcData.MapNpcData.baseid)
                    {
                        this.npc.EnableGuideEffect(true, this.npcTempId);
                    }
                    return;
                }
            });
        }
    }

    private Transform GetBagItemTransform(uint baseId)
    {
        object obj = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetBagItemTransform", new object[]
        {
            baseId
        })[0];
        if (obj == null)
        {
            return null;
        }
        Transform transform = (Transform)obj;
        if (transform == null)
        {
            return null;
        }
        return transform.parent;
    }

    private int GetBagItemNum(uint baseId)
    {
        int num = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemCount", new object[]
        {
            baseId
        })[0]);
        if (num == 1)
        {
            this._needSkipStep = true;
        }
        else
        {
            this._needSkipStep = false;
        }
        return num;
    }

    public void CheckCloseUI(Transform uipanel)
    {
        if (this._transPanelBase == uipanel && this._transPanelBase)
        {
            this.EndGuide();
        }
    }

    public void CheckOpenUI(Transform uipanel)
    {
        if (uipanel && uipanel.name == this._bondUIName)
        {
            this._transPanelBase = uipanel;
            this.SetUIOffset(uipanel, false);
        }
        if (uipanel && uipanel.name == this._secondUIName)
        {
            this.SetUIOffset(uipanel, true);
        }
    }

    private void SetUIOffset(Transform ui, bool isSecondUI)
    {
        if (isSecondUI)
        {
            if (this._offsetSecondUI != Vector3.zero)
            {
                ui.localPosition = this._offsetSecondUI;
            }
        }
        else if (this._offsetUI != Vector3.zero)
        {
            ui.localPosition = this._offsetUI;
        }
    }

    private void SetUITransAndSecondUI()
    {
        this._transPanelBase = UIManager.GetUITransform(this._bondUIName);
        if (this._transPanelBase)
        {
            this.SetUIOffset(this._transPanelBase, false);
        }
        this._transSecondPanelBase = UIManager.GetUITransform(this._secondUIName);
        if (this._transSecondPanelBase)
        {
            this.SetUIOffset(this._transSecondPanelBase, true);
        }
        else if (this._secondUIName != string.Empty)
        {
            if (this._secondUIName == "UI_Bag")
            {
                if (!UIManager.IsLuaPanelExists("UI_Bag") || UIManager.GetLuaUIPanel("UI_Bag").uiRoot.transform.localPosition == new Vector3(10000f, 10000f, 0f))
                {
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterPackage", new object[]
                    {
                        Util.GetLuaTable("BagCtrl")
                    });
                }
                else
                {
                    UIManager.GetLuaUIPanel("UI_Bag").uiRoot.transform.SetAsLastSibling();
                }
            }
            else if (this._secondUIName == "UI_Character")
            {
                UIManager.Instance.ShowUI<UI_Character>("UI_Character", delegate ()
                {
                    if (this._transPanelBase)
                    {
                        this._transPanelBase.SetAsLastSibling();
                    }
                }, UIManager.ParentType.CommonUI, false);
            }
        }
    }

    private Vector3 GetTransFromString(string str)
    {
        string[] array = str.Split(new char[]
        {
            ','
        });
        if (array == null || array.Length < 3)
        {
            return Vector3.zero;
        }
        return new Vector3((float)array[0].ToInt(), (float)array[1].ToInt(), (float)array[2].ToInt());
    }

    private void SetGuideModel()
    {
        if (this.avatarid == 0U)
        {
            return;
        }
        if (this.lastavatarid == this.avatarid)
        {
            return;
        }
        this.lastavatarid = this.avatarid;
        if (this._guideModel != null)
        {
            UnityEngine.Object.DestroyImmediate(this._guideModel);
            this._guideModel = null;
        }
        if (this._guideModel == null || this._guideModel.GetComponent<Animator>().avatar == null)
        {
            this.imgIcon.gameObject.SetActive(false);
            uint loadAvatarid = this.avatarid;
            GlobalRegister.ShowNpcOrPlayerRTT(this.imgIcon, this.avatarid, 2, delegate (GameObject obj)
            {
                if (loadAvatarid != this.avatarid)
                {
                    UnityEngine.Object.DestroyImmediate(obj);
                    return;
                }
                this._guideModel = obj;
                this.imgIcon.gameObject.SetActive(true);
            });
        }
    }

    public UI_Guide.GuideState guideState;

    public Transform mRoot;

    private Transform content;

    private GameObject imgArea;

    private GameObject panelGuide;

    private GameObject imgArea_mask;

    private GameObject img_maskevent;

    private Transform img_mask;

    private Transform guideBtnParent;

    private Transform imgArrow;

    private Transform contentBg;

    private RawImage imgIcon;

    private Text txtGuide;

    private Transform transExplain;

    private Transform transTips1;

    private Transform transTips2;

    private Button btnCloseExplain;

    private Button btnCloseGuide;

    private Image centerImage;

    private Image topImage;

    private Image bottomImage;

    private Image leftImage;

    private Image rightImage;

    private float originalMaskAlpha;

    private Action stepGuideCallBack;

    private GameObject curHightLightObj;

    private Vector3 curHightLightObjPos = Vector3.zero;

    private string _curHightLightObjPath;

    private LuaTable curConfig;

    private UIEventListener.VoidDelegate OnClickCallBack;

    private UIEventListener.VoidDelegate SkillBtnOnDragCallBack;

    private UIEventListener.VoidDelegate SkillBtnOnEndDragCallBack;

    private UIEventListener.VoidDelegate SkillBtnBeginDragCallBack;

    private UIEventListener listener;

    private bool timeEnableClick;

    private bool _iffreeze;

    private bool _useButtonEvent;

    private uint _guideId;

    private uint _guideType;

    public uint npcId;

    public ulong npcTempId;

    public uint showNpcId;

    private uint _highlighttype = 1U;

    private string[] _childIndex = new string[]
    {
        "0"
    };

    private uint _itemBaseId;

    private Npc npc;

    private float maxFindTime = 4f;

    private float Now;

    private float lastTime;

    private float viewTime = 0.2f;

    private float _maxStepLastTime = 20f;

    private float _stepLastTimeEndBig;

    private float _stepLastTimeEndSmall;

    private bool _needLastTimeEndBig;

    private bool _needLastTimeEndSmall;

    private bool _needSkipStep;

    private Vector3 imgMaskInitPostion = new Vector3(-3000f, 0f, 0f);

    private float guidemaskBlank = 40f;

    private float showNextStateTime = 0.2f;

    private Transform _transPanelBase;

    private Transform _transSecondPanelBase;

    private Vector3 _offsetUI = Vector3.zero;

    private Vector3 _offsetSecondUI = Vector3.zero;

    private string _bondUIName = string.Empty;

    private string _secondUIName = string.Empty;

    private Vector3 _zoomMask = Vector3.one;

    private int _guideStepNum;

    private int _curGuideStepNum;

    private List<LuaTable> _guideQueue = new List<LuaTable>();

    private Vector3 helpButtonPositon = Vector3.zero;

    private bool isMouseEnter;

    private float mouseStayTimer;

    private bool isNeedCheckCenterImageMouseEvent;

    private float stepMouseHoverStayTime;

    private bool isIn;

    private UI_Guide.GuideState stateFindChild;

    private UI_Guide.AnimationState animationState;

    private uint arrowWith = 100U;

    private GameObject _guideModel;

    private uint avatarid = 2130U;

    private uint lastavatarid;

    public enum guideDispearType
    {
        clickDispear = 1,
        delayDispear
    }

    public enum GuideState
    {
        None,
        Npc = 5,
        Quest,
        ChangeRole = 8,
        BagFull = 10,
        GetItem,
        ViewContent,
        FindHightLightObj,
        FindHightLightPos,
        FindHightLightGroupPos,
        BagItem,
        ChildItem
    }

    private enum AnimationState
    {
        None,
        Start,
        Animating,
        Guiding,
        End
    }
}
