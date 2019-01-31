using System;

namespace SampleApp {
	class Program {
		static void Main(string[] args) {
			var obj = new MyClass();
			obj.MyMethod1(2, 2);
			
			try {
				obj.MyMethod1(1, null);
			} catch ( Exception e ) {
				Console.WriteLine("Expected: " + e.Message);
			}
			
			Console.WriteLine(obj.MyMethod2(2, 2));
			Console.WriteLine(obj.MyMethod2(null, null));
		}
	}
}