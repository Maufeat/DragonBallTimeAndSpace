using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UnLockSkillTipsItem : MonoBehaviour
{
    private void InitData(string _icon, string _name)
    {
        this.m_icon = _icon;
        this.m_name = _name;
    }

    private void OnInit()
    {
        this.img_lsIcon = base.transform.Find("img_icon").GetComponent<Image>();
        this.txt_lsInfo = base.transform.Find("txt_info").GetComponent<Text>();
        this.txt_lsname = base.transform.Find("txt_name").GetComponent<Text>();
    }

    public void Initlize(string _icon, string _name)
    {
        this.InitData(_icon, _name);
        this.OnInit();
        if (null != this.txt_lsname)
        {
            this.txt_lsname.text = this.m_name;
        }
        if (null != this.img_lsIcon)
        {
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, this.m_icon, delegate (UITextureAsset item)
            {
                if (item != null && item.textureObj != null)
                {
                    Sprite sprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                    if (this.img_lsIcon != null)
                    {
                        this.img_lsIcon.sprite = sprite;
                        this.img_lsIcon.color = Color.white;
                    }
                }
            });
        }
    }

    private Image img_lsIcon;

    private Text txt_lsInfo;

    private Text txt_lsname;

    private string m_icon;

    private string m_name;
}
