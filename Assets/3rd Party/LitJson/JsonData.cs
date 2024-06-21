using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
    public class JsonData : IJsonWrapper, IList, ICollection, IDictionary, IEquatable<JsonData>, IEnumerable, IOrderedDictionary
    {
        public JsonData()
        {
        }

        public JsonData(bool boolean)
        {
            this.type = JsonType.Boolean;
            this.inst_boolean = boolean;
        }

        public JsonData(float number)
        {
            this.type = JsonType.Float;
            this.inst_float = number;
        }

        public JsonData(int number)
        {
            this.type = JsonType.Int;
            this.inst_int = number;
        }

        public JsonData(object obj)
        {
            if (obj is bool)
            {
                this.type = JsonType.Boolean;
                this.inst_boolean = (bool)obj;
                return;
            }
            if (obj is float)
            {
                this.type = JsonType.Float;
                this.inst_float = (float)obj;
                return;
            }
            if (obj is int)
            {
                this.type = JsonType.Int;
                this.inst_int = (int)obj;
                return;
            }
            if (obj is string)
            {
                this.type = JsonType.String;
                this.inst_string = (string)obj;
                return;
            }
            throw new ArgumentException("Unable to wrap the given object with JsonData");
        }

        public JsonData(string str)
        {
            this.type = JsonType.String;
            this.inst_string = str;
        }

        int ICollection.Count
        {
            get
            {
                return this.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return this.EnsureCollection().IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this.EnsureCollection().SyncRoot;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return this.EnsureDictionary().IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return this.EnsureDictionary().IsReadOnly;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                this.EnsureDictionary();
                IList<string> keys = new List<string>();
                AotSafe.ForEach<KeyValuePair<string, JsonData>>(this.object_list, delegate (KeyValuePair<string, JsonData> entry)
                {
                    keys.Add(entry.Key);
                });
                return (ICollection)keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                this.EnsureDictionary();
                IList<JsonData> values = new List<JsonData>();
                AotSafe.ForEach<KeyValuePair<string, JsonData>>(this.object_list, delegate (KeyValuePair<string, JsonData> entry)
                {
                    values.Add(entry.Key);
                });
                return (ICollection)values;
            }
        }

        bool IJsonWrapper.IsArray
        {
            get
            {
                return this.IsArray;
            }
        }

        bool IJsonWrapper.IsBoolean
        {
            get
            {
                return this.IsBoolean;
            }
        }

        bool IJsonWrapper.IsDouble
        {
            get
            {
                return this.IsDouble;
            }
        }

        bool IJsonWrapper.IsInt
        {
            get
            {
                return this.IsInt;
            }
        }

        bool IJsonWrapper.IsLong
        {
            get
            {
                return this.IsLong;
            }
        }

        bool IJsonWrapper.IsObject
        {
            get
            {
                return this.IsObject;
            }
        }

        bool IJsonWrapper.IsString
        {
            get
            {
                return this.IsString;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return this.EnsureList().IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return this.EnsureList().IsReadOnly;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this.EnsureDictionary()[key];
            }
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("The key has to be a string");
                }
                JsonData value2 = this.ToJsonData(value);
                this[(string)key] = value2;
            }
        }

        object IOrderedDictionary.this[int idx]
        {
            get
            {
                this.EnsureDictionary();
                return this.object_list[idx].Value;
            }
            set
            {
                this.EnsureDictionary();
                JsonData value2 = this.ToJsonData(value);
                KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
                this.inst_object[keyValuePair.Key] = value2;
                KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
                this.object_list[idx] = value3;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.EnsureList()[index];
            }
            set
            {
                this.EnsureList();
                JsonData value2 = this.ToJsonData(value);
                this[index] = value2;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.EnsureCollection().CopyTo(array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            JsonData value2 = this.ToJsonData(value);
            this.EnsureDictionary().Add(key, value2);
            KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
            this.object_list.Add(item);
            this.json = null;
        }

        void IDictionary.Clear()
        {
            this.EnsureDictionary().Clear();
            this.object_list.Clear();
            this.json = null;
        }

        bool IDictionary.Contains(object key)
        {
            return this.EnsureDictionary().Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IOrderedDictionary)this).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            this.EnsureDictionary().Remove(key);
            for (int i = 0; i < this.object_list.Count; i++)
            {
                if (this.object_list[i].Key == (string)key)
                {
                    this.object_list.RemoveAt(i);
                    break;
                }
            }
            this.json = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.EnsureCollection().GetEnumerator();
        }

        bool IJsonWrapper.GetBoolean()
        {
            if (this.type != JsonType.Boolean)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
            }
            return this.inst_boolean;
        }

        float IJsonWrapper.GetFloat()
        {
            if (this.type != JsonType.Float)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a double");
            }
            return this.inst_float;
        }

        int IJsonWrapper.GetInt()
        {
            if (this.type != JsonType.Int)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold an int");
            }
            return this.inst_int;
        }

        string IJsonWrapper.GetString()
        {
            if (this.type != JsonType.String)
            {
                throw new InvalidOperationException("JsonData instance doesn't hold a string");
            }
            return this.inst_string;
        }

        void IJsonWrapper.SetBoolean(bool val)
        {
            this.type = JsonType.Boolean;
            this.inst_boolean = val;
            this.json = null;
        }

        void IJsonWrapper.SetDouble(float val)
        {
            this.type = JsonType.Float;
            this.inst_float = val;
            this.json = null;
        }

        void IJsonWrapper.SetInt(int val)
        {
            this.type = JsonType.Int;
            this.inst_int = val;
            this.json = null;
        }

        void IJsonWrapper.SetLong(long val)
        {
            this.type = JsonType.Long;
            this.inst_float = (float)val;
            this.json = null;
        }

        void IJsonWrapper.SetString(string val)
        {
            this.type = JsonType.String;
            this.inst_string = val;
            this.json = null;
        }

        string IJsonWrapper.ToJson()
        {
            return this.ToJson();
        }

        void IJsonWrapper.ToJson(JsonWriter writer)
        {
            this.ToJson(writer);
        }

        int IList.Add(object value)
        {
            return this.Add(value);
        }

        void IList.Clear()
        {
            this.EnsureList().Clear();
            this.json = null;
        }

        bool IList.Contains(object value)
        {
            return this.EnsureList().Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return this.EnsureList().IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            this.EnsureList().Insert(index, value);
            this.json = null;
        }

        void IList.Remove(object value)
        {
            this.EnsureList().Remove(value);
            this.json = null;
        }

        void IList.RemoveAt(int index)
        {
            this.EnsureList().RemoveAt(index);
            this.json = null;
        }

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
        {
            this.EnsureDictionary();
            return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
        }

        void IOrderedDictionary.Insert(int idx, object key, object value)
        {
            string text = (string)key;
            JsonData value2 = this.ToJsonData(value);
            this[text] = value2;
            KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
            this.object_list.Insert(idx, item);
        }

        void IOrderedDictionary.RemoveAt(int idx)
        {
            this.EnsureDictionary();
            this.inst_object.Remove(this.object_list[idx].Key);
            this.object_list.RemoveAt(idx);
        }

        public int Count
        {
            get
            {
                return this.EnsureCollection().Count;
            }
        }

        public bool IsArray
        {
            get
            {
                return this.type == JsonType.Array;
            }
        }

        public bool IsBoolean
        {
            get
            {
                return this.type == JsonType.Boolean;
            }
        }

        public bool IsDouble
        {
            get
            {
                return this.type == JsonType.Float;
            }
        }

        public bool IsInt
        {
            get
            {
                return this.type == JsonType.Int;
            }
        }

        public bool IsLong
        {
            get
            {
                return this.type == JsonType.Long;
            }
        }

        public bool IsObject
        {
            get
            {
                return this.type == JsonType.Object;
            }
        }

        public bool IsString
        {
            get
            {
                return this.type == JsonType.String;
            }
        }

        public IDictionary<string, JsonData> Inst_Object
        {
            get
            {
                if (this.type == JsonType.Object)
                {
                    return this.inst_object;
                }
                return null;
            }
        }

        public JsonData this[string prop_name]
        {
            get
            {
                this.EnsureDictionary();
                if (this.inst_object.ContainsKey(prop_name))
                {
                    return this.inst_object[prop_name];
                }
                return null;
            }
            set
            {
                this.EnsureDictionary();
                KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
                if (this.inst_object.ContainsKey(prop_name))
                {
                    for (int i = 0; i < this.object_list.Count; i++)
                    {
                        if (this.object_list[i].Key == prop_name)
                        {
                            this.object_list[i] = keyValuePair;
                            break;
                        }
                    }
                }
                else
                {
                    this.object_list.Add(keyValuePair);
                }
                this.inst_object[prop_name] = value;
                this.json = null;
            }
        }

        public JsonData this[int index]
        {
            get
            {
                this.EnsureCollection();
                if (this.type == JsonType.Array)
                {
                    return this.inst_array[index];
                }
                return this.object_list[index].Value;
            }
            set
            {
                this.EnsureCollection();
                if (this.type == JsonType.Array)
                {
                    this.inst_array[index] = value;
                }
                else
                {
                    KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
                    KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
                    this.object_list[index] = value2;
                    this.inst_object[keyValuePair.Key] = value;
                }
                this.json = null;
            }
        }

        private ICollection EnsureCollection()
        {
            if (this.type == JsonType.Array)
            {
                return (ICollection)this.inst_array;
            }
            if (this.type == JsonType.Object)
            {
                return (ICollection)this.inst_object;
            }
            throw new InvalidOperationException("The JsonData instance has to be initialized first");
        }

        private IDictionary EnsureDictionary()
        {
            if (this.type == JsonType.Object)
            {
                return (IDictionary)this.inst_object;
            }
            if (this.type != JsonType.None)
            {
                throw new InvalidOperationException("Instance of JsonData is not a dictionary");
            }
            this.type = JsonType.Object;
            this.inst_object = new Dictionary<string, JsonData>();
            this.object_list = new List<KeyValuePair<string, JsonData>>();
            return (IDictionary)this.inst_object;
        }

        private IList EnsureList()
        {
            if (this.type == JsonType.Array)
            {
                return (IList)this.inst_array;
            }
            if (this.type != JsonType.None)
            {
                throw new InvalidOperationException("Instance of JsonData is not a list");
            }
            this.type = JsonType.Array;
            this.inst_array = new List<JsonData>();
            return (IList)this.inst_array;
        }

        private JsonData ToJsonData(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is JsonData)
            {
                return (JsonData)obj;
            }
            return new JsonData(obj);
        }

        private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
        {
            try
            {
                if (obj.IsString)
                {
                    writer.Write(obj.GetString());
                    return;
                }
                if (obj.IsBoolean)
                {
                    writer.Write(obj.GetBoolean());
                    return;
                }
                if (obj.IsDouble)
                {
                    writer.Write(obj.GetFloat());
                    return;
                }
                if (obj.IsInt)
                {
                    writer.Write(obj.GetInt());
                    return;
                }
                if (obj.IsArray)
                {
                    writer.WriteArrayStart();
                    AotSafe.ForEach<IList>(obj, delegate (IList elem)
                    {
                        JsonData.WriteJson((JsonData)elem, writer);
                    });
                    writer.WriteArrayEnd();
                    return;
                }
                if (obj.IsObject)
                {
                    writer.WriteObjectStart();
                    AotSafe.ForEach<object>(obj, delegate (object entry)
                    {
                        Type type = entry.GetType();
                        string property_name = (string)type.GetProperty("Key").GetValue(entry, null);
                        JsonData obj2 = (JsonData)type.GetProperty("Value").GetValue(entry, null);
                        writer.WritePropertyName(property_name);
                        JsonData.WriteJson(obj2, writer);
                    });
                    writer.WriteObjectEnd();
                    return;
                }
            }
            catch (Exception)
            {
                FFDebug.LogError("JsonData", "Error: JsonData.WriteJson() ================= info :" + obj.ToString());
                return;
            }
            FFDebug.LogWarning("JsonData", " ::::::::::::::::::::::::::::: Try WriteJson ::: obj.GetType() = " + obj.GetType());
            FFDebug.LogWarning("JsonData", " ::::::::::::::::::::::::::::: Try WriteJson222 ::: obj.ToJson() = " + obj.ToString());
            FFDebug.LogWarning("JsonData", "Error: JsonData.WriteJson()  ================= unkonw type :" + obj.ToString());
        }

        public int Add(object value)
        {
            JsonData value2 = this.ToJsonData(value);
            this.json = null;
            return this.EnsureList().Add(value2);
        }

        public void Clear()
        {
            if (this.IsObject)
            {
                ((IDictionary)this).Clear();
                return;
            }
            if (this.IsArray)
            {
                ((IList)this).Clear();
                return;
            }
        }

        public bool Equals(JsonData x)
        {
            if (x == null)
            {
                return false;
            }
            if (x.type != this.type)
            {
                return false;
            }
            switch (this.type)
            {
                case JsonType.None:
                    return true;
                case JsonType.Object:
                    return this.inst_object.Equals(x.inst_object);
                case JsonType.Array:
                    return this.inst_array.Equals(x.inst_array);
                case JsonType.String:
                    return this.inst_string.Equals(x.inst_string);
                case JsonType.Int:
                    return this.inst_int.Equals(x.inst_int);
                case JsonType.Float:
                    return this.inst_float.Equals(x.inst_float);
                case JsonType.Boolean:
                    return this.inst_boolean.Equals(x.inst_boolean);
            }
            return false;
        }

        public JsonType GetJsonType()
        {
            return this.type;
        }

        public void SetJsonType(JsonType type)
        {
            if (this.type == type)
            {
                return;
            }
            switch (type)
            {
                case JsonType.Object:
                    this.inst_object = new Dictionary<string, JsonData>();
                    this.object_list = new List<KeyValuePair<string, JsonData>>();
                    break;
                case JsonType.Array:
                    this.inst_array = new List<JsonData>();
                    break;
                case JsonType.String:
                    this.inst_string = null;
                    break;
                case JsonType.Int:
                    this.inst_int = 0;
                    break;
                case JsonType.Float:
                    this.inst_float = 0f;
                    break;
                case JsonType.Boolean:
                    this.inst_boolean = false;
                    break;
            }
            this.type = type;
        }

        public string ToJson()
        {
            if (this.json != null)
            {
                return this.json;
            }
            StringWriter stringWriter = new StringWriter();
            JsonData.WriteJson(this, new JsonWriter(stringWriter)
            {
                Validate = false
            });
            this.json = stringWriter.ToString();
            return this.json;
        }

        public void ToJson(JsonWriter writer)
        {
            bool validate = writer.Validate;
            writer.Validate = false;
            JsonData.WriteJson(this, writer);
            writer.Validate = validate;
        }

        public override string ToString()
        {
            switch (this.type)
            {
                case JsonType.Object:
                    return "JsonData object";
                case JsonType.Array:
                    return "JsonData array";
                case JsonType.String:
                    return this.inst_string;
                case JsonType.Int:
                    return this.inst_int.ToString();
                case JsonType.Float:
                    return this.inst_float.ToString();
                case JsonType.Boolean:
                    return this.inst_boolean.ToString();
            }
            return "Uninitialized JsonData";
        }

        public bool GetBool()
        {
            return this.type == JsonType.Boolean && this.inst_boolean;
        }

        public float GetFloat()
        {
            if (this.type == JsonType.Float)
            {
                return this.inst_float;
            }
            return this.inst_float;
        }

        public int GetInt()
        {
            if (this.type == JsonType.Int)
            {
                return this.inst_int;
            }
            return this.inst_int;
        }

        public static implicit operator JsonData(bool data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(float data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(int data)
        {
            return new JsonData(data);
        }

        public static implicit operator JsonData(long data)
        {
            return new JsonData((float)data);
        }

        public static implicit operator JsonData(string data)
        {
            return new JsonData(data);
        }

        public static explicit operator bool(JsonData data)
        {
            if (data.type != JsonType.Boolean)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold a double");
            }
            return data.inst_boolean;
        }

        public static explicit operator float(JsonData data)
        {
            if (data.type != JsonType.Float && data.type != JsonType.Int && data.type != JsonType.Long)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold a double");
            }
            return data.inst_float;
        }

        public static explicit operator int(JsonData data)
        {
            if (data.type != JsonType.Int)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold an int");
            }
            return data.inst_int;
        }

        public static explicit operator string(JsonData data)
        {
            if (data.type != JsonType.String)
            {
                throw new InvalidCastException("Instance of JsonData doesn't hold a string");
            }
            return data.inst_string;
        }

        private IList<JsonData> inst_array;

        private bool inst_boolean;

        private float inst_float;

        private int inst_int;

        private IDictionary<string, JsonData> inst_object;

        private string inst_string;

        private string json;

        private JsonType type;

        private IList<KeyValuePair<string, JsonData>> object_list;
    }
}
