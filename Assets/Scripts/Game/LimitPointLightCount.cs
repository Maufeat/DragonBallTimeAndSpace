using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class LimitPointLightCount : MonoBehaviour
{
    private void Start()
    {
        this.GetMainPlayer();
        this.m_PointLightsMap.Clear();
        Light[] array = UnityEngine.Object.FindObjectsOfType<Light>();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].type == LightType.Point)
            {
                this.m_PointLightsMap.Add(array[i], array[i].intensity);
            }
        }
        if (this.m_PointLightsMap.Count > this.m_LimitNum)
        {
            this.m_IsCheck = true;
            this.m_PointLightsList = new List<Light>(this.m_PointLightsMap.Keys);
        }
        else
        {
            this.m_PointLightsMap.Clear();
        }
        this.m_Time = Time.realtimeSinceStartup;
    }

    public void GetMainPlayer()
    {
        MainPlayer mainPlayer = ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer;
        if (mainPlayer != null && mainPlayer.ModelObj != null)
        {
            this.m_MainPlayer = mainPlayer.ModelObj.transform;
        }
    }

    public void Update()
    {
        if (!this.m_IsCheck)
        {
            return;
        }
        if (Time.realtimeSinceStartup - this.m_Time > this.m_WaitTime)
        {
            if (this.m_MainPlayer != null)
            {
                for (int i = 0; i < this.m_PointLightsList.Count; i++)
                {
                    this.m_dis = Vector3.Distance(this.m_PointLightsList[i].transform.position, this.m_MainPlayer.position);
                    if (this.m_dis <= this.m_LightShowRadius)
                    {
                        if (!this.m_PointLightsList[i].gameObject.activeInHierarchy)
                        {
                            this.m_PointLightsList[i].gameObject.SetActive(true);
                        }
                        this.m_PointLightsList[i].intensity = Mathf.Lerp(0f, this.m_PointLightsMap[this.m_PointLightsList[i]], (this.m_LightShowRadius - this.m_dis) / this.m_LerpDis);
                    }
                    else if (this.m_PointLightsList[i].gameObject.activeInHierarchy)
                    {
                        this.m_PointLightsList[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                this.GetMainPlayer();
            }
            this.m_Time = Time.realtimeSinceStartup;
        }
    }

    public void Destory()
    {
        this.m_PointLightsMap.Clear();
    }

    private Transform m_MainPlayer;

    private Dictionary<Light, float> m_PointLightsMap = new Dictionary<Light, float>();

    private List<Light> m_PointLightsList;

    public float m_LightShowRadius = 100f;

    private float m_dis;

    private float m_LerpDis = 20f;

    private float m_WaitTime = 0.2f;

    private int m_LimitNum = 10;

    private float m_Time;

    private bool m_IsCheck;
}
