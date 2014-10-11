using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour, IBeatReceiver {

    private BeatManager BeatManagerRef;

	// Use this for initialization
	void Start () {
        BeatManagerRef = new BeatManager(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBeat(BeatEnum p1, BeatEnum p2)
    {
    }
    
}
