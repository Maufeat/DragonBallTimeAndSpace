using System;
using Net;

public class stPhoneInfo : StructData
{
    public override OctetsStream WriteData(OctetsStream ots)
    {
        ots.marshal_string(this.phone_uuid, 100);
        ots.marshal_string(this.pushid, 100);
        ots.marshal_string(this.phone_model, 100);
        ots.marshal_string(this.resolution, 100);
        ots.marshal_string(this.opengl, 100);
        ots.marshal_string(this.cpu, 100);
        ots.marshal_string(this.ram, 100);
        ots.marshal_string(this.os, 100);
        return ots;
    }

    public override OctetsStream ReadData(OctetsStream ots)
    {
        return ots;
    }

    public string phone_uuid = string.Empty;

    public string pushid = string.Empty;

    public string phone_model = string.Empty;

    public string resolution = string.Empty;

    public string opengl = string.Empty;

    public string cpu = string.Empty;

    public string ram = string.Empty;

    public string os = string.Empty;
}
