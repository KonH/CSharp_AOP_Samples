using UnityEngine;
using UnityAspectInjectorSample;

public class TestRunner : MonoBehaviour {
	public int ScoreView;
	
	GameState _state = new GameState();
	
	void Start() {
		_state.IncreaseScore(100);
		ScoreView = _state.Scores;
	}
}
