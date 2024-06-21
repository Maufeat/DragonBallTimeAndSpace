using System;
using ProtoBuf;

namespace Net
{
    public delegate void ProtoMsgCallback<T>(T MsgData) where T : IExtensible;
}
