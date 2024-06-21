using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_ReqLanchVoteOut_SC")]
    [Serializable]
    public class MSG_ReqLanchVoteOut_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
        public uint errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "lancherid", DataFormat = DataFormat.Default)]
        public string lancherid
        {
            get
            {
                return this._lancherid;
            }
            set
            {
                this._lancherid = value;
            }
        }

        [ProtoMember(3, IsRequired = true, Name = "lanchername", DataFormat = DataFormat.Default)]
        public string lanchername
        {
            get
            {
                return this._lanchername;
            }
            set
            {
                this._lanchername = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "outerid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string outerid
        {
            get
            {
                return this._outerid;
            }
            set
            {
                this._outerid = value;
            }
        }

        [ProtoMember(5, IsRequired = true, Name = "outername", DataFormat = DataFormat.Default)]
        public string outername
        {
            get
            {
                return this._outername;
            }
            set
            {
                this._outername = value;
            }
        }

        [ProtoMember(6, IsRequired = true, Name = "duration", DataFormat = DataFormat.Default)]
        public string duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        private uint _errcode;

        private string _lancherid = string.Empty;

        private string _lanchername;

        private string _outerid = string.Empty;

        private string _outername;

        private string _duration;

        private IExtension extensionObject;
    }
}
