using System;

public interface IFFComponent
{
    CompnentState State { get; set; }

    void CompAwake(FFComponentMgr Mgr);

    void CompUpdate();

    void CompDispose();

    void ResetComp();
}
