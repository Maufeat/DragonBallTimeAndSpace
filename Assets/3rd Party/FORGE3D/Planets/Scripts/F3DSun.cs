using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class F3DSun : MonoBehaviour
{
    public bool AutoUpdate;

    public F3DPlanet[] Planets;

    public float radius;

    public int PlanetLayer;
    int sunPosRef;

    public static bool SceneLoadFinished;
    private Renderer[][] m_PlanetRenderers;

    // Use this for initialization
    void Awake()
    {
        sunPosRef = Shader.PropertyToID("_SunPos");
    }

    // Update is called once per frame
    public void UpdatePlanets()
    {
        if (Planets == null)
        {
            if (F3DSun.SceneLoadFinished)
            {
                Planets = FindObjectsOfType<F3DPlanet>();
            }
        }
        else if (m_PlanetRenderers == null)
        {
            m_PlanetRenderers = new Renderer[Planets.Length][];
            for (int i = 0; i < Planets.Length; i++)
            {
                Renderer[] componentsInChildren = Planets[i].GetComponentsInChildren<Renderer>();
                m_PlanetRenderers[i] = new Renderer[componentsInChildren.Length];
                for (int j = 0; j < componentsInChildren.Length; j++)
                {
                    m_PlanetRenderers[i][j] = componentsInChildren[j];
                }
            }
        }
        if (Planets != null && AutoUpdate)
        {
            for (int i = 0; i < Planets.Length; i++)
            {
                if (Planets[i] == null)
                {
                    Debug.LogWarning("F3DSun : Planet script has been removed from one of the objects in the scene. Please refresh.");
                    Planets = null;
                    return;
                }
                
                Renderer[] planetRenderers = Planets[i].GetComponentsInChildren<Renderer>();

                for (int m = 0; m < planetRenderers.Length; m++)
                {
                    planetRenderers[m].sharedMaterial.SetVector(sunPosRef, transform.position); 
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * 0.55f);
    }

    void Update()
    {
        UpdatePlanets();
    }
}
