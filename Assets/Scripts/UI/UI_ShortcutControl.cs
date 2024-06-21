using System;
using System.Collections.Generic;
using apprentice;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShortcutControl : MonoBehaviour
{
    public SkillManager Skillmgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<SkillManager>();
        }
    }

    private int GetItemPos(int row, int col)
    {
        return (row - 1) * 100 + col;
    }

    public void OnInit(GameObject _root)
    {
        this.root = _root;
        this.btn_add = this.root.transform.Find("Add").gameObject;
        this.btn_reduce = this.root.transform.Find("Reduce").gameObject;
        this.btn_lock = this.root.transform.Find("Lock").gameObject;
        this.btn_bg = this.root.transform.Find("Bg").gameObject;
        this.btn_bg.name = "-1_0_0";
        this.btn_bg.GetComponent<Image>().enabled = false;
        UIEventListener.Get(this.btn_add).onClick = new UIEventListener.VoidDelegate(this.btn_add_on_click);
        UIEventListener.Get(this.btn_reduce).onClick = new UIEventListener.VoidDelegate(this.btn_reduce_on_click);
        UIEventListener.Get(this.btn_lock).onClick = new UIEventListener.VoidDelegate(this.btn_lock_on_click);
        this.mItemDic = new Dictionary<int, GameObject>();
        this.spBlankItem = this.root.transform.Find("Panel/1/1/Image").GetComponent<Image>().sprite;
        this.mAllDragDropButtonList = new List<DragDropButton>();
        int num = 1;
        while ((float)num < this.ROW_COL_NUM.y + 1f)
        {
            int num2 = 1;
            while ((float)num2 < this.ROW_COL_NUM.x + 1f)
            {
                GameObject gameObject = this.root.transform.Find(string.Concat(new object[]
                {
                    "Panel/",
                    num,
                    "/",
                    num2,
                    "/Image"
                })).gameObject;
                gameObject.transform.SetAsLastSibling();
                this.mItemDic.Add(this.GetItemPos(num, num2), gameObject);
                gameObject.GetComponent<Image>().material = null;
                gameObject.SetActive(true);
                DragDropButton dragDropButton = gameObject.AddComponent<DragDropButton>();
                dragDropButton.Initilize(UIRootType.Shortcut, new Vector2(0f, (float)this.GetItemPos(num, num2)), string.Empty, null);
                this.mAllDragDropButtonList.Add(dragDropButton);
                UIEventListener.Get(gameObject).onEnter = new UIEventListener.VoidDelegate(this.btn_item_on_enter);
                UIEventListener.Get(gameObject).onDestroy = new UIEventListener.VoidDelegate(this.btn_item_on_exit);
                UIEventListener.Get(gameObject).onExit = new UIEventListener.VoidDelegate(this.btn_item_on_exit);
                num2++;
            }
            num++;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (!manager.isAbattoirScene)
        {
            ServerStorageManager.Instance.GetData(ServerStorageKey.Shortcuts, 0U);
        }
        else
        {
            ServerStorageManager.Instance.GetData(ServerStorageKey.AbattoirShortcuts, 0U);
        }
        DragDropManager.Instance.RegisterPutInCb(UIRootType.Shortcut, new TwoBtnCb(this.PutInCb));
        DragDropManager.Instance.RegisterDestoryItemCb(UIRootType.Shortcut, new OneBtnCb(this.DestoryItemCb));
        DragDropManager.Instance.RegisterLeftClickCb(UIRootType.Shortcut, new OneBtnCb(this.UseItemCb));
        Scheduler.Instance.AddFrame(10U, true, new Scheduler.OnScheduler(this.PalyCDText));
        Scheduler.Instance.AddFrame(1U, true, new Scheduler.OnScheduler(this.PlayCDAnim));
    }

    private void btn_item_on_enter(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerCurrentRaycast.gameObject;
        DragDropButton component = gameObject.GetComponent<DragDropButton>();
        if (component != null && component.mData != null)
        {
            ShortcutDragDropButtonData shortcutDragDropButtonData = component.mData as ShortcutDragDropButtonData;
            if (shortcutDragDropButtonData != null && shortcutDragDropButtonData.mSaveDataItem.type == SaveDataItemType.item)
            {
                object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemData", new object[]
                {
                    component.mData.thisid
                });
                if (array.Length > 0)
                {
                    LuaTable luaTable = array[0] as LuaTable;
                    if (luaTable == null)
                    {
                        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(component.mData.mId, gameObject);
                    }
                    else
                    {
                        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(luaTable, gameObject);
                    }
                }
            }
        }
    }

    private void btn_item_on_exit(PointerEventData eventData)
    {
    }

    private void PutInCb(DragDropButton btnFrom, DragDropButton btnTo)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        if (btnFrom.mUIRootType == UIRootType.Shortcut)
        {
            if (!btnTo.IsEmptyBtn())
            {
                SaveDataItem mSaveDataItem = (btnTo.mData as ShortcutDragDropButtonData).mSaveDataItem;
                mSaveDataItem.pos = (int)btnFrom.mPos.y;
                dictionary[(int)btnFrom.mPos.y] = mSaveDataItem;
            }
            else
            {
                dictionary.RemoveAt((int)btnFrom.mPos.y);
            }
        }
        SaveDataItem value = default(SaveDataItem);
        if (btnFrom.mUIRootType == UIRootType.Bag)
        {
            BagDragDropButtonData bagDragDropButtonData = btnFrom.mData as BagDragDropButtonData;
            value.id = (int)bagDragDropButtonData.mId;
            value.thisid = bagDragDropButtonData.thisid;
            value.type = SaveDataItemType.item;
        }
        else if (btnFrom.mUIRootType == UIRootType.Shortcut)
        {
            value = (btnFrom.mData as ShortcutDragDropButtonData).mSaveDataItem;
        }
        value.pos = (int)btnTo.mPos.y;
        dictionary[(int)btnTo.mPos.y] = value;
        this.UpdateServerData();
    }

    private void DestoryItemCb(DragDropButton btnFrom)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dict = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        dict.RemoveAt((int)btnFrom.mPos.y);
        this.UpdateServerData();
    }

    private void UseItemCb(DragDropButton btnFrom)
    {
        if (btnFrom.mData != null)
        {
            this.UseItemById(btnFrom.mData.mId);
        }
    }

    public void ReqShortcutDataCb(MSG_Req_OperateClientDatas_CS msg, string key)
    {
        Dictionary<int, SaveDataItem> dictionary;
        if (key == ServerStorageKey.Shortcuts.ToString())
        {
            if (string.IsNullOrEmpty(msg.value))
            {
                msg.value = "1_2_90000_0,|1|True";
            }
            dictionary = (this.mSaveDataItemDic = new Dictionary<int, SaveDataItem>());
        }
        else
        {
            if (!(key == ServerStorageKey.AbattoirShortcuts.ToString()))
            {
                return;
            }
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            dictionary = (controller.mAbattoirSaveDataItemDic = new Dictionary<int, SaveDataItem>());
        }
        string[] array = msg.value.Split(new char[]
        {
            '|'
        });
        string[] array2 = array[0].Split(new string[]
        {
            ","
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array2.Length; i++)
        {
            string[] array3 = array2[i].Split(new char[]
            {
                '_'
            });
            if (array3.Length == 4)
            {
                int num = int.Parse(array3[0]);
                if (!dictionary.ContainsKey(num))
                {
                    dictionary.Add(num, new SaveDataItem
                    {
                        pos = num,
                        type = (SaveDataItemType)int.Parse(array3[1]),
                        id = int.Parse(array3[2]),
                        thisid = array3[3]
                    });
                }
            }
        }
        if (array.Length > 1)
        {
            this.mShowRowNum = int.Parse(array[1]);
        }
        if (array.Length > 2)
        {
            this.mIsUnlock = bool.Parse(array[2]);
            this.btn_lock.GetComponent<Image>().color = ((!this.mIsUnlock) ? Color.red : Color.green);
        }
        else
        {
            this.btn_lock.GetComponent<Image>().color = Color.green;
        }
        this.SetupPanel();
    }

    private void SetupPanel()
    {
        foreach (int pos in this.mItemDic.Keys)
        {
            this.SetupItem(pos);
        }
        this.SetupRowNum();
    }

    private void SetupRowNum()
    {
        int num = 1;
        while ((float)num < this.ROW_COL_NUM.y + 1f)
        {
            this.root.transform.Find("Panel/" + num).gameObject.SetActive(num <= this.mShowRowNum);
            num++;
        }
    }

    private void SetupItem(int pos)
    {
        if (this.mItemDic.ContainsKey(pos))
        {
            GameObject gameObject = this.mItemDic[pos];
            Image image_icon = gameObject.GetComponent<Image>();
            image_icon.sprite = this.spBlankItem;
            image_icon.color = new Color(0f, 0f, 0f, 0f);
            Text component = image_icon.transform.parent.Find("txt_ktip").GetComponent<Text>();
            component.transform.SetAsLastSibling();
            component.text = this.GetItemKeyText(pos);
            component.raycastTarget = false;
            gameObject.transform.parent.Find("txt_count").GetComponent<Text>().text = string.Empty;
            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
            if (dictionary.ContainsKey(pos))
            {
                SaveDataItem data = dictionary[pos];
                image_icon.name = this.DataToName(data);
                string imgname = string.Empty;
                string imgname2 = string.Empty;
                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)data.id);
                if (configTable != null)
                {
                    imgname = configTable.GetField_String("icon");
                    imgname2 = "quality" + configTable.GetCacheField_Int("quality");
                }
                this.SetItemCount(gameObject, data.id);
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, imgname, delegate (UITextureAsset asset)
                {
                    if (asset == null)
                    {
                        FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                        return;
                    }
                    if (image_icon == null)
                    {
                        return;
                    }
                    Texture2D textureObj = asset.textureObj;
                    Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                    image_icon.sprite = sprite;
                    image_icon.overrideSprite = sprite;
                    image_icon.color = Color.white;
                });
                GameObject qualityObj = gameObject.transform.parent.Find("quality").gameObject;
                qualityObj.SetActive(false);
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname2, delegate (UITextureAsset asset)
                {
                    if (asset == null)
                    {
                        return;
                    }
                    if (qualityObj == null)
                    {
                        return;
                    }
                    qualityObj.SetActive(true);
                    RawImage component2 = qualityObj.GetComponent<RawImage>();
                    component2.texture = asset.textureObj;
                });
                gameObject.GetComponent<DragDropButton>().mData = new ShortcutDragDropButtonData(data);
            }
            else
            {
                image_icon.name = this.DataToName(new SaveDataItem
                {
                    pos = pos,
                    type = SaveDataItemType.none,
                    id = 0,
                    thisid = "0"
                });
                gameObject.GetComponent<DragDropButton>().mData = null;
                gameObject.transform.parent.Find("quality").gameObject.SetActive(false);
            }
        }
    }

    public void RefrashItemCount()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        if (dictionary == null || this.mItemDic == null)
        {
            return;
        }
        List<int> list = new List<int>(dictionary.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            if (this.mItemDic.ContainsKey(dictionary[list[i]].pos))
            {
                GameObject gameObject = this.mItemDic[dictionary[list[i]].pos];
                gameObject.transform.parent.Find("txt_count").GetComponent<Text>().text = string.Empty;
                if (dictionary.ContainsKey(dictionary[list[i]].pos))
                {
                    this.SetItemCount(gameObject, dictionary[list[i]].id);
                }
            }
        }
    }

    private void SetItemCount(GameObject item, int id)
    {
        Text component = item.transform.parent.Find("txt_count").GetComponent<Text>();
        ContentSizeFitter contentSizeFitter = component.gameObject.GetComponent<ContentSizeFitter>();
        if (contentSizeFitter == null)
        {
            contentSizeFitter = component.gameObject.AddComponent<ContentSizeFitter>();
        }
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        string s = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            id
        })[0] + string.Empty;
        string s2 = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromTaskPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            id
        })[0] + string.Empty;
        string s3 = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromCapsulePackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            id
        })[0] + string.Empty;
        uint num = 0U;
        uint num2 = 0U;
        if (uint.TryParse(s, out num2))
        {
            num = (uint)Mathf.Max(num, num2);
        }
        uint num3 = 0U;
        if (uint.TryParse(s2, out num3))
        {
            num = (uint)Mathf.Max(num, num3);
        }
        uint num4 = 0U;
        if (uint.TryParse(s3, out num4))
        {
            num = (uint)Mathf.Max(num, num4);
        }
        Image component2 = item.GetComponent<Image>();
        component2.color = Color.white;
        if (num == 0U && id != 0)
        {
            component2.color = Color.gray;
        }
        component.text = num + string.Empty;
        component.transform.SetAsLastSibling();
        component.raycastTarget = false;
    }

    private string GetItemKeyText(int pos)
    {
        ShortcutkeyFunctionType sft = pos + (ShortcutkeyFunctionType)100;
        return ControllerManager.Instance.GetController<ShortcutsConfigController>().GetKeyNameForItemByFunctionType(sft);
    }

    public void FrashShortcutUIShowName()
    {
        List<int> list = new List<int>(this.mItemDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            GameObject gameObject = this.mItemDic[list[i]];
            Text component = gameObject.transform.parent.Find("txt_ktip").GetComponent<Text>();
            component.text = this.GetItemKeyText(list[i]);
        }
    }

    private void btn_lock_on_click(PointerEventData eventData)
    {
        this.mIsUnlock = !this.mIsUnlock;
        this.btn_lock.GetComponent<Image>().color = ((!this.mIsUnlock) ? Color.red : Color.green);
        this.UpdateServerData();
    }

    private void btn_add_on_click(PointerEventData eventData)
    {
        if (this.mIsUnlock && (float)this.mShowRowNum < this.ROW_COL_NUM.y)
        {
            this.mShowRowNum++;
            this.SetupRowNum();
            this.UpdateServerData();
        }
    }

    private void btn_reduce_on_click(PointerEventData eventData)
    {
        if (this.mIsUnlock && this.mShowRowNum > 1)
        {
            this.mShowRowNum--;
            this.SetupRowNum();
            this.UpdateServerData();
        }
    }

    public void UseItem(int row, int col)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        if (dictionary == null)
        {
            return;
        }
        int itemPos = this.GetItemPos(row, col);
        if (dictionary.ContainsKey(itemPos))
        {
            SaveDataItem data = dictionary[itemPos];
            this.ProgressUseItem(data);
        }
    }

    public float GetUseItemCd(int row, int col)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        if (dictionary == null)
        {
            return -1f;
        }
        int itemPos = this.GetItemPos(row, col);
        if (dictionary.ContainsKey(itemPos))
        {
            SaveDataItem saveDataItem = dictionary[itemPos];
            PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                saveDataItem.id
            })[0];
            if (propsBase != null && propsBase.config != null)
            {
                return propsBase.config.GetCacheField_Uint("cdtime");
            }
        }
        return -1f;
    }

    private void ProgressUseItem(SaveDataItem data)
    {
        this.UseItemById((uint)data.id);
    }

    public void UseItemById(uint id)
    {
        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            id
        })[0];
        uint range = 0U;
        uint effectratio = 0U;
        bool flag = UIBagManager.Instance.TryGetUseByPos(id, out range, out effectratio);
        if (propsBase != null && flag)
        {
            UIBagManager.Instance.SetSkillObjectData(propsBase._obj.thisid, flag, range, effectratio);
            UIBagManager.Instance.SetSelectPosType(SummonNpcState.SelectPos);
            return;
        }
        if (propsBase != null)
        {
            bool flag2 = UIBagManager.Instance.TryGetUseBySelectIndex(id, propsBase._obj.thisid);
            if (flag2)
            {
                return;
            }
        }
        uint num = UIBagManager.Instance.TryGetCallNpcID(id, out range, out effectratio);
        if (num != 0U && propsBase != null)
        {
            UIBagManager.Instance.SetSkillObjectData(propsBase._obj.thisid, num, range, effectratio);
            UIBagManager.Instance.SetSelectPosType(SummonNpcState.SelectPos);
        }
        else if (propsBase != null)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                propsBase,
                1
            });
        }
    }

    private void UpdateServerData()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        ServerStorageKey key = (!manager.isAbattoirScene) ? ServerStorageKey.Shortcuts : ServerStorageKey.AbattoirShortcuts;
        uint type = 0U;
        if (dictionary != null)
        {
            string text = string.Empty;
            foreach (SaveDataItem data in dictionary.Values)
            {
                text = text + this.DataToName(data) + ",";
            }
            string text2 = text;
            text = string.Concat(new object[]
            {
                text2,
                "|",
                this.mShowRowNum,
                "|",
                this.mIsUnlock
            });
            ServerStorageManager.Instance.AddUpdateData(key, text, type);
        }
        else
        {
            Debug.LogWarning("mSaveDataItemDic is null");
        }
    }

    private void ReqDestoryItemSureCb(SaveDataItem data)
    {
        new BagNetworker().ReqDestroyObject(data.thisid, data.packtype);
    }

    private string DataToName(SaveDataItem data)
    {
        return string.Concat(new object[]
        {
            data.pos,
            "_",
            (int)data.type,
            "_",
            data.id,
            "_",
            data.thisid
        });
    }

    public void RefreshCDPanel(Dictionary<uint, float> cdDataList)
    {
        this.mCDDataList = cdDataList;
    }

    private void PalyCDText()
    {
        if (this.mCDDataList == null)
        {
            return;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        if (dictionary == null || this.mItemDic == null)
        {
            return;
        }
        foreach (GameObject gameObject in this.mItemDic.Values)
        {
            gameObject.transform.parent.Find("txt_cd").GetComponent<Text>().text = string.Empty;
        }
        List<uint> list = new List<uint>(this.mCDDataList.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            uint num = list[i];
            int num2 = Mathf.CeilToInt(this.mCDDataList[num]);
            List<int> list2 = new List<int>(dictionary.Keys);
            for (int j = 0; j < list2.Count; j++)
            {
                if (this.mItemDic.ContainsKey(dictionary[list2[j]].pos) && (long)dictionary[list2[j]].id == (long)((ulong)num))
                {
                    GameObject gameObject2 = this.mItemDic[dictionary[list2[j]].pos];
                    GameObject gameObject3 = gameObject2.transform.parent.Find("txt_cd").gameObject;
                    if (!gameObject3.activeSelf)
                    {
                        gameObject3.SetActive(true);
                        gameObject3.transform.SetAsLastSibling();
                    }
                    if (num2 <= 0)
                    {
                        gameObject3.GetComponent<Text>().text = string.Empty;
                    }
                    else
                    {
                        gameObject3.GetComponent<Text>().text = num2.ToString();
                    }
                }
            }
        }
    }

    private void PlayCDAnim()
    {
        if (this.mCDDataList == null)
        {
            return;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Dictionary<int, SaveDataItem> dictionary = (!manager.isAbattoirScene) ? this.mSaveDataItemDic : controller.mAbattoirSaveDataItemDic;
        if (dictionary == null || this.mItemDic == null)
        {
            return;
        }
        foreach (GameObject gameObject in this.mItemDic.Values)
        {
            gameObject.transform.parent.Find("img_cd").gameObject.SetActive(false);
        }
        List<uint> list = new List<uint>(this.mCDDataList.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            uint num = list[i];
            Dictionary<uint, float> dictionary3;
            Dictionary<uint, float> dictionary2 = dictionary3 = this.mCDDataList;
            uint key2;
            uint key = key2 = num;
            float num2 = dictionary3[key2];
            dictionary2[key] = num2 - Time.deltaTime;
            if (!this.cdConfig.ContainsKey(num))
            {
                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)num);
                uint field_Uint = configTable.GetField_Uint("cdtime");
                this.CheckIsGuildSkillEffectObject(num);
                this.cdConfig.Add(num, field_Uint);
            }
            float num3 = this.mCDDataList[num] / this.cdConfig[num];
            List<int> list2 = new List<int>(dictionary.Keys);
            for (int j = 0; j < list2.Count; j++)
            {
                SaveDataItem saveDataItem = dictionary[list2[j]];
                if (this.mItemDic.ContainsKey(saveDataItem.pos) && (long)saveDataItem.id == (long)((ulong)num))
                {
                    GameObject gameObject2 = this.mItemDic[saveDataItem.pos];
                    GameObject gameObject3 = gameObject2.transform.parent.Find("img_cd").gameObject;
                    if (num3 <= 0f)
                    {
                        gameObject3.SetActive(false);
                    }
                    else
                    {
                        if (!gameObject3.activeSelf)
                        {
                            gameObject3.SetActive(true);
                            gameObject3.transform.SetAsLastSibling();
                        }
                        gameObject3.GetComponent<Image>().fillAmount = num3;
                    }
                }
            }
        }
    }

    private void CheckIsGuildSkillEffectObject(uint id)
    {
        GuildControllerNew gcn = ControllerManager.Instance.GetController<GuildControllerNew>();
        if (gcn != null)
        {
            gcn.TryGetGuildSkillLv(delegate (uint lv)
            {
                if (lv > 0U)
                {
                    LuaTable guildSkillConfigByID = gcn.GetGuildSkillConfigByID(500U + lv);
                    if (guildSkillConfigByID != null)
                    {
                        string cacheField_String = guildSkillConfigByID.GetCacheField_String("skillstaus");
                        string[] array = cacheField_String.Split(new char[]
                        {
                            ','
                        });
                        for (int i = 0; i < array.Length; i++)
                        {
                            string[] array2 = array[i].Split(new char[]
                            {
                                '-'
                            });
                            if (this.cdConfig.ContainsKey(uint.Parse(array2[1])) && array2.Length > 2)
                            {
                                float num = 1f - float.Parse(array2[2]) / 100f;
                                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)id);
                                uint field_Uint = configTable.GetField_Uint("cdtime");
                                this.cdConfig[id] = (uint)(field_Uint * num);
                            }
                        }
                    }
                }
            }, 5U);
        }
    }

    public void OnDispose()
    {
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PalyCDText));
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PlayCDAnim));
    }

    private GameObject root;

    public GameObject btn_add;

    public GameObject btn_reduce;

    public GameObject btn_lock;

    public GameObject btn_bg;

    private Sprite spBlankItem;

    private Vector2 ROW_COL_NUM = new Vector2(12f, 3f);

    private MSG_Req_OperateClientDatas_CS msg;

    private Dictionary<int, GameObject> mItemDic;

    public bool mIsUnlock = true;

    public int mShowRowNum = 1;

    private Dictionary<int, SaveDataItem> mSaveDataItemDic;

    public GameObject curOpeOriIconObj;

    public List<DragDropButton> mAllDragDropButtonList;

    private Dictionary<uint, float> mCDDataList;

    private Dictionary<uint, uint> cdConfig = new Dictionary<uint, uint>();
}
