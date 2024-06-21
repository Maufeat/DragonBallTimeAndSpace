namespace Framework.Base
{
    public interface IManagerCenter : IICompmentHoldable
    {
        void AddManager(IManager manager);

        ManagerT GetSpecialTypeManagerByName<ManagerT>(string managerName) where ManagerT : IManager;

        IManager GetManagerByName(string managerName);

        ManagerT GetManager<ManagerT>();

        void OnUpdate();
    }
}
