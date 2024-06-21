using System;
using LuaInterface;

public class mapinfo
{
    public mapinfo(LuaTable ltb)
    {
        this._centerX = ltb.GetField_Int("centerX");
        this._centerY = ltb.GetField_Int("centerY");
        this._centerZ = ltb.GetField_Int("centerZ");
        this._fileName = ltb.GetField_String("fileName");
        this._xmlName = ltb.GetField_String("xmlName");
        this._fovdistance = ltb.GetField_Int("fovdistance");
        this._fovrotationX = ltb.GetField_Int("fovrotationX");
        this._fovrotationY = ltb.GetField_Int("fovrotationY");
        this._icon = ltb.GetField_String("icon");
        this._mainmap = ltb.GetField_Int("mainmap");
        this._mapID = ltb.GetField_Uint("mapID");
        this._mapflyheight = ltb.GetField_Int("mapflyheight");
        this._name = ltb.GetField_String("name");
        this._name_en = ltb.GetField_String("name_en");
        this._showname = ltb.GetField_String("showName");
        this._showmap = ltb.GetField_Int("showmap");
        this._wordmapnode = ltb.GetField_String("wordmapnode");
        this._blockName = ltb.GetField_String("blockName");
        this._isPlay = ltb.GetField_Int("isPlay");
        this._hightName = ltb.GetField_String("hightName");
        this._mapName = ltb.GetField_String("mapname");
    }

    public void SetSceneID(ulong id)
    {
        this._sceneID = id;
    }

    public int centerX()
    {
        return this._centerX;
    }

    public int centerY()
    {
        return this._centerY;
    }

    public int centerZ()
    {
        return this._centerZ;
    }

    public string fileName()
    {
        return this._fileName;
    }

    public string xmlName()
    {
        return this._xmlName;
    }

    public int fovdistance()
    {
        return this._fovdistance;
    }

    public int fovrotationX()
    {
        return this._fovrotationX;
    }

    public int fovrotationY()
    {
        return this._fovrotationY;
    }

    public string icon()
    {
        return this._icon;
    }

    public int mainmap()
    {
        return this._mainmap;
    }

    public uint mapID()
    {
        return this._mapID;
    }

    public ulong sceneID()
    {
        return this._sceneID;
    }

    public int mapflyheight()
    {
        return this._mapflyheight;
    }

    public string name()
    {
        return this._name;
    }

    public string showName()
    {
        return this._showname;
    }

    public string name_en()
    {
        return this._name_en;
    }

    public int showmap()
    {
        return this._showmap;
    }

    public string wordmapnode()
    {
        return this._wordmapnode;
    }

    public string blockName()
    {
        return this._blockName;
    }

    public string hightName()
    {
        return this._hightName;
    }

    public int isPlay()
    {
        return this._isPlay;
    }

    public string mapName()
    {
        return this._mapName;
    }

    private int _centerX;

    private int _centerY;

    private int _centerZ;

    private string _fileName;

    private string _xmlName;

    private int _fovdistance;

    private int _fovrotationX;

    private int _fovrotationY;

    private string _icon;

    private int _mainmap;

    private uint _mapID;

    private ulong _sceneID;

    private int _mapflyheight;

    private string _name;

    private string _showname;

    private string _name_en;

    private int _showmap;

    private string _wordmapnode;

    private string _blockName;

    private string _hightName;

    private int _isPlay;

    private string _mapName;
}
