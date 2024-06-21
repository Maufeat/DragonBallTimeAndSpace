using System;
using UnityEngine;

namespace AudioStudio
{
    public class AudioGameObject : MonoBehaviour
    {
        public void Awake()
        {
            this.m_Collider = base.GetComponent<Collider>();
            if (this.IsUpdatePosition)
            {
                this.m_posData = new AkGameObjPositionData();
                AKRESULT akresult = AkSoundEngine.RegisterGameObj(this.Player, this.Player.name);
                if (akresult == AKRESULT.AK_Success)
                {
                    Vector3 position = this.GetPosition();
                    Vector3 forward = this.GetForward();
                    Vector3 up = this.GetUp();
                    AkSoundEngine.SetObjectPosition(this.Player, position.x, position.y, position.z, forward.x, forward.y, forward.z, up.x, up.y, up.z);
                }
            }
            if (this.isEnvironmentAware && this.m_Collider)
            {
                this.m_envData = new AkGameObjEnvironmentData();
                this.m_envData.AddAkEnvironment(this.m_Collider, this.m_Collider);
            }
        }

        public GameObject Player
        {
            get
            {
                if (this.IsUpdatePosition)
                {
                    return base.gameObject;
                }
                return AudioManager.Instance.GlobalSoundObject;
            }
        }

        public void OnDestroy()
        {
            if (this.StopOnDestroy && AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.StopAll(this.Player);
            }
            if (this.IsUpdatePosition && AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.UnregisterGameObj(this.Player);
            }
        }

        public void Update()
        {
            if (this.isEnvironmentAware && this.m_envData != null)
            {
                this.m_envData.UpdateAuxSend(base.gameObject, base.transform.position);
            }
            if (this.IsUpdatePosition && this.m_posData != null)
            {
                Vector3 position = this.GetPosition();
                Vector3 forward = this.GetForward();
                Vector3 up = this.GetUp();
                if (this.m_posData.position == position && this.m_posData.forward == forward && this.m_posData.up == up)
                {
                    return;
                }
                this.m_posData.position = position;
                this.m_posData.forward = forward;
                this.m_posData.up = up;
                AkSoundEngine.SetObjectPosition(this.Player, position.x, position.y, position.z, forward.x, forward.y, forward.z, up.x, up.y, up.z);
            }
        }

        public Vector3 GetPosition()
        {
            return base.transform.position;
        }

        public virtual Vector3 GetForward()
        {
            return base.transform.forward;
        }

        public virtual Vector3 GetUp()
        {
            return base.transform.up;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.isEnvironmentAware && this.m_envData != null)
            {
                this.m_envData.AddAkEnvironment(other, this.m_Collider);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (this.isEnvironmentAware && this.m_envData != null)
            {
                this.m_envData.RemoveAkEnvironment(other, this.m_Collider);
            }
        }

        public bool IsUpdatePosition = true;

        public bool StopOnDestroy = true;

        private AkGameObjPositionData m_posData;

        public bool isEnvironmentAware = true;

        private AkGameObjEnvironmentData m_envData;

        private Collider m_Collider;
    }
}
