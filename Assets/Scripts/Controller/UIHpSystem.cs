using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;
using UnityEngine;

public class UIHpSystem : ControllerBase
{
    public UI_HpSystem msystem
    {
        get
        {
            return UIManager.GetUIObject<UI_HpSystem>();
        }
    }

    public UI_HpHit UIhpHit
    {
        get
        {
            return UIManager.GetUIObject<UI_HpHit>();
        }
    }

    public void LoadUIHpSystemAsset(Action callback)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_HpHit>("UI_HpHit", delegate ()
        {
            this.UIhpHit.mRoot.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_HpSystem>("UI_HpSystem", delegate ()
            {
                if (this.msystem != null && this.msystem.mRoot != null)
                {
                    this.msystem.mRoot.transform.SetAsFirstSibling();
                    this.msystem.mRoot.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                }
                callback();
            }, UIManager.ParentType.HPRoot, false);
        }, UIManager.ParentType.HPRoot, false);
    }

    public void SetHit(EntitiesID Owner, OneSkillHitResult HitResult, bool AttIsMainPlayer, Transform target)
    {
        if (this.UIhpHit != null)
        {
            this.UIhpHit.SetHit(Owner, HitResult, AttIsMainPlayer, target);
        }
    }

    public void SetRevert(EntitiesID Owner, float change, Transform target)
    {
        if (this.UIhpHit != null)
        {
            this.UIhpHit.SetRevert(Owner, change, target);
        }
    }

    public void SetExp(float change, Transform target)
    {
        if (this.UIhpHit != null)
        {
            this.UIhpHit.SetExp(change, target);
        }
    }

    public void ResetPKModel()
    {
        this.listHp.BetterForeach(delegate (KeyValuePair<ulong, HpStruct> pair)
        {
            pair.Value.RefreshModel();
        });
    }

    public void LoadAtas(string name, Action callBack)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().LoadAtlas(name, delegate (Sprite[] sprit)
        {
            if (sprit == null)
            {
                callBack();
                return;
            }
            this.LoadComboNumSprite(sprit);
            callBack();
        });
    }

    private void LoadComboNumSprite(Sprite[] sps)
    {
        this.ComboNumSps.Clear();
        this.mSpNumNames.Clear();
        foreach (Sprite sprite in sps)
        {
            if (sprite.name.StartsWith("sz"))
            {
                this.mSpNumNames.Add(sprite.name);
                this.ComboNumSps.Add(sprite.name, sprite);
            }
            else
            {
                this.hitType[sprite.name] = sprite;
            }
        }
        this.mSpNumNames.Sort();
    }

    public Sprite GetHitSprite(string name)
    {
        return this.hitType[name];
    }

    public Sprite GetHitSprite(string name, Color Spcolor)
    {
        return this.hitType[name];
    }

    public Sprite GetSprite(int n)
    {
        return this.ComboNumSps[this.mSpNumNames[n]];
    }

    public void CreateMosterHp(CharactorBase charbase, Transform top, cs_MapNpcData npcdata, Action<HpStruct> callback)
    {
        if (this.msystem != null)
        {
            this.msystem.CreateMosterHp(charbase, top, npcdata, callback);
        }
    }

    public void CreatePlayerHp(CharactorBase charbase, Transform top, cs_MapUserData data, Action<HpStruct> callback)
    {
        if (this.msystem != null)
        {
            this.msystem.CreatePlayerHp(charbase, top, data, callback);
        }
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "uiphsystem";
        }
    }

    private bool isAssetLoaded;

    public BetterDictionary<ulong, HpStruct> listHp = new BetterDictionary<ulong, HpStruct>();

    public Dictionary<string, Sprite> ComboNumSps = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> hitType = new Dictionary<string, Sprite>();

    private List<string> mSpNumNames = new List<string>();
}
