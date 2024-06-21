namespace Framework.Base
{
    public interface IManager
    {
        string ManagerName { get; }

        void OnUpdate();

        void OnReSet();
    }
}
