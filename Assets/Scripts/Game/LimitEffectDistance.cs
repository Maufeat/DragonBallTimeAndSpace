using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class LimitEffectDistance : MonoBehaviour
{
    private void Start()
    {
        this.GetMainPlayer();
        this.disEffects.Clear();
        DistanceEffect[] array = UnityEngine.Object.FindObjectsOfType<DistanceEffect>();
        for (int i = 0; i < array.Length; i++)
        {
            this.disEffects.Add(array[i]);
        }
        this.m_Time = Time.realtimeSinceStartup;
    }

    public void GetMainPlayer()
    {
        this.oldPos = default(Vector3);
        MainPlayer mainPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer;
        if (mainPlayer != null && mainPlayer.ModelObj != null)
        {
            this.m_MainPlayer = mainPlayer.ModelObj.transform;
        }
    }

    private void Update()
    {
        if (!this.m_IsCheck)
        {
            return;
        }
        if (Time.realtimeSinceStartup - this.m_Time > this.m_WaitTime)
        {
            this.m_Time = Time.realtimeSinceStartup;
            if (this.m_MainPlayer != null)
            {
                Vector3 position = this.m_MainPlayer.position;
                if (this.oldPos != default(Vector3) && this.SqrDis(this.oldPos, position) < 0.01f)
                {
                    return;
                }
                for (int i = 0; i < this.disEffects.Count; i++)
                {
                    if (this.disEffects[i] != null)
                    {
                        Vector3 position2 = this.disEffects[i].transform.position;
                        float distance = this.disEffects[i].distance;
                        float num = this.SqrDis(position2, position);
                        this.disEffects[i].gameObject.SetActive(num < distance * distance);
                    }
                }
            }
            else
            {
                this.GetMainPlayer();
            }
        }
    }

    private float SqrDis(Vector3 oldPos, Vector3 pos)
    {
        return (oldPos.x - pos.x) * (oldPos.x - pos.x) + (oldPos.z - pos.z) * (oldPos.z - pos.z);
    }

    public void Destory()
    {
        this.disEffects.Clear();
    }

    public List<DistanceEffect> disEffects = new List<DistanceEffect>();

    private Transform m_MainPlayer;

    private float m_WaitTime = 0.2f;

    private float m_Time;

    private bool m_IsCheck;

    private Vector3 oldPos = default(Vector3);
}
