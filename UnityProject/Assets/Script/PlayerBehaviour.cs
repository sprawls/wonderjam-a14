using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerBehaviour : MonoBehaviour, IBeatReceiver {
	private BeatManager bm;
	private bool createNewGem = false;
	private bool createActiveGem = false;
	private bool aboutToSwitch = false;
	private List<Transform> gems = new List<Transform>();
	private List<int> gemsToDestroy = new List<int> ();
	private float bpm;
	private bool turnP1 = false;
	private string tag;

	public GUISkin gSkin;

	public int paddingZeroes = 7;
	public int score = 0;
	public float zLimit = 100;
	public float speed;
	public Vector3 gemSpawningPoint;

	public Transform gemPrefab;
	public Transform neutralGemPrefab;
	
	// Use this for initialization
	void Start () {
		bm = BeatManager.Instance;
		bpm = bm.interval;
		GameManager.Instance.requestBeat (this);
		tag = gameObject.tag;

		float totalGemDistance = Math.Abs(gemSpawningPoint.z - zLimit);
		float timeToReach = totalGemDistance / speed;
	}

	// Update is called once per frame
	void Update () {
		aboutToSwitch = bm.aboutToSwitch;

		// Create new gem at end of line
		if (createNewGem) {
			Transform newGem;
			if (createActiveGem)
				newGem = Transform.Instantiate(gemPrefab) as Transform;
			else
				newGem = Transform.Instantiate(neutralGemPrefab) as Transform;

			newGem.parent = this.transform;
			newGem.position = gemSpawningPoint;
			gems.Add (newGem);

			createNewGem = false;
		}
		
		foreach (Transform g in gems) {
			// Move all gems towards stromatolite
			g.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
			
			// Destroy gems on stromatolite
			if (Mathf.Abs(g.position.z - zLimit) < 0.5 ) {
				gemsToDestroy.Add (gems.IndexOf (g));
			}
		}

		foreach (int i in gemsToDestroy) {
			Transform g = gems[i];
			gems.RemoveAt(i);
			Destroy(g.gameObject);
		}
		gemsToDestroy.Clear ();
	}

    public void OnQuarterBeat() {}

	public void OnBeat(BeatEnum p1, BeatEnum p2, bool turnP1) {
		createNewGem = true;
		turnP1 = turnP1;

		createActiveGem = (((turnP1 && !aboutToSwitch) || (!turnP1 && aboutToSwitch)) && tag == "Player1") || (((!turnP1 && !aboutToSwitch) || (turnP1 && aboutToSwitch)) && tag == "Player2");
	}

	void OnGUI() {
		GUI.skin = gSkin;

		// Show score
		GUI.TextArea (new Rect (0, 0, Screen.width, 40), this.score.ToString().PadLeft(paddingZeroes, '0'), GUI.skin.GetStyle("score"));
	}
}
