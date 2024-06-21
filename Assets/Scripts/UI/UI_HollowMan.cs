using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_HollowMan : UIPanelBase
{
    public override void OnDispose()
    {
        base.Dispose();
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.BombCountChangeListener));
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PalyBombCDAnimCoolDown));
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PlayBombCDAnimDo));
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.m_Root = root.gameObject;
        this.InitGameObject();
    }

    private void InitGameObject()
    {
        this.m_rawImg = this.m_Root.transform.Find("Offset_HollowMan/btn_itm").GetComponent<RawImage>();
        this.m_cdImg = this.m_Root.transform.Find("Offset_HollowMan/btn_itm/img_cd").GetComponent<Image>();
        this.m_obj_cd = this.m_Root.transform.Find("Offset_HollowMan/btn_itm/txt_cd").gameObject;
        this.m_txt_cd = this.m_obj_cd.GetComponent<Text>();
        this.m_txt_num = this.m_Root.transform.Find("Offset_HollowMan/btn_itm/txt_num").GetComponent<Text>();
        this.m_BtnBg = this.m_Root.transform.Find("Offset_HollowMan/itm_bg").GetComponent<Image>();
        this.m_BtnBg.color = Color.clear;
        this.InitInfo();
    }

    public void InitInfo()
    {
        this.m_cdImg.fillAmount = 0f;
        this.m_obj_cd.SetActive(true);
        this.m_txt_cd.text = string.Empty;
        this.m_bombCount = this.GetItemCountById(8075U);
        this.m_txt_num.text = this.m_bombCount.ToString();
        this.SetIcon(this.m_rawImg, 8075U);
    }

    private void SetIcon(RawImage rawImg, uint itmId)
    {
        string imgname = string.Empty;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)itmId);
        if (configTable != null)
        {
            imgname = configTable.GetField_String("icon");
            this.m_cdTime = configTable.GetField_Uint("cdtime");
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, imgname, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (rawImg == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            rawImg.texture = textureObj;
        });
    }

    public void UseBomb()
    {
        this.UseItemById(8075U);
    }

    private void UseItemById(uint id)
    {
        PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetPropBaseByID", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            id
        })[0];
        if (propsBase != null)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                propsBase,
                1
            });
            if (!this.m_bListenerOpen)
            {
                this.m_bListenerOpen = true;
                Scheduler.Instance.AddFrame(1U, true, new Scheduler.OnScheduler(this.BombCountChangeListener));
            }
        }
    }

    private uint GetItemCountById(uint itemId)
    {
        string s = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            itemId
        })[0] + string.Empty;
        string s2 = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromTaskPackage", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            itemId
        })[0] + string.Empty;
        uint num = 0U;
        uint num2 = 0U;
        if (uint.TryParse(s, out num2))
        {
            num = (uint)Mathf.Max(num, num2);
        }
        uint num3 = 0U;
        if (uint.TryParse(s2, out num3))
        {
            num = (uint)Mathf.Max(num, num3);
        }
        return num;
    }

    public void CheckBombCountCloseUI()
    {
        if (this.GetItemCountById(8075U) == 0U)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_HollowMan");
        }
    }

    public void RefreshBombCDAndCnt()
    {
        this.m_cdSecond = this.m_cdTime;
        this.m_cdProgress = this.m_cdSecond;
        if (this.m_txt_cd == null || this.m_cdImg == null || this.m_txt_num == null)
        {
            return;
        }
        this.m_txt_cd.text = this.m_cdSecond.ToString();
        this.m_cdImg.fillAmount = 1f;
        this.m_txt_num.text = this.m_bombCount.ToString();
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.PalyBombCDAnimCoolDown));
        Scheduler.Instance.AddFrame(1U, true, new Scheduler.OnScheduler(this.PlayBombCDAnimDo));
    }

    private void BombCountChangeListener()
    {
        uint itemCountById = this.GetItemCountById(8075U);
        if (itemCountById != this.m_bombCount)
        {
            this.m_bombCount = itemCountById;
            this.m_bListenerOpen = false;
            this.RefreshBombCDAndCnt();
            Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.BombCountChangeListener));
        }
    }

    private void PalyBombCDAnimCoolDown()
    {
        this.m_cdSecond -= 1U;
        if (this.m_txt_cd == null)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PalyBombCDAnimCoolDown));
            return;
        }
        this.m_txt_cd.text = this.m_cdSecond.ToString();
        if (this.m_cdSecond <= 0U)
        {
            this.m_txt_cd.text = string.Empty;
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PalyBombCDAnimCoolDown));
        }
    }

    private void PlayBombCDAnimDo()
    {
        this.m_cdProgress -= Time.deltaTime;
        if (this.m_cdImg == null)
        {
            Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PlayBombCDAnimDo));
            return;
        }
        this.m_cdImg.fillAmount = this.m_cdProgress / this.m_cdTime;
        if (this.m_cdProgress <= 0f)
        {
            this.m_cdImg.fillAmount = 0f;
            Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PlayBombCDAnimDo));
        }
    }

    private const uint m_bombId = 8075U;

    public GameObject m_Root;

    private RawImage m_rawImg;

    private Image m_cdImg;

    private Image m_BtnBg;

    private GameObject m_obj_cd;

    private Text m_txt_cd;

    private Text m_txt_num;

    private uint m_cdTime = 5U;

    private uint m_cdSecond;

    private float m_cdProgress;

    private uint m_bombCount;

    private bool m_bListenerOpen;
}
