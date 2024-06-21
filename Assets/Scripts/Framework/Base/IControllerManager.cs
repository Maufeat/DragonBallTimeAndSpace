using Framework.Base.MVC;

namespace Framework.Base
{
    public interface IControllerManager : IManager
    {
        void AddController(IController ctrl);

        CtrlT AddcontrollerByType<CtrlT>() where CtrlT : IController, new();

        void RemoveControllerByName(string ctrlName);

        IController GetControllerByName(string ctrlName);

        ControllerT GetControllerByName<ControllerT>(string ctrlName) where ControllerT : IController;

        ControllerT GetSpecialTypeControllerByName<ControllerT>(string ctrlName) where ControllerT : IController;

        ControllerT GetController<ControllerT>() where ControllerT : IController;
    }
}
