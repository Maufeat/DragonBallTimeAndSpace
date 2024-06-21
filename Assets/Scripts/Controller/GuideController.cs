using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using UnityEngine;

public class GuideController : ControllerBase
{
    public event Action<string> eventGuideDeleteUI;

    public UI_Guide _uiGuide
    {
        get
        {
            return UIManager.GetUIObject<UI_Guide>();
        }
    }

    public string Initial_UI
    {
        get
        {
            return this._initial_ui;
        }
        set
        {
            this._initial_ui = value;
            if (this.eventGuideDeleteUI != null)
            {
                this.eventGuideDeleteUI(this._initial_ui);
            }
        }
    }

    public override void Awake()
    {
        this.guideNetWork = new GuideNetWork();
        this.guideNetWork.Initialize();
        UIManager.Instance.eventShowUI += this.OpenUIEvent;
        UIManager.Instance.eventDeleteUI += this.CloseUIEvent;
    }

    public override void OnDestroy()
    {
        this.Reset();
        base.OnDestroy();
    }

    public void OpenUIEvent(string uinama)
    {
        Transform uitransform = UIManager.GetUITransform(uinama);
        if (uitransform && this._uiGuide)
        {
            this._uiGuide.CheckOpenUI(uitransform);
        }
    }

    public void CloseUIEvent(string uinama)
    {
        Transform uitransform = UIManager.GetUITransform(uinama);
        if (uitransform && this._uiGuide)
        {
            this._uiGuide.CheckCloseUI(uitransform);
        }
    }

    public void CheckIsNeedGuideByID(uint geneGuideID)
    {
        if (!this.historyGuideIDs.Contains(geneGuideID))
        {
            this.ReqStartGuide(geneGuideID);
        }
    }

    public bool IsCompulsoryGuide(uint id = 0U)
    {
        if (id == 0U)
        {
            id = this.curGuideID;
        }
        return this.compulsoryGuideID != null && this.compulsoryGuideID.Count != 0 && this.compulsoryGuideID.Contains(this.curGuideID);
    }

    public void Reset()
    {
        this.bInit = false;
    }

    public void InitDicGuide(bool isForce = false)
    {
        if (this.bInit && !isForce)
        {
            return;
        }
        this.bInit = true;
        if (isForce)
        {
            LuaConfigManager.ForceLoadExcelConfig("guide");
        }
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("guide");
        tGuideCompare comparer = new tGuideCompare();
        configTableList.Sort(comparer);
        this.DicGuide.Clear();
        for (int i = 0; i < configTableList.Count; i++)
        {
            uint field_Uint = configTableList[i].GetField_Uint("guideid");
            if (!this.DicGuide.ContainsKey(field_Uint))
            {
                this.DicGuide[field_Uint] = new List<LuaTable>();
            }
            this.DicGuide[field_Uint].Add(configTableList[i]);
        }
    }

    public void Ret_GuideOver_SC(uint id)
    {
        this.IsGuide = false;
        this.CloseGuideUI();
    }

    public void ViewGuideUI(uint id, bool faceGuide = false)
    {
        this.IsGuide = true;
        this.curGuideID = id;
        this.InitDicGuide(false);
        if (!this.DicGuide.ContainsKey(id))
        {
            this.Req_GuideOver_CS(id);
            return;
        }
        if (this._uiGuide != null)
        {
            this._uiGuide.StartGuide(id);
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Guide>("UI_Guide", delegate ()
            {
                if (this._uiGuide != null)
                {
                    this._uiGuide.StartGuide(id);
                }
            }, UIManager.ParentType.Guide, false);
        }
    }

    public void CloseGuideUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Guide");
    }

    public void Req_GuideOver_CS(uint id)
    {
        this.guideNetWork.Req_GuideOver_CS(id);
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "guide_controller";
        }
    }

    public void CheckNpcGuide(uint tempId)
    {
        if (this._uiGuide == null)
        {
            return;
        }
        if ((this._uiGuide.guideState == UI_Guide.GuideState.Npc || this._uiGuide.guideState == UI_Guide.GuideState.Quest) && (ulong)tempId == this._uiGuide.npcTempId)
        {
            this._uiGuide.NextStep();
        }
    }

    public bool CheckShowNpc(uint baseId)
    {
        return !(this._uiGuide == null) && baseId == this._uiGuide.showNpcId;
    }

    public void GetCloseGuideEvent()
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.TryEsc", new object[0]);
    }

    public void OnHistoryBack(List<uint> guidedHistoryIDs)
    {
        this.historyGuideIDs.Clear();
        this.historyGuideIDs.AddRange(guidedHistoryIDs);
    }

    public void ReqStartGuide(uint guideID)
    {
        this.guideNetWork.Req_StartGuide_CS(guideID);
    }

    public void TryCacheGuidedID(uint id)
    {
        if (!this.historyGuideIDs.Contains(id))
        {
            this.historyGuideIDs.Add(id);
        }
    }

    public void CloseGuide()
    {
        if (this._uiGuide != null)
        {
            this._uiGuide.CloseGuide();
        }
    }

    private GuideNetWork guideNetWork;

    public Dictionary<uint, List<LuaTable>> DicGuide = new Dictionary<uint, List<LuaTable>>();

    public bool IsGuide;

    private List<uint> historyGuideIDs = new List<uint>();

    private List<uint> compulsoryGuideID = new List<uint>
    {
        5U,
        6U
    };

    private string _initial_ui;

    private uint curGuideID;

    private bool bInit;

    public enum CloseGuideEvent
    {
        None,
        CloseBag
    }
}
