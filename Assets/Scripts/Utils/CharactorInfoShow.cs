using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class CharactorInfoShow
{
    public void Gui()
    {
        EditorSubWindow.OnGuiSubWin(ref this.winrect, "CharactorInfo", delegate ()
        {
            if (this.Char != null && this.Char.ModelObj != null)
            {
                this.ShowChar();
            }
            else
            {
                this.ShowList();
            }
        }, true, true);
    }

    private void ShowList()
    {
        this.Pos = GUILayout.BeginScrollView(this.Pos, new GUILayoutOption[0]);
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            MyEditorGUILayout.Button(pair.Value.EID.ToString(), delegate
            {
                this.Char = pair.Value;
                this.OneStepLogList.Clear();
            }, 0f, 0f);
        });
        manager.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            MyEditorGUILayout.Button(pair.Value.EID.ToString(), delegate
            {
                this.Char = pair.Value;
                this.OneStepLogList.Clear();
            }, 0f, 0f);
        });
        GUILayout.EndScrollView();
    }

    private void ShowChar()
    {
        GUILayout.BeginVertical(new GUILayoutOption[0]);
        MyEditorGUILayout.StringField("SeverPos: ", this.Char.NextPosition2D.ToString(), 260f);
        MyEditorGUILayout.StringField("ClientPos: ", GraphUtils.GetServerPosByWorldPos(this.Char.ModelObj.transform.position, true).ToString(), 260f);
        uint movespeed;
        if (this.Char is Npc)
        {
            movespeed = (this.Char as Npc).NpcData.MapNpcData.movespeed;
        }
        else
        {
            movespeed = (this.Char as OtherPlayer).OtherPlayerData.MapUserData.mapdata.movespeed;
        }
        MyEditorGUILayout.StringField("SeverSpeed: ", movespeed.ToString(), 260f);
        MyEditorGUILayout.StringField("ClientSpeed: ", this.Char.moveSpeed.ToString(), 260f);
        this.pos1 = GUILayout.BeginScrollView(this.pos1, new GUILayoutOption[0]);
        string text = string.Empty;
        for (int i = 0; i < this.OneStepLogList.Count; i++)
        {
            text = this.OneStepLogList[i] + "\n" + text;
        }
        MyEditorGUILayout.StringArea("FinishOneStepTime: ", text, 260f);
        GUILayout.EndScrollView();
        MyEditorGUILayout.Button("Back", delegate
        {
            this.Char = null;
        }, 0f, 0f);
        GUILayout.EndVertical();
    }

    private Rect winrect = new Rect(20f, 20f, 400f, 300f);

    private CharactorBase Char;

    private Vector2 Pos = Vector2.zero;

    private Vector2 pos1;

    private List<uint> OneStepLogList = new List<uint>();

    private float LastTime;
}
