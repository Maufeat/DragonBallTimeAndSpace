using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetPlayerDirTest : UIPanelBase
{
    public override void OnDispose()
    {
        base.Dispose();
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.uiRoot = root;
        this.InitObj();
        this.InitEvent();
        this.SetCurValue();
    }

    private void InitObj()
    {
        this.sliderDir = this.uiRoot.Find("Offset/Panel_root/Slider_dir");
        this.sliderHeight = this.uiRoot.Find("Offset/Panel_root/Slider_height");
        this.sliderDist = this.uiRoot.Find("Offset/Panel_root/Slider_dist");
        this.inputResult = this.uiRoot.Find("Offset/Panel_root/InputField");
        this.btnClose = this.uiRoot.Find("Offset/Panel_root/Button_close");
    }

    private void InitEvent()
    {
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        float cacheField_Float = xmlConfigTable.GetCacheField_Table("CameraMaxdistance").GetCacheField_Float("value");
        float cacheField_Float2 = xmlConfigTable.GetCacheField_Table("CameraMindistance").GetCacheField_Float("value");
        float cacheField_Float3 = xmlConfigTable.GetCacheField_Table("CameraMaxAngle").GetCacheField_Float("value");
        float cacheField_Float4 = xmlConfigTable.GetCacheField_Table("CameraMinAngle").GetCacheField_Float("value");
        Button component = this.btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            UIManager.Instance.DeleteUI<UI_SetPlayerDirTest>();
        });
        Slider slider_dir = this.sliderDir.GetComponent<Slider>();
        slider_dir.minValue = 0f;
        slider_dir.maxValue = 180f;
        slider_dir.onValueChanged.RemoveAllListeners();
        slider_dir.onValueChanged.AddListener(delegate (float f)
        {
            if (!this.isUpdateDirAction)
            {
                this.isUpdateDirAction = true;
                return;
            }
            slider_dir.transform.Find("Text").GetComponent<Text>().text = f.ToString("f1");
            this.dir = f;
            this.UpdateResultData();
        });
        Slider slider_height = this.sliderHeight.GetComponent<Slider>();
        slider_height.minValue = cacheField_Float4;
        slider_height.maxValue = cacheField_Float3;
        slider_height.onValueChanged.RemoveAllListeners();
        slider_height.onValueChanged.AddListener(delegate (float f)
        {
            if (!this.isUpdateHeightAngleAction)
            {
                this.isUpdateHeightAngleAction = true;
                return;
            }
            slider_height.transform.Find("Text").GetComponent<Text>().text = f.ToString("f1");
            this.heightAngle = f;
            this.UpdateResultData();
        });
        Slider slider_dist = this.sliderDist.GetComponent<Slider>();
        slider_dist.minValue = cacheField_Float2;
        slider_dist.maxValue = ControllerManager.Instance.GetController<SystemSettingController>().GetMaxCameraDistance();
        slider_dist.onValueChanged.RemoveAllListeners();
        slider_dist.onValueChanged.AddListener(delegate (float f)
        {
            if (!this.isUpdateDistAction)
            {
                this.isUpdateDistAction = true;
                return;
            }
            slider_dist.transform.Find("Text").GetComponent<Text>().text = f.ToString("f1");
            this.dist = f;
            this.UpdateResultData();
        });
    }

    private void SetCurValue()
    {
        Slider component = this.sliderDir.GetComponent<Slider>();
        component.value = MainPlayer.Self.FaceDir;
        component.transform.Find("Text").GetComponent<Text>().text = component.value.ToString("f1");
        Slider component2 = this.sliderHeight.GetComponent<Slider>();
        CameraController cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
        if (cameraController)
        {
            CameraFollowTarget4 cameraFollowTarget = cameraController.CurrState as CameraFollowTarget4;
            if (cameraFollowTarget != null)
            {
                float verticleAngleBetweenCamAndPlayer = cameraFollowTarget.GetVerticleAngleBetweenCamAndPlayer();
                component2.value = verticleAngleBetweenCamAndPlayer;
                component2.transform.Find("Text").GetComponent<Text>().text = component2.value.ToString("f1");
            }
        }
        Slider component3 = this.sliderDist.GetComponent<Slider>();
        component3.value = CameraFollowTarget4.targetdistance;
        component3.transform.Find("Text").GetComponent<Text>().text = component3.value.ToString("f1");
        this.UpdateUIShow();
        this.dir = component.value;
        this.heightAngle = component.value;
        this.dist = component3.value;
    }

    private void UpdateResultData()
    {
        this.UpdateUIShow();
        this.SetResultToCamAndPlayer((uint)this.dir, this.dist, this.heightAngle);
    }

    private void UpdateUIShow()
    {
        this.result = string.Concat(new object[]
        {
            this.dir.ToInt(),
            ",",
            this.dist.ToString("f1"),
            ",",
            this.heightAngle.ToString("f1")
        });
        this.inputResult.GetComponent<InputField>().text = this.result;
    }

    private CameraController cc
    {
        get
        {
            if (this.cc_ == null)
            {
                this.cc_ = UnityEngine.Object.FindObjectOfType<CameraController>();
            }
            return this.cc_;
        }
    }

    private void SetResultToCamAndPlayer(uint dir, float dist, float angleVer)
    {
        if (this.cc != null)
        {
            CameraFollowTarget4 cameraFollowTarget = this.cc.CurrState as CameraFollowTarget4;
            if (cameraFollowTarget != null)
            {
                cameraFollowTarget.SetCameraDirDistAngleV(dir, dist, angleVer);
            }
        }
    }

    private Transform uiRoot;

    private Transform sliderDir;

    private Transform sliderHeight;

    private Transform sliderDist;

    private Transform inputResult;

    private Transform btnClose;

    private string result = string.Empty;

    private float dir;

    private float heightAngle;

    private float dist;

    private bool isUpdateDirAction;

    private bool isUpdateHeightAngleAction;

    private bool isUpdateDistAction;

    private CameraController cc_;
}
