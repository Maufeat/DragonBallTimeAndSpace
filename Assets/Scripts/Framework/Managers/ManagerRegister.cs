using Framework.Managers;

public static class ManagerRegister
{
    private static T GetManager<T>()
    {
        return ManagerCenter.Instance.GetManager<T>();
    }

    public static EntitiesManager GetEntitiesManager()
    {
        return ManagerRegister.GetManager<EntitiesManager>();
    }
}
