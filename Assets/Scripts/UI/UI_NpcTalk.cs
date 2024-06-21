using UnityEngine;

public class UI_NpcTalk : UIPanelBase
{
    public override void OnInit(Transform tran)
    {
        base.OnInit(tran);
        this.talkRoot = tran;
    }

    public Transform talkRoot;
}
