using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AbattoirTransfer : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitGameObject(root);
        this.InitEvent();
    }

    private void InitGameObject(Transform root)
    {
        this.mRoot = root;
        this.btnClose = this.mRoot.Find("Offset_Confirm/Panel_Window/btn_close");
        for (int i = 0; i < 8; i++)
        {
            Transform transform = this.mRoot.Find("Offset_Confirm/Panel_Window/btns/btn_" + i);
            if (transform != null)
            {
                this.btns.Add(transform);
            }
        }
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.btnClose.gameObject).onClick = delegate (PointerEventData eventData)
        {
            if (this.callback != null)
            {
                this.callback(-1);
                this.callback = null;
            }
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this);
        };
        this.InitTransferListConfig();
        for (int i = 0; i < this.list.Count; i++)
        {
            int index = i;
            if (i >= this.btns.Count)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "tranferPosListCouny:",
                    this.list.Count,
                    " > btns.Count:",
                    this.btns.Count
                }));
                break;
            }
            if (!(this.btns[i] == null))
            {
                Text component = this.btns[i].Find("name").GetComponent<Text>();
                if (component != null)
                {
                    component.text = this.list[i].groupName;
                }
                this.btns[i].gameObject.SetActive(true);
                UIEventListener.Get(this.btns[i].gameObject).onClick = delegate (PointerEventData eventData)
                {
                    if (this.callback != null)
                    {
                        this.callback(this.list[index].group);
                        this.callback = null;
                    }
                    ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this);
                };
            }
        }
        for (int j = this.list.Count; j < this.btns.Count; j++)
        {
            this.btns[j].gameObject.SetActive(false);
        }
    }

    private void InitTransferListConfig()
    {
        if (this.list == null)
        {
            this.list = new List<UI_AbattoirTransfer.TransferItem>();
            LuaTable field_Table = LuaConfigManager.GetXmlConfigTable("mobapk").GetField_Table("transgroup");
            if (field_Table == null)
            {
                return;
            }
            for (int i = 0; i < field_Table.Count; i++)
            {
                LuaTable luaTable = field_Table[i + 1] as LuaTable;
                if (luaTable == null)
                {
                    FFDebug.LogError(this, "mobapk -> transgroup -> item==null:" + field_Table.ToString());
                }
                else
                {
                    int field_Int = luaTable.GetField_Int("group");
                    string field_String = luaTable.GetField_String("groupname");
                    this.list.Add(new UI_AbattoirTransfer.TransferItem
                    {
                        group = field_Int,
                        groupName = field_String
                    });
                }
            }
            field_Table.Dispose();
            if (this.list.Count == 0)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    "tranferPosListCouny:",
                    this.list.Count,
                    " > btns.Count:",
                    this.btns.Count
                }));
            }
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.DisposeEvent();
        this.DisposeGameObject();
    }

    private void DisposeEvent()
    {
        this.callback = null;
    }

    private void DisposeGameObject()
    {
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public void Refresh(Action<int> callback)
    {
        this.callback = callback;
    }

    public Transform mRoot;

    public Transform btnClose;

    public List<Transform> btns = new List<Transform>();

    private Action<int> callback;

    private List<UI_AbattoirTransfer.TransferItem> list;

    public class TransferItem
    {
        public int group;

        public string groupName;
    }
}
