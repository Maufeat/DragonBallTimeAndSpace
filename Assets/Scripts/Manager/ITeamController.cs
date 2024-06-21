using System;

public interface ITeamController
{
    void RefreshTeamNemberInMap();

    bool IsMyTeamNember(EntitiesID eid);
}
