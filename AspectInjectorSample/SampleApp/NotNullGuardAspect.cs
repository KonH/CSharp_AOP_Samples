using System;
using System.Reflection;
using AspectInjector.Broker;

namespace SampleApp {
	[Aspect(Scope.Global)]
	[Injection(typeof(NotNullGuardAspect))]
	public class NotNullGuardAspect : Attribute {		
		[Advice(Kind.Around, Targets = Target.Method)]
		public object HandleMethod2(
			[Argument(Source.Arguments)] object[] arguments,
			[Argument(Source.Target)] Func<object[], object> method,
			[Argument(Source.Method)] MethodBase methodInfo,
			[Argument(Source.ReturnType)] Type retType) {
			var isSafe = true;
			var parameters = methodInfo.GetParameters();
			for ( var i = 0; i < parameters.Length; i++ ) {
				var parameter = parameters[i];
				if ( (arguments[i] == null) && (parameter.GetCustomAttribute<NotNullAttribute>() != null) ) {
					Console.WriteLine($"Trying to call method {methodInfo.Name} with null argument {parameter.Name}, it isn't safe.");
					isSafe = false;
				}
			}
			var isVoid = (retType == typeof(void));
			if ( isSafe ) {
				if ( isVoid ) {
					method(arguments);
					return null;
				}
				return method(arguments);
			}
			Console.WriteLine("Skipping method call...");
			return (!isVoid && retType.IsValueType) ? Activator.CreateInstance(retType) : null;
		}
	}

	public class NotNullAttribute : Attribute {}
}