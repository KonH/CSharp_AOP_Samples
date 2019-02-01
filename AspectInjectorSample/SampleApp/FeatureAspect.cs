using System;
using System.Collections.Generic;
using System.Linq;
using AspectInjector.Broker;

namespace SampleApp {
	[Aspect(Scope.Global, Factory = typeof(FeatureFactory))]
	public class FeatureAspect {
		readonly Func<string, bool> _enabled;
		
		public FeatureAspect(Func<string, bool> enabled) {
			_enabled = enabled;
		}

		bool IsEnabled(Attribute[] triggers) {
			foreach ( var trigger in triggers.OfType<FeatureAttribute>() ) {
				if ( !_enabled(trigger.Feature) ) {
					return false;
				}
			}
			return true;
		}
		
		[Advice(Kind.Around, Targets = Target.Method)]
		public object CallIfEnabled(
			[Argument(Source.Arguments)]  object[] arguments,
			[Argument(Source.Target)]     Func<object[], object> method,
			[Argument(Source.ReturnType)] Type retType,
			[Argument(Source.Injections)] Attribute[] triggers
		) {
			var isVoid = (retType == typeof(void));
			if ( IsEnabled(triggers) ) {
				if ( isVoid ) {
					method(arguments);
					return null;
				}
				return method(arguments);
			}
			return (!isVoid && retType.IsValueType) ? Activator.CreateInstance(retType) : null;
		}
	}

	[Injection(typeof(FeatureAspect))]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class FeatureAttribute : Attribute {
		public string Feature { get; }

		public FeatureAttribute(string feature) {
			Feature = feature;
		}
	}

	public class ConfigsHolder {
		public Dictionary<string, bool> Toggles { get; } = new Dictionary<string, bool>();
		
		public bool Resolve(string feature) {
			return Toggles.TryGetValue(feature, out var value) && value;
		}
	}
	
	public static class FeatureFactory {
		public static ConfigsHolder Configs { get; set; }
		
		public static object GetInstance(Type type) {
			return new FeatureAspect(Configs.Resolve);
		}
	}
}