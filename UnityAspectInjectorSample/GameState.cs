using System;

namespace UnityAspectInjectorSample {
	//[EnableLogging]
	public class GameState {
		// temporary, need to reference UnityEngine here
		public static Action<string> LogCallback = null;
		
		public int Scores { get; private set; }

		public GameState(int initialScore) {
			Scores = initialScore;
		}
		
		[EnableLogging]
		public void IncreaseScores(int value1, int value2, int value3) {
			IncreaseScore(value1);
			IncreaseScore(value2);
			IncreaseScore(value3);
		}

		[EnableLogging]
		bool IncreaseScore(int value) {
			Scores += value;
			return (Scores > 0);
		}

		public override string ToString() {
			return $"{{ score = {Scores} }}";
		}
	}
}