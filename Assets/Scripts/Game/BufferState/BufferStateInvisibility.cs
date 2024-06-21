using System;

public class BufferStateInvisibility : BufferState
{
    public BufferStateInvisibility(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            MainPlayer mainPlayer = this.BufferControl.Owner as MainPlayer;
            if (mainPlayer != null)
            {
                mainPlayer.GetComponent<FFMaterialEffectControl>().ResetAllMaterialData();
                mainPlayer.GetComponent<FFMaterialEffectControl>().SetRoleFloat("_Alpha", 0.2f);
                mainPlayer.GetComponent<FFMaterialEffectControl>().SetWeaponFloat("_Alpha", 0.2f);
                mainPlayer.GetComponent<FFMaterialEffectControl>().CloseCastShadow();
                mainPlayer.GetComponent<FFMaterialEffectControl>().IsInInvisibilityBuffState = true;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            MainPlayer mainPlayer = this.BufferControl.Owner as MainPlayer;
            if (mainPlayer != null)
            {
                mainPlayer.GetComponent<FFMaterialEffectControl>().SetRoleDefaultFloat("_Alpha");
                mainPlayer.GetComponent<FFMaterialEffectControl>().SetWeaponDefaultFloat("_Alpha");
                mainPlayer.GetComponent<FFMaterialEffectControl>().OpenCastShadow();
                mainPlayer.GetComponent<FFMaterialEffectControl>().IsInInvisibilityBuffState = false;
            }
        }
    }
}
