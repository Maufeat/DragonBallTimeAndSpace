using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UI_Comic : UIPanelBase
{
    private ComicController comicController
    {
        get
        {
            return ControllerManager.Instance.GetController<ComicController>();
        }
    }

    public override void OnDispose()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNext));
        base.Dispose();
        if (this.matoutline != null)
        {
            UnityEngine.Object.DestroyImmediate(this.matoutline);
            this.matoutline = null;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.matoutline = new Material(Shader.Find("UI/PureColor"));
        this.matoutline.SetColor("_Color", new Color(0.419607848f, 0.7607843f, 1f, 0.6666667f));
        this.InitObj(root);
        this.InitEvent();
    }

    public void StartPlayComic(uint id)
    {
        if (LuaConfigManager.GetXmlConfigTable("comicdata").GetField_Table("comicdata").GetField_Table(id.ToString()).GetField_Int("autotype") == 0)
        {
            this.isauto = true;
        }
        else
        {
            this.isauto = false;
        }
        if (LuaConfigManager.GetXmlConfigTable("comicdata").GetField_Table("comicdata").GetField_Table(id.ToString()).GetField_Int("skip") == 0)
        {
            this.isskip = false;
        }
        else
        {
            this.isskip = true;
        }
        if (this.isskip)
        {
            Button component = this.root.transform.Find("Offset_Comic/btn_next").GetComponent<Button>();
            component.onClick.RemoveAllListeners();
            component.onClick.AddListener(delegate ()
            {
                this.pausecomic = true;
                ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, CommonUtil.GetText(dynamic_textid.IDs.drama_skip), MsgBoxController.MsgOptionYes, MsgBoxController.MsgOptionNo, UIManager.ParentType.Loading, delegate ()
                {
                    Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNext));
                    this.comicController.OnCallBack();
                }, delegate ()
                {
                    this.pausecomic = false;
                    Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNext));
                    this.CheckNext();
                }, null);
            });
            component.gameObject.SetActive(true);
        }
        this.pausecomic = false;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNext));
        this.CheckNext();
    }

    private void InitObj(Transform root)
    {
        this.root = root;
        this.lstComic.Clear();
        int num = root.transform.Find("Offset_Comic").childCount - 1;
        for (int i = 1; i <= num; i++)
        {
            Transform transform = root.transform.Find("Offset_Comic/Panel_" + i);
            if (!(transform != null))
            {
                break;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform.Find("bg/img_bg").gameObject);
            gameObject.transform.SetParent(transform.Find("bg"), true);
            gameObject.transform.SetAsFirstSibling();
            gameObject.transform.localScale = Vector3.one;
            gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(12f, 12f);
            gameObject.GetComponent<RectTransform>().localPosition = transform.Find("bg/img_bg").localPosition;
            gameObject.name = "img_outline";
            gameObject.SetActive(false);
            this.lstComic.Add(transform);
        }
        this.currentindex = -1;
        this.picCount = (uint)this.lstComic.Count;
    }

    private void InitEvent()
    {
        Button component = this.root.transform.Find("Offset_Comic").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNext));
            this.CheckNext();
        });
    }

    public void CheckNext()
    {
        if (this.pausecomic)
        {
            return;
        }
        this.currentindex++;
        if ((long)this.currentindex >= (long)((ulong)this.picCount))
        {
            this.comicController.OnCallBack();
            return;
        }
        this.ShowPic(this.currentindex);
    }

    private void ShowPic(int index)
    {
        if (this.isauto && (long)this.currentindex < (long)((ulong)this.picCount))
        {
            Scheduler.Instance.AddTimer(4f, false, new Scheduler.OnScheduler(this.CheckNext));
        }
        int num = 0;
        while ((long)num < (long)((ulong)this.picCount))
        {
            if (index == num)
            {
                Image component = this.lstComic[num].Find("bg/img_outline").GetComponent<Image>();
                component.material = this.matoutline;
                component.gameObject.SetActive(true);
                this.lstComic[num].Find("donghua").gameObject.SetActive(true);
                UITweener[] componentsInChildren = this.lstComic[num].GetComponentsInChildren<UITweener>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].Reset();
                    componentsInChildren[i].enabled = true;
                    componentsInChildren[i].Play(true);
                }
                this.lstComic[num].SetAsLastSibling();
                this.root.transform.Find("Offset_Comic/btn_next").SetAsLastSibling();
            }
            else
            {
                Image component2 = this.lstComic[num].Find("bg/img_outline").GetComponent<Image>();
                component2.gameObject.SetActive(false);
                this.lstComic[num].transform.Find("donghua").gameObject.SetActive(false);
                this.lstComic[num].GetComponent<TweenScale>().Reset();
                this.lstComic[num].GetComponent<TweenScale>().enabled = false;
            }
            num++;
        }
    }

    private Transform root;

    private List<Transform> lstComic = new List<Transform>();

    private uint picCount;

    private int currentindex = -1;

    private bool isauto;

    private bool isskip;

    private bool pausecomic;

    private Material matoutline;
}
