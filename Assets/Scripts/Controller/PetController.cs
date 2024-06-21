using System;
using System.Collections.Generic;
using Framework.Managers;
using magic;
using Models;
using Pet;

public class PetController : ControllerBase
{
    public UI_Pet uiPet
    {
        get
        {
            return UIManager.GetUIObject<UI_Pet>();
        }
    }

    private MainUIController mainController
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    public void EnterPetInfo()
    {
        if (this.uiPet != null)
        {
            this.uiPet.EnterPetList();
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Pet>("UI_Pet", delegate ()
            {
                if (this.uiPet != null)
                {
                    this.uiPet.EnterPetList();
                }
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void ClosePetList()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_Pet");
    }

    public void UnLockPetBar()
    {
        this.petnetWork.ReqUnlockPetBar();
    }

    public void UnLockPetBarSuccess()
    {
        if (this.uiPet != null)
        {
            this.uiPet.ViewPetList();
        }
    }

    public void InitPetListData(SummonPetUseInfo info)
    {
        MainPlayer.Self.petBarCount = info.num;
        MainPlayer.Self.petBarUnlockcount = info.unlockcount;
        this.ListPetData = info.petlist;
        this.mainController.RefreshPetInfo();
    }

    public void RefreshPetList(List<PetBase> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            PetBase petBase = this.CheckIfPetInPetList(List[i]);
            if (petBase != null)
            {
                petBase = List[i];
            }
            else
            {
                this.ListPetData.Add(List[i]);
            }
        }
        this.mainController.RefreshPetInfo();
    }

    private PetBase CheckIfPetInPetList(PetBase data)
    {
        for (int i = 0; i < this.ListPetData.Count; i++)
        {
            if (this.ListPetData[i].tempid == data.tempid)
            {
                this.ListPetData[i] = data;
                return this.ListPetData[i];
            }
        }
        return null;
    }

    public void RetChangePetState(MSG_RetSwitchPetState_SC info)
    {
        for (int i = 0; i < this.ListPetData.Count; i++)
        {
            if (this.ListPetData[i].tempid == (ulong)info.tempid)
            {
                this.ListPetData[i].state.Clear();
                for (int j = 0; j < info.curstate.Count; j++)
                {
                    this.ListPetData[i].state.Add(info.curstate[j]);
                }
                if (this.uiPet != null && this.uiPet.curPetInfo.tempid == (ulong)info.tempid)
                {
                    this.uiPet.RefreshCurPetInfo(this.ListPetData[i]);
                }
            }
        }
        if (this.uiPet != null)
        {
            this.uiPet.ViewPetList();
        }
        this.mainController.RefreshPetInfo();
    }

    public void ReqSwitchPetState(ulong tmpID, PetState from, PetState to)
    {
        this.petnetWork.ReqSwitchPetState(tmpID, from, to);
    }

    public void RetRefreshPetHp(MSG_Ret_HpMpPop_SC msg)
    {
        if (msg.target.type != 1U)
        {
            return;
        }
        bool flag = false;
        for (int i = 0; i < this.ListPetData.Count; i++)
        {
            if (msg.target.id == this.ListPetData[i].tempid)
            {
                this.ListPetData[i].hp = msg.hp;
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            return;
        }
        this.mainController.RefreshPetInfo();
    }

    public override void Awake()
    {
        this.petnetWork = new PetNetWort();
        this.petnetWork.Initialize();
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "pet_controller";
        }
    }

    private PetNetWort petnetWork;

    public List<PetBase> ListPetData = new List<PetBase>();
}
