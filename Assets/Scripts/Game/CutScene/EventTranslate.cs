using System;
using UnityEngine;

public class EventTranslate : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
    }

    public void PlayEffect(string effectname, Transform parent)
    {
        Transform transform = parent;
        if (null == transform)
        {
            transform = base.transform;
        }
        CutSceneEffectController.Instance.PlayEffect(effectname, transform);
    }

    public void RemoveEffectObj(string effectname)
    {
        CutSceneEffectController.Instance.RemoveEffectObj(effectname);
    }
}
