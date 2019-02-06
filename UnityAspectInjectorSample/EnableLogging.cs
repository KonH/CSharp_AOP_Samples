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
		static int _counter = 0;

		[Advice(Kind.Before, Targets = Target.Method)]
		public void LogBeforeMethodCall(
		[Argument(Source.Instance)]   object     instance,
		[Argument(Source.Arguments)]  object[]   arguments,
		[Argument(Source.Method)]     MethodBase method) {

			var isLogRequired = IsLogRequired(method);
			if ( isLogRequired ) {
				_counter++;
				var sb = new StringBuilder();
				AppendMethodName(sb, method, true);
				AppendArgumentValues(sb, arguments, method.GetParameters());
				AppendState(sb, instance);
				Logger.Log(sb.ToString());
			}
		}

		[Advice(Kind.After, Targets = Target.Method)]
		public void LogAfterMethodCall(
			[Argument(Source.Instance)]    object     instance,
			[Argument(Source.Method)]      MethodBase method,
			[Argument(Source.ReturnValue)] object     result) {

			var isLogRequired = IsLogRequired(method);
			if ( isLogRequired ) {
				var sb = new StringBuilder();
				AppendMethodName(sb, method, false);
				_counter--;
				if ( result != null ) {
					sb.Append(": ").Append(result);
				}
				AppendState(sb, instance);
				Logger.Log(sb.ToString());
			}
		}

		static void AppendMethodName(StringBuilder sb, MethodBase method, bool inOut) {
			sb.Append(' ').Append(inOut ? '>' : '<', _counter).Append(' ');
			sb.Append(method.Name);
		}

		static void AppendArgumentValues(StringBuilder sb, object[] arguments, ParameterInfo[] parameters) {
			if ( arguments.Length != parameters.Length ) {
				return;
			}
			sb.Append(" (");
			if ( parameters.Length == arguments.Length ) {
				for ( var i = 0; i < parameters.Length; i++ ) {
					var parameter = parameters[i];
					sb.Append(parameter.Name).Append(" = ").Append(arguments[i]);
					if ( i < (parameters.Length - 1) ) {
						sb.Append("; ");
					}
				}
			}
			sb.Append(")");
		}

		static bool IsLogRequired(MethodBase method) {
			// ToString method call recursion can happen anyway via nested calls or around generated method in some cases
			return (method.Name != "ToString") && !method.Name.StartsWith("__");
		}

		static void AppendState(StringBuilder sb, object instance) {
			if ( instance != null ) {
				sb.AppendLine().Append(instance.ToString());
			}
		}
	}
}