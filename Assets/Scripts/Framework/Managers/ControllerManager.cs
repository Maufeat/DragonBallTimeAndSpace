using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Base.MVC;
using UI.Login;

namespace Framework.Managers
{
    public class ControllerManager : ManagerBaseDefault, IControllerManager, IManager
    {
        private ControllerManager()
        {
            base.ManagerName = "controller_manager";
        }

        public static ControllerManager Instance
        {
            get
            {
                if (ControllerManager._instance == null)
                {
                    ControllerManager._instance = new ControllerManager();
                }
                return ControllerManager._instance;
            }
        }

        public void AddController(IController ctrl)
        {
            if (ctrl != null)
            {
                this._controllerDic[ctrl.ControllerName] = ctrl;
                this._controllersList.Add(ctrl);
                return;
            }
            throw new Exception("ctrl is NULL");
        }

        public CtrlT AddcontrollerByType<CtrlT>() where CtrlT : IController, new()
        {
            CtrlT ctrlT = default(CtrlT);
            CtrlT ctrlT2 = (default(CtrlT) == null) ? Activator.CreateInstance<CtrlT>() : default(CtrlT);
            if (ctrlT2.ControllerName == string.Empty)
            {
                throw new Exception("name of ctrl is NULL");
            }
            this.AddController(ctrlT2);
            return ctrlT2;
        }

        public void RemoveControllerByName(string ctrlName)
        {
            IController item = this._controllerDic[ctrlName];
            this._controllerDic.Remove(ctrlName);
            this._controllersList.Remove(item);
        }

        public IController GetControllerByName(string ctrlName)
        {
            IController controller = this._controllerDic[ctrlName];
            if (controller == null)
            {
                throw new Exception("Controller " + ctrlName + " not found");
            }
            return controller;
        }

        public ControllerT GetControllerByName<ControllerT>(string ctrlName) where ControllerT : IController
        {
            ControllerT controllerT = default(ControllerT);
            IController controller;
            this._controllerDic.TryGetValue(ctrlName, out controller);
            if (controller != null)
            {
                return (ControllerT)((object)controller);
            }
            throw new Exception("Controller " + ctrlName + " not found");
        }

        public ControllerT GetSpecialTypeControllerByName<ControllerT>(string ctrlName) where ControllerT : IController
        {
            return (ControllerT)((object)this.GetControllerByName(ctrlName));
        }

        public ControllerT GetController<ControllerT>() where ControllerT : IController
        {
            ControllerT result = default(ControllerT);
            int count = this._controllersList.Count;
            for (int i = 0; i < count; i++)
            {
                IController controller = this._controllersList[i];
                if (controller.GetType() == typeof(ControllerT))
                {
                    result = (ControllerT)((object)controller);
                    break;
                }
            }
            return result;
        }

        public LoginControllerBase GetLoginController()
        {
            return this.GetController<LoginP2PController>();
        }

        public void InitControllers()
        {
            if (this.isInit)
            {
                return;
            }
            for (int i = 0; i < this._controllersList.Count; i++)
            {
                this._controllersList[i].Awake();
            }
            this.isInit = true;
        }

        public override void OnUpdate()
        {
            int count = this._controllersList.Count;
            if (this.NeedReSet)
            {
                for (int i = 0; i < count; i++)
                {
                    this._controllersList[i].OnDestroy();
                }
                this._controllerDic.Clear();
                this._controllersList.Clear();
                this.NeedReSet = false;
                return;
            }
            if (!this.isInit)
            {
                return;
            }
            for (int j = 0; j < count; j++)
            {
                this._controllersList[j].OnUpdate();
            }
        }

        public new void OnReSet()
        {
            base.OnReSet();
        }

        public void ReSet()
        {
            this.isInit = false;
            this.NeedReSet = true;
        }

        private static ControllerManager _instance;

        private Dictionary<string, IController> _controllerDic = new Dictionary<string, IController>();

        private List<IController> _controllersList = new List<IController>();

        private bool isInit;

        private bool NeedReSet;
    }
}
