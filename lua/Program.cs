using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lua {
	class Program {

		

		class Story {
			public void DanSays(string a) {
				Console.WriteLine("Dan Says: {0}", a);
			}

			public void ThorSays(string a) {
				Console.WriteLine("Thor Says: {0}", a);
			}
		}


		static void Main(string[] args) {


			Console.ReadKey();
		}
	}
}
