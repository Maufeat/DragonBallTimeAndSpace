using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_Ret_MatchMemberInfo_SC")]
    [Serializable]
    public class MSG_Ret_MatchMemberInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "members", DataFormat = DataFormat.Default)]
        public List<UserRankPkInfo> members
        {
            get
            {
                return this._members;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "leaderid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong leaderid
        {
            get
            {
                return this._leaderid;
            }
            set
            {
                this._leaderid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "season_id", DataFormat = DataFormat.TwosComplement)]
        public uint season_id
        {
            get
            {
                return this._season_id;
            }
            set
            {
                this._season_id = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "start_time", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint start_time
        {
            get
            {
                return this._start_time;
            }
            set
            {
                this._start_time = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "end_time", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint end_time
        {
            get
            {
                return this._end_time;
            }
            set
            {
                this._end_time = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "leftdays", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint leftdays
        {
            get
            {
                return this._leftdays;
            }
            set
            {
                this._leftdays = value;
            }
        }

        private readonly List<UserRankPkInfo> _members = new List<UserRankPkInfo>();

        private ulong _leaderid;

        private uint _season_id;

        private uint _start_time;

        private uint _end_time;

        private uint _leftdays;

        private IExtension extensionObject;
    }
}
