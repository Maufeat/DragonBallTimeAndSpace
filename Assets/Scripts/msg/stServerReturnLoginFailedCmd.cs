using Net;

public class stServerReturnLoginFailedCmd : StructCmd
{
    public override OctetsStream ReadStruct(OctetsStream os)
    {
        this.byReturnCode = os.unmarshal_byte();
        this.data = os.unmarshal_String();
        return os;
    }

    public byte byReturnCode;

    public string data;
}
