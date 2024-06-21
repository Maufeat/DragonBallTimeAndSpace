using System;
using Framework.Managers;

public class MainPlayerData : IFFComponent
{
    public CompnentState State { get; set; }

    public void RefreshCharacterBaseData(cs_CharacterBaseData baseData)
    {
        this.CharacterBaseData = baseData;
        this.RefreshPKModel(baseData.pkmode);
    }

    public void ListenCharBaseDataChange(Action action, bool isListenOrUnListen)
    {
        if (action != null)
        {
            if (isListenOrUnListen)
            {
                this.onCharacterBaseDataChange = (Action)Delegate.Combine(this.onCharacterBaseDataChange, action);
            }
            else
            {
                this.onCharacterBaseDataChange = (Action)Delegate.Remove(this.onCharacterBaseDataChange, action);
            }
        }
    }

    public void RefreshAttributeData(cs_AttributeData attributeData)
    {
        this.AttributeData = attributeData;
        PlayerAttributeInfo.Instance.RefreshPlayerInfo();
        this.RefreshMp(attributeData.mp);
        this.RefreshHp(attributeData.hp, attributeData.maxhp);
    }

    public void RefreshFightData(cs_CharacterFightData fightData)
    {
        if (this.FightData != null)
        {
            PlayerAttributeInfo.Instance.OnFightValueChange(this.FightData.curfightvalue, fightData.curfightvalue);
        }
        this.FightData = fightData;
        PlayerAttributeInfo.Instance.RefreshPlayerInfo();
        LuaScriptMgr.Instance.CallLuaFunction("UI_HeroInfo.RefreshFightValue", new object[]
        {
            Util.GetLuaTable("UI_HeroInfo")
        });
    }

    public void RefreshPKModel(uint newmode)
    {
        this.CharacterBaseData.pkmode = newmode;
    }

    public void RefreshHp(uint hp, uint maxhp)
    {
        this.AttributeData.hp = hp;
        if (maxhp > 0U)
        {
            this.AttributeData.maxhp = maxhp;
        }
        ControllerManager.Instance.GetController<MainUIController>().ResetMainPlayerHp(hp, this.AttributeData.maxhp);
        if (MainPlayer.Self != null && MainPlayer.Self.hpdata != null)
        {
            MainPlayer.Self.hpdata.ResetHp(hp, this.AttributeData.maxhp);
        }
    }

    public void RefreshMp(uint mp)
    {
        this.AttributeData.mp = mp;
        CallLuaListener.SendLuaEvent("OnMainPlayerEnergyChangeLuaListener", false, new object[]
        {
            mp
        });
        ControllerManager.Instance.GetController<MainUIController>().RefreshMainPlayerMp();
    }

    public void RefreshExp(uint curExp, uint curLevel)
    {
        try
        {
            if (this.CharacterBaseData != null)
            {
                uint num = (uint)this.CharacterBaseData.exp;
                uint level = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level;
                uint num2 = 0U;
                if (curLevel == level)
                {
                    if (curExp > num)
                    {
                        num2 = curExp - num;
                        if (this.Owner != null && this.Owner.hpdata != null)
                        {
                            this.Owner.hpdata.ShowExpChange((int)num2);
                        }
                    }
                }
                else if (curLevel > level)
                {
                    MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
                    for (uint num3 = level; num3 < curLevel; num3 += 1U)
                    {
                        uint num4;
                        bool flag = controller.TryGetLevelAllExp(num3, out num4);
                        if (flag)
                        {
                            num2 += num4;
                        }
                    }
                    num2 -= num;
                    num2 += curExp;
                    if (this.Owner != null && this.Owner.hpdata != null)
                    {
                        this.Owner.hpdata.ShowExpChange((int)num2);
                    }
                }
                this.CharacterBaseData.exp = (ulong)curExp;
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, ex.Message);
        }
    }

    public void RefreshCurrencyToday(uint blue, uint purple)
    {
        this.CharacterBaseData.bluecrystalincnum = blue;
        this.CharacterBaseData.bluecrystalincnum = purple;
    }

