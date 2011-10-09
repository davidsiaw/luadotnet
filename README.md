Lua .NET interface!
-------------------

This is a small little interface I created to make using Lua from .NET easy and painless. Look at the extremely simple Program.cs to see how to use it.

At the moment it only supports the most basic thing: Registering functions. There is no debugging stuff exposed whatsoever. I'll add it later if I have time, or if someone is willing to make one.

This interface allows you to very easily register a function to be called from Lua.

**What are the limitations?**

You can't register a function that passes values with ref or out at the moment and it only permits these kinds of parameters and return types:

```c#
string, double, bool, a function, Dictionary<object,object> and LuaThread
```

In the future I may add returning arrays and it will enable us to return more than one thing to Lua.

**How do I do it?**

Simple examples:


----------------------------------------------------

```c#
Lua l = new Lua();
l.Register("print", new Action<string>(x=>Console.WriteLine(x)));
```

registers a function that takes a string and prints it!

----------------------------------------------------

```c#
LuaScript ls = l.LoadScript("print('haha');");
```

loads up a small script that just prints "haha" to the screen

----------------------------------------------------

```c#
ls.Start();
```

runs the script!

