using ProtoBuf;
using System;
using System.IO;
using System.Text;

namespace Net
{
    public class OctetsStream : OctetsBuffer
    {
        private int pos;

        public OctetsStream()
        {
        }

        public OctetsStream(OctetsBuffer o)
          : base(o)
        {
        }

        public OctetsStream(int size)
          : base(size)
        {
        }

        public OctetsStream Clone()
        {
            return new OctetsStream((OctetsBuffer)base.Clone())
            {
                pos = this.pos
            };
        }

        public bool eos()
        {
            return this.pos == this.size();
        }

        public OctetsStream marshal(StructData stdata)
        {
            stdata.WriteData(this);
            return this;
        }

        public OctetsStream marshal(OctetsBuffer o)
        {
            this.marshal_int(o.size());
            this.insert(this.size(), o);
            return this;
        }

        public OctetsStream marshal(bool b)
        {
            this.push_back(b ? (byte)1 : (byte)0);
            return this;
        }

        public OctetsStream marshal(byte x)
        {
            this.push_back(x);
            return this;
        }

        public OctetsStream marshal(double x)
        {
            return this.marshal(BitConverter.ToInt64(BitConverter.GetBytes(x), 0));
        }

        public OctetsStream marshal(short x)
        {
            return this.marshal((ushort)x);
        }

        public OctetsStream marshal(int x)
        {
            return this.marshal((uint)x);
        }

        public OctetsStream marshal(long x)
        {
            return this.marshal((ulong)x);
        }

        public OctetsStream marshal(sbyte x)
        {
            this.push_back((byte)x);
            return this;
        }

        public OctetsStream marshal(float x)
        {
            return this.marshal(BitConverter.ToInt32(BitConverter.GetBytes(x), 0));
        }

        public OctetsStream marshal(string str)
        {
            return this.marshal(str, (string)null);
        }

        public OctetsStream marshal(string str, int size)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            this.insert(this.size(), bytes);
            this.insert(this.size(), new byte[size - bytes.Length]);
            return this;
        }

        public OctetsStream marshal(ushort x)
        {
            return this.marshal((byte)x).marshal((byte)((uint)x >> 8));
        }

        public OctetsStream marshal(uint x)
        {
            return this.marshal((byte)x).marshal((byte)(x >> 8)).marshal((byte)(x >> 16)).marshal((byte)(x >> 24));
        }

        public OctetsStream marshal(ulong x)
        {
            return this.marshal((byte)x).marshal((byte)(x >> 8)).marshal((byte)(x >> 16)).marshal((byte)(x >> 24)).marshal((byte)(x >> 32)).marshal((byte)(x >> 40)).marshal((byte)(x >> 48)).marshal((byte)(x >> 56));
        }

        public OctetsStream marshal(byte[] bytes)
        {
            this.marshal_int(bytes.Length);
            this.insert(this.size(), bytes);
            return this;
        }

