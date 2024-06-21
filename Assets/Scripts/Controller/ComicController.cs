using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;

public class ComicController : ControllerBase
{
    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_ComicEndStartAI));
        this.comicNetWork = new ComicNetWork();
        this.comicNetWork.Initialize();
        this.ResetData();
    }

    public override string ControllerName
    {
        get
        {
            return "comic_controller";
        }
    }

    public void ShowComic(uint id, uint delay, string callback)
    {
        this.OnCallBack();
        this.comicID = id;
        this.onComicCallback = callback;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ShowComic));
        Scheduler.Instance.AddTimer(delay, false, new Scheduler.OnScheduler(this.ShowComic));
    }

    public void ShowComic()
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("comicdata").GetCacheField_Table("comicdata");
        LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(this.comicID.ToString());
        if (cacheField_Table2 != null)
        {
            this.comicName = cacheField_Table2.GetCacheField_String("uiname");
        }
        if (!string.IsNullOrEmpty(this.comicName))
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Comic>(this.comicName, delegate ()
            {
                if (UIManager.GetUIObject<UI_Comic>() != null)
                {
                    UIManager.GetUIObject<UI_Comic>().StartPlayComic(this.comicID);
                }
            }, UIManager.ParentType.Tips, true);
        }
        else
        {
            this.ResetData();
        }
    }

    public void OnCallBack()
    {
        if (!string.IsNullOrEmpty(this.onComicCallback))
        {
            LuaProcess.ProcessLua2CsFunction(this.onComicCallback);
        }
        this.CloseComic();
    }

    public void CloseComic()
    {
        if (!string.IsNullOrEmpty(this.comicName))
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this.comicName);
        }
        this.ResetData();
    }

    private void ResetData()
    {
        this.comicName = string.Empty;
        this.onComicCallback = string.Empty;
    }

    public void Luafun_ComicEndStartAI(List<VarType> paras)
    {
        if (paras.Count != 0)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.comicNetWork.ReqStartAIRunning();
    }

    private uint comicID;

    private string onComicCallback = string.Empty;

    private string comicName = string.Empty;

    private ComicNetWork comicNetWork;
}
