using System;
using System.Collections.Generic;
using System.Text;
using Framework.Managers;
using ResoureManager;

public class LuaProcess
{
    public static void RegisertFunction(Action<List<VarType>> callback)
    {
        string name = callback.Method.Name;
        if (!LuaProcess.FunctionMap.ContainsKey(name))
        {
            LuaProcess.FunctionMap[name] = callback;
        }
    }

    public static void ProcessLua2CsFunction(string functioninfo)
    {
        if (string.IsNullOrEmpty(functioninfo))
        {
            FFDebug.LogWarning("LuaProcess", "Function information is empty!");
            return;
        }
        string[] array = functioninfo.Split(new char[]
        {
            ','
        });
        string text = array[0];
        if (!LuaProcess.FunctionMap.ContainsKey(text))
        {
            FFDebug.LogWarning("LuaProcess", "Dosent exit Method called " + text);
            return;
        }
        List<VarType> list = new List<VarType>();
        if (array.Length > 1)
        {
            for (int i = 1; i < array.Length; i++)
            {
                list.Add(new VarType(array[i]));
            }
        }
        LuaProcess.FunctionMap[text](list);
    }

    public static void ParserAndCallNpcLua(string luastr, ulong npcid, uint source)
    {
        if (TaskController.CurrnetNpcDlg != null)
        {
            TaskController.CurrnetNpcDlg.UnInit();
        }
        TaskController.CurNpcDlgSource = source;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("this = {}");
        stringBuilder.AppendLine("dlg = NpcTalkAndTaskDlgCtrl");
        luastr = luastr.Replace("[", "<");
        luastr = luastr.Replace("]", ">");
        luastr = luastr.Replace("{替换npcid}", npcid.ToString());
        stringBuilder.AppendLine(luastr);
        stringBuilder.Append("dlg:EndDlg()");
        string s = stringBuilder.ToString();
        string @string = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(s));
        LuaScriptMgr.Instance.CallLuaFunction("NpcTalkAndTaskDlgCtrl.CleanNpcTalk", new object[0]);
        LuaScriptMgr.Instance.DoString(@string);
    }

    public static void ParserAndCallNpcLua_NpcDlg()
    {
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(Util.LuaOppositePath("View/npcdlg.lua"), delegate (byte[] bytes)
        {
            string @string = Encoding.UTF8.GetString(bytes);
            if (TaskController.CurrnetNpcDlg != null)
            {
                TaskController.CurrnetNpcDlg.UnInit();
            }
            LuaScriptMgr.Instance.DoString(@string);
        });
    }

    public static void UnAllRegisertFunction()
    {
        LuaProcess.FunctionMap.Clear();
    }

    private static Dictionary<string, Action<List<VarType>>> FunctionMap = new Dictionary<string, Action<List<VarType>>>();
}
