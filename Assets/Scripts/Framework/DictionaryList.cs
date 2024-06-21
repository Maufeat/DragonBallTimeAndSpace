using System;
using System.Collections.Generic;

namespace Framework
{
    public class DictionaryList<KeyT, ValueT> : IDisposable
    {
        public ValueT GetValue(KeyT key)
        {
            ValueT result = default(ValueT);
            this._dic.TryGetValue(key, out result);
            return result;
        }

        public ValueT GetValueAt(int index)
        {
            return this._list[index];
        }

        public void Add(KeyT key, ValueT value)
        {
            this._dic.Add(key, value);
            this._list.Add(value);
        }

        public bool Remove(KeyT key)
        {
            ValueT valueT = default(ValueT);
            this._dic.TryGetValue(key, out valueT);
            if (valueT != null)
            {
                this._list.Remove(valueT);
                this._dic.Remove(key);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            this._dic.Clear();
            this._list.Clear();
        }

        public int Count
        {
            get
            {
                return this._dic.Count;
            }
        }

        public void Dispose()
        {
            this.Clear();
            this._dic = null;
            this._list = null;
        }

        private Dictionary<KeyT, ValueT> _dic = new Dictionary<KeyT, ValueT>();

        private List<ValueT> _list = new List<ValueT>();
    }
}
