using System;

public class UserInfoStorage
{
    public static void Read(UserInfoStorage p, string s)
    {
        string[] array = s.Split(new char[]
        {
            ','
        });
        p.Uid = array[0];
        p.Pwd = array[1];
        p.LastFileTick = array[2];
        if (array.Length > 3)
        {
            int.TryParse(array[3], out p.LastServer);
        }
        if (array.Length > 4)
        {
            p.LastVersion = array[4];
        }
        if (array.Length > 5)
        {
            p.ConfigPath = array[5];
        }
    }

    public static string Write(UserInfoStorage p)
    {
        return string.Concat(new object[]
        {
            p.Uid,
            ",",
            string.Empty,
            ",",
            p.LastFileTick,
            ",",
            p.LastServer,
            ",",
            p.LastVersion,
            ",",
            p.ConfigPath
        });
    }

    public override string ToString()
    {
        this.message = string.Format("user id:[{0}],user pwd:[{1}],last server:[{2}],File time trick:[{3}],last version:[{4}],config:[{5}]", new object[]
        {
            this.Uid,
            this.Pwd,
            this.LastServer,
            this.LastFileTick,
            this.LastVersion,
            this.ConfigPath
        });
        return this.message;
    }

    public Action<bool> SetHandle;

    public static UserInfoStorage StorageInfo;

    public string Uid = string.Empty;

    public string Pwd = string.Empty;

    public string Zone = string.Empty;

    public string LastFileTick = string.Empty;

    public string LastVersion = string.Empty;

    public string ConfigPath = string.Empty;

    public int LastServer;

    private string message = string.Empty;
}
