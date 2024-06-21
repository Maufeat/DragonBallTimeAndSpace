using Framework.Base;
using Framework.Managers;
using Net;
using System;
using UnityEngine;

namespace Engine
{
    public abstract class MonobehaviourManager : MonoBehaviour, ICompent
    {
        private static readonly object m_mutex = new object();
        private static MonobehaviourManager m_instance;
        private bool _hasInited;
        private IManagerCenter m_managerCenter;

        protected MonobehaviourManager()
        {
            lock (m_mutex)
            {
                if (m_instance != null)
                    throw new Exception("Singleton Error!");
            }
        }

        public static MonobehaviourManager Instance
        {
            get
            {
                lock (m_mutex)
                    return m_instance;
            }
        }

        public void toInit()
        {
            if (_hasInited)
                return;
            Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.SchedulerUpdate));
            this.initFrameWorkManagers();
            this._hasInited = true;
        }

        private void Awake()
        {
            lock (m_mutex)
            {
                if (!(m_instance == null))
                    throw new Exception("Singleton Error!");
                m_instance = this;
            }
            toInit();
            DontDestroyOnLoad(gameObject);
        }

        private void initFrameWorkManagers()
        {
            MgrCenter.AddManager(CtrlMgr);
            MgrCenter.AddManager(ModelMgr);
        }

        public IControllerManager CtrlMgr
        {
            get
            {
                return ControllerManager.Instance;
            }
        }

        public IModelManager ModelMgr
        {
            get
            {
                return (IModelManager)ModelManager.Instance;
            }
        }

        public IManagerCenter MgrCenter
        {
            get
            {
                if (this.m_managerCenter == null)
                {
                    this.m_managerCenter = (IManagerCenter)ManagerCenter.Instance;
                    this.m_managerCenter.Compent = (ICompent)this;
                }
                return this.m_managerCenter;
            }
        }

        public void OnUpdate()
        {
            try
            {
                Scheduler.Instance.Update();
                Scheduler.Instance.FixedUpdate();
            }
            catch (Exception ex)
            {
                FFDebug.LogError((object)this, (object)ex.Message);
            }
            LSingleton<NetWorkModule>.Instance.Update();
        }

        public void OnGuiUpdate()
        {
            Scheduler.Instance.GuiUpdate();
        }

        private void SchedulerUpdate()
        {
            this.MgrCenter.OnUpdate();
            LSingleton<ThreadManager>.Instance.Update();
        }

        public ManagerT GetMgr<ManagerT>(string managerName)
        {
            ManagerT managerT = default(ManagerT);
            IManager managerByName = this.MgrCenter.GetManagerByName(managerName);
            if (managerByName != null)
                managerT = (ManagerT)managerByName;
            return managerT;
        }
    }
}
