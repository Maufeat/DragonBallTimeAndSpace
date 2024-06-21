using System;
using System.Collections.Generic;
using map;
using UnityEngine;

public class AreaSoundTriggerTool : MonoBehaviour
{
    public static AreaSoundTriggerTool Instance
    {
        get
        {
            if (AreaSoundTriggerTool.instance == null)
            {
                GameObject gameObject = new GameObject(typeof(AreaSoundTriggerTool).Name, new Type[]
                {
                    typeof(AreaSoundTriggerTool)
                });
                gameObject.transform.Reset();
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                AreaSoundTriggerTool.instance = gameObject.GetComponent<AreaSoundTriggerTool>();
            }
            return AreaSoundTriggerTool.instance;
        }
    }

    public void ResetTriggers()
    {
        if (this.info != null && this.playingSound > 0)
        {
            for (int i = 0; i < this.info.list.Count; i++)
            {
                TriggerAreaInfo triggerAreaInfo = this.info.list[i];
                bool flag = (this.playingSound & 1 << i) == 1;
                if (flag)
                {
                    this.StopPlayTriggerSound(triggerAreaInfo.sound, i);
                }
            }
        }
        this.playingSound = 0;
        this.triggers.Clear();
        this.SoundObjDic.Clear();
        if (this.TriggerAreaSoundParent != null)
        {
            UnityEngine.Object.DestroyImmediate(this.TriggerAreaSoundParent);
        }
        this.info = MapLoader.triggerAreaSoundInfo;
        if (this.info == null)
        {
            return;
        }
        for (int j = 0; j < this.info.list.Count; j++)
        {
            this.triggers.Add(new List<SoundTrigger>());
        }
        this.TriggerAreaSoundParent = new GameObject("TriggerAreaSounds");
        this.TriggerAreaSoundParent.transform.SetParent(base.transform);
        this.TriggerAreaSoundParent.transform.Reset();
        for (int k = 0; k < this.info.list.Count; k++)
        {
            TriggerAreaInfo triggerAreaInfo2 = this.info.list[k];
            GameObject gameObject = new GameObject(triggerAreaInfo2.sound);
            gameObject.transform.SetParent(this.TriggerAreaSoundParent.transform);
            gameObject.transform.SetSiblingIndex(k);
            gameObject.transform.Reset();
            gameObject.name = string.Format("[{0}]:{1}", k, triggerAreaInfo2.sound);
            GameObject gameObject2 = new GameObject("Triggers");
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.transform.SetSiblingIndex(0);
            for (int l = 0; l < triggerAreaInfo2.triggers.Count; l++)
            {
                TriggerInfo triggerInfo = triggerAreaInfo2.triggers[l];
                GameObject gameObject3 = new GameObject(l.ToString(), new Type[]
                {
                    typeof(SoundTrigger)
                });
                gameObject3.transform.SetParent(gameObject2.transform);
                gameObject3.transform.SetSiblingIndex(l);
                gameObject3.transform.Reset();
                gameObject3.transform.localPosition = triggerInfo.pos;
                gameObject3.transform.eulerAngles = triggerInfo.euler;
                gameObject3.transform.localScale = triggerInfo.size;
                int type = triggerInfo.type;
                if (type != 0)
                {
                    if (type == 1)
                    {
                        GameObject gameObject4 = gameObject3;
                        gameObject4.name += "_Cube";
                        gameObject3.AddComponent<BoxCollider>().isTrigger = true;
                    }
                }
                else
                {
                    GameObject gameObject5 = gameObject3;
                    gameObject5.name += "_Sphere";
                    gameObject3.AddComponent<SphereCollider>().isTrigger = true;
                }
                SoundTrigger component = gameObject3.GetComponent<SoundTrigger>();
                component.SetData(k, l, new Action<SoundTrigger>(this.OnItemEnter), new Action<SoundTrigger>(this.OnItemExit));
            }
            GameObject gameObject6 = new GameObject("soundPosObj");
            gameObject6.transform.SetParent(gameObject.transform);
            gameObject6.transform.SetSiblingIndex(2);
            gameObject6.transform.Reset();
            this.SoundObjDic.Add(k, gameObject6);
            GameObject gameObject7 = new GameObject("Path");
            gameObject7.transform.SetParent(gameObject.transform);
            gameObject7.transform.SetSiblingIndex(1);
            for (int m = 0; m < triggerAreaInfo2.path.Count; m++)
            {
                Vector3 localPosition = triggerAreaInfo2.path[m];
                GameObject gameObject8 = new GameObject(m.ToString(), new Type[]
                {
                    typeof(SoundTrigger)
                });
                gameObject8.transform.SetParent(gameObject7.transform);
                gameObject8.transform.SetSiblingIndex(m);
                gameObject8.transform.Reset();
                gameObject8.transform.localPosition = localPosition;
                if (m == 0)
                {
                    gameObject6.transform.localPosition = localPosition;
                }
            }
        }
    }

