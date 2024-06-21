using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CutSceneContent : MonoBehaviour
{
    public string Key
    {
        get
        {
            return this.key;
        }
        set
        {
            this.key = value;
        }
    }

    public Animator CameraAnimator
    {
        get
        {
            return this.m_cameraAnimator;
        }
        set
        {
            this.m_cameraAnimator = value;
            if (this.m_cameraAnimator != null)
            {
                AnimationClip animationClip = this.m_cameraAnimator.runtimeAnimatorController.animationClips[0];
                if (animationClip != null)
                {
                    this.Duration = animationClip.length;
                }
                Camera componentInChildren = this.m_cameraAnimator.gameObject.GetComponentInChildren<Camera>();
                this.bloom = componentInChildren.GetComponent<Bloom>();
            }
        }
    }

    public bool BloomEnabled
    {
        set
        {
            if (null != this.bloom && this.bloom.enabled != value)
            {
                this.bloom.enabled = value;
            }
        }
    }

    public float RunningTime
    {
        get
        {
            return this.runningTime;
        }
        set
        {
            this.runningTime = value;
            if (this.runningTime <= 0f)
            {
                this.runningTime = 0f;
            }
            if (this.runningTime > this.duration)
            {
                this.runningTime = this.duration;
            }
        }
    }

    public float Duration
    {
        get
        {
            return this.duration;
        }
        set
        {
            this.duration = value;
            if (this.duration <= 0f)
            {
                this.duration = 0.1f;
            }
        }
    }

    public float FrameRate
    {
        get
        {
            if (this.CameraAnimator != null)
            {
                AnimationClip animationClip = this.CameraAnimator.runtimeAnimatorController.animationClips[0];
                if (animationClip != null)
                {
                    return animationClip.frameRate;
                }
            }
            return 30f;
        }
    }

    public float PlaybackRate
    {
        get
        {
            return this.playbackRate;
        }
        set
        {
            this.playbackRate = value;
        }
    }

    public DBTrackContainer[] TimelineContainers
    {
        get
        {
            if (this.timeline == null)
            {
                this.timeline = base.GetComponentsInChildren<DBTrackContainer>();
            }
            return this.timeline;
        }
    }

    public void UpdateCharactorLighInfo(string lightname)
    {
        for (int i = 0; i < this.allLights.Count; i++)
        {
            if (this.allLights[i].name == lightname)
            {
                ManagerCenter.Instance.GetManager<CutSceneManager>().UpdateCutSceneRoleRenderInfo(this.allLights[i].color, this.allLights[i].transform.forward, this.allLights[i].intensity);
                break;
            }
        }
    }

    public bool IsPlaying
    {
        get
        {
            return this.playing;
        }
        set
        {
            this.playing = value;
            this.SetAnimaSpeed();
        }
    }

    private void Awake()
    {
        Camera componentInChildren = this.m_cameraAnimator.gameObject.GetComponentInChildren<Camera>();
        this.bloom = componentInChildren.GetComponent<Bloom>();
    }

    private void Start()
    {
    }

    public Transform GetObjectByIndex(int index)
    {
        if (this.Listanimator.Count <= index)
        {
            return null;
        }
        return this.Listanimator[index].transform;
    }

    public void Play()
    {
        this.IsPlaying = true;
        Scheduler.Instance.AddFrame(1U, false, delegate
        {
            if (null == base.gameObject)
            {
                return;
            }
            base.gameObject.SetActive(true);
            for (int i = 0; i < this.Listanimator.Count; i++)
            {
                this.Listanimator[i].speed = 0f;
                this.Listanimator[i].Play(0);
            }
            if (this.CameraAnimator != null)
            {
                this.CameraAnimator.speed = 0f;
                this.CameraAnimator.Play(0);
            }
        });
    }

    public void SetAnimaSpeed()
    {
        if (this.IsPlaying)
        {
            if (this.CameraAnimator != null)
            {
                this.CameraAnimator.speed = 0f;
            }
            for (int i = 0; i < this.Listanimator.Count; i++)
            {
                this.Listanimator[i].speed = 0f;
            }
        }
        else
        {
            if (this.CameraAnimator != null)
            {
                this.CameraAnimator.speed = 0f;
            }
            for (int j = 0; j < this.Listanimator.Count; j++)
            {
                this.Listanimator[j].speed = 0f;
            }
        }
    }

    private void Update()
    {
        if (this.IsPlaying)
        {
            this.UpdateSequencer(Time.deltaTime);
        }
    }

    public void UpdateSequencer(float deltaTime)
    {
        deltaTime *= this.playbackRate;
        if (this.playing)
        {
            this.runningTime += deltaTime;
            float num = this.runningTime;
            if (num <= 0f)
            {
                num = 0f;
            }
            if (num > this.Duration)
            {
                num = this.Duration;
            }
            foreach (DBTrackContainer dbtrackContainer in this.TimelineContainers)
            {
                dbtrackContainer.ProcessTimelines(num, this.PlaybackRate);
            }
            bool flag = false;
            if (this.playbackRate > 0f && this.RunningTime >= this.duration)
            {
                flag = true;
            }
            if (this.playbackRate < 0f && this.RunningTime <= 0f)
            {
                flag = true;
            }
            if (flag)
            {
                this.playing = false;
                this.End();
            }
        }
        for (int j = 0; j < this.Listanimator.Count; j++)
        {
            this.SetAnimatorFrame(this.Listanimator[j], this.RunningTime);
        }
        this.SetAnimatorFrame(this.CameraAnimator, this.RunningTime);
        if (this.OnCutSceneUpdate != null)
        {
            this.OnCutSceneUpdate(this.RunningTime);
        }
    }

    private void SetAnimatorFrame(Animator animator, float time)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            AnimationClip animationClip = animator.runtimeAnimatorController.animationClips[0];
            float num = 0.0333333351f;
            int num2 = (int)(time / num);
            float num3 = (float)num2 * num;
            float normalizedTime = num3 / animationClip.length;
            animator.speed = 0f;
            animator.CrossFade(animationClip.name, 0f, 0, normalizedTime);
            foreach (AnimationEvent animationEvent in animationClip.events)
            {
                if (Mathf.Abs(animationEvent.time - time) < 0.01f)
                {
                    animator.SendMessage(animationEvent.functionName, animationEvent.objectReferenceParameter);
                }
            }
        }
    }

    public void LoadAssetsFinish()
    {
        this.building2hide.Clear();
        this.buildinglayer2hide.Clear();
        for (int i = 0; i < this.ListBuilding2Hide.Count; i++)
        {
            GameObject gameObject = GameObject.Find("Scene/ZoneRoot_0/NonStatic_0/" + this.ListBuilding2Hide[i].ToString());
            if (null != gameObject)
            {
                this.buildinglayer2hide.Add(gameObject.layer);
                CommonTools.SetGameObjectLayer(gameObject, "MainPlayer", true);
                this.building2hide.Add(gameObject);
            }
            gameObject = GameObject.Find("Scene/ZoneRoot_0/Static_0/" + this.ListBuilding2Hide[i].ToString());
            if (null != gameObject)
            {
                this.buildinglayer2hide.Add(gameObject.layer);
                CommonTools.SetGameObjectLayer(gameObject, "MainPlayer", true);
                this.building2hide.Add(gameObject);
            }
            gameObject = GameObject.Find("Scene/ZoneRoot_0/Static_0/batchZone0/" + this.ListBuilding2Hide[i].ToString());
            if (null != gameObject)
            {
                this.buildinglayer2hide.Add(gameObject.layer);
                CommonTools.SetGameObjectLayer(gameObject, "MainPlayer", true);
                this.building2hide.Add(gameObject);
            }
        }
        this.bLoadFinish = true;
        if (this.OnLoadAsstesFinish != null)
        {
            this.OnLoadAsstesFinish();
        }
        this.Play();
        if (null != this.m_cameraAnimator)
        {
            LODManager.Instance.SetLodCustomCamera(this.m_cameraAnimator.gameObject.GetComponentInChildren<Camera>());
        }
    }

    public void End()
    {
        if (this.PlaybackFinished != null)
        {
            this.PlaybackFinished();
        }
        foreach (DBTrackContainer dbtrackContainer in this.TimelineContainers)
        {
            foreach (DBTimelineBase dbtimelineBase in dbtrackContainer.Timelines)
            {
                if (dbtimelineBase.AffectedObject != null)
                {
                    dbtimelineBase.EndTimeline();
                }
            }
        }
        if (this.UnLoadAssets != null)
        {
            this.UnLoadAssets();
        }
        for (int k = 0; k < this.building2hide.Count; k++)
        {
            CommonTools.SetGameObjectLayer(this.building2hide[k], this.buildinglayer2hide[k], true);
        }
        this.building2hide.Clear();
        this.buildinglayer2hide.Clear();
        LODManager.Instance.SetLodCustomCamera(null);
    }

    public void SetAssets()
    {
        this.ListModelsNames.Clear();
        this.ListControllerNames.Clear();
        for (int i = 0; i < this.UseCharacters.Count; i++)
        {
            if (this.UseCharacters[i])
            {
                string item = string.Empty;
                Animator component = this.UseCharacters[i].GetComponent<Animator>();
                if (component && component.runtimeAnimatorController)
                {
                    item = component.runtimeAnimatorController.name;
                }
                string name = this.UseCharacters[i].name;
                this.ListControllerNames.Add(item);
                this.ListModelsNames.Add(name);
            }
            else
            {
                this.ListControllerNames.Add(string.Empty);
                this.ListModelsNames.Add(string.Empty);
            }
        }
    }

    public DBTrackContainer CreateNewTimelineContainer(Transform affectedObject)
    {
        DBTrackContainer dbtrackContainer = new GameObject("DBTrack For" + affectedObject.name)
        {
            transform =
            {
                parent = base.transform
            }
        }.AddComponent<DBTrackContainer>();
        dbtrackContainer.AffectedObject = affectedObject;
        int num = 0;
        foreach (DBTrackContainer dbtrackContainer2 in this.TimelineContainers)
        {
            if (dbtrackContainer2.Index > num)
            {
                num = dbtrackContainer2.Index;
            }
        }
        dbtrackContainer.Index = num + 1;
        return dbtrackContainer;
    }

    private string key = string.Empty;

    public List<GameObject> UseCharacters = new List<GameObject>();

    [HideInInspector]
    public List<Animator> Listanimator = new List<Animator>();

    [SerializeField]
    private Animator m_cameraAnimator;

    public Action LoadAssets;

    private Bloom bloom;

    private DBTrackContainer[] timeline;

    public Action PlaybackFinished = delegate ()
    {
    };

    public Action UnLoadAssets = delegate ()
    {
    };

    [HideInInspector]
    public bool bLoadFinish;

    public Action OnLoadAsstesFinish = delegate ()
    {
    };

    public Action<float> OnCutSceneUpdate;

    [SerializeField]
    private float runningTime;

    [SerializeField]
    private float duration = 10f;

    public List<string> ListModelsNames = new List<string>();

    public List<string> ListControllerNames = new List<string>();

    private float playbackRate = 1f;

    public List<string> ListBuilding2Hide = new List<string>();

    private List<GameObject> building2hide = new List<GameObject>();

    private List<int> buildinglayer2hide = new List<int>();

    public List<Light> allLights = new List<Light>();

    private bool playing;
}
