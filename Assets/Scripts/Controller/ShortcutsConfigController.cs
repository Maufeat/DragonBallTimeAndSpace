using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using apprentice;
using Framework.Managers;
using LuaInterface;
using Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShortcutsConfigController : ControllerBase
{
    private Dictionary<string, DefaulSetKeyData> dskdDic
    {
        get
        {
            if (this.dskdDic_ == null)
            {
                this.dskdDic_ = this.InitDefaulSetKeyData();
            }
            return this.dskdDic_;
        }
    }

    private List<KeyCode> invalidKeyCode
    {
        get
        {
            if (this.invalidKeyCode_ == null)
            {
                this.invalidKeyCode_ = this.InitIvalidKeyCode();
            }
            return this.invalidKeyCode_;
        }
    }

    public override void Awake()
    {
        ShortcutsConfigController.sfdl = new List<ShortcutsFunctionData>();
    }

    internal void BindShortcutsKeyDefaultKeyCode(bool isNeedLoadFromServer)
    {
        ShortcutsConfigController.sfdl.Clear();
        ServerStorageManager.Instance.RegReqCallBack(ServerStorageKey.ShortKey_Config, new Action<MSG_Req_OperateClientDatas_CS>(this.OnGetConfigData));
        List<string> list = new List<string>(this.dskdDic.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            DefaulSetKeyData defaulSetKeyData = this.dskdDic[list[i]];
            ShortcutkeyFunctionType functionType = (ShortcutkeyFunctionType)((int)Enum.Parse(typeof(ShortcutkeyFunctionType), defaulSetKeyData.funname));
            this.BindKeysToFunctionType(functionType, defaulSetKeyData.keycode);
        }
        this.BindKeysToFunctionType(ShortcutkeyFunctionType.HideGM, new List<KeyCode>
        {
            KeyCode.LeftControl,
            KeyCode.F7
        });
        this.BindKeysToFunctionType(ShortcutkeyFunctionType.HideFps, new List<KeyCode>
        {
            KeyCode.LeftControl,
            KeyCode.F8
        });
        this.BindKeysToFunctionType(ShortcutkeyFunctionType.HideUI, new List<KeyCode>
        {
            KeyCode.LeftControl,
            KeyCode.F9
        });
        this.BindKeysToFunctionType(ShortcutkeyFunctionType.HideRole, new List<KeyCode>
        {
            KeyCode.LeftControl,
            KeyCode.F10
        });
        if (isNeedLoadFromServer)
        {
            ServerStorageManager.Instance.GetData(ServerStorageKey.ShortKey_Config, 1U);
        }
    }

    private void OnGetConfigData(MSG_Req_OperateClientDatas_CS msg)
    {
        if (msg != null && !string.IsNullOrEmpty(msg.value))
        {
            MyJson.JsonNode_Object jsonNode_Object = MyJson.Parse(msg.value) as MyJson.JsonNode_Object;
            if (jsonNode_Object != null)
            {
                for (int i = 0; i < ShortcutsConfigController.sfdl.Count; i++)
                {
                    int sft = (int)ShortcutsConfigController.sfdl[i].sft;
                    string key = sft.ToString();
                    if (jsonNode_Object.ContainsKey(key))
                    {
                        MyJson.JsonNode_Object jsonNode_Object2 = jsonNode_Object[key] as MyJson.JsonNode_Object;
                        if (jsonNode_Object2 != null)
                        {
                            string text = jsonNode_Object2["key"].AsString();
                            string[] array = text.Split(new string[]
                            {
                                ","
                            }, StringSplitOptions.RemoveEmptyEntries);
                            if (array != null && array.Length > 0)
                            {
                                List<KeyCode> list = new List<KeyCode>();
                                foreach (string s in array)
                                {
                                    KeyCode item = (KeyCode)int.Parse(s);
                                    list.Add(item);
                                }
                                ShortcutsConfigController.sfdl[i].keys = list;
                            }
                            else
                            {
                                ShortcutsConfigController.sfdl[i].keys = null;
                            }
                        }
                    }
                }
            }
        }
    }

    internal void RegShortcutsKeyEvent()
    {
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Role, new Action(this.ShortcutsHeroUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.UnLockSkills, new Action(this.ShortcutsUnLockSkills));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Pokedex, new Action(this.ShortcutsPokedexUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Bag, new Action(this.ShortcutsBagUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Task, new Action(this.ShortcutsTaskUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Gene, new Action(this.ShortcutsGeneUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Budokai, new Action(this.ShortcutsBudokaiUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.FriendMenu, new Action(this.ShortcutsFriendUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.GuildMenu, new Action(this.ShortcutsGuildUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Map, new Action(this.ShortcutsMapUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Activity, new Action(this.ShortcutsActivityUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Mail, new Action(this.ShortcutsMailSwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.AutoFight, new Action(this.ShortcutsAutoFightSwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.MoveForward, new Action(this.ShortcutsMoveForward));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.MoveBack, new Action(this.ShortcutsMoveBack));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.MoveLeft, new Action(this.ShortcutsMoveLeft));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.MoveRight, new Action(this.ShortcutsMoveRight));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.Jump, new Action(this.ShortcutsJump));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.SwitchTarget, new Action(this.ShortcutsSwitchTargetUISwitch));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.SelectFriend1, new Action(this.ShortcutsSelectFriend1));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.SelectFriend2, new Action(this.ShortcutsSelectFriend2));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.SelectFriend3, new Action(this.ShortcutsSelectFriend3));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.SelectFriend4, new Action(this.ShortcutsSelectFriend4));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.PickDrop, new Action(this.ShortcutsPickDrop));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.HideGM, new Action(this.HideGM));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.HideFps, new Action(this.HideFps));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.HideUI, new Action(this.HideUI));
        this.BindEventToFunctionType(ShortcutkeyFunctionType.HideRole, new Action(this.HideRole));
    }

    private void HideGM()
    {
        if (this.GMTool == null)
        {
            this.GMTool = UnityEngine.Object.FindObjectOfType<GMToolManager>();
        }
        if (this.GMTool != null)
        {
            this.GMTool.gameObject.SetActive(!this.GMTool.gameObject.activeSelf);
        }
    }

    private void HideFps()
    {
        if (this.fps == null)
        {
            this.fps = UnityEngine.Object.FindObjectOfType<Fps>();
        }
        if (this.fps != null)
        {
            this.fps.enabled = !this.fps.enabled;
        }
    }

    private Dictionary<string, DefaulSetKeyData> InitDefaulSetKeyData()
    {
        Dictionary<string, DefaulSetKeyData> dictionary = new Dictionary<string, DefaulSetKeyData>();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("key");
        for (int i = 0; i < configTableList.Count; i++)
        {
            LuaTable luaTable = configTableList[i];
            DefaulSetKeyData defaulSetKeyData = new DefaulSetKeyData();
            defaulSetKeyData.tapType = (KeyConfigTapType)luaTable.GetCacheField_Uint("type");
            defaulSetKeyData.cname = luaTable.GetCacheField_String("showname");
            defaulSetKeyData.funname = luaTable.GetCacheField_String("funname");
            defaulSetKeyData.ktt = (KeyTriggerType)luaTable.GetCacheField_Uint("keytriggertype");
            string cacheField_String = luaTable.GetCacheField_String("keycode");
            List<KeyCode> list = new List<KeyCode>();
            if (!string.IsNullOrEmpty(cacheField_String))
            {
                string[] array = cacheField_String.Split(new char[]
                {
                    '+'
                });
                for (int j = 0; j < array.Length; j++)
                {
                    if (!string.IsNullOrEmpty(array[j]))
                    {
                        list.Add((KeyCode)((int)Enum.Parse(typeof(KeyCode), array[j])));
                    }
                }
            }
            defaulSetKeyData.keycode = list;
            dictionary.Add(defaulSetKeyData.funname, defaulSetKeyData);
        }
        return dictionary;
    }

    public void InitDataFinish()
    {
        if (ShortcutsConfigController.finishActions.Count == 0)
        {
            return;
        }
        foreach (Action action in ShortcutsConfigController.finishActions)
        {
            action();
        }
        ShortcutsConfigController.finishActions.Clear();
    }

    internal static void RegInitFinishEvent(Action cbAction)
    {
        ShortcutsConfigController.finishActions.Add(cbAction);
    }

    public override void OnUpdate()
    {
        this.InitDataFinish();
        if (null != EventSystem.current.currentSelectedGameObject && null != EventSystem.current.currentSelectedGameObject.GetComponent<InputField>())
        {
            return;
        }
        if (ShortcutsConfigController.isInGameSceneMap && !this.setKeydata && MainPlayer.Self != null)
        {
            this.SetKeysData(true);
            this.BindShortcutsKeyDefaultKeyCode(true);
            this.RegShortcutsKeyEvent();
            this.setKeydata = true;
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                controller.ReBindShortcutEvent();
            }
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (uiobject != null)
            {
                uiobject.FrashShortcutUIShowName();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MouseStateControoler.Instan.SetMoseState(MoseState.m_itemsplit);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (ControllerManager.Instance.GetController<ShopController>().shop != null)
            {
                MouseStateControoler.Instan.SetMoseState(MoseState.m_itemsell);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
        }
        if (this.isInShortcutsConfigMenu)
        {
            if (this.allCanUseKey == null || this.allCanUseKey.Count == 0)
            {
                this.allCanUseKey = this.GetAllCanUseKeyCode();
            }
            if (this.IsPickInvalidKey())
            {
                TipsWindow.ShowNotice("按键非法");
            }
            else
            {
                this.PickConfigInputKey();
            }
        }
        else if (this.isNeedCallShortcutEvent)
        {
            this.TryCallBindKeyEvent();
            this.CheckMoveData();
        }
    }

    public void ShortcutSwitch(bool state)
    {
        this.isNeedCallShortcutEvent = state;
    }

    private bool IsMoveingAction(ShortcutkeyFunctionType sft)
    {
        return !this.isScreenMove && (sft == ShortcutkeyFunctionType.MoveForward || sft == ShortcutkeyFunctionType.MoveBack || sft == ShortcutkeyFunctionType.MoveLeft || sft == ShortcutkeyFunctionType.MoveRight);
    }

    public void SetShortcutKeyEnableState(bool state, bool isScreenMove = true)
    {
        this.isInUnShortcutKeyState = !state;
        this.isScreenMove = isScreenMove;
    }

    private void CheckMoveData()
    {
        if (UI_Loading.isLoading)
        {
            SingletonForMono<InputController>.Instance.InputDir = -1;
            return;
        }
        float x = ((!this.moveLeft) ? 0.1f : -0.1f) + ((!this.moveRight) ? -0.1f : 0.1f);
        float y = ((!this.moveBack) ? 0.1f : -0.1f) + ((!this.moveForward) ? -0.1f : 0.1f);
        Vector2 v = new Vector2(x, y);
        if (MainPlayer.Self != null && MainPlayer.Self.inJumpState() && MainPlayer.Self.Fbc.CurrState.MoveDir() != -1)
        {
            SingletonForMono<InputController>.Instance.ReSetInputDir = (int)MainPlayer.Self.ServerDir;
        }
        if (SingletonForMono<InputController>.Instance.CurrentInputType == InputType.InputByUnityInput && CommonTools.CheckFloatEqual(v.x, 0f) && CommonTools.CheckFloatEqual(v.y, 0f))
        {
            SingletonForMono<InputController>.Instance.CurrentInputType = InputType.InputNone;
            SingletonForMono<InputController>.Instance.InputDir = -1;
        }
        else if (!CommonTools.CheckFloatEqual(v.x, 0f) || !CommonTools.CheckFloatEqual(v.y, 0f))
        {
            if (MainPlayer.Self == null)
            {
                return;
            }
            SingletonForMono<InputController>.Instance.CurrentInputType = InputType.InputByUnityInput;
            float num = Vector3.Angle(Vector2.up, v);
            if (v.x < 0f)
            {
                num = 360f - num;
            }
            if (this.lastInputangle != num)
            {
                SingletonForMono<InputController>.Instance.ReSet();
            }
            this.lastInputangle = num;
            if (MainPlayer.Self.inJumpState())
            {
                SingletonForMono<InputController>.Instance.FaceDir = (int)(num + CameraController.Self.Angle);
            }
            else if (MainPlayer.Self.inWalkState())
            {
                SingletonForMono<InputController>.Instance.InputDir = (int)(num + SingletonForMono<InputController>.Instance.angle);
            }
            else
            {
                SingletonForMono<InputController>.Instance.InputDir = (int)(num + CameraController.Self.Angle);
            }
        }
    }

    public void SetSettingUICloseState()
    {
        this.isInSettingMenu = false;
    }

    public void SetShortcutsConfigUIState(bool state)
    {
        this.isInShortcutsConfigMenu = state;
    }

    private string LogList<T>(List<T> listT)
    {
        string text = string.Empty;
        foreach (T t in listT)
        {
            string text2 = text;
            text = string.Concat(new object[]
            {
                text2,
                t.GetType(),
                ":",
                t,
                "|"
            });
        }
        return text;
    }

    private void LogDetectAllKeys()
    {
        this.IteratorList<KeyCode>(delegate (KeyCode item)
        {
            if (Input.GetKeyDown(item))
            {
                Debug.LogError(item);
            }
        }, this.allKey, 0);
    }

    private List<KeyCode> GetAllCanUseKeyCode()
    {
        List<KeyCode> list = new List<KeyCode>();
        list.AddRange(this.combinKey);
        list.AddRange(this.functionKey);
        return list;
    }

    private List<KeyCode> GetAllKeyCode()
    {
        Array values = Enum.GetValues(typeof(KeyCode));
        List<KeyCode> kcl = new List<KeyCode>();
        foreach (object obj in values)
        {
            if (!obj.ToString().ToLower().StartsWith("mouse") && !obj.ToString().ToLower().Contains("stick"))
            {
                kcl.Add((KeyCode)((int)obj));
            }
        }
        this.IteratorList<KeyCode>(delegate (KeyCode item)
        {
            if (kcl.Contains(item))
            {
                kcl.Remove(item);
            }
        }, this.combinKeyPrefix, 0);
        return kcl;
    }

    private bool IsPickInvalidKey()
    {
        bool re = false;
        this.IteratorList<KeyCode>(delegate (KeyCode item)
        {
            if (Input.GetKey(item))
            {
                re = true;
            }
        }, this.invalidKeyCode, 0);
        return re;
    }

    private void PickConfigInputKey()
    {
        ShortcutsConfigState shortcutsConfigState = this.configState;
        if (shortcutsConfigState != ShortcutsConfigState.InPickFunction)
        {
            if (shortcutsConfigState == ShortcutsConfigState.InPickKey)
            {
                List<KeyCode> allCanUseKey = this.GetAllCanUseKeyCode();
                bool isPickCombinKey = false;
                List<KeyCode> detect = new List<KeyCode>();
                this.IteratorList<KeyCode>(delegate (KeyCode item)
                {
                    if (Input.GetKey(item))
                    {
                        if (detect.Count == 0)
                        {
                            detect.Add(item);
                        }
                        isPickCombinKey = true;
                    }
                }, this.combinKeyPrefix, 0);
                List<KeyCode> pickUpKeyCode = new List<KeyCode>();
                this.IteratorList<KeyCode>(delegate (KeyCode item)
                {
                    if (Input.GetKeyDown(item))
                    {
                        pickUpKeyCode.Add(item);
                        if (!allCanUseKey.Contains(item))
                        {
                            if (item == KeyCode.Delete)
                            {
                                UI_ShortcutsConfig uiobject = UIManager.GetUIObject<UI_ShortcutsConfig>();
                                this.DelKeySet(uiobject.lastSelctSfd.sft);
                                uiobject.SetApplyConfigButtonState(true);
                                uiobject.RefrashUIState();
                            }
                            else
                            {
                                TipsWindow.ShowNotice("此按键不可设置");
                            }
                            return;
                        }
                    }
                }, this.allKey, 0);
                if (isPickCombinKey)
                {
                    this.IteratorList<KeyCode>(delegate (KeyCode item)
                    {
                        if (Input.GetKeyDown(item))
                        {
                            if (detect.Count == 1)
                            {
                                detect.Add(item);
                            }
                            else
                            {
                                TipsWindow.ShowNotice("按键设置数量超过两个");
                            }
                            if (detect.Count > 0)
                            {
                                this.UpdateSfdItem(detect);
                            }
                        }
                    }, this.combinKey, 0);
                }
                else
                {
                    this.IteratorList<KeyCode>(delegate (KeyCode item)
                    {
                        if (Input.GetKeyDown(item))
                        {
                            detect.Add(item);
                            if (detect.Count > 0)
                            {
                                this.UpdateSfdItem(detect);
                            }
                        }
                    }, allCanUseKey, 0);
                }
            }
        }
    }

    private void DelKeySet(ShortcutkeyFunctionType sft)
    {
        if (sft == ShortcutkeyFunctionType.End)
        {
            return;
        }
        this.IteratorList<ShortcutsFunctionData>(delegate (ShortcutsFunctionData item)
        {
            if (item.sft == sft)
            {
                item.keys = null;
                return;
            }
        }, ShortcutsConfigController.sfdl, 0);
    }

    private ShortcutsFunctionData IsConflictWithOthers(List<KeyCode> keys, ShortcutkeyFunctionType sft, out ShortcutkeyFunctionType conflictType)
    {
        conflictType = ShortcutkeyFunctionType.End;
        foreach (ShortcutsFunctionData shortcutsFunctionData in ShortcutsConfigController.sfdl)
        {
            if (shortcutsFunctionData.keys != null)
            {
                if (shortcutsFunctionData.keys.Count == keys.Count)
                {
                    bool flag = true;
                    foreach (KeyCode item in keys)
                    {
                        flag &= shortcutsFunctionData.keys.Contains(item);
                    }
                    if (flag && shortcutsFunctionData.sft != sft)
                    {
                        conflictType = shortcutsFunctionData.sft;
                        return shortcutsFunctionData;
                    }
                }
            }
        }
        return null;
    }

    private void UpdateSfdItem(List<KeyCode> keys)
    {
        UI_ShortcutsConfig uisc = UIManager.GetUIObject<UI_ShortcutsConfig>();
        uisc.SetApplyConfigButtonState(true);
        ShortcutkeyFunctionType conflictSft = ShortcutkeyFunctionType.End;
        ShortcutsFunctionData shortcutsFunctionData = this.IsConflictWithOthers(keys, uisc.lastSelctSfd.sft, out conflictSft);
        Action setSuccess = delegate ()
        {
            this.SetConfigState(ShortcutsConfigState.InPickFunction);
            int count = ShortcutsConfigController.sfdl.Count;
            for (int i = 0; i < count; i++)
            {
                if (ShortcutsConfigController.sfdl[i].sft == uisc.lastSelctSfd.sft)
                {
                    ShortcutsConfigController.sfdl[i].keys = keys;
                    break;
                }
            }
            if (conflictSft != uisc.lastSelctSfd.sft)
            {
                this.DelKeySet(conflictSft);
            }
            uisc.RefrashUIState();
        };
        if (shortcutsFunctionData != null)
        {
            string text = string.Empty;
            LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", 1603UL);
            if (configTable != null)
            {
                text = configTable.GetCacheField_String("notice");
                text = GlobalRegister.ChangeTextModel(text);
            }
            string s_describle = string.Format(text, shortcutsFunctionData.functionName);
            MsgBoxController controller = ControllerManager.Instance.GetController<MsgBoxController>();
            if (controller != null)
            {
                controller.ShowMsgBox(string.Empty, s_describle, "确定", "取消", UIManager.ParentType.CommonUI, delegate ()
                {
                    setSuccess();
                }, delegate ()
                {
                    this.SetConfigState(ShortcutsConfigState.InPickFunction);
                    uisc.RefrashUIState();
                }, delegate ()
                {
                });
            }
        }
        else
        {
            setSuccess();
        }
    }

    private void TryCallBindKeyEvent()
    {
        if (this.combinKeyPrefix == null)
        {
            return;
        }
        this.moveForward = false;
        this.moveBack = false;
        this.moveLeft = false;
        this.moveRight = false;
        bool flag = false;
        for (int i = 0; i < this.combinKeyPrefix.Count; i++)
        {
            if (Input.GetKey(this.combinKeyPrefix[i]))
            {
                flag = true;
                break;
            }
        }
        for (int j = 0; j < ShortcutsConfigController.sfdl.Count; j++)
        {
            ShortcutsFunctionData shortcutsFunctionData = ShortcutsConfigController.sfdl[j];
            if (shortcutsFunctionData.keys != null && shortcutsFunctionData.keys.Count > 0)
            {
                if (shortcutsFunctionData.keys.Count == 1)
                {
                    if (!flag)
                    {
                        bool flag2 = false;
                        switch (shortcutsFunctionData.ktt)
                        {
                            case KeyTriggerType.KeyDown:
                                if (Input.GetKeyDown(shortcutsFunctionData.keys[0]))
                                {
                                    flag2 = true;
                                }
                                break;
                            case KeyTriggerType.KeyUp:
                                if (Input.GetKeyUp(shortcutsFunctionData.keys[0]))
                                {
                                    flag2 = true;
                                }
                                break;
                            case KeyTriggerType.KeyPress:
                                if (Input.GetKey(shortcutsFunctionData.keys[0]))
                                {
                                    flag2 = true;
                                }
                                break;
                        }
                        if (flag2)
                        {
                            if (shortcutsFunctionData.sft > (ShortcutkeyFunctionType)100 && shortcutsFunctionData.sft < (ShortcutkeyFunctionType)500)
                            {
                                if (!this.isInUnShortcutKeyState || this.IsMoveingAction(shortcutsFunctionData.sft))
                                {
                                    int sft = (int)shortcutsFunctionData.sft;
                                    int num = sft / 100;
                                    int num2 = sft % 100;
                                    this.ExtendItemCallAction(shortcutsFunctionData.sft);
                                }
                            }
                            else if ((!this.isInUnShortcutKeyState || this.IsMoveingAction(shortcutsFunctionData.sft)) && shortcutsFunctionData.callBack != null)
                            {
                                shortcutsFunctionData.callBack();
                            }
                        }
                    }
                }
                else
                {
                    bool flag3 = true;
                    for (int k = 0; k < shortcutsFunctionData.keys.Count - 1; k++)
                    {
                        flag3 &= Input.GetKey(shortcutsFunctionData.keys[k]);
                    }
                    if (flag3 && Input.GetKeyDown(shortcutsFunctionData.keys[shortcutsFunctionData.keys.Count - 1]))
                    {
                        if (shortcutsFunctionData.sft > (ShortcutkeyFunctionType)100 && shortcutsFunctionData.sft < (ShortcutkeyFunctionType)500)
                        {
                            if (!this.isInUnShortcutKeyState || this.IsMoveingAction(shortcutsFunctionData.sft))
                            {
                                this.ExtendItemCallAction(shortcutsFunctionData.sft);
                            }
                        }
                        else if ((!this.isInUnShortcutKeyState || this.IsMoveingAction(shortcutsFunctionData.sft)) && shortcutsFunctionData.callBack != null)
                        {
                            shortcutsFunctionData.callBack();
                        }
                    }
                }
            }
        }
    }

    private void CloseConfigUI()
    {
        this.SetKeysData(false);
    }

    public void Reset()
    {
        ShortcutsConfigController.isInGameSceneMap = false;
        this.setKeydata = false;
        this.SetKeysData(false);
    }

    private void SetKeysData(bool isSetOrSetNull)
    {
        if (isSetOrSetNull)
        {
            this.combinKeyPrefix = new List<KeyCode>();
            this.combinKey = new List<KeyCode>();
            this.functionKey = new List<KeyCode>();
            this.allKey = new List<KeyCode>();
            this.combinKeyPrefix.Add(KeyCode.LeftControl);
            this.combinKeyPrefix.Add(KeyCode.RightControl);
            this.combinKeyPrefix.Add(KeyCode.LeftShift);
            this.combinKeyPrefix.Add(KeyCode.RightShift);
            this.IteratorForGetEnumList(delegate (int item)
            {
                this.combinKey.Add((KeyCode)item);
            }, 97, 122);
            this.IteratorForGetEnumList(delegate (int item)
            {
                this.combinKey.Add((KeyCode)item);
            }, 48, 57);
            this.IteratorForGetEnumList(delegate (int item)
            {
                this.functionKey.Add((KeyCode)item);
            }, 282, 293);
            this.combinKey.Add(KeyCode.BackQuote);
            this.combinKey.Add(KeyCode.Minus);
            this.combinKey.Add(KeyCode.Equals);
            this.combinKey.Add(KeyCode.LeftBracket);
            this.combinKey.Add(KeyCode.RightBracket);
            this.combinKey.Add(KeyCode.Backslash);
            this.combinKey.Add(KeyCode.Semicolon);
            this.combinKey.Add(KeyCode.Quote);
            this.combinKey.Add(KeyCode.Comma);
            this.combinKey.Add(KeyCode.Period);
            this.combinKey.Add(KeyCode.Slash);
            this.combinKey.Add(KeyCode.Tab);
            this.combinKey.Add(KeyCode.UpArrow);
            this.combinKey.Add(KeyCode.DownArrow);
            this.combinKey.Add(KeyCode.LeftArrow);
            this.combinKey.Add(KeyCode.RightArrow);
            this.IteratorForGetEnumList(delegate (int item)
            {
                this.combinKey.Add((KeyCode)item);
            }, 256, 265);
            this.allKey = this.GetAllKeyCode();
        }
        else
        {
            this.combinKeyPrefix = null;
            this.combinKey = null;
            this.functionKey = null;
            this.allKey = null;
        }
    }

    public void SetConfigState(ShortcutsConfigState cs)
    {
        this.configState = cs;
    }

    public override string ControllerName
    {
        get
        {
            return "shortcutsconfig_controller";
        }
    }

    public void BindEventToFunctionType(ShortcutkeyFunctionType functionType, Action callBack)
    {
        this.InitSfdl();
        this.IteratorList<ShortcutsFunctionData>(delegate (ShortcutsFunctionData item)
        {
            if (item.sft == functionType)
            {
                item.callBack = callBack;
            }
        }, ShortcutsConfigController.sfdl, 0);
    }

    public void CallSkillSlotEvent(ShortcutkeyFunctionType sft)
    {
        this.IteratorList<ShortcutsFunctionData>(delegate (ShortcutsFunctionData item)
        {
            if (item.sft == sft && item.callBack != null)
            {
                item.callBack();
            }
        }, ShortcutsConfigController.sfdl, 0);
    }

    public void BindKeysToFunctionType(ShortcutkeyFunctionType functionType, List<KeyCode> keys)
    {
        this.InitSfdl();
        this.IteratorList<ShortcutsFunctionData>(delegate (ShortcutsFunctionData item)
        {
            if (functionType == item.sft)
            {
                item.keys = keys;
                return;
            }
        }, ShortcutsConfigController.sfdl, 0);
    }

    private void InitSfdl()
    {
        if (ShortcutsConfigController.sfdl.Count != 0)
        {
            return;
        }
        string[] names = Enum.GetNames(typeof(ShortcutkeyFunctionType));
        this.IteratorArray<string>(delegate (string s)
        {
            ShortcutsFunctionData shortcutsFunctionData = new ShortcutsFunctionData((ShortcutkeyFunctionType)((int)Enum.Parse(typeof(ShortcutkeyFunctionType), s)), null, this.dskdDic, null);
            if (shortcutsFunctionData.sft != ShortcutkeyFunctionType.Activity)
            {
                ShortcutsConfigController.sfdl.Add(shortcutsFunctionData);
            }
        }, names, 0);
    }

    private void IteratorList<T>(Action<T> back, List<T> listT, int minusCount = 0)
    {
        int num = listT.Count - minusCount;
        for (int i = 0; i < num; i++)
        {
            back(listT[i]);
        }
    }

    private void IteratorForGetEnumList(Action<int> back, int startInext, int endIndex)
    {
        for (int i = startInext; i <= endIndex; i++)
        {
            back(i);
        }
    }

    private void IteratorArray<T>(Action<T> back, T[] tArray, int minusCount = 0)
    {
        int num = tArray.Length - minusCount;
        for (int i = 0; i < num; i++)
        {
            back(tArray[i]);
        }
    }

    public void ShortcutsHeroUISwitch()
    {
        ControllerManager.Instance.GetController<CardController>().OpenCharacterUI();
    }

    private void ShortcutsUnLockSkills()
    {
        if (UIManager.GetUIObject<UI_UnLockSkills>() == null)
        {
            UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
            if (controller != null)
            {
                controller.OpenFrame(UnLockSkillsController.ManualType.Self);
            }
        }
        else
        {
            UIManager.Instance.DeleteUI<UI_UnLockSkills>();
        }
    }

    public void ShortcutsPokedexUISwitch()
    {
        ControllerManager.Instance.GetController<HeroHandbookController>().OpenUI();
    }

    public void ShortcutsBagUISwitch()
    {
        UIManager.GetUIObject<UI_MainView>().enterBag(null);
    }

    public void ShortcutsTaskUISwitch()
    {
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel("UI_TaskList");
        if (luaUIPanel != null)
        {
            if (Vector3.Distance(luaUIPanel.uiRoot.transform.localPosition, Vector3.zero) > 10f)
            {
                ControllerManager.Instance.GetController<TaskUIController>().OpenTaskView();
            }
            else
            {
                LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.CloseTaskList", new object[]
                {
                    Util.GetLuaTable("NpcTalkAndTaskDlgCtrl")
                });
            }
        }
        else
        {
            ControllerManager.Instance.GetController<TaskUIController>().OpenTaskView();
        }
    }

    private void ShortcutsGeneUISwitch()
    {
        if (UIManager.GetUIObject<UI_Gene>() == null)
        {
            GlobalRegister.OpenGeneUI(0);
        }
        else
        {
            UIManager.Instance.DeleteUI<UI_Gene>();
        }
    }

    private void ShortcutsBudokaiUISwitch()
    {
        if (UIManager.GetUIObject<UI_PVPMatch>() != null)
        {
            ControllerManager.Instance.GetController<PVPMatchController>().CloseUI();
        }
        else
        {
            ControllerManager.Instance.GetController<PVPMatchController>().ShowUI();
        }
    }

    private void ShortcutsFriendUISwitch()
    {
        if (UIManager.GetUIObject<UI_FriendNew>() != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_FriendNew");
        }
        else
        {
            UIManager.GetUIObject<UI_MainView>().enterFriendView(null);
        }
    }

    private void ShortcutsGuildUISwitch()
    {
        GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
        controller.OpenCloseGuildPanel();
    }

    private void ShortcutsMailSwitch()
    {
        MailControl controller = ControllerManager.Instance.GetController<MailControl>();
        if (controller == null)
        {
            return;
        }
        UI_Mail uiobject = UIManager.GetUIObject<UI_Mail>();
        if (uiobject != null)
        {
            controller.CloseUI();
        }
        else
        {
            controller.ShowMailUI();
        }
    }

    private void ShortcutsAutoFightSwitch()
    {
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null != uiobject)
        {
            uiobject.AutoFight();
        }
    }

    private void ShortcutsMapUISwitch()
    {
        UI_Map uiobject = UIManager.GetUIObject<UI_Map>();
        if (uiobject != null)
        {
            if (uiobject.Panel_Large.gameObject.activeInHierarchy)
            {
                uiobject.CloseBigMap(null);
            }
            else
            {
                uiobject.OpenBigMap(null);
            }
        }
    }

    private void ShortcutsActivityUISwitch()
    {
        ControllerManager.Instance.GetController<MainUIController>().OpenAcitvityGuide();
    }

    private void ShortcutsMoveForward()
    {
        this.moveForward = true;
    }

    private void ShortcutsMoveBack()
    {
        this.moveBack = true;
    }

    private void ShortcutsMoveLeft()
    {
        this.moveLeft = true;
    }

    private void ShortcutsMoveRight()
    {
        this.moveRight = true;
    }

    private void ShortcutsJump()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.IsCanJump())
        {
            MainPlayer.Self.Jump(true, false);
        }
    }

    public void ShortcutsSwitchTargetUISwitch()
    {
        if (MainPlayerSkillHolder.Instance.CurrPlayerSkill == null || MainPlayerSkillHolder.Instance.CurrPlayerSkill.NormalSkill)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TabSelect.ReqTarget();
        }
        MainPlayer.Self.BreakAutoAttack();
    }

    private void ShortcutsSelectFriend1()
    {
    }

    private void ShortcutsSelectFriend2()
    {
    }

    private void ShortcutsSelectFriend3()
    {
    }

    private void ShortcutsSelectFriend4()
    {
    }

    private void ShortcutsPickDrop()
    {
        PickDropController controller = ControllerManager.Instance.GetController<PickDropController>();
        if (controller != null)
        {
            Npc autoPickTarget = controller.CheckIsHaveBagNpcInNineScreen(null);
            controller.ShortcutQuickPickAll(autoPickTarget);
        }
    }

    public void HideUI()
    {
        this.curUIState = !this.curUIState;
        if (UIManager.Instance != null && UIManager.Instance.UIRoot)
        {
            UIManager.Instance.GetUICamera().gameObject.SetActive(this.curUIState);
        }
        Fps fps = UnityEngine.Object.FindObjectOfType<Fps>();
        if (fps != null)
        {
            fps.enabled = this.curUIState;
        }
    }

    private GameObject hpRoot
    {
        get
        {
            if (this.hpRoot_ == null)
            {
                this.hpRoot_ = GameObject.Find("hpRoot");
            }
            return this.hpRoot_;
        }
    }

    private GameObject objPoosRoot
    {
        get
        {
            if (this.objPoosRoot_ == null)
            {
                this.objPoosRoot_ = GameObject.Find("ObjectPoolRoot");
            }
            return this.objPoosRoot_;
        }
    }

    public void HideRole()
    {
        int num = this.curRoleState;
        this.curRoleState = (this.curRoleState + 1) % 3;
        if (this.curRoleState == 1)
        {
            this.ToggleHidePlayer(true);
        }
        else if (num == 1)
        {
            this.ToggleHidePlayer(false);
        }
        if (this.curRoleState == 2)
        {
            if (this.hpRoot != null)
            {
                this.hpRoot.SetActive(false);
            }
            if (this.objPoosRoot != null)
            {
                this.objPoosRoot.SetActive(false);
            }
        }
        else if (num == 2)
        {
            if (this.hpRoot != null)
            {
                this.hpRoot.SetActive(true);
            }
            if (this.objPoosRoot != null)
            {
                this.objPoosRoot.SetActive(true);
            }
        }
    }

    public void ToggleHidePlayer(bool hide)
    {
        if (this.fps == null)
        {
            this.fps = UnityEngine.Object.FindObjectOfType<Fps>();
        }
        this.fps.HidePlayerByPlayer(hide);
    }

    public void ExtendItemCallAction(ShortcutkeyFunctionType sft)
    {
        int row = (int)sft / 100;
        int col = (int)sft % 100;
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null != uiobject)
        {
            uiobject.Root.GetComponent<UI_ShortcutControl>().UseItem(row, col);
        }
    }

    public float GetBindPropCdDuration(ShortcutkeyFunctionType sft)
    {
        if (sft < ShortcutkeyFunctionType.ExtendItem1_1 || sft > ShortcutkeyFunctionType.ExtendItem1_12)
        {
            return -1f;
        }
        float result = -1f;
        int row = (int)sft / 100;
        int col = (int)sft % 100;
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (null != uiobject)
        {
            result = uiobject.Root.GetComponent<UI_ShortcutControl>().GetUseItemCd(row, col);
        }
        return result;
    }

    public List<ShortcutsFunctionData> GetShortcutsDatayByAsisType(KeyConfigTapType abt)
    {
        return (from item in ShortcutsConfigController.sfdl
                where item.abt == abt
                select item).ToList<ShortcutsFunctionData>();
    }

    internal List<ShortcutsFunctionData> CloneShortcutsFunctionData(List<ShortcutsFunctionData> src)
    {
        List<ShortcutsFunctionData> list = new List<ShortcutsFunctionData>();
        foreach (ShortcutsFunctionData shortcutsFunctionData in src)
        {
            list.Add(new ShortcutsFunctionData(shortcutsFunctionData.sft, shortcutsFunctionData.callBack, this.dskdDic, shortcutsFunctionData.keys)
            {
                functionName = shortcutsFunctionData.functionName,
                abt = shortcutsFunctionData.abt,
                callBack = shortcutsFunctionData.callBack
            });
        }
        return list;
    }

    public void SaveConfig()
    {
        MyJson.JsonNode_Object jsonNode_Object = new MyJson.JsonNode_Object();
        for (int i = 0; i < ShortcutsConfigController.sfdl.Count; i++)
        {
            MyJson.JsonNode_Object jsonNode_Object2 = new MyJson.JsonNode_Object();
            string text = string.Empty;
            if (ShortcutsConfigController.sfdl[i].keys != null && ShortcutsConfigController.sfdl[i].keys.Count > 0)
            {
                for (int j = 0; j < ShortcutsConfigController.sfdl[i].keys.Count; j++)
                {
                    text = text + (int)ShortcutsConfigController.sfdl[i].keys[j] + ",";
                }
            }
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
            }
            jsonNode_Object2["key"] = new MyJson.JsonNode_ValueString(text);
            Dictionary<string, MyJson.IJsonNode> dictionary = jsonNode_Object;
            int sft = (int)ShortcutsConfigController.sfdl[i].sft;
            dictionary[sft.ToString()] = jsonNode_Object2;
        }
        ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.ShortKey_Config, jsonNode_Object.ToString(), 1U);
    }

    private string ConfigFileName
    {
        get
        {
            return MainPlayer.Self.OtherPlayerData.BaseData.id + "_shortcutsconfig.config";
        }
    }

    public string GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType sft)
    {
        for (int i = 0; i < ShortcutsConfigController.sfdl.Count; i++)
        {
            if (ShortcutsConfigController.sfdl[i].sft == sft)
            {
                return ShortcutsConfigController.sfdl[i].keyShowNameInMainUI;
            }
        }
        return string.Empty;
    }

    public string GetTitleNameForItemByFunctionType(ShortcutkeyFunctionType sft)
    {
        for (int i = 0; i < ShortcutsConfigController.sfdl.Count; i++)
        {
            if (ShortcutsConfigController.sfdl[i].sft == sft)
            {
                return ShortcutsConfigController.sfdl[i].functionName;
            }
        }
        return string.Empty;
    }

    private List<KeyCode> InitIvalidKeyCode()
    {
        List<KeyCode> list = new List<KeyCode>();
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("other").GetCacheField_Table("invalidkey");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            KeyCode item = (KeyCode)((int)Enum.Parse(typeof(KeyCode), luaTable.GetField_String("keycodename")));
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
        return list;
    }

    private List<KeyCode> combinKeyPrefix;

    private List<KeyCode> allKey;

    private List<KeyCode> combinKey;

    private List<KeyCode> functionKey;

    private List<KeyCode> allCanUseKey;

    private bool isInSettingMenu;

    private bool isInShortcutsConfigMenu;

    private bool moveForward;

    private bool moveBack;

    private bool moveLeft;

    private bool moveRight;

    private bool isInUnShortcutKeyState;

    private bool isScreenMove;

    public static List<ShortcutsFunctionData> sfdl = null;

    public ShortcutsConfigState configState;

    private Dictionary<string, DefaulSetKeyData> dskdDic_;

    private List<KeyCode> invalidKeyCode_;

    public static bool isInGameSceneMap = false;

    private GMToolManager GMTool;

    private Fps fps;

    internal static List<Action> finishActions = new List<Action>();

    private bool setKeydata;

    private bool isNeedCallShortcutEvent = true;

    private float lastInputangle = -1f;

    private bool curUIState = true;

    private int curRoleState;

    private GameObject hpRoot_;

    private GameObject objPoosRoot_;
}
