using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_3DIconModelPosCheck : UIPanelBase
{
    public override void OnDispose()
    {
        base.Dispose();
        this.isCreate = false;
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        if (this.lastCreatIrc != null)
        {
            Color black = Color.black;
            black.a = 0f;
            this.lastCreatIrc.cameraRT.backgroundColor = black;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.uiRoot = root;
        string field_String = LuaConfigManager.GetXmlConfigTable("other").GetField_Table("headGmOutLineTex").GetField_String("texname");
        this.maskTexName = field_String.Split(new char[]
        {
            ','
        });
        this.InitObj();
        this.InitEvent();
        this.orignalData = new float[6];
        this.texMask = new Texture[this.maskTexName.Length];
    }

    private void InitObj()
    {
        this.imageIcon = this.uiRoot.Find("Offset/Panel_root/3DIcon").GetComponent<RawImage>();
        this.imageIcon.transform.Find("mask").gameObject.SetActive(false);
        this.inputWidth = this.uiRoot.Find("Offset/Panel_root/InputField_width").GetComponent<InputField>();
        this.inputHeight = this.uiRoot.Find("Offset/Panel_root/InputField_height").GetComponent<InputField>();
        this.inputID = this.uiRoot.Find("Offset/Panel_root/InputField_npcid").GetComponent<InputField>();
        this.inputResult = this.uiRoot.Find("Offset/Panel_root/InputField").GetComponent<InputField>();
        this.sliderPx = this.uiRoot.Find("Offset/Panel_root/Slider_px").GetComponent<Slider>();
        this.sliderPy = this.uiRoot.Find("Offset/Panel_root/Slider_py").GetComponent<Slider>();
        this.sliderPz = this.uiRoot.Find("Offset/Panel_root/Slider_pz").GetComponent<Slider>();
        this.sliderRx = this.uiRoot.Find("Offset/Panel_root/Slider_rx").GetComponent<Slider>();
        this.sliderRy = this.uiRoot.Find("Offset/Panel_root/Slider_ry").GetComponent<Slider>();
        this.sliderRz = this.uiRoot.Find("Offset/Panel_root/Slider_rz").GetComponent<Slider>();
        this.btnCreate = this.uiRoot.Find("Offset/Panel_root/Button_create").GetComponent<Button>();
        this.btnReset = this.uiRoot.Find("Offset/Panel_root/Button_reset").GetComponent<Button>();
        this.btnClose = this.uiRoot.Find("Offset/Panel_root/Button_close").GetComponent<Button>();
        this.dropDown = this.uiRoot.Find("Offset/Panel_root/Dropdown").GetComponent<Dropdown>();
        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        for (int i = 0; i < this.maskTexName.Length; i++)
        {
            list.Add(new Dropdown.OptionData
            {
                text = (char)(65 + i) + "  " + this.maskTexName[i]
            });
        }
        this.dropDown.options = list;
    }

    private void InitEvent()
    {
        this.inputWidth.onValueChanged.RemoveAllListeners();
        this.inputHeight.onValueChanged.RemoveAllListeners();
        this.inputID.onValueChanged.RemoveAllListeners();
        this.sliderPx.onValueChanged.RemoveAllListeners();
        this.sliderPy.onValueChanged.RemoveAllListeners();
        this.sliderPz.onValueChanged.RemoveAllListeners();
        this.sliderRx.onValueChanged.RemoveAllListeners();
        this.sliderRy.onValueChanged.RemoveAllListeners();
        this.sliderRz.onValueChanged.RemoveAllListeners();
        this.btnCreate.onClick.RemoveAllListeners();
        this.btnReset.onClick.RemoveAllListeners();
        this.btnClose.onClick.RemoveAllListeners();
        this.sliderPz.maxValue = 12f;
        this.sliderPz.minValue = -5f;
        this.sliderPx.maxValue = 12f;
        this.sliderPx.minValue = -5f;
        this.sliderPy.maxValue = 12f;
        this.sliderPy.minValue = -5f;
        this.inputWidth.onValueChanged.AddListener(delegate (string v)
        {
            if (!string.IsNullOrEmpty(v))
            {
                this.width = int.Parse(v);
            }
        });
        this.inputHeight.onValueChanged.AddListener(delegate (string v)
        {
            if (!string.IsNullOrEmpty(v))
            {
                this.height = int.Parse(v);
            }
        });
        this.inputID.onValueChanged.AddListener(delegate (string v)
        {
            if (!string.IsNullOrEmpty(v))
            {
                this.npcid = int.Parse(v);
            }
        });
        this.sliderPx.onValueChanged.AddListener(delegate (float f)
        {
            this.sliderPx.transform.Find("InputField").GetComponent<InputField>().text = f.ToString("f2");
            this.px = f;
            this.SetTargetPosAndRot();
        });
        this.sliderPx.transform.Find("InputField").GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        this.sliderPx.transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate (string f)
        {
            this.px = float.Parse(f);
            this.SetTargetPosAndRot();
        });
        this.sliderPy.onValueChanged.AddListener(delegate (float f)
        {
            this.sliderPy.transform.Find("InputField").GetComponent<InputField>().text = f.ToString("f2");
            this.py = f;
            this.SetTargetPosAndRot();
        });
        this.sliderPy.transform.Find("InputField").GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        this.sliderPy.transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate (string f)
        {
            this.py = float.Parse(f);
            this.SetTargetPosAndRot();
        });
        this.sliderPz.onValueChanged.AddListener(delegate (float f)
        {
            this.sliderPz.transform.Find("InputField").GetComponent<InputField>().text = f.ToString("f2");
            this.pz = f;
            this.SetTargetPosAndRot();
        });
        this.sliderPz.transform.Find("InputField").GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        this.sliderPz.transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate (string f)
        {
            this.pz = float.Parse(f);
            this.SetTargetPosAndRot();
        });
        this.sliderRx.onValueChanged.AddListener(delegate (float f)
        {
            this.sliderRx.transform.Find("InputField").GetComponent<InputField>().text = f.ToString("f0");
            this.rx = f;
            this.SetTargetPosAndRot();
        });
        this.sliderRx.transform.Find("InputField").GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        this.sliderRx.transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate (string f)
        {
            this.rx = float.Parse(f);
            this.SetTargetPosAndRot();
        });
        this.sliderRy.onValueChanged.AddListener(delegate (float f)
        {
            this.sliderRy.transform.Find("InputField").GetComponent<InputField>().text = f.ToString("f0");
            this.ry = f;
            this.SetTargetPosAndRot();
        });
        this.sliderRy.transform.Find("InputField").GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        this.sliderRy.transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate (string f)
        {
            this.ry = float.Parse(f);
            this.SetTargetPosAndRot();
        });
        this.sliderRz.onValueChanged.AddListener(delegate (float f)
        {
            this.sliderRz.transform.Find("InputField").GetComponent<InputField>().text = f.ToString("f0");
            this.rz = f;
            this.SetTargetPosAndRot();
        });
        this.sliderRz.transform.Find("InputField").GetComponent<InputField>().onValueChanged.RemoveAllListeners();
        this.sliderRz.transform.Find("InputField").GetComponent<InputField>().onValueChanged.AddListener(delegate (string f)
        {
            this.rz = float.Parse(f);
            this.SetTargetPosAndRot();
        });
        this.btnCreate.onClick.AddListener(new UnityAction(this.Create));
        this.btnReset.onClick.AddListener(new UnityAction(this.Reset));
        this.btnClose.onClick.AddListener(delegate ()
        {
            UIManager.Instance.DeleteUI<UI_3DIconModelPosCheck>();
        });
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.imageIcon.gameObject.AddComponent<Icon3DPosQuickSet>();
        this.imageIcon.raycastTarget = true;
        this.dropDown.onValueChanged.RemoveAllListeners();
        this.dropDown.onValueChanged.AddListener(new UnityAction<int>(this.SelectPreviewMaskImage));
    }

    private void SelectPreviewMaskImage(int index)
    {
        if (string.IsNullOrEmpty(this.maskTexName[index]))
        {
            this.imageIcon.material.SetTexture("_MaskTex", null);
            return;
        }
        if (this.texMask[index] == null)
        {
            UITextureMgr.Instance.GetTexture(ImageType.OTHERS, this.maskTexName[index], delegate (UITextureAsset t)
            {
                this.texMask[index] = ((t != null) ? t.textureObj : null);
                if (this.imageIcon == null)
                {
                    return;
                }
                this.imageIcon.material.SetTexture("_MaskTex", this.texMask[index]);
                if (t != null)
                {
                    this.usedTextureAssets.Add(t);
                }
            });
        }
        else
        {
            this.imageIcon.material.SetTexture("_MaskTex", this.texMask[index]);
        }
    }

    public void SetPosZ(float delta)
    {
        this.sliderPz.value += delta;
    }

    public void SetPos(Vector3 newPos)
    {
        this.sliderPx.value = newPos.x;
        this.sliderPy.value = newPos.y;
        this.sliderPz.value = newPos.z;
    }

    public void SetRotY(float delta)
    {
        this.sliderRy.value += delta;
    }

    private void Create()
    {
        if (string.IsNullOrEmpty(this.inputWidth.text) || string.IsNullOrEmpty(this.inputHeight.text) || string.IsNullOrEmpty(this.inputID.text))
        {
            TipsWindow.ShowNotice("Input must params!!!");
            return;
        }
        if (this.lastCreatIrc != null)
        {
            UnityEngine.Object.DestroyImmediate(this.lastCreatIrc);
        }
        this.imageIcon.rectTransform.sizeDelta = new Vector2((float)this.width, (float)this.height);
        GlobalRegister.ShowNpcOrPlayerRTT(this.imageIcon, (uint)this.npcid, 0, null);
        Scheduler.Instance.AddTimer(1f, false, delegate
        {
            IconRenderCtrl component = this.imageIcon.GetComponent<IconRenderCtrl>();
            if (component != null)
            {
                this.isCreate = true;
                this.RecordOldData(component);
                this.lastCreatIrc = component;
                Color black = Color.black;
                black.a = 0.5f;
                this.lastCreatIrc.cameraRT.backgroundColor = black;
            }
        });
    }

    private void RecordOldData(IconRenderCtrl irc)
    {
        this.sliderPx.value = (this.orignalData[0] = irc.localPos.x);
        this.sliderPy.value = (this.orignalData[1] = irc.localPos.y);
        this.sliderPz.value = (this.orignalData[2] = irc.localPos.z);
        this.sliderRx.minValue = -180f;
        this.sliderRx.maxValue = 180f;
        this.sliderRy.minValue = -180f;
        this.sliderRy.maxValue = 180f;
        this.sliderRz.minValue = -180f;
        this.sliderRz.maxValue = 180f;
        this.sliderRx.value = (this.orignalData[3] = irc.localRot.x);
        this.sliderRy.value = (this.orignalData[4] = irc.localRot.y);
        this.sliderRz.value = (this.orignalData[5] = irc.localRot.z);
    }

    private void Reset()
    {
        if (this.lastCreatIrc != null)
        {
            this.lastCreatIrc.localPos = new Vector3(this.orignalData[0], this.orignalData[1], this.orignalData[2]);
            this.lastCreatIrc.target.transform.localEulerAngles = new Vector3(this.orignalData[3], this.orignalData[4], this.orignalData[5]);
            this.sliderPx.value = this.orignalData[0];
            this.sliderPy.value = this.orignalData[1];
            this.sliderPz.value = this.orignalData[2];
            this.sliderRx.value = this.orignalData[3];
            this.sliderRy.value = this.orignalData[4];
            this.sliderRz.value = this.orignalData[5];
        }
    }

    private void SetTargetPosAndRot()
    {
        if (this.lastCreatIrc != null)
        {
            this.lastCreatIrc.localPos = new Vector3(this.px, this.py, this.pz);
            this.lastCreatIrc.target.transform.localEulerAngles = new Vector3(this.rx, this.ry, this.rz);
        }
    }

    private void Update()
    {
        if (this.isCreate)
        {
            this.result = string.Concat(new object[]
            {
                this.px.ToString("f2"),
                "|",
                this.py.ToString("f2"),
                "|",
                this.pz.ToString("f2"),
                "|",
                this.rx.ToString("f0"),
                "|",
                this.ry.ToString("f0"),
                "|",
                this.rz.ToString("f0"),
                "|",
                this.imageIcon.rectTransform.sizeDelta.x,
                "*",
                this.imageIcon.rectTransform.sizeDelta.y
            });
            this.inputResult.text = this.result;
        }
    }

    private Transform uiRoot;

    private InputField inputWidth;

    private InputField inputHeight;

    private InputField inputID;

    private InputField inputResult;

    private Slider sliderPx;

    private Slider sliderPy;

    private Slider sliderPz;

    private Slider sliderRx;

    private Slider sliderRy;

    private Slider sliderRz;

    private Button btnCreate;

    private Button btnReset;

    private Button btnClose;

    private RawImage imageIcon;

    private Dropdown dropDown;

    private Camera camIcon;

    private int width;

    private int height;

    private int npcid;

    private float px;

    private float py;

    private float pz;

    private float rx;

    private float ry;

    private float rz;

    private string result;

    private bool isCreate;

    private float[] orignalData;

    private IconRenderCtrl lastCreatIrc;

    private Texture[] texMask;

    private string[] maskTexName;
}
