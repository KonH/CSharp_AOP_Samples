using System;
using AspectInjector.Broker;

namespace SampleApp {
	[Aspect(Scope.Global)]
	[Injection(typeof(HiddenExceptionSafeAspect))]
	public class HiddenExceptionSafeAspect : Attribute {		
		[Advice(Kind.Around, Targets = Target.Method)]
		public object HandleMethod2(
			[Argument(Source.Name)] string name,
			[Argument(Source.Arguments)] object[] arguments,
			[Argument(Source.Target)] Func<object[], object> method,
			[Argument(Source.ReturnType)] Type retType) {
			try {
				return method(arguments);
			} catch ( Exception e ) {
				Console.WriteLine("Unfortunately: " + e);
				return retType.IsValueType ? Activator.CreateInstance(retType) : null;
			}
		}
	}
}