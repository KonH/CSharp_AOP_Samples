namespace UnityAspectInjectorSample {
	[EnableLogging]
	public class GameState {		
		public int Scores { get; private set; }

		public GameState(int initialScore) {
			Scores = initialScore;
		}
		
		// For testing nested method calls
		public void IncreaseScores(int value1, int value2, int value3, int value4) {
			IncreaseScore(value1);
			IncreaseScore(value2);
			IncreaseScore(value3);
			IncreaseScore(value4);
		}
		
		bool IncreaseScore(int value) {
			Scores += value;
			return (Scores > 0);
		}

		public override string ToString() {
			return $"{{ score = {Scores} }}";
		}
	}
}