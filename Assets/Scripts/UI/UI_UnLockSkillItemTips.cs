using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class UI_UnLockSkillItemTips : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.lsCtrl = ControllerManager.Instance.GetController<UnLockSkillsController>();
        this.InitPanel(root);
    }

    private void InitPanel(Transform root)
    {
        this.obj_UnLockSkill = root.Find("Offset_ItemUse/Panel_UnLockSkill/Item").gameObject;
        this.tf_ItemParent = root.Find("Offset_ItemUse/Panel_UnLockSkill");
        this.obj_UnLockSkill.SetActive(false);
    }

    public void UpdateNewSkillItemTips(List<uint> skilllist)
    {
        skilllist.Sort();
        List<GameObject> m_sdmObjList = new List<GameObject>();
        int index = 0;
        for (int i = 0; i < skilllist.Count; i++)
        {
            uint skillid = skilllist[i];
            LuaTable config = this.lsCtrl.GetConfig(skillid, 1U);
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_UnLockSkill);
            gameObject.name = skillid.ToString();
            gameObject.transform.SetParent(this.tf_ItemParent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
            UnLockSkillTipsItem unLockSkillTipsItem = gameObject.GetComponent<UnLockSkillTipsItem>();
            if (null == unLockSkillTipsItem)
            {
                unLockSkillTipsItem = gameObject.AddComponent<UnLockSkillTipsItem>();
            }
            unLockSkillTipsItem.Initlize(config.GetCacheField_String("skillicon"), config.GetCacheField_String("skillname"));
            m_sdmObjList.Add(gameObject);
            TweenAlpha[] components = gameObject.GetComponents<TweenAlpha>();
            for (int j = 0; j < components.Length; j++)
            {
                if (components[j].delay > 0f)
                {
                    components[j].onFinished = delegate (UITweener tweener)
                    {
                        index++;
                        if (index == m_sdmObjList.Count)
                        {
                            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_UnLockSkillsItemTips");
                            return;
                        }
                        m_sdmObjList[index].SetActive(true);
                    };
                    break;
                }
            }
        }
        m_sdmObjList[0].SetActive(true);
    }

    private void PlayTweenPos(GameObject _go, Vector3 tagpos, float delaytime)
    {
        if (null == _go)
        {
            FFDebug.LogError(this, "target null...");
            return;
        }
        TweenPosition tweenPosition = _go.GetComponent<TweenPosition>();
        if (null == tweenPosition)
        {
            tweenPosition = _go.AddComponent<TweenPosition>();
            tweenPosition.style = UITweener.Style.Once;
            tweenPosition.method = UITweener.Method.Linear;
        }
        tweenPosition.from = _go.transform.position;
        tweenPosition.to = tagpos;
        tweenPosition.delay = delaytime;
        tweenPosition.duration = 0.75f;
        tweenPosition.onFinished = null;
        tweenPosition.onFinished = delegate (UITweener tweener)
        {
            if (null != _go)
            {
                UnityEngine.Object.Destroy(_go);
            }
        };
        tweenPosition.Reset();
        tweenPosition.Play(true);
    }

    private void PlayTweenScale(GameObject _go, float tagValue, float delaytime)
    {
        if (null == _go)
        {
            FFDebug.LogError(this, "target null...");
            return;
        }
        TweenScale tweenScale = _go.GetComponent<TweenScale>();
        if (null == tweenScale)
        {
            tweenScale = _go.AddComponent<TweenScale>();
            tweenScale.style = UITweener.Style.Once;
            tweenScale.method = UITweener.Method.Linear;
        }
        tweenScale.from = Vector3.one;
        tweenScale.to = Vector3.one * tagValue;
        tweenScale.delay = delaytime;
        tweenScale.duration = 0.6f;
        tweenScale.onFinished = null;
        tweenScale.Reset();
        tweenScale.Play(true);
    }

    private UnLockSkillsController lsCtrl;

    private GameObject obj_UnLockSkill;

    private Transform tf_ItemParent;
}
