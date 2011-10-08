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

        /// <summary>
        /// Create a new lua script loader
        /// </summary>
        public Lua()
        {
            var f = LuaDll.lua_getDefaultAlloc();
            luastate = LuaDll.lua_newstate(f, IntPtr.Zero);
        }

        /// <summary>
        /// Add a function that can be called from the Lua script
        /// </summary>
        /// <param name="name">name used to refer to the function in the script</param>
        /// <param name="func">the function</param>
        public void Register(string name, Delegate func)
        {
            Marshaller m = new Marshaller(func);
            LuaDll.lua_register(luastate, name, m.InvokeFromLua);
        }

        /// <summary>
        /// Add a function that can be called from the Lua script and pause execution
        /// causing LuaThread.Start to return. You can call LuaThread.Start again
        /// to continue executing the script
        /// </summary>
        /// <param name="name">name used to refer to the function in the script</param>
        /// <param name="func">the function</param>
        public void RegisterBlocking(string name, Delegate func)
        {
            Marshaller m = new Marshaller(func, Marshaller.FunctionType.BLOCKING);
            LuaDll.lua_register(luastate, name, m.InvokeFromLua);
        }

        /// <summary>
        /// Loads a script
        /// </summary>
        /// <param name="str">The string of the script to load. You can read an entire script file and pass it in here</param>
        /// <param name="chunkname">Optional. The name of the script. This helps with debugging</param>
        /// <returns></returns>
        public LuaThread LoadScript(string str, string chunkname = "unnamed thread")
        {
            LuaState newthread = LuaDll.lua_newthread(luastate);
            return new LuaThread(newthread, str, chunkname);
        }

        /// <summary>
        /// Close the Lua state
        /// </summary>
        public void Dispose()
        {
            LuaDll.lua_close(luastate);
        }
    }

       
}
