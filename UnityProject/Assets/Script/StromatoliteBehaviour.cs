using UnityEngine;
using System.Collections.Generic;

public class StromatoliteBehaviour : MonoBehaviour, IBeatReceiver {
	private float bpm;
	private BeatManager bm;
	private bool createNewGem = false;
	private List<Transform> gems = new List<Transform>();

	public Transform gemPrefab;
	public float xLimit = 100;
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.requestBeat (this);
		bm = BeatManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		// Create new gem at end of line
		if (createNewGem) {
			gems.Add (GameObject.Instantiate (gemPrefab) as Transform);
			createNewGem = false;
		}

		foreach (Transform g in gems) {
			// Move all gems towards stromatolite
			g.Translate(new Vector3(1, 0, 0) * Time.deltaTime);

			// Destroy gems on stromatolite
			if (g.position.x > xLimit) {
				Destroy(g);
			}
		}
	}

    public void OnQuarterBeat()
    {
    }

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		bpm = bm.interval;
		createNewGem = true;
	}
}
