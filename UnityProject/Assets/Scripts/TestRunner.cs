using UnityEngine;
using UnityAspectInjectorSample;

public class TestRunner : MonoBehaviour {
	public int ScoreView;
	
	GameState _state = new GameState(100);
	
	void Start() {
		GameState.LogCallback = msg => Debug.Log(msg); // temporary
		
		_state.IncreaseScores(50, 100, -250);
		ScoreView = _state.Scores;
	}
}
