using System;

namespace SampleApp {
	class Program {
		static void Main(string[] args) {
			
			Console.WriteLine("Basic cases:");
			var obj = new MyClass();
			obj.MyMethod1(2, 2);
			
			try {
				obj.MyMethod1(1, null);
			} catch ( Exception e ) {
				Console.WriteLine("Expected: " + e.Message);
			}
			
			Console.WriteLine(obj.MyMethod2(2, 2));
			Console.WriteLine(obj.MyMethod2(null, null));
			
			Console.WriteLine();
			Console.WriteLine("Features:");
			
			var myService = new MyService();
			
			var configs = new ConfigsHolder();
			FeatureFactory.Configs = configs;

			Console.WriteLine("#1");
			configs.Toggles[nameof(MyService)] = true;
			myService.MainRoutine();
			// Service perform main routine...

			Console.WriteLine("#2");
			configs.Toggles[nameof(MyService)] = false;
			myService.MainRoutine();
			// Nothing
			
			configs.Toggles[nameof(MyService)] = true;
			
			Console.WriteLine("#3");
			configs.Toggles["Feature"] = true;
			myService.MainRoutine();
			// Service perform main routine...
			// Service perform feature routine...
			
			Console.WriteLine("#4");
			configs.Toggles["Feature"] = false;
			myService.MainRoutine();
			// Service perform main routine...

			Console.WriteLine();
			Console.WriteLine("NotNullGuard:");

			var nullSafeInstance = new NullSafeClass();
			nullSafeInstance.MethodWithNullableArgs("nullable", "notNull");
			nullSafeInstance.MethodWithNullableArgs(null, "notNull");
			nullSafeInstance.MethodWithNullableArgs("nullable", null);
		}
	}
}