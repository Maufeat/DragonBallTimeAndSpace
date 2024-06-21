using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AbattoirReward : UIPanelBase
{
    private AbattoirMatchController abattoirController
    {
        get
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitGameObject(root);
        this.InitEvent();
    }

    private void InitGameObject(Transform root)
    {
        this.mRoot = root;
        this.title = this.mRoot.Find("Offset_Confirm/Panel_Window/text_up").GetComponent<Text>();
        this.timeText = this.mRoot.Find("Offset_Confirm/Panel_Window/time").GetComponent<Text>();
        for (int i = 0; i < 5; i++)
        {
            Transform transform = this.mRoot.Find("Offset_Confirm/Panel_Window/btns/btn_" + i);
            this.btns.Add(transform);
            Animator component = transform.Find("effect/box").GetComponent<Animator>();
            component.Play("close");
            BoxEffectComponent boxEffectComponent = component.gameObject.AddComponent<BoxEffectComponent>();
            boxEffectComponent.index = i + 1;
            boxEffectComponent.showObj = transform.Find("item");
            RawImage component2 = transform.Find("item/RawImage").GetComponent<RawImage>();
            if (component2 != null)
            {
                component2.enabled = false;
            }
            AnimationClip animationClip = component.runtimeAnimatorController.animationClips[0];
            animationClip.AddEvent(new AnimationEvent
            {
                time = animationClip.length,
                functionName = "OnEffectEnd"
            });
        }
        this.content = this.mRoot.Find("Offset_Confirm/Panel_Window/image/text").GetComponent<Text>();
    }

    private void InitEvent()
    {
        for (int i = 0; i < this.btns.Count; i++)
        {
            Transform transform = this.btns[i];
            int index = i + 1;
            UIEventListener.Get(transform.gameObject).onClick = delegate (PointerEventData eventData)
            {
                this.abattoirController.SendGetReward(index);
            };
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.DisposeEvent();
        this.DisposeGameObject();
    }

    private void DisposeEvent()
    {
        if (this.dele != null)
        {
            Scheduler.Instance.RemoveTimer(this.dele);
        }
    }

    private void DisposeGameObject()
    {
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    private void InitRewardConfig()
    {
        if (this.dic == null)
        {
            this.dic = new Dictionary<int, UI_AbattoirReward.RewardRankItem>();
            LuaTable field_Table = LuaConfigManager.GetXmlConfigTable("mobapk").GetField_Table("dragonreward");
            if (field_Table == null)
            {
                return;
            }
            for (int i = 0; i < field_Table.Count; i++)
            {
                LuaTable luaTable = field_Table[i + 1] as LuaTable;
                if (luaTable == null)
                {
                    FFDebug.LogError(this, "mobapk -> dragonreward -> item==null:" + field_Table.ToString());
                }
                else
                {
                    int field_Int = luaTable.GetField_Int("rank");
                    string field_String = luaTable.GetField_String("title");
                    string field_String2 = luaTable.GetField_String("reward");
                    this.dic.Add(field_Int, new UI_AbattoirReward.RewardRankItem
                    {
                        rank = field_Int,
                        title = field_String,
                        content = field_String2
                    });
                }
            }
            field_Table.Dispose();
        }
    }

    public void OpenShow(int count, int rank, int restTime)
    {
        this.InitRewardConfig();
        UI_AbattoirReward.RewardRankItem rewardRankItem;
        if (!this.dic.TryGetValue(rank, out rewardRankItem))
        {
            Debug.LogError("Reward配置中 不存在 rank=" + rank);
            return;
        }
        this.title.text = rewardRankItem.title;
        this.content.text = rewardRankItem.content;
        for (int i = 0; i < this.btns.Count; i++)
        {
            this.btns[i].gameObject.SetActive(i < count);
        }
        if (this.dele != null)
        {
            Scheduler.Instance.RemoveTimer(this.dele);
        }
        if (this.timeText != null)
        {
            this.timeText.text = restTime.ToString();
            this.dele = delegate ()
            {
                if (restTime > 0 && this.timeText != null)
                {
                    restTime--;
                    this.timeText.text = restTime.ToString();
                }
                else
                {
                    this.OnTimeEnd();
                    Scheduler.Instance.RemoveTimer(this.dele);
                    this.dele = null;
                }
            };
            Scheduler.Instance.AddTimer(1f, true, this.dele);
        }
    }

    public void SetItem(uint index, string name, uint itemId, uint count = 0U)
    {
        if (this == null || this.mRoot == null)
        {
            return;
        }
        if (index < 1U)
        {
            FFDebug.LogError(this, "Reward SetItem index<1 越界");
            return;
        }
        int index2 = (int)(index - 1U);
        Transform transform = this.btns[index2];
        try
        {
            Text component = transform.Find("item/player").GetComponent<Text>();
            if (component != null)
            {
                component.text = name;
            }
            RawImage image = transform.Find("item/RawImage").GetComponent<RawImage>();
            if (image != null)
            {
                ClassSetItemTexture.Instance.SetItemTexture(image, itemId, ImageType.ITEM, delegate (UITextureAsset textureAsset)
                {
                    if (textureAsset == null)
                    {
                        FFDebug.LogError(this, "SetItem texture is null  itemId:" + itemId);
                        return;
                    }
                    image.enabled = true;
                });
            }
            Text component2 = transform.Find("item/RawImage/count").GetComponent<Text>();
            if (component2 != null)
            {
                component2.text = count.ToString();
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, ex.ToString());
        }
        Animator component3 = transform.Find("effect/box").GetComponent<Animator>();
        component3.Play("open");
    }

    private void OnTimeEnd()
    {
        this.abattoirController.SendGetReward(0);
    }

    public void Close()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this);
    }

    public Transform mRoot;

    public Text title;

    public Text timeText;

    public Text content;

    public List<Transform> btns = new List<Transform>();

    private Scheduler.OnScheduler dele;

    private Dictionary<int, UI_AbattoirReward.RewardRankItem> dic;

    public class RewardRankItem
    {
        public int rank;

        public string title;

        public string content;
    }
}
