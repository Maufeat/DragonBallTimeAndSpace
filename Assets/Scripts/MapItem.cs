using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapItem
{
    public MapItem(GameMap map)
    {
        this.Map = map;
        if (this.Map == null)
        {
            FFDebug.LogWarning(this, "GameMap null");
        }
        this.mMapController = ControllerManager.Instance.GetController<UIMapController>();
    }

    public string MID
    {
        get
        {
            return this._MID;
        }
        set
        {
            if (this._MID != value)
            {
                this._MID = value;
                if (this._MID.IndexOf(":") > -1)
                {
                    string s = this._MID.Substring(this._MID.LastIndexOf(":") + 1);
                    this._longKey = ulong.Parse(s);
                }
            }
        }
    }

    public void SetIcon(string icontype, int Priority, GameMap.ItemType MType, bool isTaskNpc = false, bool force = false, Vector2 iconLocalSize = default(Vector2))
    {
        try
        {
            this.IsFunctionNpc = !MapItem.emInstance.FuncNpcMap.CheckBetterForeach((KeyValuePair<ulong, Npc> item) => item.Key != this._longKey);
            if (this.Map != null)
            {
                if (!(this.Map.MapImage.transform == null))
                {
                    if (!string.IsNullOrEmpty(icontype))
                    {
                        if (force || !this.IsTaskNpc)
                        {
                            if (!this.Icontype.Equals(icontype) || this.Icon == null)
                            {
                                if (!string.IsNullOrEmpty(icontype))
                                {
                                    this.DestroyIcon();
                                    this.Icon = this.Map.IconMgr.GetIcon(icontype);
                                    if (this.Icon != null)
                                    {
                                        this.Icon.SetActive(this.IsShow);
                                        this.Icon.transform.SetParent(this.Map.MapImage.transform);
                                        this.Icon.transform.localScale = Vector3.one;
                                        this.SetIconItemUIEvent(this.Icon.gameObject, icontype);
                                        this.Icon.name = this.MID;
                                        if (this.IconLocalSize != default(Vector2))
                                        {
                                            (this.Icon.transform as RectTransform).sizeDelta = this.IconLocalSize;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.SetMyPosText();
                            }
                            this.currPriority = Priority;
                            this.Icontype = icontype;
                            this.MapType = MType;
                            this.IsTaskNpc = isTaskNpc;
                            this.IconLocalSize = iconLocalSize;
                        }
                    }
                }
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, string.Format("SetIcon: {0} error : {1}", icontype, arg));
        }
    }

    private static EntitiesManager emInstance
    {
        get
        {
            if (MapItem.emInstance_ == null)
            {
                MapItem.emInstance_ = ManagerCenter.Instance.GetManager<EntitiesManager>();
            }
            return MapItem.emInstance_;
        }
    }

    public void TryInitOwnerChar(string uid)
    {
        if (string.IsNullOrEmpty(uid))
        {
            return;
        }
        string text = uid.Substring(uid.LastIndexOf(":") + 1);
        uid = text;
        if (this.OwnerChar == null && MapItem.emInstance != null)
        {
            ulong num = ulong.Parse(uid);
            if (MapItem.emInstance.CurrentNineScreenPlayers.Keys.Contains(num))
            {
                this.OwnerChar = MapItem.emInstance.CurrentNineScreenPlayers[num];
            }
            else if (MapItem.emInstance.bdicCurrentVisiblePlayer.Keys.Contains(num))
            {
                this.OwnerChar = MapItem.emInstance.bdicCurrentVisiblePlayer[num];
            }
            else if (MapItem.emInstance.bdicCurrentHidePlayer.Keys.Contains(num))
            {
                this.OwnerChar = MapItem.emInstance.bdicCurrentHidePlayer[num];
            }
            else if (MapItem.emInstance.NpcList.Keys.Contains(num))
            {
                this.OwnerChar = MapItem.emInstance.NpcList[num];
            }
            else if (MapItem.emInstance.FuncNpcMap.Keys.Contains(num))
            {
                this.OwnerChar = MapItem.emInstance.FuncNpcMap[num];
            }
            else if (MapItem.emInstance.bdicNpcKindOne.Keys.Contains(num))
            {
                this.OwnerChar = MapItem.emInstance.bdicNpcKindOne[num];
            }
        }
    }

    private void SetMyPosText()
    {
        if (this.MapType == GameMap.ItemType.My)
        {
            this.Map.TextPos.text = "X:" + ((int)this.ServerPos.x).ToString() + "   Y:" + ((int)this.ServerPos.y).ToString();
        }
    }

    private void SetIconItemUIEvent(GameObject target, string icontype)
    {
        UIMapController uiMapController = ControllerManager.Instance.GetController<UIMapController>();
        Image imageTarget = target.GetComponentInChildren<Image>(true);
        if (imageTarget == null)
        {
            return;
        }
        imageTarget.raycastTarget = true;
        UIEventListener.Get(imageTarget.gameObject).onClick = null;
        UIEventListener.Get(imageTarget.gameObject).onClick = delegate (PointerEventData data)
        {
            this.SelectedNPCBtnByID();
            if (Constant.CUR_VRESION != Constant.Version.Release && data.button == PointerEventData.InputButton.Right && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                uiMapController.MapUI.GoBigMapPointFast();
            }
        };
        UIEventListener.Get(imageTarget.gameObject).onEnter = delegate (PointerEventData data)
        {
            this.TryInitOwnerChar(this.MID);
            if (this.OwnerChar == null)
            {
                return;
            }
            if (uiMapController.MapUI.hoverTipsRoot.parent != target.transform.parent)
            {
                uiMapController.MapUI.hoverTipsRoot.SetParent(target.transform.parent);
            }
            Image component = target.transform.parent.parent.GetComponent<Image>();
            Image imageBg = uiMapController.MapUI.hoverTipsRoot.GetComponent<Image>();
            Text textTips = uiMapController.MapUI.hoverTipsRoot.GetComponentInChildren<Text>();
            textTips.text = GlobalRegister.ConfigColorToRichTextFormat(this.GetOwnerTipsStr());
            textTips.color = imageTarget.color;
            Scheduler.Instance.AddFrame(3U, false, delegate
            {
                imageBg.rectTransform.sizeDelta = textTips.rectTransform.sizeDelta * 1.1f;
            });
            Vector3 serverPosByMouseOnBigMapPoint = uiMapController.MapUI.GetServerPosByMouseOnBigMapPoint();
            float num = uiMapController.AreamapMgr.SceneSize / 2f;
            Vector3 localPosition = new Vector3((serverPosByMouseOnBigMapPoint.x - num) * uiMapController.AreamapMgr.MapSecneRate, (serverPosByMouseOnBigMapPoint.y * -1f + num) * uiMapController.AreamapMgr.MapSecneRate, 0f);
            uiMapController.MapUI.hoverTipsRoot.transform.localPosition = localPosition;
            imageBg.gameObject.SetActive(true);
            uiMapController.MapUI.hoverTipsRoot.transform.SetAsLastSibling();
            this.mMapController.isShowAreaTipReaded = string.IsNullOrEmpty(textTips.text);
        };
        UIEventListener.Get(imageTarget.gameObject).onExit = null;
        UIEventListener.Get(imageTarget.gameObject).onExit = delegate (PointerEventData data)
        {
            this.mMapController.isShowAreaTipReaded = true;
        };
    }

    private string GetOwnerTipsStr()
    {
        if (this.OwnerChar != null)
        {
            if (this.OwnerChar is OtherPlayer)
            {
                float fx = (this.OwnerChar as OtherPlayer).OtherPlayerData.MapUserData.mapdata.pos.fx;
                float fy = (this.OwnerChar as OtherPlayer).OtherPlayerData.MapUserData.mapdata.pos.fy;
                if (MainPlayer.Self.OtherPlayerData.MapUserData.charid == (this.OwnerChar as OtherPlayer).OtherPlayerData.MapUserData.charid)
                {
                    return (this.OwnerChar as OtherPlayer).OtherPlayerData.MapUserData.name;
                }
                return string.Concat(new object[]
                {
                    (this.OwnerChar as OtherPlayer).OtherPlayerData.MapUserData.name,
                    "\n(",
                    (int)fx,
                    ",",
                    (int)fy,
                    ")"
                });
            }
            else if (this.OwnerChar is Npc)
            {
                Npc npc = this.OwnerChar as Npc;
                if (string.IsNullOrEmpty(npc.NpcData.MapNpcData.name))
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                    if (configTable != null)
                    {
                        npc.NpcData.MapNpcData.name = configTable.GetCacheField_String("name");
                    }
                }
                if (this.IsTaskNpc)
                {
                    UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
                    if (controller.npcShowFirstTaskDic.ContainsKey(npc.EID.Id))
                    {
                        uint questid = controller.npcShowFirstTaskDic[npc.EID.Id].questid;
                        LuaTable configTable2 = LuaConfigManager.GetConfigTable("questconfig", (ulong)questid);
                        return string.Concat(new object[]
                        {
                            configTable2.GetField_String("name"),
                            "\n(",
                            npc.CurrServerPos.x,
                            ",",
                            npc.CurrServerPos.y,
                            ")"
                        });
                    }
                }
                else if (this.IsFunctionNpc)
                {
                    UIMapController controller2 = ControllerManager.Instance.GetController<UIMapController>();
                    LuaTable configTable3 = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
                    if (configTable3 != null)
                    {
                        return string.Concat(new object[]
                        {
                            configTable3.GetField_String("name"),
                            "\n(",
                            npc.CurrServerPos.x,
                            ",",
                            npc.CurrServerPos.y,
                            ")"
                        });
                    }
                }
                return string.Concat(new object[]
                {
                    npc.NpcData.MapNpcData.name,
                    "\n(",
                    npc.CurrServerPos.x,
                    ",",
                    npc.CurrServerPos.y,
                    ")"
                });
            }
        }
        return string.Empty;
    }

    public void SetLabel(string icontype)
    {
        if (this.Map == null)
        {
            return;
        }
        if (this.Map.MapImage == null)
        {
            FFDebug.LogWarning(this, "MapImage null");
            return;
        }
        if (this.Map.MapImage.transform == null)
        {
            return;
        }
        if ((!this.Icontype.Equals(icontype) || this.Icon == null) && !string.IsNullOrEmpty(icontype))
        {
            this.DestroyIcon();
            this.Icon = this.Map.IconMgr.GetTextIcon(icontype);
            if (this.Icon != null)
            {
                this.Icon.transform.SetParent(this.Map.MapImage.transform);
                this.Icon.transform.localScale = Vector3.one;
            }
            else
            {
                FFDebug.LogWarning(this, "Icon null");
            }
        }
        this.Icontype = icontype;
        this.MapType = GameMap.ItemType.Text;
    }

    public void SetPosByServer(Vector2 pos)
    {
        if (this.Map == null)
        {
            return;
        }
        this.ServerPos = pos;
        this.MapPos = new Vector3(pos.x - this.Map.SceneSize / 2f, -pos.y + this.Map.SceneSize / 2f, 0f) * this.Map.MapSecneRate;
        if (this.Icon == null)
        {
            return;
        }
        this.Icon.transform.localPosition = this.MapPos;
    }

    public void SetItemShowOrHide(bool show)
    {
        this.IsShow = show;
        if (this.Icon != null)
        {
            this.Icon.SetActive(this.IsShow);
        }
    }

    public void SetPosInMap(Vector2 newpos)
    {
        Vector3 localPosition = this.Icon.transform.localPosition;
        this.Icon.transform.localPosition = newpos;
    }

    public void SetDirByServer(uint Dir)
    {
        if (this.Icon == null)
        {
            FFDebug.LogWarning(this, "Icon Null");
            return;
        }
        Quaternion localRotation = Quaternion.AngleAxis(Dir * 2f + 180f, Vector3.back);
        this.Icon.transform.localRotation = localRotation;
    }

    public void ReSetPos()
    {
        this.SetPosByServer(this.ServerPos);
    }

    public void DestroyIcon()
    {
        if (this.Icon != null)
        {
            try
            {
                UIEventListener.Get(this.Icon.gameObject).onClick = null;
                UnityEngine.Object.Destroy(this.Icon);
                this.Icon = null;
                this.IconLocalSize = default(Vector2);
                this.Icontype = string.Empty;
                this.currPriority = -1;
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, string.Format("DestroyIcon error: {0}", arg));
            }
        }
    }

    public void SetImageSizeData(Vector2 newSize)
    {
        if (this.Icon != null)
        {
            this.IconLocalSize = newSize;
            (this.Icon.transform as RectTransform).sizeDelta = this.IconLocalSize;
        }
    }

    public Sprite GetImageSprite()
    {
        if (this.Icon != null)
        {
            Image component = this.Icon.GetComponent<Image>();
            if (component != null)
            {
                return component.sprite;
            }
        }
        return null;
    }

    public Color GetImageColor()
    {
        if (this.Icon != null)
        {
            Image component = this.Icon.GetComponent<Image>();
            if (component != null)
            {
                return component.color;
            }
        }
        return Color.white;
    }

    public void Hide()
    {
        if (this.Icon != null)
        {
            this.Icon.SetActive(false);
        }
    }

    public void Show()
    {
        if (this.Icon != null)
        {
            this.Icon.SetActive(true);
        }
    }

    private void SelectedNPCBtnByID()
    {
        if (this.OwnerChar == null || this.OwnerChar.EID.Etype != CharactorType.NPC)
        {
            return;
        }
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        if (controller != null && controller.MapUI != null)
        {
            controller.MapUI.SelectNPCBtnByPathID(this.mPathWayId);
        }
    }

    public string OutmapIcontype = string.Empty;

    public string Icontype = string.Empty;

    public GameObject Icon;

    public GameObject OutmapIcon;

    public Vector3 MapPos;

    public Vector3 ServerPos;

    private ulong _longKey;

    private string _MID;

    private GameMap Map;

    public int currPriority = -1;

    public uint mPathWayId;

    public CharactorBase OwnerChar;

    public bool IsTaskNpc;

    public bool IsFunctionNpc;

    public bool IsShow = true;

    private UIMapController mMapController;

    public Vector2 IconLocalSize = default(Vector2);

    public GameMap.ItemType MapType;

    private static EntitiesManager emInstance_;
}
