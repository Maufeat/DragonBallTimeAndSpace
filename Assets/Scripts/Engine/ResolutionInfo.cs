using System;
using ProtoBuf;

namespace Engine
{
    [ProtoContract]
    public class ResolutionInfo
    {
        [ProtoMember(1)]
        public string Width;

        [ProtoMember(2)]
        public string Height;

        [ProtoMember(3)]
        public int FullScreen;

        [ProtoMember(4)]
        public int ReferenceResolution;

        [ProtoMember(5)]
        public int ModeIndex;

        [ProtoMember(6)]
        public int CameraMaxdistance;

        [ProtoMember(7)]
        public int UIScale;

        public int CurResolution;

        [ProtoMember(8)]
        public uint mouseSpeed;

        [ProtoMember(9)]
        public uint pixelpercent;
    }
}
