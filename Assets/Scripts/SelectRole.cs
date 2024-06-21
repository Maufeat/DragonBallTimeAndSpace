using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;

public class SelectRole
{
    public void IsSelect(bool Select)
    {
        if (this.onSelect == Select)
        {
            return;
        }
        if (this.Modelobj == null)
        {
            return;
        }
        this.onSelect = Select;
        if (this.onSelect)
        {
            this.Modelobj.GetComponent<Animator>().CrossFade("go", 0f);
        }
        else
        {
            this.Modelobj.GetComponent<Animator>().CrossFade("back", 0f);
        }
    }

    public void Load(Action callback)
    {
        CharacterModelMgr mCharacterModelMgr = ManagerCenter.Instance.GetManager<CharacterModelMgr>();
        mCharacterModelMgr.LoadSimpleCharacterObj(this.config.GetField_String("modelname"), delegate
        {
            GameObject simpleCharacterObj = mCharacterModelMgr.GetSimpleCharacterObj(this.config.GetField_String("modelname"));
            if (simpleCharacterObj != null)
            {
                this.Modelobj = UnityEngine.Object.Instantiate<GameObject>(simpleCharacterObj);
                this.SetPos();
                if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
                {
                    ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(this.Modelobj, false);
                }
                ShadowManager.ResetRenderQueue(this.Modelobj, Const.RenderQueue.SceneObjAfterCharactor);
                callback();
            }
        });
    }

    public void Unload()
    {
        ManagerCenter.Instance.GetManager<CharacterModelMgr>().UnLoadSimpleCharacterObj(this.config.GetField_String("modelname"));
        if (this.Modelobj != null)
        {
            UnityEngine.Object.Destroy(this.Modelobj);
        }
    }

    private void SetPos()
    {
        this.Modelobj.transform.position = this.artConfig.RolePos;
        this.Modelobj.transform.rotation = this.artConfig.RoleRot;
    }

    public GameObject Modelobj;

    public uint occupation;

    public LuaTable config;

    public SingleRoleConfig artConfig;

    private bool onSelect;
}
