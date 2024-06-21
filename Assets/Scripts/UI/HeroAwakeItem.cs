using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class HeroAwakeItem : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Root
    {
        get
        {
            return this.root;
        }
    }

    public void SetAwakeHeroInfo(UI_Character par, GameObject r, int awakeHeroid)
    {
        this.currHeroAwakeHeroid = awakeHeroid;
        this.parent = par;
        this.root = r;
        this.iconImage = this.root.FindChild("Image/break_icon").GetComponent<RawImage>();
        this.gettedgo = this.root.FindChild("txt_get");
        this.txt_name = this.root.FindChild("txt_name").GetComponent<Text>();
        this.itemImage = this.root.FindChild("icon").GetComponent<RawImage>();
        this.progress = this.root.FindChild("Slide").GetComponent<Slider>();
        this.progress.value = 0f;
        this.progresstxt = this.root.FindChild("Slide/Text").GetComponent<Text>();
        this.progresstxt.text = string.Empty;
        this.awakeBtn = this.root.FindChild("btn_awaken").GetComponent<Button>();
        this.awakeBtn.onClick.AddListener(new UnityAction(this.btn_awake_click));
    }

    public void RefreshItem(LuaTable it, bool actived, bool increaseCondition)
    {
        this.getted = actived;
        this.txt_name.text = it.GetCacheField_String("name");
        this.progress.gameObject.SetActive(!this.getted);
        this.gettedgo.SetActive(this.getted);
        this.awakeBtn.gameObject.SetActive(!this.getted);
        this.parent.GetTexture(ImageType.ITEM, it.GetField_String("icon"), delegate (Texture2D item)
        {
            if (this.iconImage != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                this.iconImage.texture = sprite.texture;
                this.iconImage.color = Color.white;
            }
        });
        string field_String = it.GetField_String("evolutioncost");
        string[] array = field_String.Split(new char[]
        {
            '-'
        });
        if (array.Length < 2)
        {
            FFDebug.LogWarning("HeroAwakeItem RefreshItem evolutioncost invalid : ", field_String + ", id = " + this.currHeroAwakeHeroid.ToString());
            return;
        }
        int num = int.Parse(array[0]);
        int num2 = int.Parse(array[1]);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)((long)num));
        if (configTable == null)
        {
            return;
        }
        this.parent.GetTexture(ImageType.ROLES, configTable.GetField_String("icon"), delegate (Texture2D item)
        {
            if (this.itemImage != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                this.itemImage.texture = sprite.texture;
                this.itemImage.color = Color.white;
            }
        });
        int num3 = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemCount", new object[]
        {
            num
        })[0]) + (int)ControllerManager.Instance.GetController<DepotController>().GetItemCount((uint)num);
        this.progress.value = (float)num3 / (float)num2;
        this.progresstxt.text = num3 + "/" + num2;
        this.awakeBtn.GetComponent<Button>().interactable = (num3 >= num2 && increaseCondition);
    }

    private void btn_awake_click()
    {
        if (this.currHeroAwakeHeroid > 0)
        {
            this.parent.ReqHeroAwake((uint)this.currHeroAwakeHeroid);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.TryShowAwakeTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    private void TryShowAwakeTip()
    {
        if (this.currHeroAwakeHeroid > 0)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenHeroAwaktarPanel(this.currHeroAwakeHeroid, base.gameObject);
        }
    }

    private UI_Character parent;

    private GameObject root;

    private int currHeroAwakeHeroid;

    private RawImage iconImage;

    private RawImage itemImage;

    private GameObject gettedgo;

    private Text txt_name;

    private bool getted;

    private Slider progress;

    private Text progresstxt;

    private Button awakeBtn;
}
