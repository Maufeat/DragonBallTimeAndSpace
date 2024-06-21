using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpSystem : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root.gameObject;
        this.mRoot.SetLayer(Const.Layer.Charactor, true);
        this.playerhpPanel = root.Find("PlayerHpInfo");
        this.mosterhpPanel = root.Find("MosterHpInfo");
        this.mosterhpPanel.gameObject.SetActive(true);
        this.playerhpPanel.gameObject.SetActive(true);
        this.playerHp = root.Find("PlayerHpInfo/PanelPlayers").gameObject;
        this.mosterHp = root.Find("MosterHpInfo/PanelMonsters").gameObject;
        this.hpHitRoot = GameObject.Find("hpRoot").transform;
        this.modePool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<NormalObjectInPool>(this.playerHp.name, false);
        if (this.modePool == null)
        {
            this.modePool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().CreatPool<NormalObjectInPool>(this.playerHp, new Action<NormalObjectInPool>(this.ResetPlayerHpObj), new Action<NormalObjectInPool>(this.ResetPlayerHpObj), false, string.Empty);
        }
        this.modePoolNPC = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<NormalObjectInPool>(this.mosterHp.name, false);
        if (this.modePoolNPC == null)
        {
            this.modePoolNPC = ManagerCenter.Instance.GetManager<ObjectPoolManager>().CreatPool<NormalObjectInPool>(this.mosterHp, new Action<NormalObjectInPool>(this.ResetMosterHpObj), new Action<NormalObjectInPool>(this.ResetMosterHpObj), false, string.Empty);
        }
        this.selectBtn = root.transform.Find("choose").gameObject;
    }

    public override void OnDispose()
    {
        ManagerCenter.Instance.GetManager<ObjectPoolManager>().RemoveObjectPool(this.modePool.PoolName, false);
        ManagerCenter.Instance.GetManager<ObjectPoolManager>().RemoveObjectPool(this.modePoolNPC.PoolName, false);
        base.OnDispose();
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public void CreatePlayerHp(CharactorBase charbase, Transform top, cs_MapUserData data, Action<HpStruct> callBack)
    {
        this.modePool.GetItemFromPool(delegate (NormalObjectInPool obj)
        {
            GameObject itemObj = obj.ItemObj;
            itemObj.transform.SetParent(this.playerhpPanel);
            itemObj.transform.localScale = new Vector3(1f, 1f, 1f);
            HpStruct hpStruct = new HpStruct();
            hpStruct.SetOwner(charbase);
            hpStruct.InitHpStruct(obj, charbase.ModelObj, top, data, this.hpHitRoot);
            callBack(hpStruct);
        });
    }

    private void ResetPlayerHpObj(NormalObjectInPool obj)
    {
        UIFllowTarget component = obj.ItemObj.GetComponent<UIFllowTarget>();
        if (component != null)
        {
            component.Offset = Vector3.zero;
        }
        obj.ItemObj.transform.Find("content/PlayerInfo/Panel_name_node/txt_name").GetComponent<Outline>().effectColor = Const.TextColorNormal;
        obj.ItemObj.transform.Find("content/PlayerInfo/Panel_name_node/txt_guildname").GetComponent<Outline>().effectColor = Const.TextColorNormal;
        obj.ItemObj.FindChild("content/PlayerInfo/Panel_name_node/txt_name").GetComponent<Text>().color = Const.GetColorByName("normalwhite");
        obj.ItemObj.FindChild("content/hp/Slider/Background").GetComponent<Image>().color = Const.GetColorByName("normalwhite");
    }

    public void CreateMosterHp(CharactorBase charbase, Transform top, cs_MapNpcData data, Action<HpStruct> callBack)
    {
        this.modePoolNPC.GetItemFromPool(delegate (NormalObjectInPool obj)
        {
            GameObject itemObj = obj.ItemObj;
            itemObj.transform.SetParent(this.mosterhpPanel);
            itemObj.transform.localScale = new Vector3(1f, 1f, 1f);
            HpStruct hpStruct = new HpStruct();
            hpStruct.SetOwner(charbase);
            hpStruct.InitHpStruct(obj, charbase.ModelObj, top, data, this.hpHitRoot);
            callBack(hpStruct);
        });
    }

    private void ResetMosterHpObj(NormalObjectInPool obj)
    {
        UIFllowTarget component = obj.ItemObj.GetComponent<UIFllowTarget>();
        if (component != null)
        {
            component.Offset = Vector3.zero;
        }
        Text component2 = obj.ItemObj.transform.Find("content/occupyinfo/txt_occupyinfo").GetComponent<Text>();
        component2.text = string.Empty;
        component2.GetComponent<Outline>().effectColor = Const.TextColorNormal;
        component2.transform.parent.gameObject.SetActive(false);
        obj.ItemObj.transform.Find("content/PlayerInfo/txt_name").GetComponent<Outline>().effectColor = Const.TextColorNormal;
        obj.ItemObj.FindChild("content/PlayerInfo/txt_name").GetComponent<Text>().color = Const.GetColorByName("normalwhite");
        obj.ItemObj.FindChild("content/PlayerInfo/txt_owner").GetComponent<Text>().color = Const.GetColorByName("normalwhite");
        obj.ItemObj.FindChild("content/hp/Slider/Background").GetComponent<Image>().color = Const.GetColorByName("normalwhite");
    }

    public void SetSelectUIActive(bool active)
    {
        this.selectBtn.gameObject.SetActive(active);
    }

    public GameObject mRoot;

    public GameObject playerHp;

    private Transform playerhpPanel;

    private GameObject mosterHp;

    private Transform mosterhpPanel;

    private Transform hpHitRoot;

    private ObjectPool<NormalObjectInPool> modePool;

    private ObjectPool<NormalObjectInPool> modePoolNPC;

    private GameObject selectBtn;
}
