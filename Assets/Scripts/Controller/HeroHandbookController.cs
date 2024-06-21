using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;

public class HeroHandbookController : ControllerBase
{
    private UI_HeroHandbook mUiHeroHandbook
    {
        get
        {
            return UIManager.GetUIObject<UI_HeroHandbook>();
        }
    }

    private UI_Character mUICharacter
    {
        get
        {
            return UIManager.GetUIObject<UI_Character>();
        }
    }

    public override void Awake()
    {
        this.mCharacterNetwork = new CharacterNetwork();
        this.mCharacterNetwork.Initialize();
        base.Awake();
    }

    public int GetMainHeroId()
    {
        return this.mMainHeroId;
    }

    public int GetSelfCreateHeroId()
    {
        Dictionary<int, LuaTable> ownHeroIdList = this.GetOwnHeroIdList();
        foreach (LuaTable luaTable in ownHeroIdList.Values)
        {
            if (luaTable.GetField_Bool("self_created"))
            {
                return luaTable.GetField_Int("baseid");
            }
        }
        return 0;
    }

    public int GetSelfCreateHeroLevel()
    {
        Dictionary<int, LuaTable> ownHeroIdList = this.GetOwnHeroIdList();
        foreach (LuaTable luaTable in ownHeroIdList.Values)
        {
            if (luaTable.GetField_Bool("self_created"))
            {
                return luaTable.GetField_Int("level");
            }
        }
        return 0;
    }

    public void SwitchHero()
    {
        ulong needOnlineThisid = this.GetNeedOnlineThisid();
        if (needOnlineThisid == 0UL)
        {
            TipsWindow.ShowWindow(2101U);
            return;
        }
        this.OnlineHero(needOnlineThisid);
    }

    public void OnlineHero(ulong needOnlineThisid)
    {
        if (MainPlayer.Self.IsBattleState)
        {
            TipsWindow.ShowWindow(1417U);
            return;
        }
        MainPlayer.Self.StopMoveImmediate(null);
        GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
        if (controller.GetMyGuildId() == 0UL)
        {
            ControllerManager.Instance.GetController<ProgressUIController>().ShowProgressBar((float)Constant.SWITCH_HERO_PROGRESS_SECOND, delegate ()
            {
                LuaScriptMgr.Instance.CallLuaFunction("HerosNetwork.CReqSwitchHero_CS", new object[]
                {
                    needOnlineThisid.ToString()
                });
            });
        }
        else
        {
            controller.TryGetFastSwitchSkillBufDat(delegate (float ratio)
            {
                ControllerManager.Instance.GetController<ProgressUIController>().ShowProgressBar((float)Constant.SWITCH_HERO_PROGRESS_SECOND * ratio, delegate ()
                {
                    LuaScriptMgr.Instance.CallLuaFunction("HerosNetwork.CReqSwitchHero_CS", new object[]
                    {
                        needOnlineThisid.ToString()
                    });
                });
            });
        }
    }

    public void SetMainHero(ulong thisid, bool isOnLine)
    {
        MainPlayer.Self.StopMoveImmediate(null);
        ControllerManager.Instance.GetController<ProgressUIController>().ShowProgressBar((float)Constant.SWITCH_HERO_PROGRESS_SECOND, delegate ()
        {
            this.mCharacterNetwork.ReqSetMainHero(thisid);
            if (isOnLine)
            {
                LuaScriptMgr.Instance.CallLuaFunction("HerosNetwork.CReqSwitchHero_CS", new object[]
                {
                    thisid.ToString()
                });
            }
        });
    }

    public ulong GetNeedOnlineThisid()
    {
        uint heroid = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.heroid;
        int selfCreateHeroId = this.GetSelfCreateHeroId();
        if ((ulong)heroid == (ulong)((long)selfCreateHeroId))
        {
            LuaTable heroDataByHeroId = this.GetHeroDataByHeroId(this.mMainHeroId);
            if (heroDataByHeroId != null)
            {
                return ulong.Parse(heroDataByHeroId.GetField_String("thisid"));
            }
        }
        else
        {
            LuaTable heroDataByHeroId2 = this.GetHeroDataByHeroId(selfCreateHeroId);
            if (heroDataByHeroId2 != null)
            {
                return ulong.Parse(heroDataByHeroId2.GetField_String("thisid"));
            }
        }
        return 0UL;
    }

    public string GetOnlineHeroThisId()
    {
        return MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid;
    }

    public LuaTable GetHeroDataByHeroId(int heroId)
    {
        Dictionary<int, LuaTable> ownHeroIdList = this.GetOwnHeroIdList();
        if (ownHeroIdList.ContainsKey(heroId))
        {
            return ownHeroIdList[heroId];
        }
        return null;
    }

    public Dictionary<int, LuaTable> GetOwnHeroIdList()
    {
        Dictionary<int, LuaTable> dictionary = new Dictionary<int, LuaTable>();
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.GetOwnHeros", new object[0]);
        LuaTable luaTable = array[0] as LuaTable;
        foreach (object obj in luaTable.Values)
        {
            LuaTable luaTable2 = obj as LuaTable;
            if (!dictionary.ContainsKey(luaTable2.GetField_Int("baseid")))
            {
                bool field_Bool = luaTable2.GetField_Bool("self_created");
                dictionary.Add(luaTable2.GetField_Int("baseid"), luaTable2);
            }
        }
        return dictionary;
    }

    public void OpenUI()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            TipsWindow.ShowWindow(5001U);
            return;
        }
        if (UIManager.GetUIObject<UI_HeroHandbook>() != null)
        {
            UIManager.Instance.DeleteUI<UI_HeroHandbook>();
        }
        else
        {
            HeroAwakeController controller = ControllerManager.Instance.GetController<HeroAwakeController>();
            controller.reqEvolutionData = true;
            if (this.mUiHeroHandbook == null)
            {
                UIManager.Instance.ShowUI<UI_HeroHandbook>("UI_HeroPokedex", delegate ()
                {
                    this.mCharacterNetwork.ReqGetMainHero();
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                this.mCharacterNetwork.ReqGetMainHero();
            }
        }
    }

    public bool IsMainHeroOnline()
    {
        if (MainPlayer.Self == null || MainPlayer.Self.OtherPlayerData == null)
        {
            return false;
        }
        uint heroid = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.heroid;
        return (ulong)heroid != (ulong)((long)this.GetSelfCreateHeroId());
    }

    public int GetHeroIdByThisid(ulong thisid)
    {
        Dictionary<int, LuaTable> ownHeroIdList = this.GetOwnHeroIdList();
        foreach (LuaTable luaTable in ownHeroIdList.Values)
        {
            if (luaTable.GetField_String("thisid") == thisid.ToString())
            {
                return luaTable.GetField_Int("baseid");
            }
        }
        return 0;
    }

    public void SetupMainHero(ulong mainHeroThisid)
    {
        this.mMainHeroId = this.GetHeroIdByThisid(mainHeroThisid);
        if (this.mUiHeroHandbook != null)
        {
            this.mUiHeroHandbook.SetupInfo();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string ControllerName
    {
        get
        {
            return "herohandbook_controller";
        }
    }

    public CharacterNetwork mCharacterNetwork;

    public int mMainHeroId;

    public int DUTIAO_TIME = 5;
}
