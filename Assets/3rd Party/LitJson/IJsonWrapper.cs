using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
    public interface IJsonWrapper : IList, ICollection, IDictionary, IEnumerable, IOrderedDictionary
    {
        bool IsArray { get; }

        bool IsBoolean { get; }

        bool IsDouble { get; }

        bool IsInt { get; }

        bool IsLong { get; }

        bool IsObject { get; }

        bool IsString { get; }

        bool GetBoolean();

        float GetFloat();

        int GetInt();

        JsonType GetJsonType();

        string GetString();

        void SetBoolean(bool val);

        void SetDouble(float val);

        void SetInt(int val);

        void SetJsonType(JsonType type);

        void SetLong(long val);

        void SetString(string val);

        string ToJson();

        void ToJson(JsonWriter writer);
    }
}
