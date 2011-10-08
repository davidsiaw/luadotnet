using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using LuaState = System.IntPtr;

namespace luadotnet
{
    /// <summary>
    /// Make an instance of this using Lua.LoadScript
    /// </summary>
    public class LuaThread : IDisposable
    {
        byte[] b;
        IntPtr chunk = IntPtr.Zero;
        bool read = false;
        internal readonly LuaState luastate;
        internal LuaThread(LuaState luastate, string str, string chunkname = "unnamed thread")
        {
            this.luastate = luastate;

            b = Encoding.UTF8.GetBytes(str);
            chunk = Marshal.AllocHGlobal(b.Length);
            Marshal.Copy(b, 0, chunk, b.Length);
            var err = LuaDll.lua_load(luastate, reader, IntPtr.Zero, chunkname);
        }

        internal LuaThread(LuaState luastate)
        {
            this.luastate = luastate;
        }

        IntPtr reader(LuaState l, IntPtr d, ref int s)
        {
            if (read)
            {
                s = 0;
                return IntPtr.Zero;
            }
            read = true;
            s = b.Length;
            return chunk;
        }

        /// <summary>
        /// Start or resume execution
        /// </summary>
        public void Resume()
        {
            Start();
        }

        /// <summary>
        /// Start or resume execution
        /// </summary>
        public void Start()
        {
            LuaDll.lua_resume(luastate, 0);
        }

        /// <summary>
        /// Close this thread
        /// </summary>
        public void Dispose()
        {
            Marshal.FreeHGlobal(chunk);
            LuaDll.lua_close(luastate);
        }
    }
}
