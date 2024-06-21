using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class LocalGMController : ControllerBase
{
    public void Init()
    {
    }

    public void RegistInputField(InputField input)
    {
        this.inputchat = input;
    }

    public void RemoveInputField()
    {
        this.inputchat = null;
    }

    public bool IsLocalGmText(string Text)
    {
        string text = Text.ToLower();
        if (text == "//gui")
        {
            InputController.ShowTestBtn = !InputController.ShowTestBtn;
            return true;
        }
        if (text == "//bossdiecameramove")
        {
            CameraController.Self.EditorWinOpen = true;
            CameraController.Self.ChangeState(new BossDieCameraMove());
            return true;
        }
        if (text == "//nocd")
        {
            MainPlayerSkillBase.NoCD = true;
            return false;
        }
        if (text == "//charinfo")
        {
            InputController.IsShowCharactorInfo = true;
            return true;
        }
        if (text == "//cameratest")
        {
            CameraController.Self.EditorWinOpen = true;
            return true;
        }
        if (string.Compare(text, "//npcdlg") == 0)
        {
            LuaProcess.ParserAndCallNpcLua_NpcDlg();
            return true;
        }
        if (text.StartsWith("//gene"))
        {
            string[] array = text.Split(new char[]
            {
                ' '
            });
            GlobalRegister.OpenGeneUI(int.Parse(array[1]));
            return true;
        }
        if (text.StartsWith("//unlockskills"))
        {
            GlobalRegister.OpenUnLockSkillsUI();
            return true;
        }
        if (string.Compare(text, "//npcpos") == 0)
        {
            NpcIconSetHelper.EnableSeting();
            return true;
        }
        if (text.StartsWith("//wc"))
        {
            CopyWayCheckContoller controller = ControllerManager.Instance.GetController<CopyWayCheckContoller>();
            if (controller != null)
            {
                controller.GmTest(text);
            }
            return true;
        }
        if (text.StartsWith("//qtesettest "))
        {
            UI_QTE uiobject = UIManager.GetUIObject<UI_QTE>();
            if (uiobject == null)
            {
                UI_QTE.SetTest(text);
            }
            else
            {
                UIManager.Instance.DeleteUI<UI_QTE>();
            }
            return true;
        }
        if (string.Compare(text, "//testtask") == 0)
        {
            //TestTaskBackData.TaskTestMessageSwitch(); 
            return true;
        }
        if (string.Compare(text, "//setplayerdir") == 0)
        {
            UIManager.Instance.ShowUI<UI_SetPlayerDirTest>("UI_SetPlayerDirTest", null, UIManager.ParentType.CommonUI, false);
            return true;
        }
        if (string.Compare(text, "//tasktrackcheck") == 0)
        {
            UI_TaskTrackCheck uiobject2 = UIManager.GetUIObject<UI_TaskTrackCheck>();
            if (uiobject2 == null)
            {
                UIManager.Instance.ShowUI<UI_TaskTrackCheck>("UI_TaskTrackCheck", null, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.Instance.DeleteUI<UI_TaskTrackCheck>();
            }
            return true;
        }
        if (string.Compare(text, "//testvip") == 0)
        {
            UI_VipPrivilege uiobject3 = UIManager.GetUIObject<UI_VipPrivilege>();
            if (uiobject3 == null)
            {
                UIManager.Instance.ShowUI<UI_VipPrivilege>("UI_VipPrivilege", null, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.Instance.DeleteUI<UI_VipPrivilege>();
            }
            return true;
        }
        if (text.StartsWith("//ztest"))
        {
            string[] array2 = text.Split(new char[]
            {
                ' '
            });
            if (array2.Length == 5)
            {
                float x = float.Parse(array2[1]);
                float y = float.Parse(array2[2]);
                float num = float.Parse(array2[3]);
                float num2 = float.Parse(array2[4]);
                float x2 = 0.333333343f * num * 2f;
                float z = 0.333333343f * num2 * 2f;
                Vector3 localScale = new Vector3(x2, 0.2f, z);
                Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(new Vector2(x, y));
                worldPosByServerPos.y = MainPlayer.Self.ModelObj.transform.position.y;
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gameObject.transform.position = worldPosByServerPos;
                gameObject.transform.localScale = localScale;
            }
            return true;
        }
        if (string.Compare(text, "//testcamtonpcdist") == 0)
        {
            UIFllowTarget[] array3 = UnityEngine.Object.FindObjectsOfType<UIFllowTarget>();
            if (array3 != null)
            {
                for (int i = 0; i < array3.Length; i++)
                {
                    array3[i].testMode = !array3[i].testMode;
                }
            }
            return true;
        }
        if (string.Compare(text, "//3diconposcheck") == 0)
        {
            UIManager.Instance.ShowUI<UI_3DIconModelPosCheck>("UI_3DIconModelPosCheck", null, UIManager.ParentType.CommonUI, false);
            return true;
        }
        if (string.Compare(text, "//cleartasktestdata") == 0)
        {
            //TestTaskBackData.testBackData = string.Empty;
            return true;
        }
        if (text.CompareTo("//showblock") == 0)
        {
            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
            if (manager != null)
            {
                manager.ShowBlockPoint();
            }
            return true;
        }
        if (text.CompareTo("//resetblock") == 0)
        {
            GameScene manager2 = ManagerCenter.Instance.GetManager<GameScene>();
            if (manager2 != null)
            {
                manager2.ResetBlock();
            }
            return true;
        }
        if (text.CompareTo("//clearblock") == 0)
        {
            GameScene manager3 = ManagerCenter.Instance.GetManager<GameScene>();
            if (manager3 != null)
            {
                manager3.ClearBlock();
            }
            return true;
        }
        if (text.StartsWith("//robot:"))
        {
            int length = "//robot:".Length;
            string target = text.Substring(length, text.Length - length);
            if (this.mrobot == null)
            {
                this.mrobot = new robot();
                this.mrobot.Init();
            }
            this.mrobot.SetTarget(target);
            return true;
        }
        if (text.StartsWith("//testeffectgroup:"))
        {
            int length2 = "//testeffectgroup:".Length;
            string key = text.Substring(length2, text.Length - length2);
            FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
            FFEffectManager manager4 = ManagerCenter.Instance.GetManager<FFEffectManager>();
            if (component != null)
            {
                string[] group = manager4.GetGroup(key);
                for (int j = 0; j < group.Length; j++)
                {
                    component.AddEffect(group[j], null, null);
                }
            }
            return true;
        }

        if (text.StartsWith("//maufeat"))
        {
            string level = "//fetch id=1 num=1215752192";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, level, 0U, null);
            string diamonds = "//fetch id=2 num=1000000000";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, diamonds, 0U, null);
            string zen = "//fetch id=3 num=1000000000";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, zen, 0U, null);
            string idk = "//fetch id=4 num=1000000000";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, idk, 0U, null);
            string @char = "//fetch id=89001 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, @char, 0U, null);
            string char2 = "//fetch id=89002 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char2, 0U, null);
            string char3 = "//fetch id=89003 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char3, 0U, null);
            string char4 = "//fetch id=89004 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char4, 0U, null);
            string char5 = "//fetch id=89005 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char5, 0U, null);
            string char6 = "//fetch id=89006 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char6, 0U, null);
            string char7 = "//fetch id=89007 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char7, 0U, null);
            string char8 = "//fetch id=89008 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char8, 0U, null);
            string char9 = "//fetch id=89009 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char9, 0U, null);
            string char10 = "//fetch id=89010 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char10, 0U, null);
            string char11 = "//fetch id=89011 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char11, 0U, null);
            string char12 = "//fetch id=89012 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char12, 0U, null);
            string char13 = "//fetch id=89013 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char13, 0U, null);
            string char14 = "//fetch id=89014 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char14, 0U, null);
            string char15 = "//fetch id=89015 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char15, 0U, null);
            string char16 = "//fetch id=89016 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char16, 0U, null);
            string char17 = "//fetch id=89017 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char17, 0U, null);
            string char18 = "//fetch id=89018 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char18, 0U, null);
            string char19 = "//fetch id=89019 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char19, 0U, null);
            string char20 = "//fetch id=89020 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char20, 0U, null);
            string char21 = "//fetch id=89021 num=1";
            ControllerManager.Instance.GetController<ChatControl>().SendChat(Chat.ChannelType.ChannelType_GmTool, char21, 0U, null);
        }
        if (text.StartsWith("//setlog:"))
        {
            string[] array4 = text.Split(new char[]
            {
                ':'
            });
            if (array4.Length > 1)
            {
                if (array4[1].Equals("0"))
                {
                    FFDebug.AllDisable = true;
                }
                else if (array4[1].Equals("1"))
                {
                    FFDebug.AllDisable = false;
                    if (array4.Length > 2)
                    {
                        string[] array5 = array4[2].Split(new char[]
                        {
                            ' '
                        });
                        if (array4 == null || array5.Length < 1)
                        {
                            return true;
                        }
                        List<object> list = new List<object>();
                        for (int k = 0; k < array5.Length; k++)
                        {
                            int num3 = int.Parse(array5[k]);
                            list.Add(((FFLogType)num3).ToString());
                        }
                        FFDebug.SetLogType(list.ToArray());
                    }
                }
                else if (array4[1].Equals("2"))
                {
                    FFDebug.AllDisable = false;
                    FFDebug.ShowGUIlogButton = true;
                }
            }
            return true;
        }
        if (!text.StartsWith("//messagetest"))
        {
            if (text.StartsWith("//showstateclient"))
            {
                uint num4 = 0U;
                try
                {
                    string[] array6 = text.Split(new char[]
                    {
                        ' '
                    });
                    for (int l = 1; l < array6.Length; l++)
                    {
                        string[] array7 = array6[l].Split(new char[]
                        {
                            '='
                        });
                        if (array7[0].CompareTo("id") == 0)
                        {
                            num4 = uint.Parse(array7[1]);
                        }
                    }
                    FFDebug.LogWarning("GM", string.Format("Contain state with id {0} ?  {1}", num4, MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState((UserState)num4)));
                }
                catch (Exception)
                {
                    FFDebug.LogWarning("GM", "Parse GM Commond Failed " + text);
                }
                return true;
            }
            if (text.StartsWith("//cutscene"))
            {
                try
                {
                    string text2 = string.Empty;
                    string[] array8 = text.Split(new char[]
                    {
                        ' '
                    });
                    for (int m = 1; m < array8.Length; m++)
                    {
                        string[] array9 = array8[m].Split(new char[]
                        {
                            '='
                        });
                        if (array9[0].CompareTo("name") == 0)
                        {
                            text2 = array9[1];
                        }
                    }
                    TaskController controller2 = ControllerManager.Instance.GetController<TaskController>();
                    if (controller2 != null && !string.IsNullOrEmpty(text2))
                    {
                        uint num5 = 0U;
                        uint.TryParse(text2, out num5);
                        if (num5 > 0U)
                        {
                            controller2.ShowCutSceneByID(num5, null);
                        }
                    }
                    FFDebug.LogWarning("GM", string.Format("Contain cutscene with name {0}", text2));
                }
                catch (Exception)
                {
                    FFDebug.LogWarning("GM", "Parse GM Commond Failed " + text);
                }
                return true;
            }
            if (text == "open freecamera")
            {
                if (CameraController.Self == null || CameraController.Self.Target == null)
                {
                    FFDebug.LogError(this, "CameraController.Self == null || CameraController.Self.Target == null");
                }
                if (FreeCameraManager.freeCamera != null)
                {
                    UnityEngine.Object.DestroyImmediate(FreeCameraManager.freeCamera.gameObject);
                    FreeCameraManager.freeCamera = null;
                }
                if (FreeCameraManager.freeCamera == null)
                {
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(CameraController.Self.gameObject);
                    UnityEngine.Object.DestroyImmediate(gameObject2.GetComponent<CameraController>());
                    gameObject2.GetComponent<Camera>().depth = 0f;
                    gameObject2.tag = "Untagged";
                    FreeCameraManager.freeCamera = gameObject2.AddComponent<FreeCameraManager>();
                    FreeCameraManager.freeCamera.Init(CameraController.Self.Target.transform.position);
                }
                return true;
            }
            if (text == "close freecamera")
            {
                if (FreeCameraManager.freeCamera != null)
                {
                    UnityEngine.Object.DestroyImmediate(FreeCameraManager.freeCamera.gameObject);
                    FreeCameraManager.freeCamera = null;
                }
                return true;
            }
            if (text == "open autoattack")
            {
                MyPlayerPrefs.SetInt("AutoAttackClosed", 0);
                return true;
            }
            if (text == "close autoattack")
            {
                MyPlayerPrefs.SetInt("AutoAttackClosed", 1);
                return true;
            }
            if (text.StartsWith("//guide"))
            {
                string[] array10 = text.Split(new char[]
                {
                    ' '
                });
                if (array10.Length > 1)
                {
                    uint id = 0U;
                    if (uint.TryParse(array10[1], out id))
                    {
                        GuideController controller3 = ControllerManager.Instance.GetController<GuideController>();
                        controller3.ViewGuideUI(id, true);
                    }
                }
                return true;
            }
            if (text.StartsWith("//cfg"))
            {
                string[] array11 = text.Split(new char[]
                {
                    ' '
                });
                if (array11.Length > 1)
                {
                    LuaScriptMgr.Instance.CallLuaFunction("ConfigManager.ParseConfig", new object[]
                    {
                        Util.GetLuaTable("ConfigManager"),
                        array11[1]
                    });
                    GuideController gc = ControllerManager.Instance.GetController<GuideController>();
                    Scheduler.Instance.AddTimer(1f, false, delegate
                    {
                        gc.InitDicGuide(true);
                    });
                }
                return true;
            }
        }
        return false;
    }

    public string ProcessGMWithSpace(string s)
    {
        string result = s;
        bool flag = false;
        if (s.StartsWith(" "))
        {
            int num = s.IndexOf("//");
            if (num > 0)
            {
                bool flag2 = this.InputAllSpace(s.Substring(0, num));
                if (flag2)
                {
                    flag = true;
                }
            }
        }
        if (flag)
        {
            result = s.Substring(s.IndexOf("//"));
        }
        return result;
    }

    public void AddCachedCommand(string s)
    {
        this.cachedcommand.Add(s);
        if (this.cachedcommand.Count > 10)
        {
            this.cachedcommand.RemoveAt(0);
        }
        this.currentcommandid = this.cachedcommand.Count;
    }

    private bool InputAllSpace(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != ' ')
            {
                return false;
            }
        }
        return true;
    }

    public override string ControllerName
    {
        get
        {
            return "localgm_controller";
        }
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
        if (this.inputchat == null)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (this.cachedcommand.Count == 0)
            {
                return;
            }
            if (this.currentcommandid > 0)
            {
                this.currentcommandid--;
                this.inputchat.text = this.cachedcommand[this.currentcommandid];
                this.inputchat.ActivateInputField();
                this.inputchat.MoveTextEnd(false);
            }
            else if (this.currentcommandid == 0)
            {
                this.inputchat.text = this.cachedcommand[0];
                this.inputchat.ActivateInputField();
                this.inputchat.MoveTextEnd(false);
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (this.cachedcommand.Count == 0)
            {
                return;
            }
            if (this.currentcommandid < this.cachedcommand.Count - 1)
            {
                this.currentcommandid++;
                this.inputchat.text = this.cachedcommand[this.currentcommandid];
                this.inputchat.ActivateInputField();
                this.inputchat.MoveTextEnd(false);
            }
            else if (this.currentcommandid == this.cachedcommand.Count - 1)
            {
                this.inputchat.text = this.cachedcommand[this.currentcommandid];
                this.inputchat.ActivateInputField();
                this.inputchat.MoveTextEnd(false);
            }
        }
    }

    public bool IsOpen
    {
        get
        {
            return MainPlayerSkillHolder.Instance.skillAttackAutoAttack.AutoAttackState;
        }
    }

    public void SetAutoAttackState()
    {
        SkillAttackAutoAttack skillAttackAutoAttack = MainPlayerSkillHolder.Instance.skillAttackAutoAttack;
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.SwitchAutoAttack(!skillAttackAutoAttack.AutoAttackState);
        }
    }

    private InputField inputchat;

    private robot mrobot;

    private int currentcommandid = -1;

    public List<string> cachedcommand = new List<string>();
}
