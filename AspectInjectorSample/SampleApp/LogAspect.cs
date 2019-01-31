using System;
using AspectInjector.Broker;

namespace SampleApp {
	[Aspect(Scope.Global)]
	[Injection(typeof(LogAspect))]
	public class LogAspect : Attribute {
		
		[Advice(Kind.Before, Targets = Target.Method)]
		public void HandleMethod1([Argument(Source.Name)] string name) {
			Console.WriteLine($"Just starting method: '{name}'");
		}
		
		[Advice(Kind.Around, Targets = Target.Method)]
		public object HandleMethod2(
			[Argument(Source.Name)] string name,
			[Argument(Source.Arguments)] object[] arguments,
			[Argument(Source.Target)] Func<object[], object> method) {
			Console.WriteLine($"Starting method: '{name}' ({string.Join(",", arguments)})");
			var result = method(arguments);
			Console.WriteLine($"Ending method: '{name}' with result: {result}");
			return result;
		}
	}
}