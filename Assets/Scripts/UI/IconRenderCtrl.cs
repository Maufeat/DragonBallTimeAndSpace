using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using UnityEngine;
using UnityEngine.UI;

public class IconRenderCtrl : MonoBehaviour
{
    public PlayerCharactorCreateHelper ModelHelpHolder
    {
        get
        {
            return this.modelHelpHolder;
        }
        set
        {
            this.modelHelpHolder = value;
        }
    }

    public GameObject targetPrefab
    {
        get
        {
            return this.targetPrefab_;
        }
        set
        {
            this.targetPrefab_ = value;
        }
    }

    private GameScene gs
    {
        get
        {
            if (this.gs_ == null)
            {
                this.gs_ = ManagerCenter.Instance.GetManager<GameScene>();
            }
            return this.gs_;
        }
    }

    private void Awake()
    {
        this.rawImage = base.GetComponent<RawImage>();
        if (this.rawImage != null)
        {
            Color color = this.rawImage.color;
            color.a = 0f;
            this.rawImage.color = color;
        }
        if (this.gs != null)
        {
            this.gs.RegOnSceneLoadCallBack(new Action(this.OnSceneLoad));
        }
        IconRenderCtrl.listControl.Add(this);
    }

    public void CheckInLstControl()
    {
        if (IconRenderCtrl.listControl.Contains(this))
        {
            return;
        }
        IconRenderCtrl.listControl.Add(this);
    }

    public static void DisposeBonePObj()
    {
        for (int i = 0; i < IconRenderCtrl.listControl.Count; i++)
        {
            if (!(null == IconRenderCtrl.listControl[i]))
            {
                if (IconRenderCtrl.listControl[i].modelHelpHolder != null)
                {
                    IconRenderCtrl.listControl[i].modelHelpHolder.DisposeBonePObj();
                }
                IconRenderCtrl.listControl[i].modelHelpHolder = null;
            }
        }
    }

    public static void Reset()
    {
        IconRenderCtrl.DisposeBonePObj();
        IconRenderCtrl.listControl.Clear();
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null && null != controller.mainView)
        {
            controller.mainView.ResetLastIconList();
        }
    }

    private void Update()
    {
        if (this.startTimer > 0)
        {
            this.startTimer--;
            return;
        }
        if (Time.frameCount % 3 == 0)
        {
            this.RenderRT();
            if (this.colorToNormalProgress < 1f)
            {
                this.colorToNormalProgress += Time.deltaTime * 3f;
                Color color = this.rawImage.color;
                color.a = this.colorToNormalProgress;
                this.rawImage.color = color;
            }
        }
    }

    private void RenderRT()
    {
        if (this.target != null && this.cameraRT != null)
        {
            this.cameraRT.transform.position = this.camWorldPos;
            this.cameraRT.targetTexture = this.rt;
            if (!this.target.activeInHierarchy)
            {
                this.target.SetActive(true);
            }
            this.target.transform.position = this.cameraRT.transform.TransformPoint(this.localPos);
            this.cameraRT.Render();
        }
    }

    private void OnDisable()
    {
        if (this.target != null && this.cameraRT != null)
        {
            this.cameraRT.transform.position = this.camWorldPos;
            this.cameraRT.targetTexture = this.rt;
            this.target.SetActive(true);
            this.target.transform.position = this.cameraRT.transform.TransformPoint(this.localPos + Vector3.up * 10f);
            this.cameraRT.Render();
        }
    }

    public void ReStart()
    {
        this.startTimer = 1;
    }

    private void OnEnable()
    {
        if (this.target != null)
        {
            this.target.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (this.rt != null)
        {
            RenderTexture.ReleaseTemporary(this.rt);
            this.rt = null;
        }
        if (this.modelHelpHolder != null)
        {
            this.modelHelpHolder.DisposeBonePObj();
        }
        this.modelHelpHolder = null;
        this.target = null;
        if (this.gs != null)
        {
            this.gs.UnRegOnSceneLoadCallBack(new Action(this.OnSceneLoad));
        }
    }

    private void OnSceneLoad()
    {
        if (this.target != null && this.gs != null)
        {
            this.gs.SetMatLightInfo(this.target, true);
        }
    }

    private PlayerCharactorCreateHelper modelHelpHolder;

    private static List<IconRenderCtrl> listControl = new List<IconRenderCtrl>();

    public RenderTexture rt;

    public GameObject targetPrefab_;

    public GameObject target;

    public Camera cameraRT;

    public Vector3 localPos;

    public Vector3 localRot;

    public Vector3 camWorldPos;

    private RawImage rawImage;

    private GameScene gs_;

    private float colorToNormalProgress;

    private int startTimer = 1;
}
