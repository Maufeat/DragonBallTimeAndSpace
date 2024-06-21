using System;
using System.Collections.Generic;
using LuaInterface;
using msg;
using UnityEngine;

public class BuffIconController
{
    public BuffIconController(Transform root, int maxShowNum)
    {
        this.root = root;
        this.buffParent = root.Find("bufflist").transform;
        this.debuffParent = root.Find("debufflist").transform;
        this.source = this.buffParent.Find("buff").transform;
        this.source.gameObject.SetActive(false);
        this.lstDebuffItem = new List<BuffIconItem>();
        this.lstBuffItem = new List<BuffIconItem>();
        this.MaxShowNum = maxShowNum;
    }

    public ulong OwenerID
    {
        get
        {
            return this._owenerID;
        }
        set
        {
            this._owenerID = value;
        }
    }

    public void AddTargetItems(List<StateItem> list)
    {
        if (list == null || list.Count < 1)
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            BufferServerDate buffServerData = CommonTools.GetBuffServerData(list[i]);
            LuaTable configTable = LuaConfigManager.GetConfigTable("charstate", (ulong)buffServerData.flag);
            if (configTable != null && configTable.GetField_Uint("IconShowType") != 0U)
            {
                this.AddItem(configTable, buffServerData);
            }
        }
    }

    public void AddItem(LuaTable bufferConfig, BufferServerDate showData)
    {
        if (bufferConfig == null)
        {
            return;
        }
        if (bufferConfig.GetField_Uint("IconShowType") == 0U)
        {
            return;
        }
        this.DealBuffIcon(bufferConfig, showData);
    }

    public void TryRemoveItem(LuaTable bufferConfig, BufferServerDate showData)
    {
        uint field_Uint = bufferConfig.GetField_Uint("IconShowType");
        if (field_Uint == 0U)
        {
            return;
        }
        BuffIconItem buffIconItem = null;
        if (bufferConfig.GetField_Uint("addtype") == 0U)
        {
            buffIconItem = this.GetBuffItemByGiver(showData.giver, showData.flag, field_Uint);
        }
        else if (bufferConfig.GetField_Uint("addtype") == 1U)
        {
            buffIconItem = this.GetBuffItemByFlag(showData.flag, field_Uint);
        }
        if (buffIconItem != null)
        {
            if (buffIconItem.CurLayer <= 1U)
            {
                if (field_Uint == 1U || field_Uint == 4U)
                {
                    this.lstBuffItem.Remove(buffIconItem);
                }
                else if (field_Uint == 2U || field_Uint == 5U)
                {
                    this.lstDebuffItem.Remove(buffIconItem);
                }
            }
            buffIconItem.TryDeleteItem(showData);
            this.SetLstItemActiveDelay();
        }
    }

    public void Clear()
    {
        if (this.lstBuffItem.Count > 0)
        {
            for (int i = this.lstBuffItem.Count - 1; i >= 0; i--)
            {
                BuffIconItem buffIconItem = this.lstBuffItem[i];
                this.lstBuffItem.RemoveAt(i);
                buffIconItem.DestroyItem();
            }
        }
        if (this.lstDebuffItem.Count > 0)
        {
            for (int j = this.lstDebuffItem.Count - 1; j >= 0; j--)
            {
                BuffIconItem buffIconItem2 = this.lstDebuffItem[j];
                this.lstDebuffItem.RemoveAt(j);
                buffIconItem2.DestroyItem();
            }
        }
    }

    private void DealBuffIcon(LuaTable bufferConfig, BufferServerDate showData)
    {
        if (bufferConfig.GetField_Uint("addtype") == 0U)
        {
            if (bufferConfig.GetField_Uint("addlayer") <= 1U)
            {
                if (bufferConfig.GetField_Uint("replacetype") == 0U)
                {
                    BuffIconItem buffIconItem = this.GetBuffItemByGiver(showData.giver, showData.flag, bufferConfig.GetField_Uint("IconShowType"));
                    if (buffIconItem != null)
                    {
                        buffIconItem.UpdateItem(this.OwenerID, bufferConfig, showData);
                    }
                    else
                    {
                        buffIconItem = this.CreateItem(bufferConfig, showData);
                    }
                }
                else if (bufferConfig.GetField_Uint("replacetype") == 1U && this.GetBuffItemByGiver(showData.giver, showData.flag, bufferConfig.GetField_Uint("IconShowType")) == null)
                {
                    BuffIconItem buffIconItem2 = this.CreateItem(bufferConfig, showData);
                }
            }
            else
            {
                BuffIconItem buffIconItem3 = this.GetBuffItemByGiver(showData.giver, showData.flag, bufferConfig.GetField_Uint("IconShowType"));
                if (buffIconItem3 == null)
                {
                    buffIconItem3 = this.CreateItem(bufferConfig, showData);
                }
                else
                {
                    buffIconItem3.AddBSData(bufferConfig, showData);
                    buffIconItem3.UpdateBSLstTime(showData, bufferConfig.GetField_Uint("reset_time"));
                }
            }
        }
        else if (bufferConfig.GetField_Uint("addtype") == 1U)
        {
            if (bufferConfig.GetField_Uint("addtype") <= 1U)
            {
                if (bufferConfig.GetField_Uint("replacetype") == 0U)
                {
                    BuffIconItem buffIconItem4 = this.GetBuffItemByFlag(showData.flag, bufferConfig.GetField_Uint("IconShowType"));
                    if (buffIconItem4 != null)
                    {
                        buffIconItem4.UpdateItem(this.OwenerID, bufferConfig, showData);
                    }
                    else
                    {
                        buffIconItem4 = this.CreateItem(bufferConfig, showData);
                    }
                }
                else if (this.GetBuffItemByFlag(showData.flag, bufferConfig.GetField_Uint("IconShowType")) == null)
                {
                    BuffIconItem buffIconItem5 = this.CreateItem(bufferConfig, showData);
                }
            }
            else
            {
                BuffIconItem buffIconItem6 = this.GetBuffItemByFlag(showData.flag, bufferConfig.GetField_Uint("IconShowType"));
                if (buffIconItem6 == null)
                {
                    buffIconItem6 = this.CreateItem(bufferConfig, showData);
                }
                else
                {
                    buffIconItem6.AddBSData(bufferConfig, showData);
                    buffIconItem6.UpdateBSLstTime(showData, bufferConfig.GetField_Uint("reset_time"));
                }
            }
        }
    }

    private BuffIconItem GetBuffItemByGiver(ulong giver, UserState flag, uint bufferType)
    {
        if (bufferType == 1U || bufferType == 4U)
        {
            for (int i = 0; i < this.lstBuffItem.Count; i++)
            {
                BuffIconItem buffIconItem = this.lstBuffItem[i];
                if (buffIconItem.GiverID == giver && buffIconItem.Flag == flag)
                {
                    return buffIconItem;
                }
            }
        }
        else if (bufferType == 2U || bufferType == 5U)
        {
            for (int j = 0; j < this.lstDebuffItem.Count; j++)
            {
                BuffIconItem buffIconItem2 = this.lstDebuffItem[j];
                if (buffIconItem2.GiverID == giver && buffIconItem2.Flag == flag)
                {
                    return buffIconItem2;
                }
            }
        }
        return null;
    }

    private BuffIconItem GetBuffItemByFlag(UserState flag, uint bufferType)
    {
        if (bufferType == 1U || bufferType == 4U)
        {
            for (int i = 0; i < this.lstBuffItem.Count; i++)
            {
                BuffIconItem buffIconItem = this.lstBuffItem[i];
                if (buffIconItem.Flag == flag)
                {
                    return buffIconItem;
                }
            }
        }
        else if (bufferType == 2U || bufferType == 5U)
        {
            for (int j = 0; j < this.lstDebuffItem.Count; j++)
            {
                BuffIconItem buffIconItem2 = this.lstDebuffItem[j];
                if (buffIconItem2.Flag == flag)
                {
                    return buffIconItem2;
                }
            }
        }
        return null;
    }

    private BuffIconItem CreateItem(LuaTable bufferConfig, BufferServerDate showData)
    {
        BuffIconItem buffIconItem = null;
        uint field_Uint = bufferConfig.GetField_Uint("IconShowType");
        if (string.IsNullOrEmpty(bufferConfig.GetField_String("BuffIcon")))
        {
            FFDebug.LogResourceError(this, "BuffIcon null id: " + bufferConfig.GetField_String("id") + " IconShowType: " + bufferConfig.GetField_String("IconShowType"));
        }
        else if (field_Uint == 1U || field_Uint == 4U)
        {
            GameObject gameObject = this.InstantiateObj(this.source.gameObject, this.buffParent);
            int posByPrority = this.GetPosByPrority(bufferConfig, showData);
            gameObject.transform.SetSiblingIndex(posByPrority);
            buffIconItem = new BuffIconItem(gameObject.transform);
            buffIconItem.UpdateItem(this.OwenerID, bufferConfig, showData);
            this.lstBuffItem.Add(buffIconItem);
        }
        else if (field_Uint == 2U || field_Uint == 5U)
        {
            GameObject gameObject2 = this.InstantiateObj(this.source.gameObject, this.debuffParent);
            int posByPrority2 = this.GetPosByPrority(bufferConfig, showData);
            gameObject2.transform.SetSiblingIndex(posByPrority2);
            buffIconItem = new BuffIconItem(gameObject2.transform);
            buffIconItem.UpdateItem(this.OwenerID, bufferConfig, showData);
            this.lstDebuffItem.Add(buffIconItem);
        }
        if (buffIconItem != null)
        {
            buffIconItem.SetController(this);
            this.SetLstItemActive();
        }
        return buffIconItem;
    }

    private GameObject InstantiateObj(GameObject source, Transform parentTrans)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source);
        gameObject.transform.SetParent(parentTrans);
        gameObject.transform.localScale = source.transform.localScale;
        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        return gameObject;
    }

    private int GetPosByPrority(LuaTable bufferConfig, BufferServerDate showData)
    {
        int num = 0;
        uint field_Uint = bufferConfig.GetField_Uint("IconShowType");
        if (field_Uint == 1U || field_Uint == 4U)
        {
            num = 1;
            for (int i = 0; i < this.lstBuffItem.Count; i++)
            {
                BuffIconItem buffIconItem = this.lstBuffItem[i];
                if (buffIconItem.Priority > bufferConfig.GetField_Uint("IconPriority"))
                {
                    num++;
                }
                else if (buffIconItem.Priority == bufferConfig.GetField_Uint("IconPriority") && buffIconItem.SetTime > showData.settime)
                {
                    num++;
                }
            }
        }
        else if (field_Uint == 2U || field_Uint == 5U)
        {
            num = 0;
            for (int j = 0; j < this.lstDebuffItem.Count; j++)
            {
                BuffIconItem buffIconItem2 = this.lstDebuffItem[j];
                if (buffIconItem2.Priority > bufferConfig.GetField_Uint("IconPriority"))
                {
                    num++;
                }
                else if (buffIconItem2.Priority == bufferConfig.GetField_Uint("IconPriority") && buffIconItem2.SetTime < showData.settime)
                {
                    num++;
                }
            }
        }
        return num;
    }

    private void SetLstItemActive()
    {
        for (int i = 0; i < this.lstBuffItem.Count; i++)
        {
            BuffIconItem buffIconItem = this.lstBuffItem[i];
            if (this.lstBuffItem.Count > this.MaxShowNum)
            {
                buffIconItem.UpdateItemIsActive(this.MaxShowNum - 1);
            }
            else
            {
                buffIconItem.UpdateItemIsActive(this.MaxShowNum);
            }
        }
        for (int j = 0; j < this.lstDebuffItem.Count; j++)
        {
            BuffIconItem buffIconItem2 = this.lstDebuffItem[j];
            if (this.lstDebuffItem.Count > this.MaxShowNum)
            {
                buffIconItem2.UpdateItemIsActive(this.MaxShowNum - 1 - 1);
            }
            else
            {
                buffIconItem2.UpdateItemIsActive(this.MaxShowNum - 1);
            }
        }
    }

    private void SetLstItemActiveDelay()
    {
        Scheduler.Instance.AddFrame(1U, false, new Scheduler.OnScheduler(this.SetLstItemActive));
    }

    public void RemoveItem(uint bufferType, BuffIconItem item)
    {
        if (bufferType == 1U)
        {
            this.lstBuffItem.Remove(item);
        }
        else if (bufferType == 2U)
        {
            this.lstDebuffItem.Remove(item);
        }
    }

    private readonly int MaxShowNum;

    private Transform root;

    private Transform source;

    private Transform buffParent;

    private Transform debuffParent;

    private List<BuffIconItem> lstBuffItem;

    private List<BuffIconItem> lstDebuffItem;

    private ulong _owenerID;
}
