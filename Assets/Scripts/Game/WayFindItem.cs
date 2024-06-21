using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WayFindItem
{
    public WayFindItem(string sInput)
    {
        if (string.IsNullOrEmpty(sInput))
        {
            return;
        }
        string[] array = new string[]
        {
            "{",
            "}"
        };
        for (int i = 0; i < array.Length; i++)
        {
            if (!sInput.Contains(array[i]))
            {
                return;
            }
        }
        this.TryInitNodeListByStream(sInput, false);
    }

    public WayFindItem(string sInput, bool isPrintResult)
    {
        if (string.IsNullOrEmpty(sInput))
        {
            return;
        }
        string[] array = new string[]
        {
            "{",
            "}"
        };
        for (int i = 0; i < array.Length; i++)
        {
            if (!sInput.Contains(array[i]))
            {
                return;
            }
        }
        this.TryInitNodeListByStream(sInput, isPrintResult);
    }

    public void TryInitWayNodeList(string srcText)
    {
        List<WayNode> list = new List<WayNode>();
        Regex regex = new Regex(WayFindItem.endStrPattern, RegexOptions.IgnoreCase);
        IEnumerator enumerator = regex.Matches(srcText).GetEnumerator();
        string text = string.Empty;
        if (enumerator.MoveNext())
        {
            Match match = (Match)enumerator.Current;
            text = match.Value;
        }
        string text2 = srcText;
        if (!string.IsNullOrEmpty(text))
        {
            text2 = srcText.Replace(text, string.Empty);
        }
        Regex regex2 = new Regex(WayFindItem.patternWayFind, RegexOptions.IgnoreCase);
        IEnumerator enumerator2 = regex2.Matches(text2).GetEnumerator();
        int num = 0;
        while (enumerator2.MoveNext())
        {
            object obj = enumerator2.Current;
            Match match2 = (Match)obj;
            string text3 = text2.Substring(text2.IndexOf(match2.Value));
            string text4 = text2.Replace(text3, string.Empty);
            string text5 = text3.Replace(match2.Value, string.Empty);
            if (!string.IsNullOrEmpty(text4))
            {
                list.Add(new WayNode
                {
                    wayNodeType = WayNodeType.Text,
                    text = GlobalRegister.ConfigColorToRichTextFormat(text4)
                });
            }
            WayNode wayNode = new WayNode();
            wayNode.wayNodeType = WayNodeType.Way;
            Regex regex3 = new Regex(WayFindItem.patternPlaceNameColor, RegexOptions.IgnoreCase);
            IEnumerator enumerator3 = regex3.Matches(match2.Value).GetEnumerator();
            wayNode.color = Color.white;
            if (enumerator3.MoveNext())
            {
                Match match3 = (Match)enumerator3.Current;
                string value = match3.Value.Substring(1, 2);
                string value2 = match3.Value.Substring(3, 2);
                string value3 = match3.Value.Substring(5, 2);
                wayNode.color = new Color((float)Convert.ToInt32(value, 16) / 255f, (float)Convert.ToInt32(value2, 16) / 255f, (float)Convert.ToInt32(value3, 16) / 255f);
            }
            string text6 = match2.Value.Replace("{", string.Empty).Replace("}", string.Empty);
            text6 = GlobalRegister.ConfigColorToRichTextFormat(text6);
            wayNode.text = text6;
            if (!string.IsNullOrEmpty(text6))
            {
                wayNode.wayIdIndex = num;
                list.Add(wayNode);
                num++;
            }
            if (!string.IsNullOrEmpty(text5))
            {
                if (text5.Contains("{") && text5.Contains("}"))
                {
                    text2 = text5;
                    enumerator2.Reset();
                    regex2 = new Regex(WayFindItem.patternWayFind, RegexOptions.IgnoreCase);
                    enumerator2 = regex2.Matches(text5).GetEnumerator();
                }
                else if (!string.IsNullOrEmpty(text5))
                {
                    list.Add(new WayNode
                    {
                        wayNodeType = WayNodeType.Text,
                        text = GlobalRegister.ConfigColorToRichTextFormat(text5)
                    });
                }
            }
        }
        if (!string.IsNullOrEmpty(text))
        {
            text = text.Replace("-", string.Empty);
            WayNode wayNode2 = new WayNode();
            wayNode2.wayNodeType = WayNodeType.EndPreFix;
            wayNode2.endPreFix = new Dictionary<uint, uint>();
            string[] array = text.Split(new char[]
            {
                ':'
            });
            wayNode2.endPreFix[uint.Parse(array[0])] = uint.Parse(array[1]);
            list.Add(wayNode2);
        }
        this.wayNodeArray = list.ToArray();
    }

    public void TryInitNodeListByStream(string srcText, bool isPrintResult = false)
    {
        List<WayNode> list = new List<WayNode>();
        string pattern = "[-][0-9]{1,}[:][0-9]{1,}";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        IEnumerator enumerator = regex.Matches(srcText).GetEnumerator();
        string text = string.Empty;
        if (enumerator.MoveNext())
        {
            Match match = (Match)enumerator.Current;
            text = match.Value;
        }
        string text2 = srcText;
        if (!string.IsNullOrEmpty(text))
        {
            text2 = srcText.Replace(text, string.Empty);
        }
        int i = 0;
        bool flag = true;
        int num = 0;
        bool flag2 = false;
        while (i < text2.Length)
        {
            flag = (i == 0 || text2[i].ToString().Equals("{") || flag);
            if (flag)
            {
                StringBuilder stringBuilder = new StringBuilder();
                WayNode wayNode = new WayNode();
                if (text2[i].ToString().Equals("{") || flag2)
                {
                    wayNode.wayNodeType = WayNodeType.Way;
                    wayNode.wayIdIndex = num;
                    num++;
                }
                else
                {
                    wayNode.wayNodeType = WayNodeType.Text;
                }
                bool flag3 = false;
                while (!flag3)
                {
                    stringBuilder.Append(text2[i]);
                    i++;
                    if (i >= text2.Length)
                    {
                        if (stringBuilder.Length > 0)
                        {
                            wayNode.PaseData(stringBuilder.ToString());
                            if (!string.IsNullOrEmpty(wayNode.text))
                            {
                                list.Add(wayNode);
                            }
                        }
                        break;
                    }
                    if (text2[i].ToString().Equals("{") || text2[i].ToString().Equals("}"))
                    {
                        wayNode.PaseData(stringBuilder.ToString());
                        list.Add(wayNode);
                        flag3 = true;
                        flag2 = text2[i].ToString().Equals("{");
                    }
                }
            }
            else
            {
                i++;
            }
        }
        if (!string.IsNullOrEmpty(text))
        {
            text = text.Replace("-", string.Empty);
            WayNode wayNode2 = new WayNode();
            wayNode2.wayNodeType = WayNodeType.EndPreFix;
            wayNode2.endPreFix = new Dictionary<uint, uint>();
            string[] array = text.Split(new char[]
            {
                ':'
            });
            wayNode2.endPreFix[uint.Parse(array[0])] = uint.Parse(array[1]);
            list.Add(wayNode2);
        }
        this.wayNodeArray = list.ToArray();
    }

    public void InitUI(GameObject itemRoot, uint questID, int finishState, string degreeVar, uint curDegree, uint maxDegree)
    {
        bool flag = finishState == 2;
        Color colorByName = Const.GetColorByName("QuestDefaultColor");
        Color colorByName2 = Const.GetColorByName("QuestFinishColor");
        Color colorByName3 = Const.GetColorByName("QuestDoingColor");
        Color colorByName4 = Const.GetColorByName("QuestCommitColor");
        if (!itemRoot.activeSelf)
        {
            itemRoot.gameObject.SetActive(true);
        }
        LuaTable questCfg = LuaConfigManager.GetConfigTable("questconfig", (ulong)questID);
        GameObject gameObject = itemRoot.transform.Find("txt_item").gameObject;
        GameObject gameObject2 = itemRoot.transform.Find("txt_placename").gameObject;
        List<GameObject> list = new List<GameObject>();
        List<GameObject> list2 = new List<GameObject>();
        for (int i = 0; i < itemRoot.transform.childCount; i++)
        {
            Transform child = itemRoot.transform.GetChild(i);
            if (child.name.Equals("txt_item"))
            {
                list.Add(child.gameObject);
            }
            if (child.name.Equals("txt_placename"))
            {
                child.GetComponent<Text>().raycastTarget = true;
                list2.Add(child.gameObject);
            }
            Text component = child.GetComponent<Text>();
            if (component != null)
            {
                component.color = colorByName;
            }
        }
        if (this.wayNodeArray != null && this.wayNodeArray.Length > 0)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            for (int j = 0; j < this.wayNodeArray.Length; j++)
            {
                int num4 = j;
                WayNode wn = this.wayNodeArray[num4];
                if (wn.wayNodeType == WayNodeType.Text)
                {
                    num2++;
                    GameObject gameObject3;
                    if (list.Count > 0)
                    {
                        gameObject3 = list[0];
                        list.RemoveAt(0);
                    }
                    else
                    {
                        gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                        gameObject3.name = gameObject.name;
                        gameObject3.transform.SetParent(itemRoot.transform, false);
                    }
                    gameObject3.gameObject.SetActive(true);
                    gameObject3.transform.SetSiblingIndex(num4);
                    Text component2 = gameObject3.GetComponent<Text>();
                    component2.text = wn.text;
                    if (curDegree >= maxDegree)
                    {
                        component2.color = colorByName2;
                    }
                    if (curDegree < maxDegree || (curDegree == maxDegree && maxDegree == 0U))
                    {
                        component2.color = colorByName3;
                    }
                    if (flag)
                    {
                        component2.color = colorByName4;
                    }
                }
                if (wn.wayNodeType == WayNodeType.Way)
                {
                    num3++;
                    GameObject gameObject4;
                    if (list2.Count > 0)
                    {
                        gameObject4 = list2[0];
                        list2.RemoveAt(0);
                    }
                    else
                    {
                        gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
                        gameObject4.name = gameObject2.name;
                        gameObject4.transform.SetParent(itemRoot.transform, false);
                    }
                    gameObject4.gameObject.SetActive(true);
                    Text component3 = gameObject4.GetComponent<Text>();
                    Image componentInChildren = gameObject4.GetComponentInChildren<Image>(true);
                    gameObject4.transform.SetSiblingIndex(num4);
                    component3.text = wn.text;
                    component3.color = wn.color;
                    if (curDegree >= maxDegree && maxDegree != 0U)
                    {
                        component3.color = colorByName2;
                        componentInChildren.gameObject.SetActive(false);
                        component3.text = GlobalRegister.StripColorText(wn.text);
                    }
                    if (curDegree < maxDegree || (curDegree == maxDegree && maxDegree == 0U))
                    {
                        component3.color = colorByName3;
                        componentInChildren.gameObject.SetActive(true);
                        if (component3.text.Contains(">"))
                        {
                            componentInChildren.color = wn.color;
                        }
                        else
                        {
                            componentInChildren.color = colorByName3;
                        }
                    }
                    Button component4 = component3.GetComponent<Button>();
                    component4.onClick.RemoveAllListeners();
                    component4.onClick.AddListener(delegate ()
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                        if (finishState == 2)
                        {
                            string cacheField_String2 = questCfg.GetCacheField_String("pathwaydone");
                            string[] array4 = cacheField_String2.Split(new char[]
                            {
                                '-'
                            });
                            if (wn.wayIdIndex < array4.Length)
                            {
                                uint pathwayid = (uint)float.Parse(array4[wn.wayIdIndex]);
                                GlobalRegister.PathFindWithPathWayId(pathwayid);
                            }
                        }
                        else if (finishState == 0)
                        {
                            string cacheField_String3 = questCfg.GetCacheField_String("pathwaypre");
                            string[] array5 = cacheField_String3.Split(new char[]
                            {
                                '-'
                            });
                            if (wn.wayIdIndex < array5.Length)
                            {
                                uint pathwayid2 = (uint)float.Parse(array5[wn.wayIdIndex]);
                                GlobalRegister.PathFindWithPathWayId(pathwayid2);
                            }
                        }
                        else if (this.pathWayIds != null && wn.wayIdIndex < this.pathWayIds.Length)
                        {
                            uint mcurrentCopyID = GlobalRegister.GetMCurrentCopyID();
                            bool flag2 = true;
                            if (mcurrentCopyID == 0U)
                            {
                                flag2 = true;
                            }
                            else
                            {
                                string cacheField_String4 = questCfg.GetCacheField_String("show_copy_id");
                                if (!string.IsNullOrEmpty(cacheField_String4))
                                {
                                    string[] array6 = cacheField_String4.Split(new char[]
                                    {
                                        '-'
                                    });
                                    if (array6 != null && array6.Length > 0)
                                    {
                                        List<uint> list3 = new List<uint>();
                                        for (int l = 0; l < array6.Length; l++)
                                        {
                                            uint item = 0U;
                                            if (uint.TryParse(array6[l], out item))
                                            {
                                                list3.Add(item);
                                            }
                                        }
                                        if (!list3.Contains(mcurrentCopyID))
                                        {
                                            flag2 = false;
                                        }
                                    }
                                }
                            }
                            if (flag2)
                            {
                                GlobalRegister.PathFindWithPathWayId(this.pathWayIds[wn.wayIdIndex]);
                            }
                            else
                            {
                                TipsWindow.ShowNotice(897U);
                            }
                        }
                    });
                    if (flag)
                    {
                        componentInChildren.gameObject.SetActive(true);
                        component3.color = colorByName4;
                        component3.text = wn.text;
                        componentInChildren.color = wn.color;
                    }
                    num++;
                }
            }
            this.DestroyNoUseObj(list, num2);
            this.DestroyNoUseObj(list2, num3);
        }
        GameObject gameObject5 = itemRoot.transform.Find("txt_count").gameObject;
        GameObject gameObject6 = itemRoot.transform.Find("btn_action").gameObject;
        Text component5 = gameObject5.GetComponent<Text>();
        component5.gameObject.SetActive(true);
        component5.text = curDegree + "/" + maxDegree;
        if (flag || maxDegree == 1U || finishState == 0)
        {
            component5.text = string.Empty;
        }
        uint cacheField_Uint = questCfg.GetCacheField_Uint("if_ring");
        if (cacheField_Uint == 1U)
        {
            object[] array = LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.GetCurRingCount", new object[]
            {
                Util.GetLuaTable("NpcTalkAndTaskDlgCtrl"),
                questID
            });
            uint cacheField_Uint2 = LuaConfigManager.GetXmlConfigTable("quest").GetCacheField_Table("ringquest").GetCacheField_Uint("max_quest_num");
            component5.text = array[0] + "/" + cacheField_Uint2;
        }
        if (finishState == 0)
        {
            Transform txtTr = itemRoot.transform.parent.parent.Find("title/txt_taskname");
            Transform transform = itemRoot.transform.parent.parent.Find("title/txt_accept");
            if (transform != null && transform.GetComponent<Text>() != null)
            {
                transform.GetComponent<Text>().color = this.TryGetRichColorByRitchText(txtTr, transform.GetComponent<Text>().color);
            }
            component5.text = string.Empty;
        }
        string cacheField_String = questCfg.GetCacheField_String("useobject");
        gameObject6.SetActive(false);
        if (!string.IsNullOrEmpty(cacheField_String) && this.wayNodeArray.Length > 0)
        {
            string[] array2 = cacheField_String.Split(new char[]
            {
                ';'
            });
            string text = string.Empty;
            WayNode wayNode = this.wayNodeArray[this.wayNodeArray.Length - 1];
            for (int k = 0; k < array2.Length; k++)
            {
                string[] array3 = array2[k].Split(new char[]
                {
                    '-'
                });
                if (wayNode.wayNodeType == WayNodeType.EndPreFix && array3.Length >= 2 && wayNode.endPreFix.ContainsKey(uint.Parse(array3[0])))
                {
                    text = array3[1];
                    break;
                }
            }
            if (!string.IsNullOrEmpty(text) && !flag)
            {
                uint objRealID = 0U;
                if (uint.TryParse(text, out objRealID))
                {
                    gameObject6.gameObject.SetActive(true);
                    Button component6 = gameObject6.GetComponent<Button>();
                    component6.onClick.RemoveAllListeners();
                    this.SetItemIcon(gameObject6, objRealID);
                    component6.onClick.AddListener(delegate ()
                    {
                        if (objRealID == 8007U)
                        {
                            MainPlayerTargetSelectMgr component8 = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
                            if (component8 != null && component8.TargetCharactor == null)
                            {
                                TipsWindow.ShowNotice(930U);
                                return;
                            }
                        }
                        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
                        {
                            Util.GetLuaTable("BagCtrl"),
                            objRealID
                        })[0];
                        if (propsBase != null)
                        {
                            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
                            {
                                Util.GetLuaTable("BagCtrl"),
                                propsBase,
                                1
                            });
                        }
                    });
                }
            }
        }
        IrregularGridLayOut component7 = itemRoot.GetComponent<IrregularGridLayOut>();
        component7.FitChildItem();
    }

    private void DestroyNoUseObj(List<GameObject> objs, int useCount)
    {
        if (objs.Count > 0)
        {
            int num = (useCount != 0) ? 0 : 1;
            for (int i = objs.Count - 1; i >= num; i--)
            {
                if (objs[i])
                {
                    UnityEngine.Object.DestroyImmediate(objs[i]);
                }
            }
        }
    }

    private Color TryGetRichColorByRitchText(Transform txtTr, Color defaut)
    {
        Color result = defaut;
        if (txtTr != null)
        {
            Text component = txtTr.GetComponent<Text>();
            if (component != null && component.text.Contains("=#"))
            {
                try
                {
                    string text = component.text.Substring(component.text.IndexOf("#") + 1, 6);
                    if (text.Length >= 6)
                    {
                        result = new Color((float)Convert.ToInt32(text[0].ToString() + text[1], 16) / 255f, (float)Convert.ToInt32(text[2].ToString() + text[3], 16) / 255f, (float)Convert.ToInt32(text[4].ToString() + text[5], 16) / 255f);
                    }
                }
                catch
                {
                    FFDebug.LogError(this, "parse color error task name to accept");
                }
            }
        }
        return result;
    }

    private void SetItemIcon(GameObject obj, uint itemId)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)itemId);
        Image imgicon = obj.GetComponent<Image>();
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (imgicon == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            imgicon.sprite = sprite;
            imgicon.overrideSprite = sprite;
            imgicon.color = Color.white;
            imgicon.gameObject.SetActive(true);
            imgicon.material = null;
        });
    }

    public uint[] pathWayIds;

    public WayNode[] wayNodeArray;

    private static string endStrPattern = "[-][0-9]{1,}[:][0-9]{1,}";

    private static string patternWayFind = "{.*?}";

    private static string patternPlaceNameColor = "[[][0-9|a-f]{6}[]]";
}
