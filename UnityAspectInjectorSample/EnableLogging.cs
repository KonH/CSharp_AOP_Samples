using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using AspectInjector.Broker;

namespace UnityAspectInjectorSample {
	[Conditional("DEBUG")]
	[Aspect(Scope.Global)]
	[Injection(typeof(EnableLogging))]
	public class EnableLogging : Attribute {				
		[Advice(Kind.Around, Targets = Target.Method)]
		public object LogMethod(
			[Argument(Source.Instance)]   object                 instance,
			[Argument(Source.Arguments)]  object[]               arguments,
			[Argument(Source.Target)]     Func<object[], object> method,
			[Argument(Source.Method)]     MethodBase             methodInfo,
			[Argument(Source.ReturnType)] Type                   retType) {
			var sb = new StringBuilder();
			sb.Append(methodInfo.Name).Append(" (");
			var parameters = methodInfo.GetParameters();
			for ( var i = 0; i < parameters.Length; i++ ) {
				var parameter = parameters[i];
				// Possible issue inside AspectInjector - params and args may differ
				// (looks like invalid MethodBase is used when names are the same)
				if ( i >= arguments.Length ) {
					break;
				}
				sb.Append(parameter.Name).Append(" = ").Append(arguments[i]);
				if ( i < parameters.Length - 1 ) {
					sb.Append("; ");
				}
			}
			sb.Append(")");
			var isVoid = (retType == typeof(void));
			// ToString method call recursion can happen anyway via nested calls or around generated method in some cases
			var isLogRequired = (methodInfo.Name != "ToString") && !methodInfo.Name.StartsWith("__");
			var hasInstance = isLogRequired && (instance != null);
			var stateBefore = hasInstance ? instance.ToString() : null;
			object result = null;
			if ( isVoid ) {
				method(arguments);
			} else {
				result = method(arguments);
			}
			if ( result != null ) {
				sb.Append(": ").Append(result);
			}
			if ( stateBefore != null ) {
				var stateAfter = instance.ToString();
				sb.AppendFormat("\n[ {0} => {1} ]", stateBefore, stateAfter);
			}
			if ( isLogRequired ) {
				GameState.LogCallback(sb.ToString());
			}
			return result;
		}
	}
}