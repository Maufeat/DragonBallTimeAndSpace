using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "HoldNpcData")]
    [Serializable]
    public class HoldNpcData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "npc_tempid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong npc_tempid
        {
            get
            {
                return this._npc_tempid;
            }
            set
            {
                this._npc_tempid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "npc_name", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string npc_name
        {
            get
            {
                return this._npc_name;
            }
            set
            {
                this._npc_name = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "resttime", DataFormat = DataFormat.TwosComplement)]
        public uint resttime
        {
            get
            {
                return this._resttime;
            }
            set
            {
                this._resttime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "distance", DataFormat = DataFormat.TwosComplement)]
        public uint distance
        {
            get
            {
                return this._distance;
            }
            set
            {
                this._distance = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "holduser", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong holduser
        {
            get
            {
                return this._holduser;
            }
            set
            {
                this._holduser = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(6, IsRequired = false, Name = "holdteam", DataFormat = DataFormat.TwosComplement)]
        public ulong holdteam
        {
            get
            {
                return this._holdteam;
            }
            set
            {
                this._holdteam = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "holdguild", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong holdguild
        {
            get
            {
                return this._holdguild;
            }
            set
            {
                this._holdguild = value;
            }
        }

        private ulong _npc_tempid;

        private string _npc_name = string.Empty;

        private uint _resttime;

        private uint _distance;

        private ulong _holduser;

        private ulong _holdteam;

        private ulong _holdguild;

        private IExtension extensionObject;
    }
}
