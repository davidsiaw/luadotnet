using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using LuaState = System.IntPtr;
using lua_Number = System.Double;
using lua_Integer = System.Int64;
using size_t = System.Int32;

namespace luadotnet
{
    class LuaDll
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int lua_Writer(LuaState L, IntPtr p, size_t sz, IntPtr ud);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr lua_Reader(LuaState L, IntPtr ud, ref size_t sz);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr lua_Alloc(IntPtr ud, IntPtr ptr, size_t osize, size_t nsize);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int lua_CFunction(LuaState L);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern lua_Alloc lua_getDefaultAlloc();

        /*
        ** state manipulation
        */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern LuaState lua_newstate([MarshalAs(UnmanagedType.FunctionPtr)] lua_Alloc f, IntPtr ud);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_close(LuaState L);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern LuaState lua_newthread(LuaState L);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern lua_CFunction lua_atpanic(LuaState L, lua_CFunction panicf);


        /*
        ** basic stack manipulation
        */

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gettop(LuaState L);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settop(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushvalue(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_remove(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_insert(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_replace(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_checkstack(LuaState L, int sz);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_xmove(LuaState from, LuaState to, int n);


        /*
        ** access functions (stack -> C)
        */

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isnumber(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isstring(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_iscfunction(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isuserdata(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern TYPES lua_type(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_typename(LuaState L, int tp);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_equal(LuaState L, int idx1, int idx2);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_rawequal(LuaState L, int idx1, int idx2);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_lessthan(LuaState L, int idx1, int idx2);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern lua_Number lua_tonumber(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern lua_Integer lua_tointeger(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_toboolean(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_tolstring(LuaState L, int idx, ref size_t len);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern size_t lua_objlen(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern lua_CFunction lua_tocfunction(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_touserdata(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern LuaState lua_tothread(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_topointer(LuaState L, int idx);


        /*
        ** push functions (C -> stack)
        */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnil(LuaState L);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnumber(LuaState L, lua_Number n);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushinteger(LuaState L, lua_Integer n);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushlstring(LuaState L, string s, size_t l);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushstring(LuaState L, string s);
        //[DllImport("luanative.dll", CallingConvention=CallingConvention.Cdecl)]		public static extern  string lua_pushvfstring (LuaState L, const char *fmt, va_list argp);
        //[DllImport("luanative.dll", CallingConvention=CallingConvention.Cdecl)]		public static extern  string lua_pushfstring (LuaState L, const char *fmt, ...);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushcclosure(LuaState L, lua_CFunction fn, int n);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushboolean(LuaState L, int b);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushlightuserdata(LuaState L, IntPtr p);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_pushthread(LuaState L);


        /*
        ** get functions (Lua -> stack)
        */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_gettable(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_getfield(LuaState L, int idx, string k);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawget(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawgeti(LuaState L, int idx, int n);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_createtable(LuaState L, int narr, int nrec);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_newuserdata(LuaState L, size_t sz);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getmetatable(LuaState L, int objindex);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_getfenv(LuaState L, int idx);


        /*
        ** set functions (stack -> Lua)
        */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settable(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setfield(LuaState L, int idx, string k);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawset(LuaState L, int idx);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawseti(LuaState L, int idx, int n);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_setmetatable(LuaState L, int objindex);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_setfenv(LuaState L, int idx);


        /*
        ** `load' and `call' functions (load and run Lua code)
        */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_call(LuaState L, int nargs, int nresults);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_pcall(LuaState L, int nargs, int nresults, int errfunc);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_cpcall(LuaState L, lua_CFunction func, IntPtr ud);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_load(LuaState L, [MarshalAs(UnmanagedType.FunctionPtr)] lua_Reader reader, IntPtr dt, string chunkname);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_dump(LuaState L, lua_Writer writer, IntPtr data);


        /*
        ** coroutine functions
        */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_yield(LuaState L, int nresults);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern THREADSTATUS lua_resume(LuaState L, int narg);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_status(LuaState L);

        /*
        ** garbage-collection function and options
        */

        public enum GC
        {
            STOP = 0,
            RESTART = 1,
            COLLECT = 2,
            COUNT = 3,
            COUNTB = 4,
            STEP = 5,
            SETPAUSE = 6,
            SETSTEPMUL = 7
        }

        public enum PSEUDOINDICES : int
        {
            REGISTRYINDEX = (-10000),
            ENVIRONINDEX = (-10001),
            GLOBALSINDEX = (-10002)
        }

        public enum THREADSTATUS : int
        {
            FINISHED = 0,   // <- this is not defined in lua.h but is useful
            YIELD = 1,
            ERRRUN = 2,
            ERRSYNTAX = 3,
            ERRMEM = 4,
            ERRERR = 5
        }

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gc(LuaState L, GC what, int data);


        /*
        ** miscellaneous functions
        */

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_error(LuaState L);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_next(LuaState L, int idx);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_concat(LuaState L, int n);

        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern lua_Alloc lua_getallocf(LuaState L, ref IntPtr ud);
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setallocf(LuaState L, lua_Alloc f, IntPtr ud);

        public enum TYPES
        {
            NONE = (-1),
            NIL = 0,
            BOOLEAN = 1,
            LIGHTUSERDATA = 2,
            NUMBER = 3,
            STRING = 4,
            TABLE = 5,
            FUNCTION = 6,
            USERDATA = 7,
            THREAD = 8,

            // this is NOT in the lua spec and only here because
            // its useful in c#
            VAR_LIST = 0xff,
        }

        ///* 
        //** ===============================================================
        //** some useful macros
        //** ===============================================================
        //*/

        //#define lua_pop(L,n)		lua_settop(L, -(n)-1)
        public static void lua_pop(LuaState L, int n)
        {
            lua_settop(L, -n - 1);
        }

        //#define lua_newtable(L)		lua_createtable(L, 0, 0)
        public static void lua_newtable(LuaState L)
        {
            lua_createtable(L, 0, 0);
        }

        //#define lua_register(L,n,f) (lua_pushcfunction(L, (f)), lua_setglobal(L, (n)))
        public static void lua_register(LuaState L, string n, lua_CFunction f)
        {
            lua_pushcfunction(L, f);
            lua_setglobal(L, n);
        }

        //#define lua_pushcfunction(L,f)	lua_pushcclosure(L, (f), 0)
        public static void lua_pushcfunction(LuaState L, lua_CFunction f)
        {
            lua_pushcclosure(L, f, 0);
        }

        //#define lua_strlen(L,i)		lua_objlen(L, (i))
        public static size_t lua_strlen(LuaState L, int i)
        {
            return lua_objlen(L, i);
        }

        //#define lua_isfunction(L,n)	(lua_type(L, (n)) == LUA_TFUNCTION)
        public static bool lua_isfunction(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.FUNCTION;
        }

        //#define lua_istable(L,n)	(lua_type(L, (n)) == LUA_TTABLE)
        public static bool lua_istable(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.TABLE;
        }

        //#define lua_islightuserdata(L,n)	(lua_type(L, (n)) == LUA_TLIGHTUSERDATA)
        public static bool lua_islightuserdata(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.LIGHTUSERDATA;
        }

        //#define lua_isnil(L,n)		(lua_type(L, (n)) == LUA_TNIL)
        public static bool lua_isnil(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.NIL;
        }

        //#define lua_isboolean(L,n)	(lua_type(L, (n)) == LUA_TBOOLEAN)
        public static bool lua_isboolean(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.BOOLEAN;
        }

        //#define lua_isthread(L,n)	(lua_type(L, (n)) == LUA_TTHREAD)
        public static bool lua_isthread(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.THREAD;
        }

        //#define lua_isnone(L,n)		(lua_type(L, (n)) == LUA_TNONE)
        public static bool lua_isnone(LuaState L, size_t n)
        {
            return lua_type(L, n) == TYPES.NONE;
        }

        //#define lua_isnoneornil(L, n)	(lua_type(L, (n)) <= 0)
        public static bool lua_isnoneornil(LuaState L, size_t n)
        {
            return lua_type(L, n) <= 0;
        }

        //#define lua_pushliteral(L, s)	\
        //    lua_pushlstring(L, "" s, (sizeof(s)/sizeof(char))-1)
        public static void lua_pushliteral(LuaState L, string s)
        {
            lua_pushlstring(L, s, s.Length);
        }

        //#define lua_setglobal(L,s)	lua_setfield(L, LUA_GLOBALSINDEX, (s))
        public static void lua_setglobal(LuaState L, string s)
        {
            lua_setfield(L, (int)PSEUDOINDICES.GLOBALSINDEX, s);
        }

        //#define lua_getglobal(L,s)	lua_getfield(L, LUA_GLOBALSINDEX, (s))
        public static void lua_getglobal(LuaState L, string s)
        {
            lua_getfield(L, (int)PSEUDOINDICES.GLOBALSINDEX, s);
        }

        //#define lua_tostring(L,i)	lua_tolstring(L, (i), NULL)
        public static void lua_tostring(LuaState L, size_t i)
        {
            lua_tostring(L, i);
        }



        ///*
        //** compatibility macros and functions
        //*/

        //#define lua_open()	luaL_newstate()

        //#define lua_getregistry(L)	lua_pushvalue(L, LUA_REGISTRYINDEX)

        //#define lua_getgccount(L)	lua_gc(L, LUA_GCCOUNT, 0)

        //#define lua_Chunkreader		lua_Reader
        //#define lua_Chunkwriter		lua_Writer


        /* hack */
        [DllImport("luanative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setlevel(LuaState from, LuaState to);
    }
}
