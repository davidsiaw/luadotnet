using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lua {
	class Program {

		

		class Story {
			public void DanSays(string a, string b) {
				Console.WriteLine("Dan Says: {0} {1}", a, b);
			}

			public void ThorSays(string a) {
				Console.WriteLine("Thor Says: {0}", a);
			}
		}


		static void Main(string[] args) {

			Story s = new Story();

			Lua l = new Lua();
            l.Register("DanSays", new Action<string,string>(s.DanSays));
            l.Register("ThorSays", new Action<string>(s.ThorSays));

            string a = @"
function haha ()
    ThorSays('hahaha');
end

for v = 1,2 do
DanSays('lolol',v);
haha();
end
";
            l.DoScript(a);

			Console.ReadKey();
		}
	}
}
