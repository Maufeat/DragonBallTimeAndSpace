using System;
using UnityEngine;

public class AreaMusicTool : MonoBehaviour
{
    public static AreaMusicTool Instance
    {
        get
        {
            return AreaMusicTool.instance;
        }
    }

    private void Awake()
    {
        AreaMusicTool.instance = this;
    }

    public void UpdateMusic(Vector2 serverpos)
    {
        byte b = 0;
        if (MapLoader.areaMusicInfo != null)
        {
            Vector2 vector = serverpos;
            if (MapLoader.areaMusicInfo.isSame)
            {
                b = MapLoader.areaMusicInfo.nodes[0];
            }
            else
            {
                float f = vector.x / 3f;
                float f2 = vector.y / 3f;
                int w = Mathf.FloorToInt(f);
                int h = Mathf.FloorToInt(f2);
                b = MapLoader.areaMusicInfo.GetValueByIndex(w, h);
            }
        }
        if (this.curMusicFlag == b)
        {
            return;
        }
        this.curMusicFlag = b;
        if (!AkSoundEngine.IsInitialized())
        {
            return;
        }
        this.PostMusic(b.ToString());
    }

    public void PostMusic(string eventName)
    {
    }

    private void OnDestroy()
    {
    }

    private static AreaMusicTool instance;

    private readonly string defaultGroupName = "AreaMusic";

    private byte curMusicFlag;
}
