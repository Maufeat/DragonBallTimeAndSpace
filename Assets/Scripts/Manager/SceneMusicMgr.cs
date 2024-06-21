using System;
using Framework.Base;
using LuaInterface;

public class SceneMusicMgr : IManager
{
    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnReSet()
    {
        this.scenesAudioinfo = null;
    }

    public void OnUpdate()
    {
    }

    public void OnChangeScene(uint mapid)
    {
        if (this.Lastmapid == mapid)
        {
            return;
        }
        if (this.scenesAudioinfo == null)
        {
            this.scenesAudioinfo = LuaConfigManager.GetXmlConfigTable("scenesAudioinfo");
        }
        if (this.scenesAudioinfo.GetCacheField_Table("mapinfo").GetCacheField_Table(mapid.ToString()) == null)
        {
            return;
        }
        this.Lastmapid = mapid;
    }

    public void OnMapTourStateChange(uint mapid, bool isStart)
    {
        if (this.scenesAudioinfo == null)
        {
            this.scenesAudioinfo = LuaConfigManager.GetXmlConfigTable("scenesAudioinfo");
        }
        if (this.scenesAudioinfo.GetCacheField_Table("mapinfo").GetCacheField_Table(mapid.ToString()) == null)
        {
            FFDebug.Log("SecenMusicMgr", FFLogType.MapTour, string.Concat(new string[]
            {
                "OnMapTourStateChange cant find audioinfo !!! id = " + mapid
            }));
            return;
        }
    }

    private string LastBgMusic = string.Empty;

    private uint Lastmapid;

    private LuaTable scenesAudioinfo;
}
