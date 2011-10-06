using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaState = System.IntPtr;

namespace lua {
	class Lua {
		LuaState luastate;


		public Lua() {
			luastate = LuaDll.lua_newstate(LuaDll.lua_getDefaultAlloc(), IntPtr.Zero);
		}



	}
}
