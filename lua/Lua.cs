using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaState = System.IntPtr;
using System.Reflection;
using System.Runtime.InteropServices;

namespace lua {
	class Lua {
		LuaState luastate;

		public Lua() {
			luastate = LuaDll.lua_newstate(LuaDll.lua_getDefaultAlloc(), IntPtr.Zero);
		}

		private Lua(LuaState luastate) {
			this.luastate = luastate;
		}

		public void Register(string name, Delegate func) {
			Marshaller m = new Marshaller(func);
			LuaDll.lua_register(luastate, name, m.InvokeFromLua);
		}

		public void DoString(string str, string chunkname = "unnamed chunk") {

			byte[] b = Encoding.UTF8.GetBytes(str);
			IntPtr chunk = Marshal.AllocHGlobal(b.Length);
			LuaDll.lua_load(luastate, (l, d, s) => {
				return d;
			}, chunk, chunkname);
			Marshal.FreeHGlobal(chunk);
		}

		class Marshaller {
			static Dictionary<LuaDll.TYPES, Func<LuaState, int, object>> parameterReturner = new Dictionary<LuaDll.TYPES, Func<LuaState, int, object>>();

			static Marshaller() {
				parameterReturner[LuaDll.TYPES.NUMBER] = (l, i) => LuaDll.lua_tonumber(l, i);
				parameterReturner[LuaDll.TYPES.BOOLEAN] = (l, i) => LuaDll.lua_toboolean(l, i);
				parameterReturner[LuaDll.TYPES.STRING] = (l, i) => { int len = LuaDll.lua_strlen(l, i); return LuaDll.lua_tolstring(l, i, ref len); };
				parameterReturner[LuaDll.TYPES.FUNCTION] = (l, i) => LuaDll.lua_tocfunction(l, i);
				parameterReturner[LuaDll.TYPES.THREAD] = (l, i) => new Lua(LuaDll.lua_tothread(l, i));
				parameterReturner[LuaDll.TYPES.TABLE] = (l, i) => {
					Dictionary<object, object> dict = new Dictionary<object, object>();
					LuaDll.lua_pushnil(l);
					while (LuaDll.lua_next(l, i) != 0) {
						object key = parameterReturner[LuaDll.lua_type(l, -2)](l, -2);
						object val = parameterReturner[LuaDll.lua_type(l, -1)](l, -1);
						dict[key] = val;
						LuaDll.lua_pop(l, 1);
					}
					return dict;
				};
			}

			Delegate func;
			LuaDll.TYPES[] paramTypeList;

			public Marshaller(Delegate func) {
				this.func = func;
				ParameterInfo[] pis = func.Method.GetParameters();
				paramTypeList = pis.Select(x => {
					if (x.ParameterType == typeof(int)) {
						return LuaDll.TYPES.NUMBER;
					} else if (x.ParameterType == typeof(bool)) {
						return LuaDll.TYPES.BOOLEAN;
					} else if (x.ParameterType == typeof(string)) {
						return LuaDll.TYPES.STRING;
					} else if (typeof(Delegate).IsAssignableFrom(x.ParameterType)) {
						return LuaDll.TYPES.FUNCTION;
					} else if (x.ParameterType == typeof(Dictionary<object, object>)) {
						return LuaDll.TYPES.TABLE;
					} else if (x.ParameterType == typeof(Lua)) {
						return LuaDll.TYPES.THREAD;
					}
					throw new Exception("Parameter " + x.Name + " not supported type " + x.ParameterType);
				}).ToArray();
					
			}

			public int InvokeFromLua(LuaState l) {
				int n = LuaDll.lua_gettop(l);
				List<object> parameters = new List<object>();
				for (int i = 0; i < n; i++) {
					parameters.Add(parameterReturner[LuaDll.lua_type(l, i)](l, i));
				}
				func.DynamicInvoke(parameters.ToArray());
				return 0;
			}
		}


	}
}
