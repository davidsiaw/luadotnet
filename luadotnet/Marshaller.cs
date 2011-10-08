using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LuaState = System.IntPtr;

namespace luadotnet
{
     class Marshaller
        {
            static Dictionary<LuaDll.TYPES, Func<LuaState, int, object>> parameterReturner = new Dictionary<LuaDll.TYPES, Func<LuaState, int, object>>();
            static Dictionary<LuaDll.TYPES, Action<LuaState, object>> parameterPusher = new Dictionary<LuaDll.TYPES, Action<LuaState, object>>();

            static Marshaller()
            {
                parameterReturner[LuaDll.TYPES.NUMBER] = (l, i) => LuaDll.lua_tonumber(l, i);
                parameterReturner[LuaDll.TYPES.BOOLEAN] = (l, i) => LuaDll.lua_toboolean(l, i);
                parameterReturner[LuaDll.TYPES.STRING] = (l, i) => { int len = LuaDll.lua_strlen(l, i); return LuaDll.lua_tolstring(l, i, ref len); };
                parameterReturner[LuaDll.TYPES.FUNCTION] = (l, i) => LuaDll.lua_tocfunction(l, i);
                parameterReturner[LuaDll.TYPES.THREAD] = (l, i) => new LuaThread(LuaDll.lua_tothread(l, i));
                parameterReturner[LuaDll.TYPES.TABLE] = (l, i) =>
                {
                    Dictionary<object, object> dict = new Dictionary<object, object>();
                    LuaDll.lua_pushnil(l);
                    while (LuaDll.lua_next(l, i) != 0)
                    {
                        object key = parameterReturner[LuaDll.lua_type(l, -2)](l, -2);
                        object val = parameterReturner[LuaDll.lua_type(l, -1)](l, -1);
                        dict[key] = val;
                        LuaDll.lua_pop(l, 1);
                    }
                    return dict;
                };
                parameterReturner[LuaDll.TYPES.NIL] = (l, i) => null;
                parameterReturner[LuaDll.TYPES.NONE] = (l, i) => null;


                parameterPusher[LuaDll.TYPES.NUMBER] = (l, o) => LuaDll.lua_pushnumber(l, (double)o);
                parameterPusher[LuaDll.TYPES.BOOLEAN] = (l, o) => LuaDll.lua_pushboolean(l, ((bool)o) ? 1 : 0);
                parameterPusher[LuaDll.TYPES.STRING] = (l, o) => LuaDll.lua_pushstring(l, o.ToString());
                parameterPusher[LuaDll.TYPES.FUNCTION] = (l, o) => LuaDll.lua_pushcfunction(l, new Marshaller((Delegate)o).InvokeFromLua);
                parameterPusher[LuaDll.TYPES.THREAD] = (l, o) => LuaDll.lua_pushthread(((LuaThread)o).luastate);
               
                parameterPusher[LuaDll.TYPES.NIL] = (l, i) => { };
                parameterPusher[LuaDll.TYPES.NONE] = (l, i) => { };
            }
            
            public enum FunctionType{
                NONBLOCKING,
                BLOCKING
            }

            Delegate func;
            LuaDll.TYPES[] paramTypeList;
            LuaDll.TYPES varlistType;
            Type intvarlistType;
            FunctionType fnType;
            LuaDll.TYPES returnType;

            public Marshaller(Delegate func, FunctionType type = FunctionType.NONBLOCKING)
            {
                this.func = func;
                this.fnType = type;
                ParameterInfo[] pis = func.Method.GetParameters();
                paramTypeList = pis.Select(x =>
                {
                    if (Attribute.IsDefined(x, typeof(ParamArrayAttribute)))
                    {
                        intvarlistType = x.ParameterType.GetElementType();
                        varlistType = GetLuaType(intvarlistType);
                        return LuaDll.TYPES.VAR_LIST;
                    }

                    return GetLuaType(x.ParameterType);
                }).ToArray();
                returnType = GetLuaType(func.Method.ReturnType);
            }

            private static LuaDll.TYPES GetLuaType(Type x)
            {
                if (x == typeof(double))
                {
                    return LuaDll.TYPES.NUMBER;
                }
                else if (x == typeof(bool))
                {
                    return LuaDll.TYPES.BOOLEAN;
                }
                else if (x == typeof(string))
                {
                    return LuaDll.TYPES.STRING;
                }
                else if (typeof(Delegate).IsAssignableFrom(x))
                {
                    return LuaDll.TYPES.FUNCTION;
                }
                else if (x == typeof(Dictionary<object, object>))
                {
                    return LuaDll.TYPES.TABLE;
                }
                else if (x == typeof(LuaThread))
                {
                    return LuaDll.TYPES.THREAD;
                }
                else if (x == typeof(void))
                {
                    return LuaDll.TYPES.NONE;
                }
                else
                {
                    throw new Exception("Parameter not supported type " + x);
                }
            }

            public int InvokeFromLua(LuaState l)
            {
                int n = LuaDll.lua_gettop(l);
                List<object> parameters = new List<object>();
                for (int i = 0; i < n; i++)
                {
                    if (paramTypeList[i] == LuaDll.TYPES.VAR_LIST)
                    {
                        List<object> varlist = new List<object>();
                        for (; i < n; i++)
                        {
                            varlist.Add(parameterReturner[varlistType](l, i + 1));
                        }
                        Array arr = Array.CreateInstance(intvarlistType, varlist.Count);
                        for (int index = 0; index < varlist.Count; index++)
                        {
                            arr.SetValue(varlist[index], index);
                        }
                        parameters.Add(arr);
                    }
                    else
                    {
                        LuaDll.TYPES t = paramTypeList[i];
                        // parameters start at stack position 1
                        parameters.Add(parameterReturner[t](l, i + 1));
                    }
                }
                object retval = func.DynamicInvoke(parameters.ToArray());
                if (fnType == FunctionType.NONBLOCKING)
                {
                    if (returnType == LuaDll.TYPES.NONE)
                    {
                        return 0;
                    }
                    else
                    {
                        parameterPusher[returnType](l, retval);
                        return 1;
                    }
                }
                return LuaDll.lua_yield(l, 0);
            }
        }

}
