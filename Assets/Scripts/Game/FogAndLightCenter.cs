using System;
using System.Collections.Generic;
using UnityEngine;

public class FogAndLightCenter : MonoBehaviour
{
    public void AddGroup(FogAndLightSplitBox box)
    {
        if (!this.m_BoxBottomVerticlesGroup.ContainsKey(box.GetAreaName()))
        {
            List<FogAndLightSplitBox> list = new List<FogAndLightSplitBox>();
            list.Add(box);
            this.m_BoxBottomVerticlesGroup.Add(box.GetAreaName(), list);
        }
        else
        {
            List<FogAndLightSplitBox> list2 = this.m_BoxBottomVerticlesGroup[box.GetAreaName()];
            if (list2.Contains(box))
            {
                return;
            }
            list2.Add(box);
        }
        box.m_TriggerEnter += this.On_TriggerEnter;
        box.m_TriggerStay += this.On_TriggerStay;
        box.m_TriggerExit += this.On_TriggerExit;
    }

    public void SetFog(Color c, float d = 0.01f)
    {
        RenderSettings.fogColor = c;
        RenderSettings.fogDensity = d;
    }

    public void SetFog(Color c, float start, float end)
    {
        RenderSettings.fogColor = c;
        RenderSettings.fogStartDistance = start;
        RenderSettings.fogEndDistance = end;
    }

    public void SetFog(FogAndLightConfig con)
    {
        if (con.fogMode == FogMode.Linear)
        {
            this.SetFog(con.fogColor, con.fogStart, con.fogEnd);
        }
        else
        {
            this.SetFog(con.fogColor, con.fogDensity);
        }
    }

    public void SetFogLerp()
    {
        if (this.m_CurrentConfig.fogMode == FogMode.Linear)
        {
            this.SetFog(Color.Lerp(this.m_CurrentConfig.fogColor, this.m_TempConfig.fogColor, this.m_TempLerp), Mathf.Lerp(this.m_CurrentConfig.fogStart, this.m_TempConfig.fogStart, this.m_TempLerp), Mathf.Lerp(this.m_CurrentConfig.fogEnd, this.m_TempConfig.fogEnd, this.m_TempLerp));
        }
        else
        {
            this.SetFog(Color.Lerp(this.m_CurrentConfig.fogColor, this.m_TempConfig.fogColor, this.m_TempLerp), Mathf.Lerp(this.m_CurrentConfig.fogDensity, this.m_TempConfig.fogDensity, this.m_TempLerp));
        }
    }

    public void SetLightAndSkyBox(Color c, float alpha, float i = 1f)
    {
        if (this.m_Light != null)
        {
            this.m_Light.color = c;
            this.m_Light.intensity = i;
        }
        if (this.m_SkyBox != null)
        {
            this.m_SkyBox.GetComponent<Renderer>().material.SetFloat("_Alpha", alpha);
        }
    }

    public void SetFogAndLight()
    {
        this.SetFogLerp();
        this.SetLightAndSkyBox(Color.Lerp(this.m_CurrentConfig.lightColor, this.m_TempConfig.lightColor, this.m_TempLerp), Mathf.Lerp(this.m_CurrentConfig.skyboxAlpha, this.m_TempConfig.skyboxAlpha, this.m_TempLerp), Mathf.Lerp(this.m_CurrentConfig.lightIntensity, this.m_TempConfig.lightIntensity, this.m_TempLerp));
    }

    public void Start()
    {
        if (this.m_Configs.Count == 0)
        {
            RenderSettings.fog = false;
            return;
        }
        RenderSettings.fog = true;
    }

    public void InitData(Transform mainPlayer, Light mainLight, GameObject skybox)
    {
        this.m_Light = mainLight;
        this.m_SkyBox = skybox;
        if (this.m_Configs.Count >= 1 && this.m_BoxBottomVerticlesGroup.Count <= 0)
        {
            RenderSettings.fogMode = this.m_Configs[0].fogMode;
            this.SetFog(this.m_Configs[0]);
            this.SetLightAndSkyBox(this.m_Configs[0].lightColor, this.m_Configs[0].skyboxAlpha, this.m_Configs[0].lightIntensity);
            return;
        }
        this.m_MainPlayer = mainPlayer;
        if (this.m_MainPlayer == null)
        {
            return;
        }
        if (this.m_Configs.Count > 1 && this.m_BoxBottomVerticlesGroup.Count > 0)
        {
            this.m_CurrentConfig = this.GetFirstConfig();
            RenderSettings.fogMode = this.m_CurrentConfig.fogMode;
            this.SetFog(this.m_CurrentConfig);
            this.SetLightAndSkyBox(this.m_CurrentConfig.lightColor, this.m_CurrentConfig.skyboxAlpha, this.m_CurrentConfig.lightIntensity);
        }
        if (RenderSettings.fog)
        {
            Shader.EnableKeyword("FogOn");
        }
        else
        {
            Shader.DisableKeyword("FogOn");
        }
    }

