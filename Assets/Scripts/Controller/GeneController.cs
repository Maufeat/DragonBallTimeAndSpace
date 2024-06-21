using System;
using System.Collections.Generic;
using apprentice;
using Framework.Managers;
using hero;
using LuaInterface;
using Models;
using UnityEngine;

public delegate bool DragUpCheckCb(DragDropButton btnUse);

public class GeneController : ControllerBase
{
    private UI_Gene ui_gene
    {
        get
        {
            return UIManager.GetUIObject<UI_Gene>();
        }
    }

    public bool isFirstOpen
    {
        get
        {
            return this.isFirstOpen_;
        }
        set
        {
            this.isFirstOpen_ = value;
        }
    }

    public override void Awake()
    {
        this.mNetWork = new GeneNetWork();
        this.mNetWork.Initialize();
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenGeneByNpc));
    }

    public void OpenGeneByNpc(List<VarType> varParams)
    {
        LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.UnInit", new object[]
        {
            Util.GetLuaTable("NpcTalkAndTaskDlgCtrl")
        });
        if (varParams.Count > 0)
        {
            GlobalRegister.OpenGeneUI(varParams[0]);
        }
    }

    internal void OnMSG_ResponseSlotOpened_SC(MSG_ResponseSlotOpened_SC msg)
    {
        MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC;
        if (this.ui_gene != null && this.dnaPageInfo.TryGetValue(msg.page, out msg_DnaPageInfo_CSC))
        {
            if (msg.type == DNASlotType.ATT)
            {
                msg_DnaPageInfo_CSC.att_opened_num = msg.cur_num;
            }
            else
            {
                msg_DnaPageInfo_CSC.def_opened_num = msg.cur_num;
            }
            this.ui_gene.RefrashDnaPageUI(msg_DnaPageInfo_CSC);
        }
    }

    public void RegDragCallBack()
    {
        DragDropManager.Instance.RegisterDragUpCheckCb(UIRootType.Gene, new DragUpCheckCb(this.IsCanDragUp));
        DragDropManager.Instance.RegisterPutInCb(UIRootType.Gene, new TwoBtnCb(this.PutInToItem));
        DragDropManager.Instance.RegisterUseItemCb(UIRootType.Gene, new OneBtnCb(this.RightClickItem));
    }

    public override string ControllerName
    {
        get
        {
            return "gene_controller";
        }
    }

    public bool IsInGeneMenu()
    {
        return this.ui_gene != null;
    }

    public void ReqGeneBagData()
    {
        this.mNetWork.ReqGeneBagInfo();
    }

    public DNAPage GetCurDnaPageType()
    {
        return this.curPage;
    }

    public void RegOnDnaPageDataChange(Action<Dictionary<string, int>, string> cb, bool isNeedReq = true)
    {
        this.dnaDataChangeNotify = cb;
        if (isNeedReq)
        {
            this.ReqGeneBagData();
        }
    }

    public void RegOnDnaPageNameChange(Action<string> cb)
    {
        ServerStorageManager.Instance.RegReqCallBack(ServerStorageKey.GenePageName, new Action<MSG_Req_OperateClientDatas_CS>(this.OnDnaPageNameChange));
        if (cb != null)
        {
            this.dnaPageNameNotify = cb;
        }
        else
        {
            this.isNotifyGenePage = true;
        }
    }

    private void OnDnaPageNameChange(MSG_Req_OperateClientDatas_CS msg)
    {
        string obj = "第一页|第二页|第三页|第四页";
        if (!string.IsNullOrEmpty(msg.value))
        {
            obj = msg.value;
        }
        if (this.dnaPageNameNotify != null)
        {
            this.dnaPageNameNotify(obj);
        }
        if (this.ui_gene != null && this.isNotifyGenePage)
        {
            this.ui_gene.ReqPageNameBack(msg);
        }
    }

    private void OnDnaPageChange(List<DnaItem> items, MSG_DnaPageInfo_CSC msg, DNAPage p)
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        if (this.dnaDataChangeNotify != null)
        {
            this.RegOnDnaPageNameChange(delegate (string backName)
            {
                List<InBagDnaData> list = new List<InBagDnaData>();
                for (int i = 0; i < msg.att_holes.Count; i++)
                {
                    if (msg.att_holes[i].id != 0U)
                    {
                        list.Add(new InBagDnaData(msg.att_holes[i].id, msg.att_holes[i].level));
                    }
                }
                for (int j = 0; j < msg.def_holes.Count; j++)
                {
                    if (msg.def_holes[j].id != 0U)
                    {
                        list.Add(new InBagDnaData(msg.def_holes[j].id, msg.def_holes[j].level));
                    }
                }
                for (int k = 0; k < list.Count; k++)
                {
                    uint num = list[k].id + list[k].level;
                    LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
                    if (configTable != null)
                    {
                        string cacheField_String = configTable.GetCacheField_String("property");
                        if (!string.IsNullOrEmpty(cacheField_String))
                        {
                            string[] array = cacheField_String.Split(new char[]
                            {
                                '|'
                            });
                            if (array != null && array.Length > 0)
                            {
                                for (int l = 0; l < array.Length; l++)
                                {
                                    string[] array2 = array[l].Split(new char[]
                                    {
                                        ','
                                    });
                                    if (array2.Length >= 2)
                                    {
                                        int num2 = 0;
                                        int.TryParse(array2[1], out num2);
                                        if (data.ContainsKey(array2[0]))
                                        {
                                            data[array2[0]] = data[array2[0]] + num2;
                                        }
                                        else
                                        {
                                            data.Add(array2[0], num2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                string arg = string.Empty;
                if (!string.IsNullOrEmpty(backName))
                {
                    string[] array3 = backName.Split(new char[]
                    {
                        '|'
                    });
                    for (int m = 0; m < array3.Length; m++)
                    {
                        if (m + DNAPage.PAGE1 == p)
                        {
                            arg = array3[m];
                        }
                    }
                }
                this.dnaDataChangeNotify(data, arg);
            });
            ServerStorageManager.Instance.GetData(ServerStorageKey.GenePageName, 0U);
            this.isNotifyGenePage = false;
        }
    }

    internal void OnRtMSG_ReqCombineDnaInBag_CS(MSG_ReqCombineDnaInBag_CS msg)
    {
        if (this.ui_gene != null)
        {
            this.ui_gene.OnCombineDna(msg);
        }
    }

    internal void OnRtMSG_MSG_ReqCombineDna_CS(MSG_ReqCombineDna_CS msg)
    {
        if (this.ui_gene != null)
        {
            this.ui_gene.OnCombineDna(msg);
        }
    }

    internal void RetMSG_ReqChangeCurDnaPage_CS(MSG_ReqChangeCurDnaPage_CS msg)
    {
        this.curPageInUse = msg.page;
        if (this.ui_gene != null)
        {
            this.ui_gene.OnPageChange(msg.page);
        }
    }

    internal void SwapPage(DNAPage page)
    {
        this.mNetWork.ReqChangeCurDnaPage(page);
    }

    internal static Dictionary<string, AttDatas> attChiniseNameCache
    {
        get
        {
            if (GeneController.attChiniseNameCache_.Count == 0)
            {
                LuaTable config = LuaConfigManager.GetConfig("attribute_config");
                foreach (object obj in config.Values)
                {
                    LuaTable luaTable = obj as LuaTable;
                    if (luaTable != null)
                    {
                        AttDatas attDatas = new AttDatas();
                        attDatas.id = luaTable.GetCacheField_Uint("id");
                        attDatas.attName = luaTable.GetCacheField_String("attribute");
                        attDatas.cName = luaTable.GetCacheField_String("name");
                        attDatas.type = luaTable.GetCacheField_Uint("type");
                        GeneController.attChiniseNameCache_[attDatas.attName] = attDatas;
                    }
                }
            }
            return GeneController.attChiniseNameCache_;
        }
    }

    public string GetAttShowNameByConfigKey(string key)
    {
        AttDatas attDatas;
        if (GeneController.attChiniseNameCache.TryGetValue(key, out attDatas))
        {
            return attDatas.cName;
        }
        return string.Empty;
    }

    public AttDatas GetAttAttDatasByConfigKey(string key)
    {
        AttDatas result;
        if (GeneController.attChiniseNameCache.TryGetValue(key, out result))
        {
            return result;
        }
        return null;
    }

    public Dictionary<string, int> GetAttDataByType(Dictionary<string, int> orginalData, uint type)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        List<string> list = new List<string>(orginalData.Keys);
        return result;
    }

    internal void OnGetAllDnaItemData(MSG_DnaBagInfo_CSC msg)
    {
        this.orignalDnaItemList = msg.datas;
        this.curPage = msg.cur_page;
        this.curPageInUse = msg.cur_page;
        if (this.ui_gene != null && !this.isFirstOpen)
        {
            DNAPage dnaPageInDropDown = this.ui_gene.GetDnaPageInDropDown();
            this.curPage = dnaPageInDropDown;
        }
        this.isFirstOpen = false;
        if (this.ui_gene != null)
        {
            this.ui_gene.OnGetDnaBagInfo(msg);
        }
        this.mNetWork.ReqMSG_AllDnaPageInfo_CSC();
    }

    internal void OnGetDnaPageData(MSG_DnaPageInfo_CSC pageInfo)
    {
        this.dnaPageInfo[pageInfo.page] = pageInfo;
        this.curPage = pageInfo.page;
        if (this.IsInGeneMenu())
        {
            bool isExcusiveInPage = this.ui_gene.curGot == GeneOperationType.Fuse;
            this.ui_gene.RefrashGeneBagUI(this.FilterBagDnaList(this.curPage, isExcusiveInPage));
            this.ui_gene.RefrashDnaPageUI(pageInfo);
            this.ui_gene.FuseCancel(false);
        }
        this.OnDnaPageChange(this.orignalDnaItemList, pageInfo, this.curPage);
    }

    public List<DnaItem> FilterBagDnaList(DNAPage dp, bool isExcusiveInPage)
    {
        List<DnaItem> list = this.CloneDnaList(this.orignalDnaItemList);
        MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC = null;
        if (this.dnaPageInfo.TryGetValue(dp, out msg_DnaPageInfo_CSC))
        {
            List<Hole> list2 = new List<Hole>();
            list2.AddRange(msg_DnaPageInfo_CSC.att_holes);
            list2.AddRange(msg_DnaPageInfo_CSC.def_holes);
            if (isExcusiveInPage)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].num -= (uint)this.GetPageInsertMaxCount(list[i].id, list[i].level);
                }
            }
            else
            {
                for (int j = 0; j < list.Count; j++)
                {
                    for (int k = 0; k < list2.Count; k++)
                    {
                        if (list[j].id == list2[k].id && list[j].level == list2[k].level)
                        {
                            list[j].num -= 1U;
                            if (list[j].num < 0U)
                            {
                            }
                        }
                    }
                }
            }
        }
        for (int l = list.Count - 1; l >= 0; l--)
        {
            if (list[l].num == 0U)
            {
                list.RemoveAt(l);
            }
        }
        return list;
    }

    private List<DnaItem> CloneDnaList(List<DnaItem> from)
    {
        List<DnaItem> list = new List<DnaItem>();
        for (int i = 0; i < from.Count; i++)
        {
            list.Add(new DnaItem
            {
                id = from[i].id,
                level = from[i].level,
                num = from[i].num
            });
        }
        return list;
    }

    public bool IsCanDragUp(DragDropButton btnItem)
    {
        GeneDragDropData geneDragDropData = btnItem.mData as GeneDragDropData;
        return (!(this.ui_gene != null) || this.ui_gene.curGot == GeneOperationType.Insert) && geneDragDropData != null && geneDragDropData.curDnaItem != null;
    }

    public uint SlotOpenLevel(int index, DNAPage page, DNASlotType dst, uint alreadOpenCount)
    {
        uint curLevel = MainPlayer.Self.GetCurLevel();
        if (this.slotOpenLevelDic.ContainsKey(index))
        {
            return this.slotOpenLevelDic[index];
        }
        if (this.slotOpenLevelConfig.Count == 0)
        {
            LuaTable config = LuaConfigManager.GetConfig("dnaslot_config");
            foreach (object obj in config.Values)
            {
                LuaTable luaTable = obj as LuaTable;
                if (luaTable != null)
                {
                    uint cacheField_Uint = luaTable.GetCacheField_Uint("level");
                    if (cacheField_Uint != 0U)
                    {
                        this.slotOpenLevelConfig[cacheField_Uint] = luaTable;
                    }
                }
            }
        }
        string field = ((dst != DNASlotType.DEF) ? "attslotpage" : "defslotpage") + (int)page;
        int num = index - (int)alreadOpenCount;
        if (num > 0)
        {
            uint num2 = 1U;
            for (; ; )
            {
                LuaTable luaTable2 = null;
                if (!this.slotOpenLevelConfig.TryGetValue(num2, out luaTable2))
                {
                    goto IL_11C;
                }
                if (num2 > curLevel)
                {
                    uint cacheField_Uint2 = luaTable2.GetCacheField_Uint(field);
                    num -= (int)cacheField_Uint2;
                }
                if (num <= 0)
                {
                    break;
                }
                num2 += 1U;
            }
            this.slotOpenLevelDic[index] = num2;
            return num2;
        IL_11C:;
        }
        return 0U;
    }

    public void PutInToItem(DragDropButton btnFrom, DragDropButton btnTo)
    {
        if (this.ui_gene == null)
        {
            return;
        }
        switch (this.ui_gene.curGot)
        {
            case GeneOperationType.Insert:
                this.HandleInsertDragOperation(btnFrom, btnTo);
                break;
        }
    }

    public void HandleInsertDragOperation(DragDropButton btnFrom, DragDropButton btnTo)
    {
        GeneDragDropData geneDragDropData = btnFrom.mData as GeneDragDropData;
        GeneDragDropData geneDragDropData2 = btnTo.mData as GeneDragDropData;
        if (geneDragDropData.curDnaItem == null)
        {
            return;
        }
        if (geneDragDropData.sf == SourceFrom.Bag && geneDragDropData2.sf == SourceFrom.Page)
        {
            if (geneDragDropData2.IsThisPosHaveCard())
            {
                if (!geneDragDropData.Equals(btnTo))
                {
                    ActionData ad = new ActionData(ActionType.Insert, geneDragDropData, geneDragDropData2, 0U);
                    this.AddOperationAction(ad);
                }
            }
            else if (geneDragDropData2.isThisSlotOpen)
            {
                ActionData ad2 = new ActionData(ActionType.Insert, geneDragDropData, geneDragDropData2, 0U);
                this.AddOperationAction(ad2);
            }
            else
            {
                TipsWindow.ShowNotice(2902U);
                this.isInDragIng = true;
            }
        }
        if (geneDragDropData.sf == SourceFrom.Page)
        {
            if (geneDragDropData2.sf == SourceFrom.Bag)
            {
                ActionData ad3 = new ActionData(ActionType.UnInsert, geneDragDropData, null, 0U);
                this.AddOperationAction(ad3);
            }
            if (geneDragDropData2.sf == SourceFrom.Page)
            {
                if (geneDragDropData2.IsThisPosHaveCard())
                {
                    if (!geneDragDropData2.Equals(geneDragDropData))
                    {
                        ActionData ad4 = new ActionData(ActionType.Insert, geneDragDropData, geneDragDropData2, 0U);
                        this.AddOperationAction(ad4);
                        ActionData ad5 = new ActionData(ActionType.Insert, geneDragDropData2, geneDragDropData, 0U);
                        this.AddOperationAction(ad5);
                    }
                }
                else if (geneDragDropData2.isThisSlotOpen)
                {
                    if (geneDragDropData.dst == geneDragDropData2.dst)
                    {
                        ActionData ad6 = new ActionData(ActionType.UnInsert, geneDragDropData, null, 0U);
                        this.AddOperationAction(ad6);
                        uint insertPos = (uint)btnTo.mPos.x;
                        ActionData ad7 = new ActionData(ActionType.Insert, geneDragDropData, null, insertPos);
                        this.AddOperationAction(ad7);
                    }
                    else
                    {
                        TipsWindow.ShowNotice(2902U);
                    }
                }
                else
                {
                    TipsWindow.ShowNotice(2902U);
                    this.isInDragIng = true;
                }
            }
        }
    }

    internal List<DnaItem> InitBaseItem()
    {
        List<DnaItem> list = new List<DnaItem>();
        Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
        for (int i = 0; i < this.orignalDnaItemList.Count; i++)
        {
            DnaItem dnaItem = this.orignalDnaItemList[i];
            uint num = dnaItem.id + dnaItem.level;
            if (dictionary.ContainsKey(num))
            {
                Dictionary<uint, uint> dictionary3;
                Dictionary<uint, uint> dictionary2 = dictionary3 = dictionary;
                uint num2;
                uint key = num2 = num;
                num2 = dictionary3[num2];
                dictionary2[key] = num2 + dnaItem.num;
            }
            else
            {
                dictionary[num] = dnaItem.num;
            }
        }
        LuaTable config = LuaConfigManager.GetConfig("dnachip_config");
        if (config != null)
        {
            foreach (object obj in config.Values)
            {
                LuaTable luaTable = obj as LuaTable;
                if (luaTable != null)
                {
                    DnaItem dnaItem2 = new DnaItem();
                    uint cacheField_Uint = luaTable.GetCacheField_Uint("chipid");
                    dnaItem2.level = cacheField_Uint % 100U;
                    dnaItem2.id = cacheField_Uint - dnaItem2.level;
                    if (dictionary.ContainsKey(cacheField_Uint))
                    {
                        dnaItem2.num = dictionary[cacheField_Uint];
                    }
                    list.Add(dnaItem2);
                }
            }
        }
        list.Sort((DnaItem a, DnaItem b) => a.id.CompareTo(b.id));
        return list;
    }

    public bool IsThisGeneCardInFuse(uint pagePos, DNASlotType dst)
    {
        List<uint> list = new List<uint>(this.gddFuseDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            if (this.gddFuseDic[list[i]].dst == dst && this.gddFuseDic[list[i]].fromPagePos == pagePos && this.gddFuseDic[list[i]].sf == SourceFrom.Page)
            {
                return true;
            }
        }
        return false;
    }

    public void StartFuse()
    {
        uint geneMaxLevel = this.GetGeneMaxLevel();
        if (this.gdddCurClick.curDnaItem.level >= geneMaxLevel)
        {
            TipsWindow.ShowNotice(2907U);
            return;
        }
        uint num = this.gdddCurClick.curDnaItem.id + this.gdddCurClick.curDnaItem.level + 1U;
        LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
        uint cacheField_Uint = configTable.GetCacheField_Uint("goldcost");
        uint currencyByID = MainPlayer.Self.GetCurrencyByID(3U);
        if (currencyByID < cacheField_Uint)
        {
            TipsWindow.ShowNotice(3006U);
            return;
        }
        uint fuseNum = this.ui_gene.GetFuseNum();
        if (this.gdddCurClick.sf == SourceFrom.Page)
        {
            this.isFusedGeneInPageBefor = false;
            DNAPage curDnaPageType = this.GetCurDnaPageType();
            DNASlotType dst = this.gdddCurClick.dst;
            uint inPagePos = this.gdddCurClick.inPagePos;
            this.mNetWork.ReqFuseGeneInPage(curDnaPageType, inPagePos, dst, fuseNum);
        }
        else
        {
            this.isFusedGeneInPageBefor = true;
            this.mNetWork.ReqFuseGeneInBag(this.gdddCurClick.curDnaItem.id, this.gdddCurClick.curDnaItem.level, fuseNum);
        }
    }

    public void ReqExpendSlot(DNASlotType type)
    {
        DNAPage curDnaPageType = this.GetCurDnaPageType();
        this.mNetWork.ReqMSG_ReqBuySlot_SC(curDnaPageType, type);
    }

    public void RightClickItem(DragDropButton ddb)
    {
        GeneDragDropData geneDragDropData = ddb.mData as GeneDragDropData;
        switch (this.ui_gene.curGot)
        {
            case GeneOperationType.Fuse:
                if (this.IsInGeneMenu() && !this.ui_gene.inPlayEffect)
                {
                    this.gdddCurClick = (ddb.mData as GeneDragDropData);
                    if (this.gdddCurClick != null && this.gdddCurClick.curDnaItem != null)
                    {
                        this.ui_gene.RefrashFuseUI(this.gdddCurClick);
                    }
                }
                break;
            case GeneOperationType.Insert:
                if (!ddb.IsEmptyBtn() && geneDragDropData.curDnaItem != null && geneDragDropData.curDnaItem.id != 0U)
                {
                    ActionType at = (geneDragDropData.sf != SourceFrom.Bag) ? ActionType.UnInsert : ActionType.Insert;
                    ActionData ad = new ActionData(at, geneDragDropData, null, 0U);
                    this.AddOperationAction(ad);
                }
                break;
        }
        if (geneDragDropData.sf == SourceFrom.Page && !geneDragDropData.isThisSlotOpen)
        {
            this.ExpendClick(geneDragDropData);
        }
    }

    public void ExpendClick(GeneDragDropData gddd)
    {
        if (this.isInDragIng)
        {
            this.isInDragIng = false;
            return;
        }
        MsgBoxController controller = ControllerManager.Instance.GetController<MsgBoxController>();
        if (controller != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", 3009UL);
            string text = string.Empty;
            if (configTable != null)
            {
                text = configTable.GetCacheField_String("notice");
            }
            uint needDiamondsCount = this.GetOpenCurSlotPrice(gddd.dst);
            text = string.Format(text, needDiamondsCount);
            controller.ShowMsgBox(string.Empty, text, "确定", "取消", UIManager.ParentType.Tips, delegate ()
            {
                uint currencyByID = MainPlayer.Self.GetCurrencyByID(2U);
                DNAPage curDnaPageType = this.GetCurDnaPageType();
                MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC;
                if (this.dnaPageInfo.TryGetValue(curDnaPageType, out msg_DnaPageInfo_CSC))
                {
                    uint num = (gddd.dst != DNASlotType.ATT) ? msg_DnaPageInfo_CSC.def_opened_num : msg_DnaPageInfo_CSC.att_opened_num;
                    if (gddd.inPagePos > num + 1U)
                    {
                        TipsWindow.ShowNotice(3010U);
                        return;
                    }
                }
                if (currencyByID >= needDiamondsCount)
                {
                    this.ReqExpendSlot(gddd.dst);
                }
                else
                {
                    TipsWindow.ShowNotice(3005U);
                }
            }, delegate ()
            {
            }, null);
        }
    }

    private uint GetOpenCurSlotPrice(DNASlotType type)
    {
        uint result = 100U;
        DNAPage curDnaPageType = this.GetCurDnaPageType();
        MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC;
        if (this.dnaPageInfo.TryGetValue(curDnaPageType, out msg_DnaPageInfo_CSC))
        {
            uint num = (type != DNASlotType.ATT) ? msg_DnaPageInfo_CSC.def_opened_num : msg_DnaPageInfo_CSC.att_opened_num;
            LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("rune");
            string field = string.Empty;
            if (type == DNASlotType.ATT)
            {
                field = "dnaattslotnum";
            }
            else
            {
                field = "dnadefslotnum";
            }
            LuaTable cacheField_Table = xmlConfigTable.GetCacheField_Table(field);
            uint cacheField_Uint = cacheField_Table.GetCacheField_Uint("slot" + (int)curDnaPageType);
            uint cacheField_Uint2 = xmlConfigTable.GetCacheField_Table("dnaslotcost").GetCacheField_Uint("num");
            result = cacheField_Uint2 * (num - cacheField_Uint + 1U);
        }
        return result;
    }

    public uint GetGeneMaxLevel()
    {
        if (this.dnaItemMaxLevel == 0U)
        {
            LuaTable config = LuaConfigManager.GetConfig("dnachip_config");
            if (config != null)
            {
                foreach (object obj in config.Values)
                {
                    LuaTable luaTable = obj as LuaTable;
                    if (luaTable != null)
                    {
                        uint cacheField_Uint = luaTable.GetCacheField_Uint("level");
                        this.dnaItemMaxLevel = ((this.dnaItemMaxLevel <= cacheField_Uint) ? cacheField_Uint : this.dnaItemMaxLevel);
                    }
                }
            }
        }
        return this.dnaItemMaxLevel;
    }

    public int GetPageInsertMaxCount(uint id, uint level)
    {
        int num = int.MinValue;
        List<DNAPage> list = new List<DNAPage>(this.dnaPageInfo.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC = this.dnaPageInfo[list[i]];
            uint num2 = id + level;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num2);
            if (configTable != null)
            {
                List<Hole> list2 = (configTable.GetCacheField_Int("type") != 0) ? msg_DnaPageInfo_CSC.def_holes : msg_DnaPageInfo_CSC.att_holes;
                int num3 = 0;
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list2[j].id == id && list2[j].level == level)
                    {
                        num3++;
                    }
                }
                num = Math.Max(num3, num);
            }
        }
        return num;
    }

    public int GetBagCardMinCountExceptInPage(uint id, uint level)
    {
        int result = int.MaxValue;
        bool flag = false;
        List<DNAPage> list = new List<DNAPage>(this.dnaPageInfo.Keys);
        int num = 0;
        for (int i = 0; i < list.Count; i++)
        {
            MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC = this.dnaPageInfo[list[i]];
            uint num2 = id + level;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num2);
            if (configTable != null)
            {
                List<Hole> list2 = (configTable.GetCacheField_Int("type") != 0) ? msg_DnaPageInfo_CSC.def_holes : msg_DnaPageInfo_CSC.att_holes;
                int num3 = 0;
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list2[j].id == id && list2[j].level == level)
                    {
                        num3++;
                        flag = true;
                    }
                }
                num = Mathf.Max(num, num3);
            }
        }
        for (int k = 0; k < this.orignalDnaItemList.Count; k++)
        {
            if (this.orignalDnaItemList[k].id == id && this.orignalDnaItemList[k].level == level)
            {
                flag = true;
                result = (int)(this.orignalDnaItemList[k].num - (uint)num);
                break;
            }
        }
        if (!flag)
        {
            result = 0;
        }
        return result;
    }

    public void OnGetAllDnaPageInfo(MSG_AllDnaPageInfo_CSC msg)
    {
        for (int i = 0; i < msg.pages.Count; i++)
        {
            this.dnaPageInfo[msg.pages[i].page] = msg.pages[i];
        }
        MSG_DnaPageInfo_CSC pageInfo = null;
        if (this.dnaPageInfo.TryGetValue(this.curPage, out pageInfo))
        {
            if (this.IsInGeneMenu())
            {
                if (this.ui_gene.curGot == GeneOperationType.SwapPage && this.curPage != DNAPage.PAGE1)
                {
                    GeneController.notCallSelectEvent = true;
                }
                this.ui_gene.SetCurDnaPageToDropdwonUI(this.curPage);
            }
            this.OnGetDnaPageData(pageInfo);
        }
        if (this.isFusedGeneInPageBefor && this.gdddCurClick != null && this.gdddCurClick.curDnaItem != null)
        {
            int bagCardMinCountExceptInPage = this.GetBagCardMinCountExceptInPage(this.gdddCurClick.curDnaItem.id, this.gdddCurClick.curDnaItem.level);
            if (bagCardMinCountExceptInPage > 0 && this.IsInGeneMenu())
            {
                this.ui_gene.RefrashFuseUI(this.gdddCurClick);
            }
        }
    }

    public void AddOperationAction(ActionData ad)
    {
        this.actionDataQueue.Enqueue(ad);
    }

    public override void OnUpdate()
    {
        if (!this.isInDoing && this.actionDataQueue.Count > 0)
        {
            ActionData ad = this.actionDataQueue.Dequeue();
            this.DoAction(ad);
        }
        if (this.ui_gene != null)
        {
            this.ui_gene.OnUpdate();
        }
    }

    public void SetActionState(bool state)
    {
        this.isInDoing = state;
    }

    private void DoAction(ActionData ad)
    {
        GeneDragDropData gdddFrom = ad.gdddFrom;
        if (ad.at == ActionType.Insert)
        {
            uint num = gdddFrom.curDnaItem.id + gdddFrom.curDnaItem.level;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
            if (ad.gdddTo != null)
            {
                GeneDragDropData gdddTo = ad.gdddTo;
                if (configTable != null)
                {
                    if (gdddTo.isThisSlotOpen)
                    {
                        int cacheField_Int = configTable.GetCacheField_Int("type");
                        if (cacheField_Int == (int)gdddTo.dst)
                        {
                            DNAPage curDnaPageType = this.GetCurDnaPageType();
                            DNASlotType dst = gdddTo.dst;
                            this.mNetWork.ReqMSG_ReqPutOnDna_CS(curDnaPageType, gdddTo.inPagePos, gdddFrom.curDnaItem.id, gdddFrom.curDnaItem.level, dst);
                        }
                        else
                        {
                            TipsWindow.ShowNotice(2902U);
                        }
                    }
                    else
                    {
                        TipsWindow.ShowNotice(2902U);
                    }
                }
            }
            else
            {
                uint num2 = ad.insertPos;
                if (num2 == 0U)
                {
                    num2 = this.GetCurPageCanUsePos(gdddFrom.dst);
                }
                if (num2 != 0U)
                {
                    if (configTable != null)
                    {
                        int cacheField_Int2 = configTable.GetCacheField_Int("type");
                        DNAPage curDnaPageType2 = this.GetCurDnaPageType();
                        this.mNetWork.ReqMSG_ReqPutOnDna_CS(curDnaPageType2, num2, gdddFrom.curDnaItem.id, gdddFrom.curDnaItem.level, (DNASlotType)cacheField_Int2);
                    }
                }
                else
                {
                    TipsWindow.ShowNotice(2908U);
                }
            }
        }
        else
        {
            this.mNetWork.ReqMSG_MSG_ReqPutOffDna_CS(gdddFrom.inPagePos, gdddFrom.dst);
        }
    }

    public uint GetCurPageCanUsePos(DNASlotType dst)
    {
        MSG_DnaPageInfo_CSC msg_DnaPageInfo_CSC = null;
        if (this.dnaPageInfo.TryGetValue(this.curPage, out msg_DnaPageInfo_CSC))
        {
            uint num = (dst != DNASlotType.ATT) ? msg_DnaPageInfo_CSC.def_opened_num : msg_DnaPageInfo_CSC.att_opened_num;
            List<Hole> list;
            if (dst == DNASlotType.ATT)
            {
                list = msg_DnaPageInfo_CSC.att_holes;
            }
            else
            {
                list = msg_DnaPageInfo_CSC.def_holes;
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == 0U && list[i].pos <= num)
                {
                    return list[i].pos;
                }
            }
        }
        return 0U;
    }

    public GeneNetWork mNetWork;

    private Action<Dictionary<string, int>, string> dnaDataChangeNotify;

    private Action<string> dnaPageNameNotify;

    private bool isNotifyGenePage;

    public bool isFusedGeneInPageBefor;

    public bool isInDragIng;

    private DNAPage curPage = DNAPage.PAGE1;

    public DNAPage curPageInUse = DNAPage.PAGE1;

    public List<DnaItem> orignalDnaItemList = new List<DnaItem>();

    public List<DnaItem> babDnaItemList = new List<DnaItem>();

    public Dictionary<DNAPage, MSG_DnaPageInfo_CSC> dnaPageInfo = new Dictionary<DNAPage, MSG_DnaPageInfo_CSC>();

    public List<DnaItem> sealdDnaItemList = new List<DnaItem>();

    public Dictionary<uint, GeneDragDropData> gddFuseDic = new Dictionary<uint, GeneDragDropData>();

    private Dictionary<int, uint> slotOpenLevelDic = new Dictionary<int, uint>();

    private Dictionary<uint, LuaTable> slotOpenLevelConfig = new Dictionary<uint, LuaTable>();

    private GeneDragDropData gdddCurClick;

    private bool isFirstOpen_;

    private static Dictionary<string, AttDatas> attChiniseNameCache_ = new Dictionary<string, AttDatas>();

    public static bool notCallSelectEvent = false;

    private uint dnaItemMaxLevel;

    private Queue<ActionData> actionDataQueue = new Queue<ActionData>();

    private bool isInDoing;
}
