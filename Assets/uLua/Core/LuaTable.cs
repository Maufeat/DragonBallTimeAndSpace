using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LuaInterface
{
    /*
     * Wrapper class for Lua tables
     *
     * Author: Fabio Mascarenhas
     * Version: 1.0
     */
    public class LuaTable : LuaBase
    {
        private BetterDictionary<string, object> dic_strfield;
        private BetterDictionary<object, object> dic_objfield;

        public LuaTable(int reference, LuaState interpreter)
        {
            _Reference = reference;
            _Interpreter = interpreter;
            translator = interpreter.translator;
            dic_strfield = new BetterDictionary<string, object>();
            dic_objfield = new BetterDictionary<object, object>();
        }

        public LuaTable(int reference, IntPtr L)
        {            
            _Reference = reference;
            translator = ObjectTranslator.FromState(L);
            _Interpreter = translator.interpreter;
            dic_strfield = new BetterDictionary<string, object>();
            dic_objfield = new BetterDictionary<object, object>();
        }

        /*
         * Indexer for string fields of the table
         */
        public object this[string field]
        {
            get
            {
                return _Interpreter.getObject(_Reference, field);
            }
            set
            {
                _Interpreter.setObject(_Reference, field, value);
            }
        }
        /*
         * Indexer for numeric fields of the table
         */
        public object this[object field]
        {
            get
            {
                return _Interpreter.getObject(_Reference, field);
            }
            set
            {
                _Interpreter.setObject(_Reference, field, value);
            }
        }


        public System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return _Interpreter.GetTableDict(this).GetEnumerator();
        }

        public int Count
        {
            get
            {
                //push(_Interpreter.L);
                //LuaDLL.lua_objlen(_Interpreter.L, -1);
                return _Interpreter.GetTableDict(this).Count;
            }
        }

        public ICollection Keys
        {
            get { return _Interpreter.GetTableDict(this).Keys; }
        }

        public ICollection Values
        {
            get { return _Interpreter.GetTableDict(this).Values; }
        }
		
		public void SetMetaTable(LuaTable metaTable)
		{
			push(_Interpreter.L);
			metaTable.push(_Interpreter.L);
			LuaDLL.lua_setmetatable(_Interpreter.L, -2);
			LuaDLL.lua_pop(_Interpreter.L, 1);
		}

        public T[] ToArray<T>()
        {
            IntPtr L = _Interpreter.L;            
            push(L);
            return LuaScriptMgr.GetArrayObject<T>(L, -1);            
        }

        public void Set(string key, object o)
        {
            IntPtr L = _Interpreter.L;
            push(L);
            LuaDLL.lua_pushstring(L, key);
            PushArgs(L, o);
            LuaDLL.lua_rawset(L, -3);
            LuaDLL.lua_settop(L, 0);
        }

        /*
         * Gets an string fields of a table ignoring its metatable,
         * if it exists
         */
        internal object rawget(string field)
        {
            return _Interpreter.rawGetObject(_Reference, field);
        }

        internal object rawgetFunction(string field)
        {
            object obj = _Interpreter.rawGetObject(_Reference, field);

            if (obj is LuaCSFunction)
                return new LuaFunction((LuaCSFunction)obj, _Interpreter);
            else
                return obj;
        }

        public LuaFunction RawGetFunc(string field)
        {            
            IntPtr L = _Interpreter.L;
            LuaTypes type = LuaTypes.LUA_TNONE;
            LuaFunction func = null;

            int oldTop = LuaDLL.lua_gettop(L);
            LuaDLL.lua_getref(L, _Reference);
            LuaDLL.lua_pushstring(L, field);
            LuaDLL.lua_gettable(L, -2);

            type = LuaDLL.lua_type(L, -1);

            if (type == LuaTypes.LUA_TFUNCTION)
            {
                func = new LuaFunction(LuaDLL.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX), L);                
            }

            LuaDLL.lua_settop(L, oldTop);
            return func;
        }

        /*
         * Pushes this table into the Lua stack
        // */
        internal void push(IntPtr luaState)
        {
            LuaDLL.lua_getref(luaState, _Reference);
        }     

        public override string ToString()
        {
            return "table";
        }

        /*
         * Added by Maufeat - Extension
         */


        public LuaTable GetCacheField_Table(string field)
        {
            if (!this.dic_strfield.ContainsKey(field))
            {
                LuaTable fieldTable = this.GetField_Table(field);
                this.dic_strfield.Add(field, (object)fieldTable);
            }
            return (LuaTable)this.dic_strfield[field];
        }

        public LuaTable GetCacheField_Table(object field)
        {
            if (!this.dic_objfield.ContainsKey(field))
            {
                LuaTable fieldTable = this.GetField_Table(field);
                this.dic_objfield.Add(field, (object)fieldTable);
            }
            return (LuaTable)this.dic_objfield[field];
        }

        public LuaTable GetField_Table(string field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return (LuaTable)null;
            if (obj != null && obj.GetType() == typeof(LuaTable))
                return (LuaTable)obj;
            return (LuaTable)null;
        }

        public LuaTable GetField_Table(object field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return (LuaTable)null;
            if (obj != null && obj.GetType() == typeof(LuaTable))
                return (LuaTable)obj;
            return (LuaTable)null;
        }

        public double GetCacheField_Double(string field)
        {
            if (!this.dic_strfield.ContainsKey(field))
            {
                double fieldDouble = this.GetField_Double(field);
                this.dic_strfield.Add(field, (object)fieldDouble);
            }
            return (double)this.dic_strfield[field];
        }

        public double GetCacheField_Double(object field)
        {
            if (!this.dic_objfield.ContainsKey(field))
            {
                double fieldDouble = this.GetField_Double(field);
                this.dic_objfield.Add(field, (object)fieldDouble);
            }
            return (double)this.dic_objfield[field];
        }

        public double GetField_Double(string field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return 0.0;
            if (obj.GetType() == typeof(double))
                return (double)obj;
            if (obj.GetType() != typeof(string))
                return 0.0;
            double result = 0.0;
            double.TryParse((string)obj, out result);
            return result;
        }

        public double GetField_Double(object field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return 0.0;
            if (obj.GetType() == typeof(double))
                return (double)obj;
            if (obj.GetType() != typeof(string))
                return 0.0;
            double result = 0.0;
            double.TryParse((string)obj, out result);
            return result;
        }

        public float GetCacheField_Float(string field)
        {
            return (float)this.GetCacheField_Double(field);
        }

        public float GetField_Float(object field)
        {
            return (float)this.GetField_Double(field);
        }

        public float GetField_Float(string field)
        {
            return (float)this.GetField_Double(field);
        }

        public int GetCacheField_Int(string field)
        {
            return (int)this.GetCacheField_Double(field);
        }

        public int GetField_Int(object field)
        {
            return (int)this.GetField_Double(field);
        }

        public int GetField_Int(string field)
        {
            return (int)this.GetField_Double(field);
        }

        public uint GetCacheField_Uint(string field)
        {
            return (uint)this.GetCacheField_Double(field);
        }

        public uint GetField_Uint(object field)
        {
            return (uint)this.GetField_Double(field);
        }

        public uint GetField_Uint(string field)
        {
            return (uint)this.GetField_Double(field);
        }

        public ulong GetField_ULong(string field)
        {
            return (ulong)this.GetField_Double(field);
        }

        public string GetCacheField_String(string field)
        {
            if (!this.dic_strfield.ContainsKey(field))
            {
                string fieldString = this.GetField_String(field);
                this.dic_strfield.Add(field, (object)fieldString);
            }
            return (string)this.dic_strfield[field];
        }

        public string GetCacheField_String(object field)
        {
            if (!this.dic_objfield.ContainsKey(field))
            {
                string fieldString = this.GetField_String(field);
                this.dic_objfield.Add(field, (object)fieldString);
            }
            return (string)this.dic_objfield[field];
        }

        public string GetField_String(string field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }

        public string GetField_String(object field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return string.Empty;
            return obj.ToString();
        }

        public bool GetCacheField_Bool(object field)
        {
            if (!this.dic_objfield.ContainsKey(field))
            {
                bool fieldBool = this.GetField_Bool(field);
                this.dic_objfield.Add(field, (object)fieldBool);
            }
            return (bool)this.dic_objfield[field];
        }

        public bool GetCacheField_Bool(string field)
        {
            if (!this.dic_strfield.ContainsKey(field))
            {
                bool fieldBool = this.GetField_Bool(field);
                this.dic_strfield.Add(field, (object)fieldBool);
            }
            return (bool)this.dic_strfield[field];
        }

        public bool GetField_Bool(object field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return false;
            if (obj.GetType() == typeof(bool))
                return (bool)obj;
            if (obj.GetType() == typeof(double))
                return (int)(double)obj != 0;
            return false;
        }

        public bool GetField_Bool(string field)
        {
            object obj = this._Interpreter.getObject(this._Reference, field);
            if (obj == null)
                return false;
            if (obj.GetType() == typeof(bool))
                return (bool)obj;
            if (obj.GetType() == typeof(double))
                return (int)(double)obj != 0;
            return false;
        }
    }
}
