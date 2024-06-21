using System;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem
{
    public SkillItem(GameObject obj)
    {
        this.itemobj = obj;
        this.Txt_cd = this.itemobj.transform.Find("txt_cd").GetComponent<Text>();
        this.Img_cd = this.itemobj.transform.Find("img_cd").GetComponent<Image>();
    }

    public void UpdataCD(ulong Now)
    {
        this.Img_cd.gameObject.SetActive(false);
        this.Txt_cd.gameObject.SetActive(false);
        if (MainPlayerSkillHolder.Instance != null)
        {
            MainPlayerSkillBase mainPlayerSkill = MainPlayerSkillHolder.Instance.GetMainPlayerSkill(this.Config.GetField_Uint("skillid"));
            if (mainPlayerSkill != null)
            {
                float num = mainPlayerSkill.GetCDLeft(Now);
                if (num > 0f)
                {
                    this.UpdateButtonCD(num, mainPlayerSkill.CDLength);
                }
                else if (mainPlayerSkill.SkillConfig.GetField_Uint("publicCD") != 0U)
                {
                    PublicCDData publicCDDataByGroup = MainPlayer.Self.GetComponent<SkillPublicCDControl>().GetPublicCDDataByGroup(mainPlayerSkill.SkillConfig.GetField_Uint("publicCD"));
                    if (publicCDDataByGroup != null)
                    {
                        this.UpdateButtonCD(publicCDDataByGroup.GetCDLeft(Now), publicCDDataByGroup.CDLength);
                    }
                }
            }
        }
    }

    public void UpdateButtonCD(float cdtime, float maxCDTime)
    {
        if (cdtime < 10000f && cdtime > 0f)
        {
            this.Txt_cd.gameObject.SetActive(true);
            this.Txt_cd.text = string.Format("{0}", (uint)(cdtime / 1000f));
        }
        else
        {
            this.Txt_cd.gameObject.SetActive(false);
        }
        if (cdtime > 0f)
        {
            this.Img_cd.gameObject.SetActive(true);
            this.Img_cd.fillAmount = cdtime / maxCDTime;
        }
        else
        {
            cdtime = 0f;
            this.Img_cd.gameObject.SetActive(false);
        }
    }

    public GameObject itemobj;

    public LuaTable Config;

    public Text Txt_cd;

    public Image Img_cd;
}
