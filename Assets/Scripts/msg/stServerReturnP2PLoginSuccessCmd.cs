using System;
using Net;

public class stServerReturnP2PLoginSuccessCmd : StructCmd
{
    public override OctetsStream ReadStruct(OctetsStream os)
    {
        this.dwUserID = os.unmarshal_uint();
        this.loginTempID = os.unmarshal_uint();
        this.pstrIP = os.unmarshal_String(16);
        this.wdPort = os.unmarshal_ushort();
        this.key = os.unmarshal_bytes(256);
        this.state = os.unmarshal_uint();
        return os;
    }

    public uint dwUserID;

    public uint loginTempID;

    public string pstrIP;

    public ushort wdPort;

    public byte[] key;

    public uint state;
}
