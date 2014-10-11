using UnityEngine;
using System.Collections;

public class StromatoliteBehaviour : MonoBehaviour, IBeatReceiver {

	// Use this for initialization
	void Start () {
		GameManager.Instance.requestBeat (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		float bpm = BeatManager.Instance.interval;

		Debug.Log (bpm);
	}
}
