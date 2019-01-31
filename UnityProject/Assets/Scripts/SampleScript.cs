using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Weaver;

public class SampleScript : MonoBehaviour {

	[OnChanged(nameof(OnMyPropertyChanged))]
	public string MyProperty { get; private set; }
	
	[MethodTimer]
	[ProfileSample]
	void Start() {
		gameObject.SetActive(false);
		MyProperty = "newValue";
	}

	void OnMyPropertyChanged(string newValue) {
		Debug.Log("Value changed to " + newValue);
	}
}
