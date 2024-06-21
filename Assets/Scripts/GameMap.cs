using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class GameMap
{
    public GameMap(RawImage Map, Transform OrgIcon, float sceneSize, float areaRate, Vector2 Rect, float Angle = 0f, string mapname = "")
    {
        this.Mapname = mapname;
        this.MapImage = Map;
        this.MapAngle = Angle;
        this.MapImage.transform.localEulerAngles = new Vector3(0f, 0f, this.MapAngle);
        this.IconMgr = new GameMap.MapIconMgr(OrgIcon);
        this.SceneSize = sceneSize;
        this.MapRect = this.MapImage.GetComponent<RectTransform>();
        this.SetMapSecneRate(areaRate);
        this.LoadMap();
        this.MapShowRect = Rect;
        if (Map.transform.parent.parent.parent.FindChild("img_pos/Text") != null)
        {
            this.TextPos = Map.transform.parent.parent.parent.FindChild("img_pos/Text").GetComponent<Text>();
        }
        if (Map.transform.parent.parent.FindChild("img_pos/Text") != null)
        {
            this.TextPos = Map.transform.parent.parent.FindChild("img_pos/Text").GetComponent<Text>();
        }
    }

    public void LoadMap()
    {
        if (string.IsNullOrEmpty(this.Mapname))
        {
            this.Mapname = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.fileName();
        }
        string text = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapName();
        if (!string.IsNullOrEmpty(text))
        {
            this.Mapname = text;
        }
        FFDebug.Log(this, FFLogType.Default, string.Format("loadmap : {0}", this.Mapname));
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.OTHERS, this.Mapname + "_map", delegate (UITextureAsset item)
        {
            if (this.MapImage == null)
            {
                return;
            }
            if (item == null)
            {
                return;
            }
            this.usedTextureAssets.Add(item);
            this.MapImage.gameObject.SetActive(true);
            this.MapImage.texture = item.textureObj;
            this.MapImage.color = Color.white;
        });
    }

    public void SetMapSecneRate(float rate)
    {
        if (Math.Abs(this.MapSecneRate - rate) < 0.01f)
        {
            return;
        }
        this.MapSecneRate = rate;
        this.MapRect.sizeDelta = new Vector2(this.SceneSize * this.MapSecneRate, this.SceneSize * this.MapSecneRate);
        this.ResetIconPos();
    }

    public void ResetIconPos()
    {
        this.MapItemList.BetterForeach(delegate (KeyValuePair<string, MapItem> item)
        {
            item.Value.ReSetPos();
        });
    }

    public MapItem AddMyIcon()
    {
        if (this.MyIcon == null)
        {
            MapItem orAddIcon = this.GetOrAddIcon(MainPlayer.Self.EID.ToString(), GameMap.ItemType.My, false);
            orAddIcon.SetIcon("img_me", 3, GameMap.ItemType.My, false, false, default(Vector2));
            this.MyIcon = orAddIcon;
        }
        return this.MyIcon;
    }

    public MapItem GetOrAddIcon(string subid, GameMap.ItemType type, bool isNPC = false)
    {
        string text;
        if (isNPC)
        {
            text = subid;
        }
        else
        {
            text = GameMap.GetItemKey(subid, type);
        }
        MapItem mapItem;
        if (!this.MapItemList.ContainsKey(text))
        {
            mapItem = new MapItem(this);
            mapItem.MID = text;
            this.MapItemList.Add(mapItem.MID, mapItem);
        }
        else
        {
            mapItem = this.MapItemList[text];
        }
        return mapItem;
    }

    public static string GetItemKey(string Subid, GameMap.ItemType type)
    {
        return type + ":" + Subid;
    }

    public MapItem SetIconInfo(string subid, GameMap.ItemType type, string icontype, Vector2 pos, int Priority, bool isTaskNpc = false, bool force = false, CharactorBase cb = null, Vector2 sizeData = default(Vector2))
    {
        try
        {
            MapItem orAddIcon = this.GetOrAddIcon(subid, type, false);
            orAddIcon.SetIcon(icontype, Priority, type, isTaskNpc, force, sizeData);
            orAddIcon.SetPosByServer(pos);
            return orAddIcon;
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, string.Format("SetIconInfo error {0}", arg));
        }
        return null;
    }

    public MapItem SetNPCIconInfo(string subid, GameMap.ItemType type, string icontype, Vector2 pos, int Priority, bool isTaskNpc = false, bool force = false, CharactorBase cb = null)
    {
        try
        {
            MapItem orAddIcon = this.GetOrAddIcon(subid, type, true);
            orAddIcon.SetIcon(icontype, Priority, type, isTaskNpc, force, default(Vector2));
            orAddIcon.SetPosByServer(pos);
            return orAddIcon;
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, string.Format("SetIconInfo error {0}", arg));
        }
        return null;
    }

    public Vector3 GetNpcAreaLeftBottomPos(uint baseid)
    {
        if (this.npcIconLeftBottom.ContainsKey(baseid))
        {
            return this.npcIconLeftBottom[baseid];
        }
        return Vector3.zero;
    }

    public Vector3 GetNpcAreaRightTopPos(uint baseid)
    {
        if (this.npcIconRightTop.ContainsKey(baseid))
        {
            return this.npcIconRightTop[baseid];
        }
        return Vector3.zero;
    }

    public void OpenShowOneNpc(uint baseid)
    {
        this.CloseShowOneNpc();
        if (this.mapShowOneIconDic.ContainsKey(baseid))
        {
            List<MapItem> list = this.mapShowOneIconDic[baseid];
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetItemShowOrHide(true);
            }
            this.mCurBaseid = baseid;
        }
    }

    public void CloseShowOneNpc()
    {
        if (this.mapShowOneIconDic.ContainsKey(this.mCurBaseid))
        {
            List<MapItem> list = this.mapShowOneIconDic[this.mCurBaseid];
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetItemShowOrHide(i == 0);
            }
        }
    }

    public void ClearShowOneData()
    {
        this.mapShowOneIconDic.Clear();
        this.mapShowOneIconDic = new Dictionary<uint, List<MapItem>>();
    }

    public MapItem SetNPCIconInfo(CharactorBase Charactor, GameMap.ItemType type, string icontype, Vector2 pos, int Priority, bool isTaskNpc = false, bool force = false)
    {
        MapItem mapItem = this.SetIconInfo(Charactor.EID.ToString(), type, icontype, pos, Priority, isTaskNpc, force, null, default(Vector2));
        if (mapItem != null)
        {
            mapItem.OwnerChar = Charactor;
            if (Charactor is Npc)
            {
                Npc npc = Charactor as Npc;
                if (npc.CheckIsShowByTask(false))
                {
                    uint baseid = npc.NpcData.MapNpcData.baseid;
                    LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
                    if (configTable.GetField_Uint("show_map") == 1U)
                    {
                        if (this.mapShowOneIconDic.ContainsKey(baseid))
                        {
                            List<MapItem> list = this.mapShowOneIconDic[baseid];
                            if (list.Contains(mapItem))
                            {
                                if (list.IndexOf(mapItem) == 0)
                                {
                                    mapItem.SetItemShowOrHide(true);
                                }
                                else
                                {
                                    mapItem.SetItemShowOrHide(false);
                                }
                            }
                            else
                            {
                                mapItem.SetItemShowOrHide(false);
                                this.mapShowOneIconDic[baseid].Add(mapItem);
                            }
                        }
                        else
                        {
                            mapItem.SetItemShowOrHide(true);
                            this.mapShowOneIconDic.Add(baseid, new List<MapItem>());
                            this.mapShowOneIconDic[baseid].Add(mapItem);
                        }
                    }
                    else
                    {
                        mapItem.SetItemShowOrHide(true);
                    }
                    Vector3 mapPos = mapItem.MapPos;
                    if (!this.npcIconLeftBottom.ContainsKey(baseid))
                    {
                        this.npcIconLeftBottom.Add(baseid, mapPos);
                    }
                    else
                    {
                        Vector3 value = this.npcIconLeftBottom[baseid];
                        if (value.x > mapPos.x)
                        {
                            value.x = mapPos.x;
                        }
                        if (value.y > mapPos.y)
                        {
                            value.y = mapPos.y;
                        }
                        this.npcIconLeftBottom[baseid] = value;
                    }
                    if (!this.npcIconRightTop.ContainsKey(baseid))
                    {
                        this.npcIconRightTop.Add(baseid, mapPos);
                    }
                    else
                    {
                        Vector3 value2 = this.npcIconRightTop[baseid];
                        if (value2.x < mapPos.x)
                        {
                            value2.x = mapPos.x;
                        }
                        if (value2.y < mapPos.y)
                        {
                            value2.y = mapPos.y;
                        }
                        this.npcIconRightTop[baseid] = value2;
                    }
                }
                else
                {
                    mapItem.SetItemShowOrHide(false);
                }
            }
        }
        return mapItem;
    }

    public MapItem SetLabelInfo(string subid, GameMap.ItemType type, string icontype, Vector2 pos)
    {
        MapItem orAddIcon = this.GetOrAddIcon(subid, type, false);
        orAddIcon.SetLabel(icontype);
        orAddIcon.SetPosByServer(pos);
        return orAddIcon;
    }

    private void CheckIconOutMap(MapItem Item)
    {
        if (!string.IsNullOrEmpty(Item.OutmapIcontype) && this.MapShowRect.x > 0f && this.MapShowRect.y > 0f)
        {
            float num = 0f;
            Vector2 posInMap;
            if (this.IsOutMap(Item.MapPos, out posInMap, out num))
            {
                Item.SetPosInMap(posInMap);
                Item.SetDirByServer((uint)num);
            }
        }
    }

    private bool IsOutMap(Vector2 pos, out Vector2 fix, out float angle)
    {
        float num = -1f;
        bool flag = false;
        fix = pos;
        angle = -720f;
        if (pos.x > this.MyIcon.MapPos.x + this.MapShowRect.x / 2f - num)
        {
            flag = true;
            fix.x = this.MyIcon.MapPos.x + this.MapShowRect.x / 2f - num;
        }
        else if (pos.x < this.MyIcon.MapPos.x - this.MapShowRect.x / 2f + num)
        {
            flag = true;
            fix.x = this.MyIcon.MapPos.x - this.MapShowRect.x / 2f + num;
        }
        if (pos.y > this.MyIcon.MapPos.y + this.MapShowRect.y / 2f - num)
        {
            flag = true;
            fix.y = this.MyIcon.MapPos.y + this.MapShowRect.y / 2f - num;
        }
        else if (pos.y < this.MyIcon.MapPos.y - this.MapShowRect.y / 2f + num)
        {
            flag = true;
            fix.y = this.MyIcon.MapPos.y - this.MapShowRect.y / 2f + num;
        }
        if (flag)
        {
            Vector2 b = new Vector2(this.MyIcon.MapPos.x, this.MyIcon.MapPos.y);
            Vector2 to = fix - b;
            angle = Vector2.Angle(Vector2.up, to);
            if (to.x < 0f)
            {
                angle = 360f - angle;
            }
            angle /= 2f;
        }
        return flag;
    }

    public MapItem SetIconMy(Vector2 pos, uint Dir)
    {
        if (this.MyIcon == null)
        {
            this.AddMyIcon();
        }
        this.MyIcon.SetPosByServer(pos);
        this.MyIcon.SetIcon("img_me", 3, GameMap.ItemType.My, false, false, default(Vector2));
        this.MyIcon.SetDirByServer(Dir);
        return this.MyIcon;
    }

    public void DeleteIconbyType(GameMap.ItemType type)
    {
        this.MapItemList.BetterForeach(delegate (KeyValuePair<string, MapItem> item)
        {
            if (item.Value.MID.StartsWith(type.ToString()))
            {
                item.Value.DestroyIcon();
                this.MapItemList.Remove(item.Key);
            }
        });
    }

    public void DeleteIcon(string subid, GameMap.ItemType type)
    {
        string itemKey = GameMap.GetItemKey(subid, type);
        if (this.MapItemList.ContainsKey(itemKey))
        {
            MapItem mapItem = this.MapItemList[itemKey];
            mapItem.DestroyIcon();
            this.MapItemList.Remove(itemKey);
        }
    }

    public void HideNpcIcon(List<string> typeList)
    {
        this.MapItemList.BetterForeach(delegate (KeyValuePair<string, MapItem> item)
        {
            if (item.Value.MID.StartsWith(GameMap.ItemType.NPC.ToString()) && item.Value.Icon != null)
            {
                item.Value.Icon.SetActive(!typeList.Contains(item.Value.Icontype));
            }
        });
    }

    private bool IsAllHideed()
    {
        List<string> list = new List<string>(this.MapItemList.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            MapItem mapItem = this.MapItemList[list[i]];
            if (mapItem.MapType == GameMap.ItemType.Path && mapItem.Icon != null && mapItem.Icon.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    public void SetPathIconList(List<Vector2> Pathv2s)
    {
        if (Pathv2s == null || Pathv2s.Count == 0 || this.mPrePathv2s == null || (this.mPrePathv2s != null && this.mPrePathv2s.Count != 0 && this.mPrePathv2s[this.mPrePathv2s.Count - 1] != Pathv2s[Pathv2s.Count - 1]) || this.IsAllHideed())
        {
            this.HideAllPathIcon();
            if (Pathv2s == null)
            {
                return;
            }
            int num = 0;
            for (int i = Pathv2s.Count - 1; i >= 0; i--)
            {
                Vector2 pos = Pathv2s[i];
                int num2 = Pathv2s.Count - 1 - i;
                bool flag = false;
                if (num2 % this.PathIconinterval == 0)
                {
                    if (!flag && num2 < 8)
                    {
                        flag = true;
                    }
                    MapItem mapItem = this.SetPathIcon(num++, pos, flag);
                    if (mapItem != null)
                    {
                        mapItem.Show();
                    }
                }
            }
            this.mPrePathv2s = new List<Vector2>(Pathv2s);
        }
        else
        {
            int num3 = (this.mPrePathv2s.Count - 1) / this.PathIconinterval;
            int num4 = 0;
            for (int j = 0; j < this.mPrePathv2s.Count; j++)
            {
                if (Pathv2s[0] == this.mPrePathv2s[j])
                {
                    break;
                }
                num4++;
            }
            for (int k = (this.mPrePathv2s.Count - num4 - 1) / this.PathIconinterval; k <= num3; k++)
            {
                MapItem mapItem2 = this.SetPathIcon(k, Vector2.zero, false);
                if (mapItem2 != null)
                {
                    mapItem2.Hide();
                }
            }
        }
    }

    private MapItem SetPathIcon(int index, Vector2 pos, bool isDest)
    {
        try
        {
            MapItem orAddIcon = this.GetOrAddIcon(string.Empty + index, GameMap.ItemType.Path, false);
            orAddIcon.SetIcon((!isDest) ? "img_point" : "img_point_dest", 0, GameMap.ItemType.Path, false, false, default(Vector2));
            orAddIcon.SetPosByServer(pos);
            return orAddIcon;
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, string.Format("SetPathIcon error {0}", arg));
        }
        return null;
    }

    public void HideAllPathIcon()
    {
        this.MapItemList.BetterForeach(delegate (KeyValuePair<string, MapItem> item)
        {
            if (item.Value.MapType == GameMap.ItemType.Path)
            {
                item.Value.Hide();
            }
        });
    }

    public List<MapItem> GetNpcItemList()
    {
        this.NpcItemTmp.Clear();
        this.MapItemList.BetterForeach(delegate (KeyValuePair<string, MapItem> item)
        {
            if (item.Value.MapType == GameMap.ItemType.NPC && item.Value.IsShow)
            {
                Npc npc = item.Value.OwnerChar.BaseData.Owner as Npc;
                if (npc != null && npc.IsMapShowNpc())
                {
                    this.NpcItemTmp.Add(item.Value);
                }
            }
        });
        return this.NpcItemTmp;
    }

    public void SetMeToCenter()
    {
        Quaternion rotation = Quaternion.AngleAxis(this.MapAngle, Vector3.forward);
        Vector3 point = new Vector3(-this.MyIcon.MapPos.x, -this.MyIcon.MapPos.y, 0f);
        this.MapImage.transform.localPosition = rotation * point;
        float num = (this.MapRect.sizeDelta.x - (this.MapImage.transform.parent as RectTransform).sizeDelta.x) / 2f;
        if (num < 0f)
        {
            num = 0f;
        }
        if (this.MapImage.transform.localPosition.x < -num)
        {
            this.MapImage.transform.localPosition = new Vector3(-num, this.MapImage.transform.localPosition.y, 0f);
        }
        else if (this.MapImage.transform.localPosition.x > num)
        {
            this.MapImage.transform.localPosition = new Vector3(num, this.MapImage.transform.localPosition.y, 0f);
        }
        if (this.MapImage.transform.localPosition.y < -num)
        {
            this.MapImage.transform.localPosition = new Vector3(this.MapImage.transform.localPosition.x, -num, 0f);
        }
        else if (this.MapImage.transform.localPosition.y > num)
        {
            this.MapImage.transform.localPosition = new Vector3(this.MapImage.transform.localPosition.x, num, 0f);
        }
    }

    public void ClearMap()
    {
        for (int i = 0; i < this.usedTextureAssets.Count; i++)
        {
            this.usedTextureAssets[i].TryUnload();
        }
        this.usedTextureAssets.Clear();
        this.MapItemList.BetterForeach(delegate (KeyValuePair<string, MapItem> item)
        {
            item.Value.DestroyIcon();
        });
        this.MapItemList.Clear();
    }

    public void ReSortMapItemShowOrder()
    {
    }

    public float SceneSize = 150f;

    public float MapSecneRate;

    public RawImage MapImage;

    public Text TextPos;

    private RectTransform MapRect;

    public MapItem MyIcon;

    private float MapAngle;

    public GameMap.MapIconMgr IconMgr;

    private BetterDictionary<string, MapItem> MapItemList = new BetterDictionary<string, MapItem>();

    public List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();

    private Vector2 MapShowRect;

    private string Mapname;

    private Dictionary<uint, List<MapItem>> mapShowOneIconDic = new Dictionary<uint, List<MapItem>>();

    private Dictionary<uint, Vector3> npcIconLeftBottom = new Dictionary<uint, Vector3>();

    private Dictionary<uint, Vector3> npcIconRightTop = new Dictionary<uint, Vector3>();

    private uint mCurBaseid;

    private List<Vector2> mPrePathv2s;

    private int PathIconinterval = 8;

    private List<MapItem> NpcItemTmp = new List<MapItem>();

    public class MapIconMgr
    {
        public MapIconMgr(Transform root)
        {
            this.IconRoot = root;
            this.IconRoot.gameObject.SetActive(false);
            this.AddIconToIconOrgObj("img_me");
            this.AddIconToIconOrgObj("img_friend");
            this.AddIconToIconOrgObj("iconenemy/img_player");
            this.AddIconToIconOrgObj("iconteam/img_team1");
            this.AddIconToIconOrgObj("iconteam/img_team2");
            this.AddIconToIconOrgObj("iconteam/img_team3");
            this.AddIconToIconOrgObj("icontask/img_no");
            this.AddIconToIconOrgObj("icontask/img_in");
            this.AddIconToIconOrgObj("icontask/img_ok");
            this.AddIconToIconOrgObj("icontask/img_go");
            this.AddIconToIconOrgObj("img_point");
            this.AddIconToIconOrgObj("img_point_dest");
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("npcmapinfo").GetCacheField_Table("npcmapitem");
            if (cacheField_Table != null)
            {
                IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    LuaTable luaTable = obj as LuaTable;
                    this.AddIconToIconOrgObj(luaTable.GetField_String("icon"));
                }
            }
            if (this.IconRoot.FindChild("Text") != null)
            {
                this.TextObj = this.IconRoot.FindChild("Text").gameObject;
            }
        }

        public void AddIconToIconOrgObj(string path)
        {
            Transform transform = this.IconRoot.transform.FindChild(path);
            if (transform != null)
            {
                this.IconOrgObj[path] = transform.gameObject;
                if (this.orgimage == null)
                {
                    this.orgimage = transform.gameObject.GetComponent<Image>();
                }
            }
            else
            {
                if (this.loadCaches.Contains(path))
                {
                    return;
                }
                this.loadCaches.Add(path);
                string name = path.Substring(0, path.LastIndexOf('/'));
                string name2 = path.Substring(path.LastIndexOf('/') + 1);
                Transform father = this.IconRoot.Find(name);
                if (father != null)
                {
                    GameMap.MapIconMgr.CreateImage(name2, this.orgimage, delegate (Image image)
                    {
                        if (!this.IconOrgObj.ContainsKey(path))
                        {
                            this.IconOrgObj[path] = image.gameObject;
                            image.rectTransform.SetParent(father.GetComponent<RectTransform>(), Vector2.zero);
                        }
                    });
                }
                else
                {
                    FFDebug.LogWarning(this, "not find Icon: " + path);
                }
            }
        }

        public static void CreateImage(string name, Image otherimage, Action<Image> callback = null)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", name, delegate (Sprite sp)
            {
                if (sp != null)
                {
                    Image image = UnityEngine.Object.Instantiate<Image>(otherimage);
                    image.name = name;
                    image.sprite = sp;
                    RectTransform component = image.GetComponent<RectTransform>();
                    component.anchoredPosition = Vector2.zero;
                    component.sizeDelta = new Vector2(sp.rect.width, sp.rect.height);
                    if (callback != null)
                    {
                        callback(image);
                    }
                }
            });
        }

        public GameObject GetIcon(string type)
        {
            GameObject gameObject = null;
            if (!this.IconOrgObj.ContainsKey(type))
            {
                this.AddIconToIconOrgObj(type);
            }
            if (this.IconOrgObj.ContainsKey(type) && this.IconOrgObj[type] != null)
            {
                Image component = this.IconOrgObj[type].GetComponent<Image>();
                if (component != null)
                {
                    Image image = this.SaveImageClone(component);
                    if (image != null)
                    {
                        gameObject = image.gameObject;
                    }
                    else
                    {
                        FFDebug.LogWarning(this, "SaveImageClone error");
                    }
                }
                else
                {
                    gameObject = UnityEngine.Object.Instantiate<GameObject>(this.IconOrgObj[type]);
                }
                if (gameObject != null)
                {
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.gameObject.SetActive(true);
                }
            }
            return gameObject;
        }

        public GameObject GetTextIcon(string type)
        {
            Text org = null;
            if (this.TextObj != null)
            {
                org = this.TextObj.GetComponent<Text>();
            }
            Text text = this.SaveTextClone(org);
            if (text != null)
            {
                text.transform.localScale = Vector3.one;
                text.gameObject.SetActive(true);
                text.text = type;
                return text.gameObject;
            }
            FFDebug.LogWarning(this, "GetTextIcon Text null");
            return null;
        }

        private Text SaveTextClone(Text org)
        {
            GameObject gameObject;
            if (org != null)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(org.gameObject);
            }
            else
            {
                gameObject = new GameObject();
            }
            Text text = gameObject.GetComponent<Text>();
            if (text == null)
            {
                text = gameObject.AddComponent<Text>();
                if (org != null)
                {
                    text.font = org.font;
                    text.fontStyle = org.fontStyle;
                    text.fontSize = org.fontSize;
                    text.lineSpacing = org.lineSpacing;
                    text.alignment = org.alignment;
                    text.horizontalOverflow = org.horizontalOverflow;
                    text.color = org.color;
                }
            }
            return text;
        }

        private Image SaveImageClone(Image org)
        {
            GameObject gameObject;
            if (org != null)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(org.gameObject);
            }
            else
            {
                gameObject = new GameObject();
            }
            Image image = gameObject.GetComponent<Image>();
            if (image == null)
            {
                image = gameObject.AddComponent<Image>();
                if (org != null)
                {
                    image.eventAlphaThreshold = org.eventAlphaThreshold;
                    image.fillAmount = org.fillAmount;
                    image.fillCenter = org.fillCenter;
                    image.fillClockwise = org.fillClockwise;
                    image.fillMethod = org.fillMethod;
                    image.fillOrigin = org.fillOrigin;
                    image.overrideSprite = org.overrideSprite;
                    image.preserveAspect = org.preserveAspect;
                    image.sprite = org.sprite;
                    image.type = org.type;
                }
            }
            return image;
        }

        public const string Ionc_My = "img_me";

        public const string Ionc_Friend = "img_friend";

        public const string Ionc_Enemy = "iconenemy/img_player";

        public const string Ionc_Team_num1 = "iconteam/img_team1";

        public const string Ionc_Team_num2 = "iconteam/img_team2";

        public const string Ionc_Team_num3 = "iconteam/img_team3";

        public const string Ionc_Team_go = "iconteam/img_go";

        public const string Ionc_Task_no = "icontask/img_no";

        public const string Ionc_Task_in = "icontask/img_in";

        public const string Ionc_Task_ok = "icontask/img_ok";

        public const string Ionc_Task_go = "icontask/img_go";

        public const string Ionc_Task_dialog = "icontask/img_dialog";

        public const string Ionc_img_point = "img_point";

        public const string Ionc_img_point_dest = "img_point_dest";

        private Transform IconRoot;

        private BetterDictionary<string, GameObject> IconOrgObj = new BetterDictionary<string, GameObject>();

        private List<string> loadCaches = new List<string>();

        private GameObject TextObj;

        private Image orgimage;
    }

    public enum ItemType
    {
        None,
        NPC,
        Team,
        Enemy,
        My,
        Text,
        Path,
        Friend,
        ReliveArea,
        RadarArea,
        DraginBall,
        Capsule
    }
}
