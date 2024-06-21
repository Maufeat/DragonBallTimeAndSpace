namespace Framework.Base
{
    public interface ICompent
    {
        IManagerCenter MgrCenter { get; }

        IControllerManager CtrlMgr { get; }

        IModelManager ModelMgr { get; }

        ManagerT GetMgr<ManagerT>(string managerName);
    }
}
