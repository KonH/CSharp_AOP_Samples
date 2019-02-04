using System;

namespace SampleApp {
	[NotNullGuardAspect]
	class NullSafeClass {
		public void MethodWithNullableArgs(object nullableArg, [NotNull] object notNullArg) {
			Console.WriteLine(
				$"MethodWithNullableArgs called with " +
				$"{(nullableArg == null ? "null" : nullableArg.ToString())}" +
				$" and " +
				$"{notNullArg.ToString()}"
			);
		}
	}
}
