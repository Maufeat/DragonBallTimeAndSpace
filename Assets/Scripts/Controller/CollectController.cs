using System;
using Framework.Managers;
using Models;

public class CollectController : ControllerBase
{
    public OccupyNetWork occupyNetWork
    {
        get
        {
            return ControllerManager.Instance.GetController<OccupyController>().occupyNetWork;
        }
    }

    public override void Awake()
    {
    }

    public void ReqHoldOnCollectNPC(ulong npcID, uint npctype, Action<bool> oncollectfinish = null)
    {
        if (MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_GATHER) || MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_HOLD_NPC))
        {
            return;
        }
        CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(npcID, CharactorType.NPC);
        if (charactorByID != null)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().manualSelect.SetTarget(charactorByID, false, true);
        }
        this.occupyNetWork.ReqHoldon(npcID, 1U, npctype);
    }

    public override string ControllerName
    {
        get
        {
            return "collect";
        }
    }

    public Action<bool> OnCollectFinish;
}
