using System;

namespace SampleApp {
	public class MyClass {
		[LogAspect]
		[NullCheckAspect]
		public int? MyMethod1(int? x, int? y) {
			Console.WriteLine("MyMethod1 body...");
			return x + y;
		}
		
		[HiddenExceptionSafeAspect]
		public int MyMethod2(int? x, int? y) {
			Console.WriteLine("MyMethod2 body...");
			var res = x.Value + y.Value;
			return res;
		}
	}
}