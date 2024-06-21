namespace Framework.Base.MVC
{
    public interface IController
    {
        string ControllerName { get; }

        void Awake();

        void OnUpdate();

        void OnDestroy();
    }
}
