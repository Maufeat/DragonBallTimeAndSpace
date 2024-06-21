using System;
using System.Collections.Generic;
using System.ComponentModel;
using hero;
using Obj;
using ProtoBuf;

namespace mail
{
    [ProtoContract(Name = "mail_item")]
    [Serializable]
    public class mail_item : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "mailid", DataFormat = DataFormat.Default)]
        public string mailid
        {
            get
            {
                return this._mailid;
            }
            set
            {
                this._mailid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "fromid", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string fromid
        {
            get
            {
                return this._fromid;
            }
            set
            {
                this._fromid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(3, IsRequired = false, Name = "fromname", DataFormat = DataFormat.Default)]
        public string fromname
        {
            get
            {
                return this._fromname;
            }
            set
            {
                this._fromname = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "title", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "text", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }

        [ProtoMember(6, IsRequired = false, Name = "createtime", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string createtime
        {
            get
            {
                return this._createtime;
            }
            set
            {
                this._createtime = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "deltime", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string deltime
        {
            get
            {
                return this._deltime;
            }
            set
            {
                this._deltime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        public uint state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(9, IsRequired = false, Name = "itemgot", DataFormat = DataFormat.TwosComplement)]
        public uint itemgot
        {
            get
            {
                return this._itemgot;
            }
            set
            {
                this._itemgot = value;
            }
        }

        [ProtoMember(10, Name = "obj_list", DataFormat = DataFormat.Default)]
        public List<t_Object> obj_list
        {
            get
            {
                return this._obj_list;
            }
        }

        [ProtoMember(11, Name = "hero_list", DataFormat = DataFormat.Default)]
        public List<Hero> hero_list
        {
            get
            {
                return this._hero_list;
            }
        }

        public void Set(mail_item item)
        {
            this.mailid = item.mailid;
            this.fromid = item.fromid;
            this.fromname = item.fromname;
            this.title = item.title;
            this.text = item.text;
            this.createtime = item.createtime;
            this.deltime = item.deltime;
            this.state = item.state;
            this.itemgot = item.itemgot;
            this.obj_list.Clear();
            this.obj_list.AddRange(item.obj_list);
        }

        private string _mailid = string.Empty;

        private string _fromid = string.Empty;

        private string _fromname = string.Empty;

        private string _title = string.Empty;

        private string _text = string.Empty;

        private string _createtime = string.Empty;

        private string _deltime = string.Empty;

        private uint _state;

        private uint _itemgot;

        private readonly List<t_Object> _obj_list = new List<t_Object>();

        private readonly List<Hero> _hero_list = new List<Hero>();

        private IExtension extensionObject;
    }
}