        public OctetsStream marshal(string str, string charset)
        {
            try
            {
                if (charset == null)
                    this.marshal(Encoding.UTF8.GetBytes(str));
                else
                    this.marshal(Encoding.GetEncoding(charset).GetBytes(str));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
            return this;
        }

        public void marshal_string(string str, int size)
        {
            this.marshal(str, size);
        }

        public void marshal_string(string str)
        {
            this.marshal(str);
        }

        public void marshal_boolean(bool x)
        {
            this.marshal(x);
        }

        public void marshal_byte(byte x)
        {
            this.marshal(x);
        }

        public void marshal_double(double x)
        {
            this.marshal(x);
        }

        public void marshal_float(float x)
        {
            this.marshal(x);
        }

        public void marshal_int(int x)
        {
            this.marshal(x);
        }

        public void marshal_long(long x)
        {
            this.marshal(x);
        }

        public void marshal_Octets(OctetsBuffer o)
        {
            this.marshal(o);
        }

        public void marshal_sbyte(sbyte x)
        {
            this.marshal(x);
        }

        public void marshal_short(short x)
        {
            this.marshal(x);
        }

        public void marshal_uint(uint x)
        {
            this.marshal(x);
        }

        public void marshal_ulong(ulong x)
        {
            this.marshal(x);
        }

        public void marshal_ushort(ushort x)
        {
            this.marshal(x);
        }

        public void marshal_proto<T>(T t) where T : IExtensible
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    Serializer.Serialize<T>((Stream)memoryStream, t);
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    this.marshal_ushort((ushort)memoryStream.Length);
                    this.push_bytes(memoryStream.ToArray());
                }
                catch (Exception ex)
                {
                    FFDebug.LogWarning((object)this, (object)("marshal_proto Exception: " + ex.Message));
                }
            }
        }

        public OctetsStream marshal_struct(StructCmd scmd)
        {
            return scmd.WriteStruct(this);
        }

        public OctetsStream marshal_struct(StructData sd)
        {
            return sd.WriteData(this);
        }

        public void write_ushort(int index, ushort x)
        {
            this.buffer()[index] = (byte)x;
            this.buffer()[index + 1] = (byte)((uint)x >> 8);
        }

        public void write_byte(int index, byte x)
        {
            this.buffer()[index] = x;
        }

        public int position()
        {
            return this.pos;
        }

        public int position(int pos)
        {
            this.pos = pos;
            return this.pos;
        }

        public OctetsStream push_bytes(byte[] bytes)
        {
            this.insert(this.size(), bytes);
            return this;
        }

        public OctetsStream push_bytes(byte[] bytes, int len)
        {
            this.insert(this.size(), bytes, 0, len);
            return this;
        }

        public int remain()
        {
            return this.size() - this.pos;
        }

        public OctetsStream unmarshal(OctetsBuffer os)
        {
            int size = (int)this.unmarshal_uint();
            if (this.pos + size > this.size())
                throw new MarshalException();
            os.replace((OctetsBuffer)this, this.pos, size);
            this.pos += size;
            return this;
        }

        public OctetsStream unmarshal(OctetsBuffer os, int size)
        {
            if (this.pos + size > this.size())
                throw new MarshalException();
            os.replace((OctetsBuffer)this, this.pos, size);
            this.pos += size;
            return this;
        }

        public bool unmarshal_boolean()
        {
            return this.unmarshal_byte() == (byte)1;
        }

        public byte unmarshal_byte()
        {
            if (this.pos + 1 > this.size())
                throw new MarshalException();
            return this.getByte(this.pos++);
        }

        public byte[] unmarshal_bytes()
        {
            int length = (int)this.unmarshal_ushort();
            if (this.pos + length > this.size())
                throw new MarshalException();
            byte[] numArray = new byte[length];
            Array.Copy((Array)this.buffer(), this.pos, (Array)numArray, 0, length);
            this.pos += length;
            return numArray;
        }

        public byte[] unmarshal_bytes(int length)
        {
            if (this.pos + length > this.size())
                throw new MarshalException();
            byte[] numArray = new byte[length];
            Array.Copy((Array)this.buffer(), this.pos, (Array)numArray, 0, length);
            this.pos += length;
            return numArray;
        }

        public double unmarshal_double()
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(this.unmarshal_long()), 0);
        }

        public float unmarshal_float()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(this.unmarshal_int()), 0);
        }

        public int unmarshal_int()
        {
            if (this.pos + 4 > this.size())
                throw new MarshalException();
            return (int)this.getByte(this.pos++) & (int)byte.MaxValue | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 8 | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 16 | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 24;
        }

        public long unmarshal_long()
        {
            return (long)this.unmarshal_ulong();
        }

        public OctetsBuffer unmarshal_Octets()
        {
            int size = (int)this.unmarshal_uint();
            if (this.pos + size > this.size())
                throw new MarshalException();
            OctetsBuffer octetsBuffer = new OctetsBuffer((OctetsBuffer)this, this.pos, size);
            this.pos += size;
            return octetsBuffer;
        }

        public sbyte unmarshal_sbyte()
        {
            if (this.pos + 1 > this.size())
                throw new MarshalException();
            return (sbyte)this.getByte(this.pos++);
        }

        public short unmarshal_short()
        {
            return (short)this.unmarshal_ushort();
        }

        public string unmarshal_String(int length)
        {
            try
            {
                int len = length;
                if (this.pos + len > this.size())
                    throw new MarshalException();
                int pos = this.pos;
                this.pos += len;
                return string.Copy(this.getString(pos, len));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public string unmarshal_String()
        {
            return this.unmarshal_String((string)null);
        }

        public string unmarshal_String(string charset)
        {
            try
            {
                int len = (int)this.unmarshal_ushort();
                if (this.pos + len > this.size())
                    throw new MarshalException();
                int pos = this.pos;
                this.pos += len;
                return charset == null ? string.Copy(this.getString(pos, len)) : string.Copy(this.getString(pos, len, charset));
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public uint unmarshal_uint()
        {
            if (this.pos + 4 > this.size())
                throw new MarshalException();
            return (uint)((int)this.getByte(this.pos++) & (int)byte.MaxValue | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 8 | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 16 | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 24);
        }

        public ulong unmarshal_ulong()
        {
            if (this.pos + 8 > this.size())
                throw new MarshalException();
            return (ulong)((long)this.getByte(this.pos++) & (long)byte.MaxValue | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 8 | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 16 | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 24 | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 32 | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 40 | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 48 | ((long)this.getByte(this.pos++) & (long)byte.MaxValue) << 56);
        }

        public ushort unmarshal_ushort()
        {
            if (this.pos + 2 > this.size())
                throw new MarshalException();
            return (ushort)((int)this.getByte(this.pos++) & (int)byte.MaxValue | ((int)this.getByte(this.pos++) & (int)byte.MaxValue) << 8);
        }

        public T unmarshal_proto<T>() where T : IExtensible
        {
            T obj = default(T);
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(this.unmarshal_bytes()))
                    return Serializer.Deserialize<T>((Stream)memoryStream);
            }
            catch (Exception ex)
            {
                FFDebug.LogWarning((object)"ProtoCmd", (object)("ProtoCmd Deserialize Exception: " + ex.Message + " mssage: " + (object)typeof(T)));
                throw;
            }
        }

        public OctetsStream unmarshal_struct(StructCmd scmd)
        {
            return scmd.ReadStruct(this);
        }

        public OctetsStream unmarshal_struct(StructData sd)
        {
            return sd.ReadData(this);
        }

        public static OctetsStream wrap(OctetsBuffer o)
        {
            OctetsStream octetsStream = new OctetsStream();
            octetsStream.swap(o);
            return octetsStream;
        }
    }
}
