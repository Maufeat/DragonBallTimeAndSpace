using System;
using evolution;
using Models;

public class HeroAwakeController : ControllerBase
{
    public UI_Character mUICharacter
    {
        get
        {
            return UIManager.GetUIObject<UI_Character>();
        }
    }

    public MSG_RetUserEvolution_SC AwakeHeroInfo
    {
        get
        {
            return this.awakeHeroInfo;
        }
        set
        {
            this.awakeHeroInfo = value;
            if (null != this.mUICharacter)
            {
                this.mUICharacter.RefreshAwakeInfo();
            }
        }
    }

    public void PlayEvolutionEffect()
    {
        if (this.reqEvolutionData)
        {
            this.reqEvolutionData = false;
            return;
        }
        FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
        if (component == null)
        {
            FFDebug.LogWarning(this, "FFEffectControl is null");
            return;
        }
        component.AddEffectGroupOnce("levelupeffect");
    }

    public override void Awake()
    {
        this.Init();
    }

    public void Init()
    {
        this.mHeroAwakeNetWork = new HeroAwakeNetWorker();
        this.mHeroAwakeNetWork.Initialize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public bool CheckActived(int fashionid)
    {
        return this.awakeHeroInfo != null && this.awakeHeroInfo.evolutions.Contains((uint)fashionid);
    }

    public void ReqHeroAwake(uint id)
    {
        this.mHeroAwakeNetWork.ReqHeroAwake(id);
    }

    public override string ControllerName
    {
        get
        {
            return "awakehero_controller";
        }
    }

    private HeroAwakeNetWorker mHeroAwakeNetWork;

    private MSG_RetUserEvolution_SC awakeHeroInfo;

    public bool reqEvolutionData;
}
