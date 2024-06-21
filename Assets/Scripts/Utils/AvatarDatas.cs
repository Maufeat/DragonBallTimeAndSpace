using System;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class AvatarDatas : ScriptableObject
{
    [ProtoMember(126)]
    public string UObjname
    {
        get
        {
            return base.name;
        }
        set
        {
            base.name = value;
        }
    }

    public ModelBody GetModelPart(string ModelPartname)
    {
        for (int i = 0; i < this.ModelPartList.Length; i++)
        {
            if (this.ModelPartList[i].name == ModelPartname)
            {
                return this.ModelPartList[i];
            }
        }
        return null;
    }

    [ProtoMember(127)]
    [NonSerialized]
    public AvatarDatas[] ProtoList;

    [ProtoMember(1)]
    public string Character;

    [ProtoMember(2)]
    public string Model;

    [ProtoMember(3)]
    public ModelBody[] ModelPartList;
}
