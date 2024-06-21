using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoleUIAnimationControl : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject controlTarget
    {
        get
        {
            return this.controlTarget_;
        }
        set
        {
            this.controlTarget_ = value;
            this.animator = this.controlTarget_.GetComponent<Animator>();
            this.rac = this.animator.runtimeAnimatorController;
            this.startTimer = 0f;
            this.bornAniName = this.rac.animationClips[0].name;
            for (int i = 0; i < this.rac.animationClips.Length; i++)
            {
                if (this.rac.animationClips[i].name.Contains("born"))
                {
                    this.bornAniName = this.rac.animationClips[i].name;
                }
            }
            this.StartModelMoveInAnimation();
            if (!(this.oldTarget != null) || this.oldTarget != value)
            {
            }
            this.PlayAnimationByKewords(this.bornAniName, 0f);
            this.PlayHeroOnShow();
            this.PlayEffect();
            this.oldTarget = value;
        }
    }

    private void PlayHeroOnShow()
    {
        this.camera_beginpos = Camera.main.transform.position;
        this.PlayEffect();
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PlayCamera));
        Scheduler.Instance.AddTimer(0.65f, false, new Scheduler.OnScheduler(this.PlayCamera));
    }

    private void PlayEffect()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PlayEffect1));
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PlayEffect2));
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PlayEffect3));
        Scheduler.Instance.AddTimer(0.5f, false, new Scheduler.OnScheduler(this.PlayEffect1));
        Scheduler.Instance.AddTimer(0.65f, false, new Scheduler.OnScheduler(this.PlayEffect2));
        if (this.curHeroid == 70025U)
        {
            Scheduler.Instance.AddTimer(0.65f, false, new Scheduler.OnScheduler(this.PlayEffect3));
        }
    }

    private void PlayEffect1()
    {
        if (this.objEffect1 != null)
        {
            UnityEngine.Object.Destroy(this.objEffect1);
        }
        string name = "lz_syr_sjyd01";
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj(name, delegate
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj(name);
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    ManagerCenter.Instance.GetManager<FFEffectManager>().SetObjectPoolUnit(0UL, name, OIP);
                    foreach (object obj in this.controlTarget.transform)
                    {
                        Transform transform = (Transform)obj;
                        if (transform.name == name)
                        {
                            UnityEngine.Object.Destroy(transform.gameObject);
                        }
                    }
                    this.objEffect1 = OIP.ItemObj;
                    this.objEffect1.name = name;
                    this.objEffect1.transform.SetParent(this.controlTarget.transform);
                    this.objEffect1.transform.localPosition = Vector3.zero;
                    ParticleSystem componentInChildren = this.objEffect1.GetComponentInChildren<ParticleSystem>();
                    componentInChildren.playOnAwake = false;
                    componentInChildren.Play();
                });
            }
        });
    }

    private void PlayEffect2()
    {
        if (this.objEffect2 != null)
        {
            UnityEngine.Object.Destroy(this.objEffect2);
        }
        string name = "lz_buff_dengchang";
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj(name, delegate
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj(name);
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    ManagerCenter.Instance.GetManager<FFEffectManager>().SetObjectPoolUnit(0UL, name, OIP);
                    foreach (object obj in this.controlTarget.transform)
                    {
                        Transform transform = (Transform)obj;
                        if (transform.name == name)
                        {
                            UnityEngine.Object.Destroy(transform.gameObject);
                        }
                    }
                    this.objEffect2 = OIP.ItemObj;
                    this.objEffect2.name = name;
                    this.objEffect2.transform.SetParent(this.controlTarget.transform);
                    this.objEffect2.transform.localPosition = Vector3.zero;
                    ParticleSystem componentInChildren = this.objEffect2.GetComponentInChildren<ParticleSystem>();
                    componentInChildren.playOnAwake = false;
                    componentInChildren.Play();
                });
            }
        });
    }

    private void PlayEffect3()
    {
        if (this.objEffect3 != null)
        {
            UnityEngine.Object.Destroy(this.objEffect3);
        }
        string name = "lz_buff_dengchang01";
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj(name, delegate
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj(name);
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    ManagerCenter.Instance.GetManager<FFEffectManager>().SetObjectPoolUnit(0UL, name, OIP);
                    foreach (object obj in this.controlTarget.transform)
                    {
                        Transform transform = (Transform)obj;
                        if (transform.name == name)
                        {
                            UnityEngine.Object.Destroy(transform.gameObject);
                        }
                    }
                    this.objEffect3 = OIP.ItemObj;
                    this.objEffect3.name = name;
                    this.objEffect3.transform.SetParent(this.controlTarget.transform);
                    this.objEffect3.transform.localPosition = Vector3.zero;
                    ParticleSystem componentInChildren = this.objEffect3.GetComponentInChildren<ParticleSystem>();
                    componentInChildren.playOnAwake = false;
                    componentInChildren.Play();
                });
            }
        });
    }

    private void PlayCamera()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CameraDown));
        if (Camera.main == null)
        {
            return;
        }
        TweenPosition.Begin(Camera.main.gameObject, 0.1f, this.camera_beginpos + new Vector3(0f, 0.1f, 0f));
        Scheduler.Instance.AddTimer(0.1f, false, new Scheduler.OnScheduler(this.CameraDown));
    }

    private void CameraDown()
    {
        TweenPosition.Begin(Camera.main.gameObject, 0.1f, this.camera_beginpos);
    }

    public GameObject oldTarget
    {
        get
        {
            return this.oldTarget_;
        }
        set
        {
            this.oldTarget_ = value;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.downState = true;
        this.mouseDownPos = eventData.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isEnter = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.downState = false;
        this.lastMousePos = null;
    }

    private bool isEnter
    {
        get
        {
            return this.isEnter_;
        }
        set
        {
            this.isEnter_ = value;
            if (!this.isEnter_ || !string.IsNullOrEmpty(this.bornAniName))
            {
            }
        }
    }

    public bool OnDelete
    {
        get
        {
            return this.onDelete;
        }
        set
        {
            this.onDelete = value;
        }
    }

    private void Update()
    {
        if (this.IsInTweenAnimation())
        {
            return;
        }
        this.startTimer += Time.deltaTime;
        if (this.startTimer > this.switchStateInterval)
        {
            if (this.rac != null && !this.isEnter && !this.onDelete)
            {
                this.PlayAnimationByKewords("$normalidle", 0.2f);
            }
            this.startTimer = 0f;
        }
        if (this.animator != null && this.animator != null && this.playingTimer < 0f)
        {
            AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
            this.PlayAnimationByKewords("$normalidle", 0.2f);
        }
        else
        {
            this.playingTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            this.downState = false;
            this.lastMousePos = null;
        }
        if (this.downState)
        {
            Vector3? vector = this.lastMousePos;
            if (vector == null)
            {
                this.lastMousePos = new Vector3?(Input.mousePosition);
                return;
            }
            Vector3 mousePosition = Input.mousePosition;
            Vector3? vector2 = this.lastMousePos;
            Vector3 vector3 = mousePosition - vector2.Value;
            if (this.controlTarget && !this.onDelete)
            {
                this.controlTarget.transform.Rotate(Vector3.up, -vector3.x * Time.deltaTime * 0.5f);
            }
            this.lastMousePos = new Vector3?(Input.mousePosition);
        }
        if (this.isEnter && this.controlTarget)
        {
            UI_SelectRole uiobject = UIManager.GetUIObject<UI_SelectRole>();
            if (uiobject == null)
            {
                return;
            }
            if (uiobject.onSelectPage)
            {
                return;
            }
            float axis = Input.GetAxis("Mouse ScrollWheel");
            this.distToChange += Time.deltaTime * axis * 600f;
            float num = (this.distToChange <= 0f) ? (-this.distToChange) : this.distToChange;
            if (num > 0f)
            {
                float num2;
                if (num > 0.15f)
                {
                    num2 = this.distToChange / 40f;
                }
                else
                {
                    num2 = this.distToChange / 15f;
                }
                this.distToChange -= num2;
                float num3 = this.camMain.transform.localPosition.z;
                num3 -= num2;
                num3 = Mathf.Clamp(num3, this.targetDistLimit[0], this.targetDistLimit[1]);
                Vector3 localPosition = this.controlTarget.transform.localPosition;
                float t = 1f - (num3 - this.targetDistLimit[0]) / (this.targetDistLimit[1] - this.targetDistLimit[0]);
                float y = Mathf.Lerp(this.camStartHeigth, this.camEndHeigth, t);
                localPosition.z = num3;
                localPosition.y = y;
                this.camMain.transform.position = localPosition;
            }
        }
    }

    private bool IsInTweenAnimation()
    {
        if (this.controlTarget != null)
        {
            if (this.targetTP == null)
            {
                this.targetTP = this.controlTarget.GetComponent<TweenPosition>();
            }
            if (this.targetTP != null && this.targetTP.enabled)
            {
                return true;
            }
        }
        return false;
    }

    private float PlayAnimationByKewords(string keyWords, float transationDuaration = 0f)
    {
        this.aniNames.Clear();
        this.curAnilength = 0f;
        for (int i = 0; i < this.rac.animationClips.Length; i++)
        {
            if (this.rac.animationClips[i].name.ToLower().Contains(keyWords.ToLower()))
            {
                this.aniNames.Add(this.rac.animationClips[i].name);
            }
        }
        if (this.aniNames.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, this.aniNames.Count);
            this.curPlayAniName = this.aniNames[index];
        }
        if (this.animator != null)
        {
            this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            this.animator.CrossFade(this.curPlayAniName, transationDuaration, 0, 0f);
        }
        for (int j = 0; j < this.rac.animationClips.Length; j++)
        {
            if (this.rac.animationClips[j].name.Equals(this.curPlayAniName))
            {
                this.curAnilength = this.rac.animationClips[j].length;
            }
        }
        if (this.curAnilength > 0.05f)
        {
            this.switchStateInterval = this.curAnilength;
        }
        this.playingTimer = this.curAnilength;
        return this.curAnilength;
    }

    public void PlayStartAnimationAndDoActon(Scheduler.OnScheduler action, float delayTime)
    {
        float num = this.PlayAnimationByKewords("attack", 0f);
        num -= 0.1f;
        num = Mathf.Max(delayTime, num);
        Scheduler.Instance.AddTimer(num, false, action);
    }

    public void StartModelMoveInAnimation()
    {
        if (this.controlTarget == null)
        {
            return;
        }
        this.TargetMoveByTween(this.controlTarget, this.modelStartPos, this.modelDestPos, 0.2f, false);
    }

    public void StartModelMoveOutAnimation()
    {
        if (this.oldTarget == null)
        {
            return;
        }
        this.TargetMoveByTween(this.oldTarget, this.oldTarget.transform.position, this.modelOutPos, 0.15f, true);
    }

    private void TargetMoveByTween(GameObject target, Vector3 startPos, Vector3 endPos, float len, bool isOut = false)
    {
        target.transform.position = startPos;
        TweenPosition.Begin(target, len, endPos);
        TweenPosition component = target.GetComponent<TweenPosition>();
        component.onFinished = delegate (UITweener obj)
        {
            if (isOut && target)
            {
                target.SetActive(false);
            }
        };
    }

    private const float BEGIN_TIME = 0.5f;

    private const float ONLINE_TIME = 0.65f;

    private const float half_cost_time = 0.1f;

    private bool downState;

    private Vector3 mouseDownPos;

    private Vector3? lastMousePos;

    public Vector3 modelDestPos;

    public Vector3 modelStartPos;

    public Vector3 modelOutPos;

    public float camStartHeigth;

    public float camEndHeigth;

    private GameObject controlTarget_;

    private string curAnimationName;

    public float[] targetDistLimit = new float[]
    {
        3f,
        5f
    };

    public Camera camMain;

    public uint curHeroid;

    private Vector3 camera_beginpos;

    private GameObject objEffect1;

    private GameObject objEffect2;

    private GameObject objEffect3;

    private GameObject oldTarget_;

    public RuntimeAnimatorController rac;

    public Animator animator;

    private float startTimer;

    private float playingTimer = float.MaxValue;

    private float switchStateInterval = 5f;

    private string curPlayAniName = string.Empty;

    private string bornAniName = string.Empty;

    private float curAnilength;

    private float aniEndTime;

    private bool isEnter_;

    private bool onDelete;

    public float distToChange;

    private TweenPosition targetTP;

    private List<string> aniNames = new List<string>();
}
