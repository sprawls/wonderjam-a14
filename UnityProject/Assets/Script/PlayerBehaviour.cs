﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour, IBeatReceiver {
	private BeatManager bm;
	private bool createNewGem = false;
	private List<Transform> gems = new List<Transform>();
	private List<int> gemsToDestroy = new List<int> ();
	private float bpm;

	public GUISkin gSkin;
	public int paddingZeroes = 7;
	public int score = 0;
	public Transform gemPrefab;
	public float zLimit = 100;
	public Vector3 gemSpawningPoint;
	public float speed;

	// Use this for initialization
	void Start () {
		bm = BeatManager.Instance;
		bpm = bm.interval;
		GameManager.Instance.requestBeat (this);
	}

	// Update is called once per frame
	void Update () {
		// Create new gem at end of line
		if (createNewGem) {
			Transform newGem = Transform.Instantiate(gemPrefab) as Transform;
			newGem.parent = this.transform;
			newGem.position = gemSpawningPoint;
			gems.Add (newGem);

			createNewGem = false;
		}
		
		foreach (Transform g in gems) {
			// Move all gems towards stromatolite
			g.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
			
			// Destroy gems on stromatolite
			if (Mathf.Abs(g.position.z - zLimit) < 0.3 ) {
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
	}

	void OnGUI() {
		GUI.skin = gSkin;

		// Show score
		GUI.TextArea (new Rect (0, 0, Screen.width, 40), this.score.ToString().PadLeft(paddingZeroes, '0'), GUI.skin.GetStyle("score"));
	}
}
