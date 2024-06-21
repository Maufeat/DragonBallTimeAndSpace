using System;
using Framework.Managers;
using LuaInterface;
using magic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Dialog_Revive : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root.gameObject;
        this.btn_local = this.Root.transform.Find("Offset_Revive/PanelRevive/btns/btn_local").gameObject;
        this.btn_cemetery = this.Root.transform.Find("Offset_Revive/PanelRevive/btns/btn_cemetery").gameObject;
    }

    public void Viewrevive(MSG_Ret_MainUserDeath_SC data)
    {
        this.Root.transform.Find("Offset_Revive/PanelRevive/yuandi/txt_count").GetComponent<Text>().text = "x  " + data.relivecostnum.ToString();
        this.reviveData = data;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)data.relivecostid);
        if (configTable != null)
        {
            base.GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (Texture2D item)
            {
                Image component = this.Root.transform.Find("Offset_Revive/PanelRevive/yuandi/img_itemicon").GetComponent<Image>();
                if (item == null)
                {
                    FFDebug.LogWarning(this, "    req  texture   is  null ");
                }
                Sprite overrideSprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                component.overrideSprite = overrideSprite;
            });
        }
        FFDebug.Log(this, FFLogType.Default, string.Concat(new object[]
        {
            "  count  ",
            data.relivecostnum,
            "  revive  ",
            data.canrelive
        }));
        this.InitEvent();
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.btn_local).onClick = delegate (PointerEventData data)
        {
        };
        UIEventListener.Get(this.btn_cemetery).onClick = delegate (PointerEventData data)
        {
            this.cemeteryRevive();
        };
    }

    private void localRevive()
    {
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqRelive(true);
        this.close();
    }

    private void cemeteryRevive()
    {
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqRelive(false);
        this.close();
    }

    private void close()
    {
        this.Root.gameObject.SetActive(false);
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("ui_revive");
    }

    public override void OnDispose()
    {
        base.OnDispose();
        if (this.Root != null)
        {
            UnityEngine.Object.Destroy(this.Root);
        }
        if (this.OnClose != null)
        {
            this.OnClose();
        }
    }

    private GameObject Root;

    private GameObject btn_local;

    private GameObject btn_cemetery;

    public Action OnClose;

    private MSG_Ret_MainUserDeath_SC reviveData;
}
