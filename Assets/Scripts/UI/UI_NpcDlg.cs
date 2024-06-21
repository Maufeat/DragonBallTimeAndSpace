using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_NpcDlg
{
    public UI_NpcDlg()
    {
        TaskController.CurrnetNpcDlg = this;
    }

    public void UnInit()
    {
        this.isend = false;
        this.TheOnlyFunctionInfo = string.Empty;
        this.currentdramagroupcallbackinfo = string.Empty;
        this.normalitemnum = 0;
        LuaScriptMgr.Instance.CallLuaFunction("this.Dispose", new object[0]);
        this.DestroyDlg();
        this.CleanDramaPics();
        TaskController.CurNpcDlgSource = 0U;
        TaskController.CurrnetNpcDlg = null;
        if (MainPlayer.Self != null)
        {
            BreakAutoattackUIMgr component = MainPlayer.Self.GetComponent<BreakAutoattackUIMgr>();
            if (component != null)
            {
                component.ProcessDeletedUI("UI_NPCtalk");
            }
        }
        for (int i = 0; i < this.imageSeperateWithAlphaList.Count; i++)
        {
            this.imageSeperateWithAlphaList[i].Dispose();
        }
        this.imageSeperateWithAlphaList.Clear();
        for (int j = 0; j < this.usedTextureAssets.Count; j++)
        {
            this.usedTextureAssets[j].TryUnload();
        }
        this.usedTextureAssets.Clear();
        if (this.npcDlg != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DramaActiviteUI(this.npcDlg);
        }
    }

    private void LoadObj(Action callback)
    {
        if (this.npcDlg == null)
        {
            ControllerManager.Instance.GetController<UINpcDlgController>().ShowNpcTalk(delegate (Transform transform)
            {
                this.npcDlg = transform.gameObject;
                this.normalDlg = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCtalk").gameObject;
                this.dramaDlg = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog").gameObject;
                this.npcDlg.transform.SetAsLastSibling();
                this.npcDlg.transform.localPosition = Vector3.zero;
                this.npcDlg.transform.localScale = Vector3.one;
                this.lbPrefab = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCtalk/ScrollbarRect/Rect/txt_content").gameObject;
                this.btnPrefab = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCtalk/ScrollbarRect/Rect/btn_choose").gameObject;
                this.scrollRect = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCtalk/ScrollbarRect").GetComponent<ScrollRect>();
                this.btnNormaltalkClose = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCtalk/background/btn_close").GetComponent<Button>();
                this.txtNormaltalkName = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCtalk/txt_npcname").GetComponent<Text>();
                this.dramaBtnPrefab = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/btns/Rect/btn_choose").gameObject;
                this.dramaScrollRect = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/btns").GetComponent<ScrollRect>();
                this.dramaText = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/img_talkbg/Text").GetComponent<Text>();
                this.btnDramaNext = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/img_talkbg/img_next").GetComponent<Button>();
                this.btnDramaSkip = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/btn_next").GetComponent<Button>();
                this.picL = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/img_left").GetComponent<RawImage>();
                this.picM = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/img_middle").GetComponent<RawImage>();
                this.picR = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/img_right").GetComponent<RawImage>();
                this.dramaName = this.npcDlg.transform.Find("Offset_NPCtalk/Panel_NPCDialog/img_talkbg/img_bg").gameObject;
                this.lbPrefab.SetActive(false);
                this.btnPrefab.SetActive(false);
                this.npcDlg.SetActive(true);
                callback();
            });
        }
        else
        {
            this.npcDlg.SetActive(true);
            callback();
        }
    }

    private void CreatDlg()
    {
        this.LoadObj(delegate
        {
            if (this.normalitemnum > 0 || this.normaltalknum > 0)
            {
                this.normalDlg.SetActive(true);
                this.SetNormalTalkName();
            }
            else
            {
                this.normalDlg.SetActive(false);
            }
            if (this.dramatalknum > 0 || this.dramaitemnum > 0)
            {
                this.dramaDlg.SetActive(true);
                if (this.dramatalknum > 0)
                {
                    this.dramaDlg.transform.Find("img_talkbg").gameObject.SetActive(true);
                    ManagerCenter.Instance.GetManager<UIManager>().DramaHideUI(this.npcDlg);
                }
                if (this.dramaitemnum > 0)
                {
                    this.dramaDlg.transform.Find("btns").gameObject.SetActive(true);
                }
            }
            else
            {
                this.dramaDlg.SetActive(false);
            }
            while (this.actionQueue.Count > 0)
            {
                this.actionQueue.Dequeue()();
            }
            this.btnNormaltalkClose.onClick.RemoveAllListeners();
            this.btnNormaltalkClose.onClick.AddListener(delegate ()
            {
                this.ResetNPCDir();
                this.UnInit();
            });
        });
    }

    private void ClearDlg()
    {
        if (this.npcDlg == null)
        {
            return;
        }
        for (int i = 0; i < this.dlgItems.Count; i++)
        {
            GameObject obj = this.dlgItems[i];
            this.dlgItems[i] = null;
            UnityEngine.Object.Destroy(obj);
        }
        this.dlgItems.Clear();
        for (int j = 0; j < this.dramaDlgItems.Count; j++)
        {
            GameObject obj2 = this.dramaDlgItems[j];
            this.dramaDlgItems[j] = null;
            UnityEngine.Object.Destroy(obj2);
        }
        this.dramaDlgItems.Clear();
        this.dramaText.text = string.Empty;
    }

    private void DestroyDlg()
    {
        this.normalitemnum = 0;
        this.normaltalknum = 0;
        this.dramatalknum = 0;
        this.dramaitemnum = 0;
        this.actionQueue.Clear();
        if (this.npcDlg == null)
        {
            return;
        }
        this.ClearDlg();
        this.npcDlg.SetActive(false);
        this.ResetDramaGroupData();
    }

    public void CallCsFunctionImmediate(string funinfo)
    {
        this.callCsFunctionImmediate = true;
        LuaProcess.ProcessLua2CsFunction(funinfo);
        this.UnInit();
    }

    public void EndDlg()
    {
        this.isend = true;
        FFDebug.Log(this, FFLogType.Lua, "EndDlg  " + this.normalitemnum);
        this.CreatDlg();
        if (MainPlayer.Self != null)
        {
            BreakAutoattackUIMgr component = MainPlayer.Self.GetComponent<BreakAutoattackUIMgr>();
            if (component != null)
            {
                component.ProcessOpenedUI("UI_NPCtalk", true);
            }
        }
        if (this.normalitemnum == 1 && !string.IsNullOrEmpty(this.TheOnlyFunctionInfo))
        {
            if (this.TheOnlyFunctionInfo.StartsWith("this"))
            {
                this.DestroyDlg();
                LuaScriptMgr.Instance.CallLuaFunction(this.TheOnlyFunctionInfo, new object[0]);
            }
            else
            {
                LuaProcess.ProcessLua2CsFunction(this.TheOnlyFunctionInfo);
                this.UnInit();
            }
        }
        if (this.callCsFunctionImmediate)
        {
            this.UnInit();
        }
    }

    public void AddTalk(string content)
    {
        this.normaltalknum++;
        this.actionQueue.Enqueue(delegate
        {
            this.CreatLabel(content);
        });
        if (this.isend)
        {
            this.CreatDlg();
        }
    }

    public void AddTalkByID(uint id)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("dialogueconfig", (ulong)id);
        if (configTable != null)
        {
            this.AddTalk(configTable.GetField_String("dialogue"));
        }
        else
        {
            this.AddTalk(id.ToString());
        }
    }

    public void AddDialogItem(string texture, string content, string callbackinfo)
    {
        this.normalitemnum++;
        if (this.normalitemnum == 1)
        {
            this.TheOnlyFunctionInfo = callbackinfo;
        }
        if (this.normalitemnum > 1)
        {
            this.TheOnlyFunctionInfo = string.Empty;
        }
        this.actionQueue.Enqueue(delegate
        {
            GameObject gameObject = this.CreatButton(texture, content);
            gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                if (string.IsNullOrEmpty(callbackinfo))
                {
                    this.UnInit();
                    return;
                }
                if (callbackinfo.StartsWith("this"))
                {
                    this.DestroyDlg();
                    LuaScriptMgr.Instance.CallLuaFunction(callbackinfo, new object[0]);
                }
                else
                {
                    LuaProcess.ProcessLua2CsFunction(callbackinfo);
                    this.UnInit();
                }
            });
        });
        if (this.isend)
        {
            this.CreatDlg();
        }
    }

    public void AddDialogItemByID(string texture, uint id, string callbackinfo)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("dialogueconfig", (ulong)id);
        if (configTable != null)
        {
            this.AddDialogItem(texture, configTable.GetField_String("dialogue"), callbackinfo);
        }
        else
        {
            this.AddDialogItem(texture, id.ToString(), callbackinfo);
        }
    }

    private GameObject CreatLabel(string content)
    {
        if (this.npcDlg == null)
        {
            return null;
        }
        if (this.lbPrefab == null)
        {
            FFDebug.LogWarning(this, "Label prefab is null!!");
            return null;
        }
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.lbPrefab);
        gameObject.transform.SetParent(this.scrollRect.transform.Find("Rect"));
        gameObject.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        Text component = gameObject.transform.Find("Text").GetComponent<Text>();
        component.text = content;
        this.dlgItems.Add(gameObject);
        this.NormalDlgSetLayout();
        return gameObject;
    }

    private GameObject CreatButton(string texture, string content)
    {
        if (this.npcDlg == null)
        {
            FFDebug.LogWarning(this, "NpcDlg == null" + "   " + this.GetHashCode());
            return null;
        }
        if (this.btnPrefab == null)
        {
            FFDebug.LogWarning(this, "Label prefab is null!!");
            return null;
        }
        GameObject btn = UnityEngine.Object.Instantiate<GameObject>(this.btnPrefab);
        btn.transform.SetParent(this.scrollRect.transform.Find("Rect"));
        btn.transform.localScale = Vector3.one;
        Text component = btn.transform.Find("content/Text").GetComponent<Text>();
        component.text = content;
        if (texture != string.Empty)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ICON, texture, delegate (UITextureAsset item)
            {
                this.usedTextureAssets.Add(item);
                if (btn == null)
                {
                    return;
                }
                RawImage component2 = btn.transform.Find("img_icon").GetComponent<RawImage>();
                if (item == null)
                {
                    component2.gameObject.SetActive(false);
                    return;
                }
                if (item.textureObj == null)
                {
                    component2.gameObject.SetActive(false);
                    return;
                }
                Sprite sprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                component2.texture = sprite.texture;
                component2.color = Color.white;
                component2.SetNativeSize();
            });
        }
        else
        {
            btn.transform.Find("img_icon").gameObject.SetActive(false);
        }
        btn.SetActive(true);
        this.dlgItems.Add(btn);
        this.NormalDlgSetLayout();
        return btn;
    }

    private void NormalDlgSetLayout()
    {
        if (this.npcDlg == null)
        {
            FFDebug.LogWarning(this, "NpcDlg == null");
            return;
        }
        if (this.scrollRect == null)
        {
            FFDebug.LogWarning(this, "scrollRect == null");
            return;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.scrollRect.content);
        if (this.scrollRect.content.GetComponent<VerticalLayoutGroup>().preferredHeight < this.scrollRect.GetComponent<RectTransform>().sizeDelta.y)
        {
            this.scrollRect.enabled = false;
            this.scrollRect.verticalScrollbar.gameObject.SetActive(false);
        }
        else
        {
            this.scrollRect.enabled = true;
            this.scrollRect.verticalNormalizedPosition = 1f;
            this.scrollRect.verticalScrollbar.gameObject.SetActive(true);
        }
    }

    private void SetNormalTalkName()
    {
        if (this.txtNormaltalkName == null)
        {
            return;
        }
        if (MainPlayer.Self == null)
        {
            this.txtNormaltalkName.gameObject.SetActive(false);
            return;
        }
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component == null)
        {
            this.txtNormaltalkName.gameObject.SetActive(false);
            return;
        }
        ulong currentVisteNpcID = component.CurrentVisteNpcID;
        if (currentVisteNpcID == 0UL)
        {
            this.txtNormaltalkName.gameObject.SetActive(false);
            return;
        }
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(currentVisteNpcID, CharactorType.NPC) as Npc;
        if (npc != null)
        {
            this.txtNormaltalkName.gameObject.SetActive(true);
            this.txtNormaltalkName.GetComponent<Text>().text = npc.NpcData.MapNpcData.name;
        }
        else
        {
            this.txtNormaltalkName.gameObject.SetActive(false);
        }
    }

    public void AddDramaTalk(string content)
    {
        this.dramatalknum++;
        this.actionQueue.Enqueue(delegate
        {
            this.CreatDramaLabel(content, null);
            this.btnDramaNext.onClick.RemoveAllListeners();
            this.btnDramaNext.onClick.AddListener(delegate ()
            {
                this.ResetNPCDir();
                this.UnInit();
            });
            this.btnDramaSkip.gameObject.SetActive(false);
            this.SetDramaTalkName(null);
        });
        if (this.isend)
        {
            this.CreatDlg();
        }
    }

    public void AddDramaTalkByID(uint id)
    {
        LuaTable config = LuaConfigManager.GetConfigTable("dialogueconfig", (ulong)id);
        if (config != null)
        {
            this.dramatalknum++;
            this.actionQueue.Enqueue(delegate
            {
                this.CreatDramaLabel(config.GetField_String("dialogue"), null);
                this.btnDramaNext.onClick.RemoveAllListeners();
                this.btnDramaNext.onClick.AddListener(delegate ()
                {
                    this.ResetNPCDir();
                    this.UnInit();
                });
                SingletonForMono<DramaAction>.Instance.PlayAction(this.dramaDlg.transform, (DramaAction.ActionType)config.GetField_Uint("Action"));
                this.btnDramaSkip.gameObject.SetActive(false);
                this.picM.gameObject.SetActive(false);
                this.picL.gameObject.SetActive(false);
                this.picR.gameObject.SetActive(false);
                string field_String = config.GetField_String("pic");
                if (!string.IsNullOrEmpty(field_String))
                {
                    ImageSeperateWithAlpha imageSeperateWithAlpha = new ImageSeperateWithAlpha();
                    imageSeperateWithAlpha.ProcessRawImageSeperateRGBA(this.picL, field_String, delegate
                    {
                        if (this.picL != null)
                        {
                            this.picL.gameObject.SetActive(true);
                        }
                    });
                    this.imageSeperateWithAlphaList.Add(imageSeperateWithAlpha);
                }
                this.SetDramaTalkName(config.GetField_String("name"));
            });
            if (this.isend)
            {
                this.CreatDlg();
            }
        }
        else
        {
            this.AddDramaTalk(id.ToString());
        }
    }

    public void AddDramaItem(string texture, string content, string callbackinfo)
    {
        this.dramaitemnum++;
        if (this.dramaitemnum == 1)
        {
            this.TheOnlyFunctionInfo = callbackinfo;
        }
        if (this.normalitemnum > 1)
        {
            this.TheOnlyFunctionInfo = string.Empty;
        }
        this.actionQueue.Enqueue(delegate
        {
            GameObject gameObject = this.CreatDramaButton(texture, content);
            gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                if (string.IsNullOrEmpty(callbackinfo))
                {
                    this.UnInit();
                    return;
                }
                if (callbackinfo.StartsWith("this"))
                {
                    this.DestroyDlg();
                    LuaScriptMgr.Instance.CallLuaFunction(callbackinfo, new object[0]);
                }
                else
                {
                    LuaProcess.ProcessLua2CsFunction(callbackinfo);
                    this.UnInit();
                }
            });
        });
        if (this.isend)
        {
            this.CreatDlg();
        }
    }

    public void AddDramaItemByID(string texture, uint id, string callbackinfo)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("dialogueconfig", (ulong)id);
        if (configTable != null)
        {
            this.AddDramaItem(texture, configTable.GetField_String("dialogue"), callbackinfo);
        }
        else
        {
            this.AddDramaItem(texture, id.ToString(), callbackinfo);
        }
    }

    public void AddDramaGroupByID(uint id, string callbackinfo)
    {
        if (ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups.ContainsKey(id))
        {
            this.currentdramagroup = id;
            this.currentdramagroupindex = -1;
            this.currentdramagroupcallbackinfo = callbackinfo;
            this.GroupCheckNext();
        }
        else
        {
            FFDebug.LogWarning(this, "Does not contain drama group with id : " + id);
        }
    }

    private void AddDramaGroupTalkByID(LuaTable config)
    {
        if (config != null)
        {
            this.dramatalknum++;
            this.actionQueue.Enqueue(delegate
            {
                this.CreatDramaLabel(config.GetField_String("dialogue"), delegate
                {
                    if (config.GetField_Uint("Auto") == 1U)
                    {
                        Scheduler.Instance.AddTimer(1.5f, false, new Scheduler.OnScheduler(this.GroupCheckNext));
                    }
                });
                this.btnDramaNext.onClick.RemoveAllListeners();
                this.btnDramaNext.onClick.AddListener(delegate ()
                {
                    Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.GroupCheckNext));
                    this.GroupCheckNext();
                });
                this.picM.gameObject.SetActive(false);
                this.picL.gameObject.SetActive(false);
                this.picR.gameObject.SetActive(false);
                this.SetDramaPicture(config.GetField_String("pic"), config.GetField_Uint("Action"));
                this.SetDramaTalkName(config.GetField_String("name"));
                this.btnDramaSkip.onClick.RemoveAllListeners();
                this.btnDramaSkip.onClick.AddListener(delegate ()
                {
                    Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.GroupCheckNext));
                    this.OnDramaGroupEnd();
                });
                this.btnDramaSkip.gameObject.SetActive(true);
            });
            if (this.isend)
            {
                this.CreatDlg();
            }
        }
        else
        {
            this.AddDramaTalk("Config Error!");
        }
    }

    private void GroupCheckNext()
    {
        if (this.dramaText != null)
        {
            TypewriterEffect component = this.dramaText.GetComponent<TypewriterEffect>();
            if (component != null && component.IsActive)
            {
                component.BreakTypeWrite();
            }
        }
        if (this.currentdramagroup == 0U)
        {
            FFDebug.LogWarning(this, "CheckNext currentdramagroup == 0");
            return;
        }
        if (!ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups.ContainsKey(this.currentdramagroup))
        {
            FFDebug.LogWarning(this, "!DramaGroups.ContainsKey(currentdramagroup.ToString())");
            return;
        }
        LuaTable luaTable;
        if (this.currentdramagroupindex >= 0)
        {
            luaTable = ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups[this.currentdramagroup][this.currentdramagroupindex];
            if (luaTable == null)
            {
                FFDebug.LogWarning(this, "config == null");
                return;
            }
            if (luaTable.GetField_Uint("Clean") == 1U)
            {
                this.CleanDramaPics();
            }
        }
        this.currentdramagroupindex++;
        if (this.currentdramagroupindex == ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups[this.currentdramagroup].Count)
        {
            this.OnDramaGroupEnd();
            return;
        }
        luaTable = ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups[this.currentdramagroup][this.currentdramagroupindex];
        if (luaTable == null)
        {
            FFDebug.LogWarning(this, "config == null");
            return;
        }
        this.AddDramaGroupTalkByID(luaTable);
    }

    private void OnDramaGroupEnd()
    {
        float cacheField_Float = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("MaskViewTime").GetCacheField_Float("value");
        ControllerManager.Instance.GetController<ImgMaskController>().ViewMask(cacheField_Float);
        if (string.IsNullOrEmpty(this.currentdramagroupcallbackinfo))
        {
            this.UnInit();
            return;
        }
        if (this.currentdramagroupcallbackinfo.StartsWith("this"))
        {
            this.DestroyDlg();
            LuaScriptMgr.Instance.CallLuaFunction(this.currentdramagroupcallbackinfo, new object[0]);
        }
        else
        {
            LuaProcess.ProcessLua2CsFunction(this.currentdramagroupcallbackinfo);
            this.UnInit();
        }
    }

    private void ResetDramaGroupData()
    {
        this.currentdramagroup = 0U;
        this.currentdramagroupindex = -1;
    }

    private void CreatDramaLabel(string content, Action callback = null)
    {
        if (this.npcDlg == null)
        {
            return;
        }
        if (this.lbPrefab == null)
        {
            FFDebug.LogWarning(this, "Label prefab is null!!");
            return;
        }
        this.dramaText.GetComponent<TypewriterEffect>().StartTypeWrite(content, callback);
    }

    private GameObject CreatDramaButton(string texture, string content)
    {
        if (this.npcDlg == null)
        {
            FFDebug.LogWarning(this, "NpcDlg == null" + "   " + this.GetHashCode());
            return null;
        }
        if (this.dramaBtnPrefab == null)
        {
            FFDebug.LogWarning(this, "Label prefab is null!!");
            return null;
        }
        GameObject btn = UnityEngine.Object.Instantiate<GameObject>(this.dramaBtnPrefab);
        btn.transform.SetParent(this.dramaScrollRect.transform.Find("Rect"));
        btn.transform.localScale = Vector3.one;
        Text component = btn.transform.Find("Text").GetComponent<Text>();
        component.text = content;
        if (texture != string.Empty)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ICON, texture, delegate (UITextureAsset item)
            {
                if (btn == null)
                {
                    return;
                }
                RawImage component2 = btn.transform.Find("img_icon").GetComponent<RawImage>();
                if (item == null)
                {
                    component2.gameObject.SetActive(false);
                    return;
                }
                this.usedTextureAssets.Add(item);
                if (item.textureObj == null)
                {
                    component2.gameObject.SetActive(false);
                    return;
                }
                Sprite sprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                component2.texture = sprite.texture;
                component2.color = Color.white;
                component2.SetNativeSize();
            });
        }
        else
        {
            btn.transform.Find("img_icon").gameObject.SetActive(false);
        }
        btn.SetActive(true);
        this.dramaDlgItems.Add(btn);
        this.DramaTalkSetLayout();
        return btn;
    }

    private void DramaTalkSetLayout()
    {
        if (this.npcDlg == null)
        {
            FFDebug.LogWarning(this, "NpcDlg == null");
            return;
        }
        if (this.dramaScrollRect == null)
        {
            FFDebug.LogWarning(this, "dramaScrollRect == null");
            return;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.dramaScrollRect.content);
        this.scrollRect.verticalNormalizedPosition = 1f;
    }

    private void CleanDramaPics()
    {
        if (this.npcDlg != null)
        {
            this.picM.gameObject.SetActive(false);
            this.picL.gameObject.SetActive(false);
            this.picR.gameObject.SetActive(false);
        }
        this.dramaPics.Clear();
    }

    private void SetDramaPicture(string picname, uint configAction)
    {
        this.SetPicDarkOrBright(this.picL, UI_NpcDlg.DramaPicState.Dark);
        this.SetPicDarkOrBright(this.picM, UI_NpcDlg.DramaPicState.Dark);
        this.SetPicDarkOrBright(this.picR, UI_NpcDlg.DramaPicState.Dark);
        this.picM.gameObject.SetActive(false);
        this.picL.gameObject.SetActive(false);
        this.picR.gameObject.SetActive(false);
        if (this.dramaPics.Count == 0)
        {
            if (string.IsNullOrEmpty(picname))
            {
                return;
            }
            this.dramaPics.Add(picname);
            ImageSeperateWithAlpha imageSeperateWithAlpha = new ImageSeperateWithAlpha();
            imageSeperateWithAlpha.ProcessRawImageSeperateRGBA(this.picM, picname, delegate
            {
                if (this.picM != null)
                {
                    this.SetPicDarkOrBright(this.picM, UI_NpcDlg.DramaPicState.Bright);
                    this.picM.gameObject.SetActive(true);
                }
            });
            this.imageSeperateWithAlphaList.Add(imageSeperateWithAlpha);
        }
        else if (this.dramaPics.Count == 1)
        {
            if (string.IsNullOrEmpty(picname))
            {
                this.picM.gameObject.SetActive(true);
                this.SetPicDarkOrBright(this.picM, UI_NpcDlg.DramaPicState.Dark);
                SingletonForMono<DramaAction>.Instance.PlayAction(this.picM.transform, (DramaAction.ActionType)configAction);
                return;
            }
            if (this.dramaPics.Contains(picname))
            {
                this.picM.gameObject.SetActive(true);
                this.SetPicDarkOrBright(this.picM, UI_NpcDlg.DramaPicState.Bright);
                SingletonForMono<DramaAction>.Instance.PlayAction(this.picM.transform, (DramaAction.ActionType)configAction);
            }
            else
            {
                this.dramaPics.Add(picname);
                ImageSeperateWithAlpha imageSeperateWithAlpha2 = new ImageSeperateWithAlpha();
                imageSeperateWithAlpha2.ProcessRawImageSeperateRGBA(this.picL, this.dramaPics[0], null);
                this.imageSeperateWithAlphaList.Add(imageSeperateWithAlpha2);
                ImageSeperateWithAlpha imageSeperateWithAlpha3 = new ImageSeperateWithAlpha();
                imageSeperateWithAlpha3.ProcessRawImageSeperateRGBA(this.picR, picname, delegate
                {
                    if (this.picL != null)
                    {
                        this.picL.gameObject.SetActive(true);
                    }
                    if (this.picR != null)
                    {
                        this.picR.gameObject.SetActive(true);
                        this.SetPicDarkOrBright(this.picR, UI_NpcDlg.DramaPicState.Bright);
                        SingletonForMono<DramaAction>.Instance.PlayAction(this.picR.transform, (DramaAction.ActionType)configAction);
                    }
                });
                this.imageSeperateWithAlphaList.Add(imageSeperateWithAlpha3);
            }
        }
        else if (this.dramaPics.Count == 2)
        {
            if (string.IsNullOrEmpty(picname))
            {
                this.picL.gameObject.SetActive(true);
                this.picR.gameObject.SetActive(true);
                if (this.dramaPics[0] == picname)
                {
                    this.SetPicDarkOrBright(this.picL, UI_NpcDlg.DramaPicState.Dark);
                    SingletonForMono<DramaAction>.Instance.PlayAction(this.picL.transform, (DramaAction.ActionType)configAction);
                }
                else
                {
                    this.SetPicDarkOrBright(this.picR, UI_NpcDlg.DramaPicState.Dark);
                    SingletonForMono<DramaAction>.Instance.PlayAction(this.picR.transform, (DramaAction.ActionType)configAction);
                }
                return;
            }
            if (this.dramaPics.Contains(picname))
            {
                this.picL.gameObject.SetActive(true);
                this.picR.gameObject.SetActive(true);
                if (this.dramaPics[0] == picname)
                {
                    this.SetPicDarkOrBright(this.picL, UI_NpcDlg.DramaPicState.Bright);
                    SingletonForMono<DramaAction>.Instance.PlayAction(this.picL.transform, (DramaAction.ActionType)configAction);
                }
                else
                {
                    this.SetPicDarkOrBright(this.picR, UI_NpcDlg.DramaPicState.Bright);
                    SingletonForMono<DramaAction>.Instance.PlayAction(this.picR.transform, (DramaAction.ActionType)configAction);
                }
            }
            else
            {
                this.dramaPics.Add(picname);
                this.dramaPics.RemoveAt(0);
                ImageSeperateWithAlpha imageSeperateWithAlpha4 = new ImageSeperateWithAlpha();
                imageSeperateWithAlpha4.ProcessRawImageSeperateRGBA(this.picL, this.dramaPics[0], null);
                this.imageSeperateWithAlphaList.Add(imageSeperateWithAlpha4);
                ImageSeperateWithAlpha imageSeperateWithAlpha5 = new ImageSeperateWithAlpha();
                imageSeperateWithAlpha5.ProcessRawImageSeperateRGBA(this.picR, picname, delegate
                {
                    if (this.picL != null)
                    {
                        this.picL.gameObject.SetActive(true);
                        this.SetPicDarkOrBright(this.picL, UI_NpcDlg.DramaPicState.Dark);
                    }
                    if (this.picR != null)
                    {
                        this.picR.gameObject.SetActive(true);
                        this.SetPicDarkOrBright(this.picR, UI_NpcDlg.DramaPicState.Bright);
                        SingletonForMono<DramaAction>.Instance.PlayAction(this.picR.transform, (DramaAction.ActionType)configAction);
                    }
                });
                this.imageSeperateWithAlphaList.Add(imageSeperateWithAlpha5);
            }
        }
    }

    private void SetPicDarkOrBright(RawImage img, UI_NpcDlg.DramaPicState state)
    {
        if (state == UI_NpcDlg.DramaPicState.Dark)
        {
            img.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            img.color = new Color(1f, 1f, 1f);
        }
    }

    private void SetDramaTalkName(string configname = null)
    {
        if (MainPlayer.Self == null)
        {
            this.dramaName.SetActive(false);
            return;
        }
        if (!string.IsNullOrEmpty(configname))
        {
            if (configname == "Player" || configname == "player")
            {
                this.dramaName.transform.Find("txt_name").GetComponent<Text>().text = MainPlayer.Self.OtherPlayerData.MapUserData.name;
            }
            else
            {
                this.dramaName.transform.Find("txt_name").GetComponent<Text>().text = configname;
            }
            return;
        }
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        ulong currentVisteNpcID = component.CurrentVisteNpcID;
        if (currentVisteNpcID == 0UL)
        {
            this.dramaName.gameObject.SetActive(false);
            return;
        }
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(currentVisteNpcID, CharactorType.NPC) as Npc;
        if (npc != null)
        {
            this.txtNormaltalkName.gameObject.SetActive(true);
            this.dramaName.transform.Find("txt_name").GetComponent<Text>().text = npc.NpcData.MapNpcData.name;
        }
        else
        {
            this.txtNormaltalkName.gameObject.SetActive(false);
        }
    }

    public void ResetNPCDir()
    {
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component == null)
        {
            return;
        }
        component.TryResetNPCDir();
    }

    private GameObject npcDlg;

    private GameObject normalDlg;

    private GameObject dramaDlg;

    private Queue<Action> actionQueue = new Queue<Action>();

    private List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();

    private GameObject lbPrefab;

    private GameObject btnPrefab;

    private Button btnNormaltalkClose;

    private ScrollRect scrollRect;

    private Text txtNormaltalkName;

    private List<GameObject> dlgItems = new List<GameObject>();

    private string TheOnlyFunctionInfo = string.Empty;

    private int normaltalknum;

    private int normalitemnum;

    private GameObject dramaBtnPrefab;

    private GameObject dramaName;

    private Text dramaText;

    private Button btnDramaNext;

    private Button btnDramaSkip;

    private ScrollRect dramaScrollRect;

    private RawImage picL;

    private RawImage picM;

    private RawImage picR;

    private List<GameObject> dramaDlgItems = new List<GameObject>();

    private int dramatalknum;

    private int dramaitemnum;

    private List<string> dramaPics = new List<string>();

    private List<ImageSeperateWithAlpha> imageSeperateWithAlphaList = new List<ImageSeperateWithAlpha>();

    private bool callCsFunctionImmediate;

    private bool isend;

    private uint currentdramagroup;

    private int currentdramagroupindex;

    private string currentdramagroupcallbackinfo = string.Empty;

    private enum DramaPicState
    {
        Dark,
        Bright
    }
}
