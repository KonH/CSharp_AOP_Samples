using System;

namespace SampleApp {
	[Feature(nameof(MyService))]
	public class MyService {
		public void MainRoutine() {
			Console.WriteLine("Service perform main routine...");
			FeatureRoutine();
		}

		[Feature("Feature")]
		void FeatureRoutine() {
			Console.WriteLine("Service perform feature routine...");
		}
	}
}