using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;

public class EscManager : IManager
{
    public void Init()
    {
        this.mCurOpenUIList = new List<string>();
        this.mEscPanelCbDic = new Dictionary<string, EscPanelCb>();
        this.mHidePanelList = new List<string>();
    }

    public void RegisterEscPanelCb(string uiname, EscPanelCb _cb)
    {
        if (!this.mEscPanelCbDic.ContainsKey(uiname))
        {
            this.mEscPanelCbDic.Add(uiname, _cb);
        }
        else
        {
            this.mEscPanelCbDic[uiname] = _cb;
        }
    }

    public void RegisterHidePanel(string uiname)
    {
        if (!this.mHidePanelList.Contains(uiname))
        {
            this.mHidePanelList.Add(uiname);
        }
    }

    public bool IsFunctionUI(string uiname)
    {
        if (this.mAllFunctionUINameList == null)
        {
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("esc_config");
            if (configTableList != null)
            {
                this.mAllFunctionUINameList = new List<string>();
                this.mAllFunctionUINameOpenTypeDic = new Dictionary<string, uint>();
                for (int i = 0; i < configTableList.Count; i++)
                {
                    LuaTable luaTable = configTableList[i];
                    string field_String = luaTable.GetField_String("template");
                    if (!this.mAllFunctionUINameList.Contains(uiname))
                    {
                        this.mAllFunctionUINameList.Add(field_String);
                        this.mAllFunctionUINameOpenTypeDic.Add(field_String, luaTable.GetField_Uint("open"));
                    }
                }
            }
        }
        return this.mAllFunctionUINameList != null && this.mAllFunctionUINameList.Contains(uiname);
    }

    public void SetUIFront(string uiname)
    {
        if (this.mCurOpenUIList.Contains(uiname) && this.mCurOpenUIList.Count > 0 && this.mCurOpenUIList[this.mCurOpenUIList.Count - 1] != uiname)
        {
            this.mCurOpenUIList[this.mCurOpenUIList.IndexOf(uiname)] = this.mCurOpenUIList[this.mCurOpenUIList.Count - 1];
            this.mCurOpenUIList[this.mCurOpenUIList.Count - 1] = uiname;
        }
    }

    public void OpenUI(string uiname)
    {
        if (this.mCurOpenUIList == null)
        {
            return;
        }
        if (!this.mCurOpenUIList.Contains(uiname))
        {
            if (this.IsFunctionUI(uiname))
            {
                this.mCurOpenUIList.Add(uiname);
                this.PlayFadeInOut(uiname, true);
                this.ProgressSameInLogic(uiname, -1);
            }
            this.RegisterSecondPanel(uiname);
        }
    }

    public void ProgressSameInLogic(string uiname, int type = -1)
    {
        if (!this.mAllFunctionUINameOpenTypeDic.ContainsKey(uiname))
        {
            return;
        }
        uint num = this.mAllFunctionUINameOpenTypeDic[uiname];
        if (type != -1)
        {
            num = (uint)type;
        }
        switch (num)
        {
            case 1U:
                List<string> list = new List<string>();
                for (int i = 0; i < this.mCurOpenUIList.Count; i++)
                {
                    string text = this.mCurOpenUIList[i];
                    if (text != uiname)
                    {
                        if (this.mAllFunctionUINameOpenTypeDic[text] == 2U)
                        {
                            list.Add(text);
                        }
                        else if (uiname != "UI_Bag" && this.mAllFunctionUINameOpenTypeDic[text] == 3U)
                        {
                            list.Add(text);
                        }
                        else
                        {

                        }
                    }
                }
                break;
            case 2U:
                while (this.mCurOpenUIList.Count > 0)
                {
                    string text3 = this.mCurOpenUIList[this.mCurOpenUIList.Count - 1];
                    if (uiname != text3)
                    {
                        this.ProgressCloseUI(text3);
                        this.CloseUI(text3);
                    }
                }
                break;
            case 3U:
                {
                    List<string> list2 = new List<string>();
                    for (int k = 0; k < this.mCurOpenUIList.Count; k++)
                    {
                        string text4 = this.mCurOpenUIList[0];
                        if (uiname != text4 && text4 != "UI_Bag")
                        {
                            list2.Add(text4);
                        }
                    }
                    for (int l = 0; l < list2.Count; l++)
                    {
                        string uiname2 = list2[l];
                        this.ProgressCloseUI(uiname2);
                        this.CloseUI(uiname2);
                    }
                    break;
                }
        }
    }

    public void PlayFadeInOut(string uiname, bool fadein)
    {
        if (this.mHidePanelList.Contains(uiname))
        {
            return;
        }
        UIPanelBase uiobject = UIManager.GetUIObject(uiname);
        Transform transform;
        if (uiobject == null)
        {
            LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(uiname);
            if (luaUIPanel == null)
            {
                return;
            }
            transform = luaUIPanel.uiRoot.transform;
        }
        else
        {
            transform = uiobject.uiPanelRoot;
        }
        CanvasGroup canvasGroup = transform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = transform.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = (float)((!fadein) ? 1 : 0);
        canvasGroup.DOFade((float)((!fadein) ? 0 : 1), EscManager.DURATION);
    }

