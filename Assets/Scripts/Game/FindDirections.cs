using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class FindDirections : IFFComponent
{
    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.RefreshCopyTask();
    }

    public void CompDispose()
    {
    }

    private TaskUIController taskUIController
    {
        get
        {
            return ControllerManager.Instance.GetController<TaskUIController>();
        }
    }

    public void CompUpdate()
    {
        this.getDirection();
    }

    public void RefreshCopyTask()
    {
        FirstCopyInfo firstCopyTask = this.taskUIController.GetFirstCopyTask();
        if (!firstCopyTask.isEmpty)
        {
            this.GetCopyTaskConfig(firstCopyTask);
        }
        else
        {
            this.Reset();
        }
    }

    public void Reset()
    {
        this.targetWithString = string.Empty;
        this.maxDistance = this.arriveMaxDistance;
        if (this.HaveDirectEffect)
        {
            FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
            component.RemoveEffectGroup(this.directionEffectName);
            this.HaveDirectEffect = false;
        }
    }

    private void GetCopyTaskConfig(FirstCopyInfo info)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("questconfig", (ulong)info.id);
        if (configTable == null)
        {
            this.targetWithString = string.Empty;
            this.lasttargetStr = string.Empty;
            return;
        }
        this.dicStatePathId.Clear();
        string[] array = configTable.GetCacheField_String("pathwaydoing").Split(new char[]
        {
            ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split(new char[]
            {
                '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            uint key = (uint)int.Parse(array2[0]);
            uint value = (uint)int.Parse(array2[1]);
            this.dicStatePathId[key] = value;
        }
        this.pathway = null;
        if (this.dicStatePathId.ContainsKey(info.state))
        {
            this.pathway = LuaConfigManager.GetConfigTable("pathway", (ulong)this.dicStatePathId[info.state]);
        }
        else if (this.dicStatePathId.ContainsKey(1U))
        {
            this.pathway = LuaConfigManager.GetConfigTable("pathway", 1UL);
        }
        if (this.pathway != null)
        {
            this.targetWithString = this.pathway.GetCacheField_String("coordinates");
        }
        else
        {
            this.Reset();
        }
    }

    private void getDirection()
    {
        if (string.IsNullOrEmpty(this.targetWithString))
        {
            return;
        }
        if (!this.targetWithString.Equals(this.lasttargetStr))
        {
            this.lasttargetStr = this.targetWithString;
            this.target = ClassPlus.GetVector2WithString(this.targetWithString);
            this.maxDistance = this.notArriveMaxDistance;
        }
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(this.target);
        FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
        Vector2 a = new Vector2(MainPlayer.Self.ModelObj.transform.position.x, MainPlayer.Self.ModelObj.transform.position.z);
        Vector2 b = new Vector2(worldPosByServerPos.x, worldPosByServerPos.z);
        if (Vector2.Distance(a, b) > (float)this.maxDistance)
        {
            if (!this.HaveDirectEffect)
            {
                component.AddEffectGroup(this.directionEffectName);
                this.HaveDirectEffect = true;
                this.maxDistance = this.notArriveMaxDistance;
            }
            this.RotateEffect(worldPosByServerPos - MainPlayer.Self.ModelObj.transform.position);
        }
        else if (this.HaveDirectEffect)
        {
            component.RemoveEffectGroup(this.directionEffectName);
            this.HaveDirectEffect = false;
            this.maxDistance = this.arriveMaxDistance;
        }
    }

    public void RotateEffect(Vector3 Dir)
    {
        FFEffectControl component = MainPlayer.Self.GetComponent<FFEffectControl>();
        Quaternion rotation = Quaternion.LookRotation(Dir);
        rotation = new Quaternion(0f, rotation.y, 0f, rotation.w);
        component.RotateEffectGroup(this.directionEffectName, rotation);
    }

    public void ResetComp()
    {
    }

    private int maxDistance = 6;

    private bool haveArrive;

    private int arriveMaxDistance = 30;

    private int notArriveMaxDistance = 6;

    public string directionEffectName = "eg085";

    public bool HaveDirectEffect;

    private Dictionary<uint, uint> dicStatePathId = new Dictionary<uint, uint>();

    private LuaTable pathway;

    private string targetWithString = string.Empty;

    private Vector2 target = Vector2.zero;

    private string lasttargetStr = string.Empty;
}
