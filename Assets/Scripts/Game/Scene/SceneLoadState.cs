using System;

namespace Game.Scene
{
    public enum SceneLoadState
    {
        None,
        WaitLoadLowPrefabComplete,
        StartInstantiateLowPrefab,
        WaitInstantiateLowPrefabComplete,
        Complete
    }
}
