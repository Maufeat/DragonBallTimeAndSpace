using System;

public class VarType : IEquatable<VarType>
{
    public VarType(string var)
    {
        this._var_string = var;
    }

    public bool Equals(VarType other)
    {
        return this._var_string.Equals(other._var_string);
    }

    public bool IsBlank()
    {
        return string.IsNullOrEmpty(this._var_string);
    }

    public override int GetHashCode()
    {
        return this._var_string.GetHashCode();
    }

    public override string ToString()
    {
        return this._var_string;
    }

    public static implicit operator string(VarType var)
    {
        return var._var_string;
    }

    public static implicit operator ulong(VarType var)
    {
        return ulong.Parse(var._var_string);
    }

    public static implicit operator uint(VarType var)
    {
        return uint.Parse(var._var_string);
    }

    public static implicit operator ushort(VarType var)
    {
        return ushort.Parse(var._var_string);
    }

    public static implicit operator byte(VarType var)
    {
        return byte.Parse(var._var_string);
    }

    public static implicit operator long(VarType var)
    {
        return long.Parse(var._var_string);
    }

    public static implicit operator int(VarType var)
    {
        return int.Parse(var._var_string);
    }

    public static implicit operator short(VarType var)
    {
        return short.Parse(var._var_string);
    }

    public static implicit operator char(VarType var)
    {
        return char.Parse(var._var_string);
    }

    public static implicit operator float(VarType var)
    {
        return float.Parse(var._var_string);
    }

    public static implicit operator double(VarType var)
    {
        return double.Parse(var._var_string);
    }

    public static implicit operator bool(VarType var)
    {
        return bool.Parse(var._var_string);
    }

    private string _var_string;
}
