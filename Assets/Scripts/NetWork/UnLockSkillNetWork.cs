using System;
using Framework.Managers;
using hero;
using Net;

public class UnLockSkillNetWork : NetWorkBase
{
    private UnLockSkillsController lsc
    {
        get
        {
            if (this._lsc == null)
            {
                this._lsc = ControllerManager.Instance.GetController<UnLockSkillsController>();
            }
            return this._lsc;
        }
    }

    public void OnRetMSG_LevelupHeroSkill_SC(string herothisid, uint skillbaseid, uint skilllevel, uint skillorgid)
    {
        this.lsc.RetLevelUpSkill(skillbaseid, skilllevel);
    }

    public void OnReqMSG_LevelupHeroSkill_CS(string herothisid, uint skillid, uint skillLv)
    {
        base.SendMsg<MSG_ReqLevelupHeroSkill_CS>(CommandID.MSG_ReqLevelupHeroSkill_CS, new MSG_ReqLevelupHeroSkill_CS
        {
            herothisid = herothisid,
            skillbaseid = skillid,
            skilllevel = skillLv
        }, false);
    }

    private UnLockSkillsController _lsc;
}
