using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using Team;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamDropItem
{
    public TeamDropItem(teamDropItem item, GameObject prefab, Action ontimeover)
    {
        this.itemdata = item;
        this.obj = this.CreatItem(prefab);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.currenttime = item.duetime;
        this.startCountDown = true;
        this.OnTimeOver = ontimeover;
    }

    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    public void DisposeThis()
    {
        for (int i = 0; i < this.commonItems.Count; i++)
        {
            this.commonItems[i].Dispose();
        }
        this.commonItems.Clear();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        this.itemdata = null;
        UnityEngine.Object.Destroy(this.obj);
    }

    private void SetupItem(Transform itemObj, uint baseId, uint num)
    {
        itemObj.gameObject.SetActive(true);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)baseId);
        if (configTable == null)
        {
            return;
        }
        GlobalRegister.SetImage(0, configTable.GetField_String("icon"), itemObj.Find("img_icon").GetComponent<Image>(), true);
        itemObj.Find("txt_count").GetComponent<Text>().text = num.ToString();
        itemObj.Find("quality").gameObject.SetActive(false);
        string imgname = "quality" + configTable.GetField_Uint("quality");
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
        {
            if (itemObj == null)
            {
                return;
            }
            if (asset == null)
            {
                return;
            }
            itemObj.Find("quality").gameObject.SetActive(true);
            itemObj.Find("quality").GetComponent<RawImage>().texture = asset.textureObj;
        });
        UIEventListener.Get(itemObj.Find("img_icon").gameObject).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(new t_Object
            {
                baseid = baseId
            }, itemObj.Find("img_icon").gameObject);
        };
        UIEventListener.Get(itemObj.Find("img_icon").gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
    }

    private GameObject CreatItem(GameObject itemprefab)
    {
        if (itemprefab == null)
        {
            return null;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemprefab);
        this.SetupItem(gameObject.FindChild("Item").transform, this.itemdata.objid, this.itemdata.num);
        Button component = gameObject.FindChild("btn_need").GetComponent<Button>();
        Button component2 = gameObject.FindChild("btn_greed").GetComponent<Button>();
        Button component3 = gameObject.FindChild("btn_giveup").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.teamController.ReqChoose(this.itemdata.thisid, ChooseType.ChooseType_Need);
        });
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(delegate ()
        {
            this.teamController.ReqChoose(this.itemdata.thisid, ChooseType.ChooseType_Greed);
        });
        component3.onClick.RemoveAllListeners();
        component3.onClick.AddListener(delegate ()
        {
            this.teamController.ReqChoose(this.itemdata.thisid, ChooseType.ChooseType_Giveup);
        });
        gameObject.SetActive(true);
        gameObject.transform.SetParent(itemprefab.transform.parent);
        gameObject.transform.localScale = Vector3.one;
        this.timeSlider = gameObject.transform.Find("Slider").GetComponent<Slider>();
        return gameObject;
    }

    private void Update()
    {
        if (this.startCountDown)
        {
            if (this.currenttime > 0f)
            {
                this.currenttime -= Scheduler.Instance.realDeltaTime;
                if (this.timeSlider != null)
                {
                    this.timeSlider.value = this.currenttime / this.itemdata.duetime;
                }
            }
            else
            {
                this.startCountDown = false;
                this.timeSlider.value = 0f;
                if (this.OnTimeOver != null)
                {
                    this.OnTimeOver();
                }
            }
        }
    }

    private teamDropItem itemdata;

    private GameObject obj;

    private float currenttime = -1f;

    private Slider timeSlider;

    private bool startCountDown;

    private Action OnTimeOver;

    private List<CommonItem> commonItems = new List<CommonItem>();
}