    private void OnItemEnter(SoundTrigger trigger)
    {
        int getSoundIndex = trigger.getSoundIndex;
        if (this.triggers.Count <= getSoundIndex)
        {
            return;
        }
        this.triggers[getSoundIndex].Add(trigger);
    }

    private void OnItemExit(SoundTrigger trigger)
    {
        int getSoundIndex = trigger.getSoundIndex;
        if (this.triggers.Count <= getSoundIndex)
        {
            return;
        }
        this.triggers[getSoundIndex].Remove(trigger);
    }

    private void LateUpdate()
    {
        if (this.info == null || this.triggers.Count == 0)
        {
            return;
        }
        for (int i = 0; i < this.triggers.Count; i++)
        {
            TriggerAreaInfo triggerAreaInfo = this.info.list[i];
            bool flag = this.triggers[i].Count > 0;
            bool flag2 = (this.playingSound & 1 << i) == 1;
            if (flag == flag2)
            {
                if (flag2)
                {
                    this.UpdateSoundPos(triggerAreaInfo, i);
                }
            }
            else if (flag)
            {
                this.PlayTriggerSound(triggerAreaInfo.sound, i);
                this.playingSound |= 1 << i;
            }
            else
            {
                this.StopPlayTriggerSound(triggerAreaInfo.sound, i);
                this.playingSound = ~(~this.playingSound | 1 << i);
            }
        }
    }

    private void PlayTriggerSound(string sound, int key)
    {
        Debug.LogError("PlayTriggerSound:" + sound);
        GameObject gameObject;
        if (!this.SoundObjDic.TryGetValue(key, out gameObject))
        {
            return;
        }
        if (AkSoundEngine.IsInitialized())
        {
        }
    }

    private void UpdateSoundPos(TriggerAreaInfo item, int key)
    {
        if (item.path.Count <= 1)
        {
            return;
        }
        if (MainPlayer.Self == null || MainPlayer.Self.ModelObj == null)
        {
            return;
        }
        Vector3 position = MainPlayer.Self.ModelObj.transform.position;
        Vector3 vector = Vector3.zero;
        float num = float.MaxValue;
        for (int i = 1; i < item.path.Count; i++)
        {
            Vector3 from = item.path[i - 1];
            Vector3 to = item.path[i];
            Vector3 vector2;
            float minDis = this.GetMinDis(position, from, to, out vector2);
            if (minDis < num)
            {
                num = minDis;
                vector = vector2;
            }
        }
        Debug.LogError(string.Concat(new object[]
        {
            "UpdateSoundPos sound:",
            item.sound,
            " soundPos:",
            vector
        }));
        GameObject gameObject;
        if (!this.SoundObjDic.TryGetValue(key, out gameObject))
        {
            return;
        }
        gameObject.transform.localPosition = vector;
        if (AkSoundEngine.IsInitialized())
        {
        }
    }

    private void StopPlayTriggerSound(string sound, int key)
    {
        Debug.LogError("StopPlayTriggerSound:" + sound);
        GameObject gameObject;
        if (!this.SoundObjDic.TryGetValue(key, out gameObject))
        {
            return;
        }
        if (AkSoundEngine.IsInitialized())
        {
        }
    }

    private float GetMinDis(Vector3 p, Vector3 from, Vector3 to, out Vector3 minDisPos)
    {
        Vector3 normalized = (to - from).normalized;
        float num = Vector3.Dot(p - from, normalized);
        if (num <= 0f)
        {
            minDisPos = from;
            return (p - from).magnitude;
        }
        float num2 = Vector3.Dot(p - to, -normalized);
        if (num2 <= 0f)
        {
            minDisPos = to;
            return (p - to).magnitude;
        }
        minDisPos = normalized * num + from;
        return (p - minDisPos).magnitude;
    }

    private static AreaSoundTriggerTool instance;

    private GameObject TriggerAreaSoundParent;

    private List<List<SoundTrigger>> triggers = new List<List<SoundTrigger>>();

    private GameMapTriggerAreaSoundInfo info;

    private int playingSound;

    private Dictionary<int, GameObject> SoundObjDic = new Dictionary<int, GameObject>();
}
