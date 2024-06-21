using System;

internal interface ISelectTarget
{
    void Init();

    bool CheckLegal(CharactorBase cb, bool ignoredeath = false);

    void SetTarget(CharactorBase cb, bool ignoredeath = false, bool switchAutoAttack = true);

    void ReqTarget();

    void Dispose();
}
