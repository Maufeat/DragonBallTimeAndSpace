using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_VisitNpcInMainView
{
    public void Init(Transform mainviewroot)
    {
        if (MainPlayer.Self != null)
        {
            FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
            if (component != null)
            {
                component.RefrashInRangeNpcTopButton();
            }
        }
    }

    public void Dispose()
    {
    }

    private void OnClickVisitNpc()
    {
        UIManager.GetUIObject<UI_MainView>().ShorcutVisitRecommendNpc();
    }

    public void ShowOrCloseBtnVisit(ulong npcid)
    {
        this.ShowNearestNpcTopButtom(npcid);
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetNpc(npcid);
        if (npc != null)
        {
            if (npc is Npc_TaskCollect && !(npc as Npc_TaskCollect).CheckStateContainDoing())
            {
                this.ShowNormalUI();
                npc.CloseTopBtn(true);
                return;
            }
            if (!npc.CheckIsShowByTask(false))
            {
                this.ShowNormalUI();
                npc.CloseTopBtn(true);
                return;
            }
            this.ShowVisitUI();
            LuaTable cacheField_Table = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(npc.NpcData.MapNpcData.baseid);
            if (cacheField_Table != null)
            {
                string cacheField_String = cacheField_Table.GetCacheField_String("visitbtn");
                if (!string.IsNullOrEmpty(cacheField_String))
                {
                    ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ICON, cacheField_String, delegate (UITextureAsset item)
                    {
                        if (item != null)
                        {
                        }
                    });
                }
                return;
            }
        }
        this.ShowNormalUI();
    }

    public void ShowNormalUI()
    {
    }

    public void ShowVisitUI()
    {
    }

    public void SetBattleVisitButtonLastSlibling()
    {
    }

    public void ShowNearestNpcTopButtom(ulong id)
    {
        ManagerCenter.Instance.GetManager<EntitiesManager>().NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            if (pair.Key == id)
            {
                if (pair.Value is Npc_TaskCollect)
                {
                    bool flag = (pair.Value as Npc_TaskCollect).CheckStateContainDoing();
                    if (flag)
                    {
                        pair.Value.ShowTopBtn();
                    }
                }
                else
                {
                    pair.Value.ShowTopBtn();
                }
            }
            else
            {
                pair.Value.CloseTopBtn(true);
            }
        });
    }

    private GameObject btn_skillnormal;

    private Button btn_battlevisitnpc;

    private Button btn_peacevisitnpc;

    private GameObject img_peacenormalicon;

    private GameObject img_battlenormalicon;

    private RawImage img_peacespecialicon;

    private RawImage img_battlespecialicon;

    private Vector3 btn_battlepos = Vector3.zero;

    private Vector3 btn_peacepos = Vector3.zero;
}
