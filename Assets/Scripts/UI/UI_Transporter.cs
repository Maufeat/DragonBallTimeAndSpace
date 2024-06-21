using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_Transporter : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.panelRoot = root.Find("Offset_Root/Panel_Root");
        this.InitObject();
        this.InitEvent();
        this.InitNpcIcon();
        this.posOpenUI = MainPlayer.Self.NextPosition2D;
        base.RegOpenUIByNpc(string.Empty);
    }

    private void InitObject()
    {
        this.itemLstRoot = this.panelRoot.Find("background/Panel_list");
        this.panelIcon = this.panelRoot.Find("background/Panel_npc_icon");
        this.btnClose = this.panelRoot.Find("background/btn_close");
    }

    private void InitEvent()
    {
        Button component = this.btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            UIManager.Instance.DeleteUI<UI_Transporter>();
        });
        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().RegSetSelcetTargetNullCallBack(new Action<CharactorBase>(this.OnSelectNpcNull));
    }

    private void OnSelectNpcNull(CharactorBase charBase)
    {
        if (UIManager.GetUIObject<UI_Transporter>() != null)
        {
            UIManager.Instance.DeleteUI<UI_Transporter>();
        }
    }

    private void InitNpcIcon()
    {
        RawImage component = this.panelIcon.Find("img_npcicon").GetComponent<RawImage>();
        Text component2 = this.panelIcon.Find("img_npcname/txt_npcname").GetComponent<Text>();
        component2.text = GlobalRegister.GetNpcTalkName();
        FFDetectionNpcControl component3 = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        ulong currentVisteNpcID = component3.CurrentVisteNpcID;
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            Npc npc = manager.GetNpc(currentVisteNpcID);
            if (npc != null)
            {
                GlobalRegister.ShowNpcOrPlayerRTT(component, npc.NpcData.GetAppearanceid(), 0, null);
            }
        }
    }

    public void EndAddItem(List<uint> showItemIds)
    {
        this.FrashTansporterItemList(showItemIds);
    }

    private void FrashTansporterItemList(List<uint> showItemIds)
    {
        if (showItemIds.Count != 0)
        {
            Transform transform = this.itemLstRoot.Find("ScrollbarRect/Rect");
            GameObject gameObject = transform.GetChild(0).gameObject;
            int num = Mathf.Max(transform.childCount, showItemIds.Count);
            RawImage component = gameObject.FindChild("img_icon").gameObject.GetComponent<RawImage>();
            component.texture = null;
            component.color = Color.clear;
            for (int i = 0; i < num; i++)
            {
                int index = i;
                GameObject gameObject2;
                if (index < transform.childCount)
                {
                    gameObject2 = transform.GetChild(index).gameObject;
                }
                else
                {
                    gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.transform.SetParent(transform, false);
                }
                if (index < showItemIds.Count)
                {
                    gameObject2.gameObject.SetActive(true);
                    LuaTable configTable = LuaConfigManager.GetConfigTable("teleport", (ulong)showItemIds[i]);
                    if (configTable != null)
                    {
                        Text component2 = gameObject2.transform.Find("txt_place").GetComponent<Text>();
                        component2.text = configTable.GetCacheField_String("Desc");
                        string cacheField_String = configTable.GetCacheField_String("cost");
                        Transform transform2 = gameObject2.transform.Find("img_icon_cost_stone");
                        Transform transform3 = gameObject2.transform.Find("img_icon_cost_gold");
                        Text component3 = gameObject2.transform.Find("txt_cost_value").GetComponent<Text>();
                        if (!string.IsNullOrEmpty(cacheField_String))
                        {
                            string[] array = cacheField_String.Split(new char[]
                            {
                                '-'
                            });
                            if (array.Length > 1)
                            {
                                component3.text = array[1];
                                if ("2".Equals(array[0]))
                                {
                                    transform2.gameObject.SetActive(true);
                                    transform3.gameObject.SetActive(false);
                                }
                                else
                                {
                                    transform2.gameObject.SetActive(false);
                                    transform3.gameObject.SetActive(true);
                                }
                            }
                        }
                        Button component4 = gameObject2.GetComponent<Button>();
                        component4.onClick.RemoveAllListeners();
                        component4.onClick.AddListener(delegate ()
                        {
                            this.OnClickTeleportItem(showItemIds[index]);
                        });
                    }
                }
                else
                {
                    gameObject2.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnClickTeleportItem(uint id)
    {
        TransporterController controller = ControllerManager.Instance.GetController<TransporterController>();
        if (controller != null)
        {
            controller.ReqTeleport(id);
        }
        UIManager.Instance.DeleteUI<UI_Transporter>();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().UnRegSetSelcetTargetNullCallBack(new Action<CharactorBase>(this.OnSelectNpcNull));
    }

    private Transform panelRoot;

    private Transform itemLstRoot;

    private Transform panelIcon;

    private Transform btnClose;

    private Vector2 posOpenUI;
}
