using System;
using msg;

public struct EntitiesID
{
    public EntitiesID(EntryIDType IDType)
    {
        this.Etype = (CharactorType)IDType.type;
        this.Id = IDType.id;
        this._IDTypeStr = null;
        this._IDTypeNPCStr = null;
    }

    public EntitiesID(ulong id, CharactorType type)
    {
        this.Etype = type;
        this.Id = id;
        this._IDTypeStr = null;
        this._IDTypeNPCStr = null;
    }

    public string IDTypeStr
    {
        get
        {
            if (string.IsNullOrEmpty(this._IDTypeStr))
            {
                this._IDTypeStr = this.ToString();
            }
            return this._IDTypeStr;
        }
    }

    public string IDTypeNPCStr
    {
        get
        {
            if (string.IsNullOrEmpty(this._IDTypeNPCStr))
            {
                this._IDTypeNPCStr = GameMap.ItemType.NPC + ":" + this.IDTypeStr;
            }
            return this._IDTypeNPCStr;
        }
    }

    public string IdStr
    {
        get
        {
            return this.Id.ToString();
        }
    }

    public EntryIDType ToEntryIDType()
    {
        return new EntryIDType
        {
            id = this.Id,
            type = (uint)this.Etype
        };
    }

    public override string ToString()
    {
        return string.Concat(new object[]
        {
            "Etype:",
            this.Etype,
            " Id:",
            this.Id
        });
    }

    public bool Equals(EntitiesID Eq)
    {
        return Eq.Id == this.Id && Eq.Etype == this.Etype;
    }

    public bool Equals(EntryIDType Eq)
    {
        return false || Eq == null || (Eq.id == this.Id && Eq.type == (uint)this.Etype);
    }

    public static bool operator ==(EntitiesID v0, EntitiesID v1)
    {
        return v0.Equals(v1);
    }

    public static bool operator !=(EntitiesID v0, EntitiesID v1)
    {
        return !v0.Equals(v1);
    }

    public ulong Id;

    public CharactorType Etype;

    private string _IDTypeStr;

    private string _IDTypeNPCStr;
}
