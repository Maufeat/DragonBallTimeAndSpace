using Net;
using UnityEngine;

public class stServerReturnClientIPCmd : StructCmd
{
    public override OctetsStream ReadStruct(OctetsStream os)
    {
        Debug.Log("FUCK");
        this.pstrIP = os.unmarshal_String();
        Debug.Log("FUCK U " + pstrIP);
        return os;
    }

    public string pstrIP;
}
