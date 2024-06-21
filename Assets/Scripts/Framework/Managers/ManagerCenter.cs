using Framework.Base;
using System;
using UnityEngine;

namespace Framework.Managers
{
    public class ManagerCenter : IICompmentHoldable, IManagerCenter
    {
        private DictionaryList<string, IManager> _managers = new DictionaryList<string, IManager>();
        private static ManagerCenter _instance;
        private ICompent _compent;

        private ManagerCenter()
        {
        }

        public static ManagerCenter Instance
        {
            get
            {
                if (ManagerCenter._instance == null)
                    ManagerCenter._instance = new ManagerCenter();
                return ManagerCenter._instance;
            }
        }

        public void AddManager(IManager manager)
        {
            if (manager == null)
                throw new Exception("manager is NULL");
            this._managers.Add(manager.ManagerName, manager);
        }

        public MgrT AddManagerByType<MgrT>() where MgrT : IManager, new()
        {
            MgrT mgrT1 = default(MgrT);
            MgrT mgrT2 = new MgrT();
            if (mgrT2.ManagerName == string.Empty)
                throw new Exception("name of ctrl is NULL");
            this.AddManager((IManager)mgrT2);
            return mgrT2;
        }

        public MgrT AddManagerAsUnityComponent<MgrT>(GameObject go) where MgrT : Component, IManager, new()
        {
            go.AddComponent<MgrT>();
            MgrT component = go.GetComponent<MgrT>();
            this.AddManager((IManager)component);
            return component;
        }

        public ManagerT GetSpecialTypeManagerByName<ManagerT>(string managerName) where ManagerT : IManager
        {
            return (ManagerT)this._managers.GetValue(managerName);
        }

        public IManager GetManagerByName(string managerName)
        {
            return this._managers.GetValue(managerName);
        }

        public void OnUpdate()
        {
            int count = this._managers.Count;
            for (int index = 0; index < count; ++index)
                this._managers.GetValueAt(index).OnUpdate();
        }

        public ICompent Compent
        {
            set
            {
                this._compent = value;
            }
            get
            {
                return this._compent;
            }
        }

        public ManagerT GetManager<ManagerT>()
        {
            ManagerT managerT = default(ManagerT);
            int count = this._managers.Count;
            for (int index = 0; index < count; ++index)
            {
                IManager valueAt = this._managers.GetValueAt(index);
                if (valueAt.GetType() == typeof(ManagerT))
                {
                    managerT = (ManagerT)valueAt;
                    break;
                }
            }
            return managerT;
        }

        public void ResetManager()
        {
            int count = this._managers.Count;
            for (int index = 0; index < count; ++index)
            {
                IManager valueAt = this._managers.GetValueAt(index);
                try
                {
                    valueAt.OnReSet();
                }
                catch (Exception ex)
                {
                    FFDebug.LogError((object)this, (object)("OnReSet error:" + (object)ex));
                }
            }
        }
    }
}
