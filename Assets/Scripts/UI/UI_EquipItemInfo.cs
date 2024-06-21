using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipItemInfo : UIPanelBase
{
    private void initBtnActionDic()
    {
        this.DicBtnAction["use"] = new UIEventListener.VoidDelegate(this.useObject);
        this.DicBtnAction["split"] = new UIEventListener.VoidDelegate(this.splitObject);
        this.DicBtnAction["discard"] = new UIEventListener.VoidDelegate(this.destoryObject);
        this.DicBtnAction["equip"] = new UIEventListener.VoidDelegate(this.equipObject);
        this.DicBtnAction["activate"] = new UIEventListener.VoidDelegate(this.ActivateObject);
        this.DicBtnAction["sell"] = new UIEventListener.VoidDelegate(this.SellObject);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.initBtnActionDic();
        this.PanelItemInfo = this.mRoot.Find("Offset_ItemInfo/Panel_ItemInfo");
        this.PanelEquipInfo = this.mRoot.Find("Offset_ItemInfo/Panel_EquipInfo");
        this.PanelEquipContrastInfo = this.mRoot.Find("Offset_ItemInfo/Panel_EquipContrast");
        this.PanelRuneInfo = this.mRoot.Find("Offset_ItemInfo/Panel_RuneInfo");
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            EventSystem.current.RaycastAll(pointerEventData, UI_EquipItemInfo.overuiresults);
            if (UI_EquipItemInfo.overuiresults.Count > 0)
            {
                if (!UITools.IsChild(this.mRoot, UI_EquipItemInfo.overuiresults[0].gameObject.transform))
                {
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseInfo", new object[]
                    {
                        Util.GetLuaTable("BagCtrl")
                    });
                }
            }
            else
            {
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseInfo", new object[]
                {
                    Util.GetLuaTable("BagCtrl")
                });
            }
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.SetItemInfoPos));
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.SetEquipContrastInfoPos));
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        this.lockScheduler = false;
        this.UnInit();
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public void UnInit()
    {
        this.viewthisID = 0UL;
        for (int i = 0; i < this.commonItems.Count; i++)
        {
            this.commonItems[i].Dispose();
        }
        this.commonItems.Clear();
    }

    private void ViewEquipInfo(PropsBase data)
    {
        this.viewthisID = ulong.Parse(data._obj.thisid);
        this.PanelEquipInfo.Find("info/txt_info1").GetComponent<Text>().text = data.config.GetField_String("desc");
        if (!string.IsNullOrEmpty(data.config.GetField_String("refence_desc")))
        {
            this.PanelEquipInfo.Find("info/txt_info2").gameObject.SetActive(true);
            this.PanelEquipInfo.Find("info/txt_info2").GetComponent<Text>().text = data.config.GetField_String("refence_desc");
        }
        else
        {
            this.PanelEquipInfo.Find("info/txt_info2").gameObject.SetActive(false);
        }
        this.SetItemBaseInfo(this.PanelEquipInfo.Find("baseinfo"), data);
        this.InitBtnAction(this.PanelEquipInfo.Find("btninfo"), data);
        this.setLastTime(this.PanelEquipInfo.Find("lasttime"), data);
        this.setAttribut(this.PanelEquipInfo.Find("attribute"), data);
        this.setFightValue(this.PanelEquipInfo.Find("fightstrength"), data);
        if ((bool)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetbOpenEquipContrast", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        })[0])
        {
            this.switceEquipContrast(true, data);
        }
        else
        {
            this.setOpenContrastBtn(false);
        }
        UIEventListener.Get(this.PanelEquipInfo.Find("attribute/btn_compete").gameObject).onClick = delegate (PointerEventData eventData)
        {
            this.switceEquipContrast(!(bool)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetbOpenEquipContrast", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            })[0], data);
        };
    }

    private void switceEquipContrast(bool bOpen, PropsBase data)
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.SetbOpenEquipContrast", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            bOpen
        });
        this.PanelEquipContrastInfo.gameObject.SetActive(bOpen);
        this.setOpenContrastBtn(bOpen);
        PropsBase propsBase = this.bHaveContrastEquip(data);
        if (propsBase != null)
        {
            this.setEquipContrastInfo(propsBase, false);
        }
        else
        {
            this.PanelEquipInfo.Find("attribute/btn_compete").gameObject.SetActive(false);
            this.PanelEquipContrastInfo.gameObject.SetActive(false);
        }
    }

    private void setOpenContrastBtn(bool bOpen)
    {
        if (bOpen)
        {
            this.PanelEquipInfo.Find("attribute/btn_compete/img_close").gameObject.SetActive(true);
            this.PanelEquipInfo.Find("attribute/btn_compete/img_open").gameObject.SetActive(false);
        }
        else
        {
            this.PanelEquipInfo.Find("attribute/btn_compete/img_close").gameObject.SetActive(false);
            this.PanelEquipInfo.Find("attribute/btn_compete/img_open").gameObject.SetActive(true);
        }
    }

    private PropsBase bHaveContrastEquip(PropsBase data)
    {
        if (data._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && data._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX)
        {
            for (int i = 0; i < Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackageCount", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            })[0]); i++)
            {
                if (((PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackage_i", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    i
                })[0])._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && ((PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackage_i", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    i
                })[0])._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX)
                {
                    return (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackage_i", new object[]
                    {
                        Util.GetLuaTable("BagCtrl"),
                        i
                    })[0];
                }
            }
        }
        for (int j = 0; j < Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackageCount", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        })[0]); j++)
        {
            if (((PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackage_i", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                j
            })[0])._obj.type == data._obj.type)
            {
                return (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.RetlisEquipPackage_i", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    j
                })[0];
            }
        }
        return null;
    }

    private void ViewEquipContrastInfo(PropsBase data, bool bCommonInfo = false)
    {
        if (!bCommonInfo)
        {
            this.viewthisID = ulong.Parse(data._obj.thisid);
        }
        this.setEquipContrastInfo(data, bCommonInfo);
    }

    private void setEquipContrastInfo(PropsBase data, bool bCommonInfo = false)
    {
        this.PanelEquipContrastInfo.Find("info/txt_info1").GetComponent<Text>().text = data.config.GetField_String("desc");
        if (!string.IsNullOrEmpty(data.config.GetField_String("refence_desc")))
        {
            this.PanelEquipContrastInfo.Find("info/txt_info2").gameObject.SetActive(true);
            this.PanelEquipContrastInfo.Find("info/txt_info2").GetComponent<Text>().text = data.config.GetField_String("refence_desc");
        }
        else
        {
            this.PanelEquipContrastInfo.Find("info/txt_info2").gameObject.SetActive(false);
        }
        this.SetItemBaseInfo(this.PanelEquipContrastInfo.Find("baseinfo"), data);
        if (!bCommonInfo)
        {
            this.setLastTime(this.PanelEquipContrastInfo.Find("lasttime"), data);
        }
        else
        {
            this.PanelEquipContrastInfo.Find("lasttime").gameObject.SetActive(false);
        }
        this.setAttribut(this.PanelEquipContrastInfo.Find("attribute"), data);
        this.setFightValue(this.PanelEquipContrastInfo.Find("fightstrength"), data);
    }

    private void viewItemInfo(PropsBase data, bool bCommonInfo = false)
    {
        if (!bCommonInfo)
        {
            this.viewthisID = ulong.Parse(data._obj.thisid);
        }
        uint tradetime = data._obj.tradetime;
        uint currServerTimeBySecond = SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        if (tradetime > currServerTimeBySecond)
        {
            UIInformationList component = this.PanelItemInfo.Find("notrade/txt_notradetime").GetComponent<UIInformationList>();
            if (component != null)
            {
                uint num = tradetime - currServerTimeBySecond;
                if (num < 60U)
                {
                    this.PanelItemInfo.Find("notrade/txt_notradetime").GetComponent<Text>().text = string.Format(component.listInformation[3].content, new object[0]);
                }
                else if (num < 3600U)
                {
                    this.PanelItemInfo.Find("notrade/txt_notradetime").GetComponent<Text>().text = string.Format(component.listInformation[2].content, num / 60U);
                }
                else if (num < 86400U)
                {
                    uint num2 = num / 3600U;
                    num %= 3600U;
                    this.PanelItemInfo.Find("notrade/txt_notradetime").GetComponent<Text>().text = string.Format(component.listInformation[1].content, num2, num / 60U);
                }
                else
                {
                    uint num3 = num / 86400U;
                    num %= 86400U;
                    this.PanelItemInfo.Find("notrade/txt_notradetime").GetComponent<Text>().text = string.Format(component.listInformation[0].content, num3, num / 3600U);
                }
                this.PanelItemInfo.Find("notrade").gameObject.SetActive(true);
            }
        }
        else
        {
            this.PanelItemInfo.Find("notrade").gameObject.SetActive(false);
        }
        if (!string.IsNullOrEmpty(data.config.GetField_String("refence_desc")))
        {
            this.PanelItemInfo.Find("txt_itemquto").gameObject.SetActive(true);
            this.PanelItemInfo.Find("txt_itemquto").GetComponent<Text>().text = data.config.GetField_String("refence_desc");
        }
        else
        {
            this.PanelItemInfo.Find("txt_itemquto").gameObject.SetActive(false);
        }
        this.PanelItemInfo.Find("txt_itemdetail").GetComponent<Text>().text = data.config.GetField_String("desc");
        this.SetItemBaseInfo(this.PanelItemInfo.Find("baseinfo"), data);
        if (!bCommonInfo)
        {
            this.InitBtnAction(this.PanelItemInfo.Find("btninfo"), data);
        }
        else
        {
            this.PanelItemInfo.Find("btninfo").gameObject.SetActive(false);
        }
        this.setLastTime(this.PanelItemInfo.Find("lasttime"), data);
    }

    private void setFightValue(Transform fighttran, PropsBase data)
    {
        if (data.FightValue == 0U)
        {
            fighttran.gameObject.SetActive(false);
            return;
        }
        fighttran.gameObject.SetActive(true);
        fighttran.transform.Find("txt_value").GetComponent<Text>().text = data.FightValue.ToString();
    }

    private void setAttribut(Transform attributTran, PropsBase data)
    {
        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            attributTran.Find("txt_att" + i.ToString()).gameObject.SetActive(false);
            if (num < data._obj.equiprand.Count)
            {
                if (data._obj.equiprand[num].type <= Equip8Prop.PROP_2)
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("equip_suffix", (ulong)data._obj.equiprand[num].id);
                    if (configTable == null)
                    {
                        FFDebug.LogWarning(this, "    config  is  null ");
                    }
                    else
                    {
                        attributTran.Find("txt_att" + i.ToString()).gameObject.SetActive(true);
                        attributTran.Find("txt_att" + i.ToString()).GetComponent<Text>().text = string.Format(configTable.GetField_String("property_explain"), data._obj.equiprand[num].value);
                        num++;
                    }
                }
            }
        }
        for (int j = 0; j < 4; j++)
        {
            attributTran.Find("txt_affix" + j.ToString()).gameObject.SetActive(false);
            if (num < data._obj.equiprand.Count)
            {
                if (data._obj.equiprand[num].type <= Equip8Prop.SUFFIX_4 && data._obj.equiprand[num].type > Equip8Prop.PROP_2)
                {
                    LuaTable configTable2 = LuaConfigManager.GetConfigTable("equip_suffix", (ulong)data._obj.equiprand[num].id);
                    if (configTable2 == null)
                    {
                        FFDebug.LogWarning(this, "    config  is  null ");
                    }
                    else
                    {
                        attributTran.Find("txt_affix" + j.ToString()).gameObject.SetActive(true);
                        attributTran.Find("txt_affix" + j.ToString()).GetComponent<Text>().text = string.Format(configTable2.GetField_String("property_explain"), data._obj.equiprand[num].value);
                        num++;
                    }
                }
            }
        }
    }

    private void setLastTime(Transform lastTimeParent, PropsBase data)
    {
        lastTimeParent.gameObject.SetActive(true);
        if (data._obj.timer <= 0U)
        {
            lastTimeParent.Find("img_bg").gameObject.SetActive(false);
            lastTimeParent.Find("txt_lasttime").gameObject.SetActive(false);
        }
        else
        {
            UIInformationList component = lastTimeParent.Find("txt_lasttime").GetComponent<UIInformationList>();
            if (component == null)
            {
                FFDebug.LogWarning(this, " uiInformation  is  Null");
                return;
            }
            lastTimeParent.Find("img_bg").gameObject.SetActive(true);
            lastTimeParent.Find("txt_lasttime").gameObject.SetActive(true);
            if (data._obj.timer > SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond())
            {
                ulong time = (ulong)(data._obj.timer - SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond());
                uint dayBySecond = SingletonForMono<GameTime>.Instance.GetDayBySecond(time);
                uint horBySecond = SingletonForMono<GameTime>.Instance.GetHorBySecond(time);
                uint minBySecond = SingletonForMono<GameTime>.Instance.GetMinBySecond(time);
                if (dayBySecond > 0U)
                {
                    lastTimeParent.Find("txt_lasttime").GetComponent<Text>().text = string.Format(component.listInformation[0].content, dayBySecond, horBySecond);
                }
                else if (horBySecond > 0U)
                {
                    lastTimeParent.Find("txt_lasttime").GetComponent<Text>().text = string.Format(component.listInformation[1].content, horBySecond, minBySecond);
                }
                else
                {
                    lastTimeParent.Find("txt_lasttime").GetComponent<Text>().text = string.Format(component.listInformation[2].content, minBySecond);
                }
            }
            else
            {
                lastTimeParent.Find("txt_lasttime").GetComponent<Text>().text = component.listInformation[3].content;
            }
        }
    }

    private void SetItemBaseInfo(Transform TranBase, PropsBase data)
    {
        TranBase.Find("txt_bind").gameObject.SetActive(true);
        UIInformationList component = TranBase.Find("txt_bind").GetComponent<UIInformationList>();
        if (component != null)
        {
            if (component.listInformation.Count < 2)
            {
                FFDebug.LogWarning(this, "uiinformation  is error");
            }
        }
        else
        {
            FFDebug.LogWarning(this, "uiinformation  is null");
        }
        if (data._obj.bind == 1U)
        {
            TranBase.Find("txt_bind").GetComponent<Text>().text = component.listInformation[0].content;
        }
        else if (data._obj.bind == 2U)
        {
            TranBase.Find("txt_bind").GetComponent<Text>().text = component.listInformation[1].content;
        }
        else
        {
            TranBase.Find("txt_bind").gameObject.SetActive(false);
        }
        TranBase.Find("txt_itemname").GetComponent<Text>().text = data._obj.name;
        switch (data._obj.quality)
        {
            case 1U:
                {
                    string modelColor = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item1");
                    TranBase.Find("txt_itemname").GetComponent<Text>().color = CommonTools.HexToColor(modelColor);
                    break;
                }
            case 2U:
                {
                    string modelColor2 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item2");
                    TranBase.Find("txt_itemname").GetComponent<Text>().color = CommonTools.HexToColor(modelColor2);
                    break;
                }
            case 3U:
                {
                    string modelColor3 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item3");
                    TranBase.Find("txt_itemname").GetComponent<Text>().color = CommonTools.HexToColor(modelColor3);
                    break;
                }
            case 4U:
                {
                    string modelColor4 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item4");
                    TranBase.Find("txt_itemname").GetComponent<Text>().color = CommonTools.HexToColor(modelColor4);
                    break;
                }
        }
        Transform transform = TranBase.Find("txt_itemtype");
        if (transform != null)
        {
            transform.GetComponent<Text>().text = data.config.GetField_String("type_desc");
        }
        Text component2 = TranBase.Find("txt_itemlv").GetComponent<Text>();
        Text component3 = TranBase.Find("txt_lv").GetComponent<Text>();
        if (data._obj.level < 2U)
        {
            component2.gameObject.SetActive(false);
            component3.gameObject.SetActive(false);
        }
        else
        {
            component2.gameObject.SetActive(true);
            component3.gameObject.SetActive(true);
            component3.text = data._obj.level.ToString();
        }
        if (MainPlayer.Self.GetCurLevel() < data._obj.level)
        {
            component3.color = Const.GetColorByName("itemlimit");
        }
        else
        {
            component3.color = Const.GetColorByName("itemnormal");
        }
        if (data.config == null)
        {
            FFDebug.LogWarning(this, "objConfig   is  null  id:" + data._obj.baseid);
            return;
        }
        CommonItem commonItem = new CommonItem(TranBase.Find("Item"));
        commonItem.SetCommonItem(data._obj.baseid, 1U, null);
        this.commonItems.Add(commonItem);
    }

    private void InitBtnAction(Transform gaBtnInfo, PropsBase data)
    {
        gaBtnInfo.gameObject.SetActive(true);
        uint type = (uint)data._obj.type;
        this.otherActionDat = data;
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("btnaction").GetCacheField_Table("btnactionmapbyid").GetCacheField_Table(data._obj.baseid.ToString());
        if (cacheField_Table == null)
        {
            cacheField_Table = LuaConfigManager.GetXmlConfigTable("btnaction").GetCacheField_Table("btnactionmap").GetCacheField_Table(type.ToString());
        }
        if (cacheField_Table == null)
        {
            gaBtnInfo.gameObject.SetActive(false);
            return;
        }
        string cacheField_String = cacheField_Table.GetCacheField_String("btns");
        string cacheField_String2 = cacheField_Table.GetCacheField_String("actions");
        string[] array = cacheField_String.Split(new char[]
        {
            '-'
        }, StringSplitOptions.RemoveEmptyEntries);
        string[] array2 = cacheField_String2.Split(new char[]
        {
            '-'
        }, StringSplitOptions.RemoveEmptyEntries);
        if (array.Length != array2.Length)
        {
            FFDebug.LogWarning(this, "btnaction.xml   is  error  ");
        }
        GameObject gameObject = gaBtnInfo.Find("btn_use").gameObject;
        GameObject gameObject2 = gaBtnInfo.Find("btn_destroy").gameObject;
        GameObject other = gaBtnInfo.Find("btn_other").gameObject;
        GameObject otherList = gaBtnInfo.Find("Panel_Btnlist").gameObject;
        gameObject.SetActive(true);
        gameObject2.SetActive(true);
        other.SetActive(true);
        otherList.SetActive(true);
        if (array.Length == 0)
        {
            gaBtnInfo.gameObject.SetActive(false);
        }
        else
        {
            gaBtnInfo.gameObject.SetActive(true);
            if (array.Length == 1)
            {
                gameObject2.SetActive(false);
                other.SetActive(false);
                otherList.SetActive(false);
                gameObject.transform.Find("Text").GetComponent<Text>().text = array[0];
                UIEventListener.Get(gameObject).onClick = this.DicBtnAction[array2[0]];
            }
            else if (array.Length == 2)
            {
                other.SetActive(false);
                otherList.SetActive(false);
                gameObject.transform.Find("Text").GetComponent<Text>().text = array[0];
                gameObject2.transform.Find("Text").GetComponent<Text>().text = array[1];
                UIEventListener.Get(gameObject).onClick = this.DicBtnAction[array2[0]];
                UIEventListener.Get(gameObject2).onClick = this.DicBtnAction[array2[1]];
            }
            else
            {
                gameObject2.SetActive(false);
                otherList.SetActive(false);
                gameObject.transform.Find("Text").GetComponent<Text>().text = array[0];
                other.transform.Find("img_icon").transform.localRotation = new Quaternion(180f, 0f, 0f, 0f);
                UIEventListener.Get(gameObject).onClick = this.DicBtnAction[array2[0]];
                GameObject gameObject3 = otherList.transform.Find("btn_other").gameObject;
                int childCount = otherList.transform.childCount;
                for (int i = 1; i < array.Length; i++)
                {
                    if (i - 1 < childCount)
                    {
                        otherList.transform.GetChild(i - 1).Find("Text").GetComponent<Text>().text = array[i];
                        if (this.DicBtnAction.ContainsKey(array2[i]))
                        {
                            UIEventListener.Get(otherList.transform.GetChild(i - 1).gameObject).onClick = this.DicBtnAction[array2[i]];
                        }
                    }
                    else
                    {
                        GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
                        gameObject4.transform.SetParent(otherList.transform);
                        gameObject4.transform.localScale = gameObject3.transform.localScale;
                        gameObject4.transform.Find("Text").GetComponent<Text>().text = array[i];
                        if (this.DicBtnAction.ContainsKey(array2[i]))
                        {
                            this.otherActionDat = data;
                            UIEventListener.Get(gameObject4).onClick = this.DicBtnAction[array2[i]];
                        }
                    }
                }
                if (childCount > array.Length - 1)
                {
                    for (int j = array.Length - 1; j < childCount; j++)
                    {
                        otherList.transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
                UIEventListener.Get(other).onClick = delegate (PointerEventData eventData)
                {
                    otherList.SetActive(!otherList.activeSelf);
                    if (otherList.activeSelf)
                    {
                        other.transform.Find("img_icon").transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                    else
                    {
                        other.transform.Find("img_icon").transform.localRotation = new Quaternion(180f, 0f, 0f, 0f);
                    }
                };
            }
        }
    }

    public void ViewInfo(PropsBase data, bool isequip)
    {
        if (this.viewthisID == ulong.Parse(data._obj.thisid))
        {
            return;
        }
        if (isequip)
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.EquipContrast);
            this.ViewEquipContrastInfo(data, false);
        }
        else if (data._obj.type == ObjectType.OBJTYPE_RUNE)
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.Rune);
            this.viewRuneInfo(data, 0, null, null);
        }
        else if ((data._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && data._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX) || (data._obj.type >= ObjectType.OBJTYPE_EQUIP_MIN && data._obj.type <= ObjectType.OBJTYPE_EQUIP_MAX))
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.Equip);
            this.ViewEquipInfo(data);
        }
        else
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.Item);
            this.viewItemInfo(data, false);
        }
        this.mRoot.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void ViewInfoCommon(PropsBase data, Transform tran = null)
    {
        this.btnTran = tran;
        if (data._obj.type == ObjectType.OBJTYPE_RUNE)
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.Rune);
            this.viewRuneInfo(data, -1, null, null);
            if (tran != null)
            {
                (this.PanelRuneInfo as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
                if (!this.lockScheduler)
                {
                    Scheduler.Instance.AddTimer(0.1f, false, new Scheduler.OnScheduler(this.SetItemInfoPos));
                    this.lockScheduler = true;
                }
            }
        }
        else if ((data._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && data._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX) || (data._obj.type >= ObjectType.OBJTYPE_EQUIP_MIN && data._obj.type <= ObjectType.OBJTYPE_EQUIP_MAX))
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.EquipContrast);
            this.ViewEquipContrastInfo(data, true);
            if (tran != null)
            {
                (this.PanelEquipContrastInfo as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
                if (!this.lockScheduler)
                {
                    Scheduler.Instance.AddTimer(0.1f, false, new Scheduler.OnScheduler(this.SetEquipContrastInfoPos));
                    this.lockScheduler = true;
                }
            }
        }
        else
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.Item);
            this.viewItemInfo(data, true);
            if (tran != null)
            {
                (this.PanelItemInfo as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
                if (!this.lockScheduler)
                {
                    Scheduler.Instance.AddTimer(0.1f, false, new Scheduler.OnScheduler(this.SetItemInfoPos));
                    this.lockScheduler = true;
                }
            }
        }
        this.mRoot.GetComponent<RectTransform>().SetAsLastSibling();
    }

    private void SetEquipContrastInfoPos()
    {
        this.lockScheduler = false;
        this.SetItemPos(false);
    }

    private void SetItemInfoPos()
    {
        this.lockScheduler = false;
        this.SetItemPos(true);
    }

    private void SetItemPos(bool isItemInfo)
    {
        RectTransform rectTransform = new RectTransform();
        if (isItemInfo)
        {
            if (this.uiState == UI_EquipItemInfo.ItemInfoState.Rune || this.uiState == UI_EquipItemInfo.ItemInfoState.RuneActivate)
            {
                rectTransform = (this.PanelRuneInfo as RectTransform);
            }
            else
            {
                rectTransform = (this.PanelItemInfo as RectTransform);
            }
        }
        else
        {
            rectTransform = (this.PanelEquipContrastInfo as RectTransform);
        }
        Vector2 vector = RectTransformUtility.WorldToScreenPoint(ManagerCenter.Instance.GetManager<UIManager>().GetUICamera().GetComponent<Camera>(), this.btnTran.position);
        RectTransform rectTransform2;
        if (this.btnTran.Find("img_bg") != null)
        {
            rectTransform2 = (this.btnTran.Find("img_bg") as RectTransform);
        }
        else
        {
            rectTransform2 = (this.btnTran.Find("btn_icon") as RectTransform);
        }
        vector += new Vector2(rectTransform2.sizeDelta.x / 2f, rectTransform2.sizeDelta.y / 2f);
        Vector2 vector2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.PanelEquipContrastInfo.parent as RectTransform, vector, ManagerCenter.Instance.GetManager<UIManager>().GetUICamera().GetComponent<Camera>(), out vector2);
        if (rectTransform.sizeDelta.x < (float)Screen.width - vector.x && rectTransform.sizeDelta.y < (float)Screen.height - vector.y)
        {
            if (isItemInfo)
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x + rectTransform.sizeDelta.x / 2f - 20f, vector2.y + rectTransform.sizeDelta.y / 2f - 15f);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x + rectTransform.sizeDelta.x / 2f - 20f, vector2.y + rectTransform.sizeDelta.y - 15f);
            }
            rectTransform.gameObject.SetActive(true);
            return;
        }
        if (rectTransform.sizeDelta.x < (float)Screen.width - vector.x && rectTransform.sizeDelta.y < vector.y)
        {
            if (isItemInfo)
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x + rectTransform.sizeDelta.x / 2f - 20f, vector2.y - rectTransform2.sizeDelta.y + 5f - rectTransform.sizeDelta.y / 2f);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x + rectTransform.sizeDelta.x / 2f - 20f, vector2.y - rectTransform2.sizeDelta.y + 5f);
            }
            rectTransform.gameObject.SetActive(true);
            return;
        }
        if (rectTransform.sizeDelta.x < vector.x && rectTransform.sizeDelta.y < (float)Screen.height - vector.y)
        {
            if (isItemInfo)
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x - rectTransform.sizeDelta.x / 2f - rectTransform2.sizeDelta.x, vector2.y + rectTransform.sizeDelta.y / 2f - 15f);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x - rectTransform.sizeDelta.x / 2f - rectTransform2.sizeDelta.x, vector2.y + rectTransform.sizeDelta.y - 15f);
            }
            rectTransform.gameObject.SetActive(true);
            return;
        }
        if (rectTransform.sizeDelta.x < vector.x && rectTransform.sizeDelta.y < vector.y)
        {
            if (isItemInfo)
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x - rectTransform.sizeDelta.x / 2f - rectTransform2.sizeDelta.x, vector2.y - rectTransform2.sizeDelta.y + 5f - rectTransform.sizeDelta.y / 2f);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x - rectTransform.sizeDelta.x / 2f - rectTransform2.sizeDelta.x, vector2.y - rectTransform2.sizeDelta.y + 5f);
            }
            rectTransform.gameObject.SetActive(true);
            return;
        }
        if (rectTransform.sizeDelta.x < (float)Screen.width - vector.x && rectTransform.sizeDelta.y > (float)Screen.height - vector.y && rectTransform.sizeDelta.y > vector.y)
        {
            if (isItemInfo)
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x + rectTransform.sizeDelta.x / 2f - 20f, 0f);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x + rectTransform.sizeDelta.x / 2f - 20f, rectTransform.sizeDelta.y / 2f);
            }
            rectTransform.gameObject.SetActive(true);
            return;
        }
        if (rectTransform.sizeDelta.x < vector.x && rectTransform.sizeDelta.y > (float)Screen.height - vector.y && rectTransform.sizeDelta.y > vector.y)
        {
            if (isItemInfo)
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x - rectTransform.sizeDelta.x / 2f - rectTransform2.sizeDelta.x, 0f);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(vector2.x - rectTransform.sizeDelta.x / 2f - rectTransform2.sizeDelta.x, rectTransform.sizeDelta.y / 2f);
            }
            rectTransform.gameObject.SetActive(true);
            return;
        }
    }

    private void setUIState(UI_EquipItemInfo.ItemInfoState state)
    {
        this.uiState = state;
        switch (this.uiState)
        {
            case UI_EquipItemInfo.ItemInfoState.Item:
                this.PanelItemInfo.gameObject.SetActive(true);
                this.PanelEquipContrastInfo.gameObject.SetActive(false);
                this.PanelEquipInfo.gameObject.SetActive(false);
                this.PanelRuneInfo.gameObject.SetActive(false);
                break;
            case UI_EquipItemInfo.ItemInfoState.Equip:
                this.PanelEquipContrastInfo.gameObject.SetActive(false);
                this.PanelItemInfo.gameObject.SetActive(false);
                this.PanelEquipInfo.gameObject.SetActive(true);
                this.PanelRuneInfo.gameObject.SetActive(false);
                break;
            case UI_EquipItemInfo.ItemInfoState.EquipContrast:
                this.PanelItemInfo.gameObject.SetActive(false);
                this.PanelEquipContrastInfo.gameObject.SetActive(true);
                this.PanelEquipInfo.gameObject.SetActive(false);
                this.PanelRuneInfo.gameObject.SetActive(false);
                break;
            case UI_EquipItemInfo.ItemInfoState.Rune:
            case UI_EquipItemInfo.ItemInfoState.RuneActivate:
                this.PanelItemInfo.gameObject.SetActive(false);
                this.PanelEquipContrastInfo.gameObject.SetActive(false);
                this.PanelEquipInfo.gameObject.SetActive(false);
                this.PanelRuneInfo.gameObject.SetActive(true);
                break;
        }
    }

    private void equipObject(PointerEventData dataevent)
    {
        if (this.otherActionDat != null)
        {
            FFDebug.Log(this, FFLogType.Equip, string.Format("equip id = {0} name = {0}", this.otherActionDat._obj.baseid, this.otherActionDat._obj.name));
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.WearEquipCS", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                this.otherActionDat
            });
        }
    }

    private void SellObject(PointerEventData dataEvent)
    {
        if (this.otherActionDat != null)
        {
            this.PanelItemInfo.gameObject.SetActive(false);
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ProcessSellItem", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                this.otherActionDat
            });
        }
    }

    private void useObject(PointerEventData dataevent)
    {
        if (this.otherActionDat != null)
        {
            FFDebug.Log(this, FFLogType.Equip, "使用:  " + this.otherActionDat._obj.name);
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                this.otherActionDat,
                1
            });
        }
    }

    private void splitObject(PointerEventData dataevent)
    {
        if (this.otherActionDat != null)
        {
            FFDebug.Log(this, FFLogType.Equip, "拆分:  " + this.otherActionDat._obj.name);
        }
    }

    private void destoryObject(PointerEventData dataevent)
    {
        if (this.otherActionDat != null)
        {
            FFDebug.Log(this, FFLogType.Equip, "丢弃:  " + this.otherActionDat._obj.name);
        }
    }

    private void ActivateObject(PointerEventData data)
    {
        if (this.otherActionDat != null)
        {
            ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
            if (controller != null)
            {
                controller.ShowTreasureHunt(this.otherActionDat);
            }
            GlobalRegister.CloseInfo();
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseBagUI", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        }
    }

    public void OpenMyRuneViewInfo(Transform tran, LuaTable data, string thisidstr, int type, Action btn1evt, Action btn2evt)
    {
        this.btnTran = tran;
        this.setUIState(UI_EquipItemInfo.ItemInfoState.Rune);
        if (tran != null)
        {
            (this.PanelRuneInfo as RectTransform).anchoredPosition = new Vector2(10000f, 10000f);
            if (!this.lockScheduler)
            {
                Scheduler.Instance.AddTimer(0.1f, false, new Scheduler.OnScheduler(this.SetItemInfoPos));
                this.lockScheduler = true;
            }
        }
        if (type < 2)
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.Rune);
            ulong thisid = ulong.Parse(thisidstr);
            if (!(bool)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.MainPackageDicContainsKey", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                thisidstr
            })[0])
            {
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseInfo", new object[]
                {
                    Util.GetLuaTable("BagCtrl")
                });
            }
            this.SetRuneBaseInfo((PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.MainPackageDic", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                thisidstr
            })[0], null);
            this.SetRuneBtn(type, thisid, btn1evt, btn2evt);
        }
        else
        {
            this.setUIState(UI_EquipItemInfo.ItemInfoState.RuneActivate);
            this.SetRuneBaseInfo(null, data);
            this.SetRuneBtn(type, 0UL, btn1evt, btn2evt);
        }
        this.mRoot.GetComponent<RectTransform>().SetAsLastSibling();
    }

    private void viewRuneInfo(PropsBase data, int type, Action btn1evt, Action btn2evt)
    {
        this.SetRuneBaseInfo(data, null);
        this.SetRuneBtn(type, ulong.Parse(data._obj.thisid), btn1evt, btn2evt);
    }

    private void SetRuneBaseInfo(PropsBase Pdata, LuaTable Ldata)
    {
        uint num = 0U;
        uint num2;
        uint num3;
        if (Pdata != null)
        {
            num2 = Pdata.config.GetField_Uint("id");
            num3 = 1U;
        }
        else
        {
            if (Ldata == null)
            {
                return;
            }
            num2 = (uint)((double)Ldata["baseid"]);
            num3 = (uint)((double)Ldata["level"]);
            Pdata = new PropsBase(num2, 1U);
            num = (uint)((double)Ldata["exp"]);
        }
        uint num4 = num2 * 1000U + num3;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)num2);
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("runeConfig", (ulong)num4);
        int runeStrength = this.GetRuneStrength(num2, num3);
        this.SetItemBaseInfo(this.PanelRuneInfo.Find("baseinfo"), Pdata);
        this.PanelRuneInfo.Find("baseinfo/txt_lv").gameObject.SetActive(this.uiState == UI_EquipItemInfo.ItemInfoState.RuneActivate);
        this.PanelRuneInfo.Find("baseinfo/txt_itemlv").gameObject.SetActive(this.uiState == UI_EquipItemInfo.ItemInfoState.RuneActivate);
        this.PanelRuneInfo.Find("baseinfo/txt_lv").GetComponent<Text>().text = num3.ToString();
        this.PanelRuneInfo.Find("baseinfo/txt_bind").gameObject.SetActive(this.uiState == UI_EquipItemInfo.ItemInfoState.RuneActivate);
        this.PanelRuneInfo.Find("fightstrength/txt_value").GetComponent<Text>().text = runeStrength.ToString();
        this.PanelRuneInfo.Find("exp/slider_exp").gameObject.SetActive(this.uiState == UI_EquipItemInfo.ItemInfoState.RuneActivate);
        this.PanelRuneInfo.Find("exp/txt_value").gameObject.SetActive(this.uiState == UI_EquipItemInfo.ItemInfoState.Rune);
        if (this.uiState == UI_EquipItemInfo.ItemInfoState.RuneActivate)
        {
            LuaTable configTable3 = LuaConfigManager.GetConfigTable("runeConfig", (ulong)(num4 + 1U));
            if (configTable3 == null)
            {
                this.PanelRuneInfo.Find("exp/slider_exp").GetComponent<Slider>().value = 1f;
                this.PanelRuneInfo.Find("exp/slider_exp/txt_count").GetComponent<Text>().text = configTable2.GetField_Uint("exp") + "/" + configTable2.GetField_Uint("exp");
            }
            else
            {
                this.PanelRuneInfo.Find("exp/slider_exp").GetComponent<Slider>().value = num / (configTable3.GetField_Uint("exp") - configTable2.GetField_Uint("exp"));
                this.PanelRuneInfo.Find("exp/slider_exp/txt_count").GetComponent<Text>().text = num + "/" + (configTable3.GetField_Uint("exp") - configTable2.GetField_Uint("exp"));
            }
        }
        else
        {
            uint num5 = num2 * 1000U + 1U;
            LuaTable configTable4 = LuaConfigManager.GetConfigTable("runeConfig", (ulong)num5);
            if (configTable4 != null)
            {
                this.PanelRuneInfo.Find("exp/txt_value").GetComponent<Text>().text = configTable4.GetField_Uint("exp").ToString();
            }
        }
        this.SetattributeText(this.PanelRuneInfo.Find("attribute/txt_att0").GetComponent<Text>(), this.PanelRuneInfo.Find("attribute/txt_att1").GetComponent<Text>(), this.PanelRuneInfo.Find("attribute/txt_att2").GetComponent<Text>(), configTable2);
    }

    public int GetRuneStrength(uint baseid, uint lv)
    {
        int result = 0;
        uint num = baseid * 1000U + lv;
        LuaTable configTable = LuaConfigManager.GetConfigTable("runeConfig", (ulong)num);
        if (configTable != null)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("rune").GetCacheField_Table("levelcoe").GetCacheField_Table(lv.ToString());
            if (cacheField_Table != null)
            {
                result = (int)(configTable.GetCacheField_Float("qualitycoe") / 10000f * (float)cacheField_Table.GetCacheField_Int("value"));
            }
        }
        return result;
    }

    private void ActiveRune(ulong thisid)
    {
        if (this._activeRune == null)
        {
            this._activeRune = LuaScriptMgr.Instance.GetLuaFunction("RuneNetWork.MSG_Req_ActiveRune_CS");
        }
        if (this._runeNetTable == null)
        {
            this._runeNetTable = Util.GetLuaTable("RuneNetWork");
        }
        this._activeRune.Call(new object[]
        {
            this._runeNetTable,
            thisid.ToString()
        });
    }

    private void SetRuneBtn(int type, ulong thisid, Action btn1evt, Action btn2evt)
    {
        Transform transform = this.PanelRuneInfo.Find("btninfo");
        Button component = this.PanelRuneInfo.Find("btninfo/btn_upgrade").GetComponent<Button>();
        Button component2 = this.PanelRuneInfo.Find("btninfo/btn_unload").GetComponent<Button>();
        component2.gameObject.SetActive(type > 0);
        if (type < 0)
        {
            component.gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
            return;
        }
        if (type == 0)
        {
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component.transform.Find("Text").GetComponent<Text>(), string.Empty, 1);
        }
        else if (type == 1)
        {
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component.transform.Find("Text").GetComponent<Text>(), string.Empty, 1);
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component2.transform.Find("Text").GetComponent<Text>(), string.Empty, 1);
        }
        else if (type == 2)
        {
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component.transform.Find("Text").GetComponent<Text>(), string.Empty, 0);
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component2.transform.Find("Text").GetComponent<Text>(), string.Empty, 1);
        }
        else if (type == 3)
        {
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component.transform.Find("Text").GetComponent<Text>(), string.Empty, 0);
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(component2.transform.Find("Text").GetComponent<Text>(), string.Empty, 0);
        }
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            if (type < 2)
            {
                this.ActiveRune(thisid);
            }
            if (btn1evt != null)
            {
                btn1evt();
            }
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseInfo", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        });
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(delegate ()
        {
            if (btn2evt != null)
            {
                btn2evt();
            }
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseInfo", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        });
    }

    private void SetattributeText(Text txt1, Text txt2, Text txt3, LuaTable runeconfig)
    {
        string[] array = runeconfig.GetField_String("property").Split(new char[]
        {
            ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        this.Setattribute.clear();
        this.Setattribute.addBystring(runeconfig.GetField_String("property"));
        txt1.gameObject.SetActive(false);
        txt2.gameObject.SetActive(false);
        txt3.gameObject.SetActive(false);
        int num = 0;
        string[] array2 = new string[]
        {
            string.Empty,
            "生命上限",
            "格斗防御",
            "气功防御",
            "格斗攻击",
            "气功攻击",
            "暴击等级",
            "格挡等级",
            "韧性等级",
            "穿透等级"
        };
        for (int i = 0; i < this.Setattribute.list.Count; i++)
        {
            uint num2 = this.Setattribute.list[i];
            if (num2 > 0U)
            {
                Text text = null;
                if (num == 0)
                {
                    text = txt1;
                }
                else if (num == 1)
                {
                    text = txt2;
                }
                else if (num == 2)
                {
                    text = txt3;
                }
                num++;
                text.gameObject.SetActive(true);
                text.text = array2[i] + ":" + num2;
            }
        }
    }

    private Transform mRoot;

    private Transform PanelItemInfo;

    private Transform PanelEquipInfo;

    private Transform PanelEquipContrastInfo;

    private Transform PanelRuneInfo;

    private Dictionary<string, UIEventListener.VoidDelegate> DicBtnAction = new Dictionary<string, UIEventListener.VoidDelegate>();

    private Transform btnTran;

    private bool lockScheduler;

    private List<CommonItem> commonItems = new List<CommonItem>();

    private static List<RaycastResult> overuiresults = new List<RaycastResult>();

    private ulong viewthisID;

    private UI_EquipItemInfo.ItemInfoState uiState;

    private PropsBase otherActionDat;

    private LuaFunction _activeRune;

    private LuaTable _runeNetTable;

    private RuneDataSetattribute Setattribute = new RuneDataSetattribute();

    private enum ItemInfoState
    {
        Item,
        Equip,
        EquipContrast,
        Rune,
        RuneActivate
    }
}
