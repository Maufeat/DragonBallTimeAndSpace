using magic;

public interface ISkillHolder : IFFComponent
{
    void StartDisplaySkill(MSG_Ret_StartMagicAttack_SC Data);

    void DisplaySkillStage(MSG_Ret_SyncSkillStage_SC Data);

    void HandleBreakSkill(MSG_Ret_InterruptSkill_SC Data);
}
