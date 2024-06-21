using System;
using avatar;
using Framework.Managers;
using Models;

public class FashionController : ControllerBase
{
    public UI_Character mUICharacter
    {
        get
        {
            return UIManager.GetUIObject<UI_Character>();
        }
    }

    public MSG_RetUserAvatars_SC AllFasion
    {
        get
        {
            return this._fasion;
        }
        set
        {
            this._fasion = value;
            if (null != this.mUICharacter)
            {
                this.mUICharacter.RefreshFashionInfo();
            }
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                controller.ShowCurrSelectEvolution();
            }
        }
    }

    public override void Awake()
    {
        this.Init();
    }

    public void Init()
    {
        this.mFashionNetWork = new FashionNetWorker();
        this.mFashionNetWork.Initialize();
    }

    public void ReqEquipFasion(uint avatarId)
    {
        if (this._fasion == null)
        {
            return;
        }
        if (avatarId == this._fasion.equipId)
        {
            return;
        }
        if (!this._fasion.avatars.Contains(avatarId))
        {
            return;
        }
        this.mFashionNetWork.ReqEquipFasion(avatarId);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public bool CheckActived(int fashionid)
    {
        return this._fasion != null && this._fasion.avatars.Contains((uint)fashionid);
    }

    public bool CheckEquiped(int fashionid)
    {
        return this._fasion != null && (long)fashionid == (long)((ulong)this._fasion.equipId);
    }

    public int CurrEquipedId()
    {
        if (this._fasion == null)
        {
            return -1;
        }
        return (int)this._fasion.equipId;
    }

    public override string ControllerName
    {
        get
        {
            return "fashion_controller";
        }
    }

    private FashionNetWorker mFashionNetWork;

    private MSG_RetUserAvatars_SC _fasion;
}
