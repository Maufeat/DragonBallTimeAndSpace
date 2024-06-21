using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class AttackWarningEffect : IFFComponent
{
    public RelationType GetRelationWithMainPlayer()
    {
        return ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.Owner);
    }

    public void SetWarningEffect(int MeshType, float time, Vector3 Pos, Quaternion Quat, float Param0, float Param1)
    {
        if (this.Owner == MainPlayer.Self)
        {
            return;
        }
        GameObject gameObject = new GameObject();
        gameObject.name = "WarningEffect_" + this.Owner.EID.ToString();
        MeshDrawManager meshDrawManager = new MeshDrawManager(gameObject, time);
        string picName;
        if (MeshType == 1)
        {
            picName = ((this.GetRelationWithMainPlayer() != RelationType.Enemy) ? "Rang6" : "Rang7");
        }
        else
        {
            picName = ((this.GetRelationWithMainPlayer() != RelationType.Enemy) ? "Rang1" : "Rang4");
        }
        meshDrawManager.CreateProjector(gameObject.transform, picName, Param0, Param1, (MeshDrawManager.MeshType)MeshType, 0.1f, 6f);
        Pos.y = this.Owner.GetCharactorY(new Vector2(Pos.x, Pos.z));
        Pos.y += this.EffectHight;
        gameObject.transform.position = Pos;
        gameObject.transform.rotation = Quat;
        this.MeshDrawManagerList.Add(meshDrawManager);
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
    }

    public void CompDispose()
    {
        for (int i = 0; i < this.MeshDrawManagerList.Count; i++)
        {
            MeshDrawManager meshDrawManager = this.MeshDrawManagerList[i];
            meshDrawManager.Dispose();
        }
        this.MeshDrawManagerList.Clear();
    }

    public void CompUpdate()
    {
        this.RemoveList.Clear();
        for (int i = 0; i < this.MeshDrawManagerList.Count; i++)
        {
            MeshDrawManager meshDrawManager = this.MeshDrawManagerList[i];
            meshDrawManager.Updata();
            if (meshDrawManager.IsOver)
            {
                meshDrawManager.Dispose();
                this.RemoveList.Add(meshDrawManager);
            }
        }
        for (int j = 0; j < this.RemoveList.Count; j++)
        {
            this.MeshDrawManagerList.Remove(this.RemoveList[j]);
        }
    }

    public void ResetComp()
    {
    }

    private CharactorBase Owner;

    private GameObject WarningEffect;

    private float EffectHight = 0.2f;

    private List<MeshDrawManager> MeshDrawManagerList = new List<MeshDrawManager>();

    private List<MeshDrawManager> RemoveList = new List<MeshDrawManager>();
}
