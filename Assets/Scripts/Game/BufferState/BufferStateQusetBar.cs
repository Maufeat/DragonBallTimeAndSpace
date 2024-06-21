using System;
using Framework.Managers;
using LuaInterface;

public class BufferStateQusetBar : BufferState
{
    public BufferStateQusetBar(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        CacheBufferInfo cacheInfo = PBControl.cacheInfo;
        if (cacheInfo != null && cacheInfo.flag == this.mFlag && this.mFlag == UserState.USTATE_QUESTBAR)
        {
            LuaTable luaTable = LuaScriptMgr.Instance.CallLuaFunction("GetEmptyTable", new object[0])[0] as LuaTable;
            luaTable["BuffAnim"] = cacheInfo.buffAnim;
            luaTable["RevertAnim"] = 0;
            luaTable["id"] = UserState.USTATE_QUESTBAR;
            base.CurrBuffConfig = luaTable;
        }
        if (PBControl.Owner == MainPlayer.Self)
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ClearMoveCachesData();
        }
        base.Enter(PBControl);
        PBControl.cacheInfo.Clear();
    }

    public override void Update(BufferServerDate newData, bool IsNewAdd)
    {
        base.Update(newData, IsNewAdd);
        if (this.m_onetimes)
        {
            this.m_onetimes = false;
            if (this.lstBuffData != null && this.lstBuffData.Count > 0)
            {
                for (int i = 0; i < this.lstBuffData.Count; i++)
                {
                    this.ShowProgressBar(this.lstBuffData[i].duartion / 1000f);
                }
            }
        }
    }

    private void ShowProgressBar(float length)
    {
        ProgressUIController controller = ControllerManager.Instance.GetController<ProgressUIController>();
        if (controller != null)
        {
            controller.ShowProgressBar(length, SliderDirection.LeftToRight, ProgressUIController.ProgressBarType.Normal, null);
        }
    }

    public override void Exit()
    {
        FFDebug.Log(this, FFLogType.Buff, "Exit........... BufferStateQusetBar");
        base.Exit();
        base.CurrBuffConfig["BuffAnim"] = 0;
        base.CurrBuffConfig["RevertAnim"] = 0;
    }

    private bool m_onetimes;
}
