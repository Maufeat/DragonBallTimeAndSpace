using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
    internal class OrderedDictionaryEnumerator : IEnumerator, IDictionaryEnumerator
    {
        public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
        {
            this.list_enumerator = enumerator;
        }

        public object Current
        {
            get
            {
                return this.Entry;
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
                return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public object Key
        {
            get
            {
                KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
                return keyValuePair.Key;
            }
        }

        public object Value
        {
            get
            {
                KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
                return keyValuePair.Value;
            }
        }

        public bool MoveNext()
        {
            return this.list_enumerator.MoveNext();
        }

        public void Reset()
        {
            this.list_enumerator.Reset();
        }

        private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
    }
}
