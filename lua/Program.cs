using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using luadotnet;

namespace lua
{
    class Program
    {



        class Story
        {
            public void DanSays(string a, string b)
            {
                Console.WriteLine("Dan Says: {0} {1}", a, b);
            }

            public void ThorSays(string a)
            {
                Console.WriteLine("Thor Says: {0}", a);
            }

            public void HarrySays(string o, params string[] a)
            {
                foreach (string s in a)
                {
                    Console.WriteLine("Harry Says: {0}", s);
                }
            }
        }


        static void Main(string[] args)
        {

            Story s = new Story();

            Lua l = new Lua();
            l.Register("DanSays", new Action<string, string>(s.DanSays));
            l.RegisterBlocking("ThorSays", new Action<string>(s.ThorSays));
            l.Register("HarrySays", new Action<string, string[]>(s.HarrySays));

            string a = @"
function haha ()
    ThorSays('hahaha');
end

for v = 1,2 do
DanSays('lolol',v);
haha();
end

HarrySays('meow',1,2,3,4);
";
            var scr = l.LoadScript(a);

            scr.Start();

            scr.Start();

            scr.Start();

            Console.ReadKey();
        }
    }
}
