using System;
using AspectInjector.Broker;

namespace SampleApp {
	[Aspect(Scope.Global)]
	[Injection(typeof(NullCheckAspect))]
	public class NullCheckAspect : Attribute {
		
		[Advice(Kind.Before, Targets = Target.Method)]
		public void CheckArgs([Argument(Source.Name)] string name, [Argument(Source.Arguments)] object[] arguments) {
			foreach ( var arg in arguments ) {
				if ( arg == null ) {
					throw new ArgumentNullException();
				}
			}
		}
	}
}