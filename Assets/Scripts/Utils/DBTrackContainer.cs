using System;
using UnityEngine;

public class DBTrackContainer : MonoBehaviour
{
    public int Index
    {
        get
        {
            return this.index;
        }
        set
        {
            this.index = value;
        }
    }

    public Transform AffectedObject
    {
        get
        {
            if (this.affectedObject == null && this.affectedObjectPath != string.Empty)
            {
                GameObject gameObject = GameObject.Find(this.affectedObjectPath);
                if (gameObject)
                {
                    this.affectedObject = gameObject.transform;
                    foreach (DBTimelineBase dbtimelineBase in this.Timelines)
                    {
                        dbtimelineBase.LateBindAffectedObjectInScene(this.affectedObject);
                    }
                }
            }
            return this.affectedObject;
        }
        set
        {
            this.affectedObject = value;
            if (this.affectedObject != null)
            {
                this.affectedObjectPath = this.affectedObject.transform.GetFullHierarchyPath();
            }
            this.RenameTimelineContainer();
        }
    }

    public string AffectedObjectPath
    {
        get
        {
            return this.affectedObjectPath;
        }
        private set
        {
            this.affectedObjectPath = value;
        }
    }

    public void RenameTimelineContainer()
    {
        if (this.affectedObject)
        {
            base.name = "Timelines for " + this.affectedObject.name;
        }
    }

    public CutSceneContent CutSceneContent
    {
        get
        {
            if (this.cutscenecontent)
            {
                return this.cutscenecontent;
            }
            this.cutscenecontent = base.transform.parent.GetComponent<CutSceneContent>();
            return this.cutscenecontent;
        }
    }

    public DBTimelineBase[] Timelines
    {
        get
        {
            this.timelines = base.GetComponentsInChildren<DBTimelineBase>();
            return this.timelines;
        }
    }

    public void ProcessTimelines(float sequencerTime, float playbackRate)
    {
        if (!base.gameObject.activeInHierarchy)
        {
            return;
        }
        foreach (DBTimelineBase dbtimelineBase in this.Timelines)
        {
            dbtimelineBase.Process(sequencerTime, playbackRate);
        }
    }

    public void ResetCachedData()
    {
        for (int i = 0; i < this.Timelines.Length; i++)
        {
            this.Timelines[i].ResetCachedData();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private int index;

    [SerializeField]
    private Transform affectedObject;

    [SerializeField]
    private string affectedObjectPath;

    private CutSceneContent cutscenecontent;

    private DBTimelineBase[] timelines;
}
