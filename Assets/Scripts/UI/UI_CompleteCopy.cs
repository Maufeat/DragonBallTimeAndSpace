using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_CompleteCopy : UIPanelBase
{
    private CompleteCopyController Controller
    {
        get
        {
            return ControllerManager.Instance.GetController<CompleteCopyController>();
        }
    }

    private CopyManager copyManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CopyManager>();
        }
    }

    public override void OnDispose()
    {
        for (int i = 0; i < this.personalAwardList.Count; i++)
        {
            this.personalAwardList[i].Dispose();
        }
        this.personalAwardList.Clear();
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI<UI_EquipItemInfo>();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitObj(root);
        this.InitEvent();
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private void InitObj(Transform root)
    {
        this.root = root;
        this.Result_Panel = root.transform.Find("Offset_InstanceOver/Panel_InstanceResult").gameObject;
        this.Over_Panel = root.transform.Find("Offset_InstanceOver/Panel_InstanceOver").gameObject;
        this.btnexit = root.transform.Find("Offset_InstanceOver/Panel_InstanceOver/btn_exit").GetComponent<Button>();
        this.txtremaintime = this.btnexit.transform.Find("txt_time").GetComponent<Text>();
        this.Over_Panel.gameObject.SetActive(false);
        this.Result_Panel.gameObject.SetActive(false);
        this.InitResult_Panel();
        this.SetResult_PanelData();
        this.InitOver_Panel();
        Scheduler.Instance.AddTimer(0.4f, false, new Scheduler.OnScheduler(this.ShowInstanceResult));
    }

    private void ShowInstanceResult()
    {
        this.Result_Panel.gameObject.SetActive(true);
        Scheduler.Instance.AddTimer(0.2f, false, new Scheduler.OnScheduler(this.PlayResult_PanelAnim));
        Scheduler.Instance.AddTimer(5f, false, delegate
        {
            if (!UIManager.GetUIObject<UI_CompleteCopy>())
            {
                return;
            }
            this.HideInstanceResult();
            this.ShowInstanceOver();
        });
    }

    private void HideInstanceResult()
    {
        this.Result_Panel.gameObject.SetActive(false);
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI<UI_EquipItemInfo>();
    }

    private void ShowInstanceOver()
    {
        if (this.Controller.boxAwardDataList.Count > 0)
        {
            this.FirstOpenBox();
        }
        else
        {
            this.StartCountDownLeaveCopy(new Action(this.ExitCopy));
        }
        this.Over_Panel.gameObject.SetActive(true);
    }

    private void InitEvent()
    {
        this.btnexit.onClick.RemoveAllListeners();
        this.btnexit.onClick.AddListener(new UnityAction(this.ExitCopy));
    }

    private void InitResult_Panel()
    {
        this.expSlider = this.Result_Panel.FindChild("Slider").GetComponent<Slider>();
        this.expText = this.Result_Panel.FindChild("Slider/txt_num").GetComponent<Text>();
        this.levelText = this.Result_Panel.FindChild("txt_lv/Text").GetComponent<Text>();
        for (int i = 1; i < 4; i++)
        {
            GameObject x = this.Result_Panel.FindChild("resource/item" + i);
            if (x != null)
            {
                UI_CompleteCopy.ResItem resItem = new UI_CompleteCopy.ResItem(x, this);
                resItem.Hide();
                this.resItemList.Add(resItem);
            }
        }
        for (int j = 1; j < 5; j++)
        {
            GameObject gameObject = this.Result_Panel.FindChild("items/Item" + j);
            if (gameObject != null)
            {
                AwardItem awardItem = new AwardItem();
                awardItem.SetGameOjcet(gameObject);
                awardItem.Hide();
                this.personalAwardList.Add(awardItem);
            }
        }
    }

    private void SetResult_PanelData()
    {
        int num = 0;
        while (num < this.Controller.resDataList.Count && num < this.resItemList.Count)
        {
            this.resItemList[num].SetData(this.Controller.resDataList[num]);
            this.resItemList[num].Show();
            num++;
        }
        int num2 = 0;
        while (num2 < this.Controller.personalAwardDataList.Count && num2 < this.personalAwardList.Count)
        {
            this.personalAwardList[num2].SetData(this.Controller.personalAwardDataList[num2]);
            this.personalAwardList[num2].Show();
            num2++;
        }
    }

    public void PlayResult_PanelAnim()
    {
        if (!UIManager.GetUIObject<UI_CompleteCopy>())
        {
            return;
        }
        uint level = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level;
        ulong exp = MainPlayer.Self.MainPlayeData.CharacterBaseData.exp;
        this.mTotalExpTo = this.TotalExp(level, exp);
        if (this.mTotalExpTo > this.Controller.expChange)
        {
            this.expChange = this.Controller.expChange;
        }
        else
        {
            this.expChange = this.mTotalExpTo;
        }
        this.mTotalExpFrom = this.mTotalExpTo - this.expChange;
        this.CurrChangeExp = 0UL;
        this.ExpAnimSpeed = this.expChange / this.animAnimTime;
        this.OnExpAnimPlay = true;
    }

    private void PlayExpAnim()
    {
        if (this.OnExpAnimPlay)
        {
            this.CurrChangeExp = (ulong)((uint)(this.CurrChangeExp + this.ExpAnimSpeed * Scheduler.Instance.realDeltaTime));
            if (this.CurrChangeExp >= this.expChange)
            {
                this.CurrChangeExp = this.expChange;
                this.OnExpAnimPlay = false;
            }
            if (this.expText != null)
            {
                this.expText.text = "+" + this.CurrChangeExp;
            }
            if (this.expSlider != null)
            {
                ulong num = 0UL;
                uint level = this.GetLevel(this.mTotalExpFrom + this.CurrChangeExp, out num);
                this.levelText.text = level.ToString();
                this.expSlider.value = 1f;
                uint num2 = 0U;
                if (!ControllerManager.Instance.GetController<MainUIController>().TryGetLevelAllExp(level, out num2))
                {
                    FFDebug.LogWarning("PlayExpAnim", "Level invalid : " + level.ToString());
                    return;
                }
                if (num2 > 0U)
                {
                    this.expSlider.value = num / num2;
                }
            }
        }
    }

    private ulong TotalExp(uint Level, ulong Lvexp)
    {
        ulong num = Lvexp;
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        uint num2 = 1U;
        while (num2 < Level && num2 < Const.MaxLevel)
        {
            uint num3;
            if (!controller.TryGetLevelAllExp(num2, out num3))
            {
                FFDebug.LogError(this, "TryGetLevelAllExp NULL LV=" + num2);
            }
            else
            {
                num += (ulong)num3;
            }
            num2 += 1U;
        }
        return num;
    }

    private uint GetLevel(ulong Texp, out ulong Lvexp)
    {
        uint num = 1U;
        Lvexp = Texp;
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        for (uint num2 = 1U; num2 < Const.MaxLevel; num2 += 1U)
        {
            uint num3;
            if (!controller.TryGetLevelAllExp(num2, out num3))
            {
                break;
            }
            if (Lvexp < (ulong)num3)
            {
                break;
            }
            Lvexp -= (ulong)num3;
            num += 1U;
        }
        return num;
    }

    private void InitOver_Panel()
    {
        this.txt_info = this.Over_Panel.FindChild("txt_info").GetComponent<Text>();
        this.txt_info2 = this.Over_Panel.FindChild("txt_info2").GetComponent<Text>();
        this.countDownOpenBoxStr = this.txt_info.text;
        this.btnexit.gameObject.SetActive(false);
        this.txt_info.gameObject.SetActive(false);
        for (int i = 1; i < 5; i++)
        {
            GameObject gameObject = this.Over_Panel.FindChild("awards" + i);
            if (gameObject != null)
            {
                UI_CompleteCopy.AwardBox awardBox = new UI_CompleteCopy.AwardBox();
                awardBox.SetObj(gameObject);
                awardBox.Hide();
                awardBox.ClickBox = new Action<UI_CompleteCopy.AwardBox>(this.ClickBox);
                this.awardBoxList.Add(awardBox);
                this.BoxCloseList.Add(awardBox);
            }
        }
    }

    public void FirstOpenBox()
    {
        for (int i = 0; i < this.awardBoxList.Count; i++)
        {
            if (i < this.Controller.boxAwardDataList.Count)
            {
                CompleteCopyController.AwardData awardData = this.Controller.boxAwardDataList[i];
                this.awardBoxList[i].SetData(awardData.Prop, string.Empty, string.Empty);
            }
            else
            {
                this.awardBoxList[i].SetData(null, string.Empty, string.Empty);
            }
            this.awardBoxList[i].Show();
            this.awardBoxList[i].SetState(true);
        }
        Scheduler.Instance.AddTimer(1.5f, false, delegate
        {
            if (!UIManager.GetUIObject<UI_CompleteCopy>())
            {
                return;
            }
            for (int j = 0; j < this.awardBoxList.Count; j++)
            {
                this.awardBoxList[j].SetState(false);
            }
            this.StartCountDownOpenBox(delegate
            {
                this.ClickBox(this.awardBoxList[0]);
            });
        });
    }

    private void OpenBox()
    {
        if (this.MyAwardBox != null)
        {
            for (int i = 0; i < this.Controller.boxAwardDataList.Count; i++)
            {
                if (this.Controller.boxAwardDataList[i].OwnerId == MainPlayer.Self.EID.Id)
                {
                    this.MyAwardBox.SetData(this.Controller.boxAwardDataList[i].Prop, MainPlayer.Self.OtherPlayerData.MapUserData.name, "element3");
                    this.MyAwardBox.SetState(true);
                    this.BoxCloseList.Remove(this.MyAwardBox);
                    this.Controller.boxAwardDataList.RemoveAt(i);
                    break;
                }
            }
        }
        Scheduler.Instance.AddTimer(1.2f, false, delegate
        {
            if (!UIManager.GetUIObject<UI_CompleteCopy>())
            {
                return;
            }
            while (this.BoxCloseList.Count > 0)
            {
                this.RandomOpenBox();
            }
            this.StartCountDownLeaveCopy(new Action(this.ExitCopy));
        });
    }

    private void RandomOpenBox()
    {
        CompleteCopyController.AwardData awardData = null;
        UI_CompleteCopy.AwardBox awardBox = null;
        if (this.Controller.boxAwardDataList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, this.Controller.boxAwardDataList.Count);
            awardData = this.Controller.boxAwardDataList[index];
            this.Controller.boxAwardDataList.RemoveAt(index);
        }
        if (this.BoxCloseList.Count > 0)
        {
            int index2 = UnityEngine.Random.Range(0, this.BoxCloseList.Count);
            awardBox = this.BoxCloseList[index2];
            this.BoxCloseList.RemoveAt(index2);
        }
        if (awardData != null && awardBox != null)
        {
            awardBox.SetData(awardData.Prop, this.GetOwnerName(awardData.OwnerId), string.Empty);
            awardBox.SetState(true);
        }
    }

    private string GetOwnerName(ulong id)
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        Memember teamMememberInfo = controller.GetTeamMememberInfo(id);
        if (teamMememberInfo != null)
        {
            return teamMememberInfo.name;
        }
        return string.Empty;
    }

    private void ClickBox(UI_CompleteCopy.AwardBox box)
    {
        this.MyAwardBox = box;
        this.Controller.ReqOpenBox();
        this.txt_info.gameObject.SetActive(false);
        this.OpenBox();
    }

    private void ExitCopy()
    {
        this.copyManager.CloseCompleteCopyView();
        this.copyManager.ReqFinalExitCopy();
    }

    private void Update()
    {
        this.PlayExpAnim();
        this.UpdataCountDownOpenBox();
        this.UpdataCountDownLeaveCopy();
    }

    private void StartCountDownOpenBox(Action callback)
    {
        this.OnCountDownOpenBox = true;
        this.OnCountDownLeaveCopy = false;
        this.runningTime = this.Controller.openBoxTime;
        this.CountDownFinish = callback;
        this.txt_info.gameObject.SetActive(true);
        this.txt_info2.gameObject.SetActive(true);
        this.UpdataCountDownOpenBox();
        if (this.copyManager != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("copymapmaster", (ulong)this.copyManager.MCurrentCopyID);
            if (configTable != null && configTable.GetField_Uint("type") == 3U)
            {
                this.txt_info2.gameObject.SetActive(false);
            }
        }
    }

    private void UpdataCountDownOpenBox()
    {
        if (this.OnCountDownOpenBox)
        {
            this.runningTime -= Scheduler.Instance.realDeltaTime;
            if (this.runningTime <= 0f)
            {
                this.OnCountDownOpenBox = false;
                this.runningTime = 0f;
                this.txt_info.gameObject.SetActive(false);
                if (this.CountDownFinish != null)
                {
                    this.CountDownFinish();
                }
            }
            this.txt_info.text = string.Format(this.countDownOpenBoxStr, (this.runningTime + 1f).ToInt());
        }
    }

    private void StartCountDownLeaveCopy(Action callback)
    {
        this.OnCountDownLeaveCopy = true;
        this.OnCountDownOpenBox = false;
        this.runningTime = this.Controller.leaveCopyTime;
        this.btnexit.gameObject.SetActive(true);
        this.txt_info.gameObject.SetActive(false);
        this.CountDownFinish = callback;
        this.UpdataCountDownLeaveCopy();
        if (this.copyManager != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("copymapmaster", (ulong)this.copyManager.MCurrentCopyID);
            if (configTable != null && configTable.GetField_Uint("type") == 3U)
            {
                this.txt_info2.gameObject.SetActive(false);
            }
        }
    }

    private void UpdataCountDownLeaveCopy()
    {
        if (this.OnCountDownLeaveCopy)
        {
            this.runningTime -= Scheduler.Instance.realDeltaTime;
            if (this.runningTime <= 0f)
            {
                this.OnCountDownLeaveCopy = false;
                this.runningTime = 0f;
                if (this.CountDownFinish != null)
                {
                    this.CountDownFinish();
                }
            }
            this.txtremaintime.text = (this.runningTime + 1f).ToInt().ToString();
        }
    }

    private Transform root;

    private Button btnexit;

    private Text txtremaintime;

    private Text txt_info;

    private Text txt_info2;

    private uint openBoxTime = 10U;

    private uint leaveCopyTime = 10U;

    private GameObject Result_Panel;

    private GameObject Over_Panel;

    private Slider expSlider;

    private Text expText;

    private Text levelText;

    private List<UI_CompleteCopy.ResItem> resItemList = new List<UI_CompleteCopy.ResItem>();

    private List<AwardItem> personalAwardList = new List<AwardItem>();

    private float animAnimTime = 3.2f;

    private ulong expChange;

    private bool OnExpAnimPlay;

    private ulong CurrChangeExp;

    private float ExpAnimSpeed;

    private ulong mTotalExpTo;

    private ulong mTotalExpFrom;

    private List<UI_CompleteCopy.AwardBox> awardBoxList = new List<UI_CompleteCopy.AwardBox>();

    private List<UI_CompleteCopy.AwardBox> BoxCloseList = new List<UI_CompleteCopy.AwardBox>();

    private string countDownOpenBoxStr;

    private UI_CompleteCopy.AwardBox MyAwardBox;

    private float runningTime;

    private bool OnCountDownOpenBox;

    private Action CountDownFinish;

    private bool OnCountDownLeaveCopy;

    private class ResItem
    {
        public ResItem(GameObject root, UI_CompleteCopy copy)
        {
            this.Root = root;
            root.gameObject.SetActive(false);
            this.img_icon = root.FindChild("img_icon").GetComponent<Image>();
            this.txt_count = root.FindChild("txt_value").GetComponent<Text>();
            this.Copy = copy;
        }

        public void SetData(PropsBase prop)
        {
            this.Prop = prop;
            this.SetImg_icon(this.Prop.config.GetField_String("icon"));
            this.SetNum(this.Prop.Count);
        }

        private void SetImg_icon(string strIcon)
        {
            if (this.img_icon == null)
            {
                return;
            }
            this.img_icon.gameObject.SetActive(false);
            this.Copy.GetTexture(ImageType.ITEM, strIcon, delegate (Texture2D sp)
            {
                if (sp == null)
                {
                    FFDebug.LogWarning(this, string.Format("GetTexture Tex is null config.icon = {0}", strIcon));
                    return;
                }
                Sprite sprite = Sprite.Create(sp, new Rect(0f, 0f, (float)sp.width, (float)sp.height), new Vector2(0f, 0f));
                this.img_icon.sprite = sprite;
                this.img_icon.gameObject.SetActive(true);
            });
        }

        private void SetNum(uint Count)
        {
            if (this.txt_count != null)
            {
                this.txt_count.text = string.Empty + Count;
            }
        }

        public void Hide()
        {
            if (this.Root != null)
            {
                this.Root.SetActive(false);
            }
        }

        public void Show()
        {
            if (this.Root != null)
            {
                this.Root.SetActive(true);
            }
        }

        private Image img_icon;

        private Text txt_count;

        private PropsBase Prop;

        private GameObject Root;

        private UI_CompleteCopy Copy;
    }

    private class AwardBox
    {
        public void SetObj(GameObject obj)
        {
            this.Root = obj;
            this.open = this.Root.transform.Find("img_open").gameObject;
            this.close = this.Root.transform.Find("img_close").gameObject;
            this.ownerName = this.Root.transform.Find("txt_name").GetComponent<Text>();
            GameObject gameObject = this.Root.transform.Find("Item").gameObject;
            if (gameObject != null)
            {
                this.mAwardItem = new AwardItem();
                this.mAwardItem.SetGameOjcet(gameObject);
                this.mAwardItem.Hide();
            }
            this.SetState(false);
            Button component = this.Root.GetComponent<Button>();
            if (component != null)
            {
                component.onClick.RemoveAllListeners();
                component.onClick.AddListener(delegate ()
                {
                    if (this.ClickBox != null && !this.isBoxopen)
                    {
                        this.ClickBox(this);
                    }
                });
            }
        }

        public void SetState(bool isopen)
        {
            this.isBoxopen = isopen;
            this.open.SetActive(this.isBoxopen);
            this.close.SetActive(!this.isBoxopen);
            if (this.mAwardItem != null)
            {
                if (this.isBoxopen)
                {
                    this.mAwardItem.Show();
                }
                else
                {
                    this.mAwardItem.Hide();
                }
            }
            if (this.ownerName != null)
            {
                this.ownerName.gameObject.SetActive(this.isBoxopen);
            }
        }

        public void SetData(PropsBase prop, string owner, string ColorName = "")
        {
            if (this.ownerName != null)
            {
                this.ownerName.text = owner;
                if (!string.IsNullOrEmpty(ColorName))
                {
                    this.ownerName.color = Const.GetColorByName(ColorName);
                }
            }
            if (this.mAwardItem != null)
            {
                this.mAwardItem.SetData(prop);
            }
        }

        public void Hide()
        {
            if (this.Root != null)
            {
                this.Root.SetActive(false);
            }
        }

        public void Show()
        {
            if (this.Root != null)
            {
                this.Root.SetActive(true);
            }
        }

        private GameObject Root;

        private AwardItem mAwardItem;

        private GameObject open;

        private GameObject close;

        private Text ownerName;

        public Action<UI_CompleteCopy.AwardBox> ClickBox;

        private bool isBoxopen;
    }
}
