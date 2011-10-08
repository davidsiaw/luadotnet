using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using LuaState = System.IntPtr;

namespace luadotnet
{


    public class Lua : IDisposable
    {
        LuaState luastate;

        public Lua()
        {
            var f = LuaDll.lua_getDefaultAlloc();
            luastate = LuaDll.lua_newstate(f, IntPtr.Zero);
        }

        // Register a function that does something in c#
        public void Register(string name, Delegate func)
        {
            Marshaller m = new Marshaller(func);
            LuaDll.lua_register(luastate, name, m.InvokeFromLua);
        }

        // Register a function that pauses the script's execution
        public void RegisterBlocking(string name, Delegate func)
        {
            Marshaller m = new Marshaller(func, Marshaller.FunctionType.BLOCKING);
            LuaDll.lua_register(luastate, name, m.InvokeFromLua);
        }

        public LuaThread LoadScript(string str, string chunkname = "unnamed thread")
        {
            LuaState newthread = LuaDll.lua_newthread(luastate);
            return new LuaThread(newthread, str, chunkname);
        }

        public void Dispose()
        {
            LuaDll.lua_close(luastate);
        }
    }

       
}
