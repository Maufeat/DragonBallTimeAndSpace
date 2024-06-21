using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AbattoirPray : UIPanelBase
{
    private AbattoirPrayController prayController
    {
        get
        {
            return ControllerManager.Instance.GetController<AbattoirPrayController>();
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
        this.timeText = this.mRoot.Find("Offset/Content/obj_pray_info/time").GetComponent<Text>();
        this.btnPray = this.mRoot.Find("Offset/Content/obj_pray_info/btn_pray");
        this.prayGroup1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1").GetComponent<ToggleGroup>();
        this.prayGroup2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2").GetComponent<ToggleGroup>();
        this.prayGroup3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3").GetComponent<ToggleGroup>();
        this.toggle_1_1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1/1").GetComponent<Toggle>();
        this.toggle_1_2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1/2").GetComponent<Toggle>();
        this.toggle_1_3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1/3").GetComponent<Toggle>();
        this.toggle_2_1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2/1").GetComponent<Toggle>();
        this.toggle_2_2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2/2").GetComponent<Toggle>();
        this.toggle_2_3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2/3").GetComponent<Toggle>();
        this.toggle_3_1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3/1").GetComponent<Toggle>();
        this.toggle_3_2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3/2").GetComponent<Toggle>();
        this.toggle_3_3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3/3").GetComponent<Toggle>();
        this.text_1_1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1/1/text").GetComponent<Text>();
        this.text_1_2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1/2/text").GetComponent<Text>();
        this.text_1_3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_1/3/text").GetComponent<Text>();
        this.text_2_1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2/1/text").GetComponent<Text>();
        this.text_2_2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2/2/text").GetComponent<Text>();
        this.text_2_3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_2/3/text").GetComponent<Text>();
        this.text_3_1 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3/1/text").GetComponent<Text>();
        this.text_3_2 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3/2/text").GetComponent<Text>();
        this.text_3_3 = this.mRoot.Find("Offset/Content/obj_pray_info/img_title_3/3/text").GetComponent<Text>();
    }

    private void InitEvent()
    {
        UIEventListener.Get(this.btnPray.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPray);
    }

    private void OnClickPray(PointerEventData eventData)
    {
        uint lv1Index = 0U;
        uint lv2Index = 0U;
        uint lv3Index = 0U;
        if (this.prayGroup1.gameObject.activeSelf)
        {
            IEnumerable<Toggle> enumerable = this.prayGroup1.ActiveToggles();
            using (IEnumerator<Toggle> enumerator = enumerable.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    Toggle x = enumerator.Current;
                    if (x == this.toggle_1_1)
                    {
                        lv1Index = 0U;
                    }
                    else if (x == this.toggle_1_2)
                    {
                        lv1Index = 1U;
                    }
                    else if (x == this.toggle_1_3)
                    {
                        lv1Index = 2U;
                    }
                }
            }
        }
        if (this.prayGroup2.gameObject.activeSelf)
        {
            IEnumerable<Toggle> enumerable2 = this.prayGroup2.ActiveToggles();
            using (IEnumerator<Toggle> enumerator2 = enumerable2.GetEnumerator())
            {
                if (enumerator2.MoveNext())
                {
                    Toggle x2 = enumerator2.Current;
                    if (x2 == this.toggle_2_1)
                    {
                        lv2Index = 0U;
                    }
                    else if (x2 == this.toggle_2_2)
                    {
                        lv2Index = 1U;
                    }
                    else if (x2 == this.toggle_2_3)
                    {
                        lv2Index = 2U;
                    }
                }
            }
        }
        if (this.prayGroup3.gameObject.activeSelf)
        {
            IEnumerable<Toggle> enumerable3 = this.prayGroup3.ActiveToggles();
            using (IEnumerator<Toggle> enumerator3 = enumerable3.GetEnumerator())
            {
                if (enumerator3.MoveNext())
                {
                    Toggle x3 = enumerator3.Current;
                    if (x3 == this.toggle_3_1)
                    {
                        lv3Index = 0U;
                    }
                    else if (x3 == this.toggle_3_2)
                    {
                        lv3Index = 1U;
                    }
                    else if (x3 == this.toggle_3_3)
                    {
                        lv3Index = 2U;
                    }
                }
            }
        }
        this.prayController.SendPray(lv1Index, lv2Index, lv3Index);
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

    public void OpenShow(List<string> hopes, int restTime = 15)
    {
        if (hopes.Count != 3 && hopes.Count != 6 && hopes.Count != 9)
        {
            FFDebug.LogError(this, "hopes.Count != 3,6,9 hopes.Count=" + hopes.Count);
            return;
        }
        if (this.dele != null)
        {
            Scheduler.Instance.RemoveTimer(this.dele);
        }
        this.prayGroup1.gameObject.SetActive(hopes.Count >= 3);
        this.prayGroup2.gameObject.SetActive(hopes.Count >= 6);
        this.prayGroup3.gameObject.SetActive(hopes.Count >= 9);
        if (this.prayGroup1.gameObject.activeSelf)
        {
            this.text_1_1.text = hopes[0];
            this.text_1_2.text = hopes[1];
            this.text_1_3.text = hopes[2];
        }
        if (this.prayGroup2.gameObject.activeSelf)
        {
            this.text_2_1.text = hopes[3];
            this.text_2_2.text = hopes[4];
            this.text_2_3.text = hopes[5];
        }
        if (this.prayGroup3.gameObject.activeSelf)
        {
            this.text_3_1.text = hopes[6];
            this.text_3_2.text = hopes[7];
            this.text_3_3.text = hopes[8];
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
                    Scheduler.Instance.RemoveTimer(this.dele);
                    this.dele = null;
                    this.OnTimeEnd();
                }
            };
            Scheduler.Instance.AddTimer(1f, true, this.dele);
        }
    }

    public void UpdateShow(int restTime)
    {
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
                    Scheduler.Instance.RemoveTimer(this.dele);
                    this.dele = null;
                    this.OnTimeEnd();
                }
            };
            Scheduler.Instance.AddTimer(1f, true, this.dele);
        }
    }

    private void OnTimeEnd()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI(this);
    }

    public Transform mRoot;

    private Scheduler.OnScheduler dele;

    private Transform btnPray;

    private Text timeText;

    private ToggleGroup prayGroup1;

    private ToggleGroup prayGroup2;

    private ToggleGroup prayGroup3;

    private Toggle toggle_1_1;

    private Toggle toggle_1_2;

    private Toggle toggle_1_3;

    private Toggle toggle_2_1;

    private Toggle toggle_2_2;

    private Toggle toggle_2_3;

    private Toggle toggle_3_1;

    private Toggle toggle_3_2;

    private Toggle toggle_3_3;

    private Text text_1_1;

    private Text text_1_2;

    private Text text_1_3;

    private Text text_2_1;

    private Text text_2_2;

    private Text text_2_3;

    private Text text_3_1;

    private Text text_3_2;

    private Text text_3_3;
}
