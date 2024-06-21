using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIconItem
{
    public BuffIconItem(Transform _root)
    {
        this.root = _root;
        this.Img_Icon = this.root.Find("img_buff").gameObject.GetComponent<Image>();
        this.Img_CD = this.root.Find("img_cd").gameObject.GetComponent<Image>();
        this.Txt_layer = this.root.Find("txt_layer").gameObject.GetComponent<Text>();
        this.Txt_Time = this.root.Find("txt_time").gameObject.GetComponent<Text>();
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.TimeUpdator));
        UIEventListener.Get(this.root.gameObject).onEnter = new UIEventListener.VoidDelegate(this.skill_button_enter);
        UIEventListener.Get(this.root.gameObject).onExit = new UIEventListener.VoidDelegate(this.skill_button_exit);
    }

    public uint Priority
    {
        get
        {
            return this._priority;
        }
    }

    public ulong SetTime
    {
        get
        {
            return this._setTime;
        }
    }

    public uint CurLayer
    {
        get
        {
            return (uint)this.lstBuffData.Count;
        }
    }

    public ulong OverTime
    {
        get
        {
            return this._overTime;
        }
    }

    public uint BufferType
    {
        get
        {
            return this._bufferType;
        }
    }

    public uint BufferID
    {
        get
        {
            return this._bufferID;
        }
    }

    public ulong OwnerID
    {
        get
        {
            return this._ownerID;
        }
        set
        {
            this._ownerID = value;
        }
    }

    public ulong GiverID
    {
        get
        {
            return this._giverID;
        }
        set
        {
            this._giverID = value;
        }
    }

    public UserState Flag
    {
        get
        {
            return this._flag;
        }
    }

    public List<ulong> Effects
    {
        get
        {
            return this._effects;
        }
    }

    private void skill_button_exit(PointerEventData eventData)
    {
        this.mEnterBuffButton = null;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.TryShowSkillTip));
    }

    private void skill_button_enter(PointerEventData eventData)
    {
        this.mEnterBuffButton = eventData.pointerEnter.gameObject;
        Scheduler.Instance.AddTimer(0.5f, false, new Scheduler.OnScheduler(this.TryShowSkillTip));
    }

    private void TryShowSkillTip()
    {
        if (this.mEnterBuffButton == null)
        {
            return;
        }
        ControllerManager.Instance.GetController<ItemTipController>().OpenBuffPanel(this.Flag, this.mEnterBuffButton, this.Effects);
    }

    public void UpdateItem(ulong ownerID, LuaTable BufferConfig, BufferServerDate data)
    {
        this._bufferID = BufferConfig.GetField_Uint("id");
        this._bufferType = BufferConfig.GetField_Uint("IconShowType");
        this._priority = BufferConfig.GetField_Uint("IconPriority");
        this.OwnerID = ownerID;
        this.GiverID = data.giver;
        this._curTime = data.curTime;
        this._duartion = data.duartion;
        this._setTime = data.settime;
        this._flag = data.flag;
        this._effects = data.effects;
        this._overTime = data.overtime;
        this._configTime = data.configTime;
        if (this._configTime == 0UL && this._bufferID >= 1347U && this._bufferID <= 1361U && data.duartion > 0UL)
        {
            this._configTime = data.duartion;
        }
        this.RemoveSameData(data.thisid);
        this.lstBuffData.Add(data);
        this.root.transform.name = string.Concat(new object[]
        {
            ownerID,
            " ",
            this.GiverID,
            " ",
            data.flag.ToString()
        });
        this.UpdateLayerText();
        this.UpdateIcon(BufferConfig.GetField_String("BuffIcon"));
        this.root.gameObject.SetActive(true);
        if (this._duartion == 0UL && this.OverTime == 0UL)
        {
            this.Img_CD.fillAmount = 0f;
            this.Txt_Time.text = string.Empty;
            this._isPermanent = true;
        }
        else if (this._configTime > 0UL)
        {
            this._now = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
            float f = (this._overTime - this._now) / 1000f;
            int num = Mathf.CeilToInt(f);
            this.Txt_Time.text = CommonTools.GetTimeStringByCeilToInt((float)num);
        }
        this.Txt_Time.gameObject.SetActive(this._bufferType != 4U && this._bufferType != 5U);
        this.Img_CD.gameObject.SetActive(this._bufferType != 4U && this._bufferType != 5U);
    }

    private void UpdateIcon(string strIcon)
    {
        if (string.IsNullOrEmpty(strIcon))
        {
            return;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, strIcon, delegate (UITextureAsset asset)
        {
            if (this._isDetroy)
            {
                return;
            }
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "BuffIconItem.UpdateIcon req  texture   is  null strIcon:" + strIcon);
                return;
            }
            if (this.Img_Icon == null)
            {
                return;
            }
            this.textureasset = asset;
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            this.Img_Icon.sprite = sprite;
            this.Img_Icon.gameObject.SetActive(true);
        });
    }

    public void DestroyItem()
    {
        this._isDetroy = true;
        if (this.textureasset != null)
        {
            this.textureasset.TryUnload();
        }
        if (this.root != null)
        {
            this.lstBuffData.Clear();
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.TimeUpdator));
            UnityEngine.Object.Destroy(this.root.gameObject);
        }
    }

    public void AddBSData(LuaTable BufferConfig, BufferServerDate data)
    {
        this._bufferID = BufferConfig.GetField_Uint("id");
        this._bufferType = BufferConfig.GetField_Uint("IconShowType");
        this._priority = BufferConfig.GetField_Uint("IconPriority");
        this.GiverID = data.giver;
        this._flag = data.flag;
        this._effects = data.effects;
        this.RemoveSameData(data.thisid);
        if (this.CurLayer < BufferConfig.GetField_Uint("addlayer"))
        {
            this._setTime = data.settime;
            this._duartion = data.duartion;
            this._curTime = data.curTime;
            this._overTime = data.overtime;
            this.lstBuffData.Add(data);
        }
        else if (BufferConfig.GetField_Uint("replacetype") == 0U)
        {
            this.TryRemoveOneBS();
            this._setTime = data.settime;
            this._duartion = data.duartion;
            this._curTime = data.curTime;
            this._overTime = data.overtime;
            this.lstBuffData.Add(data);
        }
        this.UpdateLayerText();
        this.Txt_Time.gameObject.SetActive(this._bufferType != 4U && this._bufferType != 5U);
        this.Img_CD.gameObject.SetActive(this._bufferType != 4U && this._bufferType != 5U);
    }

    public void UpdateBSLstTime(BufferServerDate newData, uint resettype)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return;
        }
        if (resettype == 0U)
        {
            return;
        }
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            bufferServerDate.overtime = newData.overtime;
            bufferServerDate.duartion = newData.duartion;
        }
    }

    private void TimeUpdator()
    {
        if (this._isPermanent || this.Img_CD == null)
        {
            return;
        }
        this._now = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        if (this._now < this._overTime)
        {
            if (this._configTime != 0UL)
            {
                this.Img_CD.fillAmount = (this._overTime - this._now) * 1f / this._configTime;
            }
        }
        else
        {
            this._curTime = this._duartion;
            this.Img_CD.fillAmount = 0f;
        }
        this.BuffTimerUpdator();
        if (this._isAutoDestroy)
        {
            this.AutoDestroyUpdator();
        }
    }

    private void BuffTimerUpdator()
    {
        if (this._isPermanent || this.Txt_Time == null)
        {
            return;
        }
        if (this._now < this._overTime)
        {
            if (this._configTime != 0UL)
            {
                float f = (this._overTime - this._now) / 1000f;
                int num = Mathf.CeilToInt(f);
                this.Txt_Time.text = CommonTools.GetTimeStringByCeilToInt((float)num);
            }
        }
        else
        {
            CommonUtil.SetActive(this.Txt_Time.gameObject, false);
        }
    }

    private bool TryRemoveOneBS()
    {
        bool result = false;
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return result;
        }
        uint num = 0U;
        int index = 0;
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (num == 0U || bufferServerDate.overtime < (ulong)num)
            {
                index = i;
                result = true;
                break;
            }
        }
        this.lstBuffData.RemoveAt(index);
        return result;
    }

    public void TryDeleteItem(BufferServerDate deleteData)
    {
        if (this.CurLayer <= 1U)
        {
            this.DestroyItem();
        }
        else
        {
            for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
            {
                BufferServerDate bufferServerDate = this.lstBuffData[i];
                if (bufferServerDate.thisid == deleteData.thisid)
                {
                    this.lstBuffData.RemoveAt(i);
                }
            }
            this.UpdateLayerText();
            if (this.lstBuffData.Count > 0)
            {
                int index = this.lstBuffData.Count - 1;
                BufferServerDate bufferServerDate2 = this.lstBuffData[index];
                this.GiverID = bufferServerDate2.giver;
                this._setTime = bufferServerDate2.settime;
                this._duartion = bufferServerDate2.duartion;
                this._curTime = 0UL;
                ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
                if (bufferServerDate2.overtime > currServerTime)
                {
                    ulong num = bufferServerDate2.overtime - currServerTime;
                    if (this._duartion > num)
                    {
                        this._curTime = this._duartion - num;
                        bufferServerDate2.curTime = (ulong)((uint)this._curTime);
                    }
                }
            }
        }
    }

    public void UpdateItemIsActive(int target)
    {
        int siblingIndex = this.root.transform.GetSiblingIndex();
        this.root.gameObject.SetActive(siblingIndex <= target);
    }

    public void SetController(BuffIconController ctrl)
    {
        this._controller = ctrl;
    }

    private void AutoDestroyUpdator()
    {
        if (this.lstBuffData.Count < 1)
        {
            if (this._controller != null)
            {
                this._controller.RemoveItem(this._bufferType, this);
            }
            this.DestroyItem();
        }
        ulong currServerTime = SingletonForMono<GameTime>.Instance.GetCurrServerTime();
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (currServerTime >= bufferServerDate.overtime)
            {
                this.lstBuffData.RemoveAt(i);
                this.UpdateLayerText();
            }
        }
    }

    private void UpdateLayerText()
    {
        if (this.CurLayer > 1U)
        {
            this.Txt_layer.text = this.CurLayer.ToString();
        }
        else
        {
            this.Txt_layer.text = string.Empty;
        }
    }

    private void RemoveSameData(ulong thisid)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return;
        }
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.thisid == thisid)
            {
                this.lstBuffData.RemoveAt(i);
            }
        }
    }

    private bool CheckSameData(ulong thisid)
    {
        if (this.lstBuffData == null || this.lstBuffData.Count < 1)
        {
            return false;
        }
        for (int i = this.lstBuffData.Count - 1; i >= 0; i--)
        {
            BufferServerDate bufferServerDate = this.lstBuffData[i];
            if (bufferServerDate.thisid == thisid)
            {
                return true;
            }
        }
        return false;
    }

    private BuffIconController _controller;

    private float _fMinAplhaTime = 5f;

    private Transform root;

    private Image Img_Icon;

    private Image Img_CD;

    private Text Txt_layer;

    private Text Txt_Time;

    private ulong _duartion;

    private ulong _curTime;

    private bool _isAutoDestroy;

    private uint _priority;

    private ulong _configTime;

    private ulong _setTime;

    private ulong _overTime;

    private uint _bufferType;

    private uint _bufferID;

    private ulong _ownerID;

    private ulong _giverID;

    private UserState _flag;

    private List<ulong> _effects;

    private List<BufferServerDate> lstBuffData = new List<BufferServerDate>();

    private UITextureAsset textureasset;

    private bool _isPermanent;

    private bool _isDetroy;

    private GameObject mEnterBuffButton;

    private ulong _now;
}
