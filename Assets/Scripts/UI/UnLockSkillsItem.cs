using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnLockSkillsItem : MonoBehaviour
{
    private void OnInit()
    {
        Transform transform = base.transform.Find("img_skill_icon");
        Transform transform2 = base.transform.Find("txt_skil_name");
        Transform transform3 = base.transform.Find("txt_skill_lv_num");
        Transform transform4 = base.transform.Find("img_skill_select");
        Transform transform5 = base.transform.Find("img_skill_stay");
        Transform transform6 = base.transform.Find("txt_skil_type1");
        Transform transform7 = base.transform.Find("txt_skil_type2");
        if (null != transform)
        {
            this.mIcon = transform.GetComponent<RawImage>();
        }
        if (null != transform2)
        {
            this.mName = transform2.GetComponent<Text>();
        }
        if (null != transform3)
        {
            this.mLvNumber = transform3.GetComponent<Text>();
        }
        if (null != transform4)
        {
            this.mSelectObj = transform4.gameObject;
        }
        if (null != transform5)
        {
            this.mStayObj = transform5.gameObject;
        }
        if (null != transform7)
        {
            this.mPassiveObj = transform7.gameObject;
        }
        if (null != transform6)
        {
            this.mInitiativeObj = transform6.gameObject;
        }
    }

    private void InitEvent()
    {
        UIEventListener.Get(base.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnBtnClick);
        UIEventListener.Get(base.gameObject).onEnter = delegate (PointerEventData go)
        {
            if (null != this.mStayObj)
            {
                this.mStayObj.SetActive(true);
            }
        };
        UIEventListener.Get(base.gameObject).onExit = delegate (PointerEventData go)
        {
            if (null != this.mStayObj)
            {
                this.mStayObj.SetActive(false);
            }
        };
    }

    private void OnBtnClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        UI_UnLockSkills uiobject = UIManager.GetUIObject<UI_UnLockSkills>();
        if (null != uiobject)
        {
            uiobject.OnSkillItemClick(this.skillId);
        }
    }

    public void Initilize(uint _skillId, uint _lv, bool available)
    {
        this.skillId = _skillId;
        this.skilllv = _lv;
        this.OnInit();
        this.InitEvent();
        LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Getlv_config((ulong)_skillId, _lv);
        if (null != this.mLvNumber)
        {
            if (available)
            {
                string str = string.Empty;
                UI_UnLockSkills uiobject = UIManager.GetUIObject<UI_UnLockSkills>();
                if (null != uiobject)
                {
                    str = uiobject.textModelDic["180013"][0];
                }
                this.mLvNumber.text = this.skilllv.ToString() + str;
            }
            else
            {
                int field_Int = luaTable.GetField_Int("unlocklevel");
                string[] array = luaTable.GetField_String("unlockevolution").Split(new char[]
                {
                    '|'
                });
                int num = 0;
                if (array.Length > 0)
                {
                    int.TryParse(array[0], out num);
                }
                if (num > 0)
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", 4034UL);
                    LuaTable configTable2 = LuaConfigManager.GetConfigTable("evolution_config", (ulong)((long)num));
                    this.mLvNumber.text = string.Format(configTable.GetField_String("tips"), configTable2.GetField_String("name"));
                }
                else
                {
                    LuaTable configTable3 = LuaConfigManager.GetConfigTable("textconfig", 4035UL);
                    this.mLvNumber.text = string.Format(configTable3.GetField_String("tips"), field_Int);
                }
            }
        }
        if (null != this.mName)
        {
            this.mName.text = luaTable.GetField_String("skillname");
        }
        if (null != this.mIcon)
        {
            string[] array2 = luaTable.GetField_String("skillicon").Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries);
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, array2[0], delegate (UITextureAsset item)
            {
                if (item != null && item.textureObj != null && null != this.mIcon)
                {
                    this.mIcon.texture = item.textureObj;
                    this.mIcon.color = ((!available) ? Color.gray : Color.white);
                }
            });
        }
        if (null != this.mSelectObj)
        {
            UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
            if (controller != null)
            {
                this.mSelectObj.SetActive(controller.IsSameSkill(this.skillId, controller.CurrSkillId));
            }
        }
        bool flag = luaTable.GetCacheField_Uint("canbe_passive") == 1U;
        this.mPassiveObj.SetActive(flag);
        this.mInitiativeObj.SetActive(!flag);
    }

    public RawImage mIcon;

    public Text mName;

    public Text mLvNumber;

    public GameObject mSelectObj;

    public GameObject mStayObj;

    public GameObject mPassiveObj;

    public GameObject mInitiativeObj;

    private uint skillId;

    private uint skilllv;
}
