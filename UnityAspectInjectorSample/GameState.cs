namespace UnityAspectInjectorSample {
	public class GameState {
		public int Scores { get; private set; }

		public void IncreaseScore(int value) {
			Scores += value;
		}
	}
}