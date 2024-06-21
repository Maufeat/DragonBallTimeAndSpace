using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class TipsWindow
{
    public static TipsUI tipsUI
    {
        get
        {
            return UIManager.GetUIObject<TipsUI>();
        }
    }

    public static void InitWindow()
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<TipsUI>("UI_Tips", delegate ()
        {
            while (TipsWindow.loadUIFinish.Count > 0)
            {
                TipsWindow.loadUIFinish.Dequeue()();
            }
        }, UIManager.ParentType.Tips, false);
    }

    public static void ShowWindow(string contain)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(contain))
        {
            return;
        }
        TipsWindow.ShowWindow(new NoticeModel
        {
            texEffectColor = Const.TextColorTipsRed,
            content = contain
        });
    }

    public static void ShowWindow(uint textid)
    {
        TipsWindow.ShowWindow(textid, null);
    }

    public static void ShowWindow(uint textid, string[] args)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", (ulong)textid);
        if (configTable == null)
        {
            FFDebug.LogWarning("TipsWindow", "Does exist text id " + textid);
            return;
        }
        string str_chat = string.Empty;
        if (configTable.GetField_Uint("texttype") == 1U)
        {
            str_chat = configTable.GetField_String("tips");
        }
        else if (args == null)
        {
            str_chat = configTable.GetField_String("notice");
        }
        else if (args.Length == 0)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(configTable.GetField_String("notice"));
        }
        else if (args.Length == 1)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), args[0]));
        }
        else if (args.Length == 2)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), args[0], args[1]));
        }
        else if (args.Length == 3)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), args[0], args[1], args[2]));
        }
        else if (args.Length == 4)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3]
            }));
        }
        else if (args.Length == 5)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4]
            }));
        }
        else if (args.Length == 6)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4],
                args[5]
            }));
        }
        else if (args.Length == 7)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4],
                args[5],
                args[6]
            }));
        }
        else if (args.Length == 8)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4],
                args[5],
                args[6],
                args[7]
            }));
        }
        else
        {
            FFDebug.LogWarning("TipsWindow", "Too much args in Text Error!!!");
        }
        MSG_Ret_ChannelChat_SC msg_Ret_ChannelChat_SC = new MSG_Ret_ChannelChat_SC();
        msg_Ret_ChannelChat_SC.textid = textid;
        msg_Ret_ChannelChat_SC.str_chat = str_chat;
        msg_Ret_ChannelChat_SC.channel_type = (ChannelType)configTable.GetField_Uint("channeltype");
        string text = string.Empty;
        if (MainPlayer.Self != null)
        {
            text = MainPlayer.Self.GetMainPlayerName();
            msg_Ret_ChannelChat_SC.src_name = MainPlayer.Self.OtherPlayerData.MapUserData.name;
        }
        ControllerManager.Instance.GetController<ChatControl>().OnReceiveChatMsg(msg_Ret_ChannelChat_SC);
    }

    public static void ShowWindow(TipsType type, string[] args = null)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", (ulong)type);
        if (configTable == null)
        {
            FFDebug.LogWarning("TipsWindow", string.Format("Does exist text  type ={0}, id = {1}", type, (uint)type));
            return;
        }
        string str_chat = string.Empty;
        if (configTable.GetField_Uint("texttype") == 1U)
        {
            str_chat = configTable.GetField_String("tips");
        }
        else if (args == null)
        {
            str_chat = configTable.GetField_String("notice");
        }
        else if (args.Length == 0)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(configTable.GetField_String("notice"));
        }
        else if (args.Length == 1)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), args[0]));
        }
        else if (args.Length == 2)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), args[0], args[1]));
        }
        else if (args.Length == 3)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), args[0], args[1], args[2]));
        }
        else if (args.Length == 4)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3]
            }));
        }
        else if (args.Length == 5)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4]
            }));
        }
        else if (args.Length == 6)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4],
                args[5]
            }));
        }
        else if (args.Length == 7)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4],
                args[5],
                args[6]
            }));
        }
        else if (args.Length == 8)
        {
            str_chat = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(configTable.GetField_String("notice"), new object[]
            {
                args[0],
                args[1],
                args[2],
                args[3],
                args[4],
                args[5],
                args[6],
                args[7]
            }));
        }
        else
        {
            FFDebug.LogWarning("TipsWindow", "Too much args in Text Error!!!");
        }
        MSG_Ret_ChannelChat_SC msg_Ret_ChannelChat_SC = new MSG_Ret_ChannelChat_SC();
        msg_Ret_ChannelChat_SC.textid = (uint)type;
        if (MainPlayer.Self == null)
        {
            msg_Ret_ChannelChat_SC.src_name = string.Empty;
        }
        else
        {
            msg_Ret_ChannelChat_SC.src_name = MainPlayer.Self.OtherPlayerData.MapUserData.name;
        }
        msg_Ret_ChannelChat_SC.str_chat = str_chat;
        ControllerManager.Instance.GetController<ChatControl>().OnReceiveChatMsg(msg_Ret_ChannelChat_SC);
        string text = string.Empty;
        if (MainPlayer.Self != null)
        {
            text = MainPlayer.Self.GetMainPlayerName();
        }
    }

    public static void ShowWindow(string contain, Color color)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(contain))
        {
            return;
        }
        TipsWindow.ShowWindow(new NoticeModel
        {
            texEffectColor = color,
            content = contain
        });
    }

    public static void ShowWindow(NoticeModel contain)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(contain.content))
        {
            return;
        }
        TipsWindow.tipsContain = contain;
        if (TipsWindow.tipsUI == null)
        {
            TipsWindow.loadUIFinish.Enqueue(delegate
            {
                if (TipsWindow.tipsUI != null)
                {
                    TipsWindow.tipsUI.ShowTips(TipsWindow.tipsContain);
                }
            });
            TipsWindow.InitWindow();
        }
        else
        {
            TipsWindow.tipsUI.ShowTips(contain);
        }
    }

    public static void ShowNotice(string content)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        TipsWindow.ShowNotice(new NoticeModel
        {
            content = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(content)
        });
    }

    public static void ShowNotice(uint index)
    {
        string text = string.Empty;
        LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", (ulong)index);
        if (configTable != null)
        {
            text = configTable.GetCacheField_String("tips");
        }
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        TipsWindow.ShowNotice(new NoticeModel
        {
            content = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(text)
        });
    }

    public static void ShowNotice(NoticeModel tmpModel)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(tmpModel.content))
        {
            return;
        }
        TipsWindow.noticeContain.Enqueue(tmpModel);
        if (TipsWindow.tipsUI == null)
        {
            TipsWindow.loadUIFinish.Enqueue(delegate
            {
                if (TipsWindow.tipsUI != null)
                {
                    TipsWindow.tipsUI.ShowNotice();
                }
            });
            TipsWindow.InitWindow();
        }
        else
        {
            TipsWindow.tipsUI.ShowNotice();
        }
    }

    public static void ShowTaskTips(string content)
    {
        if (!TipsWindow.CheckCanShowTips())
        {
            return;
        }
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        if (TipsWindow.tipsUI == null)
        {
            TipsWindow.loadUIFinish.Enqueue(delegate
            {
                if (TipsWindow.tipsUI != null)
                {
                    TipsWindow.tipsUI.ShowTaskTips(content);
                }
            });
            TipsWindow.InitWindow();
        }
        else
        {
            TipsWindow.tipsUI.ShowTaskTips(content);
        }
    }

    private static bool CheckCanShowTips()
    {
        if (UIManager.GetUIObject<UI_CompleteCopy>())
        {
            return false;
        }
        if (CameraController.Self != null)
        {
            BossDieCameraMove bossDieCameraMove = CameraController.Self.CurrStatebyType<BossDieCameraMove>();
            if (bossDieCameraMove != null)
            {
                return false;
            }
        }
        return true;
    }

    public static Queue<NoticeModel> noticeContain = new Queue<NoticeModel>();

    private static Queue<Action> loadUIFinish = new Queue<Action>();

    private static NoticeModel tipsContain;
}
