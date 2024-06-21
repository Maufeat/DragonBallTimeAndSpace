using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LuaPanelBase
{
    public LuaPanelBase(GameObject uiroot)
    {
        this.uiRoot = uiroot;
        this.uiName = this.uiRoot.name;
        this.usedTextureAssets.Clear();
    }

    public static LuaScriptMgr LuaScriptMgrInstance
    {
        get
        {
            return LuaScriptMgr.Instance;
        }
    }

    public bool byNpcdlg { get; private set; }

    public void SetActive(bool bactive)
    {
        if (this.uiRoot != null)
        {
            if (bactive)
            {
                this.uiRoot.transform.localPosition = Vector3.zero;
            }
            else
            {
                this.uiRoot.transform.localPosition = new Vector3(0f, 5000f, 0f);
            }
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < this.usedTextureAssets.Count; i++)
        {
            this.usedTextureAssets[i].TryUnload();
        }
        this.usedTextureAssets.Clear();
        this.ClearClick();
        if (this.uiRoot != null)
        {
            UnityEngine.Object.DestroyImmediate(this.uiRoot);
        }
    }

    public void Awake(bool bynpcdlg)
    {
        this.byNpcdlg = bynpcdlg;
        this.CallMethod("Awake", this);
    }

    public void OnDispose()
    {
        this.CallMethod("OnDispose", this);
    }

    public string GetTextModel(string text)
    {
        return ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(text);
    }

    public string GetModelContent(GameObject tx, int id)
    {
        if (tx != null)
        {
            UIInformationList component = tx.GetComponent<UIInformationList>();
            if (component != null)
            {
                return this.GetTextModel(component.listInformation[id].content);
            }
            Text component2 = tx.GetComponent<Text>();
            if (component2 != null)
            {
                return component2.text;
            }
        }
        return string.Empty;
    }

    public void SetTextModel(Text tx, string value, int id)
    {
        ControllerManager.Instance.GetController<TextModelController>().SetTextModel(tx, value, id);
    }

    public void AddClick(string buttonName, LuaFunction func)
    {
        Transform transform = this.uiRoot.transform.Find(buttonName);
        if (transform == null)
        {
            return;
        }
        this.AddObjectClick(transform.gameObject, func);
    }

    public void AddClick(Button buttonObj, LuaFunction func)
    {
        this._lstLuaFunc.Add(func);
        buttonObj.onClick.AddListener(delegate ()
        {
            func.Call(new object[]
            {
                buttonObj.gameObject
            });
        });
    }

    public void AddUIClickListener(GameObject obj, LuaFunction func)
    {
        this._lstLuaFunc.Add(func);
        UIEventListener.Get(obj).onClick = delegate (PointerEventData eventData)
        {
            func.Call(new object[]
            {
                obj.gameObject
            });
        };
    }

    public void AddUIDragListener(GameObject obj, LuaFunction func)
    {
        this._lstLuaFunc.Add(func);
        UIEventListener.Get(obj).onDrag = delegate (PointerEventData eventData)
        {
            func.Call(new object[]
            {
                eventData
            });
        };
    }

    public void AddObjectClick(GameObject gameObj, LuaFunction func)
    {
        this._lstLuaFunc.Add(func);
        gameObj.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            func.Call(new object[]
            {
                gameObj
            });
        });
    }

    public void ClearClick()
    {
        if (this._lstLuaFunc.Count == 0)
        {
            return;
        }
        for (int i = 0; i < this._lstLuaFunc.Count; i++)
        {
            this._lstLuaFunc[i].Dispose();
            this._lstLuaFunc[i] = null;
        }
        this._lstLuaFunc.Clear();
    }

    protected object[] CallMethod(string func)
    {
        if (LuaPanelBase.LuaScriptMgrInstance == null)
        {
            return null;
        }
        string name = this.uiName + "." + func;
        return LuaPanelBase.LuaScriptMgrInstance.CallLuaFunction(name, new object[]
        {
            Util.GetLuaTable(this.uiName)
        });
    }

    protected object[] CallMethod(string func, LuaPanelBase panelbase)
    {
        string name = this.uiName + "." + func;
        return LuaScriptMgr.Instance.CallLuaFunction(name, new object[]
        {
            Util.GetLuaTable(this.uiName),
            panelbase
        });
    }

    public void GetTexture(int type, string imgname, LuaFunction func)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture((ImageType)type, imgname, delegate (UITextureAsset _asset)
        {
            if (_asset == null)
            {
                func.Call();
                return;
            }
            this.usedTextureAssets.Add(_asset);
            if (_asset.textureObj == null)
            {
                func.Call();
                return;
            }
            Texture2D textureObj = _asset.textureObj;
            func.Call(new object[]
            {
                textureObj
            });
        });
    }

    private void GetTexture(ImageType type, string imgname, Action<Texture2D> callback)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(type, imgname, delegate (UITextureAsset _asset)
        {
            if (_asset == null)
            {
                callback(null);
                return;
            }
            this.usedTextureAssets.Add(_asset);
            if (_asset.textureObj == null)
            {
                callback(null);
                return;
            }
            Texture2D textureObj = _asset.textureObj;
            callback(textureObj);
        });
    }

    public void GetSprite(int type, string imgname, LuaFunction func)
    {
        this.GetTexture((ImageType)type, imgname, delegate (Texture2D texture)
        {
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f));
                func.Call(new object[]
                {
                    sprite
                });
            }
            else
            {
                func.Call();
            }
        });
    }

    public void GetSpriteFromCommonAtlas(string strName, LuaFunction func)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", strName, delegate (Sprite sprite)
        {
            func.Call(new object[]
            {
                sprite
            });
        });
    }

    public void SetImageGrey(Image img, bool isGrey)
    {
        UITextureMgr manager = ManagerCenter.Instance.GetManager<UITextureMgr>();
        if (manager != null)
        {
            manager.SetImageGrey(img, isGrey);
        }
    }

    public void SetRawImageGrey(RawImage img, bool isGrey)
    {
        UITextureMgr manager = ManagerCenter.Instance.GetManager<UITextureMgr>();
        if (manager != null)
        {
            manager.SetImageGrey(img, isGrey);
        }
    }

    private List<LuaFunction> _lstLuaFunc = new List<LuaFunction>();

    public string uiName = string.Empty;

    public GameObject uiRoot;

    public List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();
}
