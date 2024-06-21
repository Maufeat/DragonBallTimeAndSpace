using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ChooseRole : UIPanelBase
{
    private ChooseRoleController Controller
    {
        get
        {
            return ControllerManager.Instance.GetController<ChooseRoleController>();
        }
    }

    public static void LoadView(Action Loadover)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ChooseRole>("UI_ChooseRole", delegate ()
        {
            if (Loadover != null)
            {
                Loadover();
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        this.ChooseJobView = root.Find("Offset_ChooseRole/Panel_ChooseJob");
        this.CreatRoleView = root.Find("Offset_ChooseRole/Panel_CreateRole");
        this.input_name = this.CreatRoleView.Find("input_name").GetComponent<InputField>();
        this.CreatRoleView.gameObject.SetActive(false);
        this.ChooseJobView.gameObject.SetActive(true);
        this.txt_name = this.CreatRoleView.Find("jobbasic/txt_name").GetComponent<Text>();
        this.txt_en = this.CreatRoleView.Find("jobbasic/txt_en").GetComponent<Text>();
        this.txt_detail = this.CreatRoleView.Find("jobbasic/txt_detail").GetComponent<Text>();
        this.txt_jobinfo = this.CreatRoleView.Find("job_info/txt_info").GetComponent<Text>();
        this.SexanglerObj = this.CreatRoleView.Find("job_info/rader/img_rader").gameObject;
        this.SexanglerObj.transform.localEulerAngles = new Vector3(300f, 90f, 270f);
        this.SexanglerObj.transform.localScale = new Vector3(97f, 97f, 97f);
        this.SexanglerMeshF = this.SexanglerObj.AddComponent<MeshFilter>();
        this.SexanglerMat = new Material(Shader.Find("Unlit/Texture"));
        Material sexanglerMat = this.SexanglerMat;
        this.SexanglerObj.AddComponent<MeshRenderer>().sharedMaterial = sexanglerMat;
        this.SexanglerMat = sexanglerMat;
        this.LoadTexture();
        this.UITweenerArray = this.CreatRoleView.Find("job_info").GetComponentsInChildren<UITweener>();
        this.InitEvent();
        this.Root.gameObject.SetActive(false);
    }

    private void LoadTexture()
    {
        base.GetTexture(ImageType.OTHERS, "sexangler", delegate (Texture2D item)
        {
            if (item != null)
            {
                this.SexanglerMat.mainTexture = item;
            }
        });
    }

    public void PlayUITweener()
    {
        if (this.UITweenerArray == null)
        {
            FFDebug.LogWarning(this, "UITweenerArray null");
            return;
        }
        for (int i = 0; i < this.UITweenerArray.Length; i++)
        {
            this.UITweenerArray[i].Reset();
            this.UITweenerArray[i].Play(true);
        }
    }

    public void Show()
    {
        this.Root.gameObject.SetActive(true);
    }

    private void InitEvent()
    {
        GameObject gameObject = this.CreatRoleView.Find("btn_start").gameObject;
        GameObject gameObject2 = this.CreatRoleView.Find("btn_return").gameObject;
        UIEventListener.Get(gameObject2).onClick = delegate (PointerEventData data)
        {
            this.BackToChooseJobView();
        };
        UIEventListener.Get(gameObject).onClick = delegate (PointerEventData data)
        {
            this.StartGame();
        };
        Transform transform = this.CreatRoleView.Find("joblist");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name != "unknown")
            {
                child.Find("img_none").gameObject.SetActive(true);
                child.Find("img_bg").gameObject.SetActive(false);
                child.Find("img_on").gameObject.SetActive(false);
            }
        }
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("newUser");
        for (int j = 0; j < configTableList.Count; j++)
        {
            LuaTable luaTable = configTableList[j];
            Transform transform2 = transform.Find(luaTable.GetField_String("Button"));
            if (!(transform2 == null))
            {
                transform2.Find("img_none").gameObject.SetActive(false);
                transform2.Find("img_bg").gameObject.SetActive(true);
                transform2.Find("img_on").gameObject.SetActive(false);
                UI_ChooseRole.JobSelectBtn jobSelectBtn = new UI_ChooseRole.JobSelectBtn(luaTable.GetField_Uint("id"), transform2);
                jobSelectBtn.Config = luaTable;
                this.JobSelectBtnList[jobSelectBtn.JobKey] = jobSelectBtn;
                jobSelectBtn.OnClick = new Action<UI_ChooseRole.JobSelectBtn>(this.OnClickJobSelectBtn);
                jobSelectBtn.SetSelect(false);
            }
        }
    }

    private void OnClickJobSelectBtn(UI_ChooseRole.JobSelectBtn btn)
    {
        this.Controller.SwitchSelectOnCreatRoleView(btn.JobKey);
    }

    public void SetSelect(uint job)
    {
        this.JobSelectBtnList.BetterForeach(delegate (KeyValuePair<uint, UI_ChooseRole.JobSelectBtn> item)
        {
            item.Value.SetSelect(job == item.Key);
        });
    }

    private void BackToChooseJobView()
    {
        this.CreatRoleView.gameObject.SetActive(false);
        this.ChooseJobView.gameObject.SetActive(true);
        this.Controller.BackToRoleSelectView();
        this.JobSelectBtnList.BetterForeach(delegate (KeyValuePair<uint, UI_ChooseRole.JobSelectBtn> item)
        {
            item.Value.SetSelect(false);
        });
    }

    private void StartGame()
    {
        this.Controller.Register();
    }

    public void OpenCreatRoleView(SelectRole Role)
    {
        this.CreatRoleView.gameObject.SetActive(true);
        this.ChooseJobView.gameObject.SetActive(false);
        this.SetSelect(Role.config.GetField_Uint("id"));
        this.SetCreatRoleView(Role.config);
        this.PlayUITweener();
    }

    private void SetCreatRoleView(LuaTable config)
    {
        this.txt_name.text = config.GetField_String("name");
        this.txt_en.text = config.GetField_String("Foreign");
        this.txt_detail.text = config.GetField_String("Des");
        this.txt_jobinfo.text = config.GetField_String("Charact");
        this.SetJpWord(config.GetField_String("japanese"));
        this.SetSexangler(config.GetField_String("Ability"));
    }

    private void SetJpWord(string name)
    {
        Transform transform = this.CreatRoleView.Find("jobbasic/jp");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(child.name == name);
        }
    }

    private void SetSexangler(string ParamStr)
    {
        string[] array = ParamStr.Split(new char[]
        {
            '-'
        });
        this.SetSexangParamList.Clear();
        for (int i = 0; i < this.order.Length; i++)
        {
            float item = 0f;
            try
            {
                item = float.Parse(array[this.order[i]]) / 6f;
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, string.Format("SetSexangler: {0} error {1}", ParamStr, arg));
            }
            this.SetSexangParamList.Add(item);
        }
        this.SexanglerMeshF.mesh = CommonTools.CreateMeshByAray(this.SetSexangParamList.ToArray(), 360f);
    }

    public override void OnDispose()
    {
        base.OnDispose();
        if (this.Root != null)
        {
            UnityEngine.Object.Destroy(this.Root.gameObject);
        }
        if (this.SexanglerMat != null)
        {
            if (this.SexanglerMat.mainTexture != null)
            {
                UnityEngine.Object.DestroyImmediate(this.SexanglerMat.mainTexture, true);
            }
            UnityEngine.Object.DestroyImmediate(this.SexanglerMat, true);
        }
    }

    private Transform ChooseJobView;

    private Transform CreatRoleView;

    public InputField input_name;

    private Transform Root;

    private GameObject SexanglerObj;

    public MeshFilter SexanglerMeshF;

    private Text txt_name;

    private Text txt_en;

    private Text txt_detail;

    private Text txt_jobinfo;

    private Material SexanglerMat;

    private UITweener[] UITweenerArray;

    private BetterDictionary<uint, UI_ChooseRole.JobSelectBtn> JobSelectBtnList = new BetterDictionary<uint, UI_ChooseRole.JobSelectBtn>();

    private List<float> SetSexangParamList = new List<float>();

    private int[] order = new int[]
    {
        0,
        1,
        2,
        3,
        4,
        5
    };

    private class JobSelectBtn
    {
        public JobSelectBtn(uint Job, Transform btn)
        {
            this.btnTran = btn;
            this.JobKey = Job;
            this.onSelectObj = this.btnTran.Find("img_on").gameObject;
            this.onSelectObj.SetActive(false);
            UIEventListener.Get(this.btnTran.gameObject).onClick = delegate (PointerEventData data)
            {
                this.click();
            };
        }

        public void SetSelect(bool value)
        {
            if (this.IsSelect == value)
            {
                return;
            }
            this.IsSelect = value;
            this.onSelectObj.SetActive(this.IsSelect);
        }

        private void click()
        {
            if (this.IsSelect)
            {
                return;
            }
            if (this.OnClick != null)
            {
                this.OnClick(this);
            }
        }

        private Transform btnTran;

        private GameObject onSelectObj;

        public uint JobKey;

        public LuaTable Config;

        public Action<UI_ChooseRole.JobSelectBtn> OnClick;

        private bool IsSelect;
    }
}
