using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UnlockSkillsPanel
{
    public UnlockSkillsPanel(Transform root)
    {
        if (null == root)
        {
            return;
        }
        this.OnInit(root);
    }

    private void OnInit(Transform root)
    {
        this.tf_btnClose = root.Find("Offset/UI_BG/Panel_title/btn_close");
        Transform transform = root.Find("Offset/UI_BG/Panel_title/txt_name");
        this.tf_skillScrollRect = root.Find("Offset/Content/obj_skill_menu/skill_ScrollRect");
        this.tf_skillgrid = root.Find("Offset/Content/obj_skill_menu/skill_ScrollRect/grid");
        this.tf_skillItem = root.Find("Offset/Content/obj_skill_menu/skill_ScrollRect/grid/skill_Item");
        this.tf_scrollbar = root.Find("Offset/Content/obj_skill_menu/Scrollbar");
        Transform transform2 = root.Find("Offset/Content/obj_skill_lvinfo/skill_curLv/txt_lv/txt_lv_num");
        Transform transform3 = root.Find("Offset/Content/obj_skill_lvinfo/skill_curLv/Panel_condition/txt_distance/txt_distance_num");
        Transform transform4 = root.Find("Offset/Content/obj_skill_lvinfo/skill_curLv/Panel_condition/txt_cooltime/txt_cooltime_num");
        Transform transform5 = root.Find("Offset/Content/obj_skill_lvinfo/skill_curLv/Panel_condition/txt_prepostime/txt_prepostime_num");
        Transform transform6 = root.Find("Offset/Content/obj_skill_lvinfo/skill_curLv/txt_skill_info");
        Transform transform7 = root.Find("Offset/Content/obj_skill_lvinfo/skill_curLv/txt_lv_type");
        Transform transform8 = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv/Panel_condition/txt_prepostime/txt_prepostime_num");
        Transform transform9 = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv/Panel_condition/txt_cooltime/txt_cooltime_num");
        Transform transform10 = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv/Panel_condition/txt_distance/txt_distance_num");
        Transform transform11 = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv/txt_lv/txt_lv_num");
        Transform transform12 = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv/txt_skill_info");
        Transform transform13 = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv/txt_lv_type");
        this.tf_nextskillLvup = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/btn_lvup");
        Transform transform14 = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/txt_needlv/txt_lv_num");
        Transform transform15 = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/txt_costgold/txt_gold_num");
        Transform transform16 = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/txt_current_gold/txt_gold_num");
        Transform transform17 = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/txt_costgold/img_gold");
        this.tf_costGrid = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/cost/costgrid");
        this.tf_costItem = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost/cost/costgrid/costItem");
        this.tf_skillMaxLv = root.Find("Offset/Content/obj_skill_lvinfo/skill_maxLv");
        this.tf_skillNextLv = root.Find("Offset/Content/obj_skill_lvinfo/skill_nextLv");
        this.tf_skillCost = root.Find("Offset/Content/obj_skill_lvinfo/skill_levelCost");
        this.tf_skillMaxCost = root.Find("Offset/Content/obj_skill_lvinfo/skill_maxCost");
        this.wayFind = root.Find("Offset/Content/txt_way_find/txt_npc");
        this.obj_content = root.Find("Offset/Content").gameObject;
        this.obj_maxTips = root.Find("Offset/UI_BG/txt_maxtips").gameObject;
        if (null != transform)
        {
            this.txt_characterName = transform.GetComponent<Text>();
        }
        if (null != transform2)
        {
            this.txt_curskillLv = transform2.GetComponent<Text>();
        }
        if (null != transform3)
        {
            this.txt_curskillDis = transform3.GetComponent<Text>();
        }
        if (null != transform4)
        {
            this.txt_curcoolTime = transform4.GetComponent<Text>();
        }
        if (null != transform5)
        {
            this.txt_curpostTime = transform5.GetComponent<Text>();
        }
        if (null != transform6)
        {
            this.txt_curskllInfo = transform6.GetComponent<Text>();
        }
        if (null != transform7)
        {
            this.txt_curskillType = transform7.GetComponent<Text>();
        }
        if (null != transform8)
        {
            this.txt_nextpostTime = transform8.GetComponent<Text>();
        }
        if (null != transform9)
        {
            this.txt_nextcoolTime = transform9.GetComponent<Text>();
        }
        if (null != transform10)
        {
            this.txt_nextskillDis = transform10.GetComponent<Text>();
        }
        if (null != transform11)
        {
            this.txt_nextskillLv = transform11.GetComponent<Text>();
        }
        if (null != transform12)
        {
            this.txt_nextskllInfo = transform12.GetComponent<Text>();
        }
        if (null != transform13)
        {
            this.txt_nextskllType = transform13.GetComponent<Text>();
        }
        if (null != transform15)
        {
            this.txt_costneedGold = transform15.GetComponent<Text>();
        }
        if (null != transform16)
        {
            this.txt_currentGold = transform16.GetComponent<Text>();
        }
        if (null != transform14)
        {
            this.txt_costneedLv = transform14.GetComponent<Text>();
        }
        if (null != transform17)
        {
            this.img_costGold = transform17.GetComponent<RawImage>();
        }
        this.SetWayFindBtnState();
    }

    public void SetWayFindBtnState()
    {
        UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
        if (controller != null && this.wayFind)
        {
            this.wayFind.parent.gameObject.SetActive(controller.manualType == UnLockSkillsController.ManualType.Self);
        }
    }

    public void SetViewStateWhenMaxLv(bool ismax)
    {
        this.obj_maxTips.SetActive(ismax);
        this.obj_content.SetActive(!ismax);
    }

    public Transform tf_btnClose;

    public Text txt_characterName;

    public GameObject obj_content;

    public GameObject obj_maxTips;

    public Transform tf_skillScrollRect;

    public Transform tf_skillgrid;

    public Transform tf_skillItem;

    public Transform tf_scrollbar;

    public Text txt_curskillLv;

    public Text txt_curskillDis;

    public Text txt_curcoolTime;

    public Text txt_curpostTime;

    public Text txt_curskllInfo;

    public Text txt_curskillType;

    public Text txt_nextpostTime;

    public Text txt_nextcoolTime;

    public Text txt_nextskillDis;

    public Text txt_nextskillLv;

    public Text txt_nextskllInfo;

    public Text txt_nextskllType;

    public Transform tf_nextskillLvup;

    public Text txt_costneedLv;

    public Text txt_costneedGold;

    public Text txt_currentGold;

    public Transform tf_costGrid;

    public Transform tf_costItem;

    public RawImage img_costGold;

    public Transform tf_skillMaxLv;

    public Transform tf_skillNextLv;

    public Transform tf_skillCost;

    public Transform tf_skillMaxCost;

    public Transform wayFind;
}
