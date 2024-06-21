using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_Ret_VisitNpcTrade_SC")]
    [Serializable]
    public class MSG_Ret_VisitNpcTrade_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "action", DataFormat = DataFormat.TwosComplement)]
        public uint action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "flag", DataFormat = DataFormat.TwosComplement)]
        public uint flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                this._flag = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "npc_temp_id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong npc_temp_id
        {
            get
            {
                return this._npc_temp_id;
            }
            set
            {
                this._npc_temp_id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "conv_exchange", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint conv_exchange
        {
            get
            {
                return this._conv_exchange;
            }
            set
            {
                this._conv_exchange = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "show_type", DataFormat = DataFormat.TwosComplement)]
        public uint show_type
        {
            get
            {
                return this._show_type;
            }
            set
            {
                this._show_type = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "crc_ret", DataFormat = DataFormat.TwosComplement)]
        public uint crc_ret
        {
            get
            {
                return this._crc_ret;
            }
            set
            {
                this._crc_ret = value;
            }
        }

        [ProtoMember(9, IsRequired = false, Name = "user_menu", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string user_menu
        {
            get
            {
                return this._user_menu;
            }
            set
            {
                this._user_menu = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(10, IsRequired = false, Name = "npc_menu", DataFormat = DataFormat.Default)]
        public string npc_menu
        {
            get
            {
                return this._npc_menu;
            }
            set
            {
                this._npc_menu = value;
            }
        }

        [ProtoMember(11, Name = "allcrc", DataFormat = DataFormat.Default)]
        public List<questCRC> allcrc
        {
            get
            {
                return this._allcrc;
            }
        }

        [ProtoMember(12, IsRequired = false, Name = "source", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        private uint _action;

        private uint _retcode;

        private uint _flag;

        private ulong _npc_temp_id;

        private uint _type;

        private uint _conv_exchange;

        private uint _show_type;

        private uint _crc_ret;

        private string _user_menu = string.Empty;

        private string _npc_menu = string.Empty;

        private readonly List<questCRC> _allcrc = new List<questCRC>();

        private uint _source;

        private IExtension extensionObject;
    }
}
