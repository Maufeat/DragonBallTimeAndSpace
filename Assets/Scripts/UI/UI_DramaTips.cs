using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_DramaTips
{
    public void Init(Transform mainviewroot)
    {
        this.dramaTips = mainviewroot.Find("Offset_Main/Panel_Say").gameObject;
        this.dramaTipsName = this.dramaTips.transform.Find("RawImage/img_namebg").gameObject;
        this.dramaTipsText = this.dramaTips.transform.Find("img_txtbg/txt_say").GetComponent<Text>();
        this.picTips = this.dramaTips.transform.Find("RawImage").GetComponent<RawImage>();
        Button button = this.dramaTips.AddComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate ()
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DramaTipsGroupCheckNext));
            this.DramaTipsGroupCheckNext();
        });
        this.dramaTips.SetActive(false);
    }

    public void Dispose()
    {
    }

    public void ShowDramaTips(uint id)
    {
        if (ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups.ContainsKey(id))
        {
            this.dramaTips.SetActive(true);
            this.currentdramatipsgroup = id;
            this.currentdramatipsgroupindex = -1;
            this.DramaTipsGroupCheckNext();
        }
        else
        {
            FFDebug.LogWarning(this, "Does not contain drama group with id : " + id);
        }
    }

    private void CloseDramaTips()
    {
        this.ResetDramaTipsGroupData();
        for (int i = 0; i < this.imageSeperateWithAlphaList.Count; i++)
        {
            this.imageSeperateWithAlphaList[i].Dispose();
        }
        this.imageSeperateWithAlphaList.Clear();
        this.dramaTips.SetActive(false);
    }

    public void Show(bool show, uint npcId, uint dialogId)
    {
        if (this.dramaTips == null || this.picTips == null || this.dramaTipsText == null)
        {
            return;
        }
        this.dramaTips.SetActive(show);
        if (!show)
        {
            return;
        }
        GlobalRegister.ShowNpcOrPlayerRTT(this.picTips, npcId, 0, null);
        LuaTable configTable = LuaConfigManager.GetConfigTable("dialogueconfig", (ulong)dialogId);
        if (configTable == null)
        {
            return;
        }
        string field_String = configTable.GetField_String("dialogue");
        this.dramaTipsText.text = field_String;
    }

    private void AddDramaGroupTipsTalkByID(LuaTable config)
    {
        if (config != null)
        {
            this.CreatDramaTipsLabel(config.GetField_String("dialogue"));
            Scheduler.Instance.AddTimer(1.2f, false, new Scheduler.OnScheduler(this.DramaTipsGroupCheckNext));
            this.picTips.gameObject.SetActive(false);
            string field_String = config.GetField_String("IconHeroIDL");
            if (!string.IsNullOrEmpty(field_String))
            {
                if (field_String.ToLower().Equals("player"))
                {
                    GlobalRegister.ShowMainPlayerRTT(this.picTips, GlobalRegister.GetPlayerHeroID(0UL));
                    this.picTips.gameObject.SetActive(true);
                }
                else
                {
                    float num = 0f;
                    if (float.TryParse(field_String, out num))
                    {
                        GlobalRegister.ShowNpcOrPlayerRTT(this.picTips, (uint)num, 0, null);
                        this.picTips.gameObject.SetActive(true);
                    }
                }
            }
            string text = config.GetField_String("name");
            if (!string.IsNullOrEmpty(text) && text.ToLower().Equals("player"))
            {
                text = MainPlayer.Self.OtherPlayerData.MapUserData.name;
            }
            this.SetDramaTipsTalkName(text);
        }
        else
        {
            this.CloseDramaTips();
        }
    }

    private void DramaTipsGroupCheckNext()
    {
        if (this.currentdramatipsgroup == 0U)
        {
            this.CloseDramaTips();
            FFDebug.LogWarning(this, "CheckNext currentdramagroup == 0");
            return;
        }
        if (!ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups.ContainsKey(this.currentdramatipsgroup))
        {
            FFDebug.LogWarning(this, "!DramaGroups.ContainsKey(currentdramagroup.ToString())");
            this.CloseDramaTips();
            return;
        }
        this.currentdramatipsgroupindex++;
        if (this.currentdramatipsgroupindex == ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups[this.currentdramatipsgroup].Count)
        {
            this.CloseDramaTips();
            return;
        }
        LuaTable luaTable = ControllerManager.Instance.GetController<UINpcDlgController>().DramaGroups[this.currentdramatipsgroup][this.currentdramatipsgroupindex];
        if (luaTable == null)
        {
            FFDebug.LogWarning(this, "config == null");
            this.CloseDramaTips();
            return;
        }
        this.AddDramaGroupTipsTalkByID(luaTable);
    }

    private void ResetDramaTipsGroupData()
    {
        this.currentdramatipsgroup = 0U;
        this.currentdramatipsgroupindex = -1;
    }

    private void CreatDramaTipsLabel(string content)
    {
        this.dramaTipsText.text = content;
    }

    private void SetDramaTipsTalkName(string configname)
    {
        if (MainPlayer.Self == null)
        {
            this.dramaTipsName.SetActive(false);
            return;
        }
        if (!string.IsNullOrEmpty(configname))
        {
            if (configname == "Player")
            {
                this.dramaTipsName.transform.Find("txt_name").GetComponent<Text>().text = MainPlayer.Self.OtherPlayerData.MapUserData.name;
            }
            else
            {
                this.dramaTipsName.transform.Find("txt_name").GetComponent<Text>().text = configname;
            }
            this.dramaTipsName.SetActive(true);
        }
        else
        {
            this.dramaTipsName.SetActive(false);
        }
    }

    private GameObject dramaTips;

    private GameObject dramaTipsName;

    private Text dramaTipsText;

    private RawImage picTips;

    private uint currentdramatipsgroup;

    private int currentdramatipsgroupindex = -1;

    private List<ImageSeperateWithAlpha> imageSeperateWithAlphaList = new List<ImageSeperateWithAlpha>();
}
