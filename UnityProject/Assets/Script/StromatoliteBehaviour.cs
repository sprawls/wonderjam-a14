using UnityEngine;
using System.Collections;

public class StromatoliteBehaviour : MonoBehaviour, IBeatReceiver {
	private float bpm;
    private BeatManager beatManagerRef;
	// Use this for initialization
	void Start () {
        beatManagerRef = BeatManager.Instance;
		GameManager.Instance.requestBeat (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnQuarterBeat()
    {
    }

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		//Debug.Log ("dick");
        bpm = beatManagerRef.interval;
		//Debug.Log (bpm);
	}
}
