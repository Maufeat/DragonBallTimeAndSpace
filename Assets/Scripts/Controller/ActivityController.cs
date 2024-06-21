using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using massive;
using Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityController : ControllerBase
{
    public UI_ActivityGuide UI_ActGuide
    {
        get
        {
            return UIManager.GetUIObject<UI_ActivityGuide>();
        }
    }

    public override string ControllerName
    {
        get
        {
            return "activity_controller";
        }
    }

    public override void Awake()
    {
        base.Awake();
        this.mNetWork = new ActivityNetWork();
        this.mNetWork.Initialize();
    }

    private void InitConfig()
    {
        if (this.menuDatas == null)
        {
            this.menuDatas = new List<ActivityController.Menu>();
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("event").GetCacheField_Table("eventname");
            if (cacheField_Table != null)
            {
                for (int i = 0; i < cacheField_Table.Count; i++)
                {
                    LuaTable luaTable = cacheField_Table[i + 1] as LuaTable;
                    if (luaTable == null)
                    {
                        FFDebug.LogError(this, "event -> eventname -> item==null:" + cacheField_Table.ToString());
                    }
                    else
                    {
                        int field_Int = luaTable.GetField_Int("id");
                        string field_String = luaTable.GetField_String("name");
                        UI_ActivityGuide.MenuSkin skin = this.UI_ActGuide.AddNewMenu(field_String);
                        this.menuDatas.Add(new ActivityController.Menu
                        {
                            id = field_Int,
                            name = field_String,
                            skin = skin
                        });
                    }
                }
                int maxValue = int.MaxValue;
                string name = "即将开启";
                UI_ActivityGuide.MenuSkin skin2 = this.UI_ActGuide.AddNewMenu(name);
                this.menuDatas.Add(new ActivityController.Menu
                {
                    id = maxValue,
                    name = name,
                    skin = skin2
                });
            }
        }
        if (this.menuItemDic == null)
        {
            this.tempMenuItems.Clear();
            this.itemDic = new Dictionary<int, ActivityController.MenuItem>();
            this.menuItemDic = new Dictionary<int, List<ActivityController.MenuItem>>();
            this.unopenDic = new Dictionary<int, UI_ActivityGuide.MenuItemSkin>();
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("event_config");
            if (configTableList != null)
            {
                for (int j = 0; j < configTableList.Count; j++)
                {
                    LuaTable luaTable2 = configTableList[j];
                    if (luaTable2 == null)
                    {
                        FFDebug.LogError(this, "t_event_config -> item==null:" + configTableList.ToString());
                    }
                    else
                    {
                        int field_Int2 = luaTable2.GetField_Int("id");
                        string field_String2 = luaTable2.GetField_String("name");
                        int field_Int3 = luaTable2.GetField_Int("type");
                        int field_Int4 = luaTable2.GetField_Int("timelimit");
                        int field_Int5 = luaTable2.GetField_Int("peractive");
                        int field_Int6 = luaTable2.GetField_Int("level");
                        string field_String3 = luaTable2.GetField_String("desc");
                        string field_String4 = luaTable2.GetField_String("time");
                        string field_String5 = luaTable2.GetField_String("form");
                        string field_String6 = luaTable2.GetField_String("limit");
                        string field_String7 = luaTable2.GetField_String("reward");
                        int field_Int7 = luaTable2.GetField_Int("pathway");
                        string field_String8 = luaTable2.GetField_String("questgroup");
                        string field_String9 = luaTable2.GetField_String("notice");
                        List<ActivityController.MenuItem> list;
                        if (!this.menuItemDic.TryGetValue(field_Int3, out list))
                        {
                            list = new List<ActivityController.MenuItem>();
                            this.menuItemDic.Add(field_Int3, list);
                        }
                        UI_ActivityGuide.MenuItemSkin skin3 = this.UI_ActGuide.AddNewMenuItem(field_Int3, field_String2, field_Int4, field_Int5, field_Int6);
                        ActivityController.MenuItem menuItem = new ActivityController.MenuItem
                        {
                            id = field_Int2,
                            name = field_String2,
                            menuId = field_Int3,
                            timesLimit = field_Int4,
                            perActive = field_Int5,
                            levelLimit = field_Int6,
                            desc = field_String3,
                            time = field_String4,
                            form = field_String5,
                            limit = field_String6,
                            reward = field_String7,
                            pathway = field_Int7,
                            questgroup = field_String8,
                            notice = field_String9,
                            skin = skin3
                        };
                        this.itemDic.Add(field_Int2, menuItem);
                        list.Add(menuItem);
                        this.tempMenuItems.Add(menuItem);
                        UI_ActivityGuide.MenuItemSkin value = this.UI_ActGuide.AddNewMenuItem(int.MaxValue, field_String2, field_Int4, field_Int5, field_Int6);
                        this.unopenDic.Add(field_Int2, value);
                    }
                }
            }
            this.tempMenuItems.Sort(delegate (ActivityController.MenuItem a, ActivityController.MenuItem b)
            {
                if (a.levelLimit > b.levelLimit)
                {
                    return 1;
                }
                if (a.levelLimit == b.levelLimit)
                {
                    return 0;
                }
                return -1;
            });
            for (int k = 0; k < this.tempMenuItems.Count; k++)
            {
                ActivityController.MenuItem menuItem2 = this.tempMenuItems[k];
                menuItem2.skin.skin.transform.SetSiblingIndex(k);
                this.unopenDic[menuItem2.id].skin.transform.SetSiblingIndex(k);
            }
            this.tempMenuItems.Clear();
        }
        if (this.activityRewardDic == null)
        {
            this.tempRewardItems.Clear();
            this.activityRewardDic = new Dictionary<int, ActivityController.ActivityRewardItem>();
            LuaTable cacheField_Table2 = LuaConfigManager.GetXmlConfigTable("event").GetCacheField_Table("activity");
            if (cacheField_Table2 != null)
            {
                for (int l = 0; l < cacheField_Table2.Count; l++)
                {
                    LuaTable luaTable3 = cacheField_Table2[l + 1] as LuaTable;
                    if (luaTable3 == null)
                    {
                        FFDebug.LogError(this, "event -> activity -> item==null:" + cacheField_Table2.ToString());
                    }
                    else
                    {
                        int field_Int8 = luaTable3.GetField_Int("id");
                        int field_Int9 = luaTable3.GetField_Int("active");
                        if (this.maxActivity < field_Int9)
                        {
                            this.maxActivity = field_Int9;
                        }
                        string field_String10 = luaTable3.GetField_String("reward");
                        UI_ActivityGuide.RewardItemSkin skin4 = this.UI_ActGuide.AddActiveRewardItem(field_Int9, field_String10);
                        ActivityController.ActivityRewardItem activityRewardItem = new ActivityController.ActivityRewardItem
                        {
                            id = field_Int8,
                            active = field_Int9,
                            reward = field_String10,
                            skin = skin4
                        };
                        this.activityRewardDic.Add(field_Int9, activityRewardItem);
                        this.tempRewardItems.Add(activityRewardItem);
                    }
                }
            }
            this.tempRewardItems.Sort(delegate (ActivityController.ActivityRewardItem a, ActivityController.ActivityRewardItem b)
            {
                if (a.active > b.active)
                {
                    return 1;
                }
                if (a.active == b.active)
                {
                    return 0;
                }
                return -1;
            });
            for (int m = 0; m < this.tempRewardItems.Count; m++)
            {
                ActivityController.ActivityRewardItem activityRewardItem2 = this.tempRewardItems[m];
                activityRewardItem2.skin.skin.transform.SetSiblingIndex(m);
                RectTransform rectTransform = activityRewardItem2.skin.skin.transform as RectTransform;
                float x = (rectTransform.parent as RectTransform).sizeDelta.x;
                rectTransform.anchoredPosition = Vector2.right * (x * (float)activityRewardItem2.active / (float)this.maxActivity);
            }
            this.tempRewardItems.Clear();
        }
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.UI_ActGuide.btnClose.gameObject).onClick = delegate (PointerEventData eventData)
        {
            this.CloseActivityGuide();
        };
        UIEventListener.Get(this.UI_ActGuide.btnJoin).onClick = delegate (PointerEventData eventData)
        {
            this.PathFindToNPC((uint)this.pathway);
        };
        for (int i = 0; i < this.menuDatas.Count; i++)
        {
            ActivityController.Menu item = this.menuDatas[i];
            UIEventListener.Get(item.skin.skin).onClick = delegate (PointerEventData eventData)
            {
                this.OnSelectMenu(item.id);
            };
        }
        foreach (int menuId2 in this.menuItemDic.Keys)
        {
            int menuId = menuId2;
            List<ActivityController.MenuItem> list = this.menuItemDic[menuId];
            for (int j = 0; j < list.Count; j++)
            {
                ActivityController.MenuItem menuItem = list[j];
                int id = menuItem.id;
                UIEventListener.Get(menuItem.skin.skin).onClick = delegate (PointerEventData eventData)
                {
                    this.OnSelectItem(menuId, id);
                };
            }
        }
        foreach (int num in this.unopenDic.Keys)
        {
            int id = num;
            UIEventListener.Get(this.unopenDic[num].skin).onClick = delegate (PointerEventData eventData)
            {
                this.OnSelectItem(id);
            };
        }
        foreach (int active2 in this.activityRewardDic.Keys)
        {
            int active = active2;
            ActivityController.ActivityRewardItem activityRewardItem = this.activityRewardDic[active];
            GameObject btnObj = activityRewardItem.skin.skin;
            UIEventListener.Get(btnObj).onClick = delegate (PointerEventData eventData)
            {
                this.OnRewardGetItem(active);
            };
            string[] array = activityRewardItem.reward.Split(new char[]
            {
                '-'
            });
            if (array.Length != 0)
            {
                uint id = uint.Parse(array[0]);
                Scheduler.OnScheduler TryShowItemTip = delegate ()
                {
                    ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(id, btnObj);
                };
                UIEventListener.Get(btnObj).onEnter = delegate (PointerEventData eventData)
                {
                    if (MouseStateControoler.Instan.IsContinuedMouseState())
                    {
                        Scheduler.Instance.AddTimer(0.5f, false, TryShowItemTip);
                    }
                    else
                    {
                        TryShowItemTip();
                    }
                };
                UIEventListener.Get(btnObj).onDestroy = delegate (PointerEventData eventData)
                {
                    Scheduler.Instance.RemoveTimer(TryShowItemTip);
                };
                UIEventListener.Get(btnObj).onExit = delegate (PointerEventData eventData)
                {
                    Scheduler.Instance.RemoveTimer(TryShowItemTip);
                };
            }
        }
    }

    private void OnSelectMenu(int menuId)
    {
        this.OnSelectItem(this.curMenuId, -1);
        this.curMenuId = menuId;
        for (int i = 0; i < this.menuDatas.Count; i++)
        {
            ActivityController.Menu menu = this.menuDatas[i];
            UI_ActivityGuide.MenuItemParentSkin menuItemParent = this.UI_ActGuide.GetMenuItemParent(menu.id);
            if (!(menuItemParent.skin == null))
            {
                menuItemParent.skin.SetActive(menuId == menu.id);
                menuItemParent.bar.value = 0f;
                menu.skin.selected.SetActive(menuId == menu.id);
            }
        }
    }

    private void OnSelectItem(int menuId, int id)
    {
        this.curMenuItemId = id;
        if (menuId < 0)
        {
            return;
        }
        if (menuId == 2147483647)
        {
            this.OnSelectItem(id);
            return;
        }
        List<ActivityController.MenuItem> list;
        if (!this.menuItemDic.TryGetValue(menuId, out list))
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            ActivityController.MenuItem menuItem = list[i];
            menuItem.skin.selected.SetActive(menuItem.id == id);
        }
        this.OnInfo(menuId, id);
    }

    private void OnSelectItem(int id)
    {
        this.curMenuItemId = id;
        ActivityController.MenuItem menuItem = null;
        foreach (int num in this.unopenDic.Keys)
        {
            UI_ActivityGuide.MenuItemSkin menuItemSkin = this.unopenDic[num];
            if (id == num)
            {
                menuItemSkin.selected.SetActive(true);
                this.itemDic.TryGetValue(id, out menuItem);
            }
            else
            {
                menuItemSkin.selected.SetActive(false);
            }
        }
        if (menuItem != null)
        {
            this.OnInfo(menuItem.menuId, id);
        }
        else
        {
            this.OnInfo(-1, -1);
        }
    }

    private void OnInfo(int menuId, int id)
    {
        if (menuId < 0 || id < 0)
        {
            this.UI_ActGuide.infoRoot.gameObject.SetActive(false);
            return;
        }
        List<ActivityController.MenuItem> list = this.menuItemDic[menuId];
        if (list == null)
        {
            this.UI_ActGuide.infoRoot.gameObject.SetActive(false);
            return;
        }
        ActivityController.MenuItem menuItem = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].id == id)
            {
                menuItem = list[i];
            }
        }
        if (menuItem == null)
        {
            this.UI_ActGuide.infoRoot.gameObject.SetActive(false);
            return;
        }
        this.UI_ActGuide.infoRoot.gameObject.SetActive(true);
        this.SetInfo(menuItem.name, menuItem.time, menuItem.form, menuItem.limit, menuItem.desc, menuItem.reward, menuItem.pathway, menuItem.notice);
    }

    public void SetInfo(string title, string time, string form, string limit, string detail, string awards, int pathway, string notice)
    {
        this.UI_ActGuide.infoTile.text = title;
        this.UI_ActGuide.infoTime.text = time;
        this.UI_ActGuide.infoType.text = form;
        this.UI_ActGuide.infoLimit.text = limit;
        this.UI_ActGuide.infoDetail.text = detail;
        this.UI_ActGuide.noticeShow.SetActive(!string.IsNullOrEmpty(notice));
        this.UI_ActGuide.infoNotice.text = notice;
        this.pathway = pathway;
        string[] array = awards.Split(new char[]
        {
            ','
        });
        this.UI_ActGuide.UpdateShowInfoAwards(array.Length);
        if (array.Length == 0)
        {
            return;
        }
        for (int i = 0; i < array.Length; i++)
        {
            uint itemId = uint.Parse(array[i]);
            UI_ActivityGuide.InfoAwardItemSkin infoAward = this.UI_ActGuide.GetInfoAward(i);
            if (infoAward == null)
            {
                break;
            }
            GameObject hoverObj = infoAward.hoverObj;
            Image image = infoAward.img;
            try
            {
                if (image != null)
                {
                    ClassSetItemTexture.Instance.SetItemTexture(image, itemId, ImageType.ITEM, delegate (UITextureAsset textureAsset)
                    {
                        if (textureAsset == null)
                        {
                            FFDebug.LogError(this, "SetItem texture is null  itemId:" + itemId);
                            return;
                        }
                        if (image == null)
                        {
                            return;
                        }
                        image.enabled = true;
                    });
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, ex.ToString());
            }
            Scheduler.OnScheduler TryShowItemTip = delegate ()
            {
                ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(itemId, hoverObj);
            };
            UIEventListener.Get(hoverObj).onEnter = delegate (PointerEventData eventData)
            {
                if (MouseStateControoler.Instan.IsContinuedMouseState())
                {
                    Scheduler.Instance.AddTimer(0.5f, false, TryShowItemTip);
                }
                else
                {
                    TryShowItemTip();
                }
            };
            UIEventListener.Get(hoverObj).onDestroy = delegate (PointerEventData eventData)
            {
                Scheduler.Instance.RemoveTimer(TryShowItemTip);
            };
            UIEventListener.Get(hoverObj).onExit = delegate (PointerEventData eventData)
            {
                Scheduler.Instance.RemoveTimer(TryShowItemTip);
            };
        }
    }

    private void OnRewardGetItem(int active)
    {
        if (this.curActivity < active)
        {
            return;
        }
        this.mNetWork.ReqGetActivityAward((uint)active);
    }

    public void OnRetVitalityDegree(MSG_RetVitalityDegree_SC proto)
    {
        if (this.UI_ActGuide == null)
        {
            return;
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        LuaTable heroDataByHeroId = controller.GetHeroDataByHeroId(controller.GetSelfCreateHeroId());
        uint cacheField_Uint = heroDataByHeroId.GetCacheField_Uint("level");
        this.curActivity = (int)proto.info.totaldegree;
        List<VitalityItem> item = proto.info.item;
        for (int i = 0; i < item.Count; i++)
        {
            VitalityItem vitalityItem = item[i];
            ActivityController.MenuItem menuItem;
            if (this.itemDic.TryGetValue((int)vitalityItem.id, out menuItem))
            {
                menuItem.curTimes = (int)vitalityItem.attendtimes;
                menuItem.curActivity = (int)vitalityItem.vitalitydegree;
                menuItem.skin.timesProgress.text = menuItem.curTimes + "/" + menuItem.timesLimit;
                menuItem.skin.activeProgress.text = menuItem.curActivity + "/" + menuItem.perActive * menuItem.timesLimit;
                if ((ulong)cacheField_Uint < (ulong)((long)menuItem.levelLimit))
                {
                    menuItem.skin.levelLimit.gameObject.SetActive(true);
                }
                else
                {
                    menuItem.skin.levelLimit.gameObject.SetActive(false);
                }
            }
        }
        foreach (int key in this.unopenDic.Keys)
        {
            UI_ActivityGuide.MenuItemSkin menuItemSkin = this.unopenDic[key];
            ActivityController.MenuItem menuItem2;
            if (this.itemDic.TryGetValue(key, out menuItem2))
            {
                if ((ulong)cacheField_Uint < (ulong)((long)menuItem2.levelLimit))
                {
                    menuItemSkin.skin.SetActive(true);
                }
                else
                {
                    menuItemSkin.skin.SetActive(false);
                }
            }
            else
            {
                menuItemSkin.skin.SetActive(false);
            }
        }
        this.UI_ActGuide.UpdateActivity(this.curActivity, this.maxActivity);
        List<VitalityReward> reward = proto.info.reward;
        for (int j = 0; j < reward.Count; j++)
        {
            VitalityReward vitalityReward = reward[j];
            ActivityController.ActivityRewardItem activityRewardItem;
            if (this.activityRewardDic.TryGetValue((int)vitalityReward.degree, out activityRewardItem))
            {
                activityRewardItem.geted = (vitalityReward.state == 2U);
                activityRewardItem.skin.btn.interactable = (vitalityReward.state != 2U);
                activityRewardItem.skin.img_0.gameObject.SetActive(vitalityReward.state == 2U);
                activityRewardItem.skin.img_2.gameObject.SetActive(vitalityReward.state == 1U);
                activityRewardItem.skin.img_1.gameObject.SetActive(false);
            }
        }
        if (item.Count > 0)
        {
            this.OnSelectMenu((int)item[0].id);
        }
    }

    public void OnRetGetVitalityAward(uint resultCode, uint degree)
    {
        if (this.UI_ActGuide == null)
        {
            return;
        }
        if (resultCode != 0U)
        {
            return;
        }
        ActivityController.ActivityRewardItem activityRewardItem;
        if (this.activityRewardDic.TryGetValue((int)degree, out activityRewardItem))
        {
            activityRewardItem.geted = true;
            activityRewardItem.skin.btn.interactable = false;
            activityRewardItem.skin.img_0.gameObject.SetActive(true);
            activityRewardItem.skin.img_2.gameObject.SetActive(false);
            activityRewardItem.skin.img_1.gameObject.SetActive(false);
        }
    }

    public void PathFindToNPC(uint pathWayID)
    {
        if (pathWayID == 0U)
        {
            TipsWindow.ShowWindow(TipsType.PATHID_IS_ZERO, null);
            FFDebug.LogWarning(this, "PathFindToNPC pathid= 0!!!");
            return;
        }
        TaskUIController controller = ControllerManager.Instance.GetController<TaskUIController>();
        if (controller == null)
        {
            return;
        }
        controller.FindPathInterface(pathWayID, null);
    }

    public void ShowActivityGuide()
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ActivityGuide>("UI_ActivityGuide", delegate ()
        {
            try
            {
                this.InitConfig();
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, " 活跃度配置 InitConfig 初始化错误！ " + ex.ToString());
            }
            this.InitEvent();
            this.mNetWork.ReqActivityDegree();
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseActivityGuide()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_ActivityGuide");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        this.OnDispos();
    }

    public void OnDispos()
    {
        this.curActivity = 0;
        this.maxActivity = 0;
        this.curMenuId = -1;
        this.curMenuItemId = -1;
        this.menuDatas = null;
        this.menuItemDic = null;
        this.activityRewardDic = null;
        this.itemDic = null;
        this.unopenDic = null;
    }

    private const int menuUnopen = 2147483647;

    private const string unopenName = "即将开启";

    public ActivityNetWork mNetWork;

    public int curActivity;

    public int maxActivity;

    public int curMenuId = -1;

    public int curMenuItemId = -1;

    public List<ActivityController.Menu> menuDatas;

    public Dictionary<int, List<ActivityController.MenuItem>> menuItemDic;

    public Dictionary<int, ActivityController.ActivityRewardItem> activityRewardDic;

    public Dictionary<int, ActivityController.MenuItem> itemDic;

    public Dictionary<int, UI_ActivityGuide.MenuItemSkin> unopenDic;

    private List<ActivityController.MenuItem> tempMenuItems = new List<ActivityController.MenuItem>();

    private List<ActivityController.ActivityRewardItem> tempRewardItems = new List<ActivityController.ActivityRewardItem>();

    private int pathway;

    public class Menu
    {
        public int id;

        public string name;

        public UI_ActivityGuide.MenuSkin skin;
    }

    public class MenuItem
    {
        public int id;

        public string name;

        public int menuId;

        public int timesLimit;

        public int perActive;

        public int levelLimit;

        public string desc;

        public string time;

        public string form;

        public string limit;

        public string reward;

        public int pathway;

        public string questgroup;

        public int curTimes;

        public int curActivity;

        public string notice;

        public UI_ActivityGuide.MenuItemSkin skin;
    }

    public class ActivityRewardItem
    {
        public int id;

        public int active;

        public string reward;

        public bool geted;

        public UI_ActivityGuide.RewardItemSkin skin;
    }
}