    public void CloseUI(string uiname)
    {
        if (this.mCurOpenUIList.Contains(uiname))
        {
            this.mCurOpenUIList.Remove(uiname);
            this.PlayFadeInOut(uiname, false);
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnReSet()
    {
    }

    private bool IsInGame()
    {
        this.isInGame = (UIManager.GetUIObject<UI_MainView>() != null);
        return this.isInGame;
    }

    public void OnUpdate()
    {
        if ((this.isInGame || this.IsInGame()) && Input.GetKeyDown(KeyCode.Escape))
        {
            if (MainPlayer.Self == null)
            {
                return;
            }
            GuideController controller = ControllerManager.Instance.GetController<GuideController>();
            if (controller.IsGuide)
            {
                if (controller.IsCompulsoryGuide(0U))
                {
                    return;
                }
                controller.CloseGuide();
            }
            else if (MouseStateControoler.Instan.IsContinuedMouseState())
            {
                MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
            }
            else if (ManagerCenter.Instance.GetManager<CutSceneManager>().InPlayCutScene())
            {
                ManagerCenter.Instance.GetManager<CutSceneManager>().Stop();
            }
            else if (MouseStateControoler.Instan.CheckMouseState())
            {
                MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
            }
            else if (this.mCurOpenUIList.Count > 0)
            {
                string uiname = this.mCurOpenUIList[this.mCurOpenUIList.Count - 1];
                this.ProgressCloseUI(uiname);
            }
            else if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TargetCharactor == null && ControllerManager.Instance.GetController<MainUIController>().GetNormalTargetActive())
            {
                MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().SetTargetNull();
                if (MainPlayer.Self != null)
                {
                    MainPlayer.Self.SwitchAutoAttack(false);
                }
            }
            else if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TargetCharactor != null)
            {
                MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().SetTargetNull();
                if (MainPlayer.Self != null)
                {
                    MainPlayer.Self.SwitchAutoAttack(false);
                }
            }
            else if (this.mCurOpenUIList.Count == 0)
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
        }
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            GameObject gameObject = this.OnePointColliderObject();
            if (gameObject != null)
            {
                UIIdentifier componentInParent = gameObject.transform.GetComponentInParent<UIIdentifier>();
                if (componentInParent != null)
                {
                    GameObject gameObject2 = componentInParent.gameObject;
                    if (this.IsFunctionUI(gameObject2.name))
                    {
                        this.SetUIFront(gameObject2.name);
                        gameObject2.transform.SetAsLastSibling();
                    }
                }
            }
        }
        if (this.mCurTipObj != null)
        {
            if (this.mCurTipObj != this.OnePointColliderObject())
            {
                this.CleanTip();
            }
        }
        else if (this.DestoryListening)
        {
            this.CleanTip();
        }
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && UIManager.GetUIObject<UI_TeamSecondary>() != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject gameObject3 = this.OnePointColliderObject();
                UIIdentifier componentInParent2 = gameObject3.GetComponentInParent<UIIdentifier>();
                if (gameObject3 == null || componentInParent2 == null || componentInParent2.gameObject.name != "UI_TeamSecondary")
                {
                    UIManager.Instance.DeleteUI<UI_TeamSecondary>();
                }
            }
            else
            {
                UIManager.Instance.DeleteUI<UI_TeamSecondary>();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject gameObject4 = this.OnePointColliderObject();
                if (gameObject4 != null)
                {
                    SecondPanelTag componentInParent3 = gameObject4.GetComponentInParent<SecondPanelTag>();
                    SecondPanelBtnTag componentInParent4 = gameObject4.GetComponentInParent<SecondPanelBtnTag>();
                    if (componentInParent3 == null && componentInParent4 == null)
                    {
                        this.CloseAllSecondPanel();
                    }
                }
            }
            else
            {
                this.CloseAllSecondPanel();
            }
        }
    }

    private void RegisterSecondPanel(string uiname)
    {
        UIPanelBase uiobject = UIManager.GetUIObject(uiname);
        if (uiobject != null)
        {
            SecondPanelTag[] componentsInChildren = uiobject.uiPanelRoot.GetComponentsInChildren<SecondPanelTag>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                this.transAllSecondPanel.Add(componentsInChildren[i].gameObject.transform);
            }
        }
    }

    public void RegisterSecondPanel(Transform transform)
    {
        this.transAllSecondPanel.Add(transform);
    }

    private void CloseAllSecondPanel()
    {
        for (int i = 0; i < this.transAllSecondPanel.Count; i++)
        {
            if (this.transAllSecondPanel[i] != null)
            {
                this.transAllSecondPanel[i].gameObject.SetActive(false);
            }
        }
    }

    private void ProgressCloseUI(string uiname)
    {
        if (this.mEscPanelCbDic.ContainsKey(uiname))
        {
            this.mEscPanelCbDic[uiname]();
        }
        else
        {
            UIManager.Instance.DeleteUI(uiname);
        }
        if (this.mHidePanelList.Contains(uiname))
        {
            this.CloseUI(uiname);
        }
    }

    private void CleanTip()
    {
        ItemTipController controller = ControllerManager.Instance.GetController<ItemTipController>();
        if (controller != null)
        {
            controller.ClosePanel();
        }
        UIManager.Instance.DeleteUI<UI_TextTip>();
        this.mCurTipObj = null;
        this.DestoryListening = false;
    }

    public GameObject OnePointColliderObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, list);
        if (list.Count > 0)
        {
            return list[0].gameObject;
        }
        return null;
    }

    public void SetCurTipObj(GameObject obj)
    {
        this.mCurTipObj = obj;
        this.DestoryListening = true;
    }

    public static float DURATION = 0.1f;

    private List<string> mAllFunctionUINameList;

    private Dictionary<string, uint> mAllFunctionUINameOpenTypeDic;

    private List<string> mCurOpenUIList;

    private Dictionary<string, EscPanelCb> mEscPanelCbDic;

    public List<string> mHidePanelList = new List<string>();

    private bool isInGame;

    private List<Transform> transAllSecondPanel = new List<Transform>();

    private bool DestoryListening;

    private GameObject mCurTipObj;
}