    public FogAndLightConfig GetFirstConfig()
    {
        RaycastHit[] array = Physics.RaycastAll(new Ray(this.m_MainPlayer.position, Vector3.down), 500f);
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].collider.gameObject.GetComponent<FogAndLightAreaName>() != null)
            {
                return this.GetConfigByAreaName(array[i].collider.gameObject.GetComponent<FogAndLightAreaName>().m_AreaName);
            }
        }
        return this.m_Configs[0];
    }

    public FogAndLightConfig GetConfigByAreaName(string areaName)
    {
        return this.m_Configs.Find((FogAndLightConfig c) => c.areaName == areaName);
    }

    public FogAndLightConfig GetNextConfig(string areaName)
    {
        string[] array = areaName.Split(new char[]
        {
            '|'
        });
        if (array.Length <= 1)
        {
            return null;
        }
        if (this.m_CurrentConfig != this.GetConfigByAreaName(array[1]))
        {
            return this.GetConfigByAreaName(array[1]);
        }
        return this.GetConfigByAreaName(array[0]);
    }

    public float GetValueLerp(FogAndLightSplitBox bbvo)
    {
        if (this.m_LeftToRight)
        {
            return bbvo.GetState(this.m_MainPlayer).m_ToLeft / bbvo.GetState(this.m_MainPlayer).m_Dis;
        }
        return bbvo.GetState(this.m_MainPlayer).m_ToRight / bbvo.GetState(this.m_MainPlayer).m_Dis;
    }

    public void On_TriggerEnter(FogAndLightSplitBox bbvo)
    {
        if (bbvo.GetState(this.m_MainPlayer).m_ToLeft > bbvo.GetState(this.m_MainPlayer).m_ToRight)
        {
            this.m_LeftToRight = false;
        }
        else
        {
            this.m_LeftToRight = true;
        }
        this.m_TempConfig = this.GetNextConfig(bbvo.GetAreaName());
        this.m_TempLerp = this.GetValueLerp(bbvo);
        this.SetFogAndLight();
    }

    public void On_TriggerStay(FogAndLightSplitBox bbvo)
    {
        this.m_TempLerp = this.GetValueLerp(bbvo);
        this.SetFogAndLight();
    }

    public void On_TriggerExit(FogAndLightSplitBox bbvo)
    {
        if ((this.m_LeftToRight && bbvo.GetState(this.m_MainPlayer).m_ToLeft > bbvo.GetState(this.m_MainPlayer).m_ToRight) || (!this.m_LeftToRight && bbvo.GetState(this.m_MainPlayer).m_ToLeft < bbvo.GetState(this.m_MainPlayer).m_ToRight))
        {
            this.m_CurrentConfig = this.m_TempConfig;
        }
        this.SetFog(this.m_CurrentConfig);
        this.SetLightAndSkyBox(this.m_CurrentConfig.lightColor, this.m_CurrentConfig.skyboxAlpha, this.m_CurrentConfig.lightIntensity);
    }

    public void Destory()
    {
        RenderSettings.fog = false;
    }

    public List<FogAndLightConfig> m_Configs = new List<FogAndLightConfig>();

    private FogAndLightConfig m_CurrentConfig;

    private Dictionary<string, List<FogAndLightSplitBox>> m_BoxBottomVerticlesGroup = new Dictionary<string, List<FogAndLightSplitBox>>();

    [HideInInspector]
    public Transform m_MainPlayer;

    private Light m_Light;

    private bool m_LeftToRight = true;

    private FogAndLightConfig m_TempConfig;

    private float m_TempLerp;

    private GameObject m_SkyBox;
}
