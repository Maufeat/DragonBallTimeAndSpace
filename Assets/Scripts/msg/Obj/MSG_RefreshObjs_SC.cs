using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_RefreshObjs_SC")]
    [Serializable]
    public class MSG_RefreshObjs_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "objs", DataFormat = DataFormat.Default)]
        public List<t_Object> objs
        {
            get
            {
                return this._objs;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "show_addnew_anim", DataFormat = DataFormat.Default)]
        [DefaultValue(true)]
        public bool show_addnew_anim
        {
            get
            {
                return this._show_addnew_anim;
            }
            set
            {
                this._show_addnew_anim = value;
            }
        }

        private readonly List<t_Object> _objs = new List<t_Object>();

        private bool _show_addnew_anim = true;

        private IExtension extensionObject;
    }
}
