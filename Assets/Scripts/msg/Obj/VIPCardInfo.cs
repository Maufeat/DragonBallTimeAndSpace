using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "VIPCardInfo")]
    [Serializable]
    public class VIPCardInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "remaintime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong remaintime
        {
            get
            {
                return this._remaintime;
            }
            set
            {
                this._remaintime = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "raffcount", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint raffcount
        {
            get
            {
                return this._raffcount;
            }
            set
            {
                this._raffcount = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "total_raffcount", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint total_raffcount
        {
            get
            {
                return this._total_raffcount;
            }
            set
            {
                this._total_raffcount = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "dayprize_state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint dayprize_state
        {
            get
            {
                return this._dayprize_state;
            }
            set
            {
                this._dayprize_state = value;
            }
        }

        [ProtoMember(5, Name = "arrprize", DataFormat = DataFormat.Default)]
        public List<PrizeBase> arrprize
        {
            get
            {
                return this._arrprize;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "objraffcount", DataFormat = DataFormat.TwosComplement)]
        public uint objraffcount
        {
            get
            {
                return this._objraffcount;
            }
            set
            {
                this._objraffcount = value;
            }
        }

        private ulong _remaintime;

        private uint _raffcount;

        private uint _total_raffcount;

        private uint _dayprize_state;

        private readonly List<PrizeBase> _arrprize = new List<PrizeBase>();

        private uint _objraffcount;

        private IExtension extensionObject;
    }
}
