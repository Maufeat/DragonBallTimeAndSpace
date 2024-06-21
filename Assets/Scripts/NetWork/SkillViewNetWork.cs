using System;
using System.Collections.Generic;
using career;
using Framework.Managers;
using magic;
using Net;

public class SkillViewNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    private SkillViewControll skillViewController
    {
        get
        {
            return ControllerManager.Instance.GetController<SkillViewControll>();
        }
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetCareerSkillInfo_SC>(2109, new ProtoMsgCallback<MSG_RetCareerSkillInfo_SC>(this.RetSkillInfo));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUpgradeExtSkill_SC>(2113, new ProtoMsgCallback<MSG_RetUpgradeExtSkill_SC>(this.RetUpGradeSkill_SC));
    }

    public void ReqUpGradeSkill(uint id)
    {
        base.SendMsg<MSG_ReqUpgradeExtSkill_CS>(CommandID.MSG_ReqUpgradeExtSkill_CS, new MSG_ReqUpgradeExtSkill_CS
        {
            extskillid = id
        }, false);
    }

    public void RetUpGradeSkill_SC(MSG_RetUpgradeExtSkill_SC msgInfo)
    {
        FFDebug.LogWarning(this, "  UpGradeSkill   return " + msgInfo.errcode);
        if (msgInfo.errcode == 0U)
        {
            this.skillViewController.RetUpGradeSkill_SC();
        }
    }

    public void ReqOpenSkillView()
    {
        base.SendMsg<MSG_ReqCareerSkillInfo_CS>(CommandID.MSG_ReqCareerSkillInfo_CS, new MSG_ReqCareerSkillInfo_CS(), false);
    }

    public void RetSkillInfo(MSG_RetCareerSkillInfo_SC msgInfo)
    {
        this.skillViewController.SetSkillData(msgInfo.skillinfo);
        this.skillTmpList.Clear();
        for (int i = 0; i < msgInfo.skillinfo.unlockskills.Count; i++)
        {
            SkillData skill = msgInfo.skillinfo.unlockskills[i].skill;
            if (skill != null)
            {
                this.skillTmpList.Add(skill);
            }
        }
        ManagerCenter.Instance.GetManager<SkillManager>().RefreshSkillServerData(this.skillTmpList);
    }

    private List<SkillData> skillTmpList = new List<SkillData>();
}
