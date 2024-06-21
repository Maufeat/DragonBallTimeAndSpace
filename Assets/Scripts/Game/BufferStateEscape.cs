using System;

public class BufferStateEscape : BufferState
{
    public BufferStateEscape(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (this.BufferControl != null && MainPlayer.Self != null && this.BufferControl.Owner.EID.Id == MainPlayer.Self.EID.Id)
        {
            LuaScriptMgr.Instance.CallLuaFunction("EscapeCtrl.ShowEscapeView", new object[]
            {
                Util.GetLuaTable("EscapeCtrl"),
                (int)this.mFlag
            });
        }
    }

    public override void Exit()
    {
        if (this.BufferControl != null && MainPlayer.Self != null && this.BufferControl.Owner.EID.Id == MainPlayer.Self.EID.Id)
        {
            LuaScriptMgr.Instance.CallLuaFunction("EscapeCtrl.TryCloseEscapeView", new object[]
            {
                Util.GetLuaTable("EscapeCtrl"),
                (int)this.mFlag
            });
        }
        base.Exit();
    }
}
