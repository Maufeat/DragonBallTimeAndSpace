using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombCD
{
    public void Init(UI_MainView _mainview)
    {
        this.mainview = _mainview;
        this.objcombcd = this.mainview.uiPanelRoot.transform.Find("Offset_Main/PanelSkillTips").gameObject;
        this.cdnormal = this.objcombcd.transform.Find("cd_normal").gameObject;
        this.cdbreak = this.objcombcd.transform.Find("cd_break").gameObject;
        this.cdsuccess = this.objcombcd.transform.Find("cd_success").gameObject;
        this.imgskill = this.objcombcd.transform.Find("btn_skill").GetComponent<Image>();
        this.sldnormalleft = this.cdnormal.transform.Find("cdl").GetComponent<Slider>();
        this.sldnormalright = this.cdnormal.transform.Find("cdr").GetComponent<Slider>();
        this.sldbreakleft = this.cdbreak.transform.Find("cdl").GetComponent<Slider>();
        this.sldbreakright = this.cdbreak.transform.Find("cdr").GetComponent<Slider>();
        this.sldsuccessleft = this.cdsuccess.transform.Find("cdl").GetComponent<Slider>();
        this.sldsuccessright = this.cdsuccess.transform.Find("cdr").GetComponent<Slider>();
        this.ta_skillimage = this.imgskill.GetComponent<TweenAlpha>();
        this.ta_leftbreakimage = this.sldbreakleft.transform.Find("FillArea/Fill").GetComponent<TweenAlpha>();
        this.ta_rightbreakimage = this.sldbreakright.transform.Find("FillArea/Fill").GetComponent<TweenAlpha>();
        this.ta_leftbreakbackimage = this.sldbreakleft.transform.Find("Background").GetComponent<TweenAlpha>();
        this.ta_rightbreakbackimage = this.sldbreakright.transform.Find("Background").GetComponent<TweenAlpha>();
        this.ta_leftsuccimage = this.sldsuccessleft.transform.Find("FillArea/Fill").GetComponent<TweenAlpha>();
        this.ta_rightsuccimage = this.sldsuccessright.transform.Find("FillArea/Fill").GetComponent<TweenAlpha>();
        this.ta_leftsuccbackimage = this.sldsuccessleft.transform.Find("Background").GetComponent<TweenAlpha>();
        this.ta_rightsuccbackimage = this.sldsuccessright.transform.Find("Background").GetComponent<TweenAlpha>();
        this.objcombcd.SetActive(false);
    }

    public void Dispose()
    {
    }

    public void CombStart(uint skillid)
    {
        if (MainPlayerSkillHolder.Instance.MainPlayerSkillList.ContainsKey(skillid))
        {
            string curSkillIconName = MainPlayerSkillHolder.Instance.MainPlayerSkillList[skillid].GetCurSkillIconName();
            this.mainview.GetSprite(ImageType.ITEM, curSkillIconName, delegate (Sprite item)
            {
                if (item != null)
                {
                    this.imgskill.overrideSprite = item;
                }
            });
        }
        this.ta_skillimage.Reset();
        this.ta_leftbreakimage.Reset();
        this.ta_rightbreakimage.Reset();
        this.ta_leftbreakbackimage.Reset();
        this.ta_rightbreakbackimage.Reset();
        this.ta_leftsuccbackimage.Reset();
        this.ta_rightsuccbackimage.Reset();
        this.ta_leftsuccimage.Reset();
        this.ta_rightsuccimage.Reset();
        this.ta_skillimage.enabled = false;
        this.ta_leftbreakimage.enabled = false;
        this.ta_rightbreakimage.enabled = false;
        this.ta_leftbreakbackimage.enabled = false;
        this.ta_rightbreakbackimage.enabled = false;
        this.ta_leftsuccbackimage.enabled = false;
        this.ta_rightsuccbackimage.enabled = false;
        this.ta_leftsuccimage.enabled = false;
        this.ta_rightsuccimage.enabled = false;
        this.sldnormalleft.value = 1f;
        this.sldnormalright.value = 1f;
        this.sldbreakleft.value = 1f;
        this.sldbreakright.value = 1f;
        this.sldsuccessleft.value = 1f;
        this.sldsuccessright.value = 1f;
        this.objcombcd.SetActive(true);
        this.cdnormal.SetActive(true);
        this.cdbreak.SetActive(false);
        this.cdsuccess.SetActive(false);
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.OnTimerCloseCD));
        this.pauseupdate = false;
    }

    private void CombUpdate(float curvalue, float maxvalue)
    {
        if (this.pauseupdate)
        {
            return;
        }
        float value = curvalue / maxvalue;
        this.sldnormalleft.value = value;
        this.sldnormalright.value = value;
        this.sldbreakleft.value = value;
        this.sldbreakright.value = value;
        this.sldsuccessleft.value = value;
        this.sldsuccessright.value = value;
    }

    public void CombBreak(MainPlayerSkillCombo.ComboBreakType type)
    {
        this.pauseupdate = true;
        switch (type)
        {
            case MainPlayerSkillCombo.ComboBreakType.CombSuccess:
                this.ShowSuccess();
                break;
            case MainPlayerSkillCombo.ComboBreakType.TimeOverBreak:
            case MainPlayerSkillCombo.ComboBreakType.OtherBreak:
                this.ShowBreak();
                break;
        }
    }

    private void ShowBreak()
    {
        this.cdsuccess.SetActive(false);
        this.cdbreak.SetActive(true);
        this.cdnormal.SetActive(false);
        this.ta_skillimage.enabled = true;
        this.ta_leftbreakimage.enabled = true;
        this.ta_rightbreakimage.enabled = true;
        this.ta_skillimage.Play(true);
        this.ta_leftbreakimage.Play(true);
        this.ta_rightbreakimage.Play(true);
        this.ta_leftbreakbackimage.Play(true);
        this.ta_rightbreakbackimage.Play(true);
        Scheduler.Instance.AddTimer(0.3f, false, new Scheduler.OnScheduler(this.OnTimerCloseCD));
    }

    private void ShowSuccess()
    {
        this.cdbreak.SetActive(false);
        this.cdnormal.SetActive(false);
        this.cdsuccess.SetActive(true);
        this.ta_skillimage.enabled = true;
        this.ta_leftsuccimage.enabled = true;
        this.ta_rightsuccimage.enabled = true;
        this.ta_skillimage.Play(true);
        this.ta_leftsuccimage.Play(true);
        this.ta_rightsuccimage.Play(true);
        this.ta_leftsuccbackimage.Play(true);
        this.ta_rightsuccbackimage.Play(true);
        Scheduler.Instance.AddTimer(0.3f, false, new Scheduler.OnScheduler(this.OnTimerCloseCD));
    }

    private void OnTimerCloseCD()
    {
        this.objcombcd.SetActive(false);
    }

    private GameObject objcombcd;

    private GameObject cdnormal;

    private GameObject cdbreak;

    private GameObject cdsuccess;

    private Image imgskill;

    private Slider sldnormalleft;

    private Slider sldnormalright;

    private Slider sldbreakleft;

    private Slider sldbreakright;

    private Slider sldsuccessleft;

    private Slider sldsuccessright;

    private TweenAlpha ta_skillimage;

    private TweenAlpha ta_leftbreakbackimage;

    private TweenAlpha ta_rightbreakbackimage;

    private TweenAlpha ta_leftbreakimage;

    private TweenAlpha ta_rightbreakimage;

    private TweenAlpha ta_leftsuccbackimage;

    private TweenAlpha ta_rightsuccbackimage;

    private TweenAlpha ta_leftsuccimage;

    private TweenAlpha ta_rightsuccimage;

    private bool pauseupdate;

    private UI_MainView mainview;
}
