using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RankPkReqPrepare_SC")]
    [Serializable]
    public class MSG_RankPkReqPrepare_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "readystate", DataFormat = DataFormat.TwosComplement)]
        public uint readystate
        {
            get
            {
                return this._readystate;
            }
            set
            {
                this._readystate = value;
            }
        }

        [ProtoMember(2, IsRequired = true, Name = "readynum", DataFormat = DataFormat.TwosComplement)]
        public uint readynum
        {
            get
            {
                return this._readynum;
            }
            set
            {
                this._readynum = value;
            }
        }

        [ProtoMember(3, IsRequired = true, Name = "totalnum", DataFormat = DataFormat.TwosComplement)]
        public uint totalnum
        {
            get
            {
                return this._totalnum;
            }
            set
            {
                this._totalnum = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "lefttime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lefttime
        {
            get
            {
                return this._lefttime;
            }
            set
            {
                this._lefttime = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "enemyreadynum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint enemyreadynum
        {
            get
            {
                return this._enemyreadynum;
            }
            set
            {
                this._enemyreadynum = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "enemytotalnum", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint enemytotalnum
        {
            get
            {
                return this._enemytotalnum;
            }
            set
            {
                this._enemytotalnum = value;
            }
        }

        private uint _readystate;

        private uint _readynum;

        private uint _totalnum;

        private uint _lefttime;

        private uint _enemyreadynum;

        private uint _enemytotalnum;

        private IExtension extensionObject;
    }
}
