using UnityEngine;
using UnityAspectInjectorSample;

public class TestRunner : MonoBehaviour {
	public int ScoreView;
	
	GameState _state = null;
	
	void Start() {
		UnityAspectInjectorSample.Logger.Initialize(Debug.Log);
		_state = new GameState(100);
		_state.IncreaseScores(50, 100, -250, 0);
		ScoreView = _state.Scores;
	}
}
