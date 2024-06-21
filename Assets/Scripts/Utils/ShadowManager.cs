using System;
using Framework.Managers;
using Game.Scene;
using UnityEngine;

public class ShadowManager
{
    public static void OpenShadow()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().Light != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().Light.shadows = LightShadows.None;
        }
        Shader.EnableKeyword("CUSTOM_SHADOW");
    }

    public static void CloseShadow()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().Light != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().Light.shadows = LightShadows.None;
        }
        if (ShadowManager.sr != null)
        {
            UnityEngine.Object.Destroy(ShadowManager.sr);
            ShadowManager.sr = null;
        }
    }

    public static void ResetShader(GameObject inGo, Shader inShader)
    {
        Renderer[] componentsInChildren = inGo.GetComponentsInChildren<Renderer>();
        if (componentsInChildren != null)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i] as SkinnedMeshRenderer;
                if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMaterial != null)
                {
                    skinnedMeshRenderer.sharedMaterial.shader = inShader;
                }
            }
        }
    }

    public static void ResetRenderQueue(GameObject inGo, int inQueue)
    {
        Renderer[] componentsInChildren = inGo.GetComponentsInChildren<Renderer>();
        if (componentsInChildren != null)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i] as SkinnedMeshRenderer;
                if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMaterial != null)
                {
                    skinnedMeshRenderer.sharedMaterial.renderQueue = inQueue;
                }
            }
        }
    }

    public static void ResetRenderQueue()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag(Const.Tags.Wall);
        if (array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Renderer[] componentsInChildren = array[i].GetComponentsInChildren<Renderer>();
                if (componentsInChildren != null)
                {
                    for (int j = 0; j < componentsInChildren.Length; j++)
                    {
                        if (componentsInChildren[j].sharedMaterial != null)
                        {
                            componentsInChildren[j].sharedMaterial.renderQueue = Const.RenderQueue.SceneObj;
                        }
                    }
                }
            }
        }
        GameObject[] array2 = GameObject.FindGameObjectsWithTag(Const.Tags.Floor);
        if (array2 != null)
        {
            for (int k = 0; k < array2.Length; k++)
            {
                Renderer[] componentsInChildren2 = array2[k].GetComponentsInChildren<Renderer>();
                if (componentsInChildren2 != null)
                {
                    for (int l = 0; l < componentsInChildren2.Length; l++)
                    {
                        if (componentsInChildren2[l].sharedMaterial != null)
                        {
                            componentsInChildren2[l].sharedMaterial.renderQueue = Const.RenderQueue.SceneObj;
                        }
                    }
                }
            }
        }
        GameObject[] array3 = GameObject.FindGameObjectsWithTag(Const.Tags.Effect);
        if (array3 != null)
        {
            for (int m = 0; m < array3.Length; m++)
            {
                Renderer[] componentsInChildren3 = array3[m].GetComponentsInChildren<Renderer>();
                if (componentsInChildren3 != null)
                {
                    for (int n = 0; n < componentsInChildren3.Length; n++)
                    {
                        if (componentsInChildren3[n].sharedMaterial != null)
                        {
                            componentsInChildren3[n].sharedMaterial.renderQueue = Const.RenderQueue.Transparent;
                        }
                    }
                }
            }
        }
    }

    private static int _genViewSize = 25;

    private static ShadowRender sr;
}