    public void RefreshCurrency(uint id, uint changeNum, bool isAdd, uint todayget)
    {
        if (this.CharacterBaseData == null)
        {
            return;
        }
        switch (id)
        {
            case 2U:
                if (isAdd)
                {
                    this.CharacterBaseData.Stone += changeNum;
                }
                else if (this.CharacterBaseData.Stone >= changeNum)
                {
                    this.CharacterBaseData.Stone -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.Stone = 0U;
                }
                goto IL_404;
            case 3U:
                if (isAdd)
                {
                    this.CharacterBaseData.Money += changeNum;
                }
                else if (this.CharacterBaseData.Money >= changeNum)
                {
                    this.CharacterBaseData.Money -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.Money = 0U;
                }
                goto IL_404;
            case 4U:
                if (isAdd)
                {
                    this.CharacterBaseData.WelPoint += changeNum;
                }
                else if (this.CharacterBaseData.WelPoint >= changeNum)
                {
                    this.CharacterBaseData.WelPoint -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.WelPoint = 0U;
                }
                goto IL_404;
            case 11U:
                if (isAdd)
                {
                    this.CharacterBaseData.familyatt += changeNum;
                }
                else if (this.CharacterBaseData.familyatt >= changeNum)
                {
                    this.CharacterBaseData.familyatt -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.familyatt = 0U;
                }
                goto IL_404;
            case 13U:
                this.CharacterBaseData.bluecrystalincnum = todayget;
                if (isAdd)
                {
                    this.CharacterBaseData.bluecrystal += changeNum;
                }
                else if (this.CharacterBaseData.bluecrystal >= changeNum)
                {
                    this.CharacterBaseData.bluecrystal -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.bluecrystal = 0U;
                }
                goto IL_404;
            case 14U:
                this.CharacterBaseData.purplecrystalincnum = todayget;
                if (isAdd)
                {
                    this.CharacterBaseData.purplecrystal += changeNum;
                }
                else if (this.CharacterBaseData.purplecrystal >= changeNum)
                {
                    this.CharacterBaseData.purplecrystal -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.purplecrystal = 0U;
                }
                goto IL_404;
            case 15U:
                if (isAdd)
                {
                    this.CharacterBaseData.EduPoint += changeNum;
                }
                else if (this.CharacterBaseData.EduPoint >= changeNum)
                {
                    this.CharacterBaseData.EduPoint -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.EduPoint = 0U;
                }
                goto IL_404;
            case 16U:
                if (isAdd)
                {
                    this.CharacterBaseData.cooppoint += changeNum;
                }
                else if (this.CharacterBaseData.cooppoint >= changeNum)
                {
                    this.CharacterBaseData.cooppoint -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.cooppoint = 0U;
                }
                goto IL_404;
            case 17U:
                if (isAdd)
                {
                    this.CharacterBaseData.vigourpoint += changeNum;
                }
                else if (this.CharacterBaseData.vigourpoint >= changeNum)
                {
                    this.CharacterBaseData.vigourpoint -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.vigourpoint = 0U;
                }
                goto IL_404;
            case 18U:
                if (isAdd)
                {
                    this.CharacterBaseData.doublepoint += changeNum;
                }
                else if (this.CharacterBaseData.doublepoint >= changeNum)
                {
                    this.CharacterBaseData.doublepoint -= changeNum;
                }
                else
                {
                    this.CharacterBaseData.doublepoint = 0U;
                }
                goto IL_404;
        }
        FFDebug.LogError(this, string.Format("RefreshCurrency Unknow currency type !!! id = {0}", id));
    IL_404:
        if (this.onCharacterBaseDataChange != null)
        {
            this.onCharacterBaseDataChange();
        }
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    public CharactorBase Owner;

    public cs_CharacterBaseData CharacterBaseData;

    public cs_AttributeData AttributeData;

    public cs_CharacterFightData FightData;

    private Action onCharacterBaseDataChange;
}
